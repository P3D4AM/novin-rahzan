using System.Runtime.InteropServices;

namespace zkemkeeper;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _IZKEMEvents_OnEnrollFingerExEventHandler([In][MarshalAs(UnmanagedType.BStr)] string EnrollNumber, [In] int FingerIndex, [In] int ActionResult, [In] int TemplateLength);
