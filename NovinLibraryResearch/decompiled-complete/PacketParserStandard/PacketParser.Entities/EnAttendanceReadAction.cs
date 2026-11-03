namespace PacketParser.Entities;

public enum EnAttendanceReadAction : short
{
	[PersianNameAttribute.PersianName("خواندن")]
	Read,
	[PersianNameAttribute.PersianName("حذف")]
	Delete
}
