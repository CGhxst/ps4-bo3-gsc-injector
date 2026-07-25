namespace PS4BO3GSCInjector
{
    public static class CompiledScriptValidator
    {
        private static readonly byte[] T7Magic = { 0x80, 0x47, 0x53, 0x43, 0x0D, 0x0A, 0x00, 0x1C };

        public static bool IsValid(byte[] script)
        {
            if (script == null || script.Length < 0x50)
                return false;

            for (var index = 0; index < T7Magic.Length; index++)
            {
                if (script[index] != T7Magic[index])
                    return false;
            }

            return true;
        }
    }
}
