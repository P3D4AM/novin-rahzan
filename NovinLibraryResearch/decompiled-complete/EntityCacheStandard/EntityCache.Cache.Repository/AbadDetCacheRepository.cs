using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository;

public class AbadDetCacheRepository : MasterCacheRepository<AbadDetBusiness>, IAbadDetRepository, IPersistenceMasterRepository<AbadDetBusiness>, IDisposable
{
	private IAbadDetRepository dbRepository;

	public new bool IsDisposed { get; private set; }

	public AbadDetCacheRepository(IAbadDetRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<AbadDetBusiness>)dbRepository, CachingTime)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\AbadDetCacheRepository.cs", 30);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(List<AbadDetBusiness> items, Guid MasterGuid, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}
}
