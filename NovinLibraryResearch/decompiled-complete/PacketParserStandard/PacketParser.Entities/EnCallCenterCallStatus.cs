namespace PacketParser.Entities;

public enum EnCallCenterCallStatus : short
{
	[PersianNameAttribute.PersianName("در حال")]
	Pending,
	[PersianNameAttribute.PersianName("یکپارچه")]
	Completed,
	[PersianNameAttribute.PersianName("صیادی")]
	Canceled,
	[PersianNameAttribute.PersianName("صیادی")]
	Queuee,
	[PersianNameAttribute.PersianName("صیادی")]
	AgentAssigned,
	[PersianNameAttribute.PersianName("صیادی")]
	Bussy,
	[PersianNameAttribute.PersianName("صیادی")]
	Reject,
	[PersianNameAttribute.PersianName("صیادی")]
	Error
}
