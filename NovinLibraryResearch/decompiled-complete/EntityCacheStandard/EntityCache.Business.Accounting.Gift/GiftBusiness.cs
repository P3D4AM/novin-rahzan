using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.DiscountCoupon;
using EntityCache.Business.Core;
using EntityCache.Core;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Gift;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;

namespace EntityCache.Business.Accounting.Gift;

public class GiftBusiness : BaseBusiness<GiftBusiness>, IGift, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, ISyncFromClient2Server<GiftBusiness>, IDeliverableEntity, IEditableDelivery
{
	private static SemaphoreSlim databaseSemaphor = new SemaphoreSlim(1, 1);

	private static CancellationTokenSource _sendEntitiesToServerToken;

	private List<GiftDetailBusiness> detailList;

	private bool _isDisposed;

	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public override string ContextGuid { get; set; }

	public string Title { get; set; }

	public EnTypeChose Type { get; set; }

	public string Discription { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public DateTime Modified { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public bool Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public int DeliveryId { get; set; }

	public GiftMetaData MetaData { get; set; }

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public IReadOnlyCollection<GiftDetailBusiness> DetailList
	{
		get
		{
			if (detailList == null)
			{
				detailList = new List<GiftDetailBusiness>();
			}
			detailList.RemoveAll((GiftDetailBusiness q) => q?.IsDisposed ?? true);
			return detailList;
		}
	}

	public event Func<object, Guid, Task> OnDetailChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public GiftBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autotran = string.IsNullOrEmpty(tranName);
		if (autotran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool trantionBeenCreate = false;
		ClsCache cache = ClsCache.GetCache(ContextGuid);
		_sendEntitiesToServerToken?.Cancel();
		_sendEntitiesToServerToken = new CancellationTokenSource();
		CancellationToken sendEntitiesToServerToken = CancellationTokenSource.CreateLinkedTokenSource(token, _sendEntitiesToServerToken.Token).Token;
		try
		{
			_ = 4;
			ReturnedSaveFuncInfo result;
			try
			{
				using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(true, "GiftBusiness/SaveAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 71);
					}
					if (autotran)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await cache.AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 72), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 72);
					}
					trantionBeenCreate = true;
					if (res.HasError)
					{
						result = res;
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 75);
						if (res.HasError)
						{
							result = res;
						}
						else
						{
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await cache.CacheRepository.Gift.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 77);
							if (!res.HasError && system == EnSystem.UserInterface)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await CouponDiscountBusiness.UpdateServerStatusByGiftGuidAsync(ContextGuid, Guid, ServerStatusForClient.None, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 79);
								goto end_IL_018f;
							}
							result = res;
						}
					}
					goto end_IL_00ec;
					end_IL_018f:;
				}
				goto end_IL_00db;
				end_IL_00ec:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 83);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 84);
				goto end_IL_00db;
			}
			return result;
			end_IL_00db:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 89), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 89);
			}
			if (cache.BufferEnable && cache.Source == ENSource.Accounting && system != EnSystem.Transfer)
			{
				Task.Run(() => SendEntitiesToServerAsync(sendEntitiesToServerToken));
			}
		}
		return res;
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(500);
			if (!token.IsCancellationRequested)
			{
				await CouponDiscountBusiness.ResendNotSentsAsync(ContextGuid, token, "", 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 104);
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Title))
			{
				await res.AddErrorAsync("عنوان هدیه را وارد کنید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 111);
			}
			if (!(DetailList?.Any() ?? false))
			{
				await res.AddErrorAsync("حداقل یک کالا را به عنوان هدیه انتخاب کنید.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 113);
			}
			if (Status && Type == EnTypeChose.GiftPackageForProduct)
			{
				decimal num = MetaData?.MaxGiftCount ?? 0m;
				decimal num2 = DetailList?.Sum((GiftDetailBusiness q) => q.Count) ?? 0m;
				if (num > 0m && num2 > 0m && num2 > num)
				{
					await res.AddErrorAsync("تعداد کالای هدیه از تعداد مجاز بیشتر است", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 120);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 125);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 126);
		}
		return res;
	}

	public static Task<IEnumerable<GiftBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Gift.GetAllAsync(status, token, tranName);
	}

	public static Task<GiftBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Gift.GetAsync(guid, token, tranName);
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		throw new NotImplementedException();
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.Gift;
	}

	public override void Dispose(bool disposing)
	{
		using (new JaegerMonitor("Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs"))
		{
			try
			{
				if (_isDisposed)
				{
					return;
				}
				_isDisposed = true;
				foreach (GiftDetailBusiness item in detailList?.ToList() ?? new List<GiftDetailBusiness>())
				{
					item?.Dispose();
				}
				base.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 150);
			}
		}
	}

	public async Task AddRangeToDetailListAsync(List<GiftDetailBusiness> list, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (GiftDetailBusiness item in list)
			{
				if (item != null)
				{
					if (detailList?.FirstOrDefault((GiftDetailBusiness q) => q.ProductGuid == item.ProductGuid) != null)
					{
						await RemoveDetailAsync(item.ProductGuid, token);
					}
					item.ContextGuid = ContextGuid;
					if (string.IsNullOrEmpty(item?.Name))
					{
						await item.LoadProductAsync(token);
					}
					await AddDetailListAsync(item, token);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 171);
		}
	}

	public Task AddDetailListAsync(GiftDetailBusiness detail, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (detail == null)
			{
				return Task.CompletedTask;
			}
			if (detailList == null)
			{
				detailList = new List<GiftDetailBusiness>();
			}
			detail.OnDisposed += Det_OnDisposed;
			detailList.Add(detail);
			if (this.OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnDetailChanged, this, detail.ProductGuid, token, 60, "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 185);
		}
		return Task.CompletedTask;
	}

	private Task Det_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs"))
		{
			try
			{
				if ((detailList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (GiftDetailBusiness item in detailList?.ToList()?.Where((GiftDetailBusiness p) => p == null || p == sender)?.ToList() ?? new List<GiftDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				detailList?.RemoveAll((GiftDetailBusiness p) => p == null || p == sender);
				if (this.OnItemDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Det_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 206);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveDetailAsync(Guid productGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			GiftDetailBusiness giftDetailBusiness = detailList?.FirstOrDefault((GiftDetailBusiness q) => q.ProductGuid == productGuid);
			if (giftDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			detailList.Remove(giftDetailBusiness);
			if (this.OnDetailChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnDetailChanged, this, productGuid, token, 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Gift\\GiftBusiness.cs", 222);
		}
		return Task.CompletedTask;
	}
}
