using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidHazineMapper : IItemAsyncMapper<TolidFormulaHazineBusiness, TolidHazineBusiness>, IItemAsyncMapper<TolidHazineBusiness, TolidHazineBusiness>
{
	public static TolidHazineMapper Instance { get; private set; } = new TolidHazineMapper();


	public Task<TolidHazineBusiness> MapAsync(string contextGuid, TolidFormulaHazineBusiness source, TolidHazineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidHazineBusiness(contextGuid);
			}
			destination.Guid = Guid.NewGuid();
			destination.Modified = DateTime.Now;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Price = source.Price;
			destination.Description = source.Description;
			destination.Type = source.Type;
			destination.ContextGuid = contextGuid;
			destination.TolidFormulaGuid = source.MasterGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidHazineMapper.cs", 40);
			throw;
		}
	}

	public Task<TolidHazineBusiness> MapAsync(string contextGuid, TolidHazineBusiness source, TolidHazineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidHazineBusiness(contextGuid);
			}
			destination.Guid = Guid.NewGuid();
			destination.Modified = DateTime.Now;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Price = source.Price;
			destination.Description = source.Description;
			destination.Type = source.Type;
			destination.ContextGuid = contextGuid;
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			destination.TolidFormulaIndex = source.TolidFormulaIndex;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidHazineMapper.cs", 69);
			throw;
		}
	}

	public Task<List<TolidHazineBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaHazineBusiness> source, List<TolidHazineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidHazineBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidHazineBusiness> source, List<TolidHazineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
