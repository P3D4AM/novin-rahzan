namespace PacketParser.Entities;

public enum EnBackUpType : short
{
	[PersianNameAttribute.PersianName("پشتیبان گیری")]
	BackUp,
	[PersianNameAttribute.PersianName("بازیابی پشتیبان")]
	Restore
}
