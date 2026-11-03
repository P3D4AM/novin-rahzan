using System;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.Sanad;
using EntityCache.Mapping.BusinessMappers.Accounting.Sanad;
using PacketParser.Caching;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Sanad;

public class SanadArchivesCacheRepository : SyncCacheRepository<SanadArchivesBusiness>, ISanadArchivesRepository, ISyncRepository<SanadArchivesBusiness>, IEditableDelivery, IRepository<SanadArchivesBusiness>, INoGuidRepository<SanadArchivesBusiness>, IDisposable
{
	private ISanadArchivesRepository _dbRepository;

	public override IItemAsyncMapper<SanadArchivesBusiness, SanadArchivesBusiness> Mapper => SanadArchivesMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SanadArchivesBusiness> TimingCache { get; set; }

	public SanadArchivesCacheRepository(ISanadArchivesRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<SanadArchivesBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, SanadArchivesBusiness>(base.ContextGuid, Mapper, CachingTime);
	}
}
