namespace PacketParser.Entities;

public enum EnReasonTravel : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("تحصیل")]
	Education,
	[PersianNameAttribute.PersianName("تفریح")]
	Recreation,
	[PersianNameAttribute.PersianName("سفر کاری")]
	BusinessTrip,
	[PersianNameAttribute.PersianName("سایر")]
	More
}
