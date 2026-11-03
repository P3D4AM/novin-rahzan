using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ClientHub.TcpConnection;
using Ninject;
using Ninject.Parameters;
using Ninject.Syntax;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer.Interfaces;

namespace ClientHub;

public class Buffer : ISender, IAsyncDisposable
{
	private CancellationTokenSource _internalConnectToken = new CancellationTokenSource();

	private CancellationTokenSource _connectionWatcherToken;

	private TaskCompletionSource<LoginResponseEventArgs> _loggedInTask = new TaskCompletionSource<LoginResponseEventArgs>();

	private bool isMeasuringPerformance;

	private static ConcurrentDictionary<string, Buffer> _CustomerBufferPool = new ConcurrentDictionary<string, Buffer>();

	private static ConcurrentDictionary<EnConnectionType, Buffer> _connectionBufferPool = new ConcurrentDictionary<EnConnectionType, Buffer>();

	public static ExpiredPacketLogger _logger = new ExpiredPacketLogger(300000);

	private BufferBlock<Packet> _collection_High = new BufferBlock<Packet>();

	private BufferBlock<Packet> _collection_Medium = new BufferBlock<Packet>();

	private BufferBlock<Packet> _collection_Low = new BufferBlock<Packet>();

	private ConcurrentDictionary<string, TaskCompletionSource<string>> performanceSources = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

	private IEnumerable<ClientConfig> _configs;

	private NinjectLoggerKernel kernel;

	private bool _isDisposed;

	private CancellationTokenSource token;

	private Guid _dbGuid;

	private CancellationTokenSource _connectToken;

	private SemaphoreSlim _cleanUpSemaphore = new SemaphoreSlim(1, 1);

	private string winnerId { get; set; } = "";


	public ConcurrentDictionary<string, Client> Clients { get; private set; } = new ConcurrentDictionary<string, Client>();


	private ENSource Source { get; set; }

	private Guid DbGuid { get; set; }

	public EnConnectionType ConnectionType { get; set; }

	public int reconnectDelay
	{
		get
		{
			double num = Math.Pow(2.0, failedConnections) * 1000.0;
			if (num < 1000.0)
			{
				return 1000;
			}
			if (num > 60000.0)
			{
				return 60000;
			}
			return checked((int)num);
		}
	}

	public short failedConnections { get; private set; }

	public CancellationToken CancellationToken => token?.Token ?? new CancellationToken(canceled: true);

	public List<ClientConfig> Configs
	{
		get
		{
			if (_configs == null)
			{
				return null;
			}
			List<ClientConfig> list = new List<ClientConfig>();
			foreach (ClientConfig config in _configs)
			{
				list.Add(config.Copy());
			}
			return list;
		}
	}

	public LoginResponseEventArgs LastLoginState { get; private set; } = new LoginResponseEventArgs
	{
		Status = EnLoginResult.Unknown,
		LoginResultMessage = "هنوز درخواست لاگین ارسال نشده یا درخواست لاگین پاسخ داده نشده"
	};


	public EnConnectionState ConnectionState { get; private set; } = EnConnectionState.Disconnected;


	public double QueueLength
	{
		get
		{
			using (new JaegerMonitor("QueueLength", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
			{
				try
				{
					if (string.IsNullOrEmpty(winnerId) || !Clients.TryGetValue(winnerId, out var value))
					{
						return 0.0;
					}
					int num = (value.IsSending ? 1 : 0);
					return checked(_collection_Low.Count + _collection_Medium.Count + _collection_High.Count + num);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "QueueLength", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 131);
					return 0.0;
				}
			}
		}
	}

	public int ClientLoggedInCount
	{
		get
		{
			try
			{
				return (Clients[winnerId].LoginState.Status == EnLoginResult.Success) ? 1 : 0;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ClientLoggedInCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 146);
				return 0;
			}
		}
	}

	public bool IsEmpty => QueueLength == 0.0;

	public bool IsDispsed => _isDisposed;

	public WebContextHolder Context { get; private set; }

	public event Func<object, LoginResponseEventArgs, Task> LoggedIn;

	public event Func<object, BaseEventArgs, Task> Connected;

	public event Func<object, EventArgs, Task> Connecting;

	public event Func<object, BaseEventArgs, Task> Disconnected;

	public event Func<object, BufferStateChangedEventArgs, Task> StateChanged;

	public event Func<object, BaseEventArgs, Task> Free;

	public event Func<object, string, Task> StopConnection;

	public async Task<EnConnectionState> GetStateAsync()
	{
		if (!string.IsNullOrEmpty(winnerId) && Clients.TryGetValue(winnerId, out var value) && value != null)
		{
			return await value.GetStateAsync();
		}
		return EnConnectionState.Disconnected;
	}

	public async Task<int> GetClientConnectedCountAsync()
	{
		try
		{
			return (await Clients[winnerId].GetStateAsync() == EnConnectionState.Connected) ? 1 : 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"cliens count:{Clients?.Count ?? (-1)}//winnerId:{winnerId}", "GetClientConnectedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 159);
			return 0;
		}
	}

	public async Task<int> GetClientDisconnectedCountAsync()
	{
		try
		{
			return (await Clients[winnerId].GetStateAsync() == EnConnectionState.Disconnected) ? 1 : 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetClientDisconnectedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 171);
			return 0;
		}
	}

	public Buffer(ENSource source, NinjectLoggerKernel kernel, WebContextHolder _context, EnConnectionType connectionType)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				Context = _context;
				Source = source;
				this.kernel = kernel;
				ConnectionType = connectionType;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 189);
			}
		}
	}

	public static Buffer GetInstance(ENSource source, NinjectLoggerKernel kernel, WebContextHolder _context, EnConnectionType connectionType)
	{
		try
		{
			if (_connectionBufferPool == null)
			{
				_connectionBufferPool = new ConcurrentDictionary<EnConnectionType, Buffer>();
			}
			_connectionBufferPool.TryGetValue(connectionType, out var value);
			if (value != null)
			{
				return value;
			}
			if (value == null)
			{
				value = new Buffer(source, kernel, _context, connectionType);
				_connectionBufferPool.TryAdd(connectionType, value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 208);
			throw;
		}
	}

	public static Buffer GetInstance(EnConnectionType connectionType)
	{
		try
		{
			if (_connectionBufferPool == null)
			{
				return null;
			}
			_connectionBufferPool.TryGetValue(connectionType, out var value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 222);
			throw;
		}
	}

	private Client InitializeClient(ClientConfig conf, IConnection connection, int timeout = 30000)
	{
		using (new JaegerMonitor("InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				if (ConnectionType != EnConnectionType.TcpConnection && (string.IsNullOrEmpty(conf?.UserName) || string.IsNullOrEmpty(conf?.Password) || string.IsNullOrEmpty(conf?.Url)))
				{
					return null;
				}
				if (conf != null && conf.Source == ENSource.Android_HesabYar && conf.AndroidIme <= 0)
				{
					return null;
				}
				Client client = new Client(Context, conf, timeout, connection);
				client.StopConnection += Client_StopConnection;
				client.Connected += Client_Connected;
				client.Disconnected += Client_Disconnected;
				client.LoggedIn += Client_LoggedIn;
				client.ReceivedRaw += Client_ReceivedRaw;
				return client;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "InitializeClient", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 246);
				return null;
			}
		}
	}

	private async Task Client_ReceivedRaw(object sender, RawDataEventArgs e)
	{
		int line = 0;
		try
		{
			line = 1;
			if (string.IsNullOrEmpty(winnerId) || !Clients.TryGetValue(winnerId, out var value) || sender == null || (Client)sender != value)
			{
				return;
			}
			line = 2;
			if (e.Payload.StartsWith("no"))
			{
				line = 3;
				await value.DisconnectAsync("pong received and is no");
				line = 4;
				Task.Run(() => RaiseDisconnected(e.ConnectionUniqueId));
				Task.Run(async delegate
				{
					EnConnectionState isConnected = await GetStateAsync();
					return RaiseStateChanged(isConnected);
				});
				line = 5;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Client_ReceivedRaw", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 273);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> ConnectAsync(IEnumerable<ClientConfig> configs = null, CancellationToken? token = null, Guid? dbGuid = null, int timeOutMiliseconds = 30000)
	{
		if (!(configs?.Any() ?? false) && !(_configs?.Any() ?? false))
		{
			throw new Exception("config not setYet! please set buffer configuration!");
		}
		ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
		CancellationToken tempToken = default(CancellationToken);
		try
		{
			if (configs == null)
			{
				configs = _configs;
			}
			_internalConnectToken?.Cancel();
			_internalConnectToken = new CancellationTokenSource();
			tempToken = _internalConnectToken.Token;
			CancellationTokenSource tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tempToken, _internalConnectToken.Token);
			if (dbGuid.HasValue)
			{
				_dbGuid = dbGuid.Value;
			}
			if (IsDispsed)
			{
				return ret.AddError(EnSystem.SignalR, "Disposed!", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 292);
			}
			TaskCompletionSource<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> connectResult = new TaskCompletionSource<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>>();
			Task timeout = Task.Delay(timeOutMiliseconds, tokenSource.Token);
			CancellationToken token2 = tokenSource.Token;
			new Thread((ThreadStart)async delegate
			{
				await ConnectAsync_(configs, token2, _dbGuid, connectResult, timeOutMiliseconds);
			}).Start();
			if (await Task.WhenAny(new Task[2] { timeout, connectResult.Task }) == timeout)
			{
				tokenSource?.Cancel();
				return ret.AddError("Connect timed out", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 303);
			}
			ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> returnedSaveFuncInfoWithValue = await connectResult.Task;
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 306);
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 311);
			WebErrorLog.Instance.Error(ex, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 312);
		}
		finally
		{
			if (IsDispsed)
			{
				ret.AddError(EnSystem.SignalR, "Disposed!", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 317);
			}
			if (!tempToken.IsCancellationRequested && !IsDispsed)
			{
				_connectionWatcherToken?.Cancel();
				_connectionWatcherToken = new CancellationTokenSource();
				CancellationToken tkn = _connectionWatcherToken.Token;
				Task.Run(() => ConnectionWatcher(tkn));
			}
		}
		return ret;
	}

	private async Task ConnectAsync_(IEnumerable<ClientConfig> configs, CancellationToken tokenInternal, Guid dbGuid, TaskCompletionSource<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> connecResult, int timeOutMiliseconds = 60000)
	{
		if (connecResult == null)
		{
			connecResult = new TaskCompletionSource<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>>();
		}
		ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
		checked
		{
			using (new JaegerMonitor("ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
			{
				int line = 0;
				try
				{
					_connectToken?.Cancel();
					_connectToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { tokenInternal });
					CancellationToken token = _connectToken.Token;
					if (IsDispsed)
					{
						ret.AddError(EnSystem.SignalR, "Is Disposed!", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 348);
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.ClientError
						};
					}
					if (configs != null && configs.Count() > 1)
					{
						ret.AddError(EnSystem.SignalR, "duplicate configurations has bug! please use single configurations,Thank you (-_-)", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 353);
						return;
					}
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 359);
						return;
					}
					RaiseConnecting();
					RaiseStateChanged(EnConnectionState.Connecting);
					line = 1;
					DbGuid = dbGuid;
					line = 2;
					if (configs != null)
					{
						_configs = configs;
					}
					line = 3;
					failedConnections++;
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 385);
						return;
					}
					line = 5;
					Dictionary<string, Client> localClients = new Dictionary<string, Client>();
					Client winner = null;
					await CleanUpAsync(token);
					line = 10;
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 396);
						return;
					}
					if (_configs != null)
					{
						foreach (ClientConfig item in _configs?.ToList())
						{
							if (ConnectionType != EnConnectionType.TcpConnection && (string.IsNullOrEmpty(item?.UserName) || string.IsNullOrEmpty(item?.Password)))
							{
								throw new Exception("UserName or password is null\r\nConfigs:" + Json.ToStringJson(configs));
							}
							if (token.IsCancellationRequested)
							{
								ret.value = new LoginResponseEventArgs
								{
									Status = EnLoginResult.LoginCanceled
								};
								ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 407);
								return;
							}
							line = 15;
							item.DbGuid = DbGuid;
							if (item.Source == ENSource.None)
							{
								item.Source = Source;
							}
							if (token.IsCancellationRequested)
							{
								ret.value = new LoginResponseEventArgs
								{
									Status = EnLoginResult.LoginCanceled
								};
								ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 416);
								return;
							}
							WebErrorLog.Instance.Debug(string.Format("{0})Connecting to {1}", DateTime.Now, item?.Url ?? "null"), "", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 419);
							line = 171;
							if (token.IsCancellationRequested)
							{
								line = 172;
								ret.value = new LoginResponseEventArgs
								{
									Status = EnLoginResult.LoginCanceled
								};
								line = 173;
								ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 426);
								return;
							}
							line = 174;
							IConnection connection = ConnectionType switch
							{
								EnConnectionType.SignalRCore => ResolutionExtensions.Get<IConnection>((IResolutionRoot)(object)kernel, (IParameter[])(object)new IParameter[5]
								{
									(IParameter)new ConstructorArgument("contextGuid", (object)Context.Guid),
									(IParameter)new ConstructorArgument("url", (object)item.Url),
									(IParameter)new ConstructorArgument("compressedMode", (object)item.CompressedMode),
									(IParameter)new ConstructorArgument("miliSecondsDelayBetweenSend", (object)item.MilisecondsDelayBetweenSend),
									(IParameter)new ConstructorArgument("source", (object)item.Source)
								}), 
								EnConnectionType.TcpConnection => new TcpCoreConnection(Context.Guid, item.Url, item.CompressedMode, item.MilisecondsDelayBetweenSend, item.Source), 
								_ => throw new NotImplementedException($"ConnectionType {ConnectionType} is Not Implemented !!"), 
							};
							line = 175;
							if (token.IsCancellationRequested)
							{
								line = 176;
								ret.value = new LoginResponseEventArgs
								{
									Status = EnLoginResult.LoginCanceled
								};
								line = 177;
								ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 453);
								return;
							}
							line = 180;
							Client client = InitializeClient(item, connection);
							line = 20;
							if (client != null)
							{
								if (token.IsCancellationRequested)
								{
									ret.value = new LoginResponseEventArgs
									{
										Status = EnLoginResult.LoginCanceled
									};
									ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 464);
									return;
								}
								Clients.TryAdd(client.ConnectionId, client);
								if (token.IsCancellationRequested)
								{
									ret.value = new LoginResponseEventArgs
									{
										Status = EnLoginResult.LoginCanceled
									};
									ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 471);
									return;
								}
								localClients.Add(client.ConnectionId, client);
								performanceSources.TryAdd(client.ConnectionId, new TaskCompletionSource<string>());
								if (token.IsCancellationRequested)
								{
									ret.value = new LoginResponseEventArgs
									{
										Status = EnLoginResult.LoginCanceled
									};
									ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 479);
									return;
								}
								line = 25;
							}
						}
					}
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 487);
						return;
					}
					line = 30;
					if (!(Clients?.Any() ?? false))
					{
						ret.value = LastLoginState;
						ret.AddError(EnSystem.SignalR, "There is no client! Clients?.Count <= 0", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 494);
						return;
					}
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 500);
						return;
					}
					line = 35;
					isMeasuringPerformance = true;
					line = 36;
					try
					{
						if (token.IsCancellationRequested)
						{
							ret.value = new LoginResponseEventArgs
							{
								Status = EnLoginResult.LoginCanceled
							};
							ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 511);
							return;
						}
						line = 37;
						WebErrorLog.Instance.Trace("MeasurePerformance start", "", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 515);
						winner = await MeasurePerformance(token, timeOutMiliseconds);
						winnerId = winner?.ConnectionId ?? "";
						WebErrorLog.Instance.Trace("MeasurePerformance finished", "", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 518);
						if (token.IsCancellationRequested)
						{
							ret.value = new LoginResponseEventArgs
							{
								Status = EnLoginResult.LoginCanceled
							};
							ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 522);
							return;
						}
						line = 40;
					}
					catch (Exception ex)
					{
						ret.AddReturnedValue(ex, "", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 529);
						WebErrorLog.Instance.StartLog(ex, "", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 530);
						winnerId = "";
					}
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 536);
						return;
					}
					line = 45;
					isMeasuringPerformance = false;
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 544);
						return;
					}
					if (string.IsNullOrEmpty(winnerId))
					{
						ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await WinnerFailedAsync(localClients), ReturnedState.None, "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 548);
					}
					else
					{
						ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await WinnerSetAsync(winner, localClients, token, token), ReturnedState.None, "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 550);
					}
					line = 50;
					if (token.IsCancellationRequested)
					{
						ret.value = new LoginResponseEventArgs
						{
							Status = EnLoginResult.LoginCanceled
						};
						ret.AddError(EnSystem.SignalR, "token is cancelled! token.IsCancellationRequested || t.IsCancellationRequested", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 555);
					}
					else if (ret.value == null)
					{
						ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.value = await _loggedInTask.Task;
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//configs:{Json.ToStringJson(configs)}//_configs:{Json.ToStringJson(_configs)}", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 565);
					ret.AddReturnedValue(ex2, "", "ConnectAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 566);
					ret.value = new LoginResponseEventArgs
					{
						Status = EnLoginResult.ClientError
					};
				}
				finally
				{
					connecResult?.TrySetResult(ret);
				}
			}
		}
	}

	private async Task ConnectionWatcher(CancellationToken connectionWatcherToken)
	{
		try
		{
			Guid.NewGuid();
			await Task.Delay(60000);
			while (!connectionWatcherToken.IsCancellationRequested && !IsDispsed)
			{
				int line = 3;
				try
				{
					line = 4;
					await Task.Delay(1000);
					line = 5;
					line = 6;
					_internalConnectToken?.Cancel();
					line = 7;
					_internalConnectToken = new CancellationTokenSource();
					CancellationToken token3 = _internalConnectToken.Token;
					line = 8;
					CancellationTokenSource token = CancellationTokenSource.CreateLinkedTokenSource(connectionWatcherToken, token3);
					line = 90;
					line = 11;
					await Task.Delay(reconnectDelay, token.Token);
					line = 12;
					line = 13;
					if (ConnectionState != EnConnectionState.Connected || (LastLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
					{
						line = 14;
						line = 15;
						CancellationToken token2 = token.Token;
						TaskCompletionSource<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> connectResult = new TaskCompletionSource<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>>();
						new Thread((ThreadStart)async delegate
						{
							await ConnectAsync_(Configs, token2, DbGuid, connectResult);
						}).Start();
						line = 16;
						Task obj = await Task.WhenAny(new Task[2]
						{
							connectResult.Task,
							Task.Delay(60000)
						});
						line = 17;
						if (obj == connectResult.Task)
						{
							line = 19;
							string message = "ConnectTask is success!";
							line = 20;
							WebErrorLog.Instance.Trace(message, "", "ConnectionWatcher", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 626);
							line = 21;
							line = 22;
						}
						else
						{
							line = 23;
							string message2 = $"ConnectTask result:tomedOut//Toke is canceled:{token.IsCancellationRequested}";
							line = 24;
							WebErrorLog.Instance.Trace(message2, "", "ConnectionWatcher", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 636);
							line = 25;
							line = 26;
						}
						line = 27;
					}
					line = 28;
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "ConnectionWatcher", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 646);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex4)
		{
			WebErrorLog.Instance.StartLog(ex4, "", "ConnectionWatcher", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 650);
		}
	}

	private async Task<ReturnedSaveFuncInfo> WinnerSetAsync(Client winner, Dictionary<string, Client> clients, CancellationToken token, CancellationToken t)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (clients == null)
			{
				throw new NullReferenceException("clients");
			}
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 664);
				return ret;
			}
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 679);
				return ret;
			}
			if (winner == null)
			{
				ret.AddReturnedValue(ReturnedState.Error, "winner==null", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 685);
				return ret;
			}
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 691);
				return ret;
			}
			IEnumerable<Client> clients2 = clients.Values.Except(new List<Client> { winner });
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 698);
				return ret;
			}
			await DisposeAllAsync(clients2);
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 706);
				return ret;
			}
			_loggedInTask = new TaskCompletionSource<LoginResponseEventArgs>();
			LoginResponseEventArgs loginResult = await winner.Login(winner.CancellationToken.Token, 60000);
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 714);
				return ret;
			}
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 720);
				return ret;
			}
			if (loginResult.Status != EnLoginResult.Success)
			{
				if (token.IsCancellationRequested || t.IsCancellationRequested)
				{
					ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 728);
					return ret;
				}
				await winner.DisconnectAsync("Login timed out");
				ret.AddReturnedValue(ReturnedState.Error, "خطا در لاگین به سرور".GetTranslate() + " (" + loginResult.Status.GetDisplay().GetTranslate() + ")- " + $"{(short)loginResult.Status}\r\n" + loginResult.LoginResultMessage, "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 735);
				if (token.IsCancellationRequested || t.IsCancellationRequested)
				{
					ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 739);
					return ret;
				}
				return ret;
			}
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 747);
				return ret;
			}
			this.token = CancellationTokenSource.CreateLinkedTokenSource(new CancellationToken[1] { winner.CancellationToken.Token });
			if (token.IsCancellationRequested || t.IsCancellationRequested)
			{
				ret.AddReturnedValue(ReturnedState.Error, "token Cancelled!", "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 754);
				return ret;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 760);
			ret.AddReturnedValue(ex, "", "WinnerSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 761);
		}
		finally
		{
			Task.Run(() => RaiseFree(new BaseEventArgs()), token);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> WinnerFailedAsync(Dictionary<string, Client> clients)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await DisposeAllAsync(clients?.Values), ReturnedState.None, "WinnerFailedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 774);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WinnerFailedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 778);
			ret.AddReturnedValue(ex, "", "WinnerFailedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 779);
		}
		return ret;
	}

	private async Task CleanUpAsync(CancellationToken token)
	{
		try
		{
			if (Clients == null || !Clients.Any())
			{
				return;
			}
			List<Client> clientsCopy = new List<Client>();
			await _cleanUpSemaphore.WaitAsync(token);
			bool flag = true;
			try
			{
				foreach (Client value in Clients.Values)
				{
					clientsCopy.Add(value);
				}
				Clients.Clear();
				performanceSources.Clear();
				winnerId = "";
			}
			finally
			{
				if (flag)
				{
					_cleanUpSemaphore.Release();
				}
			}
			foreach (Client item in clientsCopy)
			{
				item.StopConnection -= Client_StopConnection;
				item.Connected -= Client_Connected;
				item.Disconnected -= Client_Disconnected;
				item.LoggedIn -= Client_LoggedIn;
				item.ReceivedRaw -= Client_ReceivedRaw;
				if (item != null)
				{
					await item.DisposeAsync();
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CleanUpAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 819);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>> LoginWaiter(CancellationToken cancellationToken, int timeout = 60000)
	{
		ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs> ret = new ReturnedSaveFuncInfoWithValue<LoginResponseEventArgs>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(timeout, cancellationToken),
				_loggedInTask.Task
			}) != _loggedInTask.Task)
			{
				ret.AddError(EnSystem.SignalR, "loginWaiter timedOut!", "LoginWaiter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 829);
				ret.value = new LoginResponseEventArgs
				{
					Status = EnLoginResult.TimeOut
				};
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "LoginWaiter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 835);
			ret.value = new LoginResponseEventArgs
			{
				Status = EnLoginResult.LoginCanceled
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoginWaiter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 840);
			ret.AddReturnedValue(ex, "", "LoginWaiter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 841);
			ret.value = new LoginResponseEventArgs
			{
				Status = EnLoginResult.ClientError
			};
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> DisposeAllAsync(IEnumerable<Client> clients)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (clients?.Any() ?? false)
			{
				foreach (Client client in clients)
				{
					if (client != null)
					{
						client.StopConnection -= Client_StopConnection;
						client.Connected -= Client_Connected;
						client.Disconnected -= Client_Disconnected;
						client.LoggedIn -= Client_LoggedIn;
						client.ReceivedRaw -= Client_ReceivedRaw;
						await client.DisposeAsync();
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 863);
		}
		return ret;
	}

	private async Task<Client> MeasurePerformance(CancellationToken token, int timeout = 60000)
	{
		try
		{
			if (IsDispsed || performanceSources == null)
			{
				return null;
			}
			Task delay = Task.Delay(timeout);
			IEnumerable<Task<string>> enumerable = performanceSources.Values?.Select((TaskCompletionSource<string> x) => x.Task);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			Task<Task<string>> t1 = Task.WhenAny(enumerable);
			if (await Task.WhenAny(new Task[2] { t1, delay }) == delay)
			{
				return null;
			}
			string winnerId = await (await t1);
			Task.Run(() => RaiseConnected(winnerId, token));
			Task.Run(() => RaiseStateChanged(EnConnectionState.Connected));
			if (Clients.TryGetValue(winnerId, out var value))
			{
				return value;
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MeasurePerformance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 889);
			return null;
		}
	}

	private Task Client_StopConnection(object sender, string e)
	{
		if (!isMeasuringPerformance)
		{
			return Utilities.RaiseEventOnNewThread(this.StopConnection, this, e);
		}
		return Task.CompletedTask;
	}

	private async Task SendHighWorkerAsync(CancellationToken token)
	{
		while (!token.IsCancellationRequested && !IsDispsed)
		{
			try
			{
				Packet packet = await _collection_High.ReceiveAsync(token);
				if (!(packet?.IsDisposed ?? false) && !(await Send(packet)))
				{
					_collection_High.Post(packet);
				}
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendHighWorkerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 919);
			}
		}
	}

	private async Task SendMediumWorkerAsync(CancellationToken token)
	{
		while (!token.IsCancellationRequested && !IsDispsed)
		{
			try
			{
				Packet packet = await _collection_Medium.ReceiveAsync(token);
				while (_collection_High.Count > 0)
				{
					await Task.Delay(100);
					if (IsDispsed || token.IsCancellationRequested || (packet?.IsDisposed ?? false))
					{
						return;
					}
				}
				if (!(packet?.IsDisposed ?? false) && !(await Send(packet)))
				{
					_collection_Medium.Post(packet);
				}
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendMediumWorkerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 942);
			}
		}
	}

	private async Task SendLowWorkerAsync(CancellationToken token)
	{
		while (!token.IsCancellationRequested && !IsDispsed)
		{
			try
			{
				Packet packet = await _collection_Low.ReceiveAsync(token);
				while (_collection_High.Count > 0 || _collection_Medium.Count > 0)
				{
					await Task.Delay(100);
					if (IsDispsed || token.IsCancellationRequested || (packet?.IsDisposed ?? false))
					{
						return;
					}
				}
				if (packet?.IsDisposed ?? false)
				{
					continue;
				}
				if (!(await Send(packet)))
				{
					_collection_Low.Post(packet);
				}
				if (_collection_High.Count <= 0 && _collection_Medium.Count <= 0 && _collection_Low.Count <= 0)
				{
					Task.Run(() => RaiseFree(new BaseEventArgs()));
					WebErrorLog.Instance.Trace("Free invoked", "", "SendLowWorkerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 967);
				}
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendLowWorkerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 972);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendData(List<Packet> lstPacket, Priority PriorityLevel)
	{
		using (new JaegerMonitor("SendData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1;
				if (lstPacket == null)
				{
					return ret.AddError(EnSystem.SignalR, "lstPacket == null", "lstPacket", "SendData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 983);
				}
				line = 2;
				foreach (Packet item in lstPacket)
				{
					line = 3;
					if (item != null)
					{
						line = 4;
						await SendData(item, PriorityLevel);
						line = 5;
					}
					line = 6;
				}
				line = 7;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"ListPacket//Line:{line}", "SendData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1000);
				ret.AddReturnedValue(ex, "", "SendData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1001);
			}
			return ret;
		}
	}

	public async Task SendData(Packet packetData, Priority PriorityLevel)
	{
		using (new JaegerMonitor("SendData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			int num = 0;
			try
			{
				_logger.GetOrAdd(packetData, null, isReceivedPacket: false, packetData.GetPacketActionName());
				num = 1;
				if (packetData != null)
				{
					num = 2;
					num = 3;
					switch (PriorityLevel)
					{
					case Priority.High:
						num = 4;
						_collection_High.Post(packetData);
						break;
					case Priority.Medium:
						num = 6;
						_collection_Medium.Post(packetData);
						num = 7;
						break;
					case Priority.Low:
						num = 8;
						_collection_Low.Post(packetData);
						num = 9;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"singlePacket//line:{num}", "SendData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1036);
			}
		}
	}

	private async Task<bool> Send(Packet packetData)
	{
		int line = 0;
		using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			_ = 3;
			try
			{
				line = 2;
				Client client = await NextClientAsync();
				line = 30;
				bool flag = client?.IsDisposed ?? true;
				if (!flag)
				{
					flag = await client.GetStateAsync() != EnConnectionState.Connected;
				}
				if (flag)
				{
					return false;
				}
				while (client.IsSending)
				{
					await Task.Delay(100);
					if (IsDispsed)
					{
						return false;
					}
				}
				bool result = await client.Send(packetData);
				if (IsDispsed)
				{
					return false;
				}
				packetData.Dispose();
				return result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"singlePacket//line={line}//isDisposed:{IsDispsed}//_collection_High is null:{_collection_High == null}", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1067);
			}
			return false;
		}
	}

	public Task SendData(Packet Data, string guid)
	{
		return SendData(Data, Priority.High);
	}

	public async Task<ReturnedSaveFuncInfo> SendAndWaitForResponse(List<Packet> packets, CancellationToken token, int TimeOutSeconds = 20, short RetryCount = 2, short NextRetrySeconds = 10, short BatchCount = 10)
	{
		if (packets == null)
		{
			throw new NullReferenceException("packets");
		}
		if (!packets.Any())
		{
			throw new ArgumentException("nameof(packets) is empty!");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (isMeasuringPerformance)
		{
			return ret.AddError(EnSystem.SignalR, "isMeasuringPerformancing...", "isMeasuringPerformance", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1077);
		}
		int line = 0;
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1081);
			}
			line = 1;
			if (packets == null || packets.Count <= 0)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1083);
			}
			line = 2;
			List<Task> sendingList = new List<Task>();
			line = 3;
			foreach (Packet packet in packets)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1089);
				}
				line = 4;
				if (packet == null)
				{
					throw new ArgumentException("packets contains null objects!");
				}
				line = 5;
				sendingList.Add(PacketDeliveryManager.Send(this, packet, Context, TimeOutSeconds, RetryCount, NextRetrySeconds, null));
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1096);
				}
				if (sendingList.Count >= BatchCount)
				{
					await Task.WhenAny(sendingList);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1099);
				}
			}
			line = 11;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1103);
			}
			await Task.WhenAll(sendingList);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1105);
			}
			line = 12;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1107);
			}
			if (packets.Any((Packet e) => e.SendResult == false))
			{
				return ret.AddError(EnSystem.SignalR, "packets.Any(e => e.SendResult == false)", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1108);
			}
			if (token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("", "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1109);
			}
		}
		catch (ArgumentException)
		{
			throw;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"line:{line}", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1115);
			ret.AddReturnedValue(ex3, "", "SendAndWaitForResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1116);
		}
		return ret;
	}

	private async Task<Client> NextClientAsync()
	{
		if (Clients == null || Clients.Count <= 0)
		{
			return null;
		}
		int line = 2;
		using (new JaegerMonitor("NextClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				line = 3;
				if (IsDispsed)
				{
					return null;
				}
				line = 4;
				Client client = default(Client);
				bool flag = !string.IsNullOrEmpty(winnerId) && Clients.TryGetValue(winnerId, out client) && client != null;
				if (flag)
				{
					flag = await client.GetStateAsync() == EnConnectionState.Connected;
				}
				if (flag && client.LoginState.Status == EnLoginResult.Success && !client.IsSending && !client.IsDisposed)
				{
					line = 5;
					return client;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Error in Line {line}", "NextClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1148);
			}
			return null;
		}
	}

	private async Task Client_LoggedIn(object sender, LoginResponseEventArgs e)
	{
		try
		{
			LastLoginState = e;
			_loggedInTask.TrySetResult(e);
			if (IsDispsed)
			{
				return;
			}
			Client client = (Client)sender;
			if (!string.IsNullOrEmpty(winnerId) && Clients.TryGetValue(winnerId, out var value) && client == value)
			{
				Task.Run(() => RaiseLoggedIn(e));
				Task.Run(() => SendHighWorkerAsync(e.connection.cancellationToken));
				Task.Run(() => SendMediumWorkerAsync(e.connection.cancellationToken));
				Task.Run(() => SendLowWorkerAsync(e.connection.cancellationToken));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Client_LoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1173);
		}
	}

	private async Task Client_Disconnected(object sender, SingleConnectionEventArgs e)
	{
		_ = 1;
		try
		{
			if (!IsDispsed && (string.IsNullOrEmpty(winnerId) || (Clients.TryGetValue(winnerId, out var value) && (Client)sender == value)))
			{
				EnConnectionState isConnected = await GetStateAsync();
				await Task.WhenAll(RaiseStateChanged(isConnected), RaiseDisconnected(e.ConnectionId));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Client_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1189);
		}
	}

	private Task Client_Connected(object sender, SingleConnectionEventArgs e)
	{
		using (new JaegerMonitor("Client_Connected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				if (IsDispsed)
				{
					return Task.CompletedTask;
				}
				if (sender == null)
				{
					return Task.CompletedTask;
				}
				Client client = (Client)sender;
				if (!performanceSources.TryGetValue(client.ConnectionId, out var value))
				{
					return Task.CompletedTask;
				}
				value.TrySetResult(client.ConnectionId);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Client_Connected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1203);
			}
			return Task.CompletedTask;
		}
	}

	public void Clear()
	{
		using (new JaegerMonitor("Clear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				_collection_High = new BufferBlock<Packet>();
				_collection_Medium = new BufferBlock<Packet>();
				_collection_Low = new BufferBlock<Packet>();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Clear", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1216);
			}
		}
	}

	private async Task RaiseFree(BaseEventArgs args)
	{
		try
		{
			await Utilities.RaiseEvent(this.Free, this, args, default(CancellationToken), 60, "RaiseFree", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseFree", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1225);
		}
	}

	private async Task RaiseDisconnected(string connectionUniqueId)
	{
		using (new JaegerMonitor("RaiseDisconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			int line = 0;
			try
			{
				if (!IsDispsed)
				{
					line = 1;
					await CleanUpAsync(default(CancellationToken));
					line = 2;
					token?.Cancel();
					line = 3;
					if (this.Disconnected != null)
					{
						line = 5;
						ConnectionState = EnConnectionState.Disconnected;
						Utilities.RaiseEvent(this.Disconnected, this, new BaseEventArgs
						{
							Connection = new ConnectionData(default(CancellationToken), Context),
							ConnectionUniqueId = connectionUniqueId,
							PacketReferenceId = Guid.Empty,
							Part = 0,
							TotalParts = 0
						}, default(CancellationToken), 60, "RaiseDisconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "RaiseDisconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1271);
			}
			finally
			{
				_connectionWatcherToken?.Cancel();
				_connectionWatcherToken = new CancellationTokenSource();
				CancellationToken tkn = _connectionWatcherToken.Token;
				Task.Run(() => ConnectionWatcher(tkn));
			}
		}
	}

	private Task RaiseConnected(string connectionUniqueId, CancellationToken token)
	{
		using (new JaegerMonitor("RaiseConnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				if (IsDispsed)
				{
					return Task.CompletedTask;
				}
				if (this.Connected == null)
				{
					return Task.CompletedTask;
				}
				ConnectionState = EnConnectionState.Connected;
				Utilities.RaiseEvent(this.Connected, this, new BaseEventArgs
				{
					Connection = new ConnectionData(token, Context),
					ConnectionUniqueId = connectionUniqueId,
					PacketReferenceId = Guid.Empty,
					Part = 0,
					TotalParts = 0
				}, default(CancellationToken), 60, "RaiseConnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RaiseConnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1312);
			}
			return Task.CompletedTask;
		}
	}

	private Task RaiseConnecting()
	{
		using (new JaegerMonitor("RaiseConnecting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				if (IsDispsed)
				{
					return Task.CompletedTask;
				}
				Func<object, EventArgs, Task> connecting = this.Connecting;
				if (connecting == null)
				{
					return Task.CompletedTask;
				}
				ConnectionState = EnConnectionState.Connecting;
				Delegate[] invocationList = connecting.GetInvocationList();
				Task[] handlerTasks = new Task[invocationList.Length];
				for (int i = 0; i < invocationList.Length; i = checked(i + 1))
				{
					handlerTasks[i] = ((Func<object, EventArgs, Task>)invocationList[i])(this, new EventArgs());
				}
				Task.Run(() => Task.WhenAll(handlerTasks));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RaiseConnecting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1332);
			}
			return Task.CompletedTask;
		}
	}

	private Task RaiseStateChanged(EnConnectionState isConnected)
	{
		using (new JaegerMonitor("RaiseStateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				if (IsDispsed)
				{
					return Task.CompletedTask;
				}
				Func<object, BufferStateChangedEventArgs, Task> stateChanged = this.StateChanged;
				if (stateChanged == null)
				{
					return Task.CompletedTask;
				}
				Delegate[] invocationList = stateChanged.GetInvocationList();
				Task[] handlerTasks = new Task[invocationList.Length];
				for (int i = 0; i < invocationList.Length; i = checked(i + 1))
				{
					handlerTasks[i] = ((Func<object, BufferStateChangedEventArgs, Task>)invocationList[i])(this, new BufferStateChangedEventArgs
					{
						Status = isConnected
					});
				}
				Task.Run(() => Task.WhenAll(handlerTasks));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RaiseStateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1360);
			}
			return Task.CompletedTask;
		}
	}

	private Task RaiseLoggedIn(LoginResponseEventArgs e)
	{
		using (new JaegerMonitor("RaiseLoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			try
			{
				LastLoginState = e;
				if (IsDispsed)
				{
					return Task.CompletedTask;
				}
				if (e.Status == EnLoginResult.Success)
				{
					failedConnections = 0;
				}
				if (this.LoggedIn == null)
				{
					return Task.CompletedTask;
				}
				Utilities.RaiseEvent(this.LoggedIn, this, e, default(CancellationToken), 60, "RaiseLoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs");
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RaiseLoggedIn", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1388);
			}
			return Task.CompletedTask;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<Buffer>> GetCustomerBufferAsync(string customerUserName, string customerPassword, CancellationToken token, int timeout = 60000)
	{
		using (new JaegerMonitor("GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs"))
		{
			string key = "";
			ReturnedSaveFuncInfoWithValue<Buffer> ret = new ReturnedSaveFuncInfoWithValue<Buffer>();
			int line = 0;
			try
			{
				if (IsDispsed)
				{
					ret.AddError(EnSystem.SignalR, "Disposed!", "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1400);
				}
				line = 1;
				if (_configs?.Any((ClientConfig conf) => conf.CustomerUsername == customerUserName) ?? false)
				{
					ret.value = this;
					return ret;
				}
				line = 5;
				key = customerUserName + "/" + customerPassword;
				line = 6;
				if (_CustomerBufferPool == null)
				{
					_CustomerBufferPool = new ConcurrentDictionary<string, Buffer>();
				}
				_CustomerBufferPool.TryGetValue(key, out var value);
				ret.value = value;
				line = 7;
				if ((ret.value?.LastLoginState?.Status).GetValueOrDefault() == EnLoginResult.Success)
				{
					return ret;
				}
				line = 10;
				ret.value?.Dispose();
				List<ClientConfig> list = new List<ClientConfig>();
				line = 11;
				if (Configs != null)
				{
					foreach (ClientConfig item in list)
					{
						if (item != null)
						{
							line = 12;
							item.DbGuid = DbGuid;
							item.SyncRequested = false;
							item.CustomerUsername = customerUserName;
							item.CustomerPassword = customerPassword;
							line = 15;
						}
					}
				}
				line = 16;
				ret.value = new Buffer(Source, kernel, Context, ConnectionType);
				line = 17;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1435);
				}
				ReturnedSaveFuncInfoWithValue<Buffer> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ret.value.ConnectAsync(list, token, DbGuid), ReturnedState.None, "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1436);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1437);
				}
				line = 18;
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ret.value.LoginWaiter(token), ReturnedState.None, "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1439);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1443);
				ret.AddReturnedValue(ex, "", "GetCustomerBufferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1444);
			}
			finally
			{
				_CustomerBufferPool.TryRemove(key, out var _);
				if (!ret.HasError && ret.value != null)
				{
					_CustomerBufferPool.TryAdd(key, ret.value);
				}
			}
			return ret;
		}
	}

	public void Dispose()
	{
		DisposeAsync();
	}

	public virtual async ValueTask DisposeAsync()
	{
		_ = 1;
		try
		{
			if (_isDisposed)
			{
				return;
			}
			await RaiseStateChanged(EnConnectionState.Disconnected);
			_isDisposed = true;
			if (this.Disconnected != null)
			{
				Utilities.RaiseEvent(this.Disconnected, this, new BaseEventArgs(), default(CancellationToken), 60, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs");
			}
			await DisposeAllAsync(Clients?.Values);
			this.LoggedIn = null;
			this.Connected = null;
			this.Disconnected = null;
			Clients = null;
			_configs = null;
			_internalConnectToken?.Cancel();
			_internalConnectToken = null;
			_connectionWatcherToken?.Cancel();
			_connectionWatcherToken = null;
			if (_CustomerBufferPool != null)
			{
				foreach (Buffer value in _CustomerBufferPool.Values)
				{
					value?.Dispose();
				}
			}
			_CustomerBufferPool?.Clear();
			_CustomerBufferPool = null;
			this.LoggedIn = null;
			this.Connected = null;
			_collection_High = null;
			performanceSources?.Clear();
			performanceSources = null;
			this.StopConnection = null;
			kernel = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Buffer.cs", 1492);
		}
	}
}
