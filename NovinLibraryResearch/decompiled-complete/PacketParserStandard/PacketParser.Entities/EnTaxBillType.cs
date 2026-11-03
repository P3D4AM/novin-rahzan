namespace PacketParser.Entities;

public enum EnTaxBillType
{
	[PersianNameAttribute.PersianName("غیرفعال")]
	Disable,
	[PersianNameAttribute.PersianName("بدون تنظیم")]
	None,
	[PersianNameAttribute.PersianName("نوع اول")]
	TypeOne,
	[PersianNameAttribute.PersianName("نوع دوم")]
	TypeTwo,
	[PersianNameAttribute.PersianName("نوع سوم")]
	TypeThree
}
