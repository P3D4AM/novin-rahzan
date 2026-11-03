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
public class WebVisit : Serializable<WebVisit>, IVisit, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private const int ChunckSizeKb = 100;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; } = DateTime.Now;


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime ScheduledDate { get; set; } = DateTime.Now;


	public DateTime StartDate { get; set; } = DateTime.Now;


	public DateTime CreationdDate { get; set; } = DateTime.Now;


	public string Description { get; set; }

	public int VisitNo { get; set; }

	public bool VisitIsCompleted { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public List<Guid> TicketGuids { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, GeneralReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebVisit>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebVisit>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebVisit>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				GeneralReadEventArgs generalReadEventArgs = new GeneralReadEventArgs
				{
					Modified = modified
				};
				list.Add(generalReadEventArgs.ToString());
				return new Packet(PacketType.Visit, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 86);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebVisit> items, int partitionId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs"))
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
					result = items.GeneratePackets(PacketType.Visit, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 101);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebVisit> titles, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs"))
		{
			try
			{
				string text = titles.Select((WebVisit p) => $"{p.Guid}/{p.Guid}").GenerateDebugDescription(5);
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = titles.GeneratePackets(PacketType.Visit, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 116);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs"))
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
					result = responses.GeneratePackets(PacketType.Visit, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 131);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebVisit> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.Visit, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 146);
				throw;
			}
		}
	}

	public string GetSerializedTicketGuids()
	{
		return Json.ToStringJson(TicketGuids);
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			VisitActions visitActions = (VisitActions)action;
			return visitActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 163);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((VisitActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 176);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs"))
		{
			_ = 4;
			try
			{
				switch ((VisitActions)pack.PacketActions)
				{
				case VisitActions.SaveRequest:
				{
					ConnectionEventArgs<WebVisit> args3 = new ConnectionEventArgs<WebVisit>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs");
					break;
				}
				case VisitActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs");
					break;
				}
				case VisitActions.ReadRequest:
				{
					GeneralReadEventArgs generalReadEventArgs = pack.Args[1].FromJson<GeneralReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 198);
					generalReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					generalReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, generalReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs");
					break;
				}
				case VisitActions.ReadResponse:
				{
					ConnectionEventArgs<WebVisit> args = new ConnectionEventArgs<WebVisit>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs");
					}
					break;
				}
				case (VisitActions)3:
				case (VisitActions)4:
				case VisitActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 220);
			}
		}
	}

	Task IReceiver.DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		throw new NotImplementedException();
	}

	public new void Dispose()
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebVisit.cs", 241);
		}
	}
}
