using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DistributeServer
{
    static class Program
    {
        static void Main()
        {
            var ServiceToRun = new DManager();

            if (Environment.UserInteractive)
            {
                try
                {
                    Console.WriteLine("Running Debug Mode...");
                    Console.WriteLine("Press Enter to terminate ... ");
                    ServiceToRun.Start();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("GlobalException : " + ex.Message);
                    Console.WriteLine("GlobalException : " + ex.StackTrace);
                }

                Console.ReadLine();
                Console.WriteLine("NowVote BackgroundWorker has ended.");
            }
            else
            {
                try
                {
                    Logger.Current.Debug("Running Service Mode...");
                    ServiceBase.Run(ServiceToRun);
                }
                catch (Exception ex)
                {
                    Logger.Current.Debug("GlobalException : " + ex.Message);
                    Logger.Current.Debug("GlobalException : " + ex.StackTrace);
                }
            }
        }
    }
}
