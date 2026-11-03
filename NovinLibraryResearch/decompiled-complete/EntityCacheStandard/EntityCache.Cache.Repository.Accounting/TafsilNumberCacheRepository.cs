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
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting;

public class TafsilNumberCacheRepository : CacheRepositoryNoGuiId<TafsilNumberBusiness, Guid>, ITafsilNumberRepository, INoGuidRepository<TafsilNumberBusiness>, IDisposable
{
	private ITafsilNumberRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<TafsilNumberBusiness, TafsilNumberBusiness> Mapper => TafsilNumberMapper.Instance;

	protected override TimingCache<Guid, TafsilNumberBusiness> TimingCache { get; set; }

	public TafsilNumberCacheRepository(ITafsilNumberRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<TafsilNumberBusiness>)dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TafsilNumberBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<List<TafsilNumberBusiness>> GetAllAsync(Guid tafsilGuid, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(tafsilGuid, token, traName);
	}

	public async Task<List<TafsilNumberBusiness>> GetInfoAsync(long number, bool? status, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetInfoAsync(number, status, token, traName);
	}

	public Task<List<string>> GetTitleListAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetTitleListAsync(token, tranName);
	}

	public async Task<List<TafsilNumberBusiness>> GetInfoByCalcNumberAsync(long number, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetInfoByCalcNumberAsync(number, token, traName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(List<Guid> tafsilGuids, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAsync(tafsilGuids, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.RemoveAsync(tafsilGuid, tryCount, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, long Number, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.RemoveAsync(tafsilGuid, Number, tryCount, token, tranName);
	}

	public async Task<List<TafsilNumberReportBusiness>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		return await dbRepository.GetAllReportAsync(token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\TafsilNumberCacheRepository.cs", 77);
		}
	}

	public Task<List<TafsilNumberBusiness>> GetAllAsync(List<Guid> tafsilGuids, CancellationToken token, string tranName)
	{
		return ((ITafsilNumberRepository)base.DbRepository).GetAllAsync(tafsilGuids, token, tranName);
	}
}
