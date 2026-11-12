using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SMDOCX;

[ComImport]
[TypeLibType(4304)]
[Guid("108F6D7F-6E01-4B22-8E6E-A15CA728A1F0")]
public interface _MahakScaleOCX
{
	void _VtblGap7_482();

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
	[DispId(1610809358)]
	[TypeLibFunc(64)]
	bool ReadScaleInfo([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809359)]
	bool WriteDescription([In] byte DeviceID, [In][MarshalAs(UnmanagedType.BStr)] string ShopName, [In][MarshalAs(UnmanagedType.BStr)] string Endmessage);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809360)]
	bool ReadDescription([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809361)]
	bool ManagerLockOn([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809362)]
	bool ManagerLockOff([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809363)]
	bool WriteClockDate([In] byte DeviceID, [In] byte CurrentMin, [In] byte CurrentHour, [In] byte CurrentDay, [In] byte CurrentMonth, [In] short CurrentYear);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809364)]
	bool ReadClockDate([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809365)]
	bool WriteAccessCode([In] byte DeviceID, [In] byte IDAcc, [In] byte BoudRate, [In] byte RenameMemoryAcc, [In] byte RenameShopNameAcc, [In][MarshalAs(UnmanagedType.Struct)] object ChangeClockDateAcc, [In][Out][MarshalAs(UnmanagedType.Struct)] ref object ChangeDivisionAcc);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809366)]
	bool WriteFactorNumber([In] byte DeviceID, [In] int FactorNumber);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809367)]
	bool ReadFactorNumber([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809368)]
	bool ReadManagerLock([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809369)]
	bool WritePavilionNumber([In] byte DeviceID, [In] byte IDNo, [In] byte PavilionNo);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809370)]
	bool ReadWeight([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809371)]
	bool WriteDirectMemory([In] byte DeviceID, [In] short MemoryNo, [In] short MemoryType, [In][MarshalAs(UnmanagedType.BStr)] string MemoryName, [In] int UntiPrice, [In] double GPrice, [In] double GWeight, [In] bool ChangG);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809372)]
	bool ReadDirectMemory([In] byte DeviceID, [In] short MemoryNo);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809373)]
	bool ReadSaleMemory([In] byte DeviceID, [In] bool BeforSale);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809374)]
	bool ReadDisplay([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(1610809377)]
	void ManageEVRecieve();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809378)]
	bool EraseE2prom([In] byte DeviceID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809379)]
	[return: MarshalAs(UnmanagedType.Struct)]
	object Answer([In] byte Nom);
}
