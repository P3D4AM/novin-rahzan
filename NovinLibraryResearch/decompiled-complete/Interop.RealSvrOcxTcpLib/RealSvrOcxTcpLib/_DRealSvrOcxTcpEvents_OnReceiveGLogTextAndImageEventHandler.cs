using System.Runtime.InteropServices;

namespace RealSvrOcxTcpLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEventHandler([MarshalAs(UnmanagedType.BStr)] string astrClientIP, int anClientPort, [MarshalAs(UnmanagedType.BStr)] string astrLogText, [MarshalAs(UnmanagedType.BStr)] string astrLogImage);
