namespace PacketParser.Entities;

public enum PercentageMode : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("درصد")]
	Percent,
	[PersianNameAttribute.PersianName("مبلغ")]
	Price
}
