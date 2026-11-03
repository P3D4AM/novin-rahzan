using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SBXPCLib;

namespace AxSBXPCLib;

[DefaultEvent("OnReceiveEventXML")]
[DesignTimeVisible(true)]
[Clsid("{2894e36d-6941-48e0-abf9-0d38241884fb}")]
public class AxSBXPC : AxHost
{
	private _DSBXPC ocx;

	private AxSBXPCEventMulticaster eventMulticaster;

	private ConnectionPointCookie cookie;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(1)]
	public virtual int CommPort
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CommPort", (ActiveXInvokeKind)1);
			}
			return ocx.CommPort;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("CommPort", (ActiveXInvokeKind)2);
			}
			ocx.CommPort = value;
		}
	}

	[DispId(2)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public virtual int Baudrate
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Baudrate", (ActiveXInvokeKind)1);
			}
			return ocx.Baudrate;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("Baudrate", (ActiveXInvokeKind)2);
			}
			ocx.Baudrate = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DispId(3)]
	public virtual bool ReadMark
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ReadMark", (ActiveXInvokeKind)1);
			}
			return ocx.ReadMark;
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (ocx == null)
			{
				throw new InvalidActiveXStateException("ReadMark", (ActiveXInvokeKind)2);
			}
			ocx.ReadMark = value;
		}
	}

	public event _DSBXPCEvents_OnReceiveEventXMLEventHandler OnReceiveEventXML;

	public AxSBXPC()
		: base("2894e36d-6941-48e0-abf9-0d38241884fb")
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		((AxHost)this).SetAboutBoxDelegate(new AboutBoxDelegate(AboutBox));
	}

	public virtual bool GetDepartName(int dwMachineNumber, int dwDepartNumber, int dwDepartOrDaigong, ref string lpszUserName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDepartName", (ActiveXInvokeKind)0);
		}
		return ocx.GetDepartName(dwMachineNumber, dwDepartNumber, dwDepartOrDaigong, ref lpszUserName);
	}

	public virtual bool SetDepartName(int dwMachineNumber, int dwDepartNumber, int dwDepartOrDaigong, ref string lpszUserName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDepartName", (ActiveXInvokeKind)0);
		}
		return ocx.SetDepartName(dwMachineNumber, dwDepartNumber, dwDepartOrDaigong, ref lpszUserName);
	}

	public virtual bool StartEventCapture(int dwCommType, int dwParam1, int dwParam2)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StartEventCapture", (ActiveXInvokeKind)0);
		}
		return ocx.StartEventCapture(dwCommType, dwParam1, dwParam2);
	}

	public virtual void StopEventCapture()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("StopEventCapture", (ActiveXInvokeKind)0);
		}
		ocx.StopEventCapture();
	}

	public virtual int XML_ParseInt(ref string lpszXML, string lpszTagname)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseInt", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseInt(ref lpszXML, lpszTagname);
	}

	public virtual int XML_ParseLong(ref string lpszXML, string lpszTagname)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseLong", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseLong(ref lpszXML, lpszTagname);
	}

	public virtual bool XML_ParseBoolean(ref string lpszXML, string lpszTagname)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseBoolean", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseBoolean(ref lpszXML, lpszTagname);
	}

	public virtual bool XML_ParseString(ref string lpszXML, string lpszTagname, ref string lpszValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseString", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseString(ref lpszXML, lpszTagname, ref lpszValue);
	}

	public virtual bool XML_ParseBinaryByte(ref string lpszXML, string lpszTagname, ref byte pData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseBinaryByte", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseBinaryByte(ref lpszXML, lpszTagname, ref pData, dwLen);
	}

	public virtual bool XML_ParseBinaryWord(ref string lpszXML, string lpszTagname, ref ushort pData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseBinaryWord", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseBinaryWord(ref lpszXML, lpszTagname, ref pData, dwLen);
	}

	public virtual bool XML_ParseBinaryLong(ref string lpszXML, string lpszTagname, ref int pData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseBinaryLong", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseBinaryLong(ref lpszXML, lpszTagname, ref pData, dwLen);
	}

	public virtual bool XML_ParseBinaryUnicode(ref string lpszXML, string lpszTagname, ref string pData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseBinaryUnicode", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseBinaryUnicode(ref lpszXML, lpszTagname, ref pData, dwLen);
	}

	public virtual bool XML_AddInt(ref string lpszXML, string lpszTagname, int nValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddInt", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddInt(ref lpszXML, lpszTagname, nValue);
	}

	public virtual bool XML_AddLong(ref string lpszXML, string lpszTagname, int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddLong", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddLong(ref lpszXML, lpszTagname, dwValue);
	}

	public virtual bool XML_AddBoolean(ref string lpszXML, string lpszTagname, bool bValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBoolean", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBoolean(ref lpszXML, lpszTagname, bValue);
	}

	public virtual bool XML_AddString(ref string lpszXML, string lpszTagname, string lpszValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddString", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddString(ref lpszXML, lpszTagname, lpszValue);
	}

	public virtual bool XML_AddBinaryByte(ref string lpszXML, string lpszTagname, ref byte dwData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBinaryByte", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBinaryByte(ref lpszXML, lpszTagname, ref dwData, dwLen);
	}

	public virtual bool XML_AddBinaryWord(ref string lpszXML, string lpszTagname, ref ushort dwData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBinaryWord", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBinaryWord(ref lpszXML, lpszTagname, ref dwData, dwLen);
	}

	public virtual bool XML_AddBinaryLong(ref string lpszXML, string lpszTagname, ref int dwData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBinaryLong", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBinaryLong(ref lpszXML, lpszTagname, ref dwData, dwLen);
	}

	public virtual bool XML_AddBinaryUnicode(ref string lpszXML, string lpszTagname, ref string lpszData)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBinaryUnicode", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBinaryUnicode(ref lpszXML, lpszTagname, ref lpszData);
	}

	public virtual bool XML_AddBinaryGlyph(ref string lpszXML, string lpszMessage, int width, int height, string lpszFontDescriptor)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBinaryGlyph", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBinaryGlyph(ref lpszXML, lpszMessage, width, height, lpszFontDescriptor);
	}

	public virtual int ConnectP2p(ref string lpszMachineID, ref string lpszServerIPAddress, int dwServerPortNumber, int dwPassWord, ref int pnError)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ConnectP2p", (ActiveXInvokeKind)0);
		}
		return ocx.ConnectP2p(ref lpszMachineID, ref lpszServerIPAddress, dwServerPortNumber, dwPassWord, ref pnError);
	}

	public virtual bool PrepareP2p(ref string lpszMachineID, ref string lpszServerIPAddress, int dwServerPortNumber, ref int dwYearStart, ref int dwMonthStart, ref int dwDayStart, ref int dwYearEnd, ref int dwMonthEnd, ref int dwDayEnd, ref int pnError)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PrepareP2p", (ActiveXInvokeKind)0);
		}
		return ocx.PrepareP2p(ref lpszMachineID, ref lpszServerIPAddress, dwServerPortNumber, ref dwYearStart, ref dwMonthStart, ref dwDayStart, ref dwYearEnd, ref dwMonthEnd, ref dwDayEnd, ref pnError);
	}

	public virtual bool XML_AddBinaryNameGlyph(int dwMachineNumber, ref string lpszXML, ref string lpszName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_AddBinaryNameGlyph", (ActiveXInvokeKind)0);
		}
		return ocx.XML_AddBinaryNameGlyph(dwMachineNumber, ref lpszXML, ref lpszName);
	}

	public virtual bool XML_ParseMultiUnicode(ref string lpszXML, string lpszTagname, ref string pData, int dwLen)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("XML_ParseMultiUnicode", (ActiveXInvokeKind)0);
		}
		return ocx.XML_ParseMultiUnicode(ref lpszXML, lpszTagname, ref pData, dwLen);
	}

	public virtual bool SetDeviceTime1(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceTime1", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceTime1(dwMachineNumber, dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
	}

	public virtual void Disconnect1(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Disconnect1", (ActiveXInvokeKind)0);
		}
		ocx.Disconnect1(dwMachineNumber);
	}

	public virtual void DisconnectAll()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DisconnectAll", (ActiveXInvokeKind)0);
		}
		ocx.DisconnectAll();
	}

	public virtual bool GeneralOperationXML1(int dwMachineNumber, ref string lpszReqNResXML)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GeneralOperationXML1", (ActiveXInvokeKind)0);
		}
		return ocx.GeneralOperationXML1(dwMachineNumber, ref lpszReqNResXML);
	}

	public virtual void UseInternalRedraw(int redraw)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("UseInternalRedraw", (ActiveXInvokeKind)0);
		}
		ocx.UseInternalRedraw(redraw);
	}

	public virtual int GetInternalFwVer(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetInternalFwVer", (ActiveXInvokeKind)0);
		}
		return ocx.GetInternalFwVer(dwMachineNumber);
	}

	public virtual void AboutBox()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("AboutBox", (ActiveXInvokeKind)0);
		}
		ocx.AboutBox();
	}

	public virtual bool GetAllUserID(int dwMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnable)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllUserID", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllUserID(dwMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwBackupNumber, ref dwMachinePrivilege, ref dwEnable);
	}

	public virtual bool GetSerialNumber(int dwMachineNumber, ref string lpszSerialNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSerialNumber", (ActiveXInvokeKind)0);
		}
		return ocx.GetSerialNumber(dwMachineNumber, ref lpszSerialNumber);
	}

	public virtual int GetBackupNumber(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetBackupNumber", (ActiveXInvokeKind)0);
		}
		return ocx.GetBackupNumber(dwMachineNumber);
	}

	public virtual bool GetProductCode(int dwMachineNumber, ref string lpszProductCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetProductCode", (ActiveXInvokeKind)0);
		}
		return ocx.GetProductCode(dwMachineNumber, ref lpszProductCode);
	}

	public virtual bool ClearKeeperData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ClearKeeperData", (ActiveXInvokeKind)0);
		}
		return ocx.ClearKeeperData(dwMachineNumber);
	}

	public virtual bool EmptyEnrollData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EmptyEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.EmptyEnrollData(dwMachineNumber);
	}

	public virtual bool EmptyGeneralLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EmptyGeneralLogData", (ActiveXInvokeKind)0);
		}
		return ocx.EmptyGeneralLogData(dwMachineNumber);
	}

	public virtual bool EmptySuperLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EmptySuperLogData", (ActiveXInvokeKind)0);
		}
		return ocx.EmptySuperLogData(dwMachineNumber);
	}

	public virtual bool GetUserName(int deviceKind, int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, ref object lpszUserName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserName", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserName(deviceKind, dwMachineNumber, dwEnrollNumber, dwEMachineNumber, ref lpszUserName);
	}

	public virtual bool GetUserName1(int dwMachineNumber, int dwEnrollNumber, ref string lpszUserName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetUserName1", (ActiveXInvokeKind)0);
		}
		return ocx.GetUserName1(dwMachineNumber, dwEnrollNumber, ref lpszUserName);
	}

	public virtual bool SetUserName(int deviceKind, int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, ref object lpszUserName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserName", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserName(deviceKind, dwMachineNumber, dwEnrollNumber, dwEMachineNumber, ref lpszUserName);
	}

	public virtual bool SetUserName1(int dwMachineNumber, int dwEnrollNumber, ref string lpszUserName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetUserName1", (ActiveXInvokeKind)0);
		}
		return ocx.SetUserName1(dwMachineNumber, dwEnrollNumber, ref lpszUserName);
	}

	public virtual bool GetCompanyName(int dwMachineNumber, ref object dwCompanyName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetCompanyName", (ActiveXInvokeKind)0);
		}
		return ocx.GetCompanyName(dwMachineNumber, ref dwCompanyName);
	}

	public virtual bool GetCompanyName1(int dwMachineNumber, ref string dwCompanyName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetCompanyName1", (ActiveXInvokeKind)0);
		}
		return ocx.GetCompanyName1(dwMachineNumber, ref dwCompanyName);
	}

	public virtual bool SetCompanyName(int dwMachineNumber, int vKind, ref object dwCompanyName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetCompanyName", (ActiveXInvokeKind)0);
		}
		return ocx.SetCompanyName(dwMachineNumber, vKind, ref dwCompanyName);
	}

	public virtual bool SetCompanyName1(int dwMachineNumber, int vKind, ref string dwCompanyName)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetCompanyName1", (ActiveXInvokeKind)0);
		}
		return ocx.SetCompanyName1(dwMachineNumber, vKind, ref dwCompanyName);
	}

	public virtual bool GetDoorStatus(int dwMachineNumber, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDoorStatus", (ActiveXInvokeKind)0);
		}
		return ocx.GetDoorStatus(dwMachineNumber, ref dwValue);
	}

	public virtual bool SetDoorStatus(int dwMachineNumber, int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDoorStatus", (ActiveXInvokeKind)0);
		}
		return ocx.SetDoorStatus(dwMachineNumber, dwValue);
	}

	public virtual bool GetBellTime(int dwMachineNumber, ref int dwValue, ref int dwBellInfo)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetBellTime", (ActiveXInvokeKind)0);
		}
		return ocx.GetBellTime(dwMachineNumber, ref dwValue, ref dwBellInfo);
	}

	public virtual bool SetBellTime(int dwMachineNumber, int dwValue, ref int dwBellInfo)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetBellTime", (ActiveXInvokeKind)0);
		}
		return ocx.SetBellTime(dwMachineNumber, dwValue, ref dwBellInfo);
	}

	public virtual bool ConnectSerial(int dwMachineNumber, int dwCommPort, int dwBaudRate)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ConnectSerial", (ActiveXInvokeKind)0);
		}
		return ocx.ConnectSerial(dwMachineNumber, dwCommPort, dwBaudRate);
	}

	public virtual bool ConnectTcpip(int dwMachineNumber, ref string lpszIPAddress, int dwPortNumber, int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ConnectTcpip", (ActiveXInvokeKind)0);
		}
		return ocx.ConnectTcpip(dwMachineNumber, ref lpszIPAddress, dwPortNumber, dwPassWord);
	}

	public virtual void Disconnect()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("Disconnect", (ActiveXInvokeKind)0);
		}
		ocx.Disconnect();
	}

	public virtual bool SetIPAddress(ref string lpszIPAddress, int dwPortNumber, int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetIPAddress", (ActiveXInvokeKind)0);
		}
		return ocx.SetIPAddress(ref lpszIPAddress, dwPortNumber, dwPassWord);
	}

	public virtual bool OpenCommPort(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("OpenCommPort", (ActiveXInvokeKind)0);
		}
		return ocx.OpenCommPort(dwMachineNumber);
	}

	public virtual void CloseCommPort()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("CloseCommPort", (ActiveXInvokeKind)0);
		}
		ocx.CloseCommPort();
	}

	public virtual bool GetLastError(ref int dwErrorCode)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetLastError", (ActiveXInvokeKind)0);
		}
		return ocx.GetLastError(ref dwErrorCode);
	}

	public virtual bool GeneralOperationXML(ref string lpszReqNResXML)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GeneralOperationXML", (ActiveXInvokeKind)0);
		}
		return ocx.GeneralOperationXML(ref lpszReqNResXML);
	}

	public virtual bool GetDeviceLongInfo(int dwMachineNumber, int dwInfo, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceLongInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceLongInfo(dwMachineNumber, dwInfo, ref dwValue);
	}

	public virtual bool SetDeviceLongInfo(int dwMachineNumber, int dwInfo, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceLongInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceLongInfo(dwMachineNumber, dwInfo, ref dwValue);
	}

	public virtual bool ModifyDuressFP(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, int dwDuressSetting)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ModifyDuressFP", (ActiveXInvokeKind)0);
		}
		return ocx.ModifyDuressFP(dwMachineNumber, dwEnrollNumber, dwBackupNumber, dwDuressSetting);
	}

	public virtual bool GetMachineIP(string lpszProduct, int dwMachineNumber, ref string lpszIPBuf)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetMachineIP", (ActiveXInvokeKind)0);
		}
		return ocx.GetMachineIP(lpszProduct, dwMachineNumber, ref lpszIPBuf);
	}

	public virtual bool SetMachineType(string lpszMachineType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetMachineType", (ActiveXInvokeKind)0);
		}
		return ocx.SetMachineType(lpszMachineType);
	}

	public virtual void DotNET()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DotNET", (ActiveXInvokeKind)0);
		}
		ocx.DotNET();
	}

	public virtual bool GetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref object dwEnrollData, ref int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.GetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, ref dwMachinePrivilege, ref dwEnrollData, ref dwPassWord);
	}

	public virtual bool GetEnrollData1(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnrollData, ref int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetEnrollData1", (ActiveXInvokeKind)0);
		}
		return ocx.GetEnrollData1(dwMachineNumber, dwEnrollNumber, dwBackupNumber, ref dwMachinePrivilege, ref dwEnrollData, ref dwPassWord);
	}

	public virtual bool SetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, ref object dwEnrollData, int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.SetEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege, ref dwEnrollData, dwPassWord);
	}

	public virtual bool SetEnrollData1(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, int dwMachinePrivilege, ref int dwEnrollData, int dwPassWord)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetEnrollData1", (ActiveXInvokeKind)0);
		}
		return ocx.SetEnrollData1(dwMachineNumber, dwEnrollNumber, dwBackupNumber, dwMachinePrivilege, ref dwEnrollData, dwPassWord);
	}

	public virtual bool DeleteEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("DeleteEnrollData", (ActiveXInvokeKind)0);
		}
		return ocx.DeleteEnrollData(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber);
	}

	public virtual bool ReadSuperLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadSuperLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadSuperLogData(dwMachineNumber);
	}

	public virtual bool GetSuperLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int dwSMachineNumber, ref int dwGEnrollNumber, ref int dwGMachineNumber, ref int dwManipulation, ref int dwBackupNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetSuperLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetSuperLogData(dwMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref dwSMachineNumber, ref dwGEnrollNumber, ref dwGMachineNumber, ref dwManipulation, ref dwBackupNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
	}

	public virtual bool ReadGeneralLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadGeneralLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadGeneralLogData(dwMachineNumber);
	}

	public virtual bool GetGeneralLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetGeneralLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetGeneralLogData(dwMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
	}

	public virtual bool ReadAllSLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllSLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllSLogData(dwMachineNumber);
	}

	public virtual bool GetAllSLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int dwSMachineNumber, ref int dwGEnrollNumber, ref int dwGMachineNumber, ref int dwManipulation, ref int dwBackupNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllSLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllSLogData(dwMachineNumber, ref dwTMachineNumber, ref dwSEnrollNumber, ref dwSMachineNumber, ref dwGEnrollNumber, ref dwGMachineNumber, ref dwManipulation, ref dwBackupNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
	}

	public virtual bool ReadAllGLogData(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllGLogData", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllGLogData(dwMachineNumber);
	}

	public virtual bool GetAllGLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetAllGLogData", (ActiveXInvokeKind)0);
		}
		return ocx.GetAllGLogData(dwMachineNumber, ref dwTMachineNumber, ref dwEnrollNumber, ref dwEMachineNumber, ref dwVerifyMode, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
	}

	public virtual bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceStatus", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceStatus(dwMachineNumber, dwStatus, ref dwValue);
	}

	public virtual bool GetDeviceInfo(int dwMachineNumber, int dwInfo, ref int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceInfo", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceInfo(dwMachineNumber, dwInfo, ref dwValue);
	}

	public virtual bool SetDeviceInfo(int dwMachineNumber, int dwInfo, int dwValue)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceInfo", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceInfo(dwMachineNumber, dwInfo, dwValue);
	}

	public virtual bool EnableDevice(int dwMachineNumber, int bFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableDevice", (ActiveXInvokeKind)0);
		}
		return ocx.EnableDevice(dwMachineNumber, bFlag);
	}

	public virtual bool EnableUser(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int bFlag)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("EnableUser", (ActiveXInvokeKind)0);
		}
		return ocx.EnableUser(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, bFlag);
	}

	public virtual bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond, ref int dwDayOfWeek)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("GetDeviceTime", (ActiveXInvokeKind)0);
		}
		return ocx.GetDeviceTime(dwMachineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond, ref dwDayOfWeek);
	}

	public virtual bool SetDeviceTime(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("SetDeviceTime", (ActiveXInvokeKind)0);
		}
		return ocx.SetDeviceTime(dwMachineNumber);
	}

	public virtual bool PowerOnAllDevice()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PowerOnAllDevice", (ActiveXInvokeKind)0);
		}
		return ocx.PowerOnAllDevice();
	}

	public virtual bool PowerOffDevice(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("PowerOffDevice", (ActiveXInvokeKind)0);
		}
		return ocx.PowerOffDevice(dwMachineNumber);
	}

	public virtual bool ModifyPrivilege(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ModifyPrivilege", (ActiveXInvokeKind)0);
		}
		return ocx.ModifyPrivilege(dwMachineNumber, dwEnrollNumber, dwEMachineNumber, dwBackupNumber, dwMachinePrivilege);
	}

	public virtual bool ReadAllUserID(int dwMachineNumber)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (ocx == null)
		{
			throw new InvalidActiveXStateException("ReadAllUserID", (ActiveXInvokeKind)0);
		}
		return ocx.ReadAllUserID(dwMachineNumber);
	}

	protected override void CreateSink()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			eventMulticaster = new AxSBXPCEventMulticaster(this);
			cookie = new ConnectionPointCookie((object)ocx, (object)eventMulticaster, typeof(_DSBXPCEvents));
		}
		catch (Exception)
		{
		}
	}

	protected override void DetachSink()
	{
		try
		{
			cookie.Disconnect();
		}
		catch (Exception)
		{
		}
	}

	protected override void AttachInterfaces()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			ocx = (_DSBXPC)((AxHost)this).GetOcx();
		}
		catch (Exception)
		{
		}
	}

	internal void RaiseOnOnReceiveEventXML(object sender, _DSBXPCEvents_OnReceiveEventXMLEvent e)
	{
		if (this.OnReceiveEventXML != null)
		{
			this.OnReceiveEventXML(sender, e);
		}
	}
}
