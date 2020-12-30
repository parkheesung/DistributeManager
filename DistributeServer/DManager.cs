using DM.Library;
using Newtonsoft.Json;
using OctopusV3.Core;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DistributeServer
{
    public partial class DManager : ServiceBase
    {
        delegate void AppendTextDelegate(string s);
        AppendTextDelegate _textAppender;

        private ConcurrentQueue<ServerCommand> WorkQueue { get; set; } = new ConcurrentQueue<ServerCommand>();

        private delegate void AddTextDelegate(string strText); // 크로스 쓰레드 호출

        private Timer WorkTimer { get; set; }

        public DManager()
        {
            InitializeComponent();
            _textAppender = new AppendTextDelegate(AppendText);
        }

        void AppendText(string s)
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine(s);
            }
            else
            {
                Logger.Current.Debug(s);
            }
        }


        public void Start()
        {
            this.OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            string strIp = ConfigurationManager.AppSettings["ServerIP"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

            SocketServer.Current.Setup(strIp, port, 10);
            SocketServer.Current.Received = new Action<string>((msg) =>
            {
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ServerCommand cc = JsonConvert.DeserializeObject<ServerCommand>(msg);
                    WorkQueue.Enqueue(cc);
                }
            });
            SocketServer.Current.ClientConnectEvent = new Action<string>((msg) =>
            {
                AppendText($"클라이언트 접속 : {msg}");
            });
            SocketServer.Current.Listen(new Action(() => {
                AppendText("Server Start!!");
            }));

            this.WorkTimer = new Timer(Work_Tick, null, 1000, 1000);
        }

        private async void Work_Tick(object state)
        {
            ServerCommand command = null;

            if (WorkQueue.TryDequeue(out command))
            {
                if (command != null && !string.IsNullOrWhiteSpace(command.Command))
                {
                    switch (command.Command.Trim().ToUpper())
                    {
                        case "COPY":
                            FileInfo fi = new FileInfo(command.Original);
                            if (fi.Exists)
                            {
                                //File.Copy(command.Original, command.Target, true);
                                await Task.Factory.StartNew(() => ProcessXcopy(command.Original, command.Target));

                                AppendText($"파일 복사 : {command.Original}을(를) {command.Target}(으)로");
                            }
                            else
                            {
                                AppendText($"파일 복사 실패 : {command.Original}을(를) 찾을 수 없습니다.");
                            }
                            break;
                    }
                }
            }
        }

        private void ProcessXcopy(string SolutionDirectory, string TargetDirectory)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            //Give the name as Xcopy
            startInfo.FileName = "xcopy";
            //make the window Hidden
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //Send the Source and destination as Arguments to the process
            startInfo.Arguments = "\"" + SolutionDirectory + "\"" + " " + "\"" + TargetDirectory + "\"" + @" /e /y /I";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception exp)
            {
                throw exp;
                Logger.Current.Error(exp);
            }
        }


        protected override void OnStop()
        {
            try
            {
                SocketServer.Current.Dispose();
                this.WorkTimer.Dispose();
                this.WorkTimer = null;

                if (Environment.UserInteractive)
                {
                    Console.WriteLine($"Server Stop");
                }
                else
                {
                    Logger.Current.Info("Server Stop");
                }
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    Console.WriteLine($"Server Stop Error : {ex.Message}");
                }
                else
                {
                    Logger.Current.Error(ex);
                }
            }
        }
    }
}
