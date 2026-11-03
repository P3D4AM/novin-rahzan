using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebReception : Serializable<WebReception>, IReception, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid, IReceiver, INpDisposable, IDisposable
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/Reception/";

	public int Id { get; set; }

	public bool IsChecked { get; set; }

	public int Id2 { get; set; }

	public int MoeinId { get; set; }

	public int TafsilId { get; set; }

	public string FullName { get; set; }

	public Guid TafsilGuid { get; set; }

	public short SanadSt { get; set; }

	public DateTime? TransferDate { get; set; }

	public DateTime? DateMoaser { get; set; }

	public DateTime? DateUtc { get; set; }

	[Obsolete]
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

	public short StatusId { get; set; }

	public int SanadNumber { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public virtual decimal SumNaghd => ListNaghd?.Sum((WebReceptionNaghd p) => p?.Price ?? 0m) ?? 0m;

	[XmlIgnore]
	[JsonIgnore]
	public virtual decimal SumBank => ListBank?.Sum((WebReceptionHavale p) => p?.Price ?? 0m) ?? 0m;

	[XmlIgnore]
	[JsonIgnore]
	public virtual decimal SumCheck => ListCheck?.Where((WebReceptionCheck p) => p.Status != ENUMCheck.BatelShode).Sum((WebReceptionCheck p) => p?.Price ?? 0m) ?? 0m;

	[XmlIgnore]
	[JsonIgnore]
	public virtual decimal SumEnteghal => ListEnteghal?.Sum((WebSanad_Det p) => p.Debit - p.Credit) ?? 0m;

	public decimal AfterAccount { get; set; }

	public decimal BeforeAccount { get; set; }

	public bool Status { get; set; }

	public virtual short IdUser { get; set; }

	public string Description { get; set; }

	public Guid Guid { get; set; } = Guid.Empty;


	public DateTime Modified { get; set; }

	public DateTime Date { get; set; }

	[JsonIgnore]
	public string DateSh => Date.MiladiToCalture();

	[JsonIgnore]
	public virtual decimal Sum => SumCheck + SumEnteghal + SumBank + SumNaghd;

	public ENSource Source { get; set; }

	public string LkSerial { get; set; }

	public string HddSerial { get; set; }

	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public string AndroidIme { get; set; }

	public string Ip { get; set; }

	public int ReferencedId { get; set; }

	public List<WebReceptionCheck> ListCheck { get; set; }

	public List<WebReceptionNaghd> ListNaghd { get; set; }

	public List<WebReceptionHavale> ListBank { get; set; }

	public List<WebSanad_Det> ListEnteghal { get; set; }

	[JsonIgnore]
	public string ReceptionData { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public bool? DeliveryStatus { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

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

	public bool IsTemp { get; set; }

	public DependencyType DependencyType { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public WebCus CusInfo { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid UserGuid { get; set; }

	public string HostRefrenced { get; set; }

	public ReceptionMetaData MetaData { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public Guid? OrderGuid { get; set; }

	public Document? OrderType { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public event Func<object, ConnectionEventArgs<WebReception>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ReadOperationEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebReception>, Task> OnReadResponse;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebReception> items)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			try
			{
				if (items == null || !items.Any())
				{
					throw new NullReferenceException();
				}
				return items?.GeneratePackets(PacketType.Reception, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 106);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> lst)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			try
			{
				if (lst == null || !lst.Any())
				{
					throw new NullReferenceException();
				}
				return lst?.GeneratePackets(PacketType.Reception, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 120);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(int HostID, Guid guid, DateTime Date, string Shortmessage, string message, bool SaveResultStatus)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
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
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 142);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(string[] Args, string ConnectionUniqueId, List<ReadStatus> status, bool ChangeReadST, short Top, ENSource Source, short DepId, int UId, long LKSerial)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)5).ToString(),
					new ReadOperationEventArgs
					{
						ChangeReadST = ChangeReadST,
						Top = Top,
						Status = status
					}.ToString()
				};
				return new Packet(PacketType.Reception, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 166);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebReception> items)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.Reception, 6, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 180);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebReception> receptions)
	{
		if (receptions == null || !receptions.Any())
		{
			throw new NullReferenceException();
		}
		try
		{
			return receptions.GeneratePackets(PacketType.Reception, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 193);
			throw;
		}
	}

	public ReturnedSaveFuncInfo CheckValidation()
	{
		using (new JaegerMonitor("CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				if (Date < new DateTime(2010, 1, 1) || Date > new DateTime(2099, 1, 1))
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "تاریخ وارد شده جهت برگه دریافت معتبر نمی باشد", "Date", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 204);
				}
				if (TafsilId <= 0 && TafsilGuid == Guid.Empty)
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "ردیف طرف حساب انتخاب شده جهت صدور برگه دریافت معتبر نمی باشد", "TafsilId", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 206);
				}
				if (Sum <= 0m)
				{
					returnedSaveFuncInfo.AddReturnedValue(ReturnedState.Error, "برگه دریافت با مبلغ صفر یا منفی قابل ثبت نمیباشد.", "Sum", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 207);
				}
				_ = Guid;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
			}
			catch (Exception ex)
			{
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 212);
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 213);
			}
			return returnedSaveFuncInfo;
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		ReceptionActions receptionActions = (ReceptionActions)action;
		return receptionActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((ReceptionActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			_ = 3;
			try
			{
				switch ((ReceptionActions)pack.PacketActions)
				{
				case ReceptionActions.ReadResponse:
				{
					ConnectionEventArgs<WebReception> args2 = new ConnectionEventArgs<WebReception>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs");
					break;
				}
				case ReceptionActions.ReadRequest:
				{
					ReadOperationEventArgs readOperationEventArgs = pack.Args[1].FromJson<ReadOperationEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 233);
					readOperationEventArgs.ConnectionUniqueId = connectionUniqueId;
					readOperationEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, readOperationEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs");
					break;
				}
				case ReceptionActions.SaveRequest:
					Json.FromJson<List<WebReception>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 239);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, new ConnectionEventArgs<WebReception>(pack, connectionUniqueId, connection), connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs");
					break;
				case ReceptionActions.SaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs");
					break;
				}
				case (ReceptionActions)3:
				case (ReceptionActions)4:
				case ReceptionActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 251);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebReception>> Send2ServerAsync(string userName, string password, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebReception> result = new ReturnedSaveFuncInfoWithValue<WebReception>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebUser>> returnedSaveFuncInfoWithValue = await DepClient.GetByUserAsync(userName, password, token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 259);
			if (result.HasError)
			{
				return result;
			}
			if (returnedSaveFuncInfoWithValue.value == null || returnedSaveFuncInfoWithValue.value.Data == null)
			{
				result.AddError("دپارتمان مورد نظر معتبر نمی باشد", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 263);
				return result;
			}
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await new RestTokenManagement(returnedSaveFuncInfoWithValue.value.Data.Type, userName, password).GetToken(token);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 267);
			if (result.HasError)
			{
				return result;
			}
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue2.value ?? "") ?? ""
			} };
			ReturnedSaveFuncInfoWithValue<WebResponse<WebReception>> returnedSaveFuncInfoWithValue3 = await HttpHelpers.PostToApi<WebReception, WebReception>(this, apiUrl ?? "", headers, strongDeserialize: false, 15000, 3);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 274);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue3.value.Data;
		}
		catch (Exception ex)
		{
			result.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 280);
			WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 281);
		}
		return result;
	}

	public byte[] ToBytes()
	{
		using (new JaegerMonitor("ToBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs"))
		{
			try
			{
				return Json.ToBytesJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToBytes", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 294);
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
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebReception.cs", 310);
		}
	}
}
