using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Accounting.Bill.Reports;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Accounting.DiscountCoupon.Records;
using EntityCache.Business.Accounting.DiscountCoupon.Reports;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Business.Accounting.Note;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Accounting.Tolid;
using EntityCache.Business.Core;
using EntityCache.Business.Customers;
using EntityCache.Business.Extensions;
using EntityCache.Business.Global;
using EntityCache.Business.OrderDepot;
using EntityCache.Business.OrderDepot.OpenInvoices;
using EntityCache.Business.OrderDepot.OrderCouponCalculation;
using EntityCache.Business.OrderDepot.OrderSanad;
using EntityCache.Business.Products;
using EntityCache.Business.Report;
using EntityCache.Cache.Repository;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Entities;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting.Tolid;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Sanad;
using PacketParser.EventArgs.TaxService;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.Filters.CouponDiscount;
using PacketParser.Print;
using PacketParser.Records;
using PacketParser.Records.Aghsat;
using PacketParser.Records.Order;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer;

namespace EntityCache.Business;

public class OrderBusiness : BaseSanadBusiness<OrderBusiness>, IOrder, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IEntityCache, IDisposable, IChangeIntegrity, ISanadBusiness, IOrderV5, IHasContext, IHasSanadStatusReport
{
	private CancellationTokenSource _setB1MoeinGuid;

	private CancellationTokenSource _setB2MoeinGuid;

	private CancellationTokenSource _setB3MoeinGuid;

	private CancellationTokenSource _setB4MoeinGuid;

	private CancellationTokenSource _send2ServerToken = new CancellationTokenSource();

	private static SemaphoreSlim _paralelOrderSend2ServerSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _setTolidSemaphore = new SemaphoreSlim(1, 1);

	public SemaphoreSlim _modifiedDetListSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _sendTaxResultSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _setTaxResultSemaphore = new SemaphoreSlim(1, 1);

	private static BaseLocks _removeSemaphores = new BaseLocks();

	private decimal _afterAccount;

	private decimal _lastSum;

	private decimal _tolidQuantity;

	private decimal? _beforeAccount;

	private short? _linkCount;

	internal int _Id2;

	private SanadSt _sanadSt = SanadSt.Movaghat;

	private DateTime _Date = DateManagment.DateM;

	private List<OrderDetailBusiness> _listOrderDetails;

	private TableRelationShipBusiness _linkedSanad;

	private string _sendToServerStatus = "";

	private string _bazaryabFullName;

	private string _barnameNo;

	private string _cpuSerial;

	private string _description;

	private string _hddSerial;

	private string _ip;

	private string _lkSerial;

	private string _hostRefrenced;

	private string _deliveryMessage;

	private string _deliveryShortMessage;

	private Guid? _receptionGuid;

	private Guid? _pardakhtGuid;

	private Guid? _discountTafsilGuid;

	private Guid _tafsilGuid;

	private Guid _productMoeinGuid;

	private Guid _discountMoeinGuid;

	private Guid _guid;

	private Guid _serviceMoeinGuid;

	private bool _isTemp;

	private bool _isDisposed;

	private bool _ChangeDate;

	private OrderBusiness _oldOrder;

	private TolidBusiness _oldTolid;

	private TolidBusiness _tolid;

	private SemaphoreSlim _getOldOrderSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _getOldTolidSemaphore = new SemaphoreSlim(1, 1);

	private PercentagePropertyContainer _discount;

	private PercentagePropertyContainer _keraye;

	private Document _factorType;

	private AghsatM_GroupBusiness _installment;

	private static CancellationTokenSource _sendEntitiesToServerToken;

	private ReceptionBusiness _reception;

	private PardakhtBusiness _pardakht;

	private Guid? _b1TafsilGuid;

	private Guid? _b2TafsilGuid;

	private Guid? _b3TafsilGuid;

	private Guid? _b4TafsilGuid;

	private Guid? _b1MoeinGuid;

	private Guid? _b2MoeinGuid;

	private Guid? _b3MoeinGuid;

	private Guid? _b4MoeinGuid;

	private Guid? _avarezTafsilGuid;

	private Guid? _avarezMoeinGuid;

	private Guid? _taxMoeinGuid;

	private Guid? _taxTafsilGuid;

	private Guid? _aghsatGuid;

	private Guid? _hazineTafsilGuid1;

	private Guid? _hazineTafsilGuid2;

	private Guid? _hazineTafsilGuid3;

	private Guid? _hazineTafsilGuid4;

	private Guid? _hazineTafsilGuid5;

	private Guid? _hazineMoeinGuid1;

	private Guid? _hazineMoeinGuid2;

	private Guid? _hazineMoeinGuid3;

	private Guid? _hazineMoeinGuid4;

	private Guid? _hazineMoeinGuid5;

	private Guid? _fareTafsilGuid;

	private Guid? _fareMoeinGuid;

	private ENSource _source;

	private Guid userAccGuid;

	private SanadBusiness connectedSanad;

	private CancellationTokenSource _sumChangedToken;

	private CancellationTokenSource _updateTolidToken;

	private OrderBusiness eventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.OrderBusiness;

	public override Document SanadType => FactorType;

	public Document AsType => SanadType;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public bool? DeliveryStatus { get; set; }

	public Guid ProductMoeinGuid
	{
		get
		{
			try
			{
				if (_productMoeinGuid != Guid.Empty)
				{
					return _productMoeinGuid;
				}
				AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
				if (setting.CompanyInfo.SanadType == 1)
				{
					switch (FactorType)
					{
					case Document.OrderSell:
						_productMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Product;
						break;
					case Document.OrderBackSell:
						_productMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Product;
						break;
					default:
						return setting.CodeSetting.Permanent.ProductMoeinGuid;
					}
				}
				else
				{
					switch (FactorType)
					{
					case Document.OrderSell:
					case Document.PishFactor:
						_productMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Product;
						break;
					case Document.OrderBuy:
					case Document.OrderPreBuy:
						_productMoeinGuid = setting.CodeSetting.BuyCoding.Debit_MoeinGuid_Product;
						break;
					case Document.OrderBackBuy:
						_productMoeinGuid = setting.CodeSetting.BackBuyCoding.Credit_MoeinGuid_Product;
						break;
					case Document.OrderBackSell:
						_productMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Product;
						break;
					case Document.Zayeat:
						_productMoeinGuid = setting.CodeSetting.ZayeatCoding.Credit_MoeinGuid_Product;
						break;
					case Document.AmaniKhoroj:
						_productMoeinGuid = setting.CodeSetting.AmaniKhorojCoding.Credit_MoeinGuid_Product;
						break;
					case Document.AmaniVorod:
						_productMoeinGuid = setting.CodeSetting.AmaniVorodCoding.Debit_MoeinGuid_Product;
						break;
					default:
						return setting.CodeSetting.SellCoding.Credit_MoeinGuid_Product;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ProductMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 158);
			}
			return _productMoeinGuid;
		}
		set
		{
			_productMoeinGuid = value;
		}
	}

	public Guid DiscountMoeinGuid
	{
		get
		{
			try
			{
				if (_discountMoeinGuid == Guid.Empty)
				{
					AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
					switch (FactorType)
					{
					case Document.OrderSell:
						_discountMoeinGuid = setting.CodeSetting.SellCoding.Debit_MoeinGuid_Discount;
						break;
					case Document.OrderBuy:
						_discountMoeinGuid = setting.CodeSetting.BuyCoding.Credit_MoeinGuid_Discount;
						break;
					case Document.OrderBackBuy:
						_discountMoeinGuid = setting.CodeSetting.BackBuyCoding.Debit_MoeinGuid_Discount;
						break;
					case Document.OrderBackSell:
						_discountMoeinGuid = setting.CodeSetting.BackSellCoding.Credit_MoeinGuid_Discount;
						break;
					default:
						return setting.CodeSetting.SellCoding.Debit_MoeinGuid_Discount;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DiscountMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 191);
			}
			return _discountMoeinGuid;
		}
		set
		{
			_discountMoeinGuid = value;
		}
	}

	public Guid? DiscountTafsilGuid
	{
		get
		{
			try
			{
				if (!_discountTafsilGuid.HasValue || _discountTafsilGuid == Guid.Empty)
				{
					AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
					switch (FactorType)
					{
					case Document.OrderSell:
						_discountTafsilGuid = setting.CodeSetting.SellCoding.Debit_TafsilGuid_Discount;
						break;
					case Document.OrderBuy:
						_discountTafsilGuid = setting.CodeSetting.BuyCoding.Credit_TafsilGuid_Discount;
						break;
					case Document.OrderBackBuy:
						_discountTafsilGuid = setting.CodeSetting.BackBuyCoding.Debit_TafsilGuid_Discount;
						break;
					case Document.OrderBackSell:
						_discountTafsilGuid = setting.CodeSetting.BackSellCoding.Credit_TafsilGuid_Discount;
						break;
					default:
						return setting.CodeSetting.SellCoding.Debit_TafsilGuid_Discount;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DiscountTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 224);
			}
			return _discountTafsilGuid;
		}
		set
		{
			_discountTafsilGuid = value;
		}
	}

	public ENSource Source
	{
		get
		{
			if (_source == ENSource.None)
			{
				return ClsCache.GetCache(ContextGuid)?.Source ?? ENSource.None;
			}
			return _source;
		}
		set
		{
			_source = value;
		}
	}

	public int SanadNo { get; set; }

	public int AttachmentNo { get; set; }

	[JsonIgnore]
	public WooOrder WooOrder { get; set; } = new WooOrder();


	[Obsolete]
	public int FishNo { get; set; }

	public short StatusId { get; set; }

	public DateTime? DateUtc { get; set; }

	public Guid? IntroductionMethodGuid { get; set; }

	public Guid? CashBackCouponGuid { get; set; }

	public bool IsCashBackProgressed { get; set; }

	public Guid? ServiceMoeinGuid
	{
		get
		{
			try
			{
				if (_serviceMoeinGuid != Guid.Empty)
				{
					return _serviceMoeinGuid;
				}
				AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
				switch (FactorType)
				{
				case Document.OrderSell:
					_serviceMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Service;
					break;
				case Document.OrderBackSell:
					_serviceMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Service;
					break;
				default:
					_serviceMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Service;
					break;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ServiceMoeinGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 258);
			}
			return _serviceMoeinGuid;
		}
		set
		{
			_serviceMoeinGuid = value ?? Guid.Empty;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public IReadOnlyList<OrderDetailBusiness> ListKala
	{
		get
		{
			if (_listOrderDetails == null)
			{
				_listOrderDetails = new List<OrderDetailBusiness>();
			}
			_listOrderDetails.RemoveAll((OrderDetailBusiness p) => p == null || p.IsDisposed || ((p.GrouppingGuid ?? Guid.Empty) != Guid.Empty && (p.GrouppingGuid ?? Guid.Empty) != p.Guid));
			return _listOrderDetails;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public IEnumerable<OrderDetailBusiness> ListProduct => ListKala?.ToList()?.Where((OrderDetailBusiness p) => (p?._prdInfo?.Type).GetValueOrDefault(EnProductType.None) != EnProductType.Service);

	[XmlIgnore]
	[JsonIgnore]
	public IEnumerable<OrderDetailBusiness> ListService => ListKala?.ToList()?.Where((OrderDetailBusiness p) => (p?._prdInfo?.Type).GetValueOrDefault(EnProductType.None) == EnProductType.Service);

	[XmlIgnore]
	[JsonIgnore]
	public IReadOnlyDictionary<Guid, OrderDetailBusiness> DictionaryKala => ListKala.ToDictionary((OrderDetailBusiness x) => x.Guid);

	public string SendToServerStatus
	{
		get
		{
			return _sendToServerStatus;
		}
		set
		{
			_sendToServerStatus = value?.FixString() ?? "";
		}
	}

	[JsonIgnore]
	public string SellBasketText
	{
		get
		{
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala == null || listKala.Count() <= 0)
			{
				return "سبد خرید";
			}
			return $"{ListKala.Count} مورد- {TotalSum:N0} {ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit}";
		}
	}

	[JsonIgnore]
	public decimal Tax => (ListKala?.Where((OrderDetailBusiness q) => !q.IsTaxImpactOnCost)?.Sum((OrderDetailBusiness p) => p?.Tax ?? 0m)).GetValueOrDefault();

	[JsonIgnore]
	public decimal TotalTax => Tax + TaxImpact;

	[JsonIgnore]
	public decimal RowDiscount
	{
		get
		{
			decimal result = default(decimal);
			List<OrderDetailBusiness> listOrderDetails = _listOrderDetails;
			if (listOrderDetails != null && listOrderDetails.Any())
			{
				foreach (OrderDetailBusiness listOrderDetail in _listOrderDetails)
				{
					result += listOrderDetail.Discount;
				}
			}
			return result;
		}
	}

	[JsonIgnore]
	public decimal TotalHazine1
	{
		get
		{
			decimal result = default(decimal);
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala != null && listKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					result += item.Hazine1Price;
				}
			}
			return result;
		}
	}

	[JsonIgnore]
	public decimal TotalHazine2
	{
		get
		{
			decimal result = default(decimal);
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala != null && listKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					result += item.Hazine2Price;
				}
			}
			return result;
		}
	}

	[JsonIgnore]
	public decimal TotalHazine3
	{
		get
		{
			decimal result = default(decimal);
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala != null && listKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					result += item.Hazine3Price;
				}
			}
			return result;
		}
	}

	[JsonIgnore]
	public decimal TotalHazine4
	{
		get
		{
			decimal result = default(decimal);
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala != null && listKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					result += item.Hazine4Price;
				}
			}
			return result;
		}
	}

	[JsonIgnore]
	public decimal TotalHazine5
	{
		get
		{
			decimal result = default(decimal);
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala != null && listKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					result += item.Hazine5Price;
				}
			}
			return result;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public decimal TotalDiscount => Discount + RowDiscount;

	[XmlIgnore]
	[JsonIgnore]
	public decimal TotalHazine => TotalHazine1 + TotalHazine2 + TotalHazine3 + TotalHazine4 + TotalHazine5;

	[XmlIgnore]
	[JsonIgnore]
	public decimal TotalBazaryab => B1 + B2 + B3 + B4;

	[XmlIgnore]
	[JsonIgnore]
	public decimal SumKala => (ListKala?.ToList()?.Where((OrderDetailBusiness p) => (p?._prdInfo?.Type).GetValueOrDefault(EnProductType.None) != EnProductType.Service)?.Sum((OrderDetailBusiness p) => p?.RowSum ?? 0m)).GetValueOrDefault();

	[XmlIgnore]
	[JsonIgnore]
	public decimal SumService => (ListKala?.ToList()?.Where((OrderDetailBusiness p) => (p?._prdInfo?.Type).GetValueOrDefault(EnProductType.None) == EnProductType.Service)?.Sum((OrderDetailBusiness p) => p?.RowSum ?? 0m)).GetValueOrDefault();

	[XmlIgnore]
	[JsonIgnore]
	public decimal Sum => SumKalaKhadamat + Keraye - Discount;

	[XmlIgnore]
	[JsonIgnore]
	public decimal SumKalaKhadamat => SumKala + SumService;

	[XmlIgnore]
	[JsonIgnore]
	public decimal TotalSum => Sum + Avarez + TotalTax;

	public Guid UserAccGuid
	{
		get
		{
			return userAccGuid;
		}
		set
		{
			userAccGuid = value;
		}
	}

	public Guid CashierGuid => UserAccGuid;

	public Guid? B1TafsilGuid
	{
		get
		{
			if (!((_b1TafsilGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b1TafsilGuid;
			}
			return null;
		}
	}

	public Guid? B2TafsilGuid
	{
		get
		{
			if (!((_b2TafsilGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b2TafsilGuid;
			}
			return null;
		}
	}

	public Guid? B3TafsilGuid
	{
		get
		{
			if (!((_b3TafsilGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b3TafsilGuid;
			}
			return null;
		}
	}

	public Guid? B4TafsilGuid
	{
		get
		{
			if (!((_b4TafsilGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b4TafsilGuid;
			}
			return null;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public decimal B1 => ListKala?.Sum((OrderDetailBusiness x) => x.VisitorPrice1) ?? 0m;

	[JsonIgnore]
	public decimal B2 => ListKala?.Sum((OrderDetailBusiness x) => x.VisitorPrice2) ?? 0m;

	[JsonIgnore]
	public decimal B3 => ListKala?.Sum((OrderDetailBusiness x) => x.VisitorPrice3) ?? 0m;

	[JsonIgnore]
	public decimal B4 => ListKala?.Sum((OrderDetailBusiness x) => x.VisitorPrice4) ?? 0m;

	public Guid MoeinGuid { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public MoeinBusiness Moein => MoeinBusiness.Get(ContextGuid, MoeinGuid);

	public Guid? B1MoeinGuid
	{
		get
		{
			if (!((_b1MoeinGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b1MoeinGuid;
			}
			return null;
		}
		set
		{
			_setB1MoeinGuid?.Cancel();
			_b1MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B2MoeinGuid
	{
		get
		{
			if (!((_b2MoeinGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b2MoeinGuid;
			}
			return null;
		}
		set
		{
			_setB2MoeinGuid?.Cancel();
			_b2MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B3MoeinGuid
	{
		get
		{
			if (!((_b3MoeinGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b3MoeinGuid;
			}
			return null;
		}
		set
		{
			_setB3MoeinGuid?.Cancel();
			_b3MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? B4MoeinGuid
	{
		get
		{
			if (!((_b4MoeinGuid ?? Guid.Empty) == Guid.Empty))
			{
				return _b4MoeinGuid;
			}
			return null;
		}
		set
		{
			_setB4MoeinGuid?.Cancel();
			_b4MoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? AvarezTafsilGuid
	{
		get
		{
			if ((!_avarezTafsilGuid.HasValue || _avarezTafsilGuid == Guid.Empty) && Avarez > 0m)
			{
				AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
				switch (FactorType)
				{
				case Document.OrderSell:
					_avarezTafsilGuid = setting.CodeSetting.SellCoding.Credit_TafsilGuid_Avarez;
					break;
				case Document.OrderBuy:
					_avarezTafsilGuid = setting.CodeSetting.BuyCoding.Debit_TafsilGuid_Avarez;
					break;
				case Document.OrderBackBuy:
					_avarezTafsilGuid = setting.CodeSetting.BackBuyCoding.Credit_TafsilGuid_Avarez;
					break;
				case Document.OrderBackSell:
					_avarezTafsilGuid = setting.CodeSetting.BackSellCoding.Debit_TafsilGuid_Avarez;
					break;
				default:
					_avarezTafsilGuid = setting.CodeSetting.SellCoding.Credit_TafsilGuid_Avarez;
					break;
				}
				if ((_avarezTafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					_avarezTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil29772;
				}
			}
			return _avarezTafsilGuid;
		}
		set
		{
			_avarezTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? AvarezMoeinGuid
	{
		get
		{
			if ((!_avarezMoeinGuid.HasValue || _avarezMoeinGuid == Guid.Empty) && Avarez > 0m)
			{
				AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
				switch (FactorType)
				{
				case Document.OrderSell:
					_avarezMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Avarez;
					break;
				case Document.OrderBuy:
					_avarezMoeinGuid = setting.CodeSetting.BuyCoding.Debit_MoeinGuid_Avarez;
					break;
				case Document.OrderBackBuy:
					_avarezMoeinGuid = setting.CodeSetting.BackBuyCoding.Credit_MoeinGuid_Avarez;
					break;
				case Document.OrderBackSell:
					_avarezMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Avarez;
					break;
				default:
					_avarezMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Avarez;
					break;
				}
			}
			return _avarezMoeinGuid;
		}
		set
		{
			_avarezMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? TaxTafsilGuid
	{
		get
		{
			if ((!_taxTafsilGuid.HasValue || _taxTafsilGuid == Guid.Empty) && Tax > 0m)
			{
				AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
				switch (FactorType)
				{
				case Document.OrderSell:
					_taxTafsilGuid = setting.CodeSetting.SellCoding.Credit_TafsilGuid_Tax;
					break;
				case Document.OrderBuy:
					_taxTafsilGuid = setting.CodeSetting.BuyCoding.Debit_TafsilGuid_Tax;
					break;
				case Document.OrderBackBuy:
					_taxTafsilGuid = setting.CodeSetting.BackBuyCoding.Credit_TafsilGuid_Tax;
					break;
				case Document.OrderBackSell:
					_taxTafsilGuid = setting.CodeSetting.BackSellCoding.Debit_TafsilGuid_Tax;
					break;
				default:
					_taxTafsilGuid = setting.CodeSetting.SellCoding.Credit_TafsilGuid_Tax;
					break;
				}
				if ((_taxTafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					_taxTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil29773;
				}
			}
			return _taxTafsilGuid;
		}
		set
		{
			_taxTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? TaxMoeinGuid
	{
		get
		{
			if ((!_taxMoeinGuid.HasValue || _taxMoeinGuid == Guid.Empty) && Tax > 0m)
			{
				AccGlobalSettings setting = LocalSettingBusiness.GetSetting(ContextGuid);
				switch (FactorType)
				{
				case Document.OrderSell:
					_taxMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Tax;
					break;
				case Document.OrderBuy:
					_taxMoeinGuid = setting.CodeSetting.BuyCoding.Debit_MoeinGuid_Tax;
					break;
				case Document.OrderBackBuy:
					_taxMoeinGuid = setting.CodeSetting.BackBuyCoding.Credit_MoeinGuid_Tax;
					break;
				case Document.OrderBackSell:
					_taxMoeinGuid = setting.CodeSetting.BackSellCoding.Debit_MoeinGuid_Tax;
					break;
				default:
					_taxMoeinGuid = setting.CodeSetting.SellCoding.Credit_MoeinGuid_Tax;
					break;
				}
			}
			return _taxMoeinGuid;
		}
		set
		{
			_taxMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public override Guid Guid
	{
		get
		{
			if (_guid == Guid.Empty)
			{
				throw new Exception("Guid is empty! please create order with GenerateNewWithDefaultsAsync function.");
			}
			return _guid;
		}
		set
		{
			ConnectedSanad.Guid = value;
			if (value == Guid.Empty)
			{
				throw new Exception("Guid is empty! please create order with GenerateNewWithDefaultsAsync function.");
			}
			if (_guid != Guid.Empty && _guid != value)
			{
				foreach (OrderDetailBusiness item2 in ListKala)
				{
					item2.Guid = Guid.NewGuid();
				}
				_receptionGuid = Guid.Empty;
				_pardakhtGuid = Guid.Empty;
				IOrderRepository iOrder = ClsCache.GetCache(ContextGuid).CacheRepository.IOrder;
				((OrderCacheRepository)iOrder).TimingCache.TryRemove(_guid, out var _);
				_guid = value;
				iOrder.Add2CacheAsync(this, default(CancellationToken), "");
				_oldOrder = null;
			}
			else
			{
				_guid = value;
			}
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid? AghsatGuid
	{
		get
		{
			return _aghsatGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				_aghsatGuid = null;
			}
			else
			{
				_aghsatGuid = value;
			}
		}
	}

	public string AndroidIme
	{
		get
		{
			return AndroidImeLong.ToString();
		}
		set
		{
			AndroidImeLong = value.ParseToLong(0L);
		}
	}

	public long AndroidImeLong { get; set; }

	public decimal Avarez => (ListKala?.ToList()?.Sum((OrderDetailBusiness p) => p?.Avarez ?? 0m)).GetValueOrDefault();

	public string BarnameNo
	{
		get
		{
			return _barnameNo;
		}
		set
		{
			_barnameNo = value?.FixString() ?? "";
		}
	}

	public string BazaryabFullName
	{
		get
		{
			return _bazaryabFullName ?? "";
		}
		set
		{
			_bazaryabFullName = value?.FixString() ?? "";
		}
	}

	[JsonIgnore]
	public string Time => $"{GetDate().Hour}:{GetDate().Minute}:{GetDate().Second}";

	public string CpuSerial
	{
		get
		{
			return _cpuSerial;
		}
		set
		{
			_cpuSerial = value?.FixString() ?? "";
		}
	}

	public DateTime DateSabt { get; set; } = DateTime.Now;


	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public decimal Discount
	{
		get
		{
			return _discount.GetPrice();
		}
		set
		{
			if (this.OnChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
			if (_discount.SetPrice(value) && eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
			{
				_lastSum = TotalSum;
				PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, default(CancellationToken), 60, "Discount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
		}
	}

	public decimal DiscountRows => _listOrderDetails?.Sum((OrderDetailBusiness p) => p?.Discount ?? 0m) ?? 0m;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public Document FactorType
	{
		get
		{
			return _factorType;
		}
		set
		{
			if (_factorType == value)
			{
				return;
			}
			bool flag = (_factorType == Document.PishFactor && value == Document.OrderSell) || (_factorType == Document.AmaniVorod && value == Document.OrderBuy) || (_factorType == Document.AmaniKhoroj && value == Document.OrderSell) || _factorType == Document.None || (_factorType == Document.OrderSell && value == Document.OrderBackSell) || (_factorType == Document.OrderBuy && value == Document.OrderBackBuy) || (_factorType == Document.OrderPreBuy && value == Document.OrderBuy);
			if (flag && _factorType != 0)
			{
				_oldOrder = null;
				SanadNo = 0;
				_Date = DateTime.Now;
				_ChangeDate = false;
				foreach (OrderDetailBusiness item in _listOrderDetails ?? new List<OrderDetailBusiness>())
				{
					item.Guid = Guid.Empty;
				}
			}
			if (ChangeStatus == EntityChangeSt.Edited && !flag)
			{
				try
				{
					throw new ArgumentException("FactorType is Changed" + $"Old value is:{_factorType}" + $"new value is:{value}");
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "موقتا کد رو غیر فعال کردم چون در تبدیل پیش فاکتور به فروش مشکل میخوریدم و راهکار نداشتم براش", "FactorType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1421);
				}
			}
			_factorType = value;
		}
	}

	public OpenInvoiceBusiness OpenInvoice { get; private set; }

	public OrderMetaData MetaData { get; set; }

	public short DailyFactorNo { get; set; }

	public Guid? HazineTafsilGuid1
	{
		get
		{
			return _hazineTafsilGuid1;
		}
		set
		{
			_hazineTafsilGuid1 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid2
	{
		get
		{
			return _hazineTafsilGuid2;
		}
		set
		{
			_hazineTafsilGuid2 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid3
	{
		get
		{
			return _hazineTafsilGuid3;
		}
		set
		{
			_hazineTafsilGuid3 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid4
	{
		get
		{
			return _hazineTafsilGuid4;
		}
		set
		{
			_hazineTafsilGuid4 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineTafsilGuid5
	{
		get
		{
			return _hazineTafsilGuid5;
		}
		set
		{
			_hazineTafsilGuid5 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid1
	{
		get
		{
			return _hazineMoeinGuid1;
		}
		set
		{
			_hazineMoeinGuid1 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid2
	{
		get
		{
			return _hazineMoeinGuid2;
		}
		set
		{
			_hazineMoeinGuid2 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid3
	{
		get
		{
			return _hazineMoeinGuid3;
		}
		set
		{
			_hazineMoeinGuid3 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid4
	{
		get
		{
			return _hazineMoeinGuid4;
		}
		set
		{
			_hazineMoeinGuid4 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? HazineMoeinGuid5
	{
		get
		{
			return _hazineMoeinGuid5;
		}
		set
		{
			_hazineMoeinGuid5 = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid TafsilGuid => _tafsilGuid;

	public string HddSerial
	{
		get
		{
			return _hddSerial ?? "";
		}
		set
		{
			_hddSerial = value?.FixString() ?? "";
		}
	}

	public string Ip
	{
		get
		{
			return _ip;
		}
		set
		{
			_ip = value?.FixString() ?? "";
		}
	}

	public decimal Keraye => _keraye.GetPrice();

	public Guid? FareMoeinGuid
	{
		get
		{
			if (!_fareMoeinGuid.HasValue && Keraye != 0m)
			{
				AccCodeSetting codeSetting = LocalSettingBusiness.GetSetting(ContextGuid).CodeSetting;
				if (FactorType == Document.OrderBuy)
				{
					_fareMoeinGuid = codeSetting.BuyCoding.Debit_MoeinGuid_Keraye;
				}
			}
			return _fareMoeinGuid;
		}
		set
		{
			_fareMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? FareTafsilGuid
	{
		get
		{
			return _fareTafsilGuid;
		}
		set
		{
			_fareTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public string LkSerial
	{
		get
		{
			return _lkSerial;
		}
		set
		{
			_lkSerial = value?.FixString() ?? "";
		}
	}

	public Guid? PardakhtGuid
	{
		get
		{
			if (FactorType != Document.OrderBuy && FactorType != Document.OrderBackSell)
			{
				return null;
			}
			return _pardakhtGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				_pardakhtGuid = null;
			}
			else
			{
				_pardakhtGuid = value;
			}
		}
	}

	public Document Type
	{
		get
		{
			return FactorType;
		}
		set
		{
			FactorType = value;
		}
	}

	public Guid? PriceTypeGuid { get; set; }

	public short PrintCount { get; set; }

	public DateTime? PrintDate { get; set; } = DateTime.Now;


	public Guid? ReceptionGuid
	{
		get
		{
			if (FactorType != Document.OrderSell && FactorType != Document.OrderBackBuy)
			{
				return null;
			}
			return _receptionGuid;
		}
		set
		{
			_receptionGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public int ReceptionId { get; set; }

	public string HostRefrenced
	{
		get
		{
			return _hostRefrenced;
		}
		set
		{
			_hostRefrenced = value?.FixString() ?? "";
		}
	}

	public SanadSt SanadSt
	{
		get
		{
			if (_sanadSt == SanadSt.None)
			{
				_sanadSt = SanadSt.Movaghat;
			}
			return _sanadSt;
		}
		set
		{
			_sanadSt = value;
		}
	}

	public int TasvieDay { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage
	{
		get
		{
			return _deliveryMessage;
		}
		set
		{
			_deliveryMessage = value?.FixString() ?? "";
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public string DeliveryShortMessage
	{
		get
		{
			return _deliveryShortMessage;
		}
		set
		{
			_deliveryShortMessage = value?.FixString() ?? "";
		}
	}

	public string CustName { get; set; }

	public string CustFamily { get; set; }

	public string CustMobile { get; set; }

	public string CustAddress { get; set; }

	public string CustZipCode { get; set; }

	public string CustTell { get; set; }

	public PaymentMethod PaymentMethod { get; set; }

	public string SellerFactorNo { get; set; }

	public override EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	public bool SanadAffective { get; set; } = true;


	public bool KardexAffective { get; set; } = true;


	public bool IsFromGardesh { get; set; }

	public string DiscountString
	{
		get
		{
			return Math.Round(Discount, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Discount = (decimal.TryParse(value, out var result) ? result : 0m);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float DiscountPercent
	{
		get
		{
			try
			{
				if (SumKalaKhadamat <= 0m)
				{
					return 0f;
				}
				return (float)Math.Round(100m * Discount / SumKalaKhadamat, DigitForRound);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DiscountPercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1808);
			}
			return 0f;
		}
		set
		{
			Discount = Math.Round(SumKalaKhadamat * (decimal)value / 100m, DigitAfterPoint);
		}
	}

	[JsonIgnore]
	public string DiscountPercentString
	{
		get
		{
			return Math.Round(DiscountPercent, DigitForRound).ToString().Replace(".", "/");
		}
		set
		{
			DiscountPercent = value.ParseTofloat(0f, 3);
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public IReadOnlyCollection<OrderDetailBusiness> ListValidKala => ListKala;

	public List<LastPriceBusiness> LastPriceList
	{
		get
		{
			try
			{
				List<LastPriceBusiness> list = new List<LastPriceBusiness>();
				IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
				if (listKala == null || !listKala.Any())
				{
					return list;
				}
				foreach (OrderDetailBusiness item in ListKala)
				{
					if (!list.Select((LastPriceBusiness q) => q.PrdGuid).Contains(item.ProductGuid) && ListKala.Where((OrderDetailBusiness q) => q.ProductGuid == item.ProductGuid)?.OrderByDescending((OrderDetailBusiness q) => q.Ordinal)?.FirstOrDefault() != null)
					{
						list.Add(new LastPriceBusiness(ContextGuid)
						{
							AsGuid = Guid,
							AsType = FactorType,
							ContextGuid = ContextGuid,
							Date = GetDate(),
							Invalidated = false,
							Modified = DateTime.Now,
							PrdGuid = item.ProductGuid,
							Price = item.Price,
							ServerStatus = ServerStatusForClient.None,
							TafsilGuid = TafsilGuid,
							Guid = Guid.NewGuid(),
							Status = true
						});
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LastPriceList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1869);
				throw;
			}
		}
	}

	public DateTime? TasvieDate { get; set; }

	public DateTime? TasvieDateMoaser { get; set; }

	[JsonIgnore]
	public decimal TaxImpact => (ListKala?.Where((OrderDetailBusiness q) => q.IsTaxImpactOnCost)?.Sum((OrderDetailBusiness p) => p?.Tax ?? 0m)).GetValueOrDefault();

	[JsonIgnore]
	public short DigitForRound { get; }

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public TolidBusiness Tolid => _tolid;

	public SanadBusiness ConnectedSanad => GetConnectedSanad(EnSystem.None);

	public decimal Debit => ConnectedSanad?.Debit ?? 0m;

	public decimal Credit => ConnectedSanad?.Credit ?? 0m;

	public IReadOnlyList<Sanad_DetBusiness> ListSanad_Det => ConnectedSanad?.ListSanad_Det;

	public float KerayePercent
	{
		get
		{
			if (!(SumKalaKhadamat > 0m))
			{
				return 0f;
			}
			return (float)Math.Round(100m * Keraye / SumKalaKhadamat, DigitForRound);
		}
		set
		{
			SetKeraye(Math.Round(SumKala * (decimal)value / 100m, DigitAfterPoint));
		}
	}

	public string KerayePercentString
	{
		get
		{
			return KerayePercent.ToString().Replace(".", "/");
		}
		set
		{
			KerayePercent = value.ParseTofloat(0f, 3);
		}
	}

	public string KerayeString
	{
		get
		{
			return Keraye.NpToString("N0", 0);
		}
		set
		{
			SetKeraye(value.ParseToDecimal());
		}
	}

	public bool IsTemp
	{
		get
		{
			try
			{
				return GetIsTemp();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "IsTemp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4919);
			}
			return false;
		}
	}

	public DateTime? TransferDate { get; set; }

	public event Func<object, EventArgs, Task> OnChanged;

	public event Func<object, EventArgs, Task> OnSumChanged_static;

	public event Func<object, EventArgs, Task> OnVisitorChanged_static;

	public event Func<object, EventArgs, Task> OnDepotChanged_static;

	public event Func<object, EventArgs, Task> OnTolidChanged_static;

	public event Func<object, EventArgs, Task> OnTaxResultChanged_static;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged_static;

	public event Func<object, OrderDetailBusiness, Task> OnItemAdd;

	public event Func<object, OrderDetailBusiness, Task> OnItemRemoved;

	public event Func<object, EventArgs, Task> OnChangedTafsilGuid;

	public async Task<ReceptionBusiness> GetReceptionAsync(EnSystem system = EnSystem.UserInterface, string tranName = "", bool fromDatabase = false, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		using (new JaegerMonitor("GetReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			if (string.IsNullOrEmpty(ContextGuid))
			{
				throw new ArgumentException("ContextGuid is null or empty!");
			}
			try
			{
				if (system == EnSystem.Transfer)
				{
					return null;
				}
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					fromDatabase = true;
				}
				line = 1;
				if (FactorType == Document.OrderSell || FactorType == Document.OrderBackBuy)
				{
					line = 2;
					if (_reception == null)
					{
						_reception = await ReceptionBusiness.GetAsync(ContextGuid, _receptionGuid ?? Guid.Empty, system, this, fromDatabase, token, tranName);
					}
					line = 3;
					if (_reception == null)
					{
						line = 4;
						ReceptionBusiness receptionBusiness = new ReceptionBusiness(ContextGuid, this)
						{
							UserGuid = UserAccGuid,
							MoeinGuid = MoeinGuid,
							IsTemp = GetIsTemp(),
							Date = GetDate()
						};
						ReceptionBusiness receptionBusiness2 = receptionBusiness;
						string translate = "بابت".GetTranslate();
						string translate2 = FactorType.GetDisplay().GetTranslate();
						int num = await GetId2Async(system, tranName, token);
						receptionBusiness2.Description = translate + "  " + translate2 + " " + $"({num})";
						ReceptionBusiness receptionBusiness3 = receptionBusiness;
						receptionBusiness3.SanadNo = await GetSanadNumberAsync(token, tranName);
						receptionBusiness.SanadSt = (short)SanadSt;
						receptionBusiness.Source = Source;
						receptionBusiness.Status = Status;
						receptionBusiness.ChangeStatus = EntityChangeSt.New;
						_reception = receptionBusiness;
						await _reception.SetTafsilGuidAsync(GetTafsilGuid(), system, token, tranName);
						line = 5;
						_receptionGuid = _reception.Guid;
						line = 6;
					}
					if (_reception.MoeinGuid != MoeinGuid)
					{
						_reception.MoeinGuid = MoeinGuid;
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, string.Format("Line:{0}//_reception:{1}//ClsCache.GetCache(contextGuid).CacheRepository.IReception is null:{2}//ClsCache.GetCache(contextGuid).isDisposed:{3}", line, _reception?.ToString() ?? "Null", ClsCache.GetCache(ContextGuid)?.CacheRepository?.IReception == null, ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true), "GetReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 307);
			}
			return _reception;
		}
	}

	public void SetReception(ReceptionBusiness value)
	{
		_reception = value;
	}

	public async Task<PardakhtBusiness> GetPardakhtAsync(EnSystem system = EnSystem.UserInterface, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				if (FactorType != Document.OrderBuy && FactorType != Document.OrderBackSell)
				{
					return _pardakht;
				}
				if (_pardakht == null)
				{
					_pardakht = await PardakhtBusiness.GetAsync(ContextGuid, _pardakhtGuid ?? Guid.Empty, fromDatabase: true, token, tranName);
				}
				if (_pardakht != null)
				{
					if (_pardakht.MoeinGuid != MoeinGuid)
					{
						_pardakht.MoeinGuid = MoeinGuid;
					}
					return _pardakht;
				}
				line = 5;
				PardakhtBusiness pardakhtBusiness = new PardakhtBusiness(ContextGuid, this)
				{
					Date = GetDate(),
					MoeinGuid = MoeinGuid,
					IsTemp = GetIsTemp(),
					UserGuid = UserAccGuid
				};
				PardakhtBusiness pardakhtBusiness2 = pardakhtBusiness;
				string translate = "بابت".GetTranslate();
				string translate2 = FactorType.GetDisplay().GetTranslate();
				int num = await GetId2Async(system, tranName, token);
				pardakhtBusiness2.Description = translate + " " + translate2 + " " + $" ({num})";
				pardakhtBusiness.ChangeStatus = EntityChangeSt.New;
				_pardakht = pardakhtBusiness;
				await _pardakht.SetTafsilGuidAsync(GetTafsilGuid(), system, token, tranName);
				_pardakhtGuid = _pardakht.Guid;
				return _pardakht;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//IsDisposed:{IsDisposed}", "GetPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 343);
				return null;
			}
		}
	}

	public void SetPardakht(PardakhtBusiness value)
	{
		_pardakht = value;
	}

	public async Task<decimal> GetLinkedReceptionSumAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		decimal ret = default(decimal);
		await GetLinkedSanadAsync(system, token);
		return ret;
	}

	public async Task<List<Guid>> GetLinkedFilesAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		List<Guid> ret = null;
		try
		{
			TableRelationShipBusiness tableRelationShipBusiness = await GetLinkedSanadAsync(system, token);
			if (tableRelationShipBusiness == null)
			{
				return null;
			}
			ret = tableRelationShipBusiness?.ChildList?.Where((TableRelationShipBusiness e) => e.SanadType == Document.FileManagement)?.Select((TableRelationShipBusiness q) => q.SanadGuid)?.ToList() ?? new List<Guid>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLinkedFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 380);
		}
		return ret;
	}

	public async Task SetLinkedFilesAsync(List<Guid> value, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			_linkCount = null;
			if (system == EnSystem.Transfer || system == EnSystem.Mapper)
			{
				return;
			}
			TableRelationShipBusiness tblRelation = await GetLinkedSanadAsync(system, token);
			if (tblRelation == null)
			{
				return;
			}
			tblRelation?.ChildList?.Where((TableRelationShipBusiness q) => q.SanadType != Document.FileManagement);
			if (value == null)
			{
				return;
			}
			foreach (Guid item in value)
			{
				await tblRelation.AddChildListAsync(new List<TableRelationShipBusiness>
				{
					new TableRelationShipBusiness(ContextGuid)
					{
						SanadGuid = item,
						SanadType = Document.FileManagement
					}
				});
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLinkedFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 403);
		}
	}

	public async Task<short> GetLinkCountAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<Guid> list = await GetLinkedFilesAsync(system, token);
			if (list != null && list.Count > 0)
			{
				_linkCount = checked((short)list.Count);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLinkCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 413);
		}
		return _linkCount.GetValueOrDefault();
	}

	public void SetLinkCount(short value)
	{
		_linkCount = value;
	}

	public async Task<TableRelationShipBusiness> GetLinkedSanadAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			if (system == EnSystem.Transfer)
			{
				return _linkedSanad;
			}
			if (_linkedSanad != null)
			{
				if (_linkedSanad.SanadType == Document.None)
				{
					_linkedSanad.SanadType = FactorType;
				}
				if (_linkedSanad.SanadGuid != Guid)
				{
					_linkedSanad.SanadGuid = Guid;
				}
				return _linkedSanad;
			}
			IEnumerable<TableRelationShipBusiness> relatedSanad = await TableRelationShipBusiness.GetAllRelatedSanadAsync(ContextGuid, Guid, FactorType, token, tranName);
			IEnumerable<TableRelationShipBusiness> enumerable = await TableRelationShipBusiness.GetAllInRelationSanadAsync(ContextGuid, Guid, FactorType, token, tranName);
			if (enumerable != null)
			{
				relatedSanad = relatedSanad?.Union(enumerable);
			}
			await AddListToLinkedSanadsAsync(relatedSanad);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetLinkedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 435);
		}
		return _linkedSanad;
	}

	public async Task<decimal> GetLinkedPardakhtSumAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		decimal ret = default(decimal);
		TableRelationShipBusiness tableRelationShipBusiness = await GetLinkedSanadAsync(system, token);
		if (tableRelationShipBusiness == null)
		{
			return ret;
		}
		foreach (TableRelationShipBusiness item in tableRelationShipBusiness?.ChildList ?? new List<TableRelationShipBusiness>())
		{
			if (item.SanadType == Document.Pardakht)
			{
				ret += item.Price;
			}
		}
		return ret;
	}

	public Task<CusCompactBusiness> GetCusInfoAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return CusCompactBusiness.GetAsync(ContextGuid, system, GetTafsilGuid(), null, tranName, token);
	}

	public Task<CusBusiness> GetCustomerBusinessAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return CusBusiness.GetAsync(ContextGuid, system, GetTafsilGuid(), token, tranName);
	}

	public Task<TafsilBusiness> GetTafsilAsync(EnSystem system = EnSystem.UserInterface, string tranName = "", CancellationToken token = default(CancellationToken), bool useCache = true)
	{
		if (!useCache)
		{
			return TafsilBusiness.GetFromDatabaseAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
		}
		return TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
	}

	public async Task<decimal?> GetEtebarReminAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			CusBusiness cusBusiness = await GetCustomerBusinessAsync(system, token, tranName);
			if (cusBusiness.Etebar == 0m)
			{
				return null;
			}
			decimal etebar = cusBusiness.Etebar;
			return etebar + await GetAfterAccountAsync(system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetEtebarReminAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 470);
			return null;
		}
	}

	public async Task<decimal?> GetChekReminAsync(EnSystem system, CancellationToken token, string tranName)
	{
		if (system == EnSystem.Transfer)
		{
			throw new InvalidCastException();
		}
		try
		{
			if ((await GetCustomerBusinessAsync(system, token, tranName)).EtebarCheck == 0m)
			{
				return null;
			}
			decimal SumCheck = default(decimal);
			decimal etebarCheck = (await GetCustomerBusinessAsync(system, token, tranName)).EtebarCheck;
			decimal num = etebarCheck + await GetAfterAccountAsync(system, token, tranName);
			return num - ((await GetReceptionAsync(system, tranName, fromDatabase: false, token))?.SumCheck ?? 0m) - SumCheck;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetChekReminAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 493);
			return null;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResult(EnSystem system, SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			int line = 0;
			try
			{
				_ = 2;
				try
				{
					line = 1;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 524), ReturnedState.None, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 524);
					}
					transactionHasBeenCreated = true;
					line = 5;
					if ((item != null) ? (!item.SaveResultStatus) : (!GetIsTemp() && autoTran))
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await RollBackBalance(system, tryCount, token, tranName), ReturnedState.None, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 529);
					}
					line = 10;
					if (ret.HasError)
					{
						return ret;
					}
					line = 11;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 534);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"isDisposed:{IsDisposed}//line:{line}", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 538);
					ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 539);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 544), ReturnedState.None, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 544);
				}
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> RollBackBalance(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 2;
			try
			{
				line = 1;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 559), ReturnedState.None, "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 559);
				}
				transactionHasBeenCreated = true;
				line = 2;
				await SetIsTempAsync(system, value: true, token, tranName);
				line = 3;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: true, this, tryCount, token, tranName), ReturnedState.None, "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 564);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).DatabaseManager is null:{ClsCache.GetCache(ContextGuid)?.DatabaseManager == null}", "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 568);
				ret.AddReturnedValue(ex, "", "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 569);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 574), ReturnedState.None, "RollBackBalance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 574);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, Document factorType, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.IOrder == null)
			{
				throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.IOrder");
			}
			int line = 0;
			try
			{
				_ = 3;
				try
				{
					throw new NotImplementedException("تابع Generic از کلاس Base باید صدا زده شود".GetTranslate());
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 671);
				}
				catch (Exception ex)
				{
					if (!ex.Message.ToLower().Contains("task was canceled".ToLower()))
					{
						WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.IOrder is null:{ClsCache.GetCache(ContextGuid)?.CacheRepository?.IOrder == null}", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 675);
					}
					ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 676);
				}
			}
			finally
			{
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 682), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 682);
				}
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IOrder;
	}

	public async Task<string> GetFullNameAsync(EnSystem system = EnSystem.UserInterface, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		string name = "";
		try
		{
			name = (await GetTafsilAsync(system, tranName, token))?.Name ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFullNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 703);
		}
		return name;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, 5);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			int line = 0;
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
			{
				return ret;
			}
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Order;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال فاکتورها".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 725);
				}
				line = 1000;
				if (ClsCache.GetCache(contextGuid)?.CacheRepository?.IOrder == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.IOrder");
				}
				List<OrderBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllNotSentAsync(token, tranName);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 730);
				}
				if (list != null && list.Count > 0)
				{
					if (uiUpdater != null)
					{
						uiUpdater.TotalCount = list?.Count ?? 0;
					}
					List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
					line = 2;
					int current = 0;
					ENSource source = ClsCache.GetCache(contextGuid).Source;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					foreach (OrderBusiness item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 740);
						}
						line = 10;
						if (item != null)
						{
							current = checked(current + 1);
							if (uiUpdater != null)
							{
								uiUpdater.Count = current;
							}
							line = 11;
							if (source == ENSource.WebSite)
							{
								throw new NotImplementedException();
							}
							line = 16;
							if (item != null)
							{
								tasks.Add(item.Send2ServerAsync(EnSystem.SignalR, "", "", 300000, token, "OrderBusiness/ResendNotSentsAsync/Source!=WebSite", tranName));
							}
							if (tasks.Count > maxParalelTasks)
							{
								Task<ReturnedSaveFuncInfo> completedTask = await Task.WhenAny(tasks);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await completedTask, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 770);
								tasks.Remove(completedTask);
							}
						}
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 779);
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 780);
					line = 25;
				}
				line = 30;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 788);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 789);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	private async Task SetB1MoeinGuidAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if ((FactorType != Document.PishFactor && FactorType != Document.OrderBackSell && FactorType != Document.OrderSell) || IsDisposed || token.IsCancellationRequested || (_b1TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, B1TafsilGuid.Value, tranName, token, "SetB1MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			if (IsDisposed || token.IsCancellationRequested || tafsilBusiness == null)
			{
				return;
			}
			TafsilGroupBusiness tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(system, token, tranName);
			if (!IsDisposed && !token.IsCancellationRequested && tafsilGroupBusiness != null)
			{
				if (FactorType == Document.OrderBackSell)
				{
					B1MoeinGuid = tafsilGroupBusiness.MoeinBazaryabBackSell1;
				}
				else if (FactorType == Document.OrderSell || FactorType == Document.PishFactor)
				{
					B1MoeinGuid = tafsilGroupBusiness.MoeinBazaryabSell1;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB1MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 853);
		}
	}

	private async Task SetB2MoeinGuidAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if ((FactorType != Document.PishFactor && FactorType != Document.OrderBackSell && FactorType != Document.OrderSell) || IsDisposed || token.IsCancellationRequested || (_b2TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, B2TafsilGuid.Value, tranName, token, "SetB2MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			if (IsDisposed || token.IsCancellationRequested || tafsilBusiness == null)
			{
				return;
			}
			TafsilGroupBusiness tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(system, token, tranName);
			if (!IsDisposed && !token.IsCancellationRequested && tafsilGroupBusiness != null)
			{
				if (FactorType == Document.OrderBackSell)
				{
					B2MoeinGuid = tafsilGroupBusiness.MoeinBazaryabBackSell2;
				}
				else if (FactorType == Document.OrderSell || FactorType == Document.PishFactor)
				{
					B2MoeinGuid = tafsilGroupBusiness.MoeinBazaryabSell2;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB2MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 873);
		}
	}

	private async Task SetB3MoeinGuidAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if ((FactorType != Document.PishFactor && FactorType != Document.OrderBackSell && FactorType != Document.OrderSell) || IsDisposed || token.IsCancellationRequested || (_b3TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, B3TafsilGuid.Value, "", token, "SetB3MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			if (IsDisposed || token.IsCancellationRequested || tafsilBusiness == null)
			{
				return;
			}
			TafsilGroupBusiness tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(system, token);
			if (!IsDisposed && !token.IsCancellationRequested && tafsilGroupBusiness != null)
			{
				if (FactorType == Document.OrderBackSell)
				{
					B3MoeinGuid = tafsilGroupBusiness.MoeinBazaryabBackSell3;
				}
				else if (FactorType == Document.OrderSell || FactorType == Document.PishFactor)
				{
					B3MoeinGuid = tafsilGroupBusiness.MoeinBazaryabSell3;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB3MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 893);
		}
	}

	private async Task SetB4MoeinGuidAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if ((FactorType != Document.PishFactor && FactorType != Document.OrderBackSell && FactorType != Document.OrderSell) || IsDisposed || token.IsCancellationRequested || (_b4TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, B4TafsilGuid.Value, "", token, "SetB4MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			if (IsDisposed || token.IsCancellationRequested || tafsilBusiness == null)
			{
				return;
			}
			TafsilGroupBusiness tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(system, token);
			if (!IsDisposed && !token.IsCancellationRequested && tafsilGroupBusiness != null)
			{
				if (FactorType == Document.OrderBackSell)
				{
					B4MoeinGuid = tafsilGroupBusiness.MoeinBazaryabBackSell4;
				}
				else if (FactorType == Document.OrderSell || FactorType == Document.PishFactor)
				{
					B4MoeinGuid = tafsilGroupBusiness.MoeinBazaryabSell4;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB4MoeinGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 913);
		}
	}

	public async Task SetB1TafsilGuidAsync(Guid? value, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_b1TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
			_setB1MoeinGuid?.Cancel();
			_setB1MoeinGuid = new CancellationTokenSource();
			CancellationToken token2 = CancellationTokenSource.CreateLinkedTokenSource(_setB1MoeinGuid.Token, token).Token;
			await SetB1MoeinGuidAsync(system, token2, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB1TafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 930);
		}
	}

	public async Task SetB2TafsilGuidAsync(Guid? value, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			_b2TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
			_setB2MoeinGuid?.Cancel();
			_setB2MoeinGuid = new CancellationTokenSource();
			CancellationToken token2 = CancellationTokenSource.CreateLinkedTokenSource(_setB2MoeinGuid.Token, token).Token;
			await SetB2MoeinGuidAsync(system, token2, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB2TafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 943);
		}
	}

	public async Task SetB3TafsilGuidAsync(Guid? value, EnSystem system)
	{
		try
		{
			_b3TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
			_setB3MoeinGuid?.Cancel();
			_setB3MoeinGuid = new CancellationTokenSource();
			CancellationToken token = _setB3MoeinGuid.Token;
			await SetB3MoeinGuidAsync(system, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB3TafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 955);
		}
	}

	public async Task SetB4TafsilGuidAsync(Guid? value, EnSystem system)
	{
		try
		{
			_b4TafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
			_setB4MoeinGuid?.Cancel();
			_setB4MoeinGuid = new CancellationTokenSource();
			CancellationToken token = _setB4MoeinGuid.Token;
			await SetB4MoeinGuidAsync(system, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetB4TafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 967);
		}
	}

	public async Task<int> GetId2Async(EnSystem system = EnSystem.UserInterface, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (_Id2 <= 0)
				{
					_Id2 = await NextId2Async(ContextGuid, FactorType, Status, StatusId, token, tranName);
					return _Id2;
				}
				OrderReportBusiness orderReportBusiness = await GetReportById2Async(ContextGuid, _Id2, FactorType, Status, token, tranName);
				if (orderReportBusiness == null || orderReportBusiness.Guid == Guid || orderReportBusiness.Status != Status)
				{
					return _Id2;
				}
				_Id2 = await NextId2Async(ContextGuid, FactorType, Status, StatusId, token, tranName);
				return _Id2;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 999);
				throw;
			}
		}
	}

	public void SetId2(int value, CancellationToken token = default(CancellationToken))
	{
		_Id2 = value;
	}

	public static Task<Guid> Id2ToGuidAsync(string contextGuid, EnSystem system, int id2, Document factorType, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return GetById2Async(true, token, tranName, contextGuid, id2, factorType);
	}

	public async Task<decimal> GetBeforeAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 4;
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			if (system == EnSystem.UserInterface && source == ENSource.Accounting)
			{
				Guid guid = Guid;
				_beforeAccount = await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.GetBeforeAccountAsync(GetTafsilGuid(), guid, FactorType, token, tranName);
				if (_oldOrder == null)
				{
					_oldOrder = (OrderBusiness)(await GetOldItemAsync(system, token, tranName));
				}
				if (_oldOrder != null && _oldOrder.GetDate() < GetDate())
				{
					_beforeAccount -= (decimal?)_oldOrder.Sum;
				}
			}
			else if (source != ENSource.Accounting)
			{
				if (ChangeStatus == EntityChangeSt.New)
				{
					_beforeAccount = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, tranName, token);
				}
				else if (ChangeStatus == EntityChangeSt.Edited)
				{
					if (_oldOrder == null)
					{
						_oldOrder = (OrderBusiness)(await GetOldItemAsync(system, token, tranName));
					}
					if (!IsTemp && !(_oldOrder?.IsTemp ?? true))
					{
						return _beforeAccount.Value;
					}
					_beforeAccount = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, tranName, token);
				}
			}
		}
		catch (OperationCanceledException)
		{
			_beforeAccount = null;
		}
		catch (Exception ex2)
		{
			_beforeAccount = null;
			WebErrorLog.Instance.StartLog(ex2, "", "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1191);
		}
		return _beforeAccount.GetValueOrDefault();
	}

	public static async Task<OrderBusiness> FirstOrderAsync(string contextGuid, Document factorType, DateTime date, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderBusiness ret = null;
		int line = 0;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			line = 1;
			if (cashier == null)
			{
				throw new NullReferenceException("cashier is null!!");
			}
			line = 2;
			UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token, tranName);
			line = 3;
			if (userAccBusiness == null)
			{
				throw new NullReferenceException("user is null!!");
			}
			line = 4;
			Guid? cashierGuid = null;
			line = 5;
			switch (factorType)
			{
			case Document.OrderSell:
				line = 60;
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && !userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			case Document.OrderBackSell:
				line = 70;
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && !userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			default:
				throw new NotImplementedException($"GetPreviewsAsync has not implemented for order type({factorType})");
			}
			ret = await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetFirstOrderAsync(factorType, date, cashierGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "FirstOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1230);
		}
		return ret;
	}

	public static async Task<OrderBusiness> GetPreviewsAsync(string contextGuid, EnSystem userInterface, Guid guid, Document factorType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderBusiness ret = null;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashier == null)
			{
				throw new NullReferenceException("cashier is null!!");
			}
			UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token, tranName);
			if (userAccBusiness == null)
			{
				throw new NullReferenceException("user is null!!");
			}
			Guid? cashierGuid = null;
			switch (factorType)
			{
			case Document.OrderSell:
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && !userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			case Document.OrderBackSell:
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && !userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			default:
				throw new NotImplementedException($"GetPreviewsAsync has not implemented for order type({factorType})");
			}
			ret = await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetPreviewsOrderAsync(factorType, guid, cashierGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPreviewsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1270);
		}
		return ret;
	}

	public static async Task<OrderBusiness> GetNextAsync(string contextGuid, EnSystem userInterface, Guid guid, Document factorType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderBusiness ret = null;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashier == null)
			{
				throw new NullReferenceException("cashier is null!!");
			}
			UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token, tranName);
			if (userAccBusiness == null)
			{
				throw new NullReferenceException("user is null!!");
			}
			Guid? cashierGuid = null;
			switch (factorType)
			{
			case Document.OrderSell:
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && !userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			case Document.OrderBackSell:
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && !userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			default:
				throw new NotImplementedException($"GetPreviewsAsync has not implemented for order type({factorType})");
			}
			ret = await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetNextOrderAsync(factorType, guid, cashierGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1301);
		}
		return ret;
	}

	public static async Task<OrderBusiness> GetLastOrderAsync(string contextGuid, Document orderType, DateTime date, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderBusiness ret = null;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid);
			if (cashier == null)
			{
				throw new NullReferenceException("cashier is null!!");
			}
			UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token, tranName);
			if (userAccBusiness == null)
			{
				throw new NullReferenceException("user is null!!");
			}
			Guid? cashierGuid = null;
			switch (orderType)
			{
			case Document.OrderSell:
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && !userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.Sell.Sell_Report && userAccBusiness.UserAccess.Sell.Sell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			case Document.OrderBackSell:
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && !userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					return null;
				}
				if (!userAccBusiness.UserAccess.BackSell.BackSell_Report && userAccBusiness.UserAccess.BackSell.BackSell_ReportSelf)
				{
					cashierGuid = cashier.Guid;
				}
				break;
			default:
				throw new NotImplementedException($"GetPreviewsAsync has not implemented for order type({orderType})");
			}
			ret = await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetLastOrderAsync(orderType, date, cashierGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1331);
		}
		return ret;
	}

	public async Task<decimal> GetAfterAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			_afterAccount = await GetBeforeAccountAsync(system, token, tranName) + await RemAsync(system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAfterAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1340);
		}
		return _afterAccount;
	}

	public void SetAfterAccount(decimal value)
	{
		_afterAccount = value;
	}

	public void SetBeforeAccount(decimal value)
	{
		_beforeAccount = value;
	}

	public override DateTime GetDate()
	{
		if (_ChangeDate)
		{
			return _Date;
		}
		return DateManagment.DateM;
	}

	public async Task SetDateAsync(DateTime value, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SetDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				_ChangeDate = true;
				if (_Date == value)
				{
					return;
				}
				_Date = value;
				if (system != EnSystem.Transfer && system != EnSystem.Mapper)
				{
					ReceptionBusiness receptionBusiness = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
					if (receptionBusiness != null)
					{
						receptionBusiness.Date = _Date;
					}
					if (await GetPardakhtAsync(system, token, tranName) != null)
					{
						(await GetPardakhtAsync(system, token, tranName)).Date = _Date;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1366);
			}
		}
	}

	public Guid GetTafsilGuid()
	{
		return _tafsilGuid;
	}

	public async Task SetTafsilGuid(Guid value, EnSystem system, bool updateVisitorGuid = false, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		try
		{
			line = 1;
			if (value != Guid.Empty && _tafsilGuid != value)
			{
				if (system == EnSystem.UserInterface)
				{
					await GetBeforeAccountAsync(system, token, tranName);
				}
				line = 20;
				CusBusiness cus = await CusBusiness.GetAsync(ContextGuid, system, value, token, tranName);
				if (MoeinGuid == Guid.Empty && cus != null && FactorType != Document.Zayeat)
				{
					CusGroupBusiness cusGroupBusiness = await cus.GetGroupAsync(token, tranName);
					if (cusGroupBusiness != null)
					{
						MoeinGuid = (await cusGroupBusiness.GetTafsilGroupAsync(system, token, tranName))?.GetOrderMoeinGuid(FactorType) ?? Guid.Empty;
					}
				}
				line = 1000;
				CustomerRoleBusiness customerRoleBusiness = null;
				if (cus != null)
				{
					customerRoleBusiness = await cus.GetRoleAsync(token, tranName);
				}
				if (FactorType == Document.OrderBuy || FactorType == Document.OrderBackBuy || FactorType == Document.OrderPreBuy || FactorType == Document.AmaniVorod)
				{
					PriceTypeGuid = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token))?.Buy?.DefaultPriceLevelGuid;
				}
				else
				{
					PriceTypeGuid = customerRoleBusiness?.PriceTypeGuid;
				}
				line = 1001;
				if (updateVisitorGuid)
				{
					line = 10020;
					if (cus == null)
					{
						throw new NullReferenceException($"Cus is null/Guid:{GetTafsilGuid()}/value:{value}");
					}
					Guid guid = await cus.GetVisitorGuidAsync(UserAccGuid, token, tranName);
					if (guid == Guid.Empty)
					{
						guid = cus.SupervisorGuid;
					}
					line = 1003;
					if (guid != Guid.Empty && (B1TafsilGuid ?? Guid.Empty) == Guid.Empty)
					{
						line = 1004;
						await SetB1TafsilGuidAsync(guid, system, token, tranName);
					}
					line = 201;
					CusBusiness cusBusiness = await CusBusiness.GetAsync(ContextGuid, system, B1TafsilGuid ?? Guid.Empty, token, tranName);
					Guid guid2 = cusBusiness?.SupervisorGuid ?? Guid.Empty;
					if (cusBusiness != null && guid2 == Guid.Empty)
					{
						guid2 = await cusBusiness.GetVisitorGuidAsync(null, token, tranName);
					}
					line = 202;
					if (guid2 != Guid.Empty)
					{
						line = 203;
						await SetB2TafsilGuidAsync(guid2, system, token, tranName);
					}
					line = 30;
				}
				if (this.OnChanged != null)
				{
					PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
				if (eventManager?.OnChangedTafsilGuid != null)
				{
					PacketParser.Utilities.RaiseEvent(eventManager.OnChangedTafsilGuid, this, new EventArgs(), token, 60, "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
			line = 31;
			_tafsilGuid = value;
			if (system != EnSystem.Transfer && system != EnSystem.Mapper)
			{
				ReceptionBusiness receptionBusiness = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
				if (receptionBusiness != null)
				{
					await receptionBusiness.SetTafsilGuidAsync(value, system, token, tranName);
				}
				PardakhtBusiness pardakhtBusiness = await GetPardakhtAsync(system, token, tranName);
				if (pardakhtBusiness != null)
				{
					await pardakhtBusiness.SetTafsilGuidAsync(value, system, token, tranName);
				}
			}
			line = 40;
			TafsilBusiness tafsilBusiness = await GetTafsilAsync(system, tranName, token);
			if (MoeinGuid == Guid.Empty && tafsilBusiness != null)
			{
				TafsilGroupBusiness tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(system, token, tranName);
				switch (FactorType)
				{
				case Document.OrderSell:
					line = 50;
					MoeinGuid = tafsilGroupBusiness?.MoeinSellGuid ?? Guid.Empty;
					break;
				case Document.PishFactor:
					line = 55;
					MoeinGuid = tafsilGroupBusiness?.MoeinSellGuid ?? Guid.Empty;
					break;
				case Document.OrderBackBuy:
					line = 60;
					MoeinGuid = tafsilGroupBusiness?.MoeinBackBuyGuid ?? Guid.Empty;
					break;
				case Document.OrderBackSell:
					line = 65;
					MoeinGuid = tafsilGroupBusiness?.MoeinBackSellGuid ?? Guid.Empty;
					break;
				case Document.OrderBuy:
					line = 70;
					MoeinGuid = tafsilGroupBusiness?.MoeinBuyGuid ?? Guid.Empty;
					break;
				case Document.OrderPreBuy:
					line = 700;
					MoeinGuid = tafsilGroupBusiness?.MoeinBuyGuid ?? Guid.Empty;
					break;
				case Document.Zayeat:
					line = 71;
					if (tafsilGroupBusiness != null)
					{
						MoeinGuid = await tafsilGroupBusiness.GetMoeinGuidAsync(token, tranName);
					}
					break;
				case Document.AmaniKhoroj:
					line = 72;
					MoeinGuid = tafsilGroupBusiness?.MoeinAmaniMaNazdDigaranGuid ?? Guid.Empty;
					break;
				case Document.AmaniVorod:
					line = 72;
					MoeinGuid = tafsilGroupBusiness?.MoeinAmaniDigaranNazdMaGuid ?? Guid.Empty;
					break;
				default:
					line = 75;
					throw new ArgumentException("برای نوع".GetTranslate() + " " + $" {FactorType}" + " " + " تصمیم گیری انجام نشده است.".GetTranslate());
				}
			}
			if (MoeinGuid == Guid.Empty)
			{
				switch (FactorType)
				{
				case Document.OrderSell:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					break;
				case Document.PishFactor:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					break;
				case Document.OrderBackBuy:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					break;
				case Document.OrderBackSell:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					break;
				case Document.OrderBuy:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					break;
				case Document.OrderPreBuy:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
					break;
				case Document.AmaniKhoroj:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29995;
					break;
				case Document.AmaniVorod:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein29994;
					break;
				case Document.Zayeat:
					MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein11;
					break;
				default:
					throw new ArgumentException("برای نوع".GetTranslate() + " " + $" {FactorType}" + " " + " تصمیم گیری انجام نشده است.".GetTranslate());
				case Document.Tolid:
				case Document.Tolid_Hazine:
				case Document.Tolid_Mahsul:
				case Document.Tolid_Mavad:
				case Document.TolidFormula:
					break;
				}
			}
			line = 80;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SetTafsilGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1588);
		}
	}

	public void SetKeraye(decimal value)
	{
		checked
		{
			try
			{
				if (value != 0m)
				{
					decimal num = default(decimal);
					List<OrderDetailBusiness> list = ListKala.OrderBy((OrderDetailBusiness q) => q.Ordinal)?.ToList();
					if (list != null && list.Any())
					{
						for (int i = 0; i < list.Count - 1; i++)
						{
							decimal num2 = Math.Round(list[i].RowSum / SumKala, DigitForRound);
							list[i].FarePrice = num2 * value;
							num += list[i].FarePrice;
						}
						list[list.Count - 1].FarePrice = value - num;
					}
				}
				else
				{
					foreach (OrderDetailBusiness item in ListKala)
					{
						item.FarePrice = 0m;
					}
				}
				if (_keraye.SetPrice(value) && eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
				{
					_lastSum = TotalSum;
					PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, default(CancellationToken), 60, "SetKeraye", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetKeraye", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1620);
			}
		}
	}

	public async Task<decimal> PardakhtSumAsync(EnSystem system = EnSystem.UserInterface, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (system != EnSystem.Mapper)
			{
				return (await GetPardakhtAsync(system, token, tranName))?.Sum ?? 0m;
			}
			if ((PardakhtGuid ?? Guid.Empty) == Guid.Empty)
			{
				return default(decimal);
			}
			return _pardakht?.Sum ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PardakhtSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1666);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetPriceTypeAsync(EnSystem system, Guid priceLevelGuid, ReturnedState? state = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			PriceTypeGuid = priceLevelGuid;
			IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
			if (listKala != null && listKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await (item?.SetPriceTypeAsync(system, priceLevelGuid, item.Price, state, token)), ReturnedState.None, "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1678);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1682);
			res.AddReturnedValue(ex, "", "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1683);
		}
		return res;
	}

	public async Task<decimal> ReceptionSumAsync(EnSystem system = EnSystem.UserInterface, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetReceptionAsync(system, tranName, fromDatabase: false, token))?.Sum ?? 0m;
	}

	public override async Task<int> GetSanadNumberAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetSanadNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					return 0;
				}
				if (SanadNo == 0)
				{
					return await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
				}
				return SanadNo;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1715);
			}
			return SanadNo;
		}
	}

	public override bool GetIsTemp()
	{
		if (!_isTemp && ServerStatus != ServerStatusForClient.DeliveryError)
		{
			return ServerStatus == ServerStatusForClient.SendError;
		}
		return true;
	}

	public async Task<ReturnedSaveFuncInfo> SetIsTempAsync(EnSystem system, bool value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SetIsTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_isTemp = value;
				if (system == EnSystem.Transfer || system == EnSystem.Mapper)
				{
					return ret;
				}
				ReceptionBusiness receptionBusiness = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
				if (receptionBusiness != null)
				{
					receptionBusiness.IsTemp = value;
				}
				if (await GetPardakhtAsync(system, token, tranName) != null)
				{
					(await GetPardakhtAsync(system, token, tranName)).IsTemp = value;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetIsTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1751);
				ret.AddReturnedValue(ex, "", "SetIsTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1752);
			}
			return ret;
		}
	}

	public async Task<decimal> CalcSoodAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			decimal soodSum = default(decimal);
			if (ListKala != null && ListKala.Count > 0)
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					if (item != null)
					{
						string contextGuid = ContextGuid;
						Guid productGuid = item.ProductGuid;
						decimal num = await ProductCompactBusiness.LastVorodPriceAvgAsync(contextGuid, productGuid, await item.GetDepotGuidAsync(token), GetDate(), token);
						decimal num2 = Math.Round(item.Quantity * (item.Price - num)) - item.Discount - item.TotalVisitorPrice;
						soodSum += num2;
					}
				}
			}
			return soodSum;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalcSoodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1784);
			return default(decimal);
		}
	}

	public async Task<TolidBusiness> GenerateTolidAsync(EnSystem system, TolidFormulaBusiness formul, decimal quantity, CancellationToken token, string tranName)
	{
		_ = 5;
		try
		{
			if (_tolid == null)
			{
				int tolidId2 = 0;
				int tolidSanadNo = 0;
				_tolid = await TolidBusiness.GetAsync(ContextGuid, Guid, token, tranName);
				if (_tolid != null)
				{
					tolidId2 = await _tolid.GetId2Async(token, tranName);
					tolidSanadNo = _tolid.SanadNo;
				}
				TafsilBusiness tafsil = await GetTafsilAsync(system, tranName, token);
				_tolidQuantity = quantity;
				string translate = "تولید خودکار به واسطه فاکتور فروش شماره".GetTranslate();
				string desc = translate + $" ({await GetId2Async(system, tranName, token)}) ({tafsil?.Name})";
				_tolid = await TolidBusiness.GenerateToliBusiness(ContextGuid, system, formul, quantity, checkMovazene: true, token, tranName, Guid, desc, GetDate().AddSeconds(-1.0), tolidId2, tolidSanadNo);
			}
			else if ((_tolid != null && (_tolid?.ExportFormulaGuid ?? Guid.Empty) != formul.Guid) || _tolidQuantity != quantity)
			{
				await _tolid.MergeAsync(system, formul, quantity, token, tranName);
			}
			return _tolid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1904);
			throw;
		}
	}

	public async Task SetTolidAsync(TolidBusiness value, Guid? exportGuid, EnSystem system, TolidFormulaBusiness formul, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			if (_tolid == null)
			{
				_tolid = await TolidMapper.Instance.MapAsync(ContextGuid, value, _tolid, tranName, token);
			}
			if (_tolidQuantity <= 0m && _tolid != null)
			{
				_tolidQuantity = ((await _tolid.GetExportListAsync(system, tranName, token))?.ListKala?.Where((OrderDetailBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid)?.Sum((OrderDetailBusiness q) => q.Quantity)).GetValueOrDefault();
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (_tolid.MahsolMoeinGuid == Guid.Empty)
			{
				_tolid.MahsolMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportMoeinGuid;
				_tolid.MahsolTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidExportTafsilGuid;
			}
			if (_tolid.MavadMoeinGuid == Guid.Empty)
			{
				_tolid.MavadMoeinGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadMoeinGuid;
				_tolid.MavadTafsilGuid = accGlobalSettings.CodeSetting.TolidCoding.TolidMavadTafsilGuid;
			}
			OrderMapper instance = OrderMapper.Instance;
			string contextGuid = ContextGuid;
			OrderBusiness tolidExist = await instance.MapAsync(contextGuid, await _tolid.GetExportListAsync(system, tranName, token), new OrderBusiness(ContextGuid), tranName, token);
			instance = OrderMapper.Instance;
			contextGuid = ContextGuid;
			OrderBusiness export = await instance.MapAsync(contextGuid, await value.GetExportListAsync(system, tranName, token), new OrderBusiness(ContextGuid), tranName, token);
			instance = OrderMapper.Instance;
			contextGuid = ContextGuid;
			OrderBusiness mavad = await instance.MapAsync(contextGuid, await value.GetMavadListAsync(system, tranName, token), new OrderBusiness(ContextGuid), tranName, token);
			List<TolidHazineBusiness> hazine = await TolidHazineMapper.Instance.MapListAsync(ContextGuid, value.HazineList as List<TolidHazineBusiness>, null, tranName, token);
			if (exportGuid.HasValue)
			{
				IReadOnlyList<OrderDetailBusiness> listKala = tolidExist.ListKala;
				bool? obj;
				if (listKala == null)
				{
					obj = null;
				}
				else
				{
					IEnumerable<Guid> enumerable = listKala.Select((OrderDetailBusiness q) => q.ProductGuid);
					obj = ((enumerable != null) ? new bool?(!enumerable.Contains(exportGuid.Value)) : null);
				}
				bool? flag = obj;
				if (flag.GetValueOrDefault())
				{
					await _tolid.AddExportListAsync(system, export.ListKala?.Where((OrderDetailBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid), clearDets: false, tranName, token);
					await _tolid.AddMavadListAsync(system, mavad.ListKala?.Where((OrderDetailBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid), clearDets: false, tranName, token);
					await _tolid.AddHazineListAsync((value.HazineList as List<TolidHazineBusiness>)?.Where((TolidHazineBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid), system, token);
					return;
				}
			}
			if (formul == null)
			{
				await _tolid.AddExportListAsync(system, export.ListKala, clearDets: true, tranName, token);
				await _tolid.AddMavadListAsync(system, mavad.ListKala, clearDets: true, tranName, token);
				return;
			}
			await _tolid.ClearListDetByFormulaAsync(formul, system, token, tranName);
			await _tolid.AddExportListAsync(system, export.ListKala?.Where((OrderDetailBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid), clearDets: false, tranName, token);
			await _tolid.AddMavadListAsync(system, mavad.ListKala?.Where((OrderDetailBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid), clearDets: false, tranName, token);
			await _tolid.AddHazineListAsync(hazine?.Where((TolidHazineBusiness q) => (formul?.Guid ?? Guid.Empty) == Guid.Empty || (q?.TolidFormulaGuid ?? Guid.Empty) == formul.Guid), system, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 1964);
			throw;
		}
		finally
		{
			if (this.OnChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "SetTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
			if (eventManager?.OnTolidChanged_static != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnTolidChanged_static, this, EventArgs.Empty, token, 60, "SetTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
		}
	}

	public int NextOrdinal()
	{
		return ConnectedSanad.NextOrdinal();
	}

	public static async Task<OrderBusiness> OrderFactoryAsync(GardeshHesabBusiness hostGardesh, EnSystem system, bool isLoadOriginalPrice, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			OrderBusiness order = new OrderBusiness(hostGardesh, token, tranName);
			decimal fare = default(decimal);
			foreach (GardeshTafsilBusiness tafsiliItem in hostGardesh.TafsiliItems)
			{
				switch ((DocumentDet)tafsiliItem.AsType)
				{
				case DocumentDet.SellDiscount:
				case DocumentDet.BuyDiscount:
				case DocumentDet.BargashtBuyDiscount:
				case DocumentDet.BargashtSellDiscount:
					order.Discount += Math.Abs(tafsiliItem.Credit - tafsiliItem.Debit);
					break;
				case DocumentDet.SellKeraye:
				case DocumentDet.BuyKeraye:
				case DocumentDet.BargashtBuyKeraye:
				case DocumentDet.BargashtSellKeraye:
					fare += Math.Abs(tafsiliItem.Debit - tafsiliItem.Credit);
					break;
				case DocumentDet.SellPrd:
				case DocumentDet.BuyPrd:
				case DocumentDet.BargashtBuyPrd:
				case DocumentDet.BargashtSellPrd:
					await order.AddDetAsync(tafsiliItem, system, isLoadOriginalPrice, awaitOnSemaphor: true, token);
					break;
				}
			}
			order.SetKeraye(fare);
			return order;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OrderFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2011);
		}
		return null;
	}

	private OrderBusiness(GardeshHesabBusiness hostGardesh, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderBusiness orderBusiness = this;
		if (hostGardesh == null)
		{
			throw new NullReferenceException("hostGardesh");
		}
		try
		{
			ContextGuid = hostGardesh?.ContextGuid;
			Guid = hostGardesh.AsGuid.Value;
			FactorType = hostGardesh.AsType;
			Description = hostGardesh.SanadDesc;
			Task.Run(() => orderBusiness.SetDateAsync(hostGardesh.Date, EnSystem.UserInterface, token, tranName));
			SetId2(hostGardesh.Id2);
			UserAccGuid = hostGardesh.UGuid;
			Task.Run(() => orderBusiness.SetTafsilGuid(hostGardesh.TafsilGuid.Value, EnSystem.UserInterface, updateVisitorGuid: true, token, tranName));
			ChangeStatus = EntityChangeSt.Edited;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
			DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
			IsFromGardesh = true;
			_discount = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
			_keraye = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2037);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> SetHazineAsync(EnSystem system, decimal hazine1, decimal hazine2, decimal hazine3, decimal hazine4, decimal hazine5)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (!HazineMoeinGuid1.HasValue && HazineTafsilGuid1.HasValue)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا حساب معین هزینه".GetTranslate() + "1" + "را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2047);
				}
				if (!HazineMoeinGuid2.HasValue && HazineTafsilGuid2.HasValue)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا حساب معین هزینه".GetTranslate() + "2" + "را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2048);
				}
				if (!HazineMoeinGuid3.HasValue && HazineTafsilGuid3.HasValue)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا حساب معین هزینه".GetTranslate() + "3" + "را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2049);
				}
				if (!HazineMoeinGuid4.HasValue && HazineTafsilGuid4.HasValue)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا حساب معین هزینه".GetTranslate() + "4" + "را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2050);
				}
				if (!HazineMoeinGuid5.HasValue && HazineTafsilGuid5.HasValue)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا حساب معین هزینه".GetTranslate() + "5" + "را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2051);
				}
				if (!HazineMoeinGuid1.HasValue && hazine1 != 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا طرف حساب بستانکار هزینه".GetTranslate() + "1" + " را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2053);
				}
				if (!HazineMoeinGuid2.HasValue && hazine2 != 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا طرف حساب بستانکار هزینه".GetTranslate() + "2" + " را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2054);
				}
				if (!HazineMoeinGuid3.HasValue && hazine3 != 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا طرف حساب بستانکار هزینه".GetTranslate() + "3" + " را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2055);
				}
				if (!HazineMoeinGuid4.HasValue && hazine4 != 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا طرف حساب بستانکار هزینه".GetTranslate() + "4" + " را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2056);
				}
				if (!HazineMoeinGuid5.HasValue && hazine5 != 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا طرف حساب بستانکار هزینه".GetTranslate() + "5" + " را انتخاب نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2057);
				}
				if (HazineMoeinGuid1.HasValue && hazine1 == 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا مبلغ هزینه".GetTranslate() + "1" + "+را وارد نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2059);
				}
				if (HazineMoeinGuid2.HasValue && hazine2 == 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا مبلغ هزینه".GetTranslate() + "2" + "+را وارد نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2060);
				}
				if (HazineMoeinGuid3.HasValue && hazine3 == 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا مبلغ هزینه".GetTranslate() + "3" + "+را وارد نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2061);
				}
				if (HazineMoeinGuid4.HasValue && hazine4 == 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا مبلغ هزینه".GetTranslate() + "4" + "+را وارد نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2062);
				}
				if (HazineMoeinGuid5.HasValue && hazine5 == 0m)
				{
					returnedSaveFuncInfo.AddError(system, "لطفا مبلغ هزینه".GetTranslate() + "5" + "+را وارد نمایید".GetTranslate(), "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2063);
				}
				if (returnedSaveFuncInfo.HasError)
				{
					return Task.FromResult(returnedSaveFuncInfo);
				}
				if (hazine1 != 0m)
				{
					for (int i = 0; i < ListKala.Count - 1; i++)
					{
						decimal num = Math.Round(ListKala[i].RowSum / SumKala, DigitForRound);
						ListKala[i].Hazine1Price = num * hazine1;
					}
					ListKala[ListKala.Count - 1].Hazine1Price = 0m;
					ListKala[ListKala.Count - 1].Hazine1Price = hazine1 - TotalHazine1;
				}
				else
				{
					foreach (OrderDetailBusiness item in ListKala)
					{
						item.Hazine1Price = 0m;
					}
				}
				if (hazine2 != 0m)
				{
					for (int j = 0; j < ListKala.Count - 1; j++)
					{
						decimal num2 = Math.Round(ListKala[j].RowSum / SumKala, DigitForRound);
						ListKala[j].Hazine2Price = num2 * hazine2;
					}
					ListKala[ListKala.Count - 1].Hazine2Price = 0m;
					ListKala[ListKala.Count - 1].Hazine2Price = hazine2 - TotalHazine2;
				}
				else
				{
					foreach (OrderDetailBusiness item2 in ListKala)
					{
						item2.Hazine2Price = 0m;
					}
				}
				if (hazine3 != 0m)
				{
					for (int k = 0; k < ListKala.Count - 1; k++)
					{
						decimal num3 = Math.Round(ListKala[k].RowSum / SumKala, DigitForRound);
						ListKala[k].Hazine3Price = num3 * hazine3;
					}
					ListKala[ListKala.Count - 1].Hazine3Price = 0m;
					ListKala[ListKala.Count - 1].Hazine3Price = hazine3 - TotalHazine3;
				}
				else
				{
					foreach (OrderDetailBusiness item3 in ListKala)
					{
						item3.Hazine3Price = 0m;
					}
				}
				if (hazine4 != 0m)
				{
					for (int l = 0; l < ListKala.Count - 1; l++)
					{
						decimal num4 = Math.Round(ListKala[l].RowSum / SumKala, DigitForRound);
						ListKala[l].Hazine4Price = num4 * hazine4;
					}
					ListKala[ListKala.Count - 1].Hazine4Price = 0m;
					ListKala[ListKala.Count - 1].Hazine4Price = hazine4 - TotalHazine4;
				}
				else
				{
					foreach (OrderDetailBusiness item4 in ListKala)
					{
						item4.Hazine4Price = 0m;
					}
				}
				if (hazine5 != 0m)
				{
					for (int m = 0; m < ListKala.Count - 1; m++)
					{
						decimal num5 = Math.Round(ListKala[m].RowSum / SumKala, DigitForRound);
						ListKala[m].Hazine5Price = num5 * hazine5;
					}
					ListKala[ListKala.Count - 1].Hazine5Price = 0m;
					ListKala[ListKala.Count - 1].Hazine5Price = hazine5 - TotalHazine5;
				}
				else
				{
					foreach (OrderDetailBusiness item5 in ListKala)
					{
						item5.Hazine5Price = 0m;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2139);
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetHazineAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2140);
			}
			return Task.FromResult(returnedSaveFuncInfo);
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, OrderRemoveFilter filter, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			OrderBusiness old = null;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			LockItem lockItem = await _removeSemaphores.GetItemAsync(Guid.ToString(), "", ClsCache.GetCache(ContextGuid)?.BufferCancellationToken ?? default(CancellationToken), token);
			try
			{
				int num;
				_ = num - 1;
				_ = 24;
				ReturnedSaveFuncInfo result;
				try
				{
					using (await lockItem.Semaphore.GaurdAsync(true, "OrderBusiness/RemoveAsync", 60000, token))
					{
						if (autoCommit)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2158), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2158);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2159), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2159);
							transactionHasBeenCreated = true;
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: true, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2163);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								Status = false;
								old = (OrderBusiness)(await GetOldItemAsync(system, token, tranName));
								OrderReportBusiness orderReportBusiness = await GetReportById2Async(ContextGuid, _Id2, FactorType, status: false, token, tranName);
								if (orderReportBusiness != null && orderReportBusiness.Guid != Guid && !orderReportBusiness.Status)
								{
									SetId2(await NextId2Async(ContextGuid, FactorType, false, StatusId, token, tranName));
								}
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.RemoveAsync(this, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2175);
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									if (!ret.HasError && !GetIsTemp() && FactorType != Document.PishFactor && FactorType != Document.OrderPreBuy)
									{
										SanadBusiness sanadBusiness = await GetSanadAsync(system, token, tranName);
										if (sanadBusiness != null)
										{
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(FactorType, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2181);
										}
									}
									if (ret.HasError)
									{
										result = ret;
									}
									else
									{
										bool flag = filter.IsDeleteReception && ReceptionGuid.HasValue;
										if (flag)
										{
											flag = await ReceptionSumAsync(system, token, tranName) > 0m;
										}
										if (flag)
										{
											ReceptionBusiness obj = await GetReceptionAsync(system, tranName, fromDatabase: true, token);
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await obj.RemoveAsync(system, awaitOnSemaphore: true, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2188);
										}
										flag = filter.IsDeletePardakht && PardakhtGuid.HasValue;
										if (flag)
										{
											flag = await PardakhtSumAsync(system, token, tranName) > 0m;
										}
										if (flag)
										{
											PardakhtBusiness obj2 = await GetPardakhtAsync(system, token, tranName);
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await obj2.RemoveAsync(system, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2193);
										}
										if (filter.IsDeleteAghsat && (AghsatGuid ?? Guid.Empty) != Guid.Empty)
										{
											AghsatM_GroupBusiness aghsatM_GroupBusiness = await AghsatM_GroupBusiness.GetAsync(ContextGuid, AghsatGuid.Value, token, tranName);
											if (aghsatM_GroupBusiness != null)
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await aghsatM_GroupBusiness.RemoveAsync(system, sendToServer: true, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2199);
											}
										}
										if (filter.IsDeleteTolid && Tolid != null)
										{
											Tolid.CheckMovazene = false;
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await Tolid.RemoveAsync(system, 10, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2204);
										}
										if ((CashBackCouponGuid ?? Guid.Empty) != Guid.Empty)
										{
											CouponDiscountBusiness couponDiscountBusiness = await CouponDiscountBusiness.GetAsync(ContextGuid, CashBackCouponGuid.Value, token, tranName);
											if (couponDiscountBusiness != null && couponDiscountBusiness.Status)
											{
												couponDiscountBusiness.Status = false;
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await couponDiscountBusiness.SaveAsync(EnSystem.None, null, sendToServer: true, Priority.Low, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2212);
											}
										}
										if (ret.HasError)
										{
											result = ret;
										}
										else
										{
											if (!GetIsTemp() && KardexAffective)
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: true, this, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2219);
											}
											if (!ret.HasError && autoCommit && system != EnSystem.Transfer)
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2222), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2222);
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, FactorType, GetDate(), 1, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2223);
											}
											if (!ret.HasError)
											{
												if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
												{
													returnedSaveFuncInfo = ret;
													returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, FactorType, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2228);
												}
												goto end_IL_02f7;
											}
											result = ret;
										}
									}
								}
							}
						}
						goto end_IL_01fe;
						end_IL_02f7:;
					}
					goto end_IL_01e8;
					end_IL_01fe:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2232);
					ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2233);
					goto end_IL_01e8;
				}
				return result;
				end_IL_01e8:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2239), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2239);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2240), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2240);
				}
				if (old != null && ChangeStatus != EntityChangeSt.New && !ret.HasError)
				{
					ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.ClearCachedItemAsync(Guid, token, "");
					CashiersLogBusiness.Logger(ContextGuid, system, sendToServer: true, old.GetLogType(), ENAction.Delete, old);
				}
				if (!ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
				{
					Task.Run(() => SendEntitiesToServerAsync(token));
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetPrintCountAsync(DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			int line = 0;
			try
			{
				_ = 1;
				try
				{
					if (autoCommit)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2265);
						transactionHasBeenCreated = true;
						line = 1;
						ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2268);
						line = 2;
					}
					line = 3;
					ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.SetPrintCountAsync(this, dateM, token, tranName);
					line = 4;
					ret.AddReturnedValue(returnedSaveFuncInfo2, ReturnedState.None, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2275);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2279);
					ret.AddReturnedValue(ex, "", "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2280);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo3 = ret;
					returnedSaveFuncInfo3.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2285), ReturnedState.None, "SetPrintCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2285);
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> UpdateCustomerNameAsync(string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2298), ReturnedState.None, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2298);
					}
					transactionHasBeenCreated = true;
					ret.AddReturnedValue(ret, ReturnedState.None, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2300);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.UpdateCustomerNameAsync(this, name, token, tranName), ReturnedState.None, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2301);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2305);
					ret.AddReturnedValue(ex, "", "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2306);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2311), ReturnedState.None, "UpdateCustomerNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2311);
				}
			}
			return ret;
		}
	}

	public OrderBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
			DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
			_discount = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
			_keraye = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2326);
		}
	}

	public static async Task<OrderBusiness> GetWithLogg(string contextGuid, EnSystem system, Guid guid, Document factorType, bool fromdatabase, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderBusiness order = await GetAsync(contextGuid, guid, factorType, fromDatabase: true, token, tranName);
		EnSystem system2 = system;
		EnLog logType = order.GetLogType();
		string translate = "نمایش فاکتور فروش**شماره".GetTranslate();
		string text = $" ({await order.GetId2Async(system, tranName, token)}) **+";
		string text2 = $"({order.TotalSum}";
		string text3 = $"({order.CountOfProduct()})**";
		string text4 = await order.GetFullNameAsync(system, tranName, token);
		CashiersLogBusiness.Logger(contextGuid, system2, sendToServer: true, logType, ENAction.Report, null, null, translate + text + "مبلغ" + text2 + ") **تعداد اقلام" + text3 + "به " + text4);
		return order;
	}

	public static async Task<OrderBusiness> GetAsync(string contextGuid, Guid guid, Document factorType, bool fromDatabase = false, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _paralelOrderSend2ServerSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			ClsCache cache = ClsCache.GetCache(contextGuid);
			line = 1;
			if (cache == null)
			{
				throw new NullReferenceException("cache");
			}
			line = 2;
			if (cache.Source == ENSource.Accounting && !fromDatabase)
			{
				throw new Exception("برای سورس حسابداری فاکتور رو از دیتابیس بگیر!".GetTranslate());
			}
			line = 3;
			OrderBusiness orderBusiness = ((!fromDatabase) ? (await cache.CacheRepository.IOrder.GetAsync(guid, factorType, token, tranName)) : (await cache.CacheRepository.IOrder.GetFromDbAsync(guid, factorType, token, tranName)));
			if (orderBusiness == null)
			{
				return null;
			}
			if (factorType == Document.AmaniVorod && orderBusiness.FactorType != Document.AmaniVorod)
			{
				orderBusiness.FactorType = factorType;
			}
			if (factorType == Document.AmaniKhoroj && orderBusiness.FactorType != Document.AmaniKhoroj)
			{
				orderBusiness.FactorType = factorType;
			}
			return orderBusiness;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, $"#Line_{line}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2367);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_paralelOrderSend2ServerSemaphore.Release();
			}
		}
	}

	public static Task<OrderReportBusiness> GetReportAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetReportAsync(guid, token, tranName);
	}

	public static Task<OrderReportBusiness> GetReportById2Async(string contextGuid, int id2, Document type, bool status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetReportById2Async(id2, type, status, token, tranName);
	}

	public static Task<OrderBusiness> GetAsyncByReceptionGuid(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAsyncByReceptionGuidAsync(guid, token, tranName);
	}

	public static Task<OrderBusiness> GetAsyncByPardakhtGuid(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAsyncByPardakhtGuidAsync(guid, token, tranName);
	}

	public async Task<OrderDetailBusiness> GetAsync(EnSystem system, Guid prdGuid, bool isLoadOriginalPrice = true, decimal unitPrice = 0m, decimal? quantity = null, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 6;
			try
			{
				if (awaitOnSemaphore)
				{
					await _modifiedDetListSemaphore.WaitAsync();
				}
				using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					OrderDetailBusiness ret = null;
					if ((await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Factors.Res_Tajmi || ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
					{
						ret = _listOrderDetails?.FirstOrDefault((OrderDetailBusiness p) => p != null && p.ProductGuid == prdGuid && (unitPrice == 0m || p.Price == unitPrice));
					}
					if (ret == null)
					{
						ret = await AddDetAsync(system, isGood: true, isLoadOriginalPrice, awaitOnSemaphore: false, prdGuid, token);
						if (quantity.HasValue)
						{
							await ret.SetQuantityAsync(system, quantity.Value, token);
						}
						if (unitPrice > 0m)
						{
							await ret.SetPriceAsync(system, unitPrice, token);
						}
					}
					else
					{
						if (ret.Price != unitPrice && unitPrice > 0m)
						{
							await ret.SetPriceAsync(system, unitPrice, token);
						}
						if (quantity.HasValue)
						{
							await ret.SetQuantityAsync(system, ret.Quantity + quantity.GetValueOrDefault(), token);
						}
					}
					return ret;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2413);
				throw;
			}
			finally
			{
				if (awaitOnSemaphore)
				{
					_modifiedDetListSemaphore?.Release();
				}
			}
		}
	}

	public static async Task<IEnumerable<OrderReportBusiness>> GetAllAsync(string contextGuid, EnSystem system, OrderFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (system == EnSystem.UserInterface)
		{
			CashiersLogBusiness.Logger(contextGuid, system, sendToServer: true, GetLogType(filter.FactorType), ENAction.Report, null, null, "گزارش فاکتور ".GetTranslate() + " " + (filter.FactorType.GetDisplay() ?? "").GetTranslate());
			EnSystem system2 = system;
			return await ApplyAccessAsync(contextGuid, system2, await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllAsync(filter, token, tranName));
		}
		return await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllAsync(filter, token, tranName);
	}

	public static async Task<OrderCountFilter> GetCountOrderAsync(string contextGuid, OrderFilter filter, EnSystem system = EnSystem.UserInterface, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<OrderReportBusiness> obj = await GetAllAsync(contextGuid, system, filter, token, tranName);
			int valueOrDefault = (obj?.Where((OrderReportBusiness q) => q.DateSabt <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59) && q.DateSabt >= DateTime.Today)?.Count()).GetValueOrDefault();
			int valueOrDefault2 = (obj?.Where((OrderReportBusiness q) => q.DateSabt >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0) && q.DateSabt <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))?.Count()).GetValueOrDefault();
			int valueOrDefault3 = (obj?.Where((OrderReportBusiness q) => q.DateSabt >= PacketParser.Calendar.OneWeekAgo() && q.DateSabt <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59))?.Count()).GetValueOrDefault();
			int num = obj?.Count() ?? 0;
			return new OrderCountFilter
			{
				DayCountOrder = valueOrDefault,
				MonthCount = valueOrDefault2,
				ListCount = num,
				WeekCount = valueOrDefault3
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCountOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2449);
			throw;
		}
	}

	public static Task<int> NextId2Async(string contextGuid, Document factorType, bool? status, short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.NextId2Async(factorType, status, statusId, token, tranName);
	}

	public Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return NextId2Async(ContextGuid, FactorType, Status, statusId, token, tranName);
	}

	public static Task<Guid> GetById2Async(bool? status, CancellationToken token, string tranName, string contextGuid, int id2, Document factorType)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetById2Async(status, token, tranName, id2, factorType);
	}

	public async Task<List<AndroidPrintStyle>> GetPrintStylesAsync()
	{
		using (new JaegerMonitor("GetPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				List<AndroidPrintStyle> list = (await UserInformationBusiness.GetAsync(ContextGuid))?.GetAndroidPrintStyles(GetPrintType());
				if (list == null)
				{
					return list;
				}
				if (GetIsTemp())
				{
					list = list?.Where((AndroidPrintStyle w) => w.PrintFormat == PrintFormat.Html).ToList();
				}
				foreach (AndroidPrintStyle item in list)
				{
					item.Html.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"")
						.Replace("&apos;", "'")
						.Replace("&amp;", "&");
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2472);
				throw;
			}
		}
	}

	public async Task<List<AndroidPrintStyle>> GetFilledPrintStylesAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetFilledPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 4;
			try
			{
				int widthPx = ((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token))?.Settings?.Print?.PrintWidth).GetValueOrDefault(550);
				if (widthPx == 0)
				{
					widthPx = 550;
				}
				List<AndroidPrintStyle> styles = await GetPrintStylesAsync();
				ReceptionBusiness reception = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
				foreach (AndroidPrintStyle item in styles)
				{
					if (item.PrintFormat == PrintFormat.Html)
					{
						await FillPrintStyleImageAsync(item, reception?.Sum ?? 0m, widthPx, token, tranName, system);
					}
					else
					{
						await FillPrintStyleTextAsync(ContextGuid, item, reception?.Sum ?? 0m, token, tranName);
					}
				}
				return styles;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFilledPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2496);
				throw;
			}
		}
	}

	private PrintType GetPrintType()
	{
		return FactorType switch
		{
			Document.OrderSell => PrintType.Sell, 
			Document.OrderBackBuy => PrintType.BackBuy, 
			Document.OrderBackSell => PrintType.BackSell, 
			Document.OrderBuy => PrintType.Buy, 
			Document.PishFactor => PrintType.PishFactor, 
			_ => PrintType.none, 
		};
	}

	private async Task FillPrintStyleImageAsync(AndroidPrintStyle style, decimal receptionSum, int widthPx, CancellationToken token, string tranName, EnSystem system)
	{
		if (style == null)
		{
			throw new ArgumentNullException();
		}
		if (string.IsNullOrEmpty(style.Html))
		{
			return;
		}
		using (new JaegerMonitor("FillPrintStyleImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			int line = 0;
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				UserInformationBusiness userInfo = await UserInformationBusiness.GetAsync(ContextGuid);
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				CusCompactBusiness cus = await GetCusInfoAsync(EnSystem.UserInterface, token, tranName);
				CusCompactBusiness visitror = await GetVisitorInfoAsync(EnSystem.UserInterface, token, tranName);
				ReceptionBusiness reception = await GetReceptionAsync(EnSystem.UserInterface, tranName, fromDatabase: false, token);
				decimal beforeAccount = await GetBeforeAccountAsync(EnSystem.UserInterface, token, tranName);
				decimal afterAccount = await GetAfterAccountAsync(EnSystem.UserInterface, token, tranName);
				CompanySettings dep = setting.CompanyInfo;
				line = 5;
				string text = ListKala.Sum((OrderDetailBusiness s) => s.Quantity).NpToString("N0", 0);
				decimal? num = ListKala.Sum((OrderDetailBusiness s) => s?.V2Qt);
				style.Html = ParameterReplacement.SetParameter_StringImage("WidthPx", widthPx + "px", style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CompanyInfo", dep.CompanyInfo, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Description", Description, style.Html);
				line = 10;
				decimal? num2 = num;
				style.Html = ParameterReplacement.SetParameter_StringImage("CollectItems", ((num2.GetValueOrDefault() > default(decimal)) & num2.HasValue) ? (text + "<br/>" + ToStr(num)) : text, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("RowCount", ListKala.Count.ToString("N0"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("HeadFont", cashier.Settings.Print.HeadFont ?? (40 + "px"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("BodyFont", cashier.Settings.Print.BodyFont ?? (20 + "px"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("FactorType", FactorType.GetDisplay(), style.Html);
				line = 15;
				AndroidPrintStyle androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_StringImage("NO", (await GetId2Async(EnSystem.UserInterface, tranName, token)).ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Date", GetDate().MiladiToCalture(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Time", GetDate().Hour + ":" + GetDate().Minute, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CodeCus", cus.Code.ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CusName", cus.FullName, style.Html);
				if (visitror != null)
				{
					List<TafsilNumberBusiness> list = await visitror.TafsilNumbers(token);
					style.Html = ParameterReplacement.SetParameter_StringImage("VisitorName", visitror.FullName, style.Html);
					if (list != null && list.Count > 0)
					{
						style.Html = ParameterReplacement.SetParameter_StringImage("VisitorMobile", list.First().CalcedNumber.ToString(), style.Html);
					}
					if (!string.IsNullOrEmpty(visitror.Description))
					{
						style.Html = ParameterReplacement.SetParameter_StringImage("VisitorDesc", visitror.Description, style.Html);
					}
				}
				line = 20;
				style.Html = ParameterReplacement.SetParameter_StringImage("TelWithTitle", cus.PrintTelWithLabel, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("AddWithTitle", cus.PrintAddWithLabel, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Add1", cus.Address1, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Add2", cus.Address2, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepComTitle", dep.CompanyTitle, style.Html);
				line = 25;
				style.Html = ParameterReplacement.SetParameter_StringImage("DepComEmail", dep.CompanyEmail, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepComZip", dep.CompanyZip, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepFax", dep.Fax, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepAddWithTitle", userInfo.PrintDepAddWithTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepAdd", dep.Address, style.Html);
				line = 30;
				style.Html = ParameterReplacement.SetParameter_StringImage("DepTel", dep.Telephone, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepTelWithTitle", userInfo.PrintDepTelWithTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepWebsite", dep.WebSite, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepMob", dep.Mobile, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepWebAbout", dep.Web_Pages_About, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepWebContact", dep.Web_Pages_ContactUS, style.Html);
				line = 35;
				style.Html = ParameterReplacement.SetParameter_DecimalImage("AfterAccount", afterAccount, style.Html, addPrintModifier: true);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("BeforeAccount", beforeAccount, style.Html, addPrintModifier: true);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("CurrentAccount", cus.Account, style.Html, addPrintModifier: true);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Keraye", Keraye, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("DiscountSum", DiscountRows, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("SumKhadamat", 0m, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("SumKala", SumKala, style.Html);
				line = 40;
				style.Html = ParameterReplacement.SetParameter_DecimalImage("TotalSum", Sum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Discount", Discount, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Factor_Rem", Sum - receptionSum, style.Html, addPrintModifier: true);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Reception_Sum", receptionSum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Sum_Naghd", reception?.SumNaghd ?? 0m, style.Html);
				line = 45;
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Sum_Check", reception?.SumCheck ?? 0m, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Sum_Havale", reception?.SumBank ?? 0m, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("BeforAddFactorSum", beforeAccount - Sum, style.Html, addPrintModifier: true);
				line = 50;
				if ((DateTime.Now - GetDate()).TotalSeconds > 30.0)
				{
					style.Html = ParameterReplacement.SetParameter_StringImage("Display", "block", style.Html);
				}
				if (GetIsTemp() && TotalSum > 0m)
				{
					style.Html = ParameterReplacement.SetParameter_StringImage("DisplayImage", "inline", style.Html);
				}
				line = 55;
				androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_StringImage("FactorFooterLabel", await GetFactorFooterLabelAsync(token, tranName), style.Html);
				androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_StringImage("FactorFooterValue", await GetFactorFooterValueAsync(tranName, token), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("FooterString", setting.printSettings.GetFooterString(style.PrintType), style.Html);
				androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_StringImage("RecPaySummary", await ReceptionSummaryAsync(token, tranName, system) + await PaymentSummaryAsync(system, token, tranName), style.Html);
				if (TotalDiscount <= 0m)
				{
					style.Html = ParameterReplacement.SetParameter_StringImage("HasDiscount", "display:none;", style.Html);
				}
				line = 60;
				await SetParameters_AghlamImageAsync(style);
				style.Html = ParameterReplacement.SetParameter_StringImage("HideDiscountColumn", (ListKala.Count((OrderDetailBusiness w) => w.DiscountPercent > 0m) == 0) ? "display:none;" : "", style.Html);
				if (!ReceptionGuid.Equals(Guid.Empty))
				{
					await SetParameters_ReceptionImageAsync(style);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"GeneratePrintText/Line:{line}", "FillPrintStyleImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2621);
				throw;
			}
		}
	}

	private Task<CusCompactBusiness?> GetVisitorInfoAsync(EnSystem system, CancellationToken token, string tranName)
	{
		if (!B1TafsilGuid.HasValue)
		{
			return Task.FromResult<CusCompactBusiness>(null);
		}
		return CusCompactBusiness.GetAsync(ContextGuid, system, B1TafsilGuid.Value, null, tranName, token);
	}

	private string ToStr(decimal? sumQuantity2)
	{
		if (!sumQuantity2.HasValue)
		{
			return "";
		}
		return Math.Round(sumQuantity2.Value, 3).ToString().Replace(".", "/");
	}

	private async Task FillPrintStyleTextAsync(string contextGuid, AndroidPrintStyle style, decimal receptionSum, CancellationToken token, string tranName)
	{
		if (style == null)
		{
			throw new ArgumentNullException();
		}
		if (string.IsNullOrEmpty(style.Html))
		{
			return;
		}
		using (new JaegerMonitor("FillPrintStyleTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 9;
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
				UserInformationBusiness userInfo = await UserInformationBusiness.GetAsync(contextGuid);
				ReceptionBusiness reception = await GetReceptionAsync(EnSystem.UserInterface, tranName, fromDatabase: false, token);
				CusCompactBusiness cus = await GetCusInfoAsync(EnSystem.UserInterface, token, tranName);
				CompanySettings dep = (await LocalSettingBusiness.GetSettingAsync(contextGuid, token)).CompanyInfo;
				decimal beforeAccount = await GetBeforeAccountAsync(EnSystem.UserInterface, token, tranName);
				decimal afterAccount = await GetAfterAccountAsync(EnSystem.UserInterface, token, tranName);
				style.Html = ParameterReplacement.SetParameter_StringText("CollectItems", ListKala.Sum((OrderDetailBusiness s) => s.Quantity).NpToString("N0", 0), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("CompanyInfo", dep.CompanyInfo, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("RowCount", ListKala.Count.ToString("N0"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("NO", (await GetId2Async(EnSystem.UserInterface, tranName, token)).ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("Date", GetDate().MiladiToCalture(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("Time", GetDate().Hour + ":" + GetDate().Minute, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("CodeCus", cus.Code.ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("CusName", cus.FullName, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("TelWithTitle", cus.PrintTelWithLabel, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("AddWithTitle", cus.PrintAddWithLabel, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("Add1", cus.Address1, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("Add2", cus.Address2, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepComTitle", dep.CompanyTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepComEmail", dep.CompanyEmail, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepComZip", dep.CompanyZip, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepFax", dep.Fax, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepAdd", dep.Address, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepAddWithTitle", userInfo.PrintDepAddWithTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepTel", dep.Telephone, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepTelWithTitle", userInfo.PrintDepTelWithTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepWebsite", dep.WebSite, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepMob", dep.Mobile, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepWebAbout", dep.Web_Pages_About, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("DepWebContact", dep.Web_Pages_ContactUS, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("AfterAccount", afterAccount, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("BeforeAccount", beforeAccount, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("CurrentAccount", cus.Account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Keraye", Keraye, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("DiscountSum", DiscountRows, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("SumKhadamat", 0m, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("SumKala", SumKala, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("TotalSum", Sum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Discount", Discount, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Factor_Rem", Sum - receptionSum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Reception_Sum", receptionSum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Sum_Naghd", reception.SumNaghd, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Sum_Check", reception.SumCheck, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Sum_Havale", reception.SumBank, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("BeforAddFactorSum", beforeAccount - Sum, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("FooterString", setting.printSettings.GetFooterString(style.PrintType), style.Html);
				await SetParameters_AghlamTextAsync(style);
				if (!ReceptionGuid.Equals(Guid.Empty))
				{
					await SetParameters_ReceptionTextAsync(style, token);
				}
				style.Html = style.Html.ToLower();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "GeneratePrintText", "FillPrintStyleTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2700);
				throw;
			}
		}
	}

	private async Task SetParameters_AghlamImageAsync(AndroidPrintStyle style, CancellationToken token = default(CancellationToken), EnSystem system = EnSystem.None)
	{
		using (new JaegerMonitor("SetParameters_AghlamImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				string aghlam = "";
				ClsRegex clsRegex = new ClsRegex(style.Html, "<ROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀:=\"a-zA-Z|;,*0-9-></ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوژءآ\r\n ]*</ROW>");
				string aghlamTemplate = clsRegex.SuccessfullMatch.Replace("<ROW>", "").Replace("</ROW>", "");
				string aghlamTemplateWithRow = clsRegex.SuccessfullMatch;
				short index = 1;
				CultureInfo faCulture = CultureInfo.CreateSpecificCulture("fa-IR");
				if (ListKala != null && ListKala.Count > 0)
				{
					foreach (OrderDetailBusiness item in ListKala)
					{
						ProductCompactBusiness prd = await item.GetPrdInfoAsync(system, token);
						string temp11 = aghlamTemplate;
						temp11 = ParameterReplacement.SetParameter_StringImage("RowDesc", item.RowDescriptionForPrint, temp11);
						IReadOnlyList<PrdTechnicalBusiness> obj = await prd.GetPrdTechnicalListAsync(token);
						string parameterValue = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 1)?.Name ?? "";
						string parameterValue2 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 2)?.Name ?? "";
						string parameterValue3 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 3)?.Name ?? "";
						string parameterValue4 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 4)?.Name ?? "";
						string parameterValue5 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 5)?.Name ?? "";
						string parameterValue6 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 6)?.Name ?? "";
						string parameterValue7 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 7)?.Name ?? "";
						string parameterValue8 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 8)?.Name ?? "";
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI1", parameterValue, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI2", parameterValue2, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI3", parameterValue3, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI4", parameterValue4, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI5", parameterValue5, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI6", parameterValue6, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage("RowTechI7", parameterValue7, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage("RowTechI8", parameterValue8, temp11), parameterName: "Radif", parameterValue: index.ToString()), parameterName: "PRDCode", parameterValue: prd.IdProduct), parameterName: "PRDName", parameterValue: prd.Name);
						if (!string.IsNullOrEmpty(item.DiscountPercentString))
						{
							temp11 = ParameterReplacement.SetParameter_StringImage("RowDiscountPercent", item.DiscountPercentString + "%<br/>" + item.DiscountString, temp11);
						}
						temp11 = ParameterReplacement.SetParameter_StringImage("QT", item.QuantityString, temp11);
						temp11 = ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage(data: (item.VGuid.Equals(Guid.Empty) || !item.V2Qt.HasValue) ? ParameterReplacement.SetParameter_StringImage("QT2", string.Empty, temp11) : ParameterReplacement.SetParameter_StringImage("QT2", item.V2QtString + item.Unit2, temp11), parameterName: "QtForDisplay", parameterValue: item.QuantityForPrint), parameterName: "QtWithVahed", parameterValue: item.QtWithVahedPrint()), parameterName: "UnitPrice", parameterValue: item.Price), parameterName: "RowDiscount", parameterValue: item.Discount), parameterName: "RowSum", parameterValue: item.RowSum), parameterName: "VahedName", parameterValue: item.ProductVahed), parameterName: "VahedName2", parameterValue: item.Unit2), parameterName: "V2QT", parameterValue: prd.V2Qt.ToString("N0", faCulture));
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + temp11) : temp11);
						index = checked((short)(index + 1));
					}
				}
				if (!string.IsNullOrEmpty(aghlamTemplateWithRow))
				{
					style.Html = style.Html.Replace(aghlamTemplateWithRow, aghlam);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "SetParameters_AghlamImage", "SetParameters_AghlamImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2775);
				throw;
			}
		}
	}

	private async Task SetParameters_AghlamTextAsync(AndroidPrintStyle style, CancellationToken token = default(CancellationToken), EnSystem system = EnSystem.None)
	{
		using (new JaegerMonitor("SetParameters_AghlamTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				string aghlam = "";
				ClsRegex clsRegex = new ClsRegex(style.Html, "<ROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀:=\"a-zA-Z|,*0-9-></ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوژءآ\r\n ]*?</ROW>");
				clsRegex.SuccessfullMatch.Replace("<ROW>", "").Replace("</ROW>", "");
				_ = clsRegex.SuccessfullMatch;
				short index = 1;
				CultureInfo faCulture = CultureInfo.CreateSpecificCulture("fa-IR");
				if (ListKala != null && ListKala.Count > 0)
				{
					foreach (OrderDetailBusiness item in ListKala)
					{
						ProductCompactBusiness prd = await item.GetPrdInfoAsync(system, token);
						string temp11 = style.Html;
						if (index == ListKala.Count)
						{
							temp11 = temp11.Replace("┼", "┴").Replace("├", "└").Replace("┤", "┘");
						}
						temp11 = ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText("RowDesc", item.RowDescriptionForPrint, temp11), parameterName: "CollectItems", parameterValue: ListKala.Sum((OrderDetailBusiness s) => s.Quantity).NpToString("N0", 0));
						IReadOnlyList<PrdTechnicalBusiness> obj = await prd.GetPrdTechnicalListAsync(token);
						string parameterValue = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 1)?.Name ?? "";
						string parameterValue2 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 2)?.Name ?? "";
						string parameterValue3 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 3)?.Name ?? "";
						string parameterValue4 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 4)?.Name ?? "";
						string parameterValue5 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 5)?.Name ?? "";
						string parameterValue6 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 6)?.Name ?? "";
						string parameterValue7 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 7)?.Name ?? "";
						string parameterValue8 = obj?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 8)?.Name ?? "";
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI1", parameterValue, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI2", parameterValue2, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI3", parameterValue3, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI4", parameterValue4, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI5", parameterValue5, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI6", parameterValue6, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText("RowTechI7", parameterValue7, temp11);
						temp11 = ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText("RowTechI8", parameterValue8, temp11), parameterName: "Radif", parameterValue: index.ToString()), parameterName: "PRDCode", parameterValue: prd.IdProduct), parameterName: "PRDName", parameterValue: prd.Name), parameterName: "QT", parameterValue: item.QuantityString);
						temp11 = ParameterReplacement.SetParameter_DecimalText(data: ParameterReplacement.SetParameter_DecimalText(data: ParameterReplacement.SetParameter_StringText(data: (item.VGuid.Equals(Guid.Empty) || !item.V2Qt.HasValue) ? ParameterReplacement.SetParameter_StringText("QT2", item.Quantity.ToString("N0", faCulture), temp11) : ParameterReplacement.SetParameter_StringText("QT2", item.V2Qt.Value.ToString("N0", faCulture), temp11), parameterName: "QtForDisplay", parameterValue: item.QuantityForPrint), parameterName: "UnitPrice", parameterValue: item.Price), parameterName: "RowDiscount", parameterValue: item.Discount);
						temp11 = ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_StringText(data: ParameterReplacement.SetParameter_DecimalText(data: ParameterReplacement.SetParameter_DecimalText("RowDiscountPercent", 0m, temp11), parameterName: "RowSum", parameterValue: item.RowSum), parameterName: "VahedName", parameterValue: item.ProductVahed), parameterName: "VahedName2", parameterValue: item.Unit2), parameterName: "V2QT", parameterValue: prd.V2Qt.ToString("N0", faCulture));
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + temp11) : temp11);
						index = checked((short)(index + 1));
					}
				}
				if (!string.IsNullOrEmpty(aghlam))
				{
					style.Html = aghlam;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "SetParameters_AghlamText", "SetParameters_AghlamTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2859);
				throw;
			}
		}
	}

	private async Task SetParameters_ReceptionImageAsync(AndroidPrintStyle style, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SetParameters_ReceptionImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				string aghlam = "";
				ClsRegex clsRegex = new ClsRegex(style.Html, "<RECEPTIONROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀:=\";a-zA-Z|,*0-9-></ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوژءآ\r\n ]*</RECEPTIONROW>");
				string aghlamTemplate = clsRegex.SuccessfullMatch.Replace("<RECEPTIONROW>", "").Replace("</RECEPTIONROW>", "");
				string aghlamTemplateWithRow = clsRegex.SuccessfullMatch;
				ReceptionBusiness receptionBusiness = null;
				if (ReceptionGuid.HasValue)
				{
					receptionBusiness = await ReceptionBusiness.GetAsync(ContextGuid, ReceptionGuid.Value, EnSystem.UserInterface, this, fromDatabase: false, token);
				}
				CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fa-IR");
				if (receptionBusiness != null && receptionBusiness.AllItems(ContextGuid).Count > 0)
				{
					foreach (ReceptionCheckBusiness item in receptionBusiness.ListCheck)
					{
						string data = ParameterReplacement.SetParameter_StringImage("Description", item.Description, aghlamTemplate);
						data = ParameterReplacement.SetParameter_StringImage("Price", item.Price.ToString("N0", CultureInfo.CreateSpecificCulture("fa-IR").NumberFormat), data);
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data) : data);
					}
					foreach (ReceptionHavaleBusiness item2 in receptionBusiness.ListBank)
					{
						string data2 = ParameterReplacement.SetParameter_StringImage("Description", item2.Description, aghlamTemplate);
						data2 = ParameterReplacement.SetParameter_StringImage("Price", item2.Price.ToString("N0", cultureInfo.NumberFormat), data2);
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data2) : data2);
					}
					foreach (ReceptionNaghdBusiness item3 in receptionBusiness.ListNaghd)
					{
						string data3 = ParameterReplacement.SetParameter_StringImage("Description", item3.Description, aghlamTemplate);
						data3 = ParameterReplacement.SetParameter_StringImage("Price", item3.Price.ToString("N0", cultureInfo.NumberFormat), data3);
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data3) : data3);
					}
				}
				if (!string.IsNullOrEmpty(aghlamTemplateWithRow))
				{
					style.Html = style.Html.Replace(aghlamTemplateWithRow, aghlam);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "SetParameters_Aghlam/data:", "SetParameters_ReceptionImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2914);
				throw;
			}
		}
	}

	private async Task SetParameters_ReceptionTextAsync(AndroidPrintStyle style, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SetParameters_ReceptionTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				string aghlam = "";
				ClsRegex clsRegex = new ClsRegex(style.Html, "<RECEPTIONROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀:=\"a-zA-Z|,*0-9-></ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوژءآ\r\n ]*</RECEPTIONROW>");
				string aghlamTemplate = clsRegex.SuccessfullMatch.Replace("<RECEPTIONROW>", "").Replace("</RECEPTIONROW>", "");
				string aghlamTemplateWithRow = clsRegex.SuccessfullMatch;
				ReceptionBusiness receptionBusiness = await ReceptionBusiness.GetAsync(ContextGuid, ReceptionGuid.Value, EnSystem.UserInterface, this, fromDatabase: false, token);
				CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fa-IR");
				if (receptionBusiness != null && receptionBusiness.AllItems(ContextGuid).Count > 0)
				{
					foreach (ReceptionCheckBusiness item in receptionBusiness.ListCheck)
					{
						string data = ParameterReplacement.SetParameter_StringText("Description", item.Description, aghlamTemplate);
						data = ParameterReplacement.SetParameter_StringText("Price", item.Price.ToString("N0", cultureInfo.NumberFormat), data);
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data) : data);
					}
					foreach (ReceptionHavaleBusiness item2 in receptionBusiness.ListBank)
					{
						string data2 = ParameterReplacement.SetParameter_StringText("Description", item2.Description, aghlamTemplate);
						data2 = ParameterReplacement.SetParameter_StringText("Price", item2.Price.ToString("N0", cultureInfo.NumberFormat), data2);
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data2) : data2);
					}
					foreach (ReceptionNaghdBusiness item3 in receptionBusiness.ListNaghd)
					{
						string data3 = ParameterReplacement.SetParameter_StringText("Description", item3.Description, aghlamTemplate);
						data3 = ParameterReplacement.SetParameter_StringText("Price", item3.Price.ToString("N0", cultureInfo.NumberFormat), data3);
						aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data3) : data3);
					}
				}
				if (!string.IsNullOrEmpty(aghlamTemplateWithRow))
				{
					style.Html = style.Html.Replace(aghlamTemplateWithRow, aghlam);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "SetParameters_Aghlam/data:", "SetParameters_ReceptionTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 2965);
				throw;
			}
		}
	}

	public static Task<short> GetNextDailyFactorNoAsync(string contextGuid, DateTime dateM, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetNextDailyFactorNoAsync(dateM, token, tranName);
	}

	private async Task SetDailyFactorNoAsync(CancellationToken token, string tranName)
	{
		checked
		{
			using (new JaegerMonitor("SetDailyFactorNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
			{
				_ = 5;
				try
				{
					DateTime date = GetDate();
					AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token, tranName);
					if (!(setting?.Factors?.FishNoResetTimeList?.Any()).GetValueOrDefault())
					{
						DailyFactorNo = await GetNextDailyFactorNoAsync(ContextGuid, date, token, tranName);
						return;
					}
					List<DateTime> list2 = new List<DateTime>();
					foreach (TimeSpan fishNoResetTime in setting.Factors.FishNoResetTimeList)
					{
						DateTime dateTime = date.AddDays(-1.0);
						DateTime dateTime2 = date.AddDays(1.0);
						list2.Add(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, fishNoResetTime.Hours, fishNoResetTime.Minutes, fishNoResetTime.Seconds));
						list2.Add(new DateTime(date.Year, date.Month, date.Day, fishNoResetTime.Hours, fishNoResetTime.Minutes, fishNoResetTime.Seconds));
						list2.Add(new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, fishNoResetTime.Hours, fishNoResetTime.Minutes, fishNoResetTime.Seconds));
					}
					string value = await LocalSettingBusiness.GetAsync(ContextGuid, FactorSettings.FishNumberLastResetTimeKey, awaitOnSemaphore: false, token);
					if (string.IsNullOrEmpty(value))
					{
						value = date.ToString();
						await LocalSettingBusiness.SaveAsync(ContextGuid, FactorSettings.FishNumberLastResetTimeKey, value.ToString(), awaitOnSemaphore: true, 10, token);
						DailyFactorNo = (short)setting.Factors.FishNoStartIndex;
						return;
					}
					DateTime dateTime3 = value.ParseToDate();
					list2 = list2.OrderBy((DateTime q) => q).ToList();
					for (int i = 0; i < list2.Count - 1; i++)
					{
						DateTime dateTime4 = list2[i];
						DateTime dateTime5 = list2[i + 1];
						if (date < dateTime5 && date >= dateTime4)
						{
							if (dateTime3 < dateTime4 || dateTime3 > dateTime5)
							{
								dateTime3 = date;
								await LocalSettingBusiness.SaveAsync(ContextGuid, FactorSettings.FishNumberLastResetTimeKey, dateTime3.ToString(), awaitOnSemaphore: true, 10, token);
								DailyFactorNo = (short)setting.Factors.FishNoStartIndex;
							}
							else
							{
								DailyFactorNo = await GetNextDailyFactorNoAsync(ContextGuid, dateTime3, token, tranName);
							}
							return;
						}
					}
					throw new Exception();
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetDailyFactorNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3021);
					throw;
				}
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, int oldSanadNumber, CancellationToken token = default(CancellationToken))
	{
		_oldOrder = new OrderBusiness(ContextGuid)
		{
			SanadNo = oldSanadNumber
		};
		return SaveAsync(system, awaitOnSemaphore: true, sendToServer: true, isTemp: false, 10, "", "", "", 60000, await4sent: false, token);
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 2;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebOrder webEntity = await ToWebItemAsync();
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid)
				{
					Action = ENAction.Error,
					ApplicationVersion = (ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? ""),
					CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid),
					CashierName = cashierBusiness?.Name,
					ContextGuid = ContextGuid,
					DateM = DateTime.Now,
					DateUtc = DateTime.Now.ToUniversalTime(),
					Guid = Guid.NewGuid(),
					System = system,
					Source = ClsCache.GetCache(ContextGuid).Source,
					SanadType = GetLogType(),
					SanadGuid = Guid
				};
				CashiersLogBusiness cashiersLogBusiness2 = cashiersLogBusiness;
				string translate = "خطا در ذخیره فاکتور".GetTranslate();
				string translate2 = FactorType.GetDisplay().GetTranslate();
				object arg = webEntity.Id2;
				string arg2 = await GetFullNameAsync(system);
				cashiersLogBusiness2.Title = translate + "  " + translate2 + " " + $"({arg}){arg2}-{Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3057);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool awaitOnSemaphore = true, bool sendToServer = true, bool isTemp = false, short tryCount = 10, string tranName = "", string CustomerUserName = "", string CustomerPassword = "", int timeout = 60000, bool await4sent = false, CancellationToken token = default(CancellationToken), IMessageSender messageSender = null)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			int line = 0;
			ENAction action = ENAction.Save;
			bool fromDatabase = false;
			EntityChangeSt _originalChangeSt = ChangeStatus;
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			Stopwatch sw = Stopwatch.StartNew();
			StringBuilder sb = new StringBuilder(100000);
			Stopwatch tempSw = Stopwatch.StartNew();
			List<ProductsChanges> changeListForSms = null;
			_sendEntitiesToServerToken?.Cancel();
			_sendEntitiesToServerToken = new CancellationTokenSource();
			CancellationToken sendEntitiesToServerToken = CancellationTokenSource.CreateLinkedTokenSource(token, _sendEntitiesToServerToken.Token).Token;
			try
			{
				_ = 77;
				ReturnedSaveFuncInfo result;
				try
				{
					if (source == ENSource.Android_HesabYar && _listOrderDetails != null)
					{
						foreach (OrderDetailBusiness item in _listOrderDetails.ToList())
						{
							if (item.Quantity <= 0m)
							{
								item.Dispose();
							}
						}
					}
					if (autoCommit)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3090), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3090);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3091), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3091);
					}
					if ((FactorType == Document.PishFactor || FactorType == Document.OrderPreBuy) && KardexAffective)
					{
						KardexAffective = false;
					}
					if (!Status)
					{
						OrderBusiness orderBusiness = this;
						bool kardexAffective = (SanadAffective = false);
						orderBusiness.KardexAffective = kardexAffective;
					}
					decimal receptionSum = default(decimal);
					decimal pardakhtSum = default(decimal);
					ReceptionBusiness reception = null;
					PardakhtBusiness pardakht = null;
					AghsatM_GroupBusiness installment = null;
					if (MetaData == null)
					{
						MetaData = new OrderMetaData();
					}
					if (system != EnSystem.Transfer)
					{
						if ((ReceptionGuid ?? Guid.Empty) != Guid.Empty)
						{
							reception = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
							receptionSum = await ReceptionSumAsync(system, token, tranName);
						}
						else if ((PardakhtGuid ?? Guid.Empty) != Guid.Empty)
						{
							pardakht = await GetPardakhtAsync(system, token, tranName);
							pardakhtSum = await PardakhtSumAsync(system, token, tranName);
						}
						if ((AghsatGuid ?? Guid.Empty) != Guid.Empty)
						{
							installment = await GetInstallmentAsync(token, tranName);
						}
					}
					if (ListKala?.Any() ?? false)
					{
						foreach (OrderDetailBusiness item2 in ListKala)
						{
							if ((item2?.ProductTafsilGuid ?? Guid.Empty) == Guid.Empty)
							{
								await item2.GetProductTafsilGuidAsync(token);
							}
						}
					}
					AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
					if (ClsCache.GetCache(ContextGuid).OrderSanadGenerator == null)
					{
						if (setting != null)
						{
							switch ((MaliYearSanadType)setting.CompanyInfo.SanadType)
							{
							case MaliYearSanadType.Periodic:
								ClsCache.GetCache(ContextGuid).OrderSanadGenerator = new OrderSanadGeneratorPeriodic();
								break;
							case MaliYearSanadType.Permanent:
								ClsCache.GetCache(ContextGuid).OrderSanadGenerator = new OrderSanadGeneratorPermanent();
								break;
							default:
								throw new NotImplementedException();
							}
						}
						else
						{
							ClsCache.GetCache(ContextGuid).OrderSanadGenerator = new OrderSanadGeneratorPeriodic();
						}
					}
					sb.AppendLine($"GetReceptionAsync Finished {tempSw.ElapsedMilliseconds} ms");
					tempSw.Restart();
					sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
					line = 1;
					using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "OrderBusiness/SaveAsync", 60000, token);
					line = 2;
					sb.AppendLine($"Semaphore Has Been Taken {tempSw.ElapsedMilliseconds} ms");
					tempSw.Restart();
					if (system == EnSystem.SignalR)
					{
						if (reception != null)
						{
							ReceptionBusiness receptionBusiness = reception;
							receptionBusiness.Description = await SetKhazanehDescriptionAsync(system, token, tranName);
						}
						if (pardakht != null)
						{
							PardakhtBusiness pardakhtBusiness = pardakht;
							pardakhtBusiness.Description = await SetKhazanehDescriptionAsync(system, token, tranName);
						}
					}
					SanadBusiness sanad;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3156);
					}
					else
					{
						line = 3;
						transactionHasBeenCreated = true;
						line = 4;
						if ((source == ENSource.Android_HesabYar || source == ENSource.Android_SefareshGir) && ServerStatus == ServerStatusForClient.None)
						{
							line = 5;
							await SetDateAsync(DateTime.Now, system, token, tranName);
						}
						line = 1001;
						await SetIsTempAsync(system, isTemp, token, tranName);
						sb.AppendLine($"SetIsTemp Finished {tempSw.ElapsedMilliseconds} ms");
						tempSw.Restart();
						if (!_isTemp)
						{
							line = 1002;
							ServerStatus = ServerStatusForClient.None;
							HostRefrenced = null;
						}
						sb.AppendLine($"GetIsTemp Finished {tempSw.ElapsedMilliseconds} ms");
						tempSw.Restart();
						line = 1003;
						if (reception != null && receptionSum > 0m)
						{
							ReceptionBusiness receptionBusiness = reception;
							receptionBusiness.SanadNo = await GetSanadNumberAsync(token, tranName);
						}
						if (pardakht != null && pardakhtSum > 0m)
						{
							PardakhtBusiness pardakhtBusiness = pardakht;
							pardakhtBusiness.SanadNo = await GetSanadNumberAsync(token, tranName);
						}
						if (installment != null)
						{
							AghsatM_GroupBusiness aghsatM_GroupBusiness = installment;
							aghsatM_GroupBusiness.SanadNo = await GetSanadNumberAsync(token, tranName);
						}
						sb.AppendLine($"GetReceptionSanadNo Finished {tempSw.ElapsedMilliseconds} ms");
						tempSw.Restart();
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3190);
						if (system != EnSystem.UserInterface)
						{
							ret.throwExceptionIfError();
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							sb.AppendLine($"CheckValidation Finished {tempSw.ElapsedMilliseconds} ms");
							tempSw.Restart();
							line = 101;
							action = ((_oldOrder == null) ? ENAction.Add : ENAction.Edit);
							line = 102;
							if (!(_oldOrder?.GetIsTemp() ?? false) && _oldOrder != null && (_oldOrder.ServerStatus == ServerStatusForClient.Delivered || _oldOrder.ServerStatus == ServerStatusForClient.DirectDelivery))
							{
								result = ret.AddInformation("فاکتور ثبت شده است".GetTranslate(), "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3201);
							}
							else
							{
								line = 15;
								if (!string.IsNullOrEmpty(CustomerUserName) && !string.IsNullOrEmpty(CustomerPassword))
								{
									await SetIsTempAsync(system, value: false, token, tranName);
								}
								line = 20;
								if (system == EnSystem.SignalR && source == ENSource.Accounting && TotalBazaryab > 0m)
								{
									Guid tafsilGuid = TafsilGuid;
									_tafsilGuid = Guid.Empty;
									await SetTafsilGuid(tafsilGuid, system, updateVisitorGuid: true, token, tranName);
								}
								if (source == ENSource.Accounting)
								{
									fromDatabase = true;
								}
								sanad = null;
								if (SanadAffective && !GetIsTemp())
								{
									sanad = await ClsCache.GetCache(ContextGuid).OrderSanadGenerator.GenerateSanadAsync(this, _oldOrder, EnSystem.None, token, tranName);
									sb.AppendLine($"GenerateSanad Finished {tempSw.ElapsedMilliseconds} ms");
									tempSw.Restart();
									goto IL_1e1e;
								}
								if (ChangeStatus != EntityChangeSt.Edited || FactorType == Document.PishFactor || FactorType == Document.OrderPreBuy || FactorType == Document.AmaniKhoroj || FactorType == Document.AmaniVorod)
								{
									goto IL_1e1e;
								}
								SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
								if (sanadBusiness != null)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(FactorType, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3226);
								}
								if ((FactorType == Document.OrderBuy || FactorType == Document.OrderBackSell) && pardakht != null && pardakhtSum != 0m)
								{
									if (await PardakhtBusiness.GetReportAsync(ContextGuid, PardakhtGuid.Value, token, tranName) != null)
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await pardakht.RemoveAsync(system, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3231);
									}
									if (!ret.HasError)
									{
										_pardakht = null;
										goto IL_1d10;
									}
									result = ret;
								}
								else
								{
									if ((FactorType != Document.OrderSell && FactorType != Document.OrderBackBuy) || reception == null || !(receptionSum != 0m))
									{
										goto IL_1d10;
									}
									if (await ReceptionBusiness.GetReportAsync(ContextGuid, ReceptionGuid.Value, token, tranName) != null)
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await reception.RemoveAsync(system, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3239);
									}
									if (!ret.HasError)
									{
										_reception = null;
										goto IL_1d10;
									}
									result = ret;
								}
							}
						}
					}
					goto end_IL_0e39;
					IL_2ccf:
					AghsatGuid = installment.Guid;
					goto IL_2ce5;
					IL_1d10:
					if (FactorType != Document.OrderSell || installment == null)
					{
						goto IL_1e00;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await installment.RemoveAsync(system, sendToServer: true, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3245);
					if (!ret.HasError)
					{
						installment = null;
						goto IL_1e00;
					}
					result = ret;
					goto end_IL_0e39;
					IL_2de3:
					line = 48;
					if (!isTemp && system != EnSystem.Transfer && FactorType != Document.PishFactor && FactorType != Document.OrderPreBuy && KardexAffective)
					{
						if (FactorType == Document.OrderSell)
						{
							changeListForSms = await GetLstProductsAnbarChangesAsync(system, token, tranName, isDeleteAction: false);
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: false, this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3361);
						sb.AppendLine($"UpdateProductAmount Finished {tempSw.ElapsedMilliseconds} ms");
						tempSw.Restart();
					}
					if (_tolid != null && system != EnSystem.Transfer)
					{
						decimal sumMavad = await _tolid.GetMavadSumAsync(system, token, tranName);
						decimal num = await _tolid.GetExportSumAsync(system, token, tranName);
						if (_tolid.HazineSum <= 0m && sumMavad <= 0m && num <= 0m)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await _tolid.RemoveAsync(system, tryCount, token, tranName);
							if (returnedSaveFuncInfo2.HasError)
							{
								ret.AddError(system, returnedSaveFuncInfo2.GetErrorMessage(), "TolidBusiness", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3374);
							}
							_tolid = null;
						}
						else
						{
							_tolid.SanadNo = SanadNo;
							_tolid.DateM = GetDate().AddSeconds(-1.0);
							ReturnedSaveFuncInfo returnedSaveFuncInfo3 = await _tolid.SaveAsync(system, token, awaitOnSemaphore: true, 10, tranName, sanad);
							if (returnedSaveFuncInfo3.HasError)
							{
								ret.AddError(system, returnedSaveFuncInfo3.GetErrorMessage(), "TolidBusiness", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3383);
							}
						}
					}
					else if (source == ENSource.Accounting && system != EnSystem.Transfer && setting.Sells.AddTolidAfterNegativeOrder)
					{
						_tolid = await TolidBusiness.GetAsync(ContextGuid, Guid, token, tranName);
						if (_tolid != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo4 = await _tolid.RemoveAsync(system, tryCount, token, tranName);
							if (returnedSaveFuncInfo4.HasError)
							{
								ret.AddError(system, returnedSaveFuncInfo4.GetErrorMessage(), "TolidBusiness", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3393);
							}
						}
					}
					if (ret.HasError)
					{
						result = ret;
					}
					else
					{
						if (system != EnSystem.Transfer && FactorType == Document.OrderSell && ChangeStatus == EntityChangeSt.New)
						{
							await SetDailyFactorNoAsync(token, tranName);
							sb.AppendLine($"SetDailyFactorNo Finished {tempSw.ElapsedMilliseconds} ms");
							tempSw.Restart();
						}
						await SanadStatusReportBusiness.ApplySanadStatusIdAsync(ContextGuid, this, system, token, tranName);
						if (system == EnSystem.UserInterface)
						{
							IsCashBackProgressed = false;
						}
						TableRelationShipBusiness tableRelationShipBusiness = await GetLinkedSanadAsync(system, token, tranName);
						if ((tableRelationShipBusiness?.ChildList?.Count()).GetValueOrDefault() == 1 && tableRelationShipBusiness.ChildList.Single().Price > TotalSum)
						{
							await AddToLinkedSanadsAsync(tableRelationShipBusiness.ChildList.Single().SanadGuid, tableRelationShipBusiness.ChildList.Single().SanadType, TotalSum);
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3417);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								if (OpenInvoice != null)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await OpenInvoice.SaveAsync(system, Guid, token, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3419);
								}
								sb.AppendLine($"OrderSave Finished {tempSw.ElapsedMilliseconds} ms");
								tempSw.Restart();
								line = 451;
								if (ret.HasError)
								{
									line = 53;
									await RemoveAsync(system, new OrderRemoveFilter
									{
										IsDeleteAghsat = true,
										IsDeletePardakht = true,
										IsDeleteReception = true
									}, tryCount, tranName, token);
									line = 54;
									result = ret;
								}
								else
								{
									line = 61;
									if (sanad == null && SanadAffective && !GetIsTemp())
									{
										WebErrorLog.Instance.StartLog("Sanad Is Null !!!!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3433);
									}
									line = 62;
									sb.AppendLine($"Before GetIsTemp {tempSw.ElapsedMilliseconds} ms");
									tempSw.Restart();
									if (GetIsTemp() || !SanadAffective)
									{
										goto IL_452c;
									}
									if (FactorType == Document.PishFactor || FactorType == Document.OrderPreBuy)
									{
										if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover == null || ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
										{
											goto IL_452c;
										}
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, FactorType, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3445);
										if (ret.HasError)
										{
											result = ret;
										}
										else
										{
											List<ViewSanadDetailCoverBusiness> lst = await ClsCache.GetCache(ContextGuid).OrderSanadGenerator.GetSanadCoverListAsync(this, system, token, tranName);
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.SaveArticlesAsync(lst, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3448);
											if (!ret.HasError)
											{
												goto IL_452c;
											}
											result = ret;
										}
									}
									else
									{
										line = 630;
										if (sanad == null)
										{
											throw new NullReferenceException("sanad is null!!");
										}
										line = 633;
										if (!ret.HasError)
										{
											if (SanadNo <= 0 && source == ENSource.Accounting)
											{
												SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.SetSanadNoAsync(Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3461);
												if (Tolid != null)
												{
													returnedSaveFuncInfo = ret;
													returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Tolid.SetSanadNoAsync(Tolid.Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3463);
												}
											}
											sanad.SanadNo = SanadNo;
											sb.AppendLine($"Get sanad Cover {tempSw.ElapsedMilliseconds} ms");
											List<ViewSanadDetailCoverBusiness> lstSanadCover = await ClsCache.GetCache(ContextGuid).OrderSanadGenerator.GetSanadCoverListAsync(this, system, token, tranName);
											sb.AppendLine($"Get sanad Cover {tempSw.ElapsedMilliseconds} ms");
											sb.AppendLine($"before check reception {tempSw.ElapsedMilliseconds} ms");
											if (reception != null)
											{
												List<ViewSanadDetailCoverBusiness> list = await reception.GetSanadCoverListAsync(system, token, tranName);
												if (list != null && list.Any())
												{
													lstSanadCover.AddRange(list);
												}
											}
											sb.AppendLine($"after check reception {tempSw.ElapsedMilliseconds} ms");
											sb.AppendLine($"before check pardakht {tempSw.ElapsedMilliseconds} ms");
											if (pardakht != null)
											{
												List<ViewSanadDetailCoverBusiness> list2 = await pardakht.GetSanadCoverListAsync(system, token, tranName);
												if (list2 != null && list2.Any())
												{
													lstSanadCover.AddRange(list2);
												}
											}
											sb.AppendLine($"after check pardakht {tempSw.ElapsedMilliseconds} ms");
											if (Tolid != null)
											{
												List<ViewSanadDetailCoverBusiness> list3 = await Tolid.GetSanadCoverListAsync(system, token, tranName);
												if (list3 != null && list3.Any())
												{
													lstSanadCover.AddRange(list3);
												}
											}
											if (installment != null)
											{
												List<ViewSanadDetailCoverBusiness> list4 = await installment.GetSanadCoverListAsync(system, new AghsatMSanadRecord
												{
													DateM = GetDate().AddSeconds(1.0)
												}, token, tranName);
												if (list4 != null && list4.Any())
												{
													lstSanadCover.AddRange(list4);
												}
											}
											sb.AppendLine($"before save sanad {tempSw.ElapsedMilliseconds} ms");
											ret.AddReturnedValue(await sanad.SaveAsync(system, token, isAutomaticSanad: true, awaitOnSemaphore: true, tranName, lstSanadCover, GetTupleParams(system)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3502);
											sb.AppendLine($"after save sanad {tempSw.ElapsedMilliseconds} ms");
											goto IL_452c;
										}
										result = ret;
									}
								}
							}
						}
					}
					goto end_IL_0e39;
					IL_1e00:
					if (!ret.HasError)
					{
						goto IL_1e1e;
					}
					result = ret;
					goto end_IL_0e39;
					IL_2ce5:
					if (system != EnSystem.Transfer && FactorType != Document.PishFactor && FactorType != Document.OrderPreBuy && !GetIsTemp())
					{
						await sanad.CheckValidationAsync(system, isAutomaticSanad: true, tranName, isDeleteAction: false, token, GetTupleParams(system));
						sb.AppendLine($"SanadCheckValidation Finished {tempSw.ElapsedMilliseconds} ms");
						tempSw.Restart();
					}
					goto IL_2de3;
					IL_1e1e:
					if (system == EnSystem.Transfer || !SanadAffective)
					{
						goto IL_2de3;
					}
					if (FactorType == Document.OrderBuy || FactorType == Document.OrderBackSell)
					{
						if (pardakhtSum <= 0m)
						{
							pardakht = await PardakhtBusiness.GetAsync(ContextGuid, PardakhtGuid ?? Guid.Empty, fromDatabase, token, tranName);
							if (pardakht != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await pardakht.RemoveAsync(system, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3260);
							}
							RemoveFromLinkedSanad(PardakhtGuid ?? Guid.Empty, Document.Pardakht, token);
							PardakhtGuid = null;
							OrderBusiness orderBusiness2 = this;
							PardakhtBusiness pardakht3;
							pardakht = (pardakht3 = null);
							orderBusiness2._pardakht = pardakht3;
							goto IL_2968;
						}
						pardakht.RemoveFromLinkedSanad(Guid, FactorType);
						pardakht.ServerStatus = ServerStatusForClient.LinkToOrder;
						if (string.IsNullOrEmpty(pardakht.Description))
						{
							PardakhtBusiness pardakhtBusiness = pardakht;
							pardakhtBusiness.Description = await SetKhazanehDescriptionAsync(system, token, tranName);
						}
						pardakht.IsTemp = GetIsTemp();
						pardakht.Date = GetDate().AddSeconds(1.0);
						pardakht.SetId2(0, token);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await pardakht.SaveAsync(system, sendToServer, Priority.Low, token, awaitOnSemaphore, tryCount, tranName, await4Sent: false, sanad, messageSender), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3274);
						line = 25;
						if (!ret.HasError)
						{
							goto IL_22de;
						}
						await pardakht.RemoveAsync(system, tryCount, token, tranName);
						if (!ret.HasError)
						{
							goto IL_22de;
						}
						result = ret;
					}
					else
					{
						if (FactorType != Document.OrderSell && FactorType != Document.OrderBackBuy)
						{
							goto IL_2968;
						}
						if (system != EnSystem.Transfer && receptionSum <= 0m)
						{
							ReceptionBusiness receptionBusiness2 = await ReceptionBusiness.GetFromDbAsync(ContextGuid, ReceptionGuid ?? Guid.Empty, system, this, token, tranName);
							if (receptionBusiness2 != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness2.RemoveAsync(system, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3292);
							}
							RemoveFromLinkedSanad(ReceptionGuid ?? Guid.Empty, Document.Reception, token);
							ReceptionGuid = null;
							OrderBusiness orderBusiness3 = this;
							ReceptionBusiness reception2;
							reception = (reception2 = null);
							orderBusiness3._reception = reception2;
							sb.AppendLine($"RemoveReception Finished {tempSw.ElapsedMilliseconds} ms");
							tempSw.Restart();
							goto IL_2968;
						}
						reception.RemoveFromLinkedSanad(Guid, FactorType);
						line = 35;
						reception.ServerStatus = ServerStatusForClient.LinkToOrder;
						if (string.IsNullOrEmpty(reception.Description))
						{
							ReceptionBusiness receptionBusiness = reception;
							receptionBusiness.Description = await SetKhazanehDescriptionAsync(system, token, tranName);
						}
						reception.IsTemp = GetIsTemp();
						reception.Date = GetDate().AddSeconds(1.0);
						reception.SetId2(0, token);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await reception.SaveAsync(system, sendToServer, Priority.Low, awaitOnSemaphore: true, CustomerUserName, CustomerPassword, timeout, await4sent, tryCount, tranName, token, sanad), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3309);
						sb.AppendLine($"ReceptionSave Finished {tempSw.ElapsedMilliseconds} ms");
						tempSw.Restart();
						if (!ret.HasError)
						{
							goto IL_28dc;
						}
						line = 40;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await reception.RemoveAsync(system, awaitOnSemaphore: false, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3323);
						if (!ret.HasError)
						{
							goto IL_28dc;
						}
						result = ret;
					}
					goto end_IL_0e39;
					IL_452c:
					sb.AppendLine($"After GetIsTemp {tempSw.ElapsedMilliseconds} ms");
					tempSw.Restart();
					line = 634;
					if (ret.HasError || _isTemp || system == EnSystem.Transfer || system == EnSystem.None || source != ENSource.Accounting)
					{
						goto IL_47c3;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3513), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3513);
					sb.AppendLine($"Transaction Closed {tempSw.ElapsedMilliseconds} ms");
					tempSw.Restart();
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, FactorType, GetDate(), 10, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3516);
					sb.AppendLine($"Kardex Fill {tempSw.ElapsedMilliseconds} ms");
					tempSw.Restart();
					if (!ret.HasError)
					{
						goto IL_47c3;
					}
					result = ret;
					goto end_IL_0e39;
					IL_22de:
					line = 30;
					PardakhtGuid = pardakht.Guid;
					await AddToLinkedSanadsAsync(PardakhtGuid.Value, Document.Pardakht, pardakhtSum);
					goto IL_2968;
					IL_28dc:
					ReceptionGuid = reception.Guid;
					await AddToLinkedSanadsAsync(ReceptionGuid.Value, Document.Reception, receptionSum);
					goto IL_2968;
					IL_2968:
					if (FactorType != Document.OrderSell || installment == null)
					{
						goto IL_2ce5;
					}
					installment.ServerStatus = ServerStatusForClient.LinkToOrder;
					AghsatMSanadRecord sanadRecord = new AghsatMSanadRecord
					{
						DateM = GetDate().AddSeconds(1.0)
					};
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await installment.SaveAsync(system, token, Priority.High, sendToServer, await4sent, awaitOnSemaphore, tranName, sanad, sanadRecord), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3336);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3337), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3337);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await installment.UpdateSumAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3338);
					if (!ret.HasError)
					{
						goto IL_2ccf;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await installment.RemoveAsync(system, sendToServer: true, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3342);
					if (!ret.HasError)
					{
						goto IL_2ccf;
					}
					result = ret;
					goto end_IL_0e39;
					IL_47c3:
					if (system != EnSystem.Transfer)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await UpdateDateMoaserAsync(ContextGuid, Guid, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3522);
					}
					if (system == EnSystem.UserInterface && !ret.HasError)
					{
						IEnumerable<OrderDetailBusiness> enumerable = ListKala.Where((OrderDetailBusiness q) => (q.VGuid ?? Guid.Empty) != Guid.Empty);
						if (enumerable != null && enumerable.Any())
						{
							Dictionary<Guid, Guid> dictionary = new Dictionary<Guid, Guid>();
							foreach (OrderDetailBusiness item3 in enumerable)
							{
								dictionary.TryGetValue(item3.ProductGuid, out var value);
								if (value == Guid.Empty)
								{
									dictionary.Add(item3.ProductGuid, item3.VGuid.Value);
								}
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await LocalSettingBusiness.SetLastUnitGuidAsync(ContextGuid, CashierBusiness.GetCurrentCashierGuid(ContextGuid), FactorType, dictionary, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3534);
						}
						await SaveTempBusiness.RemoveRangeAsync(ContextGuid, Guid, FactorType, token, tranName);
					}
					if (ret.HasError)
					{
						result = ret;
						goto end_IL_0e39;
					}
					List<Guid> list5 = ListKala?.Where((OrderDetailBusiness q) => (q?.DicountCouponDetailGuid ?? Guid.Empty) != Guid.Empty)?.Select((OrderDetailBusiness q) => q.DicountCouponDetailGuid.Value)?.Distinct()?.ToList();
					if (list5 != null && list5.Any())
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CouponDiscountBusiness.UpdateServerStatusAsync(ContextGuid, list5, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3544);
					}
					goto end_IL_033d;
					end_IL_0e39:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}\r\n" + $"#System_{system}\r\n" + $"ClsCache.GetCache(ContextGuid).CacheRepository.IOrder is null:{ClsCache.GetCache(ContextGuid)?.CacheRepository?.IOrder == null}\r\n" + "orderJson:" + ToString(isIndented: true), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3548);
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3552);
					goto end_IL_033d;
				}
				return result;
				end_IL_033d:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3558), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3558);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3559), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3559);
				}
				if (!ret.HasError)
				{
					ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.ClearCachedItemAsync(Guid, token, autoCommit ? "" : tranName);
					if (system != EnSystem.Transfer)
					{
						OrderBusiness orderBusiness4 = null;
						if (_oldOrder != null)
						{
							orderBusiness4 = await OrderMapper.Instance.MapAsync(ContextGuid, _oldOrder, new OrderBusiness(ContextGuid));
						}
						string contextGuid = ContextGuid;
						EnSystem system2 = system;
						bool sendToServer2 = sendToServer;
						EnLog logType = GetLogType();
						ENAction action2 = action;
						OrderBusiness obj = this;
						OrderBusiness oldObject = orderBusiness4;
						CancellationToken token2 = token;
						CashiersLogBusiness.Logger(contextGuid, system2, sendToServer2, logType, action2, obj, oldObject, "", null, token2);
					}
					ClsCache.GetCache(ContextGuid)?.WebService?.Order?.ForceToSendDataToServer();
					if (sendToServer && !isTemp && system != EnSystem.Transfer)
					{
						if (await4sent)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await Sed2ServerAsync(sendToServer, isTemp, await4sent, CustomerUserName, CustomerPassword, timeout, autoCommit ? "" : tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3576);
						}
						else
						{
							Task.Run(() => Sed2ServerAsync(sendToServer, isTemp, await4sent, CustomerUserName, CustomerPassword, timeout, "", token));
						}
					}
					ChangeStatus = EntityChangeSt.Edited;
					if (messageSender != null && system != EnSystem.Transfer)
					{
						Task.Run(() => SendMessageAsync(system, _originalChangeSt, messageSender, changeListForSms, token));
					}
					if (system != EnSystem.Transfer)
					{
						_oldOrder = await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.GetFromDbAsync(Guid, FactorType, token, autoCommit ? "" : tranName);
					}
					if (system != EnSystem.Transfer)
					{
						ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.Order.RaiseOrderSaved(this, EventArgs.Empty);
					}
					if (ClsCache.GetCache(ContextGuid).BufferEnable && source == ENSource.Accounting && system != EnSystem.Transfer)
					{
						Task.Run(() => SendEntitiesToServerAsync(sendEntitiesToServerToken));
					}
					if (system == EnSystem.UserInterface)
					{
						ClsCache.GetCache(ContextGuid).BusinessAggregator.CouponDiscountBusiness.ForceUpdateCashBackCoupons();
					}
				}
				if (ret.HasError && source == ENSource.Accounting)
				{
					Task.Run(() => SanadBusiness.RemoveDuplicateSanadNoAsync(ContextGuid, token));
				}
				if (sw.ElapsedMilliseconds > 1000)
				{
					WebErrorLog.Instance.StartLog("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3602);
				}
				if (ret.HasError && system != EnSystem.UserInterface && !token.IsCancellationRequested)
				{
					SaveLogAsync(system, ret);
				}
			}
			return ret;
		}
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token)
	{
		using (new JaegerMonitor("SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 4;
			try
			{
				await Task.Delay(500);
				await Task.WhenAll<ReturnedSaveFuncInfo>(LastPriceBusiness.ResendNotSentsAsync(ContextGuid, EnSystem.None, token, "", 20), CouponDiscountBusiness.ResendNotSentsAsync(ContextGuid, token, "", 10));
				if (!token.IsCancellationRequested)
				{
					await Task.Delay(checked((await LocalSettingBusiness.GetWebServiceSettingsAsync(ContextGuid, token)).OperationEntitiesSendToServerSeconds * 1000));
					if (!token.IsCancellationRequested)
					{
						await Task.WhenAll<ReturnedSaveFuncInfo>(KolBusiness.ResendNotSents(ContextGuid, token, "", 5), MoeinBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), TafsilBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), ProductBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), PrdSerialBusiness.ResendNotSentsAsync(ContextGuid, token, "", 5));
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3631);
			}
		}
	}

	public async Task SendMessageAsync(EnSystem system, EntityChangeSt originalChangeSt, IMessageSender messageSender, List<ProductsChanges> changeList = null, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 9;
			try
			{
				AccGlobalSettings settings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				if (!settings.Payamak.RunAfterInitializeProgram || FactorType != Document.OrderSell)
				{
					return;
				}
				if (settings.Payamak.SellAccountStatus != PayamakSendStatus.Disable)
				{
					string message4 = "";
					List<string> lstNumbers = new List<string>();
					SanadPrintBusiness sanadPrint = await SanadPrintBusiness.GetAsync(ContextGuid, Guid, FactorType, token);
					message4 = ((sanadPrint == null) ? settings.Payamak.SellMessage : settings.Payamak.SellMessage_Motafareghe);
					if (sanadPrint == null)
					{
						lstNumbers.AddRange((await TafsilNumberBusiness.GetAsync(ContextGuid, _tafsilGuid))?.Where((TafsilNumberBusiness p) => p.Mobile)?.Select((TafsilNumberBusiness p) => p.NumberForDisplay()));
					}
					else
					{
						if (!string.IsNullOrEmpty(sanadPrint.Mob))
						{
							lstNumbers.Add(sanadPrint.Mob);
						}
						if (!string.IsNullOrEmpty(sanadPrint.Tel))
						{
							lstNumbers.Add(sanadPrint.Tel);
						}
					}
					if (lstNumbers?.Any() ?? false)
					{
						message4 = await FillSsmsParametersAsync(system, message4, sanadPrint, token);
						if (!string.IsNullOrEmpty(message4))
						{
							string mobiles = string.Join(",", lstNumbers);
							CancellationToken tokenSend4 = new CancellationTokenSource(60000).Token;
							Task.Run(() => messageSender.SendSmsAsync((sanadPrint == null) ? settings.Payamak.SellManagerStatus : settings.Payamak.SellAccountStatus, settings.Payamak.Account_ID, message4, mobiles, tokenSend4));
						}
					}
				}
				if (!(changeList?.Any() ?? false) || string.IsNullOrEmpty(settings.Payamak.Payamak_Manage_Mobile))
				{
					return;
				}
				IEnumerable<PrdAnbarRelationBusiness> listData = null;
				if (settings.Payamak.ProductNegativeSend)
				{
					if (listData == null)
					{
						listData = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, system, changeList, token);
					}
					if (!(listData?.Any() ?? false))
					{
						return;
					}
					List<PrdAnbarRelationBusiness> list = listData.Where((PrdAnbarRelationBusiness q) => q.Amount < 0m)?.ToList();
					if (list != null && list.Any())
					{
						string message3 = await PrdAnbarRelationBusiness.FillSmsParametersAsync(ContextGuid, this, list, system, settings.Payamak.ProductNegativeMessage, token);
						if (!string.IsNullOrEmpty(message3))
						{
							CancellationToken tokenSend3 = new CancellationTokenSource(60000).Token;
							Task.Run(() => messageSender.SendSmsAsync(PayamakSendStatus.ImedSend, settings.Payamak.Account_ID, message3, settings.Payamak.Payamak_Manage_Mobile, tokenSend3));
						}
					}
				}
				if (settings.Payamak.ProductZiroSend)
				{
					if (listData == null)
					{
						listData = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, system, changeList, token);
					}
					if (!(listData?.Any() ?? false))
					{
						return;
					}
					List<PrdAnbarRelationBusiness> list2 = listData.Where((PrdAnbarRelationBusiness q) => q.Amount == 0m)?.ToList();
					if (list2 != null && list2.Any())
					{
						string message2 = await PrdAnbarRelationBusiness.FillSmsParametersAsync(ContextGuid, this, list2, system, settings.Payamak.ProductNegativeMessage, token);
						if (!string.IsNullOrEmpty(message2))
						{
							CancellationToken tokenSend2 = new CancellationTokenSource(60000).Token;
							Task.Run(() => messageSender.SendSmsAsync(PayamakSendStatus.ImedSend, settings.Payamak.Account_ID, message2, settings.Payamak.Payamak_Manage_Mobile, tokenSend2));
						}
					}
				}
				if (!settings.Payamak.ProductOrderPointSend)
				{
					return;
				}
				if (listData == null)
				{
					listData = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, system, changeList, token);
				}
				if (!(listData?.Any() ?? false))
				{
					return;
				}
				List<PrdAnbarRelationBusiness> list3 = listData.Where((PrdAnbarRelationBusiness q) => q.Min > 0m && q.Amount < q.Min)?.ToList();
				if (list3 == null || !list3.Any())
				{
					return;
				}
				string message = await PrdAnbarRelationBusiness.FillSmsParametersAsync(ContextGuid, this, list3, system, settings.Payamak.ProductNegativeMessage, token);
				if (!string.IsNullOrEmpty(message))
				{
					CancellationToken tokenSend = new CancellationTokenSource(60000).Token;
					Task.Run(() => messageSender.SendSmsAsync(PayamakSendStatus.ImedSend, settings.Payamak.Account_ID, message, settings.Payamak.Payamak_Manage_Mobile, tokenSend));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3723);
				throw;
			}
		}
	}

	public static async Task<List<SmsTemplateRecord>> GetSmsTemplateListAsync(string contextGuid)
	{
		try
		{
			List<SmsTemplateRecord> list = new List<SmsTemplateRecord>
			{
				new SmsTemplateRecord("[نام مشتری]"),
				new SmsTemplateRecord("[شماره]"),
				new SmsTemplateRecord("[تاریخ]"),
				new SmsTemplateRecord("[جمع اقلام]"),
				new SmsTemplateRecord("[تخفیف]"),
				new SmsTemplateRecord("[قابل پرداخت]"),
				new SmsTemplateRecord("[حساب فعلی]"),
				new SmsTemplateRecord("[عنوان فروشگاه]"),
				new SmsTemplateRecord("[تلفن فروشگاه]")
			};
			NPSMS_WebServiceSettings nPSMS_WebServiceSettings = await LocalSettingBusiness.GetWebServiceSettingsAsync(contextGuid);
			list.Add(new SmsTemplateRecord("[لینک مشاهده فاکتور]", !string.IsNullOrEmpty(nPSMS_WebServiceSettings?.NPACCUserName) && !string.IsNullOrEmpty(nPSMS_WebServiceSettings?.NPACCPassword)));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSmsTemplateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3750);
			throw;
		}
	}

	private async Task<string> FillSsmsParametersAsync(EnSystem system, string message, SanadPrintBusiness sp, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("FillSsmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 5;
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, "", token);
				if (message.Contains("[نام مشتری]"))
				{
					if (string.IsNullOrEmpty(sp?.Name))
					{
						string text = message;
						message = text.Replace("[نام مشتری]", await GetFullNameAsync(system, "", token));
					}
					else
					{
						message = message.Replace("[نام مشتری]", sp.Name);
					}
				}
				if (message.Contains("[شماره]"))
				{
					string text = message;
					message = text.Replace("[شماره]", (await GetId2Async(system, "", token)).ToString("N0"));
				}
				if (message.Contains("[تاریخ]"))
				{
					message = message.Replace("[تاریخ]", GetDate().MiladiToCalture());
				}
				if (message.Contains("[جمع اقلام]"))
				{
					message = message.Replace("[جمع اقلام]", SumKalaKhadamat.NpToString("N0", 0));
				}
				if (message.Contains("[تخفیف]"))
				{
					message = message.Replace("[تخفیف]", Discount.NpToString("N0", 0));
				}
				if (message.Contains("[قابل پرداخت]"))
				{
					string text = message;
					message = text.Replace("[قابل پرداخت]", (await RemAsync(system, token)).AccountFormated(shortFormat: true));
				}
				if (message.Contains("[حساب فعلی]"))
				{
					message = message.Replace("[حساب فعلی]", account.AccountFormated(shortFormat: true));
				}
				if (message.Contains("[عنوان فروشگاه]"))
				{
					message = message.Replace("[عنوان فروشگاه]", setting.CompanyInfo.CompanyTitle);
				}
				if (message.Contains("[تلفن فروشگاه]"))
				{
					message = message.Replace("[تلفن فروشگاه]", setting.CompanyInfo.Telephone);
				}
				if (message.Contains("[لینک مشاهده فاکتور]"))
				{
					message = message.Replace("[لینک مشاهده فاکتور]", await GenerateLinkAync(system));
				}
				return message;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillSsmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3783);
				throw;
			}
		}
	}

	private Tuple<Document, Guid>[] GetTupleParams(EnSystem system)
	{
		using (new JaegerMonitor("GetTupleParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				if (FactorType == Document.OrderSell || FactorType == Document.OrderBackBuy)
				{
					if (system == EnSystem.Transfer)
					{
						return new Tuple<Document, Guid>[1]
						{
							new Tuple<Document, Guid>(FactorType, Guid)
						};
					}
					return new Tuple<Document, Guid>[4]
					{
						new Tuple<Document, Guid>(FactorType, Guid),
						new Tuple<Document, Guid>(Document.Reception, ReceptionGuid ?? Guid.Empty),
						new Tuple<Document, Guid>(Document.Tolid, Tolid?.Guid ?? Guid.Empty),
						new Tuple<Document, Guid>(Document.AghsatMProfit, AghsatGuid ?? Guid.Empty)
					};
				}
				if (FactorType == Document.OrderBuy || FactorType == Document.OrderBackSell)
				{
					if (system == EnSystem.Transfer)
					{
						return new Tuple<Document, Guid>[1]
						{
							new Tuple<Document, Guid>(FactorType, Guid)
						};
					}
					return new Tuple<Document, Guid>[2]
					{
						new Tuple<Document, Guid>(FactorType, Guid),
						new Tuple<Document, Guid>(Document.Pardakht, PardakhtGuid ?? Guid.Empty)
					};
				}
				if (FactorType == Document.Zayeat || FactorType == Document.AmaniKhoroj || FactorType == Document.AmaniVorod)
				{
					return new Tuple<Document, Guid>[1]
					{
						new Tuple<Document, Guid>(FactorType, Guid)
					};
				}
				throw new NotImplementedException($"FactorType is :{FactorType}");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetTupleParams", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3828);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sed2ServerAsync(bool SendToServer = true, bool isTemp = false, bool await4sent = false, string CustomerUserName = "", string CustomerPassword = "", int timeout = 60000, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _paralelOrderSend2ServerSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			line = 1;
			TafsilBusiness tafsilBusiness = await GetTafsilAsync(EnSystem.None, tranName, token);
			if (!(!ret.HasError && SendToServer) || isTemp || (tafsilBusiness.ServerStatus != ServerStatusForClient.Delivered && tafsilBusiness.ServerStatus != ServerStatusForClient.DirectDelivery))
			{
				throw new NotImplementedException();
			}
			line = 2;
			_send2ServerToken?.Cancel();
			_send2ServerToken = new CancellationTokenSource(60000);
			CancellationToken send2ServerToken = _send2ServerToken.Token;
			if (await4sent)
			{
				await Send2ServerAsync(EnSystem.None, CustomerUserName, CustomerPassword, timeout, send2ServerToken, "OrderBusiness/Sed2ServerAsync_/await4sent=True", tranName);
			}
			else
			{
				Task.Run(() => Send2ServerAsync(EnSystem.None, CustomerUserName, CustomerPassword, timeout, send2ServerToken, "OrderBusiness/Sed2ServerAsync_/await4sent=False", tranName));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sed2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3859);
			ret.AddReturnedValue(ex, "", "Sed2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3860);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_paralelOrderSend2ServerSemaphore.Release();
			}
		}
		return ret;
	}

	private async Task<string> SetKhazanehDescriptionAsync(EnSystem system, CancellationToken token, string tranName)
	{
		string translate = "بابت".GetTranslate();
		string display = FactorType.GetDisplay();
		string translate2 = " شماره".GetTranslate();
		int num = await GetId2Async(system, tranName, token);
		return translate + "  " + display + " " + translate2 + $" {num}";
	}

	private async Task<ReturnedSaveFuncInfo> Send2ServerAsync(EnSystem system, string customerUserName, string customerPassword, int timeout, CancellationToken token, string caller, string tranName = "")
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (system == EnSystem.Transfer)
				{
					throw new NotImplementedException();
				}
				line = 1500;
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.CrmPortal && FactorType == Document.OrderSell)
				{
					line = 1600;
					ReceptionBusiness receptionBusiness = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
					if (Sum != (receptionBusiness?.Sum ?? 0m))
					{
						throw new Exception($"Order Sum:{Sum:N0} \r\n" + $"Reception Sum:{receptionBusiness?.Sum ?? 0m:N0} \r\n" + "Reception: " + receptionBusiness);
					}
				}
				line = 1;
				if (token.IsCancellationRequested)
				{
					return ret.AddError(system, "OrderBusiness 1 cancellationToken is cancelled!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3915);
				}
				line = 2;
				await ProductBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20);
				line = 3;
				if (token.IsCancellationRequested)
				{
					return ret.AddError(system, "OrderBusiness 2 cancellationToken is cancelled!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3919);
				}
				line = 4;
				await CusBusiness.ResendNotSentsAsync(ContextGuid, token, "", 5);
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddError(system, "OrderBusiness 3 cancellationToken is cancelled!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3923);
				}
				line = 6;
				List<WebOrder> list = new List<WebOrder>();
				List<WebOrder> list2 = list;
				list2.Add(await ToWebItemAsync());
				line = 7;
				List<Packet> packets = WebOrder.GenerateSaveRequest(list);
				line = 8;
				if (token.IsCancellationRequested)
				{
					return ret.AddError(system, "OrderBusiness 4 cancellationToken is cancelled!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3930);
				}
				line = 9;
				if (!string.IsNullOrEmpty(customerUserName) && !string.IsNullOrEmpty(customerPassword))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).SendOnCustomerBuffer(customerUserName, customerPassword, packets, token, Priority.High, timeout), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3933);
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).SendAndWaitForResponse(packets, token, 20, 2, 10, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3935);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line} \r\n Caller:{caller}", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3939);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3940);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveTempAsync(EnSystem system, string customerUsername = "", string customerPassword = "", CancellationToken token = default(CancellationToken), string tranName = "", int timeOut = 60000)
	{
		using (new JaegerMonitor("SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo result;
			try
			{
				_ = 7;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetIsTempAsync(system, value: true, token, tranName), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3953);
					if (token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3954);
						goto IL_091c;
					}
					EntityDeliveryStatusBusiness obj = new EntityDeliveryStatusBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						EntityGuid = Guid,
						EntityPart = EntityPart.Order,
						IsTemp = true
					};
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(token), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3963);
					OrderSaveTempWaiter waiter = new OrderSaveTempWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), _guid, token, timeOut);
					if (token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3966);
						goto IL_091c;
					}
					List<WebOrder> list = new List<WebOrder>();
					List<WebOrder> list2 = list;
					list2.Add(await ToWebItemAsync());
					List<Packet> list3 = WebOrder.GenerateSaveTempRequest(list);
					if (token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3968);
						goto IL_091c;
					}
					if (ret.HasError)
					{
						result = ret;
						goto IL_091c;
					}
					if (!string.IsNullOrEmpty(customerUsername))
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).SendOnCustomerBuffer(customerUsername, customerPassword, list3, token, Priority.High, timeOut), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3971);
					}
					else
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).SendData(list3, Priority.High), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3973);
					}
					ReturnedSaveFuncInfoWithValue<SaveResponse> res = await waiter.WaitForSaveResponse("OrderBusinessWaiter");
					if (token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3975);
						goto IL_091c;
					}
					if (!res.HasError && res.value != null)
					{
						ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await SetSaveResult(system, res.value, token, 1), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3978);
						if (res.value.SaveResultStatus)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await EntityDeliveryStatusBusiness.RemoveAsync(ContextGuid, Guid, EntityPart.Order, isTemp: true, token), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3980);
						}
					}
				}
				catch (ThreadAbortException exception)
				{
					ret.AddReturnedValue(exception, "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3983);
				}
				catch (OperationCanceledException exception2)
				{
					ret.AddReturnedValue(exception2, "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3984);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3985);
					ret.AddReturnedValue(ex, "", "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3985);
					throw;
				}
			}
			finally
			{
				if (autoCommit)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3989), ReturnedState.None, "SaveTempAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 3989);
				}
			}
			return ret;
			IL_091c:
			return result;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ResendTempsNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Order;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال فاکتورهای موقت";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = res;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4008);
				}
				if (ClsCache.GetCache(contextGuid)?.CacheRepository?.IOrder == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.IOrder");
				}
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Android_HesabYar && ClsCache.GetCache(contextGuid)?.CacheRepository?.EntityDeliveryStatus == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.EntityDeliveryStatus");
				}
				IEnumerable<EntityDeliveryStatusBusiness> enumerable = await EntityDeliveryStatusBusiness.GetAllAsync(contextGuid, EntityPart.Order, isTemp: true, token);
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4015);
				}
				if (enumerable == null || !enumerable.Any())
				{
					return res;
				}
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = enumerable?.Count() ?? 0;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (EntityDeliveryStatusBusiness item in enumerable)
				{
					if (token.IsCancellationRequested)
					{
						return res.AddErrorTokenIsCancelled("", "", "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4022);
					}
					if (item == null)
					{
						continue;
					}
					current = checked(current + 1);
					if (uiUpdater != null)
					{
						uiUpdater.Count = current;
					}
					if (item != null)
					{
						OrderBusiness orderBusiness = await GetAsync(contextGuid, item.EntityGuid, Document.OrderSell, fromDatabase: true, token, tranName);
						if (orderBusiness != null)
						{
							tasks.Add(orderBusiness.SaveTempAsync(EnSystem.None, "", "", token, tranName));
						}
					}
					if (tasks.Count > maxParalelTasks)
					{
						Task<ReturnedSaveFuncInfo> completedTask = await Task.WhenAny(tasks);
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await completedTask, ReturnedState.None, "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4036);
						tasks.Remove(completedTask);
					}
				}
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4042);
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4043);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4047);
				res.AddReturnedValue(ex, "", "ResendTempsNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4048);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (res.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return res;
		}
	}

	public async Task<WebOrder> ToWebItemAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				WebOrder obj = await OrderMapper.Instance.MapAsync(ContextGuid, this, new WebOrder(), "", token);
				obj.FillDescription();
				return obj;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4079);
				return null;
			}
		}
	}

	public string AutoTitle(ENAction act)
	{
		throw new NotImplementedException();
	}

	public async Task<string> TotalBarcodeAsync(EnSystem system, bool BarcodeTotalEnable, string format, string Contorol, double ZaribRial, string cashierCode, CancellationToken token, string tranName)
	{
		return new BarcodeCreator(format, Contorol, ZaribRial, 0, await GetId2Async(system, tranName, token), Sum, GetDate(), cashierCode, 0, DigitAfterPoint).TotalBarcode();
	}

	public async Task JoinAsync(EnSystem system, OrderJoinConfig config, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("JoinAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 15;
			try
			{
				if (config.ClearCurrentOrderItems)
				{
					foreach (OrderDetailBusiness item in ListKala?.ToList() ?? new List<OrderDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				if ((config?.Order?.ListKala?.Any()).GetValueOrDefault())
				{
					foreach (WebOrderDetail det in config.Order.ListKala.ToList())
					{
						OrderDetailBusiness detBusiness = await AddDetAsync(system, isGood: true, setting.Sells.AddOriginalPriceToOrder, awaitOnSemaphore: true, null, token);
						await OrderDetMapper.GetInstance(null).MapAsync(ContextGuid, det, detBusiness, "", token);
						ProductCompactBusiness productCompactBusiness = await detBusiness.GetPrdInfoAsync(system, token);
						if (config.PriceTypeGuid == Guid.Empty)
						{
							await detBusiness.SetPriceAsync(system, det.Price, token);
						}
						else if (config.PriceTypeGuid == ParentDefaults.PriceLevel.LastSellToAll)
						{
							await detBusiness.SetPriceAsync(system, (await KardexBusiness.GetLastAsync(ContextGuid, null, productCompactBusiness.Guid, Document.OrderSell, token))?.UnitPrice ?? 0m, token);
						}
						else if (config.PriceTypeGuid == ParentDefaults.PriceLevel.LastSellToTafsil)
						{
							await detBusiness.SetPriceAsync(system, (await KardexBusiness.GetLastAsync(ContextGuid, GetTafsilGuid(), productCompactBusiness.Guid, Document.OrderSell, token))?.UnitPrice ?? 0m, token);
						}
						else if (config.PriceTypeGuid == ParentDefaults.PriceLevel.LastBuyFromTafsil)
						{
							await detBusiness.SetPriceAsync(system, (await KardexBusiness.GetLastAsync(ContextGuid, GetTafsilGuid(), productCompactBusiness.Guid, Document.OrderBuy, token))?.UnitPrice ?? 0m, token);
						}
						else if (config.PriceTypeGuid == ParentDefaults.PriceLevel.LastBuyFromAll)
						{
							await detBusiness.SetPriceAsync(system, (await KardexBusiness.GetLastAsync(ContextGuid, null, productCompactBusiness.Guid, Document.OrderBuy, token))?.UnitPrice ?? 0m, token);
						}
						else
						{
							ProductPriceRelationFilter filter = new ProductPriceRelationFilter
							{
								PriceLevelGuid = config.PriceTypeGuid,
								ProductGuid = productCompactBusiness.Guid
							};
							decimal valueOrDefault = ((await ProductPriceRelationBusiness.GetAllAsync(ContextGuid, system, filter, token))?.SingleOrDefault()?.Price).GetValueOrDefault();
							await detBusiness.SetPriceAsync(system, valueOrDefault, token);
						}
						detBusiness.Guid = Guid.NewGuid();
						if (!config.ChangeDiscount)
						{
							await detBusiness.SetDiscountAsync(system, 0m, token);
						}
					}
				}
				if (!config.ChangeDiscount)
				{
					config.Order.Discount = 0m;
				}
				if (!config.ChangeVisitor)
				{
					config.Order.B1MoeinGuid = null;
					config.Order.B1TafsilGuid = null;
					config.Order.B2MoeinGuid = null;
					config.Order.B2TafsilGuid = null;
					config.Order.B3MoeinGuid = null;
					config.Order.B3TafsilGuid = null;
					config.Order.B4MoeinGuid = null;
					config.Order.B4TafsilGuid = null;
					config.Order.B1 = 0m;
					config.Order.B2 = 0m;
					config.Order.B3 = 0m;
					config.Order.B4 = 0m;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "JoinAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4162);
			}
		}
	}

	public async Task UpdateTolidAsync(OrderDetailBusiness det, ProductCompactBusiness prd, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("UpdateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				if (prd == null)
				{
					return;
				}
				await _setTolidSemaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				if (det == null || ClsCache.GetCache(ContextGuid) == null || FactorType != Document.OrderSell || ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting || det.Quantity <= 0m)
				{
					return;
				}
				decimal oldTolidExportCount = default(decimal);
				TolidBusiness tolidBusiness = await GetOldTolidAsync(token);
				if (tolidBusiness != null)
				{
					oldTolidExportCount = ((await tolidBusiness.GetExportListAsync(EnSystem.None, tranName, token))?.ListKala?.Where((OrderDetailBusiness q) => q.ProductGuid == det.ProductGuid))?.Sum((OrderDetailBusiness q) => q.Quantity) ?? 0m;
				}
				ProductsChanges productsChanges = await GetProductsAnbarChangesAsync(EnSystem.None, det.ProductGuid, oldTolidExportCount, tranName, token);
				decimal qt = productsChanges?.qt ?? 0m;
				List<TolidFormulaBusiness> tolidFormul = new List<TolidFormulaBusiness>();
				decimal am = ((await prd.GetAnbarRelationAsync(ContextGuid, productsChanges?.depGuid ?? Guid.Empty, token, tranName))?.Amount ?? 0m) + qt;
				if (am > 0m)
				{
					if (_tolid != null)
					{
						await _tolid.ClearListDetByFormulaAsync(tolidFormul?.FirstOrDefault(), EnSystem.None, token);
					}
					return;
				}
				if (((await prd.GetTolidFormulaGuidAsync(token, tranName)) ?? Guid.Empty) != Guid.Empty)
				{
					string contextGuid = ContextGuid;
					TolidFormulaBusiness tolidFormulaBusiness = await TolidFormulaBusiness.GetAsync(contextGuid, (await prd.GetTolidFormulaGuidAsync()).Value, token);
					if (tolidFormulaBusiness != null)
					{
						tolidFormul.Add(tolidFormulaBusiness);
						if (Math.Round(Math.Abs(am), 3) > 1m && _tolid != null)
						{
							await _tolid.ClearListDetByFormulaAsync(tolidFormulaBusiness, EnSystem.None, token, tranName);
							if (!((await _tolid.GetExportListAsync(EnSystem.None, tranName, token))?.ListKala?.Any()).GetValueOrDefault())
							{
								_tolid = null;
							}
						}
					}
				}
				else
				{
					tolidFormul = await TolidFormulaBusiness.GetAllByExportPrdGuidAsync(ContextGuid, det.ProductGuid, token, tranName);
				}
				if (prd == null && _tolid != null)
				{
					await _tolid.ClearListDetByFormulaAsync(tolidFormul?.FirstOrDefault(), EnSystem.None, token, tranName);
				}
				else if (tolidFormul?.Any() ?? false)
				{
					await SetTolidAsync(await GenerateTolidAsync(EnSystem.None, tolidFormul.FirstOrDefault(), Math.Round(Math.Abs(am), 3), token, tranName), det.ProductGuid, EnSystem.None, tolidFormul.FirstOrDefault(), token, tranName);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4221);
				throw;
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_setTolidSemaphore.Release();
				}
			}
		}
	}

	public async Task Det_OnQuantityChanged(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnQuantityChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (eventManager?.OnSumChanged_static == null || ClsCache.GetCache(ContextGuid) == null)
				{
					return;
				}
				_sumChangedToken?.Cancel();
				_sumChangedToken = new CancellationTokenSource();
				CancellationToken token = _sumChangedToken.Token;
				await Task.Delay(new Random().Next(500));
				OrderDetailBusiness det = (OrderDetailBusiness)sender;
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar)
				{
					ProductCompactBusiness product = await det.GetPrdInfoAsync();
					if (product == null)
					{
						return;
					}
					bool flag = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Sells.AddTolidAfterNegativeOrder;
					if (!flag)
					{
						flag = ((await product.GetTolidFormulaGuidAsync()) ?? Guid.Empty) != Guid.Empty;
					}
					if (flag)
					{
						_updateTolidToken?.Cancel();
						_updateTolidToken = new CancellationTokenSource(60000);
						CancellationToken tolidToken = _updateTolidToken.Token;
						Task.Run(() => UpdateTolidAsync(det, product, "", tolidToken));
					}
				}
				if (eventManager?.OnSumChanged_static == null || !(TotalSum != _lastSum))
				{
					return;
				}
				try
				{
					await _modifiedDetListSemaphore.WaitAsync();
					using (new JaegerMonitor("Det_OnQuantityChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
					{
						_lastSum = TotalSum;
						PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "Det_OnQuantityChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
				}
				finally
				{
					_modifiedDetListSemaphore?.Release();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Det_OnQuantityChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4268);
				throw;
			}
		}
	}

	public async Task Det_OnRowSumChanged(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (eventManager?.OnSumChanged_static == null)
				{
					return;
				}
				_sumChangedToken?.Cancel();
				_sumChangedToken = new CancellationTokenSource();
				CancellationToken token = _sumChangedToken.Token;
				await Task.Delay(new Random().Next(500));
				if (token.IsCancellationRequested || eventManager?.OnSumChanged_static == null || !(TotalSum != _lastSum))
				{
					return;
				}
				try
				{
					await _modifiedDetListSemaphore.WaitAsync();
					using (new JaegerMonitor("Det_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
					{
						_lastSum = TotalSum;
						PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "Det_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
				}
				finally
				{
					_modifiedDetListSemaphore?.Release();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Det_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4299);
			}
		}
	}

	public async Task<OrderDetailBusiness> AddDetAsync(EnSystem system, bool isGood, bool isLoadOriginalPrice, bool awaitOnSemaphore, Guid? guid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 7;
			try
			{
				if (awaitOnSemaphore)
				{
					await _modifiedDetListSemaphore.WaitAsync();
				}
				using (new JaegerMonitor("AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					if (_listOrderDetails?.Any() ?? false)
					{
						foreach (OrderDetailBusiness item in _listOrderDetails.ToList())
						{
							if (item.Quantity <= 0m)
							{
								item.Dispose();
							}
						}
					}
					if ((guid ?? Guid.Empty) != Guid.Empty && (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Factors.InsertPRDQT && _listOrderDetails?.SingleOrDefault((OrderDetailBusiness q) => q.ProductGuid == guid.Value) != null)
					{
						throw new InvalidOperationException("در فاکتور خطی کالا به صورت تکراری اضافه می شود");
					}
					OrderDetailBusiness ret;
					if (isGood)
					{
						OrderDetailBusiness orderDetailBusiness = await OrderDetailBusiness.OrderDetailFactoryAsync(ContextGuid, this, token, tranName);
						orderDetailBusiness.PriceTypeGuid = PriceTypeGuid;
						ret = await AddDetAsync(orderDetailBusiness, system, awaitOnSemaphor: false, token);
						if ((guid ?? Guid.Empty) != Guid.Empty)
						{
							await ret.SetProductGuidAsync(system, guid.Value, isLoadOriginalPrice, "", token);
						}
						ret.IsKala = true;
						return ret;
					}
					OrderDetailBusiness orderDetailBusiness2 = await OrderDetailBusiness.OrderDetailFactoryAsync(ContextGuid, this, token, tranName);
					orderDetailBusiness2.PriceTypeGuid = PriceTypeGuid;
					ret = await AddDetAsync(orderDetailBusiness2, system, awaitOnSemaphor: false, token);
					if ((guid ?? Guid.Empty) != Guid.Empty)
					{
						await ret.SetProductGuidAsync(system, guid.Value, isLoadOriginalPrice, "", token);
					}
					ret.IsKala = false;
					return ret;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4347);
				throw;
			}
			finally
			{
				if (awaitOnSemaphore)
				{
					_modifiedDetListSemaphore?.Release();
				}
			}
		}
	}

	public async Task<OrderDetailBusiness> AddDetAsync(GardeshTafsilBusiness item, EnSystem system, bool isLoadOriginalPrice, bool awaitOnSemaphor, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await AddDetAsync(await OrderDetailBusiness.OrderDetailFactoryAsync(ContextGuid, system, this, item, isLoadOriginalPrice, token, tranName), system, awaitOnSemaphor, token);
	}

	public async Task<OrderDetailBusiness> AddDetAsync(OrderDetailBusiness det, EnSystem system, bool awaitOnSemaphor, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				if (awaitOnSemaphor)
				{
					await _modifiedDetListSemaphore.WaitAsync();
				}
				using (new JaegerMonitor("AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					if (det != null)
					{
						det.OnQuantityChanged += Det_OnQuantityChanged;
						det.OnRowSumChanged += Det_OnRowSumChanged;
						det.OnDisposed += Det_OnDisposed;
						det.OnVisitorChanged += Det_OnVisitorChanged;
						det.OnDepotChanged += Det_OnDepotChanged;
						det.OnOrdinalChanged += Det_OnOrdinalChanged;
						if (_listOrderDetails == null)
						{
							_listOrderDetails = new List<OrderDetailBusiness>();
						}
						_listOrderDetails.Add(det);
						det.Ordinal = _listOrderDetails.Count;
						if (this.OnChanged != null)
						{
							PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
						}
						if (eventManager?.OnItemAdd != null)
						{
							PacketParser.Utilities.RaiseEvent(eventManager.OnItemAdd, this, det, token, 60, "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
						}
						if (eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
						{
							_lastSum = TotalSum;
							PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
						}
						Task.Run(() => CheckForTolidAsync(det, system, token));
					}
					return det;
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "AddDetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4392);
				throw;
			}
			finally
			{
				if (awaitOnSemaphor)
				{
					_modifiedDetListSemaphore?.Release();
				}
			}
		}
	}

	private async Task CheckForTolidAsync(OrderDetailBusiness det, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar || FactorType != Document.OrderSell || system != EnSystem.UserInterface)
			{
				return;
			}
			ProductCompactBusiness product = await det.GetPrdInfoAsync();
			if (product != null)
			{
				bool flag = (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Sells.AddTolidAfterNegativeOrder;
				if (!flag)
				{
					flag = ((await product.GetTolidFormulaGuidAsync()) ?? Guid.Empty) != Guid.Empty;
				}
				if (flag)
				{
					_updateTolidToken?.Cancel();
					_updateTolidToken = new CancellationTokenSource(60000);
					CancellationToken token2 = _updateTolidToken.Token;
					UpdateTolidAsync(det, product, "", token2);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckForTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4416);
		}
	}

	public async Task AddToLinkedSanadsAsync(Guid relatedSanadGuid, Document relatedSanadType, decimal price, bool isClear = false)
	{
		using (new JaegerMonitor("AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				await _modifiedDetListSemaphore.WaitAsync();
				using (new JaegerMonitor("AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					TableRelationShipBusiness tableRelationShipBusiness = _linkedSanad?.ChildList?.FirstOrDefault((TableRelationShipBusiness q) => q.SanadGuid == relatedSanadGuid && q.SanadType == relatedSanadType);
					if (tableRelationShipBusiness != null && tableRelationShipBusiness.Price == price)
					{
						return;
					}
					if (tableRelationShipBusiness != null && tableRelationShipBusiness.Price != price)
					{
						tableRelationShipBusiness.Price = price;
						return;
					}
					TableRelationShipBusiness item = new TableRelationShipBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						Guid = Guid.NewGuid(),
						IsInRelationSanad = false,
						Price = price,
						SanadGuid = relatedSanadGuid,
						SanadType = relatedSanadType,
						Status = true
					};
					if (_linkedSanad == null)
					{
						_linkedSanad = new TableRelationShipBusiness(ContextGuid);
					}
					if (isClear)
					{
						_linkedSanad?.ClearDets();
					}
					await _linkedSanad.AddChildListAsync(new List<TableRelationShipBusiness> { item });
					if (eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
					{
						_lastSum = TotalSum;
						PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, default(CancellationToken), 60, "AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4456);
				throw;
			}
			finally
			{
				_modifiedDetListSemaphore?.Release();
			}
		}
	}

	public async Task AddListToLinkedSanadsAsync(IEnumerable<TableRelationShipBusiness> lstDets, bool isClear = false)
	{
		using (new JaegerMonitor("AddListToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				foreach (TableRelationShipBusiness tblRelation in lstDets ?? new List<TableRelationShipBusiness>())
				{
					if (_linkedSanad?.ChildList?.FirstOrDefault((TableRelationShipBusiness q) => q.SanadGuid == tblRelation.SanadGuid && q.SanadType == tblRelation.SanadType) == null)
					{
						if (_linkedSanad == null)
						{
							_linkedSanad = new TableRelationShipBusiness(ContextGuid);
						}
						if (isClear)
						{
							_linkedSanad.ClearDets();
						}
						await _linkedSanad.AddChildListAsync(new List<TableRelationShipBusiness> { tblRelation });
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "AddListToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4479);
				throw;
			}
		}
	}

	private Task Det_OnDepotChanged(object arg1, EventArgs arg2)
	{
		try
		{
			CancellationToken token = default(CancellationToken);
			if (this.OnChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Det_OnDepotChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
			if (eventManager?.OnDepotChanged_static != null)
			{
				PacketParser.Utilities.RaiseEvent(eventManager.OnDepotChanged_static, this, new EventArgs(), token, 60, "Det_OnDepotChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnDepotChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4491);
		}
		return Task.CompletedTask;
	}

	private async Task Det_OnVisitorChanged(object arg1, EventArgs arg2)
	{
		try
		{
			CancellationToken token = default(CancellationToken);
			if (this.OnChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Det_OnVisitorChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
			if (eventManager?.OnVisitorChanged_static != null)
			{
				await PacketParser.Utilities.RaiseEvent(eventManager.OnVisitorChanged_static, this, new EventArgs(), token, 60, "Det_OnVisitorChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnVisitorChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4505);
			throw;
		}
	}

	private async Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 3;
			try
			{
				if (_modifiedDetListSemaphore != null)
				{
					await _modifiedDetListSemaphore.WaitAsync();
				}
				using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					CancellationToken token = new CancellationToken(IsDisposed);
					if (_tolid != null)
					{
						List<TolidFormulaBusiness> list = await TolidFormulaBusiness.GetAllByExportPrdGuidAsync(ContextGuid, ((OrderDetailBusiness)sender).ProductGuid, token);
						if (list != null && list.Count > 0)
						{
							await _tolid.ClearListDetByFormulaAsync(list.FirstOrDefault(), EnSystem.None, token);
						}
						if (!((await _tolid.GetExportListAsync(EnSystem.None, "", token))?.ListKala?.Any()).GetValueOrDefault())
						{
							_tolid = null;
						}
					}
					List<OrderDetailBusiness> list2 = _listOrderDetails?.ToList()?.Where((OrderDetailBusiness p) => p == null || p == sender)?.ToList();
					if (list2 != null && list2.Any())
					{
						List<OrderDetailBusiness> list3 = list2?.Where((OrderDetailBusiness q) => q != null)?.SelectMany((OrderDetailBusiness q) => q.DetailList)?.ToList();
						if (list3 != null && list3.Any())
						{
							foreach (OrderDetailBusiness item in list3)
							{
								item?.Dispose();
							}
						}
					}
					_listOrderDetails?.RemoveAll((OrderDetailBusiness p) => p == null || p == sender);
					if (this.OnChanged != null)
					{
						PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
					if (eventManager?.OnItemRemoved != null)
					{
						PacketParser.Utilities.RaiseEvent(eventManager.OnItemRemoved, this, (OrderDetailBusiness)sender, token, 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4543);
				throw;
			}
			finally
			{
				_modifiedDetListSemaphore?.Release();
			}
		}
	}

	public async Task<List<OrderDetailBusiness>> AddDetListAsync(List<OrderDetailBusiness> lstOrderDet, bool clearDets, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		List<OrderDetailBusiness> ret = new List<OrderDetailBusiness>();
		using (new JaegerMonitor("AddDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				if (_listOrderDetails == null)
				{
					_listOrderDetails = new List<OrderDetailBusiness>();
				}
				if (clearDets)
				{
					ClearListKala();
					ClearListService();
				}
				line = 1;
				if (lstOrderDet == null)
				{
					lstOrderDet = new List<OrderDetailBusiness>();
				}
				if (lstOrderDet.Any())
				{
					foreach (OrderDetailBusiness item in lstOrderDet)
					{
						line = 2;
						if (item != null)
						{
							ret.Add(await AddDetAsync(item, system, awaitOnSemaphor: true, token));
						}
						line = 3;
					}
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"List<OrderDetailBusiness> lstOrderDet//Line:{line}", "AddDetListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4580);
			}
			return ret;
		}
	}

	public bool IsDuplicate(OrderDetailBusiness str)
	{
		return (_listOrderDetails?.Where((OrderDetailBusiness p) => p != null && p.Guid != str.Guid && p.ProductGuid == str.ProductGuid && p.DepotGuid == str.DepotGuid && p.Price == str.Price)?.Any()).GetValueOrDefault();
	}

	public void ClearListKala()
	{
		_listOrderDetails?.RemoveAll((OrderDetailBusiness p) => p?.IsKala ?? true);
	}

	public void ClearListService()
	{
		_listOrderDetails?.RemoveAll((OrderDetailBusiness p) => p == null || !p.IsKala);
	}

	public async Task<ReturnedSaveFuncInfo> SanadDetIsOkAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SanadDetIsOkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (!(ConnectedSanad.ListSanad_Det?.Any() ?? false))
				{
					return res;
				}
				IEnumerable<Sanad_DetBusiness> enumerable = ConnectedSanad.ListSanad_Det.Where((Sanad_DetBusiness q) => q.Debit <= 0m && q.Credit <= 0m && q.MoeinGuid == Guid.Empty && (q.TafsilGuid ?? Guid.Empty) == Guid.Empty);
				if (enumerable != null && enumerable.Any())
				{
					foreach (Sanad_DetBusiness item in enumerable)
					{
						await ConnectedSanad.RemoveFromListSanad(item.Guid, system, token);
					}
				}
				if (!(ConnectedSanad.ListSanad_Det?.Any() ?? false))
				{
					return res;
				}
				decimal num = default(decimal);
				decimal num2 = default(decimal);
				int num3 = 1;
				foreach (Sanad_DetBusiness item2 in ConnectedSanad.ListSanad_Det)
				{
					num += item2.Debit;
					num2 += item2.Credit;
					item2.AsType = FactorType;
					res.AddReturnedValue(item2.CheckValidation(system, num3, isDeleteAction), ReturnedState.None, "SanadDetIsOkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4621);
					num3 = checked(num3 + 1);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SanadDetIsOkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4627);
				res.AddReturnedValue(ex, "", "SanadDetIsOkAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4628);
			}
			return res;
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (OrderDetailBusiness item in _listOrderDetails?.ToList() ?? new List<OrderDetailBusiness>())
				{
					item?.Dispose();
				}
				_listOrderDetails = null;
				_linkedSanad = null;
				_reception?.Dispose();
				_reception = null;
				_pardakht?.Dispose();
				_pardakht = null;
				_modifiedDetListSemaphore?.Dispose();
				_modifiedDetListSemaphore = null;
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4659);
			}
		}
	}

	public async Task<string> GetFactorFooterLabelAsync(CancellationToken token, string tranName)
	{
		StringBuilder ret = new StringBuilder(100000);
		try
		{
			ReceptionBusiness reception = await GetReceptionAsync(EnSystem.UserInterface, tranName, fromDatabase: false, token);
			decimal beforeAccount = await GetBeforeAccountAsync(EnSystem.UserInterface, token, tranName);
			decimal afterAccount = await GetAfterAccountAsync(EnSystem.UserInterface, token, tranName);
			await GetTafsilAsync(EnSystem.UserInterface, tranName, token);
			decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, tranName, token);
			if (Discount > 0m)
			{
				ret.Append("جمع اقلام");
				ret.Append("<br/>");
				ret.Append("تخفیف");
				ret.Append("<br/>");
			}
			ret.Append("جمع کل");
			bool flag = reception != null && reception.Sum > 0m;
			if (!flag)
			{
				bool flag2 = await GetPardakhtAsync(EnSystem.UserInterface, token, tranName) != null;
				if (flag2)
				{
					flag2 = (await GetPardakhtAsync(EnSystem.UserInterface, token, tranName)).Sum > 0m;
				}
				flag = flag2;
			}
			if (flag)
			{
				ret.Append("<br/>");
				if (reception.Sum > 0m)
				{
					ret.Append("دریافتی");
				}
				else
				{
					ret.Append("پرداختی");
				}
				ret.Append("<br/>");
				ret.Append("مانده فاکتور");
			}
			if (beforeAccount != 0m)
			{
				ret.Append("<br/>مانده از قبل");
				ret.Append("<br/>مانده پس از فاکتور");
			}
			if (afterAccount != account)
			{
				ret.Append("<br/>مانده فعلی");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactorFooterLabelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4704);
		}
		return ret.ToString();
	}

	public async Task<string> GetFactorFooterValueAsync(string tranName, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		StringBuilder ret = new StringBuilder(100000);
		try
		{
			decimal pardakhtSum = await PardakhtSumAsync(EnSystem.UserInterface, token, tranName);
			decimal receptionSum = await ReceptionSumAsync(EnSystem.UserInterface, token, tranName);
			PardakhtBusiness pardakht = await GetPardakhtAsync(EnSystem.UserInterface, token, tranName);
			ReceptionBusiness reception = await GetReceptionAsync(EnSystem.UserInterface, tranName, fromDatabase: false, token);
			decimal beforeAccount = await GetBeforeAccountAsync(EnSystem.UserInterface, token, tranName);
			decimal afterAccount = await GetAfterAccountAsync(EnSystem.UserInterface, token, tranName);
			TafsilBusiness tafsil = await GetTafsilAsync(EnSystem.UserInterface, tranName, token);
			decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, tranName, token);
			line = 1;
			if (Discount > 0m)
			{
				line = 2;
				ret.Append(SumKala.ToString("N0", CultureInfo.CreateSpecificCulture("fa-Ir")));
				line = 3;
				ret.Append("<br/>");
				line = 4;
				ret.Append(DiscountString);
				line = 5;
				ret.Append("<br/>");
				line = 6;
			}
			line = 7;
			ret.Append(Sum.ToString("N0", CultureInfo.CreateSpecificCulture("fa-Ir")));
			line = 8;
			if ((reception != null && reception.Sum > 0m) || (pardakht != null && pardakht.Sum > 0m))
			{
				line = 9;
				ret.Append("<br/>");
				line = 10;
				if (reception.Sum > 0m)
				{
					ret.Append(receptionSum.ToString("N0", CultureInfo.CreateSpecificCulture("fa-Ir")));
				}
				else
				{
					StringBuilder stringBuilder = ret;
					stringBuilder.Append((await PardakhtSumAsync(EnSystem.UserInterface, token, tranName)).ToString("N0", CultureInfo.CreateSpecificCulture("fa-Ir")));
				}
				line = 20;
				ret.Append("<br/>");
				line = 21;
				ret.Append((TotalSum - receptionSum - pardakhtSum).ToString("N0", CultureInfo.CreateSpecificCulture("fa-Ir")));
				line = 22;
			}
			line = 23;
			if (beforeAccount != 0m)
			{
				line = 24;
				ret.Append("<br/>" + Math.Abs(beforeAccount).NpToString("N0", 0) + " " + beforeAccount.AccountTashkhis());
				line = 25;
				ret.Append("<br/>" + Math.Abs(afterAccount).NpToString("N0", 0) + afterAccount.AccountTashkhis());
				line = 26;
			}
			line = 27;
			if (tafsil != null && afterAccount != account)
			{
				ret.Append("<br/>" + Math.Abs(account).NpToString("N0", 0) + account.AccountTashkhis());
			}
			line = 30;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetFactorFooterValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4770);
		}
		return ret.ToString();
	}

	public async Task<string> ReceptionSummaryAsync(CancellationToken token, string tranName, EnSystem system)
	{
		using (new JaegerMonitor("ReceptionSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			StringBuilder ret = new StringBuilder(100000);
			try
			{
				ReceptionBusiness reception = await GetReceptionAsync(EnSystem.UserInterface, tranName, fromDatabase: false, token);
				if (reception != null && reception.ListNaghd?.Count > 0)
				{
					foreach (ReceptionNaghdBusiness naghd in reception.ListNaghd)
					{
						if (naghd != null && naghd.Price > default(decimal))
						{
							if (ret.ToString() == "")
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append(await naghd.GetAndroidPrintTextAsync(system, token, tranName) + "  مبلغ:" + naghd.PriceString);
							}
							else
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append("<br/>" + await naghd.GetAndroidPrintTextAsync(system, token, tranName) + "  مبلغ:" + naghd.PriceString);
							}
						}
					}
				}
				if (reception != null && reception.ListBank?.Count > 0)
				{
					foreach (ReceptionHavaleBusiness bank in reception.ListBank)
					{
						if (bank != null && bank.Price > default(decimal))
						{
							if (ret.ToString() == "")
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append(await bank.GetAndroidPrintTextAsync(system, token, tranName) + "   مبلغ:" + bank.PriceString);
							}
							else
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append("<br/>" + await bank.GetAndroidPrintTextAsync(system, token, tranName) + "   مبلغ:" + bank.PriceString);
							}
						}
					}
				}
				if (reception != null && reception.ListCheck?.Count > 0)
				{
					foreach (ReceptionCheckBusiness item in reception.ListCheck)
					{
						if (item != null && item.Price > default(decimal))
						{
							if (ret.ToString() == "")
							{
								ret.Append(item.AndroidPrintText + "    مبلغ:" + item.PriceString);
							}
							else
							{
								ret.Append("<br/>" + item.AndroidPrintText + "     مبلغ:" + item.PriceString);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReceptionSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4805);
			}
			return ret.ToString();
		}
	}

	public static Guid ValidGuid_Next(Guid guid, Document factorType)
	{
		throw new NotImplementedException();
	}

	public async Task<string> PaymentSummaryAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("PaymentSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			StringBuilder ret = new StringBuilder(100000);
			try
			{
				PardakhtBusiness pardakht = await GetPardakhtAsync(EnSystem.UserInterface, token, tranName);
				PardakhtBusiness obj = await GetPardakhtAsync(EnSystem.UserInterface, token, tranName);
				if (obj != null && obj.Naghd?.Count > 0)
				{
					foreach (PardakhtNaghdBusiness naghd in (await GetPardakhtAsync(EnSystem.UserInterface, token, tranName)).Naghd)
					{
						if (naghd != null && naghd.Price > default(decimal))
						{
							if (ret.ToString() == "")
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append(await naghd.GetAndroidPrintTextAsync(system, token, tranName) + "  مبلغ:" + naghd.PriceString);
							}
							else
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append("<br/>" + await naghd.GetAndroidPrintTextAsync(system, token, tranName) + "  مبلغ:" + naghd.PriceString);
							}
						}
					}
				}
				if (pardakht != null && pardakht.Bank?.Count > 0)
				{
					foreach (PardakhtBankBusiness bank in pardakht.Bank)
					{
						if (bank != null && bank.Price > default(decimal))
						{
							if (ret.ToString() == "")
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append(await bank.GetAndroidPrintTextAsync(system, token, tranName) + "   مبلغ:" + bank.PriceString);
							}
							else
							{
								StringBuilder stringBuilder = ret;
								stringBuilder.Append("<br/>" + await bank.GetAndroidPrintTextAsync(system, token, tranName) + "   مبلغ:" + bank.PriceString);
							}
						}
					}
				}
				if (pardakht != null && pardakht.CheckSh?.Count > 0)
				{
					foreach (PardakhtCheckShBusiness item in pardakht.CheckSh)
					{
						if (item != null && item.Price > default(decimal))
						{
							if (ret.ToString() == "")
							{
								ret.Append(item.AndroidPrintText + "    مبلغ:" + item.PriceString);
							}
							else
							{
								ret.Append("<br/>" + item.AndroidPrintText + "     مبلغ:" + item.PriceString);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PaymentSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4840);
			}
			return ret.ToString();
		}
	}

	public async Task<decimal> RemAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (system == EnSystem.Transfer || system == EnSystem.Mapper)
			{
				return default(decimal);
			}
			if (FactorType == Document.OrderSell || FactorType == Document.OrderBackBuy)
			{
				decimal temp = TotalSum;
				return await ReceptionSumAsync(system, token, tranName) - temp;
			}
			if (FactorType == Document.OrderBuy || FactorType == Document.OrderBackSell)
			{
				decimal totalSum = TotalSum;
				decimal temp = totalSum;
				return temp - await PardakhtSumAsync(system, token, tranName);
			}
			return default(decimal);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4873);
			return default(decimal);
		}
	}

	public SanadBusiness GetConnectedSanad(EnSystem system)
	{
		try
		{
			if (connectedSanad == null)
			{
				connectedSanad = new SanadBusiness(ContextGuid, 0);
			}
			if (system != EnSystem.Mapper)
			{
				IEnumerable<Sanad_DetBusiness> enumerable = connectedSanad.ListSanad_Det?.Where((Sanad_DetBusiness q) => q.Debit <= 0m && q.Credit <= 0m && q.MoeinGuid == Guid.Empty && (q.TafsilGuid ?? Guid.Empty) == Guid.Empty);
				if (enumerable != null && enumerable.Any())
				{
					foreach (Sanad_DetBusiness item in enumerable)
					{
						item.Dispose();
					}
				}
			}
			return connectedSanad;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetConnectedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4892);
		}
		return null;
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				Guid userGuid = cashier.Guid;
				UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token);
				switch (FactorType)
				{
				case Document.OrderSell:
				{
					bool flag = userAccBusiness.UserAccess.Sell.SanadStatusAccess.SingleOrDefault((OrderSellSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.Sell.Sell_Delete || (userAccBusiness.UserAccess.Sell.Sell_Delete_Self && UserAccGuid == cashier.Guid)) && flag;
				}
				case Document.OrderBuy:
				{
					bool flag = userAccBusiness.UserAccess.Buy.SanadStatusAccess.SingleOrDefault((OrderBuySanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.Buy.Buy_Delete || (userAccBusiness.UserAccess.Buy.Buy_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.OrderBackBuy:
				{
					bool flag = userAccBusiness.UserAccess.BackBuy.SanadStatusAccess.SingleOrDefault((OrderBackBuySanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.BackBuy.BackBuy_Delete || (userAccBusiness.UserAccess.BackBuy.BackBuy_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.OrderBackSell:
				{
					bool flag = userAccBusiness.UserAccess.BackSell.SanadStatusAccess.SingleOrDefault((OrderBackSellSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.BackSell.BackSell_Delete || (userAccBusiness.UserAccess.BackSell.BackSell_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.Zayeat:
				{
					bool flag = userAccBusiness.UserAccess.Zayeat.SanadStatusAccess.SingleOrDefault((OrderZayeatSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.Zayeat.Zayeat_Delete || (userAccBusiness.UserAccess.Zayeat.Zayeat_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.PishFactor:
				{
					bool flag = userAccBusiness.UserAccess.PishFacotr.SanadStatusAccess.SingleOrDefault((OrderPreSellSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.PishFacotr.PishFactor_Delete || (userAccBusiness.UserAccess.PishFacotr.PishFactor_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.OrderPreBuy:
				{
					bool flag = userAccBusiness.UserAccess.PreBuy.SanadStatusAccess.SingleOrDefault((OrderPreBuySanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.PreBuy.PreBuy_Delete || (userAccBusiness.UserAccess.PreBuy.PreBuy_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.AmaniKhoroj:
				{
					bool flag = userAccBusiness.UserAccess.AmaniSadere.SanadStatusAccess.SingleOrDefault((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.AmaniSadere.AmaniSadere_Delete || (userAccBusiness.UserAccess.AmaniSadere.AmaniSadere_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.AmaniVorod:
				{
					bool flag = userAccBusiness.UserAccess.AmaniVarede.SanadStatusAccess.SingleOrDefault((OrderAmaniVorodSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
					return (userAccBusiness.UserAccess.AmaniSadere.AmaniSadere_Delete || (userAccBusiness.UserAccess.AmaniSadere.AmaniSadere_Delete_Self && UserAccGuid == userGuid)) && flag;
				}
				default:
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 4979);
				return false;
			}
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				Guid userGuid = cashierBusiness.Guid;
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				switch (FactorType)
				{
				case Document.OrderSell:
				{
					bool flag = userAccBusiness2.UserAccess.Sell.SanadStatusAccess.SingleOrDefault((OrderSellSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					bool flag2 = userAccBusiness2.UserAccess.Sell.Sell_Update_NODate || GetDate().Date == DateTime.Now.Date;
					return (userAccBusiness2.UserAccess.Sell.Sell_Update || (userAccBusiness2.UserAccess.Sell.Sell_Update_Self && UserAccGuid == userGuid)) && flag && flag2;
				}
				case Document.OrderBuy:
				{
					bool flag = userAccBusiness2.UserAccess.Buy.SanadStatusAccess.SingleOrDefault((OrderBuySanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.Buy.Buy_Update || (userAccBusiness2.UserAccess.Buy.Buy_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.OrderBackBuy:
				{
					bool flag = userAccBusiness2.UserAccess.BackBuy.SanadStatusAccess.SingleOrDefault((OrderBackBuySanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.BackBuy.BackBuy_Update || (userAccBusiness2.UserAccess.BackBuy.BackBuy_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.OrderBackSell:
				{
					bool flag = userAccBusiness2.UserAccess.BackSell.SanadStatusAccess.SingleOrDefault((OrderBackSellSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.BackSell.BackSell_Update || (userAccBusiness2.UserAccess.BackSell.BackSell_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.Zayeat:
				{
					bool flag = userAccBusiness2.UserAccess.Zayeat.SanadStatusAccess.SingleOrDefault((OrderZayeatSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.Zayeat.Zayeat_Update || (userAccBusiness2.UserAccess.Zayeat.Zayeat_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.PishFactor:
				{
					bool flag = userAccBusiness2.UserAccess.PishFacotr.SanadStatusAccess.SingleOrDefault((OrderPreSellSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.PishFacotr.PishFactor_Update || (userAccBusiness2.UserAccess.PishFacotr.PishFactor_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.OrderPreBuy:
				{
					bool flag = userAccBusiness2.UserAccess.PreBuy.SanadStatusAccess.SingleOrDefault((OrderPreBuySanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.PreBuy.PreBuy_Update || (userAccBusiness2.UserAccess.PreBuy.PreBuy_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.AmaniKhoroj:
				{
					bool flag = userAccBusiness2.UserAccess.AmaniSadere.SanadStatusAccess.SingleOrDefault((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.AmaniSadere.AmaniSadere_Update || (userAccBusiness2.UserAccess.AmaniSadere.AmaniSadere_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				case Document.AmaniVorod:
				{
					bool flag = userAccBusiness2.UserAccess.AmaniVarede.SanadStatusAccess.SingleOrDefault((OrderAmaniVorodSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
					return (userAccBusiness2.UserAccess.AmaniSadere.AmaniSadere_Update || (userAccBusiness2.UserAccess.AmaniSadere.AmaniSadere_Update_Self && UserAccGuid == userGuid)) && flag;
				}
				default:
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5038);
				return false;
			}
		}
	}

	public async Task<bool> CanPrintAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			int line = 0;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				line = 1;
				UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token);
				line = 2;
				Guid guid = cashier.Guid;
				line = 3;
				switch (FactorType)
				{
				case Document.OrderSell:
					line = 4;
					return userAccBusiness.UserAccess.Sell.Sell_Print || (userAccBusiness.UserAccess.Sell.Sell_Print_Self && UserAccGuid == guid);
				case Document.OrderBuy:
					line = 5;
					return userAccBusiness.UserAccess.Buy.Buy_Print || (userAccBusiness.UserAccess.Buy.Buy_Print_Self && UserAccGuid == guid);
				case Document.OrderBackBuy:
					line = 6;
					return userAccBusiness.UserAccess.BackBuy.BackBuy_Print || (userAccBusiness.UserAccess.BackBuy.BackBuy_Print_Self && UserAccGuid == guid);
				case Document.OrderBackSell:
					line = 7;
					return userAccBusiness.UserAccess.BackSell.BackSell_Print || (userAccBusiness.UserAccess.BackSell.BackSell_Print_Self && UserAccGuid == guid);
				case Document.Zayeat:
					line = 8;
					return userAccBusiness.UserAccess.Zayeat.Zayeat_Print || (userAccBusiness.UserAccess.Zayeat.Zayeat_Print_Self && UserAccGuid == guid);
				default:
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5082);
				return false;
			}
		}
	}

	public async Task<bool> CanReportAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				return await CanReportAsync(await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5096);
				return false;
			}
		}
	}

	public async Task<bool> CanReportAsync(CashierBusiness cashier)
	{
		using (new JaegerMonitor("CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				Guid UserGuid = cashier.Guid;
				AccessLevel accessLevel = await cashier.GetUserAccessAsync();
				return FactorType switch
				{
					Document.OrderSell => accessLevel.Sell.Sell_Report || (accessLevel.Sell.Sell_ReportSelf && UserAccGuid == UserGuid), 
					Document.OrderBuy => accessLevel.Buy.Buy_Report || (accessLevel.Buy.Buy_ReportSelf && UserAccGuid == UserGuid), 
					Document.OrderBackBuy => accessLevel.BackBuy.BackBuy_Report || (accessLevel.BackBuy.BackBuy_ReportSelf && UserAccGuid == UserGuid), 
					Document.OrderBackSell => accessLevel.BackSell.BackSell_Report || (accessLevel.BackSell.BackSell_ReportSelf && UserAccGuid == UserGuid), 
					Document.Zayeat => accessLevel.Zayeat.Zayeat_Report || (accessLevel.Zayeat.Zayeat_ReportSelf && UserAccGuid == UserGuid), 
					_ => true, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5119);
				return false;
			}
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				if (cashierBusiness == null)
				{
					return false;
				}
				UserAccBusiness userAccBusiness = await cashierBusiness.GetUserAsync(token);
				switch (FactorType)
				{
				case Document.OrderSell:
				{
					bool flag = userAccBusiness.UserAccess.Sell.SanadStatusAccess.SingleOrDefault((OrderSellSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.Sell.Sell_Insert && flag;
				}
				case Document.OrderBuy:
				{
					bool flag = userAccBusiness.UserAccess.Buy.SanadStatusAccess.SingleOrDefault((OrderBuySanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.Buy.Buy_Insert && flag;
				}
				case Document.OrderBackBuy:
				{
					bool flag = userAccBusiness.UserAccess.BackBuy.SanadStatusAccess.SingleOrDefault((OrderBackBuySanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.BackBuy.BackBuy_Insert && flag;
				}
				case Document.OrderBackSell:
				{
					bool flag = userAccBusiness.UserAccess.BackSell.SanadStatusAccess.SingleOrDefault((OrderBackSellSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.BackSell.BackSell_Insert && flag;
				}
				case Document.Zayeat:
				{
					bool flag = userAccBusiness.UserAccess.Zayeat.SanadStatusAccess.SingleOrDefault((OrderZayeatSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.Zayeat.Zayeat_Insert && flag;
				}
				case Document.PishFactor:
				{
					bool flag = userAccBusiness.UserAccess.PishFacotr.SanadStatusAccess.SingleOrDefault((OrderPreSellSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.PishFacotr.PishFactor_Insert && flag;
				}
				case Document.OrderPreBuy:
				{
					bool flag = userAccBusiness.UserAccess.PreBuy.SanadStatusAccess.SingleOrDefault((OrderPreBuySanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.PreBuy.PreBuy_Insert && flag;
				}
				case Document.AmaniKhoroj:
				{
					bool flag = userAccBusiness.UserAccess.AmaniSadere.SanadStatusAccess.SingleOrDefault((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.AmaniSadere.AmaniSadere_Insert && flag;
				}
				case Document.AmaniVorod:
				{
					bool flag = userAccBusiness.UserAccess.AmaniVarede.SanadStatusAccess.SingleOrDefault((OrderAmaniVorodSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
					return userAccBusiness.UserAccess.AmaniVarede.AmaniVarede_Insert && flag;
				}
				default:
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5176);
				return false;
			}
		}
	}

	public Task<bool> CanViewAmountsAsync(CancellationToken token = default(CancellationToken))
	{
		return CanViewAmountsStaticAsync(ContextGuid, FactorType, token);
	}

	public static async Task<bool> CanViewAmountsStaticAsync(string contextGuid, Document factorType, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CanViewAmountsStaticAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
				if (cashierBusiness == null)
				{
					return false;
				}
				AccessLevel accessLevel = await cashierBusiness.GetUserAccessAsync(token);
				switch (factorType)
				{
				case Document.OrderSell:
					return accessLevel.Sell.ViewInvoiceAmounts;
				case Document.OrderBuy:
					return accessLevel.Buy.ViewInvoiceAmounts;
				case Document.OrderBackBuy:
					return accessLevel.BackBuy.ViewInvoiceAmounts;
				case Document.OrderBackSell:
					return accessLevel.BackSell.ViewInvoiceAmounts;
				case Document.Zayeat:
					return accessLevel.Zayeat.ViewInvoiceAmounts;
				case Document.AmaniKhoroj:
					return accessLevel.AmaniSadere.ViewInvoiceAmounts;
				case Document.AmaniVorod:
					return accessLevel.AmaniVarede.ViewInvoiceAmounts;
				case Document.Tolid:
				case Document.Tolid_Mahsul:
				case Document.Tolid_Mavad:
					return await TolidBusiness.CanViewAmountsAsync(contextGuid, token);
				default:
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanViewAmountsStaticAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5207);
				return false;
			}
		}
	}

	public async Task<bool> EnableDiscount(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("EnableDiscount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			int line = 0;
			CashierBusiness cashier = null;
			UserAccBusiness user = null;
			try
			{
				bool flag = ChangeStatus == EntityChangeSt.Edited;
				if (flag)
				{
					flag = !(await CanUpdateAsync(token, tranName));
				}
				if (flag)
				{
					return false;
				}
				line = 1;
				cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				line = 2;
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				user = userAccBusiness;
				line = 3;
				switch (FactorType)
				{
				case Document.OrderSell:
					line = 4;
					return user.UserAccess.Sell.Sell_Discount;
				case Document.OrderBuy:
					line = 5;
					return user.UserAccess.Buy.Buy_EditDiscount;
				case Document.OrderBackBuy:
					line = 6;
					return user.UserAccess.BackBuy.BackBuy_EditDiscount;
				case Document.OrderBackSell:
					line = 7;
					return user.UserAccess.BackSell.BackSell_EditDiscount;
				case Document.PishFactor:
					line = 8;
					return user.UserAccess.PishFacotr.PishFactor_EditDiscount;
				case Document.OrderPreBuy:
					line = 8;
					return user.UserAccess.PreBuy.PreBuy_EditDiscount;
				default:
					line = 9;
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}/FactorTypr:{FactorType}/Guid:{Guid}/UserIsNull:{user == null}/CashierIsNull:{cashier == null}", "EnableDiscount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5252);
				return false;
			}
		}
	}

	public async Task<bool> EnableTax(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("EnableTax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (!(await CanUpdateAsync(token, tranName)))
				{
					return false;
				}
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return FactorType switch
				{
					Document.OrderSell => userAccBusiness2.UserAccess.Sell.Sell_EditTax, 
					Document.OrderBuy => userAccBusiness2.UserAccess.Buy.Buy_EditTax, 
					Document.OrderBackBuy => userAccBusiness2.UserAccess.BackBuy.BackBuy_EditTax, 
					Document.OrderBackSell => userAccBusiness2.UserAccess.BackSell.BackSell_EditTax, 
					_ => true, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableTax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5275);
				return false;
			}
		}
	}

	public async Task<bool> EnableAvarez(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("EnableAvarez", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (!(await CanUpdateAsync(token, tranName)))
				{
					return false;
				}
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return FactorType switch
				{
					Document.OrderSell => userAccBusiness2.UserAccess.Sell.Sell_EditAvarez, 
					Document.OrderBuy => userAccBusiness2.UserAccess.Buy.Buy_EditAvarez, 
					Document.OrderBackBuy => userAccBusiness2.UserAccess.BackBuy.BackBuy_EditAvarez, 
					Document.OrderBackSell => userAccBusiness2.UserAccess.BackSell.BackSell_EditAvarez, 
					_ => true, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableAvarez", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5303);
				return false;
			}
		}
	}

	public async Task<bool> EnableCalcSood(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("EnableCalcSood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return FactorType switch
				{
					Document.OrderSell => userAccBusiness2.UserAccess.Sell.Sell_CalcSood, 
					_ => true, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableCalcSood", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5326);
				return false;
			}
		}
	}

	public async Task<bool> EnableKeraye(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("EnableKeraye", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				bool flag = ChangeStatus == EntityChangeSt.Edited;
				if (flag)
				{
					flag = !(await CanUpdateAsync(token, tranName));
				}
				if (flag)
				{
					return false;
				}
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return FactorType switch
				{
					Document.OrderSell => userAccBusiness2.UserAccess.Sell.Sell_Keraye, 
					Document.OrderBackBuy => false, 
					Document.OrderBackSell => false, 
					_ => true, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableKeraye", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5352);
				return false;
			}
		}
	}

	public async Task<bool> EnableAghlamDiscount(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("EnableAghlamDiscount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				bool flag = ChangeStatus == EntityChangeSt.Edited;
				if (flag)
				{
					flag = !(await CanUpdateAsync(token, tranName));
				}
				if (flag)
				{
					return false;
				}
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return FactorType switch
				{
					Document.OrderSell => userAccBusiness2.UserAccess.Sell.Sell_EditAghlamDiscount, 
					Document.OrderBuy => userAccBusiness2.UserAccess.Buy.Buy_EditAghlamDiscount, 
					Document.OrderBackBuy => userAccBusiness2.UserAccess.BackBuy.BackBuy_EditAghlamDiscount, 
					Document.OrderBackSell => userAccBusiness2.UserAccess.BackSell.BackSell_EditAghlamDiscount, 
					Document.PishFactor => userAccBusiness2.UserAccess.PishFacotr.PishFactor_EditAghlamDiscount, 
					Document.OrderPreBuy => userAccBusiness2.UserAccess.PreBuy.PreBuy_EditAghlamDiscount, 
					_ => true, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableAghlamDiscount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5384);
				return false;
			}
		}
	}

	public async Task<bool> EnableEditDate(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("EnableEditDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (!(await CanUpdateAsync(token, tranName)))
				{
					return false;
				}
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				if (FactorType == Document.OrderSell)
				{
					return userAccBusiness2.UserAccess.Sell.Sell_EditDate;
				}
				return true;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableEditDate", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5404);
				return false;
			}
		}
	}

	public async Task<bool> EnableEditPrice(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("EnableEditPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				bool flag = ChangeStatus == EntityChangeSt.Edited;
				if (flag)
				{
					flag = !(await CanUpdateAsync(token, tranName));
				}
				if (flag)
				{
					return false;
				}
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				switch (FactorType)
				{
				case Document.OrderSell:
					return userAccBusiness2.UserAccess.Sell.Sell_EditPrice;
				case Document.OrderBuy:
					return userAccBusiness2.UserAccess.Buy.Buy_EditPrice;
				case Document.OrderBackBuy:
					return userAccBusiness2.UserAccess.BackBuy.BackBuy_EditPrice;
				case Document.OrderBackSell:
					return userAccBusiness2.UserAccess.BackSell.BackSell_EditPrice;
				case Document.PishFactor:
					return userAccBusiness2.UserAccess.PishFacotr.PishFactor_EditPrice;
				case Document.OrderPreBuy:
					return userAccBusiness2.UserAccess.PreBuy.PreBuy_EditPrice;
				case Document.Tolid:
				case Document.Tolid_Mahsul:
				case Document.Tolid_Mavad:
					return userAccBusiness2.UserAccess.Tolid.EditPriceMahsul;
				default:
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "EnableEditPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5433);
				return false;
			}
		}
	}

	public void SetFullNeme()
	{
		throw new NotImplementedException();
	}

	public decimal CountOfProduct()
	{
		return ListKala?.Sum((OrderDetailBusiness p) => p?.Quantity ?? 0m) ?? 0m;
	}

	public decimal CountOfProduct(Guid prdGuid)
	{
		try
		{
			if (ListKala?[0] == null || ListKala.Count == 0)
			{
				return 0m;
			}
			return ListKala.Where((OrderDetailBusiness p) => p.ProductGuid == prdGuid).ToList().Sum((OrderDetailBusiness x) => x.Quantity);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CountOfProduct", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5488);
			throw;
		}
	}

	public decimal LastDiscountPercent(Guid prdGuid)
	{
		try
		{
			if (ListKala?[0] == null || ListKala.Count == 0)
			{
				return 0m;
			}
			return ListKala.Where((OrderDetailBusiness p) => p.ProductGuid == prdGuid).ToList().Sum((OrderDetailBusiness x) => x.DiscountPercent);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LastDiscountPercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5502);
			throw;
		}
	}

	public decimal TotalSumOfProduct(Guid prdGuid)
	{
		try
		{
			if (ListKala?[0] == null || ListKala.Count == 0)
			{
				return 0m;
			}
			return ListKala.Where((OrderDetailBusiness p) => p.ProductGuid == prdGuid).ToList().Sum((OrderDetailBusiness x) => x.RowSum);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TotalSumOfProduct", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5516);
			return 0m;
		}
	}

	public async Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		using (new JaegerMonitor("GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			List<ProductsChanges> ret = new List<ProductsChanges>();
			try
			{
				OrderBusiness oldOrder = (OrderBusiness)(await GetOldItemAsync(system, token, tranName));
				if ((oldOrder?.KardexAffective ?? false) && !KardexAffective && !isDeleteAction)
				{
					isDeleteAction = true;
				}
				if ((oldOrder?.KardexAffective ?? false) || !KardexAffective || isDeleteAction)
				{
					if (oldOrder != null && oldOrder.ListKala?.Count > 0)
					{
						foreach (OrderDetailBusiness kala2 in oldOrder.ListKala.ToList())
						{
							await CalcChanges.ComputeOrderChangeAsync(new ComputeOrderChangesConfig(ContextGuid)
							{
								OrderDetial = kala2,
								FactorType = oldOrder.FactorType,
								PrdChanges = ret,
								IsNew = false,
								IsDeleteAction = isDeleteAction,
								System = system
							}, tranName);
							if (!(kala2.DetailList?.Any() ?? false))
							{
								continue;
							}
							IEnumerable<OrderDetailBusiness> enumerable = kala2.DetailList?.Where((OrderDetailBusiness q) => q.ProductGuid != kala2.ProductGuid);
							if (enumerable == null || !enumerable.Any())
							{
								continue;
							}
							foreach (OrderDetailBusiness item in enumerable)
							{
								await CalcChanges.ComputeOrderChangeAsync(new ComputeOrderChangesConfig(ContextGuid)
								{
									OrderDetial = item,
									FactorType = oldOrder.FactorType,
									PrdChanges = ret,
									IsNew = false,
									IsDeleteAction = isDeleteAction,
									System = system
								}, tranName);
							}
						}
					}
				}
				IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
				if (listKala != null && listKala.Count > 0)
				{
					foreach (OrderDetailBusiness kala in ListKala.ToList())
					{
						if (!isDeleteAction && kala.ProductGuid != Guid.Empty)
						{
							await CalcChanges.ComputeOrderChangeAsync(new ComputeOrderChangesConfig(ContextGuid)
							{
								OrderDetial = kala,
								FactorType = FactorType,
								PrdChanges = ret,
								IsNew = true,
								IsDeleteAction = false,
								System = system
							}, tranName);
						}
						if (!(kala.DetailList?.Any() ?? false))
						{
							continue;
						}
						IEnumerable<OrderDetailBusiness> enumerable2 = kala.DetailList?.Where((OrderDetailBusiness q) => q.ProductGuid != kala.ProductGuid);
						if (enumerable2 == null || !enumerable2.Any())
						{
							continue;
						}
						foreach (OrderDetailBusiness item2 in enumerable2)
						{
							if (!isDeleteAction && item2.ProductGuid != Guid.Empty)
							{
								await CalcChanges.ComputeOrderChangeAsync(new ComputeOrderChangesConfig(ContextGuid)
								{
									OrderDetial = item2,
									FactorType = FactorType,
									PrdChanges = ret,
									IsNew = true,
									IsDeleteAction = false,
									System = system
								}, tranName);
							}
						}
					}
				}
				return ret;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5608);
				throw;
			}
		}
	}

	private async Task<ProductsChanges> GetProductsAnbarChangesAsync(EnSystem system, Guid prdGuid, decimal originalTolidCount, string tranName, CancellationToken token)
	{
		using (new JaegerMonitor("GetProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			ProductsChanges productsChanges;
			try
			{
				List<ProductsChanges> ret = new List<ProductsChanges>();
				OrderBusiness oldOrder = (OrderBusiness)(await GetOldItemAsync(system, token, tranName));
				if (oldOrder != null && oldOrder.ListKala?.Count > 0)
				{
					foreach (OrderDetailBusiness item in oldOrder.ListKala?.Where((OrderDetailBusiness q) => q.ProductGuid == prdGuid)?.ToList())
					{
						await CalcChanges.ComputeOrderChangeAsync(new ComputeOrderChangesConfig(ContextGuid)
						{
							OrderDetial = item,
							FactorType = oldOrder.FactorType,
							PrdChanges = ret,
							IsNew = false,
							IsDeleteAction = true,
							System = system
						}, tranName);
					}
				}
				IReadOnlyList<OrderDetailBusiness> listKala = ListKala;
				if (listKala != null && listKala.Count > 0)
				{
					foreach (OrderDetailBusiness item2 in ListKala?.Where((OrderDetailBusiness q) => q.ProductGuid == prdGuid)?.ToList())
					{
						await CalcChanges.ComputeOrderChangeAsync(new ComputeOrderChangesConfig(ContextGuid)
						{
							OrderDetial = item2,
							FactorType = FactorType,
							PrdChanges = ret,
							IsNew = true,
							IsDeleteAction = false,
							System = system
						}, tranName);
					}
				}
				productsChanges = ret?.FirstOrDefault();
				if (productsChanges == null)
				{
					return null;
				}
				productsChanges.qt -= originalTolidCount;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5656);
				throw;
			}
			return productsChanges;
		}
	}

	private async Task<AghsatM_GroupBusiness> GetInstallmentAsync(CancellationToken token, string tranName)
	{
		try
		{
			if ((AghsatGuid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			if (_installment == null)
			{
				_installment = await AghsatM_GroupBusiness.GetAsync(ContextGuid, AghsatGuid.Value, token, tranName);
			}
			return _installment;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstallmentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5671);
			throw;
		}
	}

	public void ClearInstallment()
	{
		_installment = null;
	}

	public override async Task<BaseSanadBusiness<OrderBusiness>> GetOldItemAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetOldItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _getOldOrderSemaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				if (system == EnSystem.Transfer)
				{
					return _oldOrder;
				}
				if (_oldOrder == null)
				{
					_oldOrder = await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.GetFromDbAsync(_guid, FactorType, token, tranName);
				}
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_getOldOrderSemaphore.Release();
				}
			}
			return _oldOrder;
		}
	}

	public async Task<TolidBusiness> GetOldTolidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetOldTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _getOldTolidSemaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				if (_oldTolid == null)
				{
					_oldTolid = await ClsCache.GetCache(ContextGuid).CacheRepository.Tolid.GetFromDbAsync(Guid, token, tranName);
				}
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_getOldTolidSemaphore.Release();
				}
			}
			return _oldTolid;
		}
	}

	public async Task<Sanad_DetBusiness> AddToListSanadAsync(Guid asGuid, Document asType, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		if (asGuid != Guid || asType != FactorType)
		{
			throw new ArgumentException("asGuid or asType is not valid!");
		}
		return await AddToListSanadAsync(new Sanad_DetBusiness(ContextGuid, system, Guid, FactorType, ConnectedSanad.NextOrdinal())
		{
			MasterSanadItem = this
		}, system, token);
	}

	public async Task<Sanad_DetBusiness> AddToListSanadAsync(Sanad_DetBusiness item, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				item.MasterSanadItem = this;
				await ConnectedSanad.AddToListSanadAsync(item, system, token);
				return item;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5721);
				throw;
			}
		}
	}

	public Task AddRangeToListSanadAsync(List<Sanad_DetBusiness> Sanaddet, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		return AddRangeToListSanadAsync(Sanaddet, system, isClear: true, token);
	}

	public async Task RemoveFromListSanad(Guid Guid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				await _modifiedDetListSemaphore.WaitAsync();
				using (new JaegerMonitor("RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					ConnectedSanad.ListSanad_Det.FirstOrDefault((Sanad_DetBusiness p) => p.Guid == Guid)?.Dispose(system);
					if (eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
					{
						_lastSum = TotalSum;
						PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5743);
				throw;
			}
			finally
			{
				_modifiedDetListSemaphore?.Release();
			}
		}
	}

	public async void RemoveFromLinkedSanad(Guid sanadGuid, Document sanadType, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				await _modifiedDetListSemaphore.WaitAsync();
				using (new JaegerMonitor("RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
				{
					if (_linkedSanad == null)
					{
						_linkedSanad = new TableRelationShipBusiness(ContextGuid);
					}
					TableRelationShipBusiness tableRelationShipBusiness = _linkedSanad?.ChildList?.FirstOrDefault((TableRelationShipBusiness p) => p.SanadGuid == sanadGuid && p.SanadType == sanadType);
					if (tableRelationShipBusiness != null)
					{
						_linkedSanad.RemoveFromDets(tableRelationShipBusiness);
					}
					if (eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
					{
						_lastSum = TotalSum;
						PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5764);
			}
			finally
			{
				_modifiedDetListSemaphore?.Release();
			}
		}
	}

	public async Task AddRangeToListSanadAsync(List<Sanad_DetBusiness> sanaddet, EnSystem system, bool isClear = true, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				if (isClear)
				{
					connectedSanad = new SanadBusiness(ContextGuid, 0);
				}
				if (sanaddet == null)
				{
					return;
				}
				foreach (Sanad_DetBusiness item in sanaddet)
				{
					if (item != null)
					{
						if (item.AsGuid == Guid.Empty)
						{
							item.AsGuid = Guid;
						}
						if (item.AsType == Document.None)
						{
							item.AsType = FactorType;
						}
						await AddToListSanadAsync(item, system, token);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5784);
			}
		}
	}

	private async Task Det_OnSumChanged(object arg1, EventArgs arg2)
	{
		try
		{
			await _modifiedDetListSemaphore.WaitAsync();
			using (new JaegerMonitor("Det_OnSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
			{
				CancellationToken token = default(CancellationToken);
				if (eventManager?.OnSumChanged_static != null && TotalSum != _lastSum)
				{
					_lastSum = TotalSum;
					PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "Det_OnSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5798);
		}
		finally
		{
			_modifiedDetListSemaphore?.Release();
		}
	}

	public async Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return MoeinChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return TafsilChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		using (new JaegerMonitor("GetLstHesabChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 2;
			try
			{
				SanadBusiness newSanad = await ClsCache.GetCache(ContextGuid).OrderSanadGenerator.GenerateSanadAsync(this, _oldOrder, system, token, tranName);
				OrderBusiness orderBusiness = (OrderBusiness)(await GetOldItemAsync(system, token, tranName));
				return CalcChanges.ComputeHesabChange(await SanadBusiness.GetAsync(ContextGuid, orderBusiness?.SanadNo ?? 0, token, tranName), newSanad, isDeleteAction, autoSanadItemsForSave);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLstHesabChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5815);
				throw;
			}
		}
	}

	public async Task<SanadBusiness> GetSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		return await ClsCache.GetCache(ContextGuid).OrderSanadGenerator.GenerateSanadAsync(this, _oldOrder, system, token, tranName);
	}

	public static async Task<decimal> TotalSumOfSalesAsync(string contextGuid, Guid tafsilGuid, DateTime date, Document orderType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.TotalSumOfSales(tafsilGuid, date, orderType, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SendListAsync(IEnumerable<OrderReportBusiness> orderList, CancellationToken token)
	{
		using (new JaegerMonitor("SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				if (orderList == null || !orderList.Any())
				{
					return res;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				foreach (OrderReportBusiness item in orderList?.ToList())
				{
					tasks.Add(item.SendAsync(token));
					if (tasks.Count > 20)
					{
						await Task.WhenAny(tasks);
					}
				}
				await Task.WhenAll(tasks);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5841);
				res.AddReturnedValue(ex, "", "SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5842);
			}
			return res;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> OrderSortAsync(string contextGuid, Document orderType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				using (await _databaseSemaphore.GaurdAsync(true, "OrderBusiness/OrderSortAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5856), ReturnedState.None, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5856);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.OrderSortAsync(orderType, token, tranName), ReturnedState.None, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5858);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5862);
				res.AddReturnedValue(ex, "", "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5863);
			}
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5866), ReturnedState.None, "OrderSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5866);
			}
			return res;
		}
	}

	public static async Task<OrderBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Document factorType, ENSource source, Guid? tafsilGuid = null, string description = "", Guid? guid = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 5;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
				OrderBusiness ret = new OrderBusiness(contextGuid)
				{
					FactorType = factorType,
					ChangeStatus = EntityChangeSt.New,
					Source = source
				};
				if ((guid ?? Guid.Empty) != Guid.Empty)
				{
					ret.Guid = guid.Value;
				}
				else
				{
					ret.Guid = Guid.NewGuid();
				}
				if (system != EnSystem.Transfer && system != 0)
				{
					ret.UserAccGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
				}
				if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					await ret.SetTafsilGuid(tafsilGuid.Value, system, updateVisitorGuid: true, token, tranName);
				}
				if (!string.IsNullOrEmpty(description))
				{
					ret.Description = description.FixString();
				}
				if (ret.Source != ENSource.Accounting)
				{
					ret.SanadNo = 0;
					await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.Add2CacheAsync(ret, token, tranName);
				}
				if (factorType == Document.OrderSell && setting.Factors.Tasviye)
				{
					ret.TasvieDay = setting.Factors.TasviyeDay;
				}
				if ((factorType == Document.PishFactor || factorType == Document.OrderSell || factorType == Document.OrderBackSell) && (ret.B1TafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					await ret.SetB1TafsilGuidAsync(cashier?.VisitorGuid ?? Guid.Empty, system);
				}
				switch (factorType)
				{
				case Document.OrderSell:
					ret.KardexAffective = setting.Sells.KardexAffective;
					ret.SanadAffective = setting.Sells.SanadAffective;
					break;
				case Document.OrderBuy:
					ret.KardexAffective = setting.Buy.KardexAffective;
					ret.SanadAffective = setting.Buy.SanadAffective;
					break;
				case Document.OrderBackBuy:
					ret.KardexAffective = setting.BackBuy.KardexAffective;
					ret.SanadAffective = setting.BackBuy.SanadAffective;
					break;
				case Document.OrderBackSell:
					ret.KardexAffective = setting.BackSell.KardexAffective;
					ret.SanadAffective = setting.BackSell.SanadAffective;
					break;
				case Document.Zayeat:
					ret.KardexAffective = setting.Zayeat.KardexAffective;
					ret.SanadAffective = setting.Zayeat.SanadAffective;
					break;
				case Document.AmaniVorod:
					ret.KardexAffective = setting.AmaniVorod.KardexAffective;
					ret.SanadAffective = setting.AmaniVorod.SanadAffective;
					break;
				case Document.AmaniKhoroj:
					ret.KardexAffective = setting.AmaniKhoroj.KardexAffective;
					ret.SanadAffective = setting.AmaniKhoroj.SanadAffective;
					break;
				}
				await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, ret, system, token, tranName);
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 5940);
				throw;
			}
		}
	}

	private static async Task<IEnumerable<OrderReportBusiness>> ApplyAccessAsync(string contextGuid, EnSystem system, IEnumerable<OrderReportBusiness> orders, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			List<OrderReportBusiness> ret = new List<OrderReportBusiness>();
			try
			{
				if (system != EnSystem.UserInterface)
				{
					return orders;
				}
				CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
				if (currentCashier == null)
				{
					return orders;
				}
				UserAccBusiness user = await currentCashier.GetUserAsync(token);
				if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
				{
					orders = orders?.Where((OrderReportBusiness p) => (p?.TafsilGroupGuid ?? Guid.Empty) != Guid.Empty && user.UserAccess.TafsilGroupJustListGuid.Contains(p.TafsilGroupGuid));
				}
				if (!(orders?.Any() ?? false))
				{
					return null;
				}
				IEnumerable<int> illegalStatus = null;
				switch (orders.First().FactorType)
				{
				case Document.OrderSell:
					illegalStatus = user.UserAccess.Sell.SanadStatusAccess?.Where((OrderSellSanadStatusAccess q) => !q.Show)?.Select((OrderSellSanadStatusAccess q) => q.StatusId);
					break;
				case Document.OrderBuy:
					illegalStatus = user.UserAccess.Buy.SanadStatusAccess?.Where((OrderBuySanadStatusAccess q) => !q.Show)?.Select((OrderBuySanadStatusAccess q) => q.StatusId);
					break;
				case Document.OrderBackBuy:
					illegalStatus = user.UserAccess.BackBuy.SanadStatusAccess?.Where((OrderBackBuySanadStatusAccess q) => !q.Show)?.Select((OrderBackBuySanadStatusAccess q) => q.StatusId);
					break;
				case Document.OrderBackSell:
					illegalStatus = user.UserAccess.BackSell.SanadStatusAccess?.Where((OrderBackSellSanadStatusAccess q) => !q.Show)?.Select((OrderBackSellSanadStatusAccess q) => q.StatusId);
					break;
				case Document.PishFactor:
					illegalStatus = user.UserAccess.PishFacotr.SanadStatusAccess?.Where((OrderPreSellSanadStatusAccess q) => !q.Show)?.Select((OrderPreSellSanadStatusAccess q) => q.StatusId);
					break;
				case Document.OrderPreBuy:
					illegalStatus = user.UserAccess.PreBuy.SanadStatusAccess?.Where((OrderPreBuySanadStatusAccess q) => !q.Show)?.Select((OrderPreBuySanadStatusAccess q) => q.StatusId);
					break;
				case Document.Zayeat:
					illegalStatus = user.UserAccess.Zayeat.SanadStatusAccess?.Where((OrderZayeatSanadStatusAccess q) => !q.Show)?.Select((OrderZayeatSanadStatusAccess q) => q.StatusId);
					break;
				case Document.AmaniVorod:
					illegalStatus = user.UserAccess.AmaniVarede.SanadStatusAccess?.Where((OrderAmaniVorodSanadStatusAccess q) => !q.Show)?.Select((OrderAmaniVorodSanadStatusAccess q) => q.StatusId);
					break;
				case Document.AmaniKhoroj:
					illegalStatus = user.UserAccess.AmaniSadere.SanadStatusAccess?.Where((OrderAmaniKhorojSanadStatusAccess q) => !q.Show)?.Select((OrderAmaniKhorojSanadStatusAccess q) => q.StatusId);
					break;
				}
				if (illegalStatus?.Any() ?? false)
				{
					orders = orders?.Where((OrderReportBusiness q) => !illegalStatus.Contains(q.StatusId) || q.StatusId <= 0);
				}
				switch (orders.First().FactorType)
				{
				case Document.OrderSell:
					if (!user.UserAccess.Sell.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				case Document.OrderBuy:
					if (!user.UserAccess.Buy.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				case Document.OrderBackBuy:
					if (!user.UserAccess.BackBuy.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				case Document.OrderBackSell:
					if (!user.UserAccess.BackSell.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				case Document.Zayeat:
					if (!user.UserAccess.Zayeat.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				case Document.AmaniVorod:
					if (!user.UserAccess.AmaniVarede.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				case Document.AmaniKhoroj:
					if (!user.UserAccess.AmaniSadere.AccessToKardexAffective)
					{
						orders = orders?.Where((OrderReportBusiness q) => !q.KardexAffective);
					}
					break;
				}
				foreach (OrderReportBusiness order in orders)
				{
					if (order == null)
					{
						continue;
					}
					switch (order.FactorType)
					{
					case Document.OrderSell:
						if (user.UserAccess.Sell.Sell_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, user.UserAccess.Sell.Sell_ReportPreviuseDay, ret);
						}
						else if (user.UserAccess.Sell.Sell_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, user.UserAccess.Sell.Sell_ReportPreviuseDay, ret);
						}
						break;
					case Document.OrderBuy:
						if (user.UserAccess.Buy.Buy_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.Sell.Sell_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.OrderBackBuy:
						if (user.UserAccess.BackBuy.BackBuy_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.BackBuy.BackBuy_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.OrderBackSell:
						if (user.UserAccess.BackSell.BackSell_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.BackSell.BackSell_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.PishFactor:
						if (user.UserAccess.PishFacotr.PishFactor_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.PishFacotr.PishFactor_Delete_Self && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.OrderPreBuy:
						if (user.UserAccess.PreBuy.PreBuy_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.PreBuy.PreBuy_Delete_Self && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.Zayeat:
						if (user.UserAccess.Zayeat.Zayeat_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.Zayeat.Zayeat_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.AmaniVorod:
						if (user.UserAccess.AmaniVarede.AmaniVarede_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.AmaniVarede.AmaniVarede_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					case Document.AmaniKhoroj:
						if (user.UserAccess.AmaniSadere.AmaniSadere_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						else if (user.UserAccess.AmaniSadere.AmaniSadere_ReportSelf && order.UserAccGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(order, reportPreviuseDay: true, ret);
						}
						break;
					default:
						WebErrorLog.Instance.StartLog($"نوع تعریف نشده\r\n {order.FactorType}", "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6084);
						break;
					}
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6093);
				throw;
			}
		}
	}

	private static void CheckPreviewDaysAndAddIfHasAccess(OrderReportBusiness order, bool reportPreviuseDay, List<OrderReportBusiness> ret)
	{
		try
		{
			if (reportPreviuseDay)
			{
				ret.Add(order);
			}
			else if (order.Date.Date == DateTime.Now.Date)
			{
				ret.Add(order);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPreviewDaysAndAddIfHasAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6108);
			throw;
		}
	}

	public static Task<List<Guid>> GetNotTransferedAsync(string contextGuid, Document factorType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetNotTransferedAsync(factorType, token, tranName);
	}

	public static Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(string contextGuid, OperationTransfer filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.UpdateTransferDateAsync(filter, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> ChangePriceAsync(decimal percent, bool changeProducts, bool changeServices, decimal kasrRial, CancellationToken token = default(CancellationToken), EnSystem system = EnSystem.None)
	{
		using (new JaegerMonitor("ChangePriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				foreach (OrderDetailBusiness item in ListKala)
				{
					if (item == null)
					{
						continue;
					}
					ProductCompactBusiness productCompactBusiness = await item.GetPrdInfoAsync(system, token);
					if (productCompactBusiness != null && (productCompactBusiness.Type != 0 || changeProducts) && (productCompactBusiness.Type != EnProductType.Service || changeServices))
					{
						decimal num = Math.Round(item.Price * percent / 100m, DigitAfterPoint);
						decimal num2 = item.Price + num;
						if (kasrRial > 0m)
						{
							num2 -= num2 % kasrRial;
						}
						await item.SetPriceAsync(system, num2, token);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangePriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6137);
				res.AddReturnedValue(ex, "", "ChangePriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6138);
			}
			return res;
		}
	}

	public static Task<int> NextAttachmentNoAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.NextAttachmentNoAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateAttachmentNoAsync(string contextGuid, List<Guid> lstOrderGuid, int attachmentNo, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 1;
				try
				{
					if (autoCommit)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6153), ReturnedState.None, "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6153);
					}
					transactionHasBeenCreated = true;
					foreach (Guid item in lstOrderGuid)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.UpdateAttachmentNoAsync(item, attachmentNo, token, tranName), ReturnedState.None, "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6157);
						if (ret.HasError)
						{
							return ret;
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6163);
					ret.AddReturnedValue(ex, "", "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6164);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6169), ReturnedState.None, "UpdateAttachmentNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6169);
				}
			}
			return ret;
		}
	}

	private Task Det_OnOrdinalChanged(object sender, OrdinalChangedEventArgs e)
	{
		using (new JaegerMonitor("Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				CancellationToken token = default(CancellationToken);
				if (IsDisposed)
				{
					return Task.CompletedTask;
				}
				if (e.OldOrdinal == e.NewOrdinal || e.OldOrdinal < 0)
				{
					return Task.CompletedTask;
				}
				(ListKala?.FirstOrDefault((OrderDetailBusiness p) => p != null && p.Ordinal == e.NewOrdinal && p.Guid != e.DetailGuid))?.SetOrdinalChange(e.System, e.OldOrdinal, sender, token);
				if (this.OnChanged != null)
				{
					PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
				if (eventManager?.OnOrdinalChanged_static != null)
				{
					PacketParser.Utilities.RaiseEvent(eventManager?.OnOrdinalChanged_static, sender, e, token, 60, "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6190);
			}
			return Task.CompletedTask;
		}
	}

	public static async Task<List<OrderSummaryViewModel>> GetAllSummaryAsync(string contextGuid, Document factorType, DateTime? date1 = null, DateTime? date2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetAllSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			try
			{
				OrderFilter filter = new OrderFilter
				{
					FactorType = factorType,
					DateFrom = date1,
					DateTo = date2
				};
				IEnumerable<OrderReportBusiness> enumerable = await GetAllAsync(contextGuid, EnSystem.UserInterface, filter, token, tranName);
				if (enumerable == null || !enumerable.Any())
				{
					return null;
				}
				IOrderedEnumerable<OrderSummaryViewModel> obj = enumerable?.GroupBy((OrderReportBusiness q) => q.DateSh)?.Select((IGrouping<string, OrderReportBusiness> p) => new OrderSummaryViewModel
				{
					Count = p.Count(),
					DateSh = p.Key,
					Sum = p.Sum((OrderReportBusiness o) => o.Sum)
				})?.OrderBy((OrderSummaryViewModel q) => q.DateSh);
				OrderSummaryViewModel orderSummaryViewModel = null;
				List<OrderSummaryViewModel> list = obj?.ToList();
				foreach (OrderSummaryViewModel item in obj)
				{
					if (orderSummaryViewModel == null)
					{
						orderSummaryViewModel = item;
						continue;
					}
					DateTime dateTime = orderSummaryViewModel.DateSh.ShamsiToMiladi();
					TimeSpan timeSpan = item.DateSh.ShamsiToMiladi() - dateTime;
					if (timeSpan.Days > 1)
					{
						for (int i = 1; i <= timeSpan.Days; i = checked(i + 1))
						{
							list.Add(new OrderSummaryViewModel
							{
								Count = 0m,
								Sum = 0m,
								DateSh = dateTime.AddDays(i).MiladiToCalture()
							});
						}
						orderSummaryViewModel = item;
					}
				}
				return list?.OrderBy((OrderSummaryViewModel q) => q.DateSh)?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllSummaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6242);
				throw;
			}
		}
	}

	public static Task<OrderReportBusiness> GetLastOrderByTafsilGuidAsync(string contextGuid, Guid tafsilGuid, Document orderType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetLastOrderByTafsilGuidAsync(tafsilGuid, orderType, token, tranName);
	}

	public static async Task<decimal> GetVisitorSumPriceAsync(string contextGuid, DateTime? dateFrom, DateTime? dateTo, Document factorType, Guid visitorGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			OrderFilter filter = new OrderFilter
			{
				DateFrom = dateFrom,
				DateTo = dateTo,
				FactorType = factorType,
				VisitorTafsilGuid = visitorGuid
			};
			return await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetVisitorSumPriceAsync(filter, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVisitorSumPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6263);
			throw;
		}
	}

	public async Task RaiseOnSumChangedAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			await _modifiedDetListSemaphore.WaitAsync();
			using (new JaegerMonitor("RaiseOnSumChangedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 100))
			{
				if (eventManager?.OnSumChanged_static != null)
				{
					_lastSum = TotalSum;
					PacketParser.Utilities.RaiseEvent<EventArgs>(eventManager.OnSumChanged_static, this, EventArgs.Empty, token, 60, "RaiseOnSumChangedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseOnSumChangedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6279);
		}
		finally
		{
			_modifiedDetListSemaphore?.Release();
		}
	}

	public async Task<string> GenerateBarcodeAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		string barcode = "";
		try
		{
			string text = ((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName)) ?? throw new NullReferenceException("_currentCashier")).Settings.Barcode.FormatT_Sell;
			if (string.IsNullOrEmpty(text))
			{
				return barcode;
			}
			IEnumerable<char> enumerable = text.ToCharArray().ToList().Distinct();
			foreach (char item in enumerable)
			{
				text = await GenerateBarcodeAsync(item.ToString(), text, token);
			}
			barcode = text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6296);
		}
		return barcode;
	}

	private async Task<string> GenerateBarcodeAsync(string ch, string pattern, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GenerateBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			_ = 1;
			try
			{
				int bCount;
				string removedChar;
				checked
				{
					bCount = pattern.Split(new char[1] { Convert.ToChar(ch) }).Length - 1;
					removedChar = "";
					for (int i = 0; i < bCount; i++)
					{
						removedChar += ch;
					}
				}
				switch (ch)
				{
				case "K":
				{
					string text4 = ((short)FactorType).ToString();
					while (bCount > text4.Length)
					{
						text4 = "0" + text4;
					}
					pattern = pattern.Replace(removedChar, text4);
					break;
				}
				case "B":
				{
					string text3 = (await GetId2Async(EnSystem.UserInterface, "", token)).ToString();
					while (bCount > text3.Length)
					{
						text3 = "0" + text3;
					}
					pattern = pattern.Replace(removedChar, text3);
					break;
				}
				case "G":
				{
					string text2 = TotalSum.ToString().ParseToDecimal(0m, 3).ToString();
					while (bCount > text2.Length)
					{
						text2 = "0" + text2;
					}
					pattern = pattern.Replace(removedChar, text2);
					break;
				}
				case "L":
				{
					TafsilBusiness tafsilBusiness = await GetTafsilAsync(EnSystem.None, "", token);
					if (tafsilBusiness != null)
					{
						string text = tafsilBusiness.Code.ToString();
						while (bCount > text.Length)
						{
							text = "0" + text;
						}
						pattern = pattern.Replace(removedChar, text);
					}
					break;
				}
				case "Y":
					pattern = pattern.Replace("Y", "\r\n");
					break;
				case "N":
					pattern = pattern.Replace(removedChar, Guid.ToString());
					break;
				case "O":
					pattern = pattern.Replace(removedChar, TafsilGuid.ToString());
					break;
				}
				return pattern;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6342);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxResultAsync(string contextGuid, EnSystem system, TaxInvoiceResultEventArgs result, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			bool semaphoreHasBeenTaken = false;
			ReturnedSaveFuncInfo result2;
			try
			{
				_ = 8;
				try
				{
					if ((result?.OrderGuid ?? Guid.Empty) == Guid.Empty)
					{
						result2 = res.AddError("Result for Set is Null !!", "", "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6356);
						goto IL_0a66;
					}
					await _sendTaxResultSemaphore.WaitAsync();
					semaphoreHasBeenTaken = true;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6359), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6359);
					}
					transactionHasBeenCreated = true;
					if (res.HasError)
					{
						result2 = res;
						goto IL_0a66;
					}
					OrderBusiness order = await GetAsync(contextGuid, result.OrderGuid, Document.OrderSell, fromDatabase: true, token, tranName);
					if (order == null)
					{
						order = await GetAsync(contextGuid, result.OrderGuid, Document.OrderBackSell, fromDatabase: true, token, tranName);
						if (order == null)
						{
							result2 = res;
							goto IL_0a66;
						}
					}
					TaxOrderLogFilter filter = new TaxOrderLogFilter
					{
						OrderGuid = result.OrderGuid,
						TaxRefrencedNumber = result.TaxRefrenceNumber
					};
					if ((await TaxOrderLogBusiness.GetAllAsync(contextGuid, filter, token, tranName))?.SingleOrDefault((TaxOrderLogBusiness q) => (q?.NewState).GetValueOrDefault() == EnTaxResult.None) != null)
					{
						result2 = res;
						goto IL_0a66;
					}
					TaxOrderLogBusiness taxOrderLogBusiness = new TaxOrderLogBusiness(contextGuid)
					{
						OldState = EnTaxResult.Sent,
						ContextGuid = contextGuid,
						Guid = Guid.NewGuid(),
						OrderGuid = result.OrderGuid,
						TaxOrderId = result.InternaleSerial,
						TaxRefrenceNumber = result.TaxRefrenceNumber,
						TaxResultErrorCode = result.TaxResultErrorCode,
						TaxResultMessage = result.TaxResultMessage,
						TaxSendDate = result.TaxSendDate,
						TaxUid = result.Uid,
						InsertDate = DateTime.Now,
						NewState = null,
						Subject = result.Subject,
						BaseTaxOrderId = result.BaseTaxOrderId
					};
					TaxOrderLogBusiness taxOrderLogBusiness2 = taxOrderLogBusiness;
					taxOrderLogBusiness2.Ordinal = await TaxOrderLogBusiness.GetNextOrdinalAsync(contextGuid, result.OrderGuid, token, tranName);
					taxOrderLogBusiness.ServerStatus = ServerStatusForClient.None;
					taxOrderLogBusiness.InvoiceSeries = result.InvoiceSeries;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await taxOrderLogBusiness.SaveAsync(system, sendToServer: true, Priority.Low, await4Send: false, token, tranName), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6397);
					if (res.HasError)
					{
						result2 = res;
						goto IL_0a66;
					}
					short oldStatus = order.StatusId;
					await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, order, EnSystem.None, token, tranName, result.TaxResult);
					if (order.StatusId != oldStatus)
					{
						order.Modified = DateTime.Now;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.SaveAsync(order, 1, token, tranName), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6404);
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6409);
					res.AddReturnedValue(ex, "", "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6410);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6415), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6415);
				}
				if (semaphoreHasBeenTaken)
				{
					_sendTaxResultSemaphore.Release();
				}
				OrderBusiness orderBusiness = ClsCache.GetCache(contextGuid).BusinessAggregator.OrderBusiness;
				if (!res.HasError && orderBusiness != null && orderBusiness.OnTaxResultChanged_static != null)
				{
					PacketParser.Utilities.RaiseEvent(orderBusiness.OnTaxResultChanged_static, null, new EventArgs(), token, 60, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
			return res;
			IL_0a66:
			return result2;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetTaxResultAsync(string contextGuid, EnSystem system, List<TaxInvoiceTrackingEventArgs> list, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool semaphoreHasBeenTaken = false;
			bool transactionHasBeenCreated = false;
			ReturnedSaveFuncInfo result2;
			try
			{
				_ = 6;
				try
				{
					list = list?.Where((TaxInvoiceTrackingEventArgs q) => q.OrderGuid != Guid.Empty)?.ToList();
					if (!(list?.Any() ?? false))
					{
						result2 = res;
						goto IL_09e7;
					}
					await _setTaxResultSemaphore.WaitAsync();
					semaphoreHasBeenTaken = true;
					if (autoTran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6437), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6437);
					}
					transactionHasBeenCreated = true;
					if (res.HasError)
					{
						result2 = res;
						goto IL_09e7;
					}
					foreach (TaxInvoiceTrackingEventArgs result in list)
					{
						if ((result?.OrderGuid ?? Guid.Empty) == Guid.Empty)
						{
							continue;
						}
						TaxOrderLogFilter filter = new TaxOrderLogFilter
						{
							OrderGuid = result.OrderGuid,
							TaxRefrencedNumber = result.ReferenceNumber
						};
						TaxOrderLogBusiness oldLog = (await TaxOrderLogBusiness.GetAllAsync(contextGuid, filter, token, tranName))?.OrderByDescending((TaxOrderLogBusiness p) => p.InsertDate)?.FirstOrDefault((TaxOrderLogBusiness q) => q.OldState == EnTaxResult.Sent && !q.NewState.HasValue);
						if (oldLog == null)
						{
							continue;
						}
						oldLog.TaxDeliveryDate = DateTime.Now;
						if ((result?.Data?.Error?.Any()).GetValueOrDefault())
						{
							oldLog.NewState = EnTaxResult.Failed;
							oldLog.TaxResultErrorCode = string.Join("\r\n", result.Data.Error.Select((InvoiceTrackingResultModel q) => q.Code).ToList());
							oldLog.TaxResultMessage = string.Join("\r\n", result.Data.Error.Select((InvoiceTrackingResultModel q) => q.Message).ToList());
						}
						else
						{
							oldLog.NewState = EnTaxResult.Accepted;
							oldLog.TaxResultErrorCode = "";
							oldLog.TaxResultMessage = "";
						}
						oldLog.ServerStatus = ServerStatusForClient.None;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await oldLog.SaveAsync(system, sendToServer: true, Priority.Low, await4Send: false, token, tranName), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6467);
						if (res.HasError)
						{
							result2 = res;
						}
						else
						{
							OrderBusiness order = await GetAsync(contextGuid, result.OrderGuid, Document.OrderSell, fromDatabase: true, token, tranName);
							if (order != null)
							{
								await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, order, EnSystem.None, token, tranName, oldLog.NewState);
								order.Modified = DateTime.Now;
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.SaveAsync(order, 1, token, tranName), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6473);
								continue;
							}
							result2 = res;
						}
						goto IL_09e7;
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6478);
					res.AddReturnedValue(ex, "", "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6479);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6484), ReturnedState.None, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6484);
				}
				if (semaphoreHasBeenTaken)
				{
					_setTaxResultSemaphore.Release();
				}
				OrderBusiness orderBusiness = ClsCache.GetCache(contextGuid).BusinessAggregator.OrderBusiness;
				if (!res.HasError && orderBusiness != null && orderBusiness.OnTaxResultChanged_static != null)
				{
					PacketParser.Utilities.RaiseEvent(orderBusiness.OnTaxResultChanged_static, null, new EventArgs(), token, 60, "SetTaxResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs");
				}
			}
			return res;
			IL_09e7:
			return result2;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(string contextGuid, bool value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6501), ReturnedState.None, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6501);
					}
					transactionHasBeenCreated = true;
					if (res.HasError)
					{
						return res;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.UpdateSanadAffectiveAsync(value, token, tranName), ReturnedState.None, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6504);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6508);
					res.AddReturnedValue(ex, "", "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6509);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6514), ReturnedState.None, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6514);
				}
			}
			return res;
		}
	}

	public static Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(string contextGuid, Guid tafsilGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.UpdateCustomerFullNameAsync(tafsilGuid, name, token, tranName);
	}

	public static Task<IEnumerable<OrderDetailSerialModel>> GetAllDetailSerialAsync(string contextGuid, string serial, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllDetailSerialAsync(serial, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(string contextGuid, Guid? guid, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
			{
				return ret;
			}
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "OrderBusiness/UpdateDateMoaserAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6534), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6534);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.UpdateDateMoaserAsync(guid, token, tranName), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6536);
							goto end_IL_0151;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6533);
						goto end_IL_00b5;
						end_IL_0151:;
					}
					goto end_IL_00a4;
					end_IL_00b5:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6540);
					ret.AddReturnedValue(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6541);
					goto end_IL_00a4;
				}
				return result;
				end_IL_00a4:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6546), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6546);
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<Guid>>> GetRelatedCouponsAsync(string couponCode = "", CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<List<Guid>> res = new ReturnedSaveFuncInfoWithValue<List<Guid>>();
		try
		{
			if (ListKala?.Any((OrderDetailBusiness q) => (q?.DicountCouponDetailGuid ?? Guid.Empty) != Guid.Empty) ?? false)
			{
				return res.AddError("در فاکتور جاری سابقا کوپن تخفیف اعمال شده است.".GetTranslate() + " \r\n" + " لطفا ابتدا تخفیفات اعمال شده را حذف و مجدد اقدام به اعمال کوپن نمایید".GetTranslate(), "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6556);
			}
			if (Discount > 0m || DiscountRows > 0m)
			{
				return res.AddError("به فاکتور دارای تخفیف، کوپن تخفیف تعلق نمی گیرد.".GetTranslate() + " \r\n" + " برای استفاده از کوپن تخفیف، لطفا ابتدا تخفیفات سطری و یا تخفیف نهایی فاکتور را حذف نمایید".GetTranslate(), "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6557);
			}
			CouponFilter filter = new CouponFilter
			{
				DateFrom = GetDate(),
				DateTo = GetDate(),
				Type = null,
				CouponCode = couponCode,
				Status = true
			};
			IEnumerable<CouponDiscountReportBusiness> enumerable = (await CouponDiscountBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.Where((CouponDiscountReportBusiness q) => q.Mode == EnCouponMode.Coupon || q.Mode == EnCouponMode.Gift);
			if (enumerable == null || !enumerable.Any())
			{
				return res.AddError("در تاریخ جاری، هیچ کوپن فعالی در سیستم وجود ندارد".GetTranslate(), "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6568);
			}
			ReturnedSaveFuncInfoWithValue<List<Guid>> returnedSaveFuncInfoWithValue = await GetRelatedCouponsAsync(enumerable, null, token, tranName);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6571);
			if (res.HasError)
			{
				return res;
			}
			res.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6577);
			res.AddReturnedValue(ex, "", "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6578);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfoWithValue<List<Guid>>> GetRelatedCouponsAsync(IEnumerable<CouponDiscountReportBusiness> list, Guid? specificCouponGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<List<Guid>> res = new ReturnedSaveFuncInfoWithValue<List<Guid>>();
		try
		{
			List<OrderDetailCouponCalculationBusiness> list2 = await OrderDetMapper.GetInstance(null).MapListAsync(ContextGuid, ListKala, new List<OrderDetailCouponCalculationBusiness>(), tranName, token);
			if (list2 == null || !list2.Any())
			{
				return res;
			}
			ReturnedSaveFuncInfoWithValue<int> saveRes = await OrderDetailCouponCalculationBusiness.SaveAsync(ContextGuid, list2, token, tranName);
			res.AddReturnedValue(saveRes, ReturnedState.None, "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6590);
			if (res.HasError || saveRes.value <= 0)
			{
				return res;
			}
			SanadPrintBusiness sanadPrintBusiness = await SanadPrintBusiness.GetAsync(ContextGuid, Guid, FactorType, token, tranName);
			IEnumerable<Guid> enumerable = await CouponDiscountBusiness.GetAllOrderRelationAsync(ContextGuid, TafsilGuid, GetDate(), TotalSum, saveRes.value, specificCouponGuid, sanadPrintBusiness?.MobileLong ?? 0, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res.AddError("فاکتور جاری با هیچکدام از کوپن های تخفیف موجود در سیستم، تطابق ندارد".GetTranslate(), "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6594);
			}
			if (list?.Any() ?? false)
			{
				res.value = enumerable?.Where((Guid q) => list.Select((CouponDiscountReportBusiness q) => q.Guid).Contains(q)).ToList();
			}
			else
			{
				res.value = enumerable?.ToList();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6601);
			res.AddReturnedValue(ex, "", "GetRelatedCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6602);
		}
		return res;
	}

	public async Task ApplyDiscountAsync(List<OrderCouponRelationReportBusiness> couponList, IApplyGift uiManager)
	{
		_ = 7;
		try
		{
			if (!(couponList?.Any() ?? false))
			{
				return;
			}
			foreach (OrderCouponRelationReportBusiness coupon in couponList)
			{
				if (!(coupon.DetailList?.Any() ?? false))
				{
					continue;
				}
				foreach (OrderCouponRelationDeltailReportBusiness item2 in coupon.DetailList)
				{
					OrderDetailBusiness orderDetailBusiness = ListKala.SingleOrDefault((OrderDetailBusiness q) => q.Guid == item2.OrderDetailGuid);
					if (orderDetailBusiness != null)
					{
						orderDetailBusiness.DicountCouponDetailGuid = item2.CouponDetailGuid;
						if (item2.Discount > 0m)
						{
							await orderDetailBusiness.SetDiscountAsync(EnSystem.UserInterface, item2.Discount);
						}
					}
				}
				if (coupon.CouponOrderDiscount > 0m)
				{
					Discount += coupon.CouponOrderDiscount;
				}
				if (!((coupon?.GiftGuid ?? Guid.Empty) != Guid.Empty))
				{
					continue;
				}
				GiftBusiness gift = await GiftBusiness.GetAsync(ContextGuid, coupon.GiftGuid.Value);
				if (!(gift?.DetailList?.Any()).GetValueOrDefault())
				{
					continue;
				}
				List<GiftDetailBusiness> detList = gift.DetailList?.ToList();
				if (gift.Type == EnTypeChose.SelectGiftByCustomer && uiManager != null)
				{
					detList = (List<GiftDetailBusiness>)(await uiManager.SelectGiftAsync(gift.Guid));
				}
				if (!(detList?.Any() ?? false))
				{
					continue;
				}
				foreach (GiftDetailBusiness item3 in detList)
				{
					if (!((item3?.ProductGuid ?? Guid.Empty) == Guid.Empty))
					{
						OrderDetailBusiness det = await AddDetAsync(EnSystem.UserInterface, isGood: true, isLoadOriginalPrice: true, awaitOnSemaphore: true, item3.ProductGuid);
						await det.SetVGuidAsync(EnSystem.UserInterface, null);
						await det.SetV2QtAsync(EnSystem.UserInterface, null);
						await det.SetQuantityAsync(EnSystem.UserInterface, (gift.Type == EnTypeChose.SelectGiftByCustomer) ? item3.UserEnteredCount : item3.Count);
						await det.SetDiscountPercentAsync(EnSystem.UserInterface, 100m);
					}
				}
				foreach (OrderCouponRelationDeltailReportBusiness item in coupon.DetailList)
				{
					OrderDetailBusiness orderDetailBusiness2 = ListKala.SingleOrDefault((OrderDetailBusiness q) => q.Guid == item.OrderDetailGuid);
					if (orderDetailBusiness2 != null)
					{
						orderDetailBusiness2.GiftDetailList = detList;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyDiscountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6656);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(string contextGuid, Document factorType, bool kardexAffective, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
			{
				return ret;
			}
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "OrderBusiness/UpdateKardexAffectiveAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6672), ReturnedState.None, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6672);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.UpdateKardexAffectiveAsync(factorType, kardexAffective, token, tranName), ReturnedState.None, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6674);
							goto end_IL_014c;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6671);
						goto end_IL_00b5;
						end_IL_014c:;
					}
					goto end_IL_00a4;
					end_IL_00b5:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6678);
					ret.AddReturnedValue(ex, "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6679);
					goto end_IL_00a4;
				}
				return result;
				end_IL_00a4:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6684), ReturnedState.None, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6684);
				}
			}
			return ret;
		}
	}

	public static Task<IEnumerable<OrderTtmsReport>> GetOrderTtmsReportAsync(string contextGuid, List<Guid> orderGuidList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetOrderTtmsReportAsync(orderGuidList, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> TrackingTaxOrderAsync(string contextGuid, IEnumerable<OrderReportBusiness> _list)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(_list?.Any() ?? false))
			{
				res.AddError("هیچ فاکتوری انتخاب نشده است".GetTranslate(), "", "TrackingTaxOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6695);
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			if (string.IsNullOrEmpty(setting?.TaxService?.ClientId))
			{
				res.AddError("شناسه یکتا حافظه مالیاتی نامعتبر می باشد".GetTranslate(), "", "TrackingTaxOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6697);
			}
			if (!ClsCache.GetCache(contextGuid).BufferEnable || (ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
			{
				res.AddError("ارتباط با سرور اینترنتی نوین پرداز برقرار نمی باشد.".GetTranslate() + " \r\n" + " لطفا اتصال اینترنت خود و یا تنظیمات همگام سازی با سرور را بررسی نمایید".GetTranslate(), "", "TrackingTaxOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6699);
			}
			if (res.HasError)
			{
				return res;
			}
			List<InvoiceTrackingEventArgs> eventArgsList = new List<InvoiceTrackingEventArgs>();
			foreach (OrderReportBusiness item in _list)
			{
				TaxOrderLogBusiness taxOrderLogBusiness = (await TaxOrderLogBusiness.GetAllAsync(contextGuid, new TaxOrderLogFilter
				{
					OrderGuid = item.Guid
				}))?.OrderByDescending((TaxOrderLogBusiness q) => q.InsertDate)?.FirstOrDefault();
				if ((taxOrderLogBusiness?.NewState).GetValueOrDefault() != EnTaxResult.Accepted)
				{
					eventArgsList.Add(new InvoiceTrackingEventArgs
					{
						RefrenceId = (taxOrderLogBusiness?.TaxRefrenceNumber ?? item.TaxRefrenceNumber),
						Uid = (taxOrderLogBusiness?.TaxUid ?? item.TaxUid),
						OrderGuid = item.Guid
					});
				}
			}
			Packet item2 = WebOrder.GenerateTaxTrackingRequest(new Tuple<string, List<InvoiceTrackingEventArgs>>(setting.TaxService.ClientId, eventArgsList));
			await ClsCache.GetCache(contextGuid).SendData(new List<Packet> { item2 }, Priority.High);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TrackingTaxOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6722);
			res.AddReturnedValue(ex, "", "TrackingTaxOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6723);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SendOrdersToTaxServiceAsync(string contextGuid, IEnumerable<OrderReportBusiness> _list, EnTaxInvoiceSubject subject, string baseTaxOrderId = "", DateTime? taxBillingDate = null)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			if (!(_list?.Any() ?? false))
			{
				res.AddError("هیچ فاکتوری انتخاب نشده است".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6734);
			}
			if (string.IsNullOrEmpty(setting?.TaxService?.ClientId))
			{
				res.AddError("شناسه یکتا حافظه مالیاتی نامعتبر می باشد".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6735);
			}
			if (!ClsCache.GetCache(contextGuid).BufferEnable || (ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
			{
				res.AddError("ارتباط با سرور اینترنتی نوین پرداز برقرار نمی باشد.".GetTranslate() + "\r\n" + " لطفا اتصال اینترنت خود و یا تنظیمات همگام سازی با سرور را بررسی نمایید".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6737);
			}
			if (res.HasError)
			{
				return res;
			}
			foreach (OrderReportBusiness item in _list)
			{
				OrderBusiness order = await GetAsync(contextGuid, item.Guid, item.FactorType, fromDatabase: true);
				if (order == null)
				{
					res.AddError("فاکتور شماره".GetTranslate() + $" {item.Id2}" + "یافت نشد".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6745);
					continue;
				}
				if (!string.IsNullOrEmpty(item.TaxRefrenceNumber))
				{
					TaxOrderLogBusiness taxOrderLogBusiness = (await TaxOrderLogBusiness.GetAllAsync(contextGuid, new TaxOrderLogFilter
					{
						OrderGuid = item.Guid,
						TaxRefrencedNumber = item.TaxRefrenceNumber
					}))?.OrderByDescending((TaxOrderLogBusiness q) => q.InsertDate)?.FirstOrDefault();
					if (taxOrderLogBusiness != null && taxOrderLogBusiness.Subject != EnTaxInvoiceSubject.Delete)
					{
						switch (item.TaxResult)
						{
						case EnTaxResult.Accepted:
							if (subject == EnTaxInvoiceSubject.Sell && (taxOrderLogBusiness?.NewState).GetValueOrDefault() == EnTaxResult.Accepted)
							{
								await res.AddInformationAsync("فاکتور شماره".GetTranslate() + $" {item.Id2}".GetTranslate() + " سابقا به سامانه مودیان ارسال شده و توسط سامانه پذیرفته شده است. لذا از ارسال مجدد جلوگیری می شود".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6760);
								continue;
							}
							break;
						case EnTaxResult.Sent:
							await res.AddWarningAsync("فاکتور شماره".GetTranslate() + $" {item.Id2}" + " با پیگیری".GetTranslate() + " (" + item.TaxRefrenceNumber + ")" + "در تاریخ".GetTranslate() + $"{item.TaxSendDate.Value.Hour}:{item.TaxSendDate.Value.Minute} {item.TaxSendDate.Value.MiladiToCalture()} ارسال شده و در انتظار پاسخ از سمت سامانه می باشد".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6765);
							continue;
						default:
							throw new NotImplementedException();
						case EnTaxResult.Failed:
							break;
						}
					}
				}
				WebOrder webOrder = ((subject != EnTaxInvoiceSubject.BackSell) ? (await OrderMapper.Instance.MapAsync(contextGuid, order, new WebOrder())) : (await GenerateBackSellForTaxServiceAsync(contextGuid, order, await OrderDetailBusiness.GetMasterGuidAsync(contextGuid, (order.ListKala.Select((OrderDetailBusiness q) => q.ReferenceGuid)?.Where((Guid? q) => q.HasValue && q != Guid.Empty))?.FirstOrDefault() ?? Guid.Empty))));
				webOrder.TaxSubject = subject;
				webOrder.TaxBillingDate = taxBillingDate;
				if (subject == EnTaxInvoiceSubject.Delete || subject == EnTaxInvoiceSubject.Edit || subject == EnTaxInvoiceSubject.BackSell)
				{
					if (string.IsNullOrEmpty(baseTaxOrderId))
					{
						IEnumerable<TaxOrderLogBusiness> enumerable = null;
						switch (subject)
						{
						case EnTaxInvoiceSubject.Edit:
						case EnTaxInvoiceSubject.Delete:
							enumerable = await TaxOrderLogBusiness.GetAllAsync(contextGuid, new TaxOrderLogFilter
							{
								OrderGuid = order.Guid
							});
							break;
						case EnTaxInvoiceSubject.BackSell:
						{
							IEnumerable<Guid?> enumerable2 = order.ListKala.Select((OrderDetailBusiness q) => q.ReferenceGuid)?.Where((Guid? q) => q.HasValue && q != Guid.Empty);
							if (enumerable2 == null || !enumerable2.Any())
							{
								return res.AddError("هیچ فاکتور فروشی به فاکتور مرجوعی جاری لینک نشده است".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6797);
							}
							Guid guid = await OrderDetailBusiness.GetMasterGuidAsync(contextGuid, enumerable2.First().Value);
							if (guid == Guid.Empty)
							{
								return res.AddError("فاکتور فروش مرجع یافت نشد".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6799);
							}
							enumerable = await TaxOrderLogBusiness.GetAllAsync(contextGuid, new TaxOrderLogFilter
							{
								OrderGuid = guid
							});
							break;
						}
						}
						TaxOrderLogBusiness taxOrderLogBusiness2 = enumerable?.Where((TaxOrderLogBusiness q) => q.NewState == EnTaxResult.Accepted && q.Subject != EnTaxInvoiceSubject.Delete)?.OrderByDescending((TaxOrderLogBusiness q) => q.InsertDate)?.FirstOrDefault();
						if (taxOrderLogBusiness2 == null)
						{
							taxOrderLogBusiness2 = enumerable?.OrderBy((TaxOrderLogBusiness q) => q.InsertDate)?.FirstOrDefault();
						}
						if (string.IsNullOrEmpty(taxOrderLogBusiness2?.TaxOrderId))
						{
							await res.AddErrorAsync("شماره صورتحساب مرجع برای فاکتور شماره ".GetTranslate() + " " + $"{item.Id2}" + " " + " یافت نشد".GetTranslate(), "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6806);
							continue;
						}
						webOrder.BaseTaxId = taxOrderLogBusiness2.TaxOrderId;
					}
					else
					{
						webOrder.BaseTaxId = baseTaxOrderId;
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await webOrder.CheckTaxValidationAsync(), ReturnedState.None, "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6814);
				if (res.HasError)
				{
					return res;
				}
				Packet item2 = WebOrder.GenerateSendToTaxServiceRequest(new Tuple<string, WebOrder>(setting.TaxService.ClientId, webOrder));
				await ClsCache.GetCache(contextGuid).SendData(new List<Packet>(1) { item2 }, Priority.High);
				if (subject == EnTaxInvoiceSubject.BackSell)
				{
					OrderBusiness orderBusiness = order;
					if (orderBusiness.MetaData == null)
					{
						orderBusiness.MetaData = new OrderMetaData();
					}
					order.MetaData.BackSellForTaxService = webOrder;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await order.SaveAsync(EnSystem.None, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10), ReturnedState.None, "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6823);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6829);
			res.AddReturnedValue(ex, "", "SendOrdersToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6830);
		}
		return res;
	}

	private static async Task<WebOrder> GenerateBackSellForTaxServiceAsync(string contextGuid, OrderBusiness orderBackSell, Guid orderSellGuid)
	{
		_ = 6;
		try
		{
			if (orderBackSell == null)
			{
				throw new ArgumentException("فاکتور مرجوعی یافت نشد".GetTranslate());
			}
			if (orderSellGuid == Guid.Empty)
			{
				throw new ArgumentException("شناسه فاکتور فروش معتبر نمی باشد".GetTranslate());
			}
			OrderBusiness orderSell = (await GetAsync(contextGuid, orderSellGuid, Document.OrderSell, fromDatabase: true)) ?? throw new ArgumentException("فاکتور فروش یافت نشد".GetTranslate());
			OrderBusiness newOrder = await OrderMapper.Instance.MapAsync(contextGuid, orderSell, new OrderBusiness(contextGuid));
			await newOrder.SetDateAsync(orderBackSell.GetDate(), EnSystem.None);
			foreach (OrderDetailBusiness item in newOrder.ListKala.ToList())
			{
				IEnumerable<OrderDetailBusiness> enumerable = orderBackSell.ListKala?.Where((OrderDetailBusiness q) => q.ProductGuid == item.ProductGuid);
				OrderDetailBusiness orderDetailBusiness = (((enumerable?.Count() ?? 0) <= 1) ? enumerable?.SingleOrDefault() : orderBackSell.ListKala?.SingleOrDefault((OrderDetailBusiness q) => q.ReferenceGuid == item.Guid));
				decimal num = item.Quantity - (orderDetailBusiness?.Quantity ?? 0m);
				if (num > 0m)
				{
					await item.SetQuantityAsync(EnSystem.UserInterface, num);
				}
				else
				{
					item.Dispose(disposing: true);
				}
			}
			WebOrder mappedOrder = await OrderMapper.Instance.MapAsync(contextGuid, newOrder, new WebOrder());
			int num2;
			if (orderSell == null)
			{
				num2 = 0;
			}
			else
			{
				OrderMetaData metaData = orderSell.MetaData;
				num2 = ((metaData != null && (metaData.Tax?.TaxTasvieMethod.HasValue ?? false)) ? 1 : 0);
			}
			if (num2 != 0)
			{
				mappedOrder.TaxTasvieMethod = orderSell.MetaData.Tax.TaxTasvieMethod;
			}
			else if ((orderSell.MetaData?.Tax?.TaxBillType).GetValueOrDefault(EnTaxBillType.None) == EnTaxBillType.TypeOne)
			{
				decimal receptionSum = await orderSell.ReceptionSumAsync();
				TableRelationShipBusiness tableRelationShipBusiness = await orderSell.GetLinkedSanadAsync(EnSystem.UserInterface);
				if ((tableRelationShipBusiness?.ChildList?.Any()).GetValueOrDefault())
				{
					receptionSum += tableRelationShipBusiness.ChildList.Sum((TableRelationShipBusiness q) => q.Price);
				}
				if (receptionSum <= 0m)
				{
					mappedOrder.TaxTasvieMethod = EnTaxTasvieMethod.Etebar;
				}
				else if (receptionSum < orderSell.TotalSum)
				{
					mappedOrder.TaxTasvieMethod = EnTaxTasvieMethod.Cash_Etebar;
				}
				else
				{
					mappedOrder.TaxTasvieMethod = EnTaxTasvieMethod.Cash;
				}
			}
			else
			{
				mappedOrder.TaxTasvieMethod = EnTaxTasvieMethod.Cash;
			}
			mappedOrder.Guid = orderBackSell.Guid;
			return mappedOrder;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateBackSellForTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6879);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendDeleteToTaxServiceAsync(string contextGuid, OrderReportBusiness _order, string taxOrderId)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			if (_order == null)
			{
				res.AddError("هیچ فاکتوری انتخاب نشده است".GetTranslate(), "", "SendDeleteToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6890);
			}
			if (string.IsNullOrEmpty(setting?.TaxService?.ClientId))
			{
				res.AddError("شناسه یکتا حافظه مالیاتی نامعتبر می باشد".GetTranslate(), "", "SendDeleteToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6891);
			}
			if (!ClsCache.GetCache(contextGuid).BufferEnable || (ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.Unknown) != EnLoginResult.Success)
			{
				res.AddError("ارتباط با سرور اینترنتی نوین پرداز برقرار نمی باشد.".GetTranslate() + "\r\n" + " لطفا اتصال اینترنت خود و یا تنظیمات همگام سازی با سرور را بررسی نمایید".GetTranslate(), "", "SendDeleteToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6893);
			}
			if (res.HasError)
			{
				return res;
			}
			OrderBusiness source = await GetAsync(contextGuid, _order.Guid, _order.FactorType, fromDatabase: true);
			WebOrder webOrder = await OrderMapper.Instance.MapAsync(contextGuid, source, new WebOrder());
			webOrder.TaxSubject = EnTaxInvoiceSubject.Delete;
			webOrder.BaseTaxId = taxOrderId;
			Packet item = WebOrder.GenerateSendToTaxServiceRequest(new Tuple<string, WebOrder>(setting.TaxService.ClientId, webOrder));
			await ClsCache.GetCache(contextGuid).SendData(new List<Packet> { item }, Priority.High);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendDeleteToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6905);
			res.AddReturnedValue(ex, "", "SendDeleteToTaxServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6906);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateSanadAffectiveAsync(string contextGuid, List<Tuple<Guid, Document>> orderGuids, EnSystem system)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(orderGuids?.Any() ?? false))
			{
				return res;
			}
			foreach (Tuple<Guid, Document> orderGuid in orderGuids)
			{
				if (!(orderGuid.Item1 == Guid.Empty))
				{
					OrderBusiness orderBusiness = await GetAsync(contextGuid, orderGuid.Item1, orderGuid.Item2, fromDatabase: true);
					if (orderBusiness != null && !orderBusiness.SanadAffective)
					{
						orderBusiness.SanadAffective = true;
						orderBusiness.SanadNo = 0;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await orderBusiness.SaveAsync(system, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10), ReturnedState.None, "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6923);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6928);
			res.AddReturnedValue(ex, "", "UpdateSanadAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6929);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateKardexAffectiveAsync(string contextGuid, List<Tuple<Guid, Document>> orderGuids, EnSystem system)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(orderGuids?.Any() ?? false))
			{
				return res;
			}
			foreach (Tuple<Guid, Document> orderGuid in orderGuids)
			{
				if (orderGuid.Item1 == Guid.Empty)
				{
					continue;
				}
				OrderBusiness orderBusiness = await GetAsync(contextGuid, orderGuid.Item1, orderGuid.Item2, fromDatabase: true);
				if (orderBusiness != null && !orderBusiness.KardexAffective)
				{
					orderBusiness.KardexAffective = true;
					if (!orderBusiness.SanadAffective)
					{
						orderBusiness.SanadNo = 0;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await orderBusiness.SaveAsync(system, awaitOnSemaphore: true, sendToServer: false, isTemp: false, 10), ReturnedState.None, "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6946);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6951);
			res.AddReturnedValue(ex, "", "UpdateKardexAffectiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6952);
		}
		return res;
	}

	private async Task<string> GenerateLinkAync(EnSystem system, string tranName = "")
	{
		try
		{
			UserInformationBusiness userInfo = await UserInformationBusiness.GetAsync(ContextGuid, tranName);
			if ((userInfo?.DepId ?? 0) <= 0)
			{
				return "";
			}
			NPSMS_WebServiceSettings nPSMS_WebServiceSettings = await LocalSettingBusiness.GetWebServiceSettingsAsync(ContextGuid);
			if (string.IsNullOrEmpty(nPSMS_WebServiceSettings?.NPACCUserName) || string.IsNullOrEmpty(nPSMS_WebServiceSettings?.NPACCPassword))
			{
				return "";
			}
			Task.Run(() => SaveTempAsync(system));
			return $"{userInfo.PortalDomain}/Order/{userInfo.DepId}/{Guid}";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateLinkAync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6969);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> AddOpeinInvoiceAsync(PlacementReportsBusiness placement, int peopleCount)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (placement == null)
			{
				return res;
			}
			if (placement.State != 0)
			{
				await res.AddWarningAsync(placement.Title + " " + " در وضعیت".GetTranslate() + "  " + placement.StateForDisplay + " " + " می باشد و مجددا قابل استفاده نیست".GetTranslate(), "", "AddOpeinInvoiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 6980);
			}
			if (res.HasError)
			{
				return res;
			}
			OpenInvoice = await OpenInvoiceBusiness.GetLastRecordAsync(ContextGuid, placement.Guid, null);
			if (OpenInvoice == null)
			{
				OpenInvoice = await OpenInvoiceBusiness.GetLastRecordAsync(ContextGuid, null, Guid);
				OpenInvoice?.ClearPlacementRelation();
			}
			if (OpenInvoice == null)
			{
				OpenInvoice = await OpenInvoiceBusiness.GenerateNewAsync(ContextGuid, GetDate(), TafsilGuid);
				OpenInvoice.DateM = GetDate();
				OpenInvoice.TafsilGuid = TafsilGuid;
				OpenInvoice.PeopleCount = peopleCount;
			}
			await OpenInvoice.AddOrderRelationAsync(Guid);
			await OpenInvoice.AddPlacementRelationAsync(placement.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddOpeinInvoiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7002);
			res.AddReturnedValue(ex, "", "AddOpeinInvoiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7003);
		}
		return res;
	}

	public static async Task<List<OrderDetailAggregatRecord>> GetAllOrderDetailAggregatAsync(string contextGuid, EnSystem system, List<OrderDetailAggregatRecord> existData, CancellationToken token)
	{
		_ = 3;
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashierBusiness == null)
			{
				return null;
			}
			AccessLevel accessLevel = await cashierBusiness.GetUserAccessAsync(token);
			if (accessLevel == null)
			{
				return null;
			}
			int insertFromAccounting = accessLevel.Sell.SanadStatusDefaultAccess.InsertFromAccounting;
			if (insertFromAccounting <= 0)
			{
				return null;
			}
			OrderFilter filter = new OrderFilter
			{
				FactorType = Document.OrderSell,
				StatusId = insertFromAccounting
			};
			IEnumerable<OrderReportBusiness> enumerable = await GetAllAsync(contextGuid, system, filter, token);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<OrderDetailAggregatRecord> list2 = new List<OrderDetailAggregatRecord>();
			foreach (OrderReportBusiness item2 in enumerable)
			{
				if (item2 == null)
				{
					continue;
				}
				OrderBusiness orderBusiness = await GetAsync(contextGuid, item2.Guid, item2.FactorType, fromDatabase: true, token);
				if (orderBusiness != null)
				{
					list2.AddRange(orderBusiness.ListKala.Select((OrderDetailBusiness q) => new OrderDetailAggregatRecord
					{
						ProductName = q.ProductName,
						ProductUnit = q.ProductVahed,
						Quantity = q.Quantity,
						ProductGuid = q.ProductGuid
					}));
				}
			}
			if (!(list2?.Any() ?? false))
			{
				return null;
			}
			List<IGrouping<Guid, OrderDetailAggregatRecord>> source = (from q in list2
				group q by q.ProductGuid).ToList();
			list2 = source.Select((IGrouping<Guid, OrderDetailAggregatRecord> q) => new OrderDetailAggregatRecord
			{
				ProductGuid = q.Key,
				ProductName = q.FirstOrDefault().ProductName,
				ProductUnit = q.FirstOrDefault().ProductUnit,
				Quantity = q.Sum((OrderDetailAggregatRecord w) => w.Quantity)
			}).ToList();
			if (existData?.Any() ?? false)
			{
				foreach (OrderDetailAggregatRecord item in existData)
				{
					OrderDetailAggregatRecord orderDetailAggregatRecord = list2.FirstOrDefault((OrderDetailAggregatRecord q) => q.ProductGuid == item.ProductGuid);
					if (orderDetailAggregatRecord == null)
					{
						list2.Remove(orderDetailAggregatRecord);
					}
				}
			}
			return list2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllOrderDetailAggregatAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7068);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(string contextGuid, Guid orderGuid, int statusId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "OrderBusiness/ChangeStatusIdAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7083), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7083);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.ChangeStatusIdAsync(orderGuid, statusId, token, tranName), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7085);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7082);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7089);
					res.AddReturnedValue(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7090);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7095), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7095);
				}
			}
			return res;
		}
	}

	public async Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			int num = await GetId2Async(system, tranName, token);
			if (!(ListKala?.Any() ?? false))
			{
				return "بابت فروش".GetTranslate() + $" ({num})";
			}
			if (ListKala.Count == 1)
			{
				return "بابت فروش".GetTranslate() + " " + ListKala.Single().QuantityUnitForDisplay + " " + ListKala.Single().ProductName + " - فاکتور شماره" + $" ({num})";
			}
			OrderDetailBusiness prd = ListKala.FirstOrDefault((OrderDetailBusiness q) => q.Ordinal == 1);
			int? num2 = ListKala.Where((OrderDetailBusiness q) => q.Guid != prd.Guid && !IsDisposed && q.ProductGuid != Guid.Empty)?.Count();
			return "بابت فروش".GetTranslate() + " " + prd.QuantityUnitForDisplay + " " + prd.ProductName + " " + " و ".GetTranslate() + " " + $"{num2}" + " " + " قلم دیگر - فاکتور شماره ".GetTranslate() + " " + $"({num})";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetShortDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7117);
			throw;
		}
	}

	public async Task<IEnumerable<NoteBusiness>> GetAllNotesAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			NoteFilter filter = new NoteFilter
			{
				AsGuid = Guid,
				AsType = FactorType
			};
			return await NoteBusiness.GetAllAsync(ContextGuid, system, filter, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllNotesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7135);
			throw;
		}
	}

	public EnLog GetLogType()
	{
		return GetLogType(FactorType);
	}

	public static EnLog GetLogType(Document factorType)
	{
		try
		{
			return factorType switch
			{
				Document.OrderSell => EnLog.OrderSell, 
				Document.OrderBuy => EnLog.OrderBuy, 
				Document.OrderBackBuy => EnLog.OrderBackBuy, 
				Document.OrderBackSell => EnLog.OrderBackSell, 
				Document.PishFactor => EnLog.PishFactor, 
				Document.Zayeat => EnLog.Zayeat, 
				Document.AmaniVorod => EnLog.AmaniVorod, 
				Document.AmaniKhoroj => EnLog.AmaniKhoroj, 
				Document.OrderPreBuy => EnLog.OrderPreBuy, 
				_ => EnLog.None, 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLogType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7160);
			throw;
		}
	}

	public static async Task<List<SourceForDisplay>> GetAllExistingSourceAsync(string contextGuid, CancellationToken token, string tranName = "")
	{
		try
		{
			List<ENSource> list = await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllExistingSourceAsync(token, tranName);
			if (list == null || !list.Any())
			{
				return null;
			}
			return list.Select((ENSource q) => new SourceForDisplay(q)).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7174);
			throw;
		}
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		return await ClsCache.GetCache(ContextGuid).OrderSanadGenerator.GenerateSanadAsync(this, _oldOrder, system, token, tranName);
	}

	public static async Task<List<OrderReportBusiness>> GetAllMoghayerat50Async(string contextGuid, CancellationToken token, string tranName = "")
	{
		try
		{
			List<Guid> kolGuidList = new List<Guid>
			{
				ParentDefaults.KolCoding.ClsKol29998,
				ParentDefaults.KolCoding.ClsKol29999
			};
			return await ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllMoghayerat50Async(kolGuidList, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllMoghayerat50Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7193);
			throw;
		}
	}

	internal async Task<ReturnedSaveFuncInfo> CheckCashBackCouponAsync(CouponDiscountBusiness baseCoupon, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		Guid? couponGuid = null;
		try
		{
			_ = 11;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "OrderBusiness/CheckCashBackCouponAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7207);
					}
					else
					{
						if (autoCommit)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7208), ReturnedState.None, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7208);
						}
						transactionHasBeenCreated = true;
						if (baseCoupon == null)
						{
							result = await res.AddErrorAsync("BaseCoupon is Null !!", "", "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7211);
						}
						else if ((await GetRelatedCouponsAsync(null, baseCoupon.Guid, token, "")).HasError)
						{
							if ((CashBackCouponGuid ?? Guid.Empty) != Guid.Empty)
							{
								CouponDiscountBusiness couponDiscountBusiness = await CouponDiscountBusiness.GetAsync(ContextGuid, CashBackCouponGuid.Value, token, tranName);
								if (couponDiscountBusiness != null)
								{
									couponDiscountBusiness.Status = false;
									ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await couponDiscountBusiness.SaveAsync(EnSystem.None, null, sendToServer: true, Priority.Low, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7221);
								}
							}
							result = res;
						}
						else
						{
							OrderCouponRelationReportBusiness discountCalculation = await OrderCouponRelationReportBusiness.CalculateAsync(ContextGuid, this, baseCoupon, token, tranName);
							CouponDiscountBusiness coupon;
							if ((CashBackCouponGuid ?? Guid.Empty) != Guid.Empty)
							{
								coupon = await CouponDiscountBusiness.GetAsync(ContextGuid, CashBackCouponGuid.Value, token, tranName);
								if (!coupon.Status)
								{
									coupon.Status = true;
								}
								CouponDiscountDetailBusiness couponDiscountDetailBusiness = coupon.CouponDetailList?.SingleOrDefault();
								if (couponDiscountDetailBusiness != null)
								{
									couponDiscountDetailBusiness.AmountCoupon = discountCalculation.CouponDiscount;
								}
							}
							else
							{
								SanadPrintBusiness sanadPrintBusiness = await SanadPrintBusiness.GetAsync(ContextGuid, Guid, FactorType, token, tranName);
								string text = (string.IsNullOrEmpty(sanadPrintBusiness?.Name) ? (await GetTafsilAsync(EnSystem.UserInterface, tranName, token))?.Name : sanadPrintBusiness.Name);
								string title = "کوپن تخفیف در خرید بعدی".GetTranslate() + " " + (text ?? "").GetTranslate() + " ** " + "تاریخ فاکتور".GetTranslate() + " " + GetDate().MiladiToCalture() + " **" + " جمع خرید".GetTranslate() + " " + TotalSum.NpToString("N0", 0) + " " + ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit;
								CouponGeneratorRecord record = new CouponGeneratorRecord(baseCoupon, GetDate(), TafsilGuid, title, discountCalculation.CouponDiscount, EnDiscountType.OrderFixedPrice);
								coupon = await CouponDiscountBusiness.GenerateNewAsync(ContextGuid, EnSystem.UserInterface, record, token, tranName);
							}
							if (coupon == null)
							{
								result = res;
							}
							else
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await coupon.SaveAsync(EnSystem.UserInterface, this, sendToServer: true, Priority.Low, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7254);
								if (!res.HasError)
								{
									couponGuid = coupon.Guid;
									goto end_IL_017d;
								}
								result = res;
							}
						}
					}
					goto end_IL_00be;
					end_IL_017d:;
				}
				goto end_IL_00ab;
				end_IL_00be:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7260);
				res.AddReturnedValue(ex, "", "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7261);
				goto end_IL_00ab;
			}
			return result;
			end_IL_00ab:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await UpdateCashBackAsync(couponGuid, isProgressed: true, token, tranName), ReturnedState.None, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7265);
			if (autoCommit && transactionHasBeenCreated)
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7267), ReturnedState.None, "CheckCashBackCouponAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7267);
			}
		}
		return res;
	}

	internal static Task<IEnumerable<OrdersForUpdateCashBackModel>> GetAllForUpdateCashBackAsync(string contextGuid, DateTime? startDate, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetAllForUpdateCashBackAsync(startDate, token, tranName);
	}

	private async Task<ReturnedSaveFuncInfo> UpdateCashBackAsync(Guid? cashBackGuid, bool isProgressed, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				if (token.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7281);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoCommit)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7282), ReturnedState.None, "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7282);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.UpdateCashBackAsync(Guid, cashBackGuid, isProgressed, token, tranName), ReturnedState.None, "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7284);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7288);
				res.AddReturnedValue(ex, "", "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7289);
			}
		}
		finally
		{
			if (autoCommit && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7294), ReturnedState.None, "UpdateCashBackAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusiness.cs", 7294);
			}
		}
		return res;
	}

	public static Task<decimal> GetRelatedBackSellSumAsync(string contextGuid, Guid orderGuid, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetRelatedBackSellSumAsync(orderGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (system == EnSystem.Transfer || GetIsTemp())
				{
					return ret;
				}
				ENSource source = ClsCache.GetCache(ContextGuid).Source;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 29);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationReceptionAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 31);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationPardakhtAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 32);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationDetailsAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 33);
				ret.AddReturnedValue(CheckValidationVisitor(system), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 34);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationSanadAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 35);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationKerayeAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 36);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAvarezAsync(token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 37);
				if (FactorType != Document.Tolid_Mahsul && FactorType != Document.Tolid_Mavad)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationTolidAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 39);
				}
				if (!isDeleteAction && system == EnSystem.UserInterface && source == ENSource.Accounting)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckCustomerChequesAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 42);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckCustomerTasvieTimeAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 43);
				}
				if (!isDeleteAction && system == EnSystem.UserInterface)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SanadStatusReportBusiness.CheckValidationAsync(ContextGuid, system, _oldOrder?.StatusId ?? 0, StatusId, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 46);
				}
				if (ret.HasError)
				{
					return ret;
				}
				if (KardexAffective)
				{
					List<ProductsChanges> prdChanges = await GetLstProductsAnbarChangesAsync(system, token, tranName, isDeleteAction);
					CheckProductAmountConfig config = new CheckProductAmountConfig(ContextGuid)
					{
						Date = GetDate(),
						FactorType = FactorType,
						IsDeleteAction = isDeleteAction,
						IsTempOrder = GetIsTemp(),
						System = system,
						Tolid = Tolid,
						ChangeStatus = ChangeStatus
					};
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await prdChanges.CheckPrdAmountAsync(config, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 62);
				}
				if (ret.HasError || (!SanadAffective && !GetIsTemp()))
				{
					return ret;
				}
				if (SanadAffective && !GetIsTemp())
				{
					List<HesabChanges> changes = await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, new Tuple<Document, Guid>(FactorType, Guid));
					if (_reception != null && _reception.Sum > 0m)
					{
						List<HesabChanges> list = changes;
						list.AddRange(await _reception.GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, new Tuple<Document, Guid>(Document.Reception, _reception.Guid)));
					}
					if (_pardakht != null && _pardakht.Sum > 0m)
					{
						List<HesabChanges> list = changes;
						list.AddRange(await _pardakht.GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, new Tuple<Document, Guid>(Document.Pardakht, _pardakht.Guid)));
					}
					if (changes != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await changes.CheckAccountAsync(system, ContextGuid, token, tranName, FactorType, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 76);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 81);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 82);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			string contextGuid = ContextGuid;
			OrderReportBusiness orderReportBusiness = await GetReportById2Async(contextGuid, await GetId2Async(system, tranName, token), FactorType, Status, token, tranName);
			if (orderReportBusiness != null && orderReportBusiness.Guid != Guid && orderReportBusiness.Status == Status)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				EnSystem system2 = system;
				contextGuid = "شماره".GetTranslate();
				string translate = FactorType.GetDisplay().GetTranslate();
				int num = await GetId2Async(system, tranName, token);
				returnedSaveFuncInfo.AddError(system2, contextGuid + "  " + translate + " " + $" {num}" + " " + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 93);
			}
			if (Guid == Guid.Empty)
			{
				Guid = Guid.NewGuid();
			}
			if (GetDate() < new DateTime(2000, 1, 1) || GetDate() > new DateTime(2099, 1, 1))
			{
				res.AddError(system, "تاریخ وارد شده معتبر نمی باشد".GetTranslate() + " Date:" + GetDate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 96);
			}
			if (!GetIsTemp() && GetTafsilGuid() == Guid.Empty && FactorType != Document.Tolid && FactorType != Document.Tolid_Mahsul && FactorType != Document.Tolid_Mavad)
			{
				res.AddError(system, "ردیف طرف حساب تفصیلی".GetTranslate() + "  " + FactorType.GetDisplay().GetTranslate() + " " + "انتخاب شده معتبر نمی باشد".GetTranslate() + "\r\n" + $"TafsilGuid:{GetTafsilGuid()}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 98);
			}
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, GetTafsilGuid(), tranName, token, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs");
			if (!GetIsTemp() && GetTafsilGuid() != Guid.Empty && tafsilBusiness == null)
			{
				res.AddError(system, "طرف حساب فاکتور در جدول حساب ها موجود نمی باشد ".GetTranslate() + $"\r\n TafsilGuid:{GetTafsilGuid()}" + $"\r\n IsTemp:{GetIsTemp()}" + $"\r\n Tafsil is Null:{tafsilBusiness == null}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 102);
			}
			if (TotalSum < 0m && system != EnSystem.Transfer && !isDeleteAction)
			{
				res.AddError(system, "جمع کل منفی قابل ثبت نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 106);
			}
			if (system == EnSystem.UserInterface && !isDeleteAction)
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CheckOrderRemainAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 110);
				if (FactorType == Document.OrderBackSell && string.IsNullOrEmpty(Description) && setting.Factors.BackSell_WithoutDesc == ReturnedState.Error)
				{
					res.AddError("شما مجاز به ثبت فاکتور برگشت از فروش بدون توضیحات نمی باشید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 113);
				}
				if (FactorType == Document.OrderBackSell && string.IsNullOrEmpty(Description) && setting.Factors.BackSell_WithoutDesc == ReturnedState.Warning)
				{
					res.AddWarning("برای فاکتور جاری توضیحات درج ننموده اید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 115);
				}
				if (FactorType == Document.OrderSell && string.IsNullOrEmpty(Description) && setting.Factors.Sell_WithoutDesc == ReturnedState.Error)
				{
					res.AddError("شما مجاز به ثبت فاکتور فروش بدون توضیحات نمی باشید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 118);
				}
				if (FactorType == Document.OrderSell && string.IsNullOrEmpty(Description) && setting.Factors.Sell_WithoutDesc == ReturnedState.Warning)
				{
					res.AddWarning("برای فاکتور جاری توضیحات درج ننموده اید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 120);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 125);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 126);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationReceptionAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReceptionBusiness receptionBusiness = await GetReceptionAsync(system, tranName, fromDatabase: false, token);
			if (receptionBusiness != null && receptionBusiness.Sum > default(decimal) && (FactorType == Document.OrderSell || FactorType == Document.OrderBackBuy))
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.CheckValidationAsync(system, isDeleteAction, tranName, new Tuple<Document, Guid>(FactorType, Guid), token), ReturnedState.None, "CheckValidationReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 137);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 141);
			res.AddReturnedValue(ex, "", "CheckValidationReceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 142);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationPardakhtAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			PardakhtBusiness pardakhtBusiness = await GetPardakhtAsync(system, token, tranName);
			if (pardakhtBusiness != null && pardakhtBusiness.Sum > default(decimal) && (FactorType == Document.OrderBuy || FactorType == Document.OrderBackSell))
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.CheckValidationAsync(system, tranName, isDeleteAction, new Tuple<Document, Guid>(FactorType, Guid), token), ReturnedState.None, "CheckValidationPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 153);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 157);
			res.AddReturnedValue(ex, "", "CheckValidationPardakhtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 158);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationDetailsAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (ListKala == null || (ListKala.Count <= 0 && !isDeleteAction))
			{
				res.AddError(system, "هیچ کالا یا خدماتی انتخاب نشده است".GetTranslate() + "\r\n" + " شماره".GetTranslate() + " " + $"{FactorType.GetDisplay().GetTranslate()}:{_Id2}", "", "CheckValidationDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 169);
				WebTelegramMessage lkOnlineoperations_bot = WebTelegramMessage.Get_LkOnlineoperations_bot();
				string translate = "شماره".GetTranslate();
				string text = $" {FactorType.GetDisplay().GetTranslate()}:{_Id2} \r\n";
				string translate2 = "شناسه_کاربر".GetTranslate();
				string text2 = $"{UserAccGuid} \r\n ";
				string translate3 = "توضیحات".GetTranslate();
				string translate4 = (Description ?? "").GetTranslate();
				string translate5 = "شناسه قفل".GetTranslate();
				string lkSerial = LkSerial;
				string translate6 = "اطلاعات مشتری".GetTranslate();
				string translate7 = "نام مشتری".GetTranslate();
				string custName = CustName;
				string custFamily = CustFamily;
				string translate8 = " شماره موبایل".GetTranslate();
				string custMobile = CustMobile;
				string translate9 = "شماره تماس ".GetTranslate();
				string custTell = CustTell;
				WebOrder item = await ToWebItemAsync();
				lkOnlineoperations_bot.Sent2web("هیچ کالا یا خدماتی انتخاب نشده است\r\n" + translate + text + " #" + translate2 + " : " + text2 + translate3 + ":\r\n " + translate4 + "\ud83d\udd11" + translate5 + ": " + lkSerial + " \r\n" + translate6 + " \r\n " + translate7 + " : " + custName + " " + custFamily + " \r\n\ud83d\udcf1" + translate8 + " : " + custMobile + " \r\n☎" + translate9 + ": " + custTell + " \r\nOrder=>\r\n" + Json.ToStringJson(item), null, "CheckValidationDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 170);
			}
			else
			{
				foreach (OrderDetailBusiness item2 in ListKala)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item2.CheckValidationAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 184);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 188);
			res.AddReturnedValue(ex, "", "CheckValidationDetailsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 189);
		}
		return res;
	}

	private ReturnedSaveFuncInfo CheckValidationVisitor(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (FactorType == Document.OrderSell || FactorType == Document.OrderBackSell)
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
				{
					if (B1 > 0m && (!B1TafsilGuid.HasValue || B1TafsilGuid == Guid.Empty) && (!B1MoeinGuid.HasValue || B1MoeinGuid == Guid.Empty))
					{
						ResetVisitorPrice(1);
					}
					if (B2 > 0m && (!B2TafsilGuid.HasValue || B2TafsilGuid == Guid.Empty) && (!B2MoeinGuid.HasValue || B2MoeinGuid == Guid.Empty))
					{
						ResetVisitorPrice(2);
					}
					if (B3 > 0m && (!B3TafsilGuid.HasValue || B3TafsilGuid == Guid.Empty) && (!B3MoeinGuid.HasValue || B3MoeinGuid == Guid.Empty))
					{
						ResetVisitorPrice(3);
					}
					if (B4 > 0m && (!B4TafsilGuid.HasValue || B4TafsilGuid == Guid.Empty) && (!B4MoeinGuid.HasValue || B4MoeinGuid == Guid.Empty))
					{
						ResetVisitorPrice(4);
					}
				}
				if (B1 > 0m && (!B1TafsilGuid.HasValue || B1TafsilGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب تفصیلی بازاریاب".GetTranslate() + "1" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 213);
				}
				if (B2 > 0m && (!B2TafsilGuid.HasValue || B2TafsilGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب تفصیلی بازاریاب".GetTranslate() + "2" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 215);
				}
				if (B3 > 0m && (!B3TafsilGuid.HasValue || B3TafsilGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب تفصیلی بازاریاب".GetTranslate() + "3" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 217);
				}
				if (B4 > 0m && (!B4TafsilGuid.HasValue || B4TafsilGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب تفصیلی بازاریاب".GetTranslate() + "4" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 219);
				}
				if (B1 > 0m && (!B1MoeinGuid.HasValue || B1MoeinGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب معین بازاریاب ".GetTranslate() + "1" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 222);
				}
				if (B2 > 0m && (!B2MoeinGuid.HasValue || B2MoeinGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب معین بازاریاب ".GetTranslate() + "2" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 224);
				}
				if (B3 > 0m && (!B3MoeinGuid.HasValue || B3MoeinGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب معین بازاریاب ".GetTranslate() + "3" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 226);
				}
				if (B4 > 0m && (!B4MoeinGuid.HasValue || B4MoeinGuid == Guid.Empty))
				{
					returnedSaveFuncInfo.AddError(system, "طرف حساب معین بازاریاب ".GetTranslate() + "4" + "انتخاب نشده است.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 228);
				}
			}
			if (B1 < 0m)
			{
				returnedSaveFuncInfo.AddError(system, "مبلغ بازاریاب".GetTranslate() + " 1" + "منفی شده.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 231);
			}
			if (B2 < 0m)
			{
				returnedSaveFuncInfo.AddError(system, "مبلغ بازاریاب".GetTranslate() + " 2" + "منفی شده.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 232);
			}
			if (B3 < 0m)
			{
				returnedSaveFuncInfo.AddError(system, "مبلغ بازاریاب".GetTranslate() + " 3" + "منفی شده.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 233);
			}
			if (B4 < 0m)
			{
				returnedSaveFuncInfo.AddError(system, "مبلغ بازاریاب".GetTranslate() + " 4" + "منفی شده.".GetTranslate(), "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 234);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 238);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 239);
		}
		return returnedSaveFuncInfo;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationSanadAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (Debit != Credit)
			{
				res.AddError(system, "جمع بدهکار و بستانکار در سند متصل به".GetTranslate() + "  " + FactorType.GetDisplay().GetTranslate() + " " + " یکسان نیست.".GetTranslate(), "", "CheckValidationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 248);
			}
			if (ConnectedSanad.ListSanad_Det?.Any() ?? false)
			{
				IEnumerable<Sanad_DetBusiness> enumerable = ConnectedSanad.ListSanad_Det.Where((Sanad_DetBusiness q) => q.Debit <= 0m && q.Credit <= 0m && q.MoeinGuid == Guid.Empty && (q.TafsilGuid ?? Guid.Empty) == Guid.Empty);
				if (enumerable != null && enumerable.Any())
				{
					foreach (Sanad_DetBusiness item in enumerable)
					{
						await ConnectedSanad.RemoveFromListSanad(item.Guid, system, token);
					}
				}
				int num = 1;
				foreach (Sanad_DetBusiness item2 in ConnectedSanad.ListSanad_Det)
				{
					res.AddReturnedValue(item2.CheckValidation(system, num, isDeleteAction), ReturnedState.None, "CheckValidationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 260);
					num = checked(num + 1);
				}
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			EnSystem system2 = system;
			bool isDeleteAction2 = isDeleteAction;
			returnedSaveFuncInfo.AddReturnedValue(await CheckSanadValidationAsync(system2, isDeleteAction2, await GetId2Async(system, tranName, token), tranName, token, new Tuple<Document, Guid>(Document.Reception, ReceptionGuid ?? Guid.Empty), new Tuple<Document, Guid>(Document.Pardakht, PardakhtGuid ?? Guid.Empty)), ReturnedState.None, "CheckValidationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 264);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 268);
			res.AddReturnedValue(ex, "", "CheckValidationSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 269);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationKerayeAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (Keraye > 0m)
			{
				if (FactorType == Document.OrderSell)
				{
					if ((system == EnSystem.UserInterface && !FareTafsilGuid.HasValue) || FareTafsilGuid == Guid.Empty)
					{
						res.AddError(system, "طرف حساب بستانکار کرایه انتخاب نشده است".GetTranslate(), "", "CheckValidationKerayeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 284);
					}
					else if ((!FareMoeinGuid.HasValue || FareMoeinGuid == Guid.Empty) && FareTafsilGuid.HasValue && FareTafsilGuid != Guid.Empty)
					{
						TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, FareTafsilGuid.Value, tranName, token, "CheckValidationKerayeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs");
						TafsilGroupBusiness tafsilGroupBusiness = null;
						if (tafsilBusiness != null)
						{
							tafsilGroupBusiness = await (tafsilBusiness?.GetTafsilGroupAsync(system, token, tranName));
						}
						if ((tafsilGroupBusiness?.Guid ?? Guid.Empty) != Guid.Empty)
						{
							FareMoeinGuid = await tafsilGroupBusiness.GetMoeinGuidAsync(token);
						}
					}
				}
				else if (FactorType == Document.OrderBuy)
				{
					if (!FareMoeinGuid.HasValue || FareMoeinGuid == Guid.Empty)
					{
						FareMoeinGuid = accGlobalSettings.CodeSetting.BuyCoding.Debit_MoeinGuid_Keraye;
					}
					if (!FareTafsilGuid.HasValue || FareTafsilGuid == Guid.Empty)
					{
						FareTafsilGuid = accGlobalSettings.CodeSetting.BuyCoding.Debit_TafsilGuid_Keraye;
					}
				}
				if ((FareMoeinGuid ?? Guid.Empty) == Guid.Empty && FactorType != Document.PishFactor)
				{
					if (system == EnSystem.Transfer && FactorType == Document.PishFactor)
					{
						SetKeraye(0m);
					}
					else
					{
						res.AddError("مبلغ کرایه مشخص شده ولیکن طرف حساب معین کرایه مشخص نشده یا اینکه شما دسترسی به حساب معین کرایه ندارید.".GetTranslate() + "\r\n" + $"{Type.GetDisplay().GetTranslate()}({_Id2})", "", "CheckValidationKerayeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 308);
					}
				}
			}
			else
			{
				OrderBusiness orderBusiness = this;
				Guid? fareMoeinGuid = (FareTafsilGuid = null);
				orderBusiness.FareMoeinGuid = fareMoeinGuid;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationKerayeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 316);
			res.AddReturnedValue(ex, "", "CheckValidationKerayeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 317);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAvarezAsync(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (Avarez > 0m)
			{
				if (!AvarezMoeinGuid.HasValue || AvarezMoeinGuid == Guid.Empty)
				{
					AvarezMoeinGuid = accGlobalSettings.CodeSetting.SellCoding.Credit_MoeinGuid_Avarez;
				}
				if (!AvarezTafsilGuid.HasValue || AvarezTafsilGuid == Guid.Empty)
				{
					AvarezTafsilGuid = accGlobalSettings.CodeSetting.SellCoding.Credit_TafsilGuid_Avarez;
				}
				if ((AvarezMoeinGuid ?? Guid.Empty) == Guid.Empty)
				{
					res.AddError("مبلغ عوارض مشخص شده ولیکن طرف حساب معین عوارض مشخص نشده یا اینکه شما دسترسی به حساب معین عوارض ندارید.".GetTranslate(), "", "CheckValidationAvarezAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 336);
				}
			}
			else
			{
				OrderBusiness orderBusiness = this;
				Guid? avarezMoeinGuid = (AvarezTafsilGuid = null);
				orderBusiness.AvarezMoeinGuid = avarezMoeinGuid;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAvarezAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 343);
			res.AddReturnedValue(ex, "", "CheckValidationAvarezAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 344);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationTolidAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (_tolid != null)
			{
				_tolid.CheckMovazene = !isDeleteAction;
				decimal sumMavad = await _tolid.GetMavadSumAsync(system, token, tranName);
				decimal num = await _tolid.GetExportSumAsync(system, token, tranName);
				if (_tolid.HazineSum <= 0m && sumMavad <= 0m && num <= 0m)
				{
					return res;
				}
				ReturnedSaveFuncInfo resTolid = await _tolid.CheckValidationAsync(system, isDeleteAction, token, tranName);
				if (resTolid.HasError)
				{
					decimal num2 = await _tolid.GetExportSumAsync(system, token, tranName) - await _tolid.GetMavadSumAsync(system, token, tranName) - _tolid.HazineSum;
					if (num2 != 0m)
					{
						TolidHazineBusiness objects = new TolidHazineBusiness(ContextGuid)
						{
							TafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil44,
							Price = num2,
							Type = HazineType.Other,
							Description = "موازنه فرمول تولید".GetTranslate(),
							MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein43
						};
						await _tolid.AddHazineAsync(objects, system);
						resTolid = await _tolid.CheckValidationAsync(system, isDeleteAction, token, tranName);
					}
				}
				if (resTolid.HasError)
				{
					res.AddError(system, resTolid.GetErrorMessage(), "TolidBusiness", "CheckValidationTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 380);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 385);
			res.AddReturnedValue(ex, "", "CheckValidationTolidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 386);
		}
		return res;
	}

	private void ResetVisitorPrice(int index)
	{
		try
		{
			foreach (OrderDetailBusiness item in ListKala)
			{
				switch (index)
				{
				case 1:
					item.VisitorPrice1 = 0m;
					break;
				case 2:
					item.VisitorPrice2 = 0m;
					break;
				case 3:
					item.VisitorPrice3 = 0m;
					break;
				case 4:
					item.VisitorPrice4 = 0m;
					break;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetVisitorPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 404);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckCustomerChequesAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CusBusiness customer = await GetCustomerBusinessAsync(system, token, tranName);
			if (customer == null)
			{
				return res;
			}
			if (customer.ChequeBargashtiValidation != ReturnedState.Error && customer.ChequeBargashtiValidation != ReturnedState.Warning)
			{
				return res;
			}
			ReceptionChequeFilter filter = new ReceptionChequeFilter
			{
				TafsilGuid = TafsilGuid,
				Status = ENUMCheck.Bargashti
			};
			IEnumerable<ReceptionCheckReportBusiness> enumerable = await ReceptionCheckBusiness.GetAllAsync(ContextGuid, filter, token, tranName);
			if (enumerable == null || !enumerable.Any() || system != EnSystem.UserInterface || FactorType != Document.OrderSell)
			{
				return res;
			}
			if (customer.ChequeBargashtiValidation == ReturnedState.Error)
			{
				res.AddError(customer?.Name + " تعداد چک برگشتی".GetTranslate() + ":" + $" {enumerable.Count()}", "", "CheckCustomerChequesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 422);
			}
			else if (customer.ChequeBargashtiValidation == ReturnedState.Warning)
			{
				res.AddWarning(customer?.Name + "تعداد چک برگشتی".GetTranslate() + ":" + $" {enumerable.Count()}", "", "CheckCustomerChequesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 424);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCustomerChequesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 428);
			res.AddReturnedValue(ex, "", "CheckCustomerChequesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 429);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckCustomerTasvieTimeAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CusBusiness customer = await GetCustomerBusinessAsync(system, token, tranName);
			if (customer == null)
			{
				return res;
			}
			if (customer.OrderTasvieTimeValidation != ReturnedState.Error && customer.OrderTasvieTimeValidation != ReturnedState.Warning)
			{
				return res;
			}
			DateTime now = DateTime.Now;
			IEnumerable<OrderNotPayedViewModel> enumerable = await ClsCache.GetCache(ContextGuid).CacheRepository.IOrder.GetAllOrderNotPayedAsync(now, TafsilGuid, Guid, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (OrderNotPayedViewModel item in enumerable)
			{
				if (customer.OrderTasvieTimeValidation == ReturnedState.Error)
				{
					res.AddError("فاکتور فروش".GetTranslate() + " " + $" {item.Id2}" + " صادر شده در تاریخ".GetTranslate() + " " + item.DateSh + " به مبلغ".GetTranslate() + $" {item.TotalSum:N0}" + " تسویه نشده است".GetTranslate(), "", "CheckCustomerTasvieTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 449);
				}
				else if (customer.OrderTasvieTimeValidation == ReturnedState.Warning)
				{
					res.AddWarning("فاکتور فروش ".GetTranslate() + " " + $"{item.Id2} " + "صادر شده در تاریخ ".GetTranslate() + item.DateSh + "مبلغ ".GetTranslate() + $"{item.TotalSum:N0}" + "تسویه نشده است".GetTranslate(), "", "CheckCustomerTasvieTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 451);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCustomerTasvieTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 456);
			res.AddReturnedValue(ex, "", "CheckCustomerTasvieTimeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 457);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> CheckOrderRemainAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (FactorType != Document.OrderSell || !Status)
			{
				return res;
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			decimal recSum = await ReceptionSumAsync(system, token, tranName);
			if ((_linkedSanad?.ChildList?.Any()).GetValueOrDefault())
			{
				recSum = _linkedSanad.ChildList.Sum((TableRelationShipBusiness q) => q.Price);
			}
			if (AghsatGuid.HasValue && AghsatGuid != Guid.Empty)
			{
				recSum += (await AghsatMBusiness.GetAllByGroupGuidAsync(ContextGuid, AghsatGuid.Value, token, tranName))?.Sum((AghsatMBusiness q) => q.TotalPrice) ?? 0m;
			}
			if (Source != ENSource.Android_HesabYar)
			{
				recSum += await GetRelatedBackSellSumAsync(ContextGuid, Guid, token, tranName);
			}
			if (setting.Factors.Outstanding_Order == ReturnedState.Error && TotalSum > recSum && TasvieDay <= 0)
			{
				res.AddError("شما مجاز به ثبت فاکتور فروش مانده دار نمی باشید".GetTranslate(), "", "CheckOrderRemainAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 485);
			}
			else if (setting.Factors.Outstanding_Order == ReturnedState.Warning && TotalSum > recSum && TasvieDay <= 0)
			{
				res.AddWarning("کاربر گرامی فاکتور فروش جاری تسویه نشده است".GetTranslate(), "", "CheckOrderRemainAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 487);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckOrderRemainAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 491);
			res.AddReturnedValue(ex, "", "CheckOrderRemainAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderBusinessValidation.cs", 492);
		}
		return res;
	}
}
