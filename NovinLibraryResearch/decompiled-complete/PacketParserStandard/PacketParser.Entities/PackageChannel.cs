namespace PacketParser.Entities;

public enum PackageChannel
{
	[PersianNameAttribute.PersianName("همه موارد")]
	None = 0,
	[PersianNameAttribute.PersianName("آلفا(تست مرحله یک)")]
	Alpha = 1,
	[PersianNameAttribute.PersianName("بتا(تست مرحله دوم)")]
	Beta = 2,
	[PersianNameAttribute.PersianName("منتشر شده")]
	Stable = 4
}
