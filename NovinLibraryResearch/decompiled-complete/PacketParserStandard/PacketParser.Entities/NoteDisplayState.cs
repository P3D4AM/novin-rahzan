namespace PacketParser.Entities;

public enum NoteDisplayState
{
	[PersianNameAttribute.PersianName("None")]
	None = 0,
	[PersianNameAttribute.PersianName("کاربران داخلی")]
	User = 1,
	[PersianNameAttribute.PersianName("مشتریان")]
	Customer = 2,
	[PersianNameAttribute.PersianName("خودم")]
	MySelf = 4
}
