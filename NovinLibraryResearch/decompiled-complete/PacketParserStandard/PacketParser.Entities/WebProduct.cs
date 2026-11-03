using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Product;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;

namespace PacketParser.Entities;

[Serializable]
public class WebProduct : Serializable<WebProduct>, IProduct, IProductCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, ISyncFromClient2Server<WebProduct>, IDeliverableEntity, IEditableDelivery, INpDisposable, IDisposable
{
	private const int ChunckSizeKB = 100;

	private bool _isDisposed;

	private decimal _BAZARYABPERCENT;

	private decimal _Discount_Percent;

	private decimal _BPercent2;

	private decimal _BPercent3;

	private decimal _BPercent4;

	private string _v2Name;

	private string _name;

	private List<WebBarcode> barcodes;

	private List<Guid> types;

	private decimal _buy1;

	private decimal _buy2;

	private decimal _buy3;

	private decimal _sell1;

	private decimal _sell2;

	private decimal _sell3;

	private decimal _sell4;

	private decimal _sell5;

	private decimal _sell6;

	private decimal _sell7;

	private decimal _sell8;

	public int Id { get; set; }

	public EnProductType Type { get; set; }

	[Obsolete("Use PriceRoleList", true)]
	public List<ProductPrice> PriceList
	{
		get
		{
			return PriceRoleList;
		}
		set
		{
			if (value != null && value.Any())
			{
				PriceRoleList = value;
			}
		}
	}

	public List<ProductPrice> PriceRoleList { get; set; }

	public List<WebProductPriceRelation> Prices { get; set; }

	public List<WebProductGroupRelation> GroupList { get; set; }

	public string IdProduct { get; set; }

	[Obsolete]
	public string VahedName { get; set; }

	public Guid? TaxGuid { get; set; }

	public DateTime? DiscountStartDate { get; set; }

	public DateTime? DiscountEndDate { get; set; }

	public decimal DiscountPrice { get; set; }

	public short VahedId { get; set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value.FixString();
		}
	}

	public string Description { get; set; }

	[Obsolete("please use GuidGroup")]
	public short IdGroup { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[Obsolete("Use Prices")]
	public decimal Buy1
	{
		get
		{
			return _buy1;
		}
		set
		{
			_buy1 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Buy2
	{
		get
		{
			return _buy2;
		}
		set
		{
			_buy2 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Buy3
	{
		get
		{
			return _buy3;
		}
		set
		{
			_buy3 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell1
	{
		get
		{
			return _sell1;
		}
		set
		{
			_sell1 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell2
	{
		get
		{
			return _sell2;
		}
		set
		{
			_sell2 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell3
	{
		get
		{
			return _sell3;
		}
		set
		{
			_sell3 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell4
	{
		get
		{
			return _sell4;
		}
		set
		{
			_sell4 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell5
	{
		get
		{
			return _sell5;
		}
		set
		{
			_sell5 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell6
	{
		get
		{
			return _sell6;
		}
		set
		{
			_sell6 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell7
	{
		get
		{
			return _sell7;
		}
		set
		{
			_sell7 = value.FixDecimal(18, 3);
		}
	}

	[Obsolete("Use Prices")]
	public decimal Sell8
	{
		get
		{
			return _sell8;
		}
		set
		{
			_sell8 = value.FixDecimal(18, 3);
		}
	}

	public string Pic { get; set; }

	public decimal Height { get; set; }

	public decimal Width { get; set; }

	public decimal Length { get; set; }

	public string TaxCode { get; set; }

	public string TaxName { get; set; }

	public EnProductIndex IndexingType { get; set; }

	public List<Guid> Types
	{
		get
		{
			if (types != null && types.Any() && types.Count((Guid c) => c != Guid) == 0)
			{
				types.Add(Guid);
			}
			return types;
		}
		set
		{
			types = value;
		}
	}

	public decimal Weight { get; set; }

	public string T1 { get; set; }

	public string T2 { get; set; }

	public string T3 { get; set; }

	public string T4 { get; set; }

	public string T5 { get; set; }

	public string T6 { get; set; }

	public string T7 { get; set; }

	public string T8 { get; set; }

	public ProductMojodiSt Mojodi { get; set; } = ProductMojodiSt.DepotDefault;


	public decimal BPercent1
	{
		get
		{
			if (_BAZARYABPERCENT < 0m || _BAZARYABPERCENT > 100m)
			{
				_BAZARYABPERCENT = default(decimal);
			}
			return _BAZARYABPERCENT;
		}
		set
		{
			_BAZARYABPERCENT = value;
		}
	}

	public decimal DiscountPercent
	{
		get
		{
			if (_Discount_Percent < 0m || _Discount_Percent > 100m)
			{
				_Discount_Percent = default(decimal);
			}
			return _Discount_Percent;
		}
		set
		{
			_Discount_Percent = value;
		}
	}

	public PayementType PayementType { get; set; }

	public string GroupName { get; set; }

	public decimal Amount { get; set; }

	public bool IsTax { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal AvarezPercent { get; set; }

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public string FullContent { get; set; } = "";


	public string QuickText { get; set; } = "";


	public bool ScaleSt { get; set; }

	public decimal BPercent2
	{
		get
		{
			if (_BPercent2 < 0m || _BPercent2 > 100m)
			{
				_BPercent2 = default(decimal);
			}
			return _BPercent2;
		}
		set
		{
			_BPercent2 = value;
		}
	}

	public decimal BPercent3
	{
		get
		{
			if (_BPercent3 < 0m || _BPercent3 > 100m)
			{
				_BPercent3 = default(decimal);
			}
			return _BPercent3;
		}
		set
		{
			_BPercent3 = value;
		}
	}

	public decimal BPercent4
	{
		get
		{
			if (_BPercent4 > 100m || _BPercent4 < 0m)
			{
				_BPercent4 = default(decimal);
			}
			return _BPercent4;
		}
		set
		{
			_BPercent4 = value;
		}
	}

	public long Code { get; set; }

	public short V2Id { get; set; }

	public short V3Id { get; set; }

	public short V4Id { get; set; }

	public decimal V2Qt { get; set; }

	public decimal V3Qt { get; set; }

	public decimal V4Qt { get; set; }

	public string Keywords { get; set; } = "";


	public string MetaDesc { get; set; } = "";


	public string OtherPics { get; set; } = "";


	public decimal Score { get; set; }

	public decimal Sell1Qt { get; set; }

	public decimal Sell2Qt { get; set; }

	public decimal Sell3Qt { get; set; }

	public decimal Sell4Qt { get; set; }

	public decimal Sell5Qt { get; set; }

	public decimal Sell6Qt { get; set; }

	public decimal Sell7Qt { get; set; }

	public decimal Sell8Qt { get; set; }

	public bool Special { get; set; }

	public decimal SellSood1 { get; set; }

	public decimal SellSood2 { get; set; }

	public decimal SellSood3 { get; set; }

	public decimal SellSood4 { get; set; }

	public decimal SellSood5 { get; set; }

	public decimal SellSood6 { get; set; }

	public decimal SellSood7 { get; set; }

	public decimal SellSood8 { get; set; }

	public decimal SellKasr1 { get; set; }

	public decimal SellKasr2 { get; set; }

	public decimal SellKasr3 { get; set; }

	public decimal SellKasr4 { get; set; }

	public decimal SellKasr5 { get; set; }

	public decimal SellKasr6 { get; set; }

	public decimal SellKasr7 { get; set; }

	public decimal SellKasr8 { get; set; }

	public bool IsSerial { get; set; }

	public decimal BuyLast { get; set; }

	public DateTime CreateDate { get; set; } = DateTime.Now;


	public decimal KardexPrice { get; set; }

	public bool AutoSellSt { get; set; }

	public List<WebPrdAnbarRelation> PrdAnbarRelation { get; set; }

	public List<WebPrdTechnical> PrdTechnicalList { get; set; }

	public List<WebBarcode> PrdBarcode
	{
		get
		{
			return barcodes?.ToList();
		}
		set
		{
			barcodes = value;
		}
	}

	[Obsolete("Use Group Relation List")]
	public Guid GuidGroup { get; set; }

	public Guid GuidVahed { get; set; }

	public Guid? V2Guid { get; set; }

	public Guid? V3Guid { get; set; }

	public List<string> ImageListData { get; set; }

	public Guid? V4Guid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime ResetTime { get; set; }

	public string Tags { get; set; }

	[JsonIgnore]
	public DateTime? DeliveryDate { get; set; }

	[JsonIgnore]
	public int DeliveryId { get; set; }

	[JsonIgnore]
	public string DeliveryMessage { get; set; } = "";


	[JsonIgnore]
	public string DeliveryShortMessage { get; set; } = "";


	[JsonIgnore]
	public string HostRefrenced { get; set; } = "";


	public int Version { get; set; }

	public int ClientVersion { get; set; }

	public string MappId { get; set; }

	public List<Guid> ProductAttributeGuids { get; set; }

	public string Sku { get; set; }

	public string Slug { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public Guid? DefaultVariable { get; set; }

	public List<WebProductAttributeRelation> AttributeRelations { get; set; }

	public List<WebProductAttribute> RelatedAttributes { get; set; }

	public string NameForDisplay { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public string FullTextIndexOnAllFields => Name + " " + Description + " " + IdProduct + "  " + T1 + " " + T2 + " " + T3 + " " + T4 + " " + T5 + " " + T6 + " " + T7 + " " + T8 + " ";

	[JsonIgnore]
	[XmlIgnore]
	public string FullTextIndexOnNameAndCodeFields => Name + " " + IdProduct;

	[JsonIgnore]
	[XmlIgnore]
	public decimal OrderPoint { get; set; }

	public string TechnicalInfoTitle1 { get; set; }

	public string TechnicalInfoTitle2 { get; set; }

	public string TechnicalInfoTitle3 { get; set; }

	public string TechnicalInfoTitle4 { get; set; }

	public string TechnicalInfoTitle5 { get; set; }

	public string TechnicalInfoTitle6 { get; set; }

	public string TechnicalInfoTitle7 { get; set; }

	public string TechnicalInfoTitle8 { get; set; }

	public DateTime? SendToServerDate { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public Guid? MasterGuid { get; set; }

	public short PartitionId { get; set; }

	public decimal RegularPrice { get; set; }

	public EnProductKind Kind { get; set; }

	public bool IsDependentOnCurrency { get; set; }

	public WebPrdVahed Unit { get; set; }

	public string SeoDescription { get; set; }

	public string SeoTitle { get; set; }

	public string SeoFocusKeyword { get; set; }

	public event Func<object, ConnectionEventArgs<ProductReadEventArgs>, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebProduct>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebProduct>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebProduct>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeletedRequest;

	public decimal GetPrice(EnPriceType level, bool WebSiteDecimalIsToman, WooSyncProductSettings sett, AccGlobalSettings accSetting, bool applyDiscount)
	{
		decimal num = 1m;
		num *= DiscountPercent;
		DateTime now = DateTime.Now;
		DateTime? discountStartDate = DiscountStartDate;
		if (!discountStartDate.HasValue || !(now < discountStartDate.GetValueOrDefault()) || !DiscountStartDate.HasValue)
		{
			now = DateTime.Now;
			discountStartDate = DiscountEndDate;
			if (!discountStartDate.HasValue || !(now > discountStartDate.GetValueOrDefault()) || !DiscountEndDate.HasValue)
			{
				goto IL_008b;
			}
		}
		num = default(decimal);
		goto IL_008b;
		IL_008b:
		if (num > 100m || num < 0m)
		{
			num = default(decimal);
		}
		return level switch
		{
			EnPriceType.sell2 => ApplyDiscount(WebSiteDecimalIsToman, Sell2, num, DiscountPrice, applyDiscount), 
			EnPriceType.sell3 => ApplyDiscount(WebSiteDecimalIsToman, Sell3, num, DiscountPrice, applyDiscount), 
			EnPriceType.sell4 => ApplyDiscount(WebSiteDecimalIsToman, Sell4, num, DiscountPrice, applyDiscount), 
			EnPriceType.sell5 => ApplyDiscount(WebSiteDecimalIsToman, Sell5, num, DiscountPrice, applyDiscount), 
			EnPriceType.sell6 => ApplyDiscount(WebSiteDecimalIsToman, Sell6, num, DiscountPrice, applyDiscount), 
			EnPriceType.sell7 => ApplyDiscount(WebSiteDecimalIsToman, Sell7, num, DiscountPrice, applyDiscount), 
			EnPriceType.sell8 => ApplyDiscount(WebSiteDecimalIsToman, Sell8, num, DiscountPrice, applyDiscount), 
			EnPriceType.Regular => ApplyDiscount(WebSiteDecimalIsToman, RegularPrice, num, DiscountPrice, applyDiscount), 
			_ => ApplyDiscount(WebSiteDecimalIsToman, Sell1, num, DiscountPrice, applyDiscount), 
		};
	}

	public decimal GetPrice(bool WebSiteDecimalIsToman, EnPriceType level, decimal discountPercent, decimal discountPrice, bool applyDiscount)
	{
		return level switch
		{
			EnPriceType.sell2 => ApplyDiscount(WebSiteDecimalIsToman, Sell2, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.sell3 => ApplyDiscount(WebSiteDecimalIsToman, Sell3, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.sell4 => ApplyDiscount(WebSiteDecimalIsToman, Sell4, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.sell5 => ApplyDiscount(WebSiteDecimalIsToman, Sell5, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.sell6 => ApplyDiscount(WebSiteDecimalIsToman, Sell6, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.sell7 => ApplyDiscount(WebSiteDecimalIsToman, Sell7, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.sell8 => ApplyDiscount(WebSiteDecimalIsToman, Sell8, discountPercent, discountPrice, applyDiscount), 
			EnPriceType.Regular => ApplyDiscount(WebSiteDecimalIsToman, RegularPrice, discountPercent, discountPrice, applyDiscount), 
			_ => ApplyDiscount(WebSiteDecimalIsToman, Sell1, discountPercent, discountPrice, applyDiscount), 
		};
	}

	private static decimal ApplyDiscount(bool WebSiteDecimalIsToman, decimal sellPrice, decimal discountPercent, decimal discountPrice, bool applyDiscount)
	{
		if (applyDiscount)
		{
			if (discountPercent > 0m && discountPrice > 0m)
			{
				throw new InvalidOperationException("Just one of these two items must be set: discountPercent or discountPrice");
			}
			if (discountPercent < 0m)
			{
				throw new InvalidOperationException($"value({discountPercent}) is not valid for discountPercent.");
			}
			if (discountPrice < 0m)
			{
				throw new InvalidOperationException($"value({discountPrice}) is not valid for discountPrice.");
			}
		}
		if (WebSiteDecimalIsToman)
		{
			if (!applyDiscount)
			{
				return sellPrice / 10m;
			}
			if (discountPercent <= 0m || discountPercent > 100m)
			{
				return sellPrice / 10m;
			}
			if (discountPrice > 0m)
			{
				return Math.Round((sellPrice - discountPrice) / 10m, 0);
			}
			return Math.Round(sellPrice / 10m - sellPrice / 10m * discountPercent / 100m, 0);
		}
		if (!applyDiscount)
		{
			return sellPrice;
		}
		if ((discountPercent <= 0m || discountPercent > 100m) && discountPrice <= 0m)
		{
			return sellPrice;
		}
		if (discountPrice > 0m)
		{
			return Math.Round(sellPrice - discountPrice, 0);
		}
		return Math.Round(sellPrice - sellPrice * discountPercent / 100m, 0);
	}

	public static Packet GenerateReadRequest(List<Guid> ProductGuids, DateTime? modified = null, List<Guid> GroupGuids = null)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				List<ProductReadEventArgs> list2 = new List<ProductReadEventArgs>
				{
					new ProductReadEventArgs
					{
						Modified = modified,
						ProductGuids = ProductGuids,
						GroupGuids = GroupGuids
					}
				};
				list.Add(list2.ToString());
				return new Packet(PacketType.Product, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 471);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebProduct> items, int partitionId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
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
					result = items.GeneratePackets(PacketType.Product, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 486);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> products, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			try
			{
				if (products == null || (products != null && products.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (products == null)
				{
					result = null;
				}
				else
				{
					result = products.GeneratePackets(PacketType.Product, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 501);
				throw;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebProduct> products, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			try
			{
				if (products == null || (products != null && products.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (products == null)
				{
					result = null;
				}
				else
				{
					result = products.GeneratePackets(PacketType.Product, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 515);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebProduct> products, int partitionId, bool deliveryRequest = true)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			if (products == null || !products.Any())
			{
				throw new NullReferenceException("products");
			}
			try
			{
				string text = products.Select((WebProduct p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (products == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					bool deliveryRequst = deliveryRequest;
					result = products.GeneratePackets(PacketType.Product, 1, null, isSync: false, null, partitionId, deliveryRequst, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 531);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			if (responses == null || (responses != null && responses.Count() == 0))
			{
				throw new NullReferenceException();
			}
			try
			{
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.Product, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 545);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebProduct> products)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			try
			{
				object result;
				if (products == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = $"Count:{products?.Count()}";
					result = products.GeneratePackets(PacketType.Product, 7, null, isSync: false, null, 0, deliveryRequst: true, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 556);
			}
			return null;
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			ProductActions productActions = (ProductActions)action;
			return productActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 570);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((ProductActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 583);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs"))
		{
			_ = 5;
			try
			{
				switch ((ProductActions)pack.PacketActions)
				{
				case ProductActions.SaveRequest:
				{
					if (this.OnSaveRequest == null)
					{
						throw new Exception("there is no eventHandller for WebProduct.OnSaveRequest");
					}
					ConnectionEventArgs<WebProduct> args2 = new ConnectionEventArgs<WebProduct>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs");
					break;
				}
				case ProductActions.SaveResponse:
				{
					if (this.OnSaveResponse == null)
					{
						throw new Exception("there is no eventHandller for WebProduct.SaveResponse");
					}
					SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs");
					break;
				}
				case ProductActions.ReadRequest:
				{
					if (this.OnReadRequest == null)
					{
						throw new Exception("there is no eventHandller for WebProduct.ReadRequest");
					}
					ConnectionEventArgs<ProductReadEventArgs> connectionEventArgs = new ConnectionEventArgs<ProductReadEventArgs>(pack, connectionUniqueId, connection);
					connectionEventArgs.ConnectionData = pack.Args[1].FromJson<List<ProductReadEventArgs>>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 610);
					await Utilities.RaiseEvent(this.OnReadRequest, this, connectionEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs");
					break;
				}
				case ProductActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						throw new Exception("there is no eventHandller for WebProduct.ReadResponse");
					}
					ConnectionEventArgs<WebProduct> args3 = new ConnectionEventArgs<WebProduct>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 623);
						break;
					}
				}
				case ProductActions.DeleteRequest:
				{
					if (this.OnDeletedRequest == null)
					{
						throw new Exception("there is no eventHandller for WebProduct.DeleteRequest");
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeletedRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 632);
						break;
					}
				}
				default:
					throw new Exception($"action({pack.PacketActions}) is not defined!");
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 639);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
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
				PrdAnbarRelation = null;
				barcodes = null;
				Types = null;
				base.Dispose(disposing: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 673);
		}
	}

	public decimal GetVahedQuantity(Guid prdVahedGuid)
	{
		if (V2Guid == prdVahedGuid)
		{
			if (!(V2Qt <= 0m))
			{
				return V2Qt;
			}
			throw new ArgumentException("V2Qt is <=0 but prdVahedGuid has value!");
		}
		if (V3Guid == prdVahedGuid)
		{
			if (!(V3Qt <= 0m))
			{
				return V3Qt;
			}
			throw new ArgumentException("V3Qt is <=0 but prdVahedGuid has value!");
		}
		if (V4Guid == prdVahedGuid)
		{
			if (!(V4Qt <= 0m))
			{
				return V4Qt;
			}
			throw new ArgumentException("V4Qt is <=0 but prdVahedGuid has value!");
		}
		return 1m;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}

	public bool IsMultiVahed()
	{
		if ((V2Guid ?? Guid.Empty) != Guid.Empty && (V2Guid ?? Guid.Empty) != GuidVahed)
		{
			return true;
		}
		if ((V3Guid ?? Guid.Empty) != Guid.Empty && (V3Guid ?? Guid.Empty) != GuidVahed)
		{
			return true;
		}
		if ((V4Guid ?? Guid.Empty) != Guid.Empty && (V4Guid ?? Guid.Empty) != GuidVahed)
		{
			return true;
		}
		return false;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<WebProduct>>> GetAllByBarcodeAsync(string barcode, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<WebProduct>> res = new ReturnedSaveFuncInfoWithValue<List<WebProduct>>();
		try
		{
			if (string.IsNullOrEmpty(barcode))
			{
				return res.AddError("بارکد وارد شده معتبر نمی باشد", "GetAllByBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 734);
			}
			Dictionary<string, string> headers = new Dictionary<string, string> { { "Barcode", barcode } };
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebProduct>>> returnedSaveFuncInfoWithValue = await (HttpHelpers.ApiUrl + "api/ProductBarcodeRelation/GetList").GetFromApi<List<WebProduct>>(token, 1, headers);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllByBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 737);
			if (res.HasError)
			{
				return res;
			}
			res.value = returnedSaveFuncInfoWithValue.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllByBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 743);
			res.AddReturnedValue(ex, "", "GetAllByBarcodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebProduct.cs", 744);
		}
		return res;
	}

	public bool HasChangesOnVaheds(WebProduct otherPrd)
	{
		if (otherPrd == null)
		{
			throw new NullReferenceException("otherPrd");
		}
		if (GuidVahed != otherPrd.GuidVahed)
		{
			return true;
		}
		if ((V2Guid ?? Guid.Empty) != (otherPrd.V2Guid ?? Guid.Empty))
		{
			return true;
		}
		if ((V3Guid ?? Guid.Empty) != (otherPrd.V3Guid ?? Guid.Empty))
		{
			return true;
		}
		if ((V4Guid ?? Guid.Empty) != (otherPrd.V4Guid ?? Guid.Empty))
		{
			return true;
		}
		if (V2Qt != otherPrd.V2Qt)
		{
			return true;
		}
		if (V3Qt != otherPrd.V3Qt)
		{
			return true;
		}
		if (V4Qt != otherPrd.V4Qt)
		{
			return true;
		}
		return false;
	}

	public string GetTaxClassName()
	{
		string result = "NoTax";
		if (IsTax && TaxPercent > 0m)
		{
			result = $"{TaxPercent}Percent";
		}
		return result;
	}
}
