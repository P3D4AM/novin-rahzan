using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace TimeAttendanceDAL.Dal.Device;

public class DeviceUserInfo
{
	public int DeviceId { get; set; }

	public string DeviceName { get; set; } = string.Empty;


	public int DeviceUserId { get; set; }

	public string DeviceUserName { get; set; }

	public static async Task<string> GetLastHardwareCodeAsync(string contextGuid, int deviceId, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (deviceId == 0)
			{
				return null;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}

	public static async Task<List<DeviceUserInfo>> GetAllDeviceDataAsync(string contextGuid, int personId, CancellationToken token = default(CancellationToken))
	{
		List<DeviceUserInfo> ret = new List<DeviceUserInfo>();
		try
		{
			List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(contextGuid, token);
			List<HardwareCodeCus> hardwareCodeList = await HardwareCodeCus.GetAsync(contextGuid, personId, token);
			foreach (DeviceInfo device in devices)
			{
				HardwareCodeCus hardwareCode = hardwareCodeList.SingleOrDefault((HardwareCodeCus p) => p.DeviceId == device.Id);
				if (hardwareCode != null)
				{
					await DeviceInfo.GetUserNameAsync(contextGuid, device, hardwareCode);
					ret.Add(new DeviceUserInfo
					{
						DeviceId = device.Id,
						DeviceName = device.DeviceName,
						DeviceUserId = int.Parse(hardwareCode.HardwareCode),
						DeviceUserName = ""
					});
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return null;
	}
}
