namespace PacketParser.Entities;

public enum ResponseStatus
{
	[PersianNameAttribute.PersianName("موفق")]
	Success,
	[PersianNameAttribute.PersianName("فایل پیدا نشد")]
	FileNotFound,
	[PersianNameAttribute.PersianName("نام کاربری یا رمز عبور اشتباه است")]
	InvalidUserOrPassword,
	[PersianNameAttribute.PersianName("خطا در سرور")]
	ErrorInServer,
	[PersianNameAttribute.PersianName("خطا در پارامترها")]
	ErrorInParameters,
	[PersianNameAttribute.PersianName("خطا در کلاینت")]
	ErrorInClient,
	[PersianNameAttribute.PersianName("شما مجاز به استفاده از این سرویس نیستید")]
	ErrorInAccess,
	[PersianNameAttribute.PersianName("سرور موقتا در دسترس نیست")]
	ServerUnAvailable,
	[PersianNameAttribute.PersianName("سمت سرور پاسخ دهنده ای یافت نشد")]
	RequestTimeout,
	[PersianNameAttribute.PersianName("سمت سرور پاسخ دهنده ای یافت نشد")]
	BadGateway,
	[PersianNameAttribute.PersianName("سمت سرور پاسخ دهنده ای یافت نشد")]
	GatewayTimeout,
	[PersianNameAttribute.PersianName("سمت سرور پاسخ دهنده ای یافت نشد")]
	NotFound,
	[PersianNameAttribute.PersianName("سمت سرور پاسخ دهنده ای یافت نشد")]
	MethodNotAllowed,
	[PersianNameAttribute.PersianName("سرور در حال بروزر سانی است یا برای لحظاتی در دسترس نیست مجددا سعی کنید")]
	ServiceUnavailable,
	[PersianNameAttribute.PersianName("خطا 400 درخواست شما برای سرور معتبر نیست")]
	BadRequest,
	[PersianNameAttribute.PersianName("از سمت سرور به دلایل مشخص نتیجه ای بازنگردانده نشد")]
	ErrorHandledByUser
}
