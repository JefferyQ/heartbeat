using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Heartbeat;
using Timer = System.Timers.Timer;
using System.Diagnostics;

namespace HeartbeatServer
{

    /// <summary>
    /// Keeps heartbeats for the last hour.
    /// </summary>
    public class HbArchiveProcessor
    {

        private readonly List<HbTempArchiveItem> _hbTempArchiveItems;
        private readonly List<HbArchiveItem> _hbArchiveItems;

        public HbArchiveProcessor()
        {
            _hbTempArchiveItems = new List<HbTempArchiveItem>();
            _hbArchiveItems = new List<HbArchiveItem>();
            DoTimerStuff();



            //TODO : Load from DB
            //List<AppStats> appStats = BinarySerialization.ReadFromBinaryFile<List<AppStats>>("Dump.hb");

            //if (appStats != null)
            //{
            //    foreach (var appStat in appStats)
            //    {
            //        AddAppStats(appStat);
            //    }

            //}


            //HbTempArchiveItems = BinarySerialization.ReadFromBinaryFile<List<HbTempArchiveItem>>("Repo\\HbTempArchiveItems.hb");
            //
        }

        /// <summary>
        /// Add new AppStats to Temp Archive
        /// </summary>
        /// <param name="newAppStats"></param>
        public void AddAppStats(AppStats newAppStats)
        {
            bool acquiredLock = false;
            try
            {
                Monitor.Enter(_hbTempArchiveItems, ref acquiredLock);

                foreach (MethodExecutionStats newMethodStats in newAppStats.MethodStats)
                {
                    var existingItem = _hbTempArchiveItems.SingleOrDefault(m=> m.ApplicationName == newAppStats.ApplicationName && m.MethodName == newMethodStats.MethodName);
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
                        if(existingItem.ExecutionCount + newMethodStats.ExecutionCount > 0)
                            existingItem.AverageDuration = ((existingItem.AverageDuration * existingItem.ExecutionCount) + (newMethodStats.AverageDuration * newMethodStats.ExecutionCount)) / existingItem.ExecutionCount + newMethodStats.ExecutionCount;
                        existingItem.ExecutionCount = existingItem.ExecutionCount + newMethodStats.ExecutionCount;
                    }
                }
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : New AppStats added. ItemCount : " + _hbTempArchiveItems.Count);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : New AppStats added. ExecutionCount : " + _hbTempArchiveItems.Sum(m=> m.ExecutionCount));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (acquiredLock)
                    Monitor.Exit(_hbTempArchiveItems);
            }

            //BinarySerialization.WriteToBinaryFile("Repo\\HbTempArchiveItems.hb", HbTempArchiveItems, false);
        }


        private void Archive()
        {
            Console.WriteLine("Archive started.");

            //TODO: Export items older then 1 hour to HbArchiveItem and delete from HbTempArchiveItems

            //DateTime threshold = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            DateTime threshold = DateTime.Now;
            int hour = DateTime.Now.Hour;
            //int minute = DateTime.Now.Minute; // test için
            int day = int.Parse(DateTime.Now.ToString("yyyyMMdd"));

            bool acquiredLockTemp = false;
            bool acquiredLockArchive = false;
            try
            {
                Monitor.Enter(_hbTempArchiveItems, ref acquiredLockTemp);
                Monitor.Enter(_hbArchiveItems, ref acquiredLockArchive);

                //TODO : Test edin.

                // do stuff

               
                //DateTime archiveThreshold = DateTime.Now;
                //_hbArchiveItems.RemoveAll(m => DateTime.ParseExact(m.Day.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture) < archiveThreshold); // threshold archiveThreshold olarak değiştirildi


                foreach (HbTempArchiveItem newItem in _hbTempArchiveItems.Where(m => m.StatDate < threshold))
                {
                    var existingItem =
                        _hbArchiveItems.SingleOrDefault(
                            m => m.ApplicationName == newItem.ApplicationName && m.MethodName == newItem.MethodName
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
                            existingItem.AverageDuration = ((existingItem.AverageDuration * existingItem.ExecutionCount) + (newItem.AverageDuration * newItem.ExecutionCount)) / existingItem.ExecutionCount + newItem.ExecutionCount;
                        existingItem.ExecutionCount = existingItem.ExecutionCount + newItem.ExecutionCount;
                    }
                }



                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : TempArchive Expired. ItemCount : " + _hbTempArchiveItems.Count);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : TempArchive Expired. ExecutionCount : " + _hbTempArchiveItems.Sum(m => m.ExecutionCount));

                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Archive updated. ItemCount : " + _hbArchiveItems.Count);
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " : Archive updated. ExecutionCount : " + _hbArchiveItems.Sum(m => m.ExecutionCount));

                //_hbArchiveItems.RemoveAll(m => (threshold - m.ArchieveDate).TotalMinutes > 3);   for testing

                _hbArchiveItems.RemoveAll(m => (threshold - m.ArchieveDate).TotalMinutes > 32 * 24 * 60);

                _hbTempArchiveItems.RemoveAll(m => m.StatDate < threshold);

                //if (_hbArchiveItems.Count != 0)
                //{
                //    BinarySerialization.WriteToBinaryFile<List<HbArchiveItem>>("Dump.hb", _hbArchiveItems);
                //}


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
            }
        }

        private void DoTimerStuff()
        {
            var flushTimer = new Timer();
            flushTimer.Interval = 10 * 1000;
            flushTimer.Elapsed += FlushTimer_Elapsed;
            flushTimer.Start();
        }

        private void FlushTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Archive();
        }
    }
}