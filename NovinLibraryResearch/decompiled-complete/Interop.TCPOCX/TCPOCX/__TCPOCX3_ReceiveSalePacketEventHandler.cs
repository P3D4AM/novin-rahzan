using System.Runtime.InteropServices;

namespace TCPOCX;

[TypeLibType(16)]
[ComVisible(false)]
public delegate void __TCPOCX3_ReceiveSalePacketEventHandler([In] byte Vazni, [In][MarshalAs(UnmanagedType.BStr)] string DateStr, [In][MarshalAs(UnmanagedType.BStr)] string TimeStr, [In] short Memory, [In] short FactorNo, [In] double Price, [In] double Weight, [In][MarshalAs(UnmanagedType.BStr)] string IP, [In] short ISRepeat);
