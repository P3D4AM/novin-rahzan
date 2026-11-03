using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class FileInfoReTryDownloadBusiness : Serializable<FileInfoReTryDownloadBusiness>, IFileInfo, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private string _fileName;

	private long crc;

	private long fileLen;

	public static int DbCount { get; }

	public Guid Guid { get; set; }

	public string FileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			_fileName = value?.FixString() ?? "";
		}
	}

	public string FullPath { get; set; }

	public DateTime Modified { get; set; }

	public FileManagement_Parts Part { get; set; }

	public string SubDirectory { get; set; } = "";


	public bool Status { get; set; }

	public Guid EntityGuid { get; set; }

	public string Url { get; set; }

	public int ReTryCount { get; set; }

	public DateTime NextTryCount { get; set; }

	public byte[]? FileContent { get; set; }

	public ENCompressedMode CompressMode { get; set; }

	public string ContextGuid { get; set; }

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "FileInfoReTryDownloadBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 29), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 29);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IFileInfoReTryDownload.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 31);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 28);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 36);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 37);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 42), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 42);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 55), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 55);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IFileInfoReTryDownload.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 57);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 61);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 62);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 67), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 67);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, IEnumerable<FileInfoReTryDownloadBusiness> list, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (FileInfoReTryDownloadBusiness item in list)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 103);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 104);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 109);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 110);
		}
		return ret;
	}

	public static Task<IEnumerable<FileInfoReTryDownloadBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetAllAsync(status, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 126), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 126);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 128);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 132);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 133);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 138), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoReTryDownloadBusiness.cs", 138);
			}
		}
		return ret;
	}

	public static Task<FileInfoReTryDownloadBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetAsync(guid, token, tranName);
	}

	public static Task<List<FileInfoReTryDownloadBusiness>> GetFilesByPartAsync(string contextGuid, FileManagement_Parts part, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetFilesByPartAsync(part, token, tranName);
	}

	public static async Task<FileInfoReTryDownloadBusiness> GetBannerSiteUrlAsync(string contextGuid, CancellationToken token, string tranName)
	{
		FileInfoReTryDownloadBusiness fileInfoReTryDownloadBusiness = (await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).FirstOrDefault((FileInfoReTryDownloadBusiness w) => w.FileName.Contains("banner"));
		if (fileInfoReTryDownloadBusiness == null)
		{
			return new FileInfoReTryDownloadBusiness(contextGuid)
			{
				FileName = "Banner.jpg"
			};
		}
		return fileInfoReTryDownloadBusiness;
	}

	public static async Task<FileInfoReTryDownloadBusiness> GetIconSiteUrlAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		FileInfoReTryDownloadBusiness fileInfoReTryDownloadBusiness = (await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).FirstOrDefault((FileInfoReTryDownloadBusiness w) => w.FileName.Contains("favicon"));
		if (fileInfoReTryDownloadBusiness == null)
		{
			return new FileInfoReTryDownloadBusiness(contextGuid)
			{
				FileName = "favicon.jpg"
			};
		}
		return fileInfoReTryDownloadBusiness;
	}

	public static async Task<FileInfoReTryDownloadBusiness> GetLogoSiteUrlAsync(string contextGuid, CancellationToken token, string tranName)
	{
		FileInfoReTryDownloadBusiness fileInfoReTryDownloadBusiness = (await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetFilesByPartAsync(FileManagement_Parts.LogoIconBanner, token, tranName)).FirstOrDefault((FileInfoReTryDownloadBusiness w) => w.FileName.Contains("logo"));
		if (fileInfoReTryDownloadBusiness == null)
		{
			return new FileInfoReTryDownloadBusiness(contextGuid)
			{
				FileName = "logo.jpg"
			};
		}
		return fileInfoReTryDownloadBusiness;
	}

	public static Task<List<FileInfoReTryDownloadBusiness>> GetListSliderSiteAsync(string contextGuid, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetFilesByPartAsync(FileManagement_Parts.Slider, token, tranName);
	}

	public async Task<long> GetCrcAsync()
	{
		return crc;
	}

	public void SetCrc(long value)
	{
		crc = value;
	}

	public async Task<long> GetFileLenAsync()
	{
		_ = fileLen;
		return fileLen;
	}

	public void SetFileLen(long value)
	{
		fileLen = value;
	}

	public FileInfoReTryDownloadBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<FileInfoReTryDownloadBusiness> GetAsync(string contextGuid, string name, string subDirectory, FileManagement_Parts part, CancellationToken token, string tranNme)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfoReTryDownload.GetAsync(name, subDirectory, part, token, tranNme);
	}
}
