using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Repository;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository;

public class PardakhtCheckMCacheRepository : MasterCacheRepository<PardakhtCheckMBusiness>, IPardakhtCheckMRepository, IPersistenceMasterRepository<PardakhtCheckMBusiness>, IDisposable
{
	private IPardakhtCheckMRepository dbRepository;

	public PardakhtCheckMCacheRepository(IPardakhtCheckMRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<PardakhtCheckMBusiness>)dbRepository, CachingTime)
	{
		this.dbRepository = dbRepository;
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMCacheRepository.cs", 29);
		}
	}

	public Task<IEnumerable<PardakhtCheckMBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(token, tranName);
	}

	public Task<IEnumerable<PardakhtCheckMBusiness>> GetAllWithChequeGuidAsync(Guid chequeGuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllWithChequeGuidAsync(chequeGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(IEnumerable<PardakhtCheckMBusiness> items, PardakhtBusiness masterItem, EnSystem system, short trycount, CancellationToken token, string tranName)
	{
		return dbRepository.SaveAsync(items, masterItem, system, trycount, token, tranName);
	}
}
