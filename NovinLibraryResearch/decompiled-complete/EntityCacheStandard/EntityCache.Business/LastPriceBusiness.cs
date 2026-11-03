using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business;

public class LastPriceBusiness : ILastPrice, IHasModified, IHasJustGuid, IHasStatus, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public string ContextGuid { get; set; }

	public DateTime Date { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public decimal Price { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid PrdGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public bool Invalidated { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public bool Status { get; set; }

	public LastPriceBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (PrdGuid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError(system, "Product guid is Empty", "PrdGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 50);
			}
			if (TafsilGuid == Guid.Empty)
			{
				returnedSaveFuncInfo.AddError(system, "Tafsil guid is Empty", "TafsilGuid", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 53);
			}
			if (Price < 0m)
			{
				returnedSaveFuncInfo.AddError(system, $"value({Price}) is not valid for price", "Price", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 56);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 60);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 61);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, Priority priority, bool awaitOnSemaphore = true, bool sendToServer = true, bool awaitOnSend = false, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = await CheckValidationAsync(system);
		bool transactionHasBeenCreated = false;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "LastPriceBusiness/SaveAsync", 60000, token))
				{
					if (autoTran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 76), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 76);
					}
					transactionHasBeenCreated = true;
					if (!dis.Token.IsCancellationRequested)
					{
						if (!token.IsCancellationRequested)
						{
							ret.throwExceptionIfError();
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.LastPriceRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 81);
						goto end_IL_022b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 78);
					goto end_IL_014d;
					end_IL_022b:;
				}
				goto end_IL_0136;
				end_IL_014d:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 83);
				goto end_IL_0136;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 86);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 87);
				goto end_IL_0136;
			}
			return result;
			end_IL_0136:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 92), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 92);
			}
			if (sendToServer && !ret.HasError)
			{
				if (awaitOnSend)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 95);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	public static async Task<IEnumerable<LastPriceBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.LastPriceRepository.GetAllAsync(token, tranName);
	}

	public static async Task<LastPriceBusiness> GetAsync(string contextGuid, Guid prdGuid, Guid tafsilGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.LastPriceRepository.GetAsync(new LastPriceCompositeKey
		{
			ProductGuid = prdGuid,
			TafsilGuid = tafsilGuid
		}, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, Priority priority, List<LastPriceBusiness> lstLastprice, bool sendToServer, bool flush = false, short tryCount = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			using (await _databaseSemaphore.GaurdAsync(true, "LastPriceBusiness/SaveAsync", 60000, token))
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 111);
				}
				IEnumerable<string> source = lstLastprice.Select((LastPriceBusiness p) => $"{p.PrdGuid}/{p.TafsilGuid}");
				if (source.Distinct().Count() != source.Count())
				{
					throw new Exception($"items({source.Count()})//Distinct({source.Distinct().Count()})");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 115);
				}
				foreach (LastPriceBusiness item in lstLastprice)
				{
					if (token.IsCancellationRequested)
					{
						ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 119);
					}
					if (ret.HasError)
					{
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, priority, awaitOnSemaphore: true, sendToServer, awaitOnSend: false, tryCount), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 121);
				}
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 124);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 127);
			ret.AddReturnedValue(ex2, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 128);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse<LastPriceCompositeKey> item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 141), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 141);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.LastPriceRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 143);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 145);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 148);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 149);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 154), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 154);
			}
		}
		return ret;
	}

	internal static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.LastPrice;
			try
			{
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
				{
					return ret;
				}
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال آخرین قیمت کالاها".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Others;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 176);
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
					return ret.AddError(system, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 178);
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 180);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.LastPriceRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.LastPriceRepository is null");
				}
				List<LastPriceCompositeKey> list = (await ClsCache.GetCache(contextGuid).CacheRepository.LastPriceRepository.GetAllNotSentAsync(token, tranName)) ?? new List<LastPriceCompositeKey>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 60;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 188);
				}
				line = 61;
				if (list == null || !list.Any())
				{
					return ret.AddInformation("آخرین قیمت کالاها سینک هستند تعداد سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 190);
				}
				line = 62;
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				line = 63;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (LastPriceCompositeKey item in list)
					{
						line = 70;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 200);
						}
						current = checked(current + 1);
						LastPriceBusiness obj = await GetAsync(contextGuid, item.ProductGuid, item.TafsilGuid, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "";
							uiUpdater.Count = current;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 208);
						}
						line = 720;
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							line = 75;
							ret.AddInformation("current status on database is delivered!\r\nEntityType: LastPrice \r\n" + $"Product guid:{obj?.PrdGuid ?? Guid.Empty}" + $"Tafsil guid: {obj?.TafsilGuid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 214);
							line = 76;
							continue;
						}
						line = 80;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 222);
						}
						line = 81;
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, system)));
						line = 82;
						if (tasks.Count >= maxParalelTask)
						{
							line = 83;
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							line = 84;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 231);
							tasks.Remove(t);
							line = 85;
						}
						line = 86;
					}
				}
				line = 87;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 238);
				line = 88;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 240);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 243);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 246);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 247);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!ClsCache.GetCache(ContextGuid).BufferEnable)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "LastPriceBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 279);
				}
				List<WebLastPrice> list = new List<WebLastPrice>();
				List<WebLastPrice> list2 = list;
				list2.Add(await LastPriceMapper.Instance.MapAsync(ContextGuid, this, new WebLastPrice(), "", token));
				List<Packet> paks = WebLastPrice.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 281);
				}
				using LastPriceWaiter waiter = new LastPriceWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), new LastPriceCompositeKey
				{
					TafsilGuid = TafsilGuid,
					ProductGuid = PrdGuid
				}, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse<LastPriceCompositeKey>> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse();
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 285);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 287);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 289);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 292);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LastPriceBusiness.cs", 293);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, EnSystem.SignalR, token, tranName, maxParalelTask);
	}

	public static Task<IEnumerable<LastPriceBusiness>> GetAllAsync(string contextGuid, Guid productGuid, Guid tafsilGuid, Document asType, CancellationToken token, string tranName)
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.LastPriceRepository.GetAllAsync(productGuid, tafsilGuid, asType, token, tranName);
	}
}
