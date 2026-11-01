using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace TimeAttendanceDevices.FLRHysoon;

public class clsHysoon : IHasContext
{
	private readonly int _vnMachineNumber = 1;

	private readonly int _vnLicense = 1261;

	private readonly string _vpszIPAddress = "";

	private readonly int _vpszNetPort;

	private int _vpszNetPassword;

	private int _vnTimeOut = 5000;

	private int _vnProtocolType;

	private bool _connect;

	private bool _GetNewTaradodOnly;

	public string ContextGuid { get; set; }

	public bool Conected
	{
		get
		{
			_connect = FKAttendDLL.nCommHandleIndex > 0;
			return _connect;
		}
	}

	public clsHysoon(string contextGuid, int deviceNum, int password, string ip, int port, bool getNewTaradodOnly)
	{
		ContextGuid = contextGuid;
		_GetNewTaradodOnly = getNewTaradodOnly;
		_vnMachineNumber = deviceNum;
		_vpszNetPassword = password;
		_vpszIPAddress = ip;
		_vpszNetPort = port;
		_vnTimeOut = 5000;
		_vnLicense = 1261;
		_vnProtocolType = 0;
		FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectNet(_vnMachineNumber, _vpszIPAddress, _vpszNetPort, _vnTimeOut, _vnProtocolType, _vpszNetPassword, _vnLicense);
	}

	public clsHysoon(string contextGuid, int deviceNum)
	{
		ContextGuid = contextGuid;
		_vnMachineNumber = deviceNum;
		_vnLicense = 1261;
		FKAttendDLL.nCommHandleIndex = FKAttendDLL.FK_ConnectUSB(_vnMachineNumber, _vnLicense);
	}

	public bool Disconnect()
	{
		try
		{
			FKAttendDLL.FK_DisConnect(FKAttendDLL.nCommHandleIndex);
			_connect = false;
			return _connect;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 77);
			return false;
		}
	}

	public List<DeviceLogOnPerson> GetAllLogOnPublic()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		int num = 0;
		int personNo = 0;
		int num2 = 0;
		int ioMode = 0;
		DateTime dateTimeLogM = new DateTime(2020, 12, 21, 17, 52, 0);
		int num3 = (_GetNewTaradodOnly ? 1 : 0);
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			num = 1;
			if (!Conected)
			{
				return list;
			}
			int num4 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0);
			num = 2;
			num4 = FKAttendDLL.FK_LoadGeneralLogData(FKAttendDLL.nCommHandleIndex, num3);
			num = 3;
			int num5 = 1;
			while (true)
			{
				num = 4;
				num4 = FKAttendDLL.FK_GetGeneralLogData(FKAttendDLL.nCommHandleIndex, ref personNo, ref num2, ref ioMode, ref dateTimeLogM);
				num = 5;
				if (num4 != 1)
				{
					break;
				}
				if (num4 == 1)
				{
					list.Add(new DeviceLogOnPerson(ContextGuid)
					{
						PersonNo = personNo,
						DeviceNo = _vnMachineNumber,
						VerifyMode = num2.ToString(),
						IoMode = ioMode,
						DateTimeLogM = dateTimeLogM
					});
				}
				num = 6;
				num5++;
			}
			num = 7;
			Disconnect();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, $"line={num}", "GetAllLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 153);
		}
		return list;
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TestConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 230);
			return false;
		}
	}

	public List<EnrollData> FuncGetEnrollData(List<int> pEnrollList)
	{
		int vSEnrollNumber = 0;
		int num = 0;
		int machinPrivilage = 0;
		int enabled = 0;
		byte[] array = new byte[100000];
		int password = 0;
		List<EnrollDataDet> list = new List<EnrollDataDet>();
		try
		{
			int num2 = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0);
			if (num2 == 1)
			{
				num2 = FKAttendDLL.FK_ReadAllUserID(FKAttendDLL.nCommHandleIndex);
				if (num2 == 1)
				{
					num2 = FKAttendDLL.FK_GetAllUserID(FKAttendDLL.nCommHandleIndex, ref vSEnrollNumber, ref num, ref machinPrivilage, ref enabled);
					while (num2 == 1)
					{
						if (!pEnrollList.Any((int q) => q.Equals(vSEnrollNumber)))
						{
							continue;
						}
						num2 = FKAttendDLL.FK_GetEnrollData(FKAttendDLL.nCommHandleIndex, vSEnrollNumber, num, ref machinPrivilage, array, ref password);
						if (num2 == 1)
						{
							int num3 = 1700;
							switch (num)
							{
							case 0:
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
								num3 = 1700;
								break;
							case 10:
								num3 = 100;
								break;
							case 11:
								num3 = 100;
								break;
							case 12:
								num3 = 11500;
								break;
							}
							List<byte> list2 = array.ToList();
							list2.RemoveRange(num3, 100000 - num3);
							EnrollDataDet item = new EnrollDataDet
							{
								EnrollNo = vSEnrollNumber,
								DeviceNo = (short)_vnMachineNumber,
								apnBackupNumbar = num,
								MachinPrivilage = machinPrivilage,
								EnrollData = list2,
								Password = password,
								Enabled = enabled
							};
							list.Add(item);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FuncGetEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 312);
		}
		return EnrollData.ConvertEnrollDatas(list);
	}

	public int FuncPutEnrollData(List<EnrollData> pEnrollList)
	{
		int num = 0;
		try
		{
			List<EnrollDataDet> list = EnrollData.ConvertEnrollDataToDets(pEnrollList);
			num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0);
			if (num == 1)
			{
				foreach (EnrollDataDet item in list)
				{
					num = FKAttendDLL.FK_PutEnrollData(FKAttendDLL.nCommHandleIndex, item.EnrollNo, item.apnBackupNumbar, _vnMachineNumber, item.EnrollData.ToArray(), item.Password);
					if (num != 1)
					{
						break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FuncPutEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 339);
		}
		return num;
	}

	public int FuncClearEnrollData()
	{
		int result = 0;
		try
		{
			result = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0);
			result = FKAttendDLL.FK_EmptyEnrollData(FKAttendDLL.nCommHandleIndex);
			result = FKAttendDLL.FK_EmptySuperLogData(FKAttendDLL.nCommHandleIndex);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "FuncClearEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 355);
		}
		return result;
	}

	public bool SetDateTime(DateTime pDateTime)
	{
		if (FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0) == 1)
		{
			return FKAttendDLL.FK_SetDeviceTime(FKAttendDLL.nCommHandleIndex, pDateTime) > 0;
		}
		return false;
	}

	public ReturnedSaveFuncInfoWithValue<string> GetUserName(int enrollNumber)
	{
		ReturnedSaveFuncInfoWithValue<string> val = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = ""
		};
		try
		{
			int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0);
			if (num != 1)
			{
				val.AddError("ارتباط با دستگاه برقرار نشد. \r\n کد خطا: No Device", "GetUserName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 384);
				return val;
			}
			string value = new string(' ', 256);
			if (num == 1)
			{
				val.value = value;
				return val;
			}
			val.AddError($"کد  ({enrollNumber})  در دستگاه تعریف نشده است\r\nکد خطا: " + num, "GetUserName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 400);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return val;
	}

	public ReturnedSaveFuncInfoWithValue<bool> SetUserName(int enrollNumber, string name)
	{
		ReturnedSaveFuncInfoWithValue<bool> val = new ReturnedSaveFuncInfoWithValue<bool>
		{
			value = false
		};
		try
		{
			int num = FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)0);
			if (num != 1)
			{
				val.AddError("ارتباط با دستگاه برقرار نشد.\r\n کد خطا: No Device", "SetUserName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 417);
				return val;
			}
			num = 1;
			FKAttendDLL.FK_EnableDevice(FKAttendDLL.nCommHandleIndex, (byte)1);
			if (num == 1)
			{
				val.AddInformation("کاربر (" + name + ") ثبت شد", "", "SetUserName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 428);
				val.value = true;
				return val;
			}
			val.AddError($"ثبت کاربر ({name})  با کد  ({enrollNumber})  با خطا مواجه شد \r\n خطا: {num}", "SetUserName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 433);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			val.AddError(ex.Message, "SetUserName", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLRHysoon\\clsHysoon.cs", 435);
		}
		return val;
	}
}
