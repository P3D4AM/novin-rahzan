using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Entities.Interfaces;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace EntityCache.Business.Customers;

public class CusCardBusiness : Serializable<CusCardBusiness>, ICusCard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid CusGuid { get; set; }

	public string CardName { get; set; }

	public string CardNo { get; set; }

	public string Pass { get; set; }

	public DateTime DateStart { get; set; } = DateTime.Now;


	public DateTime DateExpire { get; set; } = DateTime.Now;


	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public CusCardBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<CusCardBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICusCard.GetAsync(guid, token, tranName);
	}

	public static Task<CusCardBusiness> GetAsync(string contextGuid, string cardNumber, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICusCard.GetAsync(cardNumber, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<CusCardBusiness> items, bool send2server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (CusCardBusiness item in items)
			{
				current = checked(current + 1);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 52);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(items.Count(), current, item.CardName, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 62);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 63);
		}
		finally
		{
			if (percentCompleted != null)
			{
				invokeToken2?.Cancel();
				invokeToken2 = new CancellationTokenSource(60000);
				CancellationToken invokeTkn = invokeToken2.Token;
				Task.Run(delegate
				{
					percentCompleted(new PercenteCompletedEventArgs(items?.Count() ?? 0, items?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CusCardBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 87), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 87);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICusCard.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 89);
						ret.throwExceptionIfError();
						if (!ret.HasError && system == EnSystem.UserInterface)
						{
							await (await CusBusiness.GetAsync(ContextGuid, system, CusGuid, token, tranName)).UpdateServerStatusAsync(ServerStatusForClient.None, token, tranName);
						}
						goto end_IL_0125;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 86);
					goto end_IL_007d;
					end_IL_0125:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 100);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 101);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 106), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 106);
			}
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(CardNo))
			{
				returnedSaveFuncInfo.AddError("پر کردن شماره کارت الزامی است ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 115);
			}
			if (string.IsNullOrEmpty(CardName))
			{
				returnedSaveFuncInfo.AddError("انتخاب طرف حساب الزامی است ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 116);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 120);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 121);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public static Task<IEnumerable<CusCardBusiness>> GetAllByCusGuidAsync(string contextGuid, Guid custGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICusCard.GetAllByCusGuidAsync(custGuid, status, token, tranName);
	}

	public static Task<IEnumerable<CusCardBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICusCard.GetAllAsync(status, token, tranName);
	}

	public static async Task<List<CusCardBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, bool? status = true)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<CusCardBusiness> res = await GetAllAsync(contextGuid, status, token);
				await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
				foreach (string item in search?.FixString()?.RemoveCharactersForLinkSearch()?.SplitString()?.Distinct() ?? new List<string>())
				{
					if (!string.IsNullOrEmpty(item?.Trim()))
					{
						res = res.Where((CusCardBusiness x) => x.CardNo.ToString().Contains(item) || x.CardName.Contains(item));
					}
				}
				res = res?.OrderBy((CusCardBusiness o) => o.CardNo)?.ToList();
				return token.IsCancellationRequested ? null : res?.ToList();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 146);
				throw;
			}
			return null;
		}
	}

	public static Task<CusCardBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? tafsilGuid = null, string description = "", Guid? guid = null, string tranName = "")
	{
		try
		{
			CusCardBusiness cusCardBusiness = new CusCardBusiness(contextGuid);
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				cusCardBusiness.CusGuid = tafsilGuid.Value;
			}
			if (!string.IsNullOrEmpty(description))
			{
				cusCardBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				cusCardBusiness.Guid = guid.Value;
			}
			else
			{
				cusCardBusiness.Guid = Guid.NewGuid();
			}
			return Task.FromResult(cusCardBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCardBusiness.cs", 164);
			throw;
		}
	}

	public static Task<IEnumerable<CustomerCardModel>> GetAllModelAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICusCard.GetAllModelAsync(token, tranName);
	}
}
