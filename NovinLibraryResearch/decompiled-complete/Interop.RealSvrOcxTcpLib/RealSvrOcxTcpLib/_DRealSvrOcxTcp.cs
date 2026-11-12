using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[ComImport]
[TypeLibType(4112)]
[Guid("9D2CF0EA-0F41-4665-858D-A292F269C2F9")]
[InterfaceType(2)]
public interface _DRealSvrOcxTcp
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	int OpenNetwork(int anHostPort);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	int CloseNetwork(int anHostPort);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	int SendResponse([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrResponse);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	int SendRtLogResponseV1([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrResponse);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	int SendRtLogResponseV3([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrResponse);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-552)]
	void AboutBox();
}
