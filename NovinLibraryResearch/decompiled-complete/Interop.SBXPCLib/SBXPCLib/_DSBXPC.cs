using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SBXPCLib;

[ComImport]
[Guid("6C1D3CFC-712A-41B4-A53F-CC10F6412791")]
[InterfaceType(2)]
[TypeLibType(4096)]
public interface _DSBXPC
{
	[DispId(1)]
	int CommPort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		set;
	}

	[DispId(2)]
	int Baudrate
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		set;
	}

	[DispId(3)]
	bool ReadMark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		set;
	}

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	bool SetMachineType([MarshalAs(UnmanagedType.BStr)] string lpszMachineType);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void DotNET();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	bool GetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, ref int dwMachinePrivilege, [MarshalAs(UnmanagedType.Struct)] ref object dwEnrollData, ref int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	bool GetEnrollData1(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnrollData, ref int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	bool SetEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege, [MarshalAs(UnmanagedType.Struct)] ref object dwEnrollData, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	bool SetEnrollData1(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, int dwMachinePrivilege, ref int dwEnrollData, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	bool DeleteEnrollData(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	bool ReadSuperLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	bool GetSuperLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int dwSMachineNumber, ref int dwGEnrollNumber, ref int dwGMachineNumber, ref int dwManipulation, ref int dwBackupNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	bool ReadGeneralLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(14)]
	bool GetGeneralLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(15)]
	bool ReadAllSLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(16)]
	bool GetAllSLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwSEnrollNumber, ref int dwSMachineNumber, ref int dwGEnrollNumber, ref int dwGMachineNumber, ref int dwManipulation, ref int dwBackupNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(17)]
	bool ReadAllGLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(18)]
	bool GetAllGLogData(int dwMachineNumber, ref int dwTMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwVerifyMode, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(19)]
	bool GetDeviceStatus(int dwMachineNumber, int dwStatus, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(20)]
	bool GetDeviceInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(21)]
	bool SetDeviceInfo(int dwMachineNumber, int dwInfo, int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(22)]
	bool EnableDevice(int dwMachineNumber, int bFlag);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(23)]
	bool EnableUser(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int bFlag);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(24)]
	bool GetDeviceTime(int dwMachineNumber, ref int dwYear, ref int dwMonth, ref int dwDay, ref int dwHour, ref int dwMinute, ref int dwSecond, ref int dwDayOfWeek);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(25)]
	bool SetDeviceTime(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(26)]
	bool PowerOnAllDevice();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(27)]
	bool PowerOffDevice(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(28)]
	bool ModifyPrivilege(int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, int dwBackupNumber, int dwMachinePrivilege);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(29)]
	bool ReadAllUserID(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(30)]
	bool GetAllUserID(int dwMachineNumber, ref int dwEnrollNumber, ref int dwEMachineNumber, ref int dwBackupNumber, ref int dwMachinePrivilege, ref int dwEnable);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(31)]
	bool GetSerialNumber(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszSerialNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(32)]
	int GetBackupNumber(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(33)]
	bool GetProductCode(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszProductCode);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(34)]
	bool ClearKeeperData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(35)]
	bool EmptyEnrollData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(36)]
	bool EmptyGeneralLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(37)]
	bool EmptySuperLogData(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(38)]
	bool GetUserName(int DeviceKind, int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, [MarshalAs(UnmanagedType.Struct)] ref object lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(39)]
	bool GetUserName1(int dwMachineNumber, int dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(40)]
	bool SetUserName(int DeviceKind, int dwMachineNumber, int dwEnrollNumber, int dwEMachineNumber, [MarshalAs(UnmanagedType.Struct)] ref object lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	bool SetUserName1(int dwMachineNumber, int dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	bool GetCompanyName(int dwMachineNumber, [MarshalAs(UnmanagedType.Struct)] ref object dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(43)]
	bool GetCompanyName1(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(44)]
	bool SetCompanyName(int dwMachineNumber, int vKind, [MarshalAs(UnmanagedType.Struct)] ref object dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(45)]
	bool SetCompanyName1(int dwMachineNumber, int vKind, [MarshalAs(UnmanagedType.BStr)] ref string dwCompanyName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(46)]
	bool GetDoorStatus(int dwMachineNumber, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(47)]
	bool SetDoorStatus(int dwMachineNumber, int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(48)]
	bool GetBellTime(int dwMachineNumber, ref int dwValue, ref int dwBellInfo);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(49)]
	bool SetBellTime(int dwMachineNumber, int dwValue, ref int dwBellInfo);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(50)]
	bool ConnectSerial(int dwMachineNumber, int dwCommPort, int dwBaudRate);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	bool ConnectTcpip(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszIPAddress, int dwPortNumber, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	void Disconnect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	bool SetIPAddress([MarshalAs(UnmanagedType.BStr)] ref string lpszIPAddress, int dwPortNumber, int dwPassWord);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(54)]
	bool OpenCommPort(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(55)]
	void CloseCommPort();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(56)]
	bool GetLastError(ref int dwErrorCode);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(57)]
	bool GeneralOperationXML([MarshalAs(UnmanagedType.BStr)] ref string lpszReqNResXML);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(58)]
	bool GetDeviceLongInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(59)]
	bool SetDeviceLongInfo(int dwMachineNumber, int dwInfo, ref int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(60)]
	bool ModifyDuressFP(int dwMachineNumber, int dwEnrollNumber, int dwBackupNumber, int dwDuressSetting);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(61)]
	bool GetMachineIP([MarshalAs(UnmanagedType.BStr)] string lpszProduct, int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszIPBuf);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(62)]
	bool GetDepartName(int dwMachineNumber, int dwDepartNumber, int dwDepartOrDaigong, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(63)]
	bool SetDepartName(int dwMachineNumber, int dwDepartNumber, int dwDepartOrDaigong, [MarshalAs(UnmanagedType.BStr)] ref string lpszUserName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(64)]
	bool StartEventCapture(int dwCommType, int dwParam1, int dwParam2);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(65)]
	void StopEventCapture();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(66)]
	int XML_ParseInt([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(67)]
	int XML_ParseLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(68)]
	bool XML_ParseBoolean([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(69)]
	bool XML_ParseString([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string lpszValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(70)]
	bool XML_ParseBinaryByte([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref byte pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(71)]
	bool XML_ParseBinaryWord([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref ushort pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(72)]
	bool XML_ParseBinaryLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref int pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(73)]
	bool XML_ParseBinaryUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(74)]
	bool XML_AddInt([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, int nValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(75)]
	bool XML_AddLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, int dwValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(76)]
	bool XML_AddBoolean([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, bool bValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(77)]
	bool XML_AddString([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] string lpszValue);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(78)]
	bool XML_AddBinaryByte([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref byte dwData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(79)]
	bool XML_AddBinaryWord([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref ushort dwData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(80)]
	bool XML_AddBinaryLong([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, ref int dwData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(81)]
	bool XML_AddBinaryUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string lpszData);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(82)]
	bool XML_AddBinaryGlyph([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] string lpszMessage, int width, int height, [MarshalAs(UnmanagedType.BStr)] string lpszFontDescriptor);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(83)]
	int ConnectP2p([MarshalAs(UnmanagedType.BStr)] ref string lpszMachineID, [MarshalAs(UnmanagedType.BStr)] ref string lpszServerIPAddress, int dwServerPortNumber, int dwPassWord, ref int pnError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(84)]
	bool PrepareP2p([MarshalAs(UnmanagedType.BStr)] ref string lpszMachineID, [MarshalAs(UnmanagedType.BStr)] ref string lpszServerIPAddress, int dwServerPortNumber, ref int dwYearStart, ref int dwMonthStart, ref int dwDayStart, ref int dwYearEnd, ref int dwMonthEnd, ref int dwDayEnd, ref int pnError);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(85)]
	bool XML_AddBinaryNameGlyph(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.BStr)] ref string lpszName);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(86)]
	bool XML_ParseMultiUnicode([MarshalAs(UnmanagedType.BStr)] ref string lpszXML, [MarshalAs(UnmanagedType.LPWStr)] string lpszTagname, [MarshalAs(UnmanagedType.BStr)] ref string pData, int dwLen);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(87)]
	bool SetDeviceTime1(int dwMachineNumber, int dwYear, int dwMonth, int dwDay, int dwHour, int dwMinute, int dwSecond);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(88)]
	void Disconnect1(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(89)]
	void DisconnectAll();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(90)]
	bool GeneralOperationXML1(int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] ref string lpszReqNResXML);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(91)]
	void UseInternalRedraw(int redraw);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(92)]
	int GetInternalFwVer(int dwMachineNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-552)]
	void AboutBox();
}
