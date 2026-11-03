using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class TafsilGroupCacheRepository : SyncCacheRepository<TafsilGroupBusiness>, ITafsilGroupRepository, ISyncRepository<TafsilGroupBusiness>, IEditableDelivery, IRepository<TafsilGroupBusiness>, INoGuidRepository<TafsilGroupBusiness>, IDisposable
{
	private ITafsilGroupRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<TafsilGroupBusiness, TafsilGroupBusiness> Mapper => TafsilGroupMapper.Instance;

	protected override TimingCache<Guid, TafsilGroupBusiness> TimingCache { get; set; }

	public TafsilGroupCacheRepository(ITafsilGroupRepository dbRepository, int CachingTime)
		: base((ISyncRepository<TafsilGroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, TafsilGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<List<TafsilGroupBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(search, token, status, tranName);
	}

	public Task<TafsilGroupBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(name, token, tranName);
	}

	public Task<ReturnedSaveFuncInfoWithValue<List<TafsilGroupBusiness>>> IsDuplicateCodeAsync(long codeMin, long codeMax, Guid guid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.IsDuplicateCodeAsync(codeMin, codeMax, guid, token, tranName);
	}

	public Task<long> NextMinCodeAsync(CancellationToken token, string tranName)
	{
		return dbRepository.NextMinCodeAsync(token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Coding\\TafsilGroupCacheRepository.cs", 56);
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebTafsilGroup> items, CancellationToken token, string tranName)
	{
		return dbRepository.SaveRangeAsync(items, token, tranName);
	}
}
