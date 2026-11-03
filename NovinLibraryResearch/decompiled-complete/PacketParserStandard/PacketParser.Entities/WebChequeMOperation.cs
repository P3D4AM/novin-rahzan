using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Cheque;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebChequeMOperation : IChequeMOperation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public enum ChequeMActions
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse
	}

	public DateTime DateM { get; set; }

	public DateTime? DateMUtc { get; set; }

	public Guid CashierGuid { get; set; }

	public int SanadNO { get; set; }

	public SanadSt SanadSt { get; set; }

	public DateTime DateSabt { get; set; }

	public bool? Avale2re_Transfer { get; set; }

	public Guid GuidCheque { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public short Ordinal { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public ENUMCheck? OldState { get; set; }

	public ENUMCheck NewState { get; set; }

	public bool? IsDebitTafsilAffective { get; set; }

	public bool? IsCreditTafsilAffective { get; set; }

	public string Description { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public bool IsDisposed { get; private set; }

	public Document? AsType { get; set; }

	public Guid? AsGuid { get; set; }

	public short StatusId { get; set; }

	[DefaultValue(true)]
	public bool IsSanadAffective { get; set; } = true;


	public DateTime? DateUtc { get; set; }

	public decimal Price { get; set; }

	public event Func<object, ConnectionEventArgs<WebChequeMOperation>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeMOperation.cs"))
		{
			_ = 1;
			try
			{
				switch ((ChequeMActions)pack.PacketActions)
				{
				case ChequeMActions.SaveRequest:
				{
					ConnectionEventArgs<WebChequeMOperation> args2 = new ConnectionEventArgs<WebChequeMOperation>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeMOperation.cs");
					break;
				}
				case ChequeMActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeMOperation.cs");
					break;
				}
				default:
					WebErrorLog.Instance.StartLog($"Unrecognized packetAction for CheckMOperation({pack.PacketActions})", "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeMOperation.cs", 68);
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebChequeMOperation.cs", 73);
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		return ((ChequeMActions)action).GetDisplay();
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebChequeMOperation> items, Guid packetRefrencedId, int partitionId = 0)
	{
		return items?.GeneratePackets(PacketType.CheckMOperation, 1, null, isSync: false, packetRefrencedId, partitionId, deliveryRequst: false, "GenerateSaveRequest");
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		return responses?.GeneratePackets(PacketType.CheckMOperation, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
	}
}
