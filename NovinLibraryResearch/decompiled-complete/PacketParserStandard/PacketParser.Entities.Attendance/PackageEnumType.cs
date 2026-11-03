namespace PacketParser.Entities.Attendance;

public enum PackageEnumType : short
{
	[PersianNameAttribute.PersianName("شیفتهای برنامه ریزی شده")]
	ProgrammableShift = -1,
	[PersianNameAttribute.PersianName("شیفت کاری")]
	TimeAttendance = 1,
	[PersianNameAttribute.PersianName("تمامی شیفت ها")]
	AllShifts = 20,
	[PersianNameAttribute.PersianName("تمامی پکیج ها")]
	AllPackages = 50,
	[PersianNameAttribute.PersianName("ماهانه")]
	Monthly = 51,
	[PersianNameAttribute.PersianName("جلسه ای")]
	Periodic = 52,
	[PersianNameAttribute.PersianName("همه")]
	All = 100
}
