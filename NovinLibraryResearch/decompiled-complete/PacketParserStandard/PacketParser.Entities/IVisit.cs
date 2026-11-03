using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IVisit : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	DateTime ScheduledDate { get; set; }

	DateTime StartDate { get; set; }

	DateTime CreationdDate { get; set; }

	string Description { get; set; }

	int VisitNo { get; set; }

	bool VisitIsCompleted { get; set; }
}
