namespace PacketParser.Entities;

public enum EnTypePost : short
{
	[PersianNameAttribute.PersianName("هیچکدام")]
	None,
	[PersianNameAttribute.PersianName("تصویر")]
	Image,
	[PersianNameAttribute.PersianName("صفحه وب")]
	WebPage
}
