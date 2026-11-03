using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting.Coding;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Coding;
using PacketParser.EventArgs;
using PacketParser.Events;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Coding;

public class AccountBusiness : BaseBusiness<AccountBusiness>, IAccount, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<AccountBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public Guid Guid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid TafsilGuid { get; set; }

	public decimal Account { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public AccountBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<decimal> GetAccountSumAsync(string contextGuid, Guid? moeinGuid, Guid tafsilGuid, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				return Task.FromResult(0m);
			}
			if ((moeinGuid ?? Guid.Empty) != Guid.Empty)
			{
				return GetAccountSumAsync(contextGuid, new List<Guid> { moeinGuid.Value }, new List<Guid> { tafsilGuid }, status, tranName, token);
			}
			return GetAccountSumAsync(contextGuid, null, new List<Guid> { tafsilGuid }, status, tranName, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAccountSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 56);
			throw;
		}
	}

	public static Task<ReturnedSaveFuncInfoWithValue<byte[]?>> GetBallanceAsync(string contextGuid, Guid moeinGuid, Guid tafsilGuid, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Account.GetBallanceAsync(moeinGuid, tafsilGuid, tranName, token);
	}

	public static Task<decimal> GetAccountSumAsync(string contextGuid, List<Guid> moeinGuidList, List<Guid> tafsilGuidList, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Account.GetAccountSumAsync(moeinGuidList, tafsilGuidList, status, tranName, token);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.Account;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool semaphoreHasBeenTaken = false;
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			try
			{
				await _connectionPoolSemaphore.WaitAsync();
				semaphoreHasBeenTaken = true;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 76), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 76);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Account.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 78);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 80);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 83);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 84);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 89), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 89);
			}
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, EnSystem.None, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, EnSystem system, CancellationToken token, string tranName = "", short maxParalelTask = 10)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool semaphoreHasBeenTaken = false;
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Account;
			try
			{
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
				{
					return ret;
				}
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال مانده حساب ها";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Hesabs;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 114);
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
					return ret.AddError(EnSystem.None, "کش آنلاین نیست!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 116);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 118);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.Account == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.Account is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.Account.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 124);
				}
				if (list == null || !list.Any())
				{
					return ret;
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 132);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 134);
						}
						AccountBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj == null)
						{
							continue;
						}
						if (uiUpdater != null)
						{
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Account \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 140);
							continue;
						}
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, obj.TafsilGuid, tranName, token, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs");
						if (tafsil != null)
						{
							Guid? moeinGuid = null;
							if (tafsil.HType == HesabType.Sandogh)
							{
								moeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
							}
							else if (tafsil.HType == HesabType.Bank)
							{
								moeinGuid = ParentDefaults.MoeinCoding.CLSMoein4;
							}
							decimal account = await GetAccountSumAsync(contextGuid, moeinGuid, tafsil.Guid, true, tranName, token);
							tafsil.Account = account;
							tasks.Add(Task.Run(() => tafsil.Send2ServerAsync(token, EnSystem.SignalR), token));
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 161);
							tasks.Remove(t2);
						}
					}
				}
				while (tasks?.Any() ?? false)
				{
					Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 168);
					tasks.Remove(t2);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 171);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 173);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 176);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 177);
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
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs"))
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
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "AccountBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 209);
				}
				List<WebAccount> list = new List<WebAccount>();
				List<WebAccount> list2 = list;
				list2.Add(await AccountMapper.Instance.MapAsync(ContextGuid, this, new WebAccount(), "", token));
				List<Packet> paks = WebAccount.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 211);
				}
				using AccountWaiter waiter = new AccountWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("AccountWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 215);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 217);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 219);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 222);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 223);
			}
			return ret;
		}
	}

	public static async Task<IEnumerable<AccountTableViewModel>> GetAllAsync(string contextGuid, Guid tafsilGuid, bool isLoadDefMoein = false, bool? status = true, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			IEnumerable<AccountTableViewModel> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.Account.GetAllAsync(tafsilGuid, status, tranName, token);
			if (!isLoadDefMoein || enumerable == null || !enumerable.Any())
			{
				return enumerable;
			}
			List<AccountTableViewModel> _lst = enumerable.ToList();
			decimal account = await GetAccountSumAsync(contextGuid, null, tafsilGuid, status, tranName, token);
			_lst.Add(new AccountTableViewModel
			{
				Account = account,
				MoeinCode = 0L,
				MoeinGuid = Guid.Empty,
				MoeinName = "[کلیه حساب های معین]"
			});
			return _lst;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 246);
			throw;
		}
	}

	public static Task<AccountBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.Account.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<AccountBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> SetAccountAsync(EnSystem system, byte[] gardesh, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
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
				if (await MoeinBusiness.GetAsync(ContextGuid, MoeinGuid, "", token) == null)
				{
					WebErrorLog.Instance.StartLog("یک اکانت آمده که دسترسی به معین اش احتمالا وجود مداره!", "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 265);
					result = ret;
					goto end_IL_0171;
				}
				if (await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, TafsilGuid, "", default(CancellationToken), "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs") == null)
				{
					WebErrorLog.Instance.StartLog("یک اکانت آمده که دسترسی به تفصیل اش احتمالا وجود نداره!", "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 272);
					result = ret;
					goto end_IL_0171;
				}
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AccountBusiness/SetAccountAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 278);
					}
					else
					{
						if (autoCommitTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 280), ReturnedState.None, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 280);
						}
						transactionHasBeenCreated = true;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							Modified = DateTime.Now;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Account.SetAccountAsync(this, tryCount, tranName, token), ReturnedState.None, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 284);
							if (!ret.HasError)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Account.SaveBallanceAsync(MoeinGuid, TafsilGuid, gardesh, tranName, token), ReturnedState.None, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 286);
								goto end_IL_0344;
							}
							result = ret;
						}
					}
					goto end_IL_0171;
					end_IL_0344:;
				}
				goto end_IL_00dc;
				end_IL_0171:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 290);
				ret.AddReturnedValue(ex, "", "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 291);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 297), ReturnedState.None, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 297);
				if (!ret.HasError)
				{
					RaiseAccountChangedEvent();
				}
			}
			if (!ret.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SetAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 302);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> BallanceAccountAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken))
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable && system != EnSystem.Transfer;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AccountBusiness/BallanceAccountAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 319);
					}
					else
					{
						if (autoCommitTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 321), ReturnedState.None, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 321);
						}
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							Modified = DateTime.Now;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Account.BallanceAccountAsync(this, tryCount, tranName, token), ReturnedState.None, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 325);
							goto end_IL_01bb;
						}
						result = ret;
					}
					goto end_IL_00e4;
					end_IL_01bb:;
				}
				goto end_IL_00d0;
				end_IL_00e4:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 329);
				ret.AddReturnedValue(ex, "", "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 330);
				goto end_IL_00d0;
			}
			return result;
			end_IL_00d0:;
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 336), ReturnedState.None, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 336);
				if (!ret.HasError)
				{
					RaiseAccountChangedEvent();
				}
			}
			if (!ret.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "BallanceAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 341);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	private void RaiseAccountChangedEvent()
	{
		try
		{
			TafsilEventAggregator tafsil = ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.Tafsil;
			if (tafsil.HasEventHandler)
			{
				tafsil.RaiseAccountChanged(this, TafsilGuid, "");
				tafsil.RaiseAccountSaved(this, EventArgs.Empty);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RaiseAccountChangedEvent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 358);
		}
	}

	public static async Task<List<Guid>> GetCustomerDefaultMoeinGuidsAsync(string contextGuid, Guid tafsilGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			IEnumerable<AccountTableViewModel> enumerable = await GetAllAsync(contextGuid, tafsilGuid, isLoadDefMoein: false, true, tranName, token);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			List<Guid> listMoeinGuid = new List<Guid>();
			foreach (AccountTableViewModel account in enumerable)
			{
				KolBusiness kolBusiness = await (await MoeinBusiness.GetAsync(contextGuid, account.MoeinGuid, "", token)).GetKolAsync(token, tranName);
				if (kolBusiness.GroupGuid != ParentDefaults.HesabGroupGuids.TarafHesabEntezami && kolBusiness.GroupGuid != ParentDefaults.HesabGroupGuids.HesabEntezami)
				{
					listMoeinGuid.Add(account.MoeinGuid);
				}
			}
			return listMoeinGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCustomerDefaultMoeinGuidsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 379);
			throw;
		}
	}

	public static Task<decimal> GetAccountSumAsync(string contextGuid, Guid tafsilGuid, List<Guid> moeinGuids, string tranName, CancellationToken token)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Account.GetAccountSumAsync(tafsilGuid, moeinGuids, tranName, token);
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, List<WebAccount> accounts, EnSystem signalR, CancellationToken token, bool awaitOnSemaphore = true)
	{
		string tranName = "";
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommitTran = string.IsNullOrEmpty(tranName);
		if (autoCommitTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AccountBusiness/SetAccountAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 395);
					}
					else
					{
						if (autoCommitTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 397), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 397);
						}
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Account.SaveRangeAsync(accounts, tranName, token), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 400);
							goto end_IL_0130;
						}
						result = ret;
					}
					goto end_IL_0094;
					end_IL_0130:;
				}
				goto end_IL_0083;
				end_IL_0094:;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 402);
				goto end_IL_0083;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 405);
				ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 406);
				goto end_IL_0083;
			}
			return result;
			end_IL_0083:;
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 412), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 412);
				if (ret.HasError && accounts.Count() == 1)
				{
					ret = await SaveWebAccountAsync(contextGuid, accounts.Single(), EnSystem.SignalR, token);
				}
				else if (ret.HasError && accounts.Count() > 1)
				{
					ret = new ReturnedSaveFuncInfo();
					List<Task<ReturnedSaveFuncInfo>> list = new List<Task<ReturnedSaveFuncInfo>>();
					List<WebAccount> list2 = new List<WebAccount>();
					WebAccount[] array = accounts.ToArray();
					for (int i = 0; i <= accounts.Count() / 2; i = checked(i + 1))
					{
						list2.Add(array[i]);
					}
					list.Add(SaveRangeAsync(contextGuid, list2, signalR, token, awaitOnSemaphore));
					list.Add(SaveRangeAsync(contextGuid, accounts.Except(list2).ToList(), signalR, token, awaitOnSemaphore));
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(list), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 425);
				}
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveWebAccountAsync(string contextGuid, WebAccount webAccount, EnSystem system, CancellationToken token)
	{
		string tranName = "";
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoCommitTran = string.IsNullOrEmpty(tranName);
		if (autoCommitTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 1;
			try
			{
				WebErrorLog.Instance.StartLog("خطا در ذخیره سازی حساب ها داشتم به یک تک حساب رسیدیم که این بنده خدا باشه:\r\n" + Json.ToStringJson(webAccount), "", "SaveWebAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 440);
				AccountBusiness item = await AccountMapper.Instance.MapAsync(contextGuid, webAccount, new AccountBusiness(contextGuid), "", token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.Account.SaveAsync(item, 1, token, tranName), ReturnedState.None, "SaveWebAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 443);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveWebAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 447);
				ret.AddReturnedValue(ex, "", "SaveWebAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 448);
			}
		}
		finally
		{
			if (autoCommitTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SaveWebAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 454), ReturnedState.None, "SaveWebAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 454);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<AccountBusiness>> GetAllAsync(string contextGuid, Guid? moeinGuid, Guid? tafsilGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Account.GetAllAsync(moeinGuid, tafsilGuid, status, token, tranName);
	}

	public static async Task<IEnumerable<AccountBusiness>> GetAllAsync(string contextGuid, List<Guid> tafsilGuidList, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (!(tafsilGuidList?.Any() ?? false))
			{
				return null;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.Account.GetAllAsync(tafsilGuidList, status, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Coding\\AccountBusiness.cs", 470);
			throw;
		}
	}
}
