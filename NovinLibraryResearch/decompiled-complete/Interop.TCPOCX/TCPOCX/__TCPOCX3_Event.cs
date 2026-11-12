using System.Runtime.InteropServices;

namespace TCPOCX;

[TypeLibType(16)]
[ComEventInterface(typeof(__TCPOCX3_0000), typeof(__TCPOCX3_EventProvider_0000))]
[ComVisible(false)]
public interface __TCPOCX3_Event
{
	event __TCPOCX3_ReceiveSalePacketEventHandler ReceiveSalePacket;
}
