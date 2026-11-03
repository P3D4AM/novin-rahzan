namespace PacketParser.Entities;

public enum CustomerSort
{
	[PersianNameAttribute.PersianName("نام سعودی")]
	NameAsc,
	[PersianNameAttribute.PersianName("نام نزولی")]
	NameDesc,
	[PersianNameAttribute.PersianName("کد سعودی")]
	CodeAsc,
	[PersianNameAttribute.PersianName("کد نزولی")]
	CodeDesc,
	[PersianNameAttribute.PersianName("مانده نزولی")]
	BalanceAsc,
	[PersianNameAttribute.PersianName("مانده سعودی")]
	BalanceDesc
}
