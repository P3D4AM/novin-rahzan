using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebPrdSerial : Serializable<WebPrdSerial>, IPrdSerial, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime InsertDate { get; set; }

	public Guid ProductGuid { get; set; }

	public string Serial { get; set; }

	public string GarrantyCode { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public List<WebPrdSerialDetail> DetList { get; set; }

	public bool IsTransfer { get; set; }

	public string TechnicalInfo1 { get; set; }

	public string TechnicalInfo2 { get; set; }

	public string TechnicalInfo3 { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<WebPrdSerial>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebPrdSerial>, Task> OnReadResponse;

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs"))
		{
			_ = 2;
			try
			{
				switch ((WebSerialActions)pack.PacketActions)
				{
				case WebSerialActions.SaveRequest:
				{
					ConnectionEventArgs<WebPrdSerial> args2 = new ConnectionEventArgs<WebPrdSerial>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs");
					break;
				}
				case WebSerialActions.SaveResponse:
				{
					SaveResponseEventArgs args3 = new SaveResponseEventArgs(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args3, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs");
					break;
				}
				case WebSerialActions.ReadResponse:
					if (this.OnReadResponse != null)
					{
						ConnectionEventArgs<WebPrdSerial> args = new ConnectionEventArgs<WebPrdSerial>(pack, ConnectionUniqueId, Connection);
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs");
					}
					break;
				default:
					WebErrorLog.Instance.StartLog($"Unrecognized packetAction for WebPrdAnbarGardani({pack.PacketActions})", "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs", 64);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs", 69);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		WebSerialActions webSerialActions = (WebSerialActions)action;
		return webSerialActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((WebSerialActions)action).GetDisplay();
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebPrdSerial> webPrdSerial, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs"))
		{
			try
			{
				if (webPrdSerial == null || !webPrdSerial.Any())
				{
					throw new NullReferenceException();
				}
				string text = webPrdSerial.Select((WebPrdSerial p) => $"{p.Guid}/{p.Serial}").GenerateDebugDescription(5);
				object result;
				if (webPrdSerial == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = webPrdSerial.GeneratePackets(PacketType.PrdSerial, 1, null, isSync: false, Guid.NewGuid(), partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs", 84);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs"))
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
					result = responses.GeneratePackets(PacketType.PrdSerial, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs", 99);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebPrdSerial> groups, int partitionId, bool isSync = false)
	{
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs"))
		{
			try
			{
				if (groups == null || !groups.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (groups == null)
				{
					result = null;
				}
				else
				{
					result = groups.GeneratePackets(PacketType.PrdSerial, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs", 113);
				throw;
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
				this.OnReadResponse = null;
				this.OnSaveRequest = null;
				this.OnSaveResponse = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdSerial.cs", 130);
		}
	}
}
