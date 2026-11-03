using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void _IZKEMEvents_OnEmptyCardEventHandler([In] int ActionResult);
