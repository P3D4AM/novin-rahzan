using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class RasGiriDetCacheRepository : CacheRepository<RasGiriDetBusiness>, IRasGiriDetRepository, IRepository<RasGiriDetBusiness>, INoGuidRepository<RasGiriDetBusiness>, IDisposable
{
	private new IRasGiriDetRepository DbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, RasGiriDetBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<RasGiriDetBusiness, RasGiriDetBusiness> Mapper => RasGiriDetMapper.Instance;

	public RasGiriDetCacheRepository(IRasGiriDetRepository dbRepository, int CachingTime)
		: base((IRepository<RasGiriDetBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, RasGiriDetBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<RasGiriDetBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.SaveRangeAsync(lst, tryCount, token, tranName);
	}

	public Task<IEnumerable<RasGiriDetBusiness>> GetAllByMasterGuidAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.GetAllByMasterGuidAsync(masterGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllByMasterGuidAsync(Guid masterGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return DbRepository.RemoveAllByMasterGuidAsync(masterGuid, tryCount, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\RasGiriDetCacheRepository.cs", 59);
		}
	}
}
