using System.Runtime.InteropServices;

namespace zkemkeeper;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _IZKEMEvents_OnDoorEventHandler([In] int EventType);
