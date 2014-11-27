namespace HopperWatchdog
{
    using System.Diagnostics; 
    using System.IO;
    using System.Threading;

    public class Program
    {
        static void Main(string[] args)
        {
            var period = 15 * 60 * 1000;
            var runningInterval = 5 * 60 * 1000;

            if (args.Length > 1)
            {
                period = int.Parse(args[0]);
            }

            if (args.Length > 2)
            {
                runningInterval = int.Parse(args[1]);
            }

            if (ProcessService.ProcessExists("hopper.exe"))
            {
                return;
            }

            var timer = new Timer(Run, runningInterval, 3 * 1000, period);
            var waitHandle = new ManualResetEvent(false);
            waitHandle.WaitOne();
        }

        static void Run(object runningInterval)
        {
            ClearResults();
            RunHopper(runningInterval);
            ClearResults();
        }

        static void ClearResults()
        {
            DeleteDirectory(@"\testlog");
            DeleteHopperRegistry();
        }

        static void RunHopper(object runningInterval)
        {
            var process = Process.Start(@"\windows\hopper.exe", null);
            Thread.Sleep((int)runningInterval);
            EnsureHopperNotRunningAnyMore();
        }

        static void EnsureHopperNotRunningAnyMore()
        {
            for (int i = 0; i < 20; i++)
            {
                try
                {
                    if (!ProcessService.ProcessExists("hopper.exe"))
                    {
                        return;
                    }

                    ProcessService.KillProcess("hopper.exe");
                    Thread.Sleep(1000);
                }
                catch
                {
                }
            }
        }
        
        static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                new FileInfo(file).Attributes = FileAttributes.Normal;

                File.Delete(file);
            }

            Directory.Delete(path);
        }

        static void DeleteHopperRegistry()
        {
            try
            {
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(@"Software\Microsoft\Hopper");
            }
            catch
            { 
            }
        }
    }
}
