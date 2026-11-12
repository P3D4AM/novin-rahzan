using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TCPOCX;

[ComImport]
[TypeLibType(4240)]
[InterfaceType(2)]
[Guid("B564F3F3-70AE-40F3-8AA6-1B27617E4A5E")]
public interface __TCPOCX3
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void ReceiveSalePacket([In] byte Vazni, [In][MarshalAs(UnmanagedType.BStr)] string DateStr, [In][MarshalAs(UnmanagedType.BStr)] string TimeStr, [In] short Memory, [In] short FactorNo, [In] double Price, [In] double Weight, [In][MarshalAs(UnmanagedType.BStr)] string IP, [In] short ISRepeat);
}
