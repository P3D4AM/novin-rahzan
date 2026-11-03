namespace PacketParser.Entities;

public enum CustomerSearchOption
{
	[PersianNameAttribute.PersianName("تمامی موارد")]
	All,
	[PersianNameAttribute.PersianName("کد")]
	JustCode,
	[PersianNameAttribute.PersianName("نام خانوادگی")]
	JustFamily,
	[PersianNameAttribute.PersianName("تلفن")]
	JustTell,
	[PersianNameAttribute.PersianName("همراه")]
	JustMob,
	[PersianNameAttribute.PersianName("آدرس")]
	JustAddress,
	[PersianNameAttribute.PersianName("کد-نام-نام خانوادگی")]
	CodeNameFamily
}
