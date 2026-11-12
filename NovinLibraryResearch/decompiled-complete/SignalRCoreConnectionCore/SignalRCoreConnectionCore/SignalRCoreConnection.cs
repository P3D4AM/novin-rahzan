using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Mapping;
using SignalRCoreConnectionCore.Mappers;

namespace SignalRCoreConnectionCore;

public class SignalRCoreConnection : IConnection, IDisposable
{
	private bool _isStoped;

	private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

	private HubConnection _connection;

	private ENSource _source;

	private string _url;

	public bool IsSecure { get; private set; }

	public CancellationTokenSource token
	{
		get
		{
			return _cancellationTokenSource;
		}
		private set
		{
			_cancellationTokenSource = value;
		}
	}

	public string ConnectionId { get; set; } = Guid.NewGuid().ToString();


	public string ServerUniqueId { get; set; } = Guid.NewGuid().ToString();


	public string ContextGuid { get; private set; }

	public EnConnectionType Type => EnConnectionType.SignalRCore;

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public ENSource Source => _source;

	public event Func<object, SingleConnectionSystemMessageEventArgs, Task> RecieveSystemMessages;

	public event Func<object, SingleConnectionDataEventArgs, Task> Received;

	public event Func<object, string, Task> StopConnection;

	public event Func<object, EventArgs, Task> Reset;

	public event Action Closed;

	public event Func<object, StateChange, Task> StateChanged;

	public async Task<EnConnectionState> GetStateAsync()
	{
		try
		{
			if (IsDisposed)
			{
				return EnConnectionState.Disposed;
			}
			MapperItemContainer<EnConnectionState> mapperItemContainer = await ConnectionStateMapper.Instance.MapAsync(ContextGuid, new MapperItemContainer<HubConnectionState>(_connection?.State ?? HubConnectionState.Disconnected));
			if (mapperItemContainer.Value == EnConnectionState.Connecting)
			{
				return EnConnectionState.Disconnected;
			}
			return mapperItemContainer.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 51);
			if (IsDisposed)
			{
				return EnConnectionState.Disposed;
			}
			return EnConnectionState.Disconnected;
		}
	}

	public SignalRCoreConnection(string contextGuid, string url, ENCompressedMode compressedMode, long miliSecondsDelayBetweenSend, ENSource source)
	{
		SignalRCoreConnection signalRCoreConnection = this;
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs"))
		{
			int line = 0;
			try
			{
				_source = source;
				_url = url;
				ContextGuid = contextGuid;
				line = 1;
				line = 2;
				_connection = new HubConnectionBuilder().WithUrl(url).WithAutomaticReconnect().Build();
				line = 3;
				_connection.Closed += SignalRClassicConnection_Closed;
				line = 4;
				if (token == null)
				{
					_cancellationTokenSource = new CancellationTokenSource();
				}
				line = 5;
				_connection.On("RecieveBytesAsync", delegate(byte[] data)
				{
					try
					{
						line = 6;
						SingleConnectionDataEventArgs args2 = new SingleConnectionDataEventArgs
						{
							Data = data,
							Url = url,
							Source = source,
							ConnectionId = signalRCoreConnection.ConnectionId,
							cancellationToken = (signalRCoreConnection.token?.Token ?? new CancellationToken(canceled: true))
						};
						line = 7;
						Utilities.RaiseEventOnNewThread(signalRCoreConnection.Received, signalRCoreConnection, args2);
						line = 8;
					}
					catch (Exception ex3)
					{
						WebErrorLog.Instance.StartLog(ex3, $"RecieveBytesAsync//Line:{line}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 115);
					}
				});
				line = 9;
				_connection.On("RecieveSystemMessagesAsync", delegate(EnServerMessageInstruction instruction, string data)
				{
					try
					{
						line = 10;
						if (signalRCoreConnection.token == null)
						{
							CancellationTokenSource cancellationTokenSource2 = (signalRCoreConnection.token = new CancellationTokenSource());
						}
						line = 11;
						SingleConnectionSystemMessageEventArgs args = new SingleConnectionSystemMessageEventArgs
						{
							Instruction = instruction,
							Data = data,
							Url = url,
							ConnectionId = signalRCoreConnection.ConnectionId,
							cancellationToken = signalRCoreConnection.token.Token
						};
						line = 12;
						Utilities.RaiseEventOnNewThread(signalRCoreConnection.RecieveSystemMessages, signalRCoreConnection, args);
						line = 13;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, $"RecieveSystemMessagesAsync//Line:{line}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 138);
					}
				});
				line = 14;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"signalRCtor/Line:{line}/ContextGuid:{contextGuid}/Url:{url}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 144);
			}
		}
	}

	private Task SignalRClassicConnection_Closed(Exception exception)
	{
		return Utilities.RaiseEvent(this.StateChanged, this, new StateChange
		{
			NewState = EnConnectionState.Disconnected,
			OldState = EnConnectionState.Connected
		}, default(CancellationToken), 60, "SignalRClassicConnection_Closed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs");
	}

	[DebuggerNonUserCode]
	public async Task<ReturnedSaveFuncInfo> Send(byte[] data, short tryCount = 0)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs"))
			{
				try
				{
					if (_cancellationTokenSource?.IsCancellationRequested ?? true)
					{
						ret.AddReturnedValue(ReturnedState.Error, "Token has been canceled!", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 173);
						return ret;
					}
					if (await GetStateAsync() != EnConnectionState.Connected || data == null)
					{
						ret.AddReturnedValue(ReturnedState.Error, "State != PacketParser.EventArgs.EnConnectionState.Connected || data == null", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 179);
						return ret;
					}
					if (_connection.State != HubConnectionState.Connected)
					{
						ret.AddReturnedValue(ReturnedState.Error, $"connecion is not connected state:{_connection.State}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 184);
						return ret;
					}
					if (_connection.State == HubConnectionState.Connected)
					{
						if (data.Length > 51200)
						{
							return await SendChunckedPacket(data, _cancellationTokenSource.Token);
						}
						try
						{
							await _connection.SendAsync("RecieveBytesAsync", data, token.Token);
						}
						catch (Exception exception)
						{
							ret.AddReturnedValue(exception, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 193);
						}
					}
					else
					{
						ret.AddReturnedValue(ReturnedState.Error, $"connection state is not connected! state:{_connection.State}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 195);
					}
				}
				catch (OperationCanceledException exception2)
				{
					return ret.AddReturnedValue(exception2, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 197);
				}
				catch (Exception ex)
				{
					if (ex.Message.ToLower().Contains("The 'SendCoreAsync' method cannot be called if the connection is not active".ToLower()))
					{
						return new ReturnedSaveFuncInfo(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 201);
					}
					if (_cancellationTokenSource?.IsCancellationRequested ?? true)
					{
						ret.AddReturnedValue(ReturnedState.Error, "Token has been canceled!", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 204);
						return ret;
					}
					await Task.Delay(100 * tryCount);
					if (tryCount < 10)
					{
						SignalRCoreConnection signalRCoreConnection = this;
						short num = (short)(tryCount + 1);
						tryCount = num;
						return await signalRCoreConnection.Send(data, num);
					}
					WebErrorLog.Instance.StartLog(ex, "send Bytes", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 211);
					ret.AddReturnedValue(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 212);
				}
				return ret;
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendChunckedPacket(byte[] data, CancellationToken cancellationToken)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				List<List<byte>> chunked = data.ChunkList(50m);
				if (chunked == null)
				{
					return ret;
				}
				short part = 0;
				Guid guid = Guid.NewGuid();
				foreach (List<byte> item in chunked)
				{
					if (_connection == null || _connection.State != HubConnectionState.Connected || cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connetionErro", "", "SendChunckedPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 230);
						return ret;
					}
					await _connection.SendAsync("RecieveChunckedBytesAsync", item?.ToArray(), part, (short)chunked.Count, guid);
					part++;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendChunckedPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 239);
				ret.AddReturnedValue(ex, "", "SendChunckedPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 240);
			}
			return ret;
		}
	}

	public void ResetToken()
	{
		_cancellationTokenSource?.Cancel();
		_cancellationTokenSource = new CancellationTokenSource();
	}

	public async Task Start(CancellationToken cancellationToken)
	{
		using (new JaegerMonitor("Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs"))
		{
			_ = 6;
			try
			{
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					Stop("OperationCanceledException on start");
					return;
				}
				await Utilities.RaiseEvent(this.StateChanged, this, new StateChange
				{
					OldState = EnConnectionState.Disconnected,
					NewState = EnConnectionState.Connecting
				}, default(CancellationToken), 60, "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs");
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					Stop("OperationCanceledException on start");
					return;
				}
				IsSecure = false;
				Task.Run(() => Utilities.RaiseEventOnNewThread(this.Reset, this, new EventArgs()), cancellationToken);
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					Stop("OperationCanceledException on start");
					return;
				}
				if (_connection == null)
				{
					if (cancellationToken.IsCancellationRequested || IsDisposed)
					{
						Stop("OperationCanceledException on start");
					}
					else
					{
						Stop("connection is null");
					}
					return;
				}
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					Stop("OperationCanceledException on start");
					return;
				}
				try
				{
					if (cancellationToken.IsCancellationRequested || IsDisposed)
					{
						Stop("OperationCanceledException on start");
						return;
					}
					if (_connection == null)
					{
						Dispose();
						return;
					}
					await _connection.StartAsync(cancellationToken);
				}
				catch
				{
					if (_connection == null || string.IsNullOrEmpty(_connection.ConnectionId) || _connection.State == HubConnectionState.Disconnected)
					{
						Dispose();
						return;
					}
					await Task.Delay(1000, cancellationToken);
					if (cancellationToken.IsCancellationRequested || IsDisposed)
					{
						Stop("OperationCanceledException on start");
						return;
					}
					try
					{
						if (cancellationToken.IsCancellationRequested || IsDisposed)
						{
							Stop("OperationCanceledException on start");
							return;
						}
						await _connection.StartAsync(cancellationToken);
					}
					catch
					{
						await Task.Delay(5000, cancellationToken);
						if (cancellationToken.IsCancellationRequested || IsDisposed)
						{
							Stop("OperationCanceledException on start");
							return;
						}
						await _connection.StartAsync(cancellationToken);
					}
				}
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					Stop("OperationCanceledException on start");
					return;
				}
				await Utilities.RaiseEvent(this.StateChanged, this, new StateChange
				{
					NewState = EnConnectionState.Connected,
					OldState = EnConnectionState.Connecting
				}, default(CancellationToken), 60, "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs");
			}
			catch (InvalidDataException)
			{
				Stop("System.IO.InvalidDataException");
			}
			catch (AggregateException)
			{
				Stop("System.AggregateException");
			}
			catch (WebSocketException)
			{
				Stop("System.Net.WebSockets.WebSocketException");
			}
			catch (HubException)
			{
				Stop("Microsoft.AspNetCore.SignalR.HubException");
			}
			catch (TaskCanceledException)
			{
				Stop("taskCanceled start");
			}
			catch (OperationCanceledException)
			{
				_cancellationTokenSource?.Cancel();
				Stop("OperationCanceledException on start");
			}
			catch (HttpRequestException)
			{
				Stop("System.Net.Http.HttpRequestException on start");
			}
			catch (TimeoutException)
			{
				Stop("TimeoutException on start");
			}
			catch (Exception ex9)
			{
				Stop("exception on start");
				WebErrorLog.Instance.StartLog(ex9, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 323);
			}
		}
	}

	public void Stop(string reason)
	{
		Task.Run(() => Stop_(reason));
	}

	private Task Stop_(string reason)
	{
		using (new JaegerMonitor("Stop_" + reason, "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs"))
		{
			int num = 1;
			try
			{
				if (IsDisposed)
				{
					return Task.CompletedTask;
				}
				if (_isStoped)
				{
					return Task.CompletedTask;
				}
				_isStoped = true;
				num = 2;
				if (this.StopConnection != null)
				{
					Task.Run(() => Utilities.RaiseEventOnNewThread(this.StopConnection, this, reason));
				}
				num = 4;
				if (IsDisposed)
				{
					return Task.CompletedTask;
				}
				_cancellationTokenSource?.Cancel();
				num = 6;
				IsSecure = false;
				num = 7;
				Task.Run(() => Utilities.RaiseEventOnNewThread(this.Reset, this, new EventArgs()));
				num = 9;
				Task.Run(async delegate
				{
					_ = 1;
					try
					{
						if (_connection != null)
						{
							try
							{
								await _connection.StopAsync();
							}
							catch
							{
							}
						}
						if (_connection != null)
						{
							try
							{
								await _connection.DisposeAsync();
								return;
							}
							catch
							{
								return;
							}
						}
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "Stop_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 368);
					}
				});
				num = 10;
				return Utilities.RaiseEvent(this.StateChanged, this, new StateChange
				{
					NewState = EnConnectionState.Disconnected,
					OldState = EnConnectionState.Connected
				}, default(CancellationToken), 60, "Stop_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Removed new //public void Stop(string Reason)// Line:{num}", "Stop_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 379);
			}
			return Task.CompletedTask;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected void Dispose(bool disposing)
	{
		int num = 0;
		try
		{
			using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs"))
			{
				if (!IsDisposed)
				{
					IsDisposed = true;
					if (_connection != null)
					{
						_connection.Closed -= SignalRClassicConnection_Closed;
						_connection.DisposeAsync();
					}
					_connection = null;
					num = 2;
					_cancellationTokenSource?.Cancel();
					_cancellationTokenSource = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 409);
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send(EnServerMessageInstruction instruction, string data, short tryCount = 0)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			line = 1;
			if (_cancellationTokenSource?.IsCancellationRequested ?? true)
			{
				line = 2;
				ret.AddReturnedValue(ReturnedState.Error, "Token has been canceled!", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 422);
				line = 3;
				return ret;
			}
			line = 4;
			if (await GetStateAsync() != EnConnectionState.Connected || data == null)
			{
				line = 5;
				ret.AddReturnedValue(ReturnedState.Error, "State != PacketParser.EventArgs.EnConnectionState.Connected || data == null", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 430);
				line = 6;
				return ret;
			}
			line = 7;
			if (_connection.State != HubConnectionState.Connected)
			{
				line = 8;
				ret.AddReturnedValue(ReturnedState.Error, $"connecion is not connected state:{_connection.State}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 438);
				line = 9;
				return ret;
			}
			line = 10;
			if (_connection.State == HubConnectionState.Connected)
			{
				line = 11;
				await _connection.SendAsync("RecieveSystemMessagesAsync", instruction, data, token.Token);
			}
			else
			{
				line = 12;
				ret.AddReturnedValue(ReturnedState.Error, $"conection state is :{_connection.State}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 451);
			}
			line = 3;
		}
		catch (OperationCanceledException exception)
		{
			return ret.AddReturnedValue(exception, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 455);
		}
		catch (InvalidOperationException ex)
		{
			if (!ex.Message.ToLower().Contains("The 'SendCoreAsync' method cannot be called if the connection is not active".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex, $"send systemMessages//Line:{line}", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 459);
			}
		}
		catch (Exception ex2)
		{
			ret.AddReturnedValue(ex2, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 463);
			if (!ex2.Message.ToLower().Contains("Cannot access a disposed object".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex2, $"send systemMessages//Line:{line}", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SignalRCoreConnectionCore\\SignalRCoreConnection.cs", 465);
			}
		}
		return ret;
	}
}
