using System.Runtime.InteropServices;

namespace zkemkeeper;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void _IZKEMEvents_OnEMDataEventHandler([In] int DataType, [In] int DataLen, [In] ref sbyte DataBuffer);
