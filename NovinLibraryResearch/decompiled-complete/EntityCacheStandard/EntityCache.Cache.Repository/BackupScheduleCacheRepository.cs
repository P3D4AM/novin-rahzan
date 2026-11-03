using System;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Upload;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class BackupScheduleCacheRepository : CacheRepository<BackupScheduleBusiness>, IBackupScheduleRepository, IRepository<BackupScheduleBusiness>, INoGuidRepository<BackupScheduleBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, BackupScheduleBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<BackupScheduleBusiness, BackupScheduleBusiness> Mapper => BackupScheduleMapper.Instance;

	public BackupScheduleCacheRepository(IBackupScheduleRepository dbRepository, int CachingTime)
		: base((IRepository<BackupScheduleBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, BackupScheduleBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Upload\\BackupScheduleCacheRepository.cs", 33);
		}
	}
}
