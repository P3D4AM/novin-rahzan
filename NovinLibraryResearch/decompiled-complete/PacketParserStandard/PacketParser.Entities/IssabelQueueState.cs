namespace PacketParser.Entities;

public enum IssabelQueueState : short
{
	[PersianNameAttribute.PersianName("عادی")]
	Normal,
	[PersianNameAttribute.PersianName("هشدار")]
	Warning,
	[PersianNameAttribute.PersianName("خطا")]
	Error
}
