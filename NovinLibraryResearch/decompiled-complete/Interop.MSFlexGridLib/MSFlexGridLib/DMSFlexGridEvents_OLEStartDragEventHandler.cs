using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_OLEStartDragEventHandler([In][Out][MarshalAs(UnmanagedType.Interface)] ref DataObject Data, [In][Out] ref int AllowedEffects);
