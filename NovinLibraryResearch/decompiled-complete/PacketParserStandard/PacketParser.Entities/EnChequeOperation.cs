namespace PacketParser.Entities;

public enum EnChequeOperation : short
{
	[PersianNameAttribute.PersianName("برگشت زدن")]
	BargashtZadan = 1,
	[PersianNameAttribute.PersianName("باطل کردن")]
	BatelKardan,
	[PersianNameAttribute.PersianName("واگذار کردن")]
	VagozarKardan,
	[PersianNameAttribute.PersianName("نقد کردن")]
	NaghdKardan,
	[PersianNameAttribute.PersianName("برگشتی به وضعیت قبل")]
	BargashtoToEx,
	[PersianNameAttribute.PersianName("عودت چک برگشتی")]
	OdatBargashti,
	[PersianNameAttribute.PersianName("عودت چک خرج شده")]
	OdatKharjShode,
	[PersianNameAttribute.PersianName("واگذار شده به موجود")]
	VagozarToMojod,
	[PersianNameAttribute.PersianName("وصول کردن")]
	VosolKardan,
	[PersianNameAttribute.PersianName("نقد شده به موجود")]
	NaghdToMojod,
	[PersianNameAttribute.PersianName("باطل شده به موجود")]
	BatelToMojod,
	[PersianNameAttribute.PersianName("وصول شده به واگذار")]
	VosolToVagozar,
	[PersianNameAttribute.PersianName("اعلام پرداخت")]
	ElamPardakht,
	[PersianNameAttribute.PersianName("پرداخت شده به پرداختنی")]
	PardakhtShodeToPardakhtani,
	[PersianNameAttribute.PersianName("باطل شده به پرداختنی")]
	BatelToPardakhtani,
	[PersianNameAttribute.PersianName("برگشتی به پرداختنی")]
	BargashtiToPardakhtani,
	[PersianNameAttribute.PersianName("عودت انتظامی")]
	OdatEntezami,
	[PersianNameAttribute.PersianName("خروج از عودت انتظامی")]
	OdatToMojodEntezami
}
