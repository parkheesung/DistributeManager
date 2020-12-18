using System;

namespace DM.Library
{
    public interface ISocketClient
    {
        Action<string> Receive { get; set; }
    }
}
