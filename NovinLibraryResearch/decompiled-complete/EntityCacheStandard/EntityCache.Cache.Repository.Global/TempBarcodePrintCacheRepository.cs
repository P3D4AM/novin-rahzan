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

public class TempBarcodePrintCacheRepository : CacheRepository<TempBarcodePrintBusiness>, ITempBarcodePrintRepository, IRepository<TempBarcodePrintBusiness>, INoGuidRepository<TempBarcodePrintBusiness>, IDisposable
{
	private ITempBarcodePrintRepository _dbRepository;

	public override IItemAsyncMapper<TempBarcodePrintBusiness, TempBarcodePrintBusiness> Mapper => TempBarcodePrintMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, TempBarcodePrintBusiness> TimingCache { get; set; }

	public TempBarcodePrintCacheRepository(ITempBarcodePrintRepository dbRepository, int CachingTime)
		: base((IRepository<TempBarcodePrintBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TempBarcodePrintBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Global\\TempBarcodePrintCacheRepository.cs", 38);
		}
	}

	public Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		return _dbRepository.RemoveRangeAsync(masterGuid, token, tranName);
	}
}
