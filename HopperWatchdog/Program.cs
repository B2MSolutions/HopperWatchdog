namespace HopperWatchdog
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;
    using System.IO;

    public class Program
    {
        static void Main(string[] args)
        {
            var period = 15 * 60 * 1000;
            var runningInteval = 5 * 60 * 1000;

            if (args.Length > 1)
            {
                period = int.Parse(args[0]);
            }

            if (args.Length > 2)
            {
                runningInteval = int.Parse(args[1]);
            }

            var timer = new Timer(Run, runningInteval, 3 * 1000, period);
            var waitHandle = new ManualResetEvent(false);
            waitHandle.WaitOne();
        }

        static void Run(object runningInteval)
        {
            RunHopper(runningInteval);
            ClearResults();
        }

        static void ClearResults()
        {
            DeleteDirectory(@"\testlog");
            DeleteHopperRegistry();
        }

        static void RunHopper(object runningInteval)
        {
            var process = Process.Start(@"\windows\hopper.exe", null);
            Thread.Sleep((int)runningInteval);
            process.Kill();
        }
        
        static void DeleteDirectory(string path)
        {
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
