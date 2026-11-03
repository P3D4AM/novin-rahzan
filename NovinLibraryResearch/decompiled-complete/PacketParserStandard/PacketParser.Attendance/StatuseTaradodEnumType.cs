using PacketParser.Assistance.MultiLingual;

namespace PacketParser.Attendance;

public enum StatuseTaradodEnumType : short
{
	[PersianNameAttribute.PersianName("هیچ کدام")]
	[MultiLangaugeAttribute.KeyName("[None]")]
	None = 0,
	[PersianNameAttribute.PersianName("ورود و خروج کامل")]
	[MultiLangaugeAttribute.KeyName("[VOKHKamel]")]
	VOKHKamel = 1,
	[PersianNameAttribute.PersianName("ورود و خروج ناقص")]
	[MultiLangaugeAttribute.KeyName("[VOKHNaghes]")]
	VOKHNaghes = 2,
	[PersianNameAttribute.PersianName("غیبت")]
	[MultiLangaugeAttribute.KeyName("[Absence_Day]")]
	Gheybat = 3,
	[PersianNameAttribute.PersianName("عدم قرارداد")]
	[MultiLangaugeAttribute.KeyName("[بدون قرارداد]")]
	AdamGharardad = 4,
	[PersianNameAttribute.PersianName("مرخصی روزانه")]
	[MultiLangaugeAttribute.KeyName("[Vacation_D]")]
	Morakhsi = 5,
	[PersianNameAttribute.PersianName("تعطیل رسمی")]
	[MultiLangaugeAttribute.KeyName("[تعطیلی رسمی]")]
	TatilRasmi = 6,
	[PersianNameAttribute.PersianName("تعطیل")]
	[MultiLangaugeAttribute.KeyName("[تعطیل]")]
	Tatil = 7,
	[PersianNameAttribute.PersianName("عدم شیفت")]
	[MultiLangaugeAttribute.KeyName("[AdamShift]")]
	AdamShift = 8,
	[PersianNameAttribute.PersianName("مرخص ساعتی")]
	[MultiLangaugeAttribute.KeyName("[Vacation_H]")]
	MorakhasiSaati = 9,
	[PersianNameAttribute.PersianName("استراحت")]
	[MultiLangaugeAttribute.KeyName("[استراحت]")]
	Surcease = 11,
	[PersianNameAttribute.PersianName("تعطیل رسمی و غیبت")]
	[MultiLangaugeAttribute.KeyName("[تعطیل رسمی و غیبت]")]
	TatilRasmiAndGheybat = 12,
	[PersianNameAttribute.PersianName("تعطیل و غیبت")]
	[MultiLangaugeAttribute.KeyName("[تعطیل و غیبت]")]
	TatilAndGheybat = 13,
	[PersianNameAttribute.PersianName("ماموریت روزانه")]
	[MultiLangaugeAttribute.KeyName("[Mission]")]
	Mission = 14,
	[PersianNameAttribute.PersianName("تعجیل در خروج")]
	[MultiLangaugeAttribute.KeyName("[ExitHurry]")]
	ExitHurry = 15,
	[PersianNameAttribute.PersianName("تاخیر در ورود")]
	[MultiLangaugeAttribute.KeyName("[ArrivalDelay]")]
	ArrivalDelay = 16,
	[PersianNameAttribute.PersianName("ماموریت ساعتی")]
	[MultiLangaugeAttribute.KeyName("[Mission_H]")]
	HourlyMission = 17,
	[PersianNameAttribute.PersianName("ترک کار")]
	[MultiLangaugeAttribute.KeyName("[LeavingWork]")]
	LeavingWork = 18,
	[PersianNameAttribute.PersianName("تعطیل رسمی و ترک کار")]
	[MultiLangaugeAttribute.KeyName("[تعطیل رسمی و ترک کار]")]
	TatilRasmiAndLeavingWork = 19,
	[PersianNameAttribute.PersianName("تعطیل و ترک کار")]
	[MultiLangaugeAttribute.KeyName("[تعطیل و ترک کار]")]
	TatilAndLeavingWork = 20,
	[PersianNameAttribute.PersianName("تعطیل رسمی و ورود و خروج کامل")]
	[MultiLangaugeAttribute.KeyName("[تعطیلی رسمی],[VOKHKamel]")]
	TatilRasmiAndVOKHKamel = 21,
	[PersianNameAttribute.PersianName("حضور سررسید نشده")]
	[MultiLangaugeAttribute.KeyName("---")]
	NotArrived = 22
}
