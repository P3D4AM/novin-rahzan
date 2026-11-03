namespace PacketParser.Entities;

public enum EnProductIndex : short
{
	AllFields = 1,
	Code = 2,
	Description = 4,
	NameAndCode = 8,
	Name = 0x10,
	T1 = 0x20,
	T2 = 0x40,
	T3 = 0x80,
	T4 = 0x100,
	T5 = 0x200,
	T6 = 0x400,
	T7 = 0x800,
	T8 = 0x1000
}
