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

[Serializable]
public class WebPrdAnbarGardani : Serializable<WebPrdAnbarGardani>, IPrdAnbarGardani, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IReceiver, INpDisposable, IDisposable
{
	public enum PrdAnbarGardaniActions : short
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown = 0,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest = 1,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse = 2,
		[PersianNameAttribute.PersianName("درخواست خواندن")]
		ReadRequest = 5,
		[PersianNameAttribute.PersianName("پاسخ به خواندن یا سینک")]
		ReadResponse = 6,
		[PersianNameAttribute.PersianName("ریست")]
		Reset = 7
	}

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime DateM { get; set; }

	public string DateSh { get; }

	public DateTime DateSabt { get; set; }

	public int Number { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public DateTime Date { get; set; }

	public Guid? KasrDebitTafsilGuid { get; set; }

	public Guid KasrDebitMoeinGuid { get; set; }

	public Guid? KasrCreditTafsilGuid { get; set; }

	public Guid KasrCreditMoeinGuid { get; set; }

	public Guid? EzafeDebitTafsilGuid { get; set; }

	public Guid? EzafeCreditTafsilGuid { get; set; }

	public Guid EzafeDebitMoeinGuid { get; set; }

	public Guid EzafeCreditMoeinGuid { get; set; }

	public bool ISRead { get; set; }

	public Guid UserGuid { get; set; }

	public string Description { get; set; }

	public List<WebPrdAnbarGardaniDet> DetList { get; set; } = new List<WebPrdAnbarGardaniDet>();


	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public bool? DeliveryStatus { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string LkSerial { get; set; }

	public string HddSerial { get; set; }

	public string CpuSerial { get; set; }

	public string AndroidIme { get; set; }

	public ENSource Source { get; set; }

	public string Ip { get; set; }

	public short DepID { get; set; }

	public string SerializedData { get; set; }

	public long HashID { get; set; }

	public short StatusId { get; set; }

	public SanadType Type { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal KasrSum { get; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal EzafeSum { get; }

	public DateTime? DateUtc { get; set; }

	public event Func<object, ConnectionEventArgs<WebPrdAnbarGardani>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs"))
		{
			_ = 1;
			try
			{
				switch ((PrdAnbarGardaniActions)pack.PacketActions)
				{
				case PrdAnbarGardaniActions.SaveRequest:
				{
					ConnectionEventArgs<WebPrdAnbarGardani> args2 = new ConnectionEventArgs<WebPrdAnbarGardani>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs");
					break;
				}
				case PrdAnbarGardaniActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs");
					break;
				}
				default:
					WebErrorLog.Instance.StartLog($"Unrecognized packetAction for WebPrdAnbarGardani({pack.PacketActions})", "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs", 87);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs", 92);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		return ((PrdAnbarGardaniActions)action).GetDisplay();
	}

	public string GetPacketActionName(short action)
	{
		return ((PrdAnbarGardaniActions)action).GetDisplay();
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebPrdAnbarGardani> items, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.PrdAnbarGardani, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs", 106);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebPrdAnbarGardani> titles, Guid packetRefrencedId, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.PrdAnbarGardani, 1, null, isSync: false, packetRefrencedId, partitionId, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs", 119);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.PrdAnbarGardani, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs", 133);
				throw;
			}
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public new void Dispose(bool disposing)
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPrdAnbarGardani.cs", 147);
		}
	}
}
