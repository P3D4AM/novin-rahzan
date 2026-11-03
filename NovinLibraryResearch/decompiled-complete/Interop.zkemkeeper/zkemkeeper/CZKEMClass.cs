using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComImport]
[TypeLibType(2)]
[Guid("00853A19-BD51-419B-9269-2DABE57EB61F")]
[ClassInterface(0)]
[ComSourceInterfaces("zkemkeeper._IZKEMEvents\0\0")]
public class CZKEMClass : IZKEM, CZKEM, _IZKEMEvents_Event
{
	[DispId(1)]
	public virtual extern bool ReadMark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		set;
	}

	[DispId(2)]
	public virtual extern int CommPort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[param: In]
		set;
	}

	[DispId(64)]
	public virtual extern int ConvertBIG5
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(64)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(64)]
		[param: In]
		set;
	}

	[DispId(76)]
	public virtual extern int BASE64
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(76)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(76)]
		[param: In]
		set;
	}

	[DispId(78)]
	public virtual extern uint PIN2
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(78)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(78)]
		[param: In]
		set;
	}

	[DispId(79)]
	public virtual extern int AccGroup
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(79)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(79)]
		[param: In]
		set;
	}

	[DispId(80)]
	public virtual extern int AccTimeZones
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(80)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(80)]
		[param: In]
		set;
	}

	[DispId(83)]
	public virtual extern int CardNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(83)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(83)]
		[param: In]
		set;
	}

	[DispId(102)]
	public virtual extern int PINWidth
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(102)]
		get;
	}

	[DispId(123)]
	public virtual extern int MachineNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(123)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(123)]
		[param: In]
		set;
	}

	[DispId(155)]
	public virtual extern string STR_CardNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(155)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(155)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(191)]
	public virtual extern int SSRPin
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(191)]
		get;
	}

	[DispId(241)]
	public virtual extern int PullMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(241)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(241)]
		[param: In]
		set;
	}

	public virtual extern event _IZKEMEvents_OnAttTransactionEventHandler OnAttTransaction;

	public virtual extern event _IZKEMEvents_OnKeyPressEventHandler OnKeyPress;

	public virtual extern event _IZKEMEvents_OnEnrollFingerEventHandler OnEnrollFinger;

	public virtual extern event _IZKEMEvents_OnNewUserEventHandler OnNewUser;

	public virtual extern event _IZKEMEvents_OnEMDataEventHandler OnEMData;

	public virtual extern event _IZKEMEvents_OnConnectedEventHandler OnConnected;

	public virtual extern event _IZKEMEvents_OnDisConnectedEventHandler OnDisConnected;

	public virtual extern event _IZKEMEvents_OnFingerEventHandler OnFinger;

	public virtual extern event _IZKEMEvents_OnVerifyEventHandler OnVerify;

	public virtual extern event _IZKEMEvents_OnFingerFeatureEventHandler OnFingerFeature;

	public virtual extern event _IZKEMEvents_OnHIDNumEventHandler OnHIDNum;

	public virtual extern event _IZKEMEvents_OnDoorEventHandler OnDoor;

	public virtual extern event _IZKEMEvents_OnAlarmEventHandler OnAlarm;

	public virtual extern event _IZKEMEvents_OnWriteCardEventHandler OnWriteCard;

	public virtual extern event _IZKEMEvents_OnEmptyCardEventHandler OnEmptyCard;

	public virtual extern event _IZKEMEvents_OnDeleteTemplateEventHandler OnDeleteTemplate;

	public virtual extern event _IZKEMEvents_OnAttTransactionExEventHandler OnAttTransactionEx;

	public virtual extern event _IZKEMEvents_OnEnrollFingerExEventHandler OnEnrollFingerEx;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	public virtual extern bool ClearAdministrators([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	public virtual extern bool DeleteEnrollData([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	public virtual extern bool ReadSuperLogData([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	public virtual extern bool ReadAllSLogData([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	public virtual extern bool ReadGeneralLogData([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	public virtual extern bool ReadAllGLogData([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	public virtual extern bool EnableUser([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber, [In] bool bFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	public virtual extern bool EnableDevice([In] int dwMachineNumber, [In] bool bFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	public virtual extern bool GetDeviceStatus([In] int dwMachineNumber, [In] int dwStatus, [In][Out] ref int dwValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	public virtual extern bool GetDeviceInfo([In] int dwMachineNumber, [In] int dwInfo, [In][Out] ref int dwValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	public virtual extern bool SetDeviceInfo([In] int dwMachineNumber, [In] int dwInfo, [In] int dwValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(14)]
	public virtual extern bool SetDeviceTime([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(15)]
	public virtual extern void PowerOnAllDevice();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(16)]
	public virtual extern bool PowerOffDevice([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(17)]
	public virtual extern bool ModifyPrivilege([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber, [In] int dwMachinePrivilege);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(18)]
	public virtual extern void GetLastError([In][Out] ref int dwErrorCode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(19)]
	public virtual extern bool GetEnrollData([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber, [In][Out] ref int dwMachinePrivilege, [In][Out] ref int dwEnrollData, [In][Out] ref int dwPassWord);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(20)]
	public virtual extern bool SetEnrollData([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber, [In] int dwMachinePrivilege, [In] ref int dwEnrollData, [In] int dwPassWord);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(21)]
	public virtual extern bool GetDeviceTime([In] int dwMachineNumber, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute, [In][Out] ref int dwSecond);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(22)]
	public virtual extern bool GetGeneralLogData([In] int dwMachineNumber, [In][Out] ref int dwTMachineNumber, [In][Out] ref int dwEnrollNumber, [In][Out] ref int dwEMachineNumber, [In][Out] ref int dwVerifyMode, [In][Out] ref int dwInOutMode, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(23)]
	public virtual extern bool GetSuperLogData([In] int dwMachineNumber, [In][Out] ref int dwTMachineNumber, [In][Out] ref int dwSEnrollNumber, [In][Out] ref int Params4, [In][Out] ref int Params1, [In][Out] ref int Params2, [In][Out] ref int dwManipulation, [In] ref int Params3, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(24)]
	public virtual extern bool GetAllSLogData([In] int dwMachineNumber, [In][Out] ref int dwTMachineNumber, [In][Out] ref int dwSEnrollNumber, [In][Out] ref int Params4, [In][Out] ref int Params1, [In][Out] ref int Params2, [In][Out] ref int dwManipulation, [In][Out] ref int Params3, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(25)]
	public virtual extern bool GetAllGLogData([In] int dwMachineNumber, [In][Out] ref int dwTMachineNumber, [In][Out] ref int dwEnrollNumber, [In][Out] ref int dwEMachineNumber, [In][Out] ref int dwVerifyMode, [In][Out] ref int dwInOutMode, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(26)]
	public virtual extern void ConvertPassword([In] int dwSrcPSW, [In][Out] ref int dwDestPSW, [In] int dwLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(27)]
	public virtual extern bool ReadAllUserID([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(28)]
	public virtual extern bool GetAllUserID([In] int dwMachineNumber, [In][Out] ref int dwEnrollNumber, [In][Out] ref int dwEMachineNumber, [In][Out] ref int dwBackupNumber, [In][Out] ref int dwMachinePrivilege, [In][Out] ref int dwEnable);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(29)]
	public virtual extern bool GetSerialNumber([In] int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] out string dwSerialNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(30)]
	public virtual extern bool ClearKeeperData([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(32)]
	public virtual extern int GetBackupNumber([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(33)]
	public virtual extern bool GetProductCode([In] int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] out string lpszProductCode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(34)]
	public virtual extern bool GetFirmwareVersion([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string strVersion);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(35)]
	public virtual extern bool GetSDKVersion([In][Out][MarshalAs(UnmanagedType.BStr)] ref string strVersion);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(36)]
	public virtual extern bool ClearGLog([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(37)]
	public virtual extern int GetFPTempLength([In] ref byte dwEnrollData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(38)]
	public virtual extern bool Connect_Com([In] int ComPort, [In] int MachineNumber, [In] int BaudRate);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(39)]
	public virtual extern bool Connect_Net([In][MarshalAs(UnmanagedType.BStr)] string IPAdd, [In] int Port);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(40)]
	public virtual extern void Disconnect();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(41)]
	public virtual extern bool SetUserInfo([In] int dwMachineNumber, [In] int dwEnrollNumber, [In][MarshalAs(UnmanagedType.BStr)] string Name, [In][MarshalAs(UnmanagedType.BStr)] string Password, [In] int Privilege, [In] bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(42)]
	public virtual extern bool GetUserInfo([In] int dwMachineNumber, [In] int dwEnrollNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Name, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Password, [In][Out] ref int Privilege, [In][Out] ref bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(43)]
	public virtual extern bool SetDeviceIP([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string IPAddr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(44)]
	public virtual extern bool GetDeviceIP([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string IPAddr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(45)]
	public virtual extern bool GetUserTmp([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwFingerIndex, [In][Out] ref byte TmpData, [In][Out] ref int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(46)]
	public virtual extern bool SetUserTmp([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwFingerIndex, [In] ref byte TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(47)]
	public virtual extern bool GetAllUserInfo([In] int dwMachineNumber, [In][Out] ref int dwEnrollNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Name, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Password, [In][Out] ref int Privilege, [In][Out] ref bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(48)]
	public virtual extern bool DelUserTmp([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwFingerIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(49)]
	public virtual extern bool RefreshData([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(50)]
	public virtual extern bool FPTempConvert([In] ref byte TmpData1, [In][Out] ref byte TmpData2, [In][Out] ref int Size);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(51)]
	public virtual extern bool SetCommPassword([In] int CommKey);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(52)]
	public virtual extern bool GetUserGroup([In] int dwMachineNumber, [In] int dwEnrollNumber, [In][Out] ref int UserGrp);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(53)]
	public virtual extern bool SetUserGroup([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int UserGrp);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(54)]
	public virtual extern bool GetTZInfo([In] int dwMachineNumber, [In] int TZIndex, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TZ);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(55)]
	public virtual extern bool SetTZInfo([In] int dwMachineNumber, [In] int TZIndex, [In][MarshalAs(UnmanagedType.BStr)] string TZ);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(56)]
	public virtual extern bool GetUnlockGroups([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Grps);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(57)]
	public virtual extern bool SetUnlockGroups([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string Grps);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(58)]
	public virtual extern bool GetGroupTZs([In] int dwMachineNumber, [In] int GroupIndex, [In][Out] ref int TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(59)]
	public virtual extern bool SetGroupTZs([In] int dwMachineNumber, [In] int GroupIndex, [In] ref int TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(60)]
	public virtual extern bool GetUserTZs([In] int dwMachineNumber, [In] int dwEnrollNumber, [In][Out] ref int TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(61)]
	public virtual extern bool SetUserTZs([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] ref int TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(62)]
	public virtual extern bool ACUnlock([In] int dwMachineNumber, [In] int Delay);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(63)]
	public virtual extern bool GetACFun([In][Out] ref int ACFun);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(65)]
	public virtual extern bool GetGeneralLogDataStr([In] int dwMachineNumber, [In][Out] ref int dwEnrollNumber, [In][Out] ref int dwVerifyMode, [In][Out] ref int dwInOutMode, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TimeStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(66)]
	public virtual extern bool GetUserTmpStr([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwFingerIndex, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TmpData, [In][Out] ref int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(67)]
	public virtual extern bool SetUserTmpStr([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwFingerIndex, [In][MarshalAs(UnmanagedType.BStr)] string TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(68)]
	public virtual extern bool GetEnrollDataStr([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber, [In][Out] ref int dwMachinePrivilege, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string dwEnrollData, [In][Out] ref int dwPassWord);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(69)]
	public virtual extern bool SetEnrollDataStr([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwEMachineNumber, [In] int dwBackupNumber, [In] int dwMachinePrivilege, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollData, [In] int dwPassWord);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(70)]
	public virtual extern bool GetGroupTZStr([In] int dwMachineNumber, [In] int GroupIndex, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(71)]
	public virtual extern bool SetGroupTZStr([In] int dwMachineNumber, [In] int GroupIndex, [In][MarshalAs(UnmanagedType.BStr)] string TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(72)]
	public virtual extern bool GetUserTZStr([In] int dwMachineNumber, [In] int dwEnrollNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(73)]
	public virtual extern bool SetUserTZStr([In] int dwMachineNumber, [In] int dwEnrollNumber, [In][MarshalAs(UnmanagedType.BStr)] string TZs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(74)]
	public virtual extern bool FPTempConvertStr([In][MarshalAs(UnmanagedType.BStr)] string TmpData1, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TmpData2, [In] ref int Size);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(75)]
	public virtual extern int GetFPTempLengthStr([In][MarshalAs(UnmanagedType.BStr)] string dwEnrollData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(81)]
	public virtual extern bool GetUserInfoByPIN2([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Name, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Password, [In][Out] ref int Privilege, [In][Out] ref bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(82)]
	public virtual extern bool GetUserInfoByCard([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Name, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Password, [In][Out] ref int Privilege, [In][Out] ref bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(86)]
	public virtual extern bool CaptureImage([In] bool FullImage, [In][Out] ref int Width, [In][Out] ref int Height, [In][Out] ref byte Image, [In][MarshalAs(UnmanagedType.BStr)] string ImageFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(87)]
	public virtual extern bool UpdateFirmware([In][MarshalAs(UnmanagedType.BStr)] string FirmwareFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(88)]
	public virtual extern bool StartEnroll([In] int UserID, [In] int FingerID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(89)]
	public virtual extern bool StartVerify([In] int UserID, [In] int FingerID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(90)]
	public virtual extern bool StartIdentify();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(91)]
	public virtual extern bool CancelOperation();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(92)]
	public virtual extern bool QueryState([In][Out] ref int State);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(93)]
	public virtual extern bool BackupData([In][MarshalAs(UnmanagedType.BStr)] string DataFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(94)]
	public virtual extern bool RestoreData([In][MarshalAs(UnmanagedType.BStr)] string DataFile);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(95)]
	public virtual extern bool WriteLCD([In] int Row, [In] int Col, [In][MarshalAs(UnmanagedType.BStr)] string Text);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(96)]
	public virtual extern bool ClearLCD();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(97)]
	public virtual extern bool Beep([In] int DelayMS);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(98)]
	public virtual extern bool PlayVoice([In] int Position, [In] int Length);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(99)]
	public virtual extern bool PlayVoiceByIndex([In] int Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(100)]
	public virtual extern bool EnableClock([In] int Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(101)]
	public virtual extern bool GetUserIDByPIN2([In] int PIN2, [In][Out] ref int UserID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(103)]
	public virtual extern bool GetPIN2([In] int UserID, [In][Out] ref int PIN2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(104)]
	public virtual extern bool FPTempConvertNew([In] ref byte TmpData1, [In][Out] ref byte TmpData2, [In][Out] ref int Size);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(105)]
	public virtual extern bool FPTempConvertNewStr([In][MarshalAs(UnmanagedType.BStr)] string TmpData1, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TmpData2, [In][Out] ref int Size);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(106)]
	public virtual extern bool ReadAllTemplate([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(107)]
	public virtual extern bool DisableDeviceWithTimeOut([In] int dwMachineNumber, [In] int TimeOutSec);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(108)]
	public virtual extern bool SetDeviceTime2([In] int dwMachineNumber, [In] int dwYear, [In] int dwMonth, [In] int dwDay, [In] int dwHour, [In] int dwMinute, [In] int dwSecond);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(109)]
	public virtual extern bool ClearSLog([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(110)]
	public virtual extern bool RestartDevice([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(111)]
	public virtual extern bool GetDeviceMAC([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string sMAC);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(112)]
	public virtual extern bool SetDeviceMAC([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string sMAC);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(113)]
	public virtual extern bool GetWiegandFmt([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string sWiegandFmt);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(114)]
	public virtual extern bool SetWiegandFmt([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string sWiegandFmt);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(115)]
	public virtual extern bool ClearSMS([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(116)]
	public virtual extern bool GetSMS([In] int dwMachineNumber, [In] int ID, [In][Out] ref int Tag, [In][Out] ref int ValidMinutes, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string StartTime, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Content);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(117)]
	public virtual extern bool SetSMS([In] int dwMachineNumber, [In] int ID, [In] int Tag, [In] int ValidMinutes, [In][MarshalAs(UnmanagedType.BStr)] string StartTime, [In][MarshalAs(UnmanagedType.BStr)] string Content);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(118)]
	public virtual extern bool DeleteSMS([In] int dwMachineNumber, [In] int ID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(119)]
	public virtual extern bool SetUserSMS([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int SMSID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(120)]
	public virtual extern bool DeleteUserSMS([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int SMSID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(121)]
	public virtual extern bool GetCardFun([In] int dwMachineNumber, [In][Out] ref int CardFun);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(122)]
	public virtual extern bool ClearUserSMS([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(124)]
	public virtual extern bool SetDeviceCommPwd([In] int dwMachineNumber, [In] int CommKey);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(128)]
	public virtual extern bool GetDoorState([In] int MachineNumber, [In][Out] ref int State);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(129)]
	public virtual extern bool GetVendor([In][Out][MarshalAs(UnmanagedType.BStr)] ref string strVendor);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(130)]
	public virtual extern bool GetSensorSN([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string SensorSN);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(131)]
	public virtual extern bool ReadCustData([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string CustData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(132)]
	public virtual extern bool WriteCustData([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string CustData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(133)]
	public virtual extern bool BeginBatchUpdate([In] int dwMachineNumber, [In] int UpdateFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(134)]
	public virtual extern bool BatchUpdate([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(135)]
	public virtual extern bool ClearData([In] int dwMachineNumber, [In] int DataFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(136)]
	public virtual extern bool GetDataFile([In] int dwMachineNumber, [In] int DataFlag, [In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(137)]
	public virtual extern bool WriteCard([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int dwFingerIndex1, [In] ref byte TmpData1, [In] int dwFingerIndex2, [In] ref byte TmpData2, [In] int dwFingerIndex3, [In] ref byte TmpData3, [In] int dwFingerIndex4, [In] ref byte TmpData4);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(138)]
	public virtual extern bool GetGeneralExtLogData([In] int dwMachineNumber, [In][Out] ref int dwEnrollNumber, [In][Out] ref int dwVerifyMode, [In][Out] ref int dwInOutMode, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute, [In][Out] ref int dwSecond, [In][Out] ref int dwWorkCode, [In][Out] ref int dwReserved);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(139)]
	public virtual extern bool EmptyCard([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(140)]
	public virtual extern bool GetDeviceStrInfo([In] int dwMachineNumber, [In] int dwInfo, [MarshalAs(UnmanagedType.BStr)] out string Value);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(141)]
	public virtual extern bool GetSysOption([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string Option, [MarshalAs(UnmanagedType.BStr)] out string Value);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(142)]
	public virtual extern bool SetUserInfoEx([In] int dwMachineNumber, [In] int dwEnrollNumber, [In] int VerifyStyle, [In] ref byte Reserved);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(143)]
	public virtual extern bool GetUserInfoEx([In] int dwMachineNumber, [In] int dwEnrollNumber, out int VerifyStyle, out byte Reserved);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(144)]
	public virtual extern bool DeleteUserInfoEx([In] int dwMachineNumber, [In] int dwEnrollNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(145)]
	public virtual extern bool SSR_GetGeneralLogData([In] int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] out string dwEnrollNumber, out int dwVerifyMode, out int dwInOutMode, out int dwYear, out int dwMonth, out int dwDay, out int dwHour, out int dwMinute, out int dwSecond, [In][Out] ref int dwWorkCode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(146)]
	public virtual extern bool SSR_GetAllUserInfo([In] int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] out string dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] out string Name, [MarshalAs(UnmanagedType.BStr)] out string Password, out int Privilege, out bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(147)]
	public virtual extern bool SSR_GetUserInfo([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [MarshalAs(UnmanagedType.BStr)] out string Name, [MarshalAs(UnmanagedType.BStr)] out string Password, out int Privilege, out bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(148)]
	public virtual extern bool SSR_GetUserTmp([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, out byte TmpData, out int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(149)]
	public virtual extern bool SSR_GetUserTmpStr([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, [MarshalAs(UnmanagedType.BStr)] out string TmpData, out int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(150)]
	public virtual extern bool SSR_DeleteEnrollData([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwBackupNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(151)]
	public virtual extern bool SSR_SetUserInfo([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In][MarshalAs(UnmanagedType.BStr)] string Name, [In][MarshalAs(UnmanagedType.BStr)] string Password, [In] int Privilege, [In] bool Enabled);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(152)]
	public virtual extern bool SSR_SetUserTmp([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, [In] ref byte TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(153)]
	public virtual extern bool SSR_SetUserTmpStr([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, [In][MarshalAs(UnmanagedType.BStr)] string TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(154)]
	public virtual extern bool SSR_DelUserTmp([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(156)]
	public virtual extern bool SetWorkCode([In] int WorkCodeID, [In] int AWorkCode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(157)]
	public virtual extern bool GetWorkCode([In] int WorkCodeID, out int AWorkCode);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(158)]
	public virtual extern bool DeleteWorkCode([In] int WorkCodeID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(159)]
	public virtual extern bool ClearWorkCode();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(160)]
	public virtual extern bool ReadAttRule([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(161)]
	public virtual extern bool ReadDPTInfo([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(162)]
	public virtual extern bool SaveTheDataToFile([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string TheFilePath, [In] int FileFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(163)]
	public virtual extern bool ReadTurnInfo([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(164)]
	public virtual extern bool SSR_OutPutHTMLRep([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In][MarshalAs(UnmanagedType.BStr)] string AttFile, [In][MarshalAs(UnmanagedType.BStr)] string UserFile, [In][MarshalAs(UnmanagedType.BStr)] string DeptFile, [In][MarshalAs(UnmanagedType.BStr)] string TimeClassFile, [In][MarshalAs(UnmanagedType.BStr)] string AttruleFile, [In] int BYear, [In] int BMonth, [In] int BDay, [In] int BHour, [In] int BMinute, [In] int BSecond, [In] int EYear, [In] int EMonth, [In] int EDay, [In] int EHour, [In] int EMinute, [In] int ESecond, [In][MarshalAs(UnmanagedType.BStr)] string TempPath, [In][MarshalAs(UnmanagedType.BStr)] string OutFileName, [In] int HTMLFlag, [In] int resv1, [In][MarshalAs(UnmanagedType.BStr)] string resv2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(165)]
	public virtual extern bool ReadAOptions([In][MarshalAs(UnmanagedType.BStr)] string AOption, [MarshalAs(UnmanagedType.BStr)] out string AValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(166)]
	public virtual extern bool ReadRTLog([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(167)]
	public virtual extern bool GetRTLog([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(168)]
	public virtual extern bool GetHIDEventCardNumAsStr([MarshalAs(UnmanagedType.BStr)] out string strHIDEventCardNum);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(169)]
	public virtual extern bool GetStrCardNumber([MarshalAs(UnmanagedType.BStr)] out string ACardNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(170)]
	public virtual extern bool SetStrCardNumber([In][MarshalAs(UnmanagedType.BStr)] string ACardNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(171)]
	public virtual extern bool RegEvent([In] int dwMachineNumber, [In] int EventMask);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(172)]
	public virtual extern bool CancelBatchUpdate([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(173)]
	public virtual extern bool SetSysOption([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string Option, [In][MarshalAs(UnmanagedType.BStr)] string Value);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(174)]
	public virtual extern bool Connect_Modem([In] int ComPort, [In] int MachineNumber, [In] int BaudRate, [In][MarshalAs(UnmanagedType.BStr)] string Telephone);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(175)]
	public virtual extern bool UseGroupTimeZone();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(176)]
	public virtual extern bool SetHoliday([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string Holiday);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(177)]
	public virtual extern bool GetHoliday([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Holiday);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(178)]
	public virtual extern bool SetDaylight([In] int dwMachineNumber, [In] int Support, [In][MarshalAs(UnmanagedType.BStr)] string BeginTime, [In][MarshalAs(UnmanagedType.BStr)] string EndTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(179)]
	public virtual extern bool GetDaylight([In] int dwMachineNumber, [In][Out] ref int Support, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string BeginTime, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string EndTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(180)]
	public virtual extern bool SSR_SetUnLockGroup([In] int dwMachineNumber, [In] int CombNo, [In] int Group1, [In] int Group2, [In] int Group3, [In] int Group4, [In] int Group5);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(181)]
	public virtual extern bool SSR_GetUnLockGroup([In] int dwMachineNumber, [In] int CombNo, [In][Out] ref int Group1, [In][Out] ref int Group2, [In][Out] ref int Group3, [In][Out] ref int Group4, [In][Out] ref int Group5);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(182)]
	public virtual extern bool SSR_SetGroupTZ([In] int dwMachineNumber, [In] int GroupNo, [In] int Tz1, [In] int Tz2, [In] int Tz3, [In] int VaildHoliday, [In] int VerifyStyle);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(183)]
	public virtual extern bool SSR_GetGroupTZ([In] int dwMachineNumber, [In] int GroupNo, [In][Out] ref int Tz1, [In][Out] ref int Tz2, [In][Out] ref int Tz3, [In][Out] ref int VaildHoliday, [In][Out] ref int VerifyStyle);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(184)]
	public virtual extern bool SSR_GetHoliday([In] int dwMachineNumber, [In] int HolidayID, [In][Out] ref int BeginMonth, [In][Out] ref int BeginDay, [In][Out] ref int EndMonth, [In][Out] ref int EndDay, [In][Out] ref int TimeZoneID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(185)]
	public virtual extern bool SSR_SetHoliday([In] int dwMachineNumber, [In] int HolidayID, [In] int BeginMonth, [In] int BeginDay, [In] int EndMonth, [In] int EndDay, [In] int TimeZoneID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(186)]
	public virtual extern bool GetPlatform([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string Platform);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(187)]
	public virtual extern bool SSR_SetUserSMS([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int SMSID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(188)]
	public virtual extern bool SSR_DeleteUserSMS([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int SMSID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(189)]
	public virtual extern bool IsTFTMachine([In] int dwMachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(190)]
	public virtual extern bool SSR_EnableUser([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] bool bFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(192)]
	public virtual extern bool SendCMDMsg([In] int dwMachineNumber, [In] int Param1, [In] int Param2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(193)]
	public virtual extern bool SendFile([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(194)]
	public virtual extern bool SetLanguageByID([In] int dwMachineNumber, [In] int LanguageID, [In][MarshalAs(UnmanagedType.BStr)] string Language);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(195)]
	public virtual extern bool ReadFile([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string FileName, [In][MarshalAs(UnmanagedType.BStr)] string FilePath);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(196)]
	public virtual extern bool SetLastCount([In] int count);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(197)]
	public virtual extern bool SetCustomizeAttState([In] int dwMachineNumber, [In] int StateID, [In] int NewState);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(198)]
	public virtual extern bool DelCustomizeAttState([In] int dwMachineNumber, [In] int StateID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(199)]
	public virtual extern bool EnableCustomizeAttState([In] int dwMachineNumber, [In] int StateID, [In] int Enable);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(200)]
	public virtual extern bool SetCustomizeVoice([In] int dwMachineNumber, [In] int VoiceID, [In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(201)]
	public virtual extern bool DelCustomizeVoice([In] int dwMachineNumber, [In] int VoiceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(202)]
	public virtual extern bool EnableCustomizeVoice([In] int dwMachineNumber, [In] int VoiceID, [In] int Enable);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(203)]
	public virtual extern bool StartEnrollEx([In][MarshalAs(UnmanagedType.BStr)] string UserID, [In] int FingerID, [In] int Flag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(215)]
	public virtual extern bool SSR_SetUserTmpExt([In] int dwMachineNumber, [In] int IsDeleted, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, [In] ref byte TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(216)]
	public virtual extern bool SSR_DelUserTmpExt([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(217)]
	public virtual extern bool SSR_DeleteEnrollDataExt([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwBackupNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(218)]
	public virtual extern bool SSR_GetWorkCode([In] int AWorkCode, [MarshalAs(UnmanagedType.BStr)] out string Name);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(219)]
	public virtual extern bool SSR_SetWorkCode([In] int AWorkCode, [In][MarshalAs(UnmanagedType.BStr)] string Name);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(220)]
	public virtual extern bool SSR_DeleteWorkCode([In] int PIN);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(221)]
	public virtual extern bool SSR_ClearWorkCode();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(222)]
	public virtual extern bool SSR_GetSuperLogData([In] int MachineNumber, out int Number, [MarshalAs(UnmanagedType.BStr)] out string Admin, [MarshalAs(UnmanagedType.BStr)] out string User, out int Manipulation, [MarshalAs(UnmanagedType.BStr)] out string Time, out int Params1, out int Params2, out int Params3);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(223)]
	public virtual extern bool SSR_SetShortkey([In] int ShortKeyID, [In] int ShortKeyFun, [In] int StateCode, [In][MarshalAs(UnmanagedType.BStr)] string StateName, [In] int StateAutoChange, [In][MarshalAs(UnmanagedType.BStr)] string StateAutoChangeTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(224)]
	public virtual extern bool SSR_GetShortkey([In] int ShortKeyID, [In][Out] ref int ShortKeyFun, [In][Out] ref int StateCode, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string StateName, [In][Out] ref int AutoChange, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string AutoChangeTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(225)]
	public virtual extern bool Connect_USB([In] int MachineNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(226)]
	public virtual extern bool GetSuperLogData2([In] int dwMachineNumber, [In][Out] ref int dwTMachineNumber, [In][Out] ref int dwSEnrollNumber, [In][Out] ref int Params4, [In][Out] ref int Params1, [In][Out] ref int Params2, [In][Out] ref int dwManipulation, [In][Out] ref int Params3, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute, [In][Out] ref int dwSecs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(230)]
	public virtual extern bool GetUserFace([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFaceIndex, [In][Out] ref byte TmpData, [In][Out] ref int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(231)]
	public virtual extern bool SetUserFace([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFaceIndex, [In] ref byte TmpData, [In] int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(232)]
	public virtual extern bool DelUserFace([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFaceIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(233)]
	public virtual extern bool GetUserFaceStr([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFaceIndex, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string TmpData, [In][Out] ref int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(234)]
	public virtual extern bool SetUserFaceStr([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFaceIndex, [In][MarshalAs(UnmanagedType.BStr)] string TmpData, [In] int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(235)]
	public virtual extern bool GetUserTmpEx([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, out int Flag, out byte TmpData, out int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(236)]
	public virtual extern bool GetUserTmpExStr([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, out int Flag, [MarshalAs(UnmanagedType.BStr)] out string TmpData, out int TmpLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(237)]
	public virtual extern bool SetUserTmpEx([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, [In] int Flag, [In] ref byte TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(238)]
	public virtual extern bool SetUserTmpExStr([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber, [In] int dwFingerIndex, [In] int Flag, [In][MarshalAs(UnmanagedType.BStr)] string TmpData);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(239)]
	public virtual extern bool MergeTemplate([In][Out] IntPtr Templates, [In] int FingerCount, [In][Out] ref byte TemplateDest, [In][Out] ref int FingerSize);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(240)]
	public virtual extern bool SplitTemplate([In] ref byte Template, [In][Out] IntPtr Templates, [In][Out] ref int FingerCount, [In][Out] ref int FingerSize);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(242)]
	public virtual extern bool ReadUserAllTemplate([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string dwEnrollNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(243)]
	public virtual extern bool UpdateFile([In][MarshalAs(UnmanagedType.BStr)] string FileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(244)]
	public virtual extern bool ReadLastestLogData([In] int dwMachineNumber, [In] int NewLog, [In] int dwYear, [In] int dwMonth, [In] int dwDay, [In] int dwHour, [In] int dwMinute, [In] int dwSecond);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(245)]
	public virtual extern bool SetOptionCommPwd([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string CommKey);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(246)]
	public virtual extern bool ReadSuperLogDataEx([In] int dwMachineNumber, [In] int dwYear_S, [In] int dwMonth_S, [In] int dwDay_S, [In] int dwHour_S, [In] int dwMinute_S, [In] int dwSecond_S, [In] int dwYear_E, [In] int dwMonth_E, [In] int dwDay_E, [In] int dwHour_E, [In] int dwMinute_E, [In] int dwSecond_E, [In] int dwLogIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(247)]
	public virtual extern bool GetSuperLogDataEx([In] int dwMachineNumber, [In][Out][MarshalAs(UnmanagedType.BStr)] ref string EnrollNumber, [In][Out] ref int Params4, [In][Out] ref int Params1, [In][Out] ref int Params2, [In][Out] ref int dwManipulation, [In][Out] ref int Params3, [In][Out] ref int dwYear, [In][Out] ref int dwMonth, [In][Out] ref int dwDay, [In][Out] ref int dwHour, [In][Out] ref int dwMinute, [In][Out] ref int dwSecond);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(248)]
	public virtual extern bool GetPhotoByName([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string PhotoName, out byte PhotoData, out int PhotoLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(249)]
	public virtual extern bool GetPhotoNamesByTime([In] int dwMachineNumber, [In] int iFlag, [In][MarshalAs(UnmanagedType.BStr)] string sTime, [In][MarshalAs(UnmanagedType.BStr)] string eTime, [MarshalAs(UnmanagedType.BStr)] out string AllPhotoName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(250)]
	public virtual extern bool ClearPhotoByTime([In] int dwMachineNumber, [In] int iFlag, [In][MarshalAs(UnmanagedType.BStr)] string sTime, [In][MarshalAs(UnmanagedType.BStr)] string eTime);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(251)]
	public virtual extern bool GetPhotoCount([In] int dwMachineNumber, out int count, [In] int iFlag);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(252)]
	public virtual extern bool ClearDataEx([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string TableName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(253)]
	public virtual extern bool GetDataFileEx([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string SourceFileName, [In][MarshalAs(UnmanagedType.BStr)] string DestFileName);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(254)]
	public virtual extern bool SSR_SetDeviceData([In] int dwMachineNumber, [In][MarshalAs(UnmanagedType.BStr)] string TableName, [In][MarshalAs(UnmanagedType.BStr)] string Datas, [In][MarshalAs(UnmanagedType.BStr)] string Options);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(255)]
	public virtual extern bool SSR_GetDeviceData([In] int dwMachineNumber, [MarshalAs(UnmanagedType.BStr)] out string Buffer, [In] int BufferSize, [In][MarshalAs(UnmanagedType.BStr)] string TableName, [In][MarshalAs(UnmanagedType.BStr)] string FiledNames, [In][MarshalAs(UnmanagedType.BStr)] string Filter, [In][MarshalAs(UnmanagedType.BStr)] string Options);
}
