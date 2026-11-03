using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Extensions;
using EntityCache.Business.OrderDepot;
using EntityCache.Business.Products;
using EntityCache.Filters;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Models.Accounting.Prd;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Filters.Configs;
using PacketParser.Settings.Accounting;
using Serilog;

namespace EntityCache.Business;

public class ProductCompactBusiness : ProductMicroBusiness, IProductCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private ProductBusiness _productBusiness;

	private string _pic;

	private ProductInfoModel _productInfoModel;

	private List<PrdTechnicalBusiness> _prdTech;

	private PrdVahedBusiness _prdVahed;

	private List<ProductGroupRelationBusiness> _prdGroupRelation;

	public bool Checked { get; set; }

	public Guid? TaxGuid { get; set; }

	[JsonIgnore]
	public string OrderQuantityString => ((double?)base.Order?.ListKala?.Where((OrderDetailBusiness p) => p != null && p.ProductGuid == base.Guid)?.Sum((OrderDetailBusiness p) => p?.Quantity))?.ToString()?.Replace(".", "/") ?? "";

	[JsonIgnore]
	public string OrderPriceString => base.Order?.ListKala?.FirstOrDefault((OrderDetailBusiness p) => p != null && p.ProductGuid == base.Guid)?.PriceString ?? "";

	public List<string> ImageListData { get; set; }

	public string Pic
	{
		get
		{
			if (!string.IsNullOrEmpty(_pic))
			{
				return _pic;
			}
			return "NoImagePrd.png";
		}
		set
		{
			_pic = value?.FixString() ?? "";
		}
	}

	[JsonIgnore]
	public string GroupName { get; set; }

	[JsonIgnore]
	public string GroupShortName { get; set; }

	public Guid GuidVahed { get; set; }

	public string VahedName { get; set; }

	public string AcountForDisplay => base.Amount.ToString().Replace(".", "/");

	[JsonIgnore]
	public ServerStatusForClient ServerStatus { get; set; }

	public new decimal OrderPoint { get; set; }

	public bool IsTax { get; set; }

	public bool IsChecked { get; set; }

	public decimal Weight { get; set; }

	public Guid RandomGuid => Guid.NewGuid();

	[JsonIgnore]
	public string IsDiscount
	{
		get
		{
			if (!(base.DiscountPercent > 0m))
			{
				return "#ffffff";
			}
			return "#ff005e";
		}
	}

	[JsonIgnore]
	public string IsAmount
	{
		get
		{
			if (!(base.Amount <= 0m))
			{
				return "-50%";
			}
			return "50%";
		}
	}

	public double FQuantity { get; set; }

	public DateTime? DiscountStartDate { get; set; }

	public DateTime? DiscountEndDate { get; set; }

	public decimal DiscountPrice { get; set; }

	public ProductCompactBusiness(string contextGuid)
		: base(contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public static async Task<int> DbCountAsync(string contextGuid, bool? statue = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			ProductCompactFilter filter = new ProductCompactFilter
			{
				Status = statue,
				Sort = (cashierBusiness?.Settings?.Products?.ProductSort).GetValueOrDefault()
			};
			return (await GetAllAsync(contextGuid, EnSystem.UserInterface, filter, token, tranName))?.Count() ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 73);
		}
		return 0;
	}

	public void SetFullProductNull()
	{
		_productBusiness = null;
	}

	public static async Task<ProductCompactBusiness> GetAsync(string contextGuid, Guid guid, EnSystem system, OrderBusiness order = null, string tranName = "", CancellationToken token = default(CancellationToken), bool? status = true)
	{
		int line = 0;
		try
		{
			if (guid == Guid.Empty)
			{
				return null;
			}
			line = 100;
			ProductCompactBusiness productCompactBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAsync(guid, status, token, tranName);
			if (productCompactBusiness != null && order != null)
			{
				productCompactBusiness.Order = order;
			}
			return await ApplyAccessAsync(contextGuid, productCompactBusiness, system, order, token);
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
			if (ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductCompact == null)
			{
				Log.Warning(ex3, "ریپازیتوری یا کش دیسپوز شده".GetTranslate() + $"get(guid,order)//Line:{line}//Guid:{guid}//ClsCache.GetCache(contextGuid) .CacheRepository.IProductCompact is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductCompact == null}");
			}
			else
			{
				WebErrorLog.Instance.StartLog(ex3, $"get(guid,order)//Line:{line}//Guid:{guid}//ClsCache.GetCache(contextGuid) .CacheRepository.IProductCompact is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductCompact == null}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 95);
			}
			throw;
		}
	}

	public static async Task<ProductCompactBusiness> GetAsync(string contextGuid, string code, EnSystem systen, bool? status = true, string tranName = "", OrderBusiness order = null, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		ClsCache cache = null;
		string debugMessage = "";
		try
		{
			cache = ClsCache.GetCache(contextGuid);
			line = 1;
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			debugMessage = $"CashierIsNull:{cashierBusiness == null}";
			debugMessage += $"/cache.CacheRepository.IProductCompact is null:{cache?.CacheRepository?.IProductCompact == null}";
			code = code.TrimStart(Array.Empty<char>()).TrimEnd(Array.Empty<char>());
			ProductCompactBusiness productCompactBusiness = await cache.CacheRepository.IProductCompact.GetAsync(cashierBusiness?.DepotGuid, code, status, token, tranName);
			line = 2;
			if (productCompactBusiness != null)
			{
				productCompactBusiness.Order = order;
			}
			line = 5;
			return await ApplyAccessAsync(contextGuid, productCompactBusiness, systen, order, token);
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
			WebErrorLog.Instance.StartLog(ex3, "get(code,order)\r\n" + $"#Line_{line}\r\n" + $"cache is null:{cache == null}\r\n" + "DebugMessage:" + debugMessage, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 123);
			throw;
		}
	}

	public static Task<ReturnedSaveFuncInfoWithValue<bool>> HasPicImageAsync(string contextGuid, string fileName, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.HasPicImageAsync(fileName, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<bool>> HasOtherPicImageAsync(string contextGuid, string fileName, CancellationToken token, string tranName)
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.HasOtherPicImageAsync(fileName, token, tranName);
	}

	public static Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySlugAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllEmptySlugAsync(token, tranName);
	}

	public static Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllEmptySkuAsync(token, tranName);
	}

	public static Task<IEnumerable<ProductCompactBusiness>> GetAllEmptySkuOrSlugAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllEmptySkuOrSlugAsync(token, tranName);
	}

	public new static async Task<IEnumerable<ProductCompactBusiness>> GetAllAsync(string contextGuid, EnSystem system, ProductCompactFilter filter = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (system != EnSystem.Transfer)
			{
				AccessLevel accessLevel = await cashier.GetUserAccessAsync(token, tranName);
				if (filter == null)
				{
					filter = new ProductCompactFilter
					{
						Sort = (cashier?.Settings?.Products?.ProductSort).GetValueOrDefault()
					};
				}
				if ((accessLevel?.PrdGroupListGuid?.Any()).GetValueOrDefault() || !accessLevel.Product.Product_DastrasiBeKalahayeKhas)
				{
					ProductGroupFilter filter2 = new ProductGroupFilter
					{
						System = system,
						LoadJustParents = false,
						Status = null,
						LoadParentWithGuidEmpty = false
					};
					ProductGroupModel productGroupModel = await PrdGroupBusiness.GetAllAsync(contextGuid, filter2, token, tranName);
					if (productGroupModel == null)
					{
						return null;
					}
					if (productGroupModel.AllDbCount == (productGroupModel.List?.Count() ?? 0))
					{
						filter.CashierAccessGroupGuids = null;
					}
					else
					{
						filter.CashierAccessGroupGuids = productGroupModel?.List?.Select((PrdGroupBusiness q) => q.Guid)?.ToList();
					}
				}
			}
			_ = ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact;
			if (ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact == null)
			{
				throw new NullReferenceException("prdRepo");
			}
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar || ClsCache.GetCache(contextGuid).Source == ENSource.Android_SefareshGir)
			{
				filter.ProductSearchOption = ProductSearchOption.CodeOrName;
			}
			Guid? depotGuid = null;
			if (!(cashier?.AllDepotsAccess ?? false))
			{
				depotGuid = cashier?.DepotGuid;
			}
			Document orderType = filter.Order?.FactorType ?? Document.OrderSell;
			if (filter != null)
			{
				AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
				filter.LoadBarcodeInsteadOfCode = accGlobalSettings.Products.LoadBarcodeInsteadOfCode;
			}
			await Task.Delay(new Random().Next(100));
			if (token.IsCancellationRequested)
			{
				return null;
			}
			List<ProductCompactBusiness> items = await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllAsync(depotGuid, null, orderType, token, filter, tranName);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (items == null)
			{
				items = new List<ProductCompactBusiness>();
			}
			if (!string.IsNullOrEmpty(filter.SearchText))
			{
				BarcodeBusiness barcode = await BarcodeBusiness.GetAsync(contextGuid, filter.SearchText, token, tranName);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (barcode != null && !(items?.Any((ProductCompactBusiness p) => p.Guid == barcode.PrdGuid) ?? false))
				{
					ProductCompactBusiness productCompactBusiness = await GetAsync(contextGuid, barcode.PrdGuid, system, filter.Order, tranName, token, true);
					if (productCompactBusiness != null)
					{
						items.Add(productCompactBusiness);
					}
				}
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (filter?.HasMultiVahed.HasValue ?? false)
			{
				items = items?.Where((ProductCompactBusiness q) => (q.V2Guid ?? Guid.Empty) != Guid.Empty)?.ToList();
			}
			if (items?.Any((ProductCompactBusiness q) => q.Kind == EnProductKind.Variation) ?? false)
			{
				items?.Where((ProductCompactBusiness q) => (q.MasterGuid ?? Guid.Empty) != Guid.Empty)?.ToList();
				if (filter?.LoadJustParents ?? false)
				{
					items = items?.Where((ProductCompactBusiness q) => (q.MasterGuid ?? Guid.Empty) == Guid.Empty)?.ToList();
				}
				if ((filter?.MasterGuid ?? Guid.Empty) != Guid.Empty)
				{
					items = items?.Where((ProductCompactBusiness q) => (q.MasterGuid ?? Guid.Empty) == filter.MasterGuid.Value)?.ToList();
				}
			}
			return await ApplyAccessAsync(contextGuid, items, system, filter.Order, token);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			token.ThrowIfCancellationRequested();
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, $"ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductCompact == null}//GetAllAsync(string searchText, CancellationToken token, OrderBusiness order, List<Guid> GroupGuids = null, bool status = true, SearchLocation? searchLocation = null, ProductSearchOption? productSearchOption = null, ProductSort? sort = null)", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 221);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateGroupOfProductsAsync(string contextGuid, List<ProductGroupRelationBusiness> originalList, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 233), ReturnedState.None, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 233);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.UpdateGroupOfProductsAsync(originalList, token, tranName), ReturnedState.None, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 235);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 239);
				ret.AddReturnedValue(ex, "", "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 240);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 245), ReturnedState.None, "UpdateGroupOfProductsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 245);
			}
		}
		return ret;
	}

	public static async Task<List<clsMultiVahed>> GetAllVahedsAsync(string contextGuid, EnSystem system, Guid prdGuid, CancellationToken token = default(CancellationToken))
	{
		List<clsMultiVahed> list = new List<clsMultiVahed>();
		try
		{
			if (prdGuid == Guid.Empty)
			{
				return null;
			}
			ProductBusiness product = await ProductBusiness.GetAsync(contextGuid, prdGuid, token);
			if (product == null)
			{
				return null;
			}
			if (product.GuidVahed != Guid.Empty)
			{
				List<clsMultiVahed> list2 = list;
				list2.Add(await clsMultiVahed.GetMiltiVahedInstanceAsync(contextGuid, Guid.Empty, prdGuid, system, token));
			}
			if (product.V2Guid.HasValue && product.V2Guid != Guid.Empty)
			{
				List<clsMultiVahed> list2 = list;
				list2.Add(await clsMultiVahed.GetMiltiVahedInstanceAsync(contextGuid, Guid.Empty, product.V2Guid.Value, system, token));
			}
			if (product.V3Guid.HasValue && product.V3Guid != Guid.Empty)
			{
				List<clsMultiVahed> list2 = list;
				list2.Add(await clsMultiVahed.GetMiltiVahedInstanceAsync(contextGuid, Guid.Empty, product.V3Guid.Value, system, token));
			}
			if (product.V4Guid.HasValue && product.V4Guid != Guid.Empty)
			{
				List<clsMultiVahed> list2 = list;
				list2.Add(await clsMultiVahed.GetMiltiVahedInstanceAsync(contextGuid, Guid.Empty, product.V4Guid.Value, system, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllVahedsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 268);
			throw;
		}
		return list;
	}

	public async Task<List<OrderDetailBusiness.DisplayMultiVahed>> GetMultiVahedListAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetMultiVahedListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs"))
		{
			_ = 3;
			try
			{
				ProductBusiness product = await GetFullProductAsync(token, tranName);
				List<OrderDetailBusiness.DisplayMultiVahed> ret = new List<OrderDetailBusiness.DisplayMultiVahed>
				{
					new OrderDetailBusiness.DisplayMultiVahed
					{
						Guid = GuidVahed,
						Quantity = 1m,
						Name = VahedName
					}
				};
				if (base.V2Qt > 0m && (base.V2Guid ?? Guid.Empty) != Guid.Empty)
				{
					string name = (await PrdVahedBusiness.GetAsync(base.ContextGuid, base.V2Guid.Value, token, tranName)).Name;
					ret.Add(new OrderDetailBusiness.DisplayMultiVahed
					{
						Guid = base.V2Guid.Value,
						Quantity = base.V2Qt,
						Name = $"{name} - {base.V2Qt}"
					});
				}
				if ((product?.V3Qt ?? 0m) > 0m && (product.V3Guid ?? Guid.Empty) != Guid.Empty)
				{
					string name2 = (await PrdVahedBusiness.GetAsync(base.ContextGuid, product.V3Guid.Value, token, tranName)).Name;
					ret.Add(new OrderDetailBusiness.DisplayMultiVahed
					{
						Guid = product.V3Guid.Value,
						Quantity = product.V3Qt,
						Name = $"{name2} - {product?.V3Qt}"
					});
				}
				if ((product?.V4Qt ?? 0m) > 0m && (product.V4Guid ?? Guid.Empty) != Guid.Empty)
				{
					string name3 = (await PrdVahedBusiness.GetAsync(base.ContextGuid, product.V4Guid.Value, token, tranName)).Name;
					ret.Add(new OrderDetailBusiness.DisplayMultiVahed
					{
						Guid = product.V4Guid.Value,
						Quantity = product.V4Qt,
						Name = $"{name3} - {product?.V4Qt}"
					});
				}
				return ret;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetMultiVahedListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 322);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, Guid guid, bool status, EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 3;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 336), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 336);
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 337), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 337);
					transactionHasBeenCreated = true;
				}
				ProductBusiness productBusiness = await ProductBusiness.GetAsync(contextGuid, guid, token, tranName);
				if (productBusiness == null)
				{
					res.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 343);
					return res;
				}
				productBusiness.Status = status;
				productBusiness.ServerStatus = ServerStatusForClient.None;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await productBusiness.SaveAsync(sendToServer: true, 0, token, system, awaitOnSemaphore: true, await4send: false, "", "", 60000, null, null, 10, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 348);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 352);
				res.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 353);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 359), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 359);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 360), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 360);
			}
		}
		return res;
	}

	public static async Task<bool> HasMojodiAsync(string contextGuid, Guid guid, OrderBusiness order, EnSystem systen, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			ProductCompactBusiness productCompactBusiness = await GetAsync(contextGuid, guid, systen, order, tranName, token, true);
			if (productCompactBusiness == null)
			{
				return false;
			}
			foreach (PrdAnbarRelationBusiness item in (await productCompactBusiness.GetAnbarRelationsAsync(systen, token)) ?? new List<PrdAnbarRelationBusiness>())
			{
				if (item.Amount != 0m)
				{
					return true;
				}
			}
			return false;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HasMojodiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 379);
			throw;
		}
	}

	public static async Task<decimal> Avale2reAsync(Guid guid)
	{
		return default(decimal);
	}

	public static Task<decimal> LastVorodPriceAvgAsync(string contextGuid, Guid prdGuid, Guid? depotGuid = null, DateTime? date = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.LastVorodPriceAvgAsync(prdGuid, date, depotGuid, true, token, tranName);
	}

	public static async Task<int> ProductsCountAsync(string contextGuid, EnSystem system, Guid groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await GetAllAsync(contextGuid, system, new ProductCompactFilter
			{
				GroupGuids = new List<Guid> { groupGuid }
			}, token, tranName))?.Count() ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProductsCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 395);
			throw;
		}
	}

	public Task<decimal> GetTaxPercentAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ProductBusiness.GetTaxPercentAsync(base.ContextGuid, base.Guid, token, tranName);
	}

	private static async Task<IEnumerable<ProductCompactBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<ProductCompactBusiness> items, EnSystem system, OrderBusiness order, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		int line = 0;
		try
		{
			EnSystem enSystem = system;
			if (((uint)enSystem <= 1u || (uint)(enSystem - 3) <= 1u) ? true : false)
			{
				return items;
			}
			if (items == null)
			{
				return null;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			line = 5;
			if (currentCashier == null)
			{
				return items;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			AccessLevel access = await currentCashier.GetUserAccessAsync(token, tranName);
			UserAccBusiness userAccBusiness = await currentCashier.GetUserAsync(token, tranName);
			if (!userAccBusiness.UserAccess.Product.Product_See)
			{
				return null;
			}
			if (!(userAccBusiness?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault())
			{
				items = items?.Where((ProductCompactBusiness p) => p != null && !p.Special);
			}
			line = 10;
			Document document = order?.FactorType ?? Document.None;
			bool flag = document == Document.AmaniKhoroj || document == Document.PishFactor || document == Document.OrderSell || document == Document.OrderBackBuy;
			if (document == Document.OrderBuy)
			{
				flag = !access.Buy.Buy_Insert && !access.BackSell.BackSell_Insert;
			}
			if (flag && !userAccBusiness.UserAccess.Product.Product_SeePointOrder)
			{
				items = items?.Where((ProductCompactBusiness p) => (p?.Amount ?? 0m) >= (p?.OrderPoint ?? 0m));
			}
			line = 15;
			if (token.IsCancellationRequested || items == null)
			{
				return null;
			}
			line = 250;
			items = items?.ToList();
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 440);
			throw;
		}
	}

	private static async Task<ProductCompactBusiness> ApplyAccessAsync(string contextGuid, ProductCompactBusiness item, EnSystem systen, OrderBusiness order, CancellationToken token = default(CancellationToken))
	{
		return (await ApplyAccessAsync(contextGuid, new List<ProductCompactBusiness> { item }, systen, order, token))?.SingleOrDefault();
	}

	public static async Task<IEnumerable<ProductPriceReportBusiness>> GetAllProductPriceAsync(string contextGuid, ProductPriceReportConfig config, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllProductPriceModelAsync(config, token, tranName);
	}

	internal static Task<List<Guid>> CheckExistAsync(string contextGuid, List<Guid> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.CheckExistAsync(lst, token, tranName);
	}

	public async Task<bool> GetScaleStatusAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).ScaleSt;
	}

	public async Task<Guid?> GetV3GuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).V3Guid;
	}

	public async Task<Guid?> GetV4GuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).V4Guid;
	}

	public async Task<decimal> GetV3QtAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).V3Qt;
	}

	public async Task<decimal> GetV4QtAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).V4Qt;
	}

	public async Task<decimal> GetBPercent1Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).BPercent1;
	}

	public async Task<decimal> GetBPercent2Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).BPercent2;
	}

	public async Task<decimal> GetBPercent3Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).BPercent3;
	}

	public async Task<decimal> GetBPercent4Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).BPercent4;
	}

	public async Task<bool> GetIsSerialAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).IsSerial;
	}

	public async Task<decimal> GetBuy1Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).Buy1;
	}

	public async Task<decimal> GetBuy2Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).Buy2;
	}

	public async Task<decimal> GetBuy3Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).Buy3;
	}

	public async Task<Guid?> GetTolidFormulaGuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).TolidFormulaGuid;
	}

	public async Task<ProductMojodiSt> GetMojodiAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetPrdInfoModelAsync(token, tranName)).Mojodi;
	}

	private async Task<ProductInfoModel> GetPrdInfoModelAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (_productInfoModel == null)
		{
			_productInfoModel = await ClsCache.GetCache(base.ContextGuid).CacheRepository.IProductCompact.GetPrdInfoModelAsync(base.Guid, token, tranName);
		}
		return _productInfoModel;
	}

	public async Task<PrdAnbarRelationBusiness> GetAnbarRelationAsync(string contextGuid, Guid depotGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarRelation.GetAnbarRelationsAsync(base.Guid, depotGuid, token, tranName))?.SingleOrDefault();
	}

	internal Task<ReturnedSaveFuncInfo> UpdateStatusAsync(bool status, CancellationToken token, string tranName)
	{
		return ProductBusiness.UpdateStatusAsync(base.ContextGuid, base.Guid, status, token, tranName);
	}

	public async Task<IReadOnlyList<PrdTechnicalBusiness>> GetPrdTechnicalListAsync(CancellationToken token = default(CancellationToken))
	{
		if (_prdTech == null)
		{
			_prdTech = (await ProductBusiness.GetPrdTechnicalListAsync(base.ContextGuid, base.Guid, token))?.ToList();
		}
		_prdTech = _prdTech?.Where((PrdTechnicalBusiness p) => p != null)?.ToList();
		return _prdTech;
	}

	internal async Task<PrdVahedBusiness> GetVahedAsync(CancellationToken token, string tranName)
	{
		try
		{
			if (_prdVahed != null)
			{
				return _prdVahed;
			}
			_prdVahed = await PrdVahedBusiness.GetAsync(base.ContextGuid, GuidVahed, token, tranName);
			return _prdVahed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 489);
			throw;
		}
	}

	public static async Task<List<ProductCompactBusiness>> GetAllAttributeConflictAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			List<ProductCompactBusiness> res = new List<ProductCompactBusiness>();
			ProductCompactFilter filter = new ProductCompactFilter
			{
				KindList = new List<EnProductKind> { EnProductKind.Variation }
			};
			IEnumerable<ProductCompactBusiness> enumerable = await GetAllAsync(contextGuid, EnSystem.UserInterface, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (ProductCompactBusiness item in enumerable)
			{
				ProductBusiness fullProduct = await item.GetFullProductAsync(token, tranName);
				if (fullProduct == null)
				{
					continue;
				}
				List<Guid> list = await fullProduct.GetAttributeGuidListAsync(token, tranName);
				if ((list == null || !list.Any()) && !(fullProduct?.AttributeRelations?.Any()).GetValueOrDefault())
				{
					continue;
				}
				if ((list == null || !list.Any()) && (fullProduct?.AttributeRelations?.Any()).GetValueOrDefault())
				{
					res.Add(item);
					continue;
				}
				foreach (ProductAttributeRelationBusiness attribute in fullProduct.AttributeRelations)
				{
					list.FirstOrDefault((Guid q) => q == attribute.AttributeGuid);
				}
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAttributeConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 522);
			throw;
		}
	}

	public static async Task<List<ProductCompactBusiness>> GetAllAttributeRelationConflictAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 3;
		try
		{
			List<ProductCompactBusiness> res = new List<ProductCompactBusiness>();
			ProductCompactFilter filter = new ProductCompactFilter
			{
				KindList = new List<EnProductKind> { EnProductKind.Variation }
			};
			IEnumerable<ProductCompactBusiness> enumerable = await GetAllAsync(contextGuid, EnSystem.UserInterface, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (ProductCompactBusiness item in enumerable)
			{
				ProductBusiness fullProduct = await item.GetFullProductAsync(token, tranName);
				if (fullProduct != null)
				{
					List<ProductCompactBusiness> list = await fullProduct.CheckAttributeValidationAsync(EnSystem.UserInterface, token, tranName);
					if (list != null && list.Any())
					{
						res.AddRange(list);
					}
					else if (!(fullProduct?.AttributeRelations?.Any()).GetValueOrDefault() && (fullProduct.ProductAttributes?.Any() ?? false))
					{
						res.AddRange(await ProductMapper.Instance.MapListAsync(contextGuid, fullProduct.ProductAttributes, new List<ProductCompactBusiness>(), tranName, token));
					}
				}
			}
			return res?.OrderBy((ProductCompactBusiness q) => q.Code)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAttributeRelationConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 550);
			throw;
		}
	}

	internal async Task<IEnumerable<ProductGroupRelationBusiness>> GetPrdGroupRelationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return await ProductGroupRelationBusiness.GetAllAsync(base.ContextGuid, base.Guid, null, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPrdGroupRelationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductCompactBusiness.cs", 563);
			throw;
		}
	}

	public Task<IEnumerable<ProductPriceRelationModel>> GetAllPricesAsync(EnSystem system, EnPriceLevelType? type, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return this.GetAllPricesAsync(base.ContextGuid, system, type, null, token, tranName);
	}
}
