using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class LotteryMapper : IItemAsyncMapper<LotteryBusiness, WebLottery>, IItemAsyncMapper<LotteryBusiness, LotteryBusiness>, IItemAsyncMapper<WebLottery, LotteryBusiness>
{
	public static LotteryMapper Instance { get; private set; } = new LotteryMapper();


	public Task<LotteryBusiness> MapAsync(string contextGuid, LotteryBusiness source, LotteryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LotteryBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.DateM = source.DateM;
			destination.Status = source.Status;
			destination.Description = source.Description;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\LotteryMapper.cs", 41);
			throw;
		}
	}

	public Task<LotteryBusiness> MapAsync(string contextGuid, WebLottery source, LotteryBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LotteryBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.DateM = source.DateM;
			destination.Status = source.Status;
			destination.Description = source.Description;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\LotteryMapper.cs", 69);
			throw;
		}
	}

	public Task<WebLottery> MapAsync(string contextGuid, LotteryBusiness source, WebLottery destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebLottery();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.DateM = source.DateM;
			destination.Status = source.Status;
			destination.Description = source.Description;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\LotteryMapper.cs", 95);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<LotteryBusiness>> MapListAsync(string contextGuid, IEnumerable<WebLottery> source, List<LotteryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LotteryBusiness>> MapListAsync(string contextGuid, IEnumerable<LotteryBusiness> source, List<LotteryBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebLottery>> MapListAsync(string contextGuid, IEnumerable<LotteryBusiness> source, List<WebLottery> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
