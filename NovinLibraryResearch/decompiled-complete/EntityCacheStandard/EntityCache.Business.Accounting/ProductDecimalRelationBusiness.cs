using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class ProductDecimalRelationBusiness : IProductDecimalRelation, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private decimal _amount;

	public Guid ProductGuid { get; set; }

	public Guid DecimalGuid { get; set; }

	public Guid ProductGroupGuid { get; set; }

	public decimal Amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = (decimal)(double)Math.Round(value, 3);
		}
	}

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public string ProductVahed { get; set; }

	public string DecimalName { get; set; }

	public decimal DecimalPrice { get; set; }

	public decimal LastBuy { get; set; }

	public decimal Varriance => Math.Abs(DecimalSellPrice - LastBuy);

	public decimal DecimalSellPrice => DecimalPrice * Amount;

	public string ContextGuid { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public ProductDecimalRelationBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 43);
		}
	}

	public static Task<IEnumerable<ProductDecimalRelationBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductPrices.GetAllAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<ProductDecimalRelationBusiness> list, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ProductPricesBusiness/SaveRangeAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 57);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 58), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 58);
						}
						transactionHasBeenCreated = true;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ProductPrices.RemoveRangeAsync(token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 61);
							if (!ret.HasError)
							{
								foreach (ProductDecimalRelationBusiness item in list)
								{
									if (item == null)
									{
										continue;
									}
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ProductPriceRelationBusiness.UpdateLastBuyAsync(contextGuid, item.ProductGuid, Math.Round(item.DecimalSellPrice, item.DigitForRound), token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 67);
									if (!ret.HasError)
									{
										continue;
									}
									result = ret;
									goto end_IL_0076;
								}
								goto end_IL_0113;
							}
							result = ret;
						}
					}
					goto end_IL_0076;
					end_IL_0113:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 73);
				ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 74);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 79), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductDecimalRelationBusiness.cs", 79);
			}
		}
		return ret;
	}
}
