using System.Runtime.InteropServices;

namespace MSWinsockLib;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void DMSWinsockControlEvents_SendProgressEventHandler(int bytesSent, int bytesRemaining);
