using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Scale.EventArguments;
using Serilog;

namespace PacketParser.Assistance.Net;

public abstract class TcpServerBaseConnection<T> : INpDisposable, IDisposable where T : BaseTcpClientConnection, INpDisposable
{
	private SemaphoreSlim _handleConnectionSemaphore = new SemaphoreSlim(1, 1);

	protected List<T> _clients = new List<T>();

	private CancellationTokenSource _token;

	private TcpListener listener;

	public readonly int Port;

	protected readonly ILogger logger;

	private bool _isDisposed;

	public bool IsDisposed => _isDisposed;

	public TcpServerBaseConnection(IPAddress address, int port, ILogger logger)
	{
		Port = port;
		this.logger = logger;
		listener = new TcpListener(address, port);
		Task.Run((Func<Task?>)Start_);
	}

	private async Task Start_()
	{
		while (!IsDisposed)
		{
			try
			{
				try
				{
					_token?.Cancel();
					_token = new CancellationTokenSource();
					CancellationToken token = _token.Token;
					if (listener != null)
					{
						listener.Start();
						new Thread((ThreadStart)delegate
						{
							StartListening_(token);
						}).Start();
					}
				}
				catch (Exception ex)
				{
					ILogger obj = logger;
					if (obj != null)
					{
						obj.Error(ex, $"Could not start tcp listener on port {Port}");
					}
					WebErrorLog.Instance.Warning(ex, $"Could not start tcp listener on port {Port}", "Start_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 54);
					goto end_IL_0027;
				}
				break;
				end_IL_0027:;
			}
			finally
			{
				await Task.Delay(60000);
			}
		}
	}

	private async Task StartListening_(CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				if (IsDisposed || token.IsCancellationRequested)
				{
					break;
				}
				if (listener == null)
				{
					throw new NullReferenceException("listener is null!!!");
				}
				TcpClient tcpClient = await listener.AcceptTcpClientAsync();
				if (IsDisposed || token.IsCancellationRequested)
				{
					break;
				}
				if (tcpClient != null)
				{
					AcceptConnectionAsync(tcpClient);
				}
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("An existing connection was forcibly closed by the remote host."))
				{
					WebErrorLog.Instance.StartLog(ex, "", "StartListening_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 74);
					ILogger obj = logger;
					if (obj != null)
					{
						obj.Warning(ex, "Server connection stopped");
					}
					await Task.Delay(60000);
				}
			}
		}
	}

	protected abstract Task AcceptConnectionAsync(TcpClient con);

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_token?.Cancel();
				_token?.Dispose();
				_token = null;
				listener?.Stop();
				listener = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 96);
		}
	}

	public async Task AcceptConnectionAsync_(T con)
	{
		await _handleConnectionSemaphore.WaitAsync();
		try
		{
			con.Disconnected += Connection_Disconnected;
			con.Disposed += Connection_Disposed;
			List<T> clients = _clients;
			if (clients != null && clients.Count > 1000)
			{
				WebErrorLog.Instance.StartLog("clinet count is more than 300", "", "AcceptConnectionAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 108);
				foreach (T client in _clients)
				{
					client?.Dispose();
				}
				_clients = new List<T>();
			}
			_clients.Add(con);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AcceptConnectionAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 119);
			throw;
		}
		finally
		{
			_handleConnectionSemaphore?.Release();
		}
	}

	private Task Connection_Disposed(object conn, System.EventArgs e)
	{
		try
		{
			if (conn != null)
			{
				((T)conn).Disconnected -= Connection_Disconnected;
				((T)conn).Disposed -= Connection_Disposed;
			}
			_clients.Remove((T)conn);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Connection_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 136);
		}
		return Task.CompletedTask;
	}

	private Task Connection_Disconnected(object conn, DisconnectEventArgs e)
	{
		try
		{
			if (conn != null)
			{
				((T)conn).Dispose();
				((T)conn).Disconnected -= Connection_Disconnected;
				((T)conn).Disposed -= Connection_Disposed;
			}
			_clients.Remove((T)conn);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Connection_Disconnected", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\Net\\Server\\TcpServerBaseConnection.cs", 152);
		}
		return Task.CompletedTask;
	}
}
