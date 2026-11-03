using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

internal class ContentBlackListCacheRepository : CacheRepository<ContentBlackListBusiness>, IContentBlackListRepository, IRepository<ContentBlackListBusiness>, INoGuidRepository<ContentBlackListBusiness>, IDisposable
{
	private IContentBlackListRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ContentBlackListBusiness, ContentBlackListBusiness> Mapper => ContentBlackListMapper.Instance;

	protected override TimingCache<Guid, ContentBlackListBusiness> TimingCache { get; set; }

	internal ContentBlackListCacheRepository(IContentBlackListRepository dbRepository, int CachingTime)
		: base((IRepository<ContentBlackListBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ContentBlackListBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<List<ContentBlackListBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return _dbRepository.GetAllAsync(search, token, status, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\ContentBlackListCacheRepository.cs", 43);
		}
	}
}
