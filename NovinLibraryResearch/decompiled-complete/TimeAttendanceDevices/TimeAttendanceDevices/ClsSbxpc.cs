using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using AxSBXPCLib;
using PacketParser.BusinessLayer;
using SBXPCSampleCSharp;
using Serilog;
using TimeAttendanceDAL.Dal.Enroll;

namespace TimeAttendanceDevices;

public class ClsSbxpc : IDisposable
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

	public ClsSbxpc(AxSBXPC sBXPC)
	{
		if (sBXPC != null)
		{
			bpc = sBXPC;
			gbytEnrollData = new byte[DATASIZE_FACE * 5];
			gbytEnrollData1 = new byte[DATASIZE_FACE * 5];
			gTemplngEnrollData = new int[DATASIZE_FACE];
			gTempEnrollName = new int[NAMESIZE];
			ascii = new ASCIIEncoding();
		}
	}

	public bool ConnectTcpIp(int machineNumber, ref string ipAddress, int port, int password)
	{
		try
		{
			return bpc.ConnectTcpip(machineNumber, ref ipAddress, port, password);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public bool EnableDevice(int machineNumber, int boolFlag)
	{
		try
		{
			return bpc.EnableDevice(machineNumber, boolFlag);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public bool ReadAllUserID(int machineNumber)
	{
		try
		{
			return bpc.ReadAllUserID(machineNumber);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public bool GetLastError(ref int errorCode)
	{
		try
		{
			return bpc.GetLastError(ref errorCode);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public bool GetAllUserID(int gMachineNumber, ref int vEnrollNumber, ref int vEMachineNumber, ref int vFingerNumber, ref int vPrivilege, ref int vEnable)
	{
		try
		{
			return bpc.GetAllUserID(gMachineNumber, ref vEnrollNumber, ref vEMachineNumber, ref vFingerNumber, ref vPrivilege, ref vEnable);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public bool GetEnrollData1(int gMachineNumber, int vEnrollNumber, int vFingerNumber, ref int vPrivilege, ref int nAddr, ref int glngEnrollPData)
	{
		try
		{
			return bpc.GetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, ref vPrivilege, ref nAddr, ref glngEnrollPData);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
		}
		return false;
	}

	public void Dispose()
	{
	}

	public ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>> GetAllEnrollData(int gMachineNumber)
	{
		ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>> val = new ReturnedSaveFuncInfoWithValue<List<DeviceEnroll>>
		{
			value = new List<DeviceEnroll>()
		};
		int vEnrollNumber = 0;
		int vEMachineNumber = 0;
		int vFingerNumber = 0;
		int vPrivilege = 0;
		int vEnable = 0;
		int errorCode = 0;
		string text = "";
		string companyName = "";
		val.AddInformation("Working...", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 135);
		if (!EnableDevice(gMachineNumber, 0))
		{
			val.AddError(util.gstrNoDevice, "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 140);
			return val;
		}
		Application.DoEvents();
		if (ReadAllUserID(gMachineNumber))
		{
			val.AddInformation("ReadAllUserID OK", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 146);
			_ = Cursors.WaitCursor;
			bool flag = false;
			Application.DoEvents();
			while (GetAllUserID(gMachineNumber, ref vEnrollNumber, ref vEMachineNumber, ref vFingerNumber, ref vPrivilege, ref vEnable))
			{
				flag = true;
				while (true)
				{
					int nAddr = GCHandle.Alloc(gTemplngEnrollData, GCHandleType.Pinned).AddrOfPinnedObject().ToInt32();
					if (vFingerNumber >= 50)
					{
						break;
					}
					if (!GetEnrollData1(gMachineNumber, vEnrollNumber, vFingerNumber, ref vPrivilege, ref nAddr, ref glngEnrollPData))
					{
						flag = false;
						text = "GetEnrollData";
						GetLastError(ref errorCode);
						int num = util.MessageBox(new IntPtr(0), util.ErrorPrint(errorCode) + ": Continue ?", "GetEnrollData", 4u);
						if (num == 6)
						{
							continue;
						}
						if (num == 7)
						{
							_ = Cursors.Default;
							EnableDevice(gMachineNumber, 1);
							return val;
						}
					}
					DeviceEnroll deviceEnroll = new DeviceEnroll
					{
						EMachineNumber = vEMachineNumber,
						EnrollNumber = vEnrollNumber,
						FingerNumber = vFingerNumber,
						Privilige = vPrivilege
					};
					switch (vFingerNumber)
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
					val.AddInformation($"{vEMachineNumber:D2}" + "-" + $"{vEnrollNumber:D2}" + "-" + vFingerNumber, "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 251);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(vEnrollNumber), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 253);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(vFingerNumber), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 254);
					((ReturnedSaveFuncInfo)val).AddWarning(Convert.ToString(vPrivilege), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 255);
					Application.DoEvents();
					break;
				}
			}
			_ = Cursors.Default;
			if (!flag)
			{
				val.AddInformation(text + ":" + util.ErrorPrint(errorCode), "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 261);
			}
			else
			{
				val.AddInformation("GetAllUserID OK", "", "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 261);
			}
			Application.DoEvents();
			EnableDevice(gMachineNumber, 1);
			return val;
		}
		GetLastError(ref errorCode);
		val.AddError(util.ErrorPrint(errorCode), "GetAllEnrollData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDevices\\ClsSbxpc.cs", 150);
		EnableDevice(gMachineNumber, 1);
		return val;
	}
}
