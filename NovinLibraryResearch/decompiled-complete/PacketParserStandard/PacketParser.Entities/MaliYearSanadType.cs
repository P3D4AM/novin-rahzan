namespace PacketParser.Entities;

public enum MaliYearSanadType : short
{
	[PersianNameAttribute.PersianName("ادواری")]
	Periodic,
	[PersianNameAttribute.PersianName("دائمی")]
	Permanent,
	[PersianNameAttribute.PersianName("Both")]
	Both
}
