using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class DecimalUpdatesBusiness : IDecimalUpdates, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid DecimalGuid { get; set; }

	public DateTime DateM { get; set; }

	public decimal Price { get; set; }

	public bool Status { get; set; } = true;


	public string DateSh => DateM.MiladiToCalture();

	public string Time => $"{DateM.Hour}:{DateM.Minute}";

	public string ContextGuid { get; set; }

	public decimal Tolerance { get; private set; }

	public DecimalUpdatesBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<DecimalUpdatesBusiness>> GetAllByDecimalGuidAsync(string contextGuid, Guid decimalGuid, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<DecimalUpdatesBusiness> obj = await ClsCache.GetCache(contextGuid).CacheRepository.DecimalUpdatesRepository.GetAllByDecimalGuidAsync(decimalGuid, status, token, tranName);
			CalculateTolerance(obj.OrderBy((DecimalUpdatesBusiness q) => q.DateM), token);
			return obj;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllByDecimalGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 42);
			throw;
		}
	}

	private static void CalculateTolerance(IEnumerable<DecimalUpdatesBusiness> list, CancellationToken token)
	{
		try
		{
			if (list == null || !list.Any() || token.IsCancellationRequested)
			{
				return;
			}
			decimal price = list.First().Price;
			foreach (DecimalUpdatesBusiness item in list)
			{
				item.Tolerance = item.Price - price;
				price = item.Price;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateTolerance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 60);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, IEnumerable<DecimalUpdatesBusiness> Lst, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (Lst == null || !Lst.Any())
			{
				return ret;
			}
			foreach (DecimalUpdatesBusiness item in Lst)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 71);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 75);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 76);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "DecimalUpdateBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 92), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 92);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.DecimalUpdatesRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 94);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 91);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 100);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 101);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 106), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DecimalUpdatesBusiness.cs", 106);
			}
		}
		return ret;
	}
}
