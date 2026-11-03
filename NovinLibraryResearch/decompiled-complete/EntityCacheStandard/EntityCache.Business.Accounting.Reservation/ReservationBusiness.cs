using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Reservation;
using PacketParser.Filters.Hotel;

namespace EntityCache.Business.Accounting.Reservation;

public class ReservationBusiness : IReservation, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private CashierBusiness _cashier;

	private List<ReservationDetailBusiness> _detList;

	private static SemaphoreSlim databaseSemaphor = new SemaphoreSlim(1, 1);

	public Guid Guid { get; set; }

	public bool Status { get; set; }

	public DateTime Modified { get; set; }

	public DateTime DateM { get; set; }

	public int Number { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public IReadOnlyCollection<ReservationDetailBusiness> DetailList
	{
		get
		{
			if (_detList == null)
			{
				_detList = new List<ReservationDetailBusiness>();
			}
			_detList.RemoveAll((ReservationDetailBusiness q) => q?.IsDisposed ?? true);
			return _detList;
		}
	}

	public ReservationBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.Reservation;

	public event Func<object, Guid, Task> OnDetailAdd;

	public event Func<object, EventArgs, Task> OnDetailDisposed;

	public ReservationBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public static Task<ReservationBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reservation.GetAsync(guid, token, tranName);
	}

	public static Task<ReservationBusiness> GetAsync(string contextGuid, int number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reservation.GetAsync(number, token, tranName);
	}

	public static Task<IEnumerable<ReservationReportBusiness>> GetAllAsync(string contextGuid, ReservationFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reservation.GetAllModelAsync(filter, token, tranName);
	}

	public static async Task<ReservationBusiness> GenerateNewAsync(string contextGuid, Guid? tafsilGuid = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			ReservationBusiness reservationBusiness = new ReservationBusiness(contextGuid)
			{
				ContextGuid = contextGuid,
				Guid = Guid.NewGuid(),
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				Status = true,
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			ReservationBusiness reservationBusiness2 = reservationBusiness;
			reservationBusiness2.Number = await GetNextNumberAsync(contextGuid, token, tranName);
			ReservationBusiness item = reservationBusiness;
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				item.TafsilGuid = tafsilGuid.Value;
			}
			else
			{
				tafsilGuid = await (await CashierBusiness.GetCurrentCashierAsync(contextGuid, token, tranName)).GetCusGuidAsync(EnSystem.UserInterface);
				if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
				{
					item.TafsilGuid = tafsilGuid.Value;
				}
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 87);
			throw;
		}
	}

	public async Task<string> GetCashierNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (CashierGuid == Guid.Empty)
			{
				return "";
			}
			if (_cashier == null)
			{
				_cashier = await CashierBusiness.GetAsync(ContextGuid, CashierGuid, token, tranName);
			}
			return _cashier?.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 101);
			throw;
		}
	}

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (TafsilGuid == Guid.Empty)
			{
				return null;
			}
			return await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 115);
			throw;
		}
	}

	public Task AddDetailAsync(ReservationDetailBusiness data, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (data == null)
			{
				return Task.CompletedTask;
			}
			if (_detList == null)
			{
				_detList = new List<ReservationDetailBusiness>();
			}
			data.MasterGuid = Guid;
			data.OnDisposed += Detail_OnDisposed;
			_detList.Add(data);
			if (EventManager.OnDetailAdd != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetailAdd, this, data.Guid, token, 60, "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 131);
		}
		return Task.CompletedTask;
	}

	public async Task AddDetailListAsync(List<ReservationDetailBusiness> list, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(list?.Any() ?? false))
			{
				return;
			}
			foreach (ReservationDetailBusiness item in list)
			{
				await AddDetailAsync(item, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 141);
		}
	}

	private Task Detail_OnDisposed(object sender, EventArgs e)
	{
		using (new JaegerMonitor("Detail_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs"))
		{
			try
			{
				if ((_detList?.ToList()?.Any()).GetValueOrDefault())
				{
					foreach (ReservationDetailBusiness item in _detList?.ToList()?.Where((ReservationDetailBusiness p) => p == null || p == sender)?.ToList() ?? new List<ReservationDetailBusiness>())
					{
						item?.Dispose();
					}
				}
				_detList?.RemoveAll((ReservationDetailBusiness p) => p == null || p == sender);
				if (EventManager.OnDetailDisposed != null)
				{
					PacketParser.Utilities.RaiseEvent(EventManager.OnDetailDisposed, this, new EventArgs(), default(CancellationToken), 60, "Detail_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Detail_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 161);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public Task RemoveDetailAsync(Guid detGuid, CancellationToken token = default(CancellationToken))
	{
		try
		{
			ReservationDetailBusiness reservationDetailBusiness = _detList?.FirstOrDefault((ReservationDetailBusiness q) => q.Guid == detGuid);
			if (reservationDetailBusiness == null)
			{
				return Task.CompletedTask;
			}
			_detList.Remove(reservationDetailBusiness);
			if (EventManager.OnDetailDisposed != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetailDisposed, this, new EventArgs(), default(CancellationToken), 60, "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 176);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
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
			_ = 3;
			ReturnedSaveFuncInfo result;
			try
			{
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				Modified = DateTime.Now;
				using (SemaphoreUtil dis = await databaseSemaphor.GaurdAsync(awaitOnSemaphore, "ReservationBusiness/SaveAsync", 6000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						res.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 191);
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autotran)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 192), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 192);
					}
					trantionBeenCreate = true;
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 194);
					if (!res.HasError)
					{
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.Reservation.SaveAsync(this, 1, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 196);
						goto end_IL_014c;
					}
					result = res;
					goto end_IL_0080;
					end_IL_014c:;
				}
				goto end_IL_006c;
				end_IL_0080:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 200);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 201);
				goto end_IL_006c;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (trantionBeenCreate && autotran)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 206), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 206);
			}
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!Status)
			{
				return res;
			}
			if (CashierGuid == Guid.Empty)
			{
				CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid);
			}
			if (TafsilGuid == Guid.Empty)
			{
				await res.AddErrorAsync("طرف حساب صورتحساب را انتخاب نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 217);
			}
			if (Number <= 0)
			{
				await res.AddErrorAsync("شماره رزرو را وارد نمایید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 218);
			}
			else
			{
				ReservationBusiness reservationBusiness = await GetAsync(ContextGuid, Number, token, tranName);
				if (reservationBusiness != null && reservationBusiness.Guid != Guid)
				{
					await res.AddErrorAsync("شماره".GetTranslate() + " " + $" {Number}".GetTranslate() + " " + " تکراری می باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 222);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 227);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Reservation\\ReservationBusiness.cs", 228);
		}
		return res;
	}

	public static Task<int> GetNextNumberAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reservation.GetNextNumberAsync(token, tranName);
	}
}
