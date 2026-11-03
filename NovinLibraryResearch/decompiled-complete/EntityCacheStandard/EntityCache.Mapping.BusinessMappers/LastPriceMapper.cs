using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class LastPriceMapper : IItemAsyncMapper<LastPriceBusiness, WebLastPrice>, IItemAsyncMapper<LastPriceBusiness, LastPriceBusiness>, IItemAsyncMapper<WebLastPrice, LastPriceBusiness>
{
	public static LastPriceMapper Instance { get; private set; } = new LastPriceMapper();


	public Task<WebLastPrice> MapAsync(string contextGuid, LastPriceBusiness source, WebLastPrice destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebLastPrice();
			}
			destination.Date = source.Date;
			destination.Price = source.Price;
			destination.TafsilGuid = source.TafsilGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.Invalidated = source.Invalidated;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Guid = source.Guid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\LastPriceMapper.cs", 38);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebLastPrice>> MapListAsync(string contextGuid, IEnumerable<LastPriceBusiness> source, List<WebLastPrice> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<LastPriceBusiness> MapAsync(string contextGuid, LastPriceBusiness source, LastPriceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LastPriceBusiness(contextGuid);
			}
			destination.Date = source.Date;
			destination.Price = source.Price;
			destination.TafsilGuid = source.TafsilGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.Invalidated = source.Invalidated;
			destination.ContextGuid = contextGuid;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Guid = source.Guid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\LastPriceMapper.cs", 68);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<LastPriceBusiness>> MapListAsync(string contextGuid, IEnumerable<LastPriceBusiness> source, List<LastPriceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<LastPriceBusiness> MapAsync(string contextGuid, WebLastPrice source, LastPriceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new LastPriceBusiness(contextGuid);
			}
			destination.Date = source.Date;
			destination.Price = source.Price;
			destination.TafsilGuid = source.TafsilGuid;
			destination.PrdGuid = source.PrdGuid;
			destination.Invalidated = source.Invalidated;
			destination.ContextGuid = contextGuid;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.Guid = source.Guid;
			destination.Status = source.Status;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\LastPriceMapper.cs", 98);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<LastPriceBusiness>> MapListAsync(string contextGuid, IEnumerable<WebLastPrice> source, List<LastPriceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
