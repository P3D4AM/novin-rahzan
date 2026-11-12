using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TCPOCX;

[ComImport]
[Guid("A18B23FE-5867-45FE-88FE-E7374E833AAD")]
[TypeLibType(4304)]
public interface _TCPOCX3
{
	void _VtblGap7_482();

	[DispId(1745027078)]
	TypeEnum ConnectionType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027078)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027078)]
		[param: In]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809352)]
	void OpenConnection([In] short NewPort);

	[DispId(1745027077)]
	short MaxScaleNO
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027077)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809355)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string ReadIP([In] short Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809356)]
	bool ReadOffScale();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809366)]
	void CloseConnection();

	[DispId(1745027076)]
	bool NetWork
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027076)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027076)]
		[param: In]
		set;
	}

	[DispId(1745027075)]
	ComEnum CommPort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027075)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027075)]
		[param: In]
		[param: Out]
		set;
	}

	[DispId(1745027074)]
	ModEnum Model
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027074)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027074)]
		[param: In]
		set;
	}

	[DispId(1745027073)]
	byte ErrNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027073)]
		get;
	}

	[DispId(1745027072)]
	byte SaleNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027072)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809369)]
	bool ReadScaleInfo([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809370)]
	bool WriteScaleInfo([In] byte ModelID, [In] byte VerionID, [In][MarshalAs(UnmanagedType.BStr)] string SerialNumber, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809371)]
	bool EraseE2prom([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809372)]
	bool DefaultEErom([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809373)]
	bool WriteLockTimerPass([In][MarshalAs(UnmanagedType.BStr)] string SerialNo, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809374)]
	bool ReadLockTimerPass([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809375)]
	bool WriteLockTimer([In][Out] ref float LockTimerValue, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809376)]
	bool ReadLockTimer([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809377)]
	bool WriteDescription([In][MarshalAs(UnmanagedType.BStr)] string ShopName, [In][MarshalAs(UnmanagedType.BStr)] string Endmessage, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809378)]
	bool ReadDescription([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809379)]
	bool ManagerLockOn([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809380)]
	bool ManagerLockOff([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809381)]
	bool SetLockFlag([In] byte BitLock, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809382)]
	bool WriteClockDate([In] byte CurrentMin, [In] byte CurrentHour, [In] byte CurrentDay, [In] byte CurrentMonth, [In] short CurrentYear, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809383)]
	bool ReadClockDate([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809384)]
	bool WriteAccessCode([In] byte IDAcc, [In] byte BoudRate, [In] byte RenameMemoryAcc, [In] byte RenameShopNameAcc, [In][MarshalAs(UnmanagedType.Struct)] object ChangeClockDateAcc, [In][Out][MarshalAs(UnmanagedType.Struct)] ref object ChangeDivisionAcc, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809385)]
	bool WriteFactorNumber([In] float FactorNumber, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809386)]
	bool ReadFactorNumber([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809387)]
	bool ReadManagerLock([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809388)]
	bool WritePavilionNumber([In] byte IDNo, [In] byte PavilionNo, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809389)]
	bool ReadWeight([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809390)]
	bool WriteDirectMemory([In] short MemoryNo, [In] short MemoryType, [In][MarshalAs(UnmanagedType.BStr)] string MemoryName, [In] float UntiPrice, [In] double GPrice, [In] double GWeight, [In] bool ChangG, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809391)]
	bool ReadDirectMemory([In] short MemoryNo, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809392)]
	bool ReadSaleMemory([In] bool BeforSale, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809393)]
	bool ReadDisplay([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809394)]
	bool ReadVersion([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809399)]
	double ISExistNewSalePacket();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809400)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string ReadSalePacket([In] double Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809401)]
	void ChangeToReadState([In] double Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809402)]
	short Convert([In][MarshalAs(UnmanagedType.BStr)] string msg);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809403)]
	[return: MarshalAs(UnmanagedType.BStr)]
	string SaleConvert([In][MarshalAs(UnmanagedType.BStr)] string msg);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809405)]
	void BufferProcessing([In][MarshalAs(UnmanagedType.BStr)] string InputStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809406)]
	void ManageEVRecieve();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809408)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object Answer([In] byte Nom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809409)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object SaleAnswer([In] byte Nom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809410)]
	bool ReadCalibInfo([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809411)]
	bool SetScaleID([In] byte DeviceID, [In] byte NewID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809412)]
	bool ActivateScale([In][MarshalAs(UnmanagedType.BStr)] string IPStr, [In] bool BeforSale);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809413)]
	bool ActivateScale2([In][MarshalAs(UnmanagedType.BStr)] string IPStr, [In] bool BeforSale);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809414)]
	void ActiveTimer();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809415)]
	void DeActiveTimer();
}
