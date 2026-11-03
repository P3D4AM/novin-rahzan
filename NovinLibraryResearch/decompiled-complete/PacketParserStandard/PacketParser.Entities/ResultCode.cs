namespace PacketParser.Entities;

public enum ResultCode
{
	[PersianNameAttribute.PersianName("عملیات با موفقیت انجام شد")]
	Success = 0,
	[PersianNameAttribute.PersianName("خطا در ارسال درخواست")]
	SendFailed = 1029,
	[PersianNameAttribute.PersianName("خطا در اتصال")]
	ConnectFailed = 1036,
	[PersianNameAttribute.PersianName("خطا در دستگاه پذیرنده")]
	MatchFailed = 2000,
	[PersianNameAttribute.PersianName("عملیات به درخواست کاربر لغو شد")]
	CancelByUser = 181,
	[PersianNameAttribute.PersianName("زمان انجام عملیات بیش از حد طولانی شده است")]
	TimeOut = 2001,
	[PersianNameAttribute.PersianName("خطا در قالب داده های ورودی")]
	InputDataFormatError = 180
}
