namespace PacketParser.Entities;

public enum EnDepotReceiptType : short
{
	[PersianNameAttribute.PersianName("رسید انبار")]
	Receipt = 1,
	[PersianNameAttribute.PersianName("حواله انبار")]
	Havale
}
