using System;

namespace PacketParser.Entities.Interfaces.Global.Tasks;

public interface ITaskCashierRelation
{
	Guid CashierGuid { get; set; }

	Guid TaskGuid { get; set; }

	short Type { get; set; }
}
