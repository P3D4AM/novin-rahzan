using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.EventArgs;
using PacketParser.Filters.Khazane;

namespace EntityCache.Business.Global;

public class WageBusiness : IWage, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public EnWageType Type { get; set; }

	public string TypeForDispaly => Type.GetDisplay().GetTranslate();

	public EnWageCalculationMethod CalculationMethod { get; set; }

	public string CalcutationMethodForDisplay => CalculationMethod.GetDisplay().GetTranslate();

	public decimal PriceOrPercent { get; set; }

	public string PriceOrPercentForDisplay
	{
		get
		{
			if (CalculationMethod == EnWageCalculationMethod.Price)
			{
				return PriceOrPercent.NpToString("N0", 0);
			}
			return ((double)PriceOrPercent).ToString().Replace(".", "/");
		}
	}

	public decimal MinTransaction { get; set; }

	public decimal MaxTransaction { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string ContextGuid { get; private set; }

	public EnKhazaneMaster MasterType { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public bool IsChecked { get; set; }

	public Guid? ServerGuid { get; set; }

	public WageBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<WageBusiness>> GetAllAsync(string contextGuid, WageFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Wage.GetAllAsync(filter, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool autoResolveConfilicts, CancellationToken token, bool send2Server, Priority priority = Priority.Low, bool awaitOnSemaphore = true, string tranName = "", bool awaitOnSend = false)
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "WageBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 61);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 62), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 62);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, autoResolveConfilicts, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 64);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Wage.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 66);
							goto end_IL_0145;
						}
						result = ret;
					}
					goto end_IL_0080;
					end_IL_0145:;
				}
				goto end_IL_006c;
				end_IL_0080:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 70);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 71);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 76), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 76);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool autoResolveConfilicts, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.Transfer)
			{
				return res;
			}
			if (Type == EnWageType.None)
			{
				res.AddError("نوع عملیات مشخص نشده است", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 98);
			}
			ReturnedSaveFuncInfoWithValue<List<WageBusiness>> returnedSaveFuncInfoWithValue = await IsDuplicateCodeAsync(ContextGuid, MasterType, Type, MinTransaction, MaxTransaction, Guid, CalculationMethod, token, tranName);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 100);
			if (!Status)
			{
				return res;
			}
			if (returnedSaveFuncInfoWithValue.value?.Any((WageBusiness p) => p.Guid != Guid) ?? false)
			{
				if (autoResolveConfilicts)
				{
					foreach (WageBusiness item in returnedSaveFuncInfoWithValue.value)
					{
						if (item != null)
						{
							item.Status = false;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, autoResolveConfilicts: false, token, send2Server: true, Priority.Low, awaitOnSemaphore: false, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 112);
						}
					}
				}
				else
				{
					res.AddError(system, "مبلغ حداقل یا حداکثر تراکنش".GetTranslate() + " " + (" " + Name).GetTranslate() + " " + " با کارمزد دیگری همپوشانی دارد".GetTranslate() + " \r\n" + "حداقل تراکنش".GetTranslate() + ":" + $"{MinTransaction}\r\n" + "حداکثر تراکنش".GetTranslate() + ":" + $"{MaxTransaction}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 117);
					foreach (WageBusiness item2 in returnedSaveFuncInfoWithValue.value)
					{
						if (item2 != null)
						{
							res.AddError("تداخل با کارمزد".GetTranslate() + string.Format(" ({0})-({1})-({2}=>{3})", item2.Guid, item2.Name, item2.MinTransaction.NpToString("N0", 0), item2.MaxTransaction.NpToString("N0", 0)), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 122);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 128);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 129);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, bool autoResolveConfilicts, CancellationToken token, IEnumerable<WageBusiness> lst, bool send2Server, Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (WageBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 144);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, autoResolveConfilicts, token, send2Server), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 145);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 157);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\WageBusiness.cs", 158);
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
					percentCompleted(new PercenteCompletedEventArgs(lst?.Count() ?? 0, lst?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public static Task<WageBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Wage.GetAsync(guid, token, tranName);
	}

	public static Task<WageBusiness> GetByServerGuidAsync(string contextGuid, Guid serverGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Wage.GetByServerGuidAsync(serverGuid, token, tranName);
	}

	public static Task<ReturnedSaveFuncInfoWithValue<List<WageBusiness>>> IsDuplicateCodeAsync(string contextGuid, EnKhazaneMaster master, EnWageType typeEnteghal, decimal minPrice, decimal maxPrice, Guid guid, EnWageCalculationMethod calcMethod, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Wage.IsDuplicateCodeAsync(master, typeEnteghal, minPrice, maxPrice, guid, calcMethod, token, tranName);
	}
}
