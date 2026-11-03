using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

internal class SuggestedBankCacheRepository : CacheRepository<SuggestedBankBusiness>, ISuggestedBankRepository, IRepository<SuggestedBankBusiness>, INoGuidRepository<SuggestedBankBusiness>, IDisposable
{
	private ISuggestedBankRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, SuggestedBankBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<SuggestedBankBusiness, SuggestedBankBusiness> Mapper => SuggestedBankMapper.Instance;

	public SuggestedBankCacheRepository(ISuggestedBankRepository _dbRepository, int CachingTime)
		: base((IRepository<SuggestedBankBusiness>)_dbRepository, CachingTime, cachingEnable: true)
	{
		dbRepository = _dbRepository;
		TimingCache = new TimingCache<Guid, SuggestedBankBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\SuggestedBankCacheRepository.cs", 37);
		}
	}

	public Task<SuggestedBankBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(name, token, tranName);
	}
}
