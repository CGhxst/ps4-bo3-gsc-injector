namespace PS4GSCInjector.GameProfiles
{
    public sealed class ScriptPointerOffsets
    {
        public ScriptPointerOffsets(int scriptPointerOffset, int checksumReadOffset, int checksumWriteOffset)
        {
            ScriptPointerOffset = scriptPointerOffset;
            ChecksumReadOffset = checksumReadOffset;
            ChecksumWriteOffset = checksumWriteOffset;
        }

        public int ScriptPointerOffset { get; }

        public int ChecksumReadOffset { get; }

        public int ChecksumWriteOffset { get; }
    }
}
