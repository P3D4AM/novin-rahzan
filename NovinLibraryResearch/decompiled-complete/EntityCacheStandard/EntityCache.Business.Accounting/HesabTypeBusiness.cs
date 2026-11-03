using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting;

public class HesabTypeBusiness : IHesabType, IHasJustGuid, IHasStatus, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string ContextGuid { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public short Code { get; set; }

	public HesabTypeBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<HesabTypeBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return await ClsCache.GetCache(contextGuid).CacheRepository.IHesabType.GetAsync(guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 38);
		}
		return null;
	}

	public static HesabTypeBusiness Get(string contextGuid, ENHesabType HesabType)
	{
		try
		{
			return HesabType switch
			{
				ENHesabType.None => new HesabTypeBusiness(contextGuid)
				{
					Guid = Guid.Empty,
					Name = "<انتخاب کنید>".GetTranslate()
				}, 
				ENHesabType.Darayi => new HesabTypeBusiness(contextGuid)
				{
					Guid = ParentDefaults.HesabTypeGuids.Darayi,
					Name = HesabType.GetDisplay()
				}, 
				ENHesabType.Bedehi => new HesabTypeBusiness(contextGuid)
				{
					Guid = ParentDefaults.HesabTypeGuids.Bedehi,
					Name = HesabType.GetDisplay()
				}, 
				ENHesabType.Sarmaye => new HesabTypeBusiness(contextGuid)
				{
					Guid = ParentDefaults.HesabTypeGuids.Sarmaye,
					Name = HesabType.GetDisplay()
				}, 
				ENHesabType.SodoZiani => new HesabTypeBusiness(contextGuid)
				{
					Guid = ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani,
					Name = HesabType.GetDisplay()
				}, 
				ENHesabType.Entezami => new HesabTypeBusiness(contextGuid)
				{
					Guid = ParentDefaults.HesabTypeGuids.HesabHayeEntezami,
					Name = HesabType.GetDisplay()
				}, 
				_ => throw new Exception($"HesabType {HesabType} not defined!"), 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 89);
			throw;
		}
	}

	public static List<HesabTypeBusiness> GetAll(string contextGuid, bool isLoadGuidEmpty)
	{
		try
		{
			List<HesabTypeBusiness> list = new List<HesabTypeBusiness>();
			if (isLoadGuidEmpty)
			{
				list.Add(Get(contextGuid, ENHesabType.None));
			}
			list.Add(Get(contextGuid, ENHesabType.Darayi));
			list.Add(Get(contextGuid, ENHesabType.Bedehi));
			list.Add(Get(contextGuid, ENHesabType.Sarmaye));
			list.Add(Get(contextGuid, ENHesabType.SodoZiani));
			list.Add(Get(contextGuid, ENHesabType.Entezami));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 108);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, List<HesabTypeBusiness> lstHesabsList, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(lstHesabsList?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (HesabTypeBusiness item in lstHesabsList)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 123);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 124);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsList.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 136);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 137);
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
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsList?.Count() ?? 0, lstHesabsList?.Count() ?? 0, "", invokeTkn));
				});
			}
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "HesabTypeBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 160);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 161), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 161);
						}
						ret.throwExceptionIfError();
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IHesabType.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 164);
						if (!ret.HasError)
						{
							goto end_IL_011b;
						}
						result = ret;
					}
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 169);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 170);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 175), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 175);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> FixAsync(string contextGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<HesabTypeBusiness> all = GetAll(contextGuid, isLoadGuidEmpty: false);
			foreach (HesabTypeBusiness item in all)
			{
				if (item.Guid == ParentDefaults.HesabTypeGuids.Darayi)
				{
					item.Code = 1;
				}
				if (item.Guid == ParentDefaults.HesabTypeGuids.Bedehi)
				{
					item.Code = 2;
				}
				if (item.Guid == ParentDefaults.HesabTypeGuids.Sarmaye)
				{
					item.Code = 3;
				}
				if (item.Guid == ParentDefaults.HesabTypeGuids.HesabhayeSoodoZiani)
				{
					item.Code = 7;
				}
				if (item.Guid == ParentDefaults.HesabTypeGuids.HesabHayeEntezami)
				{
					item.Code = 9;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 193);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 198);
			res.AddReturnedValue(ex, "", "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\HesabTypeBusiness.cs", 199);
		}
		return res;
	}
}
