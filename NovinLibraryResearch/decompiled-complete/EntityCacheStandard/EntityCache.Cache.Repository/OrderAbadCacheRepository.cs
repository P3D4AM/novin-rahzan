using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class OrderAbadCacheRepository : CacheRepository<OrderAbadBusiness>, IOrderAbadRepository, IRepository<OrderAbadBusiness>, INoGuidRepository<OrderAbadBusiness>, IDisposable
{
	private IOrderAbadRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, OrderAbadBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<OrderAbadBusiness, OrderAbadBusiness> Mapper => OrderAbadMapper.Instance;

	public OrderAbadCacheRepository(IOrderAbadRepository dbRepository, int CachingTime)
		: base((IRepository<OrderAbadBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, OrderAbadBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<OrderAbadBusiness> GetAsync(Guid DetailGuid, Document OrderType, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(DetailGuid, OrderType, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Order\\OrderAbadCacheRepository.cs", 42);
		}
	}
}
