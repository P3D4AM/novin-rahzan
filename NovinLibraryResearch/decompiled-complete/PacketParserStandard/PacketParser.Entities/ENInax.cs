namespace PacketParser.Entities;

public enum ENInax
{
	[PersianNameAttribute.PersianName("شارژ با موفقیت انجام شد")]
	Success = 1,
	[PersianNameAttribute.PersianName("شماره سفارش صحیح نیست")]
	Faild = -1,
	[PersianNameAttribute.PersianName("اطلاعات ارسال شده ناقص است")]
	InvalidContent = -11,
	[PersianNameAttribute.PersianName("شماره تلفن وارد شده در شارژ مستقیم صحیح نمی باشد یا با نوع اپراتور همخوانی ندارد")]
	InvalidNumber = -12,
	[PersianNameAttribute.PersianName("اطلاعات احراز هویت صحیح نیست.")]
	Unathorize = -22,
	[PersianNameAttribute.PersianName("موجودی نماینده کمتر از مبلغ درخواستی است.")]
	NotEnoughCoin = -33,
	[PersianNameAttribute.PersianName("مبلغ درخواست خارج از محدوده است یا محصول مورد نظر موجود نمی باشد")]
	ChargeRequestISOutOFRange = -44,
	[PersianNameAttribute.PersianName("اپراتور شارژ مستقیم ایرانسل در دسترس نمی باشد.")]
	IrancellOpratorNotAvailable = -66,
	[PersianNameAttribute.PersianName("اپراتور شارژ مستقیم همراه اول در دسترس نمی باشد.")]
	HamrahAvalOpratorNotAvailable = -67,
	[PersianNameAttribute.PersianName("شماره درخواست (order_id) تکراری است.")]
	DuplicateOrderRequest = -77,
	[PersianNameAttribute.PersianName("سیستم موقتا\u064b با مشکل مواجه شده است.")]
	WebServiceNotAviable = -91,
	[PersianNameAttribute.PersianName("وضعیت تراکنش معلق است، در این حالت پیش از عودت وجه تراکنش به خریدار، وضعیت آن را توسط تیکت از بخش مالی پیگیری نمائید.")]
	TransActionIsFail = -200
}
