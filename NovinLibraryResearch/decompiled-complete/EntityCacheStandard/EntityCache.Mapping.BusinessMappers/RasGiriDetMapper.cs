using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class RasGiriDetMapper : IItemAsyncMapper<RasGiriDetBusiness, WebRasGiriDet>, IItemAsyncMapper<RasGiriDetBusiness, RasGiriDetBusiness>, IItemAsyncMapper<WebRasGiriDet, RasGiriDetBusiness>
{
	public static RasGiriDetMapper Instance { get; private set; } = new RasGiriDetMapper();


	public Task<WebRasGiriDet> MapAsync(string contextGuid, RasGiriDetBusiness source, WebRasGiriDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebRasGiriDet();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Price = source.Price;
			destination.Enheraf = source.Enheraf;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\RasGiriDetMapper.cs", 36);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebRasGiriDet>> MapListAsync(string contextGuid, IEnumerable<RasGiriDetBusiness> source, List<WebRasGiriDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<RasGiriDetBusiness> MapAsync(string contextGuid, RasGiriDetBusiness source, RasGiriDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RasGiriDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Price = source.Price;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\RasGiriDetMapper.cs", 65);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<RasGiriDetBusiness>> MapListAsync(string contextGuid, IEnumerable<RasGiriDetBusiness> source, List<RasGiriDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<RasGiriDetBusiness> MapAsync(string contextGuid, WebRasGiriDet source, RasGiriDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new RasGiriDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Price = source.Price;
			destination.Description = source.Description;
			destination.DateM = source.DateM;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\RasGiriDetMapper.cs", 94);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<RasGiriDetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebRasGiriDet> source, List<RasGiriDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
