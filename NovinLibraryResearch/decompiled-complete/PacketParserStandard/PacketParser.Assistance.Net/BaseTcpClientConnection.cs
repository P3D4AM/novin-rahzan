using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Scale;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Assistance.Net;

public abstract class BaseTcpClientConnection : INpDisposable, IDisposable
{
	private CancellationTokenSource _receivedTokenSource;

	private bool _isDisposed;

	protected readonly ILogger logger;

	private System.Timers.Timer disconnectionTimer;

	protected DateTime LastReceivedDate = DateTime.Now;

	private DateTime _lastReceivedTime = DateTime.Now;

	private DateTime _lastSentTime = DateTime.Now;

	public const int ConnectionTimeoutDuration = 20000;

	public const int ConnectionTimeoutCount = 3;

	public CancellationToken Token => _receivedTokenSource?.Token ?? new CancellationToken(canceled: true);

	public IPAddress Ip { get; private set; }

	public int Port { get; private set; }

	public ConnectionStatus Status
	{
		get
		{
			TcpClient connection = Connection;
			if (connection == null || !connection.Connected)
			{
				return ConnectionStatus.Disconnected;
			}
			return ConnectionStatus.Connected;
		}
	}

	public TcpClient Connection { get; private set; }

	public bool IsDisposed => _isDisposed;

	public abstract string PingCommand { get; }

	public short BufferSize { get; private set; }

	public string ConnectionUniqueId { get; private set; }

	public event Func<object, ReadEventArgs, Task> Received;

	public event Func<object, System.EventArgs, Task> Disposed;

	public event Func<object, DisconnectEventArgs, Task> Disconnected;

	protected BaseTcpClientConnection(int port, TcpClient connection, ILogger logger, short bufferSize = 1024)
	{
		Connection = connection ?? throw new ArgumentNullException("connection");
		this.logger = logger;
		Ip = ((IPEndPoint)connection.Client.RemoteEndPoint).Address;
		Port = checked((short)port);
		BufferSize = bufferSize;
		ConnectionUniqueId = Guid.NewGuid().ToString();
		Task.Run((Func<Task?>)ConnectionWatcherAsync);
	}

	private async Task ConnectionWatcherAsync()
	{
		await Task.Delay(60000);
		while (!IsDisposed)
		{
			try
			{
				if (_lastSentTime.AddMinutes(2.0) < DateTime.Now || _lastReceivedTime.AddMinutes(2.0) < DateTime.Now)
				{
					Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ConnectionWatcherAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 66);
				Dispose();
			}
			finally
			{
				if (!IsDisposed)
				{
					await Task.Delay(1000);
				}
			}
		}
	}

	private void Disconnect(DisconnectionReason reason)
	{
		try
		{
			if (IsDisposed)
			{
				return;
			}
			if (disconnectionTimer != null)
			{
				disconnectionTimer.Elapsed -= DisconnectionTimer_Elapsed;
			}
			switch (reason)
			{
			case DisconnectionReason.ReceiveTimeout:
			{
				Utilities.RaiseEvent(this.Disconnected, this, new DisconnectEventArgs("Connection closed due receive timeout.", Ip.ToString(), Port), CancellationToken.None, 60, "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs");
				ILogger obj2 = logger;
				if (obj2 != null)
				{
					obj2.Warning("Connection is being disconnected. Receive timeout has occured.");
				}
				break;
			}
			case DisconnectionReason.PingTimeout:
			{
				Utilities.RaiseEvent(this.Disconnected, this, new DisconnectEventArgs("Connection has reached maximum timeout count.", Ip.ToString(), Port), CancellationToken.None, 60, "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs");
				ILogger obj3 = logger;
				if (obj3 != null)
				{
					obj3.Warning("Connection is being disconnected. It has reached maximum timeout count.");
				}
				break;
			}
			default:
			{
				Utilities.RaiseEvent(this.Disconnected, this, new DisconnectEventArgs("Connection was half closed .", Ip.ToString(), Port), CancellationToken.None, 60, "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs");
				ILogger obj = logger;
				if (obj != null)
				{
					obj.Warning("Connection is being disconnected. It has been suddenly cut off from the other side.");
				}
				break;
			}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Disconnect", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 98);
			ILogger obj4 = logger;
			if (obj4 != null)
			{
				obj4.Error(ex, "");
			}
		}
		finally
		{
			Dispose();
		}
	}

	private void ResetDisconnectTimeout()
	{
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("baseTcpConnectionIsDisposed!!");
			}
			disconnectionTimer.Interval = 20000.0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetDisconnectTimeout", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 113);
			throw;
		}
	}

	private void DisconnectionTimer_Elapsed(object sender, ElapsedEventArgs e)
	{
		try
		{
			if (IsDisposed || string.IsNullOrEmpty(PingCommand))
			{
				return;
			}
			TimeSpan timeSpan = DateTime.Now.Subtract(LastReceivedDate);
			if (!(timeSpan.TotalMilliseconds > 20000.0))
			{
				return;
			}
			if (timeSpan.TotalMilliseconds > 60000.0)
			{
				Disconnect(DisconnectionReason.PingTimeout);
				Dispose();
				return;
			}
			ILogger obj = logger;
			if (obj != null)
			{
				obj.Information<string>("Connection timedout({@Ip}). Sending ping command.", Ip?.ToString());
			}
			SendAsync(PingCommand);
		}
		catch (Exception ex)
		{
			ILogger obj2 = logger;
			if (obj2 != null)
			{
				obj2.Error(ex, "");
			}
			WebErrorLog.Instance.StartLog(ex, "", "DisconnectionTimer_Elapsed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 140);
		}
	}

	public async Task SendAsync(string data, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (!Connection.Connected)
			{
				throw new Exception("The underlying connection is not connected");
			}
			byte[] bytes = CodePagesEncodingProvider.Instance.GetEncoding(1256).GetBytes(data);
			NetworkStream stream = Connection.GetStream();
			ILogger obj = logger;
			if (obj != null)
			{
				obj.Debug<string, string>("Sent data to scale({@ip}). {@data}", Ip?.ToString(), data);
			}
			if (!(token == CancellationToken.None))
			{
				await stream.WriteAsync(bytes, 0, bytes.Length, token);
			}
			else
			{
				await stream.WriteAsync(bytes, 0, bytes.Length);
			}
			_lastSentTime = DateTime.Now;
		}
		catch (IOException)
		{
			Dispose();
			throw;
		}
		catch (Exception ex2)
		{
			Dispose();
			WebErrorLog.Instance.StartLog(ex2, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 165);
			throw;
		}
	}

	public async Task SendAsync(byte[] data, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (IsDisposed)
			{
				throw new ObjectDisposedException("baseTcpConnection");
			}
			if (!Connection.Connected)
			{
				throw new Exception("The underlying connection is not connected");
			}
			NetworkStream stream = Connection.GetStream();
			ILogger obj = logger;
			if (obj != null)
			{
				obj.Debug<string, byte[]>("Sent data to scale({@ip}). {@data}", Ip?.ToString(), data);
			}
			if (!(token == CancellationToken.None))
			{
				await stream.WriteAsync(data, 0, data.Length, token);
			}
			else
			{
				await stream.WriteAsync(data, 0, data.Length);
			}
			_lastSentTime = DateTime.Now;
		}
		catch (SocketException)
		{
			Dispose();
			throw;
		}
		catch (IOException)
		{
			Dispose();
			throw;
		}
		catch (Exception ex3)
		{
			Dispose();
			WebErrorLog.Instance.StartLog(ex3, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 197);
			throw;
		}
	}

	private async Task ReceiveLoop(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				byte[] readBuffer2 = new byte[BufferSize];
				if (IsDisposed || token.IsCancellationRequested || Connection == null || !Connection.Connected)
				{
					break;
				}
				NetworkStream stream = Connection.GetStream();
				if (IsDisposed || token.IsCancellationRequested || Connection == null || !Connection.Connected)
				{
					break;
				}
				Task timeOut = Task.Delay(60000, token);
				Task<int> readTask = stream.ReadAsync(readBuffer2, 0, readBuffer2.Length, token);
				if (await Task.WhenAny(new Task[2] { readTask, timeOut }) == timeOut)
				{
					break;
				}
				int num = await readTask;
				if (num == 0)
				{
					await Task.Delay(100);
					continue;
				}
				readBuffer2 = readBuffer2.Take(num).ToArray();
				LastReceivedDate = DateTime.Now;
				if (IsDisposed)
				{
					break;
				}
				ResetDisconnectTimeout();
				string @string = CodePagesEncodingProvider.Instance.GetEncoding(1256).GetString(readBuffer2, 0, num);
				ILogger obj = logger;
				if (obj != null)
				{
					obj.Debug<string, string>("Received new data({ip}). {data}", Ip?.ToString(), @string);
				}
				if (this.Received != null && !token.IsCancellationRequested)
				{
					_lastReceivedTime = DateTime.Now;
					Utilities.RaiseEventOnNewThread(this.Received, this, new ReadEventArgs
					{
						Data = @string,
						Bytes = readBuffer2
					}, token);
				}
			}
			catch (OperationCanceledException)
			{
				Dispose();
				break;
			}
			catch (IOException)
			{
				Dispose();
				break;
			}
			catch (ObjectDisposedException)
			{
				Dispose();
				break;
			}
			catch (SocketException)
			{
				Dispose();
				break;
			}
			catch (Exception ex5)
			{
				WebErrorLog.Instance.StartLog(ex5, "", "ReceiveLoop", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 265);
				Dispose();
				break;
			}
		}
	}

	protected void StartReceiving()
	{
		if (_receivedTokenSource != null)
		{
			throw new InvalidOperationException("The connection has already been started");
		}
		_receivedTokenSource?.Cancel();
		_receivedTokenSource = new CancellationTokenSource();
		CancellationToken token = _receivedTokenSource.Token;
		Task.Run(() => ReceiveLoop(token));
	}

	public virtual Task InitializeAsync()
	{
		disconnectionTimer = new System.Timers.Timer(20000.0);
		disconnectionTimer.Elapsed += DisconnectionTimer_Elapsed;
		disconnectionTimer.Start();
		if (Connection.Connected)
		{
			ILogger obj = logger;
			if (obj != null)
			{
				obj.Verbose<IPAddress>("Connection established. {ip}", ((IPEndPoint)Connection.Client.RemoteEndPoint).Address);
			}
			StartReceiving();
		}
		return Task.CompletedTask;
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_receivedTokenSource?.Cancel();
				_receivedTokenSource?.Dispose();
				_receivedTokenSource = null;
				if (disconnectionTimer != null)
				{
					disconnectionTimer.Elapsed -= DisconnectionTimer_Elapsed;
				}
				Connection?.Close();
				Connection?.Dispose();
				Connection = null;
				disconnectionTimer?.Stop();
				disconnectionTimer?.Dispose();
				disconnectionTimer = null;
				this.Received = null;
				this.Disconnected = null;
				this.Disposed?.Invoke(this, new System.EventArgs());
				this.Disposed = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Client\\BaseTcpClientConnection.cs", 325);
		}
	}
}
