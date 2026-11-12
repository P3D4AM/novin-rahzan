using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TCPOCX;

[ComImport]
[TypeLibType(32)]
[Guid("C3639507-6596-44C9-AD5A-D0DD97F42CD4")]
[ComSourceInterfaces("TCPOCX.__TCPOCX3\0\0")]
[ClassInterface(0)]
public class TCPOCX3Class : _TCPOCX3, TCPOCX3, __TCPOCX3_Event
{
	[DispId(1745027078)]
	public virtual extern TypeEnum ConnectionType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027078)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027078)]
		[param: In]
		set;
	}

	[DispId(1745027077)]
	public virtual extern short MaxScaleNO
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027077)]
		get;
	}

	[DispId(1745027076)]
	public virtual extern bool NetWork
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
	public virtual extern ComEnum CommPort
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
	public virtual extern ModEnum Model
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
	public virtual extern byte ErrNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027073)]
		get;
	}

	[DispId(1745027072)]
	public virtual extern byte SaleNumber
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1745027072)]
		get;
	}

	public virtual extern event __TCPOCX3_ReceiveSalePacketEventHandler ReceiveSalePacket;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809352)]
	public virtual extern void OpenConnection([In] short NewPort);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809355)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string ReadIP([In] short Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809356)]
	public virtual extern bool ReadOffScale();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809366)]
	public virtual extern void CloseConnection();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809369)]
	public virtual extern bool ReadScaleInfo([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809370)]
	public virtual extern bool WriteScaleInfo([In] byte ModelID, [In] byte VerionID, [In][MarshalAs(UnmanagedType.BStr)] string SerialNumber, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809371)]
	public virtual extern bool EraseE2prom([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809372)]
	public virtual extern bool DefaultEErom([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809373)]
	public virtual extern bool WriteLockTimerPass([In][MarshalAs(UnmanagedType.BStr)] string SerialNo, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809374)]
	public virtual extern bool ReadLockTimerPass([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809375)]
	public virtual extern bool WriteLockTimer([In][Out] ref float LockTimerValue, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809376)]
	public virtual extern bool ReadLockTimer([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809377)]
	public virtual extern bool WriteDescription([In][MarshalAs(UnmanagedType.BStr)] string ShopName, [In][MarshalAs(UnmanagedType.BStr)] string Endmessage, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809378)]
	public virtual extern bool ReadDescription([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809379)]
	public virtual extern bool ManagerLockOn([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809380)]
	public virtual extern bool ManagerLockOff([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809381)]
	public virtual extern bool SetLockFlag([In] byte BitLock, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809382)]
	public virtual extern bool WriteClockDate([In] byte CurrentMin, [In] byte CurrentHour, [In] byte CurrentDay, [In] byte CurrentMonth, [In] short CurrentYear, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809383)]
	public virtual extern bool ReadClockDate([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809384)]
	public virtual extern bool WriteAccessCode([In] byte IDAcc, [In] byte BoudRate, [In] byte RenameMemoryAcc, [In] byte RenameShopNameAcc, [In][MarshalAs(UnmanagedType.Struct)] object ChangeClockDateAcc, [In][Out][MarshalAs(UnmanagedType.Struct)] ref object ChangeDivisionAcc, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809385)]
	public virtual extern bool WriteFactorNumber([In] float FactorNumber, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809386)]
	public virtual extern bool ReadFactorNumber([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809387)]
	public virtual extern bool ReadManagerLock([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809388)]
	public virtual extern bool WritePavilionNumber([In] byte IDNo, [In] byte PavilionNo, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809389)]
	public virtual extern bool ReadWeight([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809390)]
	public virtual extern bool WriteDirectMemory([In] short MemoryNo, [In] short MemoryType, [In][MarshalAs(UnmanagedType.BStr)] string MemoryName, [In] float UntiPrice, [In] double GPrice, [In] double GWeight, [In] bool ChangG, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809391)]
	public virtual extern bool ReadDirectMemory([In] short MemoryNo, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809392)]
	public virtual extern bool ReadSaleMemory([In] bool BeforSale, [In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809393)]
	public virtual extern bool ReadDisplay([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809394)]
	public virtual extern bool ReadVersion([In][MarshalAs(UnmanagedType.BStr)] string IPStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809399)]
	public virtual extern double ISExistNewSalePacket();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809400)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string ReadSalePacket([In] double Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809401)]
	public virtual extern void ChangeToReadState([In] double Index);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809402)]
	public virtual extern short Convert([In][MarshalAs(UnmanagedType.BStr)] string msg);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809403)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string SaleConvert([In][MarshalAs(UnmanagedType.BStr)] string msg);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809405)]
	public virtual extern void BufferProcessing([In][MarshalAs(UnmanagedType.BStr)] string InputStr);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809406)]
	public virtual extern void ManageEVRecieve();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809408)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object Answer([In] byte Nom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809409)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object SaleAnswer([In] byte Nom);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809410)]
	public virtual extern bool ReadCalibInfo([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809411)]
	public virtual extern bool SetScaleID([In] byte DeviceID, [In] byte NewID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809412)]
	public virtual extern bool ActivateScale([In][MarshalAs(UnmanagedType.BStr)] string IPStr, [In] bool BeforSale);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809413)]
	public virtual extern bool ActivateScale2([In][MarshalAs(UnmanagedType.BStr)] string IPStr, [In] bool BeforSale);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809414)]
	public virtual extern void ActiveTimer();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809415)]
	public virtual extern void DeActiveTimer();
}
