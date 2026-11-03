using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class WageMapper : IItemAsyncMapper<WageBusiness, WageBusiness>, IItemAsyncMapper<WebWage, WageBusiness>
{
	public static WageMapper Instance { get; set; } = new WageMapper();


	public Task<WageBusiness> MapAsync(string contextGuid, WageBusiness source, WageBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WageBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Type = source.Type;
			destination.CalculationMethod = source.CalculationMethod;
			destination.PriceOrPercent = source.PriceOrPercent;
			destination.MinTransaction = source.MinTransaction;
			destination.MaxTransaction = source.MaxTransaction;
			destination.MasterType = source.MasterType;
			destination.ServerGuid = source.ServerGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\WageMapper.cs", 39);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WageBusiness> MapAsync(string contextGuid, WebWage source, WageBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WageBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Type = source.Type;
			destination.CalculationMethod = source.CalculationMethod;
			destination.PriceOrPercent = source.PriceOrPercent;
			destination.MinTransaction = source.MinTransaction;
			destination.MaxTransaction = source.MaxTransaction;
			destination.MasterType = source.MasterType;
			destination.ServerGuid = source.ServerGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\WageMapper.cs", 69);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WageBusiness>> MapListAsync(string contextGuid, IEnumerable<WageBusiness> source, List<WageBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WageBusiness>> MapListAsync(string contextGuid, IEnumerable<WebWage> source, List<WageBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
