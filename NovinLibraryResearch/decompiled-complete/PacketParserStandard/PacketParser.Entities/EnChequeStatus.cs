namespace PacketParser.Entities;

public enum EnChequeStatus : short
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
	[PersianNameAttribute.PersianName("پرداختنی")]
	Pardakhtani = 8,
	[PersianNameAttribute.PersianName("پرداخت شده")]
	PardakhtShode = 9,
	[PersianNameAttribute.PersianName("اقساط دریافتنی")]
	QestVarede = 201,
	[PersianNameAttribute.PersianName("اقساط پرداختنی")]
	QestSadere = 202
}
