namespace PacketParser.Entities;

public enum CustomerActions
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
	Reset = 7,
	[PersianNameAttribute.PersianName("کامیت درخواست ")]
	CommitRequest = 8,
	[PersianNameAttribute.PersianName("پاسخ درخواست کامیت  ")]
	CommitResponse = 9,
	[PersianNameAttribute.PersianName("درخواست ذخیره موقت")]
	TempSaveRequest = 10,
	[PersianNameAttribute.PersianName("درخواست بازیابی پسورد")]
	PasswordRecoveryRequest = 11,
	[PersianNameAttribute.PersianName(" پاسخ درخواست بازیابی پسورد")]
	PasswordRecoveryResponse = 12,
	[PersianNameAttribute.PersianName(" پاسخ ذخیره موقت")]
	TempSaveResponse = 13,
	[PersianNameAttribute.PersianName("درخواست حذف")]
	DeleteRequest = 14
}
