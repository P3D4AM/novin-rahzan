using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebSanad : Serializable<WebSanad>, ISanad, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IReceiver, INpDisposable, IDisposable
{
	public enum SanadActions : short
	{
		[PersianNameAttribute.PersianName("نامشخص")]
		Unknown = 0,
		[PersianNameAttribute.PersianName("درخواست ذخیره")]
		SaveRequest = 1,
		[PersianNameAttribute.PersianName("پاسخ به درخواست ذخیره")]
		SaveResponse = 2,
		[PersianNameAttribute.PersianName("درخواست خواندن")]
		ReadRequest = 5,
		[PersianNameAttribute.PersianName("سینک")]
		ReadResponse = 6,
		[PersianNameAttribute.PersianName("ریست")]
		Reset = 7
	}

	private int sanadNo;

	public int Id { get; set; }

	public DateTime DateM { get; set; }

	public ENSource Source { get; set; }

	[PersianNameAttribute.PersianName("تاریخ")]
	public string DateSh => DateM.MiladiToCalture();

	[PersianNameAttribute.PersianName("شرح سند")]
	public string Description { get; set; }

	public string Ip { get; set; }

	public int ReferencedId { get; set; }

	public DateTime? DeliveryDate { get; set; }

	[PersianNameAttribute.PersianName("شماره سند")]
	public int SanadNo { get; set; }

	public int DeliveryId { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	[PersianNameAttribute.PersianName("نوع سند")]
	public SanadType SanadType { get; set; }

	public bool? DeliveryStatus { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string LkSerial { get; set; }

	public string HddSerial { get; set; }

	public string CpuSerial { get; set; }

	public string AndroidIme { get; set; }

	[PersianNameAttribute.PersianName("وضعیت سند")]
	public short StatusId { get; set; }

	public SanadMetaData MetaData { get; set; }

	[PersianNameAttribute.PersianName("وضعیت سند")]
	public string SanadStName => SanadSt.GetDisplay();

	public SanadSt SanadSt { get; set; }

	public short UserId { get; set; }

	[PersianNameAttribute.PersianName("اقلام سند")]
	public List<WebSanad_Det> Lst { get; set; }

	[PersianNameAttribute.PersianName("کاربر صادرکننده")]
	public Guid UserGuid { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public bool IsTemp { get; set; }

	[PersianNameAttribute.PersianName("جمع بدهکار")]
	public decimal Debit { get; set; }

	[PersianNameAttribute.PersianName("جمع بستانکار")]
	public decimal Credit { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public short IdUser { get; set; }

	public DateTime DateSabt { get; set; }

	public int TafsilId { get; set; }

	public Guid TafsilGuid { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	public event Func<object, ConnectionEventArgs<WebSanad>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ReadOperationEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebSanad>, Task> OnReadResponse;

	public int GetSanadNo()
	{
		return sanadNo;
	}

	public void SetSanadNo(int value)
	{
		sanadNo = value;
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs"))
		{
			_ = 3;
			try
			{
				switch ((SanadActions)pack.PacketActions)
				{
				case SanadActions.ReadResponse:
				{
					ConnectionEventArgs<WebSanad> args2 = new ConnectionEventArgs<WebSanad>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnReadResponse, this, args2, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs");
					break;
				}
				case SanadActions.ReadRequest:
				{
					ReadOperationEventArgs readOperationEventArgs = pack.Args[1].FromJson<ReadOperationEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs", 80);
					readOperationEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					readOperationEventArgs.Connection = Connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, readOperationEventArgs, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs");
					break;
				}
				case SanadActions.SaveRequest:
					Json.FromJson<List<WebSanad>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs", 86);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, new ConnectionEventArgs<WebSanad>(pack, ConnectionUniqueId, Connection), Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs");
					break;
				case SanadActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs");
					break;
				}
				case (SanadActions)3:
				case (SanadActions)4:
				case SanadActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs", 98);
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		return ((SanadActions)action).GetDisplay();
	}

	public string GetPacketActionEnumName(short action)
	{
		SanadActions sanadActions = (SanadActions)action;
		return sanadActions.ToString();
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebSanad> items, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.Sanad, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs", 112);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> lst)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs"))
		{
			try
			{
				if (lst == null || (lst != null && lst.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return lst?.GeneratePackets(PacketType.Sanad, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebSanad.cs", 126);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebSanad> items, Guid packetRefrencedId, int partitionId = 0)
	{
		return items?.GeneratePackets(PacketType.Sanad, 1, null, isSync: false, packetRefrencedId, partitionId, deliveryRequst: false, "GenerateSaveRequest");
	}
}
