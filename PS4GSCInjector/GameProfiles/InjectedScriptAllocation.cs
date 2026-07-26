namespace PS4GSCInjector.GameProfiles
{
    public sealed class InjectedScriptAllocation
    {
        public InjectedScriptAllocation(ulong address, int length, int processId)
        {
            Address = address;
            Length = length;
            ProcessId = processId;
        }

        public ulong Address { get; }

        public int Length { get; }

        public int ProcessId { get; }
    }
}
