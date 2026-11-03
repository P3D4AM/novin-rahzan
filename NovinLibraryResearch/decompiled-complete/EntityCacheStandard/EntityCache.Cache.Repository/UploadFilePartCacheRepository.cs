using System;
using System.Collections.Generic;
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

public class UploadFilePartCacheRepository : CacheRepository<UploadFilePartBusiness>, IUploadFilePartRepository, IRepository<UploadFilePartBusiness>, INoGuidRepository<UploadFilePartBusiness>, IDisposable
{
	private IUploadFilePartRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, UploadFilePartBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<UploadFilePartBusiness, UploadFilePartBusiness> Mapper => UploadFilePartMapper.Instance;

	public UploadFilePartCacheRepository(IRepository<UploadFilePartBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = (IUploadFilePartRepository)base.DbRepository;
		TimingCache = new TimingCache<Guid, UploadFilePartBusiness>(base.Context.Guid, Mapper, CachingTime);
	}

	public Task<IEnumerable<UploadFilePartBusiness>> GetLastFilePartAsync(int top, bool? status, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).GetLastFilePartAsync(top, status, token, traName);
	}

	public Task ResetSentAsync(Guid fileId, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).ResetSentAsync(fileId, tryCount, token, tranName);
	}

	public Task<int> GetTotalPartCountAsync(Guid fileId, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).GetTotalPartCountAsync(fileId, token, traName);
	}

	public Task<int> GetPartSentCountAsync(Guid fileId, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).GetPartSentCountAsync(fileId, token, traName);
	}

	public Task<List<string>> GetFileOrderAsync(Guid fileId, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).GetFileOrderAsync(fileId, token, traName);
	}

	public Task<ReturnedSaveFuncInfo> RemoveAllAsync(Guid fileid, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).RemoveAllAsync(fileid, tryCount, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSentAsync(Guid filePart, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).SetSentAsync(filePart, tryCount, token, tranName);
	}

	public Task<Guid> GetFileIdAsync(Guid filePart, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return ((IUploadFilePartRepository)base.DbRepository).GetFileIdAsync(filePart, token, traName);
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(UploadFilePartBusiness item, byte[] bytes, short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SaveAsync(item, bytes, tryCount, token, tranName);
	}

	public Task<byte[]> GetBytesAsync(Guid guid, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetBytesAsync(guid, token, traName);
	}

	public Task SetRetryAsync(DateTime addMinutes, Guid guid, CancellationToken token, string traName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.SetRetryAsync(addMinutes, guid, token, traName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Upload\\UploadFilePartCacheRepository.cs", 99);
		}
	}
}
