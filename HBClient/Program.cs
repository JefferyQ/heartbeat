using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExF.Application.Base;

namespace XService
{
    class Program
    {
        static void Main(string[] args)
        {
            var bootstrapper = new Bootstrapper();
            var programBase = new ApplicationBase(args, bootstrapper);
        }
    }
}
