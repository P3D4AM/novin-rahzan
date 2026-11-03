using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Coding;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.SerializeData;
using PacketParser.Filters;

namespace EntityCache.Business.Accounting;

public class SandoghBusiness : Serializable<SandoghBusiness>, ISandogh, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _getAllSemaphore = new SemaphoreSlim(1, 10);

	private SandoghData _sandoghData;

	public SandoghData SandoghData => _sandoghData ?? (_sandoghData = (Tafsil?.SerializedData).FromJson<SandoghData>("SandoghData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 24) ?? new SandoghData());

	public TafsilBusiness Tafsil { get; private set; }

	public long Code
	{
		get
		{
			return Tafsil.Code;
		}
		set
		{
			Tafsil.Code = value;
		}
	}

	public ServerStatusForClient ServerStatus
	{
		get
		{
			return Tafsil.ServerStatus;
		}
		set
		{
			Tafsil.ServerStatus = value;
		}
	}

	public Guid Guid
	{
		get
		{
			return Tafsil.Guid;
		}
		set
		{
			Tafsil.Guid = value;
		}
	}

	public DateTime Modified
	{
		get
		{
			return Tafsil.Modified;
		}
		set
		{
			Tafsil.Modified = value;
		}
	}

	public string Name
	{
		get
		{
			return Tafsil.Name;
		}
		set
		{
			Tafsil.Name = value;
		}
	}

	public string Description
	{
		get
		{
			return Tafsil.Description;
		}
		set
		{
			Tafsil.Description = value;
		}
	}

	public decimal Account { get; private set; }

	public bool Status
	{
		get
		{
			return Tafsil.Status;
		}
		set
		{
			Tafsil.Status = value;
		}
	}

	public EntityChangeSt EntityChangeSt => Tafsil.ChangeStatus;

	public Guid TafsilGroupGuid
	{
		get
		{
			return Tafsil.TafsilGroupGuid;
		}
		set
		{
			Tafsil.TafsilGroupGuid = value;
		}
	}

	[Obsolete("Use NegativeAmount")]
	public bool Manfi
	{
		get
		{
			return SandoghData?.SandoghManfi ?? false;
		}
		set
		{
			SandoghData.SandoghManfi = value;
		}
	}

	public ReturnedState NegativeAmount
	{
		get
		{
			if (SandoghData == null)
			{
				return ReturnedState.Warning;
			}
			if (!SandoghData.NegativeAmount.HasValue)
			{
				if (!SandoghData.SandoghManfi)
				{
					return ReturnedState.Error;
				}
				return ReturnedState.Warning;
			}
			return SandoghData.NegativeAmount.Value;
		}
		set
		{
			SandoghData.NegativeAmount = value;
		}
	}

	public string ContextGuid
	{
		get
		{
			return Tafsil.ContextGuid;
		}
		set
		{
			if (Tafsil != null)
			{
				Tafsil.ContextGuid = value;
			}
		}
	}

	public SandoghBusiness(string contextGuid)
	{
		Tafsil = new TafsilBusiness(contextGuid);
		ContextGuid = contextGuid;
	}

	public static async Task<SandoghBusiness> GetFactoryAsync(TafsilBusiness tafsil, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (tafsil == null)
		{
			throw new ArgumentNullException("tafsil");
		}
		try
		{
			SandoghBusiness item = new SandoghBusiness(tafsil.ContextGuid)
			{
				Tafsil = tafsil
			};
			if (tafsil.Guid != Guid.Empty)
			{
				SandoghBusiness sandoghBusiness = item;
				sandoghBusiness.Account = await GetAccountAsync(tafsil, system, token, tranName);
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 74);
			throw;
		}
	}

	private static async Task<decimal> GetAccountAsync(TafsilBusiness tafsil, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (tafsil == null)
			{
				return default(decimal);
			}
			List<Guid> list = await CashierBusiness.GetAllTafsilGroupGuidsForTurnoverAsync(tafsil.ContextGuid, system, token, tranName);
			if (list == null)
			{
				return await AccountBusiness.GetAccountSumAsync(tafsil.ContextGuid, tafsil.Guid, new List<Guid>(1) { ParentDefaults.MoeinCoding.CLSMoein7 }, tranName, token);
			}
			if (list == null || !list.Any())
			{
				return default(decimal);
			}
			if (!list.Contains(tafsil.TafsilGroupGuid))
			{
				return default(decimal);
			}
			return await AccountBusiness.GetAccountSumAsync(tafsil.ContextGuid, tafsil.Guid, new List<Guid>(1) { ParentDefaults.MoeinCoding.CLSMoein7 }, tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 92);
			throw;
		}
	}

	public static Task<IEnumerable<SandoghBusiness>> GetAllAsync(string contextGuid, EnSystem system, string search = "", bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return GetAllAsync(contextGuid, system, search, status, tranName, token, new List<Guid> { ParentDefaults.MoeinCoding.CLSMoein7 });
	}

	public static async Task<IEnumerable<SandoghBusiness>> GetAllAsync(string contextGuid, EnSystem system, string search, bool? status, string tranName, CancellationToken token, List<Guid> lstMoeinGuid)
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _getAllSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			TafsilFilter filter = new TafsilFilter
			{
				NameSearchString = search,
				HesabType = HesabType.Sandogh,
				Status = status,
				MoeinGuidList = lstMoeinGuid
			};
			return await CreateSandoghObjectAsync(await TafsilBusiness.GetAllAsync(contextGuid, system, filter, token, tranName, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs"), system);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 118);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_getAllSemaphore.Release();
			}
		}
	}

	public static async Task<SandoghBusiness> GetAsync(string contextGuid, long code, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			TafsilBusiness tafsilBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetAsync(code, token, tranName);
			if (tafsilBusiness == null)
			{
				return null;
			}
			return await GetFactoryAsync(tafsilBusiness, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 133);
			throw;
		}
	}

	public static async Task<SandoghBusiness> GetAsync(string contextGuid, Guid guid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (Guid.Empty == guid)
			{
				return null;
			}
			TafsilBusiness tafsilBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ITafsil.GetAsync(guid, token, tranName);
			if (tafsilBusiness == null)
			{
				return null;
			}
			return await GetFactoryAsync(tafsilBusiness, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 148);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool awaitOnSemaphore, bool updateAccount, bool send2Server, short tryCount = 10, string tranName = "", short autoFixValidationErrorsCount = 0)
	{
		bool autoCommitTran = string.IsNullOrEmpty(tranName);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (autoCommitTran)
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
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "SandoghBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 162);
					}
					else
					{
						Tafsil.SerializedData = Json.ToStringJson(SandoghData);
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoCommitTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 163), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 163);
						}
						transactionHasBeenCreated = true;
						Tafsil.HType = HesabType.Sandogh;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName, autoFixValidationErrorsCount), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 166);
						ret.throwExceptionIfError();
						Modified = DateTime.Now;
						Tafsil.Modified = DateTime.Now;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Tafsil.SaveAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, dis.Token, updateAccount, awaitOnSemaphore: false, autoFixValidationErrorsCount, send2Server, await4sent: false, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 170);
						if (!ret.HasError)
						{
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
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 175);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 176);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 181), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 181);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName, short autoFixValidationErrorsCount)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				return res;
			}
			decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, ParentDefaults.MoeinCoding.CLSMoein7, Guid, true, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Tafsil.CheckValidationAsync(system, keepCurrentCodeAndChangedOtherTafsilCodesIfWasDuplicate: false, tranName, autoFixValidationErrorsCount, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 193);
			if (res.HasError)
			{
				return res;
			}
			if (Guid == Guid.Empty)
			{
				res.AddError(system, "شناسه تهی", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 195);
			}
			if (system == EnSystem.UserInterface && account > 0m)
			{
				res.AddReturnedValue(NegativeAmount, $"موجودی حساب {Name} ({Math.Abs(account):N0}) بستانکار(منفی) می باشد \r\n" + "در چنین شرایطی امکان غیرفعال کردن موجودی منفی وجود ندارد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 197);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 202);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 203);
		}
		return res;
	}

	public static async Task<decimal> TotalAccountAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await AccountBusiness.GetAllAsync(contextGuid, ParentDefaults.MoeinCoding.CLSMoein7, null, true, token))?.Sum((AccountBusiness q) => q.Account) ?? 0m;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TotalAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 216);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveFromTafsil(string contextGuid, EnSystem system, CancellationToken token, List<TafsilBusiness> lstHesabsListTafsil, bool awaitOnSemaphore, bool updateAccount, bool send2server, short tryCount = 10, string tranName = "")
	{
		bool autoCommitTran = string.IsNullOrEmpty(tranName);
		if (autoCommitTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "SandoghBusiness/SaveFromTafsil", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 229);
					}
					else
					{
						if (autoCommitTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 230), ReturnedState.None, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 230);
						}
						transactionHasBeenCreated = true;
						if (ClsCache.GetCache(contextGuid)?.DatabaseManager != null && !(ClsCache.GetCache(contextGuid)?.IsDisposed ?? true))
						{
							foreach (TafsilBusiness item in lstHesabsListTafsil)
							{
								if (item == null || item.HType != HesabType.Sandogh)
								{
									continue;
								}
								SandoghBusiness obj = await GetFactoryAsync(item, system, token, tranName);
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, token, awaitOnSemaphore: false, updateAccount, send2server, tryCount, tranName, 0), ReturnedState.None, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 237);
								if (!ret.HasError)
								{
									continue;
								}
								result = ret;
								goto end_IL_0076;
							}
							goto end_IL_0114;
						}
						result = ret.AddError(system, "cache is disposed!", "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 232);
					}
					goto end_IL_0076;
					end_IL_0114:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 243);
				throw;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 249), ReturnedState.None, "SaveFromTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 249);
			}
		}
		return ret;
	}

	public static async Task<SandoghBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? guid = null, Guid? tafsilGroupGuid = null, string description = "", CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			TafsilBusiness tafsil = new TafsilBusiness(contextGuid)
			{
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				System = false
			};
			if ((tafsilGroupGuid ?? Guid.Empty) != Guid.Empty)
			{
				tafsil.TafsilGroupGuid = tafsilGroupGuid.Value;
				tafsil.Code = (await TafsilBusiness.NextCodeInGroupAsync(contextGuid, system, tafsilGroupGuid.Value, token))?.value ?? 0;
			}
			if (!string.IsNullOrEmpty(description))
			{
				tafsil.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				tafsil.Guid = guid.Value;
			}
			else
			{
				tafsil.Guid = Guid.NewGuid();
			}
			return await GetFactoryAsync(tafsil, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 277);
			throw;
		}
	}

	private static async Task<List<SandoghBusiness>> CreateSandoghObjectAsync(IEnumerable<TafsilBusiness> list, EnSystem system)
	{
		List<SandoghBusiness> lst = new List<SandoghBusiness>();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			foreach (TafsilBusiness item in list)
			{
				List<SandoghBusiness> list2 = lst;
				list2.Add(await GetFactoryAsync(item, system));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateSandoghObjectAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\SandoghBusiness.cs", 294);
			throw;
		}
		return lst;
	}
}
