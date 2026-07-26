using System;
using System.Collections.Generic;
using libdebug;

namespace PS4GSCInjector.GameProfiles
{
    public abstract class ScriptPointerGameProfile : IGscGameProfile
    {
        protected ScriptPointerGameProfile(ScriptPointerOffsets offsets)
        {
            Offsets = offsets;
        }

        protected ScriptPointerOffsets Offsets { get; }

        public abstract string Id { get; }

        public abstract string DisplayName { get; }

        public abstract string AttachButtonText { get; }

        public abstract string ProcessName { get; }

        public abstract string CompiledScriptDescription { get; }

        public abstract bool CanCompile { get; }

        public abstract string CompilerUnavailableMessage { get; }

        public abstract IReadOnlyList<string> ConditionalSymbols { get; }

        public abstract IReadOnlyList<GameVersionProfile> Versions { get; }

        public abstract TreyarchCompiler.Utilities.CompiledCode Compile(string source);

        public abstract bool IsValidCompiledScript(byte[] script);

        public virtual void InjectCompiledScript(
            PS4DBG ps4,
            libdebug.Process process,
            GameVersionProfile version,
            byte[] script,
            IDictionary<string, InjectedScriptAllocation> injectedScripts)
        {
            var targetScriptAddress = ResolveTargetScriptAddress(ps4, process, version, script);
            if (targetScriptAddress == 0)
                throw new GscInjectionException("This target could not resolve a script hook in memory.");

            ulong newScriptAddress = 0;
            var pointerUpdated = false;

            try
            {
                var filePointerAddress = ps4.ReadMemory<ulong>(process.pid, targetScriptAddress + (ulong)Offsets.ScriptPointerOffset);

                if (filePointerAddress == 0)
                    throw new GscInjectionException("Failed to locate target script pointer in game memory. Ensure you are in a map or loaded game state.");

                int checksum = ps4.ReadMemory<int>(process.pid, filePointerAddress + (ulong)Offsets.ChecksumReadOffset);
                BitConverter.GetBytes(checksum).CopyTo(script, Offsets.ChecksumWriteOffset);

                newScriptAddress = ps4.AllocateMemory(process.pid, script.Length);
                ps4.WriteMemory(process.pid, newScriptAddress, script);
                ps4.WriteMemory(process.pid, targetScriptAddress + (ulong)Offsets.ScriptPointerOffset, newScriptAddress);
                pointerUpdated = true;

                var allocationKey = Id + ":" + version.Id + ":" + targetScriptAddress.ToString("X");
                if (injectedScripts.TryGetValue(allocationKey, out var previousAllocation) &&
                    previousAllocation.ProcessId == process.pid)
                {
                    try
                    {
                        ps4.FreeMemory(process.pid, previousAllocation.Address, previousAllocation.Length);
                    }
                    catch
                    {
                        // The new script is already active; failure to free the old allocation is non-fatal.
                    }
                }

                injectedScripts[allocationKey] = new InjectedScriptAllocation(newScriptAddress, script.Length, process.pid);
            }
            catch
            {
                if (!pointerUpdated && newScriptAddress != 0)
                {
                    try
                    {
                        ps4.FreeMemory(process.pid, newScriptAddress, script.Length);
                    }
                    catch
                    {
                        // Preserve the original injection error.
                    }
                }

                throw;
            }
        }

        protected virtual ulong ResolveTargetScriptAddress(
            PS4DBG ps4,
            libdebug.Process process,
            GameVersionProfile version,
            byte[] script)
        {
            return version.TargetScriptAddress;
        }
    }
}
