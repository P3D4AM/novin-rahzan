namespace PacketParser.Entities;

public enum CusTmpCmtRsp
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown,
	[PersianNameAttribute.PersianName("ثبت نام با موفقیت انجام شد")]
	RegisterSuccess,
	[PersianNameAttribute.PersianName("نام کاربری قبلا ثبت شده است")]
	DuplicateUserName,
	[PersianNameAttribute.PersianName("موبایل قبلا ثبت شده است")]
	DuplicateUserMobile,
	[PersianNameAttribute.PersianName("ایمیل کاربری قبلا ثبت شده است")]
	DuplicateUserEmail,
	[PersianNameAttribute.PersianName("پیامک ارسال شد")]
	SmsCodeSent,
	[PersianNameAttribute.PersianName("ایمیل ارسال شد")]
	EmailSent,
	[PersianNameAttribute.PersianName("خطا در ارسال پیامک")]
	ErrorInSendSms,
	[PersianNameAttribute.PersianName("خطایی در سرور رخ داده است")]
	ErrorInServer,
	[PersianNameAttribute.PersianName("کاربر یافت نشد")]
	UserNotFound,
	[PersianNameAttribute.PersianName("کد پیامکی نامعتبر است")]
	InvalidCode,
	[PersianNameAttribute.PersianName("تعداد درخواست ها بیش از حد مجاز است لطفا بعدا امتحان نمایید")]
	TooMuchRequest,
	[PersianNameAttribute.PersianName("کنسل شدن توکن")]
	OperationCanceled,
	[PersianNameAttribute.PersianName("خطای داخلی کلاینت")]
	ClientException
}
