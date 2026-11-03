namespace PacketParser.Entities;

public enum WooOrderStatus
{
	[PersianNameAttribute.PersianName("در انتظار بررسی")]
	on_hold,
	[PersianNameAttribute.PersianName("در انتظار پرداخت")]
	processing
}
