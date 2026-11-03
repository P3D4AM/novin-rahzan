using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;

namespace EntityCache.Business.Accounting;

public class MoeinBusiness : BaseBusiness<MoeinBusiness>, IMoein, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<MoeinBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private DateTime dateM = DateTime.Now;

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private Guid _kolGuid;

	private Guid _guid;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public Guid Guid
	{
		get
		{
			return _guid;
		}
		set
		{
			try
			{
				if (_guid == Guid.Empty || _guid == value)
				{
					_guid = value;
					return;
				}
				throw new InvalidOperationException($"change MoeinGuid from ({_guid}) to ({value}) is not valid!");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Guid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 46);
			}
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public Guid KolGuid
	{
		get
		{
			return _kolGuid;
		}
		set
		{
			try
			{
				if (_kolGuid != Guid.Empty && _kolGuid != value)
				{
					throw new InvalidOperationException($"kolGuid change from ({_kolGuid}) to ({value})");
				}
				_kolGuid = value;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KolGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 61);
			}
		}
	}

	public bool Status { get; set; }

	public string Description { get; set; }

	public ENMahiat Mahiat { get; set; }

	public bool System { get; set; }

	public bool Peygiri { get; set; }

	public bool Tedadi { get; set; }

	public decimal Account { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public long Code { get; set; }

	public bool IsChecked { get; set; }

	public byte[] ServerStatusImage
	{
		get
		{
			try
			{
				ServerStatusForClient serverStatus = ServerStatus;
				if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
				{
					return ImageResourceManager.ServerStatusDelivered;
				}
				if (ServerStatus == ServerStatusForClient.DeliveryError)
				{
					return ImageResourceManager.ServerStatusDeliveryFailed;
				}
				if (ServerStatus == ServerStatusForClient.Sent)
				{
					return ImageResourceManager.ServerStatusSent;
				}
				if (ServerStatus == ServerStatusForClient.SendError)
				{
					return ImageResourceManager.ServerStatusSentError;
				}
				return ImageResourceManager.ServerStatusNone;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ServerStatusImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 95);
			}
			return null;
		}
	}

	public DateTime DateM
	{
		get
		{
			return dateM;
		}
		set
		{
			if (value < DateTime.Now.AddYears(100) && value > DateTime.Now.AddYears(-100))
			{
				dateM = value;
			}
			else
			{
				dateM = DateTime.Now;
			}
		}
	}

	public byte[]? GardeshHesab { get; set; }

	public override string ContextGuid { get; set; }

	public MoeinBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.MoeinBusiness;

	public event Func<object, EventArgs, Task> OnItemSaved;

	public MoeinBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<int> GetCodeKolAsync(CancellationToken token = default(CancellationToken))
	{
		return (await GetKolAsync(token))?.Code ?? 0;
	}

	public KolBusiness GetKol(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return KolBusiness.Get(ContextGuid, KolGuid, token, tranName);
	}

	public async Task<KolBusiness> GetKolAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await KolBusiness.GetAsync(ContextGuid, KolGuid, token, tranName);
	}

	public MoeinBusiness(Guid guid, string name, Guid kolGuid, long code, bool st, string description, ENMahiat mahiat, bool system, bool peygiri, bool tedadi, string contextGuid)
	{
		try
		{
			Guid = guid;
			Name = name;
			KolGuid = kolGuid;
			Code = code;
			Status = st;
			Description = description;
			Mahiat = mahiat;
			System = system;
			Peygiri = peygiri;
			Tedadi = tedadi;
			ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 130);
		}
	}

	public static Task<MoeinBusiness> GetAsync(string contextGuid, Guid moeinGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetAsync(moeinGuid, token, tranName);
	}

	public static Task<MoeinBusiness> GetAsync(string contextGuid, long moeinCode, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetAsync(moeinCode, token, tranName);
	}

	public static MoeinBusiness Get(string contextGuid, Guid moeinGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(moeinGuid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IMoein.Get(moeinGuid, token, tranName);
		}
		return null;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool updateAccount, bool sendToServer, Priority priority = Priority.Low, List<Guid> listTafsillevel = null, short autoFixValidationErrorsCount = 0, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		Stopwatch.StartNew();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommitTran = string.IsNullOrEmpty(tranName);
		if (autoCommitTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		int line = 0;
		try
		{
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				line = 1;
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 153);
					}
					else
					{
						line = 2;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoCommitTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 156), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 156);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName, autoFixValidationErrorsCount), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 158);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (autoFixValidationErrorsCount > 0)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await FillKolIfIsInvalidAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 161);
							}
							line = 5;
							if (listTafsillevel != null && listTafsillevel.Any())
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await MoeinTafsilGroupRelationsBusiness.SaveMoeinRelationsAsync(ContextGuid, Guid, listTafsillevel, awaitOnSemaphore: false, tryCount, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 164);
							}
							line = 6;
							if (!ret.HasError)
							{
								line = 7;
								Modified = DateTime.Now;
								line = 8;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IMoein.SaveAsync(this, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 170);
								goto end_IL_01e3;
							}
							result = ret;
						}
					}
					goto end_IL_00f7;
					end_IL_01e3:;
				}
				goto end_IL_00e3;
				end_IL_00f7:;
			}
			catch (Exception ex)
			{
				if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.IMoein == null)
				{
					Log.Warning(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.IMoein is null{ClsCache.GetCache(ContextGuid)?.CacheRepository?.IMoein == null}");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}//ClsCache.GetCache(contextGuid).CacheRepository.IMoein is null{ClsCache.GetCache(ContextGuid)?.CacheRepository?.IMoein == null}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 177);
				}
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 178);
				goto end_IL_00e3;
			}
			return result;
			end_IL_00e3:;
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 183), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 183);
			}
			if (!ret.HasError)
			{
				ChangeStatus = EntityChangeSt.Edited;
			}
			if (!ret.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 188);
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
				WebMoein webEntity = await MoeinMapper.Instance.MapAsync(ContextGuid, this, new WebMoein());
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
				cashiersLogBusiness.SanadType = EnLog.Moein;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره حساب معین".GetTranslate() + "(" + $"{webEntity.Code}" + ")" + (Name ?? "").GetTranslate() + $"-{Guid}" + "\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 225);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveNewAsync(EnSystem system, bool updateAccount, bool sendToServer, Priority priority = Priority.Low, List<MoeinTafsilGroupRelationsBusiness> listTafsillevel = null, short autoFixValidationErrorsCount = 0, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		Stopwatch.StartNew();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommitTran = string.IsNullOrEmpty(tranName);
		if (autoCommitTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 5;
			ReturnedSaveFuncInfo result;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 239);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoCommitTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 241), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 241);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName, autoFixValidationErrorsCount), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 243);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (autoFixValidationErrorsCount > 0)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await FillKolIfIsInvalidAsync(system, token, tranName), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 246);
							}
							if ((listTafsillevel?.Any() ?? false) || system == EnSystem.UserInterface)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await MoeinTafsilGroupRelationsBusiness.SaveMoeinRelationsAsync(ContextGuid, Guid, listTafsillevel, awaitOnSemaphore: false, tryCount, tranName), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 248);
							}
							if (!ret.HasError)
							{
								Modified = DateTime.Now;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IMoein.SaveAsync(this, updateAccount, tryCount, token, tranName), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 251);
								goto end_IL_01d5;
							}
							result = ret;
						}
					}
					goto end_IL_00f0;
					end_IL_01d5:;
				}
				goto end_IL_00dc;
				end_IL_00f0:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 255);
				ret.AddReturnedValue(ex, "", "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 256);
				goto end_IL_00dc;
			}
			return result;
			end_IL_00dc:;
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 261), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 261);
			}
			if (!ret.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 264);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!ret.HasError && system == EnSystem.UserInterface && EventManager != null)
			{
				PacketParser.Utilities.RaiseEvent<EventArgs>(EventManager.OnItemSaved, this, EventArgs.Empty, token, 60, "SaveNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs");
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	public static async Task<IEnumerable<MoeinBusiness>> GetAllAsync(string contextGuid, string search = "", Guid? kolGuid = null, Guid? tafsilLevelGuid = null, CancellationToken? token = null, bool? status = true, string tranName = "")
	{
		return (await ApplyAccessAsync(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetAllAsync(search, kolGuid, tafsilLevelGuid, token.GetValueOrDefault(), status, tranName)))?.OrderBy((MoeinBusiness p) => p.Code)?.ToList();
	}

	public static async Task<ReturnedSaveFuncInfo> SaveMoeinRelationTafsilLevelAsync(string contextGuid, Guid moeinGuid, List<TafsilGroupBusiness> lst, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				CancellationToken token = default(CancellationToken);
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "MoineBusiness/SaveMoeinRelationTafsilLevelAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						if (autoTran)
						{
							tranName = Guid.NewGuid().ToString();
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 290), ReturnedState.None, "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 290);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.SaveMoeinRelationTafsilLevelAsync(moeinGuid, lst, tryCount, token, tranName), ReturnedState.None, "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 292);
						goto end_IL_00fa;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 286);
					goto end_IL_0055;
					end_IL_00fa:;
				}
				goto end_IL_0044;
				end_IL_0055:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 296);
				ret.AddReturnedValue(ex, "", "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 297);
				goto end_IL_0044;
			}
			return result;
			end_IL_0044:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 302), ReturnedState.None, "SaveMoeinRelationTafsilLevelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 302);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<MoeinBusiness> lstHesabsListMoein, bool updateAccount, short autoFixValidationErrorsCount, bool send2Server, Priority priority = Priority.Low, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CancellationTokenSource invokeToken = new CancellationTokenSource();
			int current = 0;
			if (!(lstHesabsListMoein?.Any() ?? false))
			{
				return ret;
			}
			foreach (MoeinBusiness moein in lstHesabsListMoein)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 317);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await moein.SaveAsync(system, updateAccount, send2Server, priority, null, autoFixValidationErrorsCount, awaitOnSemaphore: true, await4Send: false, tryCount, tranName, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 318);
				if (percentCompleted != null)
				{
					invokeToken?.Cancel();
					invokeToken = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken.Token;
					percentCompleted(new PercenteCompletedEventArgs(lstHesabsListMoein.Count(), current, moein.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 329);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 330);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, IEnumerable<WebMoein> lstHesabsListMoein, bool updateAccount, bool sendToServer, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lstHesabsListMoein?.Any() ?? false))
			{
				return ret;
			}
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				return returnedSaveFuncInfo.AddReturnedValue(await SaveRangeForHesabyarAsync(contextGuid, lstHesabsListMoein, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 341);
			}
			foreach (WebMoein item in lstHesabsListMoein)
			{
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 344);
				}
				ret.AddReturnedValue(await SaveAsync(contextGuid, system, item, updateAccount, sendToServer, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 347);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 352);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 353);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, EnSystem system, WebMoein moein, bool updateAccount, bool sendToServer, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 5;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 363), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 363);
				if (token.IsCancellationRequested)
				{
					ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 364);
				}
				MoeinBusiness obj = await MoeinMapper.Instance.MapAsync(contextGuid, moein, new MoeinBusiness(contextGuid), tranName, token);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(system, updateAccount, sendToServer, Priority.Low, null, 0, awaitOnSemaphore: true, await4Send: false, 10, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 366);
				if (ret.HasError || !(moein?.TafsilGroupRelationList?.Any()).GetValueOrDefault())
				{
					result = ret;
					goto IL_063c;
				}
				returnedSaveFuncInfo = ret;
				Guid? moeinGuid = moein.Guid;
				string tranName2 = tranName;
				CancellationToken token2 = token;
				returnedSaveFuncInfo.AddReturnedValue(await MoeinTafsilGroupRelationsBusiness.RemoveAsync(contextGuid, moeinGuid, null, awaitOnSemaphore: true, 10, token2, tranName2), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 369);
				if (ret.HasError)
				{
					result = ret;
					goto IL_063c;
				}
				List<MoeinTafsilGroupRelationsBusiness> list = await MoeinTafsilGroupRelationMapper.Instance.MapListAsync(contextGuid, moein.TafsilGroupRelationList, new List<MoeinTafsilGroupRelationsBusiness>(), tranName, token);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await MoeinTafsilGroupRelationsBusiness.SaveRangeAsync(list, 0, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 372);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 375);
			}
			catch (Exception exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 376);
			}
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 377), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 377);
		}
		return ret;
		IL_063c:
		return result;
	}

	private static async Task<ReturnedSaveFuncInfo> SaveRangeForHesabyarAsync(string contextGuid, IEnumerable<WebMoein> items, CancellationToken token)
	{
		string tranName = Guid.NewGuid().ToString();
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "MoeinBusiness/SaveRangeForHesabyarAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 387);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 388), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 388);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.SaveRangeAsync(items, token, tranName), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 390);
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
				ret.AddReturnedValue(exception, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 392);
				goto end_IL_005c;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 395);
				ret.AddReturnedValue(ex, "", "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 396);
				goto end_IL_005c;
			}
			return result;
			end_IL_005c:;
		}
		finally
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 400), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 400);
			if (ret.HasError && items.Count() == 1)
			{
				ret = await (await MoeinMapper.Instance.MapAsync(contextGuid, items.Single(), null, "", token)).SaveAsync(EnSystem.SignalR, updateAccount: true, sendToServer: false, Priority.Low, null, 10, awaitOnSemaphore: true, await4Send: false, 10);
			}
			else if (ret.HasError && items.Count() > 1 && !token.IsCancellationRequested)
			{
				ret = new ReturnedSaveFuncInfo();
				List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
				List<WebMoein> list2 = new List<WebMoein>();
				WebMoein[] array = items.ToArray();
				for (int i = 0; i < items.Count() / 2; i = checked(i + 1))
				{
					list2.Add(array[i]);
				}
				list.Add(SaveRangeForHesabyarAsync(contextGuid, list2, token));
				list.Add(SaveRangeForHesabyarAsync(contextGuid, items.Except(list2), token));
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeForHesabyarAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 415);
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> FillKolIfIsInvalidAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (await GetKolAsync(token, tranName) != null)
			{
				return ret;
			}
			ReturnedSaveFuncInfoWithValue<KolBusiness> returnedSaveFuncInfoWithValue = await KolBusiness.SaveAsnadDaryaftaniDefaultsync(ContextGuid, system);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "FillKolIfIsInvalidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 428);
			KolGuid = returnedSaveFuncInfoWithValue.value.Guid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillKolIfIsInvalidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 433);
			ret.AddReturnedValue(ex, "", "FillKolIfIsInvalidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 434);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", short autoFixValidationErrorsCount = 0)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			KolBusiness kolBusiness = await GetKolAsync(token, tranName);
			if (kolBusiness == null || kolBusiness.Guid != KolGuid)
			{
				ret.AddError(system, "حساب کل انتخاب نشده یا نامعتبر است".GetTranslate() + $" {KolGuid}".GetTranslate(), "KolGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 446);
			}
			line = 5;
			if (string.IsNullOrEmpty(Name))
			{
				ret.AddError(system, "نام معین پر نشده".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 449);
			}
			line = 10;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckValidationCodeAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 451);
			short moeinLen = (await LocalSettingBusiness.GetSettingAsync(ContextGuid)).Sanads.MoeinLen;
			if (system == EnSystem.UserInterface && moeinLen != Code.ToString().Length && system != EnSystem.Transfer)
			{
				ret.AddError(system, "کد حساب معین".GetTranslate() + "(" + (Name ?? "").GetTranslate() + " - " + $"{Code}" + ")" + " و طول آن".GetTranslate() + " " + $" {Code.ToString().Length}" + " " + "می باشد که با تنظیمات کدینگ".GetTranslate() + " " + $" {moeinLen}" + " " + "همخوانی ندارد".GetTranslate(), "Code", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 455);
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 459);
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}//IsDisposed:{base.IsDisposed}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 460);
		}
		finally
		{
			if (ret.HasError && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("MoeinCheckValidation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 465);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationCodeAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			KolBusiness kol = await GetKolAsync(token, tranName);
			if (Code <= 0)
			{
				ret.AddError(system, "کد".GetTranslate() + "(" + $"{Code}" + ")" + "حساب معین وارد شده نامعتبر میباشد.".GetTranslate() + "\r\n" + "نام حساب معین".GetTranslate() + ":" + (" " + Name).GetTranslate(), "Code", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 476);
			}
			if (Mahiat == ENMahiat.None)
			{
				ret.AddError(system, "ماهیت حساب انتخاب نشده است".GetTranslate() + "  " + Mahiat.GetDisplay().GetTranslate(), "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 479);
			}
			try
			{
				MoeinBusiness moeinBusiness = await GetAsync(ContextGuid, Code, token, tranName);
				if (moeinBusiness != null && moeinBusiness.Guid != Guid)
				{
					if (system == EnSystem.UserInterface)
					{
						ret.AddError(system, "کد حساب معین".GetTranslate() + "(" + (Name ?? "").GetTranslate() + " -" + $" {Code}".GetTranslate() + ")" + "تکراری می باشد".GetTranslate(), "Code", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 487);
					}
					else
					{
						Code = await NextCodeInGroupAsync(ContextGuid, system, KolGuid, token, tranName);
					}
				}
			}
			catch (Exception ex)
			{
				ret.AddError(system, "کد حساب معین".GetTranslate() + "(" + (Name ?? "").GetTranslate() + " -" + $" {Code}" + ")" + " تکراری می باشد".GetTranslate(), "Code", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 494);
				if (ex.Message.Contains("Sequence contains more than one element"))
				{
					Log.Debug(ex, "CheckCodeMoein");
				}
				else
				{
					ret.AddReturnedValue(ex, $"CheckCodeMoein. codeMoein:{Code}", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 499);
					WebErrorLog.Instance.StartLog(ex, $"CheckCodeMoein system:{system}", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 500);
				}
			}
			if (system == EnSystem.UserInterface && !Code.ToString().StartsWith(kol.Code.ToString()))
			{
				ret.AddError("کد حساب معین".GetTranslate() + "(" + (Name ?? "").GetTranslate() + ")-(" + $"{Code}".GetTranslate() + ")" + " با حساب کل".GetTranslate() + "(" + $"{kol.Code}".GetTranslate() + ")" + " تطابق ندارد.".GetTranslate(), "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 505);
			}
		}
		catch (Exception ex2)
		{
			ret.AddReturnedValue(ex2, "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 509);
			WebErrorLog.Instance.StartLog(ex2, "", "CheckValidationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 510);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateAccountAsync(string contextGuid, Guid guid, decimal sum, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 522), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 522);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.UpdateAccountAsync(guid, sum, tryCount, token, tranName), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 524);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 528);
				ret.AddReturnedValue(ex, "", "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 529);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 534), ReturnedState.None, "UpdateAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 534);
			}
		}
		return ret;
	}

	[Obsolete("Apply access implemented on client side", true)]
	public static async Task<ReturnedSaveFuncInfo> KeepRangeAsync(string contextGuid, IEnumerable<Guid> select, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 546), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 546);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.KeepRangeAsync(select, tryCount, token, tranName), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 547);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 551);
				ret.AddReturnedValue(ex, "", "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 552);
			}
		}
		finally
		{
			if (autoTran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 557), ReturnedState.None, "KeepRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 557);
			}
		}
		return ret;
	}

	public static async Task<long> GetStartCodeAsync(string contextGuid, EnSystem system, Guid kolGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		string text;
		try
		{
			KolBusiness kol = await KolBusiness.GetAsync(contextGuid, kolGuid, token, tranName);
			if (kol == null)
			{
				throw new NullReferenceException($"Kol({kolGuid}) is not exists");
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			text = kol.Code.ToString();
			if (system == EnSystem.Transfer)
			{
				return text.ParseToLong(0L);
			}
			while (text.Length < accGlobalSettings.Sanads.MoeinLen)
			{
				text += "0";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetStartCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 576);
			throw;
		}
		return text.ParseToLong(0L);
	}

	public static async Task<long> GetEndCodeAsync(string contextGuid, EnSystem system, Guid kolGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		string endCode = "";
		try
		{
			KolBusiness kol = await KolBusiness.GetAsync(contextGuid, kolGuid, token, tranName);
			if (kol == null)
			{
				throw new NullReferenceException($"Kol({kolGuid}) is not exists");
			}
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			endCode = kol.Code.ToString();
			while (endCode.Length < accGlobalSettings.Sanads.MoeinLen)
			{
				endCode += "9";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetEndCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 593);
		}
		return endCode.ParseToLong(0L);
	}

	public static async Task<long> NextCodeInGroupAsync(string contextGuid, EnSystem system, Guid KolGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 3;
		try
		{
			IMoeinRepository iMoein = ClsCache.GetCache(contextGuid).CacheRepository.IMoein;
			long num = await iMoein.NextCodeInGroupAsync(KolGuid, await GetStartCodeAsync(contextGuid, system, KolGuid, token, tranName), await GetEndCodeAsync(contextGuid, system, KolGuid, token, tranName), token, tranName);
			if (num <= 0)
			{
				num = await GetStartCodeAsync(contextGuid, system, KolGuid, token, tranName);
			}
			return num;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextCodeInGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 607);
			throw;
		}
	}

	public async Task<bool> CheckCodeInRange(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			long code = Code;
			if (code > await GetEndCodeAsync(ContextGuid, system, KolGuid, token))
			{
				return false;
			}
			code = Code;
			if (code < await GetStartCodeAsync(ContextGuid, system, KolGuid, token))
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckCodeInRange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 623);
			throw;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IMoein;
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 636), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 636);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IMoein.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 638);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 641);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 644);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 645);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 650), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 650);
			}
		}
		return ret;
	}

	public static Task<MoeinBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<MoeinBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Moein;
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال حساب های معین".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Hesabs;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 677);
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
					return ret.AddError(EnSystem.None, "کش آنلاین نیست!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 679);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 681);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.IMoein == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IMoein is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 689);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("حساب های معین سینک هستند تعداد حساب معین سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 690);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 697);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 699);
						}
						MoeinBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "حساب معین جاری".GetTranslate() + ":" + (obj?.Name ?? "").GetTranslate();
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Moein \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 708);
							continue;
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 718);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 722);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 724);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 727);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 728);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_resendToServerSemaphore.Release();
				}
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs"))
		{
			_ = 3;
			try
			{
				if (!ClsCache.GetCache(ContextGuid).BufferEnable)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "MoineBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 760);
				}
				List<WebMoein> list = new List<WebMoein>();
				List<WebMoein> list2 = list;
				list2.Add(await MoeinMapper.Instance.MapAsync(ContextGuid, this, new WebMoein(), "", token));
				List<Packet> paks = WebMoein.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 762);
				}
				using MoeinWaiter waiter = new MoeinWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("moeinBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 766);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 768);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 770);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 773);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 774);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, IEnumerable<Guid> items, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 786), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 786);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.DeleteRangeAsync(items, tryCount, token, tranName), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 788);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 792);
				ret.AddReturnedValue(ex, "", "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 793);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 798), ReturnedState.None, "DeleteRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 798);
			}
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfoWithValue<MoeinBusiness>> SaveTempMoeinDefaultAsnadDaryaftaniNazdeSandoghAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfoWithValue<MoeinBusiness> ret = new ReturnedSaveFuncInfoWithValue<MoeinBusiness>();
		try
		{
			ReturnedSaveFuncInfoWithValue<MoeinBusiness> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = await GetAsync(contextGuid, ParentDefaults.MoeinCoding.CLSMoein29976, "", token);
			if (ret.value != null && ret.value.Guid == ParentDefaults.MoeinCoding.CLSMoein29976)
			{
				return ret;
			}
			ret.value = new MoeinBusiness(contextGuid)
			{
				Account = 0m,
				DateM = DateTime.Now,
				DeliveryDate = DateTime.Now,
				ContextGuid = contextGuid,
				DeliveryId = 0,
				DeliveryMessage = "",
				DeliveryShortMessage = "",
				Description = "ثبت اتومات سیستم".GetTranslate(),
				Guid = ParentDefaults.MoeinCoding.CLSMoein29976,
				Name = "اسناد دریافتنی نزد صندوق".GetTranslate(),
				HostRefrenced = "",
				Mahiat = ENMahiat.Debit_Credit,
				Modified = DateTime.Now,
				ServerStatus = ServerStatusForClient.Delivered,
				Status = true,
				System = true,
				KolGuid = ParentDefaults.HesabGroupGuids.DarayiJari,
				Peygiri = true,
				Tedadi = true
			};
			ret.value.Code = 1102103L;
			returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await ret.value.SaveAsync(system, updateAccount: false, sendToServer: false, Priority.Low, null, 0, awaitOnSemaphore: true, await4Send: false, 10, "", token), ReturnedState.None, "SaveTempMoeinDefaultAsnadDaryaftaniNazdeSandoghAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 838);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveTempMoeinDefaultAsnadDaryaftaniNazdeSandoghAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 842);
			ret.AddReturnedValue(ex, "", "SaveTempMoeinDefaultAsnadDaryaftaniNazdeSandoghAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 843);
		}
		return ret;
	}

	public static async Task<IEnumerable<MoeinBusiness>> GetAllBetweenCodesAsync(string contextGuid, long? code1 = null, long? code2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await ApplyAccessAsync(await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetAllBetweenCodesAsync(code1, code2, token, tranName)))?.OrderBy((MoeinBusiness p) => p.Code)?.ToList();
	}

	public static Task<MoeinBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? kolGuid = null, string description = "", Guid? guid = null, bool isSystem = false, string tranName = "")
	{
		try
		{
			MoeinBusiness moeinBusiness = new MoeinBusiness(contextGuid)
			{
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				ChangeStatus = EntityChangeSt.New,
				System = isSystem
			};
			if ((kolGuid ?? Guid.Empty) != Guid.Empty)
			{
				moeinBusiness.KolGuid = kolGuid.Value;
			}
			if (!string.IsNullOrEmpty(description))
			{
				moeinBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				moeinBusiness.Guid = guid.Value;
			}
			else
			{
				moeinBusiness.Guid = Guid.NewGuid();
			}
			return Task.FromResult(moeinBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\MoeinBusiness.cs", 871);
			throw;
		}
	}

	private static async Task<IEnumerable<MoeinBusiness>> ApplyAccessAsync(IEnumerable<MoeinBusiness> moeinBusinesses)
	{
		return moeinBusinesses;
	}

	public static async Task<byte[]> GetGardeshHesabAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IMoein.GetGardeshHesabAsync(guid, token, tranName);
	}

	public static Task<ReturnedSaveFuncInfo> SaveAsync(string contextGuid, MoeinBusiness moein, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IMoein.SaveAsync(moein, updateAccount, tryCount, token, tranName);
	}
}
