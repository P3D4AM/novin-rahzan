namespace PacketParser.Entities;

public enum OrderMode : short
{
	[PersianNameAttribute.PersianName("Insert")]
	Insert,
	[PersianNameAttribute.PersianName("Update")]
	Update,
	[PersianNameAttribute.PersianName("Delete")]
	Delete
}
