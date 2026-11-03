using System;
using System.Collections.Generic;

namespace PacketParser.Entities.MetaData.Calendar;

public class CalendarSms
{
	public Guid Guid { get; set; }

	public DateTime TimeFrom { get; set; }

	public DateTime TimeTo { get; set; }

	public string SmsPattern { get; set; }

	public List<Guid> CustomerGuidList { get; set; }
}
