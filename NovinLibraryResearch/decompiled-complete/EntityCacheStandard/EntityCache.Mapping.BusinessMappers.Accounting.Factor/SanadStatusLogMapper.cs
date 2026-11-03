using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Factor;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Factor;

public class SanadStatusLogMapper : IItemAsyncMapper<SanadStatusLogBusiness, SanadStatusLogBusiness>
{
	public static SanadStatusLogMapper Instance { get; private set; } = new SanadStatusLogMapper();


	public Task<SanadStatusLogBusiness> MapAsync(string contextGuid, SanadStatusLogBusiness source, SanadStatusLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.Guid = source.Guid;
			destination.AsGuid = source.AsGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.AsType = source.AsType;
			destination.NewStatus = source.NewStatus;
			destination.DateM = source.DateM;
			destination.Ordinal = source.Ordinal;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadStatusLogMapper.cs", 34);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<SanadStatusLogBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadStatusLogBusiness> source, List<SanadStatusLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
