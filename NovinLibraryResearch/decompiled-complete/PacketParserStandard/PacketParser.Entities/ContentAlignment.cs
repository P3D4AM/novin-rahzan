namespace PacketParser.Entities;

public enum ContentAlignment
{
	[PersianNameAttribute.PersianName("None")]
	None = 0,
	[PersianNameAttribute.PersianName("TopLeft")]
	TopLeft = 1,
	[PersianNameAttribute.PersianName("TopCenter")]
	TopCenter = 2,
	[PersianNameAttribute.PersianName("TopRight")]
	TopRight = 4,
	[PersianNameAttribute.PersianName("MiddleLeft")]
	MiddleLeft = 0x10,
	[PersianNameAttribute.PersianName("MiddleCenter")]
	MiddleCenter = 0x20,
	[PersianNameAttribute.PersianName("MiddleRight")]
	MiddleRight = 0x40,
	[PersianNameAttribute.PersianName("BottomLeft")]
	BottomLeft = 0x100,
	[PersianNameAttribute.PersianName("BottomCenter")]
	BottomCenter = 0x200,
	[PersianNameAttribute.PersianName("BottomRight")]
	BottomRight = 0x400
}
