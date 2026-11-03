namespace PacketParser.Entities;

public enum Level_Hozor : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("حضور-غیاب و حقوق - دستمزد سطح 1")]
	Level1,
	[PersianNameAttribute.PersianName("حضور-غیاب و حقوق - دستمزد سطح 2")]
	Level2,
	[PersianNameAttribute.PersianName("حضور-غیاب و حقوق - دستمزد سطح 3")]
	Level3,
	[PersianNameAttribute.PersianName("حضور-غیاب و حقوق - دستمزد سطح 4")]
	Level4,
	[PersianNameAttribute.PersianName("حضور-غیاب و حقوق - دستمزد سطح 5")]
	Level5
}
