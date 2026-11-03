using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Core;
using EntityCache.Cache.Repository.Accounting;
using EntityCache.Core;
using EntityCache.Filters;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting;

public class UserAccBusiness : BaseBusiness<UserAccBusiness>, IUserAcc, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<UserAccBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static ConcurrentDictionary<Guid, long> _dicUserAccessCrc = new ConcurrentDictionary<Guid, long>();

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _getUserSemaphore = new SemaphoreSlim(1, 1);

	private static ConcurrentDictionary<string, UserAccBusiness> _userDic = new ConcurrentDictionary<string, UserAccBusiness>();

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private Guid _Guid;

	private AccessLevel _accessLevel;

	public override string ContextGuid { get; set; }

	public Guid Guid
	{
		get
		{
			return _Guid;
		}
		set
		{
			_Guid = value;
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Description { get; set; } = "";


	public string Name { get; set; } = "";


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	private short Level { get; set; }

	public bool AccessChangeSt { get; private set; }

	[Obsolete("remove it/this is database field.", true)]
	public string CryptoAccesslevel { get; set; }

	[Obsolete("Use Cashier Setting", true)]
	public Guid? DefaultCusGroup { get; set; }

	public short ID { get; set; }

	public AccessLevel UserAccess
	{
		get
		{
			return _accessLevel ?? (_accessLevel = new AccessLevel());
		}
		set
		{
			_accessLevel = value;
		}
	}

	public List<Tuple<DateTime, DateTime>> UserTime { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	[JsonIgnore]
	public string SendToServerStatus => ServerSt();

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	[JsonIgnore]
	[XmlIgnore]
	[Obsolete("use Get/SetCusGuid", true)]
	public Guid? CusGuid
	{
		get
		{
			throw new NotImplementedException();
		}
		set
		{
			throw new NotImplementedException();
		}
	}

	public event Func<object, UserAccBusiness, Task> OnUserAccessChanged;

	public UserAccBusiness(string contextGuid, Guid guid)
	{
		ContextGuid = contextGuid;
		Guid = guid;
	}

	public string ServerSt()
	{
		return ServerStatus switch
		{
			ServerStatusForClient.None => "در حال ارسال..", 
			ServerStatusForClient.Sent => ServerStatus.GetDisplay() + "(" + HostRefrenced + ")", 
			ServerStatusForClient.Delivered => ServerStatus.GetDisplay() + "(" + HostRefrenced + ")", 
			ServerStatusForClient.SendError => ServerStatus.GetDisplay(), 
			ServerStatusForClient.DeliveryError => ServerStatus.GetDisplay(), 
			ServerStatusForClient.DirectDelivery => $"{ServerStatus.GetDisplay()}({DeliveryId})", 
			_ => $"خطا در تشخیص{ServerStatus}", 
		};
	}

	private static long GetCrcUserAccess(UserAccBusiness userAcc)
	{
		return Json.ToStringJson(userAcc.UserAccess)?.CalcCrc() ?? 0;
	}

	public static async Task<UserAccBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		UserAccBusiness ret = null;
		try
		{
			ret = await ClsCache.GetCache(contextGuid).CacheRepository.IUserAcc.GetAsync(name, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 87);
		}
		finally
		{
			if (ret != null && !_dicUserAccessCrc.TryGetValue(ret.Guid, out var _))
			{
				_dicUserAccessCrc.TryAdd(ret.Guid, GetCrcUserAccess(ret));
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, short tryFixCount = 0, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
			ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
			try
			{
				if (string.IsNullOrEmpty(Name))
				{
					result.AddError(system, "نام گروه کاربری پر نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 101);
				}
				if (tryFixCount <= 0)
				{
					return result;
				}
				UserAccBusiness userAccBusiness = await GetAsync(ContextGuid, Name, token, tranName);
				if (userAccBusiness == null || userAccBusiness.Guid == Guid)
				{
					return result;
				}
				Name += "_";
				ReturnedSaveFuncInfo returnedSaveFuncInfo = result;
				UserAccBusiness userAccBusiness2 = this;
				short num = checked((short)(tryFixCount - 1));
				tryFixCount = num;
				returnedSaveFuncInfo.AddReturnedValue(await userAccBusiness2.CheckValidationAsync(system, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 107);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 111);
				result.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 112);
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationForRemoveAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			if ((await GetAllAsync(ContextGuid, token)).Count() == 1)
			{
				res.AddError(system, "حداقل یک گروه کاربری باید در سیستم وجود داشته باشد ".GetTranslate(), "", "CheckValidationForRemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 124);
			}
			if (Guid == (cashier?.UserGuid ?? Guid.Empty))
			{
				res.AddError(system, "شما قادر به حذف گروه کاربری ای نیستید که کاربر لاگین شده جاری میباشد".GetTranslate() + "\r\n" + cashier?.Name + " با شناسه کاربری دیگری وارد شده و اقدام به حذف نمایید .".GetTranslate(), "", "CheckValidationForRemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 127);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationForRemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 130);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> Deleted(CancellationToken token, EnSystem system, bool send2Server, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		using (new JaegerMonitor("Deleted", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
			if (string.IsNullOrEmpty(tranName))
			{
				tranName = Guid.NewGuid().ToString();
			}
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "UserAccBusiness/Deleted", 60000, token);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Deleted", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 141);
				}
				Status = false;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(send2Server, system, Priority.Low, awaitOnSemaphore: false, await4Send: false, tryCount, tranName, token, 0), ReturnedState.None, "Deleted", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 143);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Deleted", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 147);
				ret.AddReturnedValue(ex, "", "Deleted", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 148);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeCompactAsync(string contextGuid, IEnumerable<UserAccCompact> list, EnSystem system, CancellationToken token, bool send2Server, Priority priority)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
			{
				return ret;
			}
			line = 1;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SaveRangeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 160);
			}
			List<UserAccBusiness> users = new List<UserAccBusiness>();
			line = 12;
			if (list == null || !list.Any())
			{
				return ret;
			}
			line = 13;
			foreach (UserAccCompact user2 in list)
			{
				line = 14;
				UserAccBusiness obj = await GetAsync(contextGuid, user2.Guid, token);
				line = 15;
				if (obj == null)
				{
					users.Add(new UserAccBusiness(contextGuid, user2.Guid)
					{
						Name = user2.Name,
						ServerStatus = ServerStatusForClient.Delivered
					});
				}
				line = 17;
			}
			line = 18;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await SaveRangeAsync(users, system, send2Server, priority, token), ReturnedState.None, "SaveRangeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 179);
			line = 19;
			List<CashierBusiness> cashiers = new List<CashierBusiness>();
			foreach (UserAccCompact user2 in list)
			{
				foreach (WebCashier cashierCompact in user2?.Cashiers ?? new List<WebCashier>())
				{
					CashierBusiness obj2 = await CashierBusiness.GetAsync(contextGuid, cashierCompact.Guid, token);
					line = 15;
					if (obj2 == null)
					{
						cashiers.Add(new CashierBusiness(contextGuid)
						{
							Guid = cashierCompact.Guid,
							Name = cashierCompact.Name,
							UserName = cashierCompact.UserName,
							HashedPassword = cashierCompact.HashedPassword,
							UserGuid = user2.Guid,
							ServerStatus = ServerStatusForClient.Delivered
						});
					}
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CashierBusiness.SaveRangeAsync(cashiers, send2Server, system, priority, token), ReturnedState.None, "SaveRangeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 201);
			line = 21;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SaveRangeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 206);
			ret.AddReturnedValue(ex, "", "SaveRangeCompactAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 207);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<UserAccBusiness> list, EnSystem system, bool send2Server, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), Action<PercenteCompletedEventArgs> percentCompleted = null)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		CancellationTokenSource invokeToken2 = new CancellationTokenSource();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return ret;
			}
			int current = 0;
			foreach (UserAccBusiness item in list)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 222);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(send2Server, system, priority, awaitOnSemaphore: true, await4Send: false, 10, "", token, 10), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 223);
				if (percentCompleted != null)
				{
					invokeToken2?.Cancel();
					invokeToken2 = new CancellationTokenSource(60000);
					CancellationToken token2 = invokeToken2.Token;
					percentCompleted(new PercenteCompletedEventArgs(list.Count(), current, item.Name, token2));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 235);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 236);
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
					percentCompleted(new PercenteCompletedEventArgs(list?.Count() ?? 0, list?.Count() ?? 0, "", invokeTkn));
				});
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(bool sendToServer, EnSystem system, Priority priority = Priority.Low, bool awaitOnSemaphore = true, bool await4Send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), short tryFixCount = 0)
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		int line = 0;
		bool accessHasChanged = false;
		try
		{
			_ = 8;
			ReturnedSaveFuncInfo result;
			try
			{
				UserAccBusiness oldUser = null;
				long crcUserAccess = GetCrcUserAccess(this);
				if (_dicUserAccessCrc.TryGetValue(Guid, out var value) && value != crcUserAccess)
				{
					accessHasChanged = true;
					_dicUserAccessCrc.TryRemove(Guid, out var _);
					_dicUserAccessCrc.TryAdd(Guid, crcUserAccess);
				}
				sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "UserAccBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 271);
					}
					else
					{
						line = 1;
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran && !string.IsNullOrEmpty(ContextGuid))
						{
							IDatabaseManager accDatabaseManager = ClsCache.GetCache(ContextGuid).AccDatabaseManager;
							if (accDatabaseManager != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await accDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 276), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 276);
							}
							transactionHasBeenCreated = true;
							if (system == EnSystem.UserInterface)
							{
								oldUser = await GetFromDatabaseAsync(ContextGuid, Guid, token, tranName);
							}
						}
						line = 2;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, tryFixCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 282);
						line = 3;
						ret.throwExceptionIfError();
						line = 6;
						IUserAccRepository iUserAcc = ClsCache.GetCache(ContextGuid).CacheRepository.IUserAcc;
						if (iUserAcc != null)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await iUserAcc.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 288);
						}
						line = 7;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (oldUser == null)
							{
								goto end_IL_023c;
							}
							IEnumerable<ProductCompactBusiness> enumerable = null;
							List<Guid> currentGroupGuids = UserAccess?.PrdGroupListGuid;
							List<Guid> oldGroupGuids = oldUser.UserAccess?.PrdGroupListGuid;
							if ((currentGroupGuids?.Any() ?? false) || (oldGroupGuids?.Any() ?? false))
							{
								if (!(currentGroupGuids?.Any() ?? false) && (oldGroupGuids?.Any() ?? false))
								{
									ProductCompactFilter filter = new ProductCompactFilter
									{
										GroupGuids = oldGroupGuids,
										Status = true
									};
									enumerable = await ProductCompactBusiness.GetAllAsync(ContextGuid, system, filter, token, tranName);
								}
								if ((currentGroupGuids?.Any() ?? false) && !(oldGroupGuids?.Any() ?? false))
								{
									ProductCompactFilter filter2 = new ProductCompactFilter
									{
										GroupGuids = currentGroupGuids,
										Status = true
									};
									enumerable = await ProductCompactBusiness.GetAllAsync(ContextGuid, system, filter2, token, tranName);
								}
								if ((currentGroupGuids?.Any() ?? false) && (oldGroupGuids?.Any() ?? false))
								{
									List<Guid> list = new List<Guid>();
									IEnumerable<Guid> enumerable2 = currentGroupGuids.Where((Guid q) => !oldGroupGuids.Select((Guid w) => w).Contains(q));
									IEnumerable<Guid> enumerable3 = oldGroupGuids.Where((Guid q) => !currentGroupGuids.Select((Guid w) => w).Contains(q));
									if (enumerable2 != null && enumerable2.Any())
									{
										list.AddRange(enumerable2);
									}
									if (enumerable3 != null && enumerable3.Any())
									{
										list.AddRange(enumerable3);
									}
									if (list != null && list.Any())
									{
										ProductCompactFilter filter3 = new ProductCompactFilter
										{
											GroupGuids = list,
											Status = true
										};
										enumerable = await ProductCompactBusiness.GetAllAsync(ContextGuid, system, filter3, token, tranName);
									}
								}
								if (enumerable != null && enumerable.Any())
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ProductBusiness.UpdateServerStatusAsync(ContextGuid, enumerable.Select((ProductCompactBusiness q) => q.Guid).ToList(), ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 345);
								}
								goto end_IL_023c;
							}
							result = ret;
						}
					}
					goto end_IL_0108;
					end_IL_023c:;
				}
				goto end_IL_00f4;
				end_IL_0108:;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 348);
				goto end_IL_00f4;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//data:{Json.ToStringJson(this, indented: true)}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 351);
				ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 352);
				goto end_IL_00f4;
			}
			return result;
			end_IL_00f4:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 357), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 357);
			}
			if (sendToServer && !ret.HasError)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 361);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (accessHasChanged)
			{
				await ClsCache.GetCache(ContextGuid).BusinessAggregator.UserAccBusiness.RaiseOnUserAccessChangedAsync(this);
			}
			if (!token.IsCancellationRequested)
			{
				SaveLogAsync(system, ret);
			}
		}
		return ret;
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 1;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebUserAcc webEntity = await UserAccMapper.Instance.MapAsync(ContextGuid, this, new WebUserAcc());
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid);
				cashiersLogBusiness.Action = ENAction.Error;
				cashiersLogBusiness.ApplicationVersion = ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? "";
				cashiersLogBusiness.CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
				cashiersLogBusiness.CashierName = cashierBusiness?.Name;
				cashiersLogBusiness.ContextGuid = ContextGuid;
				cashiersLogBusiness.DateM = DateTime.Now;
				cashiersLogBusiness.DateUtc = DateTime.Now.ToUniversalTime();
				cashiersLogBusiness.Guid = Guid.NewGuid();
				cashiersLogBusiness.System = system;
				cashiersLogBusiness.Source = ClsCache.GetCache(ContextGuid).Source;
				cashiersLogBusiness.SanadType = EnLog.SaveUser;
				cashiersLogBusiness.SanadGuid = Guid;
				cashiersLogBusiness.Title = "خطا در ذخیره گروه کاربری".GetTranslate() + $" ({Name})-{Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 400);
		}
	}

	internal Task RaiseOnUserAccessChangedAsync(UserAccBusiness userAccBusiness, CancellationToken token = default(CancellationToken))
	{
		return PacketParser.Utilities.RaiseEvent(this.OnUserAccessChanged, this, userAccBusiness, token, 60, "RaiseOnUserAccessChangedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs");
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
			_ = 3;
			try
			{
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "UserAccBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 413);
				}
				WebUserAcc webUserAcc = await UserAccMapper.Instance.MapAsync(ContextGuid, this, new WebUserAcc(), "", token);
				WebCashier webCashier = webUserAcc.CashierList?.FirstOrDefault();
				webUserAcc.AccountingVersion = 2;
				webUserAcc.VisitorGuid = webCashier?.VisitorGuid;
				webUserAcc.AllVisitorAccess = webCashier?.AllVisitorAccess ?? false;
				webUserAcc.AllBanksAccess = webCashier?.AllBanksAccess ?? false;
				webUserAcc.AllSandoghsAccess = webCashier?.AllSandoghsAccess ?? false;
				webUserAcc.AllDepotsAccess = webCashier?.AllDepotsAccess ?? false;
				webUserAcc.SandoghGuid = webCashier?.SandoghGuid;
				webUserAcc.CusGuid = webCashier?.CusGuid;
				webUserAcc.DepotGuid = webCashier?.DepotGuid;
				webUserAcc.BankGuid1 = webCashier?.BankGuid1;
				webUserAcc.BankGuid2 = webCashier?.BankGuid2;
				webUserAcc.BankGuid3 = webCashier?.BankGuid3;
				webUserAcc.BankGuid4 = webCashier?.BankGuid4;
				List<Packet> paks = WebUserAcc.GenerateSaveRequest(new List<WebUserAcc> { webUserAcc });
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 433);
				}
				using UserAccWaiter waiter = new UserAccWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("UserAccBusinessWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 437);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 439);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 441);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 444);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 445);
			}
			return ret;
		}
	}

	public static Task<UserAccBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.IUserAcc.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<UserAccBusiness>(null);
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName = "", maxParalelTask = 20);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.User;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال کاربران".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Others;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 467);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "Buffer", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 469);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.IUserAcc == null)
				{
					_ = ClsCache.GetCache(contextGuid).CacheRepository.IUserAcc;
					throw new NullReferenceException("userRepo");
				}
				IUserAccRepository iUserAcc = ClsCache.GetCache(contextGuid).CacheRepository.IUserAcc;
				if (iUserAcc != null)
				{
					List<Guid> list = (await iUserAcc.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
					if (uiUpdater != null)
					{
						uiUpdater.TotalCount = list?.Count ?? 0;
					}
					if (list == null || !list.Any())
					{
						return ret.AddInformation("کاربران سینک هستند تعداد کاربر سینک شده".GetTranslate() + $":{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 481);
					}
					List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 483);
					}
					int current = 0;
					_ = (decimal)(list?.Count ?? 0);
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (list != null && list.Any())
					{
						foreach (Guid item in list)
						{
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 489);
							}
							current = checked(current + 1);
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 491);
							}
							UserAccBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
							if (uiUpdater != null)
							{
								uiUpdater.ShortMessage = "کاربر جاری:" + obj?.Name;
								uiUpdater.Count = current;
							}
							if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
							{
								ret.AddInformation("current status on database is delivered!\r\nEntityType: UserAcc \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityName:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 501);
								continue;
							}
							if (token.IsCancellationRequested)
							{
								return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 507);
							}
							tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
							if (tasks.Count >= maxParalelTask)
							{
								Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 512);
								tasks.Remove(t);
							}
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 516);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 522);
				ret.AddReturnedValue(ex2, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 523);
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

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IUserAcc;
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
				if (autoTran)
				{
					IDatabaseManager accDatabaseManager = ClsCache.GetCache(ContextGuid).AccDatabaseManager;
					if (accDatabaseManager != null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await accDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 554), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 554);
					}
					transactionHasBeenCreated = true;
				}
				IUserAccRepository iUserAcc = ClsCache.GetCache(ContextGuid).CacheRepository.IUserAcc;
				if (iUserAcc != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await iUserAcc.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 559);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 563);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 564);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 569), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 569);
			}
		}
		return ret;
	}

	private void ParseLevel(short v)
	{
		using (new JaegerMonitor("ParseLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
			try
			{
				new List<UserLevel>
				{
					UserLevel.Baje,
					UserLevel.Hesabdari,
					UserLevel.Android
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ParseLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 585);
			}
		}
	}

	public static async Task<IEnumerable<UserAccBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			AggregateSqlRepository cacheRepository = ClsCache.GetCache(contextGuid).CacheRepository;
			if (cacheRepository.IUserAcc != null)
			{
				IEnumerable<UserAccBusiness> enumerable = await cacheRepository.IUserAcc.GetAllAsync(status, token, tranName);
				if (status.HasValue)
				{
					enumerable = enumerable?.Where((UserAccBusiness q) => q.Status == status);
				}
				return enumerable;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 599);
		}
		return new List<UserAccBusiness>();
	}

	public static async Task<IEnumerable<UserAccBusiness>> GetAllAsync(string contextGuid, CancellationToken token, string search = "", bool status = true, string tranName = "")
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				IEnumerable<UserAccBusiness> enumerable = await GetAllAsync(contextGuid, status, token);
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (!string.IsNullOrEmpty(item) && item.Trim() != "")
						{
							enumerable = enumerable.Where((UserAccBusiness x) => x.Name.StringComparisons(item) || x.Description.StringComparisons(item));
						}
					}
				}
				enumerable = enumerable?.OrderBy((UserAccBusiness o) => o.Name)?.ToList();
				return token.IsCancellationRequested ? null : enumerable?.ToList();
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 622);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(string contextGuid, Guid guid, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
				if (autoTran)
				{
					IDatabaseManager accDatabaseManager = ClsCache.GetCache(contextGuid).AccDatabaseManager;
					if (accDatabaseManager != null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await accDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 638), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 638);
					}
					transactionHasBeenCreated = true;
				}
				IUserAccRepository iUserAcc = ClsCache.GetCache(contextGuid).CacheRepository.IUserAcc;
				if (iUserAcc != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await iUserAcc.RemoveAsync(guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 643);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 647);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 648);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 653), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 653);
			}
		}
		return ret;
	}

	public static async Task<bool> CheckInfo(Guid guid, string pass)
	{
		return true;
	}

	public static short GenerateLevel(IEnumerable<UserLevel> access)
	{
		checked
		{
			using (new JaegerMonitor("GenerateLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
			{
				try
				{
					short num = 0;
					if (access != null && access.Any())
					{
						foreach (UserLevel item in access)
						{
							num = (short)unchecked((int)num | (int)item);
						}
					}
					return num;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GenerateLevel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 695);
					throw;
				}
			}
		}
	}

	public void ADDVisitor(Guid guid, int vId)
	{
		using (new JaegerMonitor("ADDVisitor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs"))
		{
		}
	}

	public static async Task<UserAccBusiness> GetAsync(string contextGuid, Guid itemGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			UserAccBusiness ret = null;
			await _getUserSemaphore.WaitAsync();
			IUserAccRepository userAccRepository = ClsCache.GetCache(contextGuid)?.CacheRepository.IUserAcc;
			if (userAccRepository != null)
			{
				ret = await userAccRepository.GetAsync(itemGuid, token, tranName);
			}
			if (ret != null && !_dicUserAccessCrc.TryGetValue(ret.Guid, out var _))
			{
				_dicUserAccessCrc.TryAdd(ret.Guid, GetCrcUserAccess(ret));
			}
			return ret;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\UserAccBusiness.cs", 724);
			throw;
		}
		finally
		{
			_getUserSemaphore.Release();
		}
	}
}
