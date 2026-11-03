using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.OrderDepot;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Order;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.TransferLayer.Waiters.Accounting.TaxService;

namespace EntityCache.Business.OrderDepot;

public class TaxOrderLogBusiness : BaseBusiness<TaxOrderLogBusiness>, ITaxOrderLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<TaxOrderLogBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public Guid OrderGuid { get; set; }

	public EnTaxResult OldState { get; set; }

	public EnTaxResult? NewState { get; set; }

	public DateTime? TaxSendDate { get; set; }

	public string TaxRefrenceNumber { get; set; }

	public DateTime? TaxDeliveryDate { get; set; }

	public string TaxResultErrorCode { get; set; }

	public string TaxResultMessage { get; set; }

	public string TaxUid { get; set; }

	public string TaxOrderId { get; set; }

	public override string ContextGuid { get; set; }

	public bool Status { get; set; }

	public DateTime InsertDate { get; set; }

	public bool IsSystem { get; set; }

	public EnTaxInvoiceSubject Subject { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public short Ordinal { get; set; }

	public string BaseTaxOrderId { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public DateTime Modified { get; set; }

	public string InvoiceSeries { get; set; }

	public TaxOrderLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, bool await4Send = false, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 65), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 65);
					}
					transactionHasBeenCreated = true;
					if (res.HasError)
					{
						return res;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TaxOrderLog.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 68);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 72);
					res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 73);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 78), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 78);
				}
				if (!res.HasError && sendToServer && system != EnSystem.Transfer)
				{
					if (await4Send)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 81);
					}
					else
					{
						Task.Run(() => Send2ServerAsync(token, system, priority));
					}
				}
			}
			return res;
		}
	}

	private static async Task<ReturnedSaveFuncInfo> RemoveRangAsync(string contextGuid, List<Guid> lstGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 96), ReturnedState.None, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 96);
					}
					transactionHasBeenCreated = true;
					if (res.HasError)
					{
						return res;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog.RemoveRangAsync(lstGuid, token, tranName), ReturnedState.None, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 99);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 103);
					res.AddReturnedValue(ex, "", "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 104);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 109), ReturnedState.None, "RemoveRangAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 109);
				}
			}
			return res;
		}
	}

	public static Task<IEnumerable<TaxOrderLogBusiness>> GetAllAsync(string contextGuid, TaxOrderLogFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog.GetAllAsync(filter, token, tranName);
	}

	public static Task<IEnumerable<TaxOrderLogReportBusiness>> GetAllReportAsync(string contextGuid, Guid orderGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog.GetAllReportAsync(orderGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<TaxOrderLogBusiness> enumerable = await GetAllAsync(contextGuid, null, token);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			List<Guid> removedLogGuidList = new List<Guid>();
			List<IGrouping<Guid, TaxOrderLogBusiness>> list = (from q in enumerable
				orderby q.InsertDate
				group q by q.OrderGuid into q
				select (q)).ToList();
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (IGrouping<Guid, TaxOrderLogBusiness> item2 in list)
			{
				if (item2 == null)
				{
					continue;
				}
				List<TaxOrderLogBusiness> data = item2.Select((TaxOrderLogBusiness w) => w)?.OrderBy((TaxOrderLogBusiness q) => q.InsertDate)?.ToList();
				if (!(data?.Any() ?? false))
				{
					continue;
				}
				foreach (TaxOrderLogBusiness item in data)
				{
					if ((item?.NewState).GetValueOrDefault() != EnTaxResult.Sent)
					{
						continue;
					}
					TaxOrderLogBusiness rank2 = data.FirstOrDefault((TaxOrderLogBusiness q) => q.TaxOrderId == item.TaxOrderId && q.Guid != item.Guid);
					if (rank2 != null)
					{
						item.OldState = EnTaxResult.Sent;
						item.NewState = rank2.NewState;
						item.TaxDeliveryDate = rank2.TaxDeliveryDate;
						item.TaxResultErrorCode = rank2.TaxResultErrorCode;
						item.TaxResultMessage = rank2.TaxResultMessage;
						item.ServerStatus = ServerStatusForClient.None;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer: true, Priority.Low, await4Send: false, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 142);
						if (res.HasError)
						{
							return res;
						}
						removedLogGuidList.Add(rank2.Guid);
					}
				}
			}
			if (res.HasError || !(removedLogGuidList?.Any() ?? false))
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await RemoveRangAsync(contextGuid, removedLogGuidList, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 148);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 152);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 153);
		}
		return res;
	}

	internal static Task<short> GetNextOrdinalAsync(string contextGuid, Guid orderGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog.GetNextOrdinalAsync(orderGuid, token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.TaxOrderLog;
	}

	public static Task<TaxOrderLogBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<TaxOrderLogBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (ClsCache.GetCache(contextGuid).Source != ENSource.Accounting)
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 172);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 174);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 175);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog == null)
				{
					_ = ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog;
					throw new NullReferenceException("repo");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.TaxOrderLog.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 183);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("گزارش سامانه مودیان سینک هستند تعداد سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 184);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 185);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 192);
						}
						current = checked(current + 1);
						TaxOrderLogBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 195);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: TaxBusiness \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityDesc:" + obj?.TaxRefrenceNumber, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 198);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 204);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 209);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 213);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 214);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 218);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 219);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			_ = 2;
			try
			{
				Modified = DateTime.Now;
				using (await _databaseSemaphore.GaurdAsync(true, "TaxOrderLogBusiness/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 233), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 233);
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.TaxOrderLog.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 235);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 237);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 240);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 241);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 246), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 246);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs"))
		{
			_ = 3;
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (!cache.BufferEnable || cache.Source != ENSource.Accounting)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "TaxOrderLogBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 262);
				}
				WebTaxOrderLog item = await TaxOrderLogMapper.Instance.MapAsync(ContextGuid, this, new WebTaxOrderLog(), "", token);
				List<Packet> paks = WebTaxOrderLog.GenerateSaveRequest(new List<WebTaxOrderLog> { item }, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 265);
				}
				using TaxOrderLogWaiter waiter = new TaxOrderLogWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => cache.SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("TaxOrderLogWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 269);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 271);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 273);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 276);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\OrderDepot\\TaxOrderLogBusiness.cs", 277);
			}
			return ret;
		}
	}
}
