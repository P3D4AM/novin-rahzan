using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Core;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Customer;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Customer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Customers;

public class CustomerRoleBusiness : BaseBusiness<CustomerRoleBusiness>, ICustomerRole, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<CustomerRoleBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string WordPressName { get; set; }

	public string Description { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Dictionary<string, string> MetaData { get; set; }

	public override string ContextGuid { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public CustomerRoleBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<CustomerRoleBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole.GetAsync(guid, token, tranName);
	}

	public static Task<CustomerRoleBusiness> GetAsync(string contextGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole.GetAsync(name, token, tranName);
	}

	public static Task<CustomerRoleBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<CustomerRoleBusiness>(null);
	}

	public static Task<IEnumerable<CustomerRoleBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole.GetAllAsync(status, token, tranName);
	}

	public static async Task<IEnumerable<CustomerRoleModel>> GetAllModelAsync(string contextGuid, bool isLoadDefaultData, bool? status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<CustomerRoleModel> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole.GetAllModelAsync(status, token, tranName);
			if (!isLoadDefaultData || enumerable == null || !enumerable.Any())
			{
				return enumerable;
			}
			List<CustomerRoleModel> list = enumerable.ToList();
			list.Add(new CustomerRoleModel
			{
				Guid = Guid.Empty,
				Name = "<< فاقد نقش >>".GetTranslate()
			});
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 77);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool send2Server, bool await4Send = false, Priority priority = Priority.Low, bool awaitOnSemaphore = true, string tranName = "", bool awaitOnSend = false)
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "CustomerRoleBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 91);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 92), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 92);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 94);
						if (!ret.HasError)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CustomerRole.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 96);
							goto end_IL_01ac;
						}
						result = ret;
					}
					goto end_IL_00e2;
					end_IL_01ac:;
				}
				goto end_IL_00ce;
				end_IL_00e2:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 100);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 101);
				goto end_IL_00ce;
			}
			return result;
			end_IL_00ce:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 106), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 106);
			}
			if (!ret.HasError && send2Server)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 109);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Name))
			{
				await res.AddErrorAsync("لطفا نام نقش را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 120);
			}
			if (system == EnSystem.UserInterface && Status)
			{
				if ((PriceTypeGuid ?? Guid.Empty) == Guid.Empty)
				{
					await res.AddErrorAsync("لطفا سطح قیمت نقش را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 123);
				}
				else
				{
					IEnumerable<CustomerRoleBusiness> enumerable = (await GetAllAsync(ContextGuid, true, token, tranName))?.Where((CustomerRoleBusiness q) => q.Guid != Guid && q.WordPressName == WordPressName && (q.PriceTypeGuid ?? Guid.Empty) != Guid.Empty);
					if (enumerable != null && enumerable.Any() && ((enumerable.Select((CustomerRoleBusiness q) => q.PriceTypeGuid)?.Distinct()?.Count()).GetValueOrDefault() > 1 || enumerable.FirstOrDefault().PriceTypeGuid != PriceTypeGuid))
					{
						ProductPriceLevelBusiness productPriceLevelBusiness = await ProductPriceLevelBusiness.GetAsync(ContextGuid, PriceTypeGuid.Value, system, token, tranName);
						if (productPriceLevelBusiness != null)
						{
							await res.AddErrorAsync("برای نقش".GetTranslate() + "  " + WordPressName + " " + " سابقا سطح قیمت".GetTranslate() + " " + (" " + productPriceLevelBusiness?.Name).GetTranslate() + " " + " تعیین شده است و هر نقش صرفا باید یک سطح قیمت داشته باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 135);
						}
					}
				}
			}
			if (system == EnSystem.UserInterface && !Status)
			{
				List<Guid> list = await CusCompactBusiness.GetCustomerGuidListinRoleAsync(ContextGuid, Guid, token, tranName);
				if (list != null && list.Any())
				{
					await res.AddErrorAsync("تعداد ".GetTranslate() + " " + $"{list.Count()}" + " " + " مشتری فعال در این نقش وجود دارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 144);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 149);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 150);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, CancellationToken token, IEnumerable<CustomerRoleBusiness> lst, bool send2Server, Action<PercenteCompletedEventArgs> percentCompleted = null)
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
			foreach (CustomerRoleBusiness item in lst)
			{
				current = checked(current + 1);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 165);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, send2Server), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 166);
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
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 178);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 179);
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

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.CustomerRole;
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 205), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 205);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CustomerRole.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 207);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 209);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 212);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 213);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 218), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 218);
			}
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, EnSystem.None, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 10)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 234);
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
					return ret.AddError("کش آنلاین نیست!!", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 236);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 237);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.CustomerRole.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 241);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 249);
						}
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 251);
						}
						CustomerRoleBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj == null)
						{
							continue;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: CustomerRole \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty} \r\n" + "EntityName: " + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 256);
							continue;
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 267);
							tasks.Remove(t2);
						}
					}
				}
				while (tasks?.Any() ?? false)
				{
					Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 274);
					tasks.Remove(t2);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 277);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 279);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 282);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 283);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs"))
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
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "CustomerRoleBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 299);
				}
				List<WebCustomerRole> list = new List<WebCustomerRole>();
				List<WebCustomerRole> list2 = list;
				list2.Add(await CustomerRoleMapper.Instance.MapAsync(ContextGuid, this, new WebCustomerRole(), "", token));
				List<Packet> paks = WebCustomerRole.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 301);
				}
				using CustomerRoleWaiter waiter = new CustomerRoleWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("CustomerRoleWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 305);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 307);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 309);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 312);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 313);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, List<EnPriceType> list, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			foreach (EnPriceType item in list)
			{
				if (await GetAsync(contextGuid, item.GetDisplay(), token) == null)
				{
					CustomerRoleBusiness existItem = new CustomerRoleBusiness(contextGuid)
					{
						ChangeStatus = EntityChangeSt.New,
						ContextGuid = contextGuid,
						Description = "",
						Guid = Guid.NewGuid(),
						MetaData = null,
						Modified = DateTime.Now,
						Name = item.GetDisplay(),
						ServerStatus = ServerStatusForClient.None,
						Status = true,
						WordPressName = item.GetDisplay()
					};
					switch (item)
					{
					case EnPriceType.sell1:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell1;
						break;
					case EnPriceType.sell2:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell2;
						break;
					case EnPriceType.sell3:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell3;
						break;
					case EnPriceType.sell4:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell4;
						break;
					case EnPriceType.sell5:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell5;
						break;
					case EnPriceType.sell6:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell6;
						break;
					case EnPriceType.sell7:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell7;
						break;
					case EnPriceType.sell8:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Sell8;
						break;
					case EnPriceType.LastSellToAll:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.LastSellToAll;
						break;
					case EnPriceType.LastSellToCus:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.LastSellToTafsil;
						break;
					case EnPriceType.BuyLastFromAll:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.LastBuyFromAll;
						break;
					case EnPriceType.BuyLastFromCus:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.LastBuyFromTafsil;
						break;
					case EnPriceType.Avale2re:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.BeginPeriod;
						break;
					case EnPriceType.BuyAverage:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.AverageBuy;
						break;
					case EnPriceType.Buy1:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Buy1;
						break;
					case EnPriceType.Buy2:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Buy2;
						break;
					case EnPriceType.Buy3:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.Buy3;
						break;
					case EnPriceType.BuyLast:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.LastBuy;
						break;
					case EnPriceType.CurrentSaved:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.CurrentSaved;
						break;
					case EnPriceType.Regular:
						existItem.PriceTypeGuid = ParentDefaults.PriceLevel.RegularPrice;
						break;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await existItem.SaveAsync(system, token, send2Server: false), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 363);
					if (!res.HasError)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CusBusiness.UpdateRoleGuidAsync(contextGuid, item, existItem.Guid, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 365);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 370);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 371);
		}
		return res;
	}

	public static async Task<List<CustomerRoleConflictModel>> GetAllMoghayeratAsync(string contextGuid)
	{
		try
		{
			List<CustomerRoleConflictModel> list = new List<CustomerRoleConflictModel>();
			IEnumerable<CustomerRoleBusiness> enumerable = await GetAllAsync(contextGuid, true);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			IEnumerable<IGrouping<string, CustomerRoleBusiness>> enumerable2 = from q in enumerable
				group q by q.WordPressName;
			if (enumerable2 == null || !enumerable2.Any())
			{
				return null;
			}
			foreach (IGrouping<string, CustomerRoleBusiness> item in enumerable2)
			{
				if ((item?.Count() ?? 0) <= 1)
				{
					continue;
				}
				int valueOrDefault = (item?.Where((CustomerRoleBusiness q) => (q.PriceTypeGuid ?? Guid.Empty) != Guid.Empty)?.Select((CustomerRoleBusiness q) => q.PriceTypeGuid)?.Distinct()?.Count()).GetValueOrDefault();
				if (valueOrDefault > 1)
				{
					list.Add(new CustomerRoleConflictModel
					{
						WordPressName = item.Key,
						Count = valueOrDefault,
						ConflictName = item.Select((CustomerRoleBusiness q) => q.Name)?.ToList()
					});
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllMoghayeratAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CustomerRoleBusiness.cs", 400);
			return null;
		}
	}
}
