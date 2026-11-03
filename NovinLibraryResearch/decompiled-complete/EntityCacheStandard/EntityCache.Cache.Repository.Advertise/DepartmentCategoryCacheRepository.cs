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

public class DepartmentCategoryCacheRepository : CacheRepository<DepartmentCategoryBusiness>, IDepartmentCategoryRepository, IRepository<DepartmentCategoryBusiness>, INoGuidRepository<DepartmentCategoryBusiness>, IDisposable
{
	private IDepartmentCategoryRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<DepartmentCategoryBusiness, DepartmentCategoryBusiness> Mapper => DepartmentCategoryMapper.Instance;

	protected override TimingCache<Guid, DepartmentCategoryBusiness> TimingCache { get; set; }

	public DepartmentCategoryCacheRepository(IDepartmentCategoryRepository dbRepository, int CachingTime)
		: base((IRepository<DepartmentCategoryBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, DepartmentCategoryBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<IEnumerable<DepartmentCategoryBusiness>> GetAllAsync(Guid depGuid, bool? status, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAllAsync(depGuid, status, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\DepartmentCategoryCacheRepository.cs", 45);
		}
	}
}
