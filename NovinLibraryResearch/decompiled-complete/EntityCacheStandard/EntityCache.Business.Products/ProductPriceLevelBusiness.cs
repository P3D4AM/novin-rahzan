using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Core;
using EntityCache.Core;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers.Accounting.Product;
using EntityCache.Utilities;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Product;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Records.Products;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Products;

public class ProductPriceLevelBusiness : BaseBusiness<ProductPriceLevelBusiness>, IProductPriceLevel, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<ProductPriceLevelBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public short Ordinal { get; set; }

	public EnPriceLevelType Type { get; set; }

	[JsonIgnore]
	public string TypeForDisplay => Type.GetDisplay();

	public decimal DiscountCoefficient { get; set; }

	[JsonIgnore]
	public string DiscountCoefficientForDisplay => DiscountCoefficient.FixDecimal(18, 3).ToString().Replace(".", "/");

	public bool IsSystem { get; set; }

	public bool IsDefault { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public override string ContextGuid { get; set; }

	[JsonIgnore]
	public bool IsChecked { get; set; }

	[JsonIgnore]
	public byte[] ServerStatusImage
	{
		get
		{
			ServerStatusForClient serverStatus = ServerStatus;
			if ((serverStatus == ServerStatusForClient.Delivered || serverStatus == ServerStatusForClient.DirectDelivery) ? true : false)
			{
				return ImageResourceManager.ServerStatusDelivered;
			}
			if (ServerStatus == ServerStatusForClient.DeliveryError)
			{
				return ImageResourceManager.ServerStatusDeliveryFailed;
			}
			if (ServerStatus == ServerStatusForClient.Sent)
			{
				return ImageResourceManager.ServerStatusSent;
			}
			if (ServerStatus == ServerStatusForClient.SendError)
			{
				return ImageResourceManager.ServerStatusSentError;
			}
			return ImageResourceManager.ServerStatusNone;
		}
	}

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public ProductPriceLevelBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
		DiscountCoefficient = 1m;
	}

	public static async Task<IEnumerable<ProductPriceLevelBusiness>> GetAllAsync(string contextGuid, EnSystem system, ProductPriceLevelFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			List<ProductPriceLevelBusiness> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.GetAllAsync(filter, token, tranName))?.ToList() ?? new List<ProductPriceLevelBusiness>();
			if (filter?.IsLoadDefault ?? false)
			{
				if (list == null)
				{
					list = new List<ProductPriceLevelBusiness>();
				}
				list.Add(new ProductPriceLevelBusiness(contextGuid)
				{
					Ordinal = -1,
					Guid = Guid.Empty,
					Name = "لطفا سطح قیمت را انتخاب نمایید ...".GetTranslate()
				});
			}
			return await ApplyAccessAsync(contextGuid, list?.OrderBy((ProductPriceLevelBusiness q) => q.Ordinal)?.ToList(), system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 96);
			throw;
		}
	}

	public static async Task<IEnumerable<ProductPriceLevelPercentModel>> GetAllPercentModelAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				Status = true,
				Type = EnPriceLevelType.Sell
			};
			IEnumerable<ProductPriceLevelBusiness> enumerable = await GetAllAsync(contextGuid, system, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return enumerable.Select((ProductPriceLevelBusiness q) => new ProductPriceLevelPercentModel
			{
				Guid = q.Guid,
				Name = q.Name,
				Ordinal = q.Ordinal
			})?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllPercentModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 120);
			throw;
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
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
				using (await _databaseSemaphore.GaurdAsync(true, "ProductPriceLevelBusiness/SetSaveResultAsync", 60000, token))
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 135), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 135);
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ProductPriceLevel.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 137);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 139);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 142);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 143);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 148), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 148);
			}
		}
		return ret;
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.ProductPriceLevel;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 9;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ProductPriceLevelBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 162);
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 163), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 163);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 165);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							if (system == EnSystem.SignalR)
							{
								ProductPriceLevelBusiness productPriceLevelBusiness = await GetAsync(ContextGuid, EnSystem.None, Ordinal, Status, token, tranName);
								if (productPriceLevelBusiness != null && productPriceLevelBusiness.Guid != Guid)
								{
									productPriceLevelBusiness.Ordinal = 0;
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await productPriceLevelBusiness.SaveAsync(system, sendToServer: false, Priority.Low, token, awaitOnSemaphore: false, await4Send: false, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 174);
									returnedSaveFuncInfo = res;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 175), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 175);
									res.throwExceptionIfError();
								}
							}
							if (Ordinal <= 0)
							{
								Ordinal = await GetMaxOrdinalAsync(ContextGuid, token, tranName);
							}
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ProductPriceLevel.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 181);
							if (res.HasError)
							{
								result = res;
							}
							else
							{
								if (Status || system != EnSystem.UserInterface)
								{
									goto end_IL_01b9;
								}
								ProductPriceLevelFilter filter = new ProductPriceLevelFilter
								{
									Status = true
								};
								IEnumerable<ProductPriceLevelBusiness> enumerable = (await GetAllAsync(ContextGuid, EnSystem.None, filter, token, tranName))?.Where((ProductPriceLevelBusiness q) => q.Ordinal > Ordinal)?.OrderBy((ProductPriceLevelBusiness q) => q.Ordinal);
								if (enumerable == null || !enumerable.Any())
								{
									result = res;
								}
								else
								{
									List<ChangeOrdinalRecord> list = new List<ChangeOrdinalRecord>();
									foreach (ProductPriceLevelBusiness item in enumerable)
									{
										list.Add(new ChangeOrdinalRecord(item.Guid, checked((short)(item.Ordinal - 1))));
									}
									if (list != null && list.Any())
									{
										returnedSaveFuncInfo = res;
										returnedSaveFuncInfo.AddReturnedValue(await ChangeOrdinalAsync(ContextGuid, list, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 194);
										goto end_IL_01b9;
									}
									result = res;
								}
							}
						}
					}
					goto end_IL_00f8;
					end_IL_01b9:;
				}
				goto end_IL_00e6;
				end_IL_00f8:;
			}
			catch (OperationCanceledException exception)
			{
				res.AddReturnedValue(exception, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 197);
				goto end_IL_00e6;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 200);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 201);
				goto end_IL_00e6;
			}
			return result;
			end_IL_00e6:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 206), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 206);
			}
			if (!res.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 209);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!Status && IsSystem)
			{
				await res.AddErrorAsync("سطح قیمت پیش فرض سیستم قابل حذف شدن نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 220);
			}
			if (!Status)
			{
				int num = await ProductPriceRelationBusiness.DbCountAsync(ContextGuid, null, Guid, token, tranName);
				if (num > 0)
				{
					await res.AddErrorAsync("سطح قیمت ".GetTranslate() + " " + (Name ?? "").GetTranslate() + " " + " برای ".GetTranslate() + " " + $"{num}" + " " + " کالا درنظر گرفته شده است و شما مجاز به حذف آن نمی باشید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 224);
				}
				return res;
			}
			if (string.IsNullOrEmpty(Name))
			{
				await res.AddErrorAsync("لطفا عنوان را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 227);
			}
			ProductPriceLevelBusiness productPriceLevelBusiness = await GetAsync(ContextGuid, Name, Type, EnSystem.None, token);
			if (productPriceLevelBusiness != null && productPriceLevelBusiness.Guid != Guid)
			{
				await res.AddErrorAsync("عنوان ".GetTranslate() + " " + (Name ?? "").GetTranslate() + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 230);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 234);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 235);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, bool sendToServer, CancellationToken token, IEnumerable<ProductPriceLevelBusiness> lst, Priority priority = Priority.Low, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			lst = lst.OrderBy((ProductPriceLevelBusiness q) => q.Ordinal).ToList();
			if (system != EnSystem.SignalR && system != EnSystem.Transfer)
			{
				ReOrdinal(lst);
			}
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			foreach (ProductPriceLevelBusiness item in lst)
			{
				if (item == null)
				{
					throw new NullReferenceException("list contains null items!");
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 251);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, sendToServer, priority, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 252);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 257);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 258);
		}
		return ret;
	}

	private static void ReOrdinal(IEnumerable<ProductPriceLevelBusiness> list)
	{
		try
		{
			if (list == null || !list.Any())
			{
				return;
			}
			int num = 0;
			foreach (ProductPriceLevelBusiness item in list)
			{
				short num2 = checked((short)(++num));
				if (item.Ordinal != num2)
				{
					item.Ordinal = num2;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 274);
		}
	}

	public static async Task<ProductPriceLevelBusiness> GetAsync(string contextGuid, EnSystem system, short ordinal, bool status, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			ProductPriceLevelFilter filter = new ProductPriceLevelFilter
			{
				IsLoadDefault = true,
				IsLoadSystem = true,
				Status = status,
				Type = null
			};
			return await ApplyAccessAsync(contextGuid, (await GetAllAsync(contextGuid, system, filter, token, tranName))?.FirstOrDefault((ProductPriceLevelBusiness p) => p.Ordinal == ordinal), system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 292);
			throw;
		}
	}

	public static async Task<ProductPriceLevelBusiness> GetAsync(string contextGuid, Guid guid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			ProductPriceLevelBusiness productPriceLevelBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.GetAsync(guid, token, tranName);
			if (productPriceLevelBusiness == null)
			{
				return null;
			}
			return await ApplyAccessAsync(contextGuid, productPriceLevelBusiness, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 306);
			throw;
		}
	}

	public static async Task<ProductPriceLevelBusiness> GetAsync(string contextGuid, string name, EnPriceLevelType type, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			ProductPriceLevelBusiness productPriceLevelBusiness = await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.GetAsync(name.FixString(), type, token, tranName);
			if (productPriceLevelBusiness == null)
			{
				return null;
			}
			return await ApplyAccessAsync(contextGuid, productPriceLevelBusiness, system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 320);
			throw;
		}
	}

	private static Task<short> GetMaxOrdinalAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.GetMaxOrdinalAsync(token, tranName);
	}

	public static Task<ProductPriceLevelBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<ProductPriceLevelBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 5)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.ProductPriceLevel;
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال سطح قیمت کالاها".GetTranslate();
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Products;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 343);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 344);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 345);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel == null)
				{
					_ = ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel;
					throw new NullReferenceException("repo");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 354);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("سطوح قیمت کالاها سینک هستند تعداد سینک شده".GetTranslate() + " " + $":{list?.Count ?? 0}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 355);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 356);
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
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 363);
						}
						current = checked(current + 1);
						ProductPriceLevelBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (uiUpdater != null)
						{
							uiUpdater.ShortMessage = "سطح قیمت جاری".GetTranslate() + ":" + (obj?.Name ?? "").GetTranslate();
							uiUpdater.Count = current;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 371);
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: TaxBusiness \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}\r\n" + "EntityDesc:" + obj?.Name, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 374);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 380);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR)));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 385);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 389);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 390);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 394);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 395);
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
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs"))
		{
			_ = 3;
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (!cache.BufferEnable)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ProductPriceLevelBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 427);
				}
				WebProductPriceLevel item = await ProductPriceLevelMapper.Instance.MapAsync(ContextGuid, this, new WebProductPriceLevel(), "", token);
				List<Packet> paks = WebProductPriceLevel.GenerateSaveRequest(new List<WebProductPriceLevel> { item }, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 430);
				}
				using ProductPriceLevelWaiter waiter = new ProductPriceLevelWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => cache.SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("ProductPriceLevelWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 434);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 436);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 438);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 441);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 442);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeOrdinalAsync(string contextGuid, List<ChangeOrdinalRecord> list, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		try
		{
			_ = 2;
			ReturnedSaveFuncInfo result;
			try
			{
				if (!(list?.Any() ?? false))
				{
					result = res;
					goto end_IL_0079;
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "ProductPriceLevelBusiness/ChangeOrdinalAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 457), ReturnedState.None, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 457);
						}
						transactionHasBeenCreated = true;
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ProductPriceLevel.ChangeOrdinalAsync(list, token, tranName), ReturnedState.None, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 459);
						goto end_IL_013a;
					}
					result = res.AddErrorTokenIsCancelled("", "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 456);
					goto end_IL_0079;
					end_IL_013a:;
				}
				goto end_IL_0065;
				end_IL_0079:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 463);
				res.AddReturnedValue(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 464);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 469), ReturnedState.None, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 469);
			}
		}
		return res;
	}

	private static async Task<IEnumerable<ProductPriceLevelBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<ProductPriceLevelBusiness> items, EnSystem system, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (system != EnSystem.UserInterface)
			{
				return items;
			}
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName);
			if (cashierBusiness == null)
			{
				return items;
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			UserAccBusiness user = (await cashierBusiness.GetUserAsync(token, tranName)) ?? throw new NullReferenceException("UserIs Null!!");
			if (token.IsCancellationRequested)
			{
				return null;
			}
			if (user.UserAccess.PrdPriceLevelListGuid?.Any() ?? false)
			{
				items = items?.Where((ProductPriceLevelBusiness p) => p != null && user.UserAccess.PrdPriceLevelListGuid.Contains(p.Guid));
			}
			if (token.IsCancellationRequested)
			{
				return null;
			}
			items = items?.ToList();
			return items;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 499);
			throw;
		}
	}

	private static async Task<ProductPriceLevelBusiness> ApplyAccessAsync(string contextGuid, ProductPriceLevelBusiness item, EnSystem system, CancellationToken token, string tranName)
	{
		try
		{
			if (system == EnSystem.UserInterface)
			{
				return (await ApplyAccessAsync(contextGuid, new List<ProductPriceLevelBusiness> { item }, system, token, tranName))?.SingleOrDefault();
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Products\\ProductPrices\\ProductPriceLevelBusiness.cs", 514);
			throw;
		}
	}
}
