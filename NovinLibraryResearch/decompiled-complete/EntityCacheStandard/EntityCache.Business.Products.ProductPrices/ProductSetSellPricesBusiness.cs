using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Filters;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Business.Products.ProductPrices;

public class ProductSetSellPricesBusiness
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public List<Tuple<Guid, decimal>> List { get; set; }

	public decimal KasreRial { get; set; }

	public Guid GroupGuid { get; set; }

	public EnProductChangePriceType Type { get; set; }

	public EnPriceLevelType PriceLevelType { get; set; }

	public bool UpToDown { get; set; }

	public int DigitAfterPoint { get; private set; }

	public ProductSetSellPricesBusiness(string contextGuid)
	{
		DigitAfterPoint = (ClsCache.GetCache(contextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, ProductSetSellPricesBusiness item, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 9;
			ReturnedSaveFuncInfo result;
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ProductSetSellPricesBusiness/SaveRangeAsync", 300000, token);
				List<ProductPriceRelationBusiness> priceRelation;
				List<Guid> prdGuidList;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (dis.Token.IsCancellationRequested)
				{
					result = res.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 42);
				}
				else
				{
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 43), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 43);
					}
					transactionHasBeenCreated = true;
					if (!(item?.List?.Any()).GetValueOrDefault())
					{
						await res.AddErrorAsync("هیچ درصدی جهت تغییر قیمت وارد نشده است".GetTranslate(), "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 47);
					}
					if ((item?.List?.Any((Tuple<Guid, decimal> q) => q.Item2 > 100m)).GetValueOrDefault())
					{
						await res.AddErrorAsync("درصد وارد شده نمی تواند از 100 بزرگتر باشد".GetTranslate(), "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 50);
					}
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						ProductPriceRelationFilter filter = new ProductPriceRelationFilter
						{
							ProductGroupGuid = item.GroupGuid
						};
						priceRelation = (await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceRelation.GetAllAsync(filter, dis.Token, tranName))?.ToList();
						if (!(priceRelation?.Any() ?? false))
						{
							result = res;
						}
						else
						{
							if (item.PriceLevelType == EnPriceLevelType.Sell)
							{
								foreach (Tuple<Guid, decimal> priceLevel2 in item.List)
								{
									if ((priceLevel2?.Item1 ?? Guid.Empty) == Guid.Empty || (priceLevel2?.Item2 ?? 0m) <= 0m)
									{
										continue;
									}
									IEnumerable<ProductPriceRelationBusiness> enumerable = priceRelation.Where((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceLevel2.Item1);
									if (enumerable == null || !enumerable.Any())
									{
										continue;
									}
									foreach (ProductPriceRelationBusiness item2 in enumerable)
									{
										if (item2 != null)
										{
											decimal num = FinancialCalculator.CalculateProfit(item2.Price, (item.Type == EnProductChangePriceType.Increase) ? priceLevel2.Item2 : (-priceLevel2.Item2), item.UpToDown, item.DigitAfterPoint);
											item2.Price = SetKasr(item2.Price + num, item.KasreRial);
										}
									}
								}
								prdGuidList = priceRelation.Select((ProductPriceRelationBusiness q) => q.ProductGuid).Distinct().ToList();
								goto IL_0b57;
							}
							ProductCompactFilter productCompactFilter = new ProductCompactFilter
							{
								LoadJustParents = false,
								Status = true
							};
							if (item.GroupGuid != Guid.Empty)
							{
								productCompactFilter.GroupGuids = new List<Guid> { item.GroupGuid };
							}
							IEnumerable<ProductCompactBusiness> productList = await ProductCompactBusiness.GetAllAsync(contextGuid, EnSystem.None, productCompactFilter, token, tranName);
							if (productList?.Any() ?? false)
							{
								foreach (ProductCompactBusiness prd in productList)
								{
									if (prd == null)
									{
										continue;
									}
									decimal num2 = ((await KardexBusiness.GetLastEnteredRecordAsync(contextGuid, prd.Guid, null, token, tranName))?.RemUnitPrice).GetValueOrDefault();
									if (num2 <= 0m)
									{
										num2 = await ProductPriceRelationBusiness.GetLastBuyDecimalAsync(contextGuid, system, prd.Guid, token, tranName);
									}
									if (num2 <= 0m)
									{
										continue;
									}
									foreach (Tuple<Guid, decimal> priceLevel in item.List)
									{
										if (!((priceLevel?.Item1 ?? Guid.Empty) == Guid.Empty) && !((priceLevel?.Item2 ?? 0m) <= 0m))
										{
											ProductPriceRelationBusiness productPriceRelationBusiness = priceRelation.SingleOrDefault((ProductPriceRelationBusiness q) => q.PriceLevelGuid == priceLevel.Item1 && q.ProductGuid == prd.Guid);
											if (productPriceRelationBusiness == null)
											{
												productPriceRelationBusiness = new ProductPriceRelationBusiness(contextGuid)
												{
													Guid = Guid.NewGuid(),
													ProductGuid = prd.Guid,
													ChangeStatus = EntityChangeSt.New,
													ContextGuid = contextGuid,
													Modified = DateTime.Now,
													PriceLevelGuid = priceLevel.Item1,
													Type = EnPriceLevelType.Sell
												};
												priceRelation.Add(productPriceRelationBusiness);
											}
											decimal num3 = FinancialCalculator.CalculateProfit(num2, (item.Type == EnProductChangePriceType.Increase) ? priceLevel.Item2 : (-priceLevel.Item2), item.UpToDown, item.DigitAfterPoint);
											productPriceRelationBusiness.Price = SetKasr(num3 + num2 + num3, item.KasreRial);
										}
									}
								}
								prdGuidList = productList.Select((ProductCompactBusiness q) => q.Guid).ToList();
								goto IL_0b57;
							}
							result = res;
						}
					}
				}
				goto end_IL_0130;
				IL_0b57:
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceRelation.SaveRangeAsync(priceRelation, 1, dis.Token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 133);
				if (res.HasError || !(prdGuidList?.Any() ?? false))
				{
					result = res;
					goto end_IL_0130;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.UpdateServerStatusAsync(contextGuid, prdGuidList, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 135);
				goto end_IL_0067;
				end_IL_0130:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 137);
				goto end_IL_0067;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 140);
				res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 141);
				goto end_IL_0067;
			}
			return result;
			end_IL_0067:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 146), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 146);
			}
		}
		return res;
	}

	private static decimal SetKasr(decimal p, decimal kasr)
	{
		try
		{
			if (kasr <= 0m)
			{
				return p;
			}
			return p - p % kasr;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetKasr", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductSetSellPricesBusiness.cs", 157);
		}
		return 0m;
	}
}
