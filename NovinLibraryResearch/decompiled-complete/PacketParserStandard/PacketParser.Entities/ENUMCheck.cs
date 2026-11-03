namespace PacketParser.Entities;

public enum ENUMCheck
{
	[PersianNameAttribute.PersianName("موجود")]
	Mojod = 1,
	[PersianNameAttribute.PersianName("برگشتی")]
	Bargashti = 2,
	[PersianNameAttribute.PersianName("خرج شده")]
	KharjShode = 3,
	[PersianNameAttribute.PersianName("واگذار شده")]
	VagozarShode = 4,
	[PersianNameAttribute.PersianName("نقد شده")]
	NaghdShode = 5,
	[PersianNameAttribute.PersianName("باطل شده")]
	BatelShode = 6,
	[PersianNameAttribute.PersianName("وصول شده")]
	VosolShode = 7,
	[PersianNameAttribute.PersianName("انتظامی موجود")]
	Entezami_Mojod = 8,
	[PersianNameAttribute.PersianName("انتظامی عودت شده")]
	Entezami_Odat = 9,
	[PersianNameAttribute.PersianName("عودت شده")]
	OdatShode = 10,
	[PersianNameAttribute.PersianName("None")]
	None = 100
}
