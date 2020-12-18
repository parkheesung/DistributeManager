using System;

namespace DM.Library
{
    public interface ISocketServer
    {
        Action<string> Received { get; set; }

        void Listen(Action Complete);

        

    }
}
