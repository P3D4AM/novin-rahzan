namespace PacketParser.Entities;

public enum SanadType : short
{
	[PersianNameAttribute.PersianName("افتتاحیه")]
	Eftetahiye = 1,
	[PersianNameAttribute.PersianName("عادی")]
	Movaghat = 2,
	[PersianNameAttribute.PersianName("سود و زیانی")]
	CostAndBenefit = 3,
	[PersianNameAttribute.PersianName("اتومات")]
	Automatic = 4,
	[PersianNameAttribute.PersianName("اختتامیه")]
	Ekhtetamiye = 6,
	[PersianNameAttribute.PersianName("پیش نویس")]
	Draft = 7,
	[PersianNameAttribute.PersianName("یادداشت")]
	Note = 8
}
