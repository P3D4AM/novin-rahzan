using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class AdvCategoryCacheRepository : CacheRepository<AdvCategoryBusiness>, IAdvCategoryRepository, IRepository<AdvCategoryBusiness>, INoGuidRepository<AdvCategoryBusiness>, IDisposable
{
	private IAdvCategoryRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AdvCategoryBusiness, AdvCategoryBusiness> Mapper => AdvCategoryMapper.Instance;

	protected override TimingCache<Guid, AdvCategoryBusiness> TimingCache { get; set; }

	public AdvCategoryCacheRepository(IAdvCategoryRepository dbRepository, int CachingTime)
		: base((IRepository<AdvCategoryBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AdvCategoryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<IEnumerable<AdvCategoryBusiness>> GetAllAsync(Guid parentGuid, AdvertiseType type, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllAsync(parentGuid, type, status, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\AdvCategoryCacheRepository.cs", 43);
		}
	}
}
