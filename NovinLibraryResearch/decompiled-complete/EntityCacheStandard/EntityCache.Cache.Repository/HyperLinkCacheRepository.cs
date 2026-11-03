using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class HyperLinkCacheRepository : CacheRepository<HyperLinkBusiness>, IHyperLinkRepository, IRepository<HyperLinkBusiness>, INoGuidRepository<HyperLinkBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, HyperLinkBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<HyperLinkBusiness, HyperLinkBusiness> Mapper => HyperLinkMapper.Instance;

	public HyperLinkCacheRepository(IRepository<HyperLinkBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, HyperLinkBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<HyperLinkBusiness>> GetByCategoryAsync(string Category, int top, bool? status, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IHyperLinkRepository)base.DbRepository).GetByCategoryAsync(Category, top, status, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\HyperLinkCacheRepository.cs", 42);
		}
	}
}
