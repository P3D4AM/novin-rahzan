namespace PacketParser.Entities;

public enum EnTaxInvoiceSubject
{
	[PersianNameAttribute.PersianName("فروش")]
	Sell = 1,
	[PersianNameAttribute.PersianName("اصلاحی")]
	Edit,
	[PersianNameAttribute.PersianName("ابطالی")]
	Delete,
	[PersianNameAttribute.PersianName("برگشت از فروش")]
	BackSell
}
