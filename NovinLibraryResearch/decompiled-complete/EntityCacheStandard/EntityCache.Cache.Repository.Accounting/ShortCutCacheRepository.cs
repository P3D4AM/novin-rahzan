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

public class ShortCutCacheRepository : CacheRepository<ShortCutBusiness>, IShortCutRepository, IRepository<ShortCutBusiness>, INoGuidRepository<ShortCutBusiness>, IDisposable
{
	private IShortCutRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<ShortCutBusiness, ShortCutBusiness> Mapper => ShortCutMapper.Instance;

	protected override TimingCache<Guid, ShortCutBusiness> TimingCache { get; set; }

	public ShortCutCacheRepository(IShortCutRepository dbRepository, int CachingTime)
		: base((IRepository<ShortCutBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, ShortCutBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<ShortCutBusiness> GetAsync(string formName, string name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAsync(formName, name, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ShortCutCacheRepository.cs", 43);
		}
	}
}
