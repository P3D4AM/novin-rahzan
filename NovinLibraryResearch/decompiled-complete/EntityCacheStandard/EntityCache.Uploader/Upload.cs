using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ClientHub;
using EntityCache.Business;
using EntityCache.Uploader.Events;
using Force.Crc32;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Uploader;

public class Upload : INpDisposable, IDisposable
{
	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private TaskCompletionSource<bool> _NewFileForUploadIsReady = new TaskCompletionSource<bool>();

	private ClientHub.Buffer buffer;

	private UploadConfig _uploadConfig;

	private bool _inited;

	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private System.Timers.Timer _checkFetchAndSend;

	private UploadConfig _config;

	private string _tempPath = "";

	private bool bussyOnwrite;

	public WebContextHolder Context { get; private set; }

	public string ContextGuid => Context.Guid;

	public int MinChunking
	{
		get
		{
			try
			{
				if ((_config?.MaxChunkKb ?? 0) <= 0 || (_config?.MaxChunkKb ?? 0) > 20480)
				{
					return 524288;
				}
				return checked(_config.MaxChunkKb * 1024);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MinChunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 50);
				return 102400;
			}
		}
	}

	public int MaxChunking
	{
		get
		{
			try
			{
				return checked(_config.MinChunkKb * 1024);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MaxChunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 66);
				return 102400;
			}
		}
	}

	public bool IsDisposed { get; private set; }

	public event Func<object, FileSendProgressEventArgs, Task> OnProgressChange;

	public event Func<object, FileSentCompitedEventArgs, Task> OnSentComplete;

	public event Func<object, LoginResponseEventArgs, Task> OnBufferLoggedIn;

	public event Func<object, BaseEventArgs, Task> OnBufferConnected;

	public event Func<object, BaseEventArgs, Task> OnBufferDisConnected;

	public static event Func<object, LogEventArgs, Task> OnLogFired;

	public Upload(WebContextHolder context)
	{
		Context = context;
	}

	public void Init(UploadConfig config)
	{
		using (new JaegerMonitor("Init", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			try
			{
				if (_inited)
				{
					return;
				}
				_config = config;
				_config.TempPath = Path.Combine(_config.TempPath, "TempParts");
				if (config?.BufferClient != null)
				{
					_uploadConfig = config;
				}
				if (config == null || _uploadConfig.BufferClient == null)
				{
					return;
				}
				if (_uploadConfig.MinChunkKb < 1)
				{
					throw new ArgumentNullException("MinChunkKb", "MinChunk size is 0 or smaller than zero");
				}
				if (_uploadConfig.MaxChunkKb < 1)
				{
					throw new ArgumentException("maxChunk is 0 or smaller than zero", "MaxChunkKb");
				}
				if (config.MinChunkKb >= config.MaxChunkKb)
				{
					throw new ArgumentException("minChunk larger than or equal to maxChunk", "MinChunkKb");
				}
				_tempPath = config?.TempPath ?? "";
				if (!Directory.Exists(_tempPath))
				{
					Directory.CreateDirectory(_tempPath);
				}
				_inited = true;
				SetBuffer(config.BufferClient);
				if (config != null && config.BufferClient?.LastLoginState.Status == EnLoginResult.Success)
				{
					RaiseEvent("Task FetchAndSent Canceled From Init");
					_sendToken?.Cancel();
					_sendToken = new CancellationTokenSource();
					CancellationToken token = _sendToken.Token;
					Task.Run(() => FetchAndSend(token));
				}
				_checkFetchAndSend = new System.Timers.Timer
				{
					Enabled = true,
					AutoReset = true,
					Interval = 1800000.0
				};
				_checkFetchAndSend.Elapsed += _checkFetchAndSend_Elapsed;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 131);
			}
		}
	}

	private void _checkFetchAndSend_Elapsed(object sender, ElapsedEventArgs e)
	{
		try
		{
			if (buffer.LastLoginState.Status != EnLoginResult.Success)
			{
				return;
			}
			_sendToken?.Cancel();
			_sendToken = new CancellationTokenSource();
			CancellationToken token = _sendToken.Token;
			_checkFetchAndSend.Stop();
			Task.Run(async delegate
			{
				_ = 1;
				try
				{
					await Task.Delay(60000);
					if (!token.IsCancellationRequested)
					{
						await FetchAndSend(token);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "_checkFetchAndSend_Elapsed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 151);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_checkFetchAndSend_Elapsed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 154);
		}
		finally
		{
			_checkFetchAndSend.Start();
		}
	}

	public async Task<ReturnedSaveFuncInfo> FetchAndSend(CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (ClsCache.GetCache(Context.Guid).Source == ENSource.Android_HesabYar)
		{
			return ret;
		}
		int line = 0;
		await _semaphore.WaitAsync();
		try
		{
			while (!token.IsCancellationRequested)
			{
				if (token.IsCancellationRequested || IsDisposed || (buffer?.CancellationToken.IsCancellationRequested ?? true))
				{
					return ret.AddErrorTokenIsCancelled("", "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 175);
				}
				try
				{
					if (token.IsCancellationRequested || IsDisposed || (buffer?.CancellationToken.IsCancellationRequested ?? true))
					{
						return ret.AddErrorTokenIsCancelled("", "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 182);
					}
					line = 30;
					IEnumerable<UploadFilePartBusiness> enumerable = await UploadFilePartBusiness.GetLastFilePartAsync(ContextGuid, 10, token: token, status: true);
					if (enumerable == null || !enumerable.Any())
					{
						await Task.Delay(60000);
						continue;
					}
					line = 31;
					if (token.IsCancellationRequested || IsDisposed || (buffer?.CancellationToken.IsCancellationRequested ?? true))
					{
						return ret.AddErrorTokenIsCancelled("", "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 187);
					}
					line = 4;
					try
					{
						if (token.IsCancellationRequested || IsDisposed || (buffer?.CancellationToken.IsCancellationRequested ?? true))
						{
							return ret.AddErrorTokenIsCancelled("", "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 191);
						}
						line = 6;
						_NewFileForUploadIsReady = new TaskCompletionSource<bool>();
						if (token.IsCancellationRequested || IsDisposed || (buffer?.CancellationToken.IsCancellationRequested ?? true))
						{
							return ret.AddErrorTokenIsCancelled("", "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 194);
						}
						await Task.WhenAny(new Task[2]
						{
							Task.Delay(1800000),
							_NewFileForUploadIsReady.Task
						});
						line = 7;
					}
					catch (ArgumentOutOfRangeException)
					{
					}
				}
				catch (Exception ex2)
				{
					ret.AddReturnedValue(ex2, "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 236);
					WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 237);
					if (token.IsCancellationRequested || IsDisposed || (buffer?.CancellationToken.IsCancellationRequested ?? true))
					{
						return ret.AddErrorTokenIsCancelled("", "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 238);
					}
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 244);
		}
		catch (Exception ex3)
		{
			ret.AddReturnedValue(ex3, "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 248);
			WebErrorLog.Instance.StartLog(ex3, "", "FetchAndSend", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 249);
		}
		finally
		{
			_semaphore.Release();
		}
		return ret;
	}

	public async Task<Guid> AddToQueue(IEnumerable<byte> bytes, string fileName, FileManagement_Parts FileManagementParts, string subDirectory)
	{
		int line = 0;
		using (new JaegerMonitor("AddToQueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			_ = 5;
			try
			{
				if (bytes == null || !bytes.Any())
				{
					throw new ArgumentException("bytes is null or empty", "bytes");
				}
				line = 1;
				if (string.IsNullOrEmpty(fileName))
				{
					throw new ArgumentException("filename is null or empty", "fileName");
				}
				line = 2;
				string extension = new FileInfo(fileName).Extension;
				if (extension != null && extension.Length == 0)
				{
					throw new ArgumentException("The file does not contain extension", "fileName");
				}
				line = 3;
				long crc = Crc32Algorithm.Compute(bytes.ToArray());
				line = 4;
				UploadFileBusiness obj = await UploadFileBusiness.GetAsync(ContextGuid, crc);
				line = 5;
				if (obj != null)
				{
					return Guid.Empty;
				}
				line = 6;
				List<List<byte>> chunkedList = bytes.ChunkList(MaxChunking / 1024, MaxChunking);
				line = 7;
				UploadFileBusiness dbFile = new UploadFileBusiness(ContextGuid)
				{
					Name = fileName,
					Crc = crc,
					Guid = Guid.NewGuid(),
					FileManagementParts = FileManagementParts,
					MaxChunk = MaxChunking,
					MinChunk = MinChunking,
					SubDirectory = subDirectory,
					PartCount = chunkedList.Count
				};
				if ((await dbFile.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, 10)).HasError)
				{
					line = 10;
					RaiseEvent("File " + fileName + " not insert to database");
					throw new Exception("fetal error cannot insert to table file");
				}
				line = 12;
				RaiseEvent("File " + fileName + " insert to database");
				Guid fileid = (await UploadFileBusiness.GetAsync(ContextGuid, dbFile.Crc)).Id;
				line = 15;
				if (!(await ChunkingAsync(chunkedList, fileid, Guid.Empty)))
				{
					line = 20;
					RaiseEvent("File " + fileName + " error in chunking");
					return Guid.Empty;
				}
				line = 25;
				bytes = null;
				int count = 0;
				line = 30;
				_NewFileForUploadIsReady.TrySetResult(result: true);
				line = 35;
				while (await UploadFileBusiness.GetByFileIdAsync(ContextGuid, fileid) != null)
				{
					line = 40;
					if (count > 300)
					{
						return Guid.Empty;
					}
					await Task.Delay(1000);
					count = checked(count + 1);
				}
				line = 45;
				return fileid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"AddToQueue(IEnumerable<byte> bytes, string fileName, PacketParser.Entities.FileManagement_Parts FileManagementParts, string subDirectory)//Line:{line}", "AddToQueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 326);
				return Guid.Empty;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<UploadFileBusiness>> AddToQueue(string path, FileManagement_Parts fileManagementParts, Guid entityGuid)
	{
		using (new JaegerMonitor("AddToQueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfoWithValue<UploadFileBusiness> ret = new ReturnedSaveFuncInfoWithValue<UploadFileBusiness>();
			try
			{
				EventLogger.NewInstance(EventLoggerType.Upload, "افزودن فایل به صف ارسال", ReturnedState.Success, "", entityGuid.ToString());
				line = 1;
				line = 2;
				if (!File.Exists(path))
				{
					throw new ArgumentException("File not exist", "path");
				}
				line = 3;
				RaiseEvent("File " + Path.GetFileName(path) + " insert to database");
				line = 4;
				ReturnedSaveFuncInfoWithValue<UploadFileBusiness> returnedSaveFuncInfoWithValue = await Chunking(path, fileManagementParts, Guid.NewGuid(), entityGuid);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "AddToQueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 348);
				line = 5;
				_NewFileForUploadIsReady.TrySetResult(result: true);
				line = 6;
				if (returnedSaveFuncInfoWithValue?.value == null)
				{
					RaiseEvent("File " + Path.GetFileName(path) + " error in chunking");
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"AddToQueue(string path, FileManagement_Parts fileManagementParts, Guid entityGuid)//Line:{line}", "AddToQueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 358);
				EventLogger.NewInstance(EventLoggerType.Upload, "خطا در افزودن فایل به صف ارسال", ReturnedState.Error, "", entityGuid.ToString(), ex.Message);
				ret.AddReturnedValue(ex, "", "AddToQueue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 360);
			}
			return ret;
		}
	}

	public async Task RestAsync()
	{
	}

	private void SetBuffer(ClientHub.Buffer cBuffer)
	{
		int num = 1;
		try
		{
			buffer = cBuffer;
			num = 2;
			buffer.Connected -= Buffer_Connected;
			num = 3;
			buffer.Connected += Buffer_Connected;
			num = 4;
			buffer.Disconnected -= Buffer_Disconnected;
			num = 5;
			buffer.Disconnected += Buffer_Disconnected;
			num = 6;
			buffer.LoggedIn -= Buffer_LoggedIn;
			num = 7;
			buffer.LoggedIn += Buffer_LoggedIn;
			num = 8;
			cBuffer.Context.WebFileInfo.OnCommitResponse += FileInfoInstance_OnCommitResponse;
			num = 9;
			cBuffer.Context.WebFileInfo.OnWriteResponse += FileInfoInstance_OnWriteResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Error in Line {num}", "SetBuffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 402);
		}
	}

	private async Task FileInfoInstance_OnWriteResponse(object sender, FileWriteResponseEventArgs e)
	{
		using (new JaegerMonitor("FileInfoInstance_OnWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			_ = 3;
			try
			{
				while (bussyOnwrite)
				{
					await Task.Delay(100);
				}
				bussyOnwrite = true;
				if (e.PartNames != null && e.PartNames.Count > 0)
				{
					Guid filePart = Guid.Parse(e.PartNames[0].Replace(".tmp", ""));
					Guid file = await UploadFilePartBusiness.MarkAsSentAsync(ContextGuid, filePart, buffer, 10);
					Func<object, FileSendProgressEventArgs, Task> onProgressChange = this.OnProgressChange;
					object sender2 = this;
					await PacketParser.Utilities.RaiseEvent(onProgressChange, sender2, await FileSendProgressEventArgs.GetFileSendProgressEventArgsAsync(ContextGuid, file, success: true), e.Connection.cancellationToken, 60, "FileInfoInstance_OnWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FileInfoInstance_OnWriteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 423);
			}
			finally
			{
				bussyOnwrite = false;
			}
		}
	}

	private Task Buffer_LoggedIn(object sender, LoginResponseEventArgs e)
	{
		using (new JaegerMonitor("Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token = _sendToken.Token;
				Task.Run(() => FetchAndSend(token));
				PacketParser.Utilities.RaiseEvent(this.OnBufferLoggedIn, sender, e, buffer?.CancellationToken ?? new CancellationToken(canceled: true), 60, "Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs");
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 443);
			}
			return Task.CompletedTask;
		}
	}

	private Task Buffer_Disconnected(object sender, BaseEventArgs e)
	{
		return Task.Run(() => PacketParser.Utilities.RaiseEvent(this.OnBufferDisConnected, this, e, buffer?.CancellationToken ?? new CancellationToken(canceled: true), 60, "Buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"));
	}

	private Task Buffer_Connected(object sender, BaseEventArgs e)
	{
		return Task.Run(() => PacketParser.Utilities.RaiseEvent(this.OnBufferConnected, this, e, buffer?.CancellationToken ?? default(CancellationToken), 60, "Buffer_Connected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"));
	}

	private async Task FileInfoInstance_OnCommitResponse(object sender, FileCommitResponseEventArgs e)
	{
		checked
		{
			using (new JaegerMonitor("FileInfoInstance_OnCommitResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
			{
				_ = 12;
				try
				{
					bool isBackUpService = ClsCache.GetCache(ContextGuid).Source == ENSource.NPBackupService;
					UploadFileBusiness file = await UploadFileBusiness.GetFileAsync(ContextGuid, e.PacketReferenceId);
					if (file == null)
					{
						return;
					}
					BackupLogBusiness log = null;
					if (isBackUpService)
					{
						EventLogger.NewInstance(EventLoggerType.Upload, "آپلود فایل" + file.Name + "با موفقیت انجام شد", ReturnedState.Success, "", file.EntityGuid.ToString());
						log = (await BackupLogBusiness.GetAsync(ContextGuid, file.Id, file.Crc)) ?? new BackupLogBusiness(e.Connection.Context.Guid)
						{
							Id = Guid.NewGuid()
						};
					}
					if (e.Success)
					{
						if (file.Crc == e.Crc)
						{
							await file.RemoveAsync(10);
							await DeleteTempFile(file.Id);
							await UploadFilePartBusiness.RemoveAllAsync(e.Connection.ContextGuid, file.Id, 10);
							if (isBackUpService)
							{
								EventLogger.NewInstance(EventLoggerType.Upload, "آپلود فایل" + file.Name + "با موفقیت انجام شد", ReturnedState.Success, "", file.EntityGuid.ToString());
								log.URL = e.Url;
								log.LastStateChange = DateTime.Now;
								await log.SaveAsync(10);
							}
						}
						else if (isBackUpService)
						{
							EventLogger.NewInstance(EventLoggerType.Upload, " خطا در آپلود فایل" + file.Name, ReturnedState.Error, "", file.EntityGuid.ToString(), e.Message);
							log.Status = BackupState.Error;
							log.URL = e.Message;
							log.LastStateChange = DateTime.Now;
							await log.SaveAsync(10);
						}
					}
					else if (file.RetryCount > 2)
					{
						await file.RemoveAsync(10);
						await Task.Run(() => DeleteTempFile(file.Id));
						await UploadFilePartBusiness.RemoveAllAsync(e.Connection.ContextGuid, file.Id, 10);
						if (isBackUpService)
						{
							log.Status = BackupState.Error;
							log.Message = e.Message;
							log.LastStateChange = DateTime.Now;
							await log.SaveAsync(10);
						}
					}
					else if (_uploadConfig.RetryOnFault)
					{
						file.RetryCount++;
						file.PartSent = 0;
						await file.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, 10);
						await UploadFilePartBusiness.ResetFileAsync(ContextGuid, file.Id, 10);
					}
					if (isBackUpService)
					{
						EventLogger.NewInstance(EventLoggerType.Upload, "فایل".GetTranslate() + ":" + file.Name + " به سرور ارسال شد.".GetTranslate(), ReturnedState.Success, "", file.EntityGuid.ToString());
						this.OnSentComplete?.Invoke(this, new FileSentCompitedEventArgs(file, e.Url, e.Message));
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "FileInfoInstance_OnCommitResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 534);
				}
			}
		}
	}

	private async Task DeleteTempFile(Guid fileGuid)
	{
		using (new JaegerMonitor("DeleteTempFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			_ = 1;
			try
			{
				string[] lst = Directory.GetFiles(_tempPath);
				List<string> list = await UploadFilePartBusiness.GetFileOrderAsync(ContextGuid, fileGuid);
				if (lst.Length == 0 || !list.Any())
				{
					return;
				}
				foreach (string item in list)
				{
					string tmp = Path.Combine(_tempPath, item);
					if (lst.SingleOrDefault((string w) => w == tmp) != null)
					{
						await DeleteAsync(tmp, 50);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteTempFile", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 554);
			}
		}
	}

	private async Task DeleteAsync(string fileName, short tryCount = 50)
	{
		try
		{
			if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
			{
				File.Delete(fileName);
			}
		}
		catch (Exception ex)
		{
			if (tryCount > 0)
			{
				await Task.Delay(60000);
				Upload upload = this;
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				await upload.DeleteAsync(fileName, num);
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 573);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<UploadFileBusiness>> Chunking(string path, FileManagement_Parts fileManagementParts, Guid guid, Guid entityGuid, int retryCount = 10)
	{
		checked
		{
			using (new JaegerMonitor("Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
			{
				ReturnedSaveFuncInfoWithValue<UploadFileBusiness> ret = new ReturnedSaveFuncInfoWithValue<UploadFileBusiness>();
				try
				{
					ReturnedSaveFuncInfoWithValue<UploadFileBusiness> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await FileUtility.Instance.FixeFileAccessAsync(path, 10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 583);
					ReturnedSaveFuncInfoWithValue<string> accessDeniedPath = await CopyFileIfAccessDeniedAsync(path, default(CancellationToken), 10);
					ret.AddReturnedValue(accessDeniedPath, ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 585);
					long crc = 0L;
					int partCount = 0;
					UploadFileBusiness dbFile = new UploadFileBusiness(ContextGuid)
					{
						Id = guid,
						Name = Path.GetFileName(path),
						Crc = crc,
						Guid = guid,
						FileManagementParts = fileManagementParts,
						MaxChunk = MaxChunking,
						MinChunk = MinChunking,
						SubDirectory = "",
						PartCount = 0,
						EntityGuid = entityGuid
					};
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await dbFile.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 604);
					using (FileStream stream = new FileStream(accessDeniedPath.value, FileMode.Open))
					{
						int chunked = 0;
						byte[] fbuffer = new byte[MaxChunking];
						_ = MaxChunking;
						int num;
						while (chunked < stream.Length && (num = stream.Read(fbuffer, 0, MaxChunking)) != 0)
						{
							chunked += num;
							partCount++;
							if (num < MaxChunking)
							{
								byte[] source = fbuffer;
								_ = new byte[num];
								fbuffer = source.ToList().GetRange(0, num).ToArray();
							}
							Guid guid2 = Guid.NewGuid();
							UploadFilePartBusiness filePart = new UploadFilePartBusiness(ContextGuid)
							{
								FileId = guid,
								Guid = guid2
							};
							if (!string.IsNullOrEmpty(_tempPath))
							{
								string tempPath = _tempPath;
								Guid guid3 = guid2;
								File.WriteAllBytes(Path.Combine(tempPath, guid3.ToString() + ".tmp"), fbuffer);
								returnedSaveFuncInfoWithValue = ret;
								returnedSaveFuncInfoWithValue.AddReturnedValue(await filePart.SaveAsync(null, default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 629);
							}
							else
							{
								returnedSaveFuncInfoWithValue = ret;
								returnedSaveFuncInfoWithValue.AddReturnedValue(await filePart.SaveAsync(fbuffer, default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 633);
							}
							filePart.Dispose();
							crc = Convert.ToInt64(Crc32Algorithm.Append(Convert.ToUInt32(crc), fbuffer));
						}
						stream.Close();
					}
					UploadFileBusiness tmpFile = await UploadFileBusiness.GetAsync(ContextGuid, crc);
					if (tmpFile != null)
					{
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await UploadFilePartBusiness.RemoveAllAsync(ContextGuid, guid, 10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 644);
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await dbFile.RemoveAsync(10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 645);
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await tmpFile.RemoveAsync(10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 646);
						return ret;
					}
					dbFile.Crc = crc;
					dbFile.PartCount = Convert.ToInt32(partCount);
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await dbFile.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 653);
					if (accessDeniedPath.value != path)
					{
						try
						{
							File.Delete(accessDeniedPath.value);
						}
						catch
						{
						}
					}
					ret.value = dbFile;
				}
				catch (Exception ex)
				{
					if (retryCount > 0)
					{
						await Task.Delay(60000);
						Upload upload = this;
						int num2 = retryCount - 1;
						retryCount = num2;
						return await upload.Chunking(path, fileManagementParts, guid, entityGuid, num2);
					}
					WebErrorLog.Instance.StartLog(ex, "", "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 671);
					ret.AddReturnedValue(ex, "", "Chunking", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 672);
					ret.value = null;
				}
				return ret;
			}
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> CannotAccessToFileAsync(string path)
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await FileUtility.Instance.FixeFileAccessAsync(path, 10), ReturnedState.None, "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 685);
			string fileName = Path.GetFileName(path);
			line = 1;
			string text = Path.GetDirectoryName(path) + "\\TempAccessDeniedFile";
			line = 2;
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			line = 3;
			ret.value = Path.Combine(text, fileName);
			line = 4;
			if (File.Exists(ret.value))
			{
				try
				{
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await FileUtility.Instance.FixeFileAccessAsync(ret.value, 10), ReturnedState.None, "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 697);
					line = 5;
					File.Delete(ret.value);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 702);
					ret.AddReturnedValue(ex, "", "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 703);
				}
			}
			try
			{
				line = 6;
				File.Copy(path, ret.value);
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await FileUtility.Instance.FixeFileAccessAsync(ret.value, 10), ReturnedState.None, "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 709);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 713);
				ret.AddReturnedValue(ex2, "", "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 714);
			}
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"Line:{line}", "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 719);
			ret.AddReturnedValue(ex3, "", "CannotAccessToFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 720);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<string>> CopyFileIfAccessDeniedAsync(string path, CancellationToken token, short tryCount = 10, int timeOutSeconds = 60)
	{
		ReturnedSaveFuncInfoWithValue<string> ret = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await FileUtility.Instance.FixeFileAccessAsync(path, 10), ReturnedState.None, "CopyFileIfAccessDeniedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 730);
			returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await FileUtility.Instance.WaitForFileAsync(path, token, timeOutSeconds), ReturnedState.None, "CopyFileIfAccessDeniedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 731);
			if (ret.HasError)
			{
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await CannotAccessToFileAsync(path), ReturnedState.None, "CopyFileIfAccessDeniedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 733);
			}
			else
			{
				ret.value = path;
			}
		}
		catch (Exception ex)
		{
			if (tryCount > 0)
			{
				await Task.Delay(5000);
				Upload upload = this;
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await upload.CopyFileIfAccessDeniedAsync(path, token, num, timeOutSeconds);
			}
			ret.AddReturnedValue(ex, "", "CopyFileIfAccessDeniedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 744);
			WebErrorLog.Instance.StartLog(ex, "", "CopyFileIfAccessDeniedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 745);
			ret.value = path;
		}
		return ret;
	}

	private async Task<bool> ChunkingAsync(List<List<byte>> chunkedList, Guid fileid, Guid filepartGuid)
	{
		using (new JaegerMonitor("ChunkingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs"))
		{
			try
			{
				if (chunkedList != null && chunkedList.Count > 0)
				{
					for (int i = 0; i < chunkedList.Count; i = checked(i + 1))
					{
						UploadFilePartBusiness obj = new UploadFilePartBusiness(ContextGuid)
						{
							FileId = fileid,
							Guid = Guid.NewGuid()
						};
						chunkedList[i] = null;
						await obj.SaveAsync(chunkedList[i]?.ToArray(), default(CancellationToken), awaitOnSemaphore: true, 10);
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChunkingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 771);
				return false;
			}
		}
	}

	public static void RaiseEvent(string message)
	{
		try
		{
			PacketParser.Utilities.RaiseEvent(Upload.OnLogFired, null, new LogEventArgs(message), default(CancellationToken), 60, "RaiseEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs");
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "RaiseEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 787);
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				buffer.Dispose();
				buffer = null;
				_config = null;
				_sendToken.Cancel();
				this.OnProgressChange = null;
				this.OnSentComplete = null;
				this.OnBufferLoggedIn = null;
				this.OnBufferConnected = null;
				this.OnBufferDisConnected = null;
				Upload.OnLogFired = null;
				_checkFetchAndSend.Elapsed -= _checkFetchAndSend_Elapsed;
				_checkFetchAndSend = null;
				_uploadConfig = null;
				_inited = false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Uploader\\Upload.cs", 813);
		}
	}
}
