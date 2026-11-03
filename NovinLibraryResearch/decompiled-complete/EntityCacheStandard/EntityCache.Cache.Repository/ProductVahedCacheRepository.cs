using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class ProductVahedCacheRepository : SyncCacheRepository<PrdVahedBusiness>, IProductVahedRepository, ISyncRepository<PrdVahedBusiness>, IEditableDelivery, IRepository<PrdVahedBusiness>, INoGuidRepository<PrdVahedBusiness>, IDisposable
{
	private new IProductVahedRepository DbRepository;

	private ConcurrentDictionary<string, Guid> CacheName = new ConcurrentDictionary<string, Guid>();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, PrdVahedBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<PrdVahedBusiness, PrdVahedBusiness> Mapper => PrdVahedMapper.Instance;

	public ProductVahedCacheRepository(ISyncRepository<PrdVahedBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		DbRepository = (IProductVahedRepository)dbRepository;
		TimingCache = new TimingCache<Guid, PrdVahedBusiness>(base.Context?.Guid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				CacheName = null;
				DbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductVahedCacheRepository.cs", 42);
		}
	}

	public async Task<PrdVahedBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		int line = 0;
		try
		{
			line = 1;
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			line = 2;
			PrdVahedBusiness prdVahed2 = null;
			line = 3;
			Guid value;
			bool flag = CacheName.TryGetValue(name, out value);
			line = 4;
			if (flag)
			{
				flag = TimingCache.TryGetValue(value, out prdVahed2);
			}
			line = 5;
			if (flag && prdVahed2 != null)
			{
				line = 6;
				return prdVahed2;
			}
			line = 7;
			CacheName.TryRemove(name, out var _);
			line = 8;
			prdVahed2 = await DbRepository.GetAsync(name, token, tranName);
			line = 9;
			if (prdVahed2 != null)
			{
				line = 10;
				await TimingCache.UpdateTimeAsync(prdVahed2.Guid, prdVahed2);
				line = 11;
				CacheName.TryAdd(name, prdVahed2.Guid);
				line = 12;
			}
			line = 13;
			return prdVahed2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\ProductVahedCacheRepository.cs", 82);
			throw;
		}
	}

	public new void ResetCache()
	{
		TimingCache?.ClearAll();
	}
}
