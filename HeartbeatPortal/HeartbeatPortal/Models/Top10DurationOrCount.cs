using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HeartBeatPortal.Models
{
    public class Top10DurationOrCount
    {
        public long Duration { get; set; }
        public int ExeCount { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}