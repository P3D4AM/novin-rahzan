using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global.Tasks;

public class TaskCashierRelationMapper : IItemAsyncMapper<TaskCashierRelationBusiness, TaskCashierRelationBusiness>
{
	public static TaskCashierRelationMapper Instance { get; private set; } = new TaskCashierRelationMapper();


	public Task<TaskCashierRelationBusiness> MapAsync(string contextGuid, TaskCashierRelationBusiness source, TaskCashierRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new TaskCashierRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.TaskGuid = source.TaskGuid;
			destination.Type = source.Type;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\Tasks\\TaskCashierRelationMapper.cs", 33);
			throw;
		}
	}

	public Task<List<TaskCashierRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<TaskCashierRelationBusiness> source, List<TaskCashierRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
