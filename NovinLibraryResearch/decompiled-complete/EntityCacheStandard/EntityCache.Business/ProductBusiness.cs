using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.ProductAttrebutes;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Accounting.Tax;
using EntityCache.Business.Core;
using EntityCache.Business.Extensions;
using EntityCache.Business.Global;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Filters;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Mapping.BusinessMappers.Accounting.Product;
using EntityCache.Mapping.BusinessMappers.Accounting.ProductAttrebutes;
using EntityCache.Models.Accounting.Prd;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Product;
using Serilog;

namespace EntityCache.Business;

public class ProductBusiness : BaseBusiness<ProductBusiness>, IProduct, IProductCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<ProductBusiness>, IDeliverableEntity, IEditableDelivery, IFileAttachment, IProductMicro
{
	private List<ProductMetaDataBusiness> _metaList;

	private List<ProductBusiness> _attributes;

	private List<ProductAttributeRelationBusiness> _attributeRelation;

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _saveListSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static BaseLocks _send2ServerSemaphores = new BaseLocks();

	private CancellationTokenSource _send2ServerToken = new CancellationTokenSource();

	private List<FileInfoBusiness> _addedFileList = new List<FileInfoBusiness>();

	private List<FileInfoBusiness> _removedFileList = new List<FileInfoBusiness>();

	private decimal bPercent1;

	private decimal bPercent2;

	private decimal bPercent3;

	private decimal bPercent4;

	private decimal _v2Qt;

	private decimal _v3Qt;

	private decimal _v4Qt;

	private decimal _width;

	private decimal _length;

	private decimal _height;

	private decimal _weight;

	private decimal _discountPercent;

	private decimal? _taxPercent;

	private string _deliveryMessage = "";

	private string _deliveryShortMessage = "";

	private string _hostRefrenced = "";

	private string _idProduct;

	private string _name;

	private string _description = "";

	private string _fullContent;

	private string _keywords;

	private string _metaDesc;

	private string _otherPics;

	private string _quickText;

	private DateTime _createDate = DateTime.Now;

	private List<ProductGroupRelationBusiness> _prdGroupRelation;

	private PrdVahedBusiness _prdVahed;

	private Guid? v2Guid;

	private Guid? v3Guid;

	private Guid? v4Guid;

	private List<ProductPriceRelationBusiness> _priceList;

	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	public byte[] DeleteImage => ImageResourceManager.OpenInvoiceDelete;

	public IReadOnlyCollection<ProductBusiness> ProductAttributes
	{
		get
		{
			if (_attributes == null)
			{
				_attributes = new List<ProductBusiness>();
			}
			_attributes.RemoveAll((ProductBusiness q) => q == null || q.IsDisposed || (q.MasterGuid ?? Guid.Empty) == Guid.Empty || !q.Status);
			return _attributes;
		}
	}

	public IReadOnlyCollection<ProductAttributeRelationBusiness> AttributeRelations
	{
		get
		{
			if (_attributeRelation == null)
			{
				_attributeRelation = new List<ProductAttributeRelationBusiness>();
			}
			_attributeRelation.RemoveAll((ProductAttributeRelationBusiness q) => q?.IsDisposed ?? true);
			return _attributeRelation;
		}
	}

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Special { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public long Code { get; set; }

	public EnProductIndex IndexingType { get; set; }

	public EnProductKind Kind { get; set; }

	public bool IsDependentOnCurrency { get; set; }

	public Guid? TaxGuid { get; set; }

	public DateTime? DiscountStartDate { get; set; }

	public DateTime? DiscountEndDate { get; set; }

	public decimal DiscountPrice { get; set; }

	public ProductDecimalMetaData DecimalMetaData { get; set; }

	public override EntityChangeSt ChangeStatus { get; set; }

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value.FixString();
		}
	}

	public decimal FQuantity { get; set; }

	public FileManagement_Parts Part => FileManagement_Parts.SharedFiles;

	public IReadOnlyCollection<FileInfoBusiness> AddedFiles => _addedFileList;

	public IReadOnlyCollection<FileInfoBusiness> RemovedFiles => _removedFileList;

	public decimal DiscountPercent
	{
		get
		{
			return _discountPercent.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_discountPercent = value;
		}
	}

	public string IdProduct
	{
		get
		{
			return _idProduct;
		}
		set
		{
			_idProduct = value.FixString();
		}
	}

	public DateTime? SendToServerDate { get; set; }

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

	public int Version { get; set; }

	[JsonIgnore]
	public decimal VariationPrice { get; set; }

	[JsonIgnore]
	public decimal VariationPrice2 { get; set; }

	public Guid? V2Guid
	{
		get
		{
			return v2Guid;
		}
		set
		{
			v2Guid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? V3Guid
	{
		get
		{
			return v3Guid;
		}
		set
		{
			v3Guid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? V4Guid
	{
		get
		{
			return v4Guid;
		}
		set
		{
			v4Guid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public List<string> ImageListData { get; set; }

	public PayementType PayementType { get; set; }

	public string Sku { get; set; }

	public string Slug { get; set; }

	public decimal Weight
	{
		get
		{
			return _weight.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_weight = value;
		}
	}

	[JsonIgnore]
	public Guid? FirstGroupGuid
	{
		get
		{
			IReadOnlyCollection<ProductGroupRelationBusiness> groupList = GroupList;
			if (groupList != null && groupList.Any())
			{
				return GroupList.OrderBy((ProductGroupRelationBusiness x) => x.Ordinal).Take(1).SingleOrDefault()
					.GroupGuid;
			}
			return null;
		}
	}

	public Guid GuidVahed { get; set; }

	public decimal Amount { get; set; }

	public string AmountForDisplay
	{
		get
		{
			if ((MasterGuid ?? Guid.Empty) == Guid.Empty)
			{
				return $"{Amount:N0} (زیرمجموعه)";
			}
			return Amount.NpToString("N0", 0);
		}
	}

	public bool AutoSellSt { get; set; }

	public decimal BPercent1
	{
		get
		{
			return bPercent1.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			bPercent1 = value;
		}
	}

	public decimal BPercent2
	{
		get
		{
			return bPercent2.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			bPercent2 = value;
		}
	}

	public decimal BPercent3
	{
		get
		{
			return bPercent3.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			bPercent3 = value;
		}
	}

	public decimal BPercent4
	{
		get
		{
			return bPercent4.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			bPercent4 = value;
		}
	}

	public DateTime CreateDate
	{
		get
		{
			return _createDate;
		}
		set
		{
			if (value.Year <= 2000 || value.Year >= 2050)
			{
				throw new ArgumentOutOfRangeException($"Value of CreateDate in ProductBusiness: {value}");
			}
			_createDate = value;
		}
	}

	public string FullContent
	{
		get
		{
			return _fullContent;
		}
		set
		{
			_fullContent = value?.FixString() ?? "";
		}
	}

	public decimal Height
	{
		get
		{
			return _height.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_height = value;
		}
	}

	public bool IsTax { get; set; }

	public bool IsSerial { get; set; }

	public decimal KardexPrice { get; set; }

	public string Keywords
	{
		get
		{
			return _keywords;
		}
		set
		{
			_keywords = value?.FixString() ?? "";
		}
	}

	public string MetaDesc
	{
		get
		{
			return _metaDesc;
		}
		set
		{
			_metaDesc = value?.FixString() ?? "";
		}
	}

	public ProductMojodiSt Mojodi { get; set; }

	public string OtherPics
	{
		get
		{
			return _otherPics;
		}
		set
		{
			_otherPics = value?.FixString() ?? "";
		}
	}

	public string QuickText
	{
		get
		{
			return _quickText;
		}
		set
		{
			_quickText = value?.FixString() ?? "";
		}
	}

	public bool ScaleSt { get; set; }

	public decimal Score { get; set; }

	public decimal V2Qt
	{
		get
		{
			return _v2Qt.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_v2Qt = value;
		}
	}

	public decimal V3Qt
	{
		get
		{
			return _v3Qt.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_v3Qt = value;
		}
	}

	public decimal V4Qt
	{
		get
		{
			return _v4Qt.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_v4Qt = value;
		}
	}

	public decimal Width
	{
		get
		{
			return _width.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_width = value;
		}
	}

	public decimal Length
	{
		get
		{
			return _length.ToString().ParseToDecimal(0m, 3);
		}
		set
		{
			_length = value;
		}
	}

	public decimal OrderPoint { get; set; }

	public EnProductType Type { get; set; }

	public Guid? MasterGuid { get; set; }

	[JsonIgnore]
	public string SendToServerStatus => ServerSt();

	public ServerStatusForClient ServerStatus { get; set; }

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

	public bool IsLastPriceForCustomer { get; set; }

	[JsonIgnore]
	public OrderBusiness order { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public string MappId { get; set; }

	public string NameForDisplay { get; set; }

	public Guid? DefaultVariable { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public List<ProductTagsRecord> TagList { get; set; }

	public IReadOnlyCollection<ProductPriceRelationBusiness> PriceList
	{
		get
		{
			if (_priceList == null)
			{
				_priceList = new List<ProductPriceRelationBusiness>();
			}
			_priceList.RemoveAll((ProductPriceRelationBusiness q) => q?.IsDisposed ?? true);
			return _priceList;
		}
	}

	public IReadOnlyCollection<ProductGroupRelationBusiness> GroupList
	{
		get
		{
			if (_prdGroupRelation == null)
			{
				_prdGroupRelation = new List<ProductGroupRelationBusiness>();
			}
			_prdGroupRelation.RemoveAll((ProductGroupRelationBusiness q) => q?.IsDisposed ?? true);
			return _prdGroupRelation;
		}
	}

	public event Func<object, ProductOnBusinessSavedEventArgs, Task> OnProductSaved;

	public async Task<IEnumerable<ProductMetaDataBusiness>> GetMetaDataListAsync(bool isClear = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (isClear)
			{
				_metaList = new List<ProductMetaDataBusiness>();
			}
			else if (_metaList == null)
			{
				_metaList = new List<ProductMetaDataBusiness>();
			}
			_metaList = (await GetMetaDataListAsync(ContextGuid, Guid, token, tranName))?.ToList();
			return _metaList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMetaDataListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 32);
			throw;
		}
	}

	public void ClearListMetaData()
	{
		_metaList?.Clear();
	}

	public void RemoveDet(ProductMetaKey key)
	{
		try
		{
			ProductMetaDataBusiness productMetaDataBusiness = _metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == key);
			if (productMetaDataBusiness != null)
			{
				_metaList.Remove(productMetaDataBusiness);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 45);
		}
	}

	public void AddMetaData(ProductMetaKey key, string value, EnDataType type = EnDataType.String)
	{
		try
		{
			ProductMetaDataBusiness productMetaDataBusiness = _metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == key);
			if (productMetaDataBusiness != null)
			{
				productMetaDataBusiness.Value = value;
				return;
			}
			ProductMetaDataBusiness productMetaDataBusiness2 = new ProductMetaDataBusiness(ContextGuid, new MetaDataBusiness(ContextGuid)
			{
				Value = value,
				DataType = type,
				EntityGuid = Guid,
				EntityType = EntityPart.Product
			});
			switch (key)
			{
			case ProductMetaKey.TaxCode:
				productMetaDataBusiness2.Key = "شناسه مالیاتی کالا";
				break;
			case ProductMetaKey.TaxName:
				productMetaDataBusiness2.Key = ((short)1).ToString();
				break;
			case ProductMetaKey.AttributeGuidList:
				productMetaDataBusiness2.Key = "AttributeGuidList";
				break;
			case ProductMetaKey.SeoModel:
				productMetaDataBusiness2.Key = "SeoModel";
				break;
			case ProductMetaKey.AvailableForSell:
				productMetaDataBusiness2.Key = ((short)4).ToString();
				break;
			case ProductMetaKey.AvailableForBuy:
				productMetaDataBusiness2.Key = ((short)5).ToString();
				break;
			case ProductMetaKey.AvailableForSellToOnlineShop:
				productMetaDataBusiness2.Key = ((short)6).ToString();
				break;
			case ProductMetaKey.AvailableForReturn:
				productMetaDataBusiness2.Key = ((short)7).ToString();
				break;
			default:
				throw new NotImplementedException();
			}
			if (_metaList == null)
			{
				_metaList = new List<ProductMetaDataBusiness>();
			}
			_metaList.Add(productMetaDataBusiness2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddMetaData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 84);
			throw;
		}
	}

	public static async Task<IEnumerable<ProductMetaDataBusiness>> GetMetaDataListAsync(string contextGuid, Guid guid, CancellationToken token, string tranName = "")
	{
		try
		{
			List<ProductMetaDataBusiness> metaList = new List<ProductMetaDataBusiness>();
			MetaDataFilter filter = new MetaDataFilter
			{
				EntityGuid = guid,
				EntityType = EntityPart.Product
			};
			IEnumerable<MetaDataBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.MetaData.GetAllAsync(filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			foreach (MetaDataBusiness item in enumerable)
			{
				metaList.Add(new ProductMetaDataBusiness(contextGuid, item));
			}
			return metaList;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMetaDataListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 107);
			throw;
		}
	}

	public async Task<List<Guid>> GetAttributeGuidListAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return ((_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AttributeGuidList))?.Value).FromJson<List<Guid>>("GetAttributeGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 117)?.Distinct()?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAttributeGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 122);
			throw;
		}
	}

	public async Task<string> GetTaxNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return (_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.TaxName))?.Value ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTaxNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 136);
			throw;
		}
	}

	public async Task<string> GetTaxCodeAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return (_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.TaxCode))?.Value ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTaxCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 150);
			throw;
		}
	}

	public async Task<ProductSeoModel> GetSeoModelAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return ((_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.SeoModel))?.Value).FromJson<ProductSeoModel>("GetSeoModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 160);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSeoModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 164);
			throw;
		}
	}

	public async Task<bool> GetAvailableForSellAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return (_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForSell))?.Value.ParseToBoolean() ?? true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAvailableForSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 179);
			throw;
		}
	}

	public async Task<bool> GetAvailableForBuyAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return (_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForBuy))?.Value.ParseToBoolean() ?? true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAvailableForBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 194);
			throw;
		}
	}

	public async Task<bool> GetAvailableForSellToOnlineShopAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			ProductMetaDataBusiness productMetaDataBusiness = _metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForSellToOnlineShop);
			if (productMetaDataBusiness == null)
			{
				if (ChangeStatus == EntityChangeSt.New)
				{
					return ((await LocalSettingBusiness.GetSettingAsync(ContextGuid, token))?.Products?.DefaultAvailableSellToOnlineShop).GetValueOrDefault();
				}
				return true;
			}
			return productMetaDataBusiness.Value.ParseToBoolean();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAvailableForSellToOnlineShopAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 217);
			throw;
		}
	}

	public async Task<bool> GetAvailableForReturnAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(_metaList?.Any() ?? false))
			{
				await GetMetaDataListAsync(isClear: false, token, tranName);
			}
			return (_metaList?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.AvailableForReturn))?.Value.ParseToBoolean() ?? true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAvailableForReturnAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMetaDataBusiness.cs", 232);
			throw;
		}
	}

	public async Task AddAttributeAsync(ProductBusiness det, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (det == null)
			{
				return;
			}
			if (_attributes == null)
			{
				_attributes = new List<ProductBusiness>();
			}
			if (GroupList?.Any() ?? false)
			{
				foreach (ProductGroupRelationBusiness group in GroupList)
				{
					det.AddToGroupRelationList(group.GroupGuid);
				}
			}
			if ((PriceList?.Any() ?? false) && !(det?.PriceList?.Any()).GetValueOrDefault())
			{
				List<ProductPriceRelationBusiness> list = await ProductPriceRelationMapper.Instance.MapListAsync(ContextGuid, PriceList, new List<ProductPriceRelationBusiness>(), "", token);
				foreach (ProductPriceRelationBusiness item in list)
				{
					item.ProductGuid = det.Guid;
				}
				await det.AddRangeToPriceListAsync(list, token);
			}
			if (det.GuidVahed != GuidVahed)
			{
				det.GuidVahed = GuidVahed;
			}
			_attributes.Add(det);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAttributeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 67);
		}
	}

	public Task AddAttributeRelationAsync(ProductAttributeRelationBusiness det, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (det == null)
			{
				return Task.CompletedTask;
			}
			if (_attributeRelation == null)
			{
				_attributeRelation = new List<ProductAttributeRelationBusiness>();
			}
			if (_attributeRelation?.SingleOrDefault((ProductAttributeRelationBusiness q) => q.ProductGuid == det.ProductGuid && q.AttributeValueGuid == det.AttributeValueGuid) != null)
			{
				return Task.CompletedTask;
			}
			_attributeRelation.Add(det);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAttributeRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 79);
		}
		return Task.CompletedTask;
	}

	public async Task AddAttributeRelationListAsync(IEnumerable<ProductAttributeRelationBusiness> detList, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(detList?.Any() ?? false))
			{
				return;
			}
			foreach (ProductAttributeRelationBusiness det in detList)
			{
				await AddAttributeRelationAsync(det, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAttributeRelationListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 91);
			throw;
		}
	}

	public async Task AddAttributeRelationGuidListAsync(List<clsProductAttributeRelation> attributelist, Guid productGuid, bool isClear, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(attributelist?.Any() ?? false))
			{
				return;
			}
			if (isClear)
			{
				List<ProductAttributeRelationBusiness> list = _attributeRelation?.Where((ProductAttributeRelationBusiness q) => q.ProductGuid == productGuid)?.ToList();
				if (list != null && list.Any())
				{
					foreach (ProductAttributeRelationBusiness item in list)
					{
						_attributeRelation.Remove(item);
					}
				}
			}
			foreach (clsProductAttributeRelation item2 in attributelist)
			{
				await AddAttributeRelationAsync(new ProductAttributeRelationBusiness(ContextGuid)
				{
					AttributeValueGuid = item2.AttributeValueGuid,
					ContextGuid = ContextGuid,
					ProductGuid = productGuid,
					AttributeGuid = item2.AttributeGuid
				}, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAttributeRelationGuidListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 116);
			throw;
		}
	}

	public async Task AddAttributeListAsync(IEnumerable<ProductBusiness> detList, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(detList?.Any() ?? false))
			{
				return;
			}
			foreach (ProductBusiness det in detList)
			{
				await AddAttributeAsync(det, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddAttributeListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 129);
			throw;
		}
	}

	public Task RemoveAttributeAsync(Guid productGuid)
	{
		try
		{
			List<ProductBusiness> attributes = _attributes;
			if (attributes == null || !attributes.Any())
			{
				return Task.CompletedTask;
			}
			ProductBusiness productBusiness = _attributes.SingleOrDefault((ProductBusiness q) => q.Guid == productGuid);
			if (productBusiness == null)
			{
				return Task.CompletedTask;
			}
			productBusiness.Status = false;
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAttributeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 145);
			throw;
		}
	}

	public Task RemoveFromAttributeAsync(Guid productGuid)
	{
		try
		{
			List<ProductBusiness> attributes = _attributes;
			if (attributes == null || !attributes.Any())
			{
				return Task.CompletedTask;
			}
			ProductBusiness productBusiness = _attributes.SingleOrDefault((ProductBusiness q) => q.Guid == productGuid);
			if (productBusiness == null)
			{
				return Task.CompletedTask;
			}
			productBusiness.MasterGuid = null;
			List<ProductAttributeRelationBusiness> list = _attributeRelation?.Where((ProductAttributeRelationBusiness q) => q.ProductGuid == productGuid)?.ToList();
			if (list == null || !list.Any())
			{
				return Task.CompletedTask;
			}
			foreach (ProductAttributeRelationBusiness item in list)
			{
				_attributeRelation.Remove(item);
			}
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveFromAttributeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 164);
			throw;
		}
	}

	public async Task<List<ProductBusiness>> GenerateAttributesAsync(List<Guid> valueList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(valueList?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<ProductAttributeValueReportBusiness> enumerable = await ProductAttributeValueBusiness.GetAllAsync(ContextGuid);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			IEnumerable<ProductAttributeValueReportBusiness> attributeValueList = enumerable.Where((ProductAttributeValueReportBusiness q) => valueList.Contains(q.ValueGuid));
			if (!(attributeValueList?.Any() ?? false))
			{
				return null;
			}
			ProductAttributeBusiness[] attributeList = (await Task.WhenAll(attributeValueList?.Select((ProductAttributeValueReportBusiness q) => q.GetMasterAsync()).ToList()))?.Distinct()?.OrderBy((ProductAttributeBusiness q) => q.Ordinal).ToArray();
			List<ProductBusiness> subProducts = new List<ProductBusiness>();
			Tuple<List<clsProductAttribute>, List<ProductBusiness>> tuple = await GenerateAttributeListByOrdinalAsync(attributeList?.ToList(), attributeValueList, token, tranName);
			if ((tuple?.Item2?.Any()).GetValueOrDefault())
			{
				subProducts.AddRange(tuple.Item2);
			}
			if (!(tuple?.Item1?.Any()).GetValueOrDefault())
			{
				return subProducts;
			}
			short minOrdinal = tuple.Item1.Min((clsProductAttribute q) => q.Ordinal);
			List<clsProductAttributeName> list = await GenerateAttributeNameAsync(tuple.Item1, minOrdinal);
			if (list == null || !list.Any())
			{
				return null;
			}
			string value = $"{Code}001";
			long nextCode = value.ParseToLong(0L);
			foreach (clsProductAttributeName item in list)
			{
				List<clsProductAttributeRelation> list2 = new List<clsProductAttributeRelation>();
				foreach (Guid attributeValueGuid in item.AttributeValueGuidList)
				{
					ProductAttributeValueBusiness productAttributeValueBusiness = attributeList.SelectMany((ProductAttributeBusiness q) => q.DetailList).FirstOrDefault((ProductAttributeValueBusiness q) => q.Guid == attributeValueGuid);
					if (productAttributeValueBusiness != null)
					{
						list2.Add(new clsProductAttributeRelation(productAttributeValueBusiness.MasterGuid, attributeValueGuid));
					}
				}
				List<ProductBusiness> list3 = subProducts;
				list3.Add(await GenerateSubProductAsync(item.Name, list2, nextCode));
				nextCode = checked(nextCode + 1);
			}
			return subProducts;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateAttributesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 211);
			throw;
		}
	}

	private async Task<Tuple<List<clsProductAttribute>, List<ProductBusiness>>> GenerateAttributeListByOrdinalAsync(List<ProductAttributeBusiness> attributeList, IEnumerable<ProductAttributeValueReportBusiness> attributeValueList, CancellationToken token, string tranName = "")
	{
		checked
		{
			try
			{
				ProductAttributeBusiness lastOrdinal = attributeList?.OrderBy((ProductAttributeBusiness q) => q.Ordinal)?.LastOrDefault();
				IEnumerable<ProductAttributeValueReportBusiness> enumerable = attributeValueList.Where((ProductAttributeValueReportBusiness q) => q.MasterGuid == lastOrdinal.Guid);
				ProductAttributeBusiness smallerOrdinal = null;
				short maxOrdinal;
				for (maxOrdinal = lastOrdinal?.Ordinal ?? 0; maxOrdinal > 0; maxOrdinal--)
				{
					if (smallerOrdinal != null)
					{
						break;
					}
					smallerOrdinal = attributeList.FirstOrDefault((ProductAttributeBusiness q) => q.Ordinal == maxOrdinal - 1);
				}
				if (smallerOrdinal == null)
				{
					List<ProductBusiness> prdList = new List<ProductBusiness>();
					string value = $"{Code}001";
					long nextCode2 = value.ParseToLong(0L);
					foreach (ProductAttributeValueReportBusiness item in enumerable)
					{
						ProductBusiness productBusiness = await GenerateSubProductAsync(lastOrdinal.Name + " " + item.Value, new List<clsProductAttributeRelation>
						{
							new clsProductAttributeRelation(item.MasterGuid, item.ValueGuid)
						}, nextCode2);
						prdList.Add(productBusiness);
						nextCode2 = productBusiness?.Code ?? nextCode2;
						nextCode2++;
					}
					return new Tuple<List<clsProductAttribute>, List<ProductBusiness>>(null, prdList);
				}
				List<clsProductAttribute> list = new List<clsProductAttribute>();
				maxOrdinal = lastOrdinal?.Ordinal ?? 0;
				while (smallerOrdinal != null)
				{
					foreach (ProductAttributeValueReportBusiness item2 in enumerable)
					{
						_ = item2;
						IEnumerable<ProductAttributeValueReportBusiness> enumerable2 = attributeValueList.Where((ProductAttributeValueReportBusiness q) => q.MasterGuid == smallerOrdinal.Guid);
						if (enumerable2 == null || !enumerable2.Any())
						{
							continue;
						}
						foreach (ProductAttributeValueReportBusiness item3 in enumerable2)
						{
							list.Add(new clsProductAttribute(smallerOrdinal.Name + " " + item3.Value, item3.ValueGuid, smallerOrdinal.Ordinal));
						}
					}
					smallerOrdinal = null;
					for (; maxOrdinal > 0; maxOrdinal--)
					{
						if (smallerOrdinal != null)
						{
							break;
						}
						smallerOrdinal = attributeList.FirstOrDefault((ProductAttributeBusiness q) => q.Ordinal == maxOrdinal - 1);
					}
				}
				if (enumerable != null && enumerable.Any())
				{
					foreach (ProductAttributeValueReportBusiness item4 in enumerable)
					{
						list.Add(new clsProductAttribute(lastOrdinal.Name + " " + item4.Value, item4.ValueGuid, lastOrdinal.Ordinal));
					}
				}
				if (list == null || !list.Any())
				{
					return null;
				}
				list = (from q in list
					group q by new { q.Name, q.AttributeValueGuid, q.Ordinal } into q
					select q.Key into q
					orderby q.Ordinal
					select new clsProductAttribute(q.Name, q.AttributeValueGuid, q.Ordinal)).ToList();
				return new Tuple<List<clsProductAttribute>, List<ProductBusiness>>(list, null);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateAttributeListByOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 273);
				throw;
			}
		}
	}

	private async Task<ProductBusiness> GenerateSubProductAsync(string name, List<clsProductAttributeRelation> attributeList, long nextCode)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("name");
		}
		if (!(attributeList?.Any() ?? false))
		{
			throw new ArgumentException("attributeList");
		}
		try
		{
			if (_attributes?.Any() ?? false)
			{
				foreach (ProductBusiness variation in _attributes)
				{
					IEnumerable<ProductAttributeRelationBusiness> enumerable = _attributeRelation?.Where((ProductAttributeRelationBusiness q) => q.ProductGuid == variation.Guid);
					if (enumerable == null || !enumerable.Any())
					{
						continue;
					}
					IEnumerable<Guid> enumerable2 = enumerable.Select((ProductAttributeRelationBusiness q) => q.AttributeValueGuid).Intersect(attributeList.Select((clsProductAttributeRelation q) => q.AttributeValueGuid));
					if (enumerable2 != null && enumerable2.Any() && enumerable2.SequenceEqual(attributeList.Select((clsProductAttributeRelation q) => q.AttributeValueGuid)))
					{
						if (!variation.Status)
						{
							variation.Status = true;
						}
						return variation;
					}
				}
			}
			ProductBusiness mappedProduct = await ProductMapper.Instance.MapAsync(ContextGuid, this, new ProductBusiness(ContextGuid));
			if (mappedProduct == null)
			{
				return mappedProduct;
			}
			mappedProduct.Amount = 0m;
			Guid newGuid = (mappedProduct.Guid = Guid.NewGuid());
			mappedProduct.Name = Name + " - " + name;
			mappedProduct.MasterGuid = Guid;
			mappedProduct.Kind = EnProductKind.SimpleProduct;
			mappedProduct.IndexingType = (EnProductIndex)0;
			await FillNextCodeAsync(mappedProduct, nextCode);
			await FillSkuAndSlugAsync(mappedProduct);
			await AddAttributeAsync(mappedProduct);
			if (mappedProduct.GroupList?.Any() ?? false)
			{
				foreach (ProductGroupRelationBusiness group in mappedProduct.GroupList)
				{
					group.ProductGuid = mappedProduct.Guid;
				}
			}
			if (mappedProduct.PriceList?.Any() ?? false)
			{
				IEnumerable<ProductPriceRelationModel> enumerable3 = await ProductPriceRelationBusiness.UpdateListAsync(ContextGuid, mappedProduct.PriceList);
				mappedProduct.VariationPrice = (enumerable3?.Where((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell)?.OrderBy((ProductPriceRelationModel q) => q.Ordinal)?.FirstOrDefault()?.Price).GetValueOrDefault();
				mappedProduct.VariationPrice2 = (enumerable3?.Where((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell)?.OrderBy((ProductPriceRelationModel q) => q.Ordinal)?.Skip(1)?.FirstOrDefault()?.Price).GetValueOrDefault();
				foreach (ProductPriceRelationBusiness price in mappedProduct.PriceList)
				{
					price.Guid = Guid.NewGuid();
					price.ProductGuid = mappedProduct.Guid;
				}
			}
			foreach (clsProductAttributeRelation attribute in attributeList)
			{
				await AddAttributeRelationAsync(new ProductAttributeRelationBusiness(ContextGuid)
				{
					ContextGuid = ContextGuid,
					AttributeValueGuid = attribute.AttributeValueGuid,
					ProductGuid = newGuid,
					AttributeGuid = attribute.AttributeGuid
				});
			}
			return mappedProduct;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSubProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 344);
			throw;
		}
	}

	private async Task FillSkuAndSlugAsync(ProductBusiness mappedProduct)
	{
		_ = 3;
		checked
		{
			try
			{
				int index = 0;
				string sku = mappedProduct.Guid.ToString().WordPressFixSlugOrSku();
				ProductBusiness productBusiness = ProductAttributes?.SingleOrDefault((ProductBusiness q) => q.Sku == sku);
				if (productBusiness == null)
				{
					productBusiness = await GetBySkuAsync(ContextGuid, sku);
				}
				string newSku = sku;
				while (productBusiness != null)
				{
					index++;
					newSku = $"{sku}_{index}";
					productBusiness = ProductAttributes?.SingleOrDefault((ProductBusiness q) => q.Sku == newSku);
					if (productBusiness == null)
					{
						productBusiness = await GetBySkuAsync(ContextGuid, newSku);
					}
				}
				mappedProduct.Sku = newSku;
				index = 0;
				string slug = mappedProduct.Guid.ToString().WordPressFixSlugOrSku();
				ProductBusiness productBusiness2 = ProductAttributes?.SingleOrDefault((ProductBusiness q) => q.Slug == slug);
				if (productBusiness2 == null)
				{
					productBusiness2 = await GetBySlugAsync(ContextGuid, slug);
				}
				string newSlug = slug;
				while (productBusiness2 != null)
				{
					index++;
					newSlug = $"{slug}_{index}";
					productBusiness2 = ProductAttributes?.SingleOrDefault((ProductBusiness q) => q.Slug == newSlug);
					if (productBusiness2 == null)
					{
						productBusiness2 = await GetBySlugAsync(ContextGuid, newSlug);
					}
				}
				mappedProduct.Slug = newSlug;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillSkuAndSlugAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 382);
				throw;
			}
		}
	}

	private async Task FillNextCodeAsync(ProductBusiness mappedProduct, long nextCode)
	{
		checked
		{
			try
			{
				for (ProductBusiness productBusiness = ProductAttributes?.FirstOrDefault((ProductBusiness q) => q.Code == nextCode); productBusiness != null; productBusiness = ProductAttributes?.FirstOrDefault((ProductBusiness q) => q.Code == nextCode))
				{
					nextCode++;
				}
				for (ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, nextCode.ToString(), EnSystem.UserInterface, true); productCompactBusiness != null; productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, nextCode.ToString(), EnSystem.UserInterface, true))
				{
					nextCode++;
				}
				for (ProductBusiness productBusiness = ProductAttributes?.FirstOrDefault((ProductBusiness q) => q.Code == nextCode); productBusiness != null; productBusiness = ProductAttributes?.FirstOrDefault((ProductBusiness q) => q.Code == nextCode))
				{
					nextCode++;
				}
				mappedProduct.Code = nextCode;
				mappedProduct.IdProduct = nextCode.ToString();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 416);
				throw;
			}
		}
	}

	private Task<List<clsProductAttributeName>> GenerateAttributeNameAsync(List<clsProductAttribute> list, short minOrdinal)
	{
		if (list == null || !list.Any())
		{
			throw new ArgumentException("list");
		}
		try
		{
			List<short> list2 = (from p in list
				where p.Ordinal != minOrdinal
				select p.Ordinal).Distinct().ToList();
			List<clsProductAttributeName> list3 = (from p in list
				where p.Ordinal == minOrdinal
				select new clsProductAttributeName(p.Name, new List<Guid> { p.AttributeValueGuid })).ToList();
			foreach (short ordinal in list2)
			{
				List<clsProductAttributeName> list4 = new List<clsProductAttributeName>();
				List<clsProductAttribute> list5 = list.Where((clsProductAttribute p) => p.Ordinal == ordinal).ToList();
				foreach (clsProductAttributeName item in list3)
				{
					foreach (clsProductAttribute item2 in list5)
					{
						List<Guid> list6 = item.AttributeValueGuidList.ToList();
						list6.Add(item2.AttributeValueGuid);
						list4.Add(new clsProductAttributeName(item.Name + " " + item2.Name, list6));
					}
				}
				list3 = list4;
			}
			return Task.FromResult(list3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateAttributeNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 444);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<ProductBusiness>> GenerateAndSaveMasterProductAsync(List<Guid> attributeValueList, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<ProductBusiness> res = new ReturnedSaveFuncInfoWithValue<ProductBusiness>();
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfoWithValue<ProductBusiness> result;
		try
		{
			_ = 19;
			try
			{
				ReturnedSaveFuncInfoWithValue<ProductBusiness> returnedSaveFuncInfoWithValue;
				if (autoTran)
				{
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 458), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 458);
					returnedSaveFuncInfoWithValue = res;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 459), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 459);
					transactionHasBeenCreated = true;
				}
				ProductBusiness mappedProduct = await ProductMapper.Instance.MapAsync(ContextGuid, this, new ProductBusiness(ContextGuid), tranName, token);
				if (mappedProduct == null)
				{
					result = null;
					goto IL_13fb;
				}
				mappedProduct.Guid = Guid.NewGuid();
				mappedProduct.MasterGuid = null;
				mappedProduct.IndexingType = (EnProductIndex)0;
				mappedProduct.ServerStatus = ServerStatusForClient.None;
				mappedProduct.Kind = EnProductKind.Variation;
				if (AttributeRelations?.Any() ?? false)
				{
					await mappedProduct.AddAttributeRelationListAsync(AttributeRelations, token);
				}
				await mappedProduct.AddAttributeAsync(this);
				ProductCompactFilter filter = new ProductCompactFilter
				{
					MasterGuid = Guid,
					LoadJustParents = false
				};
				IEnumerable<ProductCompactBusiness> enumerable = await ProductCompactBusiness.GetAllAsync(ContextGuid, EnSystem.UserInterface, filter, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					foreach (ProductCompactBusiness item in enumerable)
					{
						ProductBusiness productBusiness = await item.GetFullProductAsync();
						productBusiness.MasterGuid = mappedProduct.Guid;
						await mappedProduct.AddAttributeAsync(productBusiness);
					}
				}
				ClsCache.GetCache(ContextGuid).CacheRepository.IProduct.ResetCache(token, tranName);
				mappedProduct.AddMetaData(ProductMetaKey.AttributeGuidList, Json.ToStringJson(attributeValueList?.Distinct()?.ToList()));
				Dictionary<Guid, List<PrdTechnicalBusiness>> masterTechnicalDic = new Dictionary<Guid, List<PrdTechnicalBusiness>>();
				Dictionary<Guid, List<FileInfoBusiness>> masterFileInfoDic = new Dictionary<Guid, List<FileInfoBusiness>>();
				IReadOnlyList<PrdTechnicalBusiness> readOnlyList = await GetPrdTechnicalListAsync(EnSystem.UserInterface, token, tranName);
				if (readOnlyList != null && readOnlyList.Any())
				{
					List<PrdTechnicalBusiness> list = await PrdTechnicalMapper.Instance.MapListAsync(ContextGuid, readOnlyList, new List<PrdTechnicalBusiness>(), tranName, token);
					foreach (PrdTechnicalBusiness item2 in list)
					{
						item2.ProductGuid = mappedProduct.Guid;
						item2.Guid = Guid.NewGuid();
					}
					masterTechnicalDic.Add(mappedProduct.Guid, list);
				}
				FileInfoBusiness mainImage = (await FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicPrd, token, tranName))?.SingleOrDefault((FileInfoBusiness p) => p.Status);
				List<FileInfoBusiness> secondryImage = (await FileInfoBusiness.GetAsync(ContextGuid, Guid, FileManagement_Parts.PicPrdOthers, token, tranName))?.Where((FileInfoBusiness p) => p.Status)?.ToList();
				List<FileInfoBusiness> listFileInfo = new List<FileInfoBusiness>();
				if (mainImage != null)
				{
					FileInfoBusiness fileInfoBusiness = await FileInfoMapper.Instance.MapAsync(ContextGuid, mainImage, new FileInfoBusiness(ContextGuid), tranName, token);
					fileInfoBusiness.Guid = Guid.NewGuid();
					fileInfoBusiness.EntityGuid = mappedProduct.Guid;
					listFileInfo.Add(fileInfoBusiness);
				}
				if (secondryImage?.Any() ?? false)
				{
					foreach (FileInfoBusiness item3 in await FileInfoMapper.Instance.MapListAsync(ContextGuid, secondryImage, new List<FileInfoBusiness>(), tranName, token))
					{
						item3.Guid = Guid.NewGuid();
						item3.EntityGuid = mappedProduct.Guid;
						listFileInfo.Add(item3);
					}
				}
				if (listFileInfo?.Any() ?? false)
				{
					masterFileInfoDic.Add(Guid, listFileInfo);
				}
				ClsCache.GetCache(ContextGuid).CacheRepository.IProduct.ResetCache(token, tranName);
				MasterGuid = mappedProduct.Guid;
				if ((FirstGroupGuid ?? Guid.Empty) != Guid.Empty)
				{
					Code = await GetNextCodeAsync(ContextGuid, FirstGroupGuid.Value, null, token, tranName);
				}
				IdProduct = Code.ToString();
				Kind = EnProductKind.SimpleProduct;
				await FillSkuAndSlugAsync(this);
				mappedProduct.ClearGroupRelationList();
				if (GroupList?.Any() ?? false)
				{
					foreach (ProductGroupRelationBusiness group in GroupList)
					{
						mappedProduct.AddToGroupRelationList(group.GroupGuid);
					}
				}
				mappedProduct.ClearPriceList();
				if (PriceList?.Any() ?? false)
				{
					foreach (ProductPriceRelationBusiness price in PriceList)
					{
						mappedProduct.AddToPriceList(new ProductPriceRelationBusiness(ContextGuid)
						{
							ContextGuid = ContextGuid,
							ChangeStatus = EntityChangeSt.New,
							Guid = Guid.NewGuid(),
							KasrRial = price.KasrRial,
							Price = price.Price,
							PriceLevelGuid = price.PriceLevelGuid,
							ProductGuid = mappedProduct.Guid,
							Profit = price.Profit,
							Quantity = price.Quantity,
							ServerStatus = ServerStatusForClient.None,
							Type = price.Type,
							UnitGuid = price.UnitGuid
						});
					}
				}
				returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await SaveAsync(sendToServer: true, 1, token, EnSystem.UserInterface, awaitOnSemaphore: true, await4send: false, "", "", 60000, null, null, 10, tranName), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 564);
				if (res.HasError)
				{
					result = res;
					goto IL_13fb;
				}
				returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 566), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 566);
				returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 567), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 567);
				if (res.HasError)
				{
					result = res;
					goto IL_13fb;
				}
				returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await mappedProduct.SaveAsync(sendToServer: true, 1, token, EnSystem.UserInterface, awaitOnSemaphore: true, await4send: false, "", "", 60000, masterTechnicalDic, null, 10, tranName, masterFileInfoDic), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 569);
				res.value = mappedProduct;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 574);
				res.AddReturnedValue(ex, "", "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 575);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfoWithValue<ProductBusiness> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 581), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 581);
				returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 582), ReturnedState.None, "GenerateAndSaveMasterProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 582);
			}
		}
		return res;
		IL_13fb:
		return result;
	}

	public async Task<List<ProductCompactBusiness>> CheckAttributeValidationAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			List<ProductCompactBusiness> res = new List<ProductCompactBusiness>();
			List<Guid> attributeGuidList = await GetAttributeGuidListAsync(token, tranName);
			ProductCompactFilter productCompactFilter = new ProductCompactFilter
			{
				KindList = new List<EnProductKind>(1) { EnProductKind.Variation },
				MasterGuid = Guid,
				LoadJustParents = false
			};
			productCompactFilter.KindList = null;
			IEnumerable<ProductCompactBusiness> enumerable = (await ProductCompactBusiness.GetAllAsync(ContextGuid, system, productCompactFilter, token, tranName))?.Where((ProductCompactBusiness q) => q.Guid != Guid && q.Name != Name);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			foreach (ProductCompactBusiness child in enumerable)
			{
				if ((AttributeRelations.Where((ProductAttributeRelationBusiness q) => q.ProductGuid == child.Guid)?.Count() ?? 0) != (attributeGuidList?.Count() ?? 0))
				{
					res.Add(child);
				}
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckAttributeValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusinessAttribute.cs", 613);
			throw;
		}
	}

	public ProductBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public async Task<IReadOnlyList<PrdTechnicalBusiness>> GetPrdTechnicalListAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			List<PrdTechnicalBusiness> _prdTech = ((await GetPrdTechnicalListAsync(ContextGuid, Guid, token))?.ToList())?.Where((PrdTechnicalBusiness p) => p != null)?.ToList();
			await FillPrdTechnicalKeysAsync(system, _prdTech, token, tranName);
			return _prdTech;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdTechnicalListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 195);
			throw;
		}
	}

	private async Task FillPrdTechnicalKeysAsync(EnSystem system, List<PrdTechnicalBusiness> _prdTech, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (!(_prdTech?.Any((PrdTechnicalBusiness q) => string.IsNullOrEmpty(q.Key)) ?? false) || (FirstGroupGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			PrdGroupBusiness group = await PrdGroupBusiness.GetAsync(ContextGuid, FirstGroupGuid.Value, token, tranName);
			if (group != null)
			{
				AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				PrdTechnicalBusiness prdTechnicalBusiness = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 1);
				if (prdTechnicalBusiness != null)
				{
					prdTechnicalBusiness.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo1Title)) ? group.TechInfo.TechnicalInfo1Title : accGlobalSettings.Products.TechnicalInfo1Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness2 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 2);
				if (prdTechnicalBusiness2 != null)
				{
					prdTechnicalBusiness2.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo2Title)) ? group.TechInfo.TechnicalInfo2Title : accGlobalSettings.Products.TechnicalInfo2Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness3 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 3);
				if (prdTechnicalBusiness3 != null)
				{
					prdTechnicalBusiness3.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo3Title)) ? group.TechInfo.TechnicalInfo3Title : accGlobalSettings.Products.TechnicalInfo3Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness4 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 4);
				if (prdTechnicalBusiness4 != null)
				{
					prdTechnicalBusiness4.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo4Title)) ? group.TechInfo.TechnicalInfo4Title : accGlobalSettings.Products.TechnicalInfo4Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness5 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 5);
				if (prdTechnicalBusiness5 != null)
				{
					prdTechnicalBusiness5.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo5Title)) ? group.TechInfo.TechnicalInfo5Title : accGlobalSettings.Products.TechnicalInfo5Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness6 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 6);
				if (prdTechnicalBusiness6 != null)
				{
					prdTechnicalBusiness6.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo6Title)) ? group.TechInfo.TechnicalInfo6Title : accGlobalSettings.Products.TechnicalInfo6Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness7 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 7);
				if (prdTechnicalBusiness7 != null)
				{
					prdTechnicalBusiness7.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo7Title)) ? group.TechInfo.TechnicalInfo7Title : accGlobalSettings.Products.TechnicalInfo7Name);
				}
				PrdTechnicalBusiness prdTechnicalBusiness8 = _prdTech?.SingleOrDefault((PrdTechnicalBusiness q) => q.TechnicalType == 8);
				if (prdTechnicalBusiness8 != null)
				{
					prdTechnicalBusiness8.Key = ((!string.IsNullOrEmpty(group?.TechInfo?.TechnicalInfo8Title)) ? group.TechInfo.TechnicalInfo8Title : accGlobalSettings.Products.TechnicalInfo8Name);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillPrdTechnicalKeysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 234);
			throw;
		}
	}

	public static async Task<IReadOnlyList<PrdTechnicalBusiness>> GetPrdTechnicalListAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return (await PrdTechnicalBusiness.GetAllAsync(contextGuid, guid, null, token))?.ToList()?.AsReadOnly();
	}

	public string ServerSt()
	{
		return ServerStatus switch
		{
			ServerStatusForClient.None => "در حال ارسال..", 
			ServerStatusForClient.Sent => ServerStatus.GetDisplay() + "(" + HostRefrenced + ")", 
			ServerStatusForClient.Delivered => ServerStatus.GetDisplay() + "(" + HostRefrenced + ")", 
			ServerStatusForClient.SendError => ServerStatus.GetDisplay(), 
			ServerStatusForClient.DeliveryError => ServerStatus.GetDisplay(), 
			ServerStatusForClient.DirectDelivery => $"{ServerStatus.GetDisplay()}({DeliveryId})", 
			_ => $"خطا در تشخیص{ServerStatus}", 
		};
	}

	public Task<List<ProductBusiness>> GetVariationProductAsync(CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IProduct.GetVariationProductAsync(Guid, token, "");
	}

	internal List<string> FullTextIndexOnName_CodeFields()
	{
		return (Name + " " + IdProduct).PrepareForIndexing();
	}

	public Task<string> GetPicFullPathAsync(CancellationToken token = default(CancellationToken))
	{
		return FileInfoBusiness.GetDefaultProductUrlAsync(ContextGuid, Guid, token);
	}

	public async Task<PrdAnbarRelationBusiness> GetAnbarRelationAsync(string contextGuid, Guid depotGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetAnbarRelationsAsync(Guid, depotGuid, token, tranName))?.SingleOrDefault();
	}

	public async Task<List<PrdAnbarRelationBusiness>> GetAnbarRelationsAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			IEnumerable<DepotBusiness> depots;
			if (system == EnSystem.Mapper || system == EnSystem.SignalR)
			{
				string contextGuid = ContextGuid;
				CancellationToken token2 = token;
				depots = await DepotBusiness.GetAllAsync(contextGuid, EnSystem.None, true, "", token2);
			}
			else
			{
				string contextGuid2 = ContextGuid;
				CancellationToken token2 = token;
				depots = await DepotBusiness.GetAllAsync(contextGuid2, EnSystem.UserInterface, true, "", token2);
			}
			if (depots == null)
			{
				return new List<PrdAnbarRelationBusiness>();
			}
			List<PrdAnbarRelationBusiness> list = await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarRelation.GetAnbarRelationsAsync(Guid, null, token, tranName);
			if (list == null)
			{
				return new List<PrdAnbarRelationBusiness>();
			}
			return (from r in list
				join d in depots on r.AnbarGuid equals d.Guid
				select r).ToList();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 277);
			throw;
		}
	}

	public async Task<decimal> GetTaxPercentAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_taxPercent.HasValue)
			{
				return _taxPercent.Value;
			}
			_taxPercent = await GetTaxPercentAsync(ContextGuid, Guid, token, tranName);
			return _taxPercent.Value;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 292);
			throw;
		}
	}

	public static async Task<decimal> GetTaxPercentAsync(string contextGuid, Guid prdGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(contextGuid, prdGuid, EnSystem.None, null, tranName, token, true);
			if ((productCompactBusiness?.TaxGuid ?? Guid.Empty) == Guid.Empty)
			{
				return default(decimal);
			}
			TaxBusiness taxBusiness = await TaxBusiness.GetAsync(contextGuid, productCompactBusiness.TaxGuid.Value, token, tranName);
			if (!(taxBusiness?.DetailList?.Any()).GetValueOrDefault())
			{
				return default(decimal);
			}
			return taxBusiness.DetailList.Single().ProductTaxPercent;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 309);
			throw;
		}
	}

	public Task<decimal> GetSellAsync(EnSystem system, Guid priceLevelGuid, bool isLoadOriginalPrice, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return this.GetSellAsync(ContextGuid, system, order, priceLevelGuid, isLoadOriginalPrice, token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IProduct;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			try
			{
				await _connectionPoolSemaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 327), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 327);
				}
				transactionHasBeenCreated = true;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 329);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProduct.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 330);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 332);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 335);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 336);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 341), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 341);
			}
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebProduct webPrd = await ToWebItemAsync();
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.ProductLog;
				cashiersLogBusiness.SanadGuid = webPrd.Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره کالا".GetTranslate() + "\r\n" + ret.ErrorMessage + "\r\n" + $" {webPrd.Name}-{webPrd.Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webPrd, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 374);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(bool sendToServer, short autoFixValidationErrorsCount, CancellationToken token, EnSystem system, bool awaitOnSemaphore = true, bool await4send = false, string customerUserName = "", string customerPassword = "", int timeout = 60000, Dictionary<Guid, List<PrdTechnicalBusiness>> prdTechnicalList = null, Dictionary<Guid, List<PrdAnbarRelationBusiness>> prdAnbarRalationList = null, short tryCount = 10, string tranName = "", Dictionary<Guid, List<FileInfoBusiness>> fileInfo = null, bool updateAmount = false, Dictionary<Guid, List<BarcodeBusiness>> barcodes = null, Dictionary<Guid, List<Tuple<string, string, EnDataType>>> childMetaDatas = null)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 1;
		TaskCompletionSource<bool> saveComplationTask = new TaskCompletionSource<bool>();
		ProductBusiness oldProduct = null;
		try
		{
			_ = 32;
			checked
			{
				ReturnedSaveFuncInfo result;
				try
				{
					sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
					line = 3;
					using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ProductBusiness/SaveAsync", 60000, token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, autoFixValidationErrorsCount, prdTechnicalList, prdAnbarRalationList, fileInfo, autoTran ? "" : tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 391);
					line = 4;
					ENSource source;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 393);
					}
					else
					{
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 396), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 396);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 397), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 397);
							transactionHasBeenCreated = true;
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (system == EnSystem.SignalR)
							{
								ProductBusiness productBusiness = await GetBySkuAsync(ContextGuid, Sku, token, tranName);
								if (productBusiness != null && productBusiness.Sku == Sku && productBusiness.Guid != Guid)
								{
									productBusiness.Sku = $"{productBusiness.Guid}";
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await productBusiness.SaveAsync(sendToServer: false, 0, token, system, awaitOnSemaphore: false, await4send: false, "", "", 60000, null, null, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 408);
									ret.throwExceptionIfError();
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 410), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 410);
								}
								ProductBusiness productBusiness2 = await GetBySlugAsync(ContextGuid, Slug, token, tranName);
								if (productBusiness2 != null && productBusiness2.Slug == Slug && productBusiness2.Guid != Guid)
								{
									productBusiness2.Slug = $"{productBusiness2.Guid}";
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await productBusiness2.SaveAsync(sendToServer: false, 0, token, system, awaitOnSemaphore: false, await4send: false, "", "", 60000, null, null, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 417);
									ret.throwExceptionIfError();
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 419), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 419);
								}
							}
							source = ClsCache.GetCache(ContextGuid).Source;
							if (system != EnSystem.Transfer)
							{
								oldProduct = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
								if (source == ENSource.Accounting || system != EnSystem.SignalR)
								{
									if (oldProduct != null)
									{
										ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await (await ProductMapper.Instance.MapAsync(ContextGuid, this, new WebProduct(), tranName, token)).ComapreAsync(currentBacodes: (barcodes?.SingleOrDefault((KeyValuePair<Guid, List<BarcodeBusiness>> q) => q.Key == Guid))?.Value, contextGuid: ContextGuid, system: system, old: oldProduct, token: token, tranName: tranName);
										if (returnedSaveFuncInfo2.HasError)
										{
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.OtherLogsAsync(ContextGuid, (system == EnSystem.SignalR) ? EnSystem.System : system, oldProduct, sendToServer, token, EnLog.ProductLog, ENAction.Edit, returnedSaveFuncInfo2.GetErrorMessage(), Guid, awaitOnSemaphore: false, tranName, saveComplationTask), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 435);
										}
									}
									else
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.OtherLogsAsync(ContextGuid, (system == EnSystem.SignalR) ? EnSystem.System : system, this, sendToServer, token, EnLog.ProductLog, ENAction.Add, "ایجاد کالا".GetTranslate(), Guid, awaitOnSemaphore: false, tranName, saveComplationTask), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 438);
									}
								}
								ProductPriceRelationBusiness productPriceRelationBusiness = PriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == ParentDefaults.PriceLevel.RegularPrice);
								ProductPriceRelationBusiness productPriceRelationBusiness2 = oldProduct?.PriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == ParentDefaults.PriceLevel.RegularPrice);
								if (source == ENSource.Accounting)
								{
									ProductPriceUpdateBusiness productPriceUpdateBusiness = null;
									if (!((productPriceRelationBusiness?.Price ?? 0m) <= 0m) || !((productPriceRelationBusiness2?.Price ?? 0m) <= 0m))
									{
										if ((productPriceRelationBusiness?.Price ?? 0m) != 0m && (productPriceRelationBusiness2?.Price ?? 0m) <= 0m)
										{
											productPriceUpdateBusiness = new ProductPriceUpdateBusiness(ContextGuid)
											{
												ContextGuid = ContextGuid,
												RegularPrice = productPriceRelationBusiness.Price,
												DateM = DateTime.Now,
												Guid = Guid.NewGuid(),
												Modified = DateTime.Now,
												ProductGuid = Guid
											};
										}
										else if ((productPriceRelationBusiness?.Price ?? 0m) <= 0m && (productPriceRelationBusiness2?.Price ?? 0m) != 0m)
										{
											productPriceUpdateBusiness = new ProductPriceUpdateBusiness(ContextGuid)
											{
												ContextGuid = ContextGuid,
												RegularPrice = 0m,
												DateM = DateTime.Now,
												Guid = Guid.NewGuid(),
												Modified = DateTime.Now,
												ProductGuid = Guid
											};
										}
										else if (productPriceRelationBusiness.Price != productPriceRelationBusiness2.Price)
										{
											productPriceUpdateBusiness = new ProductPriceUpdateBusiness(ContextGuid)
											{
												ContextGuid = ContextGuid,
												RegularPrice = productPriceRelationBusiness.Price,
												DateM = DateTime.Now,
												Guid = Guid.NewGuid(),
												Modified = DateTime.Now,
												ProductGuid = Guid
											};
										}
									}
									if (productPriceUpdateBusiness != null)
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await productPriceUpdateBusiness.SaveAsync(system, token, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 489);
									}
								}
								if ((FirstGroupGuid ?? Guid.Empty) != Guid.Empty && !Special)
								{
									PrdGroupBusiness prdGroupBusiness = await PrdGroupBusiness.GetAsync(ContextGuid, FirstGroupGuid.Value, token, tranName);
									if (prdGroupBusiness != null && prdGroupBusiness.Special)
									{
										Special = true;
									}
								}
							}
							if (prdTechnicalList == null || !(prdTechnicalList?.Any() ?? false))
							{
								goto IL_15bc;
							}
							List<PrdTechnicalBusiness> newList = new List<PrdTechnicalBusiness>();
							line = 6;
							foreach (KeyValuePair<Guid, List<PrdTechnicalBusiness>> item3 in prdTechnicalList)
							{
								if (item3.Value == null)
								{
									continue;
								}
								if (ChangeStatus != EntityChangeSt.New)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await PrdTechnicalBusiness.RemoveRangeAsync(ContextGuid, item3.Key, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 511);
								}
								if (!(item3.Value?.Any() ?? false))
								{
									continue;
								}
								foreach (PrdTechnicalBusiness item4 in item3.Value)
								{
									if (!dis.Token.IsCancellationRequested)
									{
										if (item4 != null && !(item4.Guid == Guid.Empty))
										{
											item4.ProductGuid = item3.Key;
											newList.Add(item4);
										}
										continue;
									}
									result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 516);
									goto end_IL_02f3;
								}
							}
							if (!dis.Token.IsCancellationRequested)
							{
								line = 7;
								if (newList.Count > 0)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await PrdTechnicalBusiness.SaveRangeAsync(dis.Token, newList, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 525);
								}
								goto IL_15bc;
							}
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 522);
						}
					}
					goto end_IL_02f3;
					IL_15bc:
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 529);
					}
					else
					{
						line = 8;
						if (system == EnSystem.UserInterface)
						{
							Version++;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProduct.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 532);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							line = 9;
							if (prdAnbarRalationList == null)
							{
								goto IL_197a;
							}
							ClsCache.GetCache(ContextGuid);
							IEnumerable<DepotBusiness> enumerable = await DepotBusiness.GetAllAsync(ContextGuid, system, null, tranName, token);
							if (enumerable != null && enumerable.Any())
							{
								List<Guid> depotsGuid = enumerable.Select((DepotBusiness p) => p.Guid).ToList();
								foreach (KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> prdAnbarRalation in prdAnbarRalationList)
								{
									if (prdAnbarRalation.Value != null)
									{
										List<PrdAnbarRelationBusiness> lst = prdAnbarRalation.Value.Where((PrdAnbarRelationBusiness p) => depotsGuid.Contains(p.AnbarGuid)).ToList();
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await PrdAnbarRelationBusiness.SaveRangeAsync(lst, prdAnbarRalation.Key, updateAmount, 10, tranName, dis.Token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 548);
									}
								}
							}
							if (!ret.HasError)
							{
								goto IL_197a;
							}
							result = ret;
						}
					}
					goto end_IL_02f3;
					IL_1ac1:
					line = 12;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 562);
						goto end_IL_02f3;
					}
					line = 13;
					if (barcodes != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await BarcodeBusiness.RemoveRangeAsync(ContextGuid, new List<Guid> { Guid }, token, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 567);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 568), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 568);
						foreach (KeyValuePair<Guid, List<BarcodeBusiness>> barcode in barcodes)
						{
							if (barcode.Value == null)
							{
								continue;
							}
							foreach (BarcodeBusiness item2 in barcode.Value)
							{
								if ((item2.DepotGuid ?? Guid.Empty) != Guid.Empty && source != ENSource.Accounting && system == EnSystem.SignalR && await DepotBusiness.GetAsync(ContextGuid, item2.DepotGuid.Value, token, tranName) == null)
								{
									item2.DepotGuid = null;
								}
								item2.PrdGuid = barcode.Key;
								if (barcode.Value.FirstOrDefault((BarcodeBusiness q) => q.Ordinal == item2.Ordinal && q.BarCode != item2.BarCode) != null || item2.Ordinal <= 0)
								{
									item2.Ordinal = (short)((barcode.Value?.Max((BarcodeBusiness p) => p.Ordinal) ?? (-1)) + 1);
								}
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await BarcodeBusiness.SaveRangeAsync(system, token, barcode.Value, awaitOnSemaphore: false, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 585);
						}
					}
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 588);
						goto end_IL_02f3;
					}
					if (system != EnSystem.Transfer)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await MetaDataBusiness.SaveRangeAsync(ContextGuid, Guid, EntityPart.Product, _metaList, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 592);
						if ((AddedFiles?.Any() ?? false) || (RemovedFiles?.Any() ?? false))
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, AddedFiles, RemovedFiles, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 595);
						}
						if (ProductAttributes?.Any() ?? false)
						{
							foreach (ProductBusiness item in ProductAttributes)
							{
								if (item == null)
								{
									throw new ArgumentNullException("Attribute is Null");
								}
								item.TaxGuid = TaxGuid;
								Tuple<string, string, EnDataType> seoModel_ = (childMetaDatas?.SingleOrDefault((KeyValuePair<Guid, List<Tuple<string, string, EnDataType>>> q) => q.Key == item.Guid))?.Value?.SingleOrDefault((Tuple<string, string, EnDataType> q) => q.Item1 == "SeoModel");
								childMetaDatas?.Remove(item.Guid);
								if (childMetaDatas == null)
								{
									childMetaDatas = new Dictionary<Guid, List<Tuple<string, string, EnDataType>>>();
								}
								string taxCode = await GetTaxCodeAsync(token, tranName);
								string text = await GetTaxNameAsync(token, tranName);
								List<Tuple<string, string, EnDataType>> list = new List<Tuple<string, string, EnDataType>>();
								if (!string.IsNullOrEmpty(taxCode))
								{
									list.Add(new Tuple<string, string, EnDataType>("شناسه مالیاتی کالا".GetTranslate(), taxCode, EnDataType.String));
								}
								if (!string.IsNullOrEmpty(text))
								{
									list.Add(new Tuple<string, string, EnDataType>(((short)1).ToString(), text, EnDataType.String));
								}
								if (seoModel_ != null)
								{
									list.Add(new Tuple<string, string, EnDataType>("SeoModel", Json.ToStringJson(seoModel_), EnDataType.String));
								}
								if (list != null && list.Any())
								{
									childMetaDatas.Add(item.Guid, list);
								}
							}
						}
						if (childMetaDatas?.Any() ?? false)
						{
							if (_attributes?.Any() ?? false)
							{
								foreach (ProductBusiness attribute in _attributes)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await MetaDataBusiness.RemoveRangeAsync(ContextGuid, attribute.Guid, EntityPart.Product, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 626);
								}
							}
							foreach (KeyValuePair<Guid, List<Tuple<string, string, EnDataType>>> metaList in childMetaDatas)
							{
								foreach (Tuple<string, string, EnDataType> item5 in metaList.Value)
								{
									if (item5 != null)
									{
										MetaDataBusiness obj = new MetaDataBusiness(ContextGuid)
										{
											EntityGuid = metaList.Key,
											DataType = item5.Item3,
											EntityType = EntityPart.Product,
											Key = item5.Item1,
											Value = item5.Item2
										};
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(awaitOnSemaphore: true, token, 10, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 641);
									}
								}
							}
						}
					}
					goto end_IL_0198;
					IL_197a:
					line = 10;
					if (fileInfo == null)
					{
						goto IL_1ac1;
					}
					foreach (KeyValuePair<Guid, List<FileInfoBusiness>> item6 in fileInfo)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await FileInfoBusiness.SaveRangeAsync(system, dis.Token, autoFixValidationErrorsCount, item6.Value, sendToServer, Priority.Low, 10), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 558);
					}
					if (!ret.HasError)
					{
						goto IL_1ac1;
					}
					result = ret;
					end_IL_02f3:;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 647);
					goto end_IL_0198;
				}
				catch (Exception ex)
				{
					if (ClsCache.GetCache(ContextGuid) == null)
					{
						Log.Warning(ex, $"#Line_:{line}//Cache is null!!");
					}
					else
					{
						WebErrorLog.Instance.StartLog(ex, $"#Line_:{line}//Cache is not null.", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 651);
					}
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 652);
					goto end_IL_0198;
				}
				return result;
			}
			end_IL_0198:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 660), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 660);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 661), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 661);
				if (!ret.HasError && system == EnSystem.UserInterface)
				{
					ClsCache.GetCache(ContextGuid).BusinessAggregator.FullTextWordsBusiness.ForceUpdateFullTextSearchIndex();
				}
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(customerUserName, customerPassword, timeout, token, system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 668);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(customerUserName, customerPassword, timeout, token, system));
				}
				await SendVariationsToServerAsync(await4send, customerUserName, customerPassword, timeout, token, system);
			}
			if (system != EnSystem.Transfer)
			{
				saveComplationTask.TrySetResult(!ret.HasError);
			}
			if (ret.HasError && system == EnSystem.SignalR)
			{
				WebProduct webPrd = await ProductMapper.Instance.MapAsync(ContextGuid, this, new WebProduct(), "", token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo3 = await Product.ComapreAsync(currentBacodes: (barcodes?.SingleOrDefault((KeyValuePair<Guid, List<BarcodeBusiness>> q) => q.Key == Guid))?.Value, current: webPrd, contextGuid: ContextGuid, system: system, old: oldProduct, token: token, tranName: "");
				string text2 = "خطا در تعریف ذخیره کالا".GetTranslate() + "\r\n" + ret.ErrorMessage + "\r\n" + returnedSaveFuncInfo3.GetErrorMessage();
				WebErrorLog.Instance.StartLog("#ProductSaveError\r\n" + text2 + "\r\n" + Json.ToStringJson(webPrd), "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 684);
				if (returnedSaveFuncInfo3.HasError || system == EnSystem.UserInterface)
				{
					if (!returnedSaveFuncInfo3.HasError && system == EnSystem.UserInterface)
					{
						returnedSaveFuncInfo3.AddError("ویرایش کالا", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 690);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CashiersLogBusiness.OtherLogsAsync(ContextGuid, (system == EnSystem.SignalR) ? EnSystem.System : system, oldProduct, sendToServer, token, EnLog.ProductLog, ENAction.Edit, text2, Guid, awaitOnSemaphore: false, "", saveComplationTask), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 691);
				}
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	private async Task SendVariationsToServerAsync(bool await4send, string customerUserName, string customerPassword, int timeout, CancellationToken token, EnSystem system)
	{
		try
		{
			if (Kind != EnProductKind.Variation || !(ProductAttributes?.Any() ?? false))
			{
				return;
			}
			ClsCache cache = ClsCache.GetCache(ContextGuid);
			if (!cache.BufferEnable || cache.BufferLoginState.Status != EnLoginResult.Success)
			{
				return;
			}
			foreach (ProductBusiness item in ProductAttributes)
			{
				if (await4send)
				{
					await item.Send2ServerAsync(customerUserName, customerPassword, timeout, token, system);
					continue;
				}
				Task.Run(() => item.Send2ServerAsync(customerUserName, customerPassword, timeout, token, system));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendVariationsToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 713);
		}
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		return Send2ServerAsync("", "", 60000, token, system);
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(string customerUserName, string customerPassword, int timeout, CancellationToken token, EnSystem system)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			StringBuilder sb = new StringBuilder(100000);
			_send2ServerToken?.Cancel();
			_send2ServerToken = new CancellationTokenSource(60000);
			CancellationToken token2 = _send2ServerToken.Token;
			LockItem lockItem = await _send2ServerSemaphores.GetItemAsync(Guid.ToString(), "", ClsCache.GetCache(ContextGuid)?.BufferCancellationToken ?? default(CancellationToken), token, token2);
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (!cache.BufferEnable || cache.BufferLoginState.Status != EnLoginResult.Success)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 728);
				}
				if (!string.IsNullOrEmpty(customerPassword) || !string.IsNullOrEmpty(customerUserName))
				{
					throw new Exception("ارسال با بافر کاستومر فعلا غیر فعال شده است.".GetTranslate());
				}
				token = lockItem.Token;
				using SemaphoreUtil dis = await lockItem.Semaphore.GaurdAsync(true, "ProductBusiness/Send2ServerAsync", 60000, token, _send2ServerToken.Token);
				UserInformationBusiness userInformationBusiness = await UserInformationBusiness.GetAsync(ContextGuid);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 734);
				}
				sb.AppendLine($"#DepId{userInformationBusiness?.DepId ?? (-1)}_{Name}");
				sb.AppendLine($"StartTime:{DateTime.Now}");
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 736);
				}
				List<WebProduct> list = new List<WebProduct>();
				List<WebProduct> list2 = list;
				list2.Add(await ToWebItemAsync(token));
				List<Packet> lstPacket = WebProduct.GenerateSaveRequest(list, 0);
				sb.AppendLine($"StartTime:{DateTime.Now}");
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 739);
				}
				if (!string.IsNullOrEmpty(customerUserName) && !string.IsNullOrEmpty(customerPassword))
				{
					throw new NotImplementedException("این بخش از کد باید حذف شود.".GetTranslate());
				}
				if (ClsCache.GetCache(ContextGuid).BufferEnable)
				{
					using ProductWaiter waiter = new ProductWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, token, 120000);
					ClsCache.GetCache(ContextGuid).SendData(lstPacket, Priority.High);
					ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("ProductBusinessWaiter");
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 747);
					if (returnedSaveFuncInfoWithValue.HasError)
					{
						ret.AddError("product send waiter contains error!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 748);
					}
					if (!ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
					{
						await UpdateServerStatusAsync(ContextGuid, new List<Guid> { Guid }, ServerStatusForClient.Delivered, token);
					}
				}
				sb.AppendLine("message:" + ret.Messages);
				sb.AppendLine("error:" + ret.GetErrorMessage());
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 755);
			}
			catch (Exception ex)
			{
				sb.AppendLine(ex.Message);
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 759);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 760);
			}
			finally
			{
				sb.AppendLine($"endTime:{DateTime.Now}");
			}
			return ret;
		}
	}

	public Task<WebProduct> ToWebItemAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ProductMapper.Instance.MapAsync(ContextGuid, this, new WebProduct(), tranName, token);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), short autoFixValidationErrorsCount = 0, Dictionary<Guid, List<PrdTechnicalBusiness>> prdTechnicalList = null, Dictionary<Guid, List<PrdAnbarRelationBusiness>> prdAnbarRalationList = null, Dictionary<Guid, List<FileInfoBusiness>> fileInfo = null, string tranName = "", ReturnedState? state = null, ProductBusiness masterProduct = null)
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			ReturnedSaveFuncInfo result;
			try
			{
				_ = 31;
				try
				{
					ClsCache cache = ClsCache.GetCache(ContextGuid);
					ENSource source = cache.Source;
					if (cache.Source == ENSource.Android_HesabYar && system != EnSystem.UserInterface)
					{
						result = ret;
						goto IL_2f97;
					}
					CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await FillSkuAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 779);
					}
					bool addError = !state.HasValue || (state.Value & ReturnedState.Error) == ReturnedState.Error;
					bool addWarning = !state.HasValue || (state.Value & ReturnedState.Warning) == ReturnedState.Warning;
					bool addInfo = !state.HasValue || (state.Value & ReturnedState.Information) == ReturnedState.Information;
					if (Kind == EnProductKind.Decimal && !IsDependentOnCurrency)
					{
						IsDependentOnCurrency = true;
					}
					if (system == EnSystem.UserInterface && string.IsNullOrEmpty(IdProduct) && Status)
					{
						ret.AddError("لطفا کد کالای".GetTranslate() + "(" + Name + ") " + "را وارد نمایید.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 786);
					}
					if (Guid == Guid.Empty && addError)
					{
						ret.AddError("Guid is empty", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 788);
					}
					line = 5;
					ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, IdProduct, system, null, tranName, null, token);
					if (productCompactBusiness == null)
					{
						productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, Code.ToString(), system, null, tranName, null, token);
					}
					if (productCompactBusiness != null && productCompactBusiness.Guid != Guid)
					{
						if (autoFixValidationErrorsCount > 0 && (system == EnSystem.SignalR || system == EnSystem.Transfer))
						{
							if (Status)
							{
								ProductBusiness exists = await GetAsync(ContextGuid, productCompactBusiness.Guid, token, tranName);
								ProductBusiness productBusiness = exists;
								productBusiness.Code = await GetNextCodeAsync(ContextGuid, exists.FirstGroupGuid ?? Guid.Empty, null, token, tranName);
								exists.IdProduct = exists.Code.ToString();
								CashiersLogBusiness.OtherLogsAsync(ContextGuid, (system == EnSystem.SignalR) ? EnSystem.System : system, source != ENSource.Accounting, token, EnLog.ProductLog, ENAction.Edit, "در حین ذخیره کالای " + Name + " به تداخل IdProduct خوردم به همین دلیل چون کالای دیگری در دیتابیس با همین کد وجود داشت و کالای جاری در وضعیت غیر فعال بود کد کالای جاری را تغییر دادم.", Guid, awaitOnSemaphore: false);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await exists.SaveAsync(sendToServer: true, 0, token, system, awaitOnSemaphore: false, await4send: false, "", "", 60000, null, null, 10), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 805);
							}
							else
							{
								CashiersLogBusiness.OtherLogsAsync(ContextGuid, (system == EnSystem.SignalR) ? EnSystem.System : system, source != ENSource.Accounting, token, EnLog.ProductLog, ENAction.Edit, "به علت اینکه کد(" + IdProduct + ") کالا به کالای دیگری(" + productCompactBusiness.Name + ") اختصاص داده شده بوده مجبورم کد را تغییر بدم.", Guid, awaitOnSemaphore: false);
								Code = await GetNextCodeAsync(ContextGuid, FirstGroupGuid ?? Guid.Empty, null, token, tranName);
								IdProduct = Code.ToString();
							}
							returnedSaveFuncInfo = ret;
							ProductBusiness productBusiness2 = this;
							CancellationToken token2 = token;
							short num = checked((short)(autoFixValidationErrorsCount - 1));
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await productBusiness2.CheckValidationAsync(system, token2, num, prdTechnicalList, prdAnbarRalationList, fileInfo, tranName, state), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 814);
							result = ret;
							goto IL_2f97;
						}
						if (addError && Status)
						{
							await ret.AddErrorAsync("کد وارد شده تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 818);
						}
					}
					bool valueOrDefault = (prdAnbarRalationList?.Where((KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> q) => q.Value?.Any() ?? false)?.SelectMany((KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> q) => q.Value)?.Any((PrdAnbarRelationBusiness p) => p.Amount < 0m)).GetValueOrDefault();
					if (autoFixValidationErrorsCount <= 0 && valueOrDefault && Mojodi == ProductMojodiSt.DisableMojodiManfi)
					{
						foreach (KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> item in prdAnbarRalationList)
						{
							if (!(item.Value?.Any() ?? false))
							{
								continue;
							}
							foreach (PrdAnbarRelationBusiness item3 in item.Value)
							{
								_ = item3;
								string[] value = await Task.WhenAll(item.Value.Select(async delegate(PrdAnbarRelationBusiness p)
								{
									string translate = "انبار".GetTranslate();
									DepotBusiness depotBusiness2 = await DepotBusiness.GetAsync(ContextGuid, p.AnbarGuid, token, tranName);
									return translate + "(" + depotBusiness2?.Name + ")" + " موجودی".GetTranslate() + ":" + $"{p.Amount}";
								}));
								if (addError)
								{
									ret.AddError(system, "به دلیل اینکه این کالا دارای موجودی منفی است مجاز به غیر فعال نمودن خصوصیت موجودی منفی برای آن نخواهید بود".GetTranslate() + "\r\n" + string.Join("\r\n", value), "Mojodi", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 832);
								}
							}
						}
					}
					if (autoFixValidationErrorsCount <= 0 && (prdAnbarRalationList?.Where((KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> q) => q.Value?.Any() ?? false)?.SelectMany((KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> q) => q.Value)?.Any((PrdAnbarRelationBusiness p) => p.Amount < 0m)).GetValueOrDefault() && Mojodi == ProductMojodiSt.DisableMojodiManfi)
					{
						foreach (KeyValuePair<Guid, List<PrdAnbarRelationBusiness>> prdAnbarRalation in prdAnbarRalationList)
						{
							if (!(prdAnbarRalation.Value?.Any() ?? false))
							{
								continue;
							}
							foreach (PrdAnbarRelationBusiness item2 in prdAnbarRalation.Value)
							{
								if (item2 != null && item2.Amount < 0m)
								{
									DepotBusiness depotBusiness = await DepotBusiness.GetAsync(ContextGuid, item2.AnbarGuid, token, tranName);
									if (depotBusiness != null && depotBusiness.NegativeAmount == ReturnedState.Error && addError)
									{
										ret.AddError(system, "به دلیل اینکه موجودی منفی برای انبار".GetTranslate() + " (" + depotBusiness.Name + ") " + " غیر فعال است".GetTranslate() + "\r\n" + "و همچنین موجودی این کالا در".GetTranslate() + "  " + depotBusiness.Name + " " + "به میزان".GetTranslate() + " " + $" {item2.Amount}" + " منفی میباشد".GetTranslate() + "\r\n" + "امکان تغیرر وضعیت موجودی بر اساس انبار نمیباشید.".GetTranslate(), "Mojodi", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 850);
									}
									else if (depotBusiness != null && depotBusiness.NegativeAmount == ReturnedState.Warning && addWarning)
									{
										ret.AddWarning("موجودی منفی برای انبار".GetTranslate() + " (" + depotBusiness.Name + ")  " + "غیر فعال است".GetTranslate() + "\r\n" + "و همچنین موجودی این کالا در ".GetTranslate() + " " + (depotBusiness.Name + " ").GetTranslate() + " " + "به میزان ".GetTranslate() + " " + $"{item2.Amount}" + " " + " منفی میباشد".GetTranslate(), "Mojodi", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 855);
									}
								}
							}
						}
					}
					line = 15;
					bool flag = GuidVahed == Guid.Empty;
					if (!flag)
					{
						flag = await PrdVahedBusiness.GetAsync(ContextGuid, GuidVahed, token) == null;
					}
					if (flag)
					{
						if (autoFixValidationErrorsCount > 0)
						{
							Guid currentVahedGuid = GuidVahed;
							GuidVahed = (await PrdVahedBusiness.GetAllAsync(ContextGuid, true, token, tranName))?.FirstOrDefault()?.Guid ?? Guid.Empty;
							if (addInfo)
							{
								ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n ChangeVahedGuid From:({currentVahedGuid}) To:({GuidVahed})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 868);
							}
							returnedSaveFuncInfo = ret;
							ProductBusiness productBusiness3 = this;
							CancellationToken token3 = token;
							short num = checked((short)(autoFixValidationErrorsCount - 1));
							autoFixValidationErrorsCount = num;
							returnedSaveFuncInfo.AddReturnedValue(await productBusiness3.CheckValidationAsync(system, token3, num, prdTechnicalList, prdAnbarRalationList, fileInfo, tranName, state), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 869);
							result = ret;
							goto IL_2f97;
						}
						if (addError)
						{
							ret.AddError("واحد کالا انتخاب نشده است".GetTranslate() + " \r\n" + "نام کالا".GetTranslate() + ":" + Name + "\r\n" + "کد کالا".GetTranslate() + ":" + $"{Code}\r\n" + "Guid کالا".GetTranslate() + ":" + $"{Guid}\r\n" + "Guid واحد کالا".GetTranslate() + ":" + $"{GuidVahed}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 873);
						}
					}
					line = 20;
					if (string.IsNullOrEmpty(Name))
					{
						if (system == EnSystem.UserInterface && addError)
						{
							ret.AddError("نام کالا خالی می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 881);
						}
						else
						{
							Name = "نام کالا خالی بوده به همین دلیل سیستم نام کالا را تغییر داد".GetTranslate();
						}
					}
					line = 25;
					if (system == EnSystem.UserInterface)
					{
						OrderBusiness orderBusiness = new OrderBusiness(ContextGuid)
						{
							FactorType = Document.OrderBuy
						};
						ProductCompactFilter filter = new ProductCompactFilter
						{
							SearchText = Name,
							Order = orderBusiness,
							Sort = (cashier?.Settings?.Products?.ProductSort).GetValueOrDefault()
						};
						if ((await ProductCompactBusiness.GetAllAsync(ContextGuid, system, filter, token, tranName))?.FirstOrDefault((ProductCompactBusiness s) => s.Name == Name && s.Guid != Guid) != null && addInfo)
						{
							ret.AddInformation("نام کالا".GetTranslate() + "(" + Name + ")" + "تکراری است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 896);
						}
					}
					line = 30;
					checked
					{
						if (DiscountPercent > 100m)
						{
							if (autoFixValidationErrorsCount > 0)
							{
								decimal discountPercent = DiscountPercent;
								DiscountPercent = 0m;
								if (addInfo)
								{
									ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n ChangeDiscountPercent From:({discountPercent}) To:({DiscountPercent})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 906);
								}
								returnedSaveFuncInfo = ret;
								ProductBusiness productBusiness4 = this;
								CancellationToken token4 = token;
								short num = (short)(autoFixValidationErrorsCount - 1);
								autoFixValidationErrorsCount = num;
								returnedSaveFuncInfo.AddReturnedValue(await productBusiness4.CheckValidationAsync(system, token4, num, prdTechnicalList, prdAnbarRalationList, fileInfo, tranName, state), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 907);
								result = ret;
								goto IL_2f97;
							}
							if (addError)
							{
								ret.AddError("درصد تخفیف نمی تواند بزرگتر از 100 باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 911);
							}
						}
						line = 50;
						if (DiscountPercent < 0m)
						{
							if (autoFixValidationErrorsCount > 0)
							{
								decimal discountPercent2 = DiscountPercent;
								DiscountPercent = 0m;
								if (addInfo)
								{
									ret.AddInformation($"autoFixValidationErrorsCount:({autoFixValidationErrorsCount}) \r\n ChangeDiscountPercent From:({discountPercent2}) To:({DiscountPercent})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 920);
								}
								ProductBusiness productBusiness5 = this;
								CancellationToken token5 = token;
								short num = (short)(autoFixValidationErrorsCount - 1);
								autoFixValidationErrorsCount = num;
								result = await productBusiness5.CheckValidationAsync(system, token5, num, prdTechnicalList, prdAnbarRalationList, fileInfo, tranName, state);
								goto IL_2f97;
							}
							if (addError)
							{
								ret.AddError("درصد تخفیف نمی تواند کوچکتر از 0 باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 924);
							}
						}
						line = 60;
						if (token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 927);
							goto IL_2f97;
						}
						BarcodeBusiness barcodeBusiness = await BarcodeBusiness.GetAsync(ContextGuid, IdProduct, token, tranName);
						line = 65;
						if (barcodeBusiness != null && barcodeBusiness.PrdGuid != Guid)
						{
							if (autoFixValidationErrorsCount > 0)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await BarcodeBusiness.RemoveAsync(ContextGuid, IdProduct, token, 1, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 934);
								returnedSaveFuncInfo = ret;
								ProductBusiness productBusiness6 = this;
								CancellationToken token6 = token;
								short num = (short)(autoFixValidationErrorsCount - 1);
								autoFixValidationErrorsCount = num;
								returnedSaveFuncInfo.AddReturnedValue(await productBusiness6.CheckValidationAsync(system, token6, num, prdTechnicalList, prdAnbarRalationList, fileInfo, tranName, state), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 935);
								result = ret;
								goto IL_2f97;
							}
							if (addError)
							{
								ret.AddError(system, "کد کالا با بارکد یک کالا مشابه است".GetTranslate() + "\r\n" + IdProduct + "\r\n" + Name + "\r\n" + $"{Guid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 939);
							}
						}
						line = 70;
						if (fileInfo != null)
						{
							foreach (KeyValuePair<Guid, List<FileInfoBusiness>> item4 in fileInfo)
							{
								if (!(item4.Value?.Any() ?? false))
								{
									continue;
								}
								foreach (FileInfoBusiness item5 in item4.Value)
								{
									if (item5 != null)
									{
										ret.AddReturnedValue(item5.CheckValidation(system, autoFixValidationErrorsCount), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 951);
									}
								}
							}
						}
					}
					if (autoFixValidationErrorsCount <= 0 && Kind != EnProductKind.Service)
					{
						IEnumerable<DepotBusiness> enumerable = await DepotBusiness.GetAllAsync(ContextGuid, system, true, tranName, token);
						if (enumerable != null && enumerable.Any())
						{
							foreach (DepotBusiness depot in enumerable)
							{
								if (depot != null)
								{
									PrdAnbarRelationBusiness prdAnbarRelationBusiness = await PrdAnbarRelationBusiness.GetAsync(ContextGuid, system, Guid, depot.Guid, token, tranName);
									if (prdAnbarRelationBusiness != null && prdAnbarRelationBusiness.Amount < 0m && Mojodi == ProductMojodiSt.DisableMojodiManfi && addError)
									{
										ret.AddError(system, "موجودی".GetTranslate() + " " + (Name ?? "").GetTranslate() + " " + " در".GetTranslate() + " " + (" " + depot.Name).GetTranslate() + " " + " به میزان".GetTranslate() + $"{Math.Abs(prdAnbarRelationBusiness.Amount)}" + " " + " منفی می باشد ".GetTranslate() + "\r\n" + "در چنین حالتی امکان غیرفعال کردن موجودی منفی وجود ندارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 965);
									}
								}
							}
						}
					}
					if (system == EnSystem.UserInterface && (IdProduct?.Length ?? 0) == 13 && IdProduct.RemoveNoNumbers() == IdProduct && !IdProduct.IsValidEAN13Barcode() && addWarning)
					{
						ret.AddWarning("رقم انتهایی مجاز برای بارکد".GetTranslate() + " EAN13\r\n" + $"{IdProduct.Checksum_EAN13Barcode()}" + "میباشد.".GetTranslate() + "\r\nبارکد وارد شده: " + IdProduct + "\r\n" + $"مقدار صحیح بارکد: {IdProduct.Substring(0, 12)}{IdProduct.Checksum_EAN13Barcode()} \r\n" + "https://novinhozor.ir/lastarticle/ean13-barcode", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 975);
					}
					if (system == EnSystem.UserInterface)
					{
						if (V2Qt != 0m && (V2Guid ?? Guid.Empty) == Guid.Empty && addError)
						{
							ret.AddError(system, "لطفا واحد دوم کالا را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 983);
						}
						if (V3Qt != 0m && (V3Guid ?? Guid.Empty) == Guid.Empty && addError)
						{
							ret.AddError(system, "لطفا واحد سوم کالا را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 984);
						}
						if (V4Qt != 0m && (V4Guid ?? Guid.Empty) == Guid.Empty && addError)
						{
							ret.AddError(system, "لطفا واحد چهارم کالا را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 985);
						}
						if (IsTax && (TaxGuid ?? Guid.Empty) == Guid.Empty)
						{
							ret.AddError("لطفا مالیات کالا را وارد نمایید".GetTranslate() + "  (" + IdProduct + ")", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 986);
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationPriceAsync(system, state, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 989);
					if (system == EnSystem.Transfer)
					{
						result = ret;
						goto IL_2f97;
					}
					if (Kind != EnProductKind.Variation)
					{
						ProductBusiness productBusiness7 = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
						if ((productBusiness7?.Kind ?? EnProductKind.SimpleProduct) == EnProductKind.Variation && (productBusiness7?.ProductAttributes?.Any()).GetValueOrDefault())
						{
							ret.AddError(system, "کالای".GetTranslate() + (" " + Name).GetTranslate() + " " + " تعداد".GetTranslate() + " " + $" {productBusiness7.ProductAttributes.Count()}" + " " + " تنوع دارد و امکان تغییر نوع کالا از تنوع کالایی به".GetTranslate() + "  " + Kind.GetDisplay().GetTranslate() + " " + " وجود ندارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 996);
						}
					}
					if (Kind == EnProductKind.Decimal && Status && DecimalMetaData != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await DecimalMetaData.CheckValidationAsync(), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 999);
					}
					if (cache.Source == ENSource.Accounting)
					{
						switch (system)
						{
						case EnSystem.SignalR:
						{
							ProductCompactBusiness productCompactBusiness2 = await ProductCompactBusiness.GetAsync(ContextGuid, IdProduct, system, null, tranName, null, token);
							if (productCompactBusiness2 != null && productCompactBusiness2.Guid != Guid && (FirstGroupGuid ?? Guid.Empty) != Guid.Empty)
							{
								IdProduct = (Code = await GetNextCodeAsync(ContextGuid, FirstGroupGuid.Value, null, token, tranName)).ToString();
								ServerStatus = ServerStatusForClient.None;
							}
							break;
						}
						case EnSystem.UserInterface:
							if ((MasterGuid ?? Guid.Empty) != Guid.Empty && masterProduct != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await CheckVariationValidationAsync(system, masterProduct, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1014);
							}
							break;
						}
						if (system == EnSystem.UserInterface && (MasterGuid ?? Guid.Empty) == Guid.Empty && (ProductAttributes?.Any() ?? false) && Status)
						{
							List<ProductCompactBusiness> list = await CheckAttributeValidationAsync(system, token, tranName);
							if (list != null && list.Any())
							{
								foreach (ProductCompactBusiness item6 in list)
								{
									await ret.AddErrorAsync("لطفا ویژگی های کالای ".GetTranslate() + " " + (item6.Name + " ").GetTranslate() + " " + "را به درستی وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1020);
								}
							}
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckDiscountValidationAsync(token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1023);
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1025);
				}
				catch (Exception ex)
				{
					if (token.IsCancellationRequested)
					{
						Log.Warning(ex, $"#Line_{line}");
					}
					else
					{
						WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1029);
					}
					ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1030);
				}
			}
			finally
			{
				if (!token.IsCancellationRequested && ret.HasError && system != EnSystem.UserInterface)
				{
					WebErrorLog instance = WebErrorLog.Instance;
					string text = $"system:{system}\r\n";
					string errorMessage = ret.GetErrorMessage();
					WebProduct webProduct = await ToWebItemAsync(token);
					instance.StartLog("Product validation error\r\n" + text + "Error:" + errorMessage + "\r\n\ud83d\udfe2WebPrd:" + webProduct?.ToString(isIndented: true), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1035);
				}
			}
			if (Guid == Guid.Parse("C110EDD4-6548-4368-9D11-2094ADA1DE37") && ret.HasError)
			{
				Console.WriteLine("http://ret.ErrorMessage/***************************/*");
			}
			else if (Guid == Guid.Parse("C110EDD4-6548-4368-9D11-2094ADA1DE37"))
			{
				Console.WriteLine("http://CheckValidateisOk/***************************/*");
			}
			return ret;
			IL_2f97:
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckVariationValidationAsync(EnSystem system, ProductBusiness masterProduct, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (system != EnSystem.UserInterface || (MasterGuid ?? Guid.Empty) == Guid.Empty || masterProduct == null)
			{
				return res;
			}
			IEnumerable<ProductAttributeRelationBusiness> attributeRelation = masterProduct.AttributeRelations?.Where((ProductAttributeRelationBusiness q) => q.ProductGuid == Guid);
			List<Guid> list = (await GetAttributeGuidListAsync(token, tranName))?.Where((Guid q) => !(attributeRelation?.Select((ProductAttributeRelationBusiness q) => q.AttributeGuid)?.Contains(q)).GetValueOrDefault())?.ToList();
			if (list != null && list.Any())
			{
				foreach (Guid item in list)
				{
					ProductAttributeBusiness productAttributeBusiness = await ProductAttributeBusiness.GetAsync(ContextGuid, item, token, tranName);
					if (productAttributeBusiness != null)
					{
						await res.AddErrorAsync("لطفا مقدار ویژگی".GetTranslate() + " " + (" " + productAttributeBusiness.Name).GetTranslate() + " را برای کالای".GetTranslate() + "  " + IdProduct + "-" + Name + " " + " وارد نمایید".GetTranslate(), "", "CheckVariationValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1061);
					}
				}
			}
			IEnumerable<IGrouping<string, ProductBusiness>> enumerable = masterProduct.ProductAttributes?.GroupBy((ProductBusiness q) => q.Sku);
			if (enumerable != null && enumerable.Any())
			{
				foreach (IGrouping<string, ProductBusiness> item2 in enumerable)
				{
					if (item2 == null)
					{
						continue;
					}
					foreach (ProductBusiness item3 in item2)
					{
						item3.Sku = Guid.NewGuid().ToString().WordPressFixSlugOrSku();
					}
				}
			}
			IEnumerable<IGrouping<string, ProductBusiness>> enumerable2 = masterProduct.ProductAttributes?.GroupBy((ProductBusiness q) => q.Slug);
			if (enumerable2 != null && enumerable2.Any())
			{
				foreach (IGrouping<string, ProductBusiness> item4 in enumerable2)
				{
					if (item4 == null)
					{
						continue;
					}
					foreach (ProductBusiness item5 in item4)
					{
						item5.Slug = Guid.NewGuid().ToString().WordPressFixSlugOrSku();
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckVariationValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1085);
			res.AddReturnedValue(ex, "", "CheckVariationValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1086);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckDiscountValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!DiscountEndDate.HasValue && !DiscountStartDate.HasValue)
			{
				return res;
			}
			if (DiscountEndDate.HasValue && DiscountStartDate.HasValue && DiscountStartDate > DiscountEndDate)
			{
				await res.AddErrorAsync("تاریخ ابتدای بازه زمانی اعمال تخفیف، از تاریخ انتها نمی تواند بزرگتر باشد".GetTranslate(), "", "CheckDiscountValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1097);
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			DateTime maliyearStart = accGlobalSettings.MaliYearSettings.MaliyearStart;
			DateTime endDate = accGlobalSettings.MaliYearSettings.MaliyearEnd;
			if (DiscountStartDate.HasValue && DiscountStartDate < maliyearStart)
			{
				await res.AddErrorAsync("تاریخ ابتدای بازه زمانی اعمال تخفیف در دوره مالی جاری قرار ندارد ".GetTranslate() + "\r\n" + " تاریخ آغاز دوره مالی".GetTranslate() + ": " + maliyearStart.MiladiToCalture(), "", "CheckDiscountValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1104);
			}
			if (DiscountEndDate.HasValue && DiscountEndDate > endDate)
			{
				await res.AddErrorAsync("تاریخ انتهای بازه زمانی اعمال تخفیف در دوره مالی جاری قرار ندارد".GetTranslate() + " \r\n" + " تاریخ پایان دوره مالی".GetTranslate() + ": " + endDate.MiladiToCalture(), "", "CheckDiscountValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1107);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckDiscountValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1111);
			res.AddReturnedValue(ex, "", "CheckDiscountValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1112);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> FillSkuAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(Sku))
				{
					if ((MasterGuid ?? Guid.Empty) == Guid.Empty)
					{
						Sku = Name.WordPressFixSlugOrSku();
					}
					else
					{
						Sku = Guid.ToString().WordPressFixSlugOrSku();
					}
				}
				if (string.IsNullOrEmpty(Slug))
				{
					if ((MasterGuid ?? Guid.Empty) == Guid.Empty)
					{
						Slug = Name.WordPressFixSlugOrSku();
					}
					else
					{
						Slug = Guid.ToString().WordPressFixSlugOrSku();
					}
				}
				if (Sku.EndsWith("-"))
				{
					Sku = Sku.Substring(0, Sku.Length - 1);
				}
				if (Slug.EndsWith("-"))
				{
					Slug = Slug.Substring(0, Slug.Length - 1);
				}
				ProductBusiness productBusiness = await GetBySkuAsync(ContextGuid, Sku, token, tranName);
				int i = 0;
				while (productBusiness != null && productBusiness.Guid != Guid)
				{
					if (i > 200)
					{
						throw new Exception();
					}
					if (!productBusiness.Status)
					{
						productBusiness.Sku = productBusiness.Guid.ToString();
						productBusiness.ServerStatus = ServerStatusForClient.None;
						productBusiness.Modified = DateTime.Now;
						await productBusiness.SaveAsync(sendToServer: true, 1, token, system, awaitOnSemaphore: false, await4send: false, "", "", 60000, null, null, 10, tranName);
					}
					else
					{
						ProductBusiness productBusiness2 = this;
						string name = Name;
						int num = i + 1;
						i = num;
						productBusiness2.Sku = name.WordPressFixSlugOrSku(num);
					}
					if (string.IsNullOrEmpty(Sku))
					{
						Sku = Guid.ToString();
					}
					productBusiness = await GetBySkuAsync(ContextGuid, Sku, token, tranName);
				}
				ProductBusiness productBusiness3 = await GetBySlugAsync(ContextGuid, Slug, token, tranName);
				i = 0;
				while (productBusiness3 != null && productBusiness3.Guid != Guid)
				{
					if (!productBusiness3.Status)
					{
						productBusiness3.Slug = productBusiness3.Guid.ToString();
						productBusiness3.ServerStatus = ServerStatusForClient.None;
						productBusiness3.Modified = DateTime.Now;
						await productBusiness3.SaveAsync(sendToServer: true, 1, token, system, awaitOnSemaphore: false, await4send: false, "", "", 60000, null, null, 10, tranName);
					}
					else
					{
						ProductBusiness productBusiness4 = this;
						string name2 = Name;
						int num = i + 1;
						i = num;
						productBusiness4.Slug = name2.WordPressFixSlugOrSku(num);
					}
					if (string.IsNullOrEmpty(Slug))
					{
						Slug = Guid.ToString();
					}
					productBusiness3 = await GetBySlugAsync(ContextGuid, Slug, token, tranName);
				}
				if (system != EnSystem.SignalR || ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					if (Sku != Sku.WordPressFixSlugOrSku())
					{
						Sku = Sku.WordPressFixSlugOrSku();
					}
					if (Slug != Slug.WordPressFixSlugOrSku())
					{
						Slug = Slug.WordPressFixSlugOrSku();
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillSkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1183);
				ret.AddReturnedValue(ex, "", "FillSkuAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1184);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationPriceAsync(EnSystem system, ReturnedState? state, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			bool addError = !state.HasValue || (state.Value & ReturnedState.Error) == ReturnedState.Error;
			List<ProductPriceRelationBusiness> list = PriceList?.Where((ProductPriceRelationBusiness q) => q.Price > 0m)?.ToList();
			if (list == null || !list.Any())
			{
				return res;
			}
			foreach (ProductPriceRelationBusiness item in list)
			{
				if (item == null)
				{
					continue;
				}
				decimal num = item.Price.FixDecimal(18, 3);
				if (item.Price != num)
				{
					if (system == EnSystem.UserInterface && addError)
					{
						ProductPriceLevelBusiness productPriceLevelBusiness = await ProductPriceLevelBusiness.GetAsync(ContextGuid, item.PriceLevelGuid, EnSystem.None, token, tranName);
						await res.AddErrorAsync("قیمت سطح ".GetTranslate() + " " + (productPriceLevelBusiness?.Name ?? "") + " " + "کالا صحیح نمی باشد".GetTranslate(), "", "CheckValidationPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1206);
					}
					else
					{
						item.Price = num;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1214);
			res.AddReturnedValue(ex, "", "CheckValidationPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1215);
		}
		return res;
	}

	public void ClearGroupRelationList()
	{
		_prdGroupRelation?.Clear();
	}

	public void AddRangeToGroupRelationList(IEnumerable<ProductGroupRelationBusiness> detList, bool emptyList)
	{
		using (new JaegerMonitor("AddRangeToGroupRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			try
			{
				if (detList == null || !detList.Any())
				{
					return;
				}
				if (emptyList)
				{
					_prdGroupRelation = new List<ProductGroupRelationBusiness>();
				}
				if (_prdGroupRelation == null)
				{
					_prdGroupRelation = new List<ProductGroupRelationBusiness>();
				}
				foreach (ProductGroupRelationBusiness item in detList)
				{
					if (_prdGroupRelation?.FirstOrDefault((ProductGroupRelationBusiness q) => q.GroupGuid == item.GroupGuid) == null)
					{
						item.ProductGuid = Guid;
						_prdGroupRelation.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToGroupRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1238);
				throw;
			}
		}
	}

	public void AddToGroupRelationList(Guid groupGuid)
	{
		using (new JaegerMonitor("AddToGroupRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			try
			{
				if (groupGuid == Guid.Empty)
				{
					return;
				}
				if (_prdGroupRelation == null)
				{
					_prdGroupRelation = new List<ProductGroupRelationBusiness>();
				}
				if (_prdGroupRelation?.FirstOrDefault((ProductGroupRelationBusiness q) => q.GroupGuid == groupGuid) == null)
				{
					ProductGroupRelationBusiness productGroupRelationBusiness = _prdGroupRelation?.OrderByDescending((ProductGroupRelationBusiness q) => q.Ordinal)?.Take(1)?.FirstOrDefault();
					_prdGroupRelation.Add(new ProductGroupRelationBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						GroupGuid = groupGuid,
						Ordinal = checked((short)((productGroupRelationBusiness?.Ordinal ?? 0) + 1)),
						ProductGuid = Guid
					});
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToGroupRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1262);
				throw;
			}
		}
	}

	public void RemoveFromGroupRelationList(Guid groupGuid)
	{
		using (new JaegerMonitor("RemoveFromGroupRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			try
			{
				if (!(groupGuid == Guid.Empty))
				{
					if (_prdGroupRelation == null)
					{
						_prdGroupRelation = new List<ProductGroupRelationBusiness>();
					}
					ProductGroupRelationBusiness productGroupRelationBusiness = _prdGroupRelation?.FirstOrDefault((ProductGroupRelationBusiness q) => q.GroupGuid == groupGuid);
					if (productGroupRelationBusiness != null)
					{
						_prdGroupRelation.Remove(productGroupRelationBusiness);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromGroupRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1279);
				throw;
			}
		}
	}

	public void RemoveFromPriceRelationList(Guid priceLevelGuid)
	{
		using (new JaegerMonitor("RemoveFromPriceRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			try
			{
				if (!(priceLevelGuid == Guid.Empty))
				{
					if (_priceList == null)
					{
						_priceList = new List<ProductPriceRelationBusiness>();
					}
					ProductPriceRelationBusiness productPriceRelationBusiness = _priceList?.FirstOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceLevelGuid);
					if (productPriceRelationBusiness != null)
					{
						_priceList.Remove(productPriceRelationBusiness);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromPriceRelationList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1296);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(string contextGuid, List<Guid> productGuids, ServerStatusForClient status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1308), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1308);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateServerStatusAsync(productGuids, status, token, tranName), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1310);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1314);
				ret.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1315);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1320), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1320);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateAmountAsync(string contextGuid, ProductsChanges prd, bool rollback, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.UpdateAccountAsync(prd, rollback, tryCount, token, tranName), ReturnedState.None, "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1329);
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
			{
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await UpdateServerStatusAsync(contextGuid, new List<Guid> { prd.PrdGuid }, ServerStatusForClient.None, token, tranName), ReturnedState.None, "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1331);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1335);
			ret.AddReturnedValue(ex, "", "UpdateAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1336);
		}
		return ret;
	}

	public static Task<ProductBusiness> GetByMappedIdAsync(string contextGuid, string mappedId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetByMappedIdAsync(mappedId, token, tranName);
	}

	public static Task<ProductBusiness> GetBySkuAsync(string contextGuid, string sku, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetBySkuAsync(sku, token, tranName);
	}

	public static Task<ProductBusiness> GetBySlugAsync(string contextGuid, string sku, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetBySlugAsync(sku, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, IEnumerable<WebProduct> items, CancellationToken token)
	{
		int line = 0;
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "productBusiness/SaveRangeForHesabyarAsync", 60000, token))
				{
					line = 4;
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1355);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1356), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1356);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.SaveRangeAsync(items, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1359);
							goto end_IL_0107;
						}
						result = ret;
					}
					goto end_IL_0070;
					end_IL_0107:;
				}
				goto end_IL_005f;
				end_IL_0070:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1362);
				goto end_IL_005f;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(contextGuid) == null)
				{
					Log.Warning(ex, $"#Line_:{line}//Cache is null!!");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_:{line}//Cache is not null.", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1368);
				}
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1369);
				goto end_IL_005f;
			}
			return result;
			end_IL_005f:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1373), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1373);
			if (ret.HasError && items.Count() == 1)
			{
				ret = await SaveRangeForHesabyarAsync(contextGuid, items, token);
			}
			else if (ret.HasError && items.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<WebProduct> list2 = new List<WebProduct>();
				WebProduct[] array = items.ToArray();
				for (int i = 0; i < items.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token));
				list.Add(SaveRangeForHesabyarAsync(contextGuid, items.Except(list2), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1385);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<SaveResponse>>> SaveRangeAsync(string contextGuid, short autoFixValidationErrorsCount, IEnumerable<WebProduct> items, CancellationToken token, bool updateAmount, short MaxParallelTask, EnSystem system, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<List<SaveResponse>> ret = new ReturnedSaveFuncInfoWithValue<List<SaveResponse>>
		{
			value = new List<SaveResponse>()
		};
		using (new JaegerMonitor("SaveRangeAsync_1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _saveListSemaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar && system == EnSystem.SignalR)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SaveRangeForHesabyarAsync(contextGuid, items, token);
					ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1404);
					ret.value = items.Select((WebProduct p) => new SaveResponse(0, p.Guid, DateTime.Now, !ret.HasError, ret.GetErrorMessage(), ret.GetErrorMessage())).ToList();
					return ret;
				}
				semaphoreHasBeenTaken = true;
				CancellationTokenSource invokeToken2 = new CancellationTokenSource();
				int current = 0;
				line = 1;
				line = 10;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1413);
				}
				ret.throwExceptionIfError();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1415);
				}
				line = 13;
				items = items?.OrderByDescending((WebProduct q) => q.MasterGuid);
				List<Task<ReturnedSaveFuncInfoWithValue<SaveResponse>>> taskList = new List<Task<ReturnedSaveFuncInfoWithValue<SaveResponse>>>();
				ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue = ret;
				if (returnedSaveFuncInfoWithValue.value == null)
				{
					returnedSaveFuncInfoWithValue.value = new List<SaveResponse>();
				}
				List<SaveResponse> value;
				if (items != null && items.Any())
				{
					foreach (WebProduct webProduct in items)
					{
						current = checked(current + 1);
						if (webProduct.ClientVersion <= 2)
						{
							ProductBusiness oldItem = await GetAsync(contextGuid, webProduct.Guid, token);
							if (oldItem != null)
							{
								if (webProduct.ClientVersion <= 0)
								{
									webProduct.RegularPrice = (oldItem.PriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == ParentDefaults.PriceLevel.RegularPrice)?.Price).GetValueOrDefault();
								}
								WebProduct webProduct2;
								if (webProduct.ClientVersion <= 1)
								{
									webProduct2 = webProduct;
									webProduct2.TaxName = await oldItem.GetTaxNameAsync(token);
									webProduct2 = webProduct;
									webProduct2.TaxCode = await oldItem.GetTaxCodeAsync(token);
								}
								webProduct2 = webProduct;
								webProduct2.AttributeRelations = await ProductAttributeRelationMapper.Instance.MapListAsync(contextGuid, oldItem.AttributeRelations, new List<WebProductAttributeRelation>());
							}
						}
						taskList.Add(Task.Run(() => SaveWebPrdAsync(contextGuid, system, webProduct, updateAmount, autoFixValidationErrorsCount, token)));
						if (taskList.Count > MaxParallelTask)
						{
							Task<ReturnedSaveFuncInfoWithValue<SaveResponse>> res = await Task.WhenAny(taskList);
							ReturnedSaveFuncInfoWithValue<List<SaveResponse>> returnedSaveFuncInfoWithValue2 = ret;
							returnedSaveFuncInfoWithValue2.AddReturnedValue(await res, ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1441);
							taskList.Remove(res);
							value = ret.value;
							value.Add((await res).value);
							if (percentCompleted != null)
							{
								invokeToken2?.Cancel();
								invokeToken2 = new CancellationTokenSource(60000);
								CancellationToken token2 = invokeToken2.Token;
								percentCompleted(new PercenteCompletedEventArgs(items.Count(), current, webProduct.Name, token2));
							}
						}
					}
				}
				value = ret.value;
				value.AddRange((await Task.WhenAll(taskList)).Select((ReturnedSaveFuncInfoWithValue<SaveResponse> p) => p.value));
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token3 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(items.Count(), items.Count(), "", token3));
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1459);
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(contextGuid)?.IsDisposed ?? true)
				{
					Log.Warning(ex, $"Line:{line}//tokenCancelled:{token.IsCancellationRequested}. cache is null or disposed!!!");
				}
				else if (!token.IsCancellationRequested)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}//tokenCancelled:{token.IsCancellationRequested}/cahce is not null or disposed.", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1465);
				}
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1466);
			}
			finally
			{
				if (ret.HasError)
				{
					ret.value = new List<SaveResponse>(1)
					{
						new SaveResponse(0, Guid.Empty, DateTime.Now, saveResultStatus: false, ret.ToString(), ret.ToString())
					};
				}
				if (semaphoreHasBeenTaken)
				{
					_saveListSemaphore.Release();
				}
			}
			return ret;
		}
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<SaveResponse>> SaveWebPrdAsync(string contextGuid, EnSystem system, WebProduct webProduct, bool updateAmount, short autoFixValidationErrorsCount, CancellationToken token)
	{
		TaskCompletionSource<bool> saveProductTask = new TaskCompletionSource<bool>();
		ReturnedSaveFuncInfoWithValue<SaveResponse> ret = new ReturnedSaveFuncInfoWithValue<SaveResponse>();
		try
		{
			await SendProduct2ScaleAsync(contextGuid, webProduct, system, saveProductTask, token);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1482);
			}
			if (webProduct == null)
			{
				return ret;
			}
			List<PrdAnbarRelationBusiness> anbarRelations = null;
			if (webProduct?.PrdAnbarRelation != null && webProduct.PrdAnbarRelation.Any())
			{
				Console.WriteLine($"SaveProduct=>{webProduct.Name}/TotalAmount:{(webProduct?.PrdAnbarRelation?.Sum((WebPrdAnbarRelation p) => p?.Amount ?? 0m)).GetValueOrDefault(-1m)}/ data:{webProduct.ToString()}");
				token.ThrowIfCancellationRequested();
				anbarRelations = await PrdAnbarRelationMapper.Instance.MapListAsync(contextGuid, webProduct.PrdAnbarRelation, null, "", token);
				foreach (PrdAnbarRelationBusiness item2 in anbarRelations)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1492);
					}
					item2.PrdGuid = webProduct.Guid;
				}
			}
			List<PrdTechnicalBusiness> technicalList = null;
			if ((webProduct?.PrdTechnicalList?.Any()).GetValueOrDefault())
			{
				technicalList = await PrdTechnicalMapper.Instance.MapListAsync(contextGuid, webProduct.PrdTechnicalList, null, "", token);
				foreach (PrdTechnicalBusiness item3 in technicalList)
				{
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1503);
					}
					item3.ProductGuid = webProduct.Guid;
				}
			}
			else if (system == EnSystem.Transfer)
			{
				technicalList = new List<PrdTechnicalBusiness>();
				if (!string.IsNullOrEmpty(webProduct.T1))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle1,
						Modified = DateTime.Now,
						Name = webProduct.T1,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 1
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T2))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle2,
						Modified = DateTime.Now,
						Name = webProduct.T2,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 2
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T3))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle3,
						Modified = DateTime.Now,
						Name = webProduct.T3,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 3
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T4))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle4,
						Modified = DateTime.Now,
						Name = webProduct.T4,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 4
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T5))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle5,
						Modified = DateTime.Now,
						Name = webProduct.T5,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 5
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T6))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle6,
						Modified = DateTime.Now,
						Name = webProduct.T6,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 6
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T7))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle7,
						Modified = DateTime.Now,
						Name = webProduct.T7,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 7
					});
				}
				if (!string.IsNullOrEmpty(webProduct.T8))
				{
					technicalList.Add(new PrdTechnicalBusiness(contextGuid)
					{
						Code = "",
						Guid = Guid.NewGuid(),
						Key = webProduct.TechnicalInfoTitle8,
						Modified = DateTime.Now,
						Name = webProduct.T8,
						ProductGuid = webProduct.Guid,
						Status = true,
						TechnicalType = 8
					});
				}
			}
			ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue;
			if (system == EnSystem.SignalR && (ClsCache.GetCache(contextGuid)?.Source ?? ENSource.Accounting) == ENSource.Accounting)
			{
				if (await PrdVahedBusiness.GetAsync(contextGuid, webProduct.GuidVahed, token) == null && webProduct.Unit != null)
				{
					PrdVahedBusiness obj = await PrdVahedMapper.Instance.MapAsync(contextGuid, webProduct.Unit, new PrdVahedBusiness(contextGuid), "", token);
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await obj.SaveAsync(EnSystem.SignalR, 10, awaitOnSemaphore: true, send2Server: true, Priority.Low, await4Send: false, token, 10), ReturnedState.None, "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1606);
				}
				if (ret.HasError)
				{
					return ret;
				}
			}
			if (system == EnSystem.SignalR && (webProduct?.RelatedAttributes?.Any()).GetValueOrDefault())
			{
				List<ProductAttributeBusiness> lst = await ProductAttributeMapper.Instance.MapListAsync(contextGuid, webProduct.RelatedAttributes, new List<ProductAttributeBusiness>(), "", token);
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await ProductAttributeBusiness.SaveRangeAsync(contextGuid, system, lst, Priority.Low, 10, "", token), ReturnedState.None, "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1615);
			}
			if (system == EnSystem.SignalR)
			{
				if ((webProduct?.RelatedAttributes?.Any()).GetValueOrDefault())
				{
					List<ProductAttributeBusiness> lst2 = await ProductAttributeMapper.Instance.MapListAsync(contextGuid, webProduct.RelatedAttributes, null, "", token);
					returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.AddReturnedValue(await ProductAttributeBusiness.SaveRangeAsync(contextGuid, system, lst2, Priority.Low, 10, "", token), ReturnedState.None, "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1623);
				}
				webProduct.AttributeRelations = null;
			}
			ProductBusiness item = await ProductMapper.Instance.MapAsync(contextGuid, webProduct, null, "", token);
			if (string.IsNullOrEmpty(item.Sku))
			{
				item.Sku = item.Guid.ToString();
			}
			if (string.IsNullOrEmpty(item.Slug))
			{
				item.Slug = item.Guid.ToString();
			}
			List<BarcodeBusiness> value = null;
			if (webProduct.PrdBarcode != null)
			{
				value = await BarcodeMapper.Instance.MapListAsync(contextGuid, webProduct.PrdBarcode, new List<BarcodeBusiness>(), "", token);
			}
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1634);
			}
			Dictionary<Guid, List<PrdAnbarRelationBusiness>> prdAnbarRalationList = new Dictionary<Guid, List<PrdAnbarRelationBusiness>> { { item.Guid, anbarRelations } };
			Dictionary<Guid, List<BarcodeBusiness>> barcodes = new Dictionary<Guid, List<BarcodeBusiness>> { { item.Guid, value } };
			Dictionary<Guid, List<PrdTechnicalBusiness>> prdTechnicalList = new Dictionary<Guid, List<PrdTechnicalBusiness>> { { item.Guid, technicalList } };
			returnedSaveFuncInfoWithValue = ret;
			CancellationToken token2 = token;
			bool updateAmount2 = updateAmount;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await item.SaveAsync(sendToServer: false, autoFixValidationErrorsCount, token2, system, awaitOnSemaphore: true, await4send: true, "", "", 60000, prdTechnicalList, prdAnbarRalationList, 10, "", null, updateAmount2, barcodes), ReturnedState.None, "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1640);
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1642);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1643);
		}
		finally
		{
			ret.value = new SaveResponse(0, webProduct.Guid, DateTime.Now, !ret.HasError, ret.GetErrorMessage(), ret.GetErrorMessage());
			saveProductTask.TrySetResult(!ret.HasError);
			if (ret.HasError)
			{
				WebErrorLog.Instance.StartLog($"Saving product({webProduct.Guid}) has error.{ret.ErrorMessage}\r\n" + webProduct.ToString(), "", "SaveWebPrdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1649);
			}
		}
		return ret;
	}

	private static async Task SendProduct2ScaleAsync(string contextGuid, WebProduct webProduct, EnSystem system, TaskCompletionSource<bool> saveProductTask, CancellationToken token = default(CancellationToken))
	{
	}

	private async Task RaiseOnSavedProductAsync(EnSystem system, WebProduct webProduct, TaskCompletionSource<bool> saveProductTask, ConcurrentDictionary<string, string> changes, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			bool flag = this.OnProductSaved != null;
			if (flag)
			{
				flag = await saveProductTask.Task;
			}
			if (flag)
			{
				ProductOnBusinessSavedEventArgs args = new ProductOnBusinessSavedEventArgs
				{
					Product = webProduct,
					System = system,
					Changes = changes
				};
				await PacketParser.Utilities.RaiseEvent(this.OnProductSaved, this, args, token, 60, "RaiseOnSavedProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseOnSavedProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1686);
		}
	}

	private static ConcurrentDictionary<string, string> GetPrdChanges(ProductCompactBusiness bus, WebProduct web)
	{
		ConcurrentDictionary<string, string> concurrentDictionary = new ConcurrentDictionary<string, string>();
		try
		{
			if (bus == null)
			{
				return concurrentDictionary;
			}
			if (bus.Amount != web.Amount)
			{
				concurrentDictionary.TryAdd("Amount", "موجودی از".GetTranslate() + " " + $" {bus.Amount}" + " " + " به".GetTranslate() + " " + $" {web.Amount}" + " " + " تغییر کرد".GetTranslate());
			}
			if (bus.Code != web.Code)
			{
				concurrentDictionary.TryAdd("Code", "کد(Long) از".GetTranslate() + " " + $" {bus.Code} " + " " + "به".GetTranslate() + " " + $" {web.Code}" + " " + " تغییر کرد".GetTranslate());
			}
			if (bus.Description != web.Description)
			{
				concurrentDictionary.TryAdd("Description", "شرح از".GetTranslate() + " " + bus.Description + " " + "به".GetTranslate() + " " + web.Description + " " + " تغییر کرد".GetTranslate());
			}
			if (bus.DiscountPercent != web.DiscountPercent)
			{
				concurrentDictionary.TryAdd("DiscountPercent", "درصد تخفیف از".GetTranslate() + " " + $"{bus.DiscountPercent}" + " به ".GetTranslate() + " " + $"{web.DiscountPercent}" + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.Guid != web.Guid)
			{
				concurrentDictionary.TryAdd("Guid", "Guid از".GetTranslate() + " " + $" {bus.Guid}" + " " + "به".GetTranslate() + " " + $" {web.Guid}" + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.GuidVahed != web.GuidVahed)
			{
				concurrentDictionary.TryAdd("GuidVahed", "واحد از".GetTranslate() + " " + $" {bus.GuidVahed}" + " " + "به ".GetTranslate() + " " + $"{web.GuidVahed}" + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.IdProduct != web.IdProduct)
			{
				concurrentDictionary.TryAdd("IdProduct", "کد از ".GetTranslate() + " " + (bus.IdProduct ?? "").GetTranslate() + " " + "به ".GetTranslate() + " " + web.IdProduct + "  " + "تغییر کرد".GetTranslate());
			}
			if (bus.IsTax != web.IsTax)
			{
				concurrentDictionary.TryAdd("IsTax", "IsTax از ".GetTranslate() + " " + $"{bus.IsTax} " + " " + "به ".GetTranslate() + " " + $"{web.IsTax} ".GetTranslate() + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.Name != web.Name)
			{
				concurrentDictionary.TryAdd("Name", "نام از ".GetTranslate() + " " + (bus.Name + " ").GetTranslate() + " " + "به ".GetTranslate() + " " + (web.Name ?? "").GetTranslate() + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.OrderPoint != web.OrderPoint)
			{
				concurrentDictionary.TryAdd("OrderPoint", "نقطه سفارش از ".GetTranslate() + " " + $"{bus.OrderPoint}" + " " + "به".GetTranslate() + " " + $"{web.OrderPoint}" + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.Pic != web.Pic)
			{
				concurrentDictionary.TryAdd("Pic", "تصویر از ".GetTranslate() + " " + (bus.Pic + " ").GetTranslate() + " " + "به".GetTranslate() + " " + web.Pic + "  " + "تغییر کرد".GetTranslate());
			}
			if (bus.Special != web.Special)
			{
				concurrentDictionary.TryAdd("Special", "Special از".GetTranslate() + " " + $" {bus.Special} " + " " + "به ".GetTranslate() + " " + $"{web.Special}" + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.Status != web.Status)
			{
				concurrentDictionary.TryAdd("Status", "وضعیت از ".GetTranslate() + " " + $"{bus.Status} " + " " + "به ".GetTranslate() + " " + $"{web.Status}".GetTranslate() + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.Type != web.Type)
			{
				concurrentDictionary.TryAdd("Type", "نوع از ".GetTranslate() + " " + $"{bus.Type} ".GetTranslate() + " " + "به ".GetTranslate() + " " + $"{web.Type} ".GetTranslate() + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.V2Guid != web.V2Guid)
			{
				concurrentDictionary.TryAdd("V2Guid", "واحد دوم از".GetTranslate() + " " + $"{bus.V2Guid}" + " " + "به ".GetTranslate() + " " + $"{web.V2Guid} " + " " + "تغییر کرد".GetTranslate());
			}
			if (bus.V2Qt != web.V2Qt)
			{
				concurrentDictionary.TryAdd("V2Qt", "مقدار واحد دوم از ".GetTranslate() + " " + $"{bus.V2Qt} " + " " + "به ".GetTranslate() + " " + $"{web.V2Qt} " + " " + "تغییر کرد".GetTranslate());
			}
			return concurrentDictionary;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdChanges", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1731);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> items, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ProductBusiness/KeepRangeAsync", 60000, token))
				{
					if (autoTran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1745), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1745);
					}
					transactionHasBeenCreated = true;
					if (items == null || !items.Any())
					{
						result = ret;
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1748);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1750);
							goto end_IL_0118;
						}
						result = ret;
					}
					goto end_IL_0076;
					end_IL_0118:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1755);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1756);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1761), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1761);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, IEnumerable<Guid> items, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ProductBusiness/DeleteRangeAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1775);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1776), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1776);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.RemoveRangeFromEntitiesAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1778);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (!token.IsCancellationRequested)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.DeleteRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1781);
								goto end_IL_0118;
							}
							result = ret.AddErrorTokenIsCancelled("", "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1780);
						}
					}
					goto end_IL_0076;
					end_IL_0118:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1786);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1787);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1792), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1792);
			}
		}
		return ret;
	}

	public static Task<ProductBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAsync(guid, token, tranName);
	}

	public static Task<ProductBusiness> GetAsync(string contextGuid, Guid guid, Guid? depotGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAsync(guid, depotGuid, token, tranName);
	}

	public static Task<int> DbCountAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", bool? status = true)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.DbCountAsync(status, token, tranName);
	}

	public static async Task<long> GetNextCodeAsync(string contextGuid, Guid defGroupGuid, EnSuggestionType? suggestionType = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (!suggestionType.HasValue)
			{
				suggestionType = ((await LocalSettingBusiness.GetSettingAsync(contextGuid, token, tranName))?.Products?.SuggestionType).GetValueOrDefault(EnSuggestionType.FillGaps);
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetNextCodeAsync(defGroupGuid, suggestionType.Value, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetNextCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1815);
			throw;
		}
	}

	public static Task<bool> UpdateAutoSellPriceOfProductsAsync(List<Guid> lstprd, bool autoSellPrice)
	{
		throw new NotImplementedException("پیاده سازی شود");
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<ProductBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<ProductBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		checked
		{
			using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
			{
				int line = 0;
				ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
				ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Product;
				bool semaphoreHasBeenTaken = false;
				try
				{
					await _resendToServerSemaphore.WaitAsync(token);
					semaphoreHasBeenTaken = true;
					if (uiUpdater != null)
					{
						uiUpdater.ShortMessage = "در حال ارسال کالاها".GetTranslate();
						uiUpdater.Status = SyncStatus.Syncing;
						uiUpdater.FinalStatus = ret;
						uiUpdater.Group = SyncPart.Products;
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1846);
					}
					ClsCache cache = ClsCache.GetCache(contextGuid);
					int num;
					if (cache == null)
					{
						num = 1;
					}
					else
					{
						_ = cache.BufferCancellationToken;
						num = 0;
					}
					if (num != 0)
					{
						return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست  ! !".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1848);
					}
					line = 5;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1850);
					}
					if (ClsCache.GetCache(contextGuid).CacheRepository.IProduct == null)
					{
						throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IProduct is null");
					}
					List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
					if (uiUpdater != null)
					{
						uiUpdater.TotalCount = list?.Count ?? 0;
					}
					line = 6;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1857);
					}
					if (list == null || !list.Any())
					{
						return ret.AddInformation("کالاها سینک هستند تعداد کالای سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1858);
					}
					List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
					int current = 0;
					decimal total = list?.Count ?? 0;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (list != null && list.Any())
					{
						foreach (Guid item in list)
						{
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1865);
							}
							current++;
							Math.Round((decimal)(100 * current) / total, 2);
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1868);
							}
							ProductBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
							if (uiUpdater != null)
							{
								uiUpdater.ShortMessage = "کالای جاری".GetTranslate() + ":" + (obj?.Name ?? "").GetTranslate();
								uiUpdater.Count = current;
							}
							if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
							{
								ret.AddInformation("current status on database is delivered!\r\nEntityType: Product \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1878);
								continue;
							}
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1884);
							}
							tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
							if (tasks.Count >= maxParalelTask)
							{
								Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
								tasks.Remove(task);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1890);
							}
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1893);
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1894);
					}
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1896);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1899);
					ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1900);
				}
				finally
				{
					if (semaphoreHasBeenTaken)
					{
						_resendToServerSemaphore.Release();
					}
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
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeTaxPercentAsync(string contextGuid, IEnumerable<Guid> productGuidList, Guid? taxGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
				if ((taxGuid ?? Guid.Empty) == Guid.Empty)
				{
					taxGuid = null;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1930), ReturnedState.None, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1930);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.ChangeTaxPercentAsync(productGuidList, taxGuid, token, tranName), ReturnedState.None, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1932);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1936);
				ret.AddReturnedValue(ex, "", "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1937);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1942), ReturnedState.None, "ChangeTaxPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1942);
			}
		}
		return ret;
	}

	internal async Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1954), ReturnedState.None, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1954);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProduct.UpdateSendToServerDateAsync(Guid, token, tranName), ReturnedState.None, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1956);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1960);
				ret.AddReturnedValue(ex, "", "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1961);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1966), ReturnedState.None, "UpdateSendToServerDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1966);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ReNewAllCodesAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1978), ReturnedState.None, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1978);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.ReNewAllCodesAsync(token, tranName), ReturnedState.None, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1980);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1984);
				ret.AddReturnedValue(ex, "", "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1985);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1990), ReturnedState.None, "ReNewAllCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 1990);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ReNewEmptyCodesAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2002), ReturnedState.None, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2002);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.ReNewEmptyCodesAsync(token, tranName), ReturnedState.None, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2004);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2008);
				ret.AddReturnedValue(ex, "", "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2009);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2014), ReturnedState.None, "ReNewEmptyCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2014);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateRegularPriceAsync(string contextGuid, Guid priceLevelGuid, CancellationToken token = default(CancellationToken), string tranName = "")
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
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2026), ReturnedState.None, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2026);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateRegularPriceAsync(priceLevelGuid, ParentDefaults.PriceLevel.RegularPrice, token, tranName), ReturnedState.None, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2028);
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ProductPriceUpdateBusiness.InsertBatchAsync(contextGuid, token, tranName), ReturnedState.None, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2030);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2034);
				ret.AddReturnedValue(ex, "", "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2035);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2040), ReturnedState.None, "UpdateRegularPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2040);
			}
		}
		return ret;
	}

	public async Task<PrdVahedBusiness> GetVahedAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_prdVahed != null)
			{
				return _prdVahed;
			}
			_prdVahed = await PrdVahedBusiness.GetAsync(ContextGuid, GuidVahed, token, tranName);
			return _prdVahed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2054);
			throw;
		}
	}

	internal static async Task<HashSet<Guid>> GetAllForIndexingAsync(string contextGuid, EnProductIndex mode, int count, CancellationToken token, string tranName = "")
	{
		ClsCache cache = ClsCache.GetCache(contextGuid);
		if (cache == null)
		{
			return null;
		}
		return await cache.CacheRepository.IProduct.GetAllForIndexingAsync(mode, count, token, tranName);
	}

	internal static Task<IEnumerable<ProductForIndexingModel>> GetAllForIndexingAsync(string contextGuid, HashSet<Guid> guids, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAllForIndexingAsync(guids, token, tranName);
	}

	internal static async Task<ReturnedSaveFuncInfo> UpdateIndexTypeAsync(string contextGuid, HashSet<Guid> guids, EnProductIndex mode, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2074), ReturnedState.None, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2074);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateIndexTypeAsync(guids, mode, token, tranName), ReturnedState.None, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2076);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2080);
				ret.AddReturnedValue(ex, "", "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2081);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2086), ReturnedState.None, "UpdateIndexTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2086);
			}
		}
		return ret;
	}

	public Task AddFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.AddFileAsync(file, _addedFileList);
	}

	public Task RemoveFileAsync(FileInfoBusiness file)
	{
		return BaseFileAttachmentBusiness.RemoveFileAsync(file, _removedFileList);
	}

	public Task<IEnumerable<ProductAttributeRelationBusiness>> GetAllAttributesAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ProductAttributeRelationRepository.GetAllAsync(Guid, token, tranName);
	}

	public static Task<IEnumerable<ProductDiscountModel>> GetAllDiscountModelAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAllDiscountModelAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateDiscountPercentAsync(string contextGuid, List<Tuple<Guid, decimal>> changes, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
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
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2104), ReturnedState.None, "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2104);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateDiscountPercentAsync(changes, token, tranName), ReturnedState.None, "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2106);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2110);
				ret.AddReturnedValue(ex, "", "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2111);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2116), ReturnedState.None, "UpdateDiscountPercentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2116);
			}
		}
		return ret;
	}

	public static async Task<ProductBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system = EnSystem.None, Guid? guid = null, EnProductType type = EnProductType.Product, Guid? groupGuid = null)
	{
		try
		{
			ProductBusiness res = new ProductBusiness(contextGuid)
			{
				CreateDate = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				Type = type
			};
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				res.Guid = guid.Value;
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			if ((groupGuid ?? Guid.Empty) != Guid.Empty)
			{
				res.AddToGroupRelationList(groupGuid.Value);
			}
			else if ((accGlobalSettings?.Products?.DefaultGroupGuid ?? Guid.Empty) != Guid.Empty)
			{
				res.AddToGroupRelationList(accGlobalSettings.Products.DefaultGroupGuid.Value);
			}
			res.Special = accGlobalSettings.Products.DefaultSpecial;
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2144);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> UpdateStatusAsync(bool newStatus, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return UpdateStatusAsync(ContextGuid, Guid, newStatus, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateStatusAsync(string contextGuid, Guid guid, bool newStatus, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2157), ReturnedState.None, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2157);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateStatusAsync(guid, newStatus, token, tranName), ReturnedState.None, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2159);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2163);
				ret.AddReturnedValue(ex, "", "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2164);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2169), ReturnedState.None, "UpdateStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2169);
			}
		}
		return ret;
	}

	public Task<string> GetGroupNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return this.GetGroupNameAsync(ContextGuid, GroupList, token, tranName);
	}

	public Task<List<PrdGroupBusiness>> GetGroupsAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return this.GetGroupsAsync(ContextGuid, GroupList, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateMasterProductAmountAsync(string contextGuid, Guid? guid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2183), ReturnedState.None, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2183);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateMasterProductAmountAsync(guid, token, tranName), ReturnedState.None, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2185);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2189);
				res.AddReturnedValue(ex, "", "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2190);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2195), ReturnedState.None, "UpdateMasterProductAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2195);
			}
		}
		return res;
	}

	public static Task<List<ProductTolidReportModel>> GetAllTolidReportAsync(string contextGuid, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAllTolidReportAsync(productGuid, token, tranName);
	}

	public Task<IEnumerable<ProductPriceRelationBusiness>> GetAllPriceRelationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ProductPriceRelation.GetAllAsync(new ProductPriceRelationFilter
		{
			ProductGuid = Guid
		}, token, tranName);
	}

	public Task<IEnumerable<ProductPriceRelationModel>> GetAllPricesAsync(EnSystem system, EnPriceLevelType? type, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return this.GetAllPricesAsync(ContextGuid, system, type, PriceList, token, tranName);
	}

	public void ClearPriceList()
	{
		_priceList?.Clear();
	}

	public Task AddRangeToPriceListAsync(List<ProductPriceRelationBusiness> priceList, CancellationToken token = default(CancellationToken))
	{
		try
		{
			priceList = priceList?.Where((ProductPriceRelationBusiness q) => q != null)?.ToList();
			if (priceList == null || !priceList.Any())
			{
				return Task.CompletedTask;
			}
			IEnumerable<ProductPriceRelationBusiness> enumerable = priceList.Where((ProductPriceRelationBusiness q) => q.ProductGuid != Guid);
			if (enumerable != null && enumerable.Any())
			{
				throw new ArgumentException();
			}
			_priceList = new List<ProductPriceRelationBusiness>();
			_priceList.AddRange(priceList);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToPriceListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2219);
		}
		return Task.CompletedTask;
	}

	public void AddToPriceList(ProductPriceRelationBusiness data)
	{
		using (new JaegerMonitor("AddToPriceList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs"))
		{
			try
			{
				if (!((data?.Price ?? 0m) <= 0m))
				{
					if (_priceList == null)
					{
						_priceList = new List<ProductPriceRelationBusiness>();
					}
					data.ProductGuid = Guid;
					if (data.Guid == Guid.Empty)
					{
						data.Guid = Guid.NewGuid();
					}
					ProductPriceRelationBusiness productPriceRelationBusiness = _priceList?.FirstOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == data.PriceLevelGuid);
					if (productPriceRelationBusiness != null)
					{
						productPriceRelationBusiness.Price = data.Price;
						productPriceRelationBusiness.Quantity = data.Quantity;
						productPriceRelationBusiness.KasrRial = data.KasrRial;
						productPriceRelationBusiness.Profit = data.Profit;
					}
					else
					{
						_priceList.Add(data);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToPriceList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2245);
				throw;
			}
		}
	}

	public async Task UpdateFirstSellAsync(EnSystem system, decimal price, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Type = EnPriceLevelType.Sell,
				Status = true
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(ContextGuid, system, filter, token, tranName);
			if (enumerable != null && enumerable.Any())
			{
				ProductPriceLevelBusiness productPriceLevelBusiness = enumerable?.SingleOrDefault((ProductPriceLevelBusiness q) => q.Ordinal == 1);
				if (productPriceLevelBusiness != null)
				{
					UpdatePrice(productPriceLevelBusiness.Guid, price, EnPriceLevelType.Sell, token, tranName);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateFirstSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2267);
			throw;
		}
	}

	public void UpdatePrice(Guid priceLevelGuid, decimal price, EnPriceLevelType type, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ProductPriceRelationBusiness productPriceRelationBusiness = PriceList?.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceLevelGuid);
			if (productPriceRelationBusiness == null)
			{
				productPriceRelationBusiness = new ProductPriceRelationBusiness(ContextGuid)
				{
					ContextGuid = ContextGuid,
					ChangeStatus = EntityChangeSt.New,
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					Price = price,
					PriceLevelGuid = priceLevelGuid,
					ProductGuid = Guid,
					ServerStatus = ServerStatusForClient.None,
					Type = type
				};
				AddToPriceList(productPriceRelationBusiness);
			}
			else
			{
				productPriceRelationBusiness.Price = price;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdatePrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2296);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateProductsAsync(string contextGuid, List<ProductUpdateBaseModel> originalList, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2308), ReturnedState.None, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2308);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateProductsAsync(originalList, token, tranName), ReturnedState.None, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2310);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2314);
				ret.AddReturnedValue(ex, "", "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2315);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2320), ReturnedState.None, "UpdateProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2320);
			}
		}
		return ret;
	}

	public static bool HasCharinCode(string idProduct)
	{
		try
		{
			bool flag = false;
			string source = "0123456789";
			char[] array = idProduct.FixString().ToLower().ToCharArray();
			foreach (char value in array)
			{
				if (!Enumerable.Contains(source, value))
				{
					flag = true;
					break;
				}
			}
			if (!flag && idProduct != idProduct.ParseToLong(0L).ToString())
			{
				flag = true;
			}
			return flag;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasCharinCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2346);
			throw;
		}
	}

	internal static async Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(string contextGuid, Guid groupGuid, bool status, CancellationToken token, string tranName)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2357), ReturnedState.None, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2357);
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.UpdateSpecialAsync(groupGuid, status, token, tranName), ReturnedState.None, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2358);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2362);
				res.AddReturnedValue(ex, "", "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2363);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2368), ReturnedState.None, "UpdateSpecialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2368);
			}
		}
		return res;
	}

	public static async Task<List<ProductAttributeAmountModel>> GetAllAttributeAmountAsync(string contextGuid, Guid productGuid, Guid rowAttributeGuid, Guid columnAttributeGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (productGuid == Guid.Empty || rowAttributeGuid == Guid.Empty || columnAttributeGuid == Guid.Empty)
			{
				return null;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.IProduct.GetAllAttributeAmountAsync(productGuid, rowAttributeGuid, columnAttributeGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAttributeAmountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductBusiness.cs", 2381);
			throw;
		}
	}
}
