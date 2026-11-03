namespace PacketParser.Entities;

public enum EnDeviceTravel : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("هوایی")]
	Aerial,
	[PersianNameAttribute.PersianName("شخصی")]
	Personal,
	[PersianNameAttribute.PersianName("اتوبوس")]
	Bus,
	[PersianNameAttribute.PersianName("قطار")]
	Train,
	[PersianNameAttribute.PersianName("دریایی")]
	Marine
}
