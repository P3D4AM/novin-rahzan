namespace PacketParser.Entities.Attendance;

public enum TypeShiftEnumType : short
{
	[PersianNameAttribute.PersianName("")]
	None = 0,
	[PersianNameAttribute.PersianName("شیفت عادی")]
	Normal = 1,
	[PersianNameAttribute.PersianName("شیفت شناور")]
	Shenavar = 2,
	[PersianNameAttribute.PersianName("شیفت چرخشی 24-48")]
	Circle24_48 = 3,
	[PersianNameAttribute.PersianName("شیفت چرخشی 12_24")]
	Circle12_24 = 4,
	[PersianNameAttribute.PersianName("شیفت چرخشی 25 روزه")]
	Circle25Roze = 5,
	[PersianNameAttribute.PersianName("راهنمای شیفت چرخشی")]
	HelpCircle = 6,
	[PersianNameAttribute.PersianName("شیفت استراحت")]
	Surcease = 11,
	[PersianNameAttribute.PersianName("شیفت چرخشی")]
	AllCircle = 100
}
