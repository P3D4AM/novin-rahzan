using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Connection;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Connection;
using Serilog;

namespace ClientHub;

public class Client : IAsyncDisposable, ISecuredConnection, IHasEncryption
{
	private CancellationTokenSource _pingToken = new CancellationTokenSource();

	private CancellationTokenSource _connectionToken = new CancellationTokenSource();

	private bool disposed;

	private bool _ConnectionPrefixIsOk;

	private readonly Guid clientGuid = Guid.NewGuid();

	private readonly SemaphoreSlim isSendingSemaphore = new SemaphoreSlim(1, 1);

	private TaskCompletionSource<LoginResponseEventArgs> source;

	private TaskCompletionSource<bool> securedSource = new TaskCompletionSource<bool>();

	private ClientConfig _config;

	private SymmetricAlgorithm symmetric = Aes.Create();

	private ICryptoTransform decryptor;

	private ICryptoTransform encryptor;

	private readonly SecurityNegotiator securityNegotiator;

	private int PingRecieved;

	private int pingTimeoutCount;

	private const int latestCapacity = 20;

	private readonly int pingDelay = 5000;

	private readonly int pingTimeoutLimit = 120;

	private readonly RSACryptoServiceProvider encryptionProvider;

	private bool isDisposed;

	private object temp = new object();

	public IConnection Connection { get; private set; }

	public bool IsDisposed
	{
		get
		{
			if (!isDisposed)
			{
				return Connection?.IsDisposed ?? true;
			}
			return true;
		}
		private set
		{
			isDisposed = value;
		}
	}

	public string ConnectionId => Connection.ConnectionId;

	public string serverUniqueId => Connection.ServerUniqueId;

	public CancellationTokenSource CancellationToken
	{
		get
		{
			if (Connection?.token == null)
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
				cancellationTokenSource.Cancel();
				return cancellationTokenSource;
			}
			return Connection.token;
		}
	}

	public bool IsSending { get; private set; }

	public bool IsSecure { get; private set; }

	public byte[] SymmetricIv { get; set; }

	public byte[] SymmetricKey { get; set; }

	public LoginResponseEventArgs LoginState { get; private set; } = new LoginResponseEventArgs
	{
		Status = EnLoginResult.Unknown
	};


	public ENCompressedMode CompressedMode { get; set; }

	public EncryptionType Encryption { get; set; } = EncryptionType.Custom;


	public WebContextHolder Context { get; private set; }

	public event Func<object, string, Task> StopConnection;

	public event Func<object, RawDataEventArgs, Task> ReceivedRaw;

	public event Func<object, SingleConnectionEventArgs, Task> Connected;

	public event Func<object, LoginResponseEventArgs, Task> LoggedIn;

	public event Func<object, SingleConnectionEventArgs, Task> Connecting;

	public event Func<object, SingleConnectionEventArgs, Task> Disconnected;

	public event Func<object, ConnectionStateEventArgs, Task> EnConnectionStateChanged;

	public event Func<object, EventArgs, Task> Sent;

	public event Func<object, PublicKeyEventArgs, Task> PublicKeyReceived;

	public event Func<object, EventArgs, Task> Secured;

	public event Func<object, EventArgs, Task> Reset;

	public Task<ReturnedSaveFuncInfo> Send(EnServerMessageInstruction instruction, string data, short TryCount = 2)
	{
		return Connection.Send(instruction, data, TryCount);
	}

	public async Task<EnConnectionState> GetStateAsync()
	{
		if (IsDisposed)
		{
			return EnConnectionState.Disposed;
		}
		if (Connection == null)
		{
			return EnConnectionState.Disconnected;
		}
		return await Connection.GetStateAsync();
	}

	public Client(WebContextHolder context, ClientConfig config, int Timeout, IConnection connection)
	{
		int num = 1;
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			try
			{
				num = 2;
				Context = context;
				num = 3;
				_config = config;
				num = 4;
				Connection = connection;
				num = 5;
				if (config.DbGuid == Guid.Empty && connection.Type != EnConnectionType.TcpConnection)
				{
					throw new ArgumentNullException("DbGuid");
				}
				num = 6;
				if (config.Url == null && connection.Type != EnConnectionType.TcpConnection)
				{
					throw new ArgumentNullException("Url");
				}
				num = 7;
				if (config.UserName == null && connection.Type != EnConnectionType.TcpConnection)
				{
					throw new ArgumentNullException("UserName");
				}
				num = 8;
				if (config.Password == null && connection.Type != EnConnectionType.TcpConnection)
				{
					throw new ArgumentNullException("Password");
				}
				num = 9;
				CompressedMode = config.CompressedMode;
				num = 10;
				num = 11;
				if (config.CpuSerial == null && connection.Type != EnConnectionType.TcpConnection)
				{
					throw new ArgumentException("CpuSerial");
				}
				num = 12;
				Context.LoginExtention.OnLoginResponse += Instance_OnLogin;
				connection.StopConnection += Connection_StopConnection;
				connection.Received += Connection_Received;
				connection.StateChanged += Connection_StateChanged;
				connection.Closed += Connection_Closed;
				connection.Reset += Connection_Reset;
				Connection.RecieveSystemMessages += Connection_RecieveSystemMessages;
				num = 19;
				encryptionProvider = new RSACryptoServiceProvider(2048);
				num = 20;
				securityNegotiator = new SecurityNegotiator(this);
				num = 21;
				securityNegotiator.KeyNegotiated += SecurityNegotiator_KeyNegotiated;
				num = 22;
				CompressedMode = config.CompressedMode;
				num = 23;
				if (!disposed)
				{
					num = 24;
					Connect(Timeout, _connectionToken.Token);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "error in line {@line}" + $"{num}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 185);
			}
		}
	}

	private async Task Connection_StateChanged(object sender, StateChange e)
	{
		if (disposed)
		{
			return;
		}
		using (new JaegerMonitor("Connection_StateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			int line = 0;
			try
			{
				line = 2;
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Warning("Old state: {@oldState} ,New state: {@newState} ,id: {@connectionId} ,url: {@url}, CustomerUserName: {@customerUsername}", new object[5]
					{
						e.OldState,
						e.NewState,
						Connection?.ConnectionId,
						_config.Url,
						_config?.CustomerUsername ?? ""
					});
				}
				line = 3;
				Task.Run(() => Utilities.RaiseEventOnNewThread(this.EnConnectionStateChanged, this, new ConnectionStateEventArgs
				{
					ConnectionUrl = _config.Url,
					NewState = e.NewState,
					PreviousState = e.OldState
				}));
				line = 4;
				if (e.NewState == EnConnectionState.Connected)
				{
					line = 5;
					Utilities.RaiseEventOnNewThread(this.Connected, this, new SingleConnectionEventArgs
					{
						ConnectionUrl = _config.Url,
						ConnectionId = ConnectionId,
						cancellationToken = (CancellationToken?.Token ?? new CancellationToken(canceled: true))
					});
					line = 6;
					try
					{
						if (Connection.Type != 0)
						{
							await Connection.Send(EnServerMessageInstruction.setting, new ClientConnectionSettings
							{
								clientVersion = _config.ClientVersion
							}.ToString(AddPrefix: false), 0);
						}
						else
						{
							await Connection.Send(Encoding.ASCII.GetBytes(new ClientConnectionSettings
							{
								clientVersion = _config.ClientVersion
							}.ToString()), 0);
						}
						ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
						if (connectionLogger2 != null)
						{
							connectionLogger2.Debug("client: settings sent");
						}
						line = 7;
						return;
					}
					catch (OperationCanceledException)
					{
						return;
					}
					catch (Exception ex2)
					{
						Connection?.Stop($"Connection_StateChanged//{ex2.Message}//Line:{line}");
						return;
					}
				}
				if (e.NewState == EnConnectionState.Connecting && e.OldState == EnConnectionState.Disconnected)
				{
					line = 11;
					Utilities.RaiseEventOnNewThread(this.Connecting, this, new SingleConnectionEventArgs
					{
						ConnectionId = ConnectionId,
						ConnectionUrl = _config.Url,
						cancellationToken = (CancellationToken?.Token ?? new CancellationToken(canceled: true))
					});
					line = 12;
				}
				else if (e.NewState == EnConnectionState.Disconnected)
				{
					Connection.ResetToken();
					line = 13;
					_pingToken?.Cancel();
					line = 14;
					ILogger connectionLogger3 = LogCache.Instance.Value.GetConnectionLogger();
					if (connectionLogger3 != null)
					{
						connectionLogger3.Debug("ping timer stopped");
					}
					line = 160;
					if (this.Disconnected != null)
					{
						line = 161;
						if (_config == null)
						{
							throw new Exception("_config is null!!");
						}
						Utilities.RaiseEventOnNewThread(this.Disconnected, this, new SingleConnectionEventArgs
						{
							ConnectionUrl = _config.Url,
							ConnectionId = ConnectionId,
							cancellationToken = (CancellationToken?.Token ?? new CancellationToken(canceled: true))
						});
					}
					line = 20;
					await DisconnectAsync("EnConnectionStateChange/NewState:Disconnected");
				}
				else if (e.NewState == EnConnectionState.Reconnecting)
				{
					line = 30;
					source = new TaskCompletionSource<LoginResponseEventArgs>();
					line = 31;
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"line:{line} Disposed:{disposed}", "Connection_StateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 276);
				ILogger connectionLogger4 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger4 != null)
				{
					connectionLogger4.Error<int, bool>(ex4, "line:{@line} Disposed:{@disposed}", line, disposed);
				}
			}
		}
	}

	private Task Connection_Reset(object sender, EventArgs e)
	{
		try
		{
			if (disposed)
			{
				return Task.CompletedTask;
			}
			return Utilities.RaiseEventOnNewThread(this.Reset, this, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Connection_Reset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 290);
			return Task.CompletedTask;
		}
	}

	private Task Connection_StopConnection(object sender, string e)
	{
		try
		{
			if (disposed)
			{
				return Task.CompletedTask;
			}
			return Utilities.RaiseEventOnNewThread(this.StopConnection, this, e);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Connection_StopConnection", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 305);
			return Task.CompletedTask;
		}
	}

	private void SecurityNegotiator_KeyNegotiated(object sender, KeyEventArgs e)
	{
		try
		{
			SymmetricIv = e.SymmetricIv;
			SymmetricKey = e.SymmetricKey;
			securedSource.TrySetResult(result: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SecurityNegotiator_KeyNegotiated", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 318);
		}
		securityNegotiator.KeyNegotiated -= SecurityNegotiator_KeyNegotiated;
	}

	public async Task<bool> Send(Packet pack, int maxTryCount = 10)
	{
		int line = 0;
		if (IsDisposed)
		{
			return false;
		}
		if (pack == null)
		{
			return false;
		}
		if (Connection?.token == null || (Connection?.token.IsCancellationRequested ?? true))
		{
			return false;
		}
		if (await GetStateAsync() != EnConnectionState.Connected)
		{
			return false;
		}
		using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			bool ret = false;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					line = 1;
					await isSendingSemaphore.WaitAsync();
					try
					{
						line = 2;
						IsSending = true;
					}
					finally
					{
						isSendingSemaphore.Release();
					}
					line = 3;
					byte[] data = pack.ToBytes();
					line = 4;
					if (LoginState.Status == EnLoginResult.Success && IsSecure)
					{
						ret = await Send(data, isLoginPacket: false);
						pack.Dispose();
					}
					else
					{
						ret = false;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "#Methode3\r\n" + $"#Line_{line}", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 365);
					ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
					if (connectionLogger != null)
					{
						connectionLogger.Error(ex, "");
					}
					DisconnectAsync("Send Exception:" + ex.Message);
				}
			}
			finally
			{
				await isSendingSemaphore.WaitAsync();
				try
				{
					IsSending = false;
				}
				finally
				{
					isSendingSemaphore.Release();
				}
				if (ret)
				{
					ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
					if (connectionLogger2 != null)
					{
						connectionLogger2.Information("sent(type:{@packetType}({@packetTypeShort}){@packetTypeName}) (Action({@packetAction}):{@packetActionName})/ExtraData:{@extraData} Length:{@packetLength}", new object[7]
						{
							pack.PacketType,
							checked((short)pack.PacketType),
							pack.PacketType.GetDisplay(),
							pack.PacketActions,
							pack.GetPacketActionName(),
							pack.ExteraDataForDebug,
							pack.ToBytes().Length
						});
					}
					if (this.Sent != null)
					{
						Utilities.RaiseEventOnNewThread(this.Sent, this, new EventArgs());
					}
				}
			}
			return ret;
		}
	}

	private async Task<bool> Send(byte[] data, bool isLoginPacket)
	{
		if (disposed)
		{
			return false;
		}
		int line = 0;
		try
		{
			string msg = $"Size:{data.Length}";
			line = 1;
			int originalSize = data.Length;
			line = 2;
			if (!isLoginPacket)
			{
				line = 3;
				data = await ConditionalCompressAsync(data, 10);
				line = 4;
				_ = msg + $"/CompressedSize:{data.Length}/Ratio:{100m * Math.Round((decimal)data.Length / (decimal)originalSize, 4)}%";
			}
			line = 5;
			byte[] array = await ConditionalEncryptAsync(data, isLoginPacket);
			line = 6;
			if (array == null)
			{
				return false;
			}
			line = 7;
			if (!_ConnectionPrefixIsOk)
			{
				await Connection.Send(array, 0);
			}
			else
			{
				line = 8;
				foreach (byte[] item in ChunkingOperations.Chunk(array))
				{
					await Connection.Send(item, 0);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "#Methode1\r\n" + $"#Line_{line}", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 435);
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			return false;
		}
		return true;
	}

	public async Task<bool> Send(List<Packet> packetData, int maxTryCount = 10)
	{
		bool ret = true;
		int line = 0;
		try
		{
			if (disposed)
			{
				return false;
			}
			line = 2;
			foreach (Packet packetDatum in packetData)
			{
				line = 3;
				if (packetDatum != null)
				{
					line = 4;
					if (!(await Send(packetDatum, maxTryCount)))
					{
						ret = false;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "#Methode2\r\n" + $"#Line_{line}", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 470);
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendRaw(string data)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (disposed)
			{
				ret.AddReturnedValue(ReturnedState.Error, "Disposed !", "", "SendRaw", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 490);
				return ret;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Connection.Send(Encoding.ASCII.GetBytes("&Raw:" + data), 0), ReturnedState.None, "SendRaw", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 493);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendRaw", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 497);
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			ret.AddReturnedValue(ex, "", "SendRaw", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 499);
		}
		return ret;
	}

	[DebuggerNonUserCode]
	private async Task<byte[]> EncryptCustomAsync(byte[] data, short tryCount = 10)
	{
		byte[] result = default(byte[]);
		object obj;
		int num;
		try
		{
			if (SymmetricKey == null || SymmetricIv == null)
			{
				throw new Exception("Symmetric key is null.");
			}
			encryptor = symmetric.CreateEncryptor(SymmetricKey, SymmetricIv);
			result = encryptor.TransformFinalBlock(data, 0, data.Length);
			return result;
		}
		catch (Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		Exception ex2 = (Exception)obj;
		if (tryCount > 0)
		{
			await Task.Delay(1000);
			Client client = this;
			short num2 = checked((short)(tryCount - 1));
			tryCount = num2;
			return await client.EncryptCustomAsync(data, num2);
		}
		if (!ex2.Message.ToLower().Contains("The handle is invalid".ToLower()))
		{
			WebErrorLog.Instance.StartLog(ex2, "", "EncryptCustomAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 524);
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex2, "");
			}
		}
		return null;
	}

	private async Task<byte[]> ConditionalEncryptAsync(byte[] data, bool isLoginPacket)
	{
		_ = 1;
		try
		{
			if (isLoginPacket)
			{
				return await EncryptCustomAsync(data, 10);
			}
			if (_config.Encryption == EncryptionType.None)
			{
				return data;
			}
			if (_config.Encryption == EncryptionType.Custom)
			{
				return await EncryptCustomAsync(data, 10);
			}
			return data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ConditionalEncryptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 542);
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			return null;
		}
	}

	private async Task<byte[]> DecryptCustomAsync(byte[] data)
	{
		try
		{
			if (SymmetricKey == null || SymmetricIv == null)
			{
				throw new Exception("Symmetric key is null.");
			}
			decryptor = symmetric.CreateDecryptor(SymmetricKey, SymmetricIv);
			return decryptor.TransformFinalBlock(data, 0, data.Length);
		}
		catch (Exception ex)
		{
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "DecryptCustomAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 560);
			return null;
		}
	}

	private async Task<byte[]> ConditionalDecryptAsync(byte[] data)
	{
		try
		{
			if (_config.Encryption != 0)
			{
				return await DecryptCustomAsync(data);
			}
			return data;
		}
		catch (Exception ex)
		{
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "ConditionalDecryptAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 577);
			return null;
		}
	}

	private async Task<byte[]> ConditionalCompressAsync(byte[] data, short tryCount = 10)
	{
		_ = 1;
		try
		{
			if (CompressedMode == ENCompressedMode.None)
			{
				return data;
			}
			if (CompressedMode == ENCompressedMode.GZip)
			{
				return await CompressGzipAsync(data, tryCount);
			}
			if (CompressedMode == ENCompressedMode.Brotli)
			{
				return await CompressBrotliAsync(data, tryCount);
			}
			return data;
		}
		catch (Exception ex)
		{
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "ConditionalCompressAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 599);
			return null;
		}
	}

	private async Task<byte[]> CompressGzipAsync(byte[] data, short tryCount = 10)
	{
		byte[] result = default(byte[]);
		object obj;
		int num;
		try
		{
			result = await GZip.Compress(data);
			return result;
		}
		catch (Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		Exception ex2 = (Exception)obj;
		ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
		if (connectionLogger != null)
		{
			connectionLogger.Error(ex2, "");
		}
		if (tryCount > 10)
		{
			await Task.Delay(1000);
			Client client = this;
			short num2 = checked((short)(tryCount - 1));
			tryCount = num2;
			return await client.CompressGzipAsync(data, num2);
		}
		WebErrorLog.Instance.StartLog(ex2, "", "CompressGzipAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 618);
		return null;
	}

	private async Task<byte[]> CompressBrotliAsync(byte[] data, short tryCount = 10)
	{
		byte[] result = default(byte[]);
		object obj;
		int num;
		try
		{
			result = await Brotli.Compress(data);
			return result;
		}
		catch (Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		Exception ex2 = (Exception)obj;
		ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
		if (connectionLogger != null)
		{
			connectionLogger.Error(ex2, "");
		}
		if (tryCount > 10)
		{
			await Task.Delay(1000);
			Client client = this;
			short num2 = checked((short)(tryCount - 1));
			tryCount = num2;
			return await client.CompressBrotliAsync(data, num2);
		}
		WebErrorLog.Instance.StartLog(ex2, "", "CompressBrotliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 637);
		return null;
	}

	private async Task<byte[]> ConditionalDecompressAsync(byte[] data, short tryCount = 10)
	{
		_ = 1;
		try
		{
			if (CompressedMode == ENCompressedMode.None)
			{
				return data;
			}
			if (CompressedMode == ENCompressedMode.GZip)
			{
				return await DecompressGzipAsync(data, tryCount);
			}
			if (CompressedMode == ENCompressedMode.Brotli)
			{
				return await DecompressBrotliAsync(data, tryCount);
			}
			return data;
		}
		catch (Exception ex)
		{
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "ConditionalDecompressAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 659);
			return null;
		}
	}

	private async Task<byte[]> DecompressGzipAsync(byte[] data, short tryCount = 10)
	{
		byte[] result = default(byte[]);
		object obj;
		int num;
		try
		{
			result = await GZip.Decompress(data);
			return result;
		}
		catch (Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		Exception ex2 = (Exception)obj;
		ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
		if (connectionLogger != null)
		{
			connectionLogger.Error(ex2, "");
		}
		if (tryCount > 10)
		{
			await Task.Delay(1000);
			Client client = this;
			short num2 = checked((short)(tryCount - 1));
			tryCount = num2;
			return await client.DecompressGzipAsync(data, num2);
		}
		WebErrorLog.Instance.StartLog(ex2, "could not decompress gzip", "DecompressGzipAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 678);
		return null;
	}

	private async Task<byte[]> DecompressBrotliAsync(byte[] data, short tryCount = 10)
	{
		byte[] result = default(byte[]);
		object obj;
		int num;
		try
		{
			result = await Brotli.Decompress(data);
			return result;
		}
		catch (Exception ex)
		{
			obj = ex;
			num = 1;
		}
		if (num != 1)
		{
			return result;
		}
		Exception ex2 = (Exception)obj;
		ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
		if (connectionLogger != null)
		{
			connectionLogger.Error(ex2, "");
		}
		if (tryCount > 10)
		{
			await Task.Delay(1000);
			Client client = this;
			short num2 = checked((short)(tryCount - 1));
			tryCount = num2;
			return await client.DecompressBrotliAsync(data, num2);
		}
		WebErrorLog.Instance.StartLog(ex2, "could not decompress brotli", "DecompressBrotliAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 697);
		return null;
	}

	private async Task Connection_Received(object sender, SingleConnectionDataEventArgs e)
	{
		StringBuilder sb = new StringBuilder(100000);
		if (disposed)
		{
			return;
		}
		int line = 1;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken token = e?.cancellationToken ?? new CancellationToken(canceled: true);
			line = 2;
			if (token.IsCancellationRequested || ret.HasError || e?.Data == null)
			{
				return;
			}
			line = 3;
			pingTimeoutCount = 0;
			line = 4;
			_pingToken?.Cancel();
			_pingToken = new CancellationTokenSource();
			CancellationToken tkn = _pingToken.Token;
			Task.Run(() => PingAsync(tkn));
			if (CheckSystemMessages(e.Data))
			{
				return;
			}
			line = 6;
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Debug("Data received. Ping timer reseted");
			}
			line = 7;
			if (token.IsCancellationRequested || ret.HasError || e?.Data == null)
			{
				return;
			}
			byte[] array = await ConditionalDecryptAsync(e.Data);
			line = 8;
			if (token.IsCancellationRequested || ret.HasError || array == null || e?.Data == null)
			{
				return;
			}
			array = await ConditionalDecompressAsync(array, 10);
			if (token.IsCancellationRequested || ret.HasError || array == null || e?.Data == null)
			{
				return;
			}
			line = 100;
			if (token.IsCancellationRequested || ret.HasError)
			{
				return;
			}
			line = 110;
			ConnectionData connection = new ConnectionData(CancellationToken.Token, Context)
			{
				Source = e.Source
			};
			ReturnedSaveFuncInfoWithValue<Packet> returnedSaveFuncInfoWithValue = await Packet.ParsePacket(Context, array, ConnectionId, connection, sb);
			if (returnedSaveFuncInfoWithValue?.value != null)
			{
				ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger2 != null)
				{
					connectionLogger2.Debug("Data received. (type: {@type} ({@packetTypeShort}){@packetTypeDisplay}) (Action({@packetAction}):{@packetActionEnumName}-{@packetActionName})Length: {@packetLength}/ExtraData:{@extraData}", new object[8]
					{
						returnedSaveFuncInfoWithValue.value.PacketType.ToString(),
						checked((short)returnedSaveFuncInfoWithValue.value.PacketType),
						returnedSaveFuncInfoWithValue.value.PacketType.GetDisplay(),
						returnedSaveFuncInfoWithValue.value.PacketActions,
						returnedSaveFuncInfoWithValue.value.GetPacketActionEnumName(),
						returnedSaveFuncInfoWithValue.value.GetPacketActionName(),
						returnedSaveFuncInfoWithValue.value.ToBytes().Length,
						returnedSaveFuncInfoWithValue?.value?.ExteraDataForDebug
					});
				}
			}
			else
			{
				WebErrorLog.Instance.StartLog("Data received. pack.value is null. Errors:{@errors}", returnedSaveFuncInfoWithValue?.GetErrorMessage(), "Connection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 756);
				ILogger connectionLogger3 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger3 != null)
				{
					connectionLogger3.Error<string>("Data received. pack.value is null. Errors:{@errors}", returnedSaveFuncInfoWithValue?.GetErrorMessage());
				}
			}
			line = 111;
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Connection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 760);
			line = 112;
			if (token.IsCancellationRequested || ret.HasError)
			{
				return;
			}
			line = 12;
			if (returnedSaveFuncInfoWithValue != null && (returnedSaveFuncInfoWithValue.value?.DeliveryRequested ?? false))
			{
				line = 15;
				Packet packet = returnedSaveFuncInfoWithValue.value?.GenerateDeliveryResponse();
				line = 150;
				if (packet != null)
				{
					await Send(packet);
				}
				line = 16;
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Connection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 774);
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "Connection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 778);
			ILogger connectionLogger4 = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger4 != null)
			{
				connectionLogger4.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, $"line: {line}", "Connection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 780);
		}
	}

	private bool CheckSystemMessages(byte[] data)
	{
		try
		{
			if (disposed)
			{
				return true;
			}
			string @string = Encoding.ASCII.GetString(data);
			if (@string.StartsWith("$public:"))
			{
				@string = @string.Replace("$public:", "");
				Utilities.RaiseEventOnNewThread(this.PublicKeyReceived, this, new PublicKeyEventArgs
				{
					Data = @string
				});
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Verbose("public key received");
				}
				return true;
			}
			if (@string == "$IsSecure$")
			{
				IsSecure = true;
				Utilities.RaiseEventOnNewThread(this.Secured, this, new EventArgs());
				ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger2 != null)
				{
					connectionLogger2.Information("channel is secured");
				}
				securedSource.TrySetResult(result: true);
				return true;
			}
			if (@string == "exit")
			{
				ExitReceived();
				return true;
			}
			if (@string.StartsWith("&Raw:"))
			{
				Utilities.RaiseEventOnNewThread(this.ReceivedRaw, this, new RawDataEventArgs(ConnectionId)
				{
					Payload = @string.Substring(5)
				});
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			ILogger connectionLogger3 = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger3 != null)
			{
				connectionLogger3.Error(ex, "Could not parse system message");
			}
			WebErrorLog.Instance.StartLog(ex, "Could not parse system message", "CheckSystemMessages", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 824);
			return false;
		}
	}

	private void Connection_Closed()
	{
		try
		{
			if (disposed)
			{
				return;
			}
			using (new JaegerMonitor("Connection_Closed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
			{
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Error<string, string>("connection closed on url: {@url}, Id: {@id}", _config?.Url, Connection?.ConnectionId);
				}
				WebErrorLog.Instance.StartLog("connection closed on url: {@url}, Id: {@id}", _config?.Url, Connection?.ConnectionId, "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 836);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Connection_Closed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 838);
		}
	}

	private Task Instance_OnLogin(object sender, LoginResponseEventArgs e)
	{
		if (disposed)
		{
			return Task.CompletedTask;
		}
		using (new JaegerMonitor("Instance_OnLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			int num = 0;
			try
			{
				num = 1;
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Debug<DateTime, EnLoginResult>("******CLIENT Instance_OnLogin******{@date}******{@status}******", DateTime.Now, e.Status);
				}
				num = 2;
				if (e.ConnectionUniqueId == ConnectionId)
				{
					num = 3;
					LoginState = e;
					num = 4;
					source.TrySetResult(e);
					num = 5;
				}
			}
			catch (Exception ex)
			{
				ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger2 != null)
				{
					connectionLogger2.Error<int, bool, string>(ex, "line:{@line}//is e null:{@isNull}//e:{@e}", num, e == null, e?.ToString() ?? "Null!!");
				}
				WebErrorLog.Instance.StartLog(ex, string.Format("line:{0}//is e null:{1}//e:{2}", num, e == null, e?.ToString() ?? "Null!!"), "Instance_OnLogin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 863);
			}
			return Task.CompletedTask;
		}
	}

	public async Task<LoginResponseEventArgs> Login(CancellationToken token, int timeoutLength = 5000)
	{
		if (disposed)
		{
			return new LoginResponseEventArgs
			{
				Status = EnLoginResult.Unknown
			};
		}
		using (new JaegerMonitor("Login", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			int line = 0;
			try
			{
				Task timeout = Task.Delay(timeoutLength);
				line = 1;
				if (Connection == null)
				{
					return new LoginResponseEventArgs
					{
						Status = EnLoginResult.LoginCanceled
					};
				}
				line = 2;
				line = 3;
				Packet pack = LoginExtention.GenerateLoginPacket(_config.ToLoginExtention());
				line = 4;
				line = 5;
				if (!(await WaitForConnection()))
				{
					return new LoginResponseEventArgs
					{
						Status = EnLoginResult.ConnectFailed,
						LoginResultMessage = "wait for connect timeOut"
					};
				}
				line = 6;
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Debug<string>("Logging in .... on url:{@url}", _config.Url);
				}
				Task<bool> task = Send(pack.ToBytes(), isLoginPacket: true);
				Task<LoginResponseEventArgs> t1 = source.Task;
				if (t1 == null)
				{
					return new LoginResponseEventArgs
					{
						Status = EnLoginResult.ClientError,
						LoginResultMessage = "t1 is null"
					};
				}
				if (!(await task))
				{
					return new LoginResponseEventArgs
					{
						Status = EnLoginResult.ConnectFailed,
						LoginResultMessage = "failed to send login packet"
					};
				}
				if (await Task.WhenAny(new Task[2] { timeout, t1 }) == timeout)
				{
					return new LoginResponseEventArgs
					{
						Status = EnLoginResult.TimeOut,
						LoginResultMessage = "login respose timeout/request has been sent"
					};
				}
				line = 15;
				LoginState = await t1;
				line = 16;
				ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger2 != null)
				{
					connectionLogger2.Information<string, string, EnLoginResult>("login successful on userName: {@username} url: {@url} status is: {@loginState}", _config.UserName, _config.Url, LoginState.Status);
				}
				line = 17;
				if (LoginState.Status == EnLoginResult.Success)
				{
					line = 18;
					_pingToken?.Cancel();
					_pingToken = new CancellationTokenSource();
					CancellationToken tkn = _pingToken.Token;
					Task.Run(() => PingAsync(tkn));
					line = 19;
					pingTimeoutCount = 0;
					line = 191;
					ILogger connectionLogger3 = LogCache.Instance.Value.GetConnectionLogger();
					if (connectionLogger3 != null)
					{
						connectionLogger3.Verbose("ping timer started");
					}
					line = 192;
				}
				line = 20;
				if (this.LoggedIn != null)
				{
					Func<object, LoginResponseEventArgs, Task> loggedIn = this.LoggedIn;
					object sender = this;
					await Utilities.RaiseEventOnNewThread(loggedIn, sender, await t1);
				}
				line = 22;
				return await t1;
			}
			catch (OperationCanceledException)
			{
				return new LoginResponseEventArgs
				{
					Status = EnLoginResult.LoginCanceled
				};
			}
			catch (Exception ex2)
			{
				ILogger connectionLogger4 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger4 != null)
				{
					connectionLogger4.Error<int>(ex2, "Line: {@line}", line);
				}
				WebErrorLog.Instance.StartLog(ex2, $"Line: {line}", "Login", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 934);
				return new LoginResponseEventArgs
				{
					Status = EnLoginResult.ClientError,
					LoginResultMessage = ex2.Message
				};
			}
		}
	}

	private async Task<bool> WaitForConnection()
	{
		Task timeOut = Task.Delay(60000);
		if (await Task.WhenAny(new Task[2] { securedSource.Task, timeOut }) == timeOut)
		{
			return false;
		}
		return true;
	}

	public async ValueTask DisposeAsync()
	{
		if (!IsDisposed)
		{
			IsDisposed = true;
			await DisposeAsync(disposing: true);
		}
	}

	public virtual async Task DisposeAsync(bool disposing)
	{
		using (new JaegerMonitor("DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			int line = 0;
			try
			{
				_pingToken?.Cancel();
				_connectionToken?.Cancel();
				line = 1;
				line = 3;
				if (Connection != null)
				{
					Context.LoginExtention.OnLoginResponse -= Instance_OnLogin;
					Connection.StopConnection -= Connection_StopConnection;
					Connection.Received -= Connection_Received;
					Connection.StateChanged -= Connection_StateChanged;
					Connection.Closed -= Connection_Closed;
					Connection.RecieveSystemMessages -= Connection_RecieveSystemMessages;
					Connection.Reset -= Connection_Reset;
				}
				line = 4;
				try
				{
					line = 5;
					await DisconnectAsync("Dispose");
					line = 6;
				}
				catch
				{
				}
				line = 7;
				line = 8;
				this.Connected = (this.Connecting = (this.Disconnected = null));
				line = 10;
				this.Sent = null;
				line = 11;
				this.LoggedIn = null;
				line = 12;
				line = 13;
				this.EnConnectionStateChanged = null;
				line = 14;
				if (Connection != null)
				{
					Connection?.Dispose();
				}
				line = 16;
				line = 17;
				_pingToken?.Cancel();
				line = 25;
			}
			catch (Exception ex)
			{
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Error<int>(ex, "line: {@line}", line);
				}
				WebErrorLog.Instance.StartLog(ex, $"line: {line}", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1015);
			}
		}
	}

	private async Task Connection_RecieveSystemMessages(object sender, SingleConnectionSystemMessageEventArgs e)
	{
		_ = 2;
		try
		{
			if (!isDisposed)
			{
				switch (e.Instruction)
				{
				case EnServerMessageInstruction.pongNotExists:
					await ExitReceived();
					break;
				case EnServerMessageInstruction.publicKey:
					await Utilities.RaiseEventOnNewThread(this.PublicKeyReceived, this, new PublicKeyEventArgs
					{
						Data = e.Data
					});
					break;
				case EnServerMessageInstruction.IsSecured:
					IsSecure = true;
					await Utilities.RaiseEventOnNewThread(this.Secured, this, new EventArgs());
					break;
				case EnServerMessageInstruction.ping:
				case EnServerMessageInstruction.pongExists:
				case EnServerMessageInstruction.symmetricKey:
				case EnServerMessageInstruction.setting:
					break;
				}
			}
		}
		catch (Exception ex)
		{
			ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger != null)
			{
				connectionLogger.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "Connection_RecieveSystemMessages", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1051);
		}
	}

	public async Task DisconnectAsync(string Reason)
	{
		using (new JaegerMonitor("DisconnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				if (!IsDisposed)
				{
					line = 2;
					bool flag = Connection != null;
					if (flag)
					{
						flag = await Connection.GetStateAsync() != EnConnectionState.Disconnected;
					}
					if (flag)
					{
						source?.TrySetCanceled();
					}
					line = 3;
					Connection?.Stop("Disconnect:" + Reason);
					line = 4;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Error<bool, int>(ex2, "isdisposed: {@IsDisposed}//line:{@line}", isDisposed, line);
				}
				WebErrorLog.Instance.StartLog(ex2, $"isdisposed: {isDisposed}//line:{line}", "DisconnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1079);
			}
		}
	}

	private async Task ExitReceived()
	{
		using (new JaegerMonitor("ExitReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			try
			{
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Information("exit received");
				}
				await DisconnectAsync("ExitReceived");
			}
			catch (TimeoutException)
			{
			}
			catch (Exception ex2)
			{
				ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger2 != null)
				{
					connectionLogger2.Error(ex2, "");
				}
				WebErrorLog.Instance.StartLog(ex2, "", "ExitReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1095);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Connect(int timeout, CancellationToken token)
	{
		using (new JaegerMonitor("Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1110);
				}
				if (disposed)
				{
					return ret.AddError(EnSystem.SignalR, "Client is disposed!!", "DisposeAsync", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1111);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1112);
				}
				if (Connection == null)
				{
					throw new NullReferenceException("Connection");
				}
				line = 1;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1115);
				}
				line = 2;
				EnConnectionState enConnectionState = await Connection.GetStateAsync();
				if (enConnectionState == EnConnectionState.Connected || enConnectionState == EnConnectionState.Connecting)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1121);
					}
					line = 7;
					return ret.AddInformation("Connection.State == EnConnectionState.Connected || Connection.State == EnConnectionState.Connecting", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1123);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1125);
				}
				line = 10;
				source?.TrySetCanceled();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1128);
				}
				line = 11;
				source = new TaskCompletionSource<LoginResponseEventArgs>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1131);
				}
				line = 12;
				if (Connection != null)
				{
					await Task.WhenAny(new Task[2]
					{
						Connection.Start(token),
						Task.Delay(timeout, token)
					});
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1136);
				}
				line = 13;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1139);
			}
			catch (TimeoutException exception2)
			{
				ret.AddReturnedValue(exception2, "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1140);
			}
			catch (SocketException exception3)
			{
				ret.AddReturnedValue(exception3, "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1141);
			}
			catch (HttpRequestException exception4)
			{
				ret.AddReturnedValue(exception4, "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1142);
			}
			catch (Exception ex)
			{
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Error(ex, "");
				}
				ret.AddReturnedValue(ex, "", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1146);
				if (!ex.Message.ToLower().StartsWith("StatusCode: 500".ToLower()) && !ex.Message.ToLower().StartsWith("StatusCode: 400".ToLower()) && !ex.Message.ToLower().StartsWith("StatusCode: 503".ToLower()) && !ex.Message.ToLower().StartsWith("StatusCode: 404".ToLower()))
				{
					WebErrorLog.Instance.StartLog(ex, $"line:{line}", "Connect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1155);
				}
			}
			return ret;
		}
	}

	private async Task SendPing()
	{
		if (disposed)
		{
			return;
		}
		try
		{
			if (await GetStateAsync() != EnConnectionState.Connected)
			{
				return;
			}
			if (Connection.Type != EnConnectionType.SignalRCore && Connection.Type != EnConnectionType.TcpConnection)
			{
				await Connection.Send(Encoding.ASCII.GetBytes("&ping*"), 0);
			}
			else
			{
				await Connection.Send(EnServerMessageInstruction.ping, "", 0);
			}
			if (checked(PingRecieved++) % 30 == 0)
			{
				ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
				if (connectionLogger != null)
				{
					connectionLogger.Verbose<string>("ping sent from client {@client}", Connection?.ConnectionId);
				}
			}
		}
		catch (Exception ex)
		{
			ILogger connectionLogger2 = LogCache.Instance.Value.GetConnectionLogger();
			if (connectionLogger2 != null)
			{
				connectionLogger2.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "SendPing", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1179);
		}
	}

	private async Task PingAsync(CancellationToken token)
	{
		checked
		{
			while (!token.IsCancellationRequested)
			{
				try
				{
					await Task.Delay(pingDelay);
					if (token.IsCancellationRequested)
					{
						break;
					}
					if (pingTimeoutCount > pingTimeoutLimit)
					{
						Connection?.Stop("PingTimer_Elapsed");
						continue;
					}
					pingTimeoutCount++;
					if (token.IsCancellationRequested)
					{
						break;
					}
					await SendPing();
				}
				catch (OperationCanceledException)
				{
					break;
				}
				catch (Exception ex2)
				{
					ILogger connectionLogger = LogCache.Instance.Value.GetConnectionLogger();
					if (connectionLogger != null)
					{
						connectionLogger.Error(ex2, "");
					}
					WebErrorLog.Instance.StartLog(ex2, "", "PingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Client.cs", 1203);
				}
			}
		}
	}
}
