namespace PacketParser.Entities;

public enum NoteStatus : short
{
	[PersianNameAttribute.PersianName("خوانده نشده")]
	UnRead,
	[PersianNameAttribute.PersianName("خوانده شده")]
	Read,
	[PersianNameAttribute.PersianName("بسته شده")]
	Closed,
	[PersianNameAttribute.PersianName("حذف شده")]
	Deleted,
	[PersianNameAttribute.PersianName("همه")]
	All
}
