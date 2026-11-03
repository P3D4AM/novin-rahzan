namespace PacketParser.Entities;

public enum EnWageCalculationMethod : short
{
	[PersianNameAttribute.PersianName("مبلغ ثابت")]
	Price,
	[PersianNameAttribute.PersianName("درصد")]
	Percent
}
