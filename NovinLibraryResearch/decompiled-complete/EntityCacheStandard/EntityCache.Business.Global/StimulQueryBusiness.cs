using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.Records;

namespace EntityCache.Business.Global;

public class StimulQueryBusiness : IStimulQuery, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid? Guid { get; set; }

	public int? Id { get; set; }

	public int RefrencedId { get; set; }

	public int Order { get; set; }

	public DateTime Date { get; set; }

	public DateTime Modified { get; set; }

	public bool Status => true;

	public string ContextGuid { get; set; }

	public string SecondKey { get; set; }

	Guid IHasJustGuid.Guid { get; set; }

	public StimulQueryBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync(string contextGuid, IEnumerable<Guid> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
			try
			{
				List<StimulQueryBusiness> list = new List<StimulQueryBusiness>();
				int num = 0;
				if (lst == null || !lst.Any())
				{
					return ret;
				}
				foreach (Guid item in lst)
				{
					StimulQueryBusiness obj = new StimulQueryBusiness(contextGuid)
					{
						Guid = item
					};
					num = (obj.Order = checked(num + 1));
					list.Add(obj);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 44);
				}
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await SaveAsync(contextGuid, list, token, tranName);
				ret.value = returnedSaveFuncInfoWithValue.value;
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 47);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 49);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 52);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 53);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync(string contextGuid, IEnumerable<int> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			List<StimulQueryBusiness> list = new List<StimulQueryBusiness>();
			int num = 0;
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (int item in lst)
			{
				StimulQueryBusiness obj = new StimulQueryBusiness(contextGuid)
				{
					Id = item
				};
				num = (obj.Order = checked(num + 1));
				list.Add(obj);
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await SaveAsync(contextGuid, list, token, tranName);
			ret.value = returnedSaveFuncInfoWithValue.value;
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 69);
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 71);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 74);
			ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 75);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync(string contextGuid, IEnumerable<StimulQueryBusiness> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = System.Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			using (await _databaseSemaphore.GaurdAsync(true, "StimulQueryBusiness/SaveAsync", 60000, token))
			{
				ReturnedSaveFuncInfoWithValue<int> result;
				try
				{
					int num;
					_ = num - 1;
					_ = 2;
					try
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue;
						if (autoTran)
						{
							returnedSaveFuncInfoWithValue = ret;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 89), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 89);
						}
						transactionHasBeenCreated = true;
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue2 = await GetNextRefrenced(contextGuid, token, tranName);
						returnedSaveFuncInfoWithValue2.throwExceptionIfError();
						if (returnedSaveFuncInfoWithValue2.value <= 0)
						{
							throw new ArgumentException($"RefrenecdId from StimulQuery is {returnedSaveFuncInfoWithValue2.value}");
						}
						ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 94);
						ret.value = returnedSaveFuncInfoWithValue2.value;
						if (ret.HasError)
						{
							result = ret;
							goto IL_0550;
						}
						foreach (StimulQueryBusiness item in lst)
						{
							item.RefrencedId = returnedSaveFuncInfoWithValue2.value;
						}
						if (!(lst?.Any() ?? false))
						{
							result = ret;
							goto IL_0550;
						}
						returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.StimulQuery.SaveRangeAsync(lst, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 99);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 103);
						ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 104);
					}
				}
				finally
				{
					if (autoTran && transactionHasBeenCreated)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = ret;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 109), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 109);
					}
				}
				return ret;
				IL_0550:
				return result;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> SaveAsync(string contextGuid, IEnumerable<StimulQueryComlpexRecord> lst, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
			try
			{
				List<StimulQueryBusiness> list = new List<StimulQueryBusiness>();
				int num = 0;
				if (lst == null || !lst.Any())
				{
					return ret;
				}
				foreach (StimulQueryComlpexRecord item in lst)
				{
					StimulQueryBusiness obj = new StimulQueryBusiness(contextGuid)
					{
						Guid = item.Guid
					};
					num = (obj.Order = checked(num + 1));
					obj.Id = item.Id;
					obj.SecondKey = item.SecondKey;
					list.Add(obj);
				}
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await SaveAsync(contextGuid, list, token, tranName);
				ret.value = returnedSaveFuncInfoWithValue.value;
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 132);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 136);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 137);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> TruncateAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<int> res = new ReturnedSaveFuncInfoWithValue<int>();
			bool autoTran = string.IsNullOrEmpty(tranName);
			if (autoTran)
			{
				tranName = System.Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			using (await _databaseSemaphore.GaurdAsync(true, "StimulQueryBusiness/TruncateAsync", 60000, token))
			{
				try
				{
					int num;
					_ = num - 1;
					_ = 1;
					try
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue;
						if (autoTran)
						{
							returnedSaveFuncInfoWithValue = res;
							returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 151), ReturnedState.None, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 151);
						}
						if (res.HasError)
						{
							return res;
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.StimulQuery.TruncateAsync(token, tranName), ReturnedState.None, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 154);
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 158);
						res.AddReturnedValue(ex, "", "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 159);
					}
				}
				finally
				{
					if (autoTran && transactionHasBeenCreated)
					{
						ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = res;
						returnedSaveFuncInfoWithValue.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 164), ReturnedState.None, "TruncateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\StimulQueryBusiness.cs", 164);
					}
				}
				return res;
			}
		}
	}

	private static Task<ReturnedSaveFuncInfoWithValue<int>> GetNextRefrenced(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.StimulQuery.GetNextRefrenced(token, tranName);
	}
}
