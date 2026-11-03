using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot.OrderCouponCalculation;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Order;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Order;

public class OrderDetailCouponCalculationCacheRepository : CacheRepository<OrderDetailCouponCalculationBusiness>, IOrderDetailCouponCalculationRepository, IRepository<OrderDetailCouponCalculationBusiness>, INoGuidRepository<OrderDetailCouponCalculationBusiness>, IDisposable
{
	private IOrderDetailCouponCalculationRepository _dbRepository;

	public override IItemAsyncMapper<OrderDetailCouponCalculationBusiness, OrderDetailCouponCalculationBusiness> Mapper => OrderDetailCouponCalculationMapper.Instance;

	public override bool HasStatus => false;

	public override bool HasModified => true;

	protected override TimingCache<Guid, OrderDetailCouponCalculationBusiness> TimingCache { get; set; }

	public OrderDetailCouponCalculationCacheRepository(IOrderDetailCouponCalculationRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<OrderDetailCouponCalculationBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, OrderDetailCouponCalculationBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrencedAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextRefrencedAsync(token, tranName);
	}
}
