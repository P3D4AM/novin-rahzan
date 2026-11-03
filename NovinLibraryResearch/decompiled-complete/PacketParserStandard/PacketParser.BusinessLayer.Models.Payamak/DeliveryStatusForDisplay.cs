using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Payamak;

public class DeliveryStatusForDisplay
{
	public DeliveryStatus DeliverySt { get; set; }

	public string ResultForDisplay => DeliverySt.GetDisplay().GetTranslate();

	public DeliveryStatusForDisplay(DeliveryStatus deliveryStatus)
	{
		DeliverySt = deliveryStatus;
	}
}
