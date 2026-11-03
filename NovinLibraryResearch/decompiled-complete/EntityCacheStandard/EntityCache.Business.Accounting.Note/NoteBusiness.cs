using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.Note;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Note;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.TransferLayer.Waiters.Accounting.Note;

namespace EntityCache.Business.Accounting.Note;

public class NoteBusiness : BaseBusiness<NoteBusiness>, INote, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<NoteBusiness>, IDeliverableEntity, IEditableDelivery, INotifyPropertyChanged
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _getCashierNameToken;

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private string _cashierName;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	bool IHasStatus.Status => Status == NoteStatus.Deleted;

	public string Title { get; set; }

	public string Text { get; set; }

	public DateTime DateM { get; set; }

	public DateTime? DateSarResidM { get; set; }

	public NotePirority Pirority { get; set; }

	public NoteStatus Status { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid? EntityGuid { get; set; }

	public Document? EntityType { get; set; }

	public string PirorityName => Pirority.GetDisplay().GetTranslate();

	public string StatusName => Status.GetDisplay().GetTranslate();

	public NoteDisplayState DisplayState { get; set; }

	public Guid? TafsilGuid { get; set; }

	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	[JsonIgnore]
	public string InsertTime => DateM.ToLongTimeString();

	[JsonIgnore]
	public string DateSarResidSh => DateSarResidM.MiladiToCalture();

	[JsonIgnore]
	public string TimeSarResid
	{
		get
		{
			if (DateSarResidM.HasValue)
			{
				return DateSarResidM.Value.ToLongTimeString();
			}
			return "";
		}
	}

	[JsonIgnore]
	public string CashierName
	{
		get
		{
			try
			{
				if ((CashierGuid ?? Guid.Empty) == Guid.Empty)
				{
					return "";
				}
				if (!string.IsNullOrEmpty(_cashierName))
				{
					return _cashierName;
				}
				_getCashierNameToken?.Cancel();
				_getCashierNameToken = new CancellationTokenSource(60000);
				CancellationToken token = _getCashierNameToken.Token;
				Task.Run(() => GetCashierNameAsync(token));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CashierName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 70);
			}
			return _cashierName;
		}
	}

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public string GroupName { get; set; }

	public DateTime? DateUtc { get; set; }

	public NoteMetaData MetaData { get; set; }

	[JsonIgnore]
	public bool ShowForMySelf => (DisplayState & NoteDisplayState.MySelf) == NoteDisplayState.MySelf;

	[JsonIgnore]
	public bool ShowForAllUsers => (DisplayState & NoteDisplayState.User) == NoteDisplayState.User;

	public event PropertyChangedEventHandler PropertyChanged;

	public NoteBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<NoteBusiness>> GetAllAsync(string contextGuid, EnSystem system, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, system, await ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.GetAllAsync(status, token, tranName), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 100);
			throw;
		}
	}

	public static async Task<decimal> GetCountNote(string contextGuid, EnSystem system, NoteFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return (await GetAllAsync(contextGuid, system, filter, token, tranName))?.Count() ?? 0;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCountNote", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 113);
			throw;
		}
	}

	public async Task GetCashierNameAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(100, 500));
			if (token.IsCancellationRequested || base.IsDisposed)
			{
				return;
			}
			_cashierName = (await CashierBusiness.GetAsync(ContextGuid, CashierGuid.Value, token)).Name;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CashierName"));
				}, "GetCashierNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 131);
		}
	}

	public static async Task<IEnumerable<NoteBusiness>> GetAllAsync(string contextGuid, EnSystem system, Guid groupGuid, NoteStatus? status = NoteStatus.All, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, system, await ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.GetAllAsync(groupGuid, status, token, tranName), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 142);
			throw;
		}
	}

	public static async Task<IEnumerable<NoteBusiness>> GetAllAsync(string contextGuid, EnSystem system, NoteFilter filter, CancellationToken token, string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, system, await ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.GetAllAsync(token, filter, tranName), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 155);
			throw;
		}
	}

	public static Task<NoteBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.GetAsync(guid, token, tranName);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.NoteRepository;
	}

	public static Task<NoteBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<NoteBusiness>(null);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool send2Server, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false, Priority priority = Priority.Low, short tryCount = 10, string tranName = "")
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
			_ = 6;
			ReturnedSaveFuncInfo result;
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "NoteBusiness/SaveAsync", 60000, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (dis.Token.IsCancellationRequested)
				{
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 174);
				}
				else
				{
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 175), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 175);
					}
					transactionHasBeenCreated = true;
					ServerStatus = ServerStatusForClient.None;
					if (!(GroupGuid == Guid.Empty) || ClsCache.GetCache(ContextGuid).Source != ENSource.Android_HesabYar)
					{
						goto IL_04b7;
					}
					IEnumerable<NoteGroupBusiness> enumerable = await NoteGroupBusiness.GetAllAsync(ContextGuid, token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						GroupGuid = enumerable.First().Guid;
						goto IL_04b7;
					}
					NoteGroupBusiness noteGroup = new NoteGroupBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						Guid = Guid.NewGuid(),
						Modified = DateTime.Now,
						Name = "درج شده از طریق حسابیار",
						ParentGuid = Guid.Empty,
						Status = true
					};
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await noteGroup.SaveAsync(token, awaitOnSemaphore, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 194);
					if (!ret.HasError)
					{
						GroupGuid = noteGroup.Guid;
						goto IL_04b7;
					}
					result = ret;
				}
				goto end_IL_01a0;
				IL_06b3:
				if (ret.HasError)
				{
					result = ret;
					goto end_IL_01a0;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.NoteRepository.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 219);
				goto end_IL_00da;
				IL_04b7:
				if (!(GroupGuid != Guid.Empty) || system != EnSystem.SignalR || ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					goto IL_06b3;
				}
				NoteGroupBusiness noteGroupBusiness = await NoteGroupBusiness.CheckNameAsync(ContextGuid, GroupGuid, GroupName, token, tranName);
				if (noteGroupBusiness != null)
				{
					if (noteGroupBusiness.Guid != GroupGuid)
					{
						GroupGuid = noteGroupBusiness.Guid;
					}
					goto IL_06b3;
				}
				noteGroupBusiness = new NoteGroupBusiness(ContextGuid)
				{
					ContextGuid = ContextGuid,
					Guid = GroupGuid,
					Modified = DateTime.Now,
					Name = GroupName,
					ParentGuid = Guid.Empty,
					Status = true
				};
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await noteGroupBusiness.SaveAsync(token, awaitOnSemaphore: true, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 213);
				if (!ret.HasError)
				{
					goto IL_06b3;
				}
				result = ret;
				end_IL_01a0:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 223);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 224);
				goto end_IL_00da;
			}
			return result;
			end_IL_00da:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 229), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 229);
			}
			if (!ret.HasError && send2Server && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 233);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!ret.HasError)
			{
				ClsCache.GetCache(ContextGuid).BusinessAggregator.Events.Note.RaiseNoteSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 253), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 253);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.NoteRepository.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 255);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 257);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 260);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 261);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 266), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 266);
			}
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, bool sendToServer, CancellationToken token, IEnumerable<NoteBusiness> lst)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (lst == null || !lst.Any())
			{
				return ret;
			}
			foreach (NoteBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 279);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, token, awaitOnSemaphore: true, await4Send: false, Priority.Low, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 280);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 285);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 286);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, Guid guid, NoteStatus status, bool sendToServer = true, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				CancellationToken token = default(CancellationToken);
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "NoteBusiness/ChangeStatusAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 301);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 302), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 302);
						}
						transactionHasBeenCreated = true;
						NoteBusiness noteBusiness = await GetAsync(contextGuid, guid, token, tranName);
						if (noteBusiness != null)
						{
							noteBusiness.Status = status;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await noteBusiness.SaveAsync(system, sendToServer, token, awaitOnSemaphore: false, await4Send: false, Priority.Low, tryCount, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 312);
							goto end_IL_0126;
						}
						ret.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 307);
						result = ret;
					}
					goto end_IL_0076;
					end_IL_0126:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 316);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 317);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 322), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 322);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, List<NoteBusiness> lst, NoteStatus status, bool sendToServer = true, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				CancellationToken token = default(CancellationToken);
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "NoteBusiness/ChangeStatusAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 338), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 338);
						}
						transactionHasBeenCreated = true;
						foreach (NoteBusiness item in lst)
						{
							NoteBusiness noteBusiness = await GetAsync(contextGuid, item.Guid, token, tranName);
							if (noteBusiness == null)
							{
								ret.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 345);
								continue;
							}
							noteBusiness.Status = status;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await noteBusiness.SaveAsync(system, sendToServer, token, awaitOnSemaphore: false, await4Send: false, Priority.Low, tryCount, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 350);
						}
						goto end_IL_0120;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 337);
					goto end_IL_0076;
					end_IL_0120:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 355);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 356);
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
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 361), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 361);
			}
		}
		return ret;
	}

	public static Task<int> CountNumbersInGroupAsync(string contextGuid, Guid groupGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.CountNumbersInGroupAsync(groupGuid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, EnSystem.None, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 10)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool semaphoreHasBeenTaken = false;
			try
			{
				if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
				{
					return ret;
				}
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 378);
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
					return ret.AddError("کش آنلاین نیست!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 379);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 380);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.NoteRepository.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 384);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 392);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 394);
						}
						NoteBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj == null)
						{
							continue;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: NoteRepository \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty} \r\n" + "EntityTitle: " + obj?.Title, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 399);
							continue;
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 410);
							tasks.Remove(t2);
						}
					}
				}
				while (tasks?.Any() ?? false)
				{
					Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 417);
					tasks.Remove(t2);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 420);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 422);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 425);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 426);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs"))
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
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "NoteBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 442);
				}
				List<WebNote> list = new List<WebNote>();
				List<WebNote> list2 = list;
				list2.Add(await NoteMapper.Instance.MapAsync(ContextGuid, this, new WebNote(), "", token));
				List<Packet> paks = WebNote.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 444);
				}
				using NoteWaiter waiter = new NoteWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("NoteWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 448);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 450);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 454);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 455);
			}
			return ret;
		}
	}

	public static Task<NoteBusiness> GenerateNewAsync(string contextGuid, Guid? asGuid = null, Document? asType = null, Guid? tafsilGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			NoteBusiness noteBusiness = new NoteBusiness(contextGuid)
			{
				ContextGuid = contextGuid,
				Guid = Guid.NewGuid(),
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				ChangeStatus = EntityChangeSt.New,
				Pirority = NotePirority.Mamoli,
				DisplayState = (NoteDisplayState)3
			};
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				noteBusiness.TafsilGuid = tafsilGuid.Value;
			}
			if ((asGuid ?? Guid.Empty) != Guid.Empty)
			{
				noteBusiness.EntityGuid = asGuid.Value;
			}
			if (asType.GetValueOrDefault() != 0)
			{
				noteBusiness.EntityType = asType.Value;
			}
			return Task.FromResult(noteBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 482);
			throw;
		}
	}

	private static Task<IEnumerable<NoteBusiness>> ApplyAccessAsync(string contextGuid, EnSystem system, IEnumerable<NoteBusiness> items, CancellationToken token, string tranName = "")
	{
		try
		{
			if (system != EnSystem.UserInterface)
			{
				return Task.FromResult(items);
			}
			Guid cashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
			items = items?.Where((NoteBusiness q) => q.ShowForAllUsers || (q.ShowForMySelf && q.CashierGuid == cashierGuid));
			items = items?.ToList();
			return Task.FromResult(items);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Note\\NoteBusiness.cs", 499);
			throw;
		}
	}
}
