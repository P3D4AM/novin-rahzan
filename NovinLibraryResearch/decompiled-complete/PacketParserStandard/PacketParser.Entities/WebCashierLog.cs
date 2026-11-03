using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebCashierLog : Serializable<WebCashierLog>, ICashierLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid SanadGuid { get; set; }

	public EnLog SanadType { get; set; }

	public string Title { get; set; }

	[Obsolete("please use Title")]
	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public Guid CashierGuid { get; set; }

	public byte[]? OldSerializedData { get; set; }

	public byte[]? NewSerializedData { get; set; }

	public ENAction Action { get; set; }

	[Obsolete("please use SanadType")]
	public WebEntityLog LogType { get; set; }

	public EnSystem System { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[Obsolete("please use SanadType")]
	public Document Part { get; set; }

	public DateTime? DateUtc { get; set; }

	public short StatusId { get; set; }

	public ReturnedState Level { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public new bool IsDisposed { get; private set; }

	public ENSource Source { get; set; }

	public string ApplicationVersion { get; set; }

	public event Func<object, GeneralReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebCashierLog>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebCashierLog>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebCashierLog>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				GeneralReadEventArgs generalReadEventArgs = new GeneralReadEventArgs
				{
					Modified = modified
				};
				list.Add(generalReadEventArgs.ToString());
				return new Packet(PacketType.UserLog, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 65);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebCashierLog> items, int partitionId, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs"))
		{
			try
			{
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.UserLog, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 79);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebCashierLog> titles, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs"))
		{
			try
			{
				string text = titles.Select((WebCashierLog p) => $"{p.Guid}/{p.Guid}").GenerateDebugDescription(5);
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = titles.GeneratePackets(PacketType.UserLog, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 93);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs"))
		{
			try
			{
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.UserLog, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 107);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebCashierLog> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.UserLog, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 120);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		UserLogActions userLogActions = (UserLogActions)action;
		return userLogActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((UserLogActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs"))
		{
			_ = 4;
			try
			{
				switch ((UserLogActions)pack.PacketActions)
				{
				case UserLogActions.SaveRequest:
				{
					ConnectionEventArgs<WebCashierLog> args3 = new ConnectionEventArgs<WebCashierLog>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs");
					break;
				}
				case UserLogActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs");
					break;
				}
				case UserLogActions.ReadRequest:
				{
					GeneralReadEventArgs generalReadEventArgs = pack.Args[1].FromJson<GeneralReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 143);
					generalReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					generalReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, generalReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs");
					break;
				}
				case UserLogActions.ReadResponse:
				{
					ConnectionEventArgs<WebCashierLog> args = new ConnectionEventArgs<WebCashierLog>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs");
					}
					break;
				}
				case (UserLogActions)3:
				case (UserLogActions)4:
				case UserLogActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 160);
			}
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCashierLog.cs", 175);
		}
	}
}
