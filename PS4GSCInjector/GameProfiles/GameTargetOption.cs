namespace PS4GSCInjector.GameProfiles
{
    public sealed class GameTargetOption
    {
        public GameTargetOption(IGscGameProfile profile, GameVersionProfile version)
        {
            Profile = profile;
            Version = version;
        }

        public IGscGameProfile Profile { get; }

        public GameVersionProfile Version { get; }

        public string Key => Profile.Id + ":" + Version.Id;

        public override string ToString()
        {
            return Profile.DisplayName + " " + Version.DisplayName;
        }
    }
}
