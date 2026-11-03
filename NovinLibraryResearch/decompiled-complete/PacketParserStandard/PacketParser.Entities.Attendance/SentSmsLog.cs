using System;
using System.Collections.Generic;
using System.Linq;

namespace PacketParser.Entities.Attendance;

public class SentSmsLog
{
	public List<SentSmsLogDet> Persons { get; set; } = new List<SentSmsLogDet>();


	public string DateSh { get; set; } = "";


	public DateTime LastUpdate { get; set; } = DateTime.Now;


	public SentSmsLogDet GetByPersonId(int personId)
	{
		return Persons?.SingleOrDefault((SentSmsLogDet p) => p.PersonId == personId);
	}

	public bool ExitsForPersonId(int personId)
	{
		return Persons?.Any((SentSmsLogDet p) => p.PersonId == personId) ?? false;
	}
}
