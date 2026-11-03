using System;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace SharedAccEntityFramworkPersistence.Entities.Global.Tasks;

[Table("Global_TaskCashierRelation")]
public class TaskCashierRelation : ITaskCashierRelation
{
	public Guid CashierGuid { get; set; }

	public Guid TaskGuid { get; set; }

	public short Type { get; set; }

	public TaskClass Tasks { get; set; }
}
