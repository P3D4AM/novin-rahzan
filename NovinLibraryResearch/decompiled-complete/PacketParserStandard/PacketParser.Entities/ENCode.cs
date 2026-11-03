namespace PacketParser.Entities;

public enum ENCode : short
{
	[PersianNameAttribute.PersianName("-")]
	none,
	[PersianNameAttribute.PersianName("ساده")]
	ForoshgahiSade,
	[PersianNameAttribute.PersianName("متوسط")]
	ForoshgahiMotavaset,
	[PersianNameAttribute.PersianName("پیشرفته")]
	ForoshgahiPishrafteh,
	[PersianNameAttribute.PersianName("جامع")]
	Jame
}
