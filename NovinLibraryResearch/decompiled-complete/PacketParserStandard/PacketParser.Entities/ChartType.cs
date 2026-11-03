namespace PacketParser.Entities;

public enum ChartType : short
{
	[PersianNameAttribute.PersianName("نقطه ای")]
	FastPoint = 1,
	[PersianNameAttribute.PersianName("حبابی")]
	Bubble,
	[PersianNameAttribute.PersianName("خطی")]
	Spline,
	[PersianNameAttribute.PersianName("ستونی")]
	StackedBar,
	[PersianNameAttribute.PersianName("میله ای")]
	Column,
	[PersianNameAttribute.PersianName("دایره ای")]
	Pie,
	[PersianNameAttribute.PersianName("هرمی")]
	Pyramid
}
