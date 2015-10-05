using System;
using ExF.Application.Base;
using System.Configuration;

namespace HeartbeatServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            var programBase = new ApplicationBase(args, bootstrapper);
        }
    }
}