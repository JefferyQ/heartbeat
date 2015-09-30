using System;
using System.Collections.Generic;

namespace Heartbeat
{
    public class InfoReserve
    {
        public DateTime Created { get; set; }
        public List<MethodExecutionInfo> ExecStats { get; set; }
    }
}