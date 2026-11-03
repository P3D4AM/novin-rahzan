using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Factor;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Factor;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Factor;

internal class mapper : IItemAsyncMapper<SanadStatusLogBusiness, SanadStatusLog>, IItemAsyncMapper<SanadStatusLog, SanadStatusLogBusiness>
{
	public Task<SanadStatusLogBusiness> MapAsync(string contextGuid, SanadStatusLog source, SanadStatusLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadStatusLogBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
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
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusLogPersistenceRepository.cs", 66);
		}
		return Task.FromResult(destination);
	}

	public Task<SanadStatusLog> MapAsync(string contextGuid, SanadStatusLogBusiness source, SanadStatusLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Sanad\\SanadStatusLogPersistenceRepository.cs", 86);
		}
		return Task.FromResult(destination);
	}

	public Task<List<SanadStatusLogBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadStatusLog> source, List<SanadStatusLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadStatusLog>> MapListAsync(string contextGuid, IEnumerable<SanadStatusLogBusiness> source, List<SanadStatusLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
