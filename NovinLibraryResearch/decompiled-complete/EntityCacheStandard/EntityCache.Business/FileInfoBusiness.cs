using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Entities;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;

namespace EntityCache.Business;

public class FileInfoBusiness : BaseBusiness<FileInfoBusiness>, IFileInfo, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<FileInfoBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _fileName;

	private string _fullPath = "";

	private long crc;

	private long fileLen;

	private bool status;

	public static int DbCount { get; }

	public Guid Guid { get; set; }

	[JsonIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).GlobalDatabaseManager;

	public string UniqueKey => $"{Part}//{FileName}//{SubDirectory}";

	public DateTime Modified { get; set; } = DateTime.Now;


	public FileManagement_Parts Part { get; set; }

	public string SubDirectory { get; set; } = "";


	public byte[]? FileContent { get; set; }

	public ENCompressedMode CompressMode { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public bool Status
	{
		get
		{
			return status;
		}
		set
		{
			status = value;
		}
	}

	public Guid EntityGuid { get; set; }

	public string Url { get; set; }

	public string FileName
	{
		get
		{
			return _fileName;
		}
		set
		{
			_fileName = value.FixString();
		}
	}

	public long Crc
	{
		get
		{
			return crc;
		}
		set
		{
			crc = value;
		}
	}

	public long FileLen
	{
		get
		{
			try
			{
				if (Status && FileContent != null)
				{
					fileLen = FileContent.Length;
					return fileLen;
				}
				return fileLen;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "Name:" + FileName, "FileLen", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 72);
				return 0L;
			}
		}
		set
		{
			fileLen = value;
		}
	}

	public override string ContextGuid { get; set; }

	public FileInfoBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public string GetLocalFilePath()
	{
		using (new JaegerMonitor("GetLocalFilePath", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs"))
		{
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (cache.Source != ENSource.Android_HesabYar)
				{
					throw new NotImplementedException();
				}
				string text = Path.Combine(cache.ImagePath, Part.GetLocalFileAttribute());
				FileUtility.CreateDirectory(text);
				string text2 = Path.Combine(text, FileName);
				if (!File.Exists(text2))
				{
					return "";
				}
				return text2;
			}
			catch (NotImplementedException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"FileName:{FileName}//Part:{Part}", "GetLocalFilePath", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 99);
				return "";
			}
		}
	}

	public ReturnedSaveFuncInfo CheckValidation(EnSystem system, short autoFixValidationErrorsCount)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (Modified > DateTime.Now)
			{
				returnedSaveFuncInfo.AddError(system, $"Modified({Modified}) of file is greater than current time({DateTime.Now})", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 108);
			}
			if (Modified.Year < checked(DateTime.Now.Year - 10))
			{
				returnedSaveFuncInfo.AddError(system, $"Modified({Modified}) is too small!!", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 109);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 113);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 114);
		}
		return returnedSaveFuncInfo;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, short autoFixValidationErrorsCount, bool sendAfterSave, Priority priority = Priority.Low, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", bool awair4send = false, CancellationToken token = default(CancellationToken))
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool fileSaved = false;
		try
		{
			_ = 6;
			ReturnedSaveFuncInfo result;
			try
			{
				FileInfoBusiness fileInfoBusiness = await GetAsync(ContextGuid, Guid, token);
				if (fileInfoBusiness != null && fileInfoBusiness.status == status && Crc == fileInfoBusiness.Crc)
				{
					result = ret;
					goto end_IL_017a;
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "FileInfoBusiness/SaveAsync", 60000, token))
				{
					if (Part == FileManagement_Parts.PicPrd || Part == FileManagement_Parts.PicPrdOthers || Part == FileManagement_Parts.PicPrdGroup || Part == FileManagement_Parts.PicCustomer || Part == FileManagement_Parts.PicSign || Part == FileManagement_Parts.Lk)
					{
						AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
						await ClsCache.GetCache(ContextGuid).Resizer.ReSizeAsync(this, accGlobalSettings.Others.MaxImageSize, accGlobalSettings.Others.jpegQuality);
					}
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 143);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckVlidationAsync(system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 144);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (autoTran)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 146), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 146);
							}
							transactionHasBeenCreated = true;
							Modified = DateTime.Now;
							ret.AddReturnedValue(CheckValidation(system, autoFixValidationErrorsCount), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 149);
							if (!ret.HasError)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IFileInfo.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 151);
								fileSaved = true;
								goto end_IL_0267;
							}
							result = ret;
						}
					}
					goto end_IL_017a;
					end_IL_0267:;
				}
				goto end_IL_00e1;
				end_IL_017a:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 154);
				goto end_IL_00e1;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 157);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 158);
				goto end_IL_00e1;
			}
			return result;
			end_IL_00e1:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 163), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 163);
			}
			if (sendAfterSave && !ret.HasError && fileSaved)
			{
				if (awair4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 166);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	private Task<ReturnedSaveFuncInfo> CheckVlidationAsync(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (Status && FileContent != null)
			{
				byte[]? fileContent = FileContent;
				if (fileContent != null && fileContent.Length > 10000000)
				{
					returnedSaveFuncInfo.AddError(system, "حجم فایل بیشتر از حد مجاز می باشد".GetTranslate(), "", "CheckVlidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 178);
				}
			}
			if (Guid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError(system, "شناسه فایل نمی تواند تهی باشد".GetTranslate(), "Guid", "CheckVlidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 181);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckVlidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 186);
			returnedSaveFuncInfo.AddError(system, ex.Message, "Guid", "CheckVlidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 187);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public async Task<WebFileInfo> ToWebItemAsync()
	{
		try
		{
			CancellationToken cancellationToken = cancellationToken;
			return await FileInfoMapper.Instance.MapAsync(ContextGuid, this, new WebFileInfo(), "", cancellationToken);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 202);
			return null;
		}
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 214), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 214);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IFileInfo.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 216);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 220);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 221);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 226), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 226);
			}
		}
		return ret;
	}

	public string Random()
	{
		return Guid.NewGuid().ToString();
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, short autoFixValidationErrorsCount, IEnumerable<FileInfoBusiness> items, bool send2Server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (items == null || !items.Any())
			{
				return ret;
			}
			foreach (FileInfoBusiness item in items.OrderBy((FileInfoBusiness p) => p.status))
			{
				if (item != null)
				{
					item.Modified = DateTime.Now;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, autoFixValidationErrorsCount, send2Server, priority, awaitOnSemaphore: true, tryCount, tranName, awair4send: false, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 241);
					if (ret.HasError)
					{
						return ret;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 247);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 248);
		}
		return ret;
	}

	public static async Task<IEnumerable<FileInfoBusiness>> GetWrongExtentionsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetWrongExtentionsAsync(token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetWrongExtentionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 262);
			return null;
		}
	}

	[Obsolete("Apply access implemented on client side", true)]
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 276), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 276);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 278);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 282);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 283);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 288), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 288);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DecompressZipedImagesAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<FileInfoBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetAllCompressedImageAsync(token, "");
			if (list == null || !list.Any())
			{
				return ret;
			}
			if (list != null && list.Any())
			{
				foreach (FileInfoBusiness zipedFile in list)
				{
					if (zipedFile.CompressMode == ENCompressedMode.GZip)
					{
						FileInfoBusiness fileInfoBusiness = zipedFile;
						fileInfoBusiness.FileContent = await GZip.Decompress(zipedFile.FileContent);
						zipedFile.CompressMode = ENCompressedMode.None;
						zipedFile.ServerStatus = ServerStatusForClient.None;
						zipedFile.Modified = DateTime.Now;
						await zipedFile.SaveAsync(system, 0, sendAfterSave: true, Priority.Low, awaitOnSemaphore: true, 10, "", awair4send: false, token);
						continue;
					}
					throw new NotImplementedException($"Unknows zip mode ({zipedFile.CompressMode})=>Guid:{zipedFile.Guid}");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DecompressZipedImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 316);
			ret.AddReturnedValue(ex, "", "DecompressZipedImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 317);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAllAsync(string contextGuid, IEnumerable<FileInfoBusiness> deleted, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		if (!(deleted?.Any() ?? false))
		{
			return ret;
		}
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				CancellationToken token = default(CancellationToken);
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "FileInfoBusiness/RemoveAllAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 333);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 334), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 334);
						}
						transactionHasBeenCreated = true;
						if (deleted?.Any() ?? false)
						{
							foreach (FileInfoBusiness item in deleted)
							{
								if (item == null)
								{
									continue;
								}
								ClsCache cache = ClsCache.GetCache(contextGuid);
								await cache.CacheRepository.IFileInfo.RemoveAsync(item.Guid, tryCount, token, tranName);
								if (cache.Source != ENSource.Android_HesabYar)
								{
									continue;
								}
								string localFilePath = item.GetLocalFilePath();
								if (File.Exists(localFilePath))
								{
									try
									{
										File.Delete(localFilePath);
									}
									catch
									{
									}
								}
							}
							goto end_IL_0139;
						}
						result = ret;
					}
					goto end_IL_0096;
					end_IL_0139:;
				}
				goto end_IL_0085;
				end_IL_0096:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 353);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 354);
				goto end_IL_0085;
			}
			return result;
			end_IL_0085:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 359), ReturnedState.None, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 359);
			}
		}
		return ret;
	}

	public static Task<FileInfoBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetAsync(guid, token, tranName);
	}

	public static async Task<List<FileInfoBusiness>> GetSliderSyteFromWebService(string contextGuid, ENSource eNSource, short depId, CancellationToken token, HttpClient httpClient = null)
	{
		_ = 2;
		try
		{
			FileReadEventArgs item = new FileReadEventArgs
			{
				EnSource = eNSource,
				DepId = depId
			};
			List<WebFileInfo> files = new List<WebFileInfo>();
			using (HttpClient client = httpClient ?? new HttpClient())
			{
				client.BaseAddress = new Uri("https://webservice.novinp.ir/Webservice/File/GetAll");
				HttpContent content = new StringContent(Json.ToStringJson(item), Encoding.UTF8, "application/json");
				string text = await (await client.PostAsync(client.BaseAddress, content, token)).Content.ReadAsStringAsync();
				if (text != null)
				{
					files = text.FromJson<List<WebFileInfo>>("GetSliderSyteFromWebService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 382);
				}
			}
			if (files != null && files.Any())
			{
				await FileInfoMapper.Instance.MapListAsync(contextGuid, files, new List<FileInfoBusiness>(), "", token);
				throw new NotImplementedException();
			}
			return null;
		}
		catch (OperationCanceledException)
		{
			return null;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetSliderSyteFromWebService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 394);
			throw;
		}
	}

	public void SetFullPath(string value)
	{
		_fullPath = value;
	}

	public static async Task<List<FileInfoBusiness>> GetAsync(string contextGuid, Guid entityGuid, FileManagement_Parts part, CancellationToken token = default(CancellationToken), string tranName = "", bool justFromCache = false, bool deCompressed = false)
	{
		_ = 1;
		try
		{
			List<FileInfoBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetAsync(entityGuid, part, justFromCache, token, tranName);
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
			{
				return list;
			}
			if (part == FileManagement_Parts.PicCustomer || part == FileManagement_Parts.PicPrd || part == FileManagement_Parts.PicPrdGroup || part == FileManagement_Parts.PicSign || part == FileManagement_Parts.Lk)
			{
				list = list?.Where((FileInfoBusiness p) => p.status && p.FileContent != null)?.ToList();
			}
			if (deCompressed)
			{
				list = await DecompressImagesAsync(list);
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 417);
			return null;
		}
	}

	private static async Task<List<FileInfoBusiness>> DecompressImagesAsync(List<FileInfoBusiness> ret)
	{
		using (new JaegerMonitor("DecompressImagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs"))
		{
			_ = 1;
			try
			{
				foreach (FileInfoBusiness item in ret)
				{
					if (item.CompressMode != 0)
					{
						if (item.CompressMode == ENCompressedMode.GZip)
						{
							FileInfoBusiness fileInfoBusiness = item;
							fileInfoBusiness.FileContent = await GZip.Decompress(item.FileContent);
						}
						else if (item.CompressMode == ENCompressedMode.Brotli)
						{
							FileInfoBusiness fileInfoBusiness = item;
							fileInfoBusiness.FileContent = await Brotli.Decompress(item.FileContent);
						}
						item.CompressMode = ENCompressedMode.None;
					}
				}
				return ret;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "");
				throw ex;
			}
		}
	}

	public static Task<FileInfoBusiness> GetAsync(string contextGuid, string name, string subDirectory, FileManagement_Parts part, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetAsync(name, subDirectory, part, token, tranName);
	}

	[DebuggerNonUserCode]
	public Task<ReturnedSaveFuncInfo> IsValidAsync(EnSystem system, IImageResizer resizer)
	{
		using (new JaegerMonitor("IsValidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs"))
		{
			return Task.FromResult(new ReturnedSaveFuncInfo());
		}
	}

	public override Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IFileInfo;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 1;
			try
			{
				if (autoTran)
				{
					line = 1;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 509), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 509);
					transactionHasBeenCreated = true;
				}
				line = 2;
				ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await ClsCache.GetCache(ContextGuid).CacheRepository.IFileInfo.SetSaveResult(item, token, tryCount, tranName);
				line = 3;
				ret.AddReturnedValue(returnedSaveFuncInfo2, ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 515);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 517);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 520);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 521);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 526), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 526);
			}
		}
		return ret;
	}

	public static Task<FileInfoBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<FileInfoBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Files;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال فایل ها".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Others;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 550);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست !!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 552);
				}
				line = 50;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 554);
				}
				line = 51;
				if (ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo == null)
				{
					line = 52;
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IFileInfo is null");
				}
				line = 53;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 54;
				if (uiUpdater != null)
				{
					line = 55;
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 60;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 570);
				}
				line = 61;
				if (list == null || !list.Any())
				{
					return ret.AddInformation("فایل ها سینک هستند تعداد فایل سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 572);
				}
				line = 62;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 574);
				}
				line = 63;
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				line = 64;
				int current = 0;
				line = 65;
				_ = (decimal)(list?.Count ?? 0);
				line = 66;
				list = list?.OrderBy((Guid p) => Guid.NewGuid()).ToList();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						line = 67;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 587);
						}
						line = 68;
						current = checked(current + 1);
						line = 69;
						FileInfoBusiness fileInfoBusiness = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						line = 70;
						if (uiUpdater != null)
						{
							line = 71;
							uiUpdater.ShortMessage = "فایل جاری".GetTranslate() + ": " + fileInfoBusiness.FileName;
							line = 72;
							uiUpdater.Count = current;
							line = 73;
						}
						line = 74;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 602);
						}
						line = 75;
						if (fileInfoBusiness.ServerStatus == ServerStatusForClient.Delivered || fileInfoBusiness.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							line = 76;
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Tafsil \r\n" + $"EntityGuid:{fileInfoBusiness?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 608);
							line = 77;
							continue;
						}
						line = 78;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 615);
						}
						line = 79;
						tasks.Add(fileInfoBusiness.Send2ServerAsync(token, EnSystem.SignalR));
						line = 80;
						if (tasks.Count >= maxParalelTask)
						{
							line = 81;
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							line = 82;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 624);
							line = 83;
							tasks.Remove(t);
						}
						line = 84;
					}
				}
				line = 85;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 631);
				line = 86;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 633);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 635);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 638);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 639);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "FileInfoBusiness/Send2ServerAsync", 60000, token, token2);
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 670);
				}
				byte[]? fileContent = FileContent;
				if (((fileContent != null) ? fileContent.Length : 0) > 51200)
				{
					AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
					await ClsCache.GetCache(ContextGuid).Resizer.ReSizeAsync(this, accGlobalSettings.Others.MaxImageSize, accGlobalSettings.Others.jpegQuality);
				}
				List<WebFileInfo> list = new List<WebFileInfo>();
				List<WebFileInfo> list2 = list;
				list2.Add(await FileInfoMapper.Instance.MapAsync(ContextGuid, this, new WebFileInfo(), "", token));
				List<Packet> lstPacket = WebFileInfo.GenerateSaveRequest(list, "FileInfoBusiness.Send2ServerAsync CacheType: #" + cache.Source);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 683);
				}
				using FileInfoWaiter waiter = new FileInfoWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("FileInfoBusinessWaiter"), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 686);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 688);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 691);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 692);
			}
			return ret;
		}
	}

	public void ClearFile()
	{
		try
		{
			Status = false;
			FileContent = null;
			ServerStatus = ServerStatusForClient.None;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ClearFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 706);
			throw;
		}
	}

	public static Task<FileInfoBusiness> GetNoImageAsync(string contextGuid, FileManagement_Parts part, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetNoImageAsync(part, token, tranName);
	}

	public static Task<List<Guid>> GetAllGuidListAsync(string contextGuid, FileInfoFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IFileInfo.GetAllGuidListAsync(filter, token, tranName);
	}

	internal static async Task<string> GetDefaultProductUrlAsync(string contextGuid, Guid productGuid, CancellationToken token)
	{
		using (new JaegerMonitor("GetDefaultProductUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs"))
		{
			_ = 1;
			try
			{
				FileInfoBusiness fileInfoBusiness = (await GetAsync(contextGuid, productGuid, FileManagement_Parts.PicPrd, token, "", justFromCache: true))?.SingleOrDefault();
				if (fileInfoBusiness == null)
				{
					fileInfoBusiness = (await GetAsync(contextGuid, productGuid, FileManagement_Parts.PicPrdOthers, token, "", justFromCache: true))?.FirstOrDefault();
				}
				if (fileInfoBusiness == null)
				{
					return "";
				}
				if (fileInfoBusiness.ServerStatus == ServerStatusForClient.None)
				{
					string localFilePath = fileInfoBusiness.GetLocalFilePath();
					if (!string.IsNullOrEmpty(localFilePath))
					{
						return localFilePath;
					}
				}
				return fileInfoBusiness.Url;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetDefaultProductUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\FileInfoBusiness.cs", 734);
			}
			return "";
		}
	}
}
