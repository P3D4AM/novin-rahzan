using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ReceptionNaghdMapper : IItemAsyncMapper<ReceptionNaghdBusiness, WebReceptionNaghd>, IItemAsyncMapper<ReceptionNaghdBusiness, ReceptionNaghdBusiness>, IItemAsyncMapper<WebReceptionNaghd, ReceptionNaghdBusiness>
{
	public static ReceptionNaghdMapper Instance { get; private set; } = new ReceptionNaghdMapper();


	public Task<WebReceptionNaghd> MapAsync(string contextGuid, ReceptionNaghdBusiness source, WebReceptionNaghd destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReceptionNaghd();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateUtc = source.DateUtc;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.Price = source.Price;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionNaghdMapper.cs", 43);
			throw;
		}
	}

	public Task<List<WebReceptionNaghd>> MapListAsync(string contextGuid, IEnumerable<ReceptionNaghdBusiness> source, List<WebReceptionNaghd> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionNaghdBusiness> MapAsync(string contextGuid, ReceptionNaghdBusiness source, ReceptionNaghdBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionNaghdBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.DateUtc = source.DateUtc;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionNaghdMapper.cs", 75);
			throw;
		}
	}

	public Task<List<ReceptionNaghdBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionNaghdBusiness> source, List<ReceptionNaghdBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionNaghdBusiness> MapAsync(string contextGuid, WebReceptionNaghd source, ReceptionNaghdBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionNaghdBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionNaghdMapper.cs", 107);
			throw;
		}
	}

	public Task<List<ReceptionNaghdBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReceptionNaghd> source, List<ReceptionNaghdBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
