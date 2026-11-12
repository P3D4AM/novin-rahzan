using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[ComImport]
[Guid("FE364B34-C0F4-42FE-8FA1-C8FE62208946")]
[ClassInterface(0)]
[ComSourceInterfaces("RealSvrOcxTcpLib._DRealSvrOcxTcpEvents\0\0")]
[TypeLibType(34)]
public class RealSvrOcxTcpClass : _DRealSvrOcxTcp, RealSvrOcxTcp, _DRealSvrOcxTcpEvents_Event
{
	public virtual extern event _DRealSvrOcxTcpEvents_OnReceiveGLogDataEventHandler OnReceiveGLogData;

	public virtual extern event _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler OnReceiveGLogDataExtend;

	public virtual extern event _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler OnReceiveGLogTextAndImage;

	public virtual extern event _DRealSvrOcxTcpEvents_OnReceiveGLogTextEventHandler OnReceiveGLogText;

	public virtual extern event _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler OnReceiveGLogTextOnDoorOpen;

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	public virtual extern int OpenNetwork(int anHostPort);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	public virtual extern int CloseNetwork(int anHostPort);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	public virtual extern int SendResponse([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrResponse);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	public virtual extern int SendRtLogResponseV1([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrResponse);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	public virtual extern int SendRtLogResponseV3([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrResponse);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(-552)]
	public virtual extern void AboutBox();
}
