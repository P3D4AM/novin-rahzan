using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class BackupLogBusiness : Serializable<BackupLogBusiness>, IBackupLog, IHasContext
{
	private bool? _UrlExists;

	public Guid Id { get; set; }

	public Guid DPId { get; set; }

	public Guid FileId { get; set; }

	public DateTime LastBackup { get; set; } = DateTime.Now;


	public BackupState Status { get; set; }

	public string Path { get; set; }

	public DateTime LastStateChange { get; set; } = DateTime.Now;


	public string Message { get; set; }

	public short RetryCount { get; set; }

	public long TotalCrc { get; set; }

	public string URL { get; set; }

	public string FileName { get; set; }

	public BackupType BackupType { get; set; }

	public bool IsDelete { get; set; }

	[JsonIgnore]
	public string DateSh => LastBackup.MiladiToCalture();

	[JsonIgnore]
	public string FileSize
	{
		get
		{
			try
			{
				if (PathExists)
				{
					decimal num = new FileInfo(FileName).Length;
					if (num < 1024m)
					{
						return num + "Byte";
					}
					if (num < 1048576m)
					{
						return Math.Round(num / 1024m, DigitForRound).ToString().Replace(".", "/") + "KB";
					}
					if (num < 1073741824m)
					{
						return Math.Round(num / 1048576m, DigitForRound).ToString().Replace(".", "/") + "MB";
					}
					return Math.Round(num / 1073741824m, DigitForRound).ToString().Replace(".", "/") + "GB";
				}
				return "-";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FileSize", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 59);
				throw;
			}
		}
	}

	[JsonIgnore]
	public bool PathExists => File.Exists(FileName);

	[JsonIgnore]
	public bool UrlExists
	{
		get
		{
			try
			{
				if (_UrlExists.HasValue && _UrlExists.Value)
				{
					return true;
				}
				if (_UrlExists.HasValue && !_UrlExists.Value)
				{
					return false;
				}
				_UrlExists = CheckURL(URL);
				return _UrlExists.Value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UrlExists", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 84);
				throw;
			}
		}
	}

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public BackupLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		DigitForRound = ClsCache.GetCache(ContextGuid).MultiLanguageConfig?.DigitForRound ?? 3;
	}

	public static bool CheckURL(string URL)
	{
		using (new JaegerMonitor("CheckURL", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(URL))
				{
					return false;
				}
				HttpWebRequest obj = WebRequest.Create(URL) as HttpWebRequest;
				obj.Method = "HEAD";
				HttpWebResponse obj2 = obj.GetResponse() as HttpWebResponse;
				obj2.Close();
				return obj2.StatusCode == HttpStatusCode.OK;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckURL", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 116);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAll(string contextGuid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 130), ReturnedState.None, "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 130);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IBackupLog.RemoveAllAsync(tryCount, token, tranName), ReturnedState.None, "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 132);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 136);
				ret.AddReturnedValue(ex, "", "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 137);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 142), ReturnedState.None, "RemoveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 142);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<BackupLogBusiness>> GetAllAsync(string contextGuid, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBackupLog.GetAllAsync(token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 156), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 156);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IBackupLog.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 158);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 162);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 163);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 169), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\BackupLogBusiness.cs", 169);
			}
		}
		return ret;
	}

	public static Task<BackupLogBusiness> GetAsync(string contextGuid, Guid fileId, long crc, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IBackupLog.GetAsync(fileId, crc, token, tranName);
	}
}
