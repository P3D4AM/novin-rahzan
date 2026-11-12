using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[TypeLibType(512)]
[InterfaceType(1)]
[Guid("00000516-0000-0010-8000-00AA006D2EA4")]
public interface ADOConnectionConstruction15
{
	[DispId(1610678272)]
	object DSO
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
	}

	[DispId(1610678273)]
	object Session
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void WrapDSOandSession([In][MarshalAs(UnmanagedType.IUnknown)] object pDSO, [In][MarshalAs(UnmanagedType.IUnknown)] object pSession);
}
