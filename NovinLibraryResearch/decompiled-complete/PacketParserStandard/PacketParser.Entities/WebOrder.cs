using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using PacketParser.EventArgs.TaxService;
using PacketParser.Records.TaxService;

namespace PacketParser.Entities;

[Serializable]
public class WebOrder : Serializable<WebOrder>, IOrder, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IReceiver, INpDisposable, IDisposable
{
	private Guid _CashierAccGuid;

	private DateTime? _deliveryDate;

	private Document _factorType = Document.OrderSell;

	private static string apiUrl = HttpHelpers.ApiUrl + "api/Order/";

	public WooOrder WooOrder { get; set; } = new WooOrder();


	public string WpRefrencedId { get; set; }

	public string HostRefrenced { get; set; }

	public Guid ProductMoeinGuid { get; set; }

	[Obsolete("Use OrderDetail", true)]
	public Guid? ProductTafsilGuid { get; set; }

	public List<WebSanad_Det> ListSanad { get; set; }

	public List<WebTableRelationShip> RelationList { get; set; }

	public EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	[Obsolete("Use ChangeStatus!", true)]
	public bool IsAdd { get; set; }

	public int AttachmentNo { get; set; }

	public string BuyFactorNumber { get; set; }

	public OrderMetaData MetaData { get; set; }

	public Guid? IntroductionMethodGuid { get; set; }

	public List<WebOrderDetail> ListKala { get; set; }

	public int Id { get; set; }

	public DateTime? TransferDate { get; set; }

	public Guid UserAccGuid
	{
		get
		{
			return _CashierAccGuid;
		}
		set
		{
			_CashierAccGuid = value;
			if (Pardakht != null)
			{
				Pardakht.UserGuid = value;
			}
			if (Reception != null)
			{
				Reception.UserGuid = value;
			}
		}
	}

	public short StatusId { get; set; }

	public int MoeinId { get; set; }

	public int Id2 { get; set; }

	public DateTime Date { get; set; } = DateTime.Now;


	[JsonIgnore]
	[XmlIgnore]
	public string Time => Date.TimeOfDay.ToString("hh\\:mm\\:ss");

	public decimal Discount { get; set; }

	public string Description { get; set; }

	public decimal Keraye { get; set; }

	public Document FactorType
	{
		get
		{
			return _factorType;
		}
		set
		{
			switch (_factorType)
			{
			case Document.OrderSell:
			case Document.OrderBuy:
			case Document.OrderBackBuy:
			case Document.OrderBackSell:
			case Document.PishFactor:
			case Document.Zayeat:
			case Document.AmaniVorod:
			case Document.AmaniKhoroj:
			case Document.OrderPreBuy:
				_factorType = value;
				break;
			default:
				throw new ArgumentException($"Value({value}) for FactorType is not valid!");
			}
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string FactorTypeFa => FactorType.GetDisplay();

	public Guid? PriceTypeGuid { get; set; }

	public Guid Guid { get; set; } = Guid.Empty;


	public Guid? ReceptionGuid { get; set; }

	public Guid? AghsatGuid { get; set; }

	public int SanadNumber => SanadNo;

	public SanadSt SanadSt { get; set; }

	public int SanadNo { get; set; }

	public int B4MoeinId { get; set; }

	public string BarnameNo { get; set; }

	public short PrintCount { get; set; }

	public DateTime? PrintDate { get; set; }

	public virtual decimal TotalBazaryab => B1 + B2 + B3 + B4;

	public string BazaryabFullName { get; set; }

	public string CustomerFullName { get; set; }

	public decimal SumKala => ListKala?.Sum((WebOrderDetail p) => p?.RowSum ?? 0m) ?? 0m;

	public decimal SumService { get; set; }

	public decimal SumQuantity => ListKala?.Sum((WebOrderDetail p) => p?.Quantity ?? 0m) ?? 0m;

	public decimal Sum => SumKala - Discount + Keraye;

	public decimal TotalSum => Sum + Avarez + Tax;

	public virtual decimal B1 { get; set; }

	public int B1TafsilId { get; set; }

	public int B1MoeinId { get; set; }

	public virtual decimal B2 { get; set; }

	public int B2TafsilId { get; set; }

	public int B2MoeinId { get; set; }

	public virtual decimal B3 { get; set; }

	public int B3TafsilId { get; set; }

	public int B3MoeinId { get; set; }

	public virtual decimal B4 { get; set; }

	public int B4TafsilId { get; set; }

	public int TasvieDay { get; set; }

	[JsonIgnore]
	public string DateSh => Date.MiladiToCalture();

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public int FishNo { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxImpact { get; set; }

	public int TaxHTreeId { get; set; }

	public Guid? TaxTafsilGuid { get; set; }

	public Guid? TaxMoeinGuid { get; set; }

	public int AvarezHTreeId { get; set; }

	public decimal Avarez { get; set; }

	public int SendPcCount { get; set; }

	public ENSource Source { get; set; }

	public decimal TotalWeight => ListKala?.Sum((WebOrderDetail q) => q.SerialzedData?.NetWeight ?? 0m) ?? 0m;

	[JsonIgnore]
	[XmlIgnore]
	public string SourceFa => Source.GetDisplay();

	public string LkSerial { get; set; }

	public string HddSerial { get; set; }

	public int TafsilId { get; set; }

	public Guid TafsilGuid { get; set; } = Guid.Empty;


	public string CpuSerial { get; set; }

	public DateTime DateSabt { get; set; }

	public DateTime DateSabtOnNpServer { get; set; }

	public long AndroidImeLong { get; set; }

	public string AndroidIme { get; set; }

	public string Ip { get; set; }

	public short DailyFactorNo { get; set; }

	public decimal TotalHazine1 { get; set; }

	public decimal TotalHazine2 { get; set; }

	public decimal TotalHazine3 { get; set; }

	public decimal TotalHazine4 { get; set; }

	public decimal TotalHazine5 { get; set; }

	public decimal TotalHazine { get; set; }

	public int ReferencedId { get; set; }

	public decimal AghlamSum { get; set; }

	public decimal TotalDiscount { get; set; }

	public int PardakhtId { get; }

	public Guid? PardakhtGuid { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal PardakhtSum => Pardakht?.Sum ?? 0m;

	public int ReceptionId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal ReceptionSum => Reception?.Sum ?? 0m;

	public decimal BeforeAccount { get; set; }

	public decimal AfterAccount { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal DiscountRows => ListKala?.Sum((WebOrderDetail p) => p?.Discount ?? 0m) ?? 0m;

	public WebPardakht Pardakht { get; set; }

	public WebReception Reception { get; set; }

	public List<WebGardeshTafsili> SanadArr { get; set; }

	public int FareMoeinId { get; set; }

	public int FareTafsilId { get; set; }

	public DateTime? DeliveryDate
	{
		get
		{
			if (DeliveryStatus.HasValue)
			{
				return _deliveryDate;
			}
			return null;
		}
		set
		{
			_deliveryDate = value;
		}
	}

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

	public bool IsTemp { get; set; }

	public Guid? FareTafsilGuid { get; set; }

	public Guid? FareMoeinGuid { get; set; }

	public Guid? B1TafsilGuid { get; set; }

	public Guid? B2TafsilGuid { get; set; }

	public Guid? B3TafsilGuid { get; set; }

	public Guid? B4TafsilGuid { get; set; }

	public Guid? B1MoeinGuid { get; set; }

	public Guid? B2MoeinGuid { get; set; }

	public Guid? B3MoeinGuid { get; set; }

	public Guid? B4MoeinGuid { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public WebCus CusInfo { get; set; }

	public string CustName { get; set; }

	public string CustFamily { get; set; }

	public string CustMobile { get; set; }

	public string CustAddress { get; set; }

	public string CustZipCode { get; set; }

	public string TaxCustZipCode { get; set; }

	public string CustTell { get; set; }

	public Guid? HazineTafsilGuid1 { get; set; }

	public Guid? HazineTafsilGuid2 { get; set; }

	public Guid? HazineTafsilGuid3 { get; set; }

	public Guid? HazineTafsilGuid4 { get; set; }

	public Guid? HazineTafsilGuid5 { get; set; }

	public Guid? HazineMoeinGuid1 { get; set; }

	public Guid? HazineMoeinGuid2 { get; set; }

	public Guid? HazineMoeinGuid3 { get; set; }

	public Guid? HazineMoeinGuid4 { get; set; }

	public Guid? HazineMoeinGuid5 { get; set; }

	public Guid? AvarezTafsilGuid { get; set; }

	public Guid? AvarezMoeinGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	public string SellerFactorNo { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string PaymentMethodFa => PaymentMethod.GetDisplay();

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public List<Guid> LinkedFiles { get; set; }

	public List<WebTableRelationShip> LinkedSanad { get; set; }

	public WebTableRelationShip LinkedSanads { get; set; }

	public Guid DiscountMoeinGuid { get; set; }

	public Guid? DiscountTafsilGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DateUtc { get; set; }

	public bool? SanadAffective { get; set; }

	public bool? KardexAffective { get; set; }

	public Guid TaxRefrencedGuid { get; set; } = Guid.NewGuid();


	public EnTaxBillType TaxBillType { get; set; }

	public string? BaseTaxId { get; set; }

	public string CustomerNationalCode { get; set; }

	public string SellerEconomicNumber { get; set; }

	public string CustomerEconomicNumber { get; set; }

	public CustomerType CustomerType { get; set; }

	public DateTime? TasvieDate { get; set; }

	public DateTime? TasvieDateMoaser { get; set; }

	public EnTaxInvoiceSubject? TaxSubject { get; set; }

	public EnTaxInvoiceTemplate? Template { get; set; }

	public string ContractUniqueId { get; set; }

	public string CustomerBrachCode { get; set; }

	public string SellerBrachCode { get; set; }

	public DateTime? TaxBillingDate { get; set; }

	public string CustomsLicenseNumber { get; set; }

	public string CustomsCode { get; set; }

	public DateTime? CottageDate { get; set; }

	public string CottageNumber { get; set; }

	public EnTaxTasvieMethod? TaxTasvieMethod { get; set; }

	public Guid? CashBackCouponGuid { get; set; }

	public bool IsCashBackProgressed { get; set; }

	public Guid? ServiceMoeinGuid { get; set; }

	[JsonIgnore]
	public bool HasDiscount => ListKala.Sum((WebOrderDetail x) => x.Discount) != 0m;

	[JsonIgnore]
	public bool HasAvarez => ListKala.Sum((WebOrderDetail x) => x.Avarez) != 0m;

	[JsonIgnore]
	public bool HasTax => ListKala.Sum((WebOrderDetail x) => x.Tax) != 0m;

	[JsonIgnore]
	public bool HasDescription => Description != "";

	[JsonIgnore]
	public string DiscountPercentForDisplay => ("(%" + ListKala.Sum((WebOrderDetail x) => x.DiscountPercent) + ")").Replace(".", "/") + " " + ListKala.Sum((WebOrderDetail x) => x.Discount).NpToString("N0", 0).Replace(".", "/");

	[JsonIgnore]
	public string TaxPercentForDisplay => ("(%" + ListKala.Sum((WebOrderDetail x) => x.TaxPercent) + ")").Replace(".", "/") + " " + ListKala.Sum((WebOrderDetail x) => x.Tax).NpToString("N0", 0).Replace(".", "/");

	[JsonIgnore]
	public string AvarezPercentForDisplay => ListKala.Sum((WebOrderDetail x) => x.AvarezPercent) + "%";

	[JsonIgnore]
	public string DiscountForDisplay => (ListKala.Sum((WebOrderDetail x) => x.Discount) + Discount).NpToString("N0", 0).Replace(".", "/");

	[JsonIgnore]
	public string TaxForDisplay => ListKala.Sum((WebOrderDetail x) => x.Tax).NpToString("N0", 0).Replace(".", "/");

	[JsonIgnore]
	public string AvarezForDisplay => ListKala.Sum((WebOrderDetail x) => x.Avarez).NpToString("N0", 0);

	[JsonIgnore]
	public string TotalSumForDisplay => TotalSum.NpToString("N0", 0);

	[JsonIgnore]
	public string AghlamSumForDisplay => AghlamSum.NpToString("N0", 0);

	public event Func<object, ConnectionEventArgs<TaxInvoiceTrackingEventArgs>, Task> OnTaxTrackSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnTaxTrackSaveResponse;

	public event Func<object, ConnectionEventArgs<WebOrder>, Task> OnSaveRequest;

	public event Func<object, ConnectionEventArgs<WebOrder>, Task> OnSaveTempRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveTempResponse;

	public event Func<object, ReadOperationEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebOrder>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<Tuple<string, WebOrder>>, Task> OnSendToTaxServiceRequest;

	public event Func<object, TaxInvoiceResultEventArgs, Task> OnSendToTaxServiceResponse;

	public event Func<object, ConnectionEventArgs<Tuple<string, List<InvoiceTrackingEventArgs>>>, Task> OnTaxServiceTrackRequest;

	public event Func<object, List<TaxInvoiceTrackingEventArgs>, Task> OnTaxServiceTrackResponse;

	public event Func<object, ConnectionEventArgs<TaxEconomicCodeRecord>, Task> OnTaxEconomicCodeStateRequest;

	public event Func<object, TaxEconomicCodeRecord, Task> OnTaxEconomicCodeStateResponse;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebOrder> item)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null || !item.Any())
				{
					throw new NullReferenceException();
				}
				foreach (WebOrder item2 in item)
				{
					if ((item2.ReceptionGuid ?? Guid.Empty) != Guid.Empty && item2.Reception != null && item2.Reception.Guid == Guid.Empty)
					{
						item2.Reception.Guid = item2.ReceptionGuid.Value;
					}
					if (item2.Reception != null && item2.Reception.Guid == Guid.Empty)
					{
						item2.Reception.Guid = Guid.NewGuid();
					}
					if ((item2.ReceptionGuid ?? Guid.Empty) == Guid.Empty && item2.Reception != null && item2.Reception.Guid != Guid.Empty)
					{
						item2.ReceptionGuid = item2.Reception.Guid;
					}
					if ((item2.PardakhtGuid ?? Guid.Empty) != Guid.Empty && item2.Pardakht != null && item2.Pardakht.Guid == Guid.Empty)
					{
						item2.Pardakht.Guid = item2.PardakhtGuid.Value;
					}
					if (item2.Pardakht != null && item2.Pardakht.Guid == Guid.Empty)
					{
						item2.Pardakht.Guid = Guid.NewGuid();
					}
					if ((item2.PardakhtGuid ?? Guid.Empty) == Guid.Empty && item2.Pardakht != null && item2.Pardakht.Guid != Guid.Empty)
					{
						item2.PardakhtGuid = item2.Pardakht.Guid;
					}
				}
				return item?.GeneratePackets(PacketType.PishFactor, 1, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 334);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveTempRequest(List<WebOrder> lst)
	{
		using (new JaegerMonitor("GenerateSaveTempRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (lst == null || !lst.Any())
				{
					throw new NullReferenceException();
				}
				return lst.GeneratePackets(PacketType.PishFactor, 3, null, isSync: false, null, 0, deliveryRequst: true, "GenerateSaveTempRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveTempRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 349);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(List<ReadStatus> status, bool ChangeReadST, short Top)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				List<string> list = new List<string>
				{
					((short)5).ToString(),
					new ReadOperationEventArgs
					{
						Status = status,
						ChangeReadST = ChangeReadST,
						Top = Top
					}.ToString()
				};
				return new Packet(PacketType.PishFactor, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 374);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(int HostID, Guid guid, DateTime Date, string Shortmessage, string message, bool SaveResultStatus)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				Guid.NewGuid();
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
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 397);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> lst)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (lst == null || (lst != null && lst.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return lst?.GeneratePackets(PacketType.PishFactor, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 412);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveTempResponse(IEnumerable<SaveResponse> lst)
	{
		using (new JaegerMonitor("GenerateSaveTempResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				return lst?.GeneratePackets(PacketType.PishFactor, 4, null, isSync: false, null, 0, deliveryRequst: true, "GenerateSaveTempResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveTempResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 426);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebOrder> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (items == null || (items != null && items.Count() == 0))
				{
					throw new NullReferenceException();
				}
				if (items != null && items.Where((WebOrder x) => x.HostRefrenced.RemoveNoNumbers().Trim() == "")?.Any() == false)
				{
					WebErrorLog.Instance.StartLog(new Exception($"Order RefrencedId not set data:{items}"), "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 438);
				}
				return items?.GeneratePackets(PacketType.PishFactor, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 443);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebOrder> orders)
	{
		return orders?.GeneratePackets(PacketType.PishFactor, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
	}

	public static Packet GenerateSendToTaxServiceRequest(Tuple<string, WebOrder> item)
	{
		using (new JaegerMonitor("GenerateSendToTaxServiceRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxSendOrder, 8, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSendToTaxServiceRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSendToTaxServiceRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 458);
				throw;
			}
		}
	}

	public static Packet GenerateSendToTaxServiceResponse(TaxInvoiceResultEventArgs item)
	{
		using (new JaegerMonitor("GenerateSendToTaxServiceResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxSendOrder, 9, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSendToTaxServiceResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSendToTaxServiceResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 472);
				throw;
			}
		}
	}

	public static Packet GenerateTaxTrackingRequest(Tuple<string, List<InvoiceTrackingEventArgs>> item)
	{
		using (new JaegerMonitor("GenerateTaxTrackingRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxTrackingOrder, 10, null, isSync: false, null, 0, deliveryRequst: false, "GenerateTaxTrackingRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTaxTrackingRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 486);
				throw;
			}
		}
	}

	public static Packet GenerateTaxTrackingSaveRequest(TaxInvoiceTrackingEventArgs item)
	{
		using (new JaegerMonitor("GenerateTaxTrackingSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxTrackingOrder, 12, null, isSync: false, null, 0, deliveryRequst: false, "GenerateTaxTrackingSaveRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTaxTrackingSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 500);
				throw;
			}
		}
	}

	public static Packet GenerateTaxTrackingSaveResponse(SaveResponseEventArgs saveResponse)
	{
		using (new JaegerMonitor("GenerateTaxTrackingSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (saveResponse == null)
				{
					throw new NullReferenceException();
				}
				return saveResponse.GeneratePacket(PacketType.TaxTrackingOrder, 13, null, isSync: false, null, 0, deliveryRequst: false, "GenerateTaxTrackingSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTaxTrackingSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 514);
				throw;
			}
		}
	}

	public static Packet GenerateTaxTrackingResponse(List<TaxInvoiceTrackingEventArgs> item)
	{
		using (new JaegerMonitor("GenerateTaxTrackingResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxTrackingOrder, 11, null, isSync: false, null, 0, deliveryRequst: false, "GenerateTaxTrackingResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTaxTrackingResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 528);
				throw;
			}
		}
	}

	public static Packet GenerateTaxEconomicCodeStateRequest(TaxEconomicCodeRecord item)
	{
		using (new JaegerMonitor("GenerateTaxEconomicCodeStateRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxEconomicCodeState, 14, null, isSync: false, null, 0, deliveryRequst: false, "GenerateTaxEconomicCodeStateRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTaxEconomicCodeStateRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 542);
				throw;
			}
		}
	}

	public static Packet GenerateTaxEconomicCodeStateResponse(TaxEconomicCodeRecord item)
	{
		using (new JaegerMonitor("GenerateTaxEconomicCodeStateResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (item == null)
				{
					throw new NullReferenceException();
				}
				return item.GeneratePacket(PacketType.TaxEconomicCodeState, 15, null, isSync: false, null, 0, deliveryRequst: false, "GenerateTaxEconomicCodeStateResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateTaxEconomicCodeStateResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 556);
				throw;
			}
		}
	}

	public virtual ReturnedSaveFuncInfo CheckValidation()
	{
		using (new JaegerMonitor("CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				_ = Guid;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				if (Date < new DateTime(2010, 1, 1) || Date > new DateTime(2099, 1, 1))
				{
					returnedSaveFuncInfo.AddError("تاریخ وارد شده معتبر نمی باشد Date:" + Date, Date.Date.ToString(), "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 569);
				}
				if (ListKala != null)
				{
					List<WebOrderDetail> listKala = ListKala;
					if (listKala == null || listKala.Count != 0)
					{
						goto IL_00f6;
					}
				}
				returnedSaveFuncInfo.AddError("عدم امکان ثبت فاکتور فاقد اقلام - وب سرویس ثبت فاکتور", "ListKala", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 570);
				goto IL_00f6;
				IL_00f6:
				if (TafsilId <= 0 && TafsilGuid == Guid.Empty)
				{
					returnedSaveFuncInfo.AddError("ردیف طرف حساب تفصیلی فاکتور فروش انتخاب شده معتبر نمی باشد", TafsilGuid.ToString(), "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 571);
				}
				if (ListKala != null)
				{
					foreach (WebOrderDetail item in ListKala)
					{
						if (item.DepotId <= 0 && item.DepotGuid == Guid.Empty)
						{
							returnedSaveFuncInfo.AddError("ردیف انبار انتخاب شده معتبر نمی باشد", item.DepotGuid.ToString(), "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 576);
						}
						if (item.ProductId <= 0 && item.ProductGuid == Guid.Empty)
						{
							returnedSaveFuncInfo.AddError("ردیف کالا انتخاب شده معتبر نمی باشد", item.ProductGuid.ToString(), "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 578);
						}
					}
				}
			}
			catch (Exception exception)
			{
				returnedSaveFuncInfo.AddReturnedValue(exception, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 581);
			}
			return returnedSaveFuncInfo;
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		OrderActions orderActions = (OrderActions)action;
		return orderActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((OrderActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			_ = 13;
			try
			{
				switch ((OrderActions)pack.PacketActions)
				{
				case OrderActions.ReadResponse:
				{
					ConnectionEventArgs<WebOrder> args3 = new ConnectionEventArgs<WebOrder>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.ReadRequest:
				{
					ReadOperationEventArgs readOperationEventArgs = pack.Args[1].FromJson<ReadOperationEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 599);
					readOperationEventArgs.ConnectionUniqueId = connectionUniqueId;
					readOperationEventArgs.Connection = connection;
					readOperationEventArgs.ConnectionUniqueId = connectionUniqueId;
					await Utilities.RaiseEvent(this.OnReadRequest, this, readOperationEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.SaveRequest:
				{
					ConnectionEventArgs<WebOrder> args11 = new ConnectionEventArgs<WebOrder>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args11, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.SaveTempRequest:
				{
					ConnectionEventArgs<WebOrder> args10 = new ConnectionEventArgs<WebOrder>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveTempRequest, this, args10, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.SaveResponse:
				{
					SaveResponseEventArgs args5 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args5, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.SaveTempResponse:
				{
					SaveResponseEventArgs args9 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveTempResponse, this, args9, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxSendOrderRequest:
				{
					ConnectionEventArgs<Tuple<string, WebOrder>> args12 = new ConnectionEventArgs<Tuple<string, WebOrder>>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSendToTaxServiceRequest, this, args12, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxSendOrderResponse:
				{
					TaxInvoiceResultEventArgs args4 = Json.FromJson<List<TaxInvoiceResultEventArgs>>(pack?.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 628)?.FirstOrDefault();
					await Utilities.RaiseEvent(this.OnSendToTaxServiceResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxOrderTrackRequest:
				{
					ConnectionEventArgs<Tuple<string, List<InvoiceTrackingEventArgs>>> args6 = new ConnectionEventArgs<Tuple<string, List<InvoiceTrackingEventArgs>>>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnTaxServiceTrackRequest, this, args6, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxOrderTrackSaveRequest:
				{
					List<TaxInvoiceTrackingEventArgs> connectionData = Json.FromJson<List<TaxInvoiceTrackingEventArgs>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 636);
					ConnectionEventArgs<TaxInvoiceTrackingEventArgs> args2 = new ConnectionEventArgs<TaxInvoiceTrackingEventArgs>
					{
						ConnectionData = connectionData
					};
					await Utilities.RaiseEvent(this.OnTaxTrackSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxOrderTrackSaveResponse:
				{
					SaveResponseEventArgs args = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnTaxTrackSaveResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxOrderTrackResponse:
				{
					List<TaxInvoiceTrackingEventArgs> args8 = Json.FromJson<List<List<TaxInvoiceTrackingEventArgs>>>(pack?.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 645)?.FirstOrDefault();
					await Utilities.RaiseEvent(this.OnTaxServiceTrackResponse, this, args8, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxEconomicCodeStateRequest:
				{
					List<TaxEconomicCodeRecord> connectionData2 = Json.FromJson<List<TaxEconomicCodeRecord>>(pack.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 649);
					ConnectionEventArgs<TaxEconomicCodeRecord> args7 = new ConnectionEventArgs<TaxEconomicCodeRecord>(pack, connectionUniqueId, connection)
					{
						ConnectionData = connectionData2
					};
					await Utilities.RaiseEvent(this.OnTaxEconomicCodeStateRequest, this, args7, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				case OrderActions.TaxEconomicCodeStateResponse:
				{
					List<TaxEconomicCodeRecord> list = Json.FromJson<List<TaxEconomicCodeRecord>>(pack?.PacketContents, sendErrorIfException: true, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 654);
					await Utilities.RaiseEvent<TaxEconomicCodeRecord>(this.OnTaxEconomicCodeStateResponse, this, list?.SingleOrDefault(), connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
					break;
				}
				default:
					throw new NotImplementedException("نوع بسته داده جهت فاکتور به درستی وارد نشده است");
				case OrderActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 661);
			}
		}
	}

	public Task RaiseSaveResponse(SaveResponseEventArgs saveResponse, CancellationToken token)
	{
		return Utilities.RaiseEvent(this.OnSaveResponse, this, saveResponse, token, 60, "RaiseSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs");
	}

	public byte[] ToBytes()
	{
		return Json.ToBytesJson(this);
	}

	public string AutoTitle(ENAction act)
	{
		using (new JaegerMonitor("AutoTitle", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
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
				switch (FactorType)
				{
				case Document.OrderSell:
					text = text + " فاکتور فروش (" + Id2 + ") " + CustomerFullName;
					break;
				case Document.OrderBuy:
					text = text + " فاکتور خرید (" + Id2 + ") " + CustomerFullName;
					break;
				}
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AutoTitle", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 693);
				throw;
			}
		}
	}

	public string TotalBarcode(bool barcodeTotalEnable, string format, string contorol, double zaribRial, string cashierCode, string tranName = "")
	{
		return new BarcodeCreator(format, contorol, zaribRial, Id, Id2, Sum, Date, cashierCode, 0, 0).TotalBarcode();
	}

	public void FillDescription()
	{
		using (new JaegerMonitor("FillDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs"))
		{
			try
			{
				if (!string.IsNullOrEmpty(CustName))
				{
					Description = Description + "**نام خانوادگی:" + CustName;
				}
				if (!string.IsNullOrEmpty(CustFamily))
				{
					Description = Description + "**نام خانوادگی:" + CustFamily;
				}
				if (!string.IsNullOrEmpty(CustAddress))
				{
					Description = Description + "**آدرس:" + CustAddress;
				}
				if (!string.IsNullOrEmpty(CustMobile))
				{
					Description = Description + "**همراه:" + CustMobile;
				}
				if (!string.IsNullOrEmpty(CustTell))
				{
					Description = Description + "**تلفن:" + CustTell;
				}
				if (!string.IsNullOrEmpty(CustZipCode))
				{
					Description = Description + "**کدپستی:" + CustZipCode;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 711);
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
				if (this.OnSaveTempResponse != null)
				{
					this.OnSaveTempResponse = null;
				}
				ListKala = null;
				ListSanad = null;
				Pardakht = null;
				SanadArr = null;
				Reception = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 732);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckTaxValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!string.IsNullOrEmpty(SellerBrachCode) && SellerBrachCode.Length != 4)
			{
				await res.AddErrorAsync("کد شعبه فروشنده باید 4 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 739);
			}
			if (!string.IsNullOrEmpty(CustomerBrachCode) && CustomerBrachCode.Length != 4)
			{
				await res.AddErrorAsync("کد شعبه خریدار باید 4 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 740);
			}
			if (!string.IsNullOrEmpty(SellerEconomicNumber) && SellerEconomicNumber.Length != 14 && SellerEconomicNumber.Length != 11)
			{
				await res.AddErrorAsync("شماره اقتصادی فروشنده باید 11 و یا 14 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 742);
			}
			if (!string.IsNullOrEmpty(ContractUniqueId) && ContractUniqueId.Length > 12)
			{
				await res.AddErrorAsync("شناسه یکتای ثبت قرارداد باید حداکثر 12 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 743);
			}
			if (Template.GetValueOrDefault() == EnTaxInvoiceTemplate.Export)
			{
				if (CottageDate.HasValue && CottageDate > Date)
				{
					await res.AddErrorAsync("تاریخ و زمان صدور باید برابر و یا بزرگتر از تاریخ کوتاژ اظهارنامه باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 747);
				}
				if (TaxSubject.HasValue && TaxSubject.GetValueOrDefault() == EnTaxInvoiceSubject.BackSell)
				{
					await res.AddErrorAsync("الگوی صادرات شامل صورتحساب مرجوعی نمی شود", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 749);
				}
				if (string.IsNullOrEmpty(CustomsLicenseNumber))
				{
					await res.AddErrorAsync("در الگوی صادرات، وارد کردن شماره پروانه گمرکی اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 750);
				}
				else if (CustomsLicenseNumber.Length > 14)
				{
					await res.AddErrorAsync("شماره پروانه گمرکی باید حداکثر 14 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 751);
				}
				if (string.IsNullOrEmpty(CustomsCode))
				{
					await res.AddErrorAsync("در الگوی صادرات، وارد کردن کد گمرک محل اظهار اجباری است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 753);
				}
				else if (CustomsCode.Length != 5)
				{
					await res.AddErrorAsync("کد گمرک محل اظهار باید 5 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 754);
				}
				if (!string.IsNullOrEmpty(CottageNumber) && CottageNumber.Length > 14)
				{
					await res.AddErrorAsync("شماره کوتاژ اظهارنامه باید حداکثر 14 کاراکتر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 757);
				}
				if (Tax != 0m)
				{
					await res.AddErrorAsync("در الگوی صادرات، مقدار ارزش افزوده باید صفر باشد", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 758);
				}
				if (!TaxTasvieMethod.HasValue)
				{
					decimal receptionSum = ReceptionSum;
					if (LinkedSanad?.Any() ?? false)
					{
						receptionSum += LinkedSanad.Sum((WebTableRelationShip q) => q.Price);
					}
					if (TaxBillType == EnTaxBillType.TypeOne)
					{
						if (receptionSum <= 0m)
						{
							TaxTasvieMethod = EnTaxTasvieMethod.Etebar;
						}
						else if (receptionSum < TotalSum)
						{
							TaxTasvieMethod = EnTaxTasvieMethod.Cash_Etebar;
						}
						else
						{
							TaxTasvieMethod = EnTaxTasvieMethod.Cash;
						}
					}
					else
					{
						TaxTasvieMethod = EnTaxTasvieMethod.Cash;
					}
				}
				if (TaxTasvieMethod.GetValueOrDefault() != EnTaxTasvieMethod.Cash)
				{
					await res.AddErrorAsync("در الگوی صادرات، روش تسویه نقدی است", "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 773);
				}
			}
			if (ListKala?.Any() ?? false)
			{
				foreach (WebOrderDetail item in ListKala)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.CheckTaxValidationAsync(Template), ReturnedState.None, "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 778);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 782);
			res.AddReturnedValue(ex, "", "CheckTaxValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 783);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebOrder>> GetAsync(Guid guid, short depId, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebOrder> res = new ReturnedSaveFuncInfoWithValue<WebOrder>();
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebDepartment>> returnedSaveFuncInfoWithValue = await DepClient.GetAsync(depId, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 807);
			if (res.HasError || returnedSaveFuncInfoWithValue.value?.Data == null)
			{
				return res;
			}
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = await new RestTokenManagement(ENSource.AccountingPortal, returnedSaveFuncInfoWithValue.value.Data.User, returnedSaveFuncInfoWithValue.value.Data.Pass).GetToken(token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 811);
			if (res.HasError)
			{
				return res;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string> { 
			{
				"Authorization",
				(returnedSaveFuncInfoWithValue2.value ?? "") ?? ""
			} };
			string url = $"{apiUrl}{guid}";
			Dictionary<string, string> headers = dictionary;
			ReturnedSaveFuncInfoWithValue<WebResponse<WebOrder>> returnedSaveFuncInfoWithValue3 = await url.GetFromApi<WebOrder>(token, 3, headers);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 817);
			if (res.HasError)
			{
				return res;
			}
			res.value = returnedSaveFuncInfoWithValue3.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 823);
			res.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebOrder.cs", 824);
		}
		return res;
	}
}
