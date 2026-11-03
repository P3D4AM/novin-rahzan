using System;
using System.Threading.Tasks;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using EntityCache.Mapping.BusinessMappers.Advertise;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Advertise;

public class AdvDepartmentCacheRepository : CacheRepository<AdvDepartmentBusiness>, IAdvDepartmentRepository, IRepository<AdvDepartmentBusiness>, INoGuidRepository<AdvDepartmentBusiness>, IDisposable
{
	private IAdvDepartmentRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AdvDepartmentBusiness, AdvDepartmentBusiness> Mapper => AdvDepartmentMapper.Instance;

	protected override TimingCache<Guid, AdvDepartmentBusiness> TimingCache { get; set; }

	public AdvDepartmentCacheRepository(IAdvDepartmentRepository dbRepository, int CachingTime)
		: base((IRepository<AdvDepartmentBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, AdvDepartmentBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.CheckNameAsync(name, guid, tranName);
	}

	public async Task<AdvDepartmentBusiness> GetAsync(string depName, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return await dbRepository.GetAsync(depName, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Advertise\\AdvDepartmentCacheRepository.cs", 46);
		}
	}
}
