using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxSBXPCLib;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SBXPCSampleCSharp;
using Serilog;
using TimeAttendanceDAL.Dal.Enroll;

namespace TimeAttendanceDevices.FLREbio;

public class FrmEbio : Form, IHasContext
{
	private int DATASIZE = 354;

	private int DATASIZE_FACE = 6917;

	private int NAMESIZE = 54;

	private int[] gTemplngEnrollData;

	private byte[] gbytEnrollData;

	private byte[] gbytEnrollData1;

	private int[] gTempEnrollName;

	private int glngEnrollPData;

	private ASCIIEncoding ascii;

	private AxSBXPC bpc;

	private int _gMachineNumber = 1;

	private readonly int _Password;

	private string _IP = "";

	private int _Port;

	public DataSet dsEnrolls;

	private IContainer components;

	public string ContextGuid { get; set; }

	public bool Conected { get; private set; }

	private void CreateAxSbxpcControl()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmEbio));
			bpc = new AxSBXPC();
			((ISupportInitialize)bpc).BeginInit();
			((AxHost)bpc).Enabled = true;
			((Control)bpc).Location = new Point(10, 9);
			((Control)bpc).Name = "bpc";
			((AxHost)bpc).OcxState = (State)componentResourceManager.GetObject("bpc.OcxState");
			((Control)bpc).Size = new Size(100, 50);
			((Control)bpc).TabIndex = 37;
			((Control)this).Controls.Add((Control)(object)bpc);
			((ISupportInitialize)bpc).EndInit();
			bpc.DotNET();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateAxSbxpcControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 65);
		}
	}

	public FrmEbio(string contextGuid, int deviceNum, int password, string ip, int port)
	{
		ContextGuid = contextGuid;
		try
		{
			InitializeComponent();
			CreateAxSbxpcControl();
			_gMachineNumber = deviceNum;
			_Password = password;
			_IP = ip;
			_Port = port;
			gbytEnrollData = new byte[DATASIZE_FACE * 5];
			gbytEnrollData1 = new byte[DATASIZE_FACE * 5];
			gTemplngEnrollData = new int[DATASIZE_FACE];
			gTempEnrollName = new int[NAMESIZE];
			ascii = new ASCIIEncoding();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 96);
		}
	}

	public FrmEbio(string contextGuid, int deviceNum)
	{
		ContextGuid = contextGuid;
		try
		{
			InitializeComponent();
			CreateAxSbxpcControl();
			_gMachineNumber = deviceNum;
			gbytEnrollData = new byte[DATASIZE_FACE * 5];
			gbytEnrollData1 = new byte[DATASIZE_FACE * 5];
			gTemplngEnrollData = new int[DATASIZE_FACE];
			gTempEnrollName = new int[NAMESIZE];
			ascii = new ASCIIEncoding();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 115);
		}
	}

	public async Task<bool> ConnectLocalAsync()
	{
		try
		{
			Stopwatch sw = Stopwatch.StartNew();
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(1000);
				if (((Control)this).IsDisposed)
				{
					return false;
				}
				if (sw.ElapsedMilliseconds > 30000)
				{
					break;
				}
			}
			if (((Control)this).IsDisposed)
			{
				return false;
			}
			if (sw.ElapsedMilliseconds > 30000)
			{
				WebErrorLog.ErrorLogInstance.StartLog($"Invoke delay:{sw.ElapsedMilliseconds}(ms) and handle is not created!", "", "ConnectLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 134);
				return false;
			}
			try
			{
				Conected = bpc.ConnectTcpip(_gMachineNumber, ref _IP, _Port, _Password);
				return Conected;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error******************************************************:\r\n " + ex.Message);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "ConnectLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 150);
		}
		return false;
	}

	public async Task<bool> ConnectSerialAsync()
	{
		try
		{
			Stopwatch sw = Stopwatch.StartNew();
			while (!((Control)this).IsHandleCreated)
			{
				await Task.Delay(1000);
				if (((Control)this).IsDisposed)
				{
					return false;
				}
				if (sw.ElapsedMilliseconds > 30000)
				{
					break;
				}
			}
			if (((Control)this).IsDisposed)
			{
				return false;
			}
			if (sw.ElapsedMilliseconds > 30000)
			{
				WebErrorLog.ErrorLogInstance.StartLog($"Invoke delay:{sw.ElapsedMilliseconds}(ms) and handle is not created!", "", "ConnectSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 173);
				return false;
			}
			try
			{
				Conected = bpc.ConnectSerial(_gMachineNumber, 0, 0);
				return Conected;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error******************************************************:\r\n " + ex.Message);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "ConnectSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 189);
		}
		return false;
	}

	public List<RecordEbio> GetLog(bool deleteAfter)
	{
		int num = 30000;
		int vTMachineNumber = 0;
		int vSEnrollNumber = 0;
		int vSMachineNumber = 0;
		int vVerifyMode = 0;
		int vYear = 1900;
		int vMonth = 1;
		int vDay = 1;
		int vHour = 0;
		int vMinute = 0;
		int vSecond = 0;
		List<RecordEbio> list = new List<RecordEbio>();
		try
		{
			if (!bpc.EnableDevice(_gMachineNumber, -1))
			{
				return null;
			}
			bool flag = bpc.ReadAllGLogData(_gMachineNumber);
			if (deleteAfter)
			{
				bpc.EmptyGeneralLogData(_gMachineNumber);
			}
			int num2 = 1;
			do
			{
				flag = bpc.GetAllGLogData(_gMachineNumber, ref vTMachineNumber, ref vSEnrollNumber, ref vSMachineNumber, ref vVerifyMode, ref vYear, ref vMonth, ref vDay, ref vHour, ref vMinute, ref vSecond);
				if (!flag)
				{
					return list;
				}
				if (flag && num2 != 1)
				{
					list.Add(new RecordEbio(vTMachineNumber, vSEnrollNumber, vSMachineNumber, vVerifyMode, vYear, vMonth, vDay, vHour, vMinute, vSecond));
				}
				num2++;
			}
			while (num2 <= num);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 235);
		}
		return list;
	}

	public List<DeviceLogOnPerson> GetAllLogOnPublic(bool deleteAfter, bool getNewTaradodOnly)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		int num = 30000;
		int num2 = 0;
		int personNo = 0;
		int deviceNo = 0;
		int num3 = 0;
		int year = 1900;
		int month = 1;
		int day = 1;
		int hour = 0;
		int minute = 0;
		int second = 0;
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			if (!Conected)
			{
				Disconnect();
				return null;
			}
			if (!bpc.EnableDevice(_gMachineNumber, -1))
			{
				return null;
			}
			if (!getNewTaradodOnly)
			{
				bpc.ReadAllGLogData(_gMachineNumber);
			}
			else
			{
				bpc.ReadGeneralLogData(_gMachineNumber);
			}
			int num4 = 1;
			while (true)
			{
				try
				{
					if (!bpc.GetAllGLogData(_gMachineNumber, ref num2, ref personNo, ref deviceNo, ref num3, ref year, ref month, ref day, ref hour, ref minute, ref second))
					{
						return list;
					}
					DeviceLogOnPerson item = new DeviceLogOnPerson(ContextGuid)
					{
						PersonNo = personNo,
						DeviceNo = deviceNo,
						IoMode = (short)num3,
						DateTimeLogM = new DateTime(year, month, day, hour, minute, second)
					};
					list.Add(item);
					num4++;
					if (num4 > num)
					{
						bpc.EmptyGeneralLogData(_gMachineNumber);
						break;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex, "GetLogOnPublic_while:i:" + num4, "GetAllLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 291);
				}
			}
			if (deleteAfter)
			{
				bpc.EmptyGeneralLogData(_gMachineNumber);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetAllLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 298);
		}
		return list;
	}

	public List<DeviceLogOnPerson> GetNewLogOnPublic(bool deleteAfter)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		int num = 30000;
		int num2 = 0;
		int personNo = 0;
		int deviceNo = 0;
		int num3 = 0;
		int year = 1900;
		int month = 1;
		int day = 1;
		int hour = 0;
		int minute = 0;
		int second = 0;
		List<DeviceLogOnPerson> list = new List<DeviceLogOnPerson>();
		try
		{
			if (!Conected)
			{
				Disconnect();
				return list;
			}
			if (!bpc.EnableDevice(_gMachineNumber, -1))
			{
				return null;
			}
			bool flag = bpc.ReadGeneralLogData(_gMachineNumber);
			if (deleteAfter)
			{
				bpc.EmptyGeneralLogData(_gMachineNumber);
			}
			int num4 = 1;
			while (true)
			{
				try
				{
					flag = bpc.GetGeneralLogData(_gMachineNumber, ref num2, ref personNo, ref deviceNo, ref num3, ref year, ref month, ref day, ref hour, ref minute, ref second);
					if (!flag)
					{
						return list;
					}
					if (flag && num4 != 1)
					{
						list.Add(new DeviceLogOnPerson(ContextGuid)
						{
							PersonNo = personNo,
							DeviceNo = deviceNo,
							VerifyMode = num3.ToString(),
							DateTimeLogM = new DateTime(year, month, day, hour, minute, second)
						});
					}
					num4++;
					if (num4 > num)
					{
						break;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex, "GetNewLogOnPublic_while:i:" + num4, "GetNewLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 350);
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetNewLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 356);
		}
		return list;
	}

	public bool Disconnect()
	{
		try
		{
			for (int i = 0; i < 3; i++)
			{
				try
				{
					bpc.Disconnect();
					Conected = false;
					return true;
				}
				catch (Exception ex)
				{
					Log.Error(ex, "");
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 378);
		}
		return false;
	}

	private bool TestIsConnected()
	{
		try
		{
			if (!Conected)
			{
				Disconnect();
				return false;
			}
			Disconnect();
			return true;
		}
		catch (Exception)
		{
		}
		return true;
	}

	public bool SetDateTime(DateTime pDateTime)
	{
		return bpc.SetDeviceTime1(_gMachineNumber, pDateTime.Year, pDateTime.Month, pDateTime.Day, pDateTime.Hour, pDateTime.Minute, pDateTime.Second);
	}

	public List<EnrollData> GetAllEnrollData(List<int> pEnrollList)
	{
		List<EnrollDataDet> list = new List<EnrollDataDet>();
		bpc.DotNET();
		int vEnrollNumber = 0;
		int num = 0;
		int num2 = 0;
		int machinPrivilage = 0;
		int num3 = 0;
		int aErrorCode = 0;
		if (!bpc.EnableDevice(_gMachineNumber, 0))
		{
			return EnrollData.ConvertEnrollDatas(list);
		}
		if (!bpc.ReadAllUserID(_gMachineNumber))
		{
			bpc.GetLastError(ref aErrorCode);
			bpc.EnableDevice(_gMachineNumber, 1);
			return EnrollData.ConvertEnrollDatas(list);
		}
		while (bpc.GetAllUserID(_gMachineNumber, ref vEnrollNumber, ref num, ref num2, ref machinPrivilage, ref num3))
		{
			int num4 = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
			if (num2 >= 50)
			{
				continue;
			}
			if (!bpc.GetEnrollData1(_gMachineNumber, vEnrollNumber, num2, ref machinPrivilage, ref num4, ref glngEnrollPData))
			{
				bpc.GetLastError(ref aErrorCode);
				switch (util.MessageBox(new IntPtr(0), util.ErrorPrint(aErrorCode) + ": Continue ?", "GetEnrollData", 4u))
				{
				case 6:
					continue;
				case 7:
					bpc.EnableDevice(_gMachineNumber, 1);
					return EnrollData.ConvertEnrollDatas(list);
				}
			}
			if (pEnrollList.Any((int q) => q.Equals(vEnrollNumber)))
			{
				EnrollDataDet enrollDataDet = new EnrollDataDet
				{
					EnrollNo = vEnrollNumber,
					DeviceNo = (short)num,
					apnBackupNumbar = num2,
					MachinPrivilage = machinPrivilage,
					Enabled = 0
				};
				switch (num2)
				{
				case 10:
				case 11:
				case 15:
					enrollDataDet.Password = glngEnrollPData;
					break;
				case 17:
				{
					enrollDataDet.Password = 0;
					for (int i = 0; i < DATASIZE_FACE; i++)
					{
						gbytEnrollData[i * 5] = 1;
						if (gTemplngEnrollData[i] < 0)
						{
							gbytEnrollData[i * 5] = 0;
							gTemplngEnrollData[i] = Math.Abs(gTemplngEnrollData[i]);
						}
						gbytEnrollData[i * 5 + 1] = (byte)(gTemplngEnrollData[i] / 256 / 256 / 256);
						gbytEnrollData[i * 5 + 2] = (byte)(gTemplngEnrollData[i] / 256 / 256 % 256);
						gbytEnrollData[i * 5 + 3] = (byte)(gTemplngEnrollData[i] / 256 % 256);
						gbytEnrollData[i * 5 + 4] = (byte)(gTemplngEnrollData[i] % 256);
					}
					byte[] array2 = new byte[DATASIZE_FACE * 5];
					for (int i = 0; i < DATASIZE_FACE * 5; i++)
					{
						array2[i] = gbytEnrollData[i];
					}
					enrollDataDet.EnrollData = array2.ToList();
					break;
				}
				default:
				{
					enrollDataDet.Password = 0;
					for (int i = 0; i < DATASIZE; i++)
					{
						gbytEnrollData[i * 5] = 1;
						if (gTemplngEnrollData[i] < 0)
						{
							gbytEnrollData[i * 5] = 0;
							gTemplngEnrollData[i] = Math.Abs(gTemplngEnrollData[i]);
						}
						gbytEnrollData[i * 5 + 1] = (byte)(gTemplngEnrollData[i] / 256 / 256 / 256);
						gbytEnrollData[i * 5 + 2] = (byte)(gTemplngEnrollData[i] / 256 / 256 % 256);
						gbytEnrollData[i * 5 + 3] = (byte)(gTemplngEnrollData[i] / 256 % 256);
						gbytEnrollData[i * 5 + 4] = (byte)(gTemplngEnrollData[i] % 256);
					}
					byte[] array = new byte[DATASIZE * 5];
					for (int i = 0; i < DATASIZE * 5; i++)
					{
						array[i] = gbytEnrollData[i];
					}
					enrollDataDet.EnrollData = array.ToList();
					break;
				}
				}
				list.Add(enrollDataDet);
			}
			bpc.EnableDevice(_gMachineNumber, 1);
		}
		return EnrollData.ConvertEnrollDatas(list);
	}

	public ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>> GetAllEnrollData(int gMachineNumber)
	{
		ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>> val = new ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>>
		{
			value = new List<DeviceEnroll>()
		};
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int aErrorCode = 0;
		string text = "";
		string companyName = "Ebio";
		val.AddInformation("Working...", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 582);
		Application.DoEvents();
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			val.AddError(util.gstrNoDevice, "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 588);
			return val;
		}
		if (bpc.ReadAllUserID(gMachineNumber))
		{
			val.AddInformation("ReadAllUserID OK", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 595);
			((Control)this).Cursor = Cursors.WaitCursor;
			bool flag = false;
			while (bpc.GetAllUserID(gMachineNumber, ref num, ref num2, ref num3, ref num4, ref num5))
			{
				flag = true;
				while (true)
				{
					int num6 = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
					if (num3 >= 50)
					{
						break;
					}
					if (!bpc.GetEnrollData1(gMachineNumber, num, num3, ref num4, ref num6, ref glngEnrollPData))
					{
						flag = false;
						text = "GetEnrollData";
						bpc.GetLastError(ref aErrorCode);
						int num7 = util.MessageBox(new IntPtr(0), util.ErrorPrint(aErrorCode) + ": Continue ?", "GetEnrollData", 4u);
						if (num7 == 6)
						{
							continue;
						}
						if (num7 == 7)
						{
							((Control)this).Cursor = Cursors.Default;
							bpc.EnableDevice(gMachineNumber, 1);
							return val;
						}
					}
					DeviceEnroll deviceEnroll = new DeviceEnroll
					{
						EMachineNumber = num2,
						EnrollNumber = num,
						FingerNumber = num3,
						Privilige = num4
					};
					switch (num3)
					{
					case 10:
					case 11:
					case 15:
						deviceEnroll.Password1 = glngEnrollPData;
						break;
					case 17:
					{
						deviceEnroll.Password1 = 0;
						for (int i = 0; i < DATASIZE_FACE; i++)
						{
							gbytEnrollData[i * 5] = 1;
							if (gTemplngEnrollData[i] < 0)
							{
								gbytEnrollData[i * 5] = 0;
								gTemplngEnrollData[i] = Math.Abs(gTemplngEnrollData[i]);
							}
							gbytEnrollData[i * 5 + 1] = (byte)(gTemplngEnrollData[i] / 256 / 256 / 256);
							gbytEnrollData[i * 5 + 2] = (byte)(gTemplngEnrollData[i] / 256 / 256 % 256);
							gbytEnrollData[i * 5 + 3] = (byte)(gTemplngEnrollData[i] / 256 % 256);
							gbytEnrollData[i * 5 + 4] = (byte)(gTemplngEnrollData[i] % 256);
						}
						byte[] array2 = new byte[DATASIZE_FACE * 5];
						for (int i = 0; i < DATASIZE_FACE * 5; i++)
						{
							array2[i] = gbytEnrollData[i];
						}
						deviceEnroll.FPData = array2;
						break;
					}
					default:
					{
						deviceEnroll.Password1 = 0;
						for (int i = 0; i < DATASIZE; i++)
						{
							gbytEnrollData[i * 5] = 1;
							if (gTemplngEnrollData[i] < 0)
							{
								gbytEnrollData[i * 5] = 0;
								gTemplngEnrollData[i] = Math.Abs(gTemplngEnrollData[i]);
							}
							gbytEnrollData[i * 5 + 1] = (byte)(gTemplngEnrollData[i] / 256 / 256 / 256);
							gbytEnrollData[i * 5 + 2] = (byte)(gTemplngEnrollData[i] / 256 / 256 % 256);
							gbytEnrollData[i * 5 + 3] = (byte)(gTemplngEnrollData[i] / 256 % 256);
							gbytEnrollData[i * 5 + 4] = (byte)(gTemplngEnrollData[i] % 256);
						}
						byte[] array = new byte[DATASIZE * 5];
						for (int i = 0; i < DATASIZE * 5; i++)
						{
							array[i] = gbytEnrollData[i];
						}
						deviceEnroll.FPData = array;
						break;
					}
					}
					deviceEnroll.CompanyName = companyName;
					val.value.Add(deviceEnroll);
					val.AddInformation($"{num2:D2}" + "-" + $"{num:D2}" + "-" + num3, "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 701);
					((Control)this).Text = $"{num:D2}";
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(num), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 703);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(num3), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 704);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(num4), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 705);
					Application.DoEvents();
					break;
				}
			}
			((Control)this).Cursor = Cursors.Default;
			if (flag)
			{
				val.AddInformation("GetAllUserID OK", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 712);
			}
			else
			{
				val.AddInformation(text + ":" + util.ErrorPrint(aErrorCode), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 714);
			}
			Application.DoEvents();
			bpc.EnableDevice(gMachineNumber, 1);
			return val;
		}
		bpc.GetLastError(ref aErrorCode);
		val.AddError(util.ErrorPrint(aErrorCode), "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\FrmEbio.cs", 600);
		bpc.EnableDevice(gMachineNumber, 1);
		return val;
	}

	public bool PutAllEnrollData(List<EnrollData> pEnrollList)
	{
		bool flag = bpc.EnableDevice(_gMachineNumber, 0);
		bpc.DotNET();
		if (!flag)
		{
			return false;
		}
		int num = 0;
		foreach (EnrollDataDet item in EnrollData.ConvertEnrollDataToDets(pEnrollList))
		{
			num++;
			if (item.apnBackupNumbar == 17)
			{
				byte[] array = item.EnrollData.ToArray();
				for (int i = 0; i < DATASIZE_FACE; i++)
				{
					gTemplngEnrollData[i] = array[i * 5 + 1];
					gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 2];
					gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 3];
					gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 4];
					if (array[i * 5] == 0)
					{
						gTemplngEnrollData[i] = -gTemplngEnrollData[i];
					}
				}
			}
			else if (item.apnBackupNumbar < 10)
			{
				byte[] array = item.EnrollData.ToArray();
				for (int i = 0; i < DATASIZE; i++)
				{
					gTemplngEnrollData[i] = array[i * 5 + 1];
					gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 2];
					gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 3];
					gTemplngEnrollData[i] = gTemplngEnrollData[i] * 256 + array[i * 5 + 4];
					if (array[i * 5] == 0)
					{
						gTemplngEnrollData[i] = -gTemplngEnrollData[i];
					}
				}
			}
			GCHandle gCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
			int num2 = gCHandle.AddrOfPinnedObject().ToInt32();
			int num3 = item.apnBackupNumbar;
			if (num3 >= 0 && num3 <= 9)
			{
				num3 += 20;
			}
			flag = bpc.SetEnrollData1(_gMachineNumber, item.EnrollNo, num3, item.MachinPrivilage, ref num2, item.Password);
			gCHandle.Free();
			if (!flag)
			{
				break;
			}
		}
		bpc.EnableDevice(_gMachineNumber, 1);
		return flag;
	}

	public bool ClearEnrollData()
	{
		int num = 0;
		bpc.DotNET();
		if (!bpc.EnableDevice(_gMachineNumber, 0))
		{
			return false;
		}
		bool flag = bpc.EmptyEnrollData(_gMachineNumber);
		if (!flag)
		{
			bpc.GetLastError(ref num);
		}
		else
		{
			flag = bpc.EmptySuperLogData(_gMachineNumber);
		}
		bpc.EnableDevice(_gMachineNumber, 1);
		return flag;
	}

	private List<EnrollData> GetEnrollData(int enrollNo)
	{
		List<EnrollDataDet> list = new List<EnrollDataDet>();
		bpc.DotNET();
		int machinPrivilage = 0;
		if (!bpc.EnableDevice(_gMachineNumber, 0))
		{
			return EnrollData.ConvertEnrollDatas(list);
		}
		for (int i = 0; i < 18; i++)
		{
			if (i == 12)
			{
				i = 15;
			}
			if (i == 16)
			{
				i = 17;
			}
			GCHandle gCHandle = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned);
			IntPtr intPtr = gCHandle.AddrOfPinnedObject();
			int password = 0;
			int num = intPtr.ToInt32();
			bool enrollData = bpc.GetEnrollData1(_gMachineNumber, enrollNo, i, ref machinPrivilage, ref num, ref password);
			gCHandle.Free();
			if (!enrollData)
			{
				continue;
			}
			EnrollDataDet enrollDataDet = new EnrollDataDet
			{
				EnrollNo = enrollNo,
				DeviceNo = (short)_gMachineNumber,
				apnBackupNumbar = i,
				MachinPrivilage = machinPrivilage,
				Enabled = 0
			};
			switch (i)
			{
			case 10:
			case 11:
			case 15:
				enrollDataDet.Password = password;
				break;
			case 17:
			{
				enrollDataDet.Password = 0;
				for (int j = 0; j < DATASIZE_FACE; j++)
				{
					gbytEnrollData[j * 5] = 1;
					if (gTemplngEnrollData[j] < 0)
					{
						gbytEnrollData[j * 5] = 0;
						gTemplngEnrollData[j] = Math.Abs(gTemplngEnrollData[j]);
					}
					gbytEnrollData[j * 5 + 1] = (byte)(gTemplngEnrollData[j] / 256 / 256 / 256);
					gbytEnrollData[j * 5 + 2] = (byte)(gTemplngEnrollData[j] / 256 / 256 % 256);
					gbytEnrollData[j * 5 + 3] = (byte)(gTemplngEnrollData[j] / 256 % 256);
					gbytEnrollData[j * 5 + 4] = (byte)(gTemplngEnrollData[j] % 256);
				}
				byte[] array2 = new byte[DATASIZE_FACE * 5];
				for (int j = 0; j < DATASIZE_FACE * 5; j++)
				{
					array2[j] = gbytEnrollData[j];
				}
				enrollDataDet.EnrollData = array2.ToList();
				break;
			}
			default:
			{
				enrollDataDet.Password = 0;
				for (int j = 0; j < DATASIZE; j++)
				{
					gbytEnrollData[j * 5] = 1;
					if (gTemplngEnrollData[j] < 0)
					{
						gbytEnrollData[j * 5] = 0;
						gTemplngEnrollData[j] = Math.Abs(gTemplngEnrollData[j]);
					}
					gbytEnrollData[j * 5 + 1] = (byte)(gTemplngEnrollData[j] / 256 / 256 / 256);
					gbytEnrollData[j * 5 + 2] = (byte)(gTemplngEnrollData[j] / 256 / 256 % 256);
					gbytEnrollData[j * 5 + 3] = (byte)(gTemplngEnrollData[j] / 256 % 256);
					gbytEnrollData[j * 5 + 4] = (byte)(gTemplngEnrollData[j] % 256);
				}
				byte[] array = new byte[DATASIZE * 5];
				for (int j = 0; j < DATASIZE * 5; j++)
				{
					array[j] = gbytEnrollData[j];
				}
				enrollDataDet.EnrollData = array.ToList();
				break;
			}
			}
			list.Add(enrollDataDet);
			bpc.EnableDevice(_gMachineNumber, 1);
		}
		return EnrollData.ConvertEnrollDatas(list);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
	}
}
