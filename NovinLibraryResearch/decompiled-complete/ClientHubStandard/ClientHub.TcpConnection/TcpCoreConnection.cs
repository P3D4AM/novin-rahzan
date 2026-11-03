using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClientHub.Mapper;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Scale;
using PacketParser.Scale.EventArguments;

namespace ClientHub.TcpConnection;

public class TcpCoreConnection : PacketParser.Connection.LowLevel.IConnection, IDisposable
{
	private bool _isStoped;

	private ServiceTcpClientConnection _connection;

	private ENSource _source;

	public bool IsSecure { get; private set; }

	public CancellationTokenSource token { get; private set; } = new CancellationTokenSource();


	public string ConnectionId { get; set; } = Guid.NewGuid().ToString();


	public string ServerUniqueId { get; set; } = Guid.NewGuid().ToString();


	public string ContextGuid { get; private set; }

	public string Url { get; private set; }

	public EnConnectionType Type => EnConnectionType.TcpConnection;

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

	public TcpCoreConnection(string contextGuid, string url, ENCompressedMode compressedMode, long miliSecondsDelayBetweenSend, ENSource source)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs"))
		{
			try
			{
				ContextGuid = contextGuid;
				Url = url;
				_source = source;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 53);
			}
		}
	}

	private Task _connection_Disposed(object arg1, EventArgs arg2)
	{
		try
		{
			Dispose(disposing: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_connection_Disposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 62);
		}
		return Task.CompletedTask;
	}

	private async Task _connection_Received(object data, ReadEventArgs e)
	{
		_ = 1;
		try
		{
			BytePacketManager bytePacketManager = BytePacketManager.ParsePacket(e.Bytes);
			SingleConnectionDataEventArgs args = new SingleConnectionDataEventArgs
			{
				Data = bytePacketManager?.Data
			};
			if (bytePacketManager != null)
			{
				EnServerMessageInstruction messageType = bytePacketManager.MessageType;
				if ((uint)(messageType - 1) <= 6u)
				{
					string @string = Encoding.ASCII.GetString(bytePacketManager.Data);
					SingleConnectionSystemMessageEventArgs args2 = new SingleConnectionSystemMessageEventArgs
					{
						Instruction = bytePacketManager.MessageType,
						Data = @string,
						Url = Url,
						ConnectionId = ConnectionId,
						cancellationToken = token.Token
					};
					await Utilities.RaiseEventOnNewThread(this.RecieveSystemMessages, this, args2);
					return;
				}
			}
			await Utilities.RaiseEventOnNewThread(this.Received, this, args);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_connection_Received", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 95);
		}
	}

	public async Task<EnConnectionState> GetStateAsync()
	{
		try
		{
			if (IsDisposed)
			{
				return EnConnectionState.Disposed;
			}
			MapperItemContainer<EnConnectionState> mapperItemContainer = await TcpConnectionStatusMapper.Instance.MapAsync(ContextGuid, new MapperItemContainer<ConnectionStatus>(_connection?.Status ?? ConnectionStatus.Disconnected));
			if (mapperItemContainer.Value == EnConnectionState.Connecting)
			{
				return EnConnectionState.Disconnected;
			}
			return mapperItemContainer.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 109);
			if (IsDisposed)
			{
				return EnConnectionState.Disposed;
			}
			return EnConnectionState.Disconnected;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send(byte[] data, short tryCount = 0)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			using (new JaegerMonitor("Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs"))
			{
				try
				{
					if (token?.IsCancellationRequested ?? true)
					{
						ret.AddReturnedValue(ReturnedState.Error, "Token has been canceled!", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 122);
						return ret;
					}
					if (await GetStateAsync() != EnConnectionState.Connected || data == null)
					{
						ret.AddReturnedValue(ReturnedState.Error, "State != PacketParser.EventArgs.EnConnectionState.Connected || data == null", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 128);
						return ret;
					}
					if (_connection.Status != ConnectionStatus.Connected)
					{
						ret.AddReturnedValue(ReturnedState.Error, $"connecion is not connected state:{_connection.Status}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 133);
						return ret;
					}
					if (_connection.Status == ConnectionStatus.Connected)
					{
						return await SendPacket(data, token.Token);
					}
					ret.AddReturnedValue(ReturnedState.Error, $"connection state is not connected! state:{_connection.Status}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 139);
				}
				catch (OperationCanceledException exception)
				{
					return ret.AddReturnedValue(exception, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 141);
				}
				catch (Exception ex)
				{
					if (ex.Message.ToLower().Contains("The 'SendCoreAsync' method cannot be called if the connection is not active".ToLower()))
					{
						return new ReturnedSaveFuncInfo(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 145);
					}
					if (token?.IsCancellationRequested ?? true)
					{
						ret.AddReturnedValue(ReturnedState.Error, "Token has been canceled!", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 148);
						return ret;
					}
					await Task.Delay(100 * tryCount);
					if (tryCount < 10)
					{
						TcpCoreConnection tcpCoreConnection = this;
						short num = (short)(tryCount + 1);
						tryCount = num;
						return await tcpCoreConnection.Send(data, num);
					}
					WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 153);
					ret.AddReturnedValue(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 154);
				}
				return ret;
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendPacket(byte[] data, CancellationToken cancellationToken)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				List<List<byte>> chunked = data.ChunkListBySpecialLenght(_connection.BufferSize - BytePacketManager.PacketHeaderSize);
				if (chunked == null)
				{
					return ret;
				}
				short part = 1;
				Guid guid = Guid.NewGuid();
				foreach (List<byte> item in chunked)
				{
					if (_connection == null || _connection.Status != ConnectionStatus.Connected || cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connetionError", "", "SendPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 172);
						return ret;
					}
					byte[] data2 = new BytePacketManager
					{
						Data = item.ToArray(),
						MessageType = EnServerMessageInstruction.ByteData,
						BufferSize = _connection.BufferSize,
						CurrentPart = part,
						TotalPart = chunked.Count,
						RefrenceGuid = guid
					}.GeneratePacket();
					await _connection.SendAsync(data2, cancellationToken);
					part++;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 192);
				ret.AddReturnedValue(ex, "", "SendPacket", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 193);
			}
			return ret;
		}
	}

	public void ResetToken()
	{
		token?.Cancel();
		token = new CancellationTokenSource();
	}

	public async Task Start(CancellationToken cancellationToken)
	{
		using (new JaegerMonitor("Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs"))
		{
			_ = 3;
			try
			{
				if (_connection == null)
				{
					Uri uri = new Uri(Url);
					TcpClient connection = new TcpClient(uri.Host, uri.Port);
					_connection = new ServiceTcpClientConnection(uri.Port, connection);
					_connection.Received += _connection_Received;
					_connection.Disposed += _connection_Disposed;
				}
				if (cancellationToken.IsCancellationRequested || IsDisposed)
				{
					Stop("OperationCanceledException on start");
					return;
				}
				await Utilities.RaiseEvent(this.StateChanged, this, new StateChange
				{
					OldState = EnConnectionState.Disconnected,
					NewState = EnConnectionState.Connecting
				}, default(CancellationToken), 60, "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs");
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
				}
				catch
				{
					if (_connection == null || _connection.Status == ConnectionStatus.Disconnected)
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
					}
					catch
					{
						await Task.Delay(5000, cancellationToken);
						if (cancellationToken.IsCancellationRequested || IsDisposed)
						{
							Stop("OperationCanceledException on start");
							return;
						}
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
				}, default(CancellationToken), 60, "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs");
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
				token?.Cancel();
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
				WebErrorLog.Instance.StartLog(ex9, "", "Start", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 283);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send(EnServerMessageInstruction instruction, string data, short tryCount = 0)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (token?.IsCancellationRequested ?? true)
			{
				ret.AddReturnedValue(ReturnedState.Error, "Token has been canceled!", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 293);
				return ret;
			}
			if (await GetStateAsync() != EnConnectionState.Connected || data == null)
			{
				ret.AddReturnedValue(ReturnedState.Error, "State != PacketParser.EventArgs.EnConnectionState.Connected || data == null", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 298);
				return ret;
			}
			if (_connection.Status != ConnectionStatus.Connected)
			{
				ret.AddReturnedValue(ReturnedState.Error, $"connecion is not connected state:{_connection.Status}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 303);
				return ret;
			}
			if (_connection.Status == ConnectionStatus.Connected)
			{
				byte[] bytes = Encoding.ASCII.GetBytes(data);
				byte[] data2 = new BytePacketManager
				{
					Data = bytes,
					MessageType = instruction,
					BufferSize = _connection.BufferSize
				}.GeneratePacket();
				await _connection.SendAsync(data2, token.Token);
			}
			else
			{
				ret.AddReturnedValue(ReturnedState.Error, $"conection state is :{_connection.Status}", "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 319);
			}
		}
		catch (OperationCanceledException exception)
		{
			return ret.AddReturnedValue(exception, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 321);
		}
		catch (InvalidOperationException ex)
		{
			if (!ex.Message.ToLower().Contains("The 'SendCoreAsync' method cannot be called if the connection is not active".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 325);
			}
		}
		catch (Exception ex2)
		{
			ret.AddReturnedValue(ex2, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 329);
			if (!ex2.Message.ToLower().Contains("Cannot access a disposed object".ToLower()))
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Send", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 331);
			}
		}
		return ret;
	}

	public void Stop(string reason)
	{
		Task.Run(() => Stop_(reason));
	}

	private Task Stop_(string reason)
	{
		using (new JaegerMonitor("Stop_" + reason, "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs"))
		{
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
				if (this.StopConnection != null)
				{
					Task.Run(() => Utilities.RaiseEventOnNewThread(this.StopConnection, this, reason));
				}
				if (IsDisposed)
				{
					return Task.CompletedTask;
				}
				token?.Cancel();
				IsSecure = false;
				Task.Run(() => Utilities.RaiseEventOnNewThread(this.Reset, this, new EventArgs()));
				Task.Run(async delegate
				{
				});
				return Utilities.RaiseEvent(this.StateChanged, this, new StateChange
				{
					NewState = EnConnectionState.Disconnected,
					OldState = EnConnectionState.Connected
				}, default(CancellationToken), 60, "Stop_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Stop_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 374);
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
		try
		{
			using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs"))
			{
				if (!IsDisposed)
				{
					IsDisposed = true;
					if (_connection != null)
					{
						_connection.Received -= _connection_Received;
						_connection.Disposed -= _connection_Disposed;
					}
					_connection = null;
					token?.Cancel();
					token = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\TcpConnection\\TcpCoreConnection.cs", 396);
		}
	}
}
