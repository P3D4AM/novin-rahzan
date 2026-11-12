using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SMDOCX;

[ComImport]
[ComSourceInterfaces("SMDOCX.__MahakScaleOCX\0\0")]
[Guid("C7C7BA84-DF4D-47B2-90A8-A4B9FD6C8DB0")]
[TypeLibType(32)]
[ClassInterface(0)]
public class MahakScaleOCXClass : _MahakScaleOCX, MahakScaleOCX, __MahakScaleOCX_Event
{
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

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809358)]
	[TypeLibFunc(64)]
	public virtual extern bool ReadScaleInfo([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809359)]
	public virtual extern bool WriteDescription([In] byte DeviceID, [In][MarshalAs(UnmanagedType.BStr)] string ShopName, [In][MarshalAs(UnmanagedType.BStr)] string Endmessage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809360)]
	public virtual extern bool ReadDescription([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809361)]
	public virtual extern bool ManagerLockOn([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809362)]
	public virtual extern bool ManagerLockOff([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809363)]
	public virtual extern bool WriteClockDate([In] byte DeviceID, [In] byte CurrentMin, [In] byte CurrentHour, [In] byte CurrentDay, [In] byte CurrentMonth, [In] short CurrentYear);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809364)]
	public virtual extern bool ReadClockDate([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809365)]
	public virtual extern bool WriteAccessCode([In] byte DeviceID, [In] byte IDAcc, [In] byte BoudRate, [In] byte RenameMemoryAcc, [In] byte RenameShopNameAcc, [In][MarshalAs(UnmanagedType.Struct)] object ChangeClockDateAcc, [In][Out][MarshalAs(UnmanagedType.Struct)] ref object ChangeDivisionAcc);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809366)]
	public virtual extern bool WriteFactorNumber([In] byte DeviceID, [In] int FactorNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809367)]
	public virtual extern bool ReadFactorNumber([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809368)]
	public virtual extern bool ReadManagerLock([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809369)]
	public virtual extern bool WritePavilionNumber([In] byte DeviceID, [In] byte IDNo, [In] byte PavilionNo);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809370)]
	public virtual extern bool ReadWeight([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809371)]
	public virtual extern bool WriteDirectMemory([In] byte DeviceID, [In] short MemoryNo, [In] short MemoryType, [In][MarshalAs(UnmanagedType.BStr)] string MemoryName, [In] int UntiPrice, [In] double GPrice, [In] double GWeight, [In] bool ChangG);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809372)]
	public virtual extern bool ReadDirectMemory([In] byte DeviceID, [In] short MemoryNo);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809373)]
	public virtual extern bool ReadSaleMemory([In] byte DeviceID, [In] bool BeforSale);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809374)]
	public virtual extern bool ReadDisplay([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(1610809377)]
	public virtual extern void ManageEVRecieve();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809378)]
	public virtual extern bool EraseE2prom([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809379)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object Answer([In] byte Nom);
}
