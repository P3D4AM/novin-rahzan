using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using Force.Crc32;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class UploadFilePartBusiness : Serializable<UploadFilePartBusiness>, IUploadFilePart, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDisposable, IHasContext
{
	private static string subDirectory = null;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 1);

	private bool disposed;

	public string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public long Order { get; set; }

	public Guid FileId { get; set; }

	public short RetryCount { get; set; }

	public bool IsSent { get; set; }

	public DateTime NextTry { get; set; } = DateTime.Now;


	public UploadFilePartBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<byte[]> GetBytesAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IUploadFilePart.GetBytesAsync(Guid, token, tranName);
	}

	public static async Task<IEnumerable<UploadFilePartBusiness>> GetLastFilePartAsync(string contextGuid, int top, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			bool semaphoreHasBeenTaken = false;
			IEnumerable<UploadFilePartBusiness> result;
			try
			{
				_ = 4;
				try
				{
					await _connectionPoolSemaphore.WaitAsync(token);
					semaphoreHasBeenTaken = true;
					using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "UploadFilePartBusiness/GetLastFilePartAsync", 60000, token);
					if (dis.Token.IsCancellationRequested)
					{
						ret.AddErrorTokenIsCancelled("", "", "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 57);
						result = null;
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 60), ReturnedState.None, "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 60);
						}
						transactionHasBeenCreated = true;
						IEnumerable<UploadFilePartBusiness> lst = await ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.GetLastFilePartAsync(top, status, token, tranName);
						if (lst?.Any() ?? false)
						{
							foreach (UploadFilePartBusiness item in lst)
							{
								if (!token.IsCancellationRequested)
								{
									if (item != null)
									{
										await item.SetRetryAsync(DateTime.Now.AddMinutes(5.0), token, tranName);
									}
									continue;
								}
								result = new List<UploadFilePartBusiness>();
								goto end_IL_01b5;
							}
						}
						result = lst;
					}
					end_IL_01b5:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"autoTran:{autoTran}", "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 74);
					throw;
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 80), ReturnedState.None, "GetLastFilePartAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 80);
				}
				if (semaphoreHasBeenTaken)
				{
					_connectionPoolSemaphore.Release();
				}
			}
			return result;
		}
	}

	private Task SetRetryAsync(DateTime addMinutes, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IUploadFilePart.SetRetryAsync(addMinutes, Guid, token, tranName);
	}

	private async Task<string> GetSubDirectoryAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetSubDirectoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs"))
		{
			try
			{
				if (subDirectory == null)
				{
					subDirectory = (await UploadFileBusiness.GetSubDirectoryAsync(ContextGuid, FileId, token)) ?? "";
				}
				return subDirectory;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSubDirectoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 99);
				throw;
			}
		}
	}

	public async Task<Packet> CreatePacketAsync(string tempPath = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CreatePacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (!File.Exists(Path.Combine(tempPath, Guid.ToString() + ".tmp")))
				{
					return null;
				}
				byte[] array = (await GetBytesAsync(token)) ?? File.ReadAllBytes(Path.Combine(tempPath, Guid.ToString() + ".tmp"));
				WebFileInfo file = new WebFileInfo
				{
					FileName = Guid.ToString() + ".tmp",
					Crc = Crc32Algorithm.Compute(array),
					FileContent = array
				};
				WebFileInfo webFileInfo = file;
				webFileInfo.SubDirectory = await GetSubDirectoryAsync(token);
				webFileInfo = file;
				webFileInfo.Part = (await UploadFileBusiness.GetByFileIdAsync(ContextGuid, FileId, token))?.FileManagementParts ?? FileManagement_Parts.None;
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				return WebFileInfo.GenerateWriteRequest(file, "UploadFilePartBusiness.CreatePacketAsync _ CacheSource: #" + cache.Source);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CreatePacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 143);
				throw;
			}
		}
	}

	public static Task<UploadFilePartBusiness> GetAsync(string contextGuid, Guid parse, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.GetAsync(parse, token, tranName);
	}

	public static async Task<Guid> MarkAsSentAsync(string contextGuid, Guid filePart, ClientHub.Buffer buffer, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		int line = 1;
		using (new JaegerMonitor("MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool awaitOnSemaphore = true;
			ReturnedSaveFuncInfo retValue = new ReturnedSaveFuncInfo();
			bool transactionHasBeenCreated = false;
			Guid result;
			try
			{
				_ = 6;
				try
				{
					using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TafsilBusiness/SaveAsync", 60000, token);
					if (dis.Token.IsCancellationRequested)
					{
						result = Guid.Empty;
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = retValue;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 164), ReturnedState.None, "MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 164);
						}
						retValue.throwExceptionIfError();
						transactionHasBeenCreated = true;
						line = 2;
						await SetSentAsync(contextGuid, filePart, tryCount, token, tranName);
						line = 3;
						Guid fileId = await GetFileIdAsync(contextGuid, filePart, token, tranName);
						line = 4;
						if (fileId == Guid.Empty)
						{
							result = Guid.Empty;
						}
						else
						{
							line = 5;
							if (!(await UploadFileBusiness.IsSentCompleteAsync(contextGuid, fileId, token, tranName)))
							{
								result = fileId;
							}
							else
							{
								line = 6;
								UploadFileBusiness ret = await UploadFileBusiness.GetByFileIdAsync(contextGuid, fileId, token, tranName);
								line = 7;
								if (ret == null)
								{
									result = Guid.Empty;
								}
								else
								{
									line = 8;
									Packet packet = WebFileInfo.GenerateCommitRequest(await GetFileOrderAsync(contextGuid, fileId, token, tranName), Path.GetFileName(ret.Name), ret.FileManagementParts, ret.Guid, ret.Crc, ret.EntityGuid, ret.Guid);
									line = 9;
									if (packet != null)
									{
										buffer.SendData(packet, Priority.Medium);
									}
									line = 10;
									ret.Dispose();
									line = 11;
									result = fileId;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					retValue.AddReturnedValue(ex, "", "MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 196);
					WebErrorLog.Instance.StartLog(ex, $"Error in Line {line}", "MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 197);
					result = Guid.Empty;
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = retValue;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, retValue.HasError, disposeContext: true, "MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 203), ReturnedState.None, "MarkAsSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 203);
				}
			}
			return result;
		}
	}

	private static Task<Guid> GetFileIdAsync(string contextGuid, Guid filePart, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.GetFileIdAsync(filePart, token, tranName);
	}

	private static async Task<ReturnedSaveFuncInfo> SetSentAsync(string contextGuid, Guid filePart, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.SetSentAsync(filePart, tryCount, token, tranName), ReturnedState.None, "SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 215);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 219);
			ret.AddReturnedValue(ex, "", "SetSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 220);
		}
		return ret;
	}

	public static Task<List<string>> GetFileOrderAsync(string contextGuid, Guid fileId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.GetFileOrderAsync(fileId, token, tranName);
	}

	public static Task ResetFileAsync(string contextGuid, Guid fileId, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranNmae = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.ResetSentAsync(fileId, tryCount, token, tranNmae);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(byte[] bytes, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		int line = 1;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				line = 3;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "UploadFilePartBusinesss/SaveAsync", 60000, token))
				{
					line = 4;
					if (!dis.Token.IsCancellationRequested)
					{
						line = 5;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 245), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 245);
						}
						line = 6;
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IUploadFilePart.SaveAsync(this, bytes, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 248);
						line = 7;
						goto end_IL_0130;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 243);
					goto end_IL_008b;
					end_IL_0130:;
				}
				goto end_IL_007a;
				end_IL_008b:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Error Line IS Line {line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 253);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 254);
				goto end_IL_007a;
			}
			return result;
			end_IL_007a:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 259), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 259);
			}
		}
		return ret;
	}

	public new void Dispose()
	{
		Dispose(disposing: true);
	}

	protected new virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			disposed = true;
		}
	}

	public static Task<int> GetTotalPartCountAsync(string contextGuid, Guid fileId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.GetTotalPartCountAsync(fileId, token, tranName);
	}

	public static Task<int> GetPartSentCountAsync(string contextGuid, Guid fileId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.GetPartSentCountAsync(fileId, token, tranName);
	}

	public static Task<IEnumerable<UploadFileBusiness>> GetFileQueueAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IUploadFile.GetAllAsync(status, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAllAsync(string contextGuid, Guid fileId, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				line = 1;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 297), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 297);
				}
				ret.throwExceptionIfError();
				transactionHasBeenCreated = true;
				line = 2;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IUploadFilePart.RemoveAllAsync(fileId, tryCount, token, tranName), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 301);
				line = 3;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"(fileid,tryCount,tranName)\\Line:{line}\\ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(contextGuid)?.DatabaseManager == null}", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 306);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 307);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 312), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Upload\\UploadFilePartBusiness.cs", 312);
			}
		}
		return ret;
	}
}
