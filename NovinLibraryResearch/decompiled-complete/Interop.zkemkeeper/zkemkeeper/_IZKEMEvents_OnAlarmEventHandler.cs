using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void _IZKEMEvents_OnAlarmEventHandler([In] int AlarmType, [In] int EnrollNumber, [In] int Verified);
