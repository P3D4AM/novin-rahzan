using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CustomerGroupCacheRepository : SyncCacheRepository<CusGroupBusiness>, ICustomerGroupRepository, ISyncRepository<CusGroupBusiness>, IEditableDelivery, IRepository<CusGroupBusiness>, INoGuidRepository<CusGroupBusiness>, IDisposable
{
	private ICustomerGroupRepository dbRepository;

	private ConcurrentDictionary<string, Guid> CacheName = new ConcurrentDictionary<string, Guid>();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CusGroupBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CusGroupBusiness, CusGroupBusiness> Mapper => CusGroupMapper.Instance;

	public CustomerGroupCacheRepository(ICustomerGroupRepository dbRepository, int CachingTime)
		: base((ISyncRepository<CusGroupBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CusGroupBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				CacheName = null;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerGroupCacheRepository.cs", 43);
		}
	}

	public async Task<CusGroupBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		int line = 0;
		try
		{
			CusGroupBusiness cusGroup2 = null;
			line = 1;
			Guid value;
			bool flag = CacheName.TryGetValue(name, out value);
			line = 2;
			if (flag)
			{
				flag = TimingCache.TryGetValue(value, out cusGroup2);
			}
			line = 5;
			if (flag && cusGroup2 != null)
			{
				return cusGroup2;
			}
			line = 10;
			CacheName.TryRemove(name, out var _);
			line = 11;
			cusGroup2 = await dbRepository.GetAsync(name, token, tranName);
			line = 12;
			if (cusGroup2 != null)
			{
				line = 13;
				await TimingCache.UpdateTimeAsync(cusGroup2.Guid, cusGroup2, tranName);
				line = 14;
				CacheName.TryAdd(name, cusGroup2.Guid);
				line = 15;
			}
			line = 16;
			return cusGroup2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Error Line Is :{line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerGroupCacheRepository.cs", 78);
			throw;
		}
	}

	public Task<int> CountInGroupActiveAsync(Guid selectedGroupGuid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CountInGroupActiveAsync(selectedGroupGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebCusGroup> items, CancellationToken token, string tranName)
	{
		return dbRepository.SaveRangeAsync(items, token, tranName);
	}
}
