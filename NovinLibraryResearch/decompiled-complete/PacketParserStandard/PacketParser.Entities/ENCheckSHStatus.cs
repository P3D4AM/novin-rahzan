namespace PacketParser.Entities;

public enum ENCheckSHStatus : short
{
	[PersianNameAttribute.PersianName("پرداختنی")]
	Pardakhtani = 101,
	[PersianNameAttribute.PersianName("وصول شده")]
	VosolShode,
	[PersianNameAttribute.PersianName("برگشت شده")]
	BargashtShode,
	[PersianNameAttribute.PersianName("باطل شده")]
	BatelShode,
	[PersianNameAttribute.PersianName("انتظامی پرداختنی")]
	Entezami_Pardakhtani,
	[PersianNameAttribute.PersianName("انتظامی عودت شده")]
	Entezami_Odat,
	[PersianNameAttribute.PersianName("عودت شده")]
	OdatShode
}
