using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class FileInfoReTryDownloadCacheRepository : CacheRepository<FileInfoReTryDownloadBusiness>, IFileInfoReTryDownloadRepository, IRepository<FileInfoReTryDownloadBusiness>, INoGuidRepository<FileInfoReTryDownloadBusiness>, IDisposable
{
	private IFileInfoReTryDownloadRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, FileInfoReTryDownloadBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<FileInfoReTryDownloadBusiness, FileInfoReTryDownloadBusiness> Mapper => FileInfoMapper.Instance;

	public FileInfoReTryDownloadCacheRepository(IRepository<FileInfoReTryDownloadBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime, cachingEnable: true)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = (IFileInfoReTryDownloadRepository)dbRepository;
		TimingCache = new TimingCache<Guid, FileInfoReTryDownloadBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<List<FileInfoReTryDownloadBusiness>> GetFilesByPartAsync(FileManagement_Parts part, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs"))
		{
			_ = 2;
			try
			{
				List<FileInfoReTryDownloadBusiness> list = (await GetAllAsync(token, tranName)).Where((FileInfoReTryDownloadBusiness x) => x.Part == part).ToList();
				if (list.Any())
				{
					return list;
				}
				List<FileInfoReTryDownloadBusiness> res = await ((IFileInfoReTryDownloadRepository)base.DbRepository).GetFilesByPartAsync(part, token, tranName);
				if (res != null)
				{
					foreach (FileInfoReTryDownloadBusiness item in res)
					{
						await TimingCache.UpdateTimeAsync(item.Guid, item);
					}
				}
				return res;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFilesByPartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs", 46);
				throw;
			}
		}
	}

	public Task<FileInfoReTryDownloadBusiness> GetAsync(string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranNme)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAsync(name, subDirectory, part, token, tranNme);
	}

	public Task<List<FileInfoReTryDownloadBusiness>> GetAllForDownloadAsync(short tryCount, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return dbRepository.GetAllForDownloadAsync(tryCount, token, tranName);
	}

	public async Task<FileInfoReTryDownloadBusiness> GetIconSiteUrlAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetIconSiteUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs"))
		{
			try
			{
				return (await GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).FirstOrDefault((FileInfoReTryDownloadBusiness x) => x.FileName.Contains("favicon"));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetIconSiteUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs", 77);
				throw;
			}
		}
	}

	public async Task<FileInfoReTryDownloadBusiness> GetLogoSiteUrlAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetLogoSiteUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs"))
		{
			try
			{
				return (await GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).FirstOrDefault((FileInfoReTryDownloadBusiness x) => x.FileName.Contains("logo"));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLogoSiteUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs", 101);
				throw;
			}
		}
	}

	public async Task<FileInfoReTryDownloadBusiness> GetBannerSiteUrlAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return (await GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).Where((FileInfoReTryDownloadBusiness x) => x.FileName.Contains("banner")).FirstOrDefault();
	}

	public Task<List<FileInfoReTryDownloadBusiness>> GetListSliderSiteAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return GetFilesByPartAsync(FileManagement_Parts.Slider, token, tranName);
	}

	public async Task<FileInfoReTryDownloadBusiness> GetIconAndroidUrlAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return (await GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).FirstOrDefault((FileInfoReTryDownloadBusiness x) => x.FileName.Contains("favicon"));
	}

	public async Task<FileInfoReTryDownloadBusiness> GetLogoAndroidUrlAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return (await GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).Where((FileInfoReTryDownloadBusiness x) => x.FileName.Contains("Logo")).FirstOrDefault();
	}

	public async Task<List<FileInfoReTryDownloadBusiness>> GetBannerAndroidUrlAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return (await GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).Where((FileInfoReTryDownloadBusiness x) => x.FileName.Contains("banner"))?.ToList();
	}

	public Task<List<FileInfoReTryDownloadBusiness>> GetListSliderAndroidAsync(CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		return GetFilesByPartAsync(FileManagement_Parts.Slider, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\FileInfoReTryDownloadCacheRepository.cs", 189);
		}
	}
}
