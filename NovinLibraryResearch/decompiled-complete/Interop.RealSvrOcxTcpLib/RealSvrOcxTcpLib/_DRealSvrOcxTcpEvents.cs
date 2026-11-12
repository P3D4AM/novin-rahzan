using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[ComImport]
[InterfaceType(2)]
[Guid("CD0828CA-D5F5-48B4-B07C-93B6545C19F4")]
[TypeLibType(4096)]
public interface _DRealSvrOcxTcpEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void OnReceiveGLogData([MarshalAs(UnmanagedType.BStr)] string astrDeviceIP, int anDevicePort, int anDeviceID, int anSEnrollNumber, int anVerifyMode, int anInOutMode, DateTime anLogDate, [MarshalAs(UnmanagedType.BStr)] string astrSerialNo);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void OnReceiveGLogDataExtend([MarshalAs(UnmanagedType.BStr)] string astrRootIP, [MarshalAs(UnmanagedType.BStr)] string astrDeviceIP, int anDevicePort, int anDeviceID, int anSEnrollNumber, int anVerifyMode, int anInOutMode, DateTime anLogDate, [MarshalAs(UnmanagedType.BStr)] string astrSerialNo);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void OnReceiveGLogTextAndImage([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrLogText, [MarshalAs(UnmanagedType.BStr)] string astrLogImage);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void OnReceiveGLogText([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrLogText);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void OnReceiveGLogTextOnDoorOpen([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrLogText, [MarshalAs(UnmanagedType.BStr)] string astrLogImage);
}
