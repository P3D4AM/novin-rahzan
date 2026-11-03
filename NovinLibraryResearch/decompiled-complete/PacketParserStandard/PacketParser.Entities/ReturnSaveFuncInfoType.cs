namespace PacketParser.Entities;

public enum ReturnSaveFuncInfoType
{
	[PersianNameAttribute.PersianName("عملیات با موفقیت انجام شد")]
	Success,
	[PersianNameAttribute.PersianName("خطا")]
	Error,
	[PersianNameAttribute.PersianName("پیغام")]
	Information,
	[PersianNameAttribute.PersianName("هشدار")]
	Warning
}
