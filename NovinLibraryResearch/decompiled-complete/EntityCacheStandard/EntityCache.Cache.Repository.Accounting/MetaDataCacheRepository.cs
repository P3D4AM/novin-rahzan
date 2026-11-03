using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class MetaDataCacheRepository : CacheRepositoryNoGuiId<MetaDataBusiness, string>, IMetaDataRepository, INoGuidRepository<MetaDataBusiness>, IDisposable
{
	private IMetaDataRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<MetaDataBusiness, MetaDataBusiness> Mapper => MetaDataMapper.Instance;

	protected override TimingCache<string, MetaDataBusiness> TimingCache { get; set; }

	public MetaDataCacheRepository(INoGuidRepository<MetaDataBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = (IMetaDataRepository)dbRepository;
		TimingCache = new TimingCache<string, MetaDataBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<MetaDataBusiness>> GetAllAsync(MetaDataFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid entityGuid, EntityPart part, CancellationToken token, string tranName)
	{
		return dbRepository.RemoveRangeAsync(entityGuid, part, token, tranName);
	}

	public Task<List<Guid>> GetGuidListForFixAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetGuidListForFixAsync(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync<T>(Guid entityGuid, EntityPart part, List<T> data, CancellationToken token, string tranName)
	{
		return dbRepository.SaveRangeAsync(entityGuid, part, data, token, tranName);
	}
}
