using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters.Customer;
using PacketParser.TransferLayer.Waiters.Accounting.Product;

namespace EntityCache.Business;

public class VisitorAreaBusiness : BaseBusiness<VisitorAreaBusiness>, IVisitorArea, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasChangeStatus, IHasContext, INotifyPropertyChanged, ISyncFromClient2Server<VisitorAreaBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore_get = new SemaphoreSlim(1, 20);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private CancellationTokenSource FullMasirDisplay;

	private VisitorAreaBusiness _parent;

	private string _fullArea = "";

	public Guid Guid { get; set; }

	public short Level { get; set; }

	public string Name { get; set; }

	public Guid? ParentGuid { get; set; }

	public string Description { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public long Code { get; set; }

	public long FullCode { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public string FullArea
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_fullArea))
				{
					return _fullArea;
				}
				_fullArea = Name ?? "";
				FullMasirDisplay?.Cancel();
				FullMasirDisplay = new CancellationTokenSource(60000);
				CancellationToken token = FullMasirDisplay.Token;
				CreateFullAreaAsync(token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FullArea", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 61);
			}
			return _fullArea;
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public event PropertyChangedEventHandler PropertyChanged;

	public VisitorAreaBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private async Task CreateFullAreaAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			if ((ParentGuid ?? Guid.Empty) == Guid.Empty)
			{
				return;
			}
			await Task.Delay(new Random().Next(500));
			if (token.IsCancellationRequested || !ParentGuid.HasValue)
			{
				return;
			}
			_parent = await GetAsync(ContextGuid, ParentGuid.Value, token);
			if (_parent == null)
			{
				return;
			}
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					try
					{
						this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullArea"));
					}
					catch (Exception ex4)
					{
						WebErrorLog.Instance.StartLog(ex4, "", "CreateFullAreaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 94);
					}
				}, "CreateFullAreaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs");
			}
			_parent.PropertyChanged -= _parent_PropertyChanged;
			_parent.PropertyChanged += _parent_PropertyChanged;
			_fullArea = _parent.FullArea + "/" + Name;
		}
		catch (OperationCanceledException)
		{
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "CreateFullAreaAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 105);
			throw;
		}
	}

	private void _parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		try
		{
			if (e.PropertyName == "FullArea")
			{
				_fullArea = _parent.FullArea + "/" + Name;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_parent_PropertyChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 116);
		}
	}

	public static async Task<IEnumerable<VisitorAreaBusiness>> GetAllAsync(string contextGuid, bool? status, Guid? parentGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _databaseSemaphore_get.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.VistorArea.GetAllAsync(parentGuid, status, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 129);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_databaseSemaphore_get.Release();
			}
		}
	}

	public static async Task<VisitorAreaBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await _databaseSemaphore_get.WaitAsync();
			}
			semaphoreHasBeenTaken = true;
			return await (ClsCache.GetCache(contextGuid) ?? throw new NullReferenceException("cache is null!!")).CacheRepository.VistorArea.GetAsync(guid, token, tranName);
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName) && semaphoreHasBeenTaken)
			{
				_databaseSemaphore_get.Release();
			}
		}
	}

	public static Task<VisitorAreaBusiness> GetAsync(string contextGuid, long code, Guid? parentGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.VistorArea.GetAsync(code, parentGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, short autoFixValidationErrorsCount, bool send2Server, Priority priority = Priority.Low, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", FileInfoBusiness fileInfo = null, bool awaitOnSend = false)
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
				send2Server = send2Server && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "VisitorAreaBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 169);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 170), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 170);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(autoFixValidationErrorsCount, system, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 172);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.VistorArea.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 174);
							goto end_IL_019c;
						}
						result = ret;
					}
					goto end_IL_00d2;
					end_IL_019c:;
				}
				goto end_IL_00be;
				end_IL_00d2:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 178);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 179);
				goto end_IL_00be;
			}
			return result;
			end_IL_00be:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 184), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 184);
			}
			if (!ret.HasError && send2Server)
			{
				Task.Run(() => Send2ServerAsync(token, system, priority));
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(short autoFixValidationErrorsCount, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.Transfer)
			{
				if (Code <= 0)
				{
					Code = await GetNextCodeAsync(ContextGuid, Level, token, tranName);
				}
				if (FullCode <= 0 && (ParentGuid ?? Guid.Empty) == Guid.Empty)
				{
					FullCode = Code;
				}
				else if ((ParentGuid ?? Guid.Empty) != Guid.Empty)
				{
					VisitorAreaBusiness visitorAreaBusiness = await GetAsync(ContextGuid, ParentGuid.Value, token, tranName);
					if (visitorAreaBusiness != null)
					{
						FullCode = $"{visitorAreaBusiness.Code}{Code}".ParseToLong(0L);
					}
				}
			}
			if (string.IsNullOrEmpty(Name?.Trim()) && system == EnSystem.UserInterface)
			{
				ret.AddError(system, "وارد کردن عنوان مسیر الزامی است".GetTranslate(), "Name", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 206);
			}
			if (system == EnSystem.UserInterface && Code <= 0 && Status)
			{
				ret.AddError(system, "کد مسیر نامشخص است ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 208);
			}
			if (system == EnSystem.UserInterface && (Code > 999 || Code < 100) && Status)
			{
				ret.AddError(system, "کد مسیر حتما باید سه رقمی باشد ".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 209);
			}
			if (system == EnSystem.UserInterface && ChangeStatus == EntityChangeSt.New)
			{
				if ((await GetAllAsync(ContextGuid, true, null, token)).FirstOrDefault((VisitorAreaBusiness p) => p.FullCode == FullCode) != null)
				{
					ret.AddError(system, "کد مسیر تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 213);
				}
			}
			Guid guid = Guid;
			Guid? parentGuid = ParentGuid;
			if (guid == parentGuid)
			{
				if (autoFixValidationErrorsCount > 0)
				{
					ParentGuid = null;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					VisitorAreaBusiness visitorAreaBusiness2 = this;
					short num = checked((short)(autoFixValidationErrorsCount - 1));
					autoFixValidationErrorsCount = num;
					returnedSaveFuncInfo.AddReturnedValue(await visitorAreaBusiness2.CheckValidationAsync(num, system, token), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 220);
					return ret;
				}
				ret.AddError(system, "یک مسیر نمیتواند والد خود باشد. لطفا والد را اصلاح کنید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 223);
			}
			if (!Status && system == EnSystem.UserInterface)
			{
				CustomerCompactFilter filter = new CustomerCompactFilter
				{
					AreaGuidList = new List<Guid> { Guid }
				};
				IEnumerable<CusCompactBusiness> enumerable = await CusCompactBusiness.GetAllAsync(ContextGuid, filter, system, token, tranName);
				if (enumerable != null && enumerable.Any())
				{
					await ret.AddErrorAsync("مسیر".GetTranslate() + " " + FullArea + " به دلیل اختصاص به".GetTranslate() + $" {enumerable.Count()}" + " شخص، قابل حذف نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 230);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 235);
			ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 236);
		}
		finally
		{
			if (ret.HasError && system != EnSystem.UserInterface)
			{
				WebErrorLog.Instance.StartLog("ProductGroup Validation Has Error:" + ret.GetErrorMessage(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 241);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<VisitorAreaBusiness> lst, short autoFixValidationErrorsCount, bool send2Server, short tryCount = 10, string tranName = "", Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (VisitorAreaBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 256);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, autoFixValidationErrorsCount, send2Server, Priority.Low, awaitOnSemaphore: true, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 257);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(lst.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 269);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 270);
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

	public static VisitorAreaBusiness SetFactory(string contextGuid)
	{
		try
		{
			return new VisitorAreaBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Level = 0,
				Name = "",
				Description = "",
				Code = 0L,
				FullCode = 0L,
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = contextGuid,
				ParentGuid = null
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFactory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 303);
			throw;
		}
	}

	public static Task<long> GetNextCodeAsync(string contextGuid, short? level, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.VistorArea.GetNextCodeAsync(level, token, tranName);
	}

	public async Task<bool> HasChildrenAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetChildrenAsync(token, tranName))?.Any() ?? false;
	}

	public Task<IEnumerable<VisitorAreaBusiness>> GetChildrenAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.VistorArea.GetChildrenAsync(Guid, token, tranName);
	}

	public async Task<IEnumerable<VisitorAreaBusiness>> GetAllChildsAndSelfAsync(string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<VisitorAreaBusiness> list = (await GetChildrenAsync(token, tranName))?.ToList();
			if (!(list?.Any() ?? false))
			{
				list = new List<VisitorAreaBusiness>();
			}
			else
			{
				for (int count = 0; count != list.Count(); count = checked(count + 1))
				{
					if (await list[count].HasChildrenAsync(token, tranName))
					{
						List<VisitorAreaBusiness> list2 = list;
						list2.AddRange(await list[count].GetChildrenAsync(token, tranName));
					}
				}
			}
			list.Add(this);
			return list.Where((VisitorAreaBusiness w) => w.Status)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllChildsAndSelfAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 332);
			throw;
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
				using (await _databaseSemaphore.GaurdAsync(true, "VisitorAreaBusiness/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 346), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 346);
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.VistorArea.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 348);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 350);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 353);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 354);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 359), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 359);
			}
		}
		return ret;
	}

	public static Task<VisitorAreaBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.VistorArea.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<VisitorAreaBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 374);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 376);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 377);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.VistorArea == null)
				{
					_ = ClsCache.GetCache(contextGuid).CacheRepository.VistorArea;
					throw new NullReferenceException("repo");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.VistorArea.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 385);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("مسیرها سینک هستند تعداد سینک شده".GetTranslate() + ":" + $"{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 386);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 387);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 394);
						}
						current = checked(current + 1);
						VisitorAreaBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 397);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: VisitorAreaBusiness \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityDesc:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 400);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 406);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 411);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 415);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 416);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 420);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 421);
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.VistorArea;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "VisitorAreaBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 436);
				}
				List<WebEntitySync> list = new List<WebEntitySync>();
				List<WebEntitySync> list2 = list;
				list2.Add(await ToWebItemAsync());
				List<Packet> lstPacket = WebEntitySync.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 438);
				}
				using EntitySyncWaiter waiter = new EntitySyncWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("EntitySyncWaiter"), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 441);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 443);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 446);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 447);
			}
			return ret;
		}
	}

	public async Task<WebEntitySync> ToWebItemAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs"))
		{
			try
			{
				return await VisitorAreaMapper.Instance.MapAsync(ContextGuid, this, new WebEntitySync(), "", token);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToWebItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 461);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<VisitorAreaBusiness> enumerable = (await GetAllAsync(contextGuid, true, null, token, tranName))?.Where((VisitorAreaBusiness q) => (q?.ParentGuid ?? Guid.Empty) != Guid.Empty);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			List<VisitorAreaBusiness> tempListForRemove = new List<VisitorAreaBusiness>();
			foreach (VisitorAreaBusiness item in enumerable)
			{
				if (await GetAsync(contextGuid, item.ParentGuid.Value, token, tranName) == null)
				{
					tempListForRemove.Add(item);
				}
			}
			if (!(tempListForRemove?.Any() ?? false))
			{
				return res;
			}
			foreach (VisitorAreaBusiness item2 in tempListForRemove)
			{
				item2.Status = false;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(system, token, 0, send2Server: true, Priority.Low, awaitOnSemaphore: true, 10), ReturnedState.None, "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 484);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 489);
			res.AddReturnedValue(ex, "", "FixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\VisitorAreaBusiness.cs", 490);
		}
		return res;
	}
}
