namespace PacketParser.Entities;

public enum ENTelegramSessionStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("شکست در هنگام ارتباط با تلگرام")]
	Faild,
	[PersianNameAttribute.PersianName("کد تایید ارسال شده است")]
	VarifidCode,
	[PersianNameAttribute.PersianName("در انتظار وارد شدن رمز تایید دو مرحله ای")]
	GetPassword,
	[PersianNameAttribute.PersianName("ورود به اکانت با موفقیت انجام شد")]
	LoginCompleted
}
