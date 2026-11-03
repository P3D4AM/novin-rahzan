using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Core;
using EntityCache.Business.Extensions;
using EntityCache.Business.Report;
using EntityCache.Core;
using EntityCache.DescriptionParser;
using EntityCache.Entities;
using EntityCache.Interfaces;
using EntityCache.Interfaces.Sync;
using EntityCache.Mapping.BusinessMappers;
using EntityCache.Mapping.BusinessMappers.Sanad;
using EntityCache.WebService.Repository;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Sanad;
using PacketParser.Filters;
using PacketParser.Print;
using PacketParser.Records;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;

namespace EntityCache.Business;

public class ReceptionBusiness : BaseSanadBusiness<ReceptionBusiness>, IReception, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid, ISyncFromClient2Server<ReceptionBusiness>, IDeliverableEntity, IEditableDelivery, IEntityCache, IDisposable, IChangeIntegrity, IBaseKhazane, IHasContext, IHasSanadStatusReport
{
	private static CancellationTokenSource _sendEntitiesToServerToken;

	private CancellationTokenSource _send2ServerToken = new CancellationTokenSource();

	private int _id2;

	private int _sanadNo;

	private int _deliveryId;

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim oldReceptionGetSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private OrderBusiness _masterOrder;

	private OrderBusiness _oldOrder;

	private DateTime _Date = DateManagment.DateM;

	private DateTime _dateSabt = DateTime.Now;

	private DateTime _modified = DateTime.Now;

	private DateTime? _deliveryDate;

	private bool _ChangeDate;

	private bool oldReceptionGet;

	private bool _isTemp;

	private bool _isChecked;

	private bool _status;

	private List<ReceptionCheckBusiness> _listCheck;

	private List<ReceptionNaghdBusiness> _listNaghd;

	private List<ReceptionHavaleBusiness> _listBank;

	private List<Sanad_DetBusiness> _listEnteghal;

	private TableRelationShipBusiness _linkedSanad;

	private string _description;

	private string _hddSerial;

	private string _ip;

	private string _lkSerial;

	private string _androidIme;

	private string _cpuSerial;

	private string _hostRefrenced;

	private string _deliveryMessage = "";

	private string _deliveryShortMessage = "";

	private Guid _tafsilGuid;

	private Guid _guid;

	private Guid _userGuid;

	private Guid _moeinGuid;

	private ReceptionBusiness _oldReception;

	private ENSource _source;

	private DependencyType _dependencyType;

	private short _sanadSt;

	private ServerStatusForClient _serverStatus;

	private bool? _deliveryStatus;

	private EntityChangeSt _changeStatus;

	private readonly int _dbCount;

	private decimal _afterAccount;

	private decimal? _beforeAccount;

	private PaymentMethod _paymentMethod;

	private List<ProductsChanges> _lstProductsAnbarChanges = new List<ProductsChanges>();

	public Guid TafsilGuid => _tafsilGuid;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public DateTime? TransferDate { get; set; }

	public bool IsTemp
	{
		get
		{
			try
			{
				return GetIsTemp();
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "IsTemp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 92);
			}
			return false;
		}
		set
		{
			_isTemp = value;
		}
	}

	public short StatusId { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsChecked
	{
		get
		{
			return _isChecked;
		}
		set
		{
			_isChecked = value;
		}
	}

	public EnKhazaneMaster Type { get; set; } = EnKhazaneMaster.Reception;


	public ENSource Source
	{
		get
		{
			return _source;
		}
		set
		{
			_source = value;
		}
	}

	public int NaghdCount => CountNaghd;

	public int BankCount => CountBank_Havale;

	public int PossCount => CountBank_Pos;

	public int CheckShCount => CountCheck;

	public decimal SumCheckSh => SumCheck;

	public int CheckMCount => 0;

	public decimal SumCheckM => 0m;

	public DateTime DateM
	{
		get
		{
			return Date;
		}
		set
		{
			Date = value;
		}
	}

	public DateTime? DateMoaser { get; set; }

	public decimal SumNaghd => ListNaghd?.Sum((ReceptionNaghdBusiness p) => p.Price) ?? 0m;

	public decimal SumBank => ListBank?.Sum((ReceptionHavaleBusiness p) => p.Price) ?? 0m;

	public decimal SumPoss => SumBankPos;

	public decimal SumBankPos => ((from p in ListBank?.ToList()
		where p.HavaleType == ENHavaleType.POS
		select p)?.Sum((ReceptionHavaleBusiness w) => w.Price)).GetValueOrDefault();

	public decimal SumBankHavale => (ListBank?.ToList()?.Where((ReceptionHavaleBusiness p) => p.HavaleType == ENHavaleType.Havale)?.Sum((ReceptionHavaleBusiness w) => w.Price)).GetValueOrDefault();

	public decimal SumCheck => (ListCheck?.Where((ReceptionCheckBusiness p) => p.Status != ENUMCheck.BatelShode)?.Sum((ReceptionCheckBusiness p) => p.Price)).GetValueOrDefault();

	public decimal SumEnteghal => GetSumEnteghal();

	public bool Status
	{
		get
		{
			return _status;
		}
		set
		{
			_status = value;
		}
	}

	public DependencyType DependencyType
	{
		get
		{
			return _dependencyType;
		}
		set
		{
			_dependencyType = value;
		}
	}

	public decimal Sum => SumCheck + SumEnteghal + SumBank + SumNaghd;

	public string SumStr => Sum.AdadToHoroof();

	public string AndroidIme
	{
		get
		{
			return _androidIme;
		}
		set
		{
			_androidIme = value;
		}
	}

	public string CpuSerial
	{
		get
		{
			return _cpuSerial;
		}
		set
		{
			_cpuSerial = value;
		}
	}

	public DateTime Date
	{
		get
		{
			if (!_ChangeDate)
			{
				return DateManagment.DateM;
			}
			return _Date;
		}
		set
		{
			_ChangeDate = true;
			_Date = value;
		}
	}

	public string Time => $"{Date.Hour}:{Date.Minute}";

	public DateTime DateSabt
	{
		get
		{
			return _dateSabt;
		}
		set
		{
			_dateSabt = value;
		}
	}

	public string DateSh => Date.MiladiToCalture();

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public string HddSerial
	{
		get
		{
			return _hddSerial;
		}
		set
		{
			_hddSerial = value?.FixString() ?? "";
		}
	}

	public Guid UserGuid
	{
		get
		{
			return _userGuid;
		}
		set
		{
			if (value == Guid.Empty && _userGuid != Guid.Empty)
			{
				throw new ArgumentException("userGuid could not set to Guid.Empty!!");
			}
			_userGuid = (_userGuid = value);
		}
	}

	public Guid CashierGuid => UserGuid;

	public Guid MoeinGuid
	{
		get
		{
			return _moeinGuid;
		}
		set
		{
			_moeinGuid = value;
		}
	}

	public string Ip
	{
		get
		{
			return _ip;
		}
		set
		{
			_ip = value?.FixString() ?? "";
		}
	}

	public string LkSerial
	{
		get
		{
			return _lkSerial;
		}
		set
		{
			_lkSerial = value?.FixString() ?? "";
		}
	}

	public string HostRefrenced
	{
		get
		{
			return _hostRefrenced;
		}
		set
		{
			_hostRefrenced = value?.FixString() ?? "";
		}
	}

	public int SanadNo
	{
		get
		{
			return _sanadNo;
		}
		set
		{
			_sanadNo = value;
		}
	}

	public short SanadSt
	{
		get
		{
			return _sanadSt;
		}
		set
		{
			_sanadSt = value;
		}
	}

	public DateTime? DeliveryDate
	{
		get
		{
			return _deliveryDate;
		}
		set
		{
			_deliveryDate = value;
		}
	}

	public int DeliveryId
	{
		get
		{
			return _deliveryId;
		}
		set
		{
			_deliveryId = value;
		}
	}

	public string DeliveryMessage
	{
		get
		{
			return _deliveryMessage;
		}
		set
		{
			_deliveryMessage = value?.FixString() ?? "";
		}
	}

	public string DeliveryShortMessage
	{
		get
		{
			return _deliveryShortMessage;
		}
		set
		{
			_deliveryShortMessage = value?.FixString() ?? "";
		}
	}

	public override Guid Guid
	{
		get
		{
			if (_guid == Guid.Empty)
			{
				_guid = Guid.NewGuid();
				ClsCache.GetCache(ContextGuid).CacheRepository.IReception.Add2CacheAsync(this, default(CancellationToken), "");
			}
			return _guid;
		}
		set
		{
			_guid = value;
		}
	}

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			_modified = value;
		}
	}

	public ServerStatusForClient ServerStatus
	{
		get
		{
			return _serverStatus;
		}
		set
		{
			_serverStatus = value;
		}
	}

	public bool? DeliveryStatus
	{
		get
		{
			return _deliveryStatus;
		}
		set
		{
			_deliveryStatus = value;
		}
	}

	public override EntityChangeSt ChangeStatus
	{
		get
		{
			return _changeStatus;
		}
		set
		{
			_changeStatus = value;
		}
	}

	public string BankDesc => $"{CountBank}  فقره -  جمع : {SumBank:N0} {ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit}";

	public string BankHavaleDesc => $"{CountBank_Havale}  فقره -  جمع : {SumBankHavale:N0}  {ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit}";

	public string BankPosDesc => $"{CountBank_Pos}  فقره -  جمع : {SumBankPos:N0}  {ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit}";

	public string NaghdDesc => $"{CountNaghd}  فقره -  جمع : {SumNaghd:N0} {ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit}";

	public string CheckDesc => $"{CountCheck}  فقره -  جمع : {SumCheck:N0} {ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit}";

	public int DbCount => _dbCount;

	public PaymentMethod PaymentMethod
	{
		get
		{
			return _paymentMethod;
		}
		set
		{
			_paymentMethod = value;
		}
	}

	public override Document SanadType => Document.Reception;

	public Document AsType => SanadType;

	public int CountNaghd => ListNaghd?.Count ?? 0;

	public int CountBank => ListBank?.Count ?? 0;

	public int CountBank_Pos => ListBank?.Where((ReceptionHavaleBusiness p) => p.HavaleType == ENHavaleType.POS).ToList().Count() ?? 0;

	public int CountBank_Havale => ListBank?.Where((ReceptionHavaleBusiness p) => p.HavaleType == ENHavaleType.Havale).ToList().Count() ?? 0;

	public int CountCheck => ListCheck?.Count() ?? 0;

	public int CountEnteghal => ListEnteghal?.Count() ?? 0;

	public ReceptionMetaData MetaData { get; set; }

	public DateTime? DateUtc { get; set; }

	public Guid? OrderGuid { get; set; }

	public Document? OrderType { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public int ItemsCount
	{
		get
		{
			int num = 0;
			checked
			{
				if (ListNaghd != null)
				{
					num += ListNaghd.Count;
				}
				if (ListCheck != null)
				{
					num += ListCheck.Count;
				}
				if (ListBank != null)
				{
					num += ListBank.Count;
				}
				if (ListEnteghal != null)
				{
					num += ListEnteghal.Count;
				}
				return num;
			}
		}
	}

	public IReadOnlyList<ReceptionCheckBusiness> ListCheck
	{
		get
		{
			if (_listCheck == null)
			{
				_listCheck = new List<ReceptionCheckBusiness>();
			}
			_listCheck.RemoveAll((ReceptionCheckBusiness q) => q?.IsDisposed ?? true);
			return _listCheck;
		}
	}

	public IReadOnlyList<ReceptionNaghdBusiness> ListNaghd
	{
		get
		{
			if (_listNaghd == null)
			{
				_listNaghd = new List<ReceptionNaghdBusiness>();
			}
			_listNaghd.RemoveAll((ReceptionNaghdBusiness q) => q?.IsDisposed ?? true);
			return _listNaghd;
		}
	}

	public IReadOnlyList<ReceptionHavaleBusiness> ListBank
	{
		get
		{
			if (_listBank == null)
			{
				_listBank = new List<ReceptionHavaleBusiness>();
			}
			_listBank.RemoveAll((ReceptionHavaleBusiness q) => q?.IsDisposed ?? true);
			return _listBank;
		}
	}

	public IReadOnlyList<Sanad_DetBusiness> ListEnteghal
	{
		get
		{
			if (_listEnteghal == null)
			{
				_listEnteghal = new List<Sanad_DetBusiness>();
			}
			_listEnteghal.RemoveAll((Sanad_DetBusiness q) => q?.IsDisposed ?? true);
			return _listEnteghal;
		}
	}

	public IReadOnlyCollection<BaseKhazaneDetail> DetailList
	{
		get
		{
			try
			{
				List<BaseKhazaneDetail> list = new List<BaseKhazaneDetail>();
				IReadOnlyList<ReceptionNaghdBusiness> listNaghd = ListNaghd;
				if (listNaghd != null && listNaghd.Any())
				{
					list.AddRange(ListNaghd);
				}
				IReadOnlyList<ReceptionHavaleBusiness> listBank = ListBank;
				if (listBank != null && listBank.Any())
				{
					list.AddRange(ListBank);
				}
				IReadOnlyList<ReceptionCheckBusiness> listCheck = ListCheck;
				if (listCheck != null && listCheck.Any())
				{
					list.AddRange(ListCheck);
				}
				IReadOnlyList<Sanad_DetBusiness> listEnteghal = ListEnteghal;
				if (listEnteghal != null && listEnteghal.Any())
				{
					list.AddRange(ListEnteghal);
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DetailList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 256);
				throw;
			}
		}
	}

	public event Func<object, EventArgs, Task> OnSumChanged;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public override bool GetIsTemp()
	{
		if (!_isTemp && ServerStatus != ServerStatusForClient.DeliveryError)
		{
			return ServerStatus == ServerStatusForClient.SendError;
		}
		return true;
	}

	public ReceptionBusiness(string contextGuid, OrderBusiness order = null)
	{
		ContextGuid = contextGuid;
		_masterOrder = order;
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token, string tranName = "", short maxParalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
			{
				return ret;
			}
			if (ClsCache.GetCache(contextGuid)?.CacheRepository?.IReception == null)
			{
				throw new NullReferenceException("cache or receptionRepo is null!!");
			}
			int line = 0;
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Reception;
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال دریافت ها";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 289);
				}
				List<ReceptionBusiness> lst = await ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetAllNotSentAsync(token, tranName);
				line = 2;
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = lst?.Count ?? 0;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				if (lst != null && lst.Count > 0)
				{
					foreach (ReceptionBusiness item in lst)
					{
						Log.Information($"SendingReception({current} from {lst}) /tokeIsCancelled:{token.IsCancellationRequested} /Guid:{item.Guid} /Price:{item.Sum} /Desc:{item.Description} /Time:{sw.ElapsedMilliseconds}(ms)");
						current = checked(current + 1);
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 300);
						}
						if (uiUpdater != null)
						{
							uiUpdater.Count = current;
						}
						UserInformationBusiness userInfo = await UserInformationBusiness.GetAsync(contextGuid);
						CusCompactBusiness cusCompactBusiness = await item.CusInfoAsync(EnSystem.SignalR, tranName, token);
						if (item != null)
						{
							if (userInfo != null && userInfo.Source == ENSource.WebSite)
							{
								tasks.Add(item.Send2ServerAsync(cusCompactBusiness.FullCustomer.User, cusCompactBusiness.FullCustomer.Password, 60000, token, Priority.High));
							}
							else
							{
								tasks.Add(item.Send2ServerAsync("", "", 60000, token, Priority.High));
							}
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 310);
						}
						if (tasks.Count >= maxParalelTask)
						{
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
							tasks.Remove(task);
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 316);
						}
					}
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 320);
				}
				while (tasks?.Any() ?? false)
				{
					Task<ReturnedSaveFuncInfo> task2 = await Task.WhenAny(tasks);
					tasks.Remove(task2);
					ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await task2;
					Log.Information($"SendingReception({current} from {lst}) /tokeIsCancelled:{token.IsCancellationRequested} /Result:{returnedSaveFuncInfo2.ToString()} /Time:{sw.ElapsedMilliseconds}(ms)");
					ret.AddReturnedValue(returnedSaveFuncInfo2, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 327);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 332);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 333);
			}
			finally
			{
				if (uiUpdater != null)
				{
					if (ret.HasError)
					{
						uiUpdater.ShortMessage = "با شکست مواجه شد";
						uiUpdater.Status = SyncStatus.SyncFailed;
					}
					else
					{
						uiUpdater.ShortMessage = "با موفقیت انجام شد.";
						uiUpdater.Status = SyncStatus.SyncedOk;
					}
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(string customerUserName, string customerPassword, int timeout, CancellationToken internalToken, Priority priority)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			Stopwatch sw = Stopwatch.StartNew();
			try
			{
				_sendToken?.Cancel();
				_sendToken = CancellationTokenSource.CreateLinkedTokenSource(new CancellationTokenSource(300000).Token, internalToken);
				CancellationToken token = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "ReceptionBusiness/Send2ServerAsync", 60000, token);
				ReceptionBusiness receptionBusiness = (await GetAsync(ContextGuid, Guid, EnSystem.SignalR, null, fromDatabase: false, token)) ?? throw new NullReferenceException($"reception for send is not exsist on database. guid=>{Guid}");
				if (receptionBusiness.ServerStatus == ServerStatusForClient.Delivered || receptionBusiness.ServerStatus == ServerStatusForClient.DirectDelivery)
				{
					return ret.AddInformation("current status on database is delivered!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 367);
				}
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 368);
				}
				List<WebReception> list = new List<WebReception>();
				List<WebReception> list2 = list;
				list2.Add(await ToWebItemAQsync(token));
				List<Packet> list3 = WebReception.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 370);
				}
				using (new ReceptionWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, token))
				{
					if (!string.IsNullOrEmpty(customerUserName) && !string.IsNullOrEmpty(customerPassword))
					{
						ClsCache.GetCache(ContextGuid).SendOnCustomerBuffer(customerUserName, customerPassword, list3, token, Priority.High, timeout);
					}
					else
					{
						ClsCache.GetCache(ContextGuid).SendData(list3, priority);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 376);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 379);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 380);
			}
			finally
			{
				ret.AddInformation($"time:{sw.ElapsedMilliseconds}(ms)/Guid:{Guid}/Desc:{Description}/Sum:{Sum}", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 382);
			}
			return ret;
		}
	}

	public Task<WebReception> ToWebItemAQsync(CancellationToken token = default(CancellationToken))
	{
		return ReceptionMapper.Instance.MapAsync(ContextGuid, this, new WebReception(), "", token);
	}

	public decimal GetSumEnteghal(string tranName = "")
	{
		using (new JaegerMonitor("GetSumEnteghal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				if (ListEnteghal == null || !ListEnteghal.Any())
				{
					return 0m;
				}
				decimal result = default(decimal);
				foreach (Sanad_DetBusiness item in ListEnteghal)
				{
					if (item != null)
					{
						result += item.Debit - item.Credit;
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSumEnteghal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 401);
				return 0m;
			}
		}
	}

	public override DateTime GetDate()
	{
		return Date;
	}

	public async Task SetTafsilGuidAsync(Guid value, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			_beforeAccount = null;
			_tafsilGuid = value;
			if (MoeinGuid == Guid.Empty)
			{
				TafsilBusiness tafsilBusiness = await GetTafsilAsync(system, token, tranName);
				if (tafsilBusiness != null)
				{
					MoeinGuid = (await tafsilBusiness.GetTafsilGroupAsync(system, token, tranName))?.MoeinReceptionGuid ?? Guid.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 420);
		}
	}

	public override Task<int> GetSanadNumberAsync(CancellationToken token, string tranName)
	{
		return Task.FromResult(SanadNo);
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IReception;
	}

	public async Task<decimal> GetBeforeAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (_beforeAccount.HasValue)
			{
				return _beforeAccount.Value;
			}
			if (system == EnSystem.Transfer)
			{
				_beforeAccount = default(decimal);
				return default(decimal);
			}
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				_beforeAccount = default(decimal);
				return default(decimal);
			}
			_beforeAccount = await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.GetBeforeAccountAsync(TafsilGuid, Guid, Document.Reception, token, tranName);
			if (_oldReception == null)
			{
				_oldReception = (ReceptionBusiness)(await GetOldItemAsync(system, token, tranName));
			}
			if (_oldReception != null && _oldReception.Date < Date)
			{
				_beforeAccount -= (decimal?)_oldReception.Sum;
			}
			return _beforeAccount.GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 446);
			throw;
		}
	}

	public void SetBeforeAccount(decimal value)
	{
		_beforeAccount = value;
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			bool autoTran = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoTran)
			{
				tranName = Guid.NewGuid().ToString();
			}
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoTran)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 460), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 460);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 462);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 466);
					ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 467);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 472), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 472);
				}
			}
			return ret;
		}
	}

	private async Task<OrderBusiness> GetOldOrderAsync(Guid guid, Document factorType, CancellationToken token, string tranName)
	{
		try
		{
			if (_oldOrder == null)
			{
				_oldOrder = await OrderBusiness.GetAsync(ContextGuid, guid, factorType, fromDatabase: true, token, tranName);
			}
			return _oldOrder;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 485);
			throw;
		}
	}

	public List<object> AllItems(string newContextGuid)
	{
		using (new JaegerMonitor("AllItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				List<object> list = new List<object>();
				if (ListNaghd != null)
				{
					foreach (ReceptionNaghdBusiness item in ListNaghd)
					{
						item.ContextGuid = newContextGuid;
						list.Add(item);
					}
				}
				if (ListBank != null)
				{
					foreach (ReceptionHavaleBusiness item2 in ListBank)
					{
						item2.ContextGuid = newContextGuid;
						list.Add(item2);
					}
				}
				if (ListCheck != null)
				{
					foreach (ReceptionCheckBusiness item3 in ListCheck)
					{
						item3.ContextGuid = newContextGuid;
						list.Add(item3);
					}
				}
				if (ListEnteghal != null)
				{
					foreach (Sanad_DetBusiness item4 in ListEnteghal)
					{
						item4.ContextGuid = newContextGuid;
						list.Add(item4);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AllItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 523);
				return new List<object>();
			}
		}
	}

	public async Task<decimal> GetAfterAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (system == EnSystem.Transfer)
			{
				return default(decimal);
			}
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				return _afterAccount;
			}
			decimal num = await GetBeforeAccountAsync(system, token, tranName);
			if ((_masterOrder?.TotalSum ?? 0m) != 0m)
			{
				_afterAccount = await _masterOrder.GetAfterAccountAsync(system, token, tranName);
			}
			else
			{
				_afterAccount = num + Sum;
			}
			return _afterAccount;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAfterAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 540);
			throw;
		}
	}

	public void SetAfterAccount(decimal value)
	{
		_afterAccount = value;
	}

	public async Task<string> GetFullNameAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetTafsilAsync(system, token, tranName))?.Name ?? "";
	}

	public override async Task<BaseSanadBusiness<ReceptionBusiness>> GetOldItemAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		try
		{
			if (system == EnSystem.Transfer)
			{
				return _oldReception ?? throw new NullReferenceException("old reception must be filled before");
			}
			if (oldReceptionGet)
			{
				return _oldReception;
			}
			oldReceptionGet = true;
			_oldReception = await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.GetFromDbAsync(Guid, _masterOrder, system, token, tranName);
			return _oldReception;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 559);
			throw;
		}
	}

	public async Task UpdateBeforeAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (system != EnSystem.Transfer && system != EnSystem.Mapper)
				{
					ReceptionBusiness receptionBusiness = (ReceptionBusiness)(await GetOldItemAsync(system, token, tranName));
					if (receptionBusiness == null || receptionBusiness.Sum <= 0m || (receptionBusiness?.IsTemp ?? true))
					{
						SetBeforeAccount(await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, tranName, token));
					}
					SetAfterAccount(await GetBeforeAccountAsync(system, token, tranName) + Sum);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 579);
				throw;
			}
		}
	}

	public static async Task<IEnumerable<ReceptionReportBusiness>> GetAllAsync(string contextGuid, EnSystem system, KhazaneFilter filters, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		CashiersLogBusiness.Logger(contextGuid, system, sendToServer: true, EnLog.Reception, ENAction.Report, null, null, "گزارش دریافت");
		EnSystem system2 = system;
		return await ApplyAccessAsync(contextGuid, system2, await ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetAllAsync(filters, token, tranName));
	}

	private static async Task<IEnumerable<ReceptionReportBusiness>> ApplyAccessAsync(string contextGuid, EnSystem system, IEnumerable<ReceptionReportBusiness> receptions, CancellationToken token = default(CancellationToken))
	{
		List<ReceptionReportBusiness> ret = new List<ReceptionReportBusiness>();
		try
		{
			if (system == EnSystem.Transfer)
			{
				return receptions;
			}
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			UserAccBusiness user = await currentCashier.GetUserAsync(token);
			IEnumerable<int> illegalStatus = user.UserAccess.Reception.SanadStatusAccess?.Where((ReceptionSanadStatusAccess q) => !q.Show)?.Select((ReceptionSanadStatusAccess q) => q.StatusId);
			if (receptions?.Any() ?? false)
			{
				if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
				{
					receptions = receptions?.Where((ReceptionReportBusiness p) => (p?.TafsilGroupGuid ?? Guid.Empty) != Guid.Empty && user.UserAccess.TafsilGroupJustListGuid.Contains(p.TafsilGroupGuid));
				}
				if (illegalStatus?.Any() ?? false)
				{
					receptions = receptions?.Where((ReceptionReportBusiness q) => !illegalStatus.Contains(q.StatusId) || q.StatusId <= 0);
				}
				foreach (ReceptionReportBusiness reception in receptions)
				{
					if (reception != null)
					{
						if (user.UserAccess.Reception.Reception_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(reception, user.UserAccess.Reception.Reception_ReportPreviuseDay, ret);
						}
						else if (user.UserAccess.Reception.Reception_ReportSelf && reception.CashierGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(reception, user.UserAccess.Reception.Reception_ReportPreviuseDay, ret);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 614);
			throw;
		}
		return ret;
	}

	private static void CheckPreviewDaysAndAddIfHasAccess(ReceptionReportBusiness reception, bool reportPreviuseDay, List<ReceptionReportBusiness> ret)
	{
		try
		{
			if (reportPreviuseDay)
			{
				ret.Add(reception);
			}
			else if (reception.DateM.Date == DateTime.Now.Date)
			{
				ret.Add(reception);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPreviewDaysAndAddIfHasAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 630);
			throw;
		}
	}

	public static async Task<ReceptionBusiness> GetAsync(string contextGuid, int id2, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			KhazaneFilter filters = new KhazaneFilter
			{
				Id2From = id2,
				Id2To = id2
			};
			IEnumerable<ReceptionReportBusiness> enumerable = await GetAllAsync(contextGuid, system, filters, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			ReceptionReportBusiness receptionReportBusiness = enumerable.SingleOrDefault();
			return await GetAsync(contextGuid, receptionReportBusiness.Guid, system, null, fromDatabase: true, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 647);
			throw;
		}
	}

	public static async Task<ReceptionBusiness> GetAsync(string contextGuid, Guid guid, EnSystem system, OrderBusiness order = null, bool fromDatabase = false, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (guid == Guid.Empty)
				{
					return null;
				}
				ClsCache cache = ClsCache.GetCache(contextGuid);
				if (cache.Source == ENSource.Accounting && !fromDatabase)
				{
					throw new Exception("بر ای سورس حسابداری دریافت رو از دیتابیس بگیر!".GetTranslate());
				}
				return (!fromDatabase) ? (await cache.CacheRepository.IReception.GetAsync(guid, order, system, token, tranName)) : (await cache.CacheRepository.IReception.GetFromDbAsync(guid, order, system, token, tranName));
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 669);
				throw;
			}
		}
	}

	public async Task<List<AndroidPrintStyle>> GetPrintStylesAsync()
	{
		using (new JaegerMonitor("GetPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 1;
			try
			{
				CancellationToken token = default(CancellationToken);
				List<AndroidPrintStyle> list = (await UserInformationBusiness.GetAsync(ContextGuid))?.AndroidPrintStyles?.Where((AndroidPrintStyle p) => p.PrintType == PrintType.Reception && !string.IsNullOrEmpty(p.Html))?.ToList();
				if (list == null)
				{
					return null;
				}
				List<AndroidPrintStyle> newStyles = new List<AndroidPrintStyle>();
				foreach (AndroidPrintStyle item in list)
				{
					item.Html.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"")
						.Replace("&apos;", "'")
						.Replace("&amp;", "&");
					AndroidPrintStyle tmp = new AndroidPrintStyle();
					await AndroidPrintStyleMapper.Instance.MapAsync(ContextGuid, item, tmp, "", token);
					newStyles.Add(tmp);
				}
				return newStyles;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 700);
				throw;
			}
		}
	}

	public async Task<List<AndroidPrintStyle>> GetFilledPrintStylesAsync(EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetFilledPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 2;
			try
			{
				int widthPx = ((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token))?.Settings?.Print?.PrintWidth).GetValueOrDefault(550);
				List<AndroidPrintStyle> styles = await GetPrintStylesAsync();
				if (styles == null)
				{
					return null;
				}
				foreach (AndroidPrintStyle item in styles)
				{
					await FillPrintSytleImageAsync(item, widthPx, tranName, token, system);
				}
				return styles;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFilledPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 719);
				return null;
			}
		}
	}

	public async Task FillPrintSytleImageAsync(AndroidPrintStyle style, int widthPx, string tranName, CancellationToken token, EnSystem system)
	{
		if (style == null)
		{
			throw new ArgumentNullException();
		}
		if (string.IsNullOrEmpty(style.Html))
		{
			return;
		}
		using (new JaegerMonitor("FillPrintSytleImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 7;
			try
			{
				UserInformationBusiness userInfo = await UserInformationBusiness.GetAsync(ContextGuid);
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				CusCompactBusiness cus = await CusCompactBusiness.GetAsync(ContextGuid, EnSystem.UserInterface, TafsilGuid, null, tranName, token);
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
				CompanySettings dep = setting.CompanyInfo;
				if (widthPx == 0)
				{
					widthPx = 550;
				}
				style.Html = ParameterReplacement.SetParameter_StringImage("WidthPx", widthPx + "px", style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("HeadFont", cashier.Settings.Print.HeadFont ?? (40 + "px"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("BodyFont", cashier.Settings.Print.BodyFont ?? (20 + "px"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("FactorType", "سند دریافت", style.Html);
				AndroidPrintStyle androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_StringImage("NO", (await GetId2Async(token)).ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Date", DateSh, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Time", Date.Hour + ":" + Date.Minute, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CodeCus", cus.Code.ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CusName", cus.FullName, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("TelWithTitle", cus.PrintTelWithLabel, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("AddWithTitle", cus.PrintAddWithLabel, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Add1", cus.Address1, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Add2", cus.Address2, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepComTitle", dep.CompanyTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepComEmail", dep.CompanyEmail, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepComZip", dep.CompanyZip, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepFax", dep.Fax, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepAddWithTitle", userInfo.PrintDepAddWithTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepAdd", dep.Address, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepTel", dep.Telephone, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepTelWithTitle", userInfo.PrintDepTelWithTitle, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepWebsite", dep.WebSite, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepMob", dep.Mobile, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepWebAbout", dep.Web_Pages_About, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("DepWebContact", dep.Web_Pages_ContactUS, style.Html);
				androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_DecimalImage("AfterAccount", await GetAfterAccountAsync(EnSystem.UserInterface, token), style.Html, addPrintModifier: true);
				androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_DecimalImage("BeforeAccount", await GetBeforeAccountAsync(EnSystem.UserInterface, token), style.Html, addPrintModifier: true);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("CurrentAccount", cus.Account, style.Html, addPrintModifier: true);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("SumKhadamat", 0m, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("TotalSum", Sum, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("FooterString", setting.printSettings.GetFooterString(style.PrintType), style.Html);
				androidPrintStyle = style;
				androidPrintStyle.Html = await SetParameters_AghlamImageAsync(style.Html, system, token, tranName);
				style.Html = style.Html.ToLower();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillPrintSytleImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 793);
				throw;
			}
		}
	}

	private async Task<string> SetParameters_AghlamImageAsync(string data, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		checked
		{
			using (new JaegerMonitor("SetParameters_AghlamImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
			{
				int line = 0;
				try
				{
					line = 1;
					string aghlam = "";
					ClsRegex clsRegex = new ClsRegex(data, "<ROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀:=\"a-zA-Z|;,*0-9-></ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوژءآ\r\n ]*</ROW>");
					line = 5;
					string aghlamTemplate = clsRegex.SuccessfullMatch.Replace("<ROW>", "").Replace("</ROW>", "");
					string aghlamTemplateWithRow = clsRegex.SuccessfullMatch;
					short index = 1;
					line = 10;
					if (ListCheck != null && ListCheck.Count > 0)
					{
						foreach (ReceptionCheckBusiness item3 in ListCheck)
						{
							line = 15;
							string data2 = ParameterReplacement.SetParameter_StringImage("Radif", index.ToString(), aghlamTemplate);
							data2 = ParameterReplacement.SetParameter_StringImage("RowDesc", item3.AndroidPrintText, data2);
							data2 = ParameterReplacement.SetParameter_DecimalImage("RowSum", item3.Price, data2);
							line = 20;
							aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data2) : data2);
							line = 25;
							index++;
						}
					}
					line = 30;
					if (ListNaghd != null && ListNaghd.Count > 0)
					{
						foreach (ReceptionNaghdBusiness item2 in ListNaghd)
						{
							line = 35;
							string temp4 = aghlamTemplate;
							temp4 = ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage("Radif", index.ToString(), temp4), parameterName: "RowDesc", parameterValue: await item2.GetAndroidPrintTextAsync(system, token, tranName)), parameterName: "RowSum", parameterValue: item2.Price);
							line = 40;
							aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + temp4) : temp4);
							line = 45;
							index++;
						}
					}
					line = 50;
					if (ListBank != null && ListBank.Count > 0)
					{
						foreach (ReceptionHavaleBusiness item in ListBank)
						{
							line = 55;
							string temp4 = aghlamTemplate;
							temp4 = ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage("Radif", index.ToString(), temp4), parameterName: "RowDesc", parameterValue: await item.GetAndroidPrintTextAsync(system, token, tranName)), parameterName: "RowSum", parameterValue: item.Price);
							line = 60;
							aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + temp4) : temp4);
							line = 70;
							index++;
						}
					}
					line = 75;
					if (aghlamTemplateWithRow == null || aghlamTemplateWithRow.Trim() == "")
					{
						return data;
					}
					return data.Replace(aghlamTemplateWithRow, aghlam);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SetParameters_AghlamImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 872);
					throw;
				}
			}
		}
	}

	public static Task<ReceptionBusiness> GetFromDbAsync(string contextGuid, Guid Guid, EnSystem system, OrderBusiness order, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetFromDbAsync(Guid, order, system, token, tranName);
	}

	public static async Task<ReceptionReportBusiness> GetReportAsync(string contextGuid, Guid Guid, CancellationToken token, string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetReportAsync(Guid, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = await CheckValidationAsync(system, isDeleteAction: true, tranName, null, token);
			if (ret.HasError)
			{
				return ret;
			}
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ReceptionBusiness obj = this;
			_sendEntitiesToServerToken?.Cancel();
			_sendEntitiesToServerToken = new CancellationTokenSource();
			CancellationToken sendEntitiesToServerToken = CancellationTokenSource.CreateLinkedTokenSource(token, _sendEntitiesToServerToken.Token).Token;
			ReturnedSaveFuncInfo result;
			try
			{
				int num;
				_ = num - 1;
				_ = 11;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 911), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 911);
					}
					transactionHasBeenCreated = true;
					bool flag = ((await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.GetFromDbAsync(Guid, _masterOrder, system, token, tranName))?.Sum ?? 0m) > 0m;
					if (system != EnSystem.UserInterface)
					{
						goto IL_0506;
					}
					if (!flag)
					{
						result = ret.AddError(system, "برگه دریافت با مشخصات مورد نظر یافت نشد Guid".GetTranslate() + $"({Guid})", "Guid", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 916);
					}
					else
					{
						ReceptionSanadStatusAccess receptionSanadStatusAccess = (await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName)).GetUserAsync(token, tranName)).UserAccess.Reception.SanadStatusAccess.SingleOrDefault((ReceptionSanadStatusAccess q) => q.StatusId == StatusId);
						if (receptionSanadStatusAccess == null || receptionSanadStatusAccess.Delete)
						{
							goto IL_0506;
						}
						result = ret.AddError("شما دسترسی به حذف برگه دریافت در وضعیت جاری را ندارید".GetTranslate(), "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 921);
					}
					goto IL_0db3;
					IL_0506:
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 924);
					if (ret.HasError)
					{
						result = ret;
					}
					else
					{
						if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.Reception, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 927);
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await UpdateCheckStatusAsync(system, tranName, isEbtal: true, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 929);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								if (IsTemp)
								{
									goto IL_09eb;
								}
								SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 935), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 935);
									if (sanad != null)
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await sanad.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(Document.Reception, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 937);
									}
									if (!ret.HasError)
									{
										goto IL_09eb;
									}
									result = ret;
								}
							}
						}
					}
					goto IL_0db3;
					IL_09eb:
					if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
					{
						IEnumerable<AghsatMBusiness> enumerable = await AghsatMBusiness.GetAllAsync(ContextGuid, new AghsatMFilter
						{
							ReceptionGuid = Guid
						}, token, tranName);
						if (enumerable != null && enumerable.Any())
						{
							foreach (AghsatMBusiness item in enumerable)
							{
								item.ReceptionGuid = null;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await item.SaveAsync(system, token, Priority.High, sendToServer: true, awaitOnSemaphore: true, await4Send: false, 10, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 948);
							}
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 955);
					ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 956);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 961), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 961);
				}
				if (!ret.HasError)
				{
					ClsCache.GetCache(ContextGuid).CacheRepository.IReception.ClearCachedItemAsync(Guid, token, "");
					CashiersLogBusiness.Logger(ContextGuid, system, sendToServer: true, EnLog.Reception, ENAction.Delete, obj);
					if (!ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
					{
						Task.Run(() => SendEntitiesToServerAsync(sendEntitiesToServerToken));
					}
				}
			}
			return ret;
			IL_0db3:
			return result;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, string contextGuid, EnSystem system, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReceptionBusiness receptionBusiness = await GetAsync(contextGuid, guid, system, null, fromDatabase: false, token);
			if (receptionBusiness != null)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.RemoveAsync(system, awaitOnSemaphore, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 979);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 983);
			ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 984);
		}
		return ret;
	}

	public static Task<Guid> Id2ToGuidAsync(string contextGuid, int id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReception.Id2ToGuidAsync(id2, token, tranName);
	}

	public int NextOrdinal(CancellationToken token = default(CancellationToken))
	{
		List<Sanad_DetBusiness> listEnteghal = _listEnteghal;
		if (listEnteghal == null || !listEnteghal.Any())
		{
			return 0;
		}
		return checked((_listEnteghal?.Max((Sanad_DetBusiness p) => p.Ordinal) ?? (-1)) + 1);
	}

	public static Task<int> NextId2Async(string contextGuid, int? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReception.NextId2Async(statusId, token, tranName);
	}

	public Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return NextId2Async(ContextGuid, statusId, token, tranName);
	}

	[Obsolete("Just for transfer reception from V5 to V6")]
	public Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, int oldSanadNumber, CancellationToken token = default(CancellationToken))
	{
		_oldReception = new ReceptionBusiness(ContextGuid);
		_oldReception.SanadNo = oldSanadNumber;
		return SaveAsync(system, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, "", token);
	}

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 2;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebReception webEntity = await ToWebItemAQsync();
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid);
				CashiersLogBusiness cashiersLogBusiness = new CashiersLogBusiness(ContextGuid)
				{
					Action = ENAction.Error,
					ApplicationVersion = (ClsCache.GetCache(ContextGuid)?.ApplicationVersion ?? ""),
					CashierGuid = CashierBusiness.GetCurrentCashierGuid(ContextGuid),
					CashierName = cashierBusiness?.Name,
					ContextGuid = ContextGuid,
					DateM = DateTime.Now,
					DateUtc = DateTime.Now.ToUniversalTime(),
					Guid = Guid.NewGuid(),
					System = system,
					Source = ClsCache.GetCache(ContextGuid).Source,
					SanadType = EnLog.Reception,
					SanadGuid = Guid
				};
				CashiersLogBusiness cashiersLogBusiness2 = cashiersLogBusiness;
				object arg = webEntity.Id2;
				cashiersLogBusiness2.Title = $"خطا در ذخیره دریافت ({arg})\r\n{await GetFullNameAsync(system)}-{Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1032);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, string CustomerUserName = "", string CustomerPassword = "", int timeout = 60000, bool await4send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), SanadBusiness orderSanad = null, IMessageSender messageSender = null)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			bool transactionHasBeenCreated = false;
			ENAction action = ENAction.Save;
			TaskCompletionSource<bool> saveFinished = new TaskCompletionSource<bool>();
			_sendEntitiesToServerToken?.Cancel();
			_sendEntitiesToServerToken = new CancellationTokenSource();
			CancellationToken sendEntitiesToServerToken = CancellationTokenSource.CreateLinkedTokenSource(token, _sendEntitiesToServerToken.Token).Token;
			try
			{
				_ = 20;
				checked
				{
					ReturnedSaveFuncInfo result;
					try
					{
						if (!_isTemp)
						{
							ServerStatus = ServerStatusForClient.None;
							HostRefrenced = null;
						}
						using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "RecptionBusiness/SaveAsync", 60000, token))
						{
							if (dis.Token.IsCancellationRequested)
							{
								result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1057);
							}
							else
							{
								if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
								{
									sendToServer = false;
									if (IsTemp)
									{
										throw new NotImplementedException("IsTemp is NotImplemented in Accounting");
									}
								}
								Modified = DateTime.Now;
								ReturnedSaveFuncInfo returnedSaveFuncInfo;
								if (autoCommit)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1064), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1064);
								}
								transactionHasBeenCreated = true;
								if (ChangeStatus == EntityChangeSt.None)
								{
									ChangeStatus = EntityChangeSt.New;
								}
								if (ChangeStatus == EntityChangeSt.New)
								{
									int id = await GetId2Async(token, tranName);
									if (await GetAsync(ContextGuid, id, system, token, tranName) != null)
									{
										SetId2(await NextId2Async(ContextGuid, StatusId, token, tranName), token);
									}
								}
								Tuple<Document, Guid> relatedOrder = null;
								if (_masterOrder != null)
								{
									relatedOrder = new Tuple<Document, Guid>(_masterOrder.FactorType, _masterOrder.Guid);
								}
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, tranName, relatedOrder, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1079);
								if (system != EnSystem.UserInterface)
								{
									ret.throwExceptionIfError();
								}
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									await UpdateBeforeAccountAsync(system, token, tranName);
									if (MetaData == null)
									{
										MetaData = new ReceptionMetaData();
									}
									if (MetaData.System == EnSystem.None)
									{
										MetaData.System = system;
									}
									await SanadStatusReportBusiness.ApplySanadStatusIdAsync(ContextGuid, this, (system == EnSystem.UserInterface) ? system : MetaData.System, token, tranName);
									List<BaseKhazaneDetail> list = DetailList.OrderBy((BaseKhazaneDetail p) => p.Ordinal).ToList();
									for (int i = 0; i < list.Count(); i++)
									{
										if (list[i].Ordinal != i + 1)
										{
											list[i].SetOrdinal(EnSystem.None, i + 1);
										}
									}
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1091);
									if (ret.HasError)
									{
										result = ret;
									}
									else
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await UpdateCheckStatusAsync(system, tranName, isEbtal: false, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1093);
										if (ret.HasError)
										{
											result = ret;
										}
										else
										{
											if (!IsTemp)
											{
												SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
												if (orderSanad == null)
												{
													if (system != EnSystem.Transfer && system != 0)
													{
														await sanad.CheckValidationAsync(system, true, tranName, false, token, new Tuple<Document, Guid>(Document.Reception, Guid));
													}
													if (SanadNo <= 0 && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
													{
														SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
														returnedSaveFuncInfo = ret;
														returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.SetSanadNoAsync(Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1105);
													}
													sanad.SanadNo = SanadNo;
													List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
													returnedSaveFuncInfo = ret;
													returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.Reception, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1110);
												}
												else
												{
													returnedSaveFuncInfo = ret;
													returnedSaveFuncInfo.AddReturnedValue(await orderSanad.JoinAsync(sanad, system, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1112);
												}
											}
											if (ret.HasError)
											{
												result = ret;
											}
											else
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await SaveFilesAsync(token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1115);
												if (ret.HasError)
												{
													result = ret;
												}
												else
												{
													if (Source == ENSource.Android_HesabYar || system == EnSystem.Transfer)
													{
														goto IL_10f2;
													}
													returnedSaveFuncInfo = ret;
													returnedSaveFuncInfo.AddReturnedValue(await UpdateDateMoaserAsync(ContextGuid, Guid, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1119);
													if (!ret.HasError)
													{
														returnedSaveFuncInfo = ret;
														returnedSaveFuncInfo.AddReturnedValue(await AghsatMBusiness.UpdateReceptionInfoAsync(ContextGuid, Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1121);
														goto IL_10f2;
													}
													result = ret;
												}
											}
										}
									}
								}
							}
							goto end_IL_017c;
							IL_10f2:
							if (!ret.HasError)
							{
								_oldReception = await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.GetFromDbAsync(Guid, _masterOrder, system, token, tranName);
								goto end_IL_027f;
							}
							result = ret;
							goto end_IL_017c;
							end_IL_027f:;
						}
						goto end_IL_0166;
						end_IL_017c:;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1128);
						ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1129);
						goto end_IL_0166;
					}
					return result;
				}
				end_IL_0166:;
			}
			finally
			{
				TaskCompletionSource<ReturnedSaveFuncInfo> transactionResult = await ClsCache.ObserveTransactionDistinity(ContextGuid, tranName);
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1135), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1135);
				}
				if (!ret.HasError)
				{
					SaveTransactionDistinity(transactionResult, system, sendToServer, action, await4send, token, priority, messageSender, CustomerUserName, CustomerPassword, timeout, autoCommit);
					if (ClsCache.GetCache(ContextGuid).BufferEnable && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
					{
						Task.Run(() => SendEntitiesToServerAsync(sendEntitiesToServerToken));
					}
				}
				if (ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					Task.Run(() => SanadBusiness.RemoveDuplicateSanadNoAsync(ContextGuid, token));
				}
				ReceptionWebServiceRepository receptionWebServiceRepository = ClsCache.GetCache(ContextGuid)?.WebService?.Reception;
				if (receptionWebServiceRepository != null)
				{
					new Thread(receptionWebServiceRepository.ForceToSendDataToServer).Start();
				}
				saveFinished?.TrySetResult(result: true);
				if (!token.IsCancellationRequested)
				{
					SaveLogAsync(system, ret);
				}
			}
			return ret;
		}
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token)
	{
		using (new JaegerMonitor("SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 3;
			try
			{
				if (ClsCache.GetCache(ContextGuid)?.BufferCancellationToken.IsCancellationRequested ?? true)
				{
					return;
				}
				await Task.Delay(500);
				if (!token.IsCancellationRequested)
				{
					await Task.Delay(checked((await LocalSettingBusiness.GetWebServiceSettingsAsync(ContextGuid, token)).OperationEntitiesSendToServerSeconds * 1000));
					if (!token.IsCancellationRequested)
					{
						await Task.WhenAll<ReturnedSaveFuncInfo>(KolBusiness.ResendNotSents(ContextGuid, token, "", 5), MoeinBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), TafsilBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), ViewCheckMBusiness.ResendNotSentAsync(ContextGuid, token, "", 20), AccountBusiness.ResendNotSentsAsync(ContextGuid, EnSystem.None, token, "", 10));
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1173);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> UpdateCheckStatusAsync(EnSystem system, string tranName, bool isEbtal, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			if (isEbtal)
			{
				foreach (ReceptionCheckBusiness item in (await ReceptionCheckBusiness.GetAllAsync(ContextGuid, Guid, token, tranName)) ?? new List<ReceptionCheckBusiness>())
				{
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(ContextGuid, item.Guid, token, tranName);
					if (receptionCheckBusiness.Status == ENUMCheck.BatelShode)
					{
						ChequeMOperationFilter filter = new ChequeMOperationFilter
						{
							ChequeGuid = receptionCheckBusiness.Guid
						};
						ChequeMOperationBusiness chequeMOperationBusiness = (await ChequeMOperationBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.OrderByDescending((ChequeMOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
						if (chequeMOperationBusiness != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await chequeMOperationBusiness.RemoveAsync(system, isRemoveOldSanad: false, ENUMCheck.None, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1193);
						}
					}
					else if (receptionCheckBusiness.Status == ENUMCheck.Mojod)
					{
						receptionCheckBusiness.Status = ENUMCheck.BatelShode;
						if (source == ENSource.Accounting)
						{
							receptionCheckBusiness.ServerStatus = ServerStatusForClient.None;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await receptionCheckBusiness.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1199);
					}
					else if (system != EnSystem.Transfer)
					{
						throw new ArgumentException($"ChequeM \r\n Status:{receptionCheckBusiness.Status.GetDisplay()} \r\n MasterGuid: {Guid}");
					}
				}
			}
			if (!isEbtal)
			{
				List<ReceptionCheckBusiness> list = (await ReceptionCheckBusiness.GetAllAsync(ContextGuid, Guid, token, tranName))?.Where((ReceptionCheckBusiness q) => !(ListCheck?.Select((ReceptionCheckBusiness u) => u.Guid)?.Contains(q.Guid)).GetValueOrDefault())?.ToList();
				foreach (ReceptionCheckBusiness item2 in list ?? new List<ReceptionCheckBusiness>())
				{
					ReceptionCheckBusiness receptionCheckBusiness2 = await ReceptionCheckBusiness.GetAsync(ContextGuid, item2.Guid, token, tranName);
					if (receptionCheckBusiness2.Status == ENUMCheck.BatelShode)
					{
						ChequeMOperationFilter filter2 = new ChequeMOperationFilter
						{
							ChequeGuid = receptionCheckBusiness2.Guid
						};
						ChequeMOperationBusiness chequeMOperationBusiness2 = (await ChequeMOperationBusiness.GetAllAsync(ContextGuid, filter2, token, tranName))?.OrderByDescending((ChequeMOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
						if (chequeMOperationBusiness2 != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await chequeMOperationBusiness2.RemoveAsync(system, isRemoveOldSanad: false, ENUMCheck.None, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1218);
						}
						continue;
					}
					if (receptionCheckBusiness2.Status == ENUMCheck.Mojod)
					{
						receptionCheckBusiness2.Status = ENUMCheck.BatelShode;
						if (source == ENSource.Accounting)
						{
							receptionCheckBusiness2.ServerStatus = ServerStatusForClient.None;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await receptionCheckBusiness2.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1224);
						continue;
					}
					throw new ArgumentException($"ChequeM \r\n Status:{receptionCheckBusiness2.Status.GetDisplay()} \r\n MasterGuid: {Guid}");
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1232);
			WebErrorLog.Instance.StartLog(ex, "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1233);
		}
		return ret;
	}

	private async Task SaveTransactionDistinity(TaskCompletionSource<ReturnedSaveFuncInfo> transactionResult, EnSystem system, bool sendToServer, ENAction action, bool await4Sent, CancellationToken token, Priority priority, IMessageSender messageSender, string CustomerUserName, string CustomerPassword, int timeout, bool autoCommit)
	{
		using (new JaegerMonitor("SaveTransactionDistinity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 3;
			try
			{
				ReturnedSaveFuncInfo ret = await transactionResult.Task;
				if (ret.HasError)
				{
					return;
				}
				ChangeStatus = EntityChangeSt.Edited;
				ClsCache.GetCache(ContextGuid).CacheRepository.IReception.ClearCachedItemAsync(Guid, token, "");
				CashiersLogBusiness.Logger(ContextGuid, system, sendToServer, EnLog.Reception, action, this);
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					CusCompactBusiness cusCompactBusiness = await CusInfoAsync(EnSystem.None, "", token);
					if (cusCompactBusiness == null)
					{
						WebErrorLog.Instance.StartLog($"Customer not exists on database. Guid({TafsilGuid})", "", "SaveTransactionDistinity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1254);
						return;
					}
					if (!ret.HasError && sendToServer && !IsTemp && (cusCompactBusiness.ServerStatus == ServerStatusForClient.Delivered || cusCompactBusiness.ServerStatus == ServerStatusForClient.DirectDelivery) && ClsCache.GetCache(ContextGuid).BufferLoginState.Status == EnLoginResult.Success && ServerStatus != ServerStatusForClient.LinkToOrder)
					{
						_send2ServerToken?.Cancel();
						_send2ServerToken = new CancellationTokenSource(60000);
						CancellationToken send2ServerToken = _send2ServerToken.Token;
						if (await4Sent)
						{
							await Send2ServerAsync(CustomerUserName, CustomerPassword, timeout, send2ServerToken, Priority.High);
						}
						else
						{
							Task.Run(() => Send2ServerAsync(CustomerUserName, CustomerPassword, timeout, send2ServerToken, Priority.High));
						}
					}
				}
				if (!ret.HasError && messageSender != null && autoCommit)
				{
					await SendMessageAsync(system, messageSender);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveTransactionDistinity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1273);
			}
		}
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[نام مشتری]"),
			new SmsTemplateRecord("[تاریخ]"),
			new SmsTemplateRecord("[شماره]"),
			new SmsTemplateRecord("[حساب فعلی]"),
			new SmsTemplateRecord("[جمع اقلام]"),
			new SmsTemplateRecord("[مبلغ]"),
			new SmsTemplateRecord("[عنوان فروشگاه]"),
			new SmsTemplateRecord("[تلفن فروشگاه]")
		};
	}

	private async Task<string> FillSmsParametersAsync(EnSystem system, string message, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, "", token);
			if (message.Contains("[نام مشتری]"))
			{
				string text = message;
				message = text.Replace("[نام مشتری]", await GetFullNameAsync(system, token));
			}
			if (message.Contains("[تاریخ]"))
			{
				message = message.Replace("[تاریخ]", GetDate().MiladiToCalture());
			}
			if (message.Contains("[شماره]"))
			{
				message = message.Replace("[شماره]", _id2.ToString("N0"));
			}
			if (message.Contains("[حساب فعلی]"))
			{
				message = message.Replace("[حساب فعلی]", account.AccountFormated(shortFormat: false));
			}
			if (message.Contains("[جمع اقلام]"))
			{
				message = message.Replace("[جمع اقلام]", Sum.NpToString("N0", 0));
			}
			if (message.Contains("[مبلغ]"))
			{
				message = message.Replace("[مبلغ]", Sum.NpToString("N0", 0));
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
			WebErrorLog.Instance.StartLog(ex, "", "FillSmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1304);
			throw;
		}
	}

	private async Task<string> FillSmsParametersTorTransferAsync(EnSystem system, Sanad_DetBusiness transferItem, string message, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if ((transferItem?.TafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				return "";
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
			decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, transferItem.TafsilGuid.Value, true, "", token);
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, system, transferItem.TafsilGuid.Value, "", token, "FillSmsParametersTorTransferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
			if (message.Contains("[نام مشتری]"))
			{
				message = message.Replace("[نام مشتری]", tafsilBusiness?.Name ?? "");
			}
			if (message.Contains("[مبلغ]"))
			{
				message = message.Replace("[مبلغ]", transferItem.Price.NpToString("N0", 0));
			}
			if (message.Contains("[طرف حساب دریافت]"))
			{
				string text = message;
				message = text.Replace("[طرف حساب دریافت]", await GetFullNameAsync(system, token));
			}
			if (message.Contains("[مانده حساب]"))
			{
				message = message.Replace("[مانده حساب]", account.AccountFormated(shortFormat: false));
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
			WebErrorLog.Instance.StartLog(ex, "", "FillSmsParametersTorTransferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1327);
			throw;
		}
	}

	private async Task SendMessageAsync(EnSystem system, IMessageSender messageSender)
	{
		try
		{
			AccGlobalSettings settings = await LocalSettingBusiness.GetSettingAsync(ContextGuid);
			if (!settings.Payamak.RunAfterInitializeProgram)
			{
				return;
			}
			if (settings.Payamak.DaryaftAccountStatus != PayamakSendStatus.Disable)
			{
				string message2 = settings.Payamak.ReceptionMessage;
				message2 = await FillSmsParametersAsync(system, message2);
				if (!string.IsNullOrEmpty(message2))
				{
					string mobiles2 = string.Join(",", (await TafsilNumberBusiness.GetAsync(ContextGuid, _tafsilGuid))?.Where((TafsilNumberBusiness p) => p.Mobile)?.Select((TafsilNumberBusiness p) => p.NumberForDisplay()));
					if (mobiles2?.Any() ?? false)
					{
						CancellationToken token2 = new CancellationTokenSource(60000).Token;
						Task.Run(() => messageSender.SendSmsAsync(settings.Payamak.DaryaftAccountStatus, settings.Payamak.Account_ID, message2, mobiles2, token2));
					}
				}
			}
			if (settings.Payamak.ReceptionEnteghalStatus == PayamakSendStatus.Disable || !(ListEnteghal?.Any() ?? false))
			{
				return;
			}
			string message = settings.Payamak.ReceptionEnteghalMessage;
			foreach (Sanad_DetBusiness item in ListEnteghal)
			{
				if ((item?.TafsilGuid ?? Guid.Empty) == Guid.Empty)
				{
					continue;
				}
				string text = await FillSmsParametersTorTransferAsync(system, item, message);
				message = text;
				if (string.IsNullOrEmpty(message))
				{
					continue;
				}
				string mobiles = string.Join(",", (await TafsilNumberBusiness.GetAsync(ContextGuid, item.TafsilGuid.Value))?.Where((TafsilNumberBusiness p) => p.Mobile)?.Select((TafsilNumberBusiness p) => p.NumberForDisplay()));
				if (mobiles?.Any() ?? false)
				{
					CancellationToken token = new CancellationTokenSource(60000).Token;
					Task.Run(() => messageSender.SendSmsAsync(settings.Payamak.ReceptionEnteghalStatus, settings.Payamak.Account_ID, message, mobiles, token));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1372);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName = "", Tuple<Document, Guid> relatedOrder = null, CancellationToken token = default(CancellationToken))
	{
		int line = 0;
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
			try
			{
				if ((system != EnSystem.Transfer && system != 0 && Date < new DateTime(2010, 1, 1)) || Date > new DateTime(2099, 1, 1))
				{
					result.AddError(system, "تاریخ وارد شده جهت برگه دریافت معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1384);
				}
				line = 1;
				if (TafsilGuid == Guid.Empty)
				{
					result.AddError(system, "ردیف طرف حساب انتخاب شده جهت طرف حساب برگه دریافت معتبر نمی باشد و یا شما دسترسی به طرف حساب انتخاب شده ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1387);
				}
				line = 2;
				if (MoeinGuid == Guid.Empty)
				{
					result.AddError(system, "حساب معین انتخاب شده جهت طرف حساب برگه دریافت معتبر نمی باشد و یا شما دسترسی به حساب معین انتخاب شده ندارید".GetTranslate() + "\r\n" + $"MoeinGuid:{MoeinGuid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1390);
				}
				line = 3;
				if (system != EnSystem.Transfer && system != 0 && !isDeleteAction && Sum <= 0m && SumBank <= 0m && SumBankHavale <= 0m && SumBankPos <= 0m && SumCheck <= 0m && SumEnteghal <= 0m && SumNaghd <= 0m)
				{
					result.AddError(system, "برگه دریافت با مبلغ صفر یا منفی قابل ثبت نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1394);
				}
				line = 4;
				if (system != EnSystem.Transfer && system != 0 && !IsTemp && string.IsNullOrEmpty(Description))
				{
					result.AddError(system, "بخش بابت سند دریافت تکمیل نشده".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1397);
				}
				line = 5;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
					if (system != EnSystem.Transfer)
					{
						result.AddInformation($"ReceptionBusiness=>Change Guid.Empty To New Guid {Guid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1403);
					}
				}
				line = 6;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = result;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationChequeAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1406);
				line = 7;
				returnedSaveFuncInfo = result;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationNaghdAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1408);
				line = 8;
				returnedSaveFuncInfo = result;
				returnedSaveFuncInfo.AddReturnedValue(await CheckValidationBankAsync(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1410);
				line = 9;
				returnedSaveFuncInfo = result;
				EnSystem system2 = system;
				bool isDeleteAction2 = isDeleteAction;
				returnedSaveFuncInfo.AddReturnedValue(await CheckSanadValidationAsync(system2, isDeleteAction2, await GetId2Async(token, tranName), tranName, token, relatedOrder), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1412);
				line = 10;
				if (system != EnSystem.Transfer && system != 0 && relatedOrder == null)
				{
					List<HesabChanges> lstChange = await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, new Tuple<Document, Guid>(Document.Reception, Guid));
					returnedSaveFuncInfo = result;
					returnedSaveFuncInfo.AddReturnedValue(await lstChange.CheckAccountAsync(system, ContextGuid, token, tranName, Document.Reception, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1417);
					line = 11;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Error Line Is :{line}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1423);
				result.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1424);
			}
			return result;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationChequeAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (ListCheck?.Any() ?? false)
			{
				foreach (ReceptionCheckBusiness item in ListCheck)
				{
					if (item == null)
					{
						res.AddError(system, "در لیست اسناد دریافتنی(چک مشتریان) مقدار نال موجود میباشد.".GetTranslate(), "", "CheckValidationChequeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1438);
						continue;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidationAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationChequeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1441);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationChequeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1446);
			res.AddReturnedValue(ex, "", "CheckValidationChequeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1447);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationNaghdAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<ReceptionNaghdBusiness> listNaghd = ListNaghd;
			if (listNaghd != null && listNaghd.Count > 0)
			{
				foreach (ReceptionNaghdBusiness item in ListNaghd)
				{
					if (item == null)
					{
						res.AddError(system, "در لیست در یافتی های نقدی مقدار نال موجود میباشد.".GetTranslate(), "", "CheckValidationNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1461);
						continue;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidationAsync(system, token, tranName), ReturnedState.None, "CheckValidationNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1464);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1469);
			res.AddReturnedValue(ex, "", "CheckValidationNaghdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1470);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationBankAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<ReceptionHavaleBusiness> listBank = ListBank;
			if (listBank != null && listBank.Count > 0)
			{
				foreach (ReceptionHavaleBusiness item in ListBank)
				{
					if (item == null)
					{
						res.AddError(system, "در لیست حواله ها مقدار نال موجود میباشد.".GetTranslate(), "", "CheckValidationBankAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1484);
						continue;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidationAsync(system, token, tranName), ReturnedState.None, "CheckValidationBankAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1487);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationBankAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1492);
			res.AddReturnedValue(ex, "", "CheckValidationBankAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1493);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> SaveFilesAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (ListCheck?.Any() ?? false)
			{
				foreach (ReceptionCheckBusiness item in ListCheck)
				{
					if ((item != null && item.AddedFiles.Any()) || (item.RemovedFiles?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, item.AddedFiles, item.RemovedFiles, token), ReturnedState.None, "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1506);
					}
				}
			}
			if (ListBank?.Any() ?? false)
			{
				foreach (ReceptionHavaleBusiness item2 in ListBank)
				{
					if ((item2 != null && item2.AddedFiles.Any()) || (item2.RemovedFiles?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, item2.AddedFiles, item2.RemovedFiles, token), ReturnedState.None, "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1513);
					}
				}
			}
			if (ListEnteghal?.Any() ?? false)
			{
				foreach (Sanad_DetBusiness item3 in ListEnteghal)
				{
					if ((item3 != null && item3.AddedFiles.Any()) || (item3.RemovedFiles?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, item3.AddedFiles, item3.RemovedFiles, token), ReturnedState.None, "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1520);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1525);
			res.AddReturnedValue(ex, "", "SaveFilesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1526);
		}
		return res;
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		_listBank = null;
		_listCheck = null;
		_listEnteghal = null;
		_listNaghd = null;
	}

	public void AddToDetList(BaseKhazaneDetail item, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				if (item == null)
				{
					return;
				}
				if (item.Ordinal <= 0)
				{
					IReadOnlyCollection<BaseKhazaneDetail> detailList = DetailList;
					if (detailList != null && detailList.Any())
					{
						int num = DetailList.Max((BaseKhazaneDetail q) => q.Ordinal);
						if (num <= 0)
						{
							item.SetOrdinal(system, 1);
						}
						else
						{
							item.SetOrdinal(system, checked(num + 1));
						}
					}
					else
					{
						item.SetOrdinal(system, 1);
					}
				}
				ReceptionNaghdBusiness receptionNaghdBusiness = item as ReceptionNaghdBusiness;
				if (receptionNaghdBusiness == null)
				{
					ReceptionCheckBusiness receptionCheckBusiness = item as ReceptionCheckBusiness;
					if (receptionCheckBusiness == null)
					{
						ReceptionHavaleBusiness receptionHavaleBusiness = item as ReceptionHavaleBusiness;
						if (receptionHavaleBusiness == null)
						{
							Sanad_DetBusiness sanad_DetBusiness = item as Sanad_DetBusiness;
							if (sanad_DetBusiness == null)
							{
								throw new NotImplementedException();
							}
							if (_listEnteghal == null)
							{
								_listEnteghal = new List<Sanad_DetBusiness>();
							}
							if (!_listEnteghal.Any((Sanad_DetBusiness p) => p.Guid == sanad_DetBusiness.Guid))
							{
								sanad_DetBusiness.AsGuid = Guid;
								sanad_DetBusiness.MasterGuid = Guid;
								if (sanad_DetBusiness.Debit > 0m)
								{
									sanad_DetBusiness.SetPrice(system, sanad_DetBusiness.Debit);
								}
								else
								{
									sanad_DetBusiness.SetPrice(system, sanad_DetBusiness.Credit);
								}
								_listEnteghal.Add(sanad_DetBusiness);
							}
						}
						else
						{
							if (_listBank == null)
							{
								_listBank = new List<ReceptionHavaleBusiness>();
							}
							if (!_listBank.Any((ReceptionHavaleBusiness p) => p.Guid == receptionHavaleBusiness.Guid))
							{
								receptionHavaleBusiness.MasterGuid = Guid;
								_listBank.Add(receptionHavaleBusiness);
							}
						}
					}
					else
					{
						if (_listCheck == null)
						{
							_listCheck = new List<ReceptionCheckBusiness>();
						}
						ReceptionCheckBusiness receptionCheckBusiness2 = _listCheck.SingleOrDefault((ReceptionCheckBusiness p) => p.Guid == receptionCheckBusiness.Guid);
						if (receptionCheckBusiness2 != null && receptionCheckBusiness2.ContextGuid != ContextGuid)
						{
							throw new ArgumentException("Exitss checkM contextGuid is :" + receptionCheckBusiness2.ContextGuid + " but reception.ContextGuid is:" + ContextGuid);
						}
						if (receptionCheckBusiness2 == null)
						{
							receptionCheckBusiness.MasterGuid = Guid;
							_listCheck.Add(receptionCheckBusiness);
						}
					}
				}
				else
				{
					if (_listNaghd == null)
					{
						_listNaghd = new List<ReceptionNaghdBusiness>();
					}
					if (!_listNaghd.Any((ReceptionNaghdBusiness p) => p.Guid == receptionNaghdBusiness.Guid))
					{
						receptionNaghdBusiness.MasterGuid = Guid;
						_listNaghd.Add(receptionNaghdBusiness);
					}
				}
				item.OnDisposed += Item_OnDisposed;
				item.OnOrdinalChanged += Item_OnOrdinalChanged;
				item.OnPriceChange += Det_OnPriceChange;
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1605);
				throw;
			}
		}
	}

	private Task Item_OnOrdinalChanged(object sender, OrdinalChangedEventArgs e)
	{
		try
		{
			if (base.IsDisposed)
			{
				return Task.CompletedTask;
			}
			if (e.OldOrdinal == e.NewOrdinal || e.OldOrdinal < 0)
			{
				return Task.CompletedTask;
			}
			(DetailList?.FirstOrDefault((BaseKhazaneDetail p) => p != null && !p.IsDisposed && p.Ordinal == e.NewOrdinal && p.Guid != e.DetailGuid))?.SetOrdinal(e.System, e.OldOrdinal);
			if (this.OnOrdinalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, sender, e, default(CancellationToken), 60, "Item_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Item_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1623);
		}
		return Task.CompletedTask;
	}

	private Task Item_OnDisposed(object sender, EventArgs e)
	{
		checked
		{
			try
			{
				BaseKhazaneDetail det = (BaseKhazaneDetail)sender;
				det.OnOrdinalChanged -= Item_OnOrdinalChanged;
				det?.Dispose();
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Item_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
				}
				List<BaseKhazaneDetail> list = (from p in DetailList
					where p.Guid != det.Guid
					orderby p.Ordinal
					select p).ToList();
				for (int i = 0; i < list.Count(); i++)
				{
					list[i].SetOrdinal(EnSystem.None, i + 1);
				}
				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Item_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1644);
				throw;
			}
		}
	}

	public async Task<TableRelationShipBusiness> GetLinkedSanadAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			if (_linkedSanad != null)
			{
				if (_linkedSanad.SanadType == Document.None)
				{
					_linkedSanad.SanadType = Document.Reception;
				}
				if (_linkedSanad.SanadGuid != Guid)
				{
					_linkedSanad.SanadGuid = Guid;
				}
				return _linkedSanad;
			}
			IEnumerable<TableRelationShipBusiness> relatedSanad = await TableRelationShipBusiness.GetAllRelatedSanadAsync(ContextGuid, Guid, Document.Reception, token, tranName);
			IEnumerable<TableRelationShipBusiness> enumerable = await TableRelationShipBusiness.GetAllInRelationSanadAsync(ContextGuid, Guid, Document.Reception, token);
			if (enumerable != null)
			{
				relatedSanad = relatedSanad?.Union(enumerable);
			}
			await AddListToLinkedSanadsAsync(relatedSanad);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetLinkedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1665);
		}
		return _linkedSanad;
	}

	public async Task AddListToLinkedSanadsAsync(IEnumerable<TableRelationShipBusiness> lstDets, bool isClear = false)
	{
		using (new JaegerMonitor("AddListToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				foreach (TableRelationShipBusiness item in lstDets ?? new List<TableRelationShipBusiness>())
				{
					await AddToLinkedSanadsAsync(item.SanadGuid, item.SanadType, item.Price, isClear);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "AddListToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1678);
			}
		}
	}

	public void RemoveFromLinkedSanad(Guid sanadGuid, Document sanadType)
	{
		using (new JaegerMonitor("RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				if (_linkedSanad == null)
				{
					_linkedSanad = new TableRelationShipBusiness(ContextGuid);
				}
				TableRelationShipBusiness tableRelationShipBusiness = _linkedSanad?.ChildList?.FirstOrDefault((TableRelationShipBusiness p) => p.SanadGuid == sanadGuid && p.SanadType == sanadType);
				if (tableRelationShipBusiness != null)
				{
					_linkedSanad.RemoveFromDets(tableRelationShipBusiness);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1689);
			}
		}
	}

	public async Task AddToLinkedSanadsAsync(Guid relatedSanadGuid, Document relatedSanadType, decimal price, bool isClear = false)
	{
		using (new JaegerMonitor("AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				if (_linkedSanad?.ChildList?.FirstOrDefault((TableRelationShipBusiness q) => q.SanadGuid == relatedSanadGuid && q.SanadType == relatedSanadType) == null)
				{
					TableRelationShipBusiness item = new TableRelationShipBusiness(ContextGuid)
					{
						ContextGuid = ContextGuid,
						Guid = Guid.NewGuid(),
						IsInRelationSanad = false,
						Price = price,
						SanadGuid = relatedSanadGuid,
						SanadType = relatedSanadType,
						Status = true
					};
					if (_linkedSanad == null)
					{
						_linkedSanad = new TableRelationShipBusiness(ContextGuid);
					}
					if (isClear)
					{
						_linkedSanad?.ClearDets();
					}
					await _linkedSanad.AddChildListAsync(new List<TableRelationShipBusiness> { item });
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1713);
			}
		}
	}

	public async Task<CashierBusiness> GetCashierAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (ChangeStatus == EntityChangeSt.Edited || Sum > 0m)
			{
				return await CashierBusiness.GetAsync(ContextGuid, UserGuid, token);
			}
			return await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1725);
			return null;
		}
	}

	public async Task<string> GetCashierNameAsync(CancellationToken token = default(CancellationToken))
	{
		return (await GetCashierAsync(token))?.Name ?? "";
	}

	public Task<CusCompactBusiness> CusInfoAsync(EnSystem system, string tranName, CancellationToken token)
	{
		return CusCompactBusiness.GetAsync(ContextGuid, system, TafsilGuid, null, tranName, token);
	}

	public TafsilBusiness GetTafsil(EnSystem system = EnSystem.None, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return TafsilBusiness.Get(ContextGuid, TafsilGuid, token, tranName);
	}

	public Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", bool useCache = true)
	{
		if (!useCache)
		{
			return TafsilBusiness.GetFromDatabaseAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
		}
		return TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
	}

	public async Task<int> GetId2Async(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("GetId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 2;
			try
			{
				if (_id2 <= 0)
				{
					_id2 = await NextId2Async(ContextGuid, StatusId, token, tranName);
					return _id2;
				}
				Guid guid = await Id2ToGuidAsync(ContextGuid, _id2, token, tranName);
				if (guid == Guid.Empty || guid == Guid)
				{
					return _id2;
				}
				_id2 = await NextId2Async(ContextGuid, StatusId, token, tranName);
				return _id2;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1754);
				throw;
			}
		}
	}

	public void SetId2(int value, CancellationToken token = default(CancellationToken))
	{
		_id2 = value;
	}

	internal static async Task<ReceptionBusiness> GetReceptionBusinessAsync(string contextGuid, EnSystem system, GardeshHesabBusiness hostGardesh, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReceptionBusiness ret = new ReceptionBusiness(contextGuid);
		if (hostGardesh == null)
		{
			throw new NullReferenceException("hostGardesh");
		}
		try
		{
			ret.ContextGuid = hostGardesh.ContextGuid;
			ret.Guid = hostGardesh.AsGuid.Value;
			ret.Description = hostGardesh.SanadDesc;
			ret.Date = hostGardesh.Date;
			ret.SetId2(hostGardesh.Id2, token);
			ret.UserGuid = hostGardesh.UGuid;
			await ret.SetTafsilGuidAsync(hostGardesh.TafsilGuid.Value, system, token, tranName);
			string desc = "";
			foreach (GardeshTafsilBusiness item in hostGardesh.TafsiliItems)
			{
				if (item != null)
				{
					switch (item.AsTypeEnum)
					{
					case DocumentDet.DaryaftCh:
					{
						ReceptionCheckBusiness receptionCheckBusiness = new ReceptionCheckBusiness(contextGuid)
						{
							Date = item.DateMoaser,
							Description = item.RowDesc,
							Guid = item.Guid,
							MasterGuid = item.Guid,
							Number = item.Serial
						};
						receptionCheckBusiness.SetPrice(system, item.Credit);
						ret.AddToDetList(receptionCheckBusiness, system, token);
						break;
					}
					case DocumentDet.DaryaftH:
					{
						ReceptionHavaleBusiness receptionHavaleBusiness2 = new ReceptionHavaleBusiness(contextGuid)
						{
							Date = item.DateMoaser,
							Description = item.RowDesc,
							Guid = item.Guid,
							MoeinGuid = item.MoeinGuid,
							DecimalGuid = item.DecimalGuid,
							MasterGuid = item.Guid,
							Number = item.Serial
						};
						ReceptionHavaleBusiness receptionHavaleBusiness = receptionHavaleBusiness2;
						receptionHavaleBusiness.TafsilGuid = (await BankBusiness.GetAllAsync(contextGuid, system, "", token, true))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						ReceptionHavaleBusiness receptionHavaleBusiness4 = receptionHavaleBusiness2;
						receptionHavaleBusiness4.SetPrice(system, item.Credit);
						ret.AddToDetList(receptionHavaleBusiness4, system, token);
						break;
					}
					case DocumentDet.DaryaftEnteghal:
					{
						ReceptionHavaleBusiness receptionHavaleBusiness = new ReceptionHavaleBusiness(contextGuid)
						{
							Date = item.DateMoaser,
							Description = "EnteghalHesab//" + item.RowDesc,
							Guid = item.Guid,
							MoeinGuid = item.MoeinGuid,
							DecimalGuid = item.DecimalGuid,
							MasterGuid = item.Guid,
							Number = item.Serial
						};
						ReceptionHavaleBusiness receptionHavaleBusiness2 = receptionHavaleBusiness;
						receptionHavaleBusiness2.TafsilGuid = (await BankBusiness.GetAllAsync(contextGuid, system, "", token, true))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						ReceptionHavaleBusiness receptionHavaleBusiness3 = receptionHavaleBusiness;
						decimal value = item.Credit + item.Debit;
						receptionHavaleBusiness3.SetPrice(system, value);
						ret.AddToDetList(receptionHavaleBusiness3, system, token);
						desc += $"*enteghal//credit-debit:{item.Credit - item.Debit}*";
						break;
					}
					case DocumentDet.DaryaftN:
					{
						ReceptionNaghdBusiness receptionNaghdBusiness = new ReceptionNaghdBusiness(contextGuid)
						{
							DateM = item.DateMoaser,
							Description = item.RowDesc,
							Guid = item.Guid,
							DecimalGuid = item.DecimalGuid,
							MasterGuid = item.Guid
						};
						ReceptionNaghdBusiness receptionNaghdBusiness2 = receptionNaghdBusiness;
						receptionNaghdBusiness2.TafsilGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true, "", token))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						ReceptionNaghdBusiness receptionNaghdBusiness3 = receptionNaghdBusiness;
						receptionNaghdBusiness3.SetPrice(system, item.Credit);
						ret.AddToDetList(receptionNaghdBusiness3, system, token);
						break;
					}
					default:
						WebErrorLog.Instance.StartLog("not defined! \r\n" + Json.ToStringJson(item, indented: true), "", "GetReceptionBusinessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1836);
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(desc))
			{
				ret.Description += desc;
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionBusinessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1844);
			throw;
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				bool flag = userAccBusiness.UserAccess.Reception.SanadStatusAccess.SingleOrDefault((ReceptionSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
				return userAccBusiness.UserAccess.Reception.Reception_Insert && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1862);
				return false;
			}
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					return IsTemp;
				}
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				bool flag = userAccBusiness2.UserAccess.Reception.SanadStatusAccess.SingleOrDefault((ReceptionSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
				return (userAccBusiness2.UserAccess.Reception.Reception_Delete || (userAccBusiness2.UserAccess.Reception.Reception_Delete_Self && UserGuid == cashier.Guid)) && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1880);
				throw;
			}
		}
	}

	public async Task<bool> CanUpdateAsync(EnSystem system = EnSystem.UserInterface, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 2;
			try
			{
				ENSource source = ClsCache.GetCache(ContextGuid).Source;
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				if (!userAccBusiness2.UserAccess.Reception.Reception_Update_NODate && Date.Date != DateTime.Now.Date)
				{
					return false;
				}
				if (source != ENSource.Accounting)
				{
					bool flag = IsTemp;
					if (!flag)
					{
						flag = (((ReceptionBusiness)(await GetOldItemAsync(system, token, tranName)))?.Sum ?? 0m) == 0m;
					}
					return flag;
				}
				bool flag2 = userAccBusiness2.UserAccess.Reception.SanadStatusAccess.SingleOrDefault((ReceptionSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
				return (userAccBusiness2.UserAccess.Reception.Reception_Update || (userAccBusiness2.UserAccess.Reception.Reception_Update_Self && UserGuid == cashier.Guid)) && flag2;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1900);
				throw;
			}
		}
	}

	public async Task<bool> CanPrintAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return userAccBusiness2.UserAccess.Reception.Reception_Print || (userAccBusiness2.UserAccess.Reception.Reception_Print_Self && UserGuid == cashier.Guid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1916);
				throw;
			}
		}
	}

	public async Task<bool> CanReportAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness userAccBusiness2 = userAccBusiness;
				return userAccBusiness2.UserAccess.Reception.Reception_Report || (userAccBusiness2.UserAccess.Reception.Reception_ReportSelf && UserGuid == cashier.Guid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1932);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SetReceptionTypeBySellAsync(string contextGuid, List<Guid> lst1, List<Guid> lst2, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
	{
		using (new JaegerMonitor("SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "RecptionBusiness/SaveAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1946);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1947), ReturnedState.None, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1947);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IReception.SetReceptionTypeBySellAsync(lst1, lst2, token, tranName), ReturnedState.None, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1949);
							if (!ret.HasError)
							{
								goto end_IL_0131;
							}
							result = ret;
						}
						goto end_IL_0095;
						end_IL_0131:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1954);
					ret.AddReturnedValue(ex, "", "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1955);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1960), ReturnedState.None, "SetReceptionTypeBySellAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 1960);
				}
			}
			return ret;
		}
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			int line = 0;
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				line = 1;
				if (_oldReception == null)
				{
					_oldReception = await ClsCache.GetCache(ContextGuid).CacheRepository.IReception.GetFromDbAsync(Guid, _masterOrder, system, token, tranName);
				}
				line = 2;
				int oldSanadNumber = ((ChangeStatus == EntityChangeSt.Edited) ? _oldReception.SanadNo : 0);
				line = 3;
				Guid oldGuid = Guid.NewGuid();
				SanadBusiness sanad = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
				if (sanad != null)
				{
					oldGuid = sanad.Guid;
				}
				SanadBusiness sanadBusiness = new SanadBusiness(ContextGuid, oldSanadNumber);
				SanadBusiness sanadBusiness2 = sanadBusiness;
				sanadBusiness2.Description = await this.GetReceptionDescAsync(token, tranName);
				sanadBusiness.DateM = Date;
				sanadBusiness.UserGuid = UserGuid;
				sanadBusiness.Guid = oldGuid;
				sanadBusiness.SanadType = PacketParser.Entities.SanadType.Automatic;
				sanad = sanadBusiness;
				line = 5;
				IReadOnlyList<ReceptionNaghdBusiness> listNaghd = ListNaghd;
				Sanad_DetBusiness naghdDebit5;
				if (listNaghd != null && listNaghd.Count > 0)
				{
					foreach (ReceptionNaghdBusiness naghd in ListNaghd.ToList())
					{
						Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
						{
							Credit = 0m,
							Debit = naghd.Price
						};
						Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
						sanad_DetBusiness2.RowDesc = await naghd.GetReceptionNaghdDescAsync(this, token, tranName);
						sanad_DetBusiness.DecimalGuid = naghd.DecimalGuid;
						sanad_DetBusiness.Guid = Guid.NewGuid();
						sanad_DetBusiness.Qt = 0m;
						sanad_DetBusiness.MasterGuid = sanad.Guid;
						naghdDebit5 = sanad_DetBusiness;
						await naghdDebit5.SetMoeinGuidAsync(naghd.MoeinGuid, token, tranName);
						line = 10;
						await naghdDebit5.SetTafsilGuidAsync(naghd.TafsilGuid, tranName, token);
						line = 11;
						await sanad.AddToListSanadAsync(naghdDebit5, system, token);
						line = 12;
					}
				}
				IReadOnlyList<ReceptionHavaleBusiness> listBank = ListBank;
				Sanad_DetBusiness sanad_DetBusiness3;
				if (listBank != null && listBank.Count > 0)
				{
					foreach (ReceptionHavaleBusiness bank2 in ListBank.ToList())
					{
						Sanad_DetBusiness sanad_DetBusiness2 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
						{
							Credit = 0m,
							Debit = bank2.Price
						};
						Sanad_DetBusiness sanad_DetBusiness = sanad_DetBusiness2;
						sanad_DetBusiness.RowDesc = await bank2.GetReceptionBankDescAsync(this, token, tranName);
						sanad_DetBusiness2.DecimalGuid = bank2.DecimalGuid;
						sanad_DetBusiness2.Guid = Guid.NewGuid();
						sanad_DetBusiness2.Qt = bank2.Qt;
						sanad_DetBusiness2.MasterGuid = sanad.Guid;
						naghdDebit5 = sanad_DetBusiness2;
						await naghdDebit5.SetMoeinGuidAsync(bank2.MoeinGuid, token, tranName);
						line = 13;
						await naghdDebit5.SetTafsilGuidAsync(bank2.TafsilGuid, tranName, token);
						line = 14;
						await sanad.AddToListSanadAsync(naghdDebit5, system, token);
						line = 15;
						if ((bank2?.WageGuid ?? Guid.Empty) != Guid.Empty && (bank2?.WagePrice ?? 0m) != 0m)
						{
							sanad_DetBusiness3 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
							{
								Credit = 0m,
								Debit = bank2.WagePrice
							};
							sanad_DetBusiness = sanad_DetBusiness3;
							sanad_DetBusiness.RowDesc = await bank2.GetReceptionBankDescAsync(this, token, tranName);
							sanad_DetBusiness3.Guid = Guid.NewGuid();
							sanad_DetBusiness3.Qt = 0m;
							sanad_DetBusiness2 = sanad_DetBusiness3;
							await sanad_DetBusiness2.SetMoeinGuidAsync(bank2.WageMoeinGuid.Value, token, tranName);
							await sanad_DetBusiness2.SetTafsilGuidAsync(bank2.WageTafsilGuid, tranName, token);
							await sanad.AddToListSanadAsync(sanad_DetBusiness2, system, token);
						}
					}
				}
				IReadOnlyList<ReceptionCheckBusiness> listCheck = ListCheck;
				if (listCheck != null && listCheck.Count > 0)
				{
					foreach (ReceptionCheckBusiness check in ListCheck.Where((ReceptionCheckBusiness p) => p.Status != ENUMCheck.BatelShode).ToList())
					{
						if ((check?.DebitTafsilGuid ?? Guid.Empty) == Guid.Empty)
						{
							check.DebitTafsilGuid = setting.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception;
						}
						sanad_DetBusiness3 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
						{
							Credit = 0m,
							Debit = check.Price
						};
						Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness3;
						sanad_DetBusiness2.RowDesc = await check.GetReceptionCheckDescAsync(this, token, tranName);
						sanad_DetBusiness3.Guid = Guid.NewGuid();
						sanad_DetBusiness3.Qt = 0m;
						sanad_DetBusiness3.MasterGuid = sanad.Guid;
						naghdDebit5 = sanad_DetBusiness3;
						await naghdDebit5.SetMoeinGuidAsync(setting.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception, token, tranName);
						line = 16;
						await naghdDebit5.SetTafsilGuidAsync(check.DebitTafsilGuid, tranName, token);
						line = 17;
						await sanad.AddToListSanadAsync(naghdDebit5, system, token);
						line = 18;
					}
				}
				IReadOnlyList<Sanad_DetBusiness> listEnteghal = ListEnteghal;
				if (listEnteghal != null && listEnteghal.Count > 0)
				{
					foreach (Sanad_DetBusiness naghdDebit5 in ListEnteghal.ToList())
					{
						Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
						{
							Credit = naghdDebit5.Credit,
							Debit = naghdDebit5.Debit
						};
						Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
						sanad_DetBusiness2.RowDesc = $"دریافت({await GetId2Async(token, tranName)} {Description} {naghdDebit5.RowDesc})";
						sanad_DetBusiness.DecimalGuid = naghdDebit5.DecimalGuid;
						sanad_DetBusiness.Guid = Guid.NewGuid();
						sanad_DetBusiness.Qt = naghdDebit5.Qt;
						sanad_DetBusiness.MasterGuid = sanad.Guid;
						sanad_DetBusiness3 = sanad_DetBusiness;
						await sanad_DetBusiness3.SetMoeinGuidAsync(naghdDebit5.MoeinGuid, token, tranName);
						line = 19;
						await sanad_DetBusiness3.SetTafsilGuidAsync(naghdDebit5.TafsilGuid, tranName, token);
						if ((TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
						{
							await sanad_DetBusiness3.SetTafsil2GuidAsync(TafsilGuid2, tranName, token);
						}
						if ((TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
						{
							await sanad_DetBusiness3.SetTafsil3GuidAsync(TafsilGuid3, tranName, token);
						}
						await sanad.AddToListSanadAsync(sanad_DetBusiness3, system, token);
						line = 21;
					}
				}
				sanad_DetBusiness3 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
				{
					Credit = Sum,
					Debit = 0m
				};
				naghdDebit5 = sanad_DetBusiness3;
				naghdDebit5.RowDesc = await this.GetReceptionDescAsync(token, tranName);
				sanad_DetBusiness3.DecimalGuid = Guid.Empty;
				sanad_DetBusiness3.Guid = Guid.NewGuid();
				sanad_DetBusiness3.Qt = 0m;
				sanad_DetBusiness3.MasterGuid = sanad.Guid;
				Sanad_DetBusiness creditReception = sanad_DetBusiness3;
				line = 6;
				await creditReception.SetMoeinGuidAsync(MoeinGuid, token, tranName);
				line = 7;
				await creditReception.SetTafsilGuidAsync(TafsilGuid, tranName, token);
				if ((TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
				{
					await creditReception.SetTafsil2GuidAsync(TafsilGuid2, tranName, token);
				}
				if ((TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
				{
					await creditReception.SetTafsil3GuidAsync(TafsilGuid3, tranName, token);
				}
				line = 8;
				await sanad.AddToListSanadAsync(creditReception, system, token);
				IReadOnlyList<ReceptionHavaleBusiness> listBank2 = ListBank;
				if (listBank2 != null && listBank2.Count > 0)
				{
					foreach (ReceptionHavaleBusiness bank2 in ListBank.ToList())
					{
						if ((bank2?.WageGuid ?? Guid.Empty) != Guid.Empty && (bank2?.WagePrice ?? 0m) != 0m)
						{
							Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Reception, sanad.NextOrdinal())
							{
								Credit = bank2.WagePrice,
								Debit = 0m
							};
							naghdDebit5 = sanad_DetBusiness;
							naghdDebit5.RowDesc = await bank2.GetReceptionBankDescAsync(this, token, tranName);
							sanad_DetBusiness.Guid = Guid.NewGuid();
							sanad_DetBusiness.Qt = 0m;
							sanad_DetBusiness3 = sanad_DetBusiness;
							await sanad_DetBusiness3.SetMoeinGuidAsync(bank2.MoeinGuid, token, tranName);
							await sanad_DetBusiness3.SetTafsilGuidAsync(bank2.TafsilGuid, tranName, token);
							await sanad.AddToListSanadAsync(sanad_DetBusiness3, system, token);
						}
					}
				}
				return sanad;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $" The Error Line IS : {line}", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2141);
				throw;
			}
		}
	}

	public Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem syste, CancellationToken token, string traName, bool isDeleteAction)
	{
		return Task.FromResult(_lstProductsAnbarChanges);
	}

	public void SetLstProductsAnbarChanges(List<ProductsChanges> value)
	{
		_lstProductsAnbarChanges = value;
	}

	public async Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return MoeinChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return TafsilChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		_ = 2;
		try
		{
			SanadBusiness newSanad = await GenerateSanadAsync(system, token, tranName);
			ReceptionBusiness receptionBusiness = (ReceptionBusiness)(await GetOldItemAsync(system, token, tranName));
			return CalcChanges.ComputeHesabChange(await SanadBusiness.GetAsync(ContextGuid, receptionBusiness?.SanadNo ?? 0, token, tranName), newSanad, isDeleteAction, autoSanadItemsForSave);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLstHesabChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2163);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveRangeAsync(string contextGuid, EnSystem system, CancellationToken token, Priority priority, List<WebReception> lst, bool sendToServer, short tryCount = 10, string tranName = "")
	{
		_ = 1;
		try
		{
			List<ReceptionBusiness> list = await ReceptionMapper.Instance.MapListAsync(contextGuid, lst, new List<ReceptionBusiness>(), "", token);
			if (list != null && list.Any())
			{
				foreach (ReceptionBusiness item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await item.SaveAsync(system, sendToServer, priority, awaitOnSemaphore: true, "", "", 60000, await4send: false, tryCount, tranName, token);
					if (returnedSaveFuncInfo.HasError)
					{
						return returnedSaveFuncInfo;
					}
				}
			}
			return new ReturnedSaveFuncInfo();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2187);
			return new ReturnedSaveFuncInfo(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2188);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendListAsync(EnSystem system, IEnumerable<ReceptionReportBusiness> receptionList, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (receptionList == null || !receptionList.Any())
			{
				return res;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			foreach (ReceptionReportBusiness item in receptionList?.ToList())
			{
				tasks.Add(item.SendAsync(system, token));
				if (tasks.Count > 20)
				{
					await Task.WhenAny(tasks);
				}
			}
			await Task.WhenAll(tasks);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2209);
			res.AddReturnedValue(ex, "", "SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2210);
		}
		return res;
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.High)
	{
		return Send2ServerAsync("", "", 60000, token, priority);
	}

	public static async Task<ReceptionBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? tafsilGuid = null, string description = "", Guid? guid = null, OrderBusiness order = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			ReceptionBusiness ret = new ReceptionBusiness(contextGuid, order)
			{
				ChangeStatus = EntityChangeSt.New,
				Source = ClsCache.GetCache(contextGuid).Source,
				UserGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				await ret.SetTafsilGuidAsync(tafsilGuid.Value, (system != EnSystem.SmsParser) ? system : EnSystem.None, token, tranName);
			}
			if (!string.IsNullOrEmpty(description))
			{
				ret.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				ret.Guid = guid.Value;
			}
			else
			{
				ret.Guid = Guid.NewGuid();
			}
			if (ret.Source != ENSource.Accounting)
			{
				ret.SanadNo = 0;
				await ClsCache.GetCache(contextGuid).CacheRepository.IReception.Add2CacheAsync(ret, token, "");
			}
			ret.MetaData = new ReceptionMetaData
			{
				System = system
			};
			await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, ret, system, token, tranName);
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2241);
			throw;
		}
	}

	public void RemoveFromDetList(BaseKhazaneDetail item)
	{
		using (new JaegerMonitor("RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				if (item == null)
				{
					return;
				}
				item.OnPriceChange -= Det_OnPriceChange;
				ReceptionNaghdBusiness receptionNaghdBusiness = item as ReceptionNaghdBusiness;
				if (receptionNaghdBusiness == null)
				{
					ReceptionCheckBusiness receptionCheckBusiness = item as ReceptionCheckBusiness;
					if (receptionCheckBusiness == null)
					{
						ReceptionHavaleBusiness receptionHavaleBusiness = item as ReceptionHavaleBusiness;
						if (receptionHavaleBusiness == null)
						{
							Sanad_DetBusiness sanad_DetBusiness = item as Sanad_DetBusiness;
							if (sanad_DetBusiness == null)
							{
								throw new NotImplementedException();
							}
							Sanad_DetBusiness sanad_DetBusiness2 = _listEnteghal?.FirstOrDefault((Sanad_DetBusiness p) => p.Guid == sanad_DetBusiness.Guid);
							if (sanad_DetBusiness2 != null)
							{
								_listEnteghal.Remove(sanad_DetBusiness2);
							}
						}
						else
						{
							ReceptionHavaleBusiness receptionHavaleBusiness2 = _listBank?.FirstOrDefault((ReceptionHavaleBusiness p) => p.Guid == receptionHavaleBusiness.Guid);
							if (receptionHavaleBusiness2 != null)
							{
								_listBank.Remove(receptionHavaleBusiness2);
							}
						}
					}
					else
					{
						ReceptionCheckBusiness receptionCheckBusiness2 = _listCheck?.FirstOrDefault((ReceptionCheckBusiness p) => p.Guid == receptionCheckBusiness.Guid);
						if (receptionCheckBusiness2 != null)
						{
							_listCheck?.Remove(receptionCheckBusiness2);
						}
					}
				}
				else
				{
					ReceptionNaghdBusiness receptionNaghdBusiness2 = _listNaghd?.FirstOrDefault((ReceptionNaghdBusiness p) => p.Guid == receptionNaghdBusiness.Guid);
					if (receptionNaghdBusiness2 != null)
					{
						_listNaghd.Remove(receptionNaghdBusiness2);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2275);
				throw;
			}
		}
	}

	public void ResetItems()
	{
		try
		{
			IReadOnlyList<ReceptionHavaleBusiness> listBank = ListBank;
			if (listBank != null && listBank.Any())
			{
				foreach (ReceptionHavaleBusiness item in ListBank.ToList())
				{
					RemoveFromDetList(item);
				}
			}
			IReadOnlyList<ReceptionCheckBusiness> listCheck = ListCheck;
			if (listCheck != null && listCheck.Any())
			{
				foreach (ReceptionCheckBusiness item2 in ListCheck.ToList())
				{
					RemoveFromDetList(item2);
				}
			}
			IReadOnlyList<ReceptionNaghdBusiness> listNaghd = ListNaghd;
			if (listNaghd != null && listNaghd.Any())
			{
				foreach (ReceptionNaghdBusiness item3 in ListNaghd.ToList())
				{
					RemoveFromDetList(item3);
				}
			}
			IReadOnlyList<Sanad_DetBusiness> listEnteghal = ListEnteghal;
			if (listEnteghal == null || !listEnteghal.Any())
			{
				return;
			}
			foreach (Sanad_DetBusiness item4 in ListEnteghal.ToList())
			{
				RemoveFromDetList(item4);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2301);
			throw;
		}
	}

	public void AddItemsToDetList(IEnumerable<BaseKhazaneDetail> items, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (items == null || !items.Any())
			{
				return;
			}
			foreach (BaseKhazaneDetail item in items)
			{
				AddToDetList(item, system, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddItemsToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2315);
			throw;
		}
	}

	public void AddRangeToDetList<T>(List<T> detList, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			try
			{
				if (detList == null || detList.Count <= 0)
				{
					return;
				}
				foreach (T det in detList)
				{
					if (!(det is ReceptionNaghdBusiness item))
					{
						if (!(det is ReceptionCheckBusiness item2))
						{
							if (!(det is ReceptionHavaleBusiness item3))
							{
								if (det is Sanad_DetBusiness item4)
								{
									AddToDetList(item4, system, token);
								}
							}
							else
							{
								AddToDetList(item3, system, token);
							}
						}
						else
						{
							AddToDetList(item2, system, token);
						}
					}
					else
					{
						AddToDetList(item, system, token);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2338);
				throw;
			}
		}
	}

	public void ListBankClear()
	{
		_listBank?.Clear();
	}

	public void ListNaghdClear()
	{
		_listNaghd?.Clear();
	}

	public void ListCheckClear()
	{
		_listCheck?.Clear();
	}

	public void ListEnteghalClear()
	{
		_listEnteghal?.Clear();
	}

	public static Task<List<Guid>> GetNotTransferedAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetNotTransferedAsync(token, tranName);
	}

	public static Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(string contextGuid, OperationTransfer filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReception.UpdateTransferDateAsync(filter, token, tranName);
	}

	public async Task<string> GetUserNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetCashierAsync(token))?.Name ?? "";
	}

	public async Task<IBaseKhazane> MapAsync(string contextGuid, IBaseKhazane source, IBaseKhazane destination, CancellationToken token)
	{
		return await ReceptionMapper.Instance.MapAsync(contextGuid, (ReceptionBusiness)source, (ReceptionBusiness)destination, "", token);
	}

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			_ = 15;
			try
			{
				if (system == EnSystem.Transfer)
				{
					return null;
				}
				List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
				if (ListNaghd?.Any() ?? false)
				{
					foreach (ReceptionNaghdBusiness item3 in ListNaghd)
					{
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = item3.MoeinGuid,
							DebitTafsilGuid = item3.TafsilGuid,
							Date = item3.DateM,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftN,
							AsTypeDocument = Document.Reception,
							Debit = item3.Price,
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = MoeinGuid;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = TafsilGuid;
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = item3.Description;
						viewSanadDetailCoverBusiness.DateMoaser = item3.DateM;
						viewSanadDetailCoverBusiness.DetailGuid = item3.Guid;
						viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness.DecimalGuid = null;
						viewSanadDetailCoverBusiness.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness.DecimalQt = 0m;
						viewSanadDetailCoverBusiness.DepotGuid = null;
						viewSanadDetailCoverBusiness.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness.Price = 0m;
						viewSanadDetailCoverBusiness.ProductGuid = null;
						viewSanadDetailCoverBusiness.Qt = 0m;
						viewSanadDetailCoverBusiness.Qt2 = 0m;
						viewSanadDetailCoverBusiness.RBazaryab = 0m;
						viewSanadDetailCoverBusiness.RDiscount = 0m;
						viewSanadDetailCoverBusiness.Serial = "";
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = null;
						list2.Add(viewSanadDetailCoverBusiness);
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, item3.TafsilGuid, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = item3.MoeinGuid,
							DebitTafsilGuid = item3.TafsilGuid,
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftN,
							AsTypeDocument = Document.Reception,
							Debit = 0m,
							Credit = item3.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness2.CreditMoeinGuid = MoeinGuid;
						viewSanadDetailCoverBusiness2.CreditTafsilGuid = TafsilGuid;
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = "نقدی به " + tafsil?.Name + " " + item3.Description;
						viewSanadDetailCoverBusiness2.DateMoaser = item3.DateM;
						viewSanadDetailCoverBusiness2.DetailGuid = item3.Guid;
						viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness2.DecimalGuid = null;
						viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness2.DecimalQt = 0m;
						viewSanadDetailCoverBusiness2.DepotGuid = null;
						viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness2.Price = 0m;
						viewSanadDetailCoverBusiness2.ProductGuid = null;
						viewSanadDetailCoverBusiness2.Qt = 0m;
						viewSanadDetailCoverBusiness2.Qt2 = 0m;
						viewSanadDetailCoverBusiness2.RBazaryab = 0m;
						viewSanadDetailCoverBusiness2.RDiscount = 0m;
						viewSanadDetailCoverBusiness2.Serial = "";
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = null;
						viewSanadDetailCoverBusiness2.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness2.TafsilGuid3 = TafsilGuid3;
						list2.Add(viewSanadDetailCoverBusiness2);
					}
				}
				if (ListCheck?.Any() ?? false)
				{
					foreach (ReceptionCheckBusiness item4 in ListCheck?.Where((ReceptionCheckBusiness q) => q.Status != ENUMCheck.BatelShode))
					{
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = item4.DebitMoeinGuid,
							DebitTafsilGuid = item4.DebitTafsilGuid,
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftCh,
							AsTypeDocument = Document.Reception,
							Debit = item4.Price,
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = item4.Description;
						viewSanadDetailCoverBusiness.DateMoaser = item4.Date;
						viewSanadDetailCoverBusiness.DetailGuid = item4.Guid;
						viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness.DecimalGuid = null;
						viewSanadDetailCoverBusiness.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness.DecimalQt = 0m;
						viewSanadDetailCoverBusiness.DepotGuid = null;
						viewSanadDetailCoverBusiness.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness.Price = 0m;
						viewSanadDetailCoverBusiness.ProductGuid = null;
						viewSanadDetailCoverBusiness.Qt = 0m;
						viewSanadDetailCoverBusiness.Qt2 = 0m;
						viewSanadDetailCoverBusiness.RBazaryab = 0m;
						viewSanadDetailCoverBusiness.RDiscount = 0m;
						viewSanadDetailCoverBusiness.Serial = item4.Number;
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = null;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness3 = viewSanadDetailCoverBusiness;
						if (item4.Aval2re)
						{
							viewSanadDetailCoverBusiness3.CreditMoeinGuid = item4.CreditA2reMoeinGuid;
							viewSanadDetailCoverBusiness3.CreditTafsilGuid = item4.CreditA2reTafsilGuid;
						}
						else
						{
							viewSanadDetailCoverBusiness3.CreditMoeinGuid = MoeinGuid;
							viewSanadDetailCoverBusiness3.CreditTafsilGuid = TafsilGuid;
						}
						list.Add(viewSanadDetailCoverBusiness3);
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = item4.DebitMoeinGuid,
							DebitTafsilGuid = item4.DebitTafsilGuid,
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftCh,
							AsTypeDocument = Document.Reception,
							Debit = 0m,
							Credit = item4.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = "چک شماره: " + item4.Number + " بانک: " + item4.BankName + " تاریخ: " + item4.DateSh + " " + item4.Description;
						viewSanadDetailCoverBusiness2.DateMoaser = item4.Date;
						viewSanadDetailCoverBusiness2.DetailGuid = item4.Guid;
						viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness2.DecimalGuid = null;
						viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness2.DecimalQt = 0m;
						viewSanadDetailCoverBusiness2.DepotGuid = null;
						viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness2.Price = 0m;
						viewSanadDetailCoverBusiness2.ProductGuid = null;
						viewSanadDetailCoverBusiness2.Qt = 0m;
						viewSanadDetailCoverBusiness2.Qt2 = 0m;
						viewSanadDetailCoverBusiness2.RBazaryab = 0m;
						viewSanadDetailCoverBusiness2.RDiscount = 0m;
						viewSanadDetailCoverBusiness2.Serial = item4.Number;
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = null;
						viewSanadDetailCoverBusiness2.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness2.TafsilGuid3 = TafsilGuid3;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness4 = viewSanadDetailCoverBusiness2;
						if (item4.Aval2re)
						{
							viewSanadDetailCoverBusiness4.CreditMoeinGuid = item4.CreditA2reMoeinGuid;
							viewSanadDetailCoverBusiness4.CreditTafsilGuid = item4.CreditA2reTafsilGuid;
						}
						else
						{
							viewSanadDetailCoverBusiness4.CreditMoeinGuid = MoeinGuid;
							viewSanadDetailCoverBusiness4.CreditTafsilGuid = TafsilGuid;
						}
						list.Add(viewSanadDetailCoverBusiness4);
					}
				}
				if (ListBank?.Any() ?? false)
				{
					foreach (ReceptionHavaleBusiness item5 in ListBank)
					{
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, item5.TafsilGuid, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = item5.MoeinGuid,
							DebitTafsilGuid = item5.TafsilGuid,
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftH,
							AsTypeDocument = Document.Reception,
							Debit = 0m,
							Credit = item5.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = MoeinGuid;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = TafsilGuid;
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = "حواله بانکی به شماره: " + item5.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item5.DateSh + " " + item5.NumberForDescription + " " + item5.RRNForDescription + " " + item5.STANForDescription + " " + item5.Description;
						viewSanadDetailCoverBusiness.DateMoaser = item5.Date;
						viewSanadDetailCoverBusiness.DetailGuid = item5.Guid;
						viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness.DecimalGuid = null;
						viewSanadDetailCoverBusiness.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness.DecimalQt = 0m;
						viewSanadDetailCoverBusiness.DepotGuid = null;
						viewSanadDetailCoverBusiness.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness.Price = 0m;
						viewSanadDetailCoverBusiness.ProductGuid = null;
						viewSanadDetailCoverBusiness.Qt = 0m;
						viewSanadDetailCoverBusiness.Qt2 = 0m;
						viewSanadDetailCoverBusiness.RBazaryab = 0m;
						viewSanadDetailCoverBusiness.RDiscount = 0m;
						viewSanadDetailCoverBusiness.Serial = item5.Number;
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = null;
						viewSanadDetailCoverBusiness.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness.TafsilGuid3 = TafsilGuid3;
						list2.Add(viewSanadDetailCoverBusiness);
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = item5.MoeinGuid,
							DebitTafsilGuid = item5.TafsilGuid,
							Date = item5.Date,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftN,
							AsTypeDocument = Document.Reception,
							Debit = item5.Price,
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness2.CreditMoeinGuid = MoeinGuid;
						viewSanadDetailCoverBusiness2.CreditTafsilGuid = TafsilGuid;
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = item5.Description + " " + item5.NumberForDescription + " " + item5.RRNForDescription + " " + item5.STANForDescription;
						viewSanadDetailCoverBusiness2.DateMoaser = item5.Date;
						viewSanadDetailCoverBusiness2.DetailGuid = item5.Guid;
						viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness2.DecimalGuid = null;
						viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness2.DecimalQt = 0m;
						viewSanadDetailCoverBusiness2.DepotGuid = null;
						viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness2.Price = 0m;
						viewSanadDetailCoverBusiness2.ProductGuid = null;
						viewSanadDetailCoverBusiness2.Qt = 0m;
						viewSanadDetailCoverBusiness2.Qt2 = 0m;
						viewSanadDetailCoverBusiness2.RBazaryab = 0m;
						viewSanadDetailCoverBusiness2.RDiscount = 0m;
						viewSanadDetailCoverBusiness2.Serial = item5.Number;
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = null;
						list2.Add(viewSanadDetailCoverBusiness2);
						if ((item5?.WageGuid ?? Guid.Empty) != Guid.Empty && item5.WagePrice != 0m)
						{
							list2 = list;
							viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
							{
								CreditMoeinGuid = item5.MoeinGuid,
								CreditTafsilGuid = item5.TafsilGuid,
								Date = DateM,
								AsGuid = Guid,
								AsType = DocumentDet.DaryaftH,
								AsTypeDocument = Document.Reception,
								Debit = item5.WagePrice,
								Credit = 0m,
								CashierGuid = UserGuid,
								SanadNo = SanadNo,
								SanadSt = (SanadSt)SanadSt
							};
							viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
							viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
							viewSanadDetailCoverBusiness.DebitMoeinGuid = item5.WageMoeinGuid.Value;
							viewSanadDetailCoverBusiness.DebitTafsilGuid = item5.WageTafsilGuid;
							viewSanadDetailCoverBusiness.SanadDesc = Description;
							viewSanadDetailCoverBusiness.RDesc = "دریافت کارمزد کارتخوان به شماره: " + item5.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item5.DateSh + " " + item5.Description;
							viewSanadDetailCoverBusiness.DateMoaser = DateM;
							viewSanadDetailCoverBusiness.DetailGuid = item5.Guid;
							viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
							viewSanadDetailCoverBusiness.DecimalGuid = null;
							viewSanadDetailCoverBusiness.DecimalPrice = 0m;
							viewSanadDetailCoverBusiness.DecimalQt = 0m;
							viewSanadDetailCoverBusiness.DepotGuid = null;
							viewSanadDetailCoverBusiness.Modified = DateTime.Now;
							viewSanadDetailCoverBusiness.Price = 0m;
							viewSanadDetailCoverBusiness.ProductGuid = null;
							viewSanadDetailCoverBusiness.Qt = 0m;
							viewSanadDetailCoverBusiness.Qt2 = 0m;
							viewSanadDetailCoverBusiness.RBazaryab = 0m;
							viewSanadDetailCoverBusiness.RDiscount = 0m;
							viewSanadDetailCoverBusiness.Serial = item5.Number;
							viewSanadDetailCoverBusiness.Status = true;
							viewSanadDetailCoverBusiness.VahedGuid = null;
							list2.Add(viewSanadDetailCoverBusiness);
							list2 = list;
							viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
							{
								CreditMoeinGuid = item5.MoeinGuid,
								CreditTafsilGuid = item5.TafsilGuid,
								Date = item5.DateM,
								AsGuid = Guid,
								AsType = DocumentDet.DaryaftH,
								AsTypeDocument = Document.Reception,
								Debit = 0m,
								Credit = item5.WagePrice,
								CashierGuid = UserGuid,
								SanadNo = SanadNo,
								SanadSt = (SanadSt)SanadSt
							};
							viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
							viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
							viewSanadDetailCoverBusiness2.DebitMoeinGuid = item5.WageMoeinGuid.Value;
							viewSanadDetailCoverBusiness2.DebitTafsilGuid = item5.WageTafsilGuid;
							viewSanadDetailCoverBusiness2.SanadDesc = Description;
							viewSanadDetailCoverBusiness2.RDesc = "دریافت کارمزد کارتخوان به شماره: " + item5.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item5.DateSh + " " + item5.Description;
							viewSanadDetailCoverBusiness2.DateMoaser = DateM;
							viewSanadDetailCoverBusiness2.DetailGuid = item5.Guid;
							viewSanadDetailCoverBusiness2.Guid = Guid.NewGuid();
							viewSanadDetailCoverBusiness2.DecimalGuid = null;
							viewSanadDetailCoverBusiness2.DecimalPrice = 0m;
							viewSanadDetailCoverBusiness2.DecimalQt = 0m;
							viewSanadDetailCoverBusiness2.DepotGuid = null;
							viewSanadDetailCoverBusiness2.Modified = DateTime.Now;
							viewSanadDetailCoverBusiness2.Price = 0m;
							viewSanadDetailCoverBusiness2.ProductGuid = null;
							viewSanadDetailCoverBusiness2.Qt = 0m;
							viewSanadDetailCoverBusiness2.Qt2 = 0m;
							viewSanadDetailCoverBusiness2.RBazaryab = 0m;
							viewSanadDetailCoverBusiness2.RDiscount = 0m;
							viewSanadDetailCoverBusiness2.Serial = item5.Number;
							viewSanadDetailCoverBusiness2.Status = true;
							viewSanadDetailCoverBusiness2.VahedGuid = null;
							viewSanadDetailCoverBusiness2.TafsilGuid2 = TafsilGuid2;
							viewSanadDetailCoverBusiness2.TafsilGuid3 = TafsilGuid3;
							list2.Add(viewSanadDetailCoverBusiness2);
						}
					}
				}
				List<Sanad_DetBusiness> lstEnteghatNew = new List<Sanad_DetBusiness>();
				if (ListEnteghal?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item2 in ListEnteghal)
					{
						lstEnteghatNew.Add(item2);
						Sanad_DetBusiness temp = await SanadDetMapper.Instance.MapAsync(ContextGuid, item2, new Sanad_DetBusiness(ContextGuid, system, item2.Guid, Document.Reception, item2.Ordinal), tranName, token);
						temp.Guid = Guid.NewGuid();
						temp.Debit = item2.Credit;
						temp.Credit = item2.Debit;
						await temp.SetMoeinGuidAsync(MoeinGuid, token, tranName);
						await temp.SetTafsilGuidAsync(TafsilGuid, tranName, token);
						if ((TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
						{
							await temp.SetTafsil2GuidAsync(TafsilGuid2, tranName, token);
						}
						if ((TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
						{
							await temp.SetTafsil3GuidAsync(TafsilGuid3, tranName, token);
						}
						lstEnteghatNew.Add(temp);
					}
				}
				if (lstEnteghatNew?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item2 in lstEnteghatNew)
					{
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = ((item2.Debit > 0m) ? new Guid?(item2.MoeinGuid) : null),
							DebitTafsilGuid = ((item2.Debit > 0m) ? item2.TafsilGuid : null),
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.DaryaftEnteghal,
							AsTypeDocument = Document.Reception,
							Debit = item2.Debit,
							Credit = item2.Credit,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = (SanadSt)SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = ((item2.Credit > 0m) ? new Guid?(item2.MoeinGuid) : null);
						viewSanadDetailCoverBusiness.CreditTafsilGuid = ((item2.Credit > 0m) ? item2.TafsilGuid : null);
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = item2.RowDesc;
						viewSanadDetailCoverBusiness.DateMoaser = DateM;
						viewSanadDetailCoverBusiness.DetailGuid = item2.Guid;
						viewSanadDetailCoverBusiness.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness.DecimalGuid = item2.DecimalGuid;
						viewSanadDetailCoverBusiness.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness.DecimalQt = item2.Qt;
						viewSanadDetailCoverBusiness.DepotGuid = null;
						viewSanadDetailCoverBusiness.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness.Price = 0m;
						viewSanadDetailCoverBusiness.ProductGuid = null;
						viewSanadDetailCoverBusiness.Qt = 0m;
						viewSanadDetailCoverBusiness.Qt2 = 0m;
						viewSanadDetailCoverBusiness.RBazaryab = 0m;
						viewSanadDetailCoverBusiness.RDiscount = 0m;
						viewSanadDetailCoverBusiness.Serial = "";
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = null;
						viewSanadDetailCoverBusiness.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness.TafsilGuid3 = TafsilGuid3;
						list2.Add(viewSanadDetailCoverBusiness);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2759);
				throw;
			}
		}
	}

	public static Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(string contextGuid, Guid tafsilGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReception.UpdateCustomerFullNameAsync(tafsilGuid, name, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(string contextGuid, Guid? guid, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Android_HesabYar)
			{
				return ret;
			}
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "RecptionBusiness/UpdateDateMoaserAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2777), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2777);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IReception.UpdateDateMoaserAsync(guid, token, tranName), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2779);
							goto end_IL_0151;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2776);
						goto end_IL_00b5;
						end_IL_0151:;
					}
					goto end_IL_00a4;
					end_IL_00b5:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2783);
					ret.AddReturnedValue(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2784);
					goto end_IL_00a4;
				}
				return result;
				end_IL_00a4:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2789), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2789);
				}
			}
			return ret;
		}
	}

	private Task Det_OnPriceChange(object arg1, EventArgs arg2)
	{
		try
		{
			if (this.OnSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Det_OnPriceChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnPriceChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2800);
		}
		return Task.CompletedTask;
	}

	public static async Task<List<SourceForDisplay>> GetAllExistingSourceAsync(string contextGuid, CancellationToken token, string tranName = "")
	{
		try
		{
			List<ENSource> list = await ClsCache.GetCache(contextGuid).CacheRepository.IReception.GetAllExistingSourceAsync(token, tranName);
			if (list == null || !list.Any())
			{
				return null;
			}
			return list.Select((ENSource q) => new SourceForDisplay(q)).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2813);
			throw;
		}
	}

	public void ComputeOrdinal(EnSystem system, CancellationToken token)
	{
		try
		{
			List<BaseKhazaneDetail> list = DetailList?.OrderBy((BaseKhazaneDetail q) => q.Ordinal)?.ToList();
			if (list == null || !list.Any())
			{
				return;
			}
			int num = 1;
			foreach (BaseKhazaneDetail item in list)
			{
				item.SetOrdinal(system, num, token);
				num = checked(num + 1);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ComputeOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2830);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(string contextGuid, Guid guid, int statusId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "ReceptionBusiness/ChangeStatusIdAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2843), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2843);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IReception.ChangeStatusIdAsync(guid, statusId, token, tranName), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2845);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2842);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2849);
					res.AddReturnedValue(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2850);
					goto end_IL_0084;
				}
				return result;
				end_IL_0084:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2855), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionBusiness.cs", 2855);
				}
			}
			return res;
		}
	}
}
