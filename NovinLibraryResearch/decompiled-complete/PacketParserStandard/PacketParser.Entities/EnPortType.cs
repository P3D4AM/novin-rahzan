namespace PacketParser.Entities;

public enum EnPortType : short
{
	[PersianNameAttribute.PersianName("سرویس")]
	Service = 1,
	[PersianNameAttribute.PersianName("کارتخوان")]
	PcPoss,
	[PersianNameAttribute.PersianName("ترازو")]
	Scale
}
