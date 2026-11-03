using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Product;
using PacketParser.EventArgs;
using PacketParser.Filters;

namespace EntityCache.Business.Products;

public class ProductPriceRelationBusiness : BaseBusiness<ProductPriceRelationBusiness>, IProductPriceRelation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<ProductPriceRelationBusiness>, IDeliverableEntity, IEditableDelivery
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[JsonIgnore]
	[Obsolete("", true)]
	public bool Status { get; set; }

	public Guid ProductGuid { get; set; }

	public decimal Price { get; set; }

	public decimal Quantity { get; set; }

	public decimal Profit { get; set; }

	public decimal KasrRial { get; set; }

	public Guid PriceLevelGuid { get; set; }

	public EnPriceLevelType Type { get; set; }

	public Guid? UnitGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	[JsonIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public ProductPriceRelationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public static async Task<IEnumerable<ProductPriceRelationModel>> GetAllAsync(string contextGuid, EnSystem system, ProductPriceRelationFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<ProductPriceRelationModel> list = await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceRelation.GetAllModelAsync(filter, token, tranName);
			IEnumerable<ProductPriceLevelBusiness> accessedList = await ProductPriceLevelBusiness.GetAllAsync(contextGuid, system, null, token, tranName);
			return list?.Where((ProductPriceRelationModel q) => (accessedList?.Select((ProductPriceLevelBusiness q) => q.Guid)?.Contains(q.PriceLevelGuid)).GetValueOrDefault());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 60);
			throw;
		}
	}

	internal static async Task<IEnumerable<ProductPriceRelationModel>> UpdateListAsync(string contextGuid, IEnumerable<ProductPriceRelationBusiness> list, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Status = true
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(contextGuid, EnSystem.None, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<ProductPriceRelationModel> list2 = new List<ProductPriceRelationModel>();
			foreach (ProductPriceLevelBusiness type in enumerable)
			{
				if (type != null)
				{
					ProductPriceRelationBusiness productPriceRelationBusiness = list.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == type.Guid);
					if (productPriceRelationBusiness != null)
					{
						list2.Add(new ProductPriceRelationModel
						{
							Guid = type.Guid,
							KasrRial = productPriceRelationBusiness.KasrRial,
							Ordinal = type.Ordinal,
							Price = productPriceRelationBusiness.Price,
							PriceLevelGuid = productPriceRelationBusiness.PriceLevelGuid,
							PriceLevelName = type.Name,
							Profit = productPriceRelationBusiness.Profit,
							Quantity = productPriceRelationBusiness.Quantity,
							Status = type.Status,
							Type = type.Type
						});
					}
				}
			}
			return list2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 98);
			throw;
		}
	}

	public static Task<ProductPriceRelationBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceRelation.GetAsync(guid, token, tranName);
	}

	private async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, string tranName)
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 112), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 112);
				}
				transactionHasBeenCreated = true;
				ServerStatus = ServerStatusForClient.None;
				Modified = DateTime.Now;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ProductPriceRelation.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 116);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 120);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 121);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 126), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 126);
			}
		}
		return res;
	}

	public static Task<int> DbCountAsync(string contextGuid, Guid? productGuid, Guid? priceLevelGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceRelation.DbCountAsync(productGuid, priceLevelGuid, token, tranName);
	}

	public static async Task<ProductPriceRelationBusiness> GetLastBuyAsync(string contextGuid, EnSystem system, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Type = EnPriceLevelType.Buy,
				Status = true,
				IsLoadSystem = true
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(contextGuid, system, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			ProductPriceLevelBusiness lastBuyType = enumerable?.SingleOrDefault((ProductPriceLevelBusiness q) => q.Guid == ParentDefaults.PriceLevel.LastBuy);
			if (lastBuyType == null)
			{
				return null;
			}
			ProductPriceRelationFilter filter2 = new ProductPriceRelationFilter
			{
				Type = EnPriceLevelType.Buy,
				ProductGuid = productGuid
			};
			IEnumerable<ProductPriceRelationModel> enumerable2 = await GetAllAsync(contextGuid, system, filter2, token, tranName);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return null;
			}
			ProductPriceRelationModel productPriceRelationModel = enumerable2?.SingleOrDefault((ProductPriceRelationModel q) => q.Status && q.PriceLevelGuid == lastBuyType.Guid);
			if (productPriceRelationModel == null)
			{
				return null;
			}
			return await GetAsync(contextGuid, productPriceRelationModel.Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 163);
			throw;
		}
	}

	public static async Task<decimal> GetLastBuyDecimalAsync(string contextGuid, EnSystem system, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await GetLastBuyAsync(contextGuid, system, productGuid, token, tranName))?.Price ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastBuyDecimalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 176);
			throw;
		}
	}

	public static async Task<ProductPriceRelationBusiness> GetFirstBuyAsync(string contextGuid, EnSystem system, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Type = EnPriceLevelType.Buy,
				Status = true
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(contextGuid, system, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			ProductPriceLevelBusiness lastBuyType = enumerable?.OrderBy((ProductPriceLevelBusiness q) => q.Ordinal)?.FirstOrDefault();
			if (lastBuyType == null)
			{
				return null;
			}
			ProductPriceRelationFilter filter2 = new ProductPriceRelationFilter
			{
				Type = EnPriceLevelType.Buy,
				ProductGuid = productGuid
			};
			IEnumerable<ProductPriceRelationModel> enumerable2 = await GetAllAsync(contextGuid, system, filter2, token, tranName);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return null;
			}
			ProductPriceRelationModel productPriceRelationModel = enumerable2?.SingleOrDefault((ProductPriceRelationModel q) => q.Status && q.PriceLevelGuid == lastBuyType.Guid);
			if (productPriceRelationModel == null)
			{
				return null;
			}
			return await GetAsync(contextGuid, productPriceRelationModel.Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFirstBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 206);
			throw;
		}
	}

	public static async Task<decimal> GetFirstBuyDecimalAsync(string contextGuid, EnSystem system, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await GetFirstBuyAsync(contextGuid, system, productGuid, token, tranName))?.Price ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFirstBuyDecimalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 219);
			throw;
		}
	}

	public static async Task<ProductPriceRelationBusiness> GetFirstSellAsync(string contextGuid, EnSystem system, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Type = EnPriceLevelType.Sell,
				Status = true
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(contextGuid, system, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			ProductPriceLevelBusiness firstSellType = enumerable?.SingleOrDefault((ProductPriceLevelBusiness q) => q.Ordinal == 1);
			if (firstSellType == null)
			{
				return null;
			}
			ProductPriceRelationFilter filter2 = new ProductPriceRelationFilter
			{
				Type = EnPriceLevelType.Sell,
				ProductGuid = productGuid
			};
			IEnumerable<ProductPriceRelationModel> enumerable2 = await GetAllAsync(contextGuid, system, filter2, token, tranName);
			if (enumerable2 == null || !enumerable2.Any())
			{
				return null;
			}
			ProductPriceRelationModel productPriceRelationModel = enumerable2?.SingleOrDefault((ProductPriceRelationModel q) => q.Status && q.PriceLevelGuid == firstSellType.Guid);
			if (productPriceRelationModel == null)
			{
				return null;
			}
			return await GetAsync(contextGuid, productPriceRelationModel.Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFirstSellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 249);
			throw;
		}
	}

	public static async Task<decimal> GetFirstSellDecimalAsync(string contextGuid, EnSystem system, Guid productGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await GetFirstSellAsync(contextGuid, system, productGuid, token, tranName))?.Price ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFirstSellDecimalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 262);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateLastBuyAsync(string contextGuid, Guid productGuid, decimal price, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (productGuid == Guid.Empty)
		{
			throw new ArgumentException("ProductGuid is Empty !!");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			_ = 5;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 275), ReturnedState.None, "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 275);
				}
				transactionHasBeenCreated = true;
				ProductPriceRelationFilter filter = new ProductPriceRelationFilter
				{
					Type = EnPriceLevelType.Buy,
					ProductGuid = productGuid
				};
				IEnumerable<ProductPriceRelationModel> list = await GetAllAsync(contextGuid, EnSystem.None, filter, token, tranName);
				ProductPriceLevelFilter filter2 = new ProductPriceLevelFilter
				{
					Type = EnPriceLevelType.Buy,
					Status = true,
					IsLoadSystem = true
				};
				IEnumerable<ProductPriceLevelBusiness> enumerable = await ProductPriceLevelBusiness.GetAllAsync(contextGuid, EnSystem.None, filter2, token, tranName);
				ProductPriceLevelBusiness lastBuyType = enumerable?.SingleOrDefault((ProductPriceLevelBusiness q) => q.IsSystem && !q.IsDefault);
				if (lastBuyType == null)
				{
					lastBuyType = new ProductPriceLevelBusiness(contextGuid)
					{
						ChangeStatus = EntityChangeSt.New,
						ServerStatus = ServerStatusForClient.None,
						Guid = Guid.NewGuid(),
						ContextGuid = contextGuid,
						Description = "پیش فرض سیستم".GetTranslate(),
						DiscountCoefficient = 0m,
						IsSystem = true,
						Modified = DateTime.Now,
						Name = "آخرین خرید".GetTranslate(),
						Ordinal = checked((short)((enumerable == null || !enumerable.Any()) ? 1 : (enumerable.Max((ProductPriceLevelBusiness q) => q.Ordinal) + 1))),
						Status = true,
						Type = EnPriceLevelType.Buy
					};
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await lastBuyType.SaveAsync(EnSystem.None, sendToServer: true, Priority.High, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 308);
					if (res.HasError)
					{
						return res;
					}
				}
				ProductPriceRelationModel productPriceRelationModel = list?.SingleOrDefault((ProductPriceRelationModel q) => q.Status && q.PriceLevelGuid == lastBuyType.Guid);
				ProductPriceRelationBusiness productPriceRelationBusiness = await GetAsync(contextGuid, productPriceRelationModel.Guid, token, tranName);
				if (productPriceRelationBusiness == null)
				{
					productPriceRelationBusiness = new ProductPriceRelationBusiness(contextGuid)
					{
						Guid = Guid.NewGuid(),
						ChangeStatus = EntityChangeSt.New,
						ContextGuid = contextGuid,
						Price = price,
						PriceLevelGuid = lastBuyType.Guid,
						ProductGuid = productGuid,
						Type = EnPriceLevelType.Buy
					};
				}
				productPriceRelationBusiness.Price = price;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await productPriceRelationBusiness.SaveAsync(token, tranName), ReturnedState.None, "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 327);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 331);
				res.AddReturnedValue(ex, "", "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 332);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 337), ReturnedState.None, "UpdateLastBuyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceRelationBusiness.cs", 337);
			}
		}
		return res;
	}

	public override IEditableDelivery GetRepository()
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}
}
