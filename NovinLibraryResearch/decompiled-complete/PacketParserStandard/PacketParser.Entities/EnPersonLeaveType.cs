namespace PacketParser.Entities;

public enum EnPersonLeaveType : short
{
	[PersianNameAttribute.PersianName("ساعتی")]
	Hourly,
	[PersianNameAttribute.PersianName("روزانه")]
	Daily
}
