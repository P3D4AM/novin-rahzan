using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Customer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class CustomerTitleCacheRepository : SyncCacheRepository<CusTitleBusiness>, ICustomerTitleRepository, ISyncRepository<CusTitleBusiness>, IEditableDelivery, IRepository<CusTitleBusiness>, INoGuidRepository<CusTitleBusiness>, IDisposable
{
	private ICustomerTitleRepository Dbrepository;

	private ConcurrentDictionary<string, Guid> CacheName = new ConcurrentDictionary<string, Guid>();

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CusTitleBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CusTitleBusiness, CusTitleBusiness> Mapper => CusTitleMapper.Instance;

	public CustomerTitleCacheRepository(ISyncRepository<CusTitleBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		Dbrepository = (ICustomerTitleRepository)dbRepository;
		TimingCache = new TimingCache<Guid, CusTitleBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				CacheName = null;
				Dbrepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerTitleCacheRepository.cs", 40);
		}
	}

	public async Task<CusTitleBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			CusTitleBusiness CusTiile = null;
			Guid value;
			bool flag = CacheName.TryGetValue(name, out value);
			if (flag)
			{
				flag = TimingCache.TryGetValue(value, out CusTiile);
			}
			if (flag && CusTiile != null)
			{
				return CusTiile;
			}
			CacheName.TryRemove(name, out var _);
			CusTiile = await Dbrepository.GetAsync(name, token, tranName);
			if (CusTiile != null)
			{
				await TimingCache.UpdateTimeAsync(CusTiile.Guid, CusTiile, tranName);
				CacheName.TryAdd(name, CusTiile.Guid);
			}
			return CusTiile;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Customer\\CustomerTitleCacheRepository.cs", 76);
			throw;
		}
	}
}
