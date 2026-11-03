using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class LotteryCacheRepository : CacheRepository<LotteryBusiness>, ILotteryRepository, IRepository<LotteryBusiness>, INoGuidRepository<LotteryBusiness>, IDisposable
{
	private ILotteryRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<LotteryBusiness, LotteryBusiness> Mapper => LotteryMapper.Instance;

	protected override TimingCache<Guid, LotteryBusiness> TimingCache { get; set; }

	public LotteryCacheRepository(ILotteryRepository dbRepository, int cachingTime)
		: base((IRepository<LotteryBusiness>)dbRepository, cachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, LotteryBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public async Task<IEnumerable<LotteryBusiness>> GetAllAsync(LotteryStatus status, DateTime date1, DateTime date2, Guid cusGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(status, date1, date2, cusGuid, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\LotteryCacheRepository.cs", 45);
		}
	}
}
