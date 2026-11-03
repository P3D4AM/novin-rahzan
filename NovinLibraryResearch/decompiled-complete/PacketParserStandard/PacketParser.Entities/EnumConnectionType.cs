namespace PacketParser.Entities;

public enum EnumConnectionType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("پارسیان")]
	Parsian,
	[PersianNameAttribute.PersianName("فناپ TCP")]
	FanapTCP,
	[PersianNameAttribute.PersianName("پرداخت نوین")]
	PardakhtNovin,
	[PersianNameAttribute.PersianName("سامان کیش TCP")]
	SamanKishTCP,
	[PersianNameAttribute.PersianName("مبنا کارت")]
	MabnaCart,
	[PersianNameAttribute.PersianName("آسان پرداخت")]
	AsanPardakht,
	[PersianNameAttribute.PersianName("به پرداخت TCP Version3")]
	BehPardakhtTCPV3,
	[PersianNameAttribute.PersianName("سداد (BlueBird)")]
	Sadad_BlueBird
}
