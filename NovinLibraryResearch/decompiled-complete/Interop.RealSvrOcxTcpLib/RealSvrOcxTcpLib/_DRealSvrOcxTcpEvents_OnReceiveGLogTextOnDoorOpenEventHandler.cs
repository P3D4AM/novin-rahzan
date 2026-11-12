using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEventHandler([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrLogText, [MarshalAs(UnmanagedType.BStr)] string astrLogImage);
