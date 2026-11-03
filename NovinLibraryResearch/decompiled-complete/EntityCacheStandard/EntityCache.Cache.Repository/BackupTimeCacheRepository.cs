using System;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Upload;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class BackupTimeCacheRepository : CacheRepository<BackupTimeBusiness>, IBackupTimeRepository, IRepository<BackupTimeBusiness>, INoGuidRepository<BackupTimeBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, BackupTimeBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<BackupTimeBusiness, BackupTimeBusiness> Mapper => BackupTimeMapper.Instance;

	public BackupTimeCacheRepository(IRepository<BackupTimeBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, BackupTimeBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Upload\\BackupTimeCacheRepository.cs", 33);
		}
	}
}
