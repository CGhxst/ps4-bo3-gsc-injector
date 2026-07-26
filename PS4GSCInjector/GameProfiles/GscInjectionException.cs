using System;

namespace PS4GSCInjector.GameProfiles
{
    public sealed class GscInjectionException : Exception
    {
        public GscInjectionException(string message)
            : base(message)
        {
        }
    }
}
