using System.Runtime.InteropServices;
using SMDOCX2;

namespace AxSMDOCX2;

[ClassInterface(ClassInterfaceType.None)]
public class AxMahakScaleOCX3EventMulticaster : __MahakScaleOCX3
{
	private AxMahakScaleOCX3 parent;

	public AxMahakScaleOCX3EventMulticaster(AxMahakScaleOCX3 parent)
	{
		this.parent = parent;
	}

	public virtual void ReceiveSalePacket(byte vazni, string dateStr, string timeStr, short memory, short factorNo, double price, double weight, string iP, short iSRepeat)
	{
		__MahakScaleOCX3_ReceiveSalePacketEvent e = new __MahakScaleOCX3_ReceiveSalePacketEvent(vazni, dateStr, timeStr, memory, factorNo, price, weight, iP, iSRepeat);
		parent.RaiseOnReceiveSalePacket(parent, e);
	}
}
