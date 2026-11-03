using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void _IZKEMEvents_OnAttTransactionEventHandler([In] int EnrollNumber, [In] int IsInValid, [In] int AttState, [In] int VerifyMethod, [In] int Year, [In] int Month, [In] int Day, [In] int Hour, [In] int Minute, [In] int Second);
