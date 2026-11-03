using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using EntityCache.Utilities;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.TransferLayer.Waiters.Accounting;

namespace EntityCache.Business.Accounting;

public class ViewCheckShBusiness : BaseBusiness<ViewCheckShBusiness>, IViewCheckSh, IHasJustGuid, IHasStatus, ISyncFromClient2Server<ViewCheckShBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private PardakhtBusiness _pardakht;

	public Guid Guid { get; set; }

	public string Number { get; set; }

	public decimal Price { get; set; }

	public ENCheckSHStatus ChequeStatus { get; set; }

	public string DateSarresid => DateSarresidM.MiladiToCalture();

	public string BankName { get; set; }

	public string ST => ChequeStatus.GetDisplay().GetTranslate();

	public string CustomerFullName { get; set; }

	public string PardakhtDate { get; set; }

	public Guid GuidSanad { get; set; }

	public bool Aval2re { get; set; }

	public DateTime DateSarresidM { get; set; }

	public string PardakhtDesc { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public bool IsChecked { get; set; }

	public bool IsEnteghali { get; set; }

	public bool SayadiRegistered { get; set; }

	public override string ContextGuid { get; set; }

	public Guid BankGuid { get; set; }

	public Guid? A2reMoeinGuid { get; set; }

	public Guid? A2reTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	bool IHasStatus.Status => true;

	public ENCheckSHStatus Status { get; set; }

	public string AutoDescription => "چک پرداختنی(" + Number + ") سررسید(" + DateSarresid + ")";

	public List<ChequeShOperationBusiness> OperationList { get; set; }

	public byte[] StatusImage
	{
		get
		{
			if (IsEnteghali)
			{
				return ImageResourceManager.ChequeAvalDore;
			}
			return ImageResourceManager.ChequeDuringDore;
		}
	}

	public DateTime? DateUtc { get; set; }

	public async Task<PardakhtBusiness> GetPardakhtObjAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_pardakht != null)
			{
				return _pardakht;
			}
			PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(ContextGuid, GuidSanad, token, tranName);
			PardakhtBusiness pardakht = ((pardakhtCheckShBusiness == null) ? null : (await PardakhtBusiness.GetAsync(ContextGuid, pardakhtCheckShBusiness.MasterGuid ?? Guid.Empty, fromDatabase: true, token, tranName)));
			_pardakht = pardakht;
			return _pardakht;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtObjAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 65);
			return null;
		}
	}

	public ViewCheckShBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(CancellationToken token, List<ViewCheckShBusiness> lst, EnSystem system, bool sendToServer, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (lst == null || !lst.Any())
			{
				line = 1;
				return ret;
			}
			line = 2;
			foreach (ViewCheckShBusiness item in lst)
			{
				line = 3;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 109);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 112);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs"))
		{
			bool transactionHasBeenCreated = false;
			int line = 0;
			try
			{
				_ = 9;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ViewCheckShBusiness/SaveAsync", 60000, token))
					{
						line = 1;
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 129);
						}
						else
						{
							line = 2;
							if (autoTran)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 131), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 131);
							}
							line = 3;
							ClsCache cache = ClsCache.GetCache(ContextGuid);
							MoeinBusiness creditMoein = await MoeinBusiness.GetAsync(ContextGuid, CreditMoeinGuid, tranName, token);
							TafsilBusiness creditTafsil = await TafsilBusiness.GetAsync(ContextGuid, system, CreditTafsilGuid ?? Guid.Empty, tranName, token, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs");
							TafsilBusiness bankInfo = await TafsilBusiness.GetAsync(ContextGuid, system, BankGuid, tranName, token, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs");
							TafsilBusiness a2reTafsil = await TafsilBusiness.GetAsync(ContextGuid, system, A2reTafsilGuid ?? Guid.Empty, tranName, token, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs");
							MoeinBusiness moeinBusiness = await MoeinBusiness.GetAsync(ContextGuid, A2reMoeinGuid ?? Guid.Empty, tranName, token);
							if (system == EnSystem.SignalR && cache.Source == ENSource.Android_HesabYar && (bankInfo == null || ((A2reTafsilGuid ?? Guid.Empty) != Guid.Empty && a2reTafsil == null) || ((A2reMoeinGuid ?? Guid.Empty) != Guid.Empty && moeinBusiness == null) || ((CreditTafsilGuid ?? Guid.Empty) != Guid.Empty && creditTafsil == null) || ((CreditTafsilGuid ?? Guid.Empty) != Guid.Empty && creditTafsil == null) || (CreditMoeinGuid != Guid.Empty && creditMoein == null)))
							{
								result = ret;
							}
							else
							{
								transactionHasBeenCreated = true;
								Modified = DateTime.Now;
								line = 4;
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									line = 5;
									ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckShRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 156);
									if (ret.HasError)
									{
										result = ret;
									}
									else
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.RemoveRangeAsync(ContextGuid, Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 158);
										if (!ret.HasError)
										{
											if (OperationList?.Any() ?? false)
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ChequeShOperationBusiness.SaveRangeAsync(OperationList, system, sendToServer, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 161);
											}
											goto end_IL_0163;
										}
										result = ret;
									}
								}
							}
						}
						goto end_IL_00a7;
						end_IL_0163:;
					}
					goto end_IL_0095;
					end_IL_00a7:;
				}
				catch (OperationCanceledException exception)
				{
					ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 163);
					goto end_IL_0095;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 166);
					ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 167);
					goto end_IL_0095;
				}
				return result;
				end_IL_0095:;
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 172), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 172);
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> DeleteRangeAsync(string contextGuid, List<Guid> eConnectionData, CancellationToken token = default(CancellationToken))
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.DeleteRangeAsync(eConnectionData, 3, token, "");
	}

	public static async Task<List<ViewCheckShBusiness>> GetAllAsync(string contextGuid, string search = null, CancellationToken token = default(CancellationToken), ENCheckSHStatus? status = null, DateTime? date1 = null, DateTime? date2 = null, string tranName = "")
	{
		try
		{
			IEnumerable<ViewCheckShBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.GetAllAsync(status, date1, date2, token, tranName);
			List<string> list = search?.SplitString();
			if (!string.IsNullOrEmpty(search))
			{
				foreach (string item in list)
				{
					if (!string.IsNullOrEmpty(item))
					{
						enumerable = enumerable?.Where((ViewCheckShBusiness p) => p.BankName.StringComparisons(item) || p.CustomerFullName.StringComparisons(item) || p.Number.StringComparisons(item) || p.Price.ToString().StringComparisons(item) || p.PardakhtDesc.StringComparisons(item));
					}
				}
			}
			return token.IsCancellationRequested ? null : enumerable?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 202);
			throw;
		}
	}

	public static Task<ViewCheckShBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool tranactionsHasBeenCreated = false;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = 2;
			try
			{
				using (await _databaseSemaphore.GaurdAsync(true, "ViewCheckSh/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 217), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 217);
					}
					tranactionsHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckShRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 219);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 222);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 225);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 226);
			}
		}
		finally
		{
			if (autoTran && tranactionsHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 231), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 231);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentAsync(string contextGuid, CancellationToken inputToken = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ClsCache cache = ClsCache.GetCache(contextGuid);
			ISendData2Server uiUpdater = cache?.SendData2ServerInstance?.ViewChequeSh;
			try
			{
				if (cache.Source != ENSource.Accounting)
				{
					return ret;
				}
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال عملیات چک های شخصی".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (inputToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 258);
				}
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!".GetTranslate(), "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 259);
				}
				CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, cache.BufferCancellationToken).Token;
				if (cache.CacheRepository.IViewCheckShRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IViewCheckSh is null");
				}
				List<Guid> list = (await cache.CacheRepository.IViewCheckShRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 267);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("چک های شخصی سینک هستند تعداد چک شخصی سینک شده".GetTranslate() + $":{list?.Count ?? 0}", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 268);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 269);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				_ = (decimal)(list?.Count ?? 0);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 277);
						}
						ViewCheckShBusiness obj = await GetAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "شماره".GetTranslate() + ": " + obj?.Number + " " + " دریافت کننده".GetTranslate() + ": " + obj?.CustomerFullName;
							uiUpdater.Count = current;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: ViewCheckSh \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 287);
							continue;
						}
						ChequeShOperationFilter filter = new ChequeShOperationFilter
						{
							ChequeGuid = obj.Guid
						};
						IEnumerable<ChequeShOperationBusiness> enumerable = await ChequeShOperationBusiness.GetAllAsync(contextGuid, filter, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							obj.OperationList = enumerable?.ToList();
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 298);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 303);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 307);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 309);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 312);
				ret.AddReturnedValue(ex, "", "ResendNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 313);
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

	public static Task<IEnumerable<ChequeSHSummaryViewModel>> GetSummaryAsync(string contextGuid, DateTime? date1 = null, DateTime? date2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository.GetSummaryAsync(date1, date2, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ViewCheckSh/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 346);
				}
				List<WebViewCheckSh> list = new List<WebViewCheckSh>();
				List<WebViewCheckSh> list2 = list;
				list2.Add(await ViewCheckShMapper.Instance.MapAsync(ContextGuid, this, new WebViewCheckSh(), "", token));
				List<Packet> lstPacket = WebViewCheckSh.GenerateSaveRequest(list, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 348);
				}
				using ViewCheckShWaiter waiter = new ViewCheckShWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("viewCheckShBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 352);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 354);
				}
				if (!token.IsCancellationRequested)
				{
					ret.throwExceptionIfError();
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 358);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 361);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 362);
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IViewCheckShRepository;
	}

	public static async Task<ReturnedSaveFuncInfo> ForceUpdateChecqueStatusAsync(string contextGuid, Guid chequeGuid, ENCheckSHStatus status, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore: true, "ViewCheckShBusiness/ForceUpdateChecqueStatusAsync", 60000, null))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 376);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 377), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 377);
						}
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							IViewCheckShRepository iViewCheckShRepository = ClsCache.GetCache(contextGuid).CacheRepository.IViewCheckShRepository;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await iViewCheckShRepository.ForceUpdateChecqueStatusAsync(chequeGuid, status, token, tranName), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 381);
							goto end_IL_00f8;
						}
						result = ret;
					}
					goto end_IL_00da;
					end_IL_00f8:;
				}
				goto end_IL_0065;
				end_IL_00da:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 383);
				goto end_IL_0065;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 386);
				ret.AddReturnedValue(ex, "", "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 387);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 392), ReturnedState.None, "ForceUpdateChecqueStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\ViewCheckShBusiness.cs", 392);
			}
		}
		return ret;
	}
}
