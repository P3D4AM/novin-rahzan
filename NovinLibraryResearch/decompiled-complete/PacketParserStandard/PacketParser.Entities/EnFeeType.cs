namespace PacketParser.Entities;

public enum EnFeeType : short
{
	[PersianNameAttribute.PersianName("انتخاب نشده")]
	None,
	[PersianNameAttribute.PersianName("درصدی از کل")]
	Percent,
	[PersianNameAttribute.PersianName("مبلغ ثابت")]
	FixedPrice,
	[PersianNameAttribute.PersianName("مبلغ به ازای هر گرم")]
	VariablePrice
}
