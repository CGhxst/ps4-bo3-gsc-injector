using System;
using System.Collections.Generic;
using libdebug;
using TreyarchCompiler;
using TreyarchCompiler.Utilities;

namespace PS4GSCInjector.GameProfiles
{
    public sealed class Bo4GameProfile : IGscGameProfile
    {
        private static readonly IReadOnlyList<string> Symbols = new[] { "BO4", "T8", "PS4", "__PS4", "_GSC" };
        private const ulong TargetCompilerScriptHash = 0x124CECFF7280BE52;

        private static readonly IReadOnlyList<GameVersionProfile> GameVersions = new[]
        {
            new GameVersionProfile("bo4-1.26-zm", "1.26 Zombies", 0, @"scripts\zm_common\load.gsc"),
            new GameVersionProfile("bo4-1.26-mp", "1.26 Multiplayer", 0, @"scripts\mp_common\bb.gsc"),
            new GameVersionProfile("bo4-1.26-common", "1.26 Frontend/Common", 0, @"scripts\core_common\load_shared.gsc")
        };

        public string Id => "bo4";

        public string DisplayName => "Black Ops 4";

        public string AttachButtonText => "Attach BO4";

        public string ProcessName => "eboot.bin";

        public string CompiledScriptDescription => "compiled Black Ops 4 T8 GSC script";

        public bool CanCompile => true;

        public string CompilerUnavailableMessage => string.Empty;

        public IReadOnlyList<string> ConditionalSymbols => Symbols;

        public IReadOnlyList<GameVersionProfile> Versions => GameVersions;

        public CompiledCode Compile(string source)
        {
            return Compiler.CompileT8(source);
        }

        public bool IsValidCompiledScript(byte[] script)
        {
            return TreyarchCompiledScriptValidator.IsValid(script, CompiledScriptFormat.T8);
        }

        public void InjectCompiledScript(
            libdebug.PS4DBG ps4,
            Process process,
            GameVersionProfile version,
            byte[] script,
            IDictionary<string, InjectedScriptAllocation> injectedScripts)
        {
            if (string.IsNullOrWhiteSpace(version.ScriptHookPath))
                throw new GscInjectionException("Black Ops 4 needs a script hook before it can inject.");

            var scriptNameHash = T8ScriptHash.Hash64(version.ScriptHookPath);
            if (!MemoryScriptPointerLocator.TryFindT8ScriptParseTreeEntries(
                ps4,
                process,
                scriptNameHash,
                TargetCompilerScriptHash,
                out var surrogateEntry,
                out var targetEntry))
            {
                throw new GscInjectionException(
                    "Could not auto-locate the BO4 script hook " + version.ScriptHookPath +
                    ". Make sure the matching game mode is loaded far enough for that script to exist in memory.");
            }

            ulong newScriptAddress = 0;
            var pointerUpdated = false;

            try
            {
                EnsureSurrogateIncludesTarget(ps4, process, surrogateEntry);

                ps4.ReadMemory(process.pid, targetEntry.BufferAddress + 0x8, 8).CopyTo(script, 0x8);

                newScriptAddress = ps4.AllocateMemory(process.pid, script.Length);
                ps4.WriteMemory(process.pid, newScriptAddress, script);
                ps4.WriteMemory(process.pid, targetEntry.EntryAddress + 0x10, newScriptAddress);
                pointerUpdated = true;

                var allocationKey = Id + ":" + version.Id + ":" + TargetCompilerScriptHash.ToString("X");
                if (injectedScripts.TryGetValue(allocationKey, out var previousAllocation) &&
                    previousAllocation.ProcessId == process.pid)
                {
                    try
                    {
                        ps4.FreeMemory(process.pid, previousAllocation.Address, previousAllocation.Length);
                    }
                    catch
                    {
                        // The new script is already active; failure to free the old allocation is non-fatal.
                    }
                }

                injectedScripts[allocationKey] = new InjectedScriptAllocation(newScriptAddress, script.Length, process.pid);
            }
            catch
            {
                if (!pointerUpdated && newScriptAddress != 0)
                {
                    try
                    {
                        ps4.FreeMemory(process.pid, newScriptAddress, script.Length);
                    }
                    catch
                    {
                        // Preserve the original injection error.
                    }
                }

                throw;
            }
        }

        private static void EnsureSurrogateIncludesTarget(
            libdebug.PS4DBG ps4,
            Process process,
            MemoryScriptPointerLocator.T8ScriptParseTreeEntry surrogateEntry)
        {
            const int includeTableOffsetOffset = 0x18;
            const int includeCountOffset = 0x58;

            byte includeCount = ps4.ReadMemory<byte>(process.pid, surrogateEntry.BufferAddress + includeCountOffset);
            int includeTableOffset = ps4.ReadMemory<int>(process.pid, surrogateEntry.BufferAddress + includeTableOffsetOffset);
            if (includeTableOffset <= 0)
                throw new GscInjectionException("BO4 hook include table is invalid.");

            if (includeCount == byte.MaxValue)
                throw new GscInjectionException("BO4 hook include table is full.");

            ulong includeTableAddress = surrogateEntry.BufferAddress + (ulong)includeTableOffset;

            for (var index = 0; index < includeCount; index++)
            {
                if (ps4.ReadMemory<ulong>(process.pid, includeTableAddress + (ulong)(index * sizeof(ulong))) == TargetCompilerScriptHash)
                    return;
            }

            ps4.WriteMemory(process.pid, includeTableAddress + (ulong)(includeCount * sizeof(ulong)), TargetCompilerScriptHash);
            ps4.WriteMemory(process.pid, surrogateEntry.BufferAddress + includeCountOffset, (byte)(includeCount + 1));
        }
    }
}
