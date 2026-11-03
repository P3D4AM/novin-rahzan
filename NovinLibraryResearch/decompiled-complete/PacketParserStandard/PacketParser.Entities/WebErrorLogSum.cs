using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebErrorLogSum : Serializable<IErrorLogSum>, IErrorLogSum, IReceiver, INpDisposable, IDisposable
{
	private bool _isDisposed;

	private string _exceptionMessage = "";

	private const int ChunckSizeKB = 100;

	public Guid Guid { get; set; }

	public int HashAll { get; set; }

	[DefaultValue(ENSource.Accounting)]
	public ENSource Source { get; set; } = ENSource.Accounting;


	public string Version { get; set; } = "";


	public string ClassName { get; set; } = "";


	public string FuncName { get; set; } = "";


	public short Status { get; set; }

	public string ExceptionType { get; set; }

	public string ExceptionMessage
	{
		get
		{
			try
			{
				return _exceptionMessage?.Replace("\r\n", "\n").Replace("\r", "\r\n").Replace("\n", "\r\n");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ExceptionMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 63);
				return "this msg from property catch value is null";
			}
		}
		set
		{
			_exceptionMessage = value;
		}
	}

	public string Description { get; set; }

	public DateTime FirstLogTime { get; set; }

	internal bool Reported { get; set; }

	public int Count { get; set; }

	public List<WebErrorLog> Errors { get; set; }

	public List<WebErrorStateChanges> StateChanges { get; set; }

	public short DepId { get; set; }

	public Priority Priority { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public event Func<object, ErrorLogSumEventArgs, Task> OnReadRequest;

	public event Func<object, List<WebErrorLogSum>, Task> OnReadResponse;

	public event Func<object, List<WebErrorLogSum>, Task> OnNewError;

	public static Packet GenerateReadRequest(short state, DateTime min, DateTime max, IEnumerable<ENSource> sources, string HashList, short top = 100)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)1).ToString(),
					Json.ToStringJson(new ErrorLogSumEventArgs
					{
						State = state,
						Min = min,
						Max = max,
						Top = top,
						Sources = sources?.ToList(),
						HashList = HashList
					})
				};
				return new Packet(PacketType.ErrorHandleSum, 1, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 116);
				throw;
			}
		}
	}

	public static Packet GenerateReadByModifiedRequest(DateTime date)
	{
		using (new JaegerMonitor("GenerateReadByModifiedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)2).ToString(),
					Json.ToStringJson(new ErrorLogSumEventArgs
					{
						Modified = date
					})
				};
				return new Packet(PacketType.ErrorHandleSum, 2, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadByModifiedRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 142);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(int id)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)1).ToString(),
					Json.ToStringJson(new ErrorLogSumEventArgs
					{
						Id = id
					})
				};
				return new Packet(PacketType.ErrorHandleSum, 1, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 169);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(List<IErrorLogSum> items)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.ErrorHandleSum, 3, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 189);
				return null;
			}
		}
	}

	public static Packet GenerateNewError(List<IErrorLogSum> items)
	{
		using (new JaegerMonitor("GenerateNewError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)4).ToString() };
				return new Packet(PacketType.ErrorHandleSum, 4, Json.ToBytesJson(items), list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateNewError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 212);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			ErrorLogSumActions errorLogSumActions = (ErrorLogSumActions)action;
			return errorLogSumActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 228);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((ErrorLogSumActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 241);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs"))
		{
			_ = 3;
			try
			{
				switch ((ErrorLogSumActions)pack.PacketActions)
				{
				case ErrorLogSumActions.ReadRequest:
				{
					ErrorLogSumEventArgs errorLogSumEventArgs2 = pack.Args[1].FromJson<ErrorLogSumEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 263);
					errorLogSumEventArgs2.ConnectionUniqueId = ConnectionUniqueId;
					errorLogSumEventArgs2.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, errorLogSumEventArgs2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs");
					break;
				}
				case ErrorLogSumActions.ReadByModifiedRequest:
				{
					ErrorLogSumEventArgs errorLogSumEventArgs = pack.Args[1].FromJson<ErrorLogSumEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 269);
					errorLogSumEventArgs.Connection = connection;
					errorLogSumEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					await Utilities.RaiseEvent(this.OnReadRequest, this, errorLogSumEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs");
					break;
				}
				case ErrorLogSumActions.ReadResponse:
				{
					List<WebErrorLogSum> args2 = Json.FromJson<List<WebErrorLogSum>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 275);
					await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs");
					break;
				}
				case ErrorLogSumActions.NewError:
				{
					List<WebErrorLogSum> args = Json.FromJson<List<WebErrorLogSum>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 279);
					await Utilities.RaiseEvent(this.OnNewError, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs");
					break;
				}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 287);
			}
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnNewError != null)
				{
					this.OnNewError = null;
				}
				Errors = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebErrorLogSum.cs", 313);
		}
	}
}
