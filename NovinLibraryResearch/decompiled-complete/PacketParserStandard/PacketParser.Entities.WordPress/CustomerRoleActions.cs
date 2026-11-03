namespace PacketParser.Entities.WordPress;

public enum CustomerRoleActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown = 0,
	[PersianNameAttribute.PersianName("درخواست ذخیره")]
	SaveRequest = 1,
	[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
	SaveResponse = 2,
	[PersianNameAttribute.PersianName("درخواست خواندن")]
	ReadRequest = 5,
	[PersianNameAttribute.PersianName("سینک")]
	ReadResponse = 6,
	[PersianNameAttribute.PersianName("ریست")]
	Reset = 7
}
