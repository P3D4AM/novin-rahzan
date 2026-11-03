namespace PacketParser.Entities;

public enum EnRasGiriType : short
{
	[PersianNameAttribute.PersianName("بدون راس (استاندارد)")]
	Standard,
	[PersianNameAttribute.PersianName("میانگین (راسی)")]
	With_Heading_Up
}
