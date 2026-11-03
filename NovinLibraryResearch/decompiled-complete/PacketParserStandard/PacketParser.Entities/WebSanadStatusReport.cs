using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebSanadStatusReport : Serializable<WebSanadStatusReport>, ISanadStatusReport, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public int Id { get; set; }

	public Guid Guid { get; set; }

	public Document AsType { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public SanadStatusReportMetaData MetaData { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public int WarningMinutes { get; set; }

	public int ErrorMinutes { get; set; }

	public long? Id2From { get; set; }

	public long? Id2To { get; set; }

	public int InactivityDays { get; set; }

	public int TargetStatusId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, SanadStatusReportReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebSanadStatusReport>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebSanadStatusReport>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebSanadStatusReport>, Task> OnSync;

	public string GetPacketActionName(short action)
	{
		return ((SanadStatusReportActions)action).GetDisplay();
	}

	public string GetPacketActionEnumName(short action)
	{
		SanadStatusReportActions sanadStatusReportActions = (SanadStatusReportActions)action;
		return sanadStatusReportActions.ToString();
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				SanadStatusReportReadEventArgs sanadStatusReportReadEventArgs = new SanadStatusReportReadEventArgs
				{
					Modified = modified
				};
				list.Add(sanadStatusReportReadEventArgs.ToString());
				return new Packet(PacketType.SanadStatusReport, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 58);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebSanadStatusReport> items, bool isSync, int partitionId)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs"))
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
					result = items.GeneratePackets(PacketType.SanadStatusReport, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 72);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebSanadStatusReport> titles, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs"))
		{
			try
			{
				if (titles == null || !titles.Any())
				{
					throw new NullReferenceException();
				}
				string text = titles.Select((WebSanadStatusReport p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = titles.GeneratePackets(PacketType.SanadStatusReport, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 87);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs"))
		{
			try
			{
				if (responses == null || !responses.Any())
				{
					throw new NullReferenceException();
				}
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.SanadStatusReport, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 102);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebSanadStatusReport> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.SanadStatusReport, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 115);
				throw;
			}
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs"))
		{
			_ = 4;
			try
			{
				switch ((SanadStatusReportActions)pack.PacketActions)
				{
				case SanadStatusReportActions.SaveRequest:
				{
					ConnectionEventArgs<WebSanadStatusReport> args3 = new ConnectionEventArgs<WebSanadStatusReport>(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs");
					break;
				}
				case SanadStatusReportActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs");
					break;
				}
				case SanadStatusReportActions.ReadRequest:
				{
					SanadStatusReportReadEventArgs sanadStatusReportReadEventArgs = pack.Args[1].FromJson<SanadStatusReportReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 136);
					sanadStatusReportReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					sanadStatusReportReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, sanadStatusReportReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs");
					break;
				}
				case SanadStatusReportActions.ReadResponse:
				{
					ConnectionEventArgs<WebSanadStatusReport> args = new ConnectionEventArgs<WebSanadStatusReport>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs");
					}
					break;
				}
				case (SanadStatusReportActions)3:
				case (SanadStatusReportActions)4:
				case SanadStatusReportActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 153);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanadStatusReport.cs", 168);
		}
	}
}
