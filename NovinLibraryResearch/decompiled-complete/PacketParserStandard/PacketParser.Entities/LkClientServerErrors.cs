namespace PacketParser.Entities;

public enum LkClientServerErrors
{
	[PersianNameAttribute.PersianName(" بدون خطا")]
	None,
	[PersianNameAttribute.PersianName("نرم افزار سرور در حال اجرا نمی باشد")]
	ServerIsNotRun,
	[PersianNameAttribute.PersianName("کلاینت شبکه برای این نوع از محصول تنظیم نشده است")]
	ClientNumberNotSet,
	[PersianNameAttribute.PersianName("تعداد کلاینت های شبکه از حد مجاز عبور کرده است")]
	ClientListIsFull,
	[PersianNameAttribute.PersianName("سرور نمی تواند اجرا شود")]
	ServerTcpNotRun,
	[PersianNameAttribute.PersianName("نرم افزار سرور برای این محصول  فعال سازی نشده است")]
	ServerNotActive,
	[PersianNameAttribute.PersianName("عدم ارتباط با سرور")]
	CanNotConnectToserver,
	[PersianNameAttribute.PersianName("عدم تطابق در ارسال و در یافت پارامتر های امنیتی")]
	IncompatibilityInParameter,
	[PersianNameAttribute.PersianName(" خطا در دریافت آی دی کلاینت")]
	CanNotParseGuidClient,
	[PersianNameAttribute.PersianName(" خطای تایپ نرم افزار ناشناخته")]
	TypeUnknown,
	[PersianNameAttribute.PersianName(" خطای نا شناخته")]
	UnKnown
}
