using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Heartbeat;

namespace XService
{
    class XService : IXService
    {
        private readonly ClientProcessor _clientProcessor;

        public XService(ClientProcessor clientProcessor)
        {
            _clientProcessor = clientProcessor;
        }

        public void DoX()
        {
            Thread.Sleep(500);
        }

        public void DoY()
        {
            Thread.Sleep(800);
        }
    }
}
