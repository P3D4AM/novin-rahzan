namespace PacketParser.Entities;

public enum TaxLoginActions
{
	[PersianNameAttribute.PersianName("نامشخص")]
	Unknown,
	[PersianNameAttribute.PersianName("درخواست لاگین")]
	LoginRequest,
	[PersianNameAttribute.PersianName("پاسخ به درخواست لاگین")]
	LoginResponse
}
