using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebPardakht : Serializable<WebPardakht>, IPardakht, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid, IReceiver, INpDisposable, IDisposable
{
	public List<WebPardakhtBank> Bank { get; set; }

	public DateTime? TransferDate { get; set; }

	public DateTime? DateUtc { get; set; }

	public DateTime? DateMoaser { get; set; }

	public List<WebPardakhtCheckM> CheckM { get; set; }

	public List<WebPardakhtCheckSh> CheckSh { get; set; }

	public List<WebPardakhtNaghd> Naghd { get; set; }

	public List<WebSanad_Det> Enteghal { get; set; }

	public short StatusId { get; set; }

	public int Id { get; set; }

	[JsonIgnore]
	public virtual decimal Sum => SumCheckSh + SumBank + SumNaghd + SumCheckM + SumEnteghal;

	public ServerStatusForClient ServerStatus { get; set; }

	[JsonIgnore]
	public virtual decimal SumCheckSh => (CheckSh?.Where((WebPardakhtCheckSh q) => q.Status != 104)?.Sum((WebPardakhtCheckSh q) => q.Price)).GetValueOrDefault();

	[JsonIgnore]
	public virtual decimal SumBank => Bank?.Sum((WebPardakhtBank q) => q.Price) ?? 0m;

	[JsonIgnore]
	public virtual decimal SumNaghd => Naghd?.Sum((WebPardakhtNaghd q) => q.Price) ?? 0m;

	[JsonIgnore]
	public virtual decimal SumCheckM => CheckM?.Sum((WebPardakhtCheckM q) => q.Price) ?? 0m;

	[JsonIgnore]
	public virtual decimal SumEnteghal => Enteghal?.Sum((WebSanad_Det q) => q.Credit - q.Debit) ?? 0m;

	[JsonIgnore]
	public string DateSh => Date.MiladiToCalture();

	public bool IsRead { get; set; }

	[Obsolete]
	public short DeviceId { get; set; }

	public int Id2 { get; set; }

	public int MoeinId { get; set; }

	public int TafsilId { get; set; }

	public Guid TafsilGuid { get; set; }

	public SanadSt SanadSt { get; set; }

	public int SanadNo
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public int SanadNumber { get; set; }

	public virtual short IdUser { get; set; }

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	public DateTime Date { get; set; }

	public PardakhtSt Statuse { get; set; }

	public Guid Guid { get; set; } = Guid.Empty;


	public ENSource Source { get; set; }

	public string LkSerial { get; set; }

	public string HddSerial { get; set; }

	public string FullName { get; set; }

	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public string AndroidIme { get; set; }

	public string Ip { get; set; }

	public int ReferencedId { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public bool? DeliveryStatus { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string DeliveryStatusFa
	{
		get
		{
			if (DeliveryStatus.HasValue)
			{
				if (!DeliveryStatus.GetValueOrDefault())
				{
					return "خطا در تحویل";
				}
				return "تحویل شده";
			}
			return "ارسال شده";
		}
	}

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public bool IsTemp { get; set; }

	public PardakhtMetaData MetaData { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid UserGuid { get; set; }

	public string HostRefrenced { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public event Func<object, ConnectionEventArgs<WebPardakht>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ReadOperationEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebPardakht>, Task> OnReadResponse;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebPardakht> item)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				if (item == null || !item.Any())
				{
					throw new NullReferenceException();
				}
				return item?.GeneratePackets(PacketType.Pardakht, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 99);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(List<ReadStatus> status, bool ChangeReadST, short Top)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)5).ToString(),
					new ReadOperationEventArgs
					{
						Status = status,
						Top = Top,
						ChangeReadST = ChangeReadST
					}.ToString()
				};
				return new Packet(PacketType.Pardakht, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 123);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(int HostID, Guid guid, DateTime Date, string Shortmessage, string message, bool SaveResultStatus)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				SaveResponse saveResponse = new SaveResponse
				{
					ReferencedId = HostID,
					Guid = guid,
					Date = Date,
					Shortmessage = Shortmessage,
					Message = message,
					SaveResultStatus = SaveResultStatus
				};
				return GenerateSaveResponse(new SaveResponse[1] { saveResponse });
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 146);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> lst)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				if (lst == null || !lst.Any())
				{
					throw new NullReferenceException();
				}
				return lst?.GeneratePackets(PacketType.Pardakht, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 160);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebPardakht> items)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.Pardakht, 6, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 174);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebPardakht> pardakhts)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				if (pardakhts == null || !pardakhts.Any())
				{
					throw new NullReferenceException();
				}
				return pardakhts.GeneratePackets(PacketType.Pardakht, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 188);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		PardakhtActions pardakhtActions = (PardakhtActions)action;
		return pardakhtActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((PardakhtActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			_ = 3;
			try
			{
				switch ((PardakhtActions)pack.PacketActions)
				{
				case PardakhtActions.ReadResponse:
				{
					Json.FromJson<List<WebPardakht>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 204);
					ConnectionEventArgs<WebPardakht> args3 = new ConnectionEventArgs<WebPardakht>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs");
					break;
				}
				case PardakhtActions.ReadRequest:
				{
					ReadOperationEventArgs readOperationEventArgs = pack.Args[1].FromJson<ReadOperationEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 209);
					readOperationEventArgs.ConnectionUniqueId = connectionUniqueId;
					readOperationEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, readOperationEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs");
					break;
				}
				case PardakhtActions.SaveRequest:
				{
					ConnectionEventArgs<WebPardakht> args2 = new ConnectionEventArgs<WebPardakht>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs");
					break;
				}
				case PardakhtActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs");
					break;
				}
				default:
					throw new NotImplementedException("نوع بسته داده جهت فاکتور به درستی وارد نشده است");
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 227);
			}
		}
	}

	public ReturnedSaveFuncInfo CheckValidation()
	{
		using (new JaegerMonitor("CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				if (Date < new DateTime(2010, 1, 1) || Date > new DateTime(2099, 1, 1))
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "تاریخ وارد شده معتبر نمی باشد", "Date", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 236);
				}
				if (TafsilId <= 0 && TafsilGuid == Guid.Empty)
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "ردیف طرف حساب تفصیلی برگه پرداخت انتخاب شده معتبر نمی باشد", "TafsilGuid", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 238);
				}
				_ = Guid;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				if (Sum <= 0m)
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "پرداخت با مبلغ صفر یا منفی قابل ثبت نمیباشد" + Source, "Source", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 240);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 244);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 245);
			}
			return returnedSaveFuncInfo;
		}
	}

	public string AutoTitle(ENAction act)
	{
		using (new JaegerMonitor("AutoTitle", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs"))
		{
			try
			{
				string text = "";
				switch (act)
				{
				case ENAction.Add:
					text = "صدور";
					break;
				case ENAction.Edit:
					text = "ویرایش";
					break;
				case ENAction.Delete:
					text = "حذف";
					break;
				case ENAction.Report:
					text = "مشاهده";
					break;
				case ENAction.Login:
					text = "ورود کاربر";
					break;
				case ENAction.Logout:
					text = "خروج کاربر";
					break;
				}
				return text + "پرداخت  (" + Id2 + ") ";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AutoTitle", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 272);
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
				Bank = null;
				CheckM = null;
				CheckSh = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPardakht.cs", 292);
		}
	}
}
