using System;
using PacketParser.BusinessLayer.Models;

namespace PacketParser.Records.OccasionCalendar;

public record OccasionCalendarRecord
{
	public Guid ParentGuid { get; set; }

	public OccasionCalendarModel Model { get; set; }
}
