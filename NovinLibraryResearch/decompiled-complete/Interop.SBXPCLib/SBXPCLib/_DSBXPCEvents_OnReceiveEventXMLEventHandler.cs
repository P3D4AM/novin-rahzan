using System.Runtime.InteropServices;

namespace SBXPCLib;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _DSBXPCEvents_OnReceiveEventXMLEventHandler([MarshalAs(UnmanagedType.BStr)] string lpszEventXML);
