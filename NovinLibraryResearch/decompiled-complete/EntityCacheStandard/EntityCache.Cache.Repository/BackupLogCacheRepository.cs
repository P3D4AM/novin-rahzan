using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Upload;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class BackupLogCacheRepository : CacheRepositoryNoGuiId<BackupLogBusiness, Guid>, IBackupLogRepository, INoGuidRepository<BackupLogBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, BackupLogBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<BackupLogBusiness, BackupLogBusiness> Mapper => BackupLogMapper.Instance;

	public BackupLogCacheRepository(IBackupLogRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<BackupLogBusiness>)dbRepository, CachingTime)
	{
		TimingCache = new TimingCache<Guid, BackupLogBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<BackupLogBusiness> GetAsync(Guid fileId, long crc, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IBackupLogRepository)base.DbRepository).GetAsync(fileId, crc, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Upload\\BackupLogCacheRepository.cs", 41);
		}
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllAsync(int tryCount, CancellationToken token, string tranName)
	{
		return ((IBackupLogRepository)base.DbRepository).RemoveAllAsync(tryCount, token, tranName);
	}
}
