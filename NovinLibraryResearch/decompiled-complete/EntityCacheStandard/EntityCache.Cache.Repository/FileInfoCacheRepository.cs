using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class FileInfoCacheRepository : SyncCacheRepository<FileInfoBusiness>, IFileInfoRepository, ISyncRepository<FileInfoBusiness>, IEditableDelivery, IRepository<FileInfoBusiness>, INoGuidRepository<FileInfoBusiness>, IDisposable
{
	private bool _isDisposed;

	private IFileInfoRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public override IItemAsyncMapper<FileInfoBusiness, FileInfoBusiness> Mapper => FileInfoMapper.Instance;

	protected override TimingCache<Guid, FileInfoBusiness> TimingCache { get; set; }

	public FileInfoCacheRepository(ISyncRepository<FileInfoBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = (IFileInfoRepository)dbRepository;
		TimingCache = new TimingCache<Guid, FileInfoBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override FileInfoBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<FileInfoBusiness> GetAsync(string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(name, subDirectory, part, token, tranName);
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoCacheRepository.cs", 51);
		}
	}

	public Task<List<FileInfoBusiness>> GetAsync(Guid entityGuid, FileManagement_Parts part, bool justFromCache, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(entityGuid, part, justFromCache, token, tranName);
	}

	public Task<List<FileInfoBusiness>> GetAllCompressedImageAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetAllCompressedImageAsync(token, tranName);
	}

	public Task<List<FileInfoBusiness>> GetWrongExtentionsAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetWrongExtentionsAsync(token, tranName);
	}

	public Task<FileInfoBusiness> GetNoImageAsync(FileManagement_Parts part, CancellationToken token, string tranName)
	{
		return dbRepository.GetNoImageAsync(part, token, tranName);
	}

	public Task<List<Guid>> GetAllGuidListAsync(FileInfoFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllGuidListAsync(filter, token, tranName);
	}
}
