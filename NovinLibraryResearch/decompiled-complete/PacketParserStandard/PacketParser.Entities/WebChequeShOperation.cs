using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Cheque;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebChequeShOperation : IChequeShOperation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public enum ChequeShActions
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse
	}

	public SanadSt SanadSt { get; set; }

	public int SanadNo { get; set; }

	public DateTime DateM { get; set; }

	public DateTime? DateMUtc { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid ChequeGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public short Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public ENCheckSHStatus? OldState { get; set; }

	public ENCheckSHStatus NewState { get; set; }

	public bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public short StatusId { get; set; }

	public DateTime? DateUtc { get; set; }

	public decimal Price { get; set; }

	public event Func<object, ConnectionEventArgs<WebChequeShOperation>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeShOperation.cs"))
		{
			_ = 1;
			try
			{
				switch ((ChequeShActions)pack.PacketActions)
				{
				case ChequeShActions.SaveRequest:
				{
					ConnectionEventArgs<WebChequeShOperation> args2 = new ConnectionEventArgs<WebChequeShOperation>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeShOperation.cs");
					break;
				}
				case ChequeShActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeShOperation.cs");
					break;
				}
				default:
					WebErrorLog.Instance.StartLog($"Unrecognized packetAction for CheckShOperation({pack.PacketActions})", "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeShOperation.cs", 59);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeShOperation.cs", 64);
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		return ((ChequeShActions)action).GetDisplay();
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebChequeShOperation> items, Guid packetRefrencedId, int partitionId = 0)
	{
		return items?.GeneratePackets(PacketType.CheckShOperation, 1, null, isSync: false, packetRefrencedId, partitionId, deliveryRequst: false, "GenerateSaveRequest");
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		return responses?.GeneratePackets(PacketType.CheckShOperation, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
	}
}
