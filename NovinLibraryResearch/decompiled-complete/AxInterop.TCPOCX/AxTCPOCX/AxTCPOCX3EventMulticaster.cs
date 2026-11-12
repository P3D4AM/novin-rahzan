using System.Runtime.InteropServices;
using TCPOCX;

namespace AxTCPOCX;

[ClassInterface(ClassInterfaceType.None)]
public class AxTCPOCX3EventMulticaster : __TCPOCX3
{
	private AxTCPOCX3 parent;

	public AxTCPOCX3EventMulticaster(AxTCPOCX3 parent)
	{
		this.parent = parent;
	}

	public virtual void ReceiveSalePacket(byte vazni, string dateStr, string timeStr, short memory, short factorNo, double price, double weight, string iP, short iSRepeat)
	{
		__TCPOCX3_ReceiveSalePacketEvent e = new __TCPOCX3_ReceiveSalePacketEvent(vazni, dateStr, timeStr, memory, factorNo, price, weight, iP, iSRepeat);
		parent.RaiseOnReceiveSalePacket(parent, e);
	}
}
