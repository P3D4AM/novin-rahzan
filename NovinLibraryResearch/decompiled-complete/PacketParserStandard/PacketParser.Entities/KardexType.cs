namespace PacketParser.Entities;

public enum KardexType : short
{
	[PersianNameAttribute.PersianName("ریالی")]
	Dec = 1,
	[PersianNameAttribute.PersianName("تعدادی")]
	QT,
	[PersianNameAttribute.PersianName("ریالی و تعدادی")]
	Both,
	[PersianNameAttribute.PersianName("سود")]
	Sood
}
