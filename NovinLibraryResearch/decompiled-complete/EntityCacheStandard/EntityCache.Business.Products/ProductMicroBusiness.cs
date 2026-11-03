using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Extensions;
using EntityCache.Filters;
using Newtonsoft.Json;
using PacketParser.Access;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Products;

public class ProductMicroBusiness : Serializable<ProductMicroBusiness>, IProductMicro, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private ProductBusiness _productBusiness;

	private OrderBusiness _order;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public long Code { get; set; }

	public decimal Amount { get; set; }

	public string ContextGuid { get; set; }

	public EnProductType Type { get; set; }

	public string IdProduct { get; set; }

	[JsonIgnore]
	public object CodeForDisplay { get; set; }

	public bool Special { get; set; }

	public Guid? V2Guid { get; set; }

	public decimal V2Qt { get; set; }

	public EnProductKind Kind { get; set; }

	public OrderBusiness Order
	{
		get
		{
			return _order;
		}
		set
		{
			_order = value;
		}
	}

	public bool IsLastPriceForCustomer { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public decimal DiscountPercent { get; set; }

	public decimal Sell1 { get; set; }

	public decimal Sell2 { get; set; }

	public Guid? MasterGuid { get; set; }

	public int OrderPoint { get; set; }

	public bool IsDependentOnCurrency { get; set; }

	public ProductMicroBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<ProductMicroBusiness>> GetAllAsync(string contextGuid, EnSystem system, ProductCompactFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (filter == null)
			{
				filter = new ProductCompactFilter
				{
					Sort = (cashierBusiness?.Settings?.Products?.ProductSort).GetValueOrDefault()
				};
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
			if (cashierBusiness == null || !cashierBusiness.AllDepotsAccess)
			{
				depotGuid = cashierBusiness?.DepotGuid;
			}
			_ = filter.Order?.FactorType;
			await Task.Delay(new Random().Next(100));
			if (token.IsCancellationRequested)
			{
				return null;
			}
			List<ProductMicroBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllAsync(filter, null, depotGuid, token, tranName);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (list == null)
			{
				list = new List<ProductMicroBusiness>();
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (filter?.HasMultiVahed.HasValue ?? false)
			{
				list = list?.Where((ProductMicroBusiness q) => (q.V2Guid ?? Guid.Empty) != Guid.Empty)?.ToList();
			}
			if (list != null && list.Any((ProductMicroBusiness q) => q.Kind == EnProductKind.Variation))
			{
				if (filter?.LoadJustParents ?? false)
				{
					list = list?.Where((ProductMicroBusiness q) => (q.MasterGuid ?? Guid.Empty) == Guid.Empty)?.ToList();
				}
				else if (!filter.MasterGuid.HasValue)
				{
					foreach (IGrouping<Guid?, ProductMicroBusiness> item in from q in list?.Where((ProductMicroBusiness q) => (q.MasterGuid ?? Guid.Empty) != Guid.Empty)?.ToList()
						group q by q.MasterGuid)
					{
						ProductMicroBusiness productMicroBusiness = list?.SingleOrDefault(delegate(ProductMicroBusiness q)
						{
							Guid guid = q.Guid;
							Guid? key = item.Key;
							return guid == key;
						});
						if (productMicroBusiness != null)
						{
							productMicroBusiness.Amount = item.Sum((ProductMicroBusiness q) => q.Amount);
						}
					}
				}
				else if ((filter?.MasterGuid ?? Guid.Empty) != Guid.Empty)
				{
					list = list?.Where((ProductMicroBusiness q) => (q.MasterGuid ?? Guid.Empty) == filter.MasterGuid.Value)?.ToList();
				}
			}
			return await ApplyAccessAsync(contextGuid, list, system, filter?.Order, token);
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
			WebErrorLog.Instance.StartLog(ex2, $"ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact is null:{ClsCache.GetCache(contextGuid)?.CacheRepository?.IProductCompact == null}//GetAllAsync(string searchText, CancellationToken token, OrderBusiness order, List<Guid> GroupGuids = null, bool status = true, SearchLocation? searchLocation = null, ProductSearchOption? productSearchOption = null, ProductSort? sort = null)", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMicroBusiness.cs", 100);
			throw;
		}
	}

	public static Task<ProductMicroBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetMicroAsync(guid, token, tranName);
	}

	public Task<string> GetPicFullPathAsync(CancellationToken token = default(CancellationToken))
	{
		return FileInfoBusiness.GetDefaultProductUrlAsync(ContextGuid, Guid, token);
	}

	private static async Task<IEnumerable<ProductMicroBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<ProductMicroBusiness> items, EnSystem system, OrderBusiness order, CancellationToken token = default(CancellationToken), string tranName = "")
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
			line = 10;
			Document document = order?.FactorType ?? Document.None;
			bool flag = document == Document.AmaniKhoroj || document == Document.PishFactor || document == Document.OrderSell || document == Document.OrderBackBuy;
			if (document == Document.OrderBuy)
			{
				flag = !access.Buy.Buy_Insert && !access.BackSell.BackSell_Insert;
			}
			if (!(userAccBusiness?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault())
			{
				items = items?.Where((ProductMicroBusiness p) => p != null && !p.Special);
			}
			if (flag && !userAccBusiness.UserAccess.Product.Product_SeePointOrder)
			{
				items = items?.Where((ProductMicroBusiness p) => (p?.Amount ?? 0m) >= (decimal)(p?.OrderPoint ?? 0));
			}
			line = 15;
			if (token.IsCancellationRequested || items == null)
			{
				return null;
			}
			line = 250;
			items = items?.ToList();
			line = 251;
			if (token.IsCancellationRequested)
			{
				return null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMicroBusiness.cs", 147);
			throw;
		}
		return items;
	}

	public async Task<ProductBusiness> GetFullProductAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetFullProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMicroBusiness.cs"))
		{
			try
			{
				if (_productBusiness == null && Guid != Guid.Empty)
				{
					_productBusiness = await ProductBusiness.GetAsync(ContextGuid, Guid, token, tranName);
				}
				return _productBusiness;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetFullProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMicroBusiness.cs", 162);
				throw;
			}
		}
	}

	public async Task<IEnumerable<PrdAnbarRelationBusiness>> GetAnbarRelationsAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			List<PrdAnbarRelationBusiness> list = new List<PrdAnbarRelationBusiness>();
			if (Kind != EnProductKind.Variation)
			{
				IEnumerable<PrdAnbarRelationBusiness> enumerable = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, system, Guid, null, token, tranName);
				if (enumerable == null || !enumerable.Any())
				{
					return null;
				}
				list.AddRange(enumerable);
			}
			else
			{
				ProductBusiness productBusiness = await GetFullProductAsync(token, tranName);
				if (base.IsDisposed || token.IsCancellationRequested || !(productBusiness?.ProductAttributes?.Any()).GetValueOrDefault())
				{
					return null;
				}
				foreach (ProductBusiness productAttribute in productBusiness.ProductAttributes)
				{
					IEnumerable<PrdAnbarRelationBusiness> enumerable2 = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, system, productAttribute.Guid, null, token, tranName);
					if (enumerable2 == null || !enumerable2.Any())
					{
						return null;
					}
					list.AddRange(enumerable2);
				}
				if (!(list?.Any() ?? false))
				{
					return null;
				}
				IEnumerable<IGrouping<Guid, PrdAnbarRelationBusiness>> enumerable3 = from q in list
					group q by q.AnbarGuid;
				list = new List<PrdAnbarRelationBusiness>();
				foreach (IGrouping<Guid, PrdAnbarRelationBusiness> item in enumerable3)
				{
					list.Add(new PrdAnbarRelationBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						AnbarGuid = item.Key,
						Amount = item.Sum((PrdAnbarRelationBusiness q) => q.Amount)
					});
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAnbarRelationsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductMicroBusiness.cs", 204);
			throw;
		}
	}

	public Task<decimal> GetSellAsync(EnSystem system, Guid priceLevelGuid, bool isLoadOriginalPrice, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return this.GetSellAsync(ContextGuid, system, Order, priceLevelGuid, isLoadOriginalPrice, token, tranName);
	}
}
