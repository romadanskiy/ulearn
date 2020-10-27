using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Failures
{
    public enum FailureType { UnexpectedShutdown, ShortNonResponding, HardwareFailures, ConnectionProblems }

    public class Device
    {
        public readonly int DeviceId;
        public readonly string Name;
        public readonly FailureType FailureType;
        public readonly DateTime FailureTime;

        public Device(int deviceId, string name, FailureType failureType, DateTime failureTime)
        {
            DeviceId = deviceId;
            Name = name;
            FailureType = failureType;
            FailureTime = failureTime;
        }
    }

    public class Common
    {
        public static int IsFailureSerious(int failureType)
        {
            var flag = IsFailureSerious((FailureType)failureType);
            
            return (flag) ? 1 : 0;
        }
        
        public static bool IsFailureSerious(FailureType failureType)
        {
            // return (int)failureType % 2 == 0;
            return failureType == FailureType.UnexpectedShutdown || failureType == FailureType.HardwareFailures;
        }
        
        public static int Earlier(object[] v, int day, int month, int year)
        {
            var vDate = new DateTime((int) v[2], (int) v[1], (int) v[0]);
            var date = new DateTime(year, month, day);
            var flag = Earlier(vDate, date);

            return (flag) ? 1 : 0;
        }
        
        public static bool Earlier(DateTime vDate, DateTime date)
        {
            return vDate < date;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">
        /// 0 for unexpected shutdown, 
        /// 1 for short non-responding, 
        /// 2 for hardware failures, 
        /// 3 for connection problems
        /// </param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(year, month, day);
            var devicesList = new List<Device>();
            for (var i = 0; i < devices.Count; i++)
            {
                var failureTime = new DateTime((int) times[i][2], (int) times[i][1], (int) times[i][0]);
                var device = new Device(deviceId[i], 
                                        devices[i]["Name"].ToString(), 
                                        (FailureType)failureTypes[i],
                                        failureTime);
                devicesList.Add(device);
            }

            return FindDevicesFailedBeforeDate(date, devicesList);
        }

        public static List<string> FindDevicesFailedBeforeDate(DateTime date, List<Device> devices)
        {
            return (
                from device
                in devices
                where Common.IsFailureSerious(device.FailureType) && Common.Earlier(device.FailureTime, date) 
                select device.Name
                ).ToList();
        }
    }
}