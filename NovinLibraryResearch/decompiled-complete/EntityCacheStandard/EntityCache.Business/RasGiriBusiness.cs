using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.RasGiri;
using EntityCache.Business.Accounting.RasGiri.Calculator;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class RasGiriBusiness : Serializable<RasGiriBusiness>, IRasGiri, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private DateTime baseDateM;

	private DateTime dateM;

	private decimal sood_Percent;

	private List<RasGiriDetBusiness> _Items;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public string Title { get; private set; }

	public string Description { get; private set; }

	[JsonIgnore]
	public IReadOnlyCollection<RasGiriDetBusiness> Items
	{
		get
		{
			if (_Items == null)
			{
				_Items = new List<RasGiriDetBusiness>();
			}
			_Items.RemoveAll((RasGiriDetBusiness p) => p?.IsDisposed ?? true);
			return _Items;
		}
	}

	public DateTime DateM
	{
		get
		{
			return dateM;
		}
		private set
		{
			dateM = value;
			IReadOnlyCollection<RasGiriDetBusiness> items = Items;
			if (items == null || !items.Any())
			{
				return;
			}
			foreach (RasGiriDetBusiness item in Items)
			{
				item.EnherafRasDate = value;
			}
		}
	}

	public string DateSh => dateM.MiladiToCalture();

	public DateTime BaseDateM
	{
		get
		{
			return baseDateM;
		}
		set
		{
			baseDateM = value;
			IReadOnlyCollection<RasGiriDetBusiness> items = Items;
			if (items != null && items.Any())
			{
				foreach (RasGiriDetBusiness item in Items)
				{
					item.EnherafBaseDate = value;
				}
			}
			Calc();
		}
	}

	public string BaseDateSh => BaseDateM.MiladiToCalture();

	public int DayCount { get; private set; }

	public int RecordCount { get; private set; }

	public decimal Sood_Mounthly { get; private set; }

	public decimal Mounth { get; private set; }

	public decimal Sood_Total { get; private set; }

	public decimal Sood_Percent
	{
		get
		{
			return sood_Percent;
		}
		set
		{
			sood_Percent = value;
			Calc();
		}
	}

	public decimal Total_Sum { get; private set; }

	public Guid TafsilGuid { get; set; }

	public Guid SanadGuid { get; set; }

	public SanadSt SanadType { get; set; }

	public string ContextGuid { get; set; }

	public EnRasGiriType RasGiriType { get; set; }

	public IRasGiriBusinesCalculator RasgiriCalculator => RasGiriType switch
	{
		EnRasGiriType.Standard => new RasGiriStandardCalculator(ContextGuid, _Items), 
		EnRasGiriType.With_Heading_Up => new RasGiri_With_Heading_Up_Calculator(ContextGuid, _Items), 
		_ => throw new NotImplementedException(), 
	};

	public EntityChangeSt ChangeStatus { get; set; }

	private RasGiriBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.RasGiriBusiness;

	public event Func<object, EventArgs, Task> OnDetsChanged;

	public event Func<object, EventArgs, Task> OnItemDisposed;

	public RasGiriBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public static async Task<RasGiriBusiness> FactoryAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		RasGiriBusiness ret = new RasGiriBusiness(contextGuid);
		try
		{
			ret.Sood_Percent = (decimal)(await CashierBusiness.GetCurrentCashierAsync(contextGuid, token)).Settings.OtherSettings.Bahre;
			ret.DateM = DateTime.Now;
			ret.BaseDateM = DateTime.Now;
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 124);
			throw;
		}
	}

	public void AddItem(RasGiriDetBusiness item, CancellationToken token = default(CancellationToken))
	{
		if (item == null)
		{
			throw new ArgumentNullException();
		}
		if (item.IsDisposed)
		{
			throw new ObjectDisposedException("item");
		}
		try
		{
			if (_Items == null)
			{
				_Items = new List<RasGiriDetBusiness>();
			}
			_Items.Add(item);
			item.OnDisposed += Item_OnDisposed;
			item.OnDateChanged += Item_OnDateChanged;
			item.OnPriceChanged += Item_OnPriceChanged;
			BaseDateM = baseDateM;
			Calc();
			if (EventManager?.OnDetsChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnDetsChanged, this, new EventArgs(), token, 60, "AddItem", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddItem", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 145);
		}
	}

	private Task Item_OnDateChanged(object arg1, DateTime arg2)
	{
		try
		{
			Calc();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Item_OnDateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 153);
		}
		return Task.CompletedTask;
	}

	private Task Item_OnPriceChanged(object arg1, decimal arg2)
	{
		try
		{
			Calc();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Item_OnPriceChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 162);
		}
		return Task.CompletedTask;
	}

	protected void Calc()
	{
		checked
		{
			try
			{
				DayCount = RasgiriCalculator.CalCulateRasDaysCalculator();
				DateM = BaseDateM.AddDays(DayCount);
				Total_Sum = Items?.Sum((RasGiriDetBusiness q) => q.Price) ?? 0m;
				Sood_Total = RasgiriCalculator.CalcTotalSoodCalculator(Sood_Percent);
				if (DayCount < 0)
				{
					DayCount *= -1;
				}
				if (DayCount != 0 && DayCount >= 30)
				{
					Mounth = (decimal)DayCount / 30m;
					Sood_Mounthly = Sood_Total / Mounth;
				}
				else
				{
					Mounth = 0m;
					Sood_Mounthly = Sood_Total;
				}
				Description = "اسناد انتخاب شده معادل یک سند به تاریخ".GetTranslate() + " " + (" " + DateSh).GetTranslate() + " " + " به مبلغ".GetTranslate() + $" {Total_Sum:N0}\r\n" + " با سود مشارکت".GetTranslate() + $" {Sood_Total:N0}".GetTranslate() + " برابر".GetTranslate() + $" {Total_Sum + Sood_Total:N0}".GetTranslate() + " میباشد".GetTranslate();
				Title = Description;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Calc", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 187);
			}
		}
	}

	private Task Item_OnDisposed(object item, EventArgs e)
	{
		try
		{
			_Items?.Remove((RasGiriDetBusiness)item);
			Calc();
			((RasGiriDetBusiness)item).OnDisposed -= Item_OnDisposed;
			((RasGiriDetBusiness)item).OnDateChanged -= Item_OnDateChanged;
			((RasGiriDetBusiness)item).OnPriceChanged -= Item_OnPriceChanged;
			if (EventManager?.OnItemDisposed != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnItemDisposed, this, new EventArgs(), default(CancellationToken), 60, "Item_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Item_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 203);
		}
		return Task.CompletedTask;
	}

	public static Task<RasGiriBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IRasGiri.GetAsync(guid, token, tranName);
	}

	public static Task<IEnumerable<RasGiriBusiness>> GetAllAsync(string contextGuid, bool? status = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IRasGiri.GetAllAsync(status, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "RasGiriBusiness/SaveAsync", 60000, token))
				{
					if (!dis.Token.IsCancellationRequested)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo;
						if (autoTran)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 220), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 220);
						}
						transactionHasBeenCreated = true;
						Modified = DateTime.Now;
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IRasGiri.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 223);
						goto end_IL_011b;
					}
					result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 219);
					goto end_IL_007d;
					end_IL_011b:;
				}
				goto end_IL_006c;
				end_IL_007d:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 227);
				throw;
			}
			return result;
			end_IL_006c:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 233), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 233);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 2;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 245), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 245);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IRasGiriDet.RemoveAllByMasterGuidAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 247);
				if (ret.HasError)
				{
					return ret;
				}
				Modified = DateTime.Now;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IRasGiri.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 250);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 254);
				throw;
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 260), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 260);
			}
		}
		return ret;
	}

	public static Task<RasGiriBusiness> GenerateRasGiriFromGardeshHesabAsync(string contextGuid, List<GardeshHesabBusiness> lstgardeshHesab)
	{
		try
		{
			RasGiriBusiness rasGiriBusiness = new RasGiriBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				Status = true,
				BaseDateM = DateTime.Now
			};
			bool flag = false;
			foreach (GardeshHesabBusiness item in lstgardeshHesab)
			{
				if (item.Ischecked && (!(item.Debit <= 0m) || !(item.Credit <= 0m)))
				{
					RasGiriDetBusiness rasGiriDetBusiness = new RasGiriDetBusiness(contextGuid)
					{
						DateM = item.Date,
						Price = item.Debit - item.Credit,
						Description = item.AutoDescriptionShort,
						Guid = Guid.NewGuid(),
						MasterGuid = rasGiriBusiness.Guid
					};
					flag = true;
					if (item.AsType == Document.Reception || item.AsType == Document.Pardakht)
					{
						rasGiriDetBusiness.DateM = item.DateMoaser;
					}
					rasGiriBusiness.AddItem(rasGiriDetBusiness);
				}
			}
			if (!flag)
			{
				foreach (GardeshHesabBusiness item2 in lstgardeshHesab)
				{
					if (!(item2.Debit <= 0m) || !(item2.Credit <= 0m))
					{
						RasGiriDetBusiness rasGiriDetBusiness2 = new RasGiriDetBusiness(contextGuid)
						{
							DateM = item2.Date,
							Price = item2.Debit - item2.Credit,
							Description = item2.DescForDisplay,
							Guid = Guid.NewGuid(),
							MasterGuid = rasGiriBusiness.Guid
						};
						flag = true;
						if (item2.AsType == Document.Reception || item2.AsType == Document.Pardakht)
						{
							rasGiriDetBusiness2.DateM = item2.DateMoaser;
						}
						rasGiriBusiness.AddItem(rasGiriDetBusiness2);
					}
				}
			}
			return Task.FromResult(rasGiriBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateRasGiriFromGardeshHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 313);
			throw;
		}
	}

	public static Task<RasGiriBusiness> GenerateRasGiriFromGardeshTafsilAsync(string contextGuid, List<GardeshTafsilBusiness> lstTafsilbind)
	{
		try
		{
			RasGiriBusiness rasGiriBusiness = new RasGiriBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				Status = true
			};
			bool flag = false;
			foreach (GardeshTafsilBusiness item3 in lstTafsilbind)
			{
				if (item3.IsChecked && (!(item3.Debit <= 0m) || !(item3.Credit <= 0m)))
				{
					RasGiriDetBusiness item = new RasGiriDetBusiness(contextGuid)
					{
						DateM = item3.DateMoaser,
						Price = item3.Debit - item3.Credit,
						Description = ((!string.IsNullOrEmpty(item3.Description)) ? item3.Description : item3.DescForDisplay),
						Guid = Guid.NewGuid(),
						MasterGuid = rasGiriBusiness.Guid
					};
					flag = true;
					rasGiriBusiness.AddItem(item);
					rasGiriBusiness.BaseDateM = DateTime.Now;
				}
			}
			if (!flag)
			{
				foreach (GardeshTafsilBusiness item4 in lstTafsilbind)
				{
					if (!(item4.Debit <= 0m) || !(item4.Credit <= 0m))
					{
						RasGiriDetBusiness item2 = new RasGiriDetBusiness(contextGuid)
						{
							DateM = item4.DateMoaser,
							Price = item4.Debit - item4.Credit,
							Description = ((!string.IsNullOrEmpty(item4.Description)) ? item4.Description : item4.DescForDisplay),
							Guid = Guid.NewGuid(),
							MasterGuid = rasGiriBusiness.Guid
						};
						flag = true;
						rasGiriBusiness.AddItem(item2);
						rasGiriBusiness.BaseDateM = DateTime.Now;
					}
				}
			}
			return Task.FromResult(rasGiriBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateRasGiriFromGardeshTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\RasGiri\\RasGiriBusiness.cs", 365);
			throw;
		}
	}
}
