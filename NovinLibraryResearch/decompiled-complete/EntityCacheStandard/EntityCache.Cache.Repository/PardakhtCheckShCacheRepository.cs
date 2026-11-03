using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Repository;
using EntityCache.Business;
using EntityCache.Core;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Cache.Repository;

public class PardakhtCheckShCacheRepository : MasterCacheRepository<PardakhtCheckShBusiness>, IPardakhtCheckShRepository, IPersistenceMasterRepository<PardakhtCheckShBusiness>, IDisposable
{
	private IPardakhtCheckShRepository dbRepository;

	public PardakhtCheckShCacheRepository(IPardakhtCheckShRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<PardakhtCheckShBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
	}

	public Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(PardakhtChequeFilter filter, CancellationToken token, string tranNamer)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranNamer);
	}

	public Task<PardakhtCheckShBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(guid, token, tranName);
	}

	public Task<IEnumerable<BedehiTalabModel>> GetAllReportAsync(BedehiTalabReportFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllReportAsync(filter, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShCacheRepository.cs", 45);
		}
	}
}
