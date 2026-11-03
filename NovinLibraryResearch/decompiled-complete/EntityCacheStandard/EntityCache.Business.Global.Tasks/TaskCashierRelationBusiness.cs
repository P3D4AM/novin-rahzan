using System;
using System.Threading;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global.Tasks;

namespace EntityCache.Business.Global.Tasks;

public class TaskCashierRelationBusiness : Serializable<TaskCashierRelationBusiness>, ITaskCashierRelation, IHasContext
{
	private SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public string ContextGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid TaskGuid { get; set; }

	public short Type { get; set; }

	public TaskCashierRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
