namespace PacketParser.Entities;

public enum EnPersonType
{
	[PersianNameAttribute.PersianName("شخص")]
	Customer = 0,
	[PersianNameAttribute.PersianName("بازاریاب")]
	Visitor = 1,
	[PersianNameAttribute.PersianName("سوپروایزر")]
	Supervisor = 2,
	[PersianNameAttribute.PersianName("None")]
	None = 100
}
