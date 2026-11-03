using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Attendance;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Attendance.AttpersonLeave;

namespace EntityCache.Business.Attendance;

public class AttPersonLeaveBusiness : BaseBusiness<AttPersonLeaveBusiness>, IHasContext, IAttPersonLeave, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<AttPersonLeaveBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public Guid PersonGuid { get; set; }

	public string FullName { get; set; } = "";


	public DateTime RequestDate { get; set; }

	public string RequestDateSh => RequestDate.MiladiToCalture() + " " + RequestDate.TimeOfDay.ToString().Substring(0, 5);

	public string RequestTimeSh => RequestDate.DateToFormatTime();

	public DateTime DateFrom { get; set; }

	public string DateFromSh => DateFrom.MiladiToCalture();

	public DateTime DateTo { get; set; }

	public string DateToSh => DateTo.MiladiToCalture();

	public DateTime? ManagerDate { get; set; }

	public string ManagerDateSh => ManagerDate.MiladiToCalture() + " " + ManagerDate?.TimeOfDay.ToString().Substring(0, 5);

	public DateTime Modified { get; set; }

	public EnPersonLeaveType Type { get; set; }

	public string TypeDisplay => Type.GetDisplay().GetTranslate();

	public EnPersonLeaveStatus LeaveStatus { get; set; }

	public string StatusDisplay => LeaveStatus.GetDisplay().GetTranslate();

	public EnPersonLeaveKind Kind { get; set; }

	public string KindDisplay => Kind.GetDisplay().GetTranslate();

	public string Reason { get; set; }

	public string ManagerMessage { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	bool IHasStatus.Status
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AttDatabaseManager;

	public int DayCount
	{
		get
		{
			if (Type == EnPersonLeaveType.Hourly)
			{
				return 0;
			}
			return checked((int)(DateTo - DateFrom).TotalDays + 1);
		}
	}

	public EnPersonLeaveQuiddity Quiddity { get; set; }

	public string QuiddityForDisplay => Quiddity.GetDisplay().GetTranslate();

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public AttPersonLeaveBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxparalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 87);
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 89);
				}
				line = 50;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 91);
				}
				line = 51;
				if (ClsCache.GetCache(contextGuid).CacheRepository.IAttPersonLeave == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IAttPersonLeave is null");
				}
				line = 52;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IAttPersonLeave.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (list == null || !list.Any())
				{
					return ret.AddInformation($"حساب های کل سینک هستند. تعداد حساب های سینک شده:{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 98);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 99);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 106);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 108);
						}
						AttPersonLeaveBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj.ServerStatus != ServerStatusForClient.Delivered && obj.ServerStatus != ServerStatusForClient.DirectDelivery)
						{
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 113);
							}
							tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
							if (tasks.Count >= maxparalelTask)
							{
								Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 118);
								tasks.Remove(t);
							}
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 122);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 124);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"CacheIsNullOrdisposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}\r\n" + "ContextxGuid:" + contextGuid, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 127);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 130);
			}
			finally
			{
				if (semaphoreHasBeenTaken)
				{
					_resendToServerSemaphore.Release();
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs"))
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
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "AttPersonLeaveBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 150);
				}
				List<WebAttPersonLeave> list = new List<WebAttPersonLeave>();
				List<WebAttPersonLeave> list2 = list;
				list2.Add(await AttPersonLeaveMapper.Instance.MapAsync(ContextGuid, this, new WebAttPersonLeave(), "", token));
				List<Packet> paks = WebAttPersonLeave.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 152);
				}
				using AttPersonLeaveWaiters waiter = new AttPersonLeaveWaiters(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token, 60000, "AttPersonLeaveBusiness.Send2ServerAsync");
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("AttPersonLeaveBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 156);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 158);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 160);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 163);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 164);
			}
			return ret;
		}
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 176), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 176);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IAttPersonLeave.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 178);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 180);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 183);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 184);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 189), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 189);
			}
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
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "AttPersonLeaveBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 202);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 203), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 203);
						}
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							if (system != EnSystem.SignalR)
							{
								ServerStatus = ServerStatusForClient.None;
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IAttPersonLeave.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 207);
							goto end_IL_017e;
						}
						result = ret;
					}
					goto end_IL_00db;
					end_IL_017e:;
				}
				goto end_IL_00ca;
				end_IL_00db:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 211);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 212);
				goto end_IL_00ca;
			}
			return result;
			end_IL_00ca:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 217), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 217);
			}
			if (!ret.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 220);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!ret.HasError)
			{
				ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.PersonLeave.RaiseLeaveSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	public static async Task<IEnumerable<AttPersonLeaveBusiness>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IAttPersonLeave.GetAllAsync(token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IAttPersonLeave;
	}

	public static Task<AttPersonLeaveBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IAttPersonLeave.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<AttPersonLeaveBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, IEnumerable<AttPersonLeaveBusiness> lst, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (AttPersonLeaveBusiness item in lst)
			{
				if (system == EnSystem.SignalR)
				{
					item.ServerStatus = ServerStatusForClient.Delivered;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, priority, token, awaitOnSemaphore, await4Send: false, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 244);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 249);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttPersonLeaveBusiness.cs", 250);
		}
		return ret;
	}
}
