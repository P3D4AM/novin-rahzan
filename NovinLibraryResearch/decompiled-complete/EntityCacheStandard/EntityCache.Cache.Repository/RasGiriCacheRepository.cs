using System;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class RasGiriCacheRepository : CacheRepository<RasGiriBusiness>, IRasGiriRepository, IRepository<RasGiriBusiness>, INoGuidRepository<RasGiriBusiness>, IDisposable
{
	private new IRasGiriRepository DbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, RasGiriBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<RasGiriBusiness, RasGiriBusiness> Mapper => RasGiriMapper.Instance;

	public RasGiriCacheRepository(IRasGiriRepository dbRepository, int CachingTime)
		: base((IRepository<RasGiriBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, RasGiriBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\RasGiriCacheRepository.cs", 36);
		}
	}
}
