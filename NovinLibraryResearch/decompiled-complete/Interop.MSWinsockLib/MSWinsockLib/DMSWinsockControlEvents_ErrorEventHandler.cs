using System.Runtime.InteropServices;

namespace MSWinsockLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSWinsockControlEvents_ErrorEventHandler(short Number, [MarshalAs(UnmanagedType.BStr)] ref string Description, int Scode, [MarshalAs(UnmanagedType.BStr)] string Source, [MarshalAs(UnmanagedType.BStr)] string HelpFile, int HelpContext, ref bool CancelDisplay);
