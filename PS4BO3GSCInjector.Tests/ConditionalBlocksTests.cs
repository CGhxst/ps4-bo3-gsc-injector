using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PS4BO3GSCInjector.Tests
{
    [TestClass]
    public class ConditionalBlocksTests
    {
        [TestMethod]
        public void ParseSource_IfDefDefinedToken_KeepsContent()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3", "PS4" });

            string input = "#ifdef BO3\n#define BO3_ACTIVE 1\n#endif";
            string output = cb.ParseSource(input);

            Assert.IsTrue(output.Contains("#define BO3_ACTIVE 1"));
        }

        [TestMethod]
        public void ParseSource_IfDefUndefinedToken_StripsContent()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            string input = "#ifdef XBOX\n#define XBOX_ACTIVE 1\n#endif";
            string output = cb.ParseSource(input);

            Assert.IsFalse(output.Contains("#define XBOX_ACTIVE 1"));
        }

        [TestMethod]
        public void ParseSource_IfNDefUndefinedToken_KeepsContent()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            string input = "#ifndef PC\n#define CONSOLE_BUILD 1\n#endif";
            string output = cb.ParseSource(input);

            Assert.IsTrue(output.Contains("#define CONSOLE_BUILD 1"));
        }

        [TestMethod]
        public void ParseSource_IfElseBranching_SelectsCorrectBranch()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            string input = "#ifdef BO3\nint target = 1;\n#else\nint target = 2;\n#endif";
            string output = cb.ParseSource(input);

            Assert.IsTrue(output.Contains("int target = 1;"));
            Assert.IsFalse(output.Contains("int target = 2;"));
        }

        [TestMethod]
        public void ParseSource_NestedUndefinedBlock_DoesNotEnableInnerElse()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            string input = "#ifdef XBOX\n#ifdef BO3\nwrong1();\n#else\nwrong2();\n#endif\n#endif\nright();";
            string output = cb.ParseSource(input);

            Assert.IsFalse(output.Contains("wrong1();"));
            Assert.IsFalse(output.Contains("wrong2();"));
            Assert.IsTrue(output.Contains("right();"));
        }

        [TestMethod]
        public void ParseSource_EscapedQuote_DoesNotEndString()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            string input = "print(\"escaped quote: \\\" #ifdef XBOX\");\n#ifdef BO3\nright();\n#endif";
            string output = cb.ParseSource(input);

            Assert.IsTrue(output.Contains("#ifdef XBOX"));
            Assert.IsTrue(output.Contains("right();"));
        }

        [TestMethod]
        public void ParseSource_StringStartingAtPositionZero_DoesNotThrowIndexOutOfBounds()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            // Regression test: Input starting with quote at index 0
            string input = "\"this is a string at start\";\n#ifdef BO3\nint x = 5;\n#endif";
            string output = cb.ParseSource(input);

            Assert.IsTrue(output.Contains("int x = 5;"));
        }

        [TestMethod]
        [ExpectedException(typeof(CBSyntaxException))]
        public void ParseSource_MissingEndIf_ThrowsCBSyntaxException()
        {
            var cb = new ConditionalBlocks();
            cb.LoadConditionalTokens(new List<string> { "BO3" });

            string input = "#ifdef BO3\nint x = 5;";
            cb.ParseSource(input);
        }
    }
}
