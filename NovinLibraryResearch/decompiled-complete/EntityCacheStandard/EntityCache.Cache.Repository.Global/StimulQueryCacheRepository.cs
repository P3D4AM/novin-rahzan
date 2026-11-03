using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Core;
using EntityCache.Core.Repository.Global;
using EntityCache.Mapping.Global;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Global;

public class StimulQueryCacheRepository : CacheRepository<StimulQueryBusiness>, IStimulQueryRepository, IRepository<StimulQueryBusiness>, INoGuidRepository<StimulQueryBusiness>, IDisposable
{
	private IStimulQueryRepository _dbRepository;

	public override IItemAsyncMapper<StimulQueryBusiness, StimulQueryBusiness> Mapper => StimulQueryMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, StimulQueryBusiness> TimingCache { get; set; }

	public StimulQueryCacheRepository(IStimulQueryRepository dbRepository, int CachingTime)
		: base((IRepository<StimulQueryBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, StimulQueryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\StimulQueryCacheRepository.cs", 38);
		}
	}

	public Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrenced(CancellationToken token, string tranName)
	{
		return _dbRepository.GetNextRefrenced(token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> TruncateAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.TruncateAsync(token, tranName);
	}
}
