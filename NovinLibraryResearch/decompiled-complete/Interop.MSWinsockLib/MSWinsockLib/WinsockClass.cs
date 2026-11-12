using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MSWinsockLib;

[ComImport]
[Guid("248DD896-BB45-11CF-9ABC-0080C7E7B78D")]
[ComSourceInterfaces("MSWinsockLib.DMSWinsockControlEvents\0\0")]
[TypeLibType(34)]
[ClassInterface(0)]
[DefaultMember("_RemoteHost")]
public class WinsockClass : IMSWinsockControl, Winsock, DMSWinsockControlEvents_Event
{
	[DispId(3)]
	public virtual extern ProtocolConstants Protocol
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[param: In]
		set;
	}

	[DispId(4)]
	public virtual extern string RemoteHostIP
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(5)]
	public virtual extern string LocalHostName
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(5)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(6)]
	public virtual extern string LocalIP
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(6)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(7)]
	public virtual extern int SocketHandle
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		get;
	}

	[DispId(0)]
	public virtual extern string _RemoteHost
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1)]
	public virtual extern int RemotePort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[param: In]
		set;
	}

	[DispId(2)]
	public virtual extern int LocalPort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[param: In]
		set;
	}

	[DispId(8)]
	public virtual extern short State
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		get;
	}

	[DispId(9)]
	public virtual extern int BytesReceived
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(9)]
		get;
	}

	[DispId(10)]
	public virtual extern string RemoteHost
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern event DMSWinsockControlEvents_ErrorEventHandler Error;

	public virtual extern event DMSWinsockControlEvents_DataArrivalEventHandler DataArrival;

	public virtual extern event DMSWinsockControlEvents_ConnectEventHandler DMSWinsockControlEvents_Event_Connect;

	public virtual extern event DMSWinsockControlEvents_ConnectionRequestEventHandler ConnectionRequest;

	public virtual extern event DMSWinsockControlEvents_CloseEventHandler DMSWinsockControlEvents_Event_Close;

	public virtual extern event DMSWinsockControlEvents_SendProgressEventHandler SendProgress;

	public virtual extern event DMSWinsockControlEvents_SendCompleteEventHandler SendComplete;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-552)]
	[TypeLibFunc(64)]
	public virtual extern void AboutBox();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(64)]
	public virtual extern void Connect([Optional][In][MarshalAs(UnmanagedType.Struct)] object RemoteHost, [Optional][In][MarshalAs(UnmanagedType.Struct)] object RemotePort);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(65)]
	public virtual extern void Listen();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(66)]
	public virtual extern void Accept([In] int requestID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(67)]
	public virtual extern void SendData([In][MarshalAs(UnmanagedType.Struct)] object data);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(68)]
	public virtual extern void GetData([In][Out][MarshalAs(UnmanagedType.Struct)] ref object data, [Optional][In][MarshalAs(UnmanagedType.Struct)] object type, [Optional][In][MarshalAs(UnmanagedType.Struct)] object maxLen);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(69)]
	public virtual extern void PeekData([In][Out][MarshalAs(UnmanagedType.Struct)] ref object data, [Optional][In][MarshalAs(UnmanagedType.Struct)] object type, [Optional][In][MarshalAs(UnmanagedType.Struct)] object maxLen);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(70)]
	public virtual extern void Close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(71)]
	public virtual extern void Bind([Optional][In][MarshalAs(UnmanagedType.Struct)] object LocalPort, [Optional][In][MarshalAs(UnmanagedType.Struct)] object LocalIP);
}
