using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void DMSFlexGridEvents_OLEDragOverEventHandler([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref int Effect, [In][Out] ref short Button, [In][Out] ref short Shift, [In][Out] ref float x, [In][Out] ref float y, [In][Out] ref short State);
