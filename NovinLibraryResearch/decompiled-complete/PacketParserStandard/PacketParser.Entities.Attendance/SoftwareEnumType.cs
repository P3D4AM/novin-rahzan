namespace PacketParser.Entities.Attendance;

public enum SoftwareEnumType : short
{
	[PersianNameAttribute.PersianName("نرم افزار حضور-غیاب و حقوق- دستمزد")]
	TimeAttendanceSoftware = 1,
	[PersianNameAttribute.PersianName("نرم افزار باشگاه")]
	SportClub
}
