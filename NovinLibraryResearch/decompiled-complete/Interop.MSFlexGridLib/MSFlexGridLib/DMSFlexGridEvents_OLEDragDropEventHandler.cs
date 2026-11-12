using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_OLEDragDropEventHandler([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref int Effect, [In][Out] ref short Button, [In][Out] ref short Shift, [In][Out] ref float x, [In][Out] ref float y);
