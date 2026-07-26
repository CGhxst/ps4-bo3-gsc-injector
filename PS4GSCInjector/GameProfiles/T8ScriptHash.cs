using System;
using System.Globalization;
using System.Text;

namespace PS4GSCInjector.GameProfiles
{
    public static class T8ScriptHash
    {
        private static readonly string[] HashIdentifierPrefixes =
        {
            "func_",
            "function_",
            "namespace_",
            "var_",
            "hash_",
            "script_"
        };

        public static ulong Hash64(string input)
        {
            if (input == null)
                return 0;

            input = NormalizeScriptPath(input);

            if (TryParsePrefixedHash(input, out var parsedHash))
                return parsedHash;

            return 0x7FFFFFFFFFFFFFFF & HashFNV1a(Encoding.ASCII.GetBytes(input));
        }

        public static string NormalizeScriptPath(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input.Trim().Replace('\\', '/').ToLowerInvariant();
        }

        private static bool TryParsePrefixedHash(string input, out ulong hash)
        {
            foreach (string hashPrefix in HashIdentifierPrefixes)
            {
                if (input.Length <= hashPrefix.Length)
                    continue;

                if (!input.StartsWith(hashPrefix, StringComparison.OrdinalIgnoreCase))
                    continue;

                return ulong.TryParse(
                    input.Substring(hashPrefix.Length),
                    NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture,
                    out hash);
            }

            hash = 0;
            return false;
        }

        private static ulong HashFNV1a(byte[] bytes)
        {
            const ulong fnv64Offset = 14695981039346656037;
            const ulong fnv64Prime = 0x100000001b3;

            ulong hash = fnv64Offset;

            for (var index = 0; index < bytes.Length; index++)
            {
                hash ^= bytes[index];
                hash *= fnv64Prime;
            }

            return hash;
        }
    }
}
