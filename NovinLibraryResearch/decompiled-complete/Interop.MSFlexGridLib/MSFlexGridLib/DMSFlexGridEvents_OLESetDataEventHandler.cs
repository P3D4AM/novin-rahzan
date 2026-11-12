using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_OLESetDataEventHandler([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref short DataFormat);
