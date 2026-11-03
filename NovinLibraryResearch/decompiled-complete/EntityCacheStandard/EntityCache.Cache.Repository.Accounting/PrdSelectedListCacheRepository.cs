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

public class PrdSelectedListCacheRepository : CacheRepository<PrdSelectedBusiness>, IPrdSelectedListRepository, IRepository<PrdSelectedBusiness>, INoGuidRepository<PrdSelectedBusiness>, IDisposable
{
	private IPrdSelectedListRepository _dbRepositiry;

	public override IItemAsyncMapper<PrdSelectedBusiness, PrdSelectedBusiness> Mapper => PrdSelectedMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, PrdSelectedBusiness> TimingCache { get; set; }

	public PrdSelectedListCacheRepository(IPrdSelectedListRepository dbRepository, int CachingTime)
		: base((IRepository<PrdSelectedBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		_dbRepositiry = dbRepository;
		TimingCache = new TimingCache<Guid, PrdSelectedBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<IEnumerable<PrdSelectedBusiness>> GetAllAsync(string title, string description, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await _dbRepositiry.GetAllAsync(title, description, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Selection\\PrdSelectedListCacheRepository.cs", 41);
		}
	}
}
