namespace PacketParser.Entities.Attendance;

public enum TypeTaradodEnumType : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None = 1,
	[PersianNameAttribute.PersianName("ورود")]
	Vorod = 3,
	[PersianNameAttribute.PersianName("خروج")]
	Khoroj = 4,
	[PersianNameAttribute.PersianName("مرخصی")]
	Vacation = 5,
	[PersianNameAttribute.PersianName("مرخصی ساعتی")]
	VacationHourly = 39,
	[PersianNameAttribute.PersianName("ماموریت")]
	Mission = 7,
	[PersianNameAttribute.PersianName("جمعه کاری")]
	FriDayWork = 8,
	[PersianNameAttribute.PersianName("تعطیل کاری")]
	TatilWork = 28,
	[PersianNameAttribute.PersianName("نوبت کاری صبح")]
	ShiftWork1 = 91,
	[PersianNameAttribute.PersianName("نوبت کاری عصر")]
	ShiftWork2 = 92,
	[PersianNameAttribute.PersianName("نوبت کاری شب")]
	ShiftWork3 = 93,
	[PersianNameAttribute.PersianName("شب کاری")]
	NightWork = 10,
	[PersianNameAttribute.PersianName("استراحت")]
	Surcease = 11,
	[PersianNameAttribute.PersianName("عادی")]
	Normal = 34,
	[PersianNameAttribute.PersianName("تاخیر سرویس")]
	ServiceDelay = 35,
	[PersianNameAttribute.PersianName("سفارشی 1")]
	Custom1 = 36,
	[PersianNameAttribute.PersianName("سفارشی 2")]
	Custom2 = 37,
	[PersianNameAttribute.PersianName("شروع ناهار")]
	LunchStart = 38,
	[PersianNameAttribute.PersianName("ش اضافه کار")]
	OverTimeIn = 24,
	[PersianNameAttribute.PersianName("پ اضافه کار")]
	OverTimeOut = 25,
	[PersianNameAttribute.PersianName("بیرون رفتن")]
	BreakOn = 26,
	[PersianNameAttribute.PersianName("رفتن به عقب")]
	BreakOff = 27,
	[PersianNameAttribute.PersianName("ش شیفت")]
	ShiftIn = 40,
	[PersianNameAttribute.PersianName("پ شیفت")]
	ShiftOut = 41,
	[PersianNameAttribute.PersianName("مرخصی و شبکاری")]
	NightWorkAndVacation = 42,
	[PersianNameAttribute.PersianName("ورود")]
	OnDuty = 12,
	[PersianNameAttribute.PersianName("خروج")]
	OffDuty = 13,
	[PersianNameAttribute.PersianName("شروع ناهار")]
	LunchIn = 14,
	[PersianNameAttribute.PersianName("پایان ناهار")]
	LunchOut = 15,
	[PersianNameAttribute.PersianName("شروع اضافه کاری")]
	OTIn = 16,
	[PersianNameAttribute.PersianName("پایان اضافه کاری")]
	OTOut = 17,
	[PersianNameAttribute.PersianName("سفارشی 1")]
	TeaIn = 18,
	[PersianNameAttribute.PersianName("سفارشی 2")]
	TeaOut = 19,
	[PersianNameAttribute.PersianName("ماموریت")]
	PrayIn = 20,
	[PersianNameAttribute.PersianName("مرخصی")]
	PrayOut = 21
}
