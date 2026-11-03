namespace PacketParser.Entities;

public enum EnChequeType : short
{
	[PersianNameAttribute.PersianName("سنتی")]
	Traditional,
	[PersianNameAttribute.PersianName("یکپارچه")]
	Integrated,
	[PersianNameAttribute.PersianName("صیادی")]
	Sayadi,
	[PersianNameAttribute.PersianName("صیادی الکترونیکی")]
	SayadiElectronic
}
