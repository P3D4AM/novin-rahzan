namespace PacketParser.Entities;

public enum EnLoginResult : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown,
	[PersianNameAttribute.PersianName("موفقیت")]
	Success,
	[PersianNameAttribute.PersianName("منقضی شدن کاربر")]
	UserExpired,
	[PersianNameAttribute.PersianName("منقضی شدن دپارتمان")]
	DepartmentExpired,
	[PersianNameAttribute.PersianName("استفاده مشترک از یک کلید تبادل")]
	Duplicate,
	[PersianNameAttribute.PersianName("خطای ورود نام کاربری یا کلمه عبور")]
	NotExists,
	[PersianNameAttribute.PersianName("دپارتمان تایید نشده")]
	DepartmentNotVerified,
	[PersianNameAttribute.PersianName("زمان لاگین از حداکثر تاخیر مجاز بیشتر شده")]
	TimeOut,
	[PersianNameAttribute.PersianName("درخواست بیش از حد به سرور(بلاک شدن)")]
	RequestCountExceeded,
	[PersianNameAttribute.PersianName("عدم تطابق")]
	SourceMismatch,
	[PersianNameAttribute.PersianName("لغو درخواست")]
	LoginCanceled,
	[PersianNameAttribute.PersianName("خطای ورود نام کاربری یا کلمه عبور مشتری")]
	CustomerNotExists,
	[PersianNameAttribute.PersianName("عدم ارتباط کاربر وب سرویس به کاربر حسابداری")]
	AccountingUserNotAssigned,
	[PersianNameAttribute.PersianName("کاربر حذف شده")]
	UserDeleted,
	[PersianNameAttribute.PersianName("دپارتمان حذف شده")]
	DepartmentDeleted,
	[PersianNameAttribute.PersianName("خطای داخلی سرور")]
	ServerError,
	[PersianNameAttribute.PersianName("خطای داخلی کلاینت")]
	ClientError,
	[PersianNameAttribute.PersianName("عدم ارتباط با سرور")]
	ConnectFailed,
	[PersianNameAttribute.PersianName("ثبت یک کلید تبادل برای از یک نرم افزار حسابداری")]
	DbGuidMisMatch,
	[PersianNameAttribute.PersianName("تنظیمات پارامتر های وب سرویس خطا دارد")]
	WebServiceParametersMismatch,
	[PersianNameAttribute.PersianName("خطای داخلی برنامه")]
	ApplicationError,
	[PersianNameAttribute.PersianName("قطع ارتباط")]
	Connection_Disconnected
}
