using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Attendance;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Attendance;

public class AttDepartmentCacheRepository : SyncCacheRepository<AttDepartmentBusiness>, IAttDepartmentRepository, ISyncRepository<AttDepartmentBusiness>, IEditableDelivery, IRepository<AttDepartmentBusiness>, INoGuidRepository<AttDepartmentBusiness>, IDisposable
{
	private IAttDepartmentRepository _dbRepo;

	public override IItemAsyncMapper<AttDepartmentBusiness, AttDepartmentBusiness> Mapper => AttDepartmentMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, AttDepartmentBusiness> TimingCache { get; set; }

	public AttDepartmentCacheRepository(IAttDepartmentRepository dbRepo, int CachingTime, bool cachingEnable = true)
		: base((ISyncRepository<AttDepartmentBusiness>)dbRepo, CachingTime, cachingEnable)
	{
		_dbRepo = dbRepo;
		TimingCache = new TimingCache<Guid, AttDepartmentBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepo = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Attendance\\AttDepartmentCacheRepository.cs", 37);
		}
	}

	public Task<AttDepartmentBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		return _dbRepo.GetAsync(name, token, tranName);
	}
}
