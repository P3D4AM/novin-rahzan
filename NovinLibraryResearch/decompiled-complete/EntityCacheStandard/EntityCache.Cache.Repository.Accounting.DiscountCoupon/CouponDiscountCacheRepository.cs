using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.DiscountCoupon.Reports;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting.DiscountCoupon;
using EntityCache.Mapping.BusinessMappers.Accounting.NewFolder;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters.CouponDiscount;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.DiscountCoupon;

public class CouponDiscountCacheRepository : SyncCacheRepository<CouponDiscountBusiness>, ICouponDiscountRepository, ISyncRepository<CouponDiscountBusiness>, IEditableDelivery, IRepository<CouponDiscountBusiness>, INoGuidRepository<CouponDiscountBusiness>, IDisposable
{
	private ICouponDiscountRepository _dbRepository;

	public override IItemAsyncMapper<CouponDiscountBusiness, CouponDiscountBusiness> Mapper => CouponDiscountMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CouponDiscountBusiness> TimingCache { get; set; }

	public CouponDiscountCacheRepository(ICouponDiscountRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<CouponDiscountBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CouponDiscountBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<CouponDiscountReportBusiness>> GetAllAsync(CouponFilter filter, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllAsync(filter, token, tranName);
	}

	public override Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		return _dbRepository.SetSaveResult(item, token, tryCount, tranName);
	}

	public Task<IEnumerable<Guid>> GetAllOrderRelationAsync(Guid tafsilGuid, DateTime orderDate, decimal orderSum, int refId, Guid? specificCouponGuid, long mobile, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllOrderRelationAsync(tafsilGuid, orderDate, orderSum, refId, specificCouponGuid, mobile, token, tranName);
	}

	public Task<CouponDiscountReportBusiness> GetAsync(string title, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(title, token, tranName);
	}

	public Task<IEnumerable<Guid>> GetAllForScheduleAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllForScheduleAsync(token, tranName);
	}

	public Task<int> GetUsedCountAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetUsedCountAsync(guid, token, tranName);
	}

	public Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllUagesAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllUagesAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(List<Guid> couponGuids, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateServerStatusAsync(couponGuids, status, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusByGiftGuidAsync(Guid giftGuid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		return _dbRepository.UpdateServerStatusByGiftGuidAsync(giftGuid, status, token, tranName);
	}
}
