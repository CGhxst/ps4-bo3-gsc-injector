using System.Collections.Generic;
using TreyarchCompiler;
using TreyarchCompiler.Utilities;

namespace PS4GSCInjector.GameProfiles
{
    public sealed class Bo3GameProfile : ScriptPointerGameProfile
    {
        private static readonly IReadOnlyList<string> Symbols = new[] { "BO3", "T7", "PS4", "__PS4", "_GSC" };

        private static readonly IReadOnlyList<GameVersionProfile> GameVersions = new[]
        {
            new GameVersionProfile("bo3-1.33", "1.33", 0x547EEF0),
            new GameVersionProfile("bo3-1.26", "1.26", 0x6B9CFD0)
        };

        public Bo3GameProfile()
            : base(new ScriptPointerOffsets(0x10, 0x8, 0x8))
        {
        }

        public override string Id => "bo3";

        public override string DisplayName => "Black Ops 3";

        public override string AttachButtonText => "Attach BO3";

        public override string ProcessName => "eboot.bin";

        public override string CompiledScriptDescription => "compiled Black Ops 3 GSC script";

        public override bool CanCompile => true;

        public override string CompilerUnavailableMessage => string.Empty;

        public override IReadOnlyList<string> ConditionalSymbols => Symbols;

        public override IReadOnlyList<GameVersionProfile> Versions => GameVersions;

        public override CompiledCode Compile(string source)
        {
            return Compiler.Compile(source);
        }

        public override bool IsValidCompiledScript(byte[] script)
        {
            return TreyarchCompiledScriptValidator.IsValid(script, CompiledScriptFormat.T7);
        }
    }
}
