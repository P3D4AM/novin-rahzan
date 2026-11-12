using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SBXPCLib;

[ComImport]
[Guid("2894E36D-6941-48E0-ABF9-0D38241884FB")]
[ClassInterface(0)]
[TypeLibType(34)]
[ComSourceInterfaces("SBXPCLib._DSBXPCEvents\0\0")]
public class SBXPCClass : _DSBXPC, SBXPC, _DSBXPCEvents_Event
{
	[DispId(1)]
	public virtual extern int CommPort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		set;
	}

	[DispId(2)]
	public virtual extern int Baudrate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		set;
	}

	[DispId(3)]
	public virtual extern bool ReadMark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		set;
	}

	public virtual extern event _DSBXPCEvents_OnReceiveEventXMLEventHandler OnReceiveEventXML;

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	public virtual extern bool SetMachineType([MarshalAs(UnmanagedType.BStr)] string lpszMachineType);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	public virtual extern void DotNET();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	public virtual extern bool GetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, [MarshalAs(UnmanagedType.Struct)] ref object dwEnrollData, ref int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	public virtual extern bool GetEnrollData1(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnrollData, ref int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	public virtual extern bool SetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, [MarshalAs(UnmanagedType.Struct)] ref object dwEnrollData, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	public virtual extern bool SetEnrollData1(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, int dwMachinePrivilege, ref int dwEnrollData, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	public virtual extern bool DeleteEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	public virtual extern bool ReadSuperLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	public virtual extern bool GetSuperLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int dwSMachineNumber, ref int dwGEnrollNumber, ref int dwGMachineNumber, ref int dwManipulation, ref int dwBackupNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	public virtual extern bool ReadGeneralLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(14)]
	public virtual extern bool GetGeneralLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(15)]
	public virtual extern bool ReadAllSLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(16)]
	public virtual extern bool GetAllSLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int dwSMachineNumber, ref int dwGEnrollNumber, ref int dwGMachineNumber, ref int dwManipulation, ref int dwBackupNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(17)]
	public virtual extern bool ReadAllGLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(18)]
	public virtual extern bool GetAllGLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(19)]
	public virtual extern bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(20)]
	public virtual extern bool GetDeviceInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(21)]
	public virtual extern bool SetDeviceInfo(int dwMachineNumber, int dwInfo, int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(22)]
	public virtual extern bool EnableDevice(int dwMachineNumber, int bFlag);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(23)]
	public virtual extern bool EnableUser(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int bFlag);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(24)]
	public virtual extern bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond, ref int dwDayOfWeek);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(25)]
	public virtual extern bool SetDeviceTime(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(26)]
	public virtual extern bool PowerOnAllDevice();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(27)]
	public virtual extern bool PowerOffDevice(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(28)]
	public virtual extern bool ModifyPrivilege(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(29)]
	public virtual extern bool ReadAllUserID(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(30)]
	public virtual extern bool GetAllUserID(int dwMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnable);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(31)]
	public virtual extern bool GetSerialNumber(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszSerialNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(32)]
	public virtual extern int GetBackupNumber(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(33)]
	public virtual extern bool GetProductCode(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszProductCode);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(34)]
	public virtual extern bool ClearKeeperData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(35)]
	public virtual extern bool EmptyEnrollData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(36)]
	public virtual extern bool EmptyGeneralLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(37)]
	public virtual extern bool EmptySuperLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(38)]
	public virtual extern bool GetUserName(int DeviceKind, int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, [MarshalAs(UnmanagedType.Struct)] ref object lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(39)]
	public virtual extern bool GetUserName1(int dwMachineNumber, int dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(40)]
	public virtual extern bool SetUserName(int DeviceKind, int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, [MarshalAs(UnmanagedType.Struct)] ref object lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	public virtual extern bool SetUserName1(int dwMachineNumber, int dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	public virtual extern bool GetCompanyName(int dwMachineNumber, [MarshalAs(UnmanagedType.Struct)] ref object dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(43)]
	public virtual extern bool GetCompanyName1(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(44)]
	public virtual extern bool SetCompanyName(int dwMachineNumber, int vKind, [MarshalAs(UnmanagedType.Struct)] ref object dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(45)]
	public virtual extern bool SetCompanyName1(int dwMachineNumber, int vKind, [MarshalAs(UnmanagedType.BStr)] ref string dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(46)]
	public virtual extern bool GetDoorStatus(int dwMachineNumber, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(47)]
	public virtual extern bool SetDoorStatus(int dwMachineNumber, int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(48)]
	public virtual extern bool GetBellTime(int dwMachineNumber, ref int dwValue, ref int dwBellInfo);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(49)]
	public virtual extern bool SetBellTime(int dwMachineNumber, int dwValue, ref int dwBellInfo);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(50)]
	public virtual extern bool ConnectSerial(int dwMachineNumber, int dwCommPort, int dwBaudRate);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	public virtual extern bool ConnectTcpip(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszIPAddress, int dwPortNumber, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	public virtual extern void Disconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	public virtual extern bool SetIPAddress([MarshalAs(UnmanagedType.BStr)] ref string lpszIPAddress, int dwPortNumber, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(54)]
	public virtual extern bool OpenCommPort(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(55)]
	public virtual extern void CloseCommPort();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(56)]
	public virtual extern bool GetLastError(ref int dwErrorCode);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(57)]
	public virtual extern bool GeneralOperationXML([MarshalAs(UnmanagedType.BStr)] ref string lpszReqNResXML);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(58)]
	public virtual extern bool GetDeviceLongInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(59)]
	public virtual extern bool SetDeviceLongInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(60)]
	public virtual extern bool ModifyDuressFP(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, int dwDuressSetting);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(61)]
	public virtual extern bool GetMachineIP([MarshalAs(UnmanagedType.BStr)] string lpszProduct, int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszIPBuf);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(62)]
	public virtual extern bool GetDepartName(int dwMachineNumber, int dwDepartNumber, int dwDepartOrDaigong, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(63)]
	public virtual extern bool SetDepartName(int dwMachineNumber, int dwDepartNumber, int dwDepartOrDaigong, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(64)]
	public virtual extern bool StartEventCapture(int dwCommType, int dwParam1, int dwParam2);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(65)]
	public virtual extern void StopEventCapture();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(66)]
	public virtual extern int XML_ParseInt([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(67)]
	public virtual extern int XML_ParseLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(68)]
	public virtual extern bool XML_ParseBoolean([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(69)]
	public virtual extern bool XML_ParseString([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string lpszValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(70)]
	public virtual extern bool XML_ParseBinaryByte([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref byte pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(71)]
	public virtual extern bool XML_ParseBinaryWord([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref ushort pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(72)]
	public virtual extern bool XML_ParseBinaryLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref int pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(73)]
	public virtual extern bool XML_ParseBinaryUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(74)]
	public virtual extern bool XML_AddInt([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, int nValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(75)]
	public virtual extern bool XML_AddLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(76)]
	public virtual extern bool XML_AddBoolean([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, bool bValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(77)]
	public virtual extern bool XML_AddString([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] string lpszValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(78)]
	public virtual extern bool XML_AddBinaryByte([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref byte dwData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(79)]
	public virtual extern bool XML_AddBinaryWord([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref ushort dwData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(80)]
	public virtual extern bool XML_AddBinaryLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref int dwData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(81)]
	public virtual extern bool XML_AddBinaryUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string lpszData);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(82)]
	public virtual extern bool XML_AddBinaryGlyph([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszMessage, int width, int height, [MarshalAs(UnmanagedType.BStr)] string lpszFontDescriptor);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(83)]
	public virtual extern int ConnectP2p([MarshalAs(UnmanagedType.BStr)] ref string lpszMachineID, [MarshalAs(UnmanagedType.BStr)] ref string lpszServerIPAddress, int dwServerPortNumber, int dwPassWord, ref int pnError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(84)]
	public virtual extern bool PrepareP2p([MarshalAs(UnmanagedType.BStr)] ref string lpszMachineID, [MarshalAs(UnmanagedType.BStr)] ref string lpszServerIPAddress, int dwServerPortNumber, ref int dwYearStart, ref int dwMonthStart, ref int dwDayStart, ref int dwYearEnd, ref int dwMonthEnd, ref int dwDayEnd, ref int pnError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(85)]
	public virtual extern bool XML_AddBinaryNameGlyph(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] ref string lpszName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(86)]
	public virtual extern bool XML_ParseMultiUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(87)]
	public virtual extern bool SetDeviceTime1(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(88)]
	public virtual extern void Disconnect1(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(89)]
	public virtual extern void DisconnectAll();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(90)]
	public virtual extern bool GeneralOperationXML1(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszReqNResXML);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(91)]
	public virtual extern void UseInternalRedraw(int redraw);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(92)]
	public virtual extern int GetInternalFwVer(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-552)]
	public virtual extern void AboutBox();
}
