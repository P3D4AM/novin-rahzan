using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_OLECompleteDragEventHandler([In][Out] ref int Effect);
