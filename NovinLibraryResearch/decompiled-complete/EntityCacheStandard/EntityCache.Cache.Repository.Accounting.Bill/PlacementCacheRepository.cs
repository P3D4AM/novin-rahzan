using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Bill;
using EntityCache.Business.Accounting.Bill.Reports;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Bill;
using EntityCache.Mapping.BusinessMappers.Accounting.Bill;
using PacketParser.BusinessLayer.Models.Hotel;
using PacketParser.BusinessLayer.Models.Placement;
using PacketParser.Caching;
using PacketParser.Filters.Hotel;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Bill;

public class PlacementCacheRepository : CacheRepository<PlacementBusiness>, IPlacementRepository, IRepository<PlacementBusiness>, INoGuidRepository<PlacementBusiness>, IDisposable
{
	private IPlacementRepository _dbRepository;

	public override IItemAsyncMapper<PlacementBusiness, PlacementBusiness> Mapper => PlacementMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, PlacementBusiness> TimingCache { get; set; }

	public PlacementCacheRepository(IPlacementRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<PlacementBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, PlacementBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<PlacementBusiness> GetAsync(int number, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(number, token, tranName);
	}

	public Task<int> GetNextNumber(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextNumber(token, tranName);
	}

	public Task<List<Guid>> GetTypePlacementListAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetTypePlacementListAsync(token, tranName);
	}

	public Task<List<RoomTypeModel>> GetTypeRoomCount(CancellationToken token, string tranName)
	{
		return _dbRepository.GetTypeRoomCount(token, tranName);
	}

	public Task<IEnumerable<PlacementReportsBusiness>> GetAllAsync(RoomFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<IEnumerable<PlacementKardexViewModel>> GetAllKardexAsync(PlacementKardexFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllKardexAsync(filter, token, tranName);
	}
}
