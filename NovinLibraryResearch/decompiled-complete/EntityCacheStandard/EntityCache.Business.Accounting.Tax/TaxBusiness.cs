using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Filters;
using EntityCache.Mapping.BusinessMappers.Accounting.Tax;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Tax;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.Tax;

public class TaxBusiness : BaseBusiness<TaxBusiness>, ITax, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<TaxBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private List<TaxDetailBusiness> _detailList;

	private bool _isDisposed;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Slug { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	public short NextOrdinal
	{
		get
		{
			IReadOnlyCollection<TaxDetailBusiness> detailList = DetailList;
			return checked((short)((detailList == null || !detailList.Any()) ? 1 : ((DetailList?.Max((TaxDetailBusiness p) => p.Ordinal) ?? (-1)) + 1)));
		}
	}

	public IReadOnlyCollection<TaxDetailBusiness> DetailList
	{
		get
		{
			if (_detailList == null)
			{
				_detailList = new List<TaxDetailBusiness>();
			}
			_detailList.RemoveAll((TaxDetailBusiness q) => q?.IsDisposed ?? true);
			return _detailList;
		}
	}

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public TaxBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.TaxBusiness;

	public event Func<object, Guid, Task> OnItemsChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public TaxBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		Guid = Guid.NewGuid();
		ChangeStatus = EntityChangeSt.New;
		Modified = DateTime.Now;
		Status = true;
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			foreach (TaxDetailBusiness item in _detailList?.ToList() ?? new List<TaxDetailBusiness>())
			{
				item?.Dispose();
			}
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 79);
		}
	}

	public Task AddDetailListAsync(TaxDetailBusiness detail, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (detail == null)
			{
				return Task.CompletedTask;
			}
			if (_detailList == null)
			{
				_detailList = new List<TaxDetailBusiness>();
			}
			detail.MasterGuid = Guid;
			detail.OnDisposed += Det_OnDisposed;
			_detailList.Add(detail);
			if (EventManager.OnItemsChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnItemsChanged, this, detail.Guid, token, 60, "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 93);
		}
		return Task.CompletedTask;
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs"))
		{
			try
			{
				if ((_detailList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (TaxDetailBusiness item in _detailList?.ToList()?.Where((TaxDetailBusiness p) => p == null || p.Master == sender || p == sender)?.ToList() ?? new List<TaxDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				_detailList?.RemoveAll((TaxDetailBusiness p) => p == null || p.Master == sender || p == sender);
				if (EventManager.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 114);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveDetailAsync(Guid detailGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			TaxDetailBusiness taxDetailBusiness = _detailList?.FirstOrDefault((TaxDetailBusiness q) => q.Guid == detailGuid);
			if (taxDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			_detailList.Remove(taxDetailBusiness);
			if (EventManager.OnItemsChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnItemsChanged, this, detailGuid, token, 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 129);
		}
		return Task.CompletedTask;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.Tax;
	}

	public static Task<TaxBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.Tax.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<TaxBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs"))
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
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 145);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 147);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 148);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.Tax == null)
				{
					_ = ClsCache.GetCache(contextGuid).CacheRepository.Tax;
					throw new NullReferenceException("repo");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.Tax.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 156);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("مالیات کالاها سینک هستند تعداد سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 157);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 158);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 165);
						}
						current = checked(current + 1);
						TaxBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 168);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: TaxBusiness \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityDesc:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 172);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 178);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 183);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 187);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 188);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 192);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 193);
			}
			return ret;
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
				using (await _databaseSemaphore.GaurdAsync(true, "TaxBusiness/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 207), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 207);
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Tax.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 209);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 211);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 214);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 215);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 220), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 220);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs"))
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
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "TaxBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 236);
				}
				WebTax item = await TaxMapper.Instance.MapAsync(ContextGuid, this, new WebTax(), "", token);
				List<Packet> paks = WebTax.GenerateSaveRequest(new List<WebTax> { item }, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 239);
				}
				using TaxWaiter waiter = new TaxWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => cache.SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("TaxWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 243);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 245);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 247);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 250);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 251);
			}
			return ret;
		}
	}

	public static Task<TaxBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Tax.GetAsync(guid, token, tranName);
	}

	public static Task<TaxReportBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Tax.GetAsync(name, token, tranName);
	}

	internal static Task<TaxReportBusiness> GetBySlugAsync(string contextGuid, string slug, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Tax.GetBySlugAsync(slug, token, tranName);
	}

	public static Task<IEnumerable<TaxReportBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Tax.GetAllReportAsync(status, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, bool sendToServer, CancellationToken token, IEnumerable<TaxBusiness> lst, Priority priority = Priority.Low, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (TaxBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 271);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, priority, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 272);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 277);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 278);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "TaxBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 291);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 292), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 292);
						}
						transactionHasBeenCreated = true;
						if (string.IsNullOrEmpty(Slug) && Status)
						{
							Slug = Name.Negasht();
						}
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 295);
						if (!res.HasError)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Tax.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 297);
							goto end_IL_0188;
						}
						result = res;
					}
					goto end_IL_00df;
					end_IL_0188:;
				}
				goto end_IL_00ce;
				end_IL_00df:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 299);
				goto end_IL_00ce;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 302);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 303);
				goto end_IL_00ce;
			}
			return result;
			end_IL_00ce:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 308), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 308);
			}
			if (!res.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 311);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Name))
			{
				await res.AddErrorAsync("لطفا عنوان را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 322);
			}
			if (string.IsNullOrEmpty(Slug))
			{
				await res.AddErrorAsync("لطفا نامک را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 323);
			}
			if (Slug.IsPersianText())
			{
				await res.AddErrorAsync("نامک حتما باید لاتین باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 324);
			}
			if (!(DetailList?.Any() ?? false))
			{
				await res.AddErrorAsync("هیچ درصدی وارد نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 325);
			}
			if ((DetailList?.Count() ?? 0) > 1)
			{
				await res.AddErrorAsync("کاربر گرامی، درحال حاضر فقط یک مقدار را برای مالیات می توانید وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 326);
			}
			foreach (TaxDetailBusiness item in DetailList ?? new List<TaxDetailBusiness>())
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidationAsync(), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 328);
			}
			if (!string.IsNullOrEmpty(Name))
			{
				TaxReportBusiness taxReportBusiness = await GetAsync(ContextGuid, Name, token, tranName);
				if (taxReportBusiness != null && taxReportBusiness.Guid != Guid)
				{
					await res.AddErrorAsync("عنوان " + (Name + " ").GetTranslate() + " " + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 332);
				}
			}
			if (!string.IsNullOrEmpty(Slug))
			{
				TaxReportBusiness taxReportBusiness2 = await GetBySlugAsync(ContextGuid, Slug, token, tranName);
				if (taxReportBusiness2 != null && taxReportBusiness2.Guid != Guid)
				{
					await res.AddErrorAsync("نامک".GetTranslate() + " " + (" " + Slug).GetTranslate() + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 337);
				}
			}
			if (!Status)
			{
				ProductCompactFilter filter = new ProductCompactFilter
				{
					Status = true,
					TaxClassGuid = Guid,
					LoadJustParents = false
				};
				IEnumerable<ProductCompactBusiness> enumerable = await ProductCompactBusiness.GetAllAsync(ContextGuid, EnSystem.None, filter, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					await res.AddErrorAsync("مالیات".GetTranslate() + " " + (" " + Name).GetTranslate() + " " + " به دلیل ارتباط با".GetTranslate() + " " + $" {enumerable.Count()}".GetTranslate() + " " + " کالا، قابل حذف نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 348);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 353);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Tax\\TaxBusiness.cs", 354);
		}
		return res;
	}
}
