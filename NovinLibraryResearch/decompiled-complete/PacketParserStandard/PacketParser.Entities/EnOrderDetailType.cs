namespace PacketParser.Entities;

public enum EnOrderDetailType : short
{
	[PersianNameAttribute.PersianName("None")]
	None,
	[PersianNameAttribute.PersianName("زیر نقطه سفارش")]
	UnderOrderPoint,
	[PersianNameAttribute.PersianName("موجودی منفی")]
	NegativeAmount
}
