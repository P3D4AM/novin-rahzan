namespace PacketParser.Entities;

public enum EnBackUpStatus : short
{
	[PersianNameAttribute.PersianName("موفق")]
	Success,
	[PersianNameAttribute.PersianName("خطا")]
	Error,
	[PersianNameAttribute.PersianName("درحال انجام")]
	Pending
}
