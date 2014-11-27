namespace HopperWatchdog
{
    using OpenNETCF.ToolHelp;

    public static class ProcessService
    {
        public static void KillProcess(string processName)
        {
            ProcessEntry[] processes = ProcessEntry.GetProcesses();
            foreach (var process in processes)
            {
                if (process.ExeFile.ToLower().CompareTo(processName.ToLower()) == 0)
                {
                    process.Kill();
                    return;
                }
            }
        }

        public static bool ProcessExists(string processName)
        {
            ProcessEntry[] processes = ProcessEntry.GetProcesses();
            foreach (var process in processes)
            {
                if (process.ExeFile.ToLower().CompareTo(processName.ToLower()) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
