using System;
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

public class NaghzCacheRepository : CacheRepository<NaghzBusiness>, INaghzRepository, IRepository<NaghzBusiness>, INoGuidRepository<NaghzBusiness>, IDisposable
{
	private INaghzRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<NaghzBusiness, NaghzBusiness> Mapper => NaghzMapper.Instance;

	protected override TimingCache<Guid, NaghzBusiness> TimingCache { get; set; }

	public NaghzCacheRepository(INaghzRepository dbRepository, int CachingTime)
		: base((IRepository<NaghzBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, NaghzBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<NaghzBusiness> GetAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAsync(token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\NaghzCacheRepository.cs", 43);
		}
	}
}
