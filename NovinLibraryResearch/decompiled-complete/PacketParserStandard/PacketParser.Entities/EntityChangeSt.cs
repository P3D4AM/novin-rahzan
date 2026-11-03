namespace PacketParser.Entities;

public enum EntityChangeSt : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("جدید")]
	New,
	[PersianNameAttribute.PersianName("ویرایش شده")]
	Edited,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted
}
