using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class DecimalUpdatedMapper : IItemAsyncMapper<DecimalUpdatesBusiness, WebDecimalUpdates>, IItemAsyncMapper<DecimalUpdatesBusiness, DecimalUpdatesBusiness>, IItemAsyncMapper<WebDecimalUpdates, DecimalUpdatesBusiness>
{
	public static DecimalUpdatedMapper Instance { get; private set; } = new DecimalUpdatedMapper();


	public Task<DecimalUpdatesBusiness> MapAsync(string contextGuid, WebDecimalUpdates source, DecimalUpdatesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DecimalUpdatesBusiness(contextGuid);
			}
			destination.DecimalGuid = source.DecimalGuid;
			destination.DateM = source.DateM;
			destination.Price = source.Price;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalUpdatedMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<DecimalUpdatesBusiness> MapAsync(string contextGuid, DecimalUpdatesBusiness source, DecimalUpdatesBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DecimalUpdatesBusiness(contextGuid);
			}
			destination.DecimalGuid = source.DecimalGuid;
			destination.DateM = source.DateM;
			destination.Price = source.Price;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalUpdatedMapper.cs", 64);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebDecimalUpdates> MapAsync(string contextGuid, DecimalUpdatesBusiness source, WebDecimalUpdates destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDecimalUpdates();
			}
			destination.DecimalGuid = source.DecimalGuid;
			destination.DateM = source.DateM;
			destination.Price = source.Price;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalUpdatedMapper.cs", 88);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<DecimalUpdatesBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDecimalUpdates> source, List<DecimalUpdatesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DecimalUpdatesBusiness>> MapListAsync(string contextGuid, IEnumerable<DecimalUpdatesBusiness> source, List<DecimalUpdatesBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDecimalUpdates>> MapListAsync(string contextGuid, IEnumerable<DecimalUpdatesBusiness> source, List<WebDecimalUpdates> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
