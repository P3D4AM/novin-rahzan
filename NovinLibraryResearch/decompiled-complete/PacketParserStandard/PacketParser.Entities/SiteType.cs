namespace PacketParser.Entities;

public enum SiteType : short
{
	[PersianNameAttribute.PersianName("--")]
	NOSite,
	[PersianNameAttribute.PersianName("نمایشگاه کالا")]
	PRDShow,
	[PersianNameAttribute.PersianName("فروشگاهی")]
	WebStore,
	[PersianNameAttribute.PersianName("ابتدا لاگین")]
	FirstLogin
}
