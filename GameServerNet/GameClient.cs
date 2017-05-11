using System;
using System.Net;

namespace GameServerNet
{
    public class GameClient
    {
        public readonly Guid Id;
        public readonly IPAddress IPAddress;
        public readonly uint Port;

        public GameClient(Guid id, IPAddress ipAddress, uint port)
        {
            Id = id;
            IPAddress = ipAddress;
            Port = port;
        }
    }
}