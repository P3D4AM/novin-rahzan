using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketParser.Attendance.Entities;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.Taradod;
using zkemkeeper;

namespace TimeAttendanceDevices.FLRZk;

public class ClsZK
{
	private readonly int _iMachineNumber = 1;

	private int _Port = 5005;

	private string _IP = "";

	private string _Pass = "";

	private int idwErrorCode;

	public string ContextGuid { get; set; }

	public bool Conected { get; private set; }

	public CZKEMClass AxCZKEM1 { get; set; } = new CZKEMClass();


	public ClsZK(string contextGuid, int DeviceNO, string CommunicationPass, string IP, int TCPPort)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		ContextGuid = contextGuid;
		_iMachineNumber = DeviceNO;
		_IP = IP;
		_Port = TCPPort;
		_Pass = CommunicationPass;
		Conected = AxCZKEM1.Connect_Net(IP, TCPPort);
	}

	public async Task<List<DeviceLogOnPerson>> GetAll(bool GetNewTaradodOnly)
	{
		List<DeviceLogOnPerson> result = new List<DeviceLogOnPerson>();
		try
		{
			if (!Conected)
			{
				return result;
			}
			List<ZkRecord> list = GetLog();
			Disconnect();
			if (list == null || list.Count == 0)
			{
				Disconnect();
				return result;
			}
			if (GetNewTaradodOnly)
			{
				List<LogOnPersonelTopLastTaradods> lastTaradods = null;
				if (DeviceInfo.GetByDeviceNoAsync(ContextGuid, _iMachineNumber) != null)
				{
					lastTaradods = await LogOnPersonelTopLastTaradods.GetAsync(ContextGuid, 1, _iMachineNumber, new CancellationTokenSource(3000).Token);
				}
				else
				{
					lastTaradods = await LogOnPersonelTopLastTaradods.GetAsync(ContextGuid, 1, new CancellationTokenSource(3000).Token);
				}
				if (lastTaradods != null && lastTaradods.Count > 0)
				{
					list = list.Where((ZkRecord p) => p.DateM >= lastTaradods[0].DateTimeLogM).ToList();
				}
			}
			return RecordAllDevice.GetAllFromZk(ContextGuid, list);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRZk\\ClsZK.cs", 64);
		}
		return result;
	}

	public bool TestConnection()
	{
		try
		{
			if (!Conected)
			{
				return false;
			}
			Disconnect();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TestConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRZk\\ClsZK.cs", 88);
			return false;
		}
	}

	public List<ZkRecord> GetLog()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (!Conected)
		{
			MessageBox.Show("Please connect the device first");
			return new List<ZkRecord>();
		}
		List<ZkRecord> list = new List<ZkRecord>();
		int num = 0;
		int idwWorkcode = 0;
		AxCZKEM1.EnableDevice(_iMachineNumber, false);
		if (AxCZKEM1.ReadGeneralLogData(_iMachineNumber))
		{
			string sdwEnrollNumber = default(string);
			int idwVerifyMode = default(int);
			int idwInOutMode = default(int);
			int idwYear = default(int);
			int idwMonth = default(int);
			int idwDay = default(int);
			int idwHour = default(int);
			int idwMinute = default(int);
			int idwSecond = default(int);
			while (AxCZKEM1.SSR_GetGeneralLogData(_iMachineNumber, ref sdwEnrollNumber, ref idwVerifyMode, ref idwInOutMode, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond, ref idwWorkcode))
			{
				list.Add(new ZkRecord(_iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode));
			}
		}
		else
		{
			AxCZKEM1.GetLastError(ref num);
			if (num != 0)
			{
				MessageBox.Show("Reading data from terminal failed,ErrorCode: " + num);
			}
			else
			{
				MessageBox.Show("No data from terminal returns!");
			}
		}
		AxCZKEM1.EnableDevice(_iMachineNumber, true);
		return list;
	}

	public bool Disconnect()
	{
		AxCZKEM1.GetLastError(ref idwErrorCode);
		Conected = false;
		return Conected;
	}
}
