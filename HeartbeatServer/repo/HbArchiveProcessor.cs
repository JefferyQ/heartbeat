using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Heartbeat;
using Timer = System.Timers.Timer;
using System.Diagnostics;
using System.Configuration;
using HeartbeatServer.Request;
using HeartbeatServer.Response;
using HeartbeatServer.dto;

namespace HeartbeatServer
{

    /// <summary>
    /// Keeps heartbeats for the last hour.
    /// </summary>
    public class HbArchiveProcessor
    {

        private readonly List<HbTempArchiveItem> _hbTempArchiveItems;
        private readonly List<HbArchiveItem> _hbArchiveItems;
        private readonly List<ServiceInfo> _allServicesInfo;

        public HbArchiveProcessor()
        {
            _hbTempArchiveItems = new List<HbTempArchiveItem>();
            _hbArchiveItems = new List<HbArchiveItem>();
            _allServicesInfo = new List<ServiceInfo>();

            DoTimerStuff();
            
            List<HbArchiveItem> archiveItems = BinarySerialization.ReadFromBinaryFile<List<HbArchiveItem>>("Archive.hb");
            List<ServiceInfo> allServicesInfo = BinarySerialization.ReadFromBinaryFile<List<ServiceInfo>>("ServicesInformation.hb");
            
            if (archiveItems != null)
                _hbArchiveItems = archiveItems;
            if (allServicesInfo != null)
                _allServicesInfo = allServicesInfo;
        }

        /// <summary>
        /// Add new AppStats to Temp Archive
        /// </summary>
        /// <param name="newAppStats"></param>
        public void AddAppStats(AppStats newAppStats)
        {
            bool acquiredLock = false;
            bool acquiredlockServices = false;
            try
            {
                Monitor.Enter(_hbTempArchiveItems, ref acquiredLock);
                Monitor.Enter(_allServicesInfo, ref acquiredlockServices);

                foreach (MethodExecutionStats newMethodStats in newAppStats.MethodStats)
                {

                    // Add or Update hbTempArchiveItems
                    var existingItem = _hbTempArchiveItems.SingleOrDefault(m=> m.ClientMachine == newAppStats.ClientMachine &&  m.ApplicationName == newAppStats.ApplicationName && m.MethodName == newMethodStats.MethodName);
                    if (existingItem == null)
                    {
                        _hbTempArchiveItems.Add(new HbTempArchiveItem
                        {
                            ApplicationName = newAppStats.ApplicationName,
                            ClientIp = newAppStats.ClientIp,
                            ClientMachine = newAppStats.ClientMachine,
                            StatDate = newAppStats.EndDate,
                            MethodName = newMethodStats.MethodName,
                            ExecutionCount = newMethodStats.ExecutionCount,
                            AverageDuration = newMethodStats.AverageDuration,
                            MaxDuration = newMethodStats.MaxDuration,
                            MinDuration = newMethodStats.MinDuration,
                            ExceptionCount = newMethodStats.ExceptionCount // exception count yoktu, eklendi
                        });
                    }
                    else
                    {
                        if (newMethodStats.MaxDuration > existingItem.MaxDuration)
                            existingItem.MaxDuration = newMethodStats.MaxDuration;
                        if (newMethodStats.MinDuration < existingItem.MinDuration)
                            existingItem.MinDuration = newMethodStats.MinDuration;
                        if (existingItem.ExecutionCount + newMethodStats.ExecutionCount > 0)
                            existingItem.AverageDuration = ((existingItem.AverageDuration * existingItem.ExecutionCount) + (newMethodStats.AverageDuration * newMethodStats.ExecutionCount)) / (existingItem.ExecutionCount + newMethodStats.ExecutionCount);
                        existingItem.ExecutionCount = existingItem.ExecutionCount + newMethodStats.ExecutionCount;
                    }

                    // Add or Update ServiceInfos
                    var existingServices = _allServicesInfo.SingleOrDefault(m => m.ApplicationName == newAppStats.ApplicationName && m.ServerName == newAppStats.ClientMachine);
                    if (existingServices == null)
                    {
                        _allServicesInfo.Add(new ServiceInfo()
                        {
                            ApplicationName = newAppStats.ApplicationName,
                            LastHeartBeatDate = newAppStats.EndDate,
                            ServerName = newAppStats.ClientMachine
                        });
                    }
                    else
                    {
                        existingServices.LastHeartBeatDate = newAppStats.EndDate;
                    }
                }
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : New AppStats added. ItemCount : " + _hbTempArchiveItems.Count);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : New AppStats added. ExecutionCount : " + _hbTempArchiveItems.Sum(m => m.ExecutionCount));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (acquiredLock)
                    Monitor.Exit(_hbTempArchiveItems);
                if (acquiredlockServices)
                    Monitor.Exit(_allServicesInfo);
            }
        }
        
        private void Archive()
        {
            Console.WriteLine("Archive started.");

            DateTime threshold = DateTime.Now;
            int hour = DateTime.Now.Hour;
            //int minute = DateTime.Now.Minute; // test için
            int day = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

            bool acquiredLockTemp = false;
            bool acquiredLockArchive = false;
            bool acquiredlockServices = false;
            try
            {
                Monitor.Enter(_hbTempArchiveItems, ref acquiredLockTemp);
                Monitor.Enter(_hbArchiveItems, ref acquiredLockArchive);
                Monitor.Enter(_allServicesInfo, ref acquiredlockServices);

                //_hbArchiveItems.RemoveAll(m => (threshold - m.ArchieveDate).TotalMinutes > 3);   for testing
                _hbArchiveItems.RemoveAll(m => (threshold - m.ArchieveDate).TotalMinutes > 32 * 24 * 60);

                foreach (HbTempArchiveItem newItem in _hbTempArchiveItems.Where(m => m.StatDate < threshold))
                {
                    var existingItem =
                        _hbArchiveItems.SingleOrDefault(
                            m => m.ApplicationName == newItem.ApplicationName && m.MethodName == newItem.MethodName && m.ClientMachine == newItem.ClientMachine
                                 && m.Day == day && m.Hour == hour);

                    if (existingItem == null)
                    {
                        _hbArchiveItems.Add(new HbArchiveItem
                        {
                            Day = day,
                            Hour = hour,
                            ExecutionCount = newItem.ExecutionCount,
                            ClientMachine = newItem.ClientMachine,
                            MethodName = newItem.MethodName,
                            MinDuration = newItem.MinDuration,
                            MaxDuration = newItem.MaxDuration,
                            ClientIp = newItem.ClientIp,
                            AverageDuration = newItem.AverageDuration,
                            ApplicationName = newItem.ApplicationName,
                            ExceptionCount = newItem.ExceptionCount, // exception count yoktu, eklendi
                            ArchieveDate = newItem.StatDate

                        });
                    }
                    else
                    {
                        if (newItem.MaxDuration > existingItem.MaxDuration)
                            existingItem.MaxDuration = newItem.MaxDuration;
                        if (newItem.MinDuration < existingItem.MinDuration)
                            existingItem.MinDuration = newItem.MinDuration;
                        if (existingItem.ExecutionCount + newItem.ExecutionCount > 0)
                            existingItem.AverageDuration = ((existingItem.AverageDuration * existingItem.ExecutionCount) + (newItem.AverageDuration * newItem.ExecutionCount)) / (existingItem.ExecutionCount + newItem.ExecutionCount);
                        existingItem.ExecutionCount = existingItem.ExecutionCount + newItem.ExecutionCount;
                    }
                }

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : TempArchive Expired. ItemCount : " + _hbTempArchiveItems.Count);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : TempArchive Expired. ExecutionCount : " + _hbTempArchiveItems.Sum(m => m.ExecutionCount));

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Archive updated. ItemCount : " + _hbArchiveItems.Count);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Archive updated. ExecutionCount : " + _hbArchiveItems.Sum(m => m.ExecutionCount));

                _hbTempArchiveItems.RemoveAll(m => m.StatDate < threshold);

                BinarySerialization.WriteToBinaryFile<List<HbArchiveItem>>("Archive.hb", _hbArchiveItems);
                BinarySerialization.WriteToBinaryFile<List<ServiceInfo>>("ServicesInformation.hb", _allServicesInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (acquiredLockTemp)
                    Monitor.Exit(_hbTempArchiveItems);
                if (acquiredLockArchive)
                    Monitor.Exit(_hbArchiveItems);
                if (acquiredlockServices)
                    Monitor.Exit(_allServicesInfo);
            }
        }

        private void DoTimerStuff()
        {
            var ArchiveIntervalSecondsString = ConfigurationManager.AppSettings["ArchiveIntervalSeconds"];

            int ArchiveIntervalSecondsOut;
            if (string.IsNullOrEmpty(ArchiveIntervalSecondsString) ||
                !int.TryParse(ArchiveIntervalSecondsString, out ArchiveIntervalSecondsOut))
            {
                Console.WriteLine("ArchiveIntervalSeconds parameter is invalid in Configuration. The value is set to 1 hour.");
                ArchiveIntervalSecondsOut = 3600;
            }

            var flushTimer = new Timer();
            flushTimer.Interval = ArchiveIntervalSecondsOut * 1000;
            flushTimer.Elapsed += FlushTimer_Elapsed;
            flushTimer.Start();
        }

        private void FlushTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Archive();
        }

        public GetServersResponse GetServers(GetServersRequest request)
        {
            var response = new GetServersResponse();
            response.ServerInfoList = new List<ServerInfo>();

            var list = _hbArchiveItems.Where(w => w.ApplicationName == request.ServiceName || request.AllServers).GroupBy(x => x.ClientMachine).Select(grp => grp.First()).ToList();

            foreach (var hbArchiveItem in list)
            {
                response.ServerInfoList.Add(new ServerInfo()
                {
                    ServerName = hbArchiveItem.ClientMachine
                });
            }           
            return response;
        }

        public GetServicesResponse GetServices(GetServicesRequest request)
        {
            var response = new GetServicesResponse();
            response.ServiceInfoList = new List<ServiceInfo>();

            response.ServiceInfoList =
                _allServicesInfo.Where(w => w.ServerName == request.ServerName || request.AllServices).ToList();

            return response;
        }

        public GetTopMethodLoadResponse TopNMethods(GetTopMethodLoadRequest request)
        {

            var response = new GetTopMethodLoadResponse();
            response.MethodLoadDetailList = new List<MethodLoadDetails>();

            var list = _hbArchiveItems.Where(x => x.ClientMachine == request.ServerName && x.ApplicationName == request.ServiceName);

            foreach (var hbArchieveItem in list)
                {
                    response.MethodLoadDetailList.Add(new MethodLoadDetails()
                    {
                        MethodName = hbArchieveItem.MethodName,
                        ApplicationName = hbArchieveItem.ApplicationName,
                        ServerName = hbArchieveItem.ClientMachine,
                        Load = hbArchieveItem.AverageDuration * hbArchieveItem.ExecutionCount,
                        ExceptionCount = hbArchieveItem.ExceptionCount,
                        ExecutionCount = hbArchieveItem.ExecutionCount

                    });
                }

            response.MethodLoadDetailList = response.MethodLoadDetailList.OrderByDescending(or => or.Load).Take(request.MethodNumber).ToList();
            return response;
        }

        public GetMethodDetailResponse MethodDetails(GetMethodDetailRequest request)
        {

            var response = new GetMethodDetailResponse {Details = new MethodDetails()};

            var list = _hbArchiveItems.Where(x => x.ClientMachine == request.ServerName && x.ApplicationName == request.ServiceName && x.MethodName == request.MethodName);

            response.Details.MethodName = request.MethodName;
            response.Details.ApplicationName = request.ServiceName;
            response.Details.TotalExceptionCount = list.Sum(s => s.ExceptionCount);
            response.Details.OverallAverageDuration = list.Sum(s => s.AverageDuration) / list.Sum(s => s.ExecutionCount);
            response.Details.FirstExecution = list.OrderBy(or => or.ArchieveDate).First().ArchieveDate;
            response.Details.LastExecution = list.OrderBy(or => or.ArchieveDate).Last().ArchieveDate;
            response.Details.TotalExecutionCount = list.Sum(s => s.ExecutionCount);

            return response;
        }

        public GetMethodDurationorCountResponse DurationOrCount(GetMethodDurationOrCountRequest request)
        {
            var today = DateTime.Now;
            var response = new GetMethodDurationorCountResponse();
            response.Details = new List<AverageOrDurationDetails>();

            var list =
                _hbArchiveItems.Where(
                    x =>
                        x.ArchieveDate.Day == today.Day && x.MethodName == request.MethodName &&
                        x.ClientMachine == request.ServerName && x.ApplicationName == request.ServiceName);

            foreach (var hbArchieveItem in list)
            {
                response.Details.Add(new AverageOrDurationDetails()
                {
                    AverageDuration = hbArchieveItem.AverageDuration,
                    ExecutionTime = hbArchieveItem.ArchieveDate,
                    ExecutionCount = hbArchieveItem.ExecutionCount
                });

            }

            if (request.DataType == "average")
                response.Details =
                    response.Details.OrderByDescending(or => or.AverageDuration).Take(request.MethodNumber).ToList();
            else
                response.Details =
                    response.Details.OrderByDescending(or => or.ExecutionCount).Take(request.MethodNumber).ToList();

            return response;

        }

        public GetMethodsOfServiceResponse GetMethodsOfService(GetMethodsOfServiceRequest request)
        {
            var response = new GetMethodsOfServiceResponse();
            response.MethodList = new List<MethodDetails>();

            var list =
                _hbArchiveItems.Where(
                    w => w.ClientMachine == request.ServerName && w.ApplicationName == request.ServiceName)
                    .GroupBy(x => x.MethodName)
                    .Select(grp => grp.First())
                    .ToList();

            foreach (var hbArchiveItem in list)
            {
                response.MethodList.Add(new MethodDetails()
                {
                    ApplicationName = hbArchiveItem.ApplicationName,
                    ServerName = hbArchiveItem.ClientMachine,
                    MethodName = hbArchiveItem.MethodName
                });
            }

            return response;

        }
    }
}