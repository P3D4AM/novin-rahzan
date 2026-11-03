using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global.Tasks;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global.Tasks;

public class TaskMapper : IItemAsyncMapper<TaskBusiness, TaskBusiness>
{
	public static TaskMapper Instance { get; private set; } = new TaskMapper();


	public async Task<TaskBusiness> MapAsync(string contextGuid, TaskBusiness source, TaskBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TaskBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Title = source.Title;
			destination.DateM = source.DateM;
			destination.CashierGuid = source.CashierGuid;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.StatusId = source.StatusId;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.MetaData = source.MetaData;
			destination.Id2 = source.Id2;
			if (source.CashierList?.Any() ?? false)
			{
				foreach (TaskCashierRelationBusiness item in await TaskCashierRelationMapper.Instance.MapListAsync(contextGuid, source.CashierList, new List<TaskCashierRelationBusiness>(), tranName, token))
				{
					await destination.AddDetailCashierRelationListAsync(item);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\Tasks\\Tasks\\TaskMapper.cs", 52);
			throw;
		}
	}

	public Task<List<TaskBusiness>> MapListAsync(string contextGuid, IEnumerable<TaskBusiness> source, List<TaskBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
