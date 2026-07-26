namespace PS4GSCInjector.GameProfiles
{
    public sealed class GameVersionProfile
    {
        public GameVersionProfile(string id, string displayName, ulong targetScriptAddress, string scriptHookPath = null)
        {
            Id = id;
            DisplayName = displayName;
            TargetScriptAddress = targetScriptAddress;
            ScriptHookPath = scriptHookPath;
        }

        public string Id { get; }

        public string DisplayName { get; }

        public ulong TargetScriptAddress { get; }

        public string ScriptHookPath { get; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
