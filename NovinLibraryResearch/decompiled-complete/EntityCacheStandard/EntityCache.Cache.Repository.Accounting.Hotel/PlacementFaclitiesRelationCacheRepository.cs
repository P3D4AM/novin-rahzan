using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using EntityCache.Mapping.BusinessMappers.Accounting.Hotel;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Hotel;

public class PlacementFaclitiesRelationCacheRepository : CacheRepositoryNoGuiId<PlacementFacilitiesRelationBusiness, string>, IPlacementFacilitiesRelationRepository, INoGuidRepository<PlacementFacilitiesRelationBusiness>, IDisposable
{
	private IPlacementFacilitiesRelationRepository _dbRepository;

	public override IItemAsyncMapper<PlacementFacilitiesRelationBusiness, PlacementFacilitiesRelationBusiness> Mapper => PlacementFacilitiesRelationMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => false;

	protected override TimingCache<string, PlacementFacilitiesRelationBusiness> TimingCache { get; set; }

	public PlacementFaclitiesRelationCacheRepository(IPlacementFacilitiesRelationRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<PlacementFacilitiesRelationBusiness>)dbRepository, cachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, PlacementFacilitiesRelationBusiness>(base.ContextGuid, Mapper, cachingTime);
	}

	public Task<IEnumerable<PlacementFacilitiesRelationBusiness>> GetAllAsync(Guid placementGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(placementGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid placementGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeAsync(placementGuid, token, tranName);
	}
}
