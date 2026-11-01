using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AxSBXPCLib;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SBXPCSampleCSharp;
using TimeAttendanceDAL.Dal.Enroll;

namespace TimeAttendanceDevices.FLREbio;

public class EbioClass : IHasContext
{
	private AxSBXPC axSBXPC1;

	private int _gMachineNumber = 1;

	private int _Password;

	private string _IP = "";

	private int _Port;

	private bool _connect;

	private List<DeviceLogOnPerson> GLogData;

	private int DATASIZE = 354;

	private int DATASIZE_FACE = 6917;

	private int NAMESIZE = 54;

	private int[] gTemplngEnrollData = new int[6917];

	private byte[] gbytEnrollData = new byte[34585];

	private byte[] gbytEnrollData1 = new byte[34585];

	private int[] gTempEnrollName = new int[54];

	private int glngEnrollPData;

	private ASCIIEncoding ascii = new ASCIIEncoding();

	private AxSBXPC bpc;

	public DataSet dsEnrolls;

	public string ContextGuid { get; set; }

	public bool Conected => _connect;

	public EbioClass(string contextGuid, int DeviceNum, int Password, string IP, int Port)
	{
		ContextGuid = contextGuid;
		_gMachineNumber = DeviceNum;
		_Password = Password;
		_IP = IP;
		_Port = Port;
		CreateAxSbxpcControl();
	}

	public EbioClass(string contextGuid, int DeviceNum)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Expected O, but got Unknown
		ContextGuid = contextGuid;
		_gMachineNumber = DeviceNum;
		try
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FrmEbio));
			axSBXPC1 = new AxSBXPC();
			((ISupportInitialize)axSBXPC1).BeginInit();
			((AxHost)axSBXPC1).Enabled = true;
			((Control)axSBXPC1).Location = new Point(12, 211);
			((Control)axSBXPC1).Name = "axSBXPC1";
			((AxHost)axSBXPC1).OcxState = (State)componentResourceManager.GetObject("axSBXPC1.OcxState");
			((Control)axSBXPC1).Size = new Size(100, 50);
			((Control)axSBXPC1).TabIndex = 0;
			((ISupportInitialize)axSBXPC1).EndInit();
			_ = ((AxHost)axSBXPC1).OcxState;
			_connect = axSBXPC1.ConnectSerial(_gMachineNumber, 0, 0);
			GetLog(DeleteAfter: false);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 105);
		}
	}

	public List<RecordEbio> GetLog(bool DeleteAfter)
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
			if (!axSBXPC1.EnableDevice(_gMachineNumber, -1))
			{
				return null;
			}
			bool flag = axSBXPC1.ReadAllGLogData(_gMachineNumber);
			if (DeleteAfter)
			{
				axSBXPC1.EmptyGeneralLogData(_gMachineNumber);
			}
			int num2 = 1;
			do
			{
				flag = axSBXPC1.GetAllGLogData(_gMachineNumber, ref vTMachineNumber, ref vSEnrollNumber, ref vSMachineNumber, ref vVerifyMode, ref vYear, ref vMonth, ref vDay, ref vHour, ref vMinute, ref vSecond);
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
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetLog", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 145);
		}
		return list;
	}

	public List<DeviceLogOnPerson> GetLogOnPublic(bool DeleteAfter)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
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
			if (!axSBXPC1.EnableDevice(_gMachineNumber, -1))
			{
				return null;
			}
			bool flag = axSBXPC1.ReadAllGLogData(_gMachineNumber);
			if (DeleteAfter)
			{
				axSBXPC1.EmptyGeneralLogData(_gMachineNumber);
			}
			int num4 = 1;
			while (true)
			{
				try
				{
					flag = axSBXPC1.GetAllGLogData(_gMachineNumber, ref num2, ref personNo, ref deviceNo, ref num3, ref year, ref month, ref day, ref hour, ref minute, ref second);
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
							IoMode = (short)num3,
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
					WebErrorLog.ErrorLogInstance.StartLog(ex, "GetLogOnPublic_while:i:" + num4, "GetLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 196);
				}
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "GetLogOnPublic", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 202);
		}
		return list;
	}

	public bool Disconnect()
	{
		try
		{
			axSBXPC1.Disconnect();
			bpc.Disconnect();
			_connect = false;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 223);
		}
		return _connect;
	}

	public bool TestConnection()
	{
		try
		{
			if (!Conected)
			{
				Disconnect();
				return false;
			}
			GetLog(DeleteAfter: false);
			Disconnect();
			return true;
		}
		catch (Exception)
		{
			try
			{
				Disconnect();
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "TestConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 252);
				return false;
			}
			try
			{
				Disconnect();
			}
			catch (Exception ex2)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "TestConnection", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 261);
			}
		}
		return true;
	}

	public bool SetDateTime(DateTime pDateTime)
	{
		return axSBXPC1.SetDeviceTime1(_gMachineNumber, pDateTime.Year, pDateTime.Month, pDateTime.Day, pDateTime.Hour, pDateTime.Minute, pDateTime.Second);
	}

	public void STAOparation()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		try
		{
			axSBXPC1 = new AxSBXPC();
			((AxHost)axSBXPC1).Enabled = true;
			((Control)axSBXPC1).CreateControl();
			_connect = axSBXPC1.ConnectTcpip(_gMachineNumber, ref _IP, _Port, _Password);
			GetLog(DeleteAfter: false);
			SetDateTime(DateTime.Now);
			GLogData = GetLogOnPublic(DeleteAfter: false);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "STAOparation", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 288);
		}
	}

	public List<DeviceLogOnPerson> GetGLogData()
	{
		try
		{
			Thread thread = new Thread(STAOparation);
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			while (thread.IsAlive)
			{
				Application.DoEvents();
			}
			return GLogData;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetGLogData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 305);
		}
		return null;
	}

	public ReturnedSaveFuncInfoWithValue<DataTable> GetAllEnrollData()
	{
		ReturnedSaveFuncInfoWithValue<DataTable> val = new ReturnedSaveFuncInfoWithValue<DataTable>();
		val.value = new DataTable();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int aErrorCode = 0;
		string value = "Ebio";
		Application.DoEvents();
		if (!axSBXPC1.EnableDevice(_gMachineNumber, 0))
		{
			val.AddError($"دستگاه با شماره {_gMachineNumber} شناسایی نشد", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 344);
			return val;
		}
		if (axSBXPC1.ReadAllUserID(_gMachineNumber))
		{
			val.AddInformation("لیست کاربران از دستگاه دریافت شد", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 351);
			DataTable dataTable = new DataTable();
			dataTable.Rows.Clear();
			while (axSBXPC1.GetAllUserID(_gMachineNumber, ref num, ref num2, ref num3, ref num4, ref num5))
			{
				while (true)
				{
					int num6 = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
					if (num3 >= 50)
					{
						break;
					}
					if (!axSBXPC1.GetEnrollData1(_gMachineNumber, num, num3, ref num4, ref num6, ref glngEnrollPData))
					{
						axSBXPC1.GetLastError(ref aErrorCode);
						int num7 = util.MessageBox(new IntPtr(0), util.ErrorPrint(aErrorCode) + ": Continue ?", "GetEnrollData", 4u);
						if (num7 == 6)
						{
							continue;
						}
						if (num7 == 7)
						{
							axSBXPC1.EnableDevice(_gMachineNumber, 1);
							return val;
						}
					}
					DataRow dataRow = dataTable.NewRow();
					dataRow["EMachineNumber"] = num2;
					dataRow["EnrollNumber"] = num;
					dataRow["FingerNumber"] = num3;
					dataRow["Privilige"] = num4;
					switch (num3)
					{
					case 10:
						dataRow["Password1"] = glngEnrollPData;
						break;
					case 15:
						dataRow["Password1"] = glngEnrollPData;
						break;
					case 11:
						dataRow["Password1"] = glngEnrollPData;
						break;
					case 17:
					{
						dataRow["Password1"] = 0;
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
						dataRow["FPdata"] = array2;
						break;
					}
					default:
					{
						dataRow["Password1"] = 0;
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
						dataRow["FPdata"] = array;
						break;
					}
					}
					dataRow["CompanyName"] = value;
					dataTable.Rows.Add(dataRow);
					Application.DoEvents();
					break;
				}
			}
			val.value = dataTable;
			dsEnrolls.GetChanges();
			Application.DoEvents();
			axSBXPC1.EnableDevice(_gMachineNumber, 1);
			return val;
		}
		axSBXPC1.GetLastError(ref aErrorCode);
		val.AddError(util.ErrorPrint(aErrorCode), "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 356);
		axSBXPC1.EnableDevice(_gMachineNumber, 1);
		return val;
	}

	private void CreateAxSbxpcControl()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		try
		{
			object @object = new ResourceManager("TimeAttendanceDevices.Resource", Assembly.GetExecutingAssembly()).GetObject("bpc.OcxState");
			bpc = new AxSBXPC();
			((ISupportInitialize)bpc).BeginInit();
			((AxHost)bpc).OcxState = (State)@object;
			((AxHost)bpc).Enabled = true;
			((Control)bpc).Location = new Point(10, 9);
			((Control)bpc).Name = "bpc";
			((Control)bpc).Size = new Size(100, 50);
			((Control)bpc).TabIndex = 37;
			((ISupportInitialize)bpc).EndInit();
			bpc.DotNET();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateAxSbxpcControl", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 551);
		}
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
		string companyName = "";
		val.AddInformation("Working...", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 574);
		Application.DoEvents();
		if (!bpc.EnableDevice(gMachineNumber, 0))
		{
			val.AddError(util.gstrNoDevice, "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 580);
			return val;
		}
		if (bpc.ReadAllUserID(gMachineNumber))
		{
			val.AddInformation("ReadAllUserID OK", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 587);
			_ = Cursors.WaitCursor;
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
							_ = Cursors.Default;
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
					val.AddInformation($"{num2:D2}" + "-" + $"{num:D2}" + "-" + num3, "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 693);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(num), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 695);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(num3), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 696);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(num4), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 697);
					Application.DoEvents();
					break;
				}
			}
			_ = Cursors.Default;
			if (flag)
			{
				val.AddInformation("GetAllUserID OK", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 704);
			}
			else
			{
				val.AddInformation(text + ":" + util.ErrorPrint(aErrorCode), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 706);
			}
			Application.DoEvents();
			bpc.EnableDevice(gMachineNumber, 1);
			return val;
		}
		bpc.GetLastError(ref aErrorCode);
		val.AddError(util.ErrorPrint(aErrorCode), "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 592);
		bpc.EnableDevice(gMachineNumber, 1);
		return val;
	}

	public bool ConnectByTcpIp()
	{
		try
		{
			_connect = bpc.ConnectTcpip(_gMachineNumber, ref _IP, _Port, _Password);
			return _connect;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ConnectByTcpIp", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\FLREbio\\EbioClass.cs", 722);
		}
		return false;
	}
}
