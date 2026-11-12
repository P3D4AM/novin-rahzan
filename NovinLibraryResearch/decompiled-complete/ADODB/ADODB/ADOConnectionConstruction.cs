using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("00000551-0000-0010-8000-00AA006D2EA4")]
[TypeLibType(512)]
[InterfaceType(1)]
public interface ADOConnectionConstruction : ADOConnectionConstruction15
{
	[DispId(1610678272)]
	new object DSO
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
	}

	[DispId(1610678273)]
	new object Session
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	new void WrapDSOandSession([In][MarshalAs(UnmanagedType.IUnknown)] object pDSO, [In][MarshalAs(UnmanagedType.IUnknown)] object pSession);
}
