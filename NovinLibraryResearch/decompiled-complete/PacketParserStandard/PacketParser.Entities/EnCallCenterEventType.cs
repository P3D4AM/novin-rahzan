namespace PacketParser.Entities;

public enum EnCallCenterEventType : short
{
	[PersianNameAttribute.PersianName("در حال")]
	NewCall,
	[PersianNameAttribute.PersianName("یکپارچه")]
	CallFinilized,
	[PersianNameAttribute.PersianName("صیادی")]
	MemberQueueEnd,
	[PersianNameAttribute.PersianName("صیادی")]
	BridgeAgentFail,
	[PersianNameAttribute.PersianName("صیادی")]
	BridgeAgentEnd,
	[PersianNameAttribute.PersianName("صیادی")]
	MemberQueueStart,
	[PersianNameAttribute.PersianName("صیادی")]
	MemberCount,
	[PersianNameAttribute.PersianName("صیادی")]
	BridgeAgentStart
}
