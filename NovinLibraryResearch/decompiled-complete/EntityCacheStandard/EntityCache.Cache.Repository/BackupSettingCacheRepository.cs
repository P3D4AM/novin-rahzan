using System;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Upload;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class BackupSettingCacheRepository : CacheRepositoryNoGuiId<BackupSettingsBusiness, int>, IBackupSettingRepository, INoGuidRepository<BackupSettingsBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<int, BackupSettingsBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<BackupSettingsBusiness, BackupSettingsBusiness> Mapper => BackupSettingMapper.Instance;

	public BackupSettingCacheRepository(INoGuidRepository<BackupSettingsBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime)
	{
		TimingCache = new TimingCache<int, BackupSettingsBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Upload\\BackupSettingCacheRepository.cs", 35);
		}
	}
}
