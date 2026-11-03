using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PardakhtNaghdMapper : IItemAsyncMapper<PardakhtNaghdBusiness, WebPardakhtNaghd>, IItemAsyncMapper<PardakhtNaghdBusiness, PardakhtNaghdBusiness>, IItemAsyncMapper<WebPardakhtNaghd, PardakhtNaghdBusiness>
{
	public static PardakhtNaghdMapper Instance { get; private set; } = new PardakhtNaghdMapper();


	public Task<WebPardakhtNaghd> MapAsync(string contextGuid, PardakhtNaghdBusiness source, WebPardakhtNaghd destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPardakhtNaghd();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.RowDesc;
			destination.Price = source.Price;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.Ordinal = source.Ordinal;
			destination.DateM = source.DateM;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdMapper.cs", 40);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebPardakhtNaghd>> MapListAsync(string contextGuid, IEnumerable<PardakhtNaghdBusiness> source, List<WebPardakhtNaghd> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtNaghdBusiness> MapAsync(string contextGuid, PardakhtNaghdBusiness source, PardakhtNaghdBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtNaghdBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.RowDesc = source.RowDesc;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateM = source.DateM;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdMapper.cs", 73);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PardakhtNaghdBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtNaghdBusiness> source, List<PardakhtNaghdBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtNaghdBusiness> MapAsync(string contextGuid, WebPardakhtNaghd source, PardakhtNaghdBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtNaghdBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.RowDesc = source.Description;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateUtc = source.DateM.ToUniversalTime();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdMapper.cs", 105);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PardakhtNaghdBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPardakhtNaghd> source, List<PardakhtNaghdBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
