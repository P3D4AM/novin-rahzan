namespace PacketParser.Entities;

public enum EnAttendancePrivilage : short
{
	[PersianNameAttribute.PersianName("کاربر")]
	Person,
	[PersianNameAttribute.PersianName("مدیر")]
	Manager
}
