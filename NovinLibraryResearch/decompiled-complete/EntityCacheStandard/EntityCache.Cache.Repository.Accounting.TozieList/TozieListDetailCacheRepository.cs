using System;
using EntityCache.Business.Accounting.TozieList;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TozieList;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository.Accounting.TozieList;

public class TozieListDetailCacheRepository : MasterCacheRepository<TozieListDetailBusiness>, ITozieListDetailRepository, IPersistenceMasterRepository<TozieListDetailBusiness>, IDisposable
{
	private ITozieListDetailRepository _dbRepository;

	public TozieListDetailCacheRepository(ITozieListDetailRepository dbRepository, int CachingTime)
		: base((IPersistenceMasterRepository<TozieListDetailBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\TozieList\\TozieListDetailCacheRepository.cs", 24);
		}
	}
}
