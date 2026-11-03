using System;

namespace PacketParser.Entities.Interfaces.Global.Tasks;

public interface ITask : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Title { get; set; }

	Guid CashierGuid { get; set; }

	DateTime DateM { get; set; }

	DateTime? StartDate { get; set; }

	DateTime? EndDate { get; set; }

	int StatusId { get; set; }

	string Description { get; set; }

	Guid? MasterGuid { get; set; }

	int Id2 { get; set; }
}
