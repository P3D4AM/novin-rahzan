using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Sanad;
using EntityCache.Mapping.BusinessMappers.Accounting.Sanad;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Sanad;

public class ViewSanadDetailCoverCacheRepository : CacheRepository<ViewSanadDetailCoverBusiness>, IViewSanadDetailCoverRepository, IRepository<ViewSanadDetailCoverBusiness>, INoGuidRepository<ViewSanadDetailCoverBusiness>, IDisposable
{
	private IViewSanadDetailCoverRepository _dbRepository;

	public override IItemAsyncMapper<ViewSanadDetailCoverBusiness, ViewSanadDetailCoverBusiness> Mapper => ViewSanadDetailCoverMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, ViewSanadDetailCoverBusiness> TimingCache { get; set; }

	public ViewSanadDetailCoverCacheRepository(IViewSanadDetailCoverRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<ViewSanadDetailCoverBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ViewSanadDetailCoverBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<ReturnedSaveFuncInfo> RemoveArticlesAsync(Guid asGuid, Document asType, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveArticlesAsync(asGuid, asType, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveArticlesAsync(IEnumerable<ViewSanadDetailCoverBusiness> lst, CancellationToken token, string tranName)
	{
		return _dbRepository.SaveArticlesAsync(lst, token, tranName);
	}

	public Task<IEnumerable<ViewSanadDetailCoverBusiness>> GetAllInvalidRelationsAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllInvalidRelationsAsync(token, tranName);
	}
}
