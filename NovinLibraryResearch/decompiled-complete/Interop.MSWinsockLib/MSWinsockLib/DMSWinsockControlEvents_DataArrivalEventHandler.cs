using System.Reflection;
using System.Runtime.InteropServices;

namespace MSWinsockLib;

[ComVisible(false)]
[DefaultMember("Invoke")]
[TypeLibType(16)]
public delegate void DMSWinsockControlEvents_DataArrivalEventHandler(int bytesTotal);
