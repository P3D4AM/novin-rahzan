namespace PacketParser.Entities.Attendance;

public enum SavedTaradodEnumType : short
{
	[PersianNameAttribute.PersianName("تردد نامشخص")]
	None,
	[PersianNameAttribute.PersianName("تردد ثبت شده واقعی")]
	RealTaradod,
	[PersianNameAttribute.PersianName("تردد ویرایش شده دستی")]
	EditTaradod,
	[PersianNameAttribute.PersianName("تردد ثبت شده شیفت شب")]
	FromSpLogonPersonelCalc,
	[PersianNameAttribute.PersianName("تردد مجازی")]
	VirtualTaradod,
	[PersianNameAttribute.PersianName("تردد اضافه شده دستی")]
	AddTaradod,
	[PersianNameAttribute.PersianName("تردد انتقالی")]
	EnteghaliTaradod
}
