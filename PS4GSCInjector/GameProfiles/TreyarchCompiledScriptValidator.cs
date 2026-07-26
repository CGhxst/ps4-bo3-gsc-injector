namespace PS4GSCInjector.GameProfiles
{
    public static class TreyarchCompiledScriptValidator
    {
        private static readonly byte[] T7Magic = { 0x80, 0x47, 0x53, 0x43, 0x0D, 0x0A, 0x00, 0x1C };
        private static readonly byte[] T8Magic = { 0x80, 0x47, 0x53, 0x43, 0x0D, 0x0A, 0x00, 0x36 };

        public static bool IsValid(byte[] script, CompiledScriptFormat format)
        {
            var minimumLength = format == CompiledScriptFormat.T8 ? 0x60 : 0x50;
            if (script == null || script.Length < minimumLength)
                return false;

            var expectedMagic = format == CompiledScriptFormat.T8 ? T8Magic : T7Magic;
            for (var index = 0; index < expectedMagic.Length; index++)
            {
                if (script[index] != expectedMagic[index])
                    return false;
            }

            return true;
        }
    }
}
