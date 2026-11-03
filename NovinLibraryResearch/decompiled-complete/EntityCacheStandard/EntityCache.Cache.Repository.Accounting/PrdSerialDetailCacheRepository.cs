using System;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository.Accounting;

internal class PrdSerialDetailCacheRepository : MasterCacheRepository<PrdSerialDetailBusiness>, IPrdSerialDetailRepository, IPersistenceMasterRepository<PrdSerialDetailBusiness>, IDisposable
{
	private IPrdSerialDetailRepository dbRepository;

	public PrdSerialDetailCacheRepository(IPrdSerialDetailRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<PrdSerialDetailBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\PrdSerialDetailCacheRepository.cs", 25);
		}
	}
}
