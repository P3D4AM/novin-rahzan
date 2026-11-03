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
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting;

public class LotteryBusiness : Serializable<LotteryBusiness>, ILottery, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	bool IHasStatus.Status => Status == LotteryStatus.Deleted;

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public DateTime DateM { get; set; }

	public LotteryStatus Status { get; set; }

	public string Description { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string StatusName => Status.GetDisplay();

	public string Time => DateM.ToLongTimeString();

	public string ContextGuid { get; set; }

	public LotteryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<LotteryBusiness>> GetAllAsync(string contextGuid, LotteryStatus status, DateTime date1, DateTime date2, Guid cusGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LotteryRepository.GetAllAsync(status, date1, date2, cusGuid, token, tranName);
	}

	public static async Task<List<LotteryBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, LotteryStatus status, DateTime date1, DateTime date2, Guid cusGuid)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<LotteryBusiness> enumerable = await GetAllAsync(contextGuid, status, date1, date2, cusGuid, token);
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable.Where((LotteryBusiness x) => x.DateSh.Contains(item) || x.Time.Contains(item) || x.StatusName.Contains(item) || x.Description.Contains(item))?.ToList();
						}
					}
				}
				enumerable = enumerable?.OrderBy((LotteryBusiness o) => o.DateSh)?.ToList();
				return token.IsCancellationRequested ? null : enumerable?.ToList();
			}
			catch (OperationCanceledException ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 69);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 74);
				return new List<LotteryBusiness>();
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 93), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 93);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.LotteryRepository.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 95);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 99);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 100);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 105), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 105);
			}
		}
		return ret;
	}

	public static async Task<LotteryBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.LotteryRepository.GetAsync(guid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LotteryBusiness> lst, bool send2server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CancellationTokenSource invokeToken = new CancellationTokenSource();
			int current = 0;
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (LotteryBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 124);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 125);
				if (percentCompleted != null)
				{
					invokeToken?.Cancel();
					invokeToken = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.DateSh, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 135);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 136);
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "LotteryBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 152), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 152);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.LotteryRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 154);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 151);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 159);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 160);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && ClsCache.GetCache(ContextGuid)?.DatabaseManager != null && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 165), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryBusiness.cs", 165);
			}
		}
		return ret;
	}
}
