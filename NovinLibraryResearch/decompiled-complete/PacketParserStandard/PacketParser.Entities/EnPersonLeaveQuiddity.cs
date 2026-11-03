namespace PacketParser.Entities;

public enum EnPersonLeaveQuiddity : short
{
	[PersianNameAttribute.PersianName("مرخصی")]
	Leave,
	[PersianNameAttribute.PersianName("اضافه کار")]
	OverTime,
	[PersianNameAttribute.PersianName("ماموریت")]
	Mission
}
