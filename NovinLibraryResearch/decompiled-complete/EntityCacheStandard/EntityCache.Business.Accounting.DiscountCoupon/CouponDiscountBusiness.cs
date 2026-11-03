using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon.Records;
using EntityCache.Business.Accounting.DiscountCoupon.Reports;
using EntityCache.Business.Accounting.Gift;
using EntityCache.Business.Accounting.Payamak;
using EntityCache.Business.Core;
using EntityCache.Business.Report;
using EntityCache.Core;
using EntityCache.Mapping.BusinessMappers.Accounting.DiscountCoupon;
using EntityCache.Mapping.BusinessMappers.Accounting.Gift;
using EntityCache.Mapping.BusinessMappers.Accounting.NewFolder;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Accounting.DiscountCoupon;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DiscountCoupon;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Filters.CouponDiscount;
using PacketParser.Records;
using PacketParser.Records.OccasionCalendar;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting.DiscountCoupon;

public class CouponDiscountBusiness : BaseBusiness<CouponDiscountBusiness>, ICouponDiscount, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, ISyncFromClient2Server<CouponDiscountBusiness>, IDeliverableEntity, IEditableDelivery, IHasContext, IDisposable
{
	private bool _isDisposed;

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private List<CouponDiscountDetailBusiness> couponDiscountDetailList;

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim databaseSemaphor = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _resendToServerSemaphore = new SemaphoreSlim(1, 1);

	private TaskCompletionSource<bool> forceUpdateTask;

	public Guid Guid { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime? DateFrom { get; set; } = DateTime.Now;


	public DateTime? DateTo { get; set; } = DateTime.Now;


	[JsonIgnore]
	public int RemainDayCount
	{
		get
		{
			try
			{
				if (!DateTo.HasValue)
				{
					return 0;
				}
				DateTime? dateTime = DateFrom;
				if (!DateFrom.HasValue)
				{
					dateTime = DateTime.Now;
				}
				if (dateTime.Value >= DateTo.Value)
				{
					return 0;
				}
				return (DateTo.Value - dateTime.Value).Days;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemainDayCount", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 64);
			}
			return 0;
		}
	}

	public override string ContextGuid { get; set; }

	public string Title { get; set; }

	public bool SingleUse { get; set; }

	public bool Status { get; set; }

	public int CountCouponUse { get; set; }

	public int CountCouponUseForUser { get; set; }

	public DateTime Modified { get; set; }

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

	public ServerStatusForClient ServerStatus { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public int DeliveryId { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public EnCouponType Type { get; set; }

	public EnCouponMode Mode { get; set; }

	public int DeadLineForUse { get; set; }

	public bool IsImmediateUseAllowed { get; set; }

	public int Version { get; set; }

	public int UsedCount { get; set; }

	public CouponDiscountMetaData MetaData { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public CouponDiscountBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.CouponDiscountBusiness;

	public IReadOnlyCollection<CouponDiscountDetailBusiness> CouponDetailList
	{
		get
		{
			if (couponDiscountDetailList == null)
			{
				couponDiscountDetailList = new List<CouponDiscountDetailBusiness>();
			}
			couponDiscountDetailList.RemoveAll((CouponDiscountDetailBusiness q) => q?.IsDisposed ?? true);
			return couponDiscountDetailList;
		}
	}

	public event Func<object, Guid, Task> OnCouponDetailAdd;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public CouponDiscountBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (CouponDiscountDetailBusiness item in couponDiscountDetailList?.ToList() ?? new List<CouponDiscountDetailBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 120);
			}
		}
	}

	public static Task<List<SmsTemplateRecord>> GetSmsTemplateListAsync()
	{
		try
		{
			return Task.FromResult(new List<SmsTemplateRecord>
			{
				new SmsTemplateRecord("[عنوان]"),
				new SmsTemplateRecord("[نام مشتری]"),
				new SmsTemplateRecord("[اعتبار از تاریخ]"),
				new SmsTemplateRecord("[اعتبار تا تاریخ]"),
				new SmsTemplateRecord("[تعداد روز باقی مانده از اعتبار]"),
				new SmsTemplateRecord("[مبلغ یا درصد]"),
				new SmsTemplateRecord("[حداقل مبلغ تخفیف]"),
				new SmsTemplateRecord("[حداکثر مبلغ تخفیف]"),
				new SmsTemplateRecord("[حداقل درصد تخفیف]"),
				new SmsTemplateRecord("[حداکثر درصد تخفیف]"),
				new SmsTemplateRecord("[حداقل میزان خرید]"),
				new SmsTemplateRecord("[حداکثر میزان خرید]"),
				new SmsTemplateRecord("[تاریخ روز جاری]"),
				new SmsTemplateRecord("[عنوان فروشگاه]"),
				new SmsTemplateRecord("[تلفن فروشگاه]")
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSmsTemplateListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 148);
			throw;
		}
	}

	public Task AddCouponDetailListAsync(CouponDiscountDetailBusiness couponDiscountDetail, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (couponDiscountDetail == null)
			{
				return Task.CompletedTask;
			}
			if (couponDiscountDetailList == null)
			{
				couponDiscountDetailList = new List<CouponDiscountDetailBusiness>();
			}
			couponDiscountDetail.MasterGuid = Guid;
			couponDiscountDetail.OnDisposed += Det_OnDisposed;
			couponDiscountDetailList.Add(couponDiscountDetail);
			if (EventManager.OnCouponDetailAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnCouponDetailAdd, this, couponDiscountDetail.Guid, token, 60, "AddCouponDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddCouponDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 164);
		}
		return Task.CompletedTask;
	}

	public async Task AddCouponDetailListAsync(List<CouponDiscountDetailBusiness> list, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (CouponDiscountDetailBusiness item in list)
			{
				await AddCouponDetailListAsync(item, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddCouponDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 174);
		}
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs"))
		{
			try
			{
				if ((couponDiscountDetailList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (CouponDiscountDetailBusiness item in couponDiscountDetailList?.ToList()?.Where((CouponDiscountDetailBusiness p) => p == null || p.Master == sender || p == sender)?.ToList() ?? new List<CouponDiscountDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				couponDiscountDetailList?.RemoveAll((CouponDiscountDetailBusiness p) => p == null || p.Master == sender || p == sender);
				if (EventManager.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 194);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveCouponDetailAsync(Guid couponDetailGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			CouponDiscountDetailBusiness couponDiscountDetailBusiness = couponDiscountDetailList?.FirstOrDefault((CouponDiscountDetailBusiness q) => q.Guid == couponDetailGuid);
			if (couponDiscountDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			couponDiscountDetailList.Remove(couponDiscountDetailBusiness);
			if (EventManager.OnCouponDetailAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnCouponDetailAdd, this, couponDetailGuid, token, 60, "RemoveCouponDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveCouponDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 209);
		}
		return Task.CompletedTask;
	}

	public async Task<int> GetUsedCountAsync(CancellationToken token, string tranName)
	{
		try
		{
			ClsCache cache = ClsCache.GetCache(ContextGuid);
			if (cache.Source != ENSource.Accounting)
			{
				return UsedCount;
			}
			return await cache.CacheRepository.CouponDiscount.GetUsedCountAsync(Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUsedCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 222);
			throw;
		}
	}

	public async Task<IEnumerable<CouponDiscountUsagesBusiness>> GetAllUagesAsync(CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			ClsCache cache = ClsCache.GetCache(ContextGuid);
			if (cache.Source == ENSource.Accounting)
			{
				return await cache.CacheRepository.CouponDiscount.GetAllUagesAsync(Guid, token, tranName);
			}
			return await CouponDiscountUsagesBusiness.GetAllAsync(ContextGuid, Guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllUagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 236);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, OrderBusiness order, bool sendToServer, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		EntityChangeSt changeStatus = ChangeStatus;
		try
		{
			_ = 3;
			checked
			{
				ReturnedSaveFuncInfo result;
				try
				{
					sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
					using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "CouponDiscountBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 251);
						}
						if (autotran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 252), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 252);
						}
						trantionBeenCreate = true;
						if (CashierGuid == Guid.Empty)
						{
							CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
						}
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							if (CountCouponUse <= 0)
							{
								CountCouponUse = 32000;
							}
							if (CountCouponUseForUser <= 0)
							{
								CountCouponUseForUser = CountCouponUse;
							}
							if (system == EnSystem.UserInterface)
							{
								Version++;
							}
							if (Version <= 0)
							{
								Version = 1;
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 260);
							if (!res.HasError)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CouponDiscount.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 262);
								goto end_IL_01ce;
							}
							result = res;
						}
						goto end_IL_0104;
						end_IL_01ce:;
					}
					goto end_IL_00f0;
					end_IL_0104:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 266);
					res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 267);
					goto end_IL_00f0;
				}
				return result;
			}
			end_IL_00f0:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 272), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 272);
			}
			if (!res.HasError && sendToServer && system != EnSystem.Transfer)
			{
				if (await4Send)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 276);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (system != EnSystem.Transfer && system != EnSystem.SignalR && Status)
			{
				Task.Run(() => SendMessageAsync(system, order, changeStatus, null, token));
			}
		}
		return res;
	}

	internal static async Task<ReturnedSaveFuncInfo> SaveRangeFromWebAsync(string contextGuid, List<WebDiscountCoupon> list, bool sendToServer, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			foreach (WebDiscountCoupon item in list)
			{
				if (item != null)
				{
					if (token.IsCancellationRequested)
					{
						return res.AddErrorTokenIsCancelled("", "", "SaveRangeFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 294);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await SaveFromWebAsync(contextGuid, item, sendToServer, priority, token, awaitOnSemaphore, await4Send), ReturnedState.None, "SaveRangeFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 295);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 300);
			res.AddReturnedValue(ex, "", "SaveRangeFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 301);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> SaveFromWebAsync(string contextGuid, WebDiscountCoupon coupon, bool sendToServer, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, bool await4Send = false, string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		try
		{
			_ = 8;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "CouponDiscountBusiness/SaveFromWebAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 314);
					}
					if (autotran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 315), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 315);
					}
					trantionBeenCreate = true;
					List<WebGift> list = coupon.DetailList.Select((WebDiscountCouponDetail q) => q.Gift)?.Where((WebGift q) => q != null)?.ToList();
					if (list != null && list.Any())
					{
						foreach (WebGift item in list)
						{
							if (item != null)
							{
								GiftBusiness obj = await GiftMapper.Instance.MapAsync(contextGuid, item, new GiftBusiness(contextGuid), tranName, token);
								ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await obj.SaveAsync(EnSystem.SignalR, token, tranName), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 325);
							}
						}
					}
					if (res.HasError || token.IsCancellationRequested)
					{
						result = res;
					}
					else
					{
						CouponDiscountBusiness couponDiscountBusiness = await GetAsync(contextGuid, coupon.Guid, token, tranName);
						if (couponDiscountBusiness == null || couponDiscountBusiness.Version < coupon.Version)
						{
							CouponDiscountBusiness couponDiscountBusiness2 = await CouponDiscountMapper.Instance.MapAsync(contextGuid, coupon, new CouponDiscountBusiness(contextGuid), tranName, token);
							if (sendToServer)
							{
								couponDiscountBusiness2.ServerStatus = ServerStatusForClient.None;
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await couponDiscountBusiness2.SaveAsync(EnSystem.SignalR, null, sendToServer, priority, token, !awaitOnSemaphore, await4Send, tranName), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 336);
						}
						if (!res.HasError && (coupon.MetaData?.Usages?.Any()).GetValueOrDefault() && !token.IsCancellationRequested)
						{
							List<CouponDiscountUsagesBusiness> list2 = await CouponDiscountUsagesMapper.Instans.MapListAsync(contextGuid, coupon.MetaData.Usages, new List<CouponDiscountUsagesBusiness>(), tranName, token);
							ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await CouponDiscountUsagesBusiness.SaveRangeAsync(contextGuid, list2, token, tranName), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 341);
							goto end_IL_012f;
						}
						result = res;
					}
					goto end_IL_0077;
					end_IL_012f:;
				}
				goto end_IL_0066;
				end_IL_0077:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 345);
				res.AddReturnedValue(ex, "", "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 346);
				goto end_IL_0066;
			}
			return result;
			end_IL_0066:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 351), ReturnedState.None, "SaveFromWebAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 351);
			}
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!Status)
			{
				return res;
			}
			if (system == EnSystem.UserInterface)
			{
				CouponDiscountReportBusiness couponDiscountReportBusiness = await GetAsync(ContextGuid, Title, token, tranName);
				if (couponDiscountReportBusiness != null && couponDiscountReportBusiness.Guid != Guid && couponDiscountReportBusiness.Status)
				{
					res.AddError("عنوان".GetTranslate() + " " + (Title ?? "").GetTranslate() + " " + "تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 365);
				}
			}
			if (DateFrom > DateTo)
			{
				res.AddError("تاریخ انقضای کوپن، نمی تواند از تاریخ شروع بزرگتر باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 367);
			}
			if (DateFrom < new DateTime(2010L) && DateTo > new DateTime(2099L))
			{
				res.AddError("تاریخ های وارد شده صحیح نیست".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 368);
			}
			if (string.IsNullOrEmpty(Title))
			{
				res.AddError("عنوان کوپن را وارد کنید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 369);
			}
			if (!(CouponDetailList?.Any() ?? false))
			{
				res.AddError("با زدن دکمه مدیریت جزییات کوپن، جزییات کوپن را مشخص کنید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 370);
			}
			if (Mode != 0 && Mode != EnCouponMode.Gift)
			{
				CouponFilter filter = new CouponFilter
				{
					ModeList = new List<EnCouponMode> { Mode },
					Status = true
				};
				if ((await GetAllAsync(ContextGuid, filter, token))?.SingleOrDefault((CouponDiscountReportBusiness q) => q.Guid != Guid) != null)
				{
					res.AddError("کوپن".GetTranslate() + " " + Mode.GetDisplay().GetTranslate() + " " + "یک بار تعریف شده است امکان تعریف مجدد این کوپن وجود ندارد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 381);
				}
			}
			foreach (CouponDiscountDetailBusiness couponDetail in CouponDetailList)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await couponDetail.CheckValidationAsync(), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 385);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 387);
		}
		return res;
	}

	private async Task SendMessageAsync(EnSystem system, OrderBusiness order, EntityChangeSt? status, IEnumerable<CouponSmsSetting> smsSettings, CancellationToken token)
	{
		using (new JaegerMonitor("SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (smsSettings == null)
				{
					if (!(MetaData?.CouponSmsSettingsList?.Any()).GetValueOrDefault() || !(CouponDetailList?.Any() ?? false) || !Status || Mode != 0)
					{
						return;
					}
					smsSettings = MetaData.CouponSmsSettingsList.Where((CouponSmsSetting q) => q.Status != PayamakSendStatus.Disable);
				}
				if (!(smsSettings?.Any() ?? false))
				{
					return;
				}
				if (status.HasValue)
				{
					if (status == EntityChangeSt.New)
					{
						smsSettings = smsSettings?.Where((CouponSmsSetting q) => q.SendSmsWhenAddCoupon);
					}
					else if (status == EntityChangeSt.Edited)
					{
						smsSettings = smsSettings?.Where((CouponSmsSetting q) => q.SendSmsWhenEditCoupon);
					}
					if (!(smsSettings?.Any() ?? false))
					{
						return;
					}
				}
				AccGlobalSettings settings = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				List<string> lstNumbers = new List<string>();
				foreach (CouponSmsSetting pattern in smsSettings)
				{
					if (string.IsNullOrEmpty(pattern.SmsText))
					{
						continue;
					}
					foreach (CouponDiscountDetailBusiness couponDetail in CouponDetailList)
					{
						if (couponDetail == null)
						{
							continue;
						}
						string message = pattern.SmsText;
						List<Guid> list = await couponDetail.GetAllCustomerContainsGuidListAsync(system, token);
						if (list == null || !list.Any())
						{
							continue;
						}
						foreach (Guid tafsilGuid in list)
						{
							if (tafsilGuid == Guid.Empty)
							{
								continue;
							}
							SanadPrintBusiness sanadPrint = null;
							List<TafsilNumberBusiness> list2 = await TafsilNumberBusiness.GetAsync(ContextGuid, tafsilGuid, token);
							if (list2 != null && list2.Any())
							{
								lstNumbers.AddRange(list2?.Where((TafsilNumberBusiness p) => p.Mobile)?.Select((TafsilNumberBusiness p) => p.NumberForDisplay()));
							}
							else if (order != null)
							{
								sanadPrint = await SanadPrintBusiness.GetAsync(ContextGuid, order.Guid, order.FactorType, token);
								if (!string.IsNullOrEmpty(sanadPrint?.Mob))
								{
									lstNumbers.Add(sanadPrint.Mob);
								}
								if (!string.IsNullOrEmpty(sanadPrint?.Tel))
								{
									lstNumbers.Add(sanadPrint.Tel);
								}
							}
							if (!(lstNumbers?.Any() ?? false))
							{
								continue;
							}
							message = await FillSmsParametersAsync(message, sanadPrint, system, tafsilGuid, token);
							if (!string.IsNullOrEmpty(message))
							{
								string number = string.Join(",", lstNumbers);
								_ = new CancellationTokenSource(60000).Token;
								SmsOutBoxBusiness outBox = new SmsOutBoxBusiness(ContextGuid)
								{
									AccountId = settings.Payamak.Account_ID,
									Message = message,
									Number = number
								};
								Task.Run(() => outBox.SaveAsync(token, ""));
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 460);
				throw;
			}
		}
	}

	private async Task<string> FillSmsParametersAsync(string message, SanadPrintBusiness sanadPrint, EnSystem system, Guid? tafsilGuid, CancellationToken token, string tranName = "")
	{
		_ = 1;
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			if (message.Contains("[عنوان]"))
			{
				message = message.Replace("[عنوان]", Title);
			}
			if (message.Contains("[نام مشتری]") && (tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				string newValue = (string.IsNullOrEmpty(sanadPrint?.Name) ? (await TafsilBusiness.GetAsync(ContextGuid, system, tafsilGuid.Value, tranName, token, "FillSmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs"))?.Name : sanadPrint.Name);
				message = message.Replace("[نام مشتری]", newValue);
			}
			if (message.Contains("[اعتبار از تاریخ]"))
			{
				message = message.Replace("[اعتبار از تاریخ]", DateFrom.MiladiToCalture());
			}
			if (message.Contains("[اعتبار تا تاریخ]"))
			{
				message = message.Replace("[اعتبار تا تاریخ]", DateTo.MiladiToCalture());
			}
			if (message.Contains("[تعداد روز باقی مانده از اعتبار]"))
			{
				message = message.Replace("[تعداد روز باقی مانده از اعتبار]", RemainDayCount.ToString());
			}
			if (message.Contains("[مبلغ یا درصد]"))
			{
				decimal num = default(decimal);
				if ((CouponDetailList?.Count() ?? 0) == 1)
				{
					num = CouponDetailList.Single().AmountCoupon;
				}
				message = message.Replace("[مبلغ یا درصد]", num.ToString("N0"));
			}
			if (message.Contains("[حداقل مبلغ تخفیف]"))
			{
				message = message.Replace("[حداقل مبلغ تخفیف]", (CouponDetailList?.Min((CouponDiscountDetailBusiness q) => q.AmountPrice) ?? 0m).ToString("N0"));
			}
			if (message.Contains("[حداکثر مبلغ تخفیف]"))
			{
				message = message.Replace("[حداکثر مبلغ تخفیف]", (CouponDetailList?.Max((CouponDiscountDetailBusiness q) => q.AmountPrice) ?? 0m).ToString("N0"));
			}
			if (message.Contains("[حداقل درصد تخفیف]"))
			{
				message = message.Replace("[حداقل درصد تخفیف]", (CouponDetailList?.Min((CouponDiscountDetailBusiness q) => q.AmountPercent) ?? 0.0).NpToString("", 0));
			}
			if (message.Contains("[حداکثر درصد تخفیف]"))
			{
				message = message.Replace("[حداکثر درصد تخفیف]", (CouponDetailList?.Max((CouponDiscountDetailBusiness q) => q.AmountPercent) ?? 0.0).NpToString("", 0));
			}
			if (message.Contains("[حداقل میزان خرید]"))
			{
				message = message.Replace("[حداقل میزان خرید]", (CouponDetailList?.Min((CouponDiscountDetailBusiness q) => q.MinBuy) ?? 0m).ToString("N0"));
			}
			if (message.Contains("[حداکثر میزان خرید]"))
			{
				message = message.Replace("[حداکثر میزان خرید]", (CouponDetailList?.Max((CouponDiscountDetailBusiness q) => q.MaxBuy) ?? 0m).ToString("N0"));
			}
			if (message.Contains("[تاریخ روز جاری]"))
			{
				message = message.Replace("[تاریخ روز جاری]", DateTime.Now.MiladiToCalture());
			}
			if (message.Contains("[عنوان فروشگاه]"))
			{
				message = message.Replace("[عنوان فروشگاه]", setting.CompanyInfo.CompanyTitle);
			}
			if (message.Contains("[تلفن فروشگاه]"))
			{
				message = message.Replace("[تلفن فروشگاه]", setting.CompanyInfo.Telephone);
			}
			return message;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillSmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 509);
			throw;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.CouponDiscount;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 10)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool semaphoreHasBeenTaken = false;
			try
			{
				await _resendToServerSemaphore.WaitAsync(token);
				if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
				{
					return ret;
				}
				semaphoreHasBeenTaken = true;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 525);
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
					return ret.AddError(EnSystem.None, "کش آنلاین نیست!!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 526);
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 527);
				}
				if (ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount is null");
				}
				List<Guid> list = (await ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 531);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 537);
						}
						CouponDiscountBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj == null)
						{
							continue;
						}
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: CouponDiscount \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 542);
							continue;
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 552);
							tasks.Remove(t2);
						}
					}
				}
				while (tasks?.Any() ?? false)
				{
					Task<ReturnedSaveFuncInfo> t2 = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 559);
					tasks.Remove(t2);
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 562);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 564);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 567);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 568);
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
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs"))
		{
			_ = 3;
			try
			{
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				if (!cache.BufferEnable || (cache.Source != ENSource.Accounting && system != EnSystem.UserInterface))
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "CouponDiscountBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 589);
				}
				WebDiscountCoupon item = await CouponDiscountMapper.Instance.MapAsync(ContextGuid, this, new WebDiscountCoupon(), "", token);
				List<Packet> paks = WebDiscountCoupon.GenerateSaveRequest(new List<WebDiscountCoupon> { item }, 0);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 592);
				}
				using DiscountCouponWaiter waiter = new DiscountCouponWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				Task.Run(() => cache.SendData(paks, priority), token);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("DiscountCouponWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 596);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 598);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 600);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 603);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 604);
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
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		bool semaphoreHasBeenTaken = false;
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 619), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 619);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.CouponDiscount.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 621);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 623);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 626);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 627);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 632), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 632);
			}
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
		return ret;
	}

	public static Task<IEnumerable<CouponDiscountReportBusiness>> GetAllAsync(string contextGuid, CouponFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.GetAllAsync(filter, token, tranName);
	}

	public static Task<CouponDiscountBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.GetAsync(guid, token, tranName);
	}

	public Task RemoveDetail(CouponDiscountDetailBusiness coupondetail)
	{
		try
		{
			if (couponDiscountDetailList.FirstOrDefault((CouponDiscountDetailBusiness q) => q.Guid == coupondetail.Guid) == null)
			{
				return Task.CompletedTask;
			}
			Status = false;
			couponDiscountDetailList.Remove(coupondetail);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetail", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 653);
			throw;
		}
	}

	public static Task<CouponDiscountBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (!(guid == Guid.Empty))
		{
			return ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.GetFromDatabaseAsync(guid, token, tranName);
		}
		return Task.FromResult<CouponDiscountBusiness>(null);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(EnSystem system, bool sendToServer, CancellationToken token, IEnumerable<CouponDiscountBusiness> lst, Priority priority = Priority.Low, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(lst?.Any() ?? false))
			{
				return ret;
			}
			foreach (CouponDiscountBusiness item in lst)
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 668);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, null, sendToServer, priority, token, awaitOnSemaphore: true, await4Send: false, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 669);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 674);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 675);
		}
		return ret;
	}

	public static Task<IEnumerable<Guid>> GetAllOrderRelationAsync(string contextGuid, Guid tafsilGuid, DateTime orderDate, decimal orderSum, int refId, Guid? specificCouponGuid, long mobile, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.GetAllOrderRelationAsync(tafsilGuid, orderDate, orderSum, refId, specificCouponGuid, mobile, token, tranName);
	}

	private Task<CouponDiscountReportBusiness> GetAsync(string contextGuid, string title, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.GetAsync(title, token, tranName);
	}

	internal static async Task<CouponDiscountBusiness> GenerateNewAsync(string contextGuid, EnSystem system, CouponGeneratorRecord record, CancellationToken token, string tranName)
	{
		_ = 3;
		try
		{
			if ((record?.TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			DateTime startDate = record.DateM.AddMinutes(1.0);
			if (!record.BaseCoupon.IsImmediateUseAllowed)
			{
				DateTime dateTime = record.DateM.AddDays(1.0);
				startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
			}
			DateTime endDate = record.DateM.AddYears(10);
			if (record.BaseCoupon.DeadLineForUse > 0)
			{
				DateTime dateTime2 = startDate.AddDays(record.BaseCoupon.DeadLineForUse);
				endDate = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 23, 59, 59);
			}
			ClsCache.GetCache(contextGuid);
			if (record.TafsilGuid != Guid.Empty)
			{
				await TafsilBusiness.GetAsync(contextGuid, system, record.TafsilGuid, tranName, token, "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs");
			}
			CouponDiscountBusiness item = new CouponDiscountBusiness(contextGuid)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = contextGuid,
				CountCouponUse = 1,
				CountCouponUseForUser = 1,
				DateFrom = startDate,
				DateTo = endDate,
				Guid = Guid.NewGuid(),
				Mode = EnCouponMode.Coupon,
				ServerStatus = ServerStatusForClient.None,
				Modified = DateTime.Now,
				SingleUse = true,
				Status = true,
				Title = record.Title,
				Type = EnCouponType.Coupon,
				MetaData = record.BaseCoupon.MetaData
			};
			CouponDiscountDetailBusiness det = await CouponDiscountDetailBusiness.CouponDetailFactoryAsync(contextGuid, item, token, tranName);
			det.Guid = Guid.NewGuid();
			det.EnDiscountType = record.DiscountType;
			det.AmountCoupon = record.DiscountAmount;
			det.Modified = DateTime.Now;
			det.Status = true;
			det.ProductContains = true;
			det.CustomerContains = true;
			await det.AddEntityAsync(new CouponDiscountEntityRelationBusiness(contextGuid)
			{
				ContextGuid = contextGuid,
				CouponDetailGuid = det.Guid,
				EntityGuid = record.TafsilGuid,
				EntityType = EntityPart.Customer
			});
			await item.AddCouponDetailListAsync(det, token);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 749);
			throw;
		}
	}

	public async Task UpdateCashBackCouponsAsync(CancellationToken token)
	{
		await Task.Delay(new Random().Next(20000, 60000));
		while (!IsDisposed)
		{
			try
			{
				if (ClsCache.GetCache(ContextGuid) == null || CashierBusiness.GetCurrentCashierGuid(ContextGuid) == Guid.Empty)
				{
					await Task.Delay(10000);
					if (IsDisposed || ClsCache.GetCache(ContextGuid) == null || CashierBusiness.GetCurrentCashierGuid(ContextGuid) == Guid.Empty)
					{
						break;
					}
				}
				forceUpdateTask = new TaskCompletionSource<bool>();
				CouponFilter filter = new CouponFilter
				{
					ModeList = new List<EnCouponMode> { EnCouponMode.CashBack },
					Status = true
				};
				IEnumerable<CouponDiscountReportBusiness> enumerable = await GetAllAsync(ContextGuid, filter, token);
				if (enumerable != null && enumerable.Any())
				{
					CouponDiscountReportBusiness couponDiscountReportBusiness = enumerable?.SingleOrDefault();
					if (couponDiscountReportBusiness != null)
					{
						CouponDiscountBusiness coupon = await GetAsync(ContextGuid, couponDiscountReportBusiness.Guid, token);
						DateTime? startDate = null;
						if (coupon.DeadLineForUse > 0)
						{
							DateTime dateTime = DateTime.Now.AddDays(checked(-coupon.DeadLineForUse));
							startDate = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
						}
						IEnumerable<OrdersForUpdateCashBackModel> enumerable2 = await OrderBusiness.GetAllForUpdateCashBackAsync(ContextGuid, startDate, token);
						if (token.IsCancellationRequested)
						{
							break;
						}
						if (enumerable2 != null && enumerable2.Any())
						{
							enumerable2 = enumerable2.OrderBy((OrdersForUpdateCashBackModel q) => Guid.NewGuid()).ToList();
							foreach (OrdersForUpdateCashBackModel item in enumerable2)
							{
								if (item != null)
								{
									OrderBusiness orderBusiness = await OrderBusiness.GetAsync(ContextGuid, item.OrderGuid, Document.OrderSell, fromDatabase: true, token);
									if (orderBusiness != null)
									{
										await orderBusiness.CheckCashBackCouponAsync(coupon, token, null);
									}
								}
							}
						}
					}
				}
				await Task.WhenAny(new Task[2]
				{
					Task.Delay(60000),
					forceUpdateTask.Task
				});
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "UpdateCashBackCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 809);
				await Task.Delay(60000);
			}
		}
	}

	public void ForceUpdateCashBackCoupons()
	{
		forceUpdateTask?.TrySetResult(result: true);
	}

	public async Task InitCouponLooperAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				return;
			}
			await Task.Delay(300000);
			while (!token.IsCancellationRequested && !IsDisposed && !token.IsCancellationRequested)
			{
				if (ClsCache.GetCache(ContextGuid) == null || CashierBusiness.GetCurrentCashierGuid(ContextGuid) == Guid.Empty)
				{
					await Task.Delay(300000);
					if (IsDisposed || token.IsCancellationRequested)
					{
						break;
					}
					continue;
				}
				Task task = GenerateBirthdayCouponsAsync(token);
				Task task2 = GenerateCalendarCouponsAsync(token);
				Task task3 = SendRemindingSmsAsync(token);
				await Task.WhenAll(task, task2, task3);
				await Task.Delay(600000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitCouponLooperAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 840);
		}
	}

	private async Task GenerateBirthdayCouponsAsync(CancellationToken token, string tranName = "")
	{
		_ = 8;
		try
		{
			CouponFilter filter = new CouponFilter
			{
				ModeList = new List<EnCouponMode> { EnCouponMode.Birthday },
				Status = true
			};
			IEnumerable<CouponDiscountReportBusiness> enumerable = await GetAllAsync(ContextGuid, filter, token);
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			CouponDiscountBusiness birthdayCoupon = await GetAsync(ContextGuid, enumerable.Single().Guid, token, tranName);
			if (!(birthdayCoupon.CouponDetailList?.Any() ?? false))
			{
				return;
			}
			MetaDataFilter filter2 = new MetaDataFilter
			{
				EntityType = EntityPart.Customer,
				Key = ((short)9).ToString()
			};
			IEnumerable<MetaDataBusiness> data = await MetaDataBusiness.GetAllAsync(ContextGuid, filter2, token, tranName);
			if (!(data?.Any() ?? false))
			{
				return;
			}
			data = data.Where((MetaDataBusiness q) => q != null && !string.IsNullOrEmpty(q.Value) && q.Value.ParseToDate().Month == DateTime.Now.Month && q.Value.ParseToDate().Day == DateTime.Now.Day)?.ToList();
			if (!(data?.Any() ?? false))
			{
				return;
			}
			List<CouponDiscountSmsRecord> sentCoupons = await LocalSettingBusiness.GetCouponsSmsSettingAsync(ContextGuid, DateTime.Now, token, tranName);
			sentCoupons = sentCoupons?.Where((CouponDiscountSmsRecord q) => q.Mode == EnCouponMode.Birthday)?.ToList();
			if (sentCoupons?.Any() ?? false)
			{
				data = data.Where((MetaDataBusiness q) => !sentCoupons.Select((CouponDiscountSmsRecord q) => q.TafsilGuid).Contains(q.EntityGuid));
				if (!(data?.Any() ?? false))
				{
					return;
				}
			}
			List<CouponDiscountSmsRecord> tempSettingList = new List<CouponDiscountSmsRecord>();
			foreach (MetaDataBusiness item in data)
			{
				if ((item?.EntityGuid ?? Guid.Empty) == Guid.Empty)
				{
					continue;
				}
				TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, item.EntityGuid, tranName, token, "GenerateBirthdayCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs");
				if (tafsil == null)
				{
					continue;
				}
				DateTime date = item.Value.ParseToDate();
				CouponDiscountBusiness coupon = await CouponDiscountMapper.Instance.MapAsync(ContextGuid, birthdayCoupon, new CouponDiscountBusiness(ContextGuid), tranName, token);
				coupon.Guid = Guid.NewGuid();
				coupon.SingleUse = true;
				coupon.CountCouponUse = 1;
				coupon.CountCouponUseForUser = 1;
				coupon.DeadLineForUse = 0;
				coupon.Mode = EnCouponMode.Coupon;
				coupon.Version = 1;
				coupon.Title = "کوپن تولد " + tafsil.Name;
				DateTime value = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
				DateTime value2 = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
				if (birthdayCoupon.DeadLineForUse > 0)
				{
					DateTime dateTime = value.AddDays(birthdayCoupon.DeadLineForUse);
					value2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
				}
				coupon.DateFrom = value;
				coupon.DateTo = value2;
				if (!(coupon.CouponDetailList?.Any() ?? false))
				{
					continue;
				}
				foreach (CouponDiscountDetailBusiness couponDetail in coupon.CouponDetailList)
				{
					if (couponDetail != null)
					{
						couponDetail.Guid = Guid.NewGuid();
						couponDetail.CustomerContains = true;
						couponDetail.ProductContains = true;
						couponDetail.ClearCustomerList();
						await couponDetail.AddEntityAsync(new CouponDiscountEntityRelationBusiness(ContextGuid)
						{
							ContextGuid = ContextGuid,
							CouponDetailGuid = couponDetail.Guid,
							EntityGuid = item.EntityGuid,
							EntityType = EntityPart.Customer
						});
					}
				}
				if (!(await coupon.SaveAsync(EnSystem.None, null, sendToServer: true, Priority.Low, token, awaitOnSemaphore: true, await4Send: false, tranName)).HasError)
				{
					tempSettingList.Add(new CouponDiscountSmsRecord
					{
						Mode = EnCouponMode.Birthday,
						TafsilGuid = item.EntityGuid
					});
				}
			}
			if (tempSettingList?.Any() ?? false)
			{
				await LocalSettingBusiness.SetCouponDiscountSmsSettingAsync(ContextGuid, DateTime.Now, tempSettingList);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateBirthdayCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 937);
			throw;
		}
	}

	private async Task GenerateCalendarCouponsAsync(CancellationToken token, string tranName = "")
	{
		_ = 1;
		try
		{
			List<OccasionCalendarRecord> list = (await GlobalReportBusiness.GetOccasionCalendarRecordListAsync(ContextGuid, token))?.Where((OccasionCalendarRecord q) => (q?.Model?.BaseCouponDiscountGuid ?? Guid.Empty) != Guid.Empty)?.ToList();
			if (list == null || !list.Any())
			{
				return;
			}
			IEnumerable<IGrouping<Guid?, OccasionCalendarRecord>> enumerable = from q in list
				group q by q.Model.BaseCouponDiscountGuid;
			if (enumerable == null || !enumerable.Any())
			{
				return;
			}
			foreach (IGrouping<Guid?, OccasionCalendarRecord> item in enumerable)
			{
				if (!((item?.Key ?? Guid.Empty) == Guid.Empty))
				{
					await GenerateCalendarCouponsAsync(item.Key.Value, item.ToList(), token, tranName);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateCalendarCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 959);
			throw;
		}
	}

	private async Task GenerateCalendarCouponsAsync(Guid couponGuid, List<OccasionCalendarRecord> data, CancellationToken token, string tranName)
	{
		_ = 6;
		try
		{
			if (couponGuid == Guid.Empty || !(data?.Any() ?? false))
			{
				return;
			}
			CouponDiscountBusiness baseCoupon = await GetAsync(ContextGuid, couponGuid, token, tranName);
			if (!(baseCoupon.CouponDetailList?.Any() ?? false))
			{
				return;
			}
			int year = Calendar.GetYearOfDateSh(DateTime.Now.MiladiToCalture());
			int mount = Calendar.GetMonthOfDate(DateTime.Now.MiladiToCalture());
			int day = Calendar.GetDayOfDateSh(DateTime.Now.MiladiToCalture());
			data = data.Where((OccasionCalendarRecord q) => !string.IsNullOrEmpty(q.Model?.Value) && Calendar.GetMonthOfDate(q.Model.Value) == mount && Calendar.GetDayOfDateSh(q.Model.Value) == day)?.ToList();
			if (!(data?.Any() ?? false))
			{
				return;
			}
			List<CouponDiscountSmsRecord> sentCoupons = await LocalSettingBusiness.GetCouponsSmsSettingAsync(ContextGuid, DateTime.Now, baseCoupon.Guid, token, tranName);
			if (sentCoupons?.Any() ?? false)
			{
				data = data.Where((OccasionCalendarRecord q) => !sentCoupons.Select((CouponDiscountSmsRecord q) => q.TafsilGuid).Contains(q.ParentGuid))?.ToList();
				if (!(data?.Any() ?? false))
				{
					return;
				}
			}
			List<CouponDiscountSmsRecord> tempSettingList = new List<CouponDiscountSmsRecord>();
			foreach (OccasionCalendarRecord item in data)
			{
				TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, EnSystem.None, item.ParentGuid, tranName, token, "GenerateCalendarCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs");
				if (tafsil == null)
				{
					continue;
				}
				DateTime date = $"{year}/{mount}/{day}".ShamsiToMiladi();
				CouponDiscountBusiness coupon = await CouponDiscountMapper.Instance.MapAsync(ContextGuid, baseCoupon, new CouponDiscountBusiness(ContextGuid), tranName, token);
				coupon.Guid = Guid.NewGuid();
				coupon.SingleUse = true;
				coupon.CountCouponUse = 1;
				coupon.CountCouponUseForUser = 1;
				coupon.DeadLineForUse = 0;
				coupon.Mode = EnCouponMode.Coupon;
				coupon.Version = 1;
				coupon.Title = baseCoupon.Title + " - " + tafsil.Name;
				DateTime value = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
				DateTime value2 = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
				if (baseCoupon.DeadLineForUse > 0)
				{
					DateTime dateTime = value.AddDays(baseCoupon.DeadLineForUse);
					value2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
				}
				coupon.DateFrom = value;
				coupon.DateTo = value2;
				if (!(coupon.CouponDetailList?.Any() ?? false))
				{
					continue;
				}
				foreach (CouponDiscountDetailBusiness couponDetail in coupon.CouponDetailList)
				{
					if (couponDetail != null)
					{
						couponDetail.Guid = Guid.NewGuid();
						couponDetail.CustomerContains = true;
						couponDetail.ProductContains = true;
						couponDetail.ClearCustomerList();
						await couponDetail.AddEntityAsync(new CouponDiscountEntityRelationBusiness(ContextGuid)
						{
							ContextGuid = ContextGuid,
							CouponDetailGuid = couponDetail.Guid,
							EntityGuid = item.ParentGuid,
							EntityType = EntityPart.Customer
						});
					}
				}
				if (!(await coupon.SaveAsync(EnSystem.None, null, sendToServer: true, Priority.Low, token, awaitOnSemaphore: true, await4Send: false, tranName)).HasError)
				{
					tempSettingList.Add(new CouponDiscountSmsRecord
					{
						Mode = baseCoupon.Mode,
						TafsilGuid = item.ParentGuid
					});
				}
			}
			if (tempSettingList?.Any() ?? false)
			{
				await LocalSettingBusiness.SetCouponDiscountSmsSettingAsync(ContextGuid, DateTime.Now, baseCoupon.Guid, tempSettingList);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateCalendarCouponsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1044);
		}
	}

	private async Task SendRemindingSmsAsync(CancellationToken token, string tranName = "")
	{
		_ = 4;
		try
		{
			IEnumerable<Guid> list = await ClsCache.GetCache(ContextGuid).CacheRepository.CouponDiscount.GetAllForScheduleAsync(token, tranName);
			if (!(list?.Any() ?? false))
			{
				return;
			}
			List<CouponDiscountReminderSmsRecord> tempSettingList = new List<CouponDiscountReminderSmsRecord>();
			List<CouponDiscountReminderSmsRecord> sentCoupons = await LocalSettingBusiness.GetCouponsReminderSmsSettingAsync(ContextGuid, DateTime.Now.Date, token, tranName);
			foreach (Guid couponGuid in list)
			{
				if (couponGuid == Guid.Empty)
				{
					continue;
				}
				CouponDiscountBusiness coupon = await GetAsync(ContextGuid, couponGuid, token, tranName);
				if (!(coupon?.MetaData?.CouponSmsSettingsList?.Any()).GetValueOrDefault() || !(coupon?.CouponDetailList?.Any()).GetValueOrDefault() || !coupon.DateFrom.HasValue || !coupon.DateTo.HasValue || !coupon.Status)
				{
					continue;
				}
				List<CouponSmsSetting> list2 = coupon.MetaData.CouponSmsSettingsList.Where((CouponSmsSetting q) => q.DayCount > 0)?.ToList();
				if (list2 == null || !list2.Any())
				{
					continue;
				}
				foreach (CouponSmsSetting metaData in list2)
				{
					if (metaData == null || metaData.Status == PayamakSendStatus.Disable || ((sentCoupons?.Any() ?? false) && sentCoupons.FirstOrDefault((CouponDiscountReminderSmsRecord q) => q.MetaDataGuid == metaData.Guid && q.CouponGuid == couponGuid) != null))
					{
						continue;
					}
					DateTime? dateTime = null;
					if (!metaData.CalculationWithEndDate)
					{
						dateTime = coupon.DateFrom.Value.AddDays(metaData.DayCount);
					}
					else if (metaData.CalculationWithEndDate)
					{
						dateTime = coupon.DateTo.Value.AddDays(checked(-metaData.DayCount));
					}
					if (dateTime.HasValue)
					{
						if (dateTime.Value.Date < DateTime.Now.Date)
						{
							tempSettingList.Add(new CouponDiscountReminderSmsRecord
							{
								CouponGuid = couponGuid,
								MetaDataGuid = metaData.Guid
							});
						}
						else if (!(dateTime.Value.Date > DateTime.Now.Date))
						{
							await coupon.SendMessageAsync(EnSystem.None, null, null, new List<CouponSmsSetting> { metaData }, token);
							tempSettingList.Add(new CouponDiscountReminderSmsRecord
							{
								CouponGuid = couponGuid,
								MetaDataGuid = metaData.Guid
							});
						}
					}
				}
			}
			if (tempSettingList?.Any() ?? false)
			{
				await LocalSettingBusiness.SetCouponDiscountReminderSmsSettingAsync(ContextGuid, DateTime.Now.Date, tempSettingList);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendRemindingSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1108);
			throw;
		}
	}

	internal static async Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(string contextGuid, List<Guid> couponGuids, ServerStatusForClient status, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1120), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1120);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.UpdateServerStatusAsync(couponGuids, status, token, tranName), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1122);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1126);
				res.AddReturnedValue(ex, "", "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1127);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1132), ReturnedState.None, "UpdateServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1132);
			}
		}
		return res;
	}

	internal static async Task<ReturnedSaveFuncInfo> UpdateServerStatusByGiftGuidAsync(string contextGuid, Guid giftGuid, ServerStatusForClient status, CancellationToken token, string tranName)
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1144), ReturnedState.None, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1144);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.CouponDiscount.UpdateServerStatusByGiftGuidAsync(giftGuid, status, token, tranName), ReturnedState.None, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1146);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1150);
				res.AddReturnedValue(ex, "", "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1151);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1156), ReturnedState.None, "UpdateServerStatusByGiftGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\DiscountCoupon\\CouponDiscountBusiness.cs", 1156);
			}
		}
		return res;
	}
}
