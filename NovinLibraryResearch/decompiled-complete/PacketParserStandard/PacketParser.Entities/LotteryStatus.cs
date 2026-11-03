namespace PacketParser.Entities;

public enum LotteryStatus : short
{
	[PersianNameAttribute.PersianName("پرداخت شده")]
	Paid,
	[PersianNameAttribute.PersianName("پرداخت نشده")]
	NotPaid,
	[PersianNameAttribute.PersianName("قرعه کشی شده")]
	QoreKeshiShode,
	[PersianNameAttribute.PersianName("قرعه کشی نشده")]
	QoreKeshiNashode,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted
}
