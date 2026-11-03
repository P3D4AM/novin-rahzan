using System.Runtime.InteropServices;

namespace zkemkeeper;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _IZKEMEvents_OnDeleteTemplateEventHandler([In] int EnrollNumber, [In] int FingerIndex);
