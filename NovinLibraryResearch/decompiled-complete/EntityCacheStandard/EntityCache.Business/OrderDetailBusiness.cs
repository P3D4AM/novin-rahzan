using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Business.Global;
using EntityCache.Business.OrderDepot;
using EntityCache.Business.Products;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DepotReceipt;
using PacketParser.Entities.Interfaces.Accounting.Tolid;
using PacketParser.EventArgs.Sanad;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business;

public class OrderDetailBusiness : Serializable<OrderDetailBusiness>, IOrderDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDisposable, IHasContext, ITolidFormulaRelation, INotifyPropertyChanged, IHasQuantity
{
	public class DisplayMultiVahed
	{
		public string Name { get; set; }

		public Guid Guid { get; set; }

		public decimal Quantity { get; set; }
	}

	private decimal _decimalQt;

	private decimal _feePrice;

	private decimal _profitPrice;

	private List<OrderDetailBusiness> _detList;

	private AccGlobalSettings _setting;

	private CancellationTokenSource _disposeToken;

	private bool _isDisposed;

	private string _prdCode;

	private string _steppedDiscount;

	private CancellationTokenSource _loadDecimalToken;

	private CancellationTokenSource _setDefaultQtToken;

	private OrderDetailSerializedData _metaData;

	private decimal? lastCost;

	private KardexBusiness lastUnitCost;

	private PercentagePropertyContainer _visitor1;

	private PercentagePropertyContainer _visitor2;

	private PercentagePropertyContainer _visitor3;

	private PercentagePropertyContainer _visitor4;

	private PercentagePropertyContainer _discount;

	private PercentagePropertyContainer _tax;

	private PercentagePropertyContainer _avarez;

	private decimal _price;

	private decimal _hazinePrice1;

	private decimal _hazinePrice2;

	private decimal _hazinePrice3;

	private decimal _hazinePrice4;

	private decimal _hazinePrice5;

	private decimal _farePrice;

	private string _description = "";

	private Guid? _decimalGuid;

	private Guid? _productTafsilGuid;

	private DecimalBusiness _decimalObject;

	internal ProductCompactBusiness _prdInfo;

	private OrderDetailSerializedData _serialzedData;

	private string _prdVahed;

	private decimal? _v2Qt;

	private Guid _productGuid = Guid.Empty;

	private Guid? _vGuid;

	private List<PrdSerialBusiness> _serialStr;

	private Guid _depotGuid = Guid.Empty;

	private bool _isKala;

	private string _backSellQuantityString;

	private decimal _backSellQuantity;

	private string _strQt = "";

	private string _contextGuid;

	private Guid guid;

	private string unit2;

	private decimal? quantity;

	public IReadOnlyCollection<OrderDetailBusiness> DetailList
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<OrderDetailBusiness>();
			}
			_detList.RemoveAll((OrderDetailBusiness q) => q?.IsDisposed ?? true);
			return _detList;
		}
	}

	public decimal DecimalQt => _decimalQt;

	public string DecimalQtString => DecimalQt.ToString(3);

	public decimal MetaSellPrice => _metaData?.SellPrice ?? 0m;

	public decimal GoldPrice => Math.Round(DecimalQt * MetaSellPrice, DigitForRound);

	public decimal FeePrice => _feePrice;

	public decimal FeePercent => _metaData?.FeePercent ?? 0m;

	public string FeePercentString => ((double)FeePercent).ToString().Replace(".", "/");

	public EnFeeType FeeType => _metaData?.FeeType ?? EnFeeType.None;

	public decimal TotalFeePrice { get; private set; }

	public decimal ProfitPrice => _profitPrice;

	public decimal ProfitPercent => _metaData?.ProfitPercent ?? 0m;

	public string ProfitPercentString => ((double)ProfitPercent).ToString().Replace(".", "/");

	public EnFeeType ProfitType => _metaData?.ProfitType ?? EnFeeType.None;

	public decimal TotalProfitPrice { get; private set; }

	public decimal ProductTaxPrice => _metaData?.ProductTaxPrice ?? 0m;

	public decimal ProductTaxPercent => _metaData?.ProductTaxPercent ?? 0m;

	public string ProductTaxPercentString => ((double)ProductTaxPercent).ToString().Replace(".", "/");

	public decimal FeeTaxPrice => _metaData?.FeeTaxPrice ?? 0m;

	public decimal FeeTaxPercent => _metaData?.FeeTaxPercent ?? 0m;

	public string FeeTaxPercentString => ((double)FeeTaxPercent).ToString().Replace(".", "/");

	public decimal ProfitTaxPrice => _metaData?.ProfitTaxPrice ?? 0m;

	public decimal ProfitTaxPercent => _metaData?.ProfitTaxPercent ?? 0m;

	public string ProfitTaxPercentString => ((double)ProfitTaxPercent).ToString().Replace(".", "/");

	public decimal TotalTaxPrice => Math.Round(ProductTaxPrice + FeeTaxPrice + ProfitTaxPrice, DigitForRound);

	public decimal DecimalUnitPrice => Math.Round(DecimalTotalPrice / Quantity, DigitForRound);

	public decimal DecimalTotalPrice => Math.Round(_detList?.Sum((OrderDetailBusiness q) => q.DecimalRowSum) ?? 0m, DigitForRound);

	public decimal DecimalRowSum
	{
		get
		{
			if (!(_decimalQt == 0m))
			{
				return Math.Round(_decimalQt * Price, DigitForRound) + TotalFeePrice;
			}
			return RowSumWithTax + TotalFeePrice;
		}
	}

	public Guid Guid
	{
		get
		{
			if (guid == Guid.Empty)
			{
				guid = Guid.NewGuid();
			}
			return guid;
		}
		set
		{
			guid = value;
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public bool ChangeInPrice { get; set; }

	public bool ChangeInDiscount { get; set; }

	public Guid? GrouppingGuid { get; set; }

	public string Serial { get; set; }

	public IReadOnlyCollection<PrdSerialBusiness> SerialStr => _serialStr ?? (_serialStr = new List<PrdSerialBusiness>());

	public OrderAbadBusiness AbadStr { get; set; }

	public string DecimalSumString => Math.Round((double)DecimalSum, DigitForRound).ToString(3);

	public int Ordinal { get; set; } = -1;


	public byte[] IncreaseImage => ImageResourceManager.OrderIncreaseImage;

	public byte[] DecreaseImage => ImageResourceManager.OrderDecreaseImage;

	public byte[] DeleteImage => ImageResourceManager.OrderDeleteImage;

	public string Unit2
	{
		get
		{
			return unit2;
		}
		private set
		{
			if ((unit2 ?? "") == (value ?? ""))
			{
				return;
			}
			unit2 = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Unit2"));
				}, "Unit2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string ProductVahed
	{
		get
		{
			return _prdVahed;
		}
		private set
		{
			_prdVahed = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductVahed"));
				}, "ProductVahed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public bool IsChecked { get; set; }

	[JsonIgnore]
	public string DepotName => depotInfo?.Name ?? "";

	public string ProductName => _prdInfo?.Name ?? "";

	[JsonIgnore]
	public EnProductType ProductType => _prdInfo?.Type ?? EnProductType.None;

	[JsonIgnore]
	public EnProductKind ProductKind => _prdInfo?.Kind ?? EnProductKind.SimpleProduct;

	public string ProductNameForDisplay
	{
		get
		{
			string text = ProductName;
			ProductCompactBusiness prdInfo = _prdInfo;
			if (prdInfo != null && !prdInfo.Status)
			{
				text += " (غیرفعال) ";
			}
			if (!string.IsNullOrEmpty(Description))
			{
				text = text + " (شرح سطر: " + Description + " )";
			}
			return text;
		}
	}

	public Guid ProductGuid => _productGuid;

	public Guid DepotGuid => _depotGuid;

	public bool IsTaxImpactOnCost { get; set; }

	[JsonIgnore]
	public DepotBusiness depotInfo => DepotBusiness.Get(ContextGuid, _depotGuid);

	[JsonIgnore]
	public short DigitForRound { get; }

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public List<GiftDetailBusiness> GiftDetailList { get; set; }

	public bool DiscountChecked
	{
		get
		{
			if (!LocalSettingBusiness.GetSetting(ContextGuid).Factors.DiscountVisible)
			{
				return Discount > 0m;
			}
			return true;
		}
	}

	[JsonIgnore]
	public bool DiscountVisible
	{
		get
		{
			try
			{
				if (MasterOrder.FactorType == Document.OrderSell || MasterOrder.FactorType == Document.OrderBackSell || MasterOrder.FactorType == Document.OrderBuy || MasterOrder.FactorType == Document.OrderBackBuy || MasterOrder.FactorType == Document.PishFactor || MasterOrder.FactorType == Document.OrderPreBuy)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"MasterOrder is null:{MasterOrder == null}\r\n" + $"IsDisposed:{IsDisposed}", "DiscountVisible", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 412);
				throw;
			}
			return false;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public bool VisitorVisible
	{
		get
		{
			if (MasterOrder.FactorType != Document.OrderSell && MasterOrder.FactorType != Document.PishFactor)
			{
				return MasterOrder.FactorType == Document.OrderBackSell;
			}
			return true;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public bool VisitorChecked
	{
		get
		{
			if (!LocalSettingBusiness.GetSetting(ContextGuid).Factors.BazaryabVisible)
			{
				return VisitorPrice1 > 0m;
			}
			return true;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public bool Visitor2Visible
	{
		get
		{
			if (MasterOrder.FactorType != Document.OrderSell && MasterOrder.FactorType != Document.PishFactor)
			{
				return MasterOrder.FactorType == Document.OrderBackSell;
			}
			return true;
		}
	}

	public bool Visitor2Checked
	{
		get
		{
			if (!LocalSettingBusiness.GetSetting(ContextGuid).Factors.Bazaryab2Visible)
			{
				return TotalVisitorPrice > 0m;
			}
			return true;
		}
	}

	public bool TaxEnable => GetTaxEnable();

	public bool TaxVisible
	{
		get
		{
			if (MasterOrder.FactorType != Document.OrderSell && MasterOrder.FactorType != Document.OrderBackSell && MasterOrder.FactorType != Document.OrderBuy && MasterOrder.FactorType != Document.OrderBackBuy)
			{
				return MasterOrder.FactorType == Document.PishFactor;
			}
			return true;
		}
	}

	public bool TaxChecked
	{
		get
		{
			if (!LocalSettingBusiness.GetSetting(ContextGuid).Factors.MaliatVisible && !(Tax > 0m))
			{
				return Avarez > 0m;
			}
			return true;
		}
	}

	public decimal Quantity
	{
		get
		{
			if (_quantity.HasValue)
			{
				return _quantity.Value;
			}
			if ((MasterOrder?.Type ?? Document.OrderSell) == Document.OrderSell)
			{
				_setDefaultQtToken?.Cancel();
				_setDefaultQtToken = new CancellationTokenSource();
				CancellationToken token = _setDefaultQtToken.Token;
				SetDefaulQtAsync(token);
			}
			return _quantity.GetValueOrDefault();
		}
	}

	public string BackSellQuantityString
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_backSellQuantityString) && (_backSellQuantityString.EndsWith("/") || _backSellQuantityString.EndsWith(".")))
				{
					return _backSellQuantityString;
				}
				_backSellQuantityString = _backSellQuantity.ToString(3);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BackSellQuantityString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 504);
			}
			return _backSellQuantityString;
		}
		set
		{
			try
			{
				if (value.EndsWith("/") || value.EndsWith("."))
				{
					_backSellQuantityString = value;
				}
				else
				{
					_backSellQuantityString = "";
				}
				_backSellQuantity = value.ParseToDecimal(0m, 3);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BackSellQuantityString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 515);
			}
		}
	}

	public decimal? Price2 { get; set; }

	public decimal Price => _price;

	public string Description
	{
		get
		{
			int num = 0;
			try
			{
				if (string.IsNullOrEmpty(_description))
				{
					OrderAbadBusiness abadStr = AbadStr;
					if (abadStr != null && abadStr.ListAbad?.Count > 0)
					{
						foreach (AbadDetBusiness item in AbadStr?.ListAbad)
						{
							num = 1;
							if (item != null)
							{
								if (string.IsNullOrEmpty(_description))
								{
									_description = item.GetDescription() ?? "";
								}
								else
								{
									_description = _description + "\r\n" + item.GetDescription();
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line={num}", "Description", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 881);
				throw;
			}
			return _description;
		}
		set
		{
			if ((_description ?? "") == (value?.FixString() ?? ""))
			{
				return;
			}
			_description = value?.FixString() ?? "";
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Description"));
				}, "Description", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public decimal Discount => _discount.GetPrice();

	public Guid? VGuid
	{
		get
		{
			if (_prdInfo == null || _prdInfo.GuidVahed == Guid.Empty)
			{
				_vGuid = null;
			}
			return _vGuid;
		}
	}

	public decimal? V2Qt => _v2Qt;

	public decimal VisitorPrice1
	{
		get
		{
			if (_visitor1.GetPrice() <= 0m)
			{
				return 0m;
			}
			if ((MasterOrder?.B1TafsilGuid ?? Guid.Empty) == Guid.Empty && ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting && ClsCache.GetCache(ContextGuid).Source != ENSource.Baje)
			{
				return 0m;
			}
			return _visitor1.GetPrice();
		}
		set
		{
			if (_visitor1.SetPrice(value) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_visitor1.SetPriceString(value.ToString(), Quantity);
		}
	}

	public decimal VisitorPrice2
	{
		get
		{
			if (_visitor2.GetPrice() <= 0m)
			{
				return 0m;
			}
			if ((MasterOrder?.B2TafsilGuid ?? Guid.Empty) == Guid.Empty && ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting && ClsCache.GetCache(ContextGuid).Source != ENSource.Baje)
			{
				return 0m;
			}
			return _visitor2.GetPrice();
		}
		set
		{
			if (_visitor2.SetPrice(value) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice2", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_visitor2.SetPriceString(value.ToString(), Quantity);
		}
	}

	public decimal VisitorPrice3
	{
		get
		{
			if (_visitor3.GetPrice() <= 0m)
			{
				return 0m;
			}
			if ((MasterOrder?.B3TafsilGuid ?? Guid.Empty) == Guid.Empty && ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting && ClsCache.GetCache(ContextGuid).Source != ENSource.Baje)
			{
				return 0m;
			}
			return _visitor3.GetPrice();
		}
		set
		{
			if (_visitor3.SetPrice(value) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice3", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_visitor3.SetPriceString(value.ToString(), Quantity);
		}
	}

	public decimal VisitorPrice4
	{
		get
		{
			if (_visitor4.GetPrice() <= 0m)
			{
				return 0m;
			}
			if ((MasterOrder?.B4TafsilGuid ?? Guid.Empty) == Guid.Empty && ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting && ClsCache.GetCache(ContextGuid).Source != ENSource.Baje)
			{
				return 0m;
			}
			return _visitor4.GetPrice();
		}
		set
		{
			if (_visitor4.SetPrice(value) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice4", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_visitor4.SetPriceString(value.ToString(), Quantity);
		}
	}

	public decimal FarePrice
	{
		get
		{
			return _farePrice;
		}
		set
		{
			_farePrice = value;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "FarePrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FarePrice"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FarePriceString"));
				}, "FarePrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string FarePriceString => Math.Round(FarePrice, DigitForRound).NpToString("N0", 0);

	public decimal TotalVisitorPrice => VisitorPrice1 + VisitorPrice2 + VisitorPrice3 + VisitorPrice4;

	public decimal TotalHazinePrice
	{
		get
		{
			return Hazine1Price + Hazine2Price + Hazine3Price + Hazine4Price + Hazine5Price + (((MasterOrder?.FactorType ?? Document.None) == Document.OrderBuy) ? FarePrice : 0m);
		}
		set
		{
		}
	}

	public decimal TaxDiscount
	{
		get
		{
			try
			{
				if (TaxPercent <= 0m || Discount <= 0m)
				{
					return 0m;
				}
				return Math.Round(Discount * TaxPercent / 100m);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TaxDiscount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1130);
			}
			return 0m;
		}
	}

	public decimal Tax
	{
		get
		{
			decimal price = _tax.GetPrice();
			if (price <= 0m)
			{
				return 0m;
			}
			return Math.Floor(price);
		}
	}

	public decimal TaxImpactonCost
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return 0m;
			}
			return Tax;
		}
	}

	public decimal TaxStandard
	{
		get
		{
			if (IsTaxImpactOnCost)
			{
				return 0m;
			}
			return Tax;
		}
	}

	public decimal TaxImpactPerUnit => TaxImpactonCost + Math.Round(Price * TaxPercent / 100m, DigitAfterPoint);

	public decimal Avarez
	{
		get
		{
			if (_avarez.GetPrice() <= 0m)
			{
				return 0m;
			}
			return _avarez.GetPrice();
		}
		set
		{
			if (_avarez.SetPrice(value) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Avarez", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_avarez.SetPriceString(value.ToString(), Quantity);
		}
	}

	public decimal RowSum
	{
		get
		{
			if (DigitAfterPoint > 0)
			{
				return (Math.Round(Quantity, 3) * Price - Discount).NpToString("", 0).ParseToDecimal(0m, DigitAfterPoint);
			}
			return Math.Floor(Math.Round(Quantity, 3) * Price) - Discount;
		}
	}

	public decimal RowSumWithTax => RowSum + TaxStandard + TaxImpactonCost + Avarez;

	public decimal V2Price { get; set; }

	public decimal Hazine1Price
	{
		get
		{
			return _hazinePrice1;
		}
		set
		{
			_hazinePrice1 = value;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Hazine1Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine1Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine1String"));
				}, "Hazine1Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public decimal Hazine2Price
	{
		get
		{
			return _hazinePrice2;
		}
		set
		{
			_hazinePrice2 = value;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Hazine2Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine2Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine2String"));
				}, "Hazine2Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public decimal Hazine3Price
	{
		get
		{
			return _hazinePrice3;
		}
		set
		{
			_hazinePrice3 = value;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Hazine3Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine3Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine3String"));
				}, "Hazine3Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public decimal Hazine4Price
	{
		get
		{
			return _hazinePrice4;
		}
		set
		{
			_hazinePrice4 = value;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Hazine4Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine4Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine4String"));
				}, "Hazine4Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public decimal Hazine5Price
	{
		get
		{
			return _hazinePrice5;
		}
		set
		{
			_hazinePrice5 = value;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Hazine5Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine5Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Hazine5String"));
				}, "Hazine5Price", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string Hazine1String
	{
		get
		{
			return Math.Round(Hazine1Price, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Hazine1Price = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string Hazine2String
	{
		get
		{
			return Math.Round(Hazine2Price, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Hazine2Price = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string Hazine3String
	{
		get
		{
			return Math.Round(Hazine3Price, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Hazine3Price = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string Hazine4String
	{
		get
		{
			return Math.Round(Hazine4Price, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Hazine4Price = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string Hazine5String
	{
		get
		{
			return Math.Round(Hazine5Price, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Hazine5Price = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public Guid? DecimalGuid
	{
		get
		{
			return _decimalGuid;
		}
		set
		{
			_decimalGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
			if (_decimalGuid.HasValue)
			{
				_loadDecimalToken?.Cancel();
				_loadDecimalToken = new CancellationTokenSource();
				CancellationToken token = _loadDecimalToken.Token;
				Task.Run(() => SetDecimalAsync(token));
			}
		}
	}

	public decimal DecimalPrice { get; set; }

	private decimal? _quantity
	{
		get
		{
			return quantity;
		}
		set
		{
			quantity = value;
		}
	}

	public string DecimalName => _decimalObject?.NameForDisplay ?? "";

	public decimal DecimalSum => DecimalQt * Quantity;

	public Guid? PriceTypeGuid { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public decimal DiscountPercent => _discount.GetPercent(RowSum + Discount);

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice1Percent
	{
		get
		{
			try
			{
				return (float)_visitor1.GetPercent(RowSum);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VisitorPrice1Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1430);
			}
			return 0f;
		}
		set
		{
			if (_visitor1.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice1Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice2Percent
	{
		get
		{
			try
			{
				return (float)_visitor2.GetPercent(RowSum);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VisitorPrice2Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1447);
			}
			return 0f;
		}
		set
		{
			if (_visitor2.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice2Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice3Percent
	{
		get
		{
			try
			{
				return (float)_visitor3.GetPercent(RowSum);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VisitorPrice3Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1464);
			}
			return 0f;
		}
		set
		{
			if (_visitor3.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice3Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public string RowSumForDisplay => RowSum.NpToString("N0", 0);

	[JsonIgnore]
	[XmlIgnore]
	public float VisitorPrice4Percent
	{
		get
		{
			try
			{
				return (float)_visitor4.GetPercent(RowSum);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "VisitorPrice4Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1482);
			}
			return 0f;
		}
		set
		{
			if (_visitor4.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice4Percent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public OrderBusiness MasterOrder { get; internal set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal ProductWeight => _prdInfo?.Weight ?? 0m;

	[XmlIgnore]
	[JsonIgnore]
	public decimal TotalProductWeight => Math.Round(ProductWeight * Quantity, DigitForRound);

	public string VisitorPrice1PercentString
	{
		get
		{
			return VisitorPrice1Percent.ToString(3);
		}
		set
		{
			if (_visitor1.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice1PercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string VisitorPrice2PercentString
	{
		get
		{
			return VisitorPrice2Percent.ToString(3);
		}
		set
		{
			if (_visitor2.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice2PercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string VisitorPrice3PercentString
	{
		get
		{
			return VisitorPrice3Percent.ToString(3);
		}
		set
		{
			if (_visitor3.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice3PercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string VisitorPrice4PercentString
	{
		get
		{
			return VisitorPrice4Percent.ToString(3);
		}
		set
		{
			if (_visitor4.SetPercentString(value.ToString(), RowSum) && this.OnVisitorChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), default(CancellationToken), 60, "VisitorPrice4PercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string QuantityUnitForDisplay => QuantityString + " " + ProductVahed;

	public string QuantityString
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_strQt) && (_strQt.EndsWith("/") || _strQt.EndsWith(TextManager.Instance.DecimalPoint)))
				{
					return _strQt;
				}
				_strQt = Quantity.ToString(3);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "QuantityString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1875);
			}
			return _strQt;
		}
	}

	public string Quantity2String => V2Qt?.ToString(3) ?? "";

	public string V2QtString => V2Qt?.ToString(3) ?? "";

	public string PriceString => Math.Round(Price, DigitForRound).NpToString("N0", 0);

	public string Price2String
	{
		get
		{
			if (!VGuid.HasValue || !V2Qt.HasValue || V2Qt.Value <= 0m)
			{
				return "";
			}
			if (V2Qt.GetValueOrDefault() != 0m && Price2.GetValueOrDefault() == 0m)
			{
				Price2 = Price * Quantity / V2Qt.Value;
			}
			return Math.Round(Price2.Value, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			Price2 = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string DiscountPercentString => DiscountPercent.ToString(3);

	public string TaxPercentString
	{
		get
		{
			return TaxPercent.ToString(3);
		}
		set
		{
			if (_tax.SetPercentString(value.ToString(), RowSum, EnRoundDecimal.Floor) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "TaxPercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public string DiscountString
	{
		get
		{
			if (!(Discount == 0m))
			{
				return Math.Round(Discount, DigitForRound).NpToString("N0", 0);
			}
			return "";
		}
	}

	public string VisitorPrice1String
	{
		get
		{
			return Math.Round(VisitorPrice1, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			_visitor1.SetPriceString(value, Quantity);
			VisitorPrice1 = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string VisitorPrice2String
	{
		get
		{
			return Math.Round(VisitorPrice2, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			_visitor2.SetPriceString(value, Quantity);
			VisitorPrice2 = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string VisitorPrice3String
	{
		get
		{
			return Math.Round(VisitorPrice3, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			_visitor3.SetPriceString(value, Quantity);
			VisitorPrice3 = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	public string VisitorPrice4String
	{
		get
		{
			return Math.Round(VisitorPrice4, DigitForRound).NpToString("N0", 0);
		}
		set
		{
			_visitor4.SetPriceString(value, Quantity);
			VisitorPrice4 = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	[Obsolete("", true)]
	public string PriceLevelName => "دستی";

	[JsonIgnore]
	public string RowDescriptionForPrint
	{
		get
		{
			int num = 0;
			try
			{
				num = 1;
				ProductCompactBusiness prdInfo = _prdInfo;
				num = 2;
				string text = "";
				num = 3;
				if (prdInfo == null)
				{
					throw new NullReferenceException($"prd is null!! //ProductGuid:{ProductGuid}");
				}
				text = text + prdInfo.IdProduct + " " + prdInfo.Name;
				num = 4;
				if (!string.IsNullOrEmpty(Description))
				{
					num = 7;
					text = text + "<br/> " + Description;
				}
				num = 6;
				return text;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "RowDescriptionForPrint", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2428);
				throw;
			}
		}
	}

	public string QuantityForPrint
	{
		get
		{
			string text = "";
			text += QuantityString;
			if (V2Qt.HasValue)
			{
				text = text + "<br/> " + V2QtString;
			}
			return text;
		}
	}

	public EntityChangeSt OrderDetState { get; set; }

	public decimal TaxPercent
	{
		get
		{
			return _tax.GetPercent(RowSum);
		}
		set
		{
			_tax.SetPercentString(value.ToString(), RowSum, EnRoundDecimal.Floor);
		}
	}

	public decimal AvarezPercent
	{
		get
		{
			return _avarez.GetPercent(RowSum);
		}
		set
		{
			_avarez.SetPercentString(value.ToString(), RowSum);
		}
	}

	public string AvarezPercentString
	{
		get
		{
			return AvarezPercent.ToString(3);
		}
		set
		{
			if (_avarez.SetPercentString(value.ToString(), RowSum) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "AvarezPercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
	}

	public bool AvarezEnable => GetAvarezEnable();

	public string TaxString
	{
		get
		{
			if (!(Tax == 0m))
			{
				return Math.Round(Tax, DigitAfterPoint).NpToString("N0", 0);
			}
			return "";
		}
	}

	public string AvarezString
	{
		get
		{
			return Math.Round(Avarez, DigitAfterPoint).NpToString("N0", 0);
		}
		set
		{
			_avarez.SetPriceString(value, Quantity);
			Avarez = value.ParseToDecimal(0m, DigitAfterPoint);
		}
	}

	[JsonIgnore]
	public string Code
	{
		get
		{
			try
			{
				if (_prdInfo?.CodeForDisplay == null && !string.IsNullOrEmpty(_prdCode))
				{
					return _prdCode;
				}
				if (_prdInfo?.CodeForDisplay == null && string.IsNullOrEmpty(_prdCode) && !string.IsNullOrEmpty(_prdInfo?.IdProduct))
				{
					return _prdInfo.IdProduct;
				}
				return _prdInfo?.CodeForDisplay.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Code", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2500);
			}
			return "";
		}
	}

	[XmlIgnore]
	[JsonIgnore]
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

	public bool IsEdit { get; set; }

	public string ContextGuid
	{
		get
		{
			if (string.IsNullOrEmpty(_contextGuid))
			{
				throw new ArgumentNullException("_contextGuid");
			}
			return _contextGuid;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}
			_contextGuid = value;
		}
	}

	public bool IsKala
	{
		get
		{
			ProductCompactBusiness prdInfo = _prdInfo;
			if (prdInfo == null || prdInfo.Guid == Guid.Empty)
			{
				return _isKala;
			}
			if (prdInfo == null)
			{
				return false;
			}
			return prdInfo.Type == EnProductType.Product;
		}
		set
		{
			_isKala = value;
		}
	}

	[Obsolete]
	public int ID { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public short TolidFormulaIndex { get; set; }

	public Guid? DicountCouponDetailGuid { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public OrderDetailSerializedData SerialzedData
	{
		get
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.AvarezMode = _avarez.Mode;
			_metaData.DiscountMode = _discount.Mode;
			_metaData.TaxMode = _tax.Mode;
			_metaData.Visitor1Mode = _visitor1.Mode;
			_metaData.Visitor2Mode = _visitor2.Mode;
			_metaData.Visitor3Mode = _visitor3.Mode;
			_metaData.Visitor4Mode = _visitor4.Mode;
			return _metaData;
		}
	}

	public Guid? ProductTafsilGuid
	{
		internal get
		{
			return _productTafsilGuid;
		}
		set
		{
			_productTafsilGuid = value;
		}
	}

	public decimal CostPrice { get; set; }

	public decimal TotalCostPrice { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public decimal TotalPrice => Math.Round(Price * Quantity, DigitForRound);

	[XmlIgnore]
	[JsonIgnore]
	public decimal SumAfterDiscount => TotalPrice - Discount;

	[XmlIgnore]
	[JsonIgnore]
	public decimal SumTaxAndAvarez => TaxStandard + TaxImpactonCost + Avarez;

	public string SteppedDiscount => _steppedDiscount;

	public event Func<object, EventArgs, Task> OnDecimalChanged;

	public event PropertyChangedEventHandler PropertyChanged;

	public event Func<object, EventArgs, Task> OnDepotChanged;

	public event Func<object, EventArgs, Task> OnRowSumChanged;

	public event Func<object, EventArgs, Task> OnVisitorChanged;

	public event Func<object, EventArgs, Task> OnQuantityChanged;

	public event Func<object, EventArgs, Task> OnDisposed;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	private void CalculateFee()
	{
		try
		{
			if (FeeType == EnFeeType.None || ProductKind != EnProductKind.Decimal)
			{
				return;
			}
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			switch (FeeType)
			{
			case EnFeeType.Percent:
				TotalFeePrice = (_feePrice = Math.Round(GoldPrice * FeePercent / 100m, DigitForRound));
				break;
			case EnFeeType.FixedPrice:
				TotalFeePrice = _feePrice;
				break;
			case EnFeeType.VariablePrice:
				if (DecimalQt <= 0m)
				{
					return;
				}
				TotalFeePrice = Math.Round(_feePrice * DecimalQt, DigitForRound);
				break;
			default:
				throw new NotImplementedException();
			}
			CalculateProfit();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateFee", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 79);
		}
	}

	private void CalculateProfit()
	{
		try
		{
			if (ProfitType == EnFeeType.None)
			{
				return;
			}
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			switch (ProfitType)
			{
			case EnFeeType.Percent:
				TotalProfitPrice = (_profitPrice = Math.Round((GoldPrice + TotalFeePrice) * ProfitPercent / 100m, DigitForRound));
				break;
			case EnFeeType.FixedPrice:
				TotalProfitPrice = _profitPrice;
				break;
			case EnFeeType.VariablePrice:
				if (!(DecimalQt <= 0m))
				{
					TotalProfitPrice = Math.Round(_profitPrice * DecimalQt, DigitForRound);
				}
				break;
			default:
				throw new NotImplementedException();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateProfit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 99);
		}
	}

	public async Task AddDetailListAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			_setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			await SetGoldAsync(system, token);
			await SetFeeAsync(system, token);
			await SetProfitAsync(system, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 110);
		}
	}

	public async Task AddDetailListAsync(EnSystem system, OrderDetailBusiness det, CancellationToken token)
	{
		using (new JaegerMonitor("AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs"))
		{
			if (det == null)
			{
				return;
			}
			try
			{
				if (_detList == null)
				{
					_detList = new List<OrderDetailBusiness>();
				}
				OrderDetailBusiness orderDetailBusiness = _detList?.FirstOrDefault((OrderDetailBusiness q) => q.Guid == det.Guid);
				if (orderDetailBusiness != null)
				{
					_detList.Remove(orderDetailBusiness);
				}
				det.GrouppingGuid = Guid;
				det.OnRowSumChanged += Det_OnRowSumChanged;
				det.OnDecimalChanged += Det_OnDecimalChanged;
				det.OnDisposed += Det_OnDisposed;
				_detList.Add(det);
				await MasterOrder.AddDetAsync(det, system, awaitOnSemaphor: true, token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 128);
			}
		}
	}

	private async Task Det_OnDisposed(object sender, EventArgs e)
	{
		try
		{
			if (this.OnRowSumChanged != null)
			{
				_disposeToken?.Cancel();
				_disposeToken = new CancellationTokenSource();
				CancellationToken token = _disposeToken.Token;
				await Task.Delay(new Random().Next(500));
				if (!token.IsCancellationRequested && this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
				}
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 145);
		}
	}

	private async Task Det_OnDecimalChanged(object sender, EventArgs e)
	{
		try
		{
			if (this.OnDecimalChanged != null)
			{
				await Task.Delay(new Random().Next(500));
				if (this.OnDecimalChanged != null)
				{
					PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Det_OnDecimalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
				}
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Det_OnDecimalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 157);
		}
	}

	private async Task Det_OnRowSumChanged(object sender, EventArgs e)
	{
		try
		{
			if (this.OnRowSumChanged != null)
			{
				await Task.Delay(new Random().Next(500));
				if (this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, default(CancellationToken), 60, "Det_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
				}
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Det_OnRowSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 169);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SetGoldAsync(EnSystem system, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			OrderDetailBusiness gold = DetailList?.SingleOrDefault((OrderDetailBusiness q) => q.ProductGuid == ProductGuid && (q.GrouppingGuid ?? Guid.Empty) != Guid.Empty);
			if (gold == null)
			{
				gold = await OrderDetailFactoryAsync(ContextGuid, MasterOrder, token);
				await gold.SetProductGuidAsync(system, ProductGuid, isLoadOriginalPrice: true, "", token);
				await AddDetailListAsync(system, gold, token);
			}
			await gold.SetQuantityAsync(system, Quantity, token);
			if (Quantity != 0m)
			{
				await gold.SetPriceAsync(system, Math.Round(GoldPrice / Quantity, DigitForRound), token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetGoldAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 190);
			res.AddReturnedValue(ex, "", "SetGoldAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 191);
		}
		return res;
	}

	public Task SetMetaSellPriceAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.SellPrice = value;
			CalculateFee();
			Task.Run(() => SetGoldAsync(system, token));
			Task.Run(() => SetFeeAsync(system, token));
			Task.Run(() => SetProfitAsync(system, token));
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetMetaSellPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetMetaSellPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetMetaSellPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 213);
		}
		return Task.CompletedTask;
	}

	public Task SetDecimalQuantityAsync(EnSystem system, decimal value, bool handleSameProducts = false, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_decimalQt = value;
			CalculateFee();
			if (handleSameProducts)
			{
				Task.Run(() => SetGoldAsync(system, token));
				Task.Run(() => SetFeeAsync(system, token));
				Task.Run(() => SetProfitAsync(system, token));
			}
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetDecimalQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetDecimalQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDecimalQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 236);
		}
		return Task.CompletedTask;
	}

	public async Task SetProductTaxPriceAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.ProductTaxPrice = value;
			_metaData.ProductTaxPercent = Math.Round(_metaData.ProductTaxPrice * 100m / GoldPrice, DigitForRound);
			if (DetailList?.FirstOrDefault((OrderDetailBusiness q) => q.ProductGuid == ProductGuid && (q.GrouppingGuid ?? Guid.Empty) != Guid.Empty) == null)
			{
				await SetGoldAsync(system, token);
			}
			SetTax(system, TotalTaxPrice, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProductTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProductTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 258);
		}
	}

	public Task SetProductTaxPercentAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.ProductTaxPercent = value;
			_metaData.ProductTaxPrice = Math.Round(GoldPrice * value / 100m, DigitForRound);
			SetTax(system, TotalTaxPrice, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProductTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProductTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 276);
		}
		return Task.CompletedTask;
	}

	private async Task<ReturnedSaveFuncInfo> SetFeeAsync(EnSystem system, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((_setting?.Products?.DefaultFeeProductGuid ?? Guid.Empty) == Guid.Empty && TotalFeePrice == 0m)
			{
				return res;
			}
			if ((_setting?.Products?.DefaultFeeProductGuid ?? Guid.Empty) == Guid.Empty && TotalFeePrice != 0m)
			{
				await res.AddErrorAsync("خدمت معادل اجرت ساخت انتخاب نشده است.".GetTranslate() + " \r\n" + "لطفا از تنظیمات، سربرگ کالاها، خدمت معادل اجرت ساخت را انتخاب نمایید".GetTranslate(), "", "SetFeeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 288);
				return res;
			}
			OrderDetailBusiness feeProduct = DetailList?.FirstOrDefault((OrderDetailBusiness q) => q.ProductGuid == _setting.Products.DefaultFeeProductGuid.Value);
			if (feeProduct != null && TotalFeePrice == 0m)
			{
				feeProduct.Dispose();
			}
			else
			{
				if (feeProduct == null)
				{
					feeProduct = await OrderDetailFactoryAsync(ContextGuid, MasterOrder, token);
					await feeProduct.SetProductGuidAsync(system, _setting.Products.DefaultFeeProductGuid.Value, isLoadOriginalPrice: true, "", token);
					await feeProduct.SetQuantityAsync(system, 1m, token);
					await AddDetailListAsync(system, feeProduct, token);
				}
				await feeProduct.SetPriceAsync(system, Math.Round(TotalFeePrice, DigitForRound), token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 307);
			res.AddReturnedValue(ex, "", "SetFeeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 308);
		}
		return res;
	}

	private async Task SetFeeAsync_(EnSystem system, CancellationToken token)
	{
		_ = 1;
		try
		{
			if (!((_setting?.Products?.DefaultFeeProductGuid ?? Guid.Empty) == Guid.Empty))
			{
				OrderDetailBusiness orderDetailBusiness = DetailList?.SingleOrDefault((OrderDetailBusiness q) => q.ProductGuid == _setting.Products.DefaultFeeProductGuid.Value);
				if (orderDetailBusiness == null)
				{
					await SetFeeAsync(system, token);
				}
				else
				{
					await orderDetailBusiness.SetPriceAsync(system, Math.Round(TotalFeePrice, DigitForRound), token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeeAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 321);
		}
	}

	public async Task SetFeePriceAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_feePrice = value;
			CalculateFee();
			await SetFeeAsync_(system, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetFeePriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetFeePriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeePriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 337);
		}
	}

	public async Task SetFeePercent(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.FeePercent = value;
			_metaData.FeeType = EnFeeType.Percent;
			CalculateFee();
			await SetFeeAsync_(system, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetFeePercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetFeePercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeePercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 355);
		}
	}

	public async Task SetFeeTypeAsync(EnSystem system, EnFeeType value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.FeeType = value;
			CalculateFee();
			await SetFeeAsync_(system, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetFeeTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetFeeTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeeTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 372);
		}
	}

	public async Task SetFeeTaxPriceAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.FeeTaxPrice = value;
			_metaData.FeeTaxPercent = Math.Floor(Math.Round(_metaData.FeeTaxPrice * 100m / GoldPrice, DigitForRound));
			await SetFeeAsync_(system, token);
			SetTax(system, TotalTaxPrice, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetFeeTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetFeeTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeeTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 390);
		}
	}

	public async Task SetFeeTaxPercentAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.FeeTaxPercent = value;
			_metaData.FeeTaxPrice = Math.Floor(Math.Round(FeePrice * value / 100m, DigitForRound));
			await SetFeeAsync_(system, token);
			SetTax(system, TotalTaxPrice, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetFeeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetFeeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFeeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 408);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SetProfitAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((_setting?.Products?.DefaultProfitProductGuid ?? Guid.Empty) == Guid.Empty && TotalProfitPrice == 0m)
			{
				return res;
			}
			if ((_setting?.Products?.DefaultProfitProductGuid ?? Guid.Empty) == Guid.Empty && TotalProfitPrice != 0m)
			{
				await res.AddErrorAsync("خدمت معادل سود فروشنده انتخاب نشده است.".GetTranslate() + " \r\n " + "لطفا از تنظیمات، سربرگ کالاها، خدمت معادل سود فروشنده را انتخاب نمایید".GetTranslate(), "", "SetProfitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 419);
				return res;
			}
			OrderDetailBusiness profitProduct = DetailList?.SingleOrDefault((OrderDetailBusiness q) => q.ProductGuid == _setting.Products.DefaultProfitProductGuid.Value);
			if (profitProduct != null && TotalProfitPrice == 0m)
			{
				profitProduct.Dispose();
			}
			else
			{
				if (profitProduct == null)
				{
					profitProduct = await OrderDetailFactoryAsync(ContextGuid, MasterOrder, token);
					await profitProduct.SetProductGuidAsync(system, _setting.Products.DefaultProfitProductGuid.Value, isLoadOriginalPrice: true, "", token);
					await profitProduct.SetQuantityAsync(system, 1m, token);
					await AddDetailListAsync(system, profitProduct, token);
				}
				await profitProduct.SetPriceAsync(system, Math.Round(TotalProfitPrice, DigitForRound), token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 438);
			res.AddReturnedValue(ex, "", "SetProfitAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 439);
		}
		return res;
	}

	private async Task SetProfitAsync_(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (!((_setting?.Products?.DefaultProfitProductGuid ?? Guid.Empty) == Guid.Empty))
			{
				OrderDetailBusiness orderDetailBusiness = DetailList?.SingleOrDefault((OrderDetailBusiness q) => q.ProductGuid == _setting.Products.DefaultProfitProductGuid.Value);
				if (orderDetailBusiness == null)
				{
					await SetProfitAsync(system, token);
				}
				else
				{
					await orderDetailBusiness.SetPriceAsync(system, Math.Round(TotalProfitPrice, DigitForRound), token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 452);
		}
	}

	public async Task SetProfitTypeAsync(EnSystem system, EnFeeType value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.ProfitType = value;
			CalculateProfit();
			await SetProfitAsync_(system, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProfitTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProfitTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 469);
		}
	}

	public async Task SetProfitPrice(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_profitPrice = value;
			CalculateProfit();
			await SetProfitAsync_(system, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProfitPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProfitPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 485);
		}
	}

	public async Task SetProfitPercentAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.ProfitPercent = value;
			_metaData.ProfitType = EnFeeType.Percent;
			CalculateProfit();
			await SetProfitAsync_(system, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProfitPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProfitPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 503);
		}
	}

	public async Task SetProfitTaxPriceAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.ProfitTaxPrice = value;
			_metaData.ProfitTaxPercent = Math.Floor(Math.Round(_metaData.ProfitTaxPrice * 100m / GoldPrice, DigitForRound));
			await SetProfitAsync_(system, token);
			SetTax(system, TotalTaxPrice, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProfitTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProfitTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitTaxPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 521);
		}
	}

	public async Task SetProfitTaxPercentAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_metaData == null)
			{
				_metaData = new OrderDetailSerializedData();
			}
			_metaData.ProfitTaxPercent = value;
			_metaData.ProfitTaxPrice = Math.Floor(Math.Round(ProfitPrice * value / 100m, DigitForRound));
			await SetProfitAsync_(system, token);
			SetTax(system, TotalTaxPrice, token);
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnDecimalChanged, this, EventArgs.Empty, token, 60, "SetProfitTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetProfitTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProfitTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DecimalOrderDetailBusiness.cs", 539);
		}
	}

	public Task SetSerialAsync(PrdSerialBusiness serial, EnSystem system, bool clearList = false)
	{
		try
		{
			if (serial == null)
			{
				return Task.CompletedTask;
			}
			if (_serialStr == null)
			{
				_serialStr = new List<PrdSerialBusiness>();
			}
			if (clearList)
			{
				_serialStr = new List<PrdSerialBusiness>();
			}
			if (system == EnSystem.Mapper || system == EnSystem.Transfer)
			{
				_serialStr.Add(serial);
				return Task.CompletedTask;
			}
			if (serial.ProductGuid != ProductGuid)
			{
				return Task.CompletedTask;
			}
			PrdSerialBusiness prdSerialBusiness = _serialStr.FirstOrDefault((PrdSerialBusiness q) => q.Serial == serial.Serial);
			if (prdSerialBusiness != null)
			{
				_serialStr.Remove(prdSerialBusiness);
			}
			Document _asType = MasterOrder.FactorType;
			PrdSerialDetailBusiness prdSerialDetailBusiness = serial.DetList?.FirstOrDefault((PrdSerialDetailBusiness q) => q.AsType == _asType && q.DetailGuid == Guid);
			if (prdSerialDetailBusiness == null)
			{
				prdSerialDetailBusiness = new PrdSerialDetailBusiness(ContextGuid)
				{
					AsType = _asType,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					MasterGuid = serial.Guid,
					Modified = DateTime.Now,
					SanadDate = MasterOrder?.GetDate()
				};
			}
			if (_asType == Document.OrderBuy || _asType == Document.OrderBackSell || _asType == Document.Tolid_Mahsul)
			{
				prdSerialDetailBusiness.IsVorod = true;
			}
			else if (_asType == Document.OrderSell || _asType == Document.OrderBackBuy || _asType == Document.Tolid_Mavad)
			{
				prdSerialDetailBusiness.IsVorod = false;
			}
			serial.AddDet(prdSerialDetailBusiness);
			_serialStr.Add(serial);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 116);
		}
		return Task.CompletedTask;
	}

	public void RemoveSerial(PrdSerialBusiness serial)
	{
		try
		{
			List<PrdSerialBusiness> serialStr = _serialStr;
			if (serialStr != null && serialStr.Any())
			{
				_serialStr.Remove(serial);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveSerial", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 126);
		}
	}

	public void ClearSerialList()
	{
		_serialStr?.Clear();
	}

	public async Task<ProductCompactBusiness> GetPrdInfoAsync(EnSystem system = EnSystem.None, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (ProductGuid == Guid.Empty)
			{
				_prdInfo = null;
				return _prdInfo;
			}
			if ((_prdInfo?.Guid ?? Guid.Empty) != ProductGuid && ProductGuid != Guid.Empty)
			{
				_prdInfo = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, system, null, tranName, token, null);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 198);
			throw;
		}
		return _prdInfo;
	}

	public async Task SetProductGuidAsync(EnSystem system, Guid value, bool isLoadOriginalPrice, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_productGuid == value || system == EnSystem.Transfer)
			{
				return;
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (setting.Factors.InsertPRDQT && MasterOrder?.ListKala?.FirstOrDefault((OrderDetailBusiness q) => q.ProductGuid == value && q.Guid != Guid) != null)
			{
				throw new InvalidOperationException("در ویرایش اقلام فاکتور خطی کالا به صورت تکراری اضافه می شود");
			}
			if (_productGuid != value)
			{
				ClearSerialList();
			}
			_productGuid = value;
			_vGuid = null;
			_visitor1.Reset();
			_visitor2.Reset();
			_visitor3.Reset();
			_visitor4.Reset();
			_discount.Reset();
			_tax.Reset();
			_avarez.Reset();
			_prdInfo = await GetPrdInfoAsync(system, token, tranName);
			decimal dpercent = _prdInfo?.DiscountPercent ?? 0m;
			if (_productGuid != Guid.Empty && MasterOrder != null && system == EnSystem.UserInterface && ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar && await GetIsMultivahedAsync(token, tranName))
			{
				Guid guid = await LocalSettingBusiness.GetLastUnitGuidAsync(ContextGuid, CashierBusiness.GetCurrentCashierGuid(ContextGuid), MasterOrder.FactorType, _productGuid, token, tranName);
				if (guid != Guid.Empty)
				{
					_v2Qt = setting.Sells.OrderSellDefaultQT;
					await SetVGuidAsync(system, guid, token, tranName);
				}
			}
			if (_prdInfo != null)
			{
				ProductVahed = (await PrdVahedBusiness.GetAsync(ContextGuid, _prdInfo.GuidVahed, token, tranName)).Name;
			}
			if (!ChangeInPrice)
			{
				if (_prdInfo != null && (PriceTypeGuid ?? Guid.Empty) != Guid.Empty && system != EnSystem.Mapper)
				{
					_price = await _prdInfo.GetSellAsync(system, PriceTypeGuid.Value, isLoadOriginalPrice, token, tranName);
				}
				else if (_prdInfo != null && (PriceTypeGuid ?? Guid.Empty) == Guid.Empty && system != EnSystem.Mapper)
				{
					_price = ((await GetAllPricesAsync(system, token, tranName))?.FirstOrDefault()?.Price).GetValueOrDefault();
				}
				else
				{
					_price = default(decimal);
				}
			}
			if (!ChangeInDiscount && (PriceTypeGuid ?? Guid.Empty) != Guid.Empty && dpercent > 0m)
			{
				decimal num = (await ProductPriceLevelBusiness.GetAsync(ContextGuid, PriceTypeGuid.Value, system, token, tranName))?.DiscountCoefficient ?? 0m;
				if (num > 1m)
				{
					dpercent *= num;
				}
			}
			if (MasterOrder.FactorType == Document.OrderSell || MasterOrder.FactorType == Document.OrderBackSell || MasterOrder.FactorType == Document.PishFactor)
			{
				if (dpercent > 0m && dpercent <= 100m)
				{
					if (MasterOrder.FactorType == Document.OrderSell || (MasterOrder.FactorType == Document.OrderBackSell && ((_prdInfo?.DiscountStartDate.HasValue ?? false) || (_prdInfo?.DiscountEndDate.HasValue ?? false))))
					{
						DateTime date = MasterOrder.GetDate();
						if (!_prdInfo.DiscountStartDate.HasValue || !_prdInfo.DiscountEndDate.HasValue || (!(_prdInfo.DiscountStartDate.Value > date) && !(_prdInfo.DiscountEndDate < date)))
						{
							_discount.UpdatePrice(Quantity, Quantity * Price, dpercent);
						}
					}
					else
					{
						_discount.UpdatePrice(Quantity, Quantity * Price, dpercent);
					}
				}
				else if ((_prdInfo?.DiscountPrice ?? 0m) > 0m)
				{
					if (MasterOrder.FactorType == Document.OrderSell || (MasterOrder.FactorType == Document.OrderBackSell && ((_prdInfo?.DiscountStartDate.HasValue ?? false) || (_prdInfo?.DiscountEndDate.HasValue ?? false))))
					{
						DateTime date2 = MasterOrder.GetDate();
						if (!_prdInfo.DiscountStartDate.HasValue || !_prdInfo.DiscountEndDate.HasValue || (!(_prdInfo.DiscountStartDate.Value > date2) && !(_prdInfo.DiscountEndDate < date2)))
						{
							_discount.SetPriceString(_prdInfo.DiscountPrice.ToString(), Quantity);
						}
					}
					else
					{
						_discount.SetPriceString(_prdInfo.DiscountPrice.ToString(), Quantity);
					}
				}
			}
			if (GetTaxEnable(token, tranName) && (_prdInfo?.IsTax ?? false) && MasterOrder.FactorType != Document.Tolid && MasterOrder.FactorType != Document.Tolid_Mahsul && MasterOrder.FactorType != Document.Tolid_Mavad)
			{
				CusBusiness customer = await MasterOrder.GetCustomerBusinessAsync(system, token, tranName);
				decimal num2 = await _prdInfo.GetTaxPercentAsync(token, tranName);
				if (customer != null)
				{
					switch (customer.TaxType)
					{
					case TypeTax.None:
						if ((setting?.Sells?.CustomerTaxEnable).GetValueOrDefault())
						{
							TaxPercentString = num2.ToString(3);
						}
						else
						{
							TaxPercentString = "0";
						}
						break;
					case TypeTax.Disable:
						TaxPercentString = "0";
						break;
					case TypeTax.Enable:
						TaxPercentString = num2.ToString(3);
						break;
					}
				}
				else
				{
					TaxPercentString = num2.ToString(3);
				}
			}
			await UpdateVisitorPricesAsync(system, token, tranName);
			await UpdateTax_AvarexAsync(system, tranName, token);
			if (this.OnQuantityChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnQuantityChanged, this, new EventArgs(), token, 60, "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnDecimalChanged, this, new EventArgs(), token, 60, "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductNameForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductName"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductVahed"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Code"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PriceString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSum"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumWithTax"));
				}, "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 337);
			throw;
		}
	}

	public async Task<decimal> GetBahayeVahedAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			bool flag = ProductGuid == Guid.Empty;
			if (!flag)
			{
				flag = await GetDepotGuidAsync() == Guid.Empty;
			}
			if (flag)
			{
				return Price;
			}
			string contextGuid = ContextGuid;
			KardexFilter kardexFilter = new KardexFilter
			{
				PrdGuid = ProductGuid
			};
			KardexFilter kardexFilter2 = kardexFilter;
			kardexFilter2.DepotGuid = await GetDepotGuidAsync();
			kardexFilter.Date2 = MasterOrder.GetDate();
			kardexFilter.CountForDisplay = (short)1;
			await SetPriceAsync(system, (((await KardexBusiness.GetAllAsync(contextGuid, kardexFilter, token))?.value?.SingleOrDefault())?.RemUnitPrice).GetValueOrDefault(), token);
			return Price;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBahayeVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 358);
			throw;
		}
	}

	public async Task<bool> GetDiscountEnable(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		short line = 0;
		UserAccBusiness user = null;
		try
		{
			user = await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).GetUserAsync(token, tranName);
			if (MasterOrder.FactorType == Document.OrderSell && user.UserAccess.Sell.Sell_EditAghlamDiscount)
			{
				return true;
			}
			line = 1;
			if (MasterOrder.FactorType == Document.OrderBackSell && user.UserAccess.BackSell.BackSell_EditAghlamDiscount)
			{
				return true;
			}
			line = 2;
			if (MasterOrder.FactorType == Document.OrderBuy && user.UserAccess.Buy.Buy_EditAghlamDiscount)
			{
				return true;
			}
			line = 3;
			if (MasterOrder.FactorType == Document.PishFactor && user.UserAccess.PishFacotr.PishFactor_EditAghlamDiscount)
			{
				return true;
			}
			if (MasterOrder.FactorType == Document.OrderPreBuy && user.UserAccess.PreBuy.PreBuy_EditAghlamDiscount)
			{
				return true;
			}
			line = 4;
			if (MasterOrder.FactorType == Document.OrderBackBuy && user.UserAccess.BackBuy.BackBuy_EditAghlamDiscount)
			{
				return true;
			}
			line = 5;
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line={line}//IsDisposed:{IsDisposed}//MasterOrder is null:{MasterOrder == null}//UserAccBusiness.GetCurrentUser(ContextGuid).UserAccess.Sell is null:{user?.UserAccess?.Sell == null}", "GetDiscountEnable", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 391);
		}
		return false;
	}

	public async Task<bool> GetVisitorEnableAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
		UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
		UserAccBusiness userAccBusiness2 = userAccBusiness;
		return (MasterOrder.FactorType == Document.OrderSell && userAccBusiness2.UserAccess.Sell.EditVisitorPercent) || (MasterOrder.FactorType == Document.OrderBackSell && userAccBusiness2.UserAccess.BackSell.EditVisitorPercent) || (MasterOrder.FactorType == Document.PishFactor && userAccBusiness2.UserAccess.PishFacotr.EditVisitorPercent);
	}

	public async Task<bool> GetVisitor2EnableAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
		UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
		UserAccBusiness userAccBusiness2 = userAccBusiness;
		return (MasterOrder.FactorType == Document.OrderSell && userAccBusiness2.UserAccess.Sell.EditVisitorPercent) || (MasterOrder.FactorType == Document.OrderBackSell && userAccBusiness2.UserAccess.BackSell.EditVisitorPercent) || (MasterOrder.FactorType == Document.PishFactor && userAccBusiness2.UserAccess.PishFacotr.EditVisitorPercent);
	}

	public bool GetTaxEnable(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ProductCompactBusiness prdInfo = _prdInfo;
		if (prdInfo != null && prdInfo.IsTax)
		{
			if ((MasterOrder?.FactorType ?? Document.None) != Document.OrderSell && (MasterOrder?.FactorType ?? Document.None) != Document.OrderBackSell && (MasterOrder?.FactorType ?? Document.None) != Document.OrderBuy && (MasterOrder?.FactorType ?? Document.None) != Document.OrderBackBuy)
			{
				return (MasterOrder?.FactorType ?? Document.None) == Document.PishFactor;
			}
			return true;
		}
		return false;
	}

	private async Task SetDefaulQtAsync(CancellationToken token)
	{
		_ = 6;
		try
		{
			do
			{
				await Task.Delay(50);
				if (_quantity.HasValue || token.IsCancellationRequested || IsDisposed)
				{
					return;
				}
			}
			while (_prdInfo == null);
			if (!_quantity.HasValue && !token.IsCancellationRequested && !IsDisposed)
			{
				ProductBusiness productBusiness = await _prdInfo.GetFullProductAsync();
				if (!_quantity.HasValue && !token.IsCancellationRequested && !IsDisposed && !productBusiness.ScaleSt)
				{
					await SetQuantityAsync(EnSystem.None, (await LocalSettingBusiness.GetSettingAsync(ContextGuid, token)).Sells.OrderSellDefaultQT, token);
					await UpdateDiscountPriceAsync(TaxPercent, AvarezPercent, EnSystem.None, token, "");
					await UpdateVisitorPricesAsync(EnSystem.None, token);
					await UpdateTax_AvarexAsync(EnSystem.None, "", token);
				}
			}
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetDefaulQtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 491);
		}
	}

	public async Task SetQuantityAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		try
		{
			if (_quantity.GetValueOrDefault() == value)
			{
				return;
			}
			if (system == EnSystem.Mapper)
			{
				_quantity = value;
				return;
			}
			Console.WriteLine(string.Format("%%%%%%%%%%%%%%%%%%%%{0}.value:{1}. Quantity:{2}", _prdInfo?.Name ?? "prnInfo is null", value, _quantity));
			decimal taxPercent = TaxPercent;
			decimal avarezPercent = AvarezPercent;
			line = 1;
			_quantity = value;
			line = 2;
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (await GetV2ZaribAsync() > 0m)
			{
				ENSource source = ClsCache.GetCache(ContextGuid).Source;
				if (source == ENSource.Accounting || source == ENSource.Baje)
				{
					if (setting.Products.ChandVahedi)
					{
						decimal valueOrDefault = _quantity.GetValueOrDefault();
						_v2Qt = valueOrDefault / await GetV2ZaribAsync();
					}
				}
				else
				{
					decimal valueOrDefault = _quantity.GetValueOrDefault();
					_v2Qt = valueOrDefault / await GetV2ZaribAsync();
				}
			}
			else
			{
				_v2Qt = null;
			}
			line = 17;
			if (system == EnSystem.UserInterface && ((MasterOrder?.FactorType ?? Document.None) == Document.OrderSell || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBackSell || (MasterOrder?.FactorType ?? Document.None) == Document.PishFactor || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBuy || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBackBuy || (MasterOrder?.FactorType ?? Document.None) == Document.OrderPreBuy))
			{
				await UpdateDiscountPriceAsync(taxPercent, avarezPercent, system, token, tranName);
				await UpdateVisitorPricesAsync(system, token, tranName);
				await UpdateTax_AvarexAsync(system, tranName, token);
			}
			line = 18;
			if (this.OnQuantityChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnQuantityChanged, this, new EventArgs(), token, 60, "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			line = 13;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.OnDecimalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnDecimalChanged, this, new EventArgs(), token, 60, "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QuantityString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSum"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumWithTax"));
				}, "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $" The Error Line IS line:{line}", "SetQuantityAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 641);
			throw;
		}
	}

	private Task UpdateTax_AvarexAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (IsDisposed)
		{
			return Task.CompletedTask;
		}
		if (MasterOrder == null)
		{
			throw new ArgumentException("MasterOrder is null");
		}
		using (new JaegerMonitor("UpdateTax_AvarexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs"))
		{
			try
			{
				if (system == EnSystem.Mapper)
				{
					return Task.CompletedTask;
				}
				if (_tax.UpdatePrice(Quantity, RowSum, TaxPercent, 0, EnRoundDecimal.Floor) && this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "UpdateTax_AvarexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
				if (_avarez.UpdatePrice(Quantity, RowSum, AvarezPercent) && this.OnRowSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "UpdateTax_AvarexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
				if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
				{
					UiInvokerHolder.Instance.RunOnUiThread(delegate
					{
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tax"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaxString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaxPercent"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TaxPercentString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Avarez"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvarezString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvarezPercent"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AvarezPercentString"));
					}, "UpdateTax_AvarexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateTax_AvarexAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 675);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	private async Task UpdateVisitorPricesAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (MasterOrder == null)
		{
			return;
		}
		using (new JaegerMonitor("UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs"))
		{
			_ = 8;
			try
			{
				if (system == EnSystem.Mapper || (MasterOrder.FactorType != Document.OrderSell && MasterOrder.FactorType != Document.OrderBackSell))
				{
					return;
				}
				decimal b1Percent = (await CusBusiness.GetAsync(ContextGuid, system, MasterOrder?.B1TafsilGuid ?? Guid.Empty, token, tranName))?.BPercent ?? 0m;
				if (b1Percent == 0m)
				{
					b1Percent = 1m;
				}
				if (b1Percent < 0m)
				{
					b1Percent = default(decimal);
				}
				decimal b2Percent = (await CusBusiness.GetAsync(ContextGuid, system, MasterOrder?.B2TafsilGuid ?? Guid.Empty, token, tranName))?.BPercent ?? 0m;
				if (b2Percent == 0m)
				{
					b2Percent = 1m;
				}
				if (b2Percent < 0m)
				{
					b2Percent = default(decimal);
				}
				decimal b3Percent = (await CusBusiness.GetAsync(ContextGuid, system, MasterOrder?.B3TafsilGuid ?? Guid.Empty, token, tranName))?.BPercent ?? 0m;
				if (b3Percent == 0m)
				{
					b3Percent = 1m;
				}
				if (b3Percent < 0m)
				{
					b3Percent = default(decimal);
				}
				decimal b4Percent = (await CusBusiness.GetAsync(ContextGuid, system, MasterOrder?.B4TafsilGuid ?? Guid.Empty, token, tranName))?.BPercent ?? 0m;
				if (b4Percent == 0m)
				{
					b4Percent = 1m;
				}
				if (b4Percent < 0m)
				{
					b4Percent = default(decimal);
				}
				ProductCompactBusiness prd = await GetPrdInfoAsync(system, token, tranName);
				if (prd == null)
				{
					return;
				}
				PercentagePropertyContainer visitor = _visitor1;
				decimal newQuantity = Quantity;
				decimal rowSum = RowSum;
				decimal num = b1Percent;
				if (visitor.UpdatePrice(newQuantity, rowSum, num * await prd.GetBPercent1Async()) && this.OnVisitorChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), token, 60, "UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
				visitor = _visitor2;
				num = Quantity;
				rowSum = RowSum;
				newQuantity = b2Percent;
				if (visitor.UpdatePrice(num, rowSum, newQuantity * await prd.GetBPercent2Async()) && this.OnVisitorChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), token, 60, "UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
				visitor = _visitor3;
				newQuantity = Quantity;
				rowSum = RowSum;
				num = b3Percent;
				if (visitor.UpdatePrice(newQuantity, rowSum, num * await prd.GetBPercent3Async()) && this.OnVisitorChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), token, 60, "UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
				visitor = _visitor4;
				num = Quantity;
				rowSum = RowSum;
				newQuantity = b4Percent;
				if (visitor.UpdatePrice(num, rowSum, newQuantity * await prd.GetBPercent4Async()) && this.OnVisitorChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnVisitorChanged, this, new EventArgs(), token, 60, "UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
				if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
				{
					UiInvokerHolder.Instance.RunOnUiThread(delegate
					{
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice1"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice1Percent"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice1PercentString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice1String"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice2"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice2Percent"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice2PercentString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice2String"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice3"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice3Percent"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice3PercentString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice3String"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice4"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice4Percent"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice4PercentString"));
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisitorPrice4String"));
					}, "UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateVisitorPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 741);
			}
		}
	}

	public async Task<decimal> GetTolidPriceAsync()
	{
		return await _prdInfo.GetBuy1Async();
	}

	public async Task SetPriceAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (value < 0m)
		{
			throw new ArgumentOutOfRangeException($"value({value}) is less than zero");
		}
		int line = 0;
		try
		{
			line = 1;
			if (_price == value)
			{
				return;
			}
			decimal taxPercent = TaxPercent;
			decimal avarezPercent = AvarezPercent;
			_price = value;
			ChangeInPrice = true;
			line = 2;
			if ((MasterOrder?.FactorType ?? Document.None) == Document.OrderSell || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBackSell || (MasterOrder?.FactorType ?? Document.None) == Document.PishFactor || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBuy || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBackBuy || (MasterOrder?.FactorType ?? Document.None) == Document.OrderPreBuy)
			{
				await UpdateDiscountPriceAsync(taxPercent, avarezPercent, system, token, tranName);
				await UpdateVisitorPricesAsync(system, token, tranName);
				await UpdateTax_AvarexAsync(system, tranName, token);
			}
			line = 3;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PriceString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSum"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumWithTax"));
				}, "SetPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line={line}", "SetPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 790);
			throw;
		}
	}

	private async Task UpdateDiscountPriceAsync(decimal taxPercent, decimal avarezPrice, EnSystem system, CancellationToken token, string tranName)
	{
		try
		{
			TaxPercent = taxPercent;
			AvarezPercent = avarezPrice;
			decimal defaultPercent = await GetDefaultDiscountPercentAsync(system, token, tranName);
			if (_discount.UpdatePrice(Quantity, Quantity * Price, defaultPercent) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "UpdateDiscountPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged == null || UiInvokerHolder.Instance == null)
			{
				return;
			}
			UiInvokerHolder.Instance.RunOnUiThread(delegate
			{
				try
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Discount"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DiscountString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DiscountPercent"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DiscountPercentString"));
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "UpdateDiscountPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 816);
				}
			}, "UpdateDiscountPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDiscountPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 819);
		}
	}

	public static Task<Guid> GetDepotGuidAsync(string contextGuid, Guid detailGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrder.GetDepotGuidAsync(detailGuid, token, tranName);
	}

	public async Task<Guid> GetDepotGuidAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (_depotGuid != Guid.Empty)
			{
				return _depotGuid;
			}
			await SetDepotGuidAsync((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token))?.DepotGuid ?? _depotGuid, token);
			return _depotGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 837);
			throw;
		}
	}

	public Task SetDepotGuidAsync(Guid value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (value == Guid.Empty || _depotGuid == value)
			{
				return Task.CompletedTask;
			}
			_depotGuid = value;
			if (this.OnDepotChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnDepotChanged, this, new EventArgs(), token, 60, "SetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepotName"));
				}, "SetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDepotGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 857);
			throw;
		}
		return Task.CompletedTask;
	}

	public async Task SetDiscountAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_discount.SetPrice(value) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetDiscountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_discount.SetPriceString(value.ToString(), Quantity);
			ChangeInDiscount = true;
			if (!IsDisposed && system != EnSystem.Mapper)
			{
				await UpdateVisitorPricesAsync(system, token);
				await UpdateTax_AvarexAsync(system, tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDiscountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 914);
			throw;
		}
	}

	public async Task SetVGuidAsync(EnSystem system, Guid? value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			_vGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
			if (system != EnSystem.Mapper)
			{
				if (_vGuid.HasValue && _prdInfo != null && _prdInfo.GuidVahed == _vGuid.Value)
				{
					_vGuid = null;
					decimal value2 = _v2Qt ?? _quantity.GetValueOrDefault();
					_v2Qt = null;
					await SetQuantityAsync(system, value2, token, tranName);
				}
				decimal? v2Qt = V2Qt;
				_v2Qt = null;
				await SetV2QtAsync(system, v2Qt, token, tranName);
			}
			if ((VGuid ?? Guid.Empty) == Guid.Empty)
			{
				Unit2 = "";
			}
			else
			{
				Unit2 = (await PrdVahedBusiness.GetAsync(ContextGuid, VGuid.Value, token, tranName)).Name;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetVGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 954);
			throw;
		}
	}

	public void SetQtAndQt2FromMapper(double qt, double? v2Qt)
	{
		_quantity = (decimal)qt;
		_v2Qt = (decimal?)v2Qt;
	}

	public async Task SetV2QtAsync(EnSystem system, decimal? value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		try
		{
			if (_v2Qt.GetValueOrDefault() == value.GetValueOrDefault() && ClsCache.GetCache(ContextGuid).Source == ENSource.Android_HesabYar)
			{
				return;
			}
			if (system == EnSystem.Mapper)
			{
				_v2Qt = value;
				return;
			}
			decimal taxPercent = TaxPercent;
			decimal avarezPercent = AvarezPercent;
			_v2Qt = value;
			decimal num = await GetV2ZaribAsync();
			if (value.HasValue && num > 0m)
			{
				OrderDetailBusiness orderDetailBusiness = this;
				decimal value2 = num;
				decimal? num2 = value;
				orderDetailBusiness._quantity = ((decimal?)value2 * num2).GetValueOrDefault();
			}
			else if (value.HasValue && num == 0m)
			{
				_quantity = value.GetValueOrDefault();
				_v2Qt = null;
				_vGuid = null;
				unit2 = "";
			}
			line = 1;
			await UpdateDiscountPriceAsync(taxPercent, avarezPercent, system, token, tranName);
			if (IsDisposed)
			{
				return;
			}
			await UpdateVisitorPricesAsync(system, token);
			await UpdateTax_AvarexAsync(system, tranName, token);
			if (this.OnQuantityChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnQuantityChanged, this, EventArgs.Empty, token, 60, "SetV2QtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			line = 2;
			if (this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(this.OnRowSumChanged, this, EventArgs.Empty, token, 60, "SetV2QtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Quantity2String"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("V2Qt"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("V2QtString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("V2Zarib"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("V2Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("QuantityString"));
				}, "SetV2QtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"line={line}", "SetV2QtAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1014);
			throw;
		}
	}

	public void SetTax(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			decimal price = value;
			if (_tax.SetPrice(price) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetTax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			_tax.SetPriceString(value.ToString(), Quantity);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTax", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1143);
		}
	}

	public void SetTaxString(EnSystem system, decimal value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			decimal num = value;
			_tax.SetPriceString(num.ToString(), Quantity);
			SetTax(system, num.ToString().ParseToDecimal(0m, DigitAfterPoint), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTaxString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1153);
		}
	}

	private async Task SetDecimalAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_decimalObject == null && DecimalGuid.HasValue && !token.IsCancellationRequested)
			{
				_decimalObject = await DecimalBusiness.GetAsync(ContextGuid, DecimalGuid.Value, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDecimalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1310);
		}
	}

	public async Task<bool> GetIsMultivahedAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_prdInfo == null)
			{
				return false;
			}
			bool flag = _prdInfo.V2Qt > 0m;
			if (!flag)
			{
				flag = await _prdInfo.GetV3QtAsync(token, tranName) > 0m;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				flag2 = await _prdInfo.GetV4QtAsync(token, tranName) > 0m;
			}
			return flag2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetIsMultivahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1338);
			throw;
		}
	}

	public async Task<bool> GetPriceVisibleAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
		UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
		UserAccBusiness userAccBusiness2 = userAccBusiness;
		return MasterOrder.FactorType != Document.Zayeat || (MasterOrder.FactorType == Document.Zayeat && userAccBusiness2.UserAccess.Product.SeeLastPrice);
	}

	public async Task<bool> PriceEnableAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		try
		{
			if (MasterOrder == null)
			{
				return false;
			}
			line = 1000;
			if (!(await MasterOrder.EnableEditPrice(token, tranName)))
			{
				return false;
			}
			line = 1;
			if (MasterOrder.Moein != null && MasterOrder.Moein.Tedadi)
			{
				return false;
			}
			line = 2;
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			line = 3;
			UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
			UserAccBusiness userAccBusiness2 = userAccBusiness;
			line = 4;
			switch (MasterOrder.FactorType)
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
			case Document.Zayeat:
			{
				bool flag = userAccBusiness2.UserAccess.Zayeat.Zayeat_EditPrice;
				if (flag)
				{
					flag = await GetPriceVisibleAsync(token, tranName);
				}
				return flag;
			}
			case Document.EnteghalBeyneAnbar:
				return userAccBusiness2.UserAccess.HavaleBeyneAnbar.EditPrice;
			case Document.AnbarGardani:
				return userAccBusiness2.UserAccess.AnbarGardani.EditPrice;
			case Document.AmaniVorod:
				return userAccBusiness2.UserAccess.AmaniVarede.EditPrice;
			case Document.AmaniKhoroj:
				return userAccBusiness2.UserAccess.AmaniSadere.EditPrice;
			case Document.Tolid:
			case Document.Tolid_Mahsul:
				return userAccBusiness2.UserAccess.Tolid.EditPriceMahsul;
			case Document.Tolid_Mavad:
				return userAccBusiness2.UserAccess.Tolid.EditPriceMavad;
			default:
				return false;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "PriceEnableAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1386);
			throw;
		}
	}

	public async Task SetDiscountPercentAsync(EnSystem system, decimal value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_discount.SetPercentString(value.ToString(), RowSum + Discount) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (IsDisposed || system == EnSystem.Mapper)
			{
				return;
			}
			await UpdateVisitorPricesAsync(system, token);
			await UpdateTax_AvarexAsync(system, tranName, token);
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductNameForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductName"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductVahed"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Code"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PriceString"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSum"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RowSumWithTax"));
				}, "SetDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1421);
		}
	}

	private OrderDetailBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
		DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
		_visitor1 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		_visitor2 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		_visitor3 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		_visitor4 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		_discount = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		_tax = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
		_avarez = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
	}

	private OrderDetailBusiness(OrderBusiness order, GardeshTafsilBusiness item)
	{
		try
		{
			ContextGuid = order.ContextGuid;
			MasterOrder = order;
			if (item != null)
			{
				if (item.AsType != 9300 && item.AsType != 9400 && item.AsType != 9700 && item.AsType != 9800)
				{
					throw new ArgumentException("SanadType is Out of range" + item.TafsilItemName);
				}
				Description = item.RowDesc;
				if (item.DepotGuid.HasValue)
				{
					_depotGuid = item.DepotGuid.Value;
				}
				DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
				DigitAfterPoint = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
				_visitor1 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				_visitor2 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				_visitor3 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				_visitor4 = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				_discount = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				_tax = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				_avarez = new PercentagePropertyContainer(DigitForRound, DigitAfterPoint);
				VisitorPrice1 = item.Bazaryab;
			}
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1540);
			throw;
		}
	}

	public static async Task<OrderDetailBusiness> OrderDetailFactoryAsync(string contextGuid, OrderBusiness order, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("OrderDetailFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs"))
		{
			OrderDetailBusiness ret = new OrderDetailBusiness(contextGuid);
			if (order == null)
			{
				throw new NullReferenceException("order");
			}
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
				AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
				ret.ContextGuid = order.ContextGuid;
				ret._depotGuid = cashier?.DepotGuid ?? Guid.Empty;
				ret.MasterOrder = order;
				ret.IsTaxImpactOnCost = accGlobalSettings.Factors.IsTaxImpactOnCost;
				ret.OnRowSumChanged += ret.MasterOrder.Det_OnRowSumChanged;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "OrderDetailFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1559);
			}
			return ret;
		}
	}

	public static async Task<OrderDetailBusiness> OrderDetailFactoryAsync(string contextGuid, EnSystem system, OrderBusiness order, GardeshTafsilBusiness item, bool isLoadOriginalPrice, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 7;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			OrderDetailBusiness det = new OrderDetailBusiness(order, item);
			if (item.GetPrdGuid().HasValue)
			{
				await det.SetProductGuidAsync(system, item.GetPrdGuid().Value, isLoadOriginalPrice, "", token);
			}
			await det.SetV2QtAsync(system, item.QT2, token);
			await det.SetVGuidAsync(system, item.VGuid, token);
			await det.SetQuantityAsync(system, item.QT, token);
			await det.SetPriceAsync(system, item.Price, token);
			await det.SetDiscountAsync(system, item.Discount, token);
			det._depotGuid = cashier?.DepotGuid ?? Guid.Empty;
			det.IsTaxImpactOnCost = setting.Factors.IsTaxImpactOnCost;
			return det;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OrderDetailFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1579);
			throw;
		}
	}

	public override void Dispose(bool disposing)
	{
		checked
		{
			try
			{
				if (!_isDisposed)
				{
					_isDisposed = true;
					List<OrderDetailBusiness> list = (from p in MasterOrder.ListKala
						where p.Guid != Guid
						orderby p.Ordinal
						select p).ToList();
					for (int i = 0; i < list.Count(); i++)
					{
						list[i].Ordinal = i + 1;
					}
					MasterOrder = null;
					if (this.OnDisposed != null)
					{
						PacketParser.Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
					}
					if (this.OnRowSumChanged != null)
					{
						PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
					}
					_setDefaultQtToken?.Cancel();
					_setDefaultQtToken?.Dispose();
					_setDefaultQtToken = null;
					_loadDecimalToken?.Cancel();
					_loadDecimalToken?.Dispose();
					_loadDecimalToken = null;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1608);
			}
		}
	}

	public void ResetCahnges()
	{
		ChangeInDiscount = false;
		ChangeInPrice = false;
	}

	public async Task<List<ProductPriceRelationModel>> GetAllPricesAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		List<ProductPriceRelationModel> ret = new List<ProductPriceRelationModel>();
		try
		{
			ProductCompactBusiness prd = await GetPrdInfoAsync(system, token, tranName);
			if (prd == null)
			{
				return null;
			}
			if (MasterOrder.ChangeStatus == EntityChangeSt.Edited && Price > 0m)
			{
				ret.Add(new ProductPriceRelationModel
				{
					Guid = Guid.NewGuid(),
					PriceLevelGuid = ParentDefaults.PriceLevel.CurrentSaved,
					Price = Price,
					PriceLevelName = EnPriceType.CurrentSaved.GetDisplay().GetTranslate(),
					ProductGuid = prd.Guid,
					Ordinal = 18
				});
			}
			if (MasterOrder.FactorType == Document.OrderSell || MasterOrder.FactorType == Document.OrderBackSell || MasterOrder.FactorType == Document.PishFactor || MasterOrder.FactorType == Document.AmaniKhoroj)
			{
				IEnumerable<ProductPriceRelationModel> enumerable = await prd.GetAllPricesAsync(system, EnPriceLevelType.Sell, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					foreach (ProductPriceRelationModel item in enumerable)
					{
						if (item != null)
						{
							ret.Add(item);
						}
					}
				}
				ENSource source = ClsCache.GetCache(ContextGuid).Source;
				if (MasterOrder.GetTafsilGuid() != Guid.Empty)
				{
					if (source == ENSource.Accounting)
					{
						KardexBusiness kardexBusiness = await KardexBusiness.GetLastAsync(ContextGuid, MasterOrder.GetTafsilGuid(), ProductGuid, Document.OrderSell, token);
						ret.Add(new ProductPriceRelationModel
						{
							Guid = Guid.NewGuid(),
							PriceLevelGuid = ParentDefaults.PriceLevel.LastSellToTafsil,
							Price = (kardexBusiness?.UnitPrice ?? 0m),
							PriceLevelName = EnPriceType.LastSellToCus.GetDisplay().GetTranslate(),
							ProductGuid = prd.Guid,
							Ordinal = 10
						});
					}
					else
					{
						IEnumerable<LastPriceBusiness> enumerable2 = await LastPriceBusiness.GetAllAsync(ContextGuid, ProductGuid, MasterOrder?.TafsilGuid ?? Guid.Empty, MasterOrder?.FactorType ?? Document.None, token, tranName);
						ret.Add(new ProductPriceRelationModel
						{
							Guid = Guid.NewGuid(),
							PriceLevelGuid = ParentDefaults.PriceLevel.LastSellToTafsil,
							Price = (enumerable2?.SingleOrDefault()?.Price).GetValueOrDefault(),
							PriceLevelName = EnPriceType.LastSellToCus.GetDisplay().GetTranslate().GetTranslate(),
							ProductGuid = prd.Guid,
							Ordinal = 10
						});
					}
				}
				KardexBusiness kardexBusiness2 = await KardexBusiness.GetLastAsync(ContextGuid, null, ProductGuid, Document.OrderSell, token);
				ret.Add(new ProductPriceRelationModel
				{
					Guid = Guid.NewGuid(),
					PriceLevelGuid = ParentDefaults.PriceLevel.LastSellToAll,
					Price = (kardexBusiness2?.UnitPrice ?? 0m),
					PriceLevelName = EnPriceType.LastSellToAll.GetDisplay().GetTranslate(),
					ProductGuid = prd.Guid,
					Ordinal = 9
				});
			}
			else if (MasterOrder.FactorType == Document.OrderBuy || MasterOrder.FactorType == Document.OrderBackBuy || MasterOrder.FactorType == Document.AmaniVorod || MasterOrder.FactorType == Document.Zayeat || MasterOrder.FactorType == Document.Tolid_Mavad || MasterOrder.FactorType == Document.Tolid_Mahsul || MasterOrder.FactorType == Document.Tolid || MasterOrder.FactorType == Document.EnteghalBeyneAnbar || MasterOrder.FactorType == Document.AnbarGardani || MasterOrder.FactorType == Document.OrderPreBuy)
			{
				List<ProductPriceRelationModel> list = ret;
				ProductPriceRelationModel productPriceRelationModel = new ProductPriceRelationModel
				{
					Guid = Guid.NewGuid(),
					PriceLevelGuid = ParentDefaults.PriceLevel.BeginPeriod
				};
				ProductPriceRelationModel productPriceRelationModel2 = productPriceRelationModel;
				productPriceRelationModel2.Price = await ProductCompactBusiness.Avale2reAsync(ProductGuid);
				productPriceRelationModel.PriceLevelName = EnPriceType.Avale2re.GetDisplay().GetTranslate();
				productPriceRelationModel.ProductGuid = prd.Guid;
				productPriceRelationModel.Ordinal = 13;
				list.Add(productPriceRelationModel);
				string value = await KardexBusiness.GetLastBuyAsync(ContextGuid, ProductGuid, EnPriceType.BuyLast, null, Document.None, token);
				ret.Add(new ProductPriceRelationModel
				{
					Guid = Guid.NewGuid(),
					PriceLevelGuid = ParentDefaults.PriceLevel.AverageBuy,
					Price = value.ParseToDecimal(0m, DigitAfterPoint),
					PriceLevelName = EnPriceType.BuyAverage.GetDisplay().GetTranslate(),
					ProductGuid = prd.Guid,
					Ordinal = 14
				});
				string value2 = await KardexBusiness.GetLastBuyAsync(ContextGuid, ProductGuid, EnPriceType.BuyLastFromCus, MasterOrder.TafsilGuid, MasterOrder.FactorType, token);
				ret.Add(new ProductPriceRelationModel
				{
					Guid = Guid.NewGuid(),
					PriceLevelGuid = ParentDefaults.PriceLevel.LastBuyFromTafsil,
					Price = value2.ParseToDecimal(0m, DigitAfterPoint),
					PriceLevelName = EnPriceType.BuyLastFromCus.GetDisplay().GetTranslate(),
					ProductGuid = prd.Guid,
					Ordinal = 12
				});
				string value3 = await KardexBusiness.GetLastBuyAsync(ContextGuid, ProductGuid, EnPriceType.BuyLastFromAll, null, Document.None, token);
				ret.Add(new ProductPriceRelationModel
				{
					Guid = Guid.NewGuid(),
					PriceLevelGuid = ParentDefaults.PriceLevel.LastBuyFromAll,
					Price = value3.ParseToDecimal(0m, DigitAfterPoint),
					PriceLevelName = EnPriceType.BuyLastFromAll.GetDisplay().GetTranslate(),
					ProductGuid = prd.Guid,
					Ordinal = 11
				});
				IEnumerable<ProductPriceRelationModel> enumerable3 = await prd.GetAllPricesAsync(system, EnPriceLevelType.Buy, token, tranName);
				if (enumerable3 != null && enumerable3.Any())
				{
					foreach (ProductPriceRelationModel item2 in enumerable3)
					{
						if (item2 != null)
						{
							ret.Add(item2);
						}
					}
				}
			}
			return ret?.OrderBy((ProductPriceRelationModel q) => q.Ordinal)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllPricesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1759);
			throw;
		}
	}

	public async Task<List<DisplayMultiVahed>> LstMultiVahedAsync(EnSystem system = EnSystem.None, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("LstMultiVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs"))
		{
			List<DisplayMultiVahed> ret = new List<DisplayMultiVahed>();
			try
			{
				ProductCompactBusiness prd = await GetPrdInfoAsync(system, token);
				if (prd == null)
				{
					return ret;
				}
				ret.Add(new DisplayMultiVahed
				{
					Guid = prd.GuidVahed,
					Quantity = 1m,
					Name = ProductVahed
				});
				if (prd.V2Qt > 0m && prd.V2Guid.HasValue)
				{
					Guid? v2Guid = prd.V2Guid;
					if (v2Guid.HasValue)
					{
						if (string.IsNullOrEmpty(Unit2))
						{
							Unit2 = (await PrdVahedBusiness.GetAsync(ContextGuid, v2Guid.Value, token)).Name;
						}
						ret.Add(new DisplayMultiVahed
						{
							Guid = v2Guid.Value,
							Quantity = prd.V2Qt,
							Name = Unit2 + " - " + prd.V2Qt.ToString(3)
						});
					}
				}
				bool flag = await prd.GetV3QtAsync() > 0m;
				if (flag)
				{
					flag = ((await prd.GetV3GuidAsync()) ?? Guid.Empty) != Guid.Empty;
				}
				if (flag)
				{
					string contextGuid = ContextGuid;
					string v3Name2 = (await PrdVahedBusiness.GetAsync(contextGuid, (await prd.GetV3GuidAsync()).Value, token)).Name;
					List<DisplayMultiVahed> list = ret;
					DisplayMultiVahed displayMultiVahed = new DisplayMultiVahed();
					DisplayMultiVahed displayMultiVahed2 = displayMultiVahed;
					displayMultiVahed2.Guid = (await prd.GetV3GuidAsync(token)).Value;
					DisplayMultiVahed displayMultiVahed3 = displayMultiVahed;
					displayMultiVahed3.Quantity = await prd.GetV3QtAsync(token);
					DisplayMultiVahed displayMultiVahed4 = displayMultiVahed;
					contextGuid = v3Name2;
					displayMultiVahed4.Name = contextGuid + " - " + (await prd.GetV3QtAsync()).ToString(3);
					list.Add(displayMultiVahed);
				}
				flag = await prd.GetV4QtAsync() > 0m;
				if (flag)
				{
					flag = ((await prd.GetV4GuidAsync()) ?? Guid.Empty) != Guid.Empty;
				}
				if (flag)
				{
					string contextGuid = ContextGuid;
					string v3Name2 = (await PrdVahedBusiness.GetAsync(contextGuid, (await prd.GetV4GuidAsync()).Value, token)).Name;
					List<DisplayMultiVahed> list = ret;
					DisplayMultiVahed displayMultiVahed2 = new DisplayMultiVahed();
					DisplayMultiVahed displayMultiVahed = displayMultiVahed2;
					displayMultiVahed.Guid = (await prd.GetV4GuidAsync()).Value;
					DisplayMultiVahed displayMultiVahed4 = displayMultiVahed2;
					displayMultiVahed4.Quantity = await prd.GetV4QtAsync();
					DisplayMultiVahed displayMultiVahed3 = displayMultiVahed2;
					contextGuid = v3Name2;
					displayMultiVahed3.Name = contextGuid + " - " + (await prd.GetV4QtAsync()).ToString(3);
					list.Add(displayMultiVahed2);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LstMultiVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1814);
				throw;
			}
			return ret;
		}
	}

	public async Task SetQuantityStringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (value.EndsWith("/") || value.EndsWith("."))
			{
				_strQt = value;
			}
			else
			{
				_strQt = "";
			}
			await SetQuantityAsync(system, value.ParseToDecimal(0m, 3), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetQuantityStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1889);
			throw;
		}
	}

	public async Task SetQuantity2StringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			await SetV2QtAsync(system, value.ParseToDecimal(0m, 3), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetQuantity2StringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1902);
			throw;
		}
	}

	public async Task SetV2QtStringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			await SetV2QtAsync(system, value.ParseToDecimal(0m, 3), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetV2QtStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1913);
		}
	}

	public async Task<decimal> GetV2ZaribAsync()
	{
		int line = 0;
		try
		{
			line = 2;
			ProductCompactBusiness prd = _prdInfo;
			if (prd == null)
			{
				return default(decimal);
			}
			line = 10;
			if ((_vGuid ?? Guid.Empty) == Guid.Empty)
			{
				return default(decimal);
			}
			line = 20;
			if ((VGuid ?? Guid.Empty) == (prd?.V2Guid ?? Guid.Empty))
			{
				return prd.V2Qt;
			}
			line = 30;
			Guid guid = VGuid ?? Guid.Empty;
			if (guid == ((await prd.GetV3GuidAsync()) ?? Guid.Empty))
			{
				return await prd.GetV3QtAsync();
			}
			line = 40;
			guid = VGuid ?? Guid.Empty;
			if (guid == ((await prd.GetV4GuidAsync()) ?? Guid.Empty))
			{
				return await prd.GetV4QtAsync();
			}
			return 1m;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "GetV2ZaribAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1937);
		}
		return 1m;
	}

	public async Task SetPriceStringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			await SetPriceAsync(system, value?.ParseToDecimal(0m, DigitAfterPoint) ?? 0m, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPriceStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1950);
		}
	}

	public async Task SetDiscountPersentStringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (string.IsNullOrEmpty(value))
			{
				value = "0";
			}
			if (_discount.SetPercentString(value.ToString(), RowSum + Discount) && this.OnRowSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnRowSumChanged, this, new EventArgs(), token, 60, "SetDiscountPersentStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
			if (system == EnSystem.UserInterface)
			{
				await UpdateVisitorPricesAsync(system, token, tranName);
				await UpdateTax_AvarexAsync(system, tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDiscountPersentStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1978);
		}
	}

	public async Task SetDiscountStringAsync(EnSystem system, string value, CancellationToken token = default(CancellationToken))
	{
		try
		{
			_discount.SetPriceString(value, Quantity);
			await SetDiscountAsync(system, value.ParseToDecimal(0m, DigitAfterPoint), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDiscountStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 1999);
		}
	}

	internal async Task<decimal?> GetLastCostAsync(CancellationToken token, string tranName)
	{
		try
		{
			if (lastCost.GetValueOrDefault() > 0m)
			{
				return lastCost;
			}
			if (ProductGuid == Guid.Empty)
			{
				lastCost = null;
				return lastCost;
			}
			if (lastUnitCost == null)
			{
				lastUnitCost = await KardexBusiness.GetAsync(ContextGuid, ProductGuid, DepotGuid, MasterOrder.GetDate(), token, tranName);
			}
			if (lastUnitCost == null)
			{
				lastCost = null;
				return lastCost;
			}
			lastCost = (lastUnitCost?.RemUnitPrice).GetValueOrDefault();
			return lastCost;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastCostAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2065);
			throw;
		}
	}

	internal async Task<decimal> GetUnitLastCostAsync(CancellationToken token, string tranName)
	{
		try
		{
			CostPrice = (await GetLastCostAsync(token, tranName)).GetValueOrDefault();
			if (CostPrice < 0m)
			{
				CostPrice = 0m;
			}
			decimal num = Math.Round(CostPrice * Quantity, DigitForRound);
			if (lastUnitCost != null && num > lastUnitCost.RemTotalPrice.Value)
			{
				return lastUnitCost.RemTotalPrice.Value;
			}
			return num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUnitLastCostAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2083);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 1;
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs"))
		{
			string message = "";
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				ProductCompactBusiness prd = await GetPrdInfoAsync(system, token, tranName);
				int index = MasterOrder.ListKala.ToList().IndexOf(this);
				index = checked(index + 1);
				line = 3;
				if (prd == null)
				{
					ret.AddError("کالا انتخاب نشده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2101);
				}
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && !isDeleteAction && system == EnSystem.UserInterface && (MasterOrder.FactorType == Document.OrderSell || MasterOrder.FactorType == Document.PishFactor || MasterOrder.FactorType == Document.OrderBackSell))
				{
					decimal? num = await GetLastCostAsync(token, tranName);
					if (num.HasValue && Quantity > 0m && num.Value > RowSum / Quantity)
					{
						ret.AddReturnedValue(setting.Factors.Sell_LowerThanBahayeTamamShodeh, "قیمت فروش انتخاب شده برای کالای ردیف".GetTranslate() + $" {index}" + " از آخرین بهای تمام شده کالا".GetTranslate() + $" ({num:N0})" + " کمتر است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2108);
					}
				}
				if (!isDeleteAction && system == EnSystem.UserInterface && (MasterOrder.FactorType == Document.OrderSell || MasterOrder.FactorType == Document.PishFactor || MasterOrder.FactorType == Document.OrderBackSell))
				{
					line = 4;
					decimal num2 = await ProductCompactBusiness.LastVorodPriceAvgAsync(ContextGuid, ProductGuid, null, null, token, tranName);
					line = 5000;
					message += $"Quantity:{Quantity}//avgPrice:{num2}//Price:{Price}//Discount:{Discount}";
					if (Quantity > 0m && num2 > RowSum / Quantity)
					{
						line = 6;
						ret.AddReturnedValue(setting.Factors.Sell_LowerThanBahayeTamamShodeh_Med, "قیمت فروش انتخاب شده برای کالای ردیف".GetTranslate() + $" {index}" + " از میانگین بهای تمام شده کالا".GetTranslate() + " " + $" ({num2:N0}) " + " " + "کمتر است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2120);
					}
				}
				line = 7;
				if (system != EnSystem.Transfer && Discount > Quantity * Price)
				{
					line = 8;
					ret.AddError(system, "مبلغ تخفیف کالای".GetTranslate() + "  " + prd?.Name + " " + " در ردیف".GetTranslate() + " " + $" {Ordinal}" + " " + " نمیتواند از جمع بیشتر باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2128);
				}
				line = 9;
				if (TotalVisitorPrice > Quantity * Price)
				{
					line = 10;
					ret.AddError(system, "مبلغ بازاریابی کالای".GetTranslate() + "  " + prd?.Name + " " + " در ردیف".GetTranslate() + " " + $" {Ordinal}" + " " + " نمیتواند از جمع بیشتر باشد ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2135);
				}
				line = 13;
				if (TotalVisitorPrice > RowSum)
				{
					line = 14;
					ret.AddError(system, "مبلغ بازاریابی کالای ردیف".GetTranslate() + " " + $"{Ordinal}" + " " + " نمیتواند از جمع بیشتر باشد ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2142);
				}
				line = 15;
				if (TotalVisitorPrice > RowSum)
				{
					line = 16;
					ret.AddError(system, "مجموع مبالغ بازاریابی و تخفیف کالای ردیف".GetTranslate() + " " + $" {Ordinal}" + " " + " نمیتواند از جمع بیشتر باشد ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2149);
				}
				line = 17;
				if (system == EnSystem.UserInterface && MasterOrder.IsDuplicate(this))
				{
					line = 18;
					ret.AddReturnedValue(setting.Factors.PrdTekrari, prd?.Name + " " + " در ردیف".GetTranslate() + " " + $"{Ordinal}" + " " + " در فاکتور تکراری است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2156);
				}
				line = 19;
				if ((prd?.Guid ?? Guid.Empty) == Guid.Empty)
				{
					line = 20;
					ret.AddError(system, "هیچ کالایی در ردیف".GetTranslate() + " " + $" {Ordinal}" + " " + " انتخاب نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2163);
				}
				line = 21;
				if (Quantity <= 0m && system != EnSystem.Transfer)
				{
					line = 22;
					ret.AddError(system, "مقدار کالای ".GetTranslate() + " " + (prd?.Name ?? "") + " " + "در ردیف ".GetTranslate() + " " + $"{Ordinal}" + " " + "مشخص نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2170);
				}
				line = 23;
				if (Price <= 0m && system != EnSystem.Transfer)
				{
					line = 24;
					ret.AddError(system, "مبلغ کالای".GetTranslate() + "  " + prd?.Name + " " + " در ردیف".GetTranslate() + " " + $" {Ordinal} " + " " + "مشخص نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2177);
				}
				line = 25;
				if (await GetDepotGuidAsync() == Guid.Empty && system != EnSystem.Transfer)
				{
					line = 26;
					ret.AddError(system, "انبار کالای".GetTranslate() + " " + (prd?.Name ?? "") + " " + " در ردیف".GetTranslate() + " " + $" {Ordinal}" + " " + " مشخص نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2184);
				}
				line = 27;
				if (RowSum > 900000000000m && system != EnSystem.Transfer)
				{
					line = 28;
					ret.AddError(system, "مبلغ فاکتور برای کالای".GetTranslate() + "  " + prd?.Name + " " + " در ردیف".GetTranslate() + " " + $"{Ordinal}" + " " + " در  بیش از اندازه بزرگ است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2191);
				}
				line = 29;
				if ((MasterOrder.TaxTafsilGuid ?? Guid.Empty) == Guid.Empty && Tax > 0m && system != EnSystem.Transfer && !IsTaxImpactOnCost)
				{
					line = 30;
					ret.AddError(system, "ابتدا طرف حساب مالیات را از قسمت تنظیمات برنامه مشخص کنید ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2198);
				}
				line = 31;
				if ((MasterOrder.AvarezTafsilGuid ?? Guid.Empty) == Guid.Empty && Avarez > 0m && system != EnSystem.Transfer)
				{
					line = 32;
					ret.AddError(system, "ابتدا طرف حساب عوارض را از قسمت تنظیمات برنامه مشخص کنید ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2205);
				}
				line = 44;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (prd != null && !prd.Status && (MasterOrder?.FactorType ?? Document.None) != Document.PishFactor)
				{
					if (MasterOrder.FactorType == Document.OrderSell && system != EnSystem.Transfer)
					{
						switch (((await ((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName)) ?? throw new ArgumentNullException("cashier")).GetUserAsync(token, tranName)) ?? throw new ArgumentNullException("user")).UserAccess?.DisableProductInOrderSell ?? ReturnedState.None)
						{
						case ReturnedState.Error:
							ret.AddError("کالای".GetTranslate() + "  " + prd?.Name + " " + " در ردیف".GetTranslate() + " " + $" {Ordinal}" + " " + " غیر فعال است و شما امکان صدور فاکتور برای کالای غیرفعال را ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2220);
							break;
						case ReturnedState.Warning:
							ret.AddWarning("کالای".GetTranslate() + "  " + prd?.Name + " " + " در ردیف ".GetTranslate() + " " + $"{Ordinal}" + "غیر فعال است".GetTranslate(), "DisableProductInOrderSell", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2223);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await prd.UpdateStatusAsync(status: true, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2224);
							break;
						default:
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await prd.UpdateStatusAsync(status: true, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2226);
							break;
						}
					}
					else
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await prd.UpdateStatusAsync(status: true, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2229);
					}
				}
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (prd != null && system == EnSystem.UserInterface && !isDeleteAction && cache.Source != ENSource.Android_HesabYar && MasterOrder.KardexAffective)
				{
					if (await prd.GetIsSerialAsync(token, tranName) && !(SerialStr?.Any() ?? false))
					{
						ret.AddReturnedValue(setting.DepotSetting.SanadWithoutSerial, "کالای ".GetTranslate() + " " + prd?.Name + "  " + "سریال دار است اما سریال وارد نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2236);
					}
					if (!(await prd.GetIsSerialAsync()))
					{
						List<PrdSerialBusiness> list = SerialStr?.Where((PrdSerialBusiness q) => q.ProductGuid == ProductGuid)?.ToList();
						if (list != null && list.Any())
						{
							foreach (PrdSerialBusiness item in list)
							{
								RemoveSerial(item);
							}
						}
					}
					List<PrdSerialBusiness> list2 = SerialStr?.Where((PrdSerialBusiness q) => q.ProductGuid != ProductGuid)?.ToList();
					if (list2 != null && list2.Any())
					{
						foreach (PrdSerialBusiness item2 in list2)
						{
							RemoveSerial(item2);
						}
					}
					if (SerialStr?.Any() ?? false)
					{
						if ((decimal)SerialStr.Count != Quantity)
						{
							ret.AddReturnedValue(setting.DepotSetting.SanadWithoutSerial, "تعداد کالای".GetTranslate() + "  " + prd?.Name + " " + " فاکتور شده".GetTranslate() + " " + $" {Quantity.ToString().ParseToDecimal(0m, 3)}" + " " + " و تعداد سریال وارد شده".GetTranslate() + " " + $" {SerialStr.Count}" + " " + " منطبق نمی باشند".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2254);
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await PrdSerialBusiness.CheckValidationAsync(ContextGuid, _serialStr, system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2255);
					}
				}
				if (system == EnSystem.UserInterface && !isDeleteAction && Discount > 0m && ((prd?.DiscountStartDate.HasValue ?? false) || (prd?.DiscountEndDate.HasValue ?? false)) && ((MasterOrder?.FactorType ?? Document.None) == Document.OrderSell || (MasterOrder?.FactorType ?? Document.None) == Document.OrderBackSell))
				{
					DateTime date = MasterOrder.GetDate();
					if (prd.DiscountStartDate.Value > date || prd.DiscountEndDate < date)
					{
						ret.AddError("بازه مجاز تخفیف کالای " + prd.Name + " از " + prd.DiscountStartDate.MiladiToCalture() + " الی " + prd.DiscountEndDate.MiladiToCalture() + "  می باشد و در تاریخ جاری، مشمول تخفیف نمی گردد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2263);
					}
				}
				if (Discount <= 0m && !string.IsNullOrEmpty(SteppedDiscount))
				{
					_steppedDiscount = "";
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckDecimalValidationAsync(), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2268);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckGiftValidationAsync(), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2269);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CheckProductMetaDataValidationAsync(token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2270);
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Error in Line {line}" + "//message:" + message + "\r\n" + Json.ToStringJson(this, indented: true), "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2276);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2279);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckDecimalValidationAsync()
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (DecimalQt != 0m && (DecimalGuid ?? Guid.Empty) == Guid.Empty)
			{
				await ret.AddErrorAsync("ارز انتخابی نامعتبر می باشد".GetTranslate(), "", "CheckDecimalValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2296);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckDecimalValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2300);
			ret.AddReturnedValue(ex, "", "CheckDecimalValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2301);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckGiftValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((DicountCouponDetailGuid ?? Guid.Empty) == Guid.Empty || !(GiftDetailList?.Any() ?? false))
			{
				return res;
			}
			foreach (GiftDetailBusiness item in GiftDetailList)
			{
				if (item != null)
				{
					OrderDetailBusiness orderDetailBusiness = MasterOrder?.ListKala?.FirstOrDefault((OrderDetailBusiness q) => q.ProductGuid == item.ProductGuid && (q.DicountCouponDetailGuid ?? Guid.Empty) == Guid.Empty);
					if (orderDetailBusiness == null)
					{
						await res.AddErrorAsync("اشانتیون کالای " + ProductName + " در اقلام فاکتور وجود ندارد", "", "CheckGiftValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2317);
					}
					else if (orderDetailBusiness.Quantity != item.Count)
					{
						await res.AddErrorAsync("تعداد اشانتیون کالای " + ProductName + "(" + orderDetailBusiness.Quantity.NpToString("", 0) + ") با تعداد تعریف شده (" + item.Count.NpToString("", 0) + ") تطابق ندارد", "", "CheckGiftValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2318);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckGiftValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2323);
			res.AddReturnedValue(ex, "", "CheckGiftValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2324);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckProductMetaDataValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (ProductGuid == Guid.Empty)
			{
				return res;
			}
			IEnumerable<ProductMetaDataBusiness> enumerable = await ProductBusiness.GetMetaDataListAsync(ContextGuid, ProductGuid, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			if (MasterOrder.FactorType == Document.OrderSell)
			{
				ProductMetaDataBusiness productMetaDataBusiness = enumerable?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForSell);
				if (string.IsNullOrEmpty(productMetaDataBusiness?.Value))
				{
					return res;
				}
				if (productMetaDataBusiness != null && productMetaDataBusiness.Value.ParseToBoolean())
				{
					return res;
				}
				await res.AddErrorAsync("کالای " + ProductName + " مجوز فروش ندارد", "", "CheckProductMetaDataValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2342);
			}
			else if (MasterOrder.FactorType == Document.OrderBuy)
			{
				ProductMetaDataBusiness productMetaDataBusiness2 = enumerable?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForBuy);
				if (string.IsNullOrEmpty(productMetaDataBusiness2?.Value))
				{
					return res;
				}
				if (productMetaDataBusiness2 != null && productMetaDataBusiness2.Value.ParseToBoolean())
				{
					return res;
				}
				await res.AddErrorAsync("کالای " + ProductName + " مجوز خرید ندارد", "", "CheckProductMetaDataValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2349);
			}
			else if (MasterOrder.FactorType == Document.OrderBackBuy || MasterOrder.FactorType == Document.OrderBackSell)
			{
				ProductMetaDataBusiness productMetaDataBusiness3 = enumerable?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForReturn);
				if (string.IsNullOrEmpty(productMetaDataBusiness3?.Value))
				{
					return res;
				}
				if (productMetaDataBusiness3 != null && productMetaDataBusiness3.Value.ParseToBoolean())
				{
					return res;
				}
				await res.AddErrorAsync("کالای " + ProductName + " مجوز مرجوع شدن ندارد", "", "CheckProductMetaDataValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2356);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckProductMetaDataValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2361);
			res.AddReturnedValue(ex, "", "CheckProductMetaDataValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2362);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetPriceTypeAsync(EnSystem system, Guid priceLevelGuid, decimal? oldPrice = null, ReturnedState? state = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			bool addWarning = state.HasValue && (state.Value & ReturnedState.Warning) == ReturnedState.Warning;
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
			ProductCompactBusiness prd = await GetPrdInfoAsync(system, token, tranName);
			if (prd == null)
			{
				return await res.AddErrorAsync("Product is Null !!", "", "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2374);
			}
			decimal num = await prd.GetSellAsync(system, priceLevelGuid, setting.Sells.AddOriginalPriceToOrder, token, tranName);
			if (num <= 0m)
			{
				return res;
			}
			if (addWarning && oldPrice.HasValue && num != oldPrice.Value)
			{
				await res.AddWarningAsync("قیمت کالای".GetTranslate() + "  " + prd.Name + " " + " از".GetTranslate() + " " + $" {oldPrice.Value:N0}" + " " + " به".GetTranslate() + " " + $" {num:N0}" + " " + " تغییر می کند. آیا ادامه می دهید؟".GetTranslate(), "", "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2378);
			}
			else
			{
				await SetPriceAsync(system, num);
				ProductPriceLevelBusiness productPriceLevelBusiness = await ProductPriceLevelBusiness.GetAsync(ContextGuid, priceLevelGuid, system, token, tranName);
				decimal num2 = prd?.DiscountPercent ?? 0m;
				num2 *= productPriceLevelBusiness?.DiscountCoefficient ?? 0m;
				if (num2 != 0m)
				{
					await SetDiscountPercentAsync(system, num2);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2391);
			res.AddReturnedValue(ex, "", "SetPriceTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2392);
		}
		return res;
	}

	public bool GetAvarezEnable(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ProductCompactBusiness prdInfo = _prdInfo;
		bool num;
		if (prdInfo == null)
		{
			if (Avarez > 0m)
			{
				goto IL_003b;
			}
			num = Tax > 0m;
		}
		else
		{
			num = prdInfo.IsTax;
		}
		if (!num)
		{
			return false;
		}
		goto IL_003b;
		IL_003b:
		if ((MasterOrder?.FactorType ?? Document.None) != Document.OrderSell && (MasterOrder?.FactorType ?? Document.None) != Document.OrderBackSell && (MasterOrder?.FactorType ?? Document.None) != Document.OrderBuy && (MasterOrder?.FactorType ?? Document.None) != Document.OrderBackBuy && (MasterOrder?.FactorType ?? Document.None) != Document.PishFactor)
		{
			return (MasterOrder?.FactorType ?? Document.None) == Document.OrderPreBuy;
		}
		return true;
	}

	public async Task SetCodeAsync(string value, EnSystem system, bool isLoadOriginalPrice, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			_prdCode = value;
			Guid guid = (await ProductCompactBusiness.GetAsync(ContextGuid, value, system, true, "", null, token))?.Guid ?? Guid.Empty;
			if (guid == Guid.Empty && !string.IsNullOrEmpty(value))
			{
				guid = (await BarcodeBusiness.GetAsync(ContextGuid, value, token))?.PrdGuid ?? Guid.Empty;
			}
			await SetProductGuidAsync(system, guid, isLoadOriginalPrice, "", token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#IsDisposed_{IsDisposed}", "SetCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2518);
			throw;
		}
	}

	public static async Task<OrderDetailBusiness> GetOrderDetailBusinessAsync(string contextGuid, OrderBusiness masterOrder, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		OrderDetailBusiness ret = await OrderDetailFactoryAsync(contextGuid, masterOrder, token, tranName);
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (masterOrder.FactorType == Document.None)
			{
				throw new Exception();
			}
			ret.MasterOrder = masterOrder;
			ret.ContextGuid = contextGuid;
			ret._depotGuid = cashierBusiness?.DepotGuid ?? Guid.Empty;
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderDetailBusinessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2540);
			throw;
		}
	}

	public string QtWithVahedPrint()
	{
		string text = "";
		text = text + QuantityString + ProductVahed;
		if (V2Qt.HasValue)
		{
			text = text + "<br/> " + V2QtString + Unit2;
		}
		return text;
	}

	public async Task<Guid?> GetProductTafsilGuidAsync(CancellationToken token)
	{
		try
		{
			if ((_productTafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				return _productTafsilGuid;
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (accGlobalSettings.CompanyInfo.SanadType == 1)
			{
				switch (MasterOrder.FactorType)
				{
				case Document.OrderSell:
					_productTafsilGuid = accGlobalSettings.CodeSetting.SellCoding.Credit_TafsilGuid_Product;
					break;
				case Document.OrderBackSell:
					_productTafsilGuid = accGlobalSettings.CodeSetting.BackSellCoding.Debit_TafsilGuid_Product;
					break;
				default:
					_productTafsilGuid = accGlobalSettings.CodeSetting.Permanent.ProductTafsilGuid;
					return _productTafsilGuid;
				}
			}
			else
			{
				switch (MasterOrder.FactorType)
				{
				case Document.OrderSell:
					_productTafsilGuid = accGlobalSettings.CodeSetting.SellCoding.Credit_TafsilGuid_Product;
					break;
				case Document.OrderBuy:
					_productTafsilGuid = accGlobalSettings.CodeSetting.BuyCoding.Debit_TafsilGuid_Product;
					break;
				case Document.OrderBackBuy:
					_productTafsilGuid = accGlobalSettings.CodeSetting.BackBuyCoding.Credit_TafsilGuid_Product;
					break;
				case Document.OrderBackSell:
					_productTafsilGuid = accGlobalSettings.CodeSetting.BackSellCoding.Debit_TafsilGuid_Product;
					break;
				case Document.Zayeat:
					_productTafsilGuid = accGlobalSettings.CodeSetting.ZayeatCoding.Credit_TafsilGuid_Product;
					break;
				case Document.AmaniKhoroj:
					_productTafsilGuid = MasterOrder.TafsilGuid;
					break;
				case Document.AmaniVorod:
					_productTafsilGuid = MasterOrder.TafsilGuid;
					break;
				default:
					_productTafsilGuid = accGlobalSettings.CodeSetting.SellCoding.Credit_TafsilGuid_Product;
					return _productTafsilGuid;
				}
			}
			return _productTafsilGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetProductTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2651);
			throw;
		}
	}

	public Task SetSerialzedDataAsync(OrderDetailSerializedData mode)
	{
		try
		{
			_metaData = mode;
			if (mode.AvarezMode == PercentageMode.None)
			{
				mode.AvarezMode = PercentageMode.Percent;
			}
			if (mode.DiscountMode == PercentageMode.None)
			{
				mode.DiscountMode = PercentageMode.Percent;
			}
			if (mode.TaxMode == PercentageMode.None)
			{
				mode.TaxMode = PercentageMode.Percent;
			}
			if (mode.Visitor1Mode == PercentageMode.None)
			{
				mode.Visitor1Mode = PercentageMode.Percent;
			}
			if (mode.Visitor2Mode == PercentageMode.None)
			{
				mode.Visitor2Mode = PercentageMode.Percent;
			}
			if (mode.Visitor3Mode == PercentageMode.None)
			{
				mode.Visitor3Mode = PercentageMode.Percent;
			}
			if (mode.Visitor4Mode == PercentageMode.None)
			{
				mode.Visitor4Mode = PercentageMode.Percent;
			}
			_avarez.SetPercentageMode(mode.AvarezMode, Quantity, RowSum);
			_discount.SetPercentageMode(mode.DiscountMode, Quantity, RowSum + Discount);
			_visitor1.SetPercentageMode(mode.Visitor1Mode, Quantity, RowSum);
			_visitor2.SetPercentageMode(mode.Visitor2Mode, Quantity, RowSum);
			_visitor3.SetPercentageMode(mode.Visitor3Mode, Quantity, RowSum);
			_visitor4.SetPercentageMode(mode.Visitor4Mode, Quantity, RowSum);
			_tax.SetPercentageMode(mode.TaxMode, Quantity, RowSum, EnRoundDecimal.Floor);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSerialzedDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2676);
		}
		return Task.CompletedTask;
	}

	private async Task<decimal> GetDefaultDiscountPercentAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (MasterOrder.FactorType != Document.OrderSell && MasterOrder.FactorType != Document.OrderBackSell && MasterOrder.FactorType != Document.PishFactor)
			{
				return default(decimal);
			}
			DateTime date = MasterOrder.GetDate();
			await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			ProductCompactBusiness prd = await GetPrdInfoAsync(system, token, tranName);
			if (prd == null || (PriceTypeGuid ?? Guid.Empty) == Guid.Empty)
			{
				return default(decimal);
			}
			ProductPriceLevelBusiness productPriceLevelBusiness = await ProductPriceLevelBusiness.GetAsync(ContextGuid, PriceTypeGuid.Value, system, token, tranName);
			if (productPriceLevelBusiness == null)
			{
				return prd?.DiscountPercent ?? 0m;
			}
			decimal discountCoefficient = productPriceLevelBusiness.DiscountCoefficient;
			if (_prdInfo.DiscountStartDate.HasValue && _prdInfo.DiscountEndDate.HasValue && (_prdInfo.DiscountStartDate.Value > date || _prdInfo.DiscountEndDate < date))
			{
				return default(decimal);
			}
			return Math.Round(discountCoefficient * prd.DiscountPercent, DigitForRound);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDefaultDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2699);
		}
		return default(decimal);
	}

	public void SetOrdinalChange(EnSystem system, int value, object sender, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!IsDisposed)
			{
				if (value < 0)
				{
					throw new ArgumentException($"value({value}) is not valid for SanadDetail.Ordinal");
				}
				int ordinal = Ordinal;
				Ordinal = value;
				if (system == EnSystem.UserInterface && Ordinal >= 0 && this.OnOrdinalChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, sender, new OrdinalChangedEventArgs(ordinal, value, Guid, system), token, 60, "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetOrdinalChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2714);
		}
	}

	public async Task<EnOrderDetailType> GetOrderDetailTypeAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			if (token.IsCancellationRequested || IsDisposed)
			{
				return EnOrderDetailType.None;
			}
			_ = Quantity;
			List<ProductsChanges> list = await MasterOrder.GetLstProductsAnbarChangesAsync(system, token, tranName, isDeleteAction: false);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return EnOrderDetailType.None;
			}
			ProductsChanges prdChange = list?.FirstOrDefault((ProductsChanges q) => q.PrdGuid == ProductGuid);
			if (ProductGuid == Guid.Empty)
			{
				return EnOrderDetailType.None;
			}
			if (token.IsCancellationRequested || IsDisposed)
			{
				return EnOrderDetailType.None;
			}
			string contextGuid = ContextGuid;
			EnSystem system2 = system;
			Guid productGuid = ProductGuid;
			PrdAnbarRelationBusiness prdAnbarRelationBusiness = await PrdAnbarRelationBusiness.GetAsync(contextGuid, system2, productGuid, await GetDepotGuidAsync(token), token, tranName);
			if (token.IsCancellationRequested || IsDisposed)
			{
				return EnOrderDetailType.None;
			}
			decimal num = ((prdChange == null) ? ((prdAnbarRelationBusiness?.Amount ?? 0m) - Quantity) : ((prdAnbarRelationBusiness?.Amount ?? 0m) + prdChange.qt));
			if (num < 0m)
			{
				return EnOrderDetailType.NegativeAmount;
			}
			if ((prdAnbarRelationBusiness?.Min ?? 0m) > (prdAnbarRelationBusiness?.Amount ?? 0m) + (prdChange?.qt ?? 0m) || ((prdAnbarRelationBusiness?.Min ?? 0m) != 0m && (prdAnbarRelationBusiness?.Amount ?? 0m) + (prdChange?.qt ?? 0m) == 0m))
			{
				return EnOrderDetailType.UnderOrderPoint;
			}
			return EnOrderDetailType.None;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetOrderDetailTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2739);
			throw;
		}
	}

	public async Task UpdateUiAsync()
	{
		try
		{
			_prdInfo = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, EnSystem.UserInterface, null, "", default(CancellationToken), true);
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductNameForDisplay"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductName"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductVahed"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Code"));
				}, "UpdateUiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateUiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2757);
		}
	}

	public static Task<Guid> GetMasterGuidAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IOrderDetail.GetMasterGuidAsync(guid, token, tranName);
	}

	public async Task SetSteppedDiscountAsync(string value, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (_steppedDiscount == value)
			{
				return;
			}
			_steppedDiscount = value;
			if (system == EnSystem.Mapper || system == EnSystem.None || string.IsNullOrEmpty(SteppedDiscount))
			{
				return;
			}
			List<string> list = SteppedDiscount.Split(new char[1] { '+' }).ToList();
			if (list == null || !list.Any())
			{
				return;
			}
			decimal discount = default(decimal);
			foreach (string item in list)
			{
				discount += Math.Round(RowSumWithTax * item.ParseToDecimal(0m, 3) / 100m);
				await SetDiscountAsync(system, discount, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetSteppedDiscountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\OrderDetailBusiness.cs", 2777);
		}
	}
}
