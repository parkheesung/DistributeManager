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
                        case "CMD":
                            if (!string.IsNullOrWhiteSpace(command.Original))
                            {
                                //File.Copy(command.Original, command.Target, true);
                                await Task.Factory.StartNew(() => ProcessRun(command.Original));

                                AppendText($"명령실행 : {command.Original}");
                            }
                            else
                            {
                                AppendText($"명령이 전달되지 않았습니다.");
                            }
                            break;
                    }
                }
            }
        }

        private void ProcessRun(string command)
        {
            /*
            using (var proc = new Process())
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = @"cmd";
                info.CreateNoWindow = true;  //cmd 창 띄움 여부 (true : 띄우지 않기, false : 띄우기)
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true; //cmd창으로부터 데이터 받기
                info.RedirectStandardInput = true; //cmd창으로 데이터 보내기
                info.RedirectStandardError = true;  //cmd 오류내용 받기
                proc.StartInfo = info;
                proc.Start();

                proc.StandardInput.Write(command);
                proc.StandardInput.Close();
                string message = proc.StandardOutput.ReadToEnd();
                Logger.Current.Debug(message);
                proc.WaitForExit();
                proc.Close();
            }
            */
            FileInfo fi = new FileInfo(command);
            if (fi.Exists)
            {
                try
                {
                    int ExitCode;
                    ProcessStartInfo ProcessInfo;
                    Process process;

                    ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + fi.Name);
                    ProcessInfo.CreateNoWindow = true;
                    ProcessInfo.UseShellExecute = false;
                    ProcessInfo.WorkingDirectory = fi.DirectoryName;
                    // *** Redirect the output ***
                    ProcessInfo.RedirectStandardError = true;
                    ProcessInfo.RedirectStandardOutput = true;

                    process = Process.Start(ProcessInfo);
                    process.WaitForExit();

                    // *** Read the streams ***
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    ExitCode = process.ExitCode;

                    AppendText(output);
                    AppendText(error);

                    process.Close();
                }
                catch (Exception ex)
                {
                    AppendText(ex.Message);
                }
            }
            else
            {
                AppendText("대상 파일이 존재하지 않습니다.");
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
