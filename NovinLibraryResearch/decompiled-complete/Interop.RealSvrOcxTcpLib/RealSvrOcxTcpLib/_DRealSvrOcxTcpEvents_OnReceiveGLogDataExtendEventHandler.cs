using System;
using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void _DRealSvrOcxTcpEvents_OnReceiveGLogDataExtendEventHandler([MarshalAs(UnmanagedType.BStr)] string astrRootIP, [MarshalAs(UnmanagedType.BStr)] string astrDeviceIP, int anDevicePort, int anDeviceID, int anSEnrollNumber, int anVerifyMode, int anInOutMode, DateTime anLogDate, [MarshalAs(UnmanagedType.BStr)] string astrSerialNo);
