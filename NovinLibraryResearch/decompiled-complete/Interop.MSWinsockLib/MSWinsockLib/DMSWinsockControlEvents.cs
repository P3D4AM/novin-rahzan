using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MSWinsockLib;

[ComImport]
[Guid("248DD893-BB45-11CF-9ABC-0080C7E7B78D")]
[TypeLibType(4112)]
[InterfaceType(2)]
[DefaultMember("DataArrival")]
public interface DMSWinsockControlEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	void Error(short Number, [MarshalAs(UnmanagedType.BStr)] ref string Description, int Scode, [MarshalAs(UnmanagedType.BStr)] string Source, [MarshalAs(UnmanagedType.BStr)] string HelpFile, int HelpContext, ref bool CancelDisplay);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(0)]
	void DataArrival(int bytesTotal);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void Connect();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void ConnectionRequest(int requestID);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void Close();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void SendProgress(int bytesSent, int bytesRemaining);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void SendComplete();
}
