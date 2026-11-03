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

public class LotteryScoreBusiness : Serializable<LotteryScoreBusiness>, ILotteryScore, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status => Status == LotteryStatus.Deleted;

	public DateTime ExpirationDate { get; set; }

	public Guid OrderGuid { get; set; }

	public DateTime DateM { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public decimal Score { get; set; }

	public string Description { get; set; }

	public LotteryStatus Status { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string ContextGuid { get; set; }

	public LotteryScoreBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<LotteryScoreBusiness>> GetAllAsync(string contextGuid, DateTime date1, DateTime date2, LotteryStatus status, Guid cusGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LotteryScoreRepository.GetAllAsync(date1, date2, status, cusGuid, token, tranName);
	}

	public static Task<LotteryScoreBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LotteryScoreRepository.GetAsync(guid, token, tranName);
	}

	public static async Task<List<LotteryScoreBusiness>> GetAllAsync(string contextGuid, string search, CancellationToken token, DateTime date1, DateTime date2, LotteryStatus status, Guid cusGuid)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<LotteryScoreBusiness> enumerable = await GetAllAsync(contextGuid, date1, date2, status, cusGuid, token);
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable.Where((LotteryScoreBusiness x) => x.DateSh.Contains(item) || x.Score.ToString().Contains(item) || x.Description.Contains(item))?.ToList();
						}
					}
				}
				enumerable = enumerable?.OrderBy((LotteryScoreBusiness o) => o.DateSh)?.ToList();
				return token.IsCancellationRequested ? null : enumerable?.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 75);
				throw;
			}
		}
	}

	public static async Task<IEnumerable<LotteryScoreBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.LotteryScoreRepository.GetAllAsync(status, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(string contextGuid, List<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 93), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 93);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.LotteryRepository.DeleteRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 95);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 99);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 100);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 105), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 105);
			}
		}
		return ret;
	}

	public static async Task<List<Guid>> StartLottery(string contextGuid, int count, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.LotteryScoreRepository.StartLottery(count, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LotteryScoreBusiness> lst, bool send2server, CancellationToken token = default(CancellationToken), short maxParalelTasks = 20)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (LotteryScoreBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 121);
				}
				tasks.Add(item.SaveAsync(token, awaitOnSemaphore: true, 10));
				if (tasks.Count > maxParalelTasks)
				{
					Task<ReturnedSaveFuncInfo> finishedTask = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await finishedTask, ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 126);
					tasks.Remove(finishedTask);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 131);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 135);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 136);
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "LotteryScoreBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 151), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 151);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.LotteryScoreRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 153);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 150);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 158);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 159);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 164), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\LotteryScoreBusiness.cs", 164);
			}
		}
		return ret;
	}
}
