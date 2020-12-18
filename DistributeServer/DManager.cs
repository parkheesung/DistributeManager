using DM.Library;
using Newtonsoft.Json;
using OctopusV3.Core;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;

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

        private void Work_Tick(object state)
        {
            string body = string.Empty;
            FileInfo fi = null;
            ServerCommand command = null;
            if (WorkQueue.TryDequeue(out command))
            {
                if (command != null && !string.IsNullOrWhiteSpace(command.Command))
                {
                    switch (command.Command.Trim().ToUpper())
                    {
                        case "COPY":
                            fi = new FileInfo(command.Original);
                            if (fi.Exists)
                            {
                                body = FileHelper.ReadFile(command.Original, Encoding.UTF8);
                                FileHelper.WriteFile(command.Target, body, Encoding.UTF8, false);
                                AppendText($"파일 복사 : {command.Original}을(를) {command.Target}(으)로");
                            }
                            else
                            {
                                AppendText($"파일 복사 실패 : {command.Original}을(를) 찾을 수 없습니다.");
                            }
                            SocketServer.Current.Send(JsonConvert.SerializeObject(new ServerCommand() { Command = "Notice", Option = $"{command.Target}로 파일이 복사되었습니다." }));
                            break;
                    }
                }
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
