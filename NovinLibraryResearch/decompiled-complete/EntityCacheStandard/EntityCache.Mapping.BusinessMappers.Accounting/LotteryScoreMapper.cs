using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class LotteryScoreMapper : IItemAsyncMapper<LotteryScoreBusiness, WebLotteryScore>, IItemAsyncMapper<LotteryScoreBusiness, LotteryScoreBusiness>, IItemAsyncMapper<WebLotteryScore, LotteryScoreBusiness>
{
	public static LotteryScoreMapper Instance { get; private set; } = new LotteryScoreMapper();


	public Task<WebLotteryScore> MapAsync(string contextGuid, LotteryScoreBusiness source, WebLotteryScore destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebLotteryScore();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.OrderGuid = source.OrderGuid;
			destination.DateM = source.DateM;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Score = source.Score;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.ExpirationDate = source.ExpirationDate;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\LotteryScoreMapper.cs", 42);
			throw;
		}
	}

	public Task<LotteryScoreBusiness> MapAsync(string contextGuid, LotteryScoreBusiness source, LotteryScoreBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LotteryScoreBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.OrderGuid = source.OrderGuid;
			destination.DateM = source.DateM;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Score = source.Score;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.ExpirationDate = source.ExpirationDate;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\LotteryScoreMapper.cs", 72);
			throw;
		}
	}

	public Task<LotteryScoreBusiness> MapAsync(string contextGuid, WebLotteryScore source, LotteryScoreBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LotteryScoreBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.OrderGuid = source.OrderGuid;
			destination.DateM = source.DateM;
			destination.TafsilGuid = source.TafsilGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.Score = source.Score;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.ExpirationDate = source.ExpirationDate;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\LotteryScoreMapper.cs", 102);
			throw;
		}
	}

	public Task<List<WebLotteryScore>> MapListAsync(string contextGuid, IEnumerable<LotteryScoreBusiness> source, List<WebLotteryScore> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LotteryScoreBusiness>> MapListAsync(string contextGuid, IEnumerable<LotteryScoreBusiness> source, List<LotteryScoreBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<LotteryScoreBusiness>> MapListAsync(string contextGuid, IEnumerable<WebLotteryScore> source, List<LotteryScoreBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
