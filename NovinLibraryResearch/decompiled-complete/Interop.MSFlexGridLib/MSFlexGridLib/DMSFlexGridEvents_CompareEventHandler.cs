using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComVisible(false)]
[TypeLibType(16)]
public delegate void DMSFlexGridEvents_CompareEventHandler(int Row1, int Row2, ref short Cmp);
