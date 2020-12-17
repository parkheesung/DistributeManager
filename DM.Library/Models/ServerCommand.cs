using System;

namespace DM.Library
{
    public class ServerCommand
    {
        public string Command { get; set; } = string.Empty;

        public string Target { get; set; } = string.Empty;

        public string Original { get; set; } = string.Empty;

        public string Option { get; set; } = string.Empty;

        public ServerCommand()
        {
        }
    }
}
