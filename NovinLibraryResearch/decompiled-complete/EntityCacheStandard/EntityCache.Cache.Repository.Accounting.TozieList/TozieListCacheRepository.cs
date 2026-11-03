using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.TozieList;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.TozieList;
using EntityCache.Mapping.BusinessMappers.Accounting.TozieList;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.TozieList;

public class TozieListCacheRepository : CacheRepository<TozieListBusiness>, ITozieListRepository, IRepository<TozieListBusiness>, INoGuidRepository<TozieListBusiness>, IDisposable
{
	private ITozieListRepository _dbRepository;

	public override IItemAsyncMapper<TozieListBusiness, TozieListBusiness> Mapper => TozieListMapper.Instance;

	public override bool HasStatus
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override bool HasModified
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	protected override TimingCache<Guid, TozieListBusiness> TimingCache { get; set; }

	public TozieListCacheRepository(ITozieListRepository dbRepository, int CachingTime)
		: base((IRepository<TozieListBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TozieListBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\TozieList\\TozieListCacheRepository.cs", 36);
		}
	}

	public Task<IEnumerable<TozieListRepostBusiness>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllReportAsync(token, tranName);
	}

	public Task<int> GetNextNumberAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextNumberAsync(token, tranName);
	}
}
