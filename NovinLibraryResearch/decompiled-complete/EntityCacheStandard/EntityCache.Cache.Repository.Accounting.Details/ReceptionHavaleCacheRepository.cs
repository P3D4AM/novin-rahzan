using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Details;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository.Accounting.Details;

public class ReceptionHavaleCacheRepository : MasterCacheRepository<ReceptionHavaleBusiness>, IReceptionHavaleRepository, IPersistenceMasterRepository<ReceptionHavaleBusiness>, IDisposable
{
	private IReceptionHavaleRepository _dbRepository;

	public ReceptionHavaleCacheRepository(IReceptionHavaleRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<ReceptionHavaleBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
	}

	public Task<bool> IsPeygiriNumberTekrariAsync(string number, Guid guid, Guid bankGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.IsPeygiriNumberTekrariAsync(number, guid, bankGuid, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Details\\ReceptionHavaleCacheRepository.cs", 35);
		}
	}

	public Task<List<Guid>> GetAllMasterGuidsForFixWageAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllMasterGuidsForFixWageAsync(token, tranName);
	}
}
