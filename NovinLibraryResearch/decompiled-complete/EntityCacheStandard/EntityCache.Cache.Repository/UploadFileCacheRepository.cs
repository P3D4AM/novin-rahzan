using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers.Upload;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class UploadFileCacheRepository : CacheRepository<UploadFileBusiness>, IUploadFileRepository, IRepository<UploadFileBusiness>, INoGuidRepository<UploadFileBusiness>, IDisposable
{
	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, UploadFileBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<UploadFileBusiness, UploadFileBusiness> Mapper => UploadFileMapper.Instance;

	public UploadFileCacheRepository(IRepository<UploadFileBusiness> dbRepository, int CachingTimeSeconds)
		: base(dbRepository, CachingTimeSeconds, cachingEnable: true)
	{
		TimingCache = new TimingCache<Guid, UploadFileBusiness>(base.ContextGuid, Mapper, base.CachingTime);
	}

	public Task<UploadFileBusiness> GetAsync(long crc, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFileRepository)base.DbRepository).GetAsync(crc, token, tranName);
	}

	public Task<UploadFileBusiness> GetByFileIdAsync(Guid fileid, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFileRepository)base.DbRepository).GetByFileIdAsync(fileid, token, tranName);
	}

	public Task<bool> IsSentCompleteAsync(Guid fileId, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFileRepository)base.DbRepository).IsSentCompleteAsync(fileId, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Upload\\UploadFileCacheRepository.cs", 53);
		}
	}
}
