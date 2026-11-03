namespace PacketParser.Entities;

public enum EnCouponUsageType : short
{
	[PersianNameAttribute.PersianName("None")]
	None,
	[PersianNameAttribute.PersianName("غیرقابل استفاده")]
	Used,
	[PersianNameAttribute.PersianName("قابل استفاده")]
	NotUsed
}
