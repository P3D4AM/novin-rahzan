namespace PacketParser.Entities.Attendance;

public enum TypeTaradodInShiftEnum : short
{
	[PersianNameAttribute.PersianName("عادی")]
	Normal,
	[PersianNameAttribute.PersianName("نوبتکاری1")]
	ShiftWork1,
	[PersianNameAttribute.PersianName("نوبتکاری2")]
	ShiftWork2,
	[PersianNameAttribute.PersianName("نوبتکاری3")]
	ShiftWork3,
	[PersianNameAttribute.PersianName("شبکاری")]
	NightWork
}
