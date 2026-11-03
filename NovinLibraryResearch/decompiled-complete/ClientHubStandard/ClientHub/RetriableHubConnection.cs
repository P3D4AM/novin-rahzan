using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using Serilog;

namespace ClientHub;

public abstract class RetriableHubConnection : IAsyncDisposable
{
	public SemaphoreSlim _connectionSemaphore = new SemaphoreSlim(20, 20);

	public CancellationTokenSource _connectionToken;

	public CancellationTokenSource _generateConnectionToken;

	private readonly ILogger logger;

	private bool _isDisposed;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _connectionTask = new TaskCompletionSource<ReturnedSaveFuncInfo>();

	public string EndPointUrl { get; private set; }

	public HubConnection Connection { get; private set; }

	public bool IsDisposed => _isDisposed;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event Func<object, ConnectionEventArgs<HubConnectionState>, Task> Changed;

	public RetriableHubConnection(string endPointUrl, ILogger logger)
	{
		RetriableHubConnection retriableHubConnection = this;
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs"))
		{
			try
			{
				if (logger != null)
				{
					logger.Information("RetriableHubConnection.ctor()");
				}
				EndPointUrl = endPointUrl;
				this.logger = logger;
				_generateConnectionToken?.Cancel();
				_generateConnectionToken = new CancellationTokenSource();
				CancellationToken generateConnectionToken = _generateConnectionToken.Token;
				Task.Run(() => retriableHubConnection.GenerateConnectionAsync(generateConnectionToken));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 46);
			}
		}
	}

	public abstract Task InitializeConnectionAsync();

	private async Task GenerateConnectionAsync(CancellationToken internalToken)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			ILogger obj = logger;
			if (obj != null)
			{
				obj.Information("GenerateConnection start....");
			}
			if (IsDisposed || internalToken.IsCancellationRequested)
			{
				return;
			}
			_connectionToken?.Cancel();
			_connectionToken = new CancellationTokenSource();
			CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(_connectionToken.Token, internalToken).Token;
			await _connectionSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			Utilities.RaiseEvent(this.Changed, this, new ConnectionEventArgs<HubConnectionState>
			{
				ConnectionData = new List<HubConnectionState> { (HubConnectionState)0 }
			}, default(CancellationToken), 60, "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
			if (IsDisposed || token.IsCancellationRequested)
			{
				return;
			}
			if (Connection != null)
			{
				Connection.Closed -= Connection_Closed;
				Task task = Task.Delay(30000);
				Task task2 = Task.Run(async delegate
				{
					if (Connection != null)
					{
						await Connection.DisposeAsync();
					}
				});
				await Task.WhenAny(new Task[2] { task, task2 });
				Connection = null;
			}
			Connection = HubConnectionBuilderExtensions.WithAutomaticReconnect(NewtonsoftJsonProtocolDependencyInjectionExtensions.AddNewtonsoftJsonProtocol<IHubConnectionBuilder>(HubConnectionBuilderHttpExtensions.WithUrl((IHubConnectionBuilder)new HubConnectionBuilder(), EndPointUrl)), new TimeSpan[1] { TimeSpan.FromSeconds(30.0) }).Build();
			Connection.KeepAliveInterval = TimeSpan.FromSeconds(60.0);
			Connection.ServerTimeout = TimeSpan.FromSeconds(60.0);
			await InitializeConnectionAsync();
			Connection.Closed += Connection_Closed;
			Utilities.RaiseEvent(this.Changed, this, new ConnectionEventArgs<HubConnectionState>
			{
				ConnectionData = new List<HubConnectionState> { (HubConnectionState)2 }
			}, default(CancellationToken), 60, "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
			await Connection.StartAsync(token);
			_connectionTask.TrySetResult(new ReturnedSaveFuncInfo());
			Utilities.RaiseEvent(this.Changed, this, new ConnectionEventArgs<HubConnectionState>
			{
				ConnectionData = new List<HubConnectionState> { (HubConnectionState)1 }
			}, default(CancellationToken), 60, "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
			Task.Run(() => PingPongAsync(token));
		}
		catch (OperationCanceledException)
		{
			_connectionTask.TrySetResult(new ReturnedSaveFuncInfo().AddErrorTokenIsCancelled("", "", "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 90));
		}
		catch (WebException ex2)
		{
			Utilities.RaiseEvent(this.Changed, this, new ConnectionEventArgs<HubConnectionState>
			{
				ConnectionData = new List<HubConnectionState> { (HubConnectionState)0 }
			}, default(CancellationToken), 60, "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
			Log.Warning((Exception)ex2, "EndPointUrl: " + EndPointUrl);
			await Task.Delay(300000);
			GenerateConnectionAsync(internalToken);
			_connectionTask.TrySetResult(new ReturnedSaveFuncInfo(ex2, "", "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 97));
		}
		catch (Exception ex)
		{
			Utilities.RaiseEvent(this.Changed, this, new ConnectionEventArgs<HubConnectionState>
			{
				ConnectionData = new List<HubConnectionState> { (HubConnectionState)0 }
			}, default(CancellationToken), 60, "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
			WebErrorLog.Instance.StartLog(ex, "EndPointUrl: " + EndPointUrl, "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 102);
			await Task.Delay(300000);
			GenerateConnectionAsync(internalToken);
			_connectionTask.TrySetResult(new ReturnedSaveFuncInfo(ex, "", "GenerateConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 105));
		}
		finally
		{
			if (!IsDisposed && semaphoreHasBeenTaken)
			{
				_connectionSemaphore?.Release();
			}
		}
	}

	private async Task Connection_Closed(Exception arg)
	{
		try
		{
			if (!IsDisposed)
			{
				if (Connection != null)
				{
					Connection.Closed -= Connection_Closed;
					await Connection.DisposeAsync();
				}
				Utilities.RaiseEvent(this.Changed, this, new ConnectionEventArgs<HubConnectionState>
				{
					ConnectionData = new List<HubConnectionState> { (HubConnectionState)0 }
				}, default(CancellationToken), 60, "Connection_Closed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
				ILogger obj = logger;
				if (obj != null)
				{
					obj.Warning<string>("Scale connection closed from {address} scale hub", EndPointUrl);
				}
				await Task.Delay(checked(new Random().Next(0, 5) * 1000));
				ILogger obj2 = logger;
				if (obj2 != null)
				{
					obj2.Information<string>("Reconnecting to {address} scale hub", EndPointUrl);
				}
				_generateConnectionToken?.Cancel();
				_generateConnectionToken = new CancellationTokenSource();
				CancellationToken token = _generateConnectionToken.Token;
				Task.Run(() => GenerateConnectionAsync(token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "EndPointUrl: " + EndPointUrl, "Connection_Closed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 129);
		}
	}

	private async Task PingPongAsync(CancellationToken token)
	{
		ILogger obj = logger;
		if (obj != null)
		{
			obj.Information("PingPongAsync start");
		}
		await Task.Delay(30000);
		while (!token.IsCancellationRequested && !IsDisposed)
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					if ((int)Connection.State != 1)
					{
						await Task.Delay(5000);
					}
					if (IsDisposed || token.IsCancellationRequested)
					{
						goto end_IL_00cc;
					}
					if ((int)Connection.State == 0)
					{
						_generateConnectionToken?.Cancel();
						_generateConnectionToken = new CancellationTokenSource();
						CancellationToken generateConnectionToken5 = _generateConnectionToken.Token;
						Task.Run(() => GenerateConnectionAsync(generateConnectionToken5));
						goto end_IL_00cc;
					}
					if ((int)Connection.State != 1)
					{
						await Task.Delay(10000);
					}
					if ((int)Connection.State != 1)
					{
						_generateConnectionToken?.Cancel();
						_generateConnectionToken = new CancellationTokenSource();
						CancellationToken generateConnectionToken4 = _generateConnectionToken.Token;
						Task.Run(() => GenerateConnectionAsync(generateConnectionToken4));
						goto end_IL_00cc;
					}
					if (IsDisposed || token.IsCancellationRequested)
					{
						goto end_IL_00cc;
					}
					if (await HubConnectionExtensions.InvokeAsync<bool>(Connection, "PingPong", (object)"ping", default(CancellationToken)))
					{
						continue;
					}
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						_generateConnectionToken?.Cancel();
						_generateConnectionToken = new CancellationTokenSource();
						CancellationToken generateConnectionToken3 = _generateConnectionToken.Token;
						Task.Run(() => GenerateConnectionAsync(generateConnectionToken3));
					}
					end_IL_00cc:;
				}
				catch (OperationCanceledException)
				{
					goto end_IL_00b1;
				}
				catch (SocketException)
				{
					if (!IsDisposed && !token.IsCancellationRequested)
					{
						_generateConnectionToken?.Cancel();
						_generateConnectionToken = new CancellationTokenSource();
						CancellationToken generateConnectionToken2 = _generateConnectionToken.Token;
						Task.Run(() => GenerateConnectionAsync(generateConnectionToken2));
					}
				}
				catch (Exception ex3)
				{
					if (!token.IsCancellationRequested && !IsDisposed)
					{
						if (!ex3.Message.ToLower().Contains("method cannot be called if the connection is not active".ToLower()) && !ex3.Message.ToLower().Contains("The server closed the connection with the following error: Connection closed with an error.".ToLower()) && !ex3.Message.ToLower().Contains("Server timeout".ToLower()))
						{
							WebErrorLog.Instance.StartLog(ex3, "EndPointUrl: " + EndPointUrl, "PingPongAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 188);
						}
						_generateConnectionToken?.Cancel();
						_generateConnectionToken = new CancellationTokenSource();
						CancellationToken generateConnectionToken = _generateConnectionToken.Token;
						Task.Run(() => GenerateConnectionAsync(generateConnectionToken));
					}
				}
				break;
				end_IL_00b1:;
			}
			finally
			{
				await Task.Delay(5000);
			}
		}
		ILogger obj2 = logger;
		if (obj2 != null)
		{
			obj2.Information("PingPongAsync Finished!");
		}
	}

	public virtual async ValueTask DisposeAsync()
	{
		_ = 2;
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_connectionToken?.Cancel();
				_connectionToken?.Dispose();
				_connectionToken = null;
				_connectionSemaphore?.Dispose();
				_connectionSemaphore = null;
				await Connection_Closed(null);
				if (Connection != null)
				{
					Connection.Closed -= Connection_Closed;
					await Connection.StopAsync(default(CancellationToken));
					await Connection.DisposeAsync();
				}
				Connection = null;
				if (this.OnDisposed != null)
				{
					Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 228);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckConnectionAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Task delay = Task.Delay(10000);
			Task<ReturnedSaveFuncInfo> task = _connectionTask.Task;
			if (await Task.WhenAny(new Task[2] { task, delay }) == delay)
			{
				return res.AddErrorTokenIsCancelled("", "", "CheckConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 238);
			}
			if ((await task).HasError)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				return await returnedSaveFuncInfo.AddReturnedValueAsync(await task, ReturnedState.None, "CheckConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 239);
			}
			if ((int)Connection.State == 0)
			{
				res.AddError("Connection is Disconnected !!", "", "CheckConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 241);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 245);
			res.AddReturnedValue(ex, "", "CheckConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\RetriableHubConnection.cs", 246);
		}
		return res;
	}
}
