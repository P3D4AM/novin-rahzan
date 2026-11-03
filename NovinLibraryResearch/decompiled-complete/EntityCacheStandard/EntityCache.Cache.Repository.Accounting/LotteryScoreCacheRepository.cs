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

public class LotteryScoreCacheRepository : CacheRepository<LotteryScoreBusiness>, ILotteryScoreRepository, IRepository<LotteryScoreBusiness>, INoGuidRepository<LotteryScoreBusiness>, IDisposable
{
	private ILotteryScoreRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<LotteryScoreBusiness, LotteryScoreBusiness> Mapper => LotteryScoreMapper.Instance;

	protected override TimingCache<Guid, LotteryScoreBusiness> TimingCache { get; set; }

	public LotteryScoreCacheRepository(ILotteryScoreRepository dbRepository, int cachingTime)
		: base((IRepository<LotteryScoreBusiness>)dbRepository, cachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, LotteryScoreBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public async Task<IEnumerable<LotteryScoreBusiness>> GetAllAsync(DateTime date1, DateTime date2, LotteryStatus status, Guid cusGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(date1, date2, status, cusGuid, token, tranName);
	}

	public async Task<List<Guid>> StartLottery(int count, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.StartLottery(count, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\LotteryScoreCacheRepository.cs", 52);
		}
	}
}
