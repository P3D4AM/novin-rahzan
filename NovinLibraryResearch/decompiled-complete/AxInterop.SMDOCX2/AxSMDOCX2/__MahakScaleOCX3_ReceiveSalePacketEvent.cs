namespace AxSMDOCX2;

public class __MahakScaleOCX3_ReceiveSalePacketEvent
{
	public byte vazni;

	public string dateStr;

	public string timeStr;

	public short memory;

	public short factorNo;

	public double price;

	public double weight;

	public string iP;

	public short iSRepeat;

	public __MahakScaleOCX3_ReceiveSalePacketEvent(byte vazni, string dateStr, string timeStr, short memory, short factorNo, double price, double weight, string iP, short iSRepeat)
	{
		this.vazni = vazni;
		this.dateStr = dateStr;
		this.timeStr = timeStr;
		this.memory = memory;
		this.factorNo = factorNo;
		this.price = price;
		this.weight = weight;
		this.iP = iP;
		this.iSRepeat = iSRepeat;
	}
}
