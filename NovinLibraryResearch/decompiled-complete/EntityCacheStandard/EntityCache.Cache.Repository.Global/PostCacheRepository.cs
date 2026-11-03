using System;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Mapping.Global;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class PostCacheRepository : CacheRepository<PostBusiness>, IPostRepository, IRepository<PostBusiness>, INoGuidRepository<PostBusiness>, IDisposable
{
	private IPostRepository _dbRepository;

	public override IItemAsyncMapper<PostBusiness, PostBusiness> Mapper => PostMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, PostBusiness> TimingCache { get; set; }

	public PostCacheRepository(IRepository<PostBusiness> dbRepository, int CachingTime, bool cachingEnable = true)
		: base(dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = (IPostRepository)dbRepository;
		TimingCache = new TimingCache<Guid, PostBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
