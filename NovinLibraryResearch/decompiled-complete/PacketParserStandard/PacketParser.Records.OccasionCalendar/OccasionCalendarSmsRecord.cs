using System;

namespace PacketParser.Records.OccasionCalendar;

public record OccasionCalendarSmsRecord
{
	public Guid ParentGuid { get; set; }

	public string Text { get; set; }

	public long Number { get; set; }

	public string Title { get; set; }
}
