using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebTaxOrderLog : ITaxOrderLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public Guid OrderGuid { get; set; }

	public EnTaxResult OldState { get; set; }

	public EnTaxResult? NewState { get; set; }

	public EnTaxInvoiceSubject Subject { get; set; }

	public DateTime? TaxSendDate { get; set; }

	public string TaxRefrenceNumber { get; set; }

	public DateTime? TaxDeliveryDate { get; set; }

	public string TaxResultErrorCode { get; set; }

	public string TaxResultMessage { get; set; }

	public string TaxUid { get; set; }

	public string TaxOrderId { get; set; }

	public DateTime InsertDate { get; set; }

	public bool IsSystem { get; set; }

	public short Ordinal { get; set; }

	public string BaseTaxOrderId { get; set; }

	public string InvoiceSeries { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; set; }

	public event Func<object, ConnectionEventArgs<WebTaxOrderLog>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs"))
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
					result = responses.GeneratePackets(PacketType.TaxOrderLog, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs", 57);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebTaxOrderLog> tax, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs"))
		{
			try
			{
				if (tax == null || !tax.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (tax == null)
				{
					result = null;
				}
				else
				{
					result = tax.GeneratePackets(PacketType.TaxOrderLog, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs", 71);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		TaxOrderLogActions taxOrderLogActions = (TaxOrderLogActions)action;
		return taxOrderLogActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((TaxOrderLogActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs"))
		{
			_ = 1;
			try
			{
				switch ((TaxOrderLogActions)pack.PacketActions)
				{
				case TaxOrderLogActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebTaxOrderLog> args2 = new ConnectionEventArgs<WebTaxOrderLog>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs");
					}
					break;
				case TaxOrderLogActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs");
					}
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs", 99);
			}
		}
	}

	public void Dispose()
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Accounting\\TaxService\\WebTaxOrderLog.cs", 110);
		}
	}
}
