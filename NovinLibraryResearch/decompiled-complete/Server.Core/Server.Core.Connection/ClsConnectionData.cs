using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.Settings.Accounting;
using PacketParser.TaxService;
using PacketParser.TransferLayer.Interfaces;
using Server.Core.Assistance;
using Server.Core.Connection.Dal;
using Server.Core.Connection.Dal.Cheque;
using Server.Core.Connection.Dal.Tickets;
using Server.Core.Mapper;
using SharedServer.Core.Connection.Dal;

namespace Server.Core.Connection;

public class ClsConnectionData : ConnectionData, IClsConnectionData, IConnectionData, IDisposable
{
	private DateTime _lastdepartmentCachedTime = DateTime.MinValue;

	private DateTime _lastUserCachedTime = DateTime.MinValue;

	private readonly bool _disposeTimerRunning;

	private SemaphoreSlim _singledPacketProcessSemaphore = new SemaphoreSlim(1, 1);

	private bool _isDisposed;

	private int _maxParalelPackets = 30;

	private ConcurrentDictionary<string, ConcurrentDictionary<short, byte[]>> _chunks = new ConcurrentDictionary<string, ConcurrentDictionary<short, byte[]>>();

	private SemaphoreSlim _mediumeQueueSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _lowQueueSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _highQueueSemaphore = new SemaphoreSlim(1, 1);

	private ConcurrentQueue<Packet> _highQueue = new ConcurrentQueue<Packet>();

	private ConcurrentQueue<Packet> _mediumQueue = new ConcurrentQueue<Packet>();

	private ConcurrentQueue<Packet> _lowQueue = new ConcurrentQueue<Packet>();

	private ICus _customerInfo;

	private IHesabTree _hesabInfo;

	private AccGlobalSettings _settings;

	private WebCashier _cashierInfo;

	private WebUserAcc _userInfo;

	private SyncInformation _SyncInfo;

	private string _username = "";

	private WebUser _user;

	private string _depname = "";

	private WebDepartment _department;

	private DateTime _LastRecivedPacket = DateTime.Now;

	private bool TimerIsRun;

	private string _reason;

	private static Dictionary<short, Tuple<WebDepartmentEntitySummery, DateTime>> _tuple;

	public bool IsAccountingFamily
	{
		get
		{
			if (base.Source != ENSource.Accounting && base.Source != ENSource.Baje && base.Source != ENSource.WordPressDebug && base.Source != ENSource.WebsiteWordPress && base.Source != ENSource.Android && base.Source != ENSource.Android_HesabYar && base.Source != ENSource.CrmPortal && base.Source != ENSource.WebNP && base.Source != ENSource.Department && base.Source != ENSource.AccountingPortal)
			{
				return base.Source == ENSource.WebSite;
			}
			return true;
		}
	}

	public bool IsAttendanceFamily => base.Source == ENSource.TimeAttendance;

	public int WaitForDelivery { get; private set; }

	private int MaxParalelPackets
	{
		get
		{
			if (base.Source == ENSource.WebsiteWordPress)
			{
				return 1;
			}
			if (base.Source == ENSource.Android_HesabYar)
			{
				return 3;
			}
			return _maxParalelPackets;
		}
		set
		{
			_maxParalelPackets = value;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public bool IsOwnedConnection { get; private set; }

	public bool SyncStart { get; set; }

	public string Username
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(_username))
				{
					_username = _user?.Name ?? "";
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Username", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 113);
			}
			return _username;
		}
		set
		{
		}
	}

	public ITaxConnectionManager TaxConnectionManager { get; private set; }

	public string DepName
	{
		get
		{
			return _department?.Name ?? "";
		}
		set
		{
		}
	}

	[XmlIgnore]
	[JsonIgnore]
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

	[XmlIgnore]
	[JsonIgnore]
	public BufferBlock<DateTime> SyncDoneBuffer { get; internal set; } = new BufferBlock<DateTime>();


	[XmlIgnore]
	[JsonIgnore]
	public DateTime SyncDoneTime { get; internal set; } = DateTime.Now.AddMinutes(-10.0);


	[JsonIgnore]
	[XmlIgnore]
	public SemaphoreSlim WordPressSemaphore { get; internal set; } = new SemaphoreSlim(1, 1);


	public string DisposedReason => _reason;

	public EncryptionType Encryption { get; private set; }

	public async Task<WebDepartment> GetDepartmentAsync()
	{
		try
		{
			if (base.DepId <= 0 || IsDisposed)
			{
				_department = null;
				return null;
			}
			if (_lastdepartmentCachedTime.AddMinutes(1.0) < DateTime.Now)
			{
				_department = null;
			}
			if (_department == null)
			{
				_department = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(base.DepId);
				_lastdepartmentCachedTime = DateTime.Now;
			}
			return _department;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetDepartmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 160);
			throw;
		}
	}

	public async Task<WebUser> GetUserAsync()
	{
		try
		{
			if (_lastUserCachedTime.AddMinutes(1.0) < DateTime.Now)
			{
				_user = null;
			}
			if (_user == null)
			{
				_user = await Server.Core.Assistance.Initializer.Dal.User.LoadDataAsync(base.UserId);
				_lastUserCachedTime = DateTime.Now;
			}
			startDisposeTimer();
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 180);
		}
		return _user;
	}

	public async Task<ICus> GetCustomerInfoAsync()
	{
		int line = 0;
		try
		{
			if (_customerInfo == null)
			{
				_customerInfo = await Server.Core.Assistance.Initializer.Dal.Cus.LoadDataAsync(base.DepId, base.CustomerTafsilGuid);
			}
			startDisposeTimer();
			return _customerInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetCustomerInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 195);
			return null;
		}
	}

	public async Task<IHesabTree> GetHesabInfoAsync()
	{
		int line = 0;
		try
		{
			if (_hesabInfo == null)
			{
				_hesabInfo = await Server.Core.Assistance.Initializer.Dal.HesabTree.LoadDataAsync(base.DepId, base.CustomerTafsilGuid, removeGardesh: true);
			}
			startDisposeTimer();
			return _hesabInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetHesabInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 211);
			return null;
		}
	}

	public async Task<AccGlobalSettings> GetSettingsAsync()
	{
		int line = 0;
		try
		{
			if (_settings == null)
			{
				line = 1;
				WebApplicationSetting webApplicationSetting = await Server.Core.Assistance.Initializer.Dal.Settings.LoadDataAsync(base.DepId, AccGlobalSettings.GetKeyName(""));
				line = 2;
				_settings = AccGlobalSettings.FromString(webApplicationSetting?.Val);
				if (_settings == null)
				{
					await GetCashierInfoAsync();
				}
			}
			line = 3;
			startDisposeTimer();
			line = 4;
			return _settings;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//isDisposed:{IsDisposed}\r\n" + $"#DepId_{base.DepId}\r\n" + $"//Server.Core.Assistance.Initializer.Dal.Settings is null:{Server.Core.Assistance.Initializer.Dal?.Settings == null}", "GetSettingsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 241);
			return null;
		}
	}

	public async Task<WebCashier> GetCashierInfoAsync(CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (_cashierInfo == null)
			{
				_cashierInfo = await Server.Core.Assistance.Initializer.Dal.UserAcc.GetByCashierAsync(base.DepId, base.UserAccGuid, token);
			}
			startDisposeTimer();
			return _cashierInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetCashierInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 260);
			return null;
		}
	}

	public async Task<SyncInformation> GetSyncInfoAsync(CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			line = 1;
			if (_SyncInfo == null && (base.Source == ENSource.Baje || base.Source == ENSource.WebSite || base.Source == ENSource.WebsiteWordPress || base.Source == ENSource.Android || base.Source == ENSource.Android_HesabYar || base.Source == ENSource.Android_SefareshGir || base.Source == ENSource.Programmer || base.Source == ENSource.TimeAttendance || base.Source == ENSource.AccountingPortal || base.Source == ENSource.Accounting))
			{
				line = 2;
				if (Server.Core.Assistance.Initializer.Dal?.Sync == null)
				{
					throw new NullReferenceException("Server.Core.Assistance.Initializer.Dal?.Sync is null!!");
				}
				line = 3;
				_SyncInfo = (await Server.Core.Assistance.Initializer.Dal.Sync.LoadDataAsync(base.HashId, _connectionToken.Token))?.value?.SyncInfo;
				line = 4;
			}
			line = 5;
			startDisposeTimer();
			line = 6;
			return _SyncInfo;
		}
		catch (OperationCanceledException)
		{
			return null;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "GetSyncInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 314);
			return null;
		}
	}

	public ClsConnectionData()
	{
	}

	public ClsConnectionData(WebUser user, ITaxConnectionManager taxManager)
	{
		_user = user;
		TaxConnectionManager = taxManager;
		Task.Run((Func<Task?>)DisposeAfter1HourceAsync);
	}

	private async Task DisposeAfter1HourceAsync()
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			for (int i = 0; i < 21600; i++)
			{
				await Task.Delay(1000, base.cancellationToken);
				if (IsDisposed)
				{
					return;
				}
			}
			if (!IsDisposed)
			{
				Dispose("disposed after 6 hour");
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "DisposeAfter1HourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 347);
		}
	}

	public static async Task<ClsConnectionData> FactoryAsync(WebContextHolder context, string ConnectionUniqueId, LoginExtention LoginRequest, WebUserInformation LoginResponse, long HashId, string Ip, WebUser user, ITaxConnectionManager taxManager)
	{
		ClsConnectionData ret = new ClsConnectionData(user, taxManager);
		try
		{
			ret.Context = context;
			context.LoginExtention.OnLoginResponseProcessed += ret.Instance_OnLoginResponseProcessed;
			ret._connectionToken = new CancellationTokenSource();
			ret.Guid = ConnectionUniqueId;
			if (LoginRequest != null)
			{
				ret.CompressedMode = LoginRequest.CompressedMode;
				ret.Encryption = LoginRequest.Encryption;
				ret.LkSerial = LoginRequest.LkSerial;
				ret.AndroidIme = LoginRequest.AndroidIme;
				ret.DbGuid = LoginRequest.DbGuid;
				ret.Version = LoginRequest.Version;
				ret.ClientVersion = LoginRequest.ClientVersion;
			}
			if (LoginResponse != null)
			{
				ret.Source = LoginResponse.Source;
				ret.UserId = LoginResponse.UserId;
				ret.UserAccGuid = LoginResponse.CashierGuid;
				ret.UserDepGuid = LoginResponse.UserDepGuid;
				ret.DepId = LoginResponse.DepId;
				WebCus customerInfo = LoginResponse.CustomerInfo;
				int num;
				if (customerInfo == null)
				{
					num = 0;
				}
				else
				{
					_ = customerInfo.Id;
					num = 1;
				}
				if (num != 0)
				{
					ret.CustomerTafsilGuid = LoginResponse.CustomerInfo.Guid;
				}
				WebHesabTree hesabInfo = LoginResponse.HesabInfo;
				int num2;
				if (hesabInfo == null)
				{
					num2 = 0;
				}
				else
				{
					_ = hesabInfo.Id;
					num2 = 1;
				}
				if (num2 != 0)
				{
					ret.CustomerTafsilGuid = LoginResponse.HesabInfo.Guid;
				}
			}
			ret.ConnectionStartTime = DateTime.Now;
			ret.TotalSendBytes = 0L;
			ret.TotalRecivedBytes = 0L;
			ret.HashId = HashId;
			ret.Ip = Ip;
			ret.IsOwnedConnection = true;
			if (ret.Source == ENSource.WebsiteWordPress)
			{
				WebTelegramMessage np_WordpressAggregate_bot = WebTelegramMessage.GetNp_WordpressAggregate_bot();
				string text = $"#DepId_{ret.DepId}\r\n";
				WebUser webUser = await ret.GetUserAsync();
				np_WordpressAggregate_bot.Sent2web("\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\ud83d\udfe2\r\n#ConnectionHasBeenHandled\r\n#ConnectionMonitor\r\n" + text + "\ud83c\udf10 #WordPressHost: " + webUser?.WordPressBasicSettings?.HostUrl + "\r\n" + $"{DateTime.Now}", null, "FactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 387);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "ctor2", "FactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 395);
		}
		return ret;
	}

	private async Task AutoDisposeAsync()
	{
		try
		{
			await Task.Delay(10000);
			WebErrorLog.Instance.StartLog("AutoDisposeAsync", "", "AutoDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 404);
			Dispose("AutoDispose");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AutoDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 407);
		}
	}

	public static async Task<ClsConnectionData> GetConnectionAsync(ConnectionData connection, WebUser user, ITaxConnectionManager taxManager)
	{
		int line = 0;
		ClsConnectionData ret = new ClsConnectionData(user, taxManager);
		try
		{
			line = 1;
			if (connection == null)
			{
				throw new Exception("connection is null!!");
			}
			if (connection.Context == null)
			{
				throw new Exception("connection.Context is null!!");
			}
			await ConnectionMapper.Instance.MapAsync(connection.Context.Guid, connection, ret);
			ret.Context = connection.Context;
			line = 2;
			ret._connectionToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { connection.cancellationToken });
			line = 3;
			ret.IsOwnedConnection = false;
			line = 4;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"ctor1//Line:{line}", "GetConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 429);
		}
		return ret;
	}

	private Task Instance_OnLoginResponseProcessed(object sender, LoginResponseProcessedEventArgs e)
	{
		using (new JaegerMonitor("Instance_OnLoginResponseProcessed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Login, 3, isReceivedPacket: true);
				if (IsDisposed)
				{
					return Task.CompletedTask;
				}
				if (e.ConnectionUniqueId == base.Guid)
				{
					base.Context.LoginExtention.OnLoginResponseProcessed -= Instance_OnLoginResponseProcessed;
					SyncStart = true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Instance_OnLoginResponseProcessed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 446);
			}
			return Task.CompletedTask;
		}
	}

	public new void Dispose()
	{
		WebErrorLog.Instance.StartLog("این یارو داره از تابع اشتباه دیسپوز استفاده میکنه!", "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 452);
		Dispose("by dispose without parameter");
	}

	public override void Dispose(string reason)
	{
		DisposeAsync(disposing: true, reason);
	}

	public override void Dispose(bool disposing)
	{
		DisposeAsync(disposing: true, "OS deposed!");
	}

	protected async Task DisposeAsync(bool disposing, string reason)
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			_lowQueueSemaphore?.Dispose();
			_lowQueueSemaphore = null;
			_mediumeQueueSemaphore?.Dispose();
			_mediumeQueueSemaphore = null;
			_highQueueSemaphore?.Dispose();
			_highQueueSemaphore = null;
			if (Server.Core.Assistance.Initializer.Dal.ClientLoginHistory != null)
			{
				Server.Core.Assistance.Initializer.Dal.ClientLoginHistory.InsertDisconnectTime(base.Guid, DateTime.Now, reason);
			}
			_reason = reason;
			using (new JaegerMonitor("DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
			{
				if (base.Source == ENSource.WebsiteWordPress)
				{
					WebTelegramMessage.GetNp_WordpressAggregate_bot().Sent2web("⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f⛔\ufe0f\r\n#ConnectionHasBeenUnHandled\r\n#ConnectionMonitor\r\n" + $"#DepId_{base.DepId}\r\n" + "\ud83c\udf10 #WordPressHost: " + _user?.WordPressBasicSettings?.HostUrl + "\r\n" + $"{DateTime.Now}\r\n" + "Reason:" + reason, null, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 492);
				}
				if (base.Source == ENSource.WebsiteWordPress)
				{
					WebTelegramMessage.GetNp_WordpressLogInforamtions().Sent2web("#Connection Dispose.User:" + (_user?.UserFullName ?? "null!!") + ",\r\nUrl:" + (_user?.WordPressBasicSettings?.HostUrl ?? "null!!") + ",\r\n Reason:" + reason, null, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 501);
				}
				base.Context.LoginExtention.OnLoginResponseProcessed -= Instance_OnLoginResponseProcessed;
				Task.Run(() => ConnectionManager.Instance.UnHandleConnection(this, "Clsconnection.Dispose Reson:" + reason, base.Source));
				_connectionToken?.Cancel();
				while ((_highQueue?.Count ?? 0) > 0)
				{
					if (_highQueue.TryDequeue(out var result))
					{
						result?.Dispose();
					}
				}
				_highQueue = null;
				while ((_mediumQueue?.Count ?? 0) > 0)
				{
					if (_mediumQueue.TryDequeue(out var result2))
					{
						result2?.Dispose();
					}
				}
				_mediumQueue = null;
				while ((_lowQueue?.Count ?? 0) > 0)
				{
					if (_lowQueue.TryDequeue(out var result3))
					{
						result3?.Dispose();
					}
				}
				_lowQueue = null;
				_customerInfo = null;
				_hesabInfo = null;
				_cashierInfo = null;
				_SyncInfo = null;
				_chunks?.Clear();
				_chunks = null;
				SyncDoneBuffer?.Complete();
				SyncDoneBuffer = null;
				base.Dispose(disposing: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 552);
		}
	}

	internal async Task<int> SendQueueAsync()
	{
		using (new JaegerMonitor("SendQueueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			_ = 1;
			try
			{
				if (IsDisposed)
				{
					return 0;
				}
				int lowCount = 0;
				int num = 0;
				await _lowQueueSemaphore.WaitAsync();
				try
				{
					lowCount = _lowQueue?.Count ?? 0;
				}
				finally
				{
					_lowQueueSemaphore.Release();
				}
				await _mediumeQueueSemaphore.WaitAsync();
				try
				{
					num = _mediumQueue?.Count ?? 0;
				}
				finally
				{
					_mediumeQueueSemaphore.Release();
				}
				if (WaitForDelivery > lowCount + num)
				{
					return WaitForDelivery;
				}
				return lowCount + num;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendQueueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 587);
				return 0;
			}
		}
	}

	private void startDisposeTimer()
	{
		try
		{
			_ = _disposeTimerRunning;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "startDisposeTimer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 600);
		}
	}

	public Task SendPacketAsync(List<Packet> packs, Priority priority, int partitionId = 0, CancellationToken token = default(CancellationToken))
	{
		return Task.Run(() => SendPacketAsync_(packs, priority, partitionId, token));
	}

	private async Task SendPacketAsync_(List<Packet> packs, Priority priority, int partitionId = 0, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (base.cancellationToken.IsCancellationRequested)
			{
				return;
			}
			line = 4;
			if (!(packs?.Any() ?? false))
			{
				return;
			}
			foreach (Packet pack in packs)
			{
				if (pack == null)
				{
					continue;
				}
				line = 6;
				if (base.cancellationToken.IsCancellationRequested)
				{
					break;
				}
				line = 7;
				if (pack != null)
				{
					line = 8;
					if (base.cancellationToken.IsCancellationRequested)
					{
						break;
					}
					line = 9;
					await SendPacketAsync(pack, priority, partitionId, token);
					line = 10;
					if (base.cancellationToken.IsCancellationRequested)
					{
						break;
					}
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"listPackets//Line:{line}", "SendPacketAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 767);
		}
	}

	public async Task SendPacketAsync(Packet packet, Priority priority, int partitionId = 0, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendPacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			int line = 0;
			try
			{
				token.ThrowIfCancellationRequested();
				if (packet == null)
				{
					return;
				}
				line = 1;
				line = 6;
				if (WaitForDelivery > MaxParalelPackets && base.Source != ENSource.WebsiteWordPress && IsOwnedConnection)
				{
					token.ThrowIfCancellationRequested();
					switch (priority)
					{
					case Priority.High:
						line = 80;
						if (IsDisposed)
						{
							return;
						}
						await _highQueueSemaphore.WaitAsync();
						try
						{
							_highQueue?.Enqueue(packet);
						}
						finally
						{
							_highQueueSemaphore.Release();
						}
						break;
					case Priority.Medium:
						line = 9;
						if (IsDisposed)
						{
							return;
						}
						await _mediumeQueueSemaphore.WaitAsync();
						try
						{
							_mediumQueue.Enqueue(packet);
						}
						finally
						{
							_mediumeQueueSemaphore.Release();
						}
						break;
					default:
						line = 8;
						if (IsDisposed)
						{
							return;
						}
						await _lowQueueSemaphore.WaitAsync();
						try
						{
							_lowQueue?.Enqueue(packet);
						}
						finally
						{
							_lowQueueSemaphore.Release();
						}
						break;
					}
					line = 10;
					Task.Run((Func<Task?>)Timer);
					line = 11;
				}
				else
				{
					line = 12;
					if (packet.DeliveryRequested)
					{
						WaitForDelivery++;
					}
					line = 13;
					await SendPacketAndUpdateVariablesAsync(packet, partitionId, token, priority);
					line = 14;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"singlePacket/Line:{line}\r\n" + $"#depId_{base.DepId}\r\n" + "#User_" + _user.Name + "\r\n" + $"#HashId_{base.HashId}\r\n" + "#depName_" + DepName, "SendPacketAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 897);
			}
		}
	}

	private async Task Timer()
	{
		int line = 0;
		try
		{
			if (!TimerIsRun)
			{
				TimerIsRun = true;
				await Task.Delay(60000, base.cancellationToken);
				if (DateTime.Now.Subtract(_LastRecivedPacket).TotalSeconds > 50.0)
				{
					WaitForDelivery = 0;
					await EnqueAndSendAsync();
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Timer", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 922);
		}
		finally
		{
			TimerIsRun = false;
		}
	}

	private async Task EnqueAndSendAsync()
	{
		_ = 6;
		try
		{
			Packet packet = null;
			if (IsDisposed)
			{
				return;
			}
			await _lowQueueSemaphore.WaitAsync();
			await _mediumeQueueSemaphore.WaitAsync();
			await _highQueueSemaphore.WaitAsync();
			try
			{
				if (((_highQueue?.Count ?? 0) <= 0 && (_mediumQueue?.Count ?? 0) <= 0 && (_lowQueue?.Count ?? 0) <= 0) || (!(_highQueue?.TryDequeue(out packet) ?? false) && !(_mediumQueue?.TryDequeue(out packet) ?? false) && !(_lowQueue?.TryDequeue(out packet) ?? false)))
				{
					return;
				}
			}
			finally
			{
				_lowQueueSemaphore.Release();
				_mediumeQueueSemaphore.Release();
				_highQueueSemaphore.Release();
			}
			WaitForDelivery++;
			Task.Run((Func<Task?>)Timer);
			await SendPacketAndUpdateVariablesAsync(packet);
			await _lowQueueSemaphore.WaitAsync();
			await _mediumeQueueSemaphore.WaitAsync();
			await _highQueueSemaphore.WaitAsync();
			try
			{
				ConcurrentQueue<Packet> highQueue = _highQueue;
				if (highQueue == null || highQueue.Count <= 0)
				{
					return;
				}
				ConcurrentQueue<Packet> mediumQueue = _mediumQueue;
				if (mediumQueue == null || mediumQueue.Count <= 0)
				{
					return;
				}
				ConcurrentQueue<Packet> lowQueue = _lowQueue;
				if (lowQueue != null && lowQueue.Count > 0 && WaitForDelivery < MaxParalelPackets)
				{
					Task.Run(() => EnqueAndSendAsync());
				}
			}
			finally
			{
				_lowQueueSemaphore.Release();
				_mediumeQueueSemaphore.Release();
				_highQueueSemaphore.Release();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "EnqueAndSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 977);
		}
	}

	internal async Task PacketDeliveredAsync(Guid PacketReferenceId, int Part)
	{
		using (new JaegerMonitor("PacketDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				Console.WriteLine($"Part{Part} Received");
				line = 2;
				_LastRecivedPacket = DateTime.Now;
				line = 3;
				WaitForDelivery--;
				line = 4;
				await EnqueAndSendAsync();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "PacketDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 995);
			}
		}
	}

	private ReturnedSaveFuncInfo CheckPacketValidationForParseRecieved(Packet packet)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			return returnedSaveFuncInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPacketValidationForParseRecieved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1019);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckPacketValidationForParseRecieved", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1020);
			return returnedSaveFuncInfo;
		}
	}

	public async Task RecievePacketAndParse(Packet packet, WebContextHolder context, string ConnectionUniqueId, string ip, StringBuilder sb)
	{
		if (packet == null)
		{
			return;
		}
		using (new JaegerMonitor("RecievePacketAndParse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			TaskCompletionSource<bool> taskCompleted = new TaskCompletionSource<bool>();
			int line2 = 0;
			try
			{
				if (IsDisposed)
				{
					return;
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(ConnectionUniqueId);
				line2 = 1;
				sb.AppendLine("RecievePacketAndParse...");
				line2 = 2;
				await _singledPacketProcessSemaphore.WaitAsync();
				line2 = 3;
				try
				{
					line2 = 4;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = CheckPacketValidationForParseRecieved(packet);
					line2 = 5;
					if (returnedSaveFuncInfo.HasError)
					{
						line2 = 6;
						WebErrorLog.Instance.StartLog("connection(" + ConnectionUniqueId + ")exceeded!\r\n" + $"CurrentPacket({packet.PacketType})Action({packet.GetPacketActionName(packet.PacketActions)})\r\n" + returnedSaveFuncInfo.GetErrorMessage(), "", "RecievePacketAndParse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1048);
						line2 = 7;
						Dispose(returnedSaveFuncInfo.GetErrorMessage());
						line2 = 8;
						return;
					}
				}
				finally
				{
					_singledPacketProcessSemaphore.Release();
				}
				line2 = 10;
				Server.Core.Assistance.Initializer.Dal.DalPacketLog?.Insert(packet.GenerateLogPacket(base.DepId, base.HashId, context, base.Guid), isSentPacket: false, 10);
				line2 = 11;
				if (base.Source == ENSource.WebsiteWordPress)
				{
					packet.DeliveryRequested = false;
				}
				line2 = 12;
				if (packet.PacketType == PacketType.UserAcc)
				{
					WebErrorLog.Instance.Trace($"User Packet Recieved date:{packet}", "", "RecievePacketAndParse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1067);
				}
				line2 = 13;
				bool DeliveryRequested = packet.DeliveryRequested;
				short Part = packet.Part;
				short TotalParts = packet.TotalParts;
				Guid PacketReferenceId = packet.PacketReferenceId;
				line2 = 15;
				if (!string.IsNullOrEmpty(ip))
				{
					base.Ip = ip;
				}
				Task.Run(() => PasrPacketTimeOut(taskCompleted, packet, ConnectionUniqueId));
				byte[] array = packet.ToBytes();
				base.TotalRecivedBytes += array.Length;
				line2 = 20;
				await Packet.ParsePacket(context, packet, ConnectionUniqueId, this, sb);
				if (DeliveryRequested && (connection?.ClientVersion ?? 0) < 15)
				{
					await SendPacketAsync(new Packet(PacketType.Packet, 1, null, null, Part, TotalParts, PacketReferenceId), Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
				line2 = 25;
				sb.AppendLine("❌OperationCanceledException");
				try
				{
					Dispose("RecievePacketAndParse_OperationCanceledException");
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line2}", "RecievePacketAndParse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1090);
				}
			}
			catch (Exception ex3)
			{
				sb.AppendLine("❌Exception:" + ex3.Message);
				try
				{
					taskCompleted?.TrySetResult(result: true);
				}
				catch
				{
				}
				WebErrorLog.Instance.StartLog(ex3, $"#Line_{line2}", "RecievePacketAndParse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1096);
			}
			finally
			{
				taskCompleted?.TrySetResult(result: true);
			}
		}
	}

	private async Task PasrPacketTimeOut(TaskCompletionSource<bool> taskCompletion, Packet packet, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("PasrPacketTimeOut", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			try
			{
				Stopwatch sw = Stopwatch.StartNew();
				Task timeOut = Task.Delay(30000);
				Task obj = await Task.WhenAny(new Task[2] { timeOut, taskCompletion.Task });
				sw.Stop();
				if (obj == timeOut)
				{
					WebErrorLog.ErrorLogInstance.StartLog(new Exception(string.Format("Programmer Log//Server timeOut ticks:{0} on partPacket type:{1}//Packet:{2}", sw.ElapsedTicks, packet?.PacketType.ToString() ?? "null", packet?.ToString() ?? "Null")), "Connection:" + ConnectionManager.Instance.GetConnection(ConnectionUniqueId), "PasrPacketTimeOut", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1114);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "PasrPacketTimeOut", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1121);
			}
		}
	}

	internal async Task<ReturnedSaveFuncInfo> SyncAsync(WebDepartmentEntitySummery customerData)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (IsDisposed)
			{
				return ret.AddError("object has been disposed! (this object!!!!=>clsConnectionData on SaveAsync operation)", "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1131);
			}
			CancellationToken connectionToken = _connectionToken.Token;
			if (await GetDepartmentAsync() == null)
			{
				return ret;
			}
			line = 1;
			CancellationToken waitForSyncDoneResponseTimeOut = new CancellationTokenSource(10000).Token;
			while (base.DepId != 256 && IsAccountingFamily && base.ClientVersion >= 3 && !SyncStart)
			{
				if (IsDisposed)
				{
					return ret.AddError("object has been disposed! (this object!!!!=>clsConnectionData on SaveAsync operation)", "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1139);
				}
				await Task.Delay(100);
				if (IsDisposed || connectionToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1141);
				}
				if (waitForSyncDoneResponseTimeOut.IsCancellationRequested)
				{
					SyncStart = true;
					break;
				}
			}
			line = 5;
			while (!IsDisposed)
			{
				line = 6;
				if (IsDisposed)
				{
					throw new Exception("syncToken has been cancelled!");
				}
				_syncToken?.Cancel();
				_syncToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { _connectionToken.Token });
				CancellationToken token = _syncToken.Token;
				line = 7;
				ret = new ReturnedSaveFuncInfo();
				line = 8;
				if (token.IsCancellationRequested || base.DepId <= 0)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1160);
				}
				line = 10;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SyncAsync_(token), ReturnedState.None, "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1162);
				line = 11;
				if (!token.IsCancellationRequested && base.Source == ENSource.Accounting && customerData != null)
				{
					await CheckCustomerDataAsync(customerData);
				}
				if (IsDisposed)
				{
					return ret.AddError(EnSystem.UserInterface, "disposed!", "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1166);
				}
				line = 12;
				if (base.Source != ENSource.WebsiteWordPress)
				{
					await Task.Delay(300000);
				}
				else
				{
					await Task.Delay(30000000);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1170);
				}
			}
			line = 13;
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, $"Line:{line}", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1174);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line={line}", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1177);
			ret.AddReturnedValue(ex, "", "SyncAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1178);
		}
		return ret;
	}

	internal async Task CheckCustomerDataAsync(WebDepartmentEntitySummery customerData)
	{
		_ = 1;
		try
		{
			if (customerData == null)
			{
				return;
			}
			WebDepartmentEntitySummery webDepartmentEntitySummery = await Server.Core.Assistance.Initializer.Dal.DepartmentEntitySummery.GetAsync(base.DepId);
			if (webDepartmentEntitySummery == null)
			{
				return;
			}
			List<IDalDepartmentEntitySummery> list = Server.Core.Assistance.Initializer.Dal.DepartmentEntitySummery.Compare(webDepartmentEntitySummery, customerData);
			if (list == null || !list.Any((IDalDepartmentEntitySummery q) => !q.IsSame))
			{
				return;
			}
			WebDepartment webDepartment = await Server.Core.Assistance.Initializer.Dal.Department.LoadDataAsync(base.DepId);
			Tuple<WebDepartmentEntitySummery, DateTime> value = null;
			_tuple?.TryGetValue(base.DepId, out value);
			if (value != null && (DateTime.Now - value.Item2).TotalMinutes >= 20.0)
			{
				_tuple.Remove(base.DepId);
				value = null;
			}
			if (value?.Item1 != null)
			{
				list = Server.Core.Assistance.Initializer.Dal.DepartmentEntitySummery.Compare(value.Item1, customerData);
				if (list != null && list.Any((IDalDepartmentEntitySummery q) => !q.IsSame))
				{
					IEnumerable<string> values = list.Select((IDalDepartmentEntitySummery p) => p.GetMessage());
					if (DateTime.Now > new DateTime(2024, 10, 25))
					{
						WebErrorLog.Instance.Warning("مغایرت در اطلاعات مشتری بین سرور و کلاینت \r\n" + $"DepId_{base.DepId} \r\n" + "\ud83d\udd35#DepartmentName=> " + webDepartment?.Name + "Errors: \r\n " + string.Join("\r\n", values), "", "CheckCustomerDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1211);
					}
				}
				_tuple.Remove(base.DepId);
			}
			else
			{
				if (_tuple == null)
				{
					_tuple = new Dictionary<short, Tuple<WebDepartmentEntitySummery, DateTime>>();
				}
				_tuple.Add(base.DepId, new Tuple<WebDepartmentEntitySummery, DateTime>(customerData, DateTime.Now));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCustomerDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1225);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncAsync_(CancellationToken token)
	{
		int line = 100;
		StringBuilder sb = new StringBuilder(100000);
		StringBuilder sbTimLog = new StringBuilder(100000);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		Stopwatch sw2 = Stopwatch.StartNew();
		try
		{
			line = 12010;
			WebDepartment webDepartment = await GetDepartmentAsync();
			line = 12011;
			if (base.ClientVersion >= 10 && base.Source == ENSource.Accounting && !webDepartment.SyncsKolMoeinTafsil)
			{
				line = 12020;
				if (webDepartment == null)
				{
					throw new NullReferenceException($"department is null #depid_{base.DepId}\r\n" + $"isDisposed:{IsDisposed}");
				}
				webDepartment.SyncsKolMoeinTafsil = true;
				line = 12020;
				line = 1203;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Department.SaveAsync(webDepartment), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1248);
			}
			line = 1300;
			if (IsAccountingFamily && base.Source != ENSource.Accounting)
			{
				await Task.WhenAny(new Task[2]
				{
					ClsUserAcc.Sync2ClientAsync(this, 1),
					Task.Delay(5000)
				});
			}
			line = 1301;
			switch (base.Source)
			{
			case ENSource.Accounting:
			{
				line = 1500;
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				line = 1501;
				line = 1502;
				WebTelegramMessage bot = WebTelegramMessage.GetNp_ServerMessaging_bot();
				line = 1503;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (base.DepId != 256)
				{
					line = 1504;
					sbTimLog.AppendLine($"سینک #HashId{base.HashId}");
					sbTimLog.AppendLine($"#LkSerial{base.LkSerial}");
					if (base.LkSerial > 0)
					{
						Guid customerTafsilGuid = base.CustomerTafsilGuid;
						WebCus webCus = await Server.Core.Assistance.Initializer.Dal.Cus.LoadDataAsync(3, customerTafsilGuid);
						if (webCus != null)
						{
							sbTimLog.AppendLine("LkCustomerName:" + webCus?.FullName?.FixForTelegram());
						}
					}
					line = 1506;
					line = 1507;
					if (base.ClientVersion >= 9)
					{
						line = 1508;
						line = 1509;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await new clsTaxService(TaxConnectionManager).SendNotDeliveredAsync(this), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1295);
						sbTimLog.AppendLine($"#TaxTracking_sent_time:{sw2.ElapsedMilliseconds:N0}(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1509;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsKol.KolInstance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1300);
						sbTimLog.AppendLine($"#Kol_sent_time:{sw2.ElapsedMilliseconds:N0}(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1510;
						line = 1511;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsMoein.MoeinInstance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1309);
						sbTimLog.AppendLine($"#Moein_sent_time:{sw2.ElapsedMilliseconds:N0}(ms)");
						sw2 = Stopwatch.StartNew();
						line = 15;
						line = 1512;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsTafsil.TafsilInstance.SendNotDeliveredAsync(this, base.Guid, IsGardeshHesab: false, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1318);
						sbTimLog.AppendLine("#Tafsil_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1513;
						line = 1514;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsProductVahed.ProductVahedInstance.SendNotDeliveredAsync(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1327);
						sbTimLog.AppendLine("#PrdVahed_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1515;
						line = 1516;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsProductGroup.ProductGroupInstance.SendNotDeliveredAsync(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1336);
						sbTimLog.AppendLine("#PrdGroup_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1517;
						line = 1518;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsDepot.DepotInstance.SendNotDeliveredAsync(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1345);
						sbTimLog.AppendLine("#CusTitle_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						line = 1518;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCusTitle.CusTitleInstance.SendNotDeliveredAsync(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1349);
						sbTimLog.AppendLine("#CusTitle_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCustomerRole.CustomerRoleInstance.SendNotDeliveredAsync(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1352);
						sw2 = Stopwatch.StartNew();
						line = 1519;
						line = 1520;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCusGroup.CusGroupInstance.SendNotDeliveredAsync(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1360);
						sbTimLog.AppendLine("#CusGroup_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1521;
						line = 1522;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsTafsilGroup.TafsilGroupInstance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1370);
						sbTimLog.AppendLine("#TafsilGroup_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
						sw2 = Stopwatch.StartNew();
						line = 1523;
						line = 1525;
						line = 1526;
						returnedSaveFuncInfo = ret;
						await returnedSaveFuncInfo.AddReturnedValueAsync(await clsProductPriceLevel.Instance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1389);
						returnedSaveFuncInfo = ret;
						await returnedSaveFuncInfo.AddReturnedValueAsync(await clsSanadStatusReport.Instance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1390);
					}
					if (base.ClientVersion >= 24)
					{
						returnedSaveFuncInfo = ret;
						await returnedSaveFuncInfo.AddReturnedValueAsync(await clsSanadPrint.Instance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1394);
					}
					line = 1527;
					line = 1528;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsProduct.ProductInstance.SendNotDeliveredAsync(this, base.Guid, token, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1405);
					sbTimLog.AppendLine("#Products_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					sw2 = Stopwatch.StartNew();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCustomer.CusInstance.SendNotDelivered(sb, this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1412);
					sbTimLog.AppendLine("#Customers_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					sw2 = Stopwatch.StartNew();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsUserAcc.UserAccInstance.SendNotDelivered(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1418);
					sbTimLog.AppendLine("#Users_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					sw2 = Stopwatch.StartNew();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsVisit.VisitInstance.SendNotDeliveredAsync(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1424);
					sbTimLog.AppendLine("#Visit_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					sw2 = Stopwatch.StartNew();
					line = 1529;
					line = 1530;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1433);
					}
					line = 1531;
					if (base.ClientVersion >= 4)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(ClsAghsatAshkhas.AghsatAshkhasInstance.SendNotDeliveredAsync(this, base.Guid), ClsAghsatShakhsi.AghsatShakhsiInstance.SendNotDeliveredAsync(this, base.Guid)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1436);
					}
					sbTimLog.AppendLine("#AghsatAshkhas_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					sw2 = Stopwatch.StartNew();
					line = 1540;
					token.ThrowIfCancellationRequested();
					line = 1545;
					if (base.ClientVersion >= 24)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(ClsIntroductionMethod.IntroductionMethodInstance.Sync2Client(this, base.Guid, 10), clsDiscountCoupon.Instance.SendNotDeliveredAsync(this, base.Guid)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1449);
					}
					ClsOrder clsOrder = new ClsOrder(TaxConnectionManager);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(ClsReception.ReceptionInstance.SendUnSent(this, base.Guid, 5), clsOrder.SendUnsent(this, 10), ClsPardakht.PardakhtInstance.SendUnsent(this, base.Guid, 5), ClsSanad.SanadInstance.SendUnsent(this)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1454);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(ChequeMOperation.ChequeMOperationInstance.SendUnsent(this), ChequeShOperation.ChequeShOperationInstance.SendUnsent(this), ClsPrdAnbarGardani.PrdAnbarGardaniInstance.SendUnsent(this)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1460);
					line = 1550;
					sbTimLog.AppendLine("#Reception_order_Pardakht_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					sw2 = Stopwatch.StartNew();
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1469);
					}
					line = 1555;
					tasks.Add(Task.Run(() => ClsFileInfo.FileInfoInstance.Sync2Client(this, base.Guid), token));
					if (base.ClientVersion >= 7)
					{
						tasks.Add(Task.Run(() => ClsCallingLog.CallingLogInstance.Sync2ClientAsync(this, base.Guid, 10), token));
					}
					if (base.ClientVersion >= 16)
					{
						tasks.Add(Task.Run(() => ClsNote.Instance.SendUnSent(this, 5), token));
					}
					sbTimLog.AppendLine("#CallingLog_File_sent_time:" + sw2.ElapsedMilliseconds.ToString("N0") + "(ms)");
					line = 1560;
					line = 1561;
				}
				line = 1565;
				token.ThrowIfCancellationRequested();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1491);
				}
				line = 1505;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsUserLog.UserLogInstance.SendUnSent(this, base.Guid, 5), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1494);
				sbTimLog.AppendLine($"#UserLog_sent_time:{sw2.ElapsedMilliseconds:N0}(ms)");
				Stopwatch.StartNew();
				tasks.Add(Task.Run(() => SendSyncData2ClientTicketsAndDesc(10), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1499);
				if (ret.HasError)
				{
					bot?.Sent2web("❌❌❌پایان ناموفق سینک حسابداری با اتمام سینک تیکت ها.\r\n" + $"{sbTimLog}\r\n" + "UserName:" + Username + "\r\n" + $"#depId_{base.DepId}\r\n" + $"#HashId{base.HashId}\r\n" + ret.GetErrorMessage(), null, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1503);
				}
				else
				{
					bot?.Sent2web("✅✅✅پایان موفق سینک حسابداری با اتمام سینک تیکت ها. \r\n" + $"{sbTimLog}\r\n" + "UserName:" + Username + "\r\n" + $"#depId_{base.DepId}\r\n" + $"#HashId{base.HashId}", null, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1513);
				}
				break;
			}
			case ENSource.WebSite:
			{
				line = 200;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Sync2Client.SyncProducts(this, base.Guid, 5), ClsNews.NewsInstance.Sync2Client(this, base.Guid), ClsHyperLink.HyperLinkInstance.Sync2Client(this, base.Guid), ClsContactsUs.ContactsUsInstance.Sync2Client(this, base.Guid), ClsCusTitle.CusTitleInstance.Sync2Client(this, base.Guid, sb), ClsCustomerRole.CustomerRoleInstance.Sync2Client(this, base.Guid, sb, 5), ClsCusGroup.CusGroupInstance.Sync2Client(this, base.Guid, sb), ClsHesabTree.HesabTreeInstance.Sync2Client(this, base.Guid, sb, 10), ClsDeliveryPartitioned.DeliveriInstance.Sync2ClientAsyncFromEntityDelivery(this, base.Guid, 10)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1524);
				if (base.DepId == 3)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCustomer.CusInstance.Sync2Client(this, base.Guid, 1, sb), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1536);
				}
				token.ThrowIfCancellationRequested();
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendSyncData2ClientTicketsAndDesc(10), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1540);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsFileInfo.FileInfoInstance.Sync2Client(this, base.Guid), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1541);
				break;
			}
			case ENSource.Android_SefareshGir:
			{
				line = 250;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(ClsProductGroup.ProductGroupInstance.Sync2Client(this, base.Guid), ClsProductVahed.ProductVahedInstance.Sync2Client(this, base.Guid), ClsFileInfo.FileInfoInstance.Sync2Client(this, base.Guid)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1546);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Sync2Client.SyncProducts(this, base.Guid, 1), ClsNews.NewsInstance.Sync2Client(this, base.Guid), ClsHyperLink.HyperLinkInstance.Sync2Client(this, base.Guid), ClsContactsUs.ContactsUsInstance.Sync2Client(this, base.Guid), ClsCusTitle.CusTitleInstance.Sync2Client(this, base.Guid, sb), ClsCustomerRole.CustomerRoleInstance.Sync2Client(this, base.Guid, sb, 5), ClsCusGroup.CusGroupInstance.Sync2Client(this, base.Guid, sb), ClsDeliveryPartitioned.DeliveriInstance.Sync2ClientAsyncFromEntityDelivery(this, base.Guid, 10)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1550);
				token.ThrowIfCancellationRequested();
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendSyncData2ClientTicketsAndDesc(10), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1561);
				break;
			}
			case ENSource.Android:
			case ENSource.Android_HesabYar:
			case ENSource.IOS_HesabYar:
			{
				line = 999;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Sync2Client.SyncProducts(this, base.Guid, 1, 240000), ClsDesc.DescInstance.Sync2Client(this, base.Guid, 1), Sync2Client.SyncHesab(this, base.Guid, sb), ClsFileInfo.FileInfoInstance.Sync2Client(this, base.Guid)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1567);
				line = 10001;
				line = 10002;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsDeliveryPartitioned.DeliveriInstance.Sync2ClientAsyncFromEntityDelivery(this, base.Guid, 1), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1575);
				if (token.IsCancellationRequested || ret.HasError)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1576);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsLastPrice.Instance.Sync2Client(this, base.Guid, 10), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1577);
				line = 10003;
				line = 10004;
				break;
			}
			case ENSource.Baje:
			case ENSource.AccountingPortal:
			{
				line = 300;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => Sync2Client.SyncProducts(this, base.Guid, 5), token), Task.Run(() => Sync2Client.SyncHesab(this, base.Guid, sb), token), Task.Run(() => ClsVisit.VisitInstance.Sync2Client(this), token), Task.Run(() => ClsDeliveryPartitioned.DeliveriInstance.Sync2ClientAsyncFromEntityDelivery(this, base.Guid, 10), token), Task.Run(() => clsSanadStatusReport.Instance.SyncToClientAsync(this, 5), token)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1594);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1601);
				}
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>
				{
					Task.Run(() => SendSyncData2ClientTicketsAndDesc(10), token),
					Task.Run(() => ClsFileInfo.FileInfoInstance.Sync2Client(this, base.Guid), token),
					Task.Run(() => ClsLastPrice.Instance.Sync2Client(this, base.Guid, 10), token)
				};
				if (base.ClientVersion >= 7)
				{
					list.Add(Task.Run(() => ClsCallingLog.CallingLogInstance.Sync2ClientAsync(this, base.Guid, 10), token));
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1610);
				break;
			}
			case ENSource.TimeAttendance:
			{
				line = 400;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsAttendaceLogOnPerson.ClsAttendaceLogOnPersonInstance.Sync2ClientAsync(this, base.Guid, 10), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1614);
				if (base.ClientVersion >= 18)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsAttPersonLeave.Instance.Sync2ClientAsync(this, base.Guid, 10), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1615);
				}
				break;
			}
			case ENSource.Programmer:
			{
				line = 450;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendSyncData2ClientTicketsAndDesc(10), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1619);
				break;
			}
			case ENSource.CrmPortal:
			{
				line = 5001;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll<ReturnedSaveFuncInfo>(Task.Run(() => Sync2Client.SyncProducts(this, base.Guid, 5), token), Task.Run(() => Sync2Client.SyncHesab(this, base.Guid, sb), token), Task.Run(() => ClsDeliveryPartitioned.DeliveriInstance.Sync2ClientAsyncFromEntityDelivery(this, base.Guid, 10), token)), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1623);
				token.ThrowIfCancellationRequested();
				List<Task<ReturnedSaveFuncInfo>> tasks2 = new List<Task<ReturnedSaveFuncInfo>>
				{
					Task.Run(() => SendSyncData2ClientTicketsAndDesc(10), token),
					Task.Run(() => ClsCallingLog.CallingLogInstance.Sync2ClientAsync(this, base.Guid, 10), token),
					Task.Run(() => ClsFileInfo.FileInfoInstance.Sync2Client(this, base.Guid), token),
					Task.Run(() => ClsLastPrice.Instance.Sync2Client(this, base.Guid, 10), token)
				};
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks2), ReturnedState.None, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1637);
				break;
			}
			}
			line = 5002;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1643);
			}
			line = 501;
			clear();
			line = 502;
			if (IsDisposed)
			{
				ret.AddError(EnSystem.None, "connection is disposed!", "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1647);
			}
			line = 503;
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(token, this));
			line = 511;
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1654);
			sb.AppendLine("❌❌Error:OperationCancell exception❌❌");
			Dispose("SyncAsync_OperationCanceledException");
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1660);
			sb.AppendLine("❌❌Exception:" + ex.Message + "❌❌");
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1662);
			Dispose("SyncAsync_exception:" + ex.Message);
		}
		finally
		{
			if (base.DepId == 3)
			{
				WebTelegramMessage.GetWebServiceMonitoring_bot().Sent2web(sb.ToString(), null, "SyncAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1668);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SendSyncData2ClientTicketsAndDesc(short maxParallelTask = 10)
	{
		using (new JaegerMonitor("SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 300000))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsTicketPart.TicketPartInstance.Sync2Client(this, base.Guid), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1680);
				if (base.Source == ENSource.Accounting)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsIntroductionMethod.IntroductionMethodInstance.SendNotDeliverd(this, base.Guid), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1683);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsSyncEntity.Instance.SendNotDeliverd(this, base.Guid), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1684);
				}
				else
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsIntroductionMethod.IntroductionMethodInstance.Sync2Client(this, base.Guid, 10), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1688);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsSyncEntity.Instance.Sync2Client(this, base.Guid, maxParallelTask), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1689);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsTicket.TicketInstance.Sync2Client(this, base.Guid), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1692);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsTicketDet.TicketDetInstance.Sync2Client(this, base.Guid), ReturnedState.None, "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1693);
			}
			catch (OperationCanceledException exception)
			{
				Dispose("SendSyncData2ClientTicketsAndDesc_OperationCanceledException");
				ret.AddReturnedValue(exception, "", "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1698);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1702);
				ret.AddReturnedValue(ex, "", "SendSyncData2ClientTicketsAndDesc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1703);
			}
			return ret;
		}
	}

	private async Task SendPacketAndUpdateVariablesAsync(Packet packet, int partitionId = 0, CancellationToken token = default(CancellationToken), Priority priority = Priority.Medium)
	{
		using (new JaegerMonitor("SendPacketAndUpdateVariablesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			int line = 0;
			try
			{
				if (packet == null)
				{
					return;
				}
				line = 1;
				line = 2;
				byte[] array = packet.ToBytes();
				line = 3;
				if (array == null || array.Length == 0)
				{
					return;
				}
				line = 4;
				base.TotalSendPacketCount++;
				base.TotalSendBytes += array.Length;
				line = 5;
				ConnectionLogger.SaveLogSend(base.HashId, packet, base.Guid, 100L);
				line = 6;
				if (Server.Core.Assistance.Initializer.Dal.DalPacketLog != null)
				{
					Server.Core.Assistance.Initializer.Dal.DalPacketLog.Insert(packet.GenerateLogPacket(base.DepId, base.HashId, base.Context, base.Guid), isSentPacket: true, 10);
				}
				line = 7;
				if (!IsOwnedConnection)
				{
					await ServerMessaging.Instance.SendToClient(packet, base.Guid, priority);
					line = 8;
				}
				else if (base.Source == ENSource.WebsiteWordPress)
				{
					line = 9;
					IWooInstance wooInstance = await Server.Core.Assistance.Initializer.Dal.WooCommerce.GetWooInstanceAsync(this, Server.Core.Assistance.Initializer.Dal);
					Task.Run(() => Server.Core.Assistance.Initializer.Dal.WooCommerce.WooSendPacketAsync(wooInstance, packet, token));
				}
				else
				{
					line = 10;
					await ServerFactory.Instance.SendToClient(packet, base.Context, base.Guid, base.CompressedMode, $"SendAndUpdateVariables//Type:{packet.PacketType}//Action:{packet.PacketActions}");
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "SendPacketAndUpdateVariablesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1752);
			}
		}
	}

	internal void clear()
	{
		try
		{
			_customerInfo = null;
			_hesabInfo = null;
			_settings = null;
			_cashierInfo = null;
			_SyncInfo = null;
			_user = null;
			_department = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "clear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1767);
		}
	}

	public byte[] ProgressReceived(byte[] bytes, short part, short totalPart, string refrenceId, StringBuilder sb)
	{
		using (new JaegerMonitor("ProgressReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs"))
		{
			try
			{
				sb.AppendLine($"Received part:{part} from {totalPart} size:{bytes.Length}");
				WebErrorLog.Instance.Trace($"Received part:{part} from {totalPart} size:{bytes.Length}", "", "ProgressReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1776);
				if (bytes == null || bytes.Length == 0)
				{
					return null;
				}
				if (totalPart <= 1)
				{
					return bytes;
				}
				if (_chunks == null)
				{
					return null;
				}
				_chunks.TryGetValue(refrenceId, out var value);
				if (value == null)
				{
					value = new ConcurrentDictionary<short, byte[]>();
					_chunks.TryAdd(refrenceId, value);
				}
				value.TryAdd(part, bytes);
				if (value.Count == totalPart)
				{
					List<byte> list = new List<byte>();
					for (short num = 0; num < value.Count; num++)
					{
						value.TryGetValue(num, out var value2);
						list.AddRange(value2);
					}
					return list.ToArray();
				}
				return null;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ProgressReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1803);
			}
			return null;
		}
	}

	public CancellationToken GetSaveSyncToken()
	{
		try
		{
			_syncToken?.Cancel();
			_syncToken = new CancellationTokenSource();
			return CancellationTokenSource.CreateLinkedTokenSource(_syncToken.Token, base.cancellationToken).Token;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSaveSyncToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1819);
			return default(CancellationToken);
		}
	}

	public CancellationTokenSource CreateLinkedToken(CancellationToken cancelationToken, TaskCompletionSource<bool> taskComplation, int timeOut = 300000)
	{
		CancellationTokenSource token = CancellationTokenSource.CreateLinkedTokenSource(cancelationToken, base.cancellationToken);
		Task.Run(() => AutoCancellTokenAsync(taskComplation, token, timeOut));
		return token;
	}

	private async Task AutoCancellTokenAsync(TaskCompletionSource<bool> taskComplation, CancellationTokenSource token, int timeOut)
	{
		try
		{
			if (token == null)
			{
				throw new NullReferenceException();
			}
			Task timeOutTask = Task.Delay(timeOut);
			if (await Task.WhenAny(new Task[2] { timeOutTask, taskComplation.Task }) == timeOutTask)
			{
				if (token == null)
				{
					throw new NullReferenceException();
				}
				token.Cancel();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AutoCancellTokenAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1844);
		}
	}

	public async Task<WebUserAcc> GetUserAccInfoAsync(CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		try
		{
			if (await GetCashierInfoAsync() == null)
			{
				throw new NullReferenceException("userInfo is null");
			}
			if (_userInfo == null)
			{
				_userInfo = await Server.Core.Assistance.Initializer.Dal.UserAcc.GetUserGroupByCashierGuid(base.DepId, base.UserAccGuid, token);
			}
			startDisposeTimer();
			return _userInfo;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetUserAccInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\ClsConnectionData.cs", 1861);
			throw;
		}
	}

	CancellationToken IClsConnectionData.get_cancellationToken()
	{
		return base.cancellationToken;
	}

	short IClsConnectionData.get_DepId()
	{
		return base.DepId;
	}

	void IClsConnectionData.set_DepId(short value)
	{
		base.DepId = value;
	}

	long IClsConnectionData.get_HashId()
	{
		return base.HashId;
	}

	void IClsConnectionData.set_HashId(long value)
	{
		base.HashId = value;
	}

	long IClsConnectionData.get_LkSerial()
	{
		return base.LkSerial;
	}

	void IClsConnectionData.set_LkSerial(long value)
	{
		base.LkSerial = value;
	}

	ENSource IClsConnectionData.get_Source()
	{
		return base.Source;
	}

	void IClsConnectionData.set_Source(ENSource value)
	{
		base.Source = value;
	}

	Guid IClsConnectionData.get_UserAccGuid()
	{
		return base.UserAccGuid;
	}

	void IClsConnectionData.set_UserAccGuid(Guid value)
	{
		base.UserAccGuid = value;
	}

	Guid IClsConnectionData.get_CustomerTafsilGuid()
	{
		return base.CustomerTafsilGuid;
	}

	void IClsConnectionData.set_CustomerTafsilGuid(Guid value)
	{
		base.CustomerTafsilGuid = value;
	}

	short IClsConnectionData.get_ClientVersion()
	{
		return base.ClientVersion;
	}

	void IClsConnectionData.set_ClientVersion(short value)
	{
		base.ClientVersion = value;
	}
}
