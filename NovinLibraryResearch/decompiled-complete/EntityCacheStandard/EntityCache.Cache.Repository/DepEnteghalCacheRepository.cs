using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class DepEnteghalCacheRepository : CacheRepository<DepEnteghalBusiness>, IDepEnteghalRepository, IRepository<DepEnteghalBusiness>, INoGuidRepository<DepEnteghalBusiness>, IDisposable
{
	private IDepEnteghalRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, DepEnteghalBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<DepEnteghalBusiness, DepEnteghalBusiness> Mapper => DepEnteghalMapper.Instance;

	public DepEnteghalCacheRepository(IDepEnteghalRepository dbRepository, int CachingTime)
		: base((IRepository<DepEnteghalBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, DepEnteghalBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<DepEnteghalBusiness> GetAsync(int no, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(no, token, tranName);
	}

	public Task<int> SelectLastIdGAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SelectLastIdGAsync(token, tranName);
	}

	public Task<int> CountQtByMasterGuidAsync(Guid masterGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CountQtByMasterGuidAsync(masterGuid, token, tranName);
	}

	public Task<IEnumerable<DepEnteghalReportBusiness>> GetAllAsync(DepEnteghalFilter filter, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<DepEnteghalBusiness> GetFromDbAsync(Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetFromDbAsync(guid, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\DepEnteghalCacheRepository.cs", 66);
		}
	}

	public Task<ReturnedSaveFuncInfo> SetSanadNoAsync(Guid guid, int sanadNo, CancellationToken token, string tranName)
	{
		return dbRepository.SetSanadNoAsync(guid, sanadNo, token, tranName);
	}
}
