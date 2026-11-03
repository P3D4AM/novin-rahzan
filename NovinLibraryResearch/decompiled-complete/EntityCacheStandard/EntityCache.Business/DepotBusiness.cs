using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;

namespace EntityCache.Business;

public class DepotBusiness : BaseBusiness<DepotBusiness>, IDepot, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<DepotBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _name;

	private string _description;

	public Guid Guid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	[Obsolete("Use NegativeAmount", true)]
	public bool Manfi { get; set; }

	public ReturnedState NegativeAmount { get; set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = value?.FixString() ?? "";
		}
	}

	public bool Status { get; set; }

	public override string ContextGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public DepotBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	internal async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!Status)
			{
				IEnumerable<PrdAnbarRelationBusiness> enumerable = await PrdAnbarRelationBusiness.GetAllAsync(ContextGuid, system, null, Guid, token);
				if (enumerable != null && enumerable.Any((PrdAnbarRelationBusiness p) => p.Amount != 0m))
				{
					ret.AddError(system, "انبار دارای کالای می باشد در چنین حالتی ابتدا موجودی انبار را تعیین وضعیت نمایید".GetTranslate(), "ListKala", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 56);
					int i = 0;
					foreach (PrdAnbarRelationBusiness prd in enumerable)
					{
						i = checked(i + 1);
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						string translate = "کالا".GetTranslate();
						ProductCompactBusiness productCompactBusiness = await ProductCompactBusiness.GetAsync(ContextGuid, prd.PrdGuid, system, null, "", token, true);
						returnedSaveFuncInfo.AddInformation(translate + "  " + productCompactBusiness?.Name + " با تعداد".GetTranslate() + $" {prd.Amount}" + " در این انبار موجود است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 61);
						if (i == 5)
						{
							ret.AddInformation("و غیره ...".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 64);
							break;
						}
					}
				}
			}
			if (string.IsNullOrEmpty(Name?.Trim()))
			{
				ret.AddError(system, "وارد کردن نام انبار الزامی است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 70);
			}
			DepotBusiness depotBusiness = (await GetAllAsync(ContextGuid, system, true, "", token))?.FirstOrDefault((DepotBusiness p) => p.Name == Name && p.Status && p.Guid != Guid);
			if (depotBusiness != null && Status)
			{
				if (system == EnSystem.Transfer)
				{
					depotBusiness.Status = false;
					depotBusiness.Name = $"{depotBusiness.Name}_{new Random().Next(1, 10)}";
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await depotBusiness.SaveAsync(system, token, sendToServer: true, Priority.Low, await4Send: false, awaitOnSemaphore: false, 10, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 79);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 80);
				}
				else
				{
					ret.AddError(system, "انباری با همین نام".GetTranslate() + " (" + Name + ") " + " در حال استفاده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 82);
				}
			}
		}
		catch (Exception ex)
		{
			if (ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true)
			{
				Log.Warning(ex, "");
				throw;
			}
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 92);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 93);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool sendToServer, Priority priority = Priority.Low, bool await4Send = false, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "DepotBusiness/SaveAsync", 60000, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (dis.Token.IsCancellationRequested)
				{
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 107);
				}
				else
				{
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 108), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 108);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 110);
					if (!ret.HasError)
					{
						goto IL_0397;
					}
					if (system != EnSystem.UserInterface)
					{
						ret.throwExceptionIfError();
						goto IL_0397;
					}
					result = ret;
				}
				goto end_IL_01ac;
				IL_0397:
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IDepot.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 116);
				goto end_IL_00ce;
				end_IL_01ac:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 118);
				goto end_IL_00ce;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 121);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 122);
				goto end_IL_00ce;
			}
			return result;
			end_IL_00ce:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 127), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 127);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 130);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepot.DbCountAsync(status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 146), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 146);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IDepot.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 148);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 152);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 153);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 158), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 158);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 171), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 171);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IDepot.KeepRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 173);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 177);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 178);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 183), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 183);
			}
		}
		return ret;
	}

	public static Task<DepotBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepot.GetAsync(guid, token, tranName);
	}

	public static DepotBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepot.Get(guid, token, tranName);
	}

	public static Task<DepotBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IDepot.GetByNameAsync(name, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<DepotBusiness> Lst, bool send2Server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(Lst?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (DepotBusiness item in Lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 204);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, send2Server, priority, await4Send: false, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 205);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(Lst.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 217);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 218);
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
					percentCompleted(new PercenteCompletedEventArgs(Lst?.Count() ?? 0, Lst?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public static async Task<IEnumerable<DepotBusiness>> GetAllAsync(string contextGuid, EnSystem system, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return await ApplyAccessAsync(contextGuid, await ClsCache.GetCache(contextGuid).CacheRepository.IDepot.GetAllAsync(status, token, tranName), system, token);
	}

	public static async Task<Guid> GetDefaultDepotGuidAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await CashierBusiness.GetCurrentCashierAsync(contextGuid, token))?.DepotGuid ?? Guid.Empty;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IDepot;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 248), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 248);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IDepot.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 250);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 252);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 255);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 256);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 261), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 261);
			}
		}
		return ret;
	}

	public static Task<DepotBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IDepot.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<DepotBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		checked
		{
			using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs"))
			{
				int line = 0;
				ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
				ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Depot;
				try
				{
					if (uiUpdater != null)
					{
						uiUpdater.ShortMessage = "در حال ارسال انبارها".GetTranslate();
						uiUpdater.Status = SyncStatus.Syncing;
						uiUpdater.FinalStatus = ret;
						uiUpdater.Group = SyncPart.Products;
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 284);
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
						return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 286);
					}
					line = 5;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 288);
					}
					if (ClsCache.GetCache(contextGuid).CacheRepository.IDepot == null)
					{
						throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IDepot is null");
					}
					List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IDepot.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
					if (uiUpdater != null)
					{
						uiUpdater.TotalCount = list?.Count ?? 0;
					}
					line = 6;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 295);
					}
					if (list == null || !list.Any())
					{
						return ret.AddInformation("انبارها سینک هستند تعداد انبار سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 296);
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 297);
					}
					List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
					int current = 0;
					decimal total = list?.Count ?? 0;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (list != null && list.Any())
					{
						foreach (Guid item in list)
						{
							current++;
							Math.Round((decimal)(100 * current) / total, 2);
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 306);
							}
							DepotBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
							if (uiUpdater != null)
							{
								uiUpdater.ShortMessage = "انبار جاری".GetTranslate() + ":" + obj?.Name;
								uiUpdater.Count = current;
							}
							if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
							{
								ret.AddInformation("current status on database is delivered!\r\nEntityType: Depot \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 316);
								continue;
							}
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 322);
							}
							tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
							if (tasks.Count >= maxParalelTask)
							{
								Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 327);
								tasks.Remove(t);
							}
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 331);
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 333);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 336);
					ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 337);
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
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "DepotBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 367);
				}
				List<WebDepot> list = new List<WebDepot>();
				List<WebDepot> list2 = list;
				list2.Add(await DepotMapper.Instance.MapAsync(ContextGuid, this, new WebDepot(), "", token));
				List<Packet> paks = WebDepot.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 369);
				}
				using DepotWaiter waiter = new DepotWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("DepotBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 373);
				if (!ret.HasError)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 375);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 377);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 380);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 381);
			}
			return ret;
		}
	}

	private static async Task<IEnumerable<DepotBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<DepotBusiness> items, EnSystem system, CancellationToken token)
	{
		try
		{
			if (system == EnSystem.SignalR || system == EnSystem.None || system == EnSystem.Transfer)
			{
				return items;
			}
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (currentCashier == null)
			{
				return items;
			}
			if (!currentCashier.AllDepotsAccess)
			{
				items = items?.Where(delegate(DepotBusiness p)
				{
					Guid guid = p.Guid;
					Guid? depotGuid = currentCashier.DepotGuid;
					return guid == depotGuid;
				});
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			items = items?.ToList();
			if (token.IsCancellationRequested)
			{
				return null;
			}
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\DepotBusiness.cs", 403);
			throw;
		}
	}

	private static async Task<DepotBusiness> ApplyAccessAsync(string contextGuid, DepotBusiness item, EnSystem systen, CancellationToken token)
	{
		return (await ApplyAccessAsync(contextGuid, new List<DepotBusiness> { item }, systen, token))?.SingleOrDefault();
	}
}
