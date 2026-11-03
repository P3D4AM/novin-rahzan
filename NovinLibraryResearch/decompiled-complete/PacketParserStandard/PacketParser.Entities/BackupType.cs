namespace PacketParser.Entities;

public enum BackupType
{
	[PersianNameAttribute.PersianName("در زمان مشخص")]
	SpecialTime,
	[PersianNameAttribute.PersianName("بعداز دقیقه")]
	AfterXmin,
	[PersianNameAttribute.PersianName("بعداز ساعت")]
	AfterXHoures,
	[PersianNameAttribute.PersianName("بعداز روز")]
	AfterXday,
	[PersianNameAttribute.PersianName("بک اپ فوری")]
	ForceBackup,
	[PersianNameAttribute.PersianName("پشتیبانگیری مجدد")]
	RetryBackup
}
