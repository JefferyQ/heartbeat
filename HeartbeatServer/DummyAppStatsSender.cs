using System;
using System.Collections.Generic;
using System.Timers;
using Heartbeat;

namespace HeartbeatServer
{
    internal class DummyAppStatsSender
    {
        private readonly AppStatsProcessor _appStatsProcessor;
        private int inc = 0;
        public DummyAppStatsSender(AppStatsProcessor appStatsProcessor)
        {
            _appStatsProcessor = appStatsProcessor;

            var flushTimer = new Timer();
            flushTimer.Interval = 1 * 1000;
            flushTimer.Elapsed += FlushTimer_Elapsed;
            flushTimer.Start();
        }

        private void FlushTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Send();
            inc = inc + 5;
        }

        internal void Send()
        {
            AppStats appStats = new AppStats
            {
                ClientMachine = "ClientMachine1",
                ApplicationName = "ApplicationName",
                ClientIp = "ClientIp",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now.AddMinutes(-1),
                Id = Guid.NewGuid(),
                MethodStats = new List<MethodExecutionStats>
                {
                    new MethodExecutionStats
                    {
                        ExceptionCount = 0,
                        MinDuration = 0,
                        MaxDuration = 0,
                        MethodName = "MethodName1",
                        AverageDuration = 0,
                        ExecutionCount = 0
                    },
                    new MethodExecutionStats
                    {
                        ExceptionCount = 5,
                        MinDuration = 5,
                        MaxDuration = 5,
                        MethodName = "MethodName2",
                        AverageDuration = 5,
                        ExecutionCount = 5
                    },
                    new MethodExecutionStats
                    {
                        ExceptionCount = 10,
                        MinDuration = 10,
                        MaxDuration = 10,
                        MethodName = "MethodName3",
                        AverageDuration = 10 + inc,
                        ExecutionCount = 10
                    }
                }
            };
            
            _appStatsProcessor.AddNewAppStats(appStats);
        }
    }
} 