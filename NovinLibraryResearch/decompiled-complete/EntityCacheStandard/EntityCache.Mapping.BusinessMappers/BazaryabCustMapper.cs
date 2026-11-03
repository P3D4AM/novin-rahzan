using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class BazaryabCustMapper : IItemAsyncMapper<BazaryabCustBusiness, WebBazaryabCust>, IItemAsyncMapper<BazaryabCustBusiness, BazaryabCustBusiness>, IItemAsyncMapper<WebBazaryabCust, BazaryabCustBusiness>
{
	public static BazaryabCustMapper Instance { get; private set; } = new BazaryabCustMapper();


	public Task<WebBazaryabCust> MapAsync(string contextGuid, BazaryabCustBusiness source, WebBazaryabCust destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		throw new NotImplementedException();
	}

	public Task<List<WebBazaryabCust>> MapListAsync(string contextGuid, IEnumerable<BazaryabCustBusiness> source, List<WebBazaryabCust> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BazaryabCustBusiness> MapAsync(string contextGuid, BazaryabCustBusiness source, BazaryabCustBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BazaryabCustBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.BazaryabGuid = source.BazaryabGuid;
			destination.CustGuid = source.CustGuid;
			destination.Guid = source.Guid;
			destination.Description = source.Description;
			destination.MantagheGuid = source.MantagheGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\BazaryabCustMapper.cs", 44);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BazaryabCustBusiness>> MapListAsync(string contextGuid, IEnumerable<BazaryabCustBusiness> source, List<BazaryabCustBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BazaryabCustBusiness> MapAsync(string contextGuid, WebBazaryabCust source, BazaryabCustBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BazaryabCustBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.BazaryabGuid = source.BazaryabGuid;
			destination.CustGuid = source.CustGuid;
			destination.Guid = source.Guid;
			destination.Description = source.Description;
			destination.MantagheGuid = source.MantagheGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\BazaryabCustMapper.cs", 72);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<BazaryabCustBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBazaryabCust> source, List<BazaryabCustBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
