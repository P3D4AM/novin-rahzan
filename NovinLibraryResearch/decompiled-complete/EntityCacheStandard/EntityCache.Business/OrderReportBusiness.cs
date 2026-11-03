using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace EntityCache.Business;

public class OrderReportBusiness : Serializable<OrderReportBusiness>, IOrder, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, ISyncFromClient2Server<OrderReportBusiness>, IDeliverableEntity, IEditableDelivery, IDeliverable, IHasContext, INotifyPropertyChanged, IDataItem, IEntityAttachment
{
	private CancellationTokenSource _updateStatusToken;

	private Guid? b1TafsilGuid;

	private Guid? b2TafsilGuid;

	private Guid? b3TafsilGuid;

	private Guid? b4TafsilGuid;

	private Guid? b1MoeinGuid;

	private Guid? b2MoeinGuid;

	private Guid? b3MoeinGuid;

	private Guid? b4MoeinGuid;

	private Guid? avarezTafsilGuid;

	private Guid? avarezMoeinGuid;

	private Guid? taxMoeinGuid;

	private Guid? taxTafsilGuid;

	private Guid? aghsatGuid;

	private Guid? hazineTafsilGuid1;

	private Guid? hazineTafsilGuid2;

	private Guid? hazineTafsilGuid3;

	private Guid? hazineTafsilGuid4;

	private Guid? hazineTafsilGuid5;

	private Guid? hazineMoeinGuid1;

	private Guid? hazineMoeinGuid2;

	private Guid? hazineMoeinGuid3;

	private Guid? hazineMoeinGuid4;

	private Guid? hazineMoeinGuid5;

	private Guid? fareTafsilGuid;

	private Guid? fareMoeinGuid;

	private Guid? pardakhtGuid;

	private Guid? receptionGuid;

	private string _customerFullName;

	private string _lastServerStatus = "";

	private decimal sood;

	public Guid UserAccGuid { get; set; }

	public decimal AfterAccount { get; set; }

	public Guid? AghsatGuid
	{
		get
		{
			return aghsatGuid;
		}
		set
		{
			aghsatGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public long AndroidImeLong { get; set; }

	public string AndroidIme { get; set; }

	public decimal Avarez { get; set; }

	public decimal B1 { get; set; }

	public Guid? B1MoeinGuid
	{
		get
		{
			return b1MoeinGuid;
		}
		set
		{
			b1MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B2MoeinGuid
	{
		get
		{
			return b2MoeinGuid;
		}
		set
		{
			b2MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B3MoeinGuid
	{
		get
		{
			return b3MoeinGuid;
		}
		set
		{
			b3MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B4MoeinGuid
	{
		get
		{
			return b4MoeinGuid;
		}
		set
		{
			b4MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B1TafsilGuid
	{
		get
		{
			return b1TafsilGuid;
		}
		set
		{
			b1TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B2TafsilGuid
	{
		get
		{
			return b2TafsilGuid;
		}
		set
		{
			b2TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B3TafsilGuid
	{
		get
		{
			return b3TafsilGuid;
		}
		set
		{
			b3TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B4TafsilGuid
	{
		get
		{
			return b4TafsilGuid;
		}
		set
		{
			b4TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public decimal B2 { get; set; }

	public decimal B3 { get; set; }

	public decimal B4 { get; set; }

	public string BarnameNo { get; set; }

	public string BazaryabFullName { get; set; }

	public decimal BeforeAccount { get; set; }

	public string CpuSerial { get; set; }

	public string CustomerMotefaregheName { get; set; }

	public string CustomerFullName
	{
		get
		{
			return _customerFullName;
		}
		set
		{
			_customerFullName = value.FixString();
		}
	}

	public decimal TafsilAccount { get; set; }

	public decimal TafsilAccountR => Math.Abs(TafsilAccount);

	public Guid CustomerGroupGuid { get; set; } = Guid.Empty;


	public DateTime Date { get; set; }

	public DateTime DateSabt { get; set; }

	public string DateSh { get; set; }

	public string Description { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountRows { get; set; }

	public Document FactorType { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string FactorTypeForDisplay => FactorType.GetDisplay();

	public short DailyFactorNo { get; set; }

	public Guid? HazineTafsilGuid1
	{
		get
		{
			return hazineTafsilGuid1;
		}
		set
		{
			hazineTafsilGuid1 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid2
	{
		get
		{
			return hazineTafsilGuid2;
		}
		set
		{
			hazineTafsilGuid2 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid3
	{
		get
		{
			return hazineTafsilGuid3;
		}
		set
		{
			hazineTafsilGuid3 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid4
	{
		get
		{
			return hazineTafsilGuid4;
		}
		set
		{
			hazineTafsilGuid4 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid5
	{
		get
		{
			return hazineTafsilGuid5;
		}
		set
		{
			hazineTafsilGuid5 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid1
	{
		get
		{
			return hazineMoeinGuid1;
		}
		set
		{
			hazineMoeinGuid1 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid2
	{
		get
		{
			return hazineMoeinGuid2;
		}
		set
		{
			hazineMoeinGuid2 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid3
	{
		get
		{
			return hazineMoeinGuid3;
		}
		set
		{
			hazineMoeinGuid3 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid4
	{
		get
		{
			return hazineMoeinGuid4;
		}
		set
		{
			hazineMoeinGuid4 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid5
	{
		get
		{
			return hazineMoeinGuid5;
		}
		set
		{
			hazineMoeinGuid5 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public string HddSerial { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public int Id2 { get; set; }

	public string Ip { get; set; }

	public decimal Keraye { get; set; }

	public Guid? FareMoeinGuid
	{
		get
		{
			return fareMoeinGuid;
		}
		set
		{
			fareMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? FareTafsilGuid
	{
		get
		{
			return fareTafsilGuid;
		}
		set
		{
			fareTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public string LkSerial { get; set; }

	public Guid? PardakhtGuid
	{
		get
		{
			return pardakhtGuid;
		}
		set
		{
			pardakhtGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? PriceTypeGuid { get; set; }

	public short PrintCount { get; set; }

	public DateTime? PrintDate { get; set; }

	public Guid? ReceptionGuid
	{
		get
		{
			return receptionGuid;
		}
		set
		{
			receptionGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public string HostRefrenced { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public ENSource Source { get; set; }

	public decimal Sum { get; set; }

	public decimal SumKala { get; set; }

	public decimal SumService { get; set; }

	public int TasvieDay { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxImpact { get; set; }

	public decimal TotalBazaryab => B1 + B2 + B3 + B4;

	public decimal TotalDiscount { get; set; }

	public decimal TotalHazine { get; set; }

	public decimal TotalHazine1 { get; set; }

	public decimal TotalHazine2 { get; set; }

	public decimal TotalHazine3 { get; set; }

	public decimal TotalHazine4 { get; set; }

	public decimal TotalHazine5 { get; set; }

	public decimal SumWithHazine => TotalHazine + TotalSum;

	public decimal TotalSum { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public bool? DeliveryStatus { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public bool IsTemp { get; set; }

	public Guid? AvarezTafsilGuid
	{
		get
		{
			return avarezTafsilGuid;
		}
		set
		{
			avarezTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? AvarezMoeinGuid
	{
		get
		{
			return avarezMoeinGuid;
		}
		set
		{
			avarezMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? TaxTafsilGuid
	{
		get
		{
			return taxTafsilGuid;
		}
		set
		{
			taxTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? TaxMoeinGuid
	{
		get
		{
			return taxMoeinGuid;
		}
		set
		{
			taxMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid Guid { get; set; }

	public int Id { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string SellerFactorNo { get; set; }

	[JsonIgnore]
	public decimal Sood
	{
		get
		{
			return sood;
		}
		set
		{
			sood = value;
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					handler(this, new PropertyChangedEventArgs("Sood"));
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 139);
				}
			}, "Sood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs");
		}
	}

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public string SendToServerStatus => ServerSt();

	public ServerStatusForClient ServerStatus { get; set; }

	public List<WebTableRelationShip> LinkedSanad { get; set; }

	public int AttachmentCount => LinkedSanad?.Count ?? 0;

	public string CustName { get; set; }

	public string CustFamily { get; set; }

	public string CustMobile { get; set; }

	public string CustAddress { get; set; }

	public string CustZipCode { get; set; }

	public string CustTell { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	[JsonIgnore]
	public string DeliveryShortMessageForAndroid
	{
		get
		{
			switch (ServerStatus)
			{
			case ServerStatusForClient.None:
				if (IsTemp)
				{
					return "موقت";
				}
				return "درحال ارسال...";
			case ServerStatusForClient.Sent:
				return ServerSt();
			case ServerStatusForClient.Delivered:
				return ServerSt();
			case ServerStatusForClient.SendError:
				return ServerSt() + "\r\n" + DeliveryShortMessage;
			case ServerStatusForClient.DeliveryError:
				return ServerSt() + "\r\n" + DeliveryShortMessage;
			case ServerStatusForClient.DirectDelivery:
				return ServerSt();
			default:
				return ServerSt();
			}
		}
	}

	public int StatusForOrdering
	{
		get
		{
			switch (ServerStatus)
			{
			case ServerStatusForClient.Sent:
				return 5;
			case ServerStatusForClient.Delivered:
				return 7;
			case ServerStatusForClient.SendError:
				return 1;
			case ServerStatusForClient.DeliveryError:
				return 2;
			case ServerStatusForClient.DirectDelivery:
				return 6;
			case ServerStatusForClient.LinkToOrder:
				return 10;
			default:
				if (!IsTemp)
				{
					return 4;
				}
				return 3;
			}
		}
	}

	public string ContextGuid { get; set; }

	public byte[] SourceImage
	{
		get
		{
			if (Source == ENSource.Baje)
			{
				return ImageResourceManager.SourceBaje;
			}
			if (Source == ENSource.Android_HesabYar)
			{
				return ImageResourceManager.SourceHesabYar;
			}
			if (Source == ENSource.CrmPortal)
			{
				return ImageResourceManager.SourceCrm;
			}
			if (Source == ENSource.Android_SefareshGir)
			{
				return ImageResourceManager.SourceSefareshGir;
			}
			if (Source == ENSource.WebsiteWordPress || Source == ENSource.WebSite)
			{
				return ImageResourceManager.SourceWordPress;
			}
			if (Source == ENSource.Department)
			{
				return ImageResourceManager.SourceDepartment;
			}
			if (Source == ENSource.RestApi)
			{
				return ImageResourceManager.SourceRestApi;
			}
			return ImageResourceManager.SourceAccounting;
		}
	}

	public byte[] ServerStatusImage
	{
		get
		{
			ServerStatusForClient serverStatus = ServerStatus;
			if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
			{
				return ImageResourceManager.ServerStatusDelivered;
			}
			if (ServerStatus == ServerStatusForClient.DeliveryError)
			{
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			if (ServerStatus == ServerStatusForClient.Sent)
			{
				return ImageResourceManager.ServerStatusSent;
			}
			if (ServerStatus == ServerStatusForClient.SendError)
			{
				return ImageResourceManager.ServerStatusSentError;
			}
			return ImageResourceManager.ServerStatusNone;
		}
	}

	public byte[] SanadStatusImage
	{
		get
		{
			if (SanadSt != SanadSt.Daemi)
			{
				return ImageResourceManager.SanadStatusMovaghat;
			}
			return ImageResourceManager.SanadStatusDaemi;
		}
	}

	public byte[] HideMoreDetail => ImageResourceManager.HideMoreDetail;

	public byte[] ShowMoreDetail => ImageResourceManager.ShowMoreDetail;

	public bool CanSend2Server
	{
		get
		{
			switch (ServerStatus)
			{
			case ServerStatusForClient.None:
				return IsTemp;
			case ServerStatusForClient.SendError:
			case ServerStatusForClient.DeliveryError:
				return true;
			default:
				return false;
			}
		}
	}

	public DateTime? TransferDate { get; set; }

	public Guid ProductMoeinGuid { get; set; }

	public Guid? ProductTafsilGuid { get; set; }

	public Guid DiscountMoeinGuid { get; set; }

	public Guid? DiscountTafsilGuid { get; set; }

	public int? OrderSellReminder
	{
		get
		{
			try
			{
				if (FactorType != Document.OrderSell)
				{
					return null;
				}
				if (TasvieDay <= 0)
				{
					return null;
				}
				if (Sum == AttachmentSum)
				{
					return null;
				}
				return (Date.AddDays(TasvieDay) - DateTime.Now).Days;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OrderSellReminder", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 257);
			}
			return null;
		}
	}

	public int AttachmentNo { get; set; }

	public bool TemplateStatus { get; set; }

	public byte[] TemplateStatusImage
	{
		get
		{
			if (!TemplateStatus)
			{
				return ImageResourceManager.OutOfTemplate;
			}
			return ImageResourceManager.InTemplate;
		}
	}

	public short StatusId { get; set; }

	public Guid? IntroductionMethodGuid { get; set; }

	public string IntroductionMethodName { get; set; }

	public decimal TotalSumWithHazine => TotalSum + TotalHazine;

	public bool SanadAffective { get; set; }

	public bool KardexAffective { get; set; }

	public EnTaxResult TaxResult { get; set; }

	public string TaxRefrenceNumber { get; set; }

	public string TaxResultErrorCode { get; set; }

	public string TaxResultMessage { get; set; }

	public string TaxUid { get; set; }

	public DateTime? TaxSendDate { get; set; }

	public byte[] TaxStatusImage => TaxResult switch
	{
		EnTaxResult.None => ImageResourceManager.TaxNotSentImage, 
		EnTaxResult.Sent => ImageResourceManager.TaxPendingImage, 
		EnTaxResult.Accepted => ImageResourceManager.TaxAcceptedImage, 
		EnTaxResult.Failed => ImageResourceManager.TaxFailedImage, 
		_ => throw new NotImplementedException(), 
	};

	public string TaxOrderId { get; set; }

	public DateTime? TasvieDate { get; set; }

	public DateTime? TasvieDateMoaser { get; set; }

	public string TasvieDateMoaserSh => TasvieDateMoaser.MiladiToCalture();

	public string TafsilName => CustomerFullName;

	public decimal AttachmentSum { get; set; }

	public DateTime? DateUtc { get; set; }

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public Guid? CashBackCouponGuid { get; set; }

	public bool IsCashBackProgressed { get; set; }

	public Guid? ServiceMoeinGuid { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public string AutoTitle(ENAction act)
	{
		return "";
	}

	public OrderReportBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 309);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendAsync(CancellationToken token, Priority priority = Priority.Low)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			line = 1;
			OrderBusiness obj = await OrderBusiness.GetAsync(ContextGuid, Guid, FactorType, fromDatabase: false, token);
			line = 2;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(EnSystem.UserInterface, awaitOnSemaphore: true, sendToServer: true, isTemp: false, 10, "", "", "", 60000, await4sent: true, token), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 321);
			line = 3;
			_updateStatusToken?.Cancel();
			_updateStatusToken = new CancellationTokenSource();
			CancellationToken token2 = _updateStatusToken.Token;
			line = 4;
			UpdateStatusAsync(token2);
			line = 5;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"The Line Error Is :{line}", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 333);
			ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 334);
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			OrderBusiness orderBusiness = await OrderBusiness.GetAsync(ContextGuid, Guid, FactorType, fromDatabase: true, token, tranName);
			if (orderBusiness != null)
			{
				OrderRemoveFilter filter = new OrderRemoveFilter
				{
					IsDeleteAghsat = true,
					IsDeletePardakht = true,
					IsDeleteReception = true,
					IsDeleteTolid = true
				};
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await orderBusiness.RemoveAsync(system, filter, 10, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 355);
			}
			ret.throwExceptionIfError();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 361);
			ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 362);
		}
		return ret;
	}

	public string ServerSt(string tranName = "")
	{
		try
		{
			if (!string.IsNullOrEmpty(_lastServerStatus))
			{
				return _lastServerStatus;
			}
			_updateStatusToken?.Cancel();
			_updateStatusToken = new CancellationTokenSource();
			CancellationToken token = _updateStatusToken.Token;
			switch (ServerStatus)
			{
			case ServerStatusForClient.None:
				_lastServerStatus = (IsTemp ? "موقت" : "در حال ارسال..");
				break;
			case ServerStatusForClient.Sent:
				_lastServerStatus = ServerStatus.GetDisplay() + "(" + HostRefrenced + ")";
				break;
			case ServerStatusForClient.Delivered:
				_lastServerStatus = ServerStatus.GetDisplay() + "(" + HostRefrenced + ")";
				break;
			case ServerStatusForClient.SendError:
				_lastServerStatus = ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DeliveryError:
				_lastServerStatus = ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DirectDelivery:
				_lastServerStatus = $"{ServerStatus.GetDisplay()}({DeliveryId})";
				break;
			default:
				_lastServerStatus = $"خطا در تشخیص{ServerStatus}";
				break;
			}
			UpdateStatusAsync(token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ServerSt", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 390);
		}
		return _lastServerStatus;
	}

	private async Task UpdateStatusAsync(CancellationToken token)
	{
		try
		{
			await Task.Delay(new Random().Next(200));
			string _currentStatus = _lastServerStatus;
			switch (ServerStatus)
			{
			case ServerStatusForClient.None:
				if (IsTemp)
				{
					_lastServerStatus = "موقت";
				}
				else if (await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.CustomerIsSend(TafsilGuid, token, ""))
				{
					_lastServerStatus = "در انتظار ارسال طرف حساب";
				}
				_lastServerStatus = "در حال ارسال..";
				break;
			case ServerStatusForClient.Sent:
				_lastServerStatus = ServerStatus.GetDisplay() + "(" + HostRefrenced + ")";
				break;
			case ServerStatusForClient.Delivered:
				_lastServerStatus = ServerStatus.GetDisplay() + "(" + HostRefrenced + ")";
				break;
			case ServerStatusForClient.SendError:
				_lastServerStatus = ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DeliveryError:
				_lastServerStatus = ServerStatus.GetDisplay();
				break;
			case ServerStatusForClient.DirectDelivery:
				_lastServerStatus = $"{ServerStatus.GetDisplay()}({DeliveryId})";
				break;
			default:
				_lastServerStatus = $"خطا در تشخیص{ServerStatus}";
				break;
			}
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if (handler == null || !(_currentStatus != _lastServerStatus) || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					handler(this, new PropertyChangedEventArgs("SendToServerStatus"));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 420);
				}
			}, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 422);
		}
	}

	public string TotalBarcode(bool BarcodeTotalEnable, string format, string Contorol, double ZaribRial, string cashierCode, string tranName)
	{
		return new BarcodeCreator(format, Contorol, ZaribRial, Id, Id2, Sum, Date, cashierCode, 0, DigitAfterPoint).TotalBarcode();
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 435), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 435);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).BusinessAggregator.OrderBusiness.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 437);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 441);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 442);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 447), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 447);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			try
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 459), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 459);
				}
				transactionHasBeenCreated = true;
				OrderBusiness orderBusiness = await OrderBusiness.GetAsync(ContextGuid, Guid, FactorType, fromDatabase: true, token);
				if (orderBusiness != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await orderBusiness.SetSaveResult(EnSystem.SignalR, item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 462);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 466);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 467);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 472), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderReportBusiness.cs", 472);
			}
		}
		return ret;
	}

	public async Task<OrderBusiness> GetOrderAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await OrderBusiness.GetAsync(ContextGuid, Guid, FactorType, fromDatabase: true, token, tranName);
	}
}
