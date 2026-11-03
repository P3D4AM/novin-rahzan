namespace PacketParser.Entities.Attendance;

public enum ShiftSeparationEnumType : short
{
	[PersianNameAttribute.PersianName("نوع ترددها تفکیک نشود")]
	NotSeparate,
	[PersianNameAttribute.PersianName("نفکیک نوع ترددها ")]
	Separate
}
