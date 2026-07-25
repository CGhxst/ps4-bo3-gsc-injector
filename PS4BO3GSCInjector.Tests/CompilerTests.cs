using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TreyarchCompiler;
using TreyarchCompiler.Utilities;

namespace PS4BO3GSCInjector.Tests
{
    [TestClass]
    public class CompilerTests
    {
        [TestMethod]
        public void TryParsePayloadEndpoint_ValidIpv4AndPort_ReturnsEndpoint()
        {
            bool result = ConnectionSettings.TryParsePayloadEndpoint("192.168.1.20", "9090", out IPEndPoint endpoint);

            Assert.IsTrue(result);
            Assert.AreEqual("192.168.1.20", endpoint.Address.ToString());
            Assert.AreEqual(9090, endpoint.Port);
        }

        [TestMethod]
        public void TryParsePayloadEndpoint_InvalidValues_ReturnFalse()
        {
            Assert.IsFalse(ConnectionSettings.TryParsePayloadEndpoint("not-an-ip", "9090", out _));
            Assert.IsFalse(ConnectionSettings.TryParsePayloadEndpoint("::1", "9090", out _));
            Assert.IsFalse(ConnectionSettings.TryParsePayloadEndpoint("192.168.1.20", "0", out _));
            Assert.IsFalse(ConnectionSettings.TryParsePayloadEndpoint("192.168.1.20", "-1", out _));
            Assert.IsFalse(ConnectionSettings.TryParsePayloadEndpoint("192.168.1.20", "65536", out _));
        }

        [TestMethod]
        public void BundledPayload_MatchesOfficialGoldHenV1119()
        {
            string payloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Payloads", "ps4debug.bin");
            Assert.IsTrue(File.Exists(payloadPath), "The payload was not copied to the application output.");

            byte[] hash;
            using (var sha256 = SHA256.Create())
            using (var payload = File.OpenRead(payloadPath))
                hash = sha256.ComputeHash(payload);

            Assert.AreEqual(
                "8AF40C2412768BB2CD64C46B9913EE2F7FF2F076C98F6A2F5C7C0534AEE15B2E",
                BitConverter.ToString(hash).Replace("-", string.Empty));
        }

        [TestMethod]
        public void Compile_ValidGscScript_ProducesCompiledBytecode()
        {
            string gscScript = @"
#namespace test_script;

init()
{
    level.test_var = 1;
}
";
            CompiledCode result = Compiler.Compile(false, gscScript);

            Assert.IsNotNull(result, "Compiler returned null object.");
            Assert.IsTrue(string.IsNullOrEmpty(result.Error), $"Compiler reported error: {result.Error}");
            Assert.IsNotNull(result.CompiledScript, "CompiledScript output is null.");
            Assert.IsTrue(result.CompiledScript.Length > 0, "CompiledScript output is empty.");
            Assert.IsTrue(CompiledScriptValidator.IsValid(result.CompiledScript), "Compiler output has an invalid T7 header.");
        }

        [TestMethod]
        public void Compile_InvalidSyntax_ReturnsCompilerError()
        {
            string invalidScript = @"
#namespace test_script;

function init(
{
    level.test_var = ;
}
";
            CompiledCode result = Compiler.Compile(false, invalidScript);

            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrEmpty(result.Error), "Compiler should report error on syntax failure.");
        }

        [TestMethod]
        public void CompiledScriptValidator_InvalidInputs_ReturnFalse()
        {
            Assert.IsFalse(CompiledScriptValidator.IsValid(null));
            Assert.IsFalse(CompiledScriptValidator.IsValid(new byte[0x50]));
            Assert.IsFalse(CompiledScriptValidator.IsValid(new byte[] { 0x80, 0x47, 0x53, 0x43 }));
        }
    }
}
