using System.Runtime.InteropServices;

namespace zkemkeeper;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _IZKEMEvents_OnVerifyEventHandler([In] int UserID);
