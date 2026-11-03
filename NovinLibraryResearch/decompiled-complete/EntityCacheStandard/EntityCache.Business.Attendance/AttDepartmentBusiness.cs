using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Attendance;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance;
using PacketParser.Entities.MetaData.Attendance;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Attendance.AttpersonLeave;

namespace EntityCache.Business.Attendance;

public class AttDepartmentBusiness : BaseBusiness<AttDepartmentBusiness>, IAttDepartment, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<AttDepartmentBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public string Name { get; set; }

	public DateTime Modified { get; set; }

	public Guid? AdminGuid { get; set; }

	public DateTime CreatedDate { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public AttDepartmentMetaData MetaData { get; set; }

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AttDatabaseManager;

	public bool IsChecked { get; set; }

	public AttDepartmentBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
		Modified = DateTime.Now;
		Status = true;
		CreatedDate = DateTime.Now;
	}

	public static Task<IEnumerable<AttDepartmentBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AttDepartment.GetAllAsync(status, token, tranName);
	}

	public static Task<AttDepartmentBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AttDepartment.GetAsync(guid, token, tranName);
	}

	public static Task<AttDepartmentBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.AttDepartment.GetAsync(name, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, bool await4send = false, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "AttDepartmentBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 68);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 69), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 69);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 71);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AttDepartment.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 73);
							goto end_IL_0172;
						}
						result = ret;
					}
					goto end_IL_00ce;
					end_IL_0172:;
				}
				goto end_IL_00bd;
				end_IL_00ce:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 75);
				goto end_IL_00bd;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 78);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 79);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 83), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 83);
			}
			if (!ret.HasError && sendToServer)
			{
				if (await4send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 86);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system));
				}
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Name))
			{
				await res.AddErrorAsync("لطفا نام دپارتمان را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 97);
			}
			if ((AdminGuid ?? Guid.Empty) == Guid.Empty)
			{
				AdminGuid = null;
			}
			if (!string.IsNullOrEmpty(Name))
			{
				AttDepartmentBusiness attDepartmentBusiness = await GetAsync(ContextGuid, Name, token, tranName);
				if (attDepartmentBusiness != null && attDepartmentBusiness.Guid != Guid)
				{
					await res.AddErrorAsync("عنوان".GetTranslate() + " " + (" " + Name).GetTranslate() + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 103);
				}
			}
			if (!Status)
			{
				int num = await AttPersonBusiness.CountInDepartmentAsync(ContextGuid, Guid, token, tranName);
				if (num > 0)
				{
					await res.AddErrorAsync("دپارتمان".GetTranslate() + "  " + Name + " " + " به دلیل داشتن".GetTranslate() + " " + $" {num}" + " " + " پرسنل فعال، قادر به حذف نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 109);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 114);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 115);
		}
		return res;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.AttDepartment;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AttDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 129), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 129);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.AttDepartment.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 131);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 133);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 136);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 137);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 142), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 142);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs"))
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
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "AttDepartmentBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 157);
				}
				List<Packet> paks = WebAttDepartment.GenerateSaveRequest(new global::_003C_003Ez__ReadOnlySingleElementList<WebAttDepartment>(await AttDepartmentMapper.Instance.MapAsync(ContextGuid, this, new WebAttDepartment(), "", token)));
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 159);
				}
				using AttDepartmentWaiter waiter = new AttDepartmentWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token, 60000, "AttDepartmentBusiness.Send2ServerAsync");
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("AttDepartmentWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 163);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 165);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 167);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 170);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 171);
			}
			return ret;
		}
	}

	public static Task<AttDepartmentBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.AttDepartment.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<AttDepartmentBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxparalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs"))
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
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 190);
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
					return ret.AddError(EnSystem.SignalR, "کش آنلاین نیست!!!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 192);
				}
				line = 50;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 194);
				}
				line = 51;
				if (ClsCache.GetCache(contextGuid).CacheRepository.AttDepartment == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.AttDepartment is null");
				}
				line = 52;
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.AttDepartment.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (list == null || !list.Any())
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 202);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 209);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 211);
						}
						AttDepartmentBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj.ServerStatus != ServerStatusForClient.Delivered && obj.ServerStatus != ServerStatusForClient.DirectDelivery)
						{
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 216);
							}
							tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
							if (tasks.Count >= maxparalelTask)
							{
								Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 221);
								tasks.Remove(t);
							}
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 225);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 227);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + $"CacheIsNullOrdisposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}\r\n" + "ContextxGuid:" + contextGuid, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 230);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Attendance\\AttDepartmentBusiness.cs", 233);
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
}
