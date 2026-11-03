using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.OrderDepot.OrderSanad;
using EntityCache.Business.Report;
using EntityCache.Cache.Repository;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Entities;
using EntityCache.Interfaces.Notification;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Uploader;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;

namespace EntityCache;

public class ClsCache : Obsevable, INpDisposable, IDisposable
{
	private TimingCache<string, TaskCompletionSource<ReturnedSaveFuncInfo>> _transactionDistinityResultCache;

	protected static ConcurrentDictionary<string, ClsCache> _cacheDictionary = new ConcurrentDictionary<string, ClsCache>();

	private ClientHub.Buffer _buffer;

	private ILocalSettingRepository _sLocalSetting;

	private CancellationTokenSource _connectTokenSource;

	private SemaphoreSlim _connectSemaphore = new SemaphoreSlim(1, 1);

	public SendData2ServerAggregator SendData2ServerInstance { get; protected set; }

	public MasterSendData2ServerAggregator MasterSendData2ServerAggregator { get; protected set; }

	public NotificationAggregator NotificationAggregator { get; protected set; }

	public WebContextHolder Context { get; protected set; }

	public string ContextGuid => Context.Guid;

	public WebServiceHandlers WebService { get; protected set; }

	public CustomerAddressCacheRepository CusAddress { get; protected set; }

	public bool BufferEnable => !(_buffer?.IsDispsed ?? true);

	public EnConnectionType BufferConnectionType => Buffer?.ConnectionType ?? EnConnectionType.SignalRCore;

	public CancellationToken BufferCancellationToken => _buffer?.CancellationToken ?? new CancellationToken(canceled: true);

	public LoginResponseEventArgs BufferLoginState
	{
		get
		{
			if ((_buffer?.ConnectionState ?? EnConnectionState.Disconnected) != EnConnectionState.Connected)
			{
				return new LoginResponseEventArgs
				{
					Status = EnLoginResult.Connection_Disconnected
				};
			}
			return _buffer.LastLoginState;
		}
	}

	private ClientHub.Buffer Buffer
	{
		get
		{
			return _buffer;
		}
		set
		{
			try
			{
				if (value != null)
				{
					if (_buffer != null)
					{
						_buffer.Connected -= Buffer_Connected;
						_buffer.Disconnected -= Buffer_Disconnected;
						_buffer.Connecting -= Buffer_Connecting;
						_buffer.LoggedIn -= Buffer_LoggedIn;
					}
					_buffer = value;
					_buffer.Connected += Buffer_Connected;
					_buffer.Disconnected += Buffer_Disconnected;
					_buffer.Connecting += Buffer_Connecting;
					_buffer.LoggedIn += Buffer_LoggedIn;
					if (Source == ENSource.NPBackupService)
					{
						CacheRepository.Upload = new Upload(Context);
						CacheRepository.Upload.Init(new UploadConfig
						{
							BufferClient = _buffer,
							MaxChunkKb = 512,
							MinChunkKb = 256,
							RetryOnFault = true,
							TempPath = ImagePath
						});
					}
					InitWebServiceHandlers();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Buffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 98);
			}
		}
	}

	public string ImagePath { get; protected set; }

	public IEnumerable<IDatabaseManager> DatabaseManager => CacheRepository?.DatabaseManager;

	public IEnumerable<EnCacheConfig> Configs => DatabaseManager.Select((IDatabaseManager p) => p.Config);

	public IEnumerable<ClientConfig> BufferConfigs => Buffer.Configs;

	public IDatabaseManager AccDatabaseManager => DatabaseManager?.SingleOrDefault((IDatabaseManager x) => x.Config == EnCacheConfig.Accounting);

	public IDatabaseManager AdvDatabaseManager => DatabaseManager?.SingleOrDefault((IDatabaseManager x) => x.Config == EnCacheConfig.Advertise);

	public IDatabaseManager GlobalDatabaseManager => DatabaseManager?.SingleOrDefault((IDatabaseManager x) => x.Config == EnCacheConfig.Global);

	public IDatabaseManager AttDatabaseManager => DatabaseManager?.SingleOrDefault((IDatabaseManager x) => x.Config == EnCacheConfig.Attendance);

	public IDatabaseManager DptDatabaseManager => DatabaseManager?.SingleOrDefault((IDatabaseManager x) => x.Config == EnCacheConfig.Department);

	public AggregateSqlRepository CacheRepository { get; protected set; }

	public bool Initialized => CacheRepository?.Initialized ?? false;

	public BusinessAggregator BusinessAggregator { get; protected set; }

	public ENSource Source { get; protected set; }

	public IImageResizer Resizer { get; set; }

	public bool IsDisposed { get; protected set; }

	public object CtorTime { get; protected set; } = DateTime.Now;


	public IOrderSanadGenerator OrderSanadGenerator { get; set; }

	public MultiLanguageConfig MultiLanguageConfig { get; private set; }

	public string ApplicationVersion { get; private set; }

	public event Func<object, LoginResponseEventArgs, Task> BufferLoggedIn;

	public event Func<object, BaseEventArgs, Task> BufferConnected;

	public event Func<object, EventArgs, Task> BufferConnecting;

	public event Func<object, BaseEventArgs, Task> BufferDisconnected;

	public void InitWebServiceHandlers()
	{
		WebService = new WebServiceHandlers(this, ImagePath);
	}

	public async Task<ReturnedSaveFuncInfo> SendOnCustomerBuffer(string customerUserName, string customerPassword, List<Packet> packets, CancellationToken token, Priority PriorityLevel = Priority.High, int timeout = 60000)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ret.AddReturnedValue(await Buffer.GetCustomerBufferAsync(customerUserName, customerPassword, token, timeout), ReturnedState.None, "SendOnCustomerBuffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 128);
			if (ret.HasError)
			{
				return ret;
			}
			token.ThrowIfCancellationRequested();
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Buffer.SendData(packets, PriorityLevel), ReturnedState.None, "SendOnCustomerBuffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 131);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendOnCustomerBuffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 135);
			ret.AddReturnedValue(ex, "", "SendOnCustomerBuffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 136);
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> SendData(List<Packet> lstPacket, Priority PriorityLevel)
	{
		if (Buffer != null)
		{
			return Buffer.SendData(lstPacket, PriorityLevel);
		}
		throw new NullReferenceException("buffer is null!");
	}

	public Task<ReturnedSaveFuncInfo> SendAndWaitForResponse(List<Packet> packets, CancellationToken token, int TimeOutSeconds = 20, short RetryCount = 2, short NextRetrySeconds = 10, short BatchCount = 10)
	{
		if (Buffer != null)
		{
			return Buffer.SendAndWaitForResponse(packets, token, TimeOutSeconds, RetryCount, NextRetrySeconds, BatchCount);
		}
		throw new NullReferenceException("buffer is null!");
	}

	public Task SendData(Packet Data, string guid)
	{
		if (Buffer != null)
		{
			return Buffer.SendData(Data, guid);
		}
		throw new NullReferenceException("buffer is null!");
	}

	public Task SendData(Packet packetData, Priority PriorityLevel)
	{
		if (Buffer != null)
		{
			return Buffer.SendData(packetData, PriorityLevel);
		}
		throw new NullReferenceException("buffer is null!");
	}

	public static async Task<ClsCache> GetCacheAsync(StringBuilder sb, string imagePath, int cachingTimeSeconds, ENSource source, IImageResizer resizer, AggregateSqlRepository aggregateSqlRepository, IEnumerable<IDatabaseManager> databasemanager, NinjectLoggerKernel kernel, SendData2ServerAggregator sendData2ServerInstance, string applicationVersion, MasterSendData2ServerAggregator masterSendData2ServerAggregator = null, NotificationAggregator notifAggregator = null)
	{
		Stopwatch swTotal = Stopwatch.StartNew();
		Stopwatch sw = Stopwatch.StartNew();
		using (new JaegerMonitor("GetCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs"))
		{
			ClsCache ret = null;
			try
			{
				_ = 3;
				try
				{
					sb.AppendLine($"Getting... CacheAsync {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					ret = new ClsCache
					{
						SendData2ServerInstance = sendData2ServerInstance,
						MasterSendData2ServerAggregator = masterSendData2ServerAggregator,
						NotificationAggregator = notifAggregator
					};
					sb.AppendLine($"ret = new ClsCache has been finished. {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					if (_cacheDictionary.TryRemove(aggregateSqlRepository.Context.Guid, out var value))
					{
						value?.Dispose();
					}
					sb.AppendLine($"old temp cache has been disposed. {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					_cacheDictionary.TryAdd(aggregateSqlRepository.Context.Guid, ret);
					ret.Source = source;
					ret.Resizer = resizer;
					ret.ApplicationVersion = applicationVersion;
					ret.ImagePath = imagePath ?? throw new ArgumentException("Argument is needed", "imagePath");
					if (!Directory.Exists(imagePath) && !string.IsNullOrEmpty(imagePath))
					{
						Directory.CreateDirectory(imagePath);
					}
					if (!Directory.Exists(imagePath) && !string.IsNullOrEmpty(imagePath))
					{
						throw new ArgumentException("Image path does not exists");
					}
					sb.AppendLine($"initializing context. {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					ret.InitContext(aggregateSqlRepository.Context.Guid);
					sb.AppendLine($"context has been initialized. {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					ret.CacheRepository = AggregatorManager.Initialize(aggregateSqlRepository, databasemanager, cachingTimeSeconds, ret.Context);
					sb.AppendLine($"repositiries has been created. {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					if (source != ENSource.Accounting && source != ENSource.WebNP)
					{
						sb.AppendLine($"getting userInfoBusiness. {sw.ElapsedMilliseconds:N0}(ms)");
						sw = Stopwatch.StartNew();
						UserInformationBusiness userInformationBusiness = await UserInformationBusiness.GetAsync(ret.ContextGuid);
						if (userInformationBusiness != null && userInformationBusiness.UserAccGuid != Guid.Empty)
						{
							CashierBusiness cashierBusiness = await CashierBusiness.GetAsync(ret.ContextGuid, userInformationBusiness.UserAccGuid);
							if (cashierBusiness != null)
							{
								await CashierBusiness.SetCurrentCashierAsync(ret.ContextGuid, cashierBusiness.Guid);
							}
						}
					}
					await ret.SetMultiLanguageSettingsAsync();
					sb.AppendLine($"handling events... {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					WebContextHolder.GetWebContextHolder(ret.ContextGuid).Sync.OnSyncDoneResponse -= ret.SyncInstance_OnSyncDoneResponse;
					WebContextHolder.GetWebContextHolder(ret.ContextGuid).Sync.OnSyncDoneResponse += ret.SyncInstance_OnSyncDoneResponse;
					sb.AppendLine($"setting kernel. {sw.ElapsedMilliseconds:N0}(ms)");
					sw = Stopwatch.StartNew();
					DI.Kernel = kernel;
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "22222", "GetCacheAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 222);
				}
			}
			finally
			{
				sb.AppendLine($"finally, creating businessAggregators. {sw.ElapsedMilliseconds:N0}(ms)");
				sw = Stopwatch.StartNew();
				ClsCache clsCache = ret;
				clsCache.BusinessAggregator = await BusinessAggregator.GetFactoryAsync(ret.ContextGuid, databasemanager?.Select((IDatabaseManager p) => p.Config));
				sb.AppendLine($"finished creating businessAggregators. {sw.ElapsedMilliseconds:N0}(ms)");
				sb.AppendLine($"totalTime. {swTotal.ElapsedMilliseconds:N0}(ms)");
			}
			return ret;
		}
	}

	public static ClsCache GetCache(string contextGuid)
	{
		try
		{
			if (string.IsNullOrEmpty(contextGuid))
			{
				return null;
			}
			if (_cacheDictionary.TryGetValue(contextGuid, out var value))
			{
				return value;
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"contextGuid:{contextGuid}//_cacheDictionary is null:{_cacheDictionary == null}", "GetCache", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 246);
			throw;
		}
	}

	protected void InitContext(string contextGuid = null)
	{
		try
		{
			if (Context == null)
			{
				WebContextHolder webContextHolder2 = (Context = WebContextHolder.GetWebContextHolder(contextGuid));
			}
			_transactionDistinityResultCache = new TimingCache<string, TaskCompletionSource<ReturnedSaveFuncInfo>>(contextGuid, TransactionDistinityMapper.Instance, 60000);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitContext", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 257);
		}
	}

	public static string GetCacheHistory(string contextGuid)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		try
		{
			ClsCache cache = GetCache(contextGuid);
			stringBuilder.AppendLine($"contextGuid({contextGuid}) cache is null:{cache == null}");
			stringBuilder.AppendLine($"CurrentDate:{DateTime.Now}-CacheCreation:{cache?.CtorTime}");
			stringBuilder.AppendLine($"CurrentCacheIsDisposed:{cache?.IsDisposed ?? true}");
			foreach (string item in _cacheDictionary?.Keys ?? new List<string>())
			{
				if (string.IsNullOrEmpty(item))
				{
					stringBuilder.AppendLine("key is null!!");
					continue;
				}
				ClsCache cache2 = GetCache(item);
				stringBuilder.AppendLine(string.Format("Other_caches) ContextGuid({0}) -CtorTime:{1} -CacheIsNull:{2} -cacheIsDisposed:{3}", item, cache2?.CtorTime ?? "", cache2 == null, cache2?.IsDisposed ?? true));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCacheHistory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 288);
			stringBuilder.AppendLine("❌ExceptionOnGetCacheHistory:" + ex.Message);
		}
		return stringBuilder.ToString();
	}

	protected async Task SyncInstance_OnSyncDoneResponse(object sender, SyncDoneResponseEventArgs e)
	{
		_ = 2;
		try
		{
			do
			{
				await Task.Delay(10000);
				if (Source == ENSource.TimeAttendance)
				{
					break;
				}
				if (e.Connection.cancellationToken.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			while (!Initialized);
			using (new JaegerMonitor("SyncInstance_OnSyncDoneResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs"))
			{
				if (e.Connection.cancellationToken.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				UserInformationBusiness userInformationBusiness = await UserInformationBusiness.GetAsync(ContextGuid);
				if (e.Connection.cancellationToken.IsCancellationRequested || IsDisposed)
				{
					return;
				}
				if (userInformationBusiness != null)
				{
					if (userInformationBusiness != null && (userInformationBusiness.SyncInfo?.SyncInfo?.ServerGenerationTime).HasValue && userInformationBusiness.SyncInfo.SyncInfo.ServerGenerationTime > e.SyncInfo.ServerGenerationTime)
					{
						return;
					}
					UserInformationBusiness userInformationBusiness2 = userInformationBusiness;
					if (userInformationBusiness2.SyncInfo == null)
					{
						userInformationBusiness2.SyncInfo = new Sync();
					}
					userInformationBusiness.SyncInfo.SyncInfo = e.SyncInfo;
					await userInformationBusiness.SaveAsync(e.Connection.cancellationToken, awaitOnSemaphore: true, 10);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSyncDoneResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 321);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> ConnectAsync(EnConnectionType connectionType, IEnumerable<ClientConfig> configs = null, CancellationToken tokenInput = default(CancellationToken), bool ignoreCashierGuidEmpty = false)
	{
		using (new JaegerMonitor("ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
			_connectTokenSource?.Cancel();
			_connectTokenSource = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { tokenInput });
			CancellationToken token = _connectTokenSource.Token;
			try
			{
				using (await _connectSemaphore.GaurdAsync(true, "clsCache.ConnectAsync", 10000, token))
				{
					IEnumerable<ClientConfig> enumerable = configs;
					if (enumerable != null && enumerable.Any())
					{
						configs = new List<ClientConfig> { enumerable.First() };
					}
					int waiteForCashierLogin = 0;
					if (Source != ENSource.TimeAttendance)
					{
						while (CashierBusiness.GetCurrentCashierGuid(ContextGuid) == Guid.Empty && !ignoreCashierGuidEmpty && !token.IsCancellationRequested)
						{
							waiteForCashierLogin = checked(waiteForCashierLogin + 1);
							await Task.Delay(1000, token);
							if (waiteForCashierLogin > 100)
							{
								WebErrorLog.Instance.StartLog($"cashier did not loggedIn after {waiteForCashierLogin}", "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 346);
								break;
							}
						}
					}
					WebDepartmentEntitySummery departmentEntitySummery = null;
					if (Source == ENSource.Accounting)
					{
						departmentEntitySummery = await GlobalReportBusiness.GetDepartmentEntitySummeryAsync(ContextGuid, token);
					}
					if (configs?.Any() ?? false)
					{
						foreach (ClientConfig config in configs)
						{
							config.DepartmentEntitySummery = departmentEntitySummery;
							config.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
							config.ClientContextGuid = ContextGuid;
						}
					}
					line = 1;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 363);
					}
					line = 2;
					while (CacheRepository?.ILocalSetting == null)
					{
						if (IsDisposed || token.IsCancellationRequested)
						{
							return ret;
						}
						line = 3;
						await Task.Delay(1000, token);
					}
					line = 4000;
					ClientConfig clientConfig = configs?.FirstOrDefault();
					WebErrorLog.ClientUsername = clientConfig?.UserName;
					if (configs != null)
					{
						if (string.IsNullOrEmpty(clientConfig?.UserName))
						{
							throw new ArgumentException("userName OnConfig is null or empty!");
						}
						if (string.IsNullOrEmpty(clientConfig?.Password))
						{
							throw new ArgumentException("Password OnConfig is null or empty!");
						}
					}
					token.ThrowIfCancellationRequested();
					if (Buffer?.IsDispsed ?? true)
					{
						DisposeBuffer();
					}
					if (Buffer == null)
					{
						Buffer = new ClientHub.Buffer(Source, DI.Kernel, Context, connectionType);
						Buffer.Disconnected += Buffer_Disconnected;
						Buffer.Connected += Buffer_Connected;
						Buffer.Disconnected += Buffer_Disconnected;
						Buffer.Connecting += Buffer_Connecting;
						Buffer.LoggedIn += Buffer_LoggedIn;
					}
					line = 40;
					if (token.IsCancellationRequested)
					{
						ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 395);
					}
					ClientHub.Buffer buffer = Buffer;
					IEnumerable<ClientConfig> configs2 = configs;
					CancellationToken? token2 = token;
					ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> returnedSaveFuncInfoWithValue = await buffer.ConnectAsync(configs2, token2, await LocalSettingBusiness.GetDbGuidAsync(ContextGuid, token));
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 397);
					ret.value = returnedSaveFuncInfoWithValue.value;
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 402);
				ret.value = new LoginResponseEventArgs
				{
					Status = EnLoginResult.LoginCanceled
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//Buffer is null:{Buffer == null}//isDisposed:{IsDisposed}//Configs:{Json.ToStringJson(configs)}", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 407);
				ret.AddReturnedValue(ex, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 408);
				ret.value = new LoginResponseEventArgs
				{
					Status = EnLoginResult.ClientError
				};
			}
			finally
			{
				if (token.IsCancellationRequested)
				{
					ret.value = new LoginResponseEventArgs
					{
						Status = EnLoginResult.LoginCanceled
					};
				}
				ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> returnedSaveFuncInfoWithValue2 = ret;
				if (returnedSaveFuncInfoWithValue2.value == null)
				{
					returnedSaveFuncInfoWithValue2.value = new LoginResponseEventArgs
					{
						Status = EnLoginResult.ClientError,
						LoginResultMessage = "Value is null!!"
					};
				}
			}
			return ret;
		}
	}

	internal static async Task<TaskCompletionSource<ReturnedSaveFuncInfo>> ObserveTransactionDistinity(string contextGuid, string tranName)
	{
		try
		{
			ClsCache cache = GetCache(contextGuid);
			if (cache._transactionDistinityResultCache.TryGetValue(tranName, out var ret2) && ret2 != null)
			{
				return ret2;
			}
			ret2 = new TaskCompletionSource<ReturnedSaveFuncInfo>();
			await cache._transactionDistinityResultCache.UpdateTimeAsync(tranName, ret2);
			return ret2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ObserveTransactionDistinity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 435);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> TransactionDestiny(string contextGuid, string tranName, bool isRollBack, bool disposeContext = true, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, string debugInfo = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (GetCache(contextGuid)?.DatabaseManager == null)
			{
				return ret.AddError(EnSystem.None, "Static cache or DataBaseManager is null", "", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 453);
			}
			IEnumerable<IDatabaseManager> enumerable = GetCache(contextGuid)?.DatabaseManager;
			if (enumerable == null || !enumerable.Any())
			{
				return ret.AddError(EnSystem.None, "dbmanager is null or empty!", "dbManagers", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 456);
			}
			foreach (IDatabaseManager dbManager in enumerable)
			{
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = await dbManager.ExistsAsync(tranName);
				if (dbManager != null && returnedSaveFuncInfoWithValue.value)
				{
					ReturnedSaveFuncInfo result;
					if (isRollBack)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						result = returnedSaveFuncInfo.AddReturnedValue(await dbManager.RollBackTransactionAsync(tranName, disposeContext, memberName, sourceFilePath, sourceLineNumber), ReturnedState.None, "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 462);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						result = returnedSaveFuncInfo.AddReturnedValue(await dbManager.CommitTransactionAsync(tranName, disposeContext, memberName, sourceFilePath, sourceLineNumber, debugInfo), ReturnedState.None, "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 463);
					}
					return result;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"memberName:{memberName}//sourceFilePath:{sourceFilePath}//sourceLineNumber:{sourceLineNumber}", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 468);
			ret.AddReturnedValue(ex, "", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 469);
		}
		finally
		{
			try
			{
				if (GetCache(contextGuid)._transactionDistinityResultCache.TryGetValue(tranName, out var ret2) && ret2 != null)
				{
					if (isRollBack)
					{
						ret2.TrySetResult(new ReturnedSaveFuncInfo().AddError("transaction has been roll backed", "", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 477));
					}
					else
					{
						ret2.TrySetResult(ret);
					}
				}
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"memberName:{memberName}//sourceFilePath:{sourceFilePath}//sourceLineNumber:{sourceLineNumber}", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 483);
				ret.AddReturnedValue(ex2, "", "TransactionDestiny", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 484);
			}
		}
		return ret;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs"))
		{
			try
			{
				if (!IsDisposed)
				{
					IsDisposed = true;
					Task.Run((Func<Task?>)DisposeObservers);
					_cacheDictionary.TryRemove(ContextGuid, out var _);
					DisposeBuffer();
					CacheRepository?.Dispose();
					CacheRepository = null;
					WebService?.Dispose();
					WebService = null;
					CusAddress?.Dispose();
					CusAddress = null;
					_sLocalSetting?.Dispose();
					_sLocalSetting = null;
					WebContextHolder.GetWebContextHolder(ContextGuid).Sync.OnSyncDoneResponse -= SyncInstance_OnSyncDoneResponse;
					CacheRepository?.Upload?.Dispose();
					this.BufferLoggedIn = null;
					this.BufferConnected = null;
					this.BufferConnecting = null;
					this.BufferDisconnected = null;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 520);
			}
		}
	}

	private Task Buffer_LoggedIn(object sender, LoginResponseEventArgs e)
	{
		return PacketParser.Utilities.RaiseEvent(this.BufferLoggedIn, this, e, default(CancellationToken), 60, "Buffer_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs");
	}

	private Task Buffer_Connecting(object sender, EventArgs e)
	{
		return PacketParser.Utilities.RaiseEvent(this.BufferConnecting, this, e, default(CancellationToken), 60, "Buffer_Connecting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs");
	}

	internal Task<ReturnedSaveFuncInfoWithValue<ClientHub.Buffer>> GetCustomerBuffer(string username, string password, CancellationToken token, int timeOut = 60000)
	{
		return Buffer.GetCustomerBufferAsync(username, password, token, timeOut);
	}

	private Task Buffer_Disconnected(object sender, BaseEventArgs e)
	{
		try
		{
			Buffer?.Clear();
			return PacketParser.Utilities.RaiseEvent(this.BufferDisconnected, this, e, default(CancellationToken), 60, "Buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Buffer_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 535);
		}
		return Task.CompletedTask;
	}

	private Task Buffer_Connected(object sender, BaseEventArgs e)
	{
		try
		{
			return PacketParser.Utilities.RaiseEvent(this.BufferConnected, this, e, default(CancellationToken), 60, "Buffer_Connected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Buffer_Connected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 544);
		}
		return Task.CompletedTask;
	}

	internal void SetCashierGuid(Guid cashierGuid)
	{
		try
		{
			if (Buffer == null)
			{
				return;
			}
			List<ClientConfig> configs = Buffer.Configs;
			if (configs != null && configs.Any())
			{
				foreach (ClientConfig item in configs)
				{
					item.CashierGuid = cashierGuid;
					item.ClientContextGuid = ContextGuid;
				}
			}
			if (GetCache(ContextGuid).Source != ENSource.Android_HesabYar && configs != null)
			{
				Buffer.ConnectAsync(configs);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetCashierGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 563);
		}
	}

	public void DisposeBuffer()
	{
		try
		{
			if (Buffer != null)
			{
				Buffer?.Dispose();
				Buffer.Connected -= Buffer_Connected;
				Buffer.Disconnected -= Buffer_Disconnected;
				Buffer.Connecting -= Buffer_Connecting;
				Buffer.LoggedIn -= Buffer_LoggedIn;
			}
			_buffer = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeBuffer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 580);
		}
	}

	public async Task SetMultiLanguageSettingsAsync()
	{
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
			MultiLanguageConfig = new MultiLanguageConfig
			{
				DigitAfterPoint = accGlobalSettings.MultiLanguage.DigitAfterPoint,
				CurrencyUnit = accGlobalSettings.MultiLanguage.CurrencyUnit,
				DigitForRound = accGlobalSettings.MultiLanguage.DigitForRound
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetMultiLanguageSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\ClsCache.cs", 594);
		}
	}
}
