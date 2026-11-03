using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Customers;
using EntityCache.Core.Repository;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters.Customer;
using PacketParser.Settings.Accounting.UserSettings;

namespace EntityCache.Business;

public class CusCompactBusiness : Serializable<CusCompactBusiness>, ICusCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext, INotifyPropertyChanged
{
	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private TafsilBusiness _tafsil;

	private CusBusiness _custBusiness;

	private decimal _account;

	private decimal _sumAghsat;

	private string _address1;

	private string _address2;

	private string _zip;

	private string _description;

	private string _picFace;

	private string _picSign;

	private CusTitleBusiness _title;

	private string _titleName;

	private CancellationTokenSource _tokenTitle;

	private static TaskCompletionSource<Tuple<bool, string>> passRevoverResponseCompletionSource;

	public string Address1
	{
		get
		{
			return _address1 ?? "";
		}
		set
		{
			_address1 = value?.FixString() ?? "";
		}
	}

	public string Address2
	{
		get
		{
			return _address2 ?? "";
		}
		set
		{
			_address2 = value?.FixString() ?? "";
		}
	}

	public string FullName => Name + " " + Family;

	public string Zip
	{
		get
		{
			return _zip;
		}
		set
		{
			_zip = value.FixString();
		}
	}

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value.FixString();
		}
	}

	[Obsolete("Use CustomerRole", true)]
	public EnPriceType DefPrice { get; set; }

	[JsonIgnore]
	public CusBusiness FullCustomer => _custBusiness ?? (_custBusiness = CusBusiness.Get(ContextGuid, Guid));

	public Guid TafsilGroupGuid { get; set; }

	[JsonIgnore]
	public TafsilBusiness Tafsil => GetTafsil();

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public string Family { get; set; }

	public bool Status { get; set; }

	public long Code { get; set; }

	public decimal Account
	{
		get
		{
			return _account;
		}
		set
		{
			_account = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Account"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AggregatedAccount"));
				}, "Account", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs");
			}
		}
	}

	public decimal SumAghsat
	{
		get
		{
			return _sumAghsat;
		}
		set
		{
			_sumAghsat = value;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Account"));
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AggregatedAccount"));
				}, "SumAghsat", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs");
			}
		}
	}

	public decimal AggregatedAccount => Account - SumAghsat;

	public bool ShowDebitCustomerWithoutInstallment => AggregatedAccount < 0m;

	public bool ShowCreditCustomersAfterInstallment
	{
		get
		{
			if (AggregatedAccount > 0m)
			{
				return SumAghsat != 0m;
			}
			return false;
		}
	}

	public bool Special { get; set; }

	public bool IsChecked { get; set; }

	public string PicFace
	{
		get
		{
			try
			{
				return string.IsNullOrEmpty(_picFace) ? "NoImageCusFace.png" : _picFace;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PicFace", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 275);
				return null;
			}
		}
		set
		{
			_picFace = value.FixString();
		}
	}

	public string PicSign
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(_picSign))
				{
					return "NoImageCusSign.png";
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PicSign", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 291);
			}
			return _picSign;
		}
		set
		{
			_picSign = value?.FixString() ?? "";
		}
	}

	public Guid GroupGuid { get; set; }

	[JsonIgnore]
	public string GroupName { get; set; }

	public Guid VisitorGuid { get; set; }

	public Guid TitleGuid { get; set; }

	[JsonIgnore]
	public string TitleName
	{
		get
		{
			try
			{
				if (!string.IsNullOrEmpty(_titleName))
				{
					return _titleName;
				}
				_tokenTitle?.Cancel();
				_tokenTitle = new CancellationTokenSource(600000);
				CancellationToken token = _tokenTitle.Token;
				Task.Run(() => UpdateTitleAsync(token));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "TitleName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 347);
			}
			return _titleName;
		}
	}

	public bool IsBazaryab { get; set; }

	public bool IsSupervisor { get; set; }

	public EnPersonType PersonType
	{
		get
		{
			if (IsSupervisor)
			{
				return EnPersonType.Supervisor;
			}
			if (IsBazaryab)
			{
				return EnPersonType.Visitor;
			}
			return EnPersonType.Customer;
		}
	}

	public bool IsEtebari { get; set; }

	[JsonIgnore]
	public ServerStatusForClient ServerStatus { get; set; }

	public string PrintTelWithLabel => "";

	public string PrintAddWithLabel
	{
		get
		{
			string text = "";
			if (!string.IsNullOrEmpty(Address1))
			{
				text += Address1;
			}
			if (string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(Address2))
			{
				text += Address2;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Insert(0, "آدرس: ");
			}
			return text;
		}
	}

	public Guid? RoleGuid { get; set; }

	public string ContextGuid { get; set; }

	public string PhoneNumbers { get; set; }

	public event PropertyChangedEventHandler PropertyChanged;

	public static Task<int> DbCount(string contextGuid, bool? status = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid)?.CacheRepository?.ICustomerCompact.DbCountAsync(status, token, tranName);
	}

	public TafsilBusiness GetTafsil(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return _tafsil ?? (_tafsil = TafsilBusiness.Get(ContextGuid, Guid, token, tranName));
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetTafsil", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 60);
			throw;
		}
	}

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			TafsilBusiness tafsilBusiness = _tafsil;
			if (tafsilBusiness == null)
			{
				tafsilBusiness = (_tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, Guid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs"));
			}
			return tafsilBusiness;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 73);
			throw;
		}
	}

	public string AccountHorofi()
	{
		if (Account == 0m)
		{
			return "بی حساب".GetTranslate();
		}
		if (Account > 0m)
		{
			return Account.NpToString("N0", 0) + "بس".GetTranslate();
		}
		return (-Account).NpToString("N0", 0) + "بد".GetTranslate();
	}

	public async Task<List<TafsilNumberBusiness>> TafsilNumbers(CancellationToken token = default(CancellationToken))
	{
		new List<TafsilNumberBusiness>();
		try
		{
			return await TafsilNumberBusiness.GetAsync(ContextGuid, Guid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TafsilNumbers", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 98);
			throw;
		}
	}

	public string AccountHorofiLong()
	{
		if (Account == 0m)
		{
			return "بی حساب".GetTranslate();
		}
		if (Account > 0m)
		{
			return Account.NpToString("N0", 0) + " بستانکار".GetTranslate();
		}
		return (-Account).NpToString("N0", 0) + " بدهکار".GetTranslate();
	}

	public static async Task<CusCompactBusiness> GetAsync(string contextGuid, EnSystem system, Guid guid, List<Guid> lstMoeinGuids, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			if (ClsCache.GetCache(contextGuid)?.CacheRepository?.ICustomerCompact == null)
			{
				return null;
			}
			EnSystem system2 = system;
			return await ApplyAccessAsync(contextGuid, system2, await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.GetAsync(guid, lstMoeinGuids, token, tranName), token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore?.Release();
			}
		}
	}

	public static async Task<CusCompactBusiness> GetAsync(string contextGuid, EnSystem system, long code, List<Guid> lstMoeinGuids, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			if (code <= 0)
			{
				return null;
			}
			EnSystem system2 = system;
			return await ApplyAccessAsync(contextGuid, system2, await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.GetAsync(code, lstMoeinGuids, token, tranName), token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 142);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static async Task<IEnumerable<CusCompactBusiness>> GetAllAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken), string Name = null, string family = null, bool? status = null, string tranName = "")
	{
		int line = 0;
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			ICustomerCompactRepository customerCompactRepository = ClsCache.GetCache(contextGuid)?.CacheRepository?.ICustomerCompact;
			if (customerCompactRepository == null)
			{
				return null;
			}
			line = 3;
			IEnumerable<CusCompactBusiness> ret = (await customerCompactRepository.GetAllAsync(null, status, token, tranName))?.Where((CusCompactBusiness x) => (string.IsNullOrEmpty(Name) || x.Name.Contains(Name)) && (string.IsNullOrEmpty(family) || x.Family.Contains(family)));
			line = 6;
			return await ApplyAccessAsync(contextGuid, system, ret, token, tranName);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"GetAllAsync(name,family,status)//Line:{line}//ClsCache.GetCache(contextGuid)?.IsDisposed:{ClsCache.GetCache(contextGuid)?.IsDisposed ?? true}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 174);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static long GetNextCode(Guid selectedValue)
	{
		return 0L;
	}

	public static async Task<bool> CheckGuid(string contextGuid, Guid guid, CancellationToken token, string tranName)
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.GetAsync(guid, token, tranName) != null;
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token)
	{
		return await ClsCache.GetCache(ContextGuid).BusinessAggregator.CusBusiness.SetDeliveryAsync(del, token, 10);
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 200), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 200);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await CusBusiness.Get(ContextGuid, Guid, token).SetSaveResultAsync(item, token, 10, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 202);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 207);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 208);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 213), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 213);
			}
		}
		return ret;
	}

	protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public async Task<CusTitleBusiness> GetTitelAsync(CancellationToken token, string tranName = "")
	{
		try
		{
			if (_title != null)
			{
				return _title;
			}
			if (TitleGuid == Guid.Empty)
			{
				return null;
			}
			_title = await CusTitleBusiness.GetAsync(ContextGuid, TitleGuid, token, tranName);
			return _title;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTitelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 314);
			throw;
		}
	}

	private async Task UpdateTitleAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await Task.Delay(new Random().Next(100, 500));
			if (token.IsCancellationRequested)
			{
				return;
			}
			_titleName = (await GetTitelAsync(token))?.Name;
			if (this.PropertyChanged != null && UiInvokerHolder.Instance != null)
			{
				UiInvokerHolder.Instance.RunOnUiThread(delegate
				{
					this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TitleName"));
				}, "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 332);
		}
	}

	public CusCompactBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<CusCompactBusiness>> GetAllVisitorsAsync(string contextGuid, EnSystem system, bool? status, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllVisitorsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs"))
		{
			try
			{
				List<CusCompactBusiness> x = new List<CusCompactBusiness>
				{
					new CusCompactBusiness(contextGuid)
					{
						Name = "<" + "نام بازاریاب".GetTranslate() + ">",
						Guid = Guid.Empty
					}
				};
				List<CusCompactBusiness> list = (from p in await GetAllAsync(contextGuid, system, token, null, null, status)
					where p.IsBazaryab
					select p into w
					orderby w._title
					select w).ToList();
				if (list != null && list.Count() > 0)
				{
					x.AddRange(list);
				}
				return x;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllVisitorsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 419);
				throw;
			}
		}
	}

	public static async Task<IEnumerable<CusCompactBusiness>> GetAllSupervisorAsync(string contextGuid, EnSystem system, bool? status, CancellationToken token)
	{
		using (new JaegerMonitor("GetAllSupervisorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs"))
		{
			try
			{
				List<CusCompactBusiness> x = new List<CusCompactBusiness>
				{
					new CusCompactBusiness(contextGuid)
					{
						Name = "<نام سوپروایزر>".GetTranslate(),
						Guid = Guid.Empty
					}
				};
				List<CusCompactBusiness> list = (await GetAllAsync(contextGuid, system, token, null, null, status))?.Where((CusCompactBusiness p) => p.IsSupervisor)?.OrderBy((CusCompactBusiness w) => w.Name)?.ToList();
				if (list.Any())
				{
					x.AddRange(list);
				}
				return x;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllSupervisorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 442);
				throw;
			}
		}
	}

	public static async Task<int> CountWithThisTitleAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.CountWithThisTitleAsync(guid, token, "");
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<CusCompactBusiness>>> GetAllAsync(string contextGuid, List<Guid> lstMoeinGuids, EnSystem system, string search, Guid? groupGuid, CancellationToken token, bool? status = true, SearchLocation searchLocation = SearchLocation.All, CustomerSearchOption cSO = CustomerSearchOption.All, string tranName = "", int? top = null)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs"))
		{
			ReturnedSaveFuncInfoWithValue<IEnumerable<CusCompactBusiness>> ret = new ReturnedSaveFuncInfoWithValue<IEnumerable<CusCompactBusiness>>();
			int line = 0;
			bool semaphoreHasBeenTaken = false;
			Stopwatch.StartNew();
			try
			{
				await _connectionPoolSemaphore.WaitAsync(token);
				semaphoreHasBeenTaken = true;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 461);
				}
				line = 4;
				ICustomerCompactRepository obj = ClsCache.GetCache(contextGuid)?.CacheRepository?.ICustomerCompact;
				ReturnedSaveFuncInfoWithValue<IEnumerable<CusCompactBusiness>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await obj.GetAllAsync(lstMoeinGuids, status, token, tranName);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 465);
				}
				line = 6;
				if (groupGuid.HasValue && groupGuid != Guid.Empty)
				{
					ret.value = ret.value?.Where(delegate(CusCompactBusiness p)
					{
						Guid groupGuid2 = p.GroupGuid;
						Guid? guid = groupGuid;
						return groupGuid2 == guid;
					});
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 469);
				}
				line = 11;
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 472);
				}
				UserCustomerSettings set = cashierBusiness?.Settings?.CustomerS;
				line = 12;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 475);
				}
				foreach (string item in search?.FixString()?.RemoveCharactersForLinkSearch()?.SplitString()?.Distinct() ?? new List<string>())
				{
					if (string.IsNullOrEmpty(item?.Trim()) || set == null)
					{
						continue;
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 479);
					}
					ret.value = ret.value.Where(delegate(CusCompactBusiness x)
					{
						if (((cSO != 0 && cSO != CustomerSearchOption.JustCode && cSO != CustomerSearchOption.CodeNameFamily) || !set.SrchByCode || x.Code != item.ParseToLong(0L)) && ((cSO != 0 && cSO != CustomerSearchOption.CodeNameFamily) || !set.SrchByName || !set.SrchByName || !x.Name.StringComparisons(item)))
						{
							if ((cSO == CustomerSearchOption.All || cSO == CustomerSearchOption.JustFamily || cSO == CustomerSearchOption.CodeNameFamily) && set.SrchByFamily && set.SrchByFamily)
							{
								string family = x.Family;
								if (family != null && family.StringComparisons(item))
								{
									goto IL_02b2;
								}
							}
							if (cSO == CustomerSearchOption.All && (cSO == CustomerSearchOption.All || cSO == CustomerSearchOption.JustAddress) && set.SrchByAdd1)
							{
								string address = x.Address1;
								if (address != null && address.Contains(item))
								{
									goto IL_02b0;
								}
							}
							if ((cSO == CustomerSearchOption.All || cSO == CustomerSearchOption.JustAddress) && set.SrchByAdd2)
							{
								string address2 = x.Address2;
								if (address2 != null && address2.Contains(item))
								{
									goto IL_02b0;
								}
							}
							if (cSO == CustomerSearchOption.All && set.SrchByZip)
							{
								string zip = x.Zip;
								if (zip != null && zip.Contains(item))
								{
									goto IL_02b0;
								}
							}
							if (cSO == CustomerSearchOption.All && set.SrchByTelMob)
							{
								string phoneNumbers = x.PhoneNumbers;
								if (phoneNumbers != null && phoneNumbers.Contains(item))
								{
									goto IL_02b0;
								}
							}
							if (cSO == CustomerSearchOption.All && set.SrchByDecription)
							{
								return x.Description?.Contains(item) ?? false;
							}
							return false;
						}
						goto IL_02b2;
						IL_02b0:
						return true;
						IL_02b2:
						return true;
					});
				}
				line = 21;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 492);
				}
				line = 22;
				if (ret.value != null && set != null)
				{
					switch (set.CustomerSort)
					{
					case CustomerSort.NameAsc:
						ret.value = ret.value?.OrderBy((CusCompactBusiness o) => o.Family)?.ThenBy((CusCompactBusiness o) => o.Name)?.ToList();
						break;
					case CustomerSort.NameDesc:
						ret.value = ret.value?.OrderByDescending((CusCompactBusiness o) => o.Family)?.ThenBy((CusCompactBusiness o) => o.Name)?.ToList();
						break;
					case CustomerSort.CodeAsc:
						ret.value = ret.value?.OrderBy((CusCompactBusiness o) => o.Code)?.ToList();
						break;
					case CustomerSort.CodeDesc:
						ret.value = ret.value?.OrderByDescending((CusCompactBusiness o) => o.Code)?.ToList();
						break;
					case CustomerSort.BalanceAsc:
						ret.value = ret.value?.OrderBy((CusCompactBusiness o) => o.Account)?.ToList();
						break;
					case CustomerSort.BalanceDesc:
						ret.value = ret.value?.OrderByDescending((CusCompactBusiness o) => o.Account)?.ToList();
						break;
					}
				}
				line = 35;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 505);
				}
				foreach (string item2 in search?.SplitString()?.Distinct() ?? new List<string>())
				{
					if (item2 == null || item2.ParseToLong(0L) <= 9999999)
					{
						continue;
					}
					if (token.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 510);
					}
					List<CusCompactBusiness> lst = new List<CusCompactBusiness>();
					CusCardBusiness cusCardBusiness = await CusCardBusiness.GetAsync(contextGuid, item2, token, tranName);
					if (cusCardBusiness != null)
					{
						List<CusCompactBusiness> list = lst;
						list.Add(await GetAsync(contextGuid, system, cusCardBusiness.CusGuid, lstMoeinGuids, tranName, token));
					}
					if (lst.Any())
					{
						if (ret.value?.Any() ?? false)
						{
							lst.AddRange(ret.value);
						}
						ret.value = lst;
					}
				}
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await ApplyAccessAsync(contextGuid, system, ret.value, token, tranName);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 533);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"GetAllAsync(search,groupGuid,cancelation,stFalse) line:{line}", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 536);
				ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 537);
			}
			finally
			{
				if (top.HasValue && top > 0)
				{
					ret.value = ret?.value?.Take(top.Value);
				}
				if (semaphoreHasBeenTaken)
				{
					_connectionPoolSemaphore.Release();
				}
			}
			return ret;
		}
	}

	public static async Task<IEnumerable<CusCompactBusiness>> GetAllAsync(string contextGuid, CustomerCompactFilter filter, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync(token);
			semaphoreHasBeenTaken = true;
			if (!(filter?.TafsilGroupGuidList?.Any()).GetValueOrDefault())
			{
				if (filter == null)
				{
					filter = new CustomerCompactFilter();
				}
				CustomerCompactFilter customerCompactFilter = filter;
				customerCompactFilter.TafsilGroupGuidList = await CashierBusiness.GetAllTafsilGroupGuidsForTurnoverAsync(contextGuid, system, token, tranName);
			}
			return await ApplyAccessAsync(contextGuid, system, await ClsCache.GetCache(contextGuid).CacheRepository.ICustomerCompact.GetAllAsync(filter, token, tranName), token, tranName);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			token.ThrowIfCancellationRequested();
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 566);
			throw;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public async Task<bool> CanShowTurnoverAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_tafsil == null)
			{
				_tafsil = await GetTafsilAsync(system, tranName, token);
			}
			if (_tafsil == null)
			{
				return false;
			}
			return await _tafsil.CanShowTurnoverAsync(system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanShowTurnoverAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 585);
			throw;
		}
	}

	public static async Task<Tuple<bool, string>> PasswordRecoveryAsync(string contextGuid, string mobileText, string usernmaeText, int timeOutSec = 20)
	{
		using (new JaegerMonitor("PasswordRecoveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs"))
		{
			_ = 1;
			try
			{
				Packet item = WebCus.GeneratePasswordRecoveryRequest(usernmaeText, mobileText);
				ClsCache.GetCache(contextGuid).SendData(new List<Packet> { item }, Priority.High);
				ClsCache.GetCache(contextGuid).Context.WebCus.OnPasswordRecoveryResponse -= CustomerInstance_OnPasswordRecoveryResponse;
				ClsCache.GetCache(contextGuid).Context.WebCus.OnPasswordRecoveryResponse += CustomerInstance_OnPasswordRecoveryResponse;
				passRevoverResponseCompletionSource = new TaskCompletionSource<Tuple<bool, string>>();
				return (await Task.WhenAny(new Task[2]
				{
					Task.Delay(checked(timeOutSec * 1000)),
					passRevoverResponseCompletionSource.Task
				}) != passRevoverResponseCompletionSource.Task) ? new Tuple<bool, string>(item1: false, "خطا در ارسال") : (await passRevoverResponseCompletionSource.Task);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "PasswordRecoveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 607);
				throw;
			}
		}
	}

	private static async Task CustomerInstance_OnPasswordRecoveryResponse(object arg1, CustomerPasswordRecoveryResponseEventArgs arg2)
	{
		using (new JaegerMonitor("CustomerInstance_OnPasswordRecoveryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs"))
		{
			try
			{
				passRevoverResponseCompletionSource?.TrySetResult(new Tuple<bool, string>(arg2.Success, arg2.Message));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CustomerInstance_OnPasswordRecoveryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 619);
			}
		}
	}

	private static async Task<CusCompactBusiness> ApplyAccessAsync(string contextGuid, EnSystem system, CusCompactBusiness item, CancellationToken token, string tranName)
	{
		return (await ApplyAccessAsync(contextGuid, system, new List<CusCompactBusiness> { item }, token, tranName))?.SingleOrDefault();
	}

	private static async Task<IEnumerable<CusCompactBusiness>> ApplyAccessAsync(string contextGuid, EnSystem system, IEnumerable<CusCompactBusiness> ret, CancellationToken token, string tranName)
	{
		int line = 0;
		try
		{
			IEnumerable<CusCompactBusiness> orginalData = ret;
			if (system != EnSystem.UserInterface)
			{
				return ret;
			}
			if (!(ret?.Any() ?? false))
			{
				return ret;
			}
			line = 1;
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			line = 3;
			if (cashier == null)
			{
				return ret;
			}
			line = 4;
			AccessLevel access = await cashier.GetUserAccessAsync(token, tranName);
			UserAccBusiness user = await cashier.GetUserAsync(token, tranName);
			line = 5;
			if (user == null)
			{
				throw new NullReferenceException("UserIs Null!!");
			}
			line = 7;
			if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
			{
				ret = ret?.Where((CusCompactBusiness p) => p != null && (user.UserAccess.TafsilGroupJustListGuid.Contains(p.TafsilGroupGuid) || p.Guid == (cashier?.VisitorGuid ?? Guid.Empty)));
			}
			if ((cashier.VisitorGuid ?? Guid.Empty) != Guid.Empty && !cashier.AllVisitorAccess)
			{
				ret = ret?.Where((CusCompactBusiness p) => p != null && (p.VisitorGuid == cashier.VisitorGuid.Value || p.Guid == cashier.VisitorGuid.Value));
			}
			if (!access.Customers.Customer_DastrasiBeAshkhaseKhas)
			{
				ret = ret?.Where((CusCompactBusiness p) => p != null && !p.Special);
			}
			ret = ret?.ToList();
			if ((cashier.GetCusGuid() ?? Guid.Empty) != Guid.Empty && !ret.Any((CusCompactBusiness p) => p != null && p.Guid == cashier.GetCusGuid().Value))
			{
				CusCompactBusiness cusCompactBusiness = orginalData.SingleOrDefault((CusCompactBusiness p) => p != null && p.Guid == cashier.GetCusGuid().Value);
				if (cusCompactBusiness != null)
				{
					((List<CusCompactBusiness>)ret).Add(cusCompactBusiness);
				}
			}
			if ((cashier.VisitorGuid ?? Guid.Empty) != Guid.Empty && !ret.Any((CusCompactBusiness p) => p != null && p.Guid == cashier.VisitorGuid.Value))
			{
				CusCompactBusiness cusCompactBusiness2 = orginalData.SingleOrDefault((CusCompactBusiness p) => p != null && p.Guid == cashier.VisitorGuid.Value);
				if (cusCompactBusiness2 != null)
				{
					((List<CusCompactBusiness>)ret).Add(cusCompactBusiness2);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 676);
			throw;
		}
		return ret;
	}

	public string Mobiles()
	{
		try
		{
			throw new NotImplementedException();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Mobiles", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 688);
		}
		return "";
	}

	public async Task<CustomerRoleBusiness> GetRoleAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if ((RoleGuid ?? Guid.Empty) == Guid.Empty)
			{
				return null;
			}
			return await CustomerRoleBusiness.GetAsync(ContextGuid, RoleGuid.Value, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRoleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Customers\\CusCompactBusiness.cs", 700);
			throw;
		}
	}

	public static Task<List<Guid>> GetCustomerGuidListinRoleAsync(string contextGuid, Guid roleGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ICustomer.GetCustomerGuidListInRoleAsync(roleGuid, token, tranName);
	}
}
