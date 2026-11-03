using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Settings;

public class OrderBarcodeDetectForDisplay
{
	public EnOrderBarcodeDetect Type { get; private set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public OrderBarcodeDetectForDisplay(EnOrderBarcodeDetect _type)
	{
		Type = _type;
	}
}
