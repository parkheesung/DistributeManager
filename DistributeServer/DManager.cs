using DM.Library;
using Newtonsoft.Json;
using OctopusV3.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        Socket mainSock;
        IPAddress thisAddress;
        string strIp = string.Empty;
        int port = 0;
        List<Socket> connectedClients = new List<Socket>();

        private delegate void AddTextDelegate(string strText); // 크로스 쓰레드 호출



        public DManager()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
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
            strIp = ConfigurationManager.AppSettings["ServerIP"].ToString();
            port = Convert.ToInt32(ConfigurationManager.AppSettings["ServerPort"]);

            IPAddress addr = IPAddress.Parse(strIp);

            // 서버에서 클라이언트의 연결 요청을 대기하기 위해
            // 소켓을 열어둔다.
            IPEndPoint serverEP = new IPEndPoint(addr, port);
            mainSock.Bind(serverEP);
            mainSock.Listen(10);

            // 비동기적으로 클라이언트의 연결 요청을 받는다.
            mainSock.BeginAccept(AcceptCallback, null);

            AppendText("Server Start!!");
        }

        void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = mainSock.EndAccept(ar);

            // 또 다른 클라이언트의 연결을 대기한다.
            mainSock.BeginAccept(AcceptCallback, null);

            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = client;

            // 연결된 클라이언트 리스트에 추가해준다.
            connectedClients.Add(client);

            // 텍스트박스에 클라이언트가 연결되었다고 써준다.
            AppendText(string.Format("클라이언트 (@ {0})가 연결되었습니다.", client.RemoteEndPoint));

            // 클라이언트의 데이터를 받는다.
            client.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
        }

        void DataReceived(IAsyncResult ar)
        {
            try
            {
                // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.
                AsyncObject obj = (AsyncObject)ar.AsyncState;

                // 데이터 수신을 끝낸다.
                int received = obj.WorkingSocket.EndReceive(ar);

                // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
                if (received <= 0)
                {
                    obj.WorkingSocket.Close();
                    return;
                }

                // 텍스트로 변환한다.
                string msg = Encoding.UTF8.GetString(obj.Buffer);
                string body = string.Empty;
                FileInfo fi = null;

                if (!string.IsNullOrWhiteSpace(msg))
                {
                    ServerCommand cc = JsonConvert.DeserializeObject<ServerCommand>(msg);
                    if (cc != null && !string.IsNullOrWhiteSpace(cc.Command))
                    {
                        switch (cc.Command.Trim().ToUpper())
                        {
                            case "COPY":
                                fi = new FileInfo(cc.Original);
                                if (fi.Exists)
                                {
                                    body = FileHelper.ReadFile(cc.Original, Encoding.UTF8);
                                    FileHelper.WriteFile(cc.Target, body, Encoding.UTF8, false);
                                    AppendText($"파일 복사 : {cc.Original}을(를) {cc.Target}(으)로");
                                }
                                else
                                {
                                    AppendText($"파일 복사 실패 : {cc.Original}을(를) 찾을 수 없습니다.");
                                }
                                break;
                        }
                    }
                }

                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                obj.ClearBuffer();

                // 수신 대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
            }
            catch (Exception ex)
            {
                AppendText(ex.Message);
                Logger.Current.Error(ex);
            }
        }

        void OnSendData(ServerCommand command)
        {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound)
            {
                AppendText("서버가 실행되고 있지 않습니다!");
                return;
            }

            if (string.IsNullOrEmpty(command.Command))
            {
                AppendText("명령이 입력되지 않았습니다!");
                return;
            }

            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));

            // 연결된 모든 클라이언트에게 전송한다.
            for (int i = connectedClients.Count - 1; i >= 0; i--)
            {
                Socket socket = connectedClients[i];
                try { socket.Send(bDts); }
                catch
                {
                    // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                    try { socket.Dispose(); } catch { }
                    connectedClients.RemoveAt(i);
                }
            }

            // 전송 완료 후 텍스트박스에 추가하고, 원래의 내용은 지운다.
            AppendText(string.Format("[보냄]{0}: {1}", thisAddress.ToString(), command.Command));
        }

        protected override void OnStop()
        {
            try
            {
                for (int i = connectedClients.Count - 1; i >= 0; i--)
                {
                    Socket socket = connectedClients[i];
                    try { socket.Disconnect(false); }
                    catch
                    {
                        // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                        try { socket.Dispose(); } catch { }
                        connectedClients.RemoveAt(i);
                    }
                }


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
