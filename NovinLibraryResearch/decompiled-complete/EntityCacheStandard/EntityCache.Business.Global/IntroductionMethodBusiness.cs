using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.Global;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Global;

public class IntroductionMethodBusiness : BaseBusiness<IntroductionMethodBusiness>, IIntroductionMethod, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<IntroductionMethodBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).GlobalDatabaseManager;

	public IntroductionMethodBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<IEnumerable<IntroductionMethodBusiness>> GetAllAsync(string contextGuid, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod.GetAllAsync(status, token, tranName);
	}

	public static Task<IntroductionMethodBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod.GetAsync(guid, token, tranName);
	}

	public static Task<IntroductionMethodBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod.GetAsync(name, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, CancellationToken token = default(CancellationToken), bool await4send = false, string tranName = "")
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
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "IntroductionMethodBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 57);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 58), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 58);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 60);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IntroductionMethod.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 62);
							goto end_IL_0196;
						}
						result = ret;
					}
					goto end_IL_00d1;
					end_IL_0196:;
				}
				goto end_IL_00bd;
				end_IL_00d1:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 64);
				goto end_IL_00bd;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 67);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 68);
				goto end_IL_00bd;
			}
			return result;
			end_IL_00bd:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 73), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 73);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 76);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system));
				}
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<IntroductionMethodBusiness> lst, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (IntroductionMethodBusiness data in lst)
			{
				current = checked(current + 1);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await data.SaveAsync(system, sendToServer: false, token, await4send: false, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 93);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, data.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 105);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 106);
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

	public static Task<IntroductionMethodBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<IntroductionMethodBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Desc;
			int line = 0;
			try
			{
				if (uiUpdater != null)
				{
					line = 1;
					uiUpdater.ShortMessage = "در حال ارسال نحوه آشنایی".GetTranslate();
					line = 2;
					uiUpdater.Status = SyncStatus.Syncing;
					line = 3;
					uiUpdater.FinalStatus = ret;
					line = 4;
					uiUpdater.Group = SyncPart.Others;
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 145);
				}
				line = 6;
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 148);
				}
				line = 7;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 150);
				}
				line = 8;
				if (ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod == null)
				{
					line = 9;
					_ = ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod;
					line = 10;
					throw new NullReferenceException("repo");
				}
				line = 11;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IntroductionMethod.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 15;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 164);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("نحوه آشنایی سینک هستند تعداد شرح سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 165);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 166);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				line = 20;
				_ = (decimal)(list?.Count ?? 0);
				line = 21;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						line = 25;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 176);
						}
						current = checked(current + 1);
						IntroductionMethodBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						line = 30;
						if (uiUpdater != null)
						{
							line = 31;
							uiUpdater.ShortMessage = "داده جاری".GetTranslate() + ":" + obj.Name;
							uiUpdater.Count = current;
						}
						line = 32;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 187);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							line = 35;
							ret.AddInformation("current status on database is delivered!\r\nEntityType: IntroductionMethod \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityDesc:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 192);
							continue;
						}
						line = 40;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 199);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							line = 45;
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 205);
							tasks.Remove(t);
						}
						line = 50;
					}
				}
				line = 51;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 211);
				line = 52;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 213);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 217);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 218);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "IntroductionMethodBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 248);
				}
				List<WebIntroductionMethod> list = new List<WebIntroductionMethod>();
				List<WebIntroductionMethod> list2 = list;
				list2.Add(await ToWebItemAsync());
				List<Packet> lstPacket = WebIntroductionMethod.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 250);
				}
				using IntroductionMethodWaiter waiter = new IntroductionMethodWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("IntroductionMethodBusinessWaiter"), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 253);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 255);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 258);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 259);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Name))
			{
				res.AddError("وارد کردن نام الزامی می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 269);
			}
			IntroductionMethodBusiness introductionMethodBusiness = await GetAsync(ContextGuid, Name, token, tranName);
			if (introductionMethodBusiness != null && introductionMethodBusiness.Guid != Guid)
			{
				res.AddError("عنوان".GetTranslate() + " " + (" " + Name).GetTranslate() + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 271);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 275);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 276);
		}
		return res;
	}

	public async Task<WebIntroductionMethod> ToWebItemAsync()
	{
		using (new JaegerMonitor("ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs"))
		{
			try
			{
				return await IntroductionMethodMapper.Instance.MapAsync(ContextGuid, this, new WebIntroductionMethod());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 292);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			try
			{
				Modified = DateTime.Now;
				using (await _databaseSemaphore.GaurdAsync(true, "IntroductionMethodBusiness/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 306), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 306);
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IntroductionMethod.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 308);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 310);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 313);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 314);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 319), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\IntroductionMethodBusiness.cs", 319);
			}
		}
		return ret;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IntroductionMethod;
	}
}
