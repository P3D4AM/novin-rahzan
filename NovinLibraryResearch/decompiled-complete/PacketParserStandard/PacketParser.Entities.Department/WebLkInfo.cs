using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.Activation;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Department;
using PacketParser.EventArgs;

namespace PacketParser.Entities.Department;

public class WebLkInfo : ILkInfo, IHasModified, IHasStatus, INpDisposable, IDisposable
{
	public enum LkInfoActions
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

	public Guid Guid { get; set; }

	public AccountingPackage AccountingPackage { get; set; }

	public ENCode TypeAccounting { get; set; }

	public HozoorPackage HozoorType { get; set; }

	public long LKSerial { get; set; }

	public Guid TafsilGuid { get; set; }

	public DateTime ExpireDate { get; set; }

	public DateTime StartDate { get; set; }

	public string Description { get; set; }

	public bool Tolid { get; set; }

	public bool BarCode { get; set; }

	public bool Scale { get; set; }

	public bool SpecialAccountingPackage { get; set; }

	public TypeHozor Hozor_Hoghogh { get; set; }

	public short ScaleCount { get; set; }

	public short MaxActivationDays { get; set; }

	public short Net_Accounting { get; set; }

	public short Net_Hozor { get; set; }

	public short Net_Scales { get; set; }

	public bool CallerID { get; set; }

	[Obsolete("", true)]
	public decimal TamdidPrice_Total_1year_Net { get; set; }

	public DateTime? FailedDate { get; set; }

	[JsonIgnore]
	public string FailedDateSh => FailedDate.MiladiToCalture();

	public string FailedDescription { get; set; }

	public bool IsFailed => !string.IsNullOrEmpty(FailedDescription);

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public short DepId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	[XmlIgnore]
	[JsonIgnore]
	public string ExpireDateSh => ExpireDate.MiladiToCalture();

	public short DepID { get; set; }

	public DateTime? DisableActivationCodeDate { get; set; }

	public Customer Customer_CLS { get; set; }

	public LkInfoMeta MetaData { get; set; }

	public EnLK_SpecialFacilities SpecialFacilities { get; set; }

	[JsonIgnore]
	public bool HasSpecialCheque_Khazane => (SpecialFacilities & EnLK_SpecialFacilities.Cheque) == EnLK_SpecialFacilities.Cheque;

	[JsonIgnore]
	public bool HasSpecialInstallment => (SpecialFacilities & EnLK_SpecialFacilities.Aghsat) == EnLK_SpecialFacilities.Aghsat;

	[JsonIgnore]
	public bool HasSpecialAccounting => (SpecialFacilities & EnLK_SpecialFacilities.AdvancedFinance) == EnLK_SpecialFacilities.AdvancedFinance;

	[JsonIgnore]
	public bool HasSpecialProductDepot_Pishrafteh => (SpecialFacilities & EnLK_SpecialFacilities.Depot) == EnLK_SpecialFacilities.Depot;

	[JsonIgnore]
	public bool HasSpecialService => (SpecialFacilities & EnLK_SpecialFacilities.Service) == EnLK_SpecialFacilities.Service;

	[JsonIgnore]
	public bool HasSpecialTax => (SpecialFacilities & EnLK_SpecialFacilities.Tax) == EnLK_SpecialFacilities.Tax;

	[JsonIgnore]
	public bool HasSpecialCustomerClub => (SpecialFacilities & EnLK_SpecialFacilities.CustomerClub) == EnLK_SpecialFacilities.CustomerClub;

	[JsonIgnore]
	public bool HasSpecialProductSerial => (SpecialFacilities & EnLK_SpecialFacilities.ProductSerial) == EnLK_SpecialFacilities.ProductSerial;

	[JsonIgnore]
	public bool HasSpecialMultiCurrency => (SpecialFacilities & EnLK_SpecialFacilities.MultiCurrency) == EnLK_SpecialFacilities.MultiCurrency;

	[JsonIgnore]
	public bool HasSpecialVoiceCall => (SpecialFacilities & EnLK_SpecialFacilities.VoiceCall) == EnLK_SpecialFacilities.VoiceCall;

	public bool IsOffline { get; set; }

	[JsonIgnore]
	public ReturnedState State
	{
		get
		{
			if (IsFailed)
			{
				return ReturnedState.Error;
			}
			int num = 0;
			if (ExpireDate > DateTime.Now)
			{
				num = checked((int)Math.Floor((ExpireDate - DateTime.Now).TotalDays));
			}
			if (num <= 30)
			{
				if (num > 0)
				{
					return ReturnedState.Warning;
				}
				return ReturnedState.Error;
			}
			return ReturnedState.Success;
		}
	}

	public event Func<object, GeneralReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebLkInfo>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebLkInfo>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebLkInfo>, Task> OnSync;

	public string DescriptionForTelegram()
	{
		string text = "";
		try
		{
			switch (TypeAccounting)
			{
			case ENCode.ForoshgahiSade:
				text += "\ud83d\udcb0حسابداری ساده";
				break;
			case ENCode.ForoshgahiMotavaset:
				text += "\ud83d\udcb0حسابدرای متوسط";
				break;
			case ENCode.ForoshgahiPishrafteh:
				text += "\ud83d\udcb0حسابدرای پیشرفته";
				break;
			case ENCode.Jame:
				text += "\ud83d\udcb0حسابداری جامع";
				break;
			}
			if (text.Length > 0)
			{
				text += "-";
			}
			switch (Hozor_Hoghogh)
			{
			case TypeHozor.Level1:
				text += "\ud83d\udd70حضور سطح1";
				break;
			case TypeHozor.Level2:
				text += "\ud83d\udd70حضور سطح2";
				break;
			case TypeHozor.Level3:
				text += "\ud83d\udd70حضور سطح3";
				break;
			case TypeHozor.Level4:
				text += "\ud83d\udd70حضور سطح4";
				break;
			case TypeHozor.Level5:
				text += "\ud83d\udd70حضور سطح5";
				break;
			}
			if (Scale)
			{
				text += "-⚖\ufe0fترازو";
			}
			if (BarCode)
			{
				text += "-\ud83d\udcb3بارکد";
			}
			if (Tolid)
			{
				text += "-\ud83c\udfedتولید";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DescriptionForTelegram", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 125);
		}
		return text;
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				GeneralReadEventArgs generalReadEventArgs = new GeneralReadEventArgs
				{
					Modified = modified
				};
				list.Add(generalReadEventArgs.ToString());
				return new Packet(PacketType.LkInfo, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 140);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebLkInfo> items, int partitionId, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
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
					result = items.GeneratePackets(PacketType.LkInfo, 6, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 154);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebLkInfo> titles, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
		{
			try
			{
				object result;
				if (titles == null)
				{
					result = null;
				}
				else
				{
					result = titles.GeneratePackets(PacketType.LkInfo, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 168);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.LkInfo, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 181);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebLkInfo> titles)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
		{
			try
			{
				return titles?.GeneratePackets(PacketType.LkInfo, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 195);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			LkInfoActions lkInfoActions = (LkInfoActions)action;
			return lkInfoActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 208);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((LkInfoActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 220);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
		{
			_ = 4;
			try
			{
				switch ((LkInfoActions)pack.PacketActions)
				{
				case LkInfoActions.SaveRequest:
				{
					ConnectionEventArgs<WebLkInfo> args3 = new ConnectionEventArgs<WebLkInfo>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs");
					break;
				}
				case LkInfoActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs");
					break;
				}
				case LkInfoActions.ReadRequest:
				{
					GeneralReadEventArgs generalReadEventArgs = pack.Args[1].FromJson<GeneralReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 241);
					generalReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					generalReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, generalReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs");
					break;
				}
				case LkInfoActions.ReadResponse:
				{
					ConnectionEventArgs<WebLkInfo> args = new ConnectionEventArgs<WebLkInfo>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs");
					}
					break;
				}
				default:
					WebErrorLog.Instance.StartLog($"Unrecognized packetAction for lkInfo({pack.PacketActions})", "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 256);
					break;
				case LkInfoActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 261);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 277);
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<IEnumerable<WebLkInfo>>>> WebLKApi(Guid tafsilGuid, CancellationToken token)
	{
		using (new JaegerMonitor("WebLKApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs"))
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<IEnumerable<WebLkInfo>>> ret = new ReturnedSaveFuncInfoWithValue<WebResponse<IEnumerable<WebLkInfo>>>();
			try
			{
				if (tafsilGuid == Guid.Empty)
				{
					return ret;
				}
				Dictionary<string, string> headers = new Dictionary<string, string> { 
				{
					"TafsilGuid",
					tafsilGuid.ToString()
				} };
				ReturnedSaveFuncInfoWithValue<WebResponse<WebResponse<IEnumerable<WebLkInfo>>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "LK/GetByTafsilGuid").GetFromApi<WebResponse<IEnumerable<WebLkInfo>>>(token, 3, headers);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "WebLKApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 289);
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "apiUrl:" + HttpHelpers.ApiUrl, "WebLKApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 294);
				ret.AddReturnedValue(ex, "", "WebLKApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 295);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebLkInfo>> GetByTaxClientIdFromApiAsync(string taxClientId, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebLkInfo> result = new ReturnedSaveFuncInfoWithValue<WebLkInfo>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { { "taxClientId", taxClientId } };
			ReturnedSaveFuncInfoWithValue<WebResponse<WebLkInfo>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "LkTaxClient/GetInfo").GetFromApi<WebLkInfo>(token, 3, headers);
			result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetByTaxClientIdFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 306);
			if (result.HasError)
			{
				return result;
			}
			result.value = returnedSaveFuncInfoWithValue.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByTaxClientIdFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 312);
			result.AddReturnedValue(ex, "", "GetByTaxClientIdFromApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebLkInfo.cs", 313);
		}
		return result;
	}
}
