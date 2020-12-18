using OctopusV3.Core;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace DM.Library
{
    public class SocketServer : ISocketServer, ISocketConnection, IDisposable
    {
        private static readonly Lazy<SocketServer> lazy = new Lazy<SocketServer>(() => new SocketServer());

        public static SocketServer Current { get { return lazy.Value; } }

        private bool disposedValue;

        protected Socket socket { get; set; }

        protected IPAddress serverIP { get; set; }

        protected ConcurrentDictionary<string, Socket> connectedClients { get; set; } = new ConcurrentDictionary<string, Socket>();

        public bool IsConnection { get; set; } = false;

        public ILogHelper Logger { get; set; }

        public IPEndPoint EndPoint { get; set; }

        protected bool IsReady { get; set; } = false;

        protected string IPaddr { get; set; } = string.Empty;

        protected int Port { get; set; }

        protected int MaximumCount { get; set; } = 10;

        public Action<string> Received { get; set; }

        public Action<string> ClientConnectEvent { get; set; }

        public SocketServer()
        {
        }

        public void Setup(string ip, int port, int maximumReady = 10)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                throw new ArgumentException("IP Address is null or empty.");
            }

            try
            {
                this.IPaddr = ip;
                this.Port = port;
                this.MaximumCount = maximumReady;
                serverIP = IPAddress.Parse(this.IPaddr);
                EndPoint = new IPEndPoint(serverIP, this.Port);
                this.IsReady = true;
            }
            catch (Exception ex)
            {
                this.IsReady = false;
                if (Logger != null)
                {
                    Logger.Error(ex);
                }
                throw ex;
            }
        }

        public void Listen(Action Complete = null)
        {
            if (this.IsReady)
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    socket.Bind(this.EndPoint);
                    socket.Listen(this.MaximumCount);
                    IsConnection = true;
                    socket.BeginAccept(AcceptCallback, null);
                    if (Complete != null)
                    {
                        Complete();
                    }
                }
                catch (Exception ex)
                {
                    IsConnection = false;
                    if (socket != null)
                    {
                        socket.Dispose();
                        socket = null;
                    }
                    if (Logger != null)
                    {
                        Logger.Error(ex);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Do not set Config : Call by Setup");
            }
        }

        protected virtual void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = socket.EndAccept(ar);

            // 또 다른 클라이언트의 연결을 대기한다.
            socket.BeginAccept(AcceptCallback, null);

            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = client;

            // 연결된 클라이언트 리스트에 추가해준다.
            connectedClients.AddOrUpdate(client.RemoteEndPoint.ToString(), client, (oldKey, oldValue) => client);

            // 텍스트박스에 클라이언트가 연결되었다고 써준다.
            if (Logger != null)
            {
                Logger.Debug(string.Format("클라이언트 (@ {0})가 연결되었습니다.", client.RemoteEndPoint));
            }

            if (ClientConnectEvent != null)
            {
                ClientConnectEvent(client.RemoteEndPoint.ToString());
            }

            // 클라이언트의 데이터를 받는다.
            client.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
        }

        protected virtual void DataReceived(IAsyncResult ar)
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


                if (!string.IsNullOrWhiteSpace(msg))
                {
                    if (Received != null)
                    {
                        Received(msg);
                    }
                }

                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                obj.ClearBuffer();

                // 수신 대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.Error(ex);
                }
            }
        }

        public virtual void Send(string msg)
        {
            try
            {
                // 서버가 대기중인지 확인한다.
                if (!socket.IsBound)
                {
                    throw new Exception("서버가 실행되고 있지 않습니다!");
                }

                if (string.IsNullOrEmpty(msg))
                {
                    throw new Exception("내용이 입력되지 않았습니다!");
                }

                // 문자열을 utf8 형식의 바이트로 변환한다.
                byte[] bDts = Encoding.UTF8.GetBytes(msg);
                Socket tmp = null;

                // 연결된 모든 클라이언트에게 전송한다.
                foreach (Socket client in connectedClients.Values)
                {
                    try { client.Send(bDts); }
                    catch
                    {
                        // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                        try { client.Dispose(); } catch { }
                        connectedClients.TryRemove(client.RemoteEndPoint.ToString(), out tmp);
                    }
                }

                // 전송 완료 후 텍스트박스에 추가하고, 원래의 내용은 지운다.
                if (Logger != null)
                {
                    Logger.Debug(string.Format($"[보냄] : {msg}"));
                }
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.Error(ex);
                }
                throw ex;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    try
                    {
                        Socket tmp = null;

                        // 연결된 모든 클라이언트에게 전송한다.
                        foreach (Socket client in connectedClients.Values)
                        {
                            try { client.Disconnect(false); }
                            catch
                            {
                                // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                                try { client.Dispose(); } catch { }
                                connectedClients.TryRemove(client.RemoteEndPoint.ToString(), out tmp);
                            }
                        }

                        if (Logger != null)
                        {
                            Logger.Debug("Server Stop");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (Logger != null)
                        {
                            Logger.Error(ex);
                        }
                    }
                }

                disposedValue = true;
            }
        }

        ~SocketServer()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
