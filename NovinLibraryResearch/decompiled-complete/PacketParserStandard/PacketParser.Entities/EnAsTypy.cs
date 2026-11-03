namespace PacketParser.Entities;

public enum EnAsTypy : short
{
	[PersianNameAttribute.PersianName("پذیرش")]
	Recipt,
	[PersianNameAttribute.PersianName("برگه تعمیرات")]
	RepairSheet,
	[PersianNameAttribute.PersianName("رسید")]
	PaperArrived
}
