namespace PacketParser.Entities;

public enum EventNotifyEnumType : short
{
	All,
	[PersianNameAttribute.PersianName("خوانده نشده")]
	NotRead,
	[PersianNameAttribute.PersianName("خوانده شده")]
	Readed,
	[PersianNameAttribute.PersianName("بعد یادآوری کن")]
	NotifyMeLater
}
