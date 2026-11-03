using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Customer;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business;

public class CusGroupBusiness : BaseBusiness<CusGroupBusiness>, ICusGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<CusGroupBusiness>, IDeliverableEntity, IEditableDelivery
{
	private IEnumerable<CusCompactBusiness> customers;

	private static readonly SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private readonly SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private Guid _defaultMoeinGuid = Guid.Empty;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Description { get; set; }

	public string Name { get; set; }

	public bool Special { get; set; }

	public bool Status { get; set; }

	public Guid MoeinDefaultGuid
	{
		get
		{
			if (_defaultMoeinGuid == Guid.Empty)
			{
				_defaultMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein9;
			}
			return _defaultMoeinGuid;
		}
		set
		{
			_defaultMoeinGuid = value;
		}
	}

	public Guid TafsilGroupGuid { get; set; }

	public static Guid DefaultTafsilLevelGuid => Guid.Parse("6db9aa94-1a33-4a0a-dfd8-8f30a23fb3ff");

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public CusGroupBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<TafsilGroupBusiness> GetTafsilGroupAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return TafsilGroupBusiness.GetAsync(ContextGuid, TafsilGroupGuid, system, token, tranName);
	}

	public async Task<IEnumerable<CusCompactBusiness>> GetCustomers(string contextGuid, EnSystem system, bool? status, CancellationToken token)
	{
		IEnumerable<CusCompactBusiness> enumerable = customers;
		if (enumerable == null)
		{
			enumerable = (customers = (await CusCompactBusiness.GetAllAsync(contextGuid, system, token, null, null, status))?.Where((CusCompactBusiness w) => w.GroupGuid == Guid));
		}
		return enumerable;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, short autoFixValidationErrorsCount, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		checked
		{
			try
			{
				if (string.IsNullOrEmpty(Name))
				{
					ret.AddError(system, "وارد کردن عنوان گروه الزامی است".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 72);
				}
				if (TafsilGroupGuid == Guid.Empty)
				{
					ret.AddError(system, "لطفا گروه تفصیلی را انتخاب نمایید".GetTranslate(), "TafsilGroupGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 73);
				}
				if (await GetTafsilGroupAsync(system, token, tranName) == null)
				{
					if (autoFixValidationErrorsCount > 0 && system != EnSystem.UserInterface)
					{
						Guid tafsilGroupGuid = TafsilGroupGuid;
						TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.PersonsAndCompany;
						ret.AddInformation("گروه تفصیلی در دیتابیس موجود نیست".GetTranslate() + "\r\n" + $"CurrentTafsilGroupGuid:{tafsilGroupGuid}\r\n" + $"UpdatedGroupGuid:{TafsilGroupGuid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 83);
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						CusGroupBusiness cusGroupBusiness = this;
						short num = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await cusGroupBusiness.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 86);
						return ret;
					}
					ret.AddError(system, "گروه تفصیلی معتبر نیست".GetTranslate() + "\r\n" + $"{TafsilGroupGuid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 91);
				}
				CusGroupBusiness cusGroupBusiness2 = await GetAsync(ContextGuid, Name, token);
				if (cusGroupBusiness2 != null && cusGroupBusiness2.Guid != Guid)
				{
					if (autoFixValidationErrorsCount > 0)
					{
						Name = $"{Name}_{autoFixValidationErrorsCount}";
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						CusGroupBusiness cusGroupBusiness3 = this;
						short num = (short)(autoFixValidationErrorsCount - 1);
						autoFixValidationErrorsCount = num;
						returnedSaveFuncInfo.AddReturnedValue(await cusGroupBusiness3.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 101);
						return ret;
					}
					ret.AddError(system, "گروه".GetTranslate() + "  " + Name + "تکراری می باشد".GetTranslate(), "dup", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 104);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 109);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 110);
			}
			finally
			{
				if (ret.HasError && system != EnSystem.UserInterface)
				{
					WebErrorLog.Instance.StartLog("CusGroup Validation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 115);
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool send2Server, short autoFixValidationErrorsCount, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
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
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CusGroupBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 129);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 130), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 130);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, autoFixValidationErrorsCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 132);
						if (system != EnSystem.UserInterface)
						{
							ret.throwExceptionIfError();
						}
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomerGroup.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 135);
							if (system == EnSystem.UserInterface && !ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await CusBusiness.UpdateSpecialAsync(ContextGuid, Guid, Special, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 137);
							}
							goto end_IL_01b4;
						}
						result = ret;
					}
					goto end_IL_00e6;
					end_IL_01b4:;
				}
				goto end_IL_00d2;
				end_IL_00e6:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"system:{system}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 141);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 142);
				goto end_IL_00d2;
			}
			return result;
			end_IL_00d2:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 147), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 147);
			}
			if (!ret.HasError && send2Server)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 150);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebCusGroup webEntity = await CusGroupMapper.Instance.MapAsync(ContextGuid, this, new WebCusGroup());
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.CustomerGroup;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره گروه اشخاص".GetTranslate() + " " + $" ({Name}) - {Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 187);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 204), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 204);
					transactionHasBeenCreated = true;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomerGroup.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 208);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 212);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 213);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 219), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 219);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<CusGroupBusiness> lst, short autoFixValidationErrorsCount, bool send2Server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (CusGroupBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 235);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, send2Server, autoFixValidationErrorsCount, priority, awaitOnSemaphore: true, await4Send: false, tryCount, tranName, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 236);
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
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 248);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 249);
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

	public static async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<CusGroupBusiness>>> GetAllAsync(string contextGuid, EnSystem system, string search, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<IEnumerable<CusGroupBusiness>> ret = new ReturnedSaveFuncInfoWithValue<IEnumerable<CusGroupBusiness>>();
			int line = 0;
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				line = 1;
				ReturnedSaveFuncInfoWithValue<IEnumerable<CusGroupBusiness>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await GetAllAsync(contextGuid, system, status, isLoadWithGuidEmpty: true, token, tranName);
				line = 2;
				List<string> list = search.SplitString()?.ToList();
				line = 3;
				if (list != null && list.Count > 0)
				{
					line = 4;
					foreach (string item in list)
					{
						line = 5;
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							line = 6;
							ret.value = ret.value.Where((CusGroupBusiness x) => x.Name.Contains(item.FixString()));
						}
					}
				}
			}
			catch (OperationCanceledException)
			{
				ret = null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 297);
				ret.AddReturnedValue(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 298);
			}
			return ret;
		}
	}

	public static async Task<IEnumerable<CusGroupBusiness>> GetAllAsync(string contextGuid, EnSystem system, bool? status = true, bool isLoadWithGuidEmpty = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs"))
		{
			int line = 0;
			try
			{
				if (string.IsNullOrEmpty(contextGuid))
				{
					return null;
				}
				IEnumerable<CusGroupBusiness> ret = new List<CusGroupBusiness>();
				line = 1;
				if (isLoadWithGuidEmpty)
				{
					((List<CusGroupBusiness>)ret).Add(new CusGroupBusiness(contextGuid)
					{
						Name = "[" + "[کلیه گروه ها]".GetTranslate() + "]",
						Description = "[" + "[تمام گروه ها]".GetTranslate() + "]",
						Status = true
					});
				}
				line = 2;
				List<CusGroupBusiness> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.GetAllAsync(token, tranName))?.OrderBy((CusGroupBusiness p) => p.Name)?.ToList();
				line = 3;
				if (list != null && list.Any())
				{
					((List<CusGroupBusiness>)ret).AddRange(list);
				}
				line = 4;
				if (status.HasValue)
				{
					ret = ret?.Where((CusGroupBusiness p) => p.Status == status.Value);
				}
				line = 5;
				return await ApplyAccessAsync(contextGuid, system, ret, token, tranName);
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"1_ #Line_{line}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 333);
				return new List<CusGroupBusiness>();
			}
		}
	}

	public static Task<CusGroupBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.GetAsync(guid, token, tranName);
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.DbCountAsync(status, token, tranName);
	}

	public static CusGroupBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.Get(guid, token, tranName);
		}
		return null;
	}

	public static Task<CusGroupBusiness> GetAsync(string contextGuid, string Name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.GetAsync(Name, token, tranName);
	}

	public static Task<int> CountInGroupActiveAsync(string contextGuid, Guid selectedGroupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(selectedGroupGuid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.CountInGroupActiveAsync(selectedGroupGuid, token, tranName);
		}
		return Task.FromResult(0);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ICustomerGroup;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 375), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 375);
					transactionHasBeenCreated = true;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ICustomerGroup.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 379);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 382);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 385);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 386);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 392), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 392);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask = 5)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<CusGroupBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<CusGroupBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName, short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.CustomerGroup;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال گروه اشخاص".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Customers;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 423);
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				int num;
				if (cache == null)
				{
					num = 1;
				}
				else
				{
					_ = cache.BufferCancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 428);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 434);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ICustomerGroup is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 451);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("گروه اشخاص سینک هستند. تعداد گروه اشخاص سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 456);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 461);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 473);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 479);
						}
						CusGroupBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "گروه اشخاص جاری".GetTranslate() + ":" + obj?.Name;
							uiUpdater.Count = current;
						}
						ServerStatusForClient serverStatus = obj.ServerStatus;
						if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: CusGroup \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 491);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 499);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 506);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 512);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 515);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 518);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 521);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 522);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.".GetTranslate();
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "CusGroupBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 554);
				}
				List<WebCusGroup> list = new List<WebCusGroup>();
				List<WebCusGroup> list2 = list;
				list2.Add(await CusGroupMapper.Instance.MapAsync(ContextGuid, this, new WebCusGroup(), "", token));
				List<Packet> paks = WebCusGroup.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 564);
				}
				using CusGroupWaiter waiter = new CusGroupWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("CusGroupBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 570);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 573);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 576);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 579);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 580);
			}
			return ret;
		}
	}

	private static async Task<IEnumerable<CusGroupBusiness>> ApplyAccessAsync(string contextGuid, EnSystem system, IEnumerable<CusGroupBusiness> ret, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return ret;
			}
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashierBusiness == null)
			{
				return ret;
			}
			UserAccBusiness user = await (cashierBusiness?.GetUserAsync(token, tranName));
			if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
			{
				ret = ret?.Where((CusGroupBusiness p) => user.UserAccess.TafsilGroupJustListGuid.Contains(p.TafsilGroupGuid) || p.Guid == Guid.Empty);
			}
			if (!user.UserAccess.Customers.Customer_DastrasiBeAshkhaseKhas)
			{
				ret = ret?.Where((CusGroupBusiness p) => !p.Special);
			}
			ret = ret?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 604);
			throw;
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, IEnumerable<WebCusGroup> items, CancellationToken token)
	{
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "CusGroupBusiness/SaveRangeForHesabyarAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 616);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 617), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 617);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerGroup.SaveRangeAsync(items, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 619);
							goto end_IL_0104;
						}
						result = ret;
					}
					goto end_IL_006d;
					end_IL_0104:;
				}
				goto end_IL_005c;
				end_IL_006d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 621);
				goto end_IL_005c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 624);
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 625);
				goto end_IL_005c;
			}
			return result;
			end_IL_005c:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 629), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 629);
			if (ret.HasError && items.Count() == 1)
			{
				ret = await (await CusGroupMapper.Instance.MapAsync(contextGuid, items.Single())).SaveAsync(EnSystem.SignalR, send2Server: false, 10, Priority.Low, awaitOnSemaphore: true, await4Send: false, 10);
			}
			else if (ret.HasError && items.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<WebCusGroup> list2 = new List<WebCusGroup>();
				WebCusGroup[] array = items.ToArray();
				for (int i = 0; i < items.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token));
				list.Add(SaveRangeForHesabyarAsync(contextGuid, items.Except(list2), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusGroupBusiness.cs", 644);
			}
		}
		return ret;
	}
}
