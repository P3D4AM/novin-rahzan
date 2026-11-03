namespace PacketParser.Entities;

public enum ENMahiat : short
{
	None,
	[PersianNameAttribute.PersianName("بدهکار و بستانکار")]
	Debit_Credit,
	[PersianNameAttribute.PersianName("فقط بدهکار")]
	JustDebit,
	[PersianNameAttribute.PersianName("فقط بستانکار")]
	JustCredit,
	[PersianNameAttribute.PersianName("بدون مانده بدهکار")]
	DebitRem,
	[PersianNameAttribute.PersianName("بدون مانده بستانکار")]
	CreditRem
}
