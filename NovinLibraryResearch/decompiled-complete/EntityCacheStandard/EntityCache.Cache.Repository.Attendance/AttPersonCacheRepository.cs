using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.Attandance;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class AttPersonCacheRepository : CacheRepository<AttPersonBusiness>, IAttPersonRepository, IRepository<AttPersonBusiness>, INoGuidRepository<AttPersonBusiness>, IDisposable
{
	private IAttPersonRepository _depRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public override IItemAsyncMapper<AttPersonBusiness, AttPersonBusiness> Mapper => AttPersonMapper.Instance;

	protected override TimingCache<Guid, AttPersonBusiness> TimingCache { get; set; }

	public AttPersonCacheRepository(IAttPersonRepository dbRepository, int CachingTime, bool cachingEnable = true)
		: base((IRepository<AttPersonBusiness>)dbRepository, CachingTime, cachingEnable)
	{
		try
		{
			_depRepository = dbRepository;
			TimingCache = new TimingCache<Guid, AttPersonBusiness>(base.ContextGuid, Mapper, CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\AttPersonCacheRepository.cs", 31);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_depRepository?.Dispose();
				_depRepository = null;
				TimingCache?.Dispose();
				TimingCache = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\AttPersonCacheRepository.cs", 48);
		}
	}

	public Task<int> CountInDepartmentAsync(Guid departmentGuid, CancellationToken token, string tranName)
	{
		return _depRepository.CountInDepartmentAsync(departmentGuid, token, tranName);
	}
}
