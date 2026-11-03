namespace PacketParser.Entities;

public enum OrderActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown,
	[PersianNameAttribute.PersianName("درخواست ذخیره")]
	SaveRequest,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
	SaveResponse,
	[PersianNameAttribute.PersianName("درخواست ذخیره موقت")]
	SaveTempRequest,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره موقت")]
	SaveTempResponse,
	[PersianNameAttribute.PersianName("درخواست خواندن")]
	ReadRequest,
	[PersianNameAttribute.PersianName("سینک")]
	ReadResponse,
	[PersianNameAttribute.PersianName("ریست")]
	Reset,
	[PersianNameAttribute.PersianName("ارسال صورت حساب به سامانه مودیان")]
	TaxSendOrderRequest,
	[PersianNameAttribute.PersianName("پاسخ به صورت حساب سامانه مودیان")]
	TaxSendOrderResponse,
	[PersianNameAttribute.PersianName("رهگیری صورت حساب از سامانه مودیان")]
	TaxOrderTrackRequest,
	[PersianNameAttribute.PersianName("پاسخ به رهگیری صورت حساب از سامانه مودیان")]
	TaxOrderTrackResponse,
	[PersianNameAttribute.PersianName("درخواست ذخیره پاسخ رهگیری مودیان")]
	TaxOrderTrackSaveRequest,
	[PersianNameAttribute.PersianName("پاسخ درخواست ذخیره رهگیری مودیان")]
	TaxOrderTrackSaveResponse,
	[PersianNameAttribute.PersianName("درخواست استعلام وضعیت کد اقتصادی در سامانه مودیان")]
	TaxEconomicCodeStateRequest,
	[PersianNameAttribute.PersianName("پاسخ درخواست استعلام وضعیت کد اقتصادی در سامانه مودیان")]
	TaxEconomicCodeStateResponse
}
