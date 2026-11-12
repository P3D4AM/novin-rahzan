using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SBXPCLib;

[ComImport]
[InterfaceType(2)]
[Guid("9FF3A121-0E3D-4C35-A776-C9C766FB14BE")]
[TypeLibType(4096)]
public interface _DSBXPCEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void OnReceiveEventXML([MarshalAs(UnmanagedType.BStr)] string lpszEventXML);
}
