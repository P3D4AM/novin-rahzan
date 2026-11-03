namespace PacketParser.Entities;

public enum PrdAnarGardaniDetStatus : byte
{
	[PersianNameAttribute.PersianName("تعیین نشده")]
	None,
	[PersianNameAttribute.PersianName("مطابقت موجودی")]
	Motabeghat,
	[PersianNameAttribute.PersianName("اضافات")]
	Ezafat,
	[PersianNameAttribute.PersianName("کسورات")]
	Kosorat
}
