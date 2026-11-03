using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Business.Products.ProductUnit;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Product;
using Serilog;

namespace EntityCache.Business;

public class PrdVahedBusiness : BaseBusiness<PrdVahedBusiness>, IPrdVahed, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<PrdVahedBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _description;

	private string _name;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

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

	public bool Status { get; set; }

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

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public bool DecimalSales { get; set; }

	public Guid? TaxUnitGuid { get; set; }

	public PrdVahedBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, short autoFixValidationCount, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.None)
			{
				return ret;
			}
			PrdVahedBusiness prdVahedBusiness = await GetByNameAsync(ContextGuid, Name, token, tranName);
			if (prdVahedBusiness != null && prdVahedBusiness.Guid != Guid)
			{
				if (autoFixValidationCount > 0 && system != EnSystem.UserInterface)
				{
					string name = Name;
					Name += "_1";
					ret.AddInformation("واحد".GetTranslate() + " (" + name + ")=>(" + Name + ")" + "تکراری می باشد".GetTranslate() + $" Guid({Guid})", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 62);
					ret.AddInformation($"PrdVahedBusiness=> autoFix({autoFixValidationCount})=>set {name}=>{Name}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 63);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					PrdVahedBusiness prdVahedBusiness2 = this;
					short num = checked((short)(autoFixValidationCount - 1));
					autoFixValidationCount = num;
					returnedSaveFuncInfo.AddReturnedValue(await prdVahedBusiness2.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 64);
					return ret;
				}
				ret.AddError(system, "گروه".GetTranslate() + "  " + Name + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 67);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 72);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 73);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, short autoFixValidationCount, bool awaitOnSemaphore = true, bool send2Server = false, Priority priority = Priority.Low, bool await4Send = false, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdVahedBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 87);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 88), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 88);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, autoFixValidationCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 90);
						if (system != EnSystem.UserInterface || !ret.HasError)
						{
							ret.throwExceptionIfError();
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProductVahed.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 93);
							goto end_IL_01ac;
						}
						result = ret;
					}
					goto end_IL_00e2;
					end_IL_01ac:;
				}
				goto end_IL_00ce;
				end_IL_00e2:;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(ContextGuid)?.IsDisposed ?? true)
				{
					Log.Warning(ex, "cache is null or disposed!");
					throw;
				}
				WebErrorLog.Instance.StartLog(ex, "cache is not null or disposed.", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 102);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 103);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 108), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 108);
			}
			if (!ret.HasError && send2Server)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 112);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority), token);
				}
				if (!token.IsCancellationRequested)
				{
					SaveLogAsync(system, ret);
				}
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
				WebPrdVahed webEntity = await PrdVahedMapper.Instance.MapAsync(ContextGuid, this, new WebPrdVahed());
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
				cashiersLogBusiness.SanadType = EnLog.ProductVahed;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره واحد کالا".GetTranslate() + " " + $" {EnLog.ProductVahed.GetDisplay().GetTranslate()}({Name})-{Guid}\r\n" + " " + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 150);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 161), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 161);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProductVahed.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 163);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 167);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 168);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 173), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 173);
			}
		}
		return ret;
	}

	public static async Task<int> CountInProductsAsync(string contextGuid, Guid vhdGuid, bool? status = true, CancellationToken token = default(CancellationToken))
	{
		return ((await ClsCache.GetCache(contextGuid).CacheRepository.IProductCompact.GetAllAsync(status, token, ""))?.Where((ProductCompactBusiness q) => q.GuidVahed == vhdGuid)).Count();
	}

	public static async Task<bool> CheckGuidAsync(string contextGuid, Guid Guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.GetAsync(Guid, token, tranName) != null;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<PrdVahedBusiness> lst, short autoFixValidationCount, bool send2Server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			int current = 0;
			foreach (PrdVahedBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 195);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, autoFixValidationCount, awaitOnSemaphore: true, send2Server, priority, await4Send: false, token, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 196);
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
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 208);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 209);
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

	public static async Task<IEnumerable<PrdVahedBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.GetAllAsync(status, token, tranName))?.OrderBy((PrdVahedBusiness x) => x.Name);
	}

	public static Task<PrdVahedBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (guid == Guid.Empty)
			{
				return Task.FromResult<PrdVahedBusiness>(null);
			}
			return ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.GetAsync(guid, token, tranName);
		}
		catch (ThreadAbortException ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 234);
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 239);
			return Task.FromResult<PrdVahedBusiness>(null);
		}
	}

	public static Task<int> DbCountAsync(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.DbCountAsync(status, token, tranName);
	}

	public static PrdVahedBusiness Get(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (guid == Guid.Empty)
			{
				return null;
			}
			return ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.Get(guid, token, tranName);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 255);
			throw;
		}
	}

	public static Task<PrdVahedBusiness> GetByNameAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.GetAsync(name, token, tranName);
	}

	public static async Task GetFromWebService(string contextGuid, EnSystem system, Guid token, HttpClient httpClient = null, CancellationToken cantoken = default(CancellationToken))
	{
		using (new JaegerMonitor("GetFromWebService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs"))
		{
			_ = 3;
			try
			{
				bool send2Server = false;
				WebServiceRequest<PrdVahedReadEventArgs> item = new WebServiceRequest<PrdVahedReadEventArgs>
				{
					Token = token
				};
				List<WebPrdVahed> prdVaheds = new List<WebPrdVahed>();
				using (HttpClient client = httpClient ?? new HttpClient())
				{
					client.BaseAddress = new Uri("https://webservice.novinp.ir/Webservice/Product/GetPrdVahed");
					StringContent content = new StringContent(Json.ToStringJson(item), Encoding.UTF8, "application/json");
					string text = await (await client.PostAsync(client.BaseAddress, content)).Content.ReadAsStringAsync();
					if (text != null)
					{
						prdVaheds = text.FromJson<List<WebPrdVahed>>("GetFromWebService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 275);
					}
				}
				if (prdVaheds != null && prdVaheds.Any())
				{
					await SaveRangeAsync(system, await PrdVahedMapper.Instance.MapListAsync(contextGuid, prdVaheds, null, "", cantoken), 10, send2Server, Priority.Low, 10, "", cantoken);
				}
				WebErrorLog.Instance.Trace($"Number of prdVahed taken from web Service:{prdVaheds?.Count}", "", "GetFromWebService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 282);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFromWebService", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 286);
				throw;
			}
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IProductVahed;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 299), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 299);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IProductVahed.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 301);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 303);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 306);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 307);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 312), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 312);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static Task<PrdVahedBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<PrdVahedBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		checked
		{
			using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs"))
			{
				int line = 0;
				ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
				ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.ProductVahed;
				try
				{
					if (uiUpdater != null)
					{
						uiUpdater.ShortMessage = "در حال ارسال واحد کالاها";
						uiUpdater.Status = SyncStatus.Syncing;
						uiUpdater.FinalStatus = ret;
						uiUpdater.Group = SyncPart.Products;
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 337);
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
						return ret.AddError(EnSystem.UserInterface, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 339);
					}
					line = 5;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 341);
					}
					if (ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed == null)
					{
						throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IProductVahed is null");
					}
					List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IProductVahed.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
					if (uiUpdater != null)
					{
						uiUpdater.TotalCount = list?.Count ?? 0;
					}
					line = 6;
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 348);
					}
					if (list == null || !list.Any())
					{
						return ret.AddInformation("واحد های کالا سینک هستند. تعداد واحد سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 349);
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 350);
					}
					List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
					int current = 0;
					decimal total = list?.Count ?? 0;
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (list != null && list.Any())
					{
						foreach (Guid item in list)
						{
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 357);
							}
							current++;
							Math.Round((decimal)(100 * current) / total, 2);
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 360);
							}
							PrdVahedBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
							if (uiUpdater != null)
							{
								uiUpdater.ShortMessage = "واحد کالای جاری".GetTranslate() + ":" + obj?.Name;
								uiUpdater.Count = current;
							}
							if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
							{
								ret.AddInformation("current status on database is delivered!\r\nEntityType: PrdVahed \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 370);
								continue;
							}
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 377);
							}
							tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
							if (tasks.Count >= maxParalelTask)
							{
								Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 382);
								tasks.Remove(t);
							}
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 386);
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 388);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 391);
					ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 392);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "PrdVahedBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 423);
				}
				List<WebPrdVahed> list = new List<WebPrdVahed>();
				List<WebPrdVahed> list2 = list;
				list2.Add(await PrdVahedMapper.Instance.MapAsync(ContextGuid, this, new WebPrdVahed(), "", token));
				List<Packet> paks = WebPrdVahed.GenerateSaveRequest(list);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 425);
				}
				using PrdVahedWaiter waiter = new PrdVahedWaiter(ClsCache.GetCache(ContextGuid).Context, Guid, token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, Priority.High), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("prdVahedBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 429);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 431);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 433);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 436);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 437);
			}
			return ret;
		}
	}

	public static async Task<IEnumerable<ProductVahedModel>> GetAllModelAsync(string contextGuid, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<PrdVahedBusiness> list = await GetAllAsync(contextGuid, status, token, tranName);
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			new List<ProductVahedModel>();
			TaxUnits setting = await LocalSettingBusiness.GetTaxUnitsAsync(contextGuid, token);
			return list.Select((PrdVahedBusiness q) => new ProductVahedModel(setting, q.TaxUnitGuid)
			{
				TaxUnitGuid = q.TaxUnitGuid,
				DecimalSales = q.DecimalSales,
				Description = q.Description,
				Guid = q.Guid,
				Modified = q.Modified,
				Name = q.Name,
				ServerStatus = q.ServerStatus,
				Status = q.Status
			})?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductUnit\\PrdVahedBusiness.cs", 464);
			throw;
		}
	}
}
