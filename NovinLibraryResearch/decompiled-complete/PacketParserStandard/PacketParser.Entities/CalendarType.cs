using System.ComponentModel;

namespace PacketParser.Entities;

public enum CalendarType
{
	[Description("هجری شمسی")]
	PersianCalendar,
	[Description("هجری قمری")]
	HijriCalendar,
	[Description("میلادی")]
	GregorianCalendar
}
