using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
using PacketParser;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Sanad;
using PacketParser.Filters;
using PacketParser.Filters.Khazane;
using PacketParser.Print;
using PacketParser.Records;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business;

public class PardakhtBusiness : BaseSanadBusiness<PardakhtBusiness>, IPardakht, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid, ISyncFromClient2Server<PardakhtBusiness>, IDeliverableEntity, IEditableDelivery, IEntityCache, IDisposable, IChangeIntegrity, IBaseKhazane, IHasContext, IHasSanadStatusReport
{
	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private PardakhtBusiness _oldPardakht;

	private Guid _tafsilGuid;

	private bool _isTemp;

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private static CancellationTokenSource _sendEntitiesToServerToken;

	private int _id2;

	private DateTime _date = DateManagment.DateM;

	private bool _changeDate;

	private List<PardakhtBankBusiness> _bank;

	private List<PardakhtCheckMBusiness> _checkM;

	private List<PardakhtCheckShBusiness> _checkSh;

	private List<PardakhtNaghdBusiness> _naghd;

	private List<Sanad_DetBusiness> _enteghal;

	private TableRelationShipBusiness _linkedSanad;

	private bool? _oldPardakhtGet;

	private decimal? _beforeAccount;

	private decimal _afterAccount;

	private OrderBusiness _masterOrder;

	private string _androidIme;

	private string _cpuSerial;

	private string _description;

	private string _hddSerial;

	private string _ip;

	private string _lkSerial;

	private string _hostRefrenced;

	private string _deliveryMessage = "";

	private string _deliveryShortMessage = "";

	private Guid _guid;

	public override Document SanadType => Document.Pardakht;

	public Document AsType => SanadType;

	public override IDatabaseManager DatabaseManager => ClsCache.GetCache(ContextGuid).AccDatabaseManager;

	public override string ContextGuid { get; set; }

	public EnKhazaneMaster Type { get; set; } = EnKhazaneMaster.Pardakht;


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

	public int PossCount => 0;

	public decimal SumPoss => 0m;

	public DateTime? DateUtc { get; set; }

	public DateTime? DateMoaser { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

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
				WebErrorLog.Instance.StartLog(ex2, "", "IsTemp", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 121);
			}
			return false;
		}
		set
		{
			_isTemp = value;
		}
	}

	public DateTime? TransferDate { get; set; }

	public short StatusId { get; set; }

	public IReadOnlyList<PardakhtBankBusiness> Bank
	{
		get
		{
			if (_bank == null)
			{
				_bank = new List<PardakhtBankBusiness>();
			}
			_bank.RemoveAll((PardakhtBankBusiness p) => p?.IsDisposed ?? true);
			return _bank?.ToList() ?? new List<PardakhtBankBusiness>();
		}
	}

	public IReadOnlyList<PardakhtCheckMBusiness> CheckM
	{
		get
		{
			if (_checkM == null)
			{
				_checkM = new List<PardakhtCheckMBusiness>();
			}
			_checkM.RemoveAll((PardakhtCheckMBusiness p) => p?.IsDisposed ?? true);
			return _checkM?.ToList() ?? new List<PardakhtCheckMBusiness>();
		}
	}

	public IReadOnlyList<PardakhtCheckShBusiness> CheckSh
	{
		get
		{
			if (_checkSh == null)
			{
				_checkSh = new List<PardakhtCheckShBusiness>();
			}
			_checkSh.RemoveAll((PardakhtCheckShBusiness p) => p?.IsDisposed ?? true);
			return _checkSh?.ToList() ?? new List<PardakhtCheckShBusiness>();
		}
	}

	public IReadOnlyList<PardakhtNaghdBusiness> Naghd
	{
		get
		{
			if (_naghd == null)
			{
				_naghd = new List<PardakhtNaghdBusiness>();
			}
			_naghd.RemoveAll((PardakhtNaghdBusiness p) => p?.IsDisposed ?? true);
			return _naghd?.ToList() ?? new List<PardakhtNaghdBusiness>();
		}
	}

	public IReadOnlyList<Sanad_DetBusiness> Enteghal
	{
		get
		{
			if (_enteghal == null)
			{
				_enteghal = new List<Sanad_DetBusiness>();
			}
			_enteghal.RemoveAll((Sanad_DetBusiness p) => p?.IsDisposed ?? true);
			return _enteghal?.ToList() ?? new List<Sanad_DetBusiness>();
		}
	}

	public IReadOnlyCollection<BaseKhazaneDetail> DetailList
	{
		get
		{
			try
			{
				List<BaseKhazaneDetail> list = new List<BaseKhazaneDetail>();
				IReadOnlyList<PardakhtNaghdBusiness> naghd = Naghd;
				if (naghd != null && naghd.Any())
				{
					list.AddRange(Naghd);
				}
				IReadOnlyList<PardakhtBankBusiness> bank = Bank;
				if (bank != null && bank.Any())
				{
					list.AddRange(Bank);
				}
				IReadOnlyList<PardakhtCheckMBusiness> checkM = CheckM;
				if (checkM != null && checkM.Any())
				{
					list.AddRange(CheckM);
				}
				IReadOnlyList<PardakhtCheckShBusiness> checkSh = CheckSh;
				if (checkSh != null && checkSh.Any())
				{
					list.AddRange(CheckSh);
				}
				IReadOnlyList<Sanad_DetBusiness> enteghal = Enteghal;
				if (enteghal != null && enteghal.Any())
				{
					list.AddRange(Enteghal);
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "DetailList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 272);
				throw;
			}
		}
	}

	public decimal SumCheckSh => (CheckSh?.Where((PardakhtCheckShBusiness p) => p.Status != ENCheckSHStatus.BatelShode)?.Sum((PardakhtCheckShBusiness p) => p.Price)).GetValueOrDefault();

	public int CheckMCount => CheckM?.Count ?? 0;

	public int CheckShCount => CheckSh?.Count ?? 0;

	public int EnteghalCount => Enteghal?.Count ?? 0;

	public int BankCount => Bank?.Count ?? 0;

	public int NaghdCount => Naghd?.Count ?? 0;

	public decimal SumCheckM => CheckM?.Sum((PardakhtCheckMBusiness x) => x.Price) ?? 0m;

	public decimal SumEnteghal
	{
		get
		{
			if (Enteghal == null || !Enteghal.Any())
			{
				return 0m;
			}
			decimal result = default(decimal);
			foreach (Sanad_DetBusiness item in Enteghal)
			{
				if (item != null)
				{
					result += item.Credit - item.Debit;
				}
			}
			return result;
		}
	}

	public decimal SumBank => Bank?.Sum((PardakhtBankBusiness x) => x.Price) ?? 0m;

	public decimal SumBankHavale => SumBank;

	public decimal SumNaghd => Naghd?.Sum((PardakhtNaghdBusiness x) => x.Price) ?? 0m;

	public decimal Sum => SumCheckSh + SumBank + SumNaghd + SumCheckM + SumEnteghal;

	public Guid MoeinGuid { get; set; }

	public string AndroidIme
	{
		get
		{
			return _androidIme;
		}
		set
		{
			_androidIme = value?.FixString() ?? "";
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
			_cpuSerial = value?.FixString() ?? "";
		}
	}

	public DateTime Date
	{
		get
		{
			if (!_changeDate)
			{
				return DateManagment.DateM;
			}
			return _date;
		}
		set
		{
			_changeDate = true;
			_date = value;
		}
	}

	public string Time => $"{Date.Hour}:{Date.Minute}";

	public DateTime DateSabt { get; set; } = DateTime.Now;


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

	public Guid TafsilGuid => _tafsilGuid;

	public Guid UserGuid { get; set; }

	public Guid CashierGuid => UserGuid;

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

	public bool IsRead { get; set; }

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

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

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
				ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.Add2CacheAsync(this, default(CancellationToken), "");
			}
			return _guid;
		}
		set
		{
			_guid = value;
		}
	}

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public override EntityChangeSt ChangeStatus { get; set; } = EntityChangeSt.New;


	public int ItemsCount
	{
		get
		{
			int num = 0;
			checked
			{
				if (Bank != null)
				{
					num += Bank.Count;
				}
				if (Naghd != null)
				{
					num += Naghd.Count;
				}
				if (CheckM != null)
				{
					num += CheckM.Count;
				}
				if (CheckSh != null)
				{
					num += CheckSh.Count;
				}
				if (Enteghal != null)
				{
					num += Enteghal.Count;
				}
				return num;
			}
		}
	}

	public ServerStatusForClient ServerStatus { get; set; }

	public bool? DeliveryStatus { get; set; }

	public ENSource Source { get; set; }

	public PardakhtMetaData MetaData { get; set; }

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public event Func<object, EventArgs, Task> OnSumChanged;

	public async Task<decimal> GetAfterAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
			{
				return _afterAccount;
			}
			_afterAccount = await GetBeforeAccountAsync(system, token, tranName) - Sum + (_masterOrder?.TotalSum ?? 0m);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAfterAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 87);
		}
		return _afterAccount;
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
			_beforeAccount = await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.GetBeforeAccountAsync(TafsilGuid, Guid, Document.Pardakht, token, tranName);
			if (_oldPardakht == null)
			{
				_oldPardakht = (PardakhtBusiness)(await GetOldItemAsync(system, token, tranName));
			}
			if (_oldPardakht != null && _oldPardakht.Date < Date)
			{
				_beforeAccount += (decimal?)_oldPardakht.Sum;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 110);
		}
		return _beforeAccount.GetValueOrDefault();
	}

	public void SetBeforeAccount(decimal value)
	{
		_beforeAccount = value;
	}

	public void SetAfterAccount(decimal value)
	{
		_afterAccount = value;
	}

	public override bool GetIsTemp()
	{
		if (!_isTemp && ServerStatus != ServerStatusForClient.DeliveryError)
		{
			return ServerStatus == ServerStatusForClient.SendError;
		}
		return true;
	}

	public async Task<CashierBusiness> GetCashierAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			if (ChangeStatus == EntityChangeSt.Edited || Sum > 0m)
			{
				return await CashierBusiness.GetAsync(ContextGuid, UserGuid, token, tranName);
			}
			return await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCashierAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 139);
			return null;
		}
	}

	public async Task<string> GetUserNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetCashierAsync(token, tranName))?.Name ?? "";
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
					_linkedSanad.SanadType = Document.Pardakht;
				}
				if (_linkedSanad.SanadGuid != Guid)
				{
					_linkedSanad.SanadGuid = Guid;
				}
				return _linkedSanad;
			}
			IEnumerable<TableRelationShipBusiness> relatedSanad = await TableRelationShipBusiness.GetAllRelatedSanadAsync(ContextGuid, Guid, Document.Pardakht, token, tranName);
			IEnumerable<TableRelationShipBusiness> enumerable = await TableRelationShipBusiness.GetAllInRelationSanadAsync(ContextGuid, Guid, Document.Pardakht, token);
			if (enumerable != null)
			{
				relatedSanad = relatedSanad?.Union(enumerable);
			}
			await AddListToLinkedSanadsAsync(relatedSanad);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLinkedSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 159);
		}
		return _linkedSanad;
	}

	public async Task AddListToLinkedSanadsAsync(IEnumerable<TableRelationShipBusiness> lstDets, bool isClear = false)
	{
		using (new JaegerMonitor("AddListToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "AddListToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 171);
			}
		}
	}

	public async Task AddToLinkedSanadsAsync(Guid relatedSanadGuid, Document relatedSanadType, decimal price, bool isClear = false)
	{
		using (new JaegerMonitor("AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
				WebErrorLog.Instance.StartLog(ex2, "", "AddToLinkedSanadsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 195);
			}
		}
	}

	public void RemoveFromLinkedSanad(Guid sanadGuid, Document sanadType)
	{
		using (new JaegerMonitor("RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromLinkedSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 206);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		using (new JaegerMonitor("SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 289), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 289);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 291);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 295);
					ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 296);
				}
			}
			finally
			{
				if (autoTran && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 301), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 301);
				}
			}
			return ret;
		}
	}

	public override IEditableDelivery GetRepository()
	{
		return ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht;
	}

	public async Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		return await ResendNotSentsAsync(contextGuid, token, tranName, maxParalelTask);
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxParalelTask = 20)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			int line = 0;
			ISendData2Server uiUpdater = ClsCache.GetCache(contextGuid)?.SendData2ServerInstance?.Pardakht;
			if (ClsCache.GetCache(contextGuid).Source == ENSource.Accounting)
			{
				return ret;
			}
			try
			{
				if (uiUpdater != null)
				{
					uiUpdater.ShortMessage = "در حال ارسال پرداخت ها";
					uiUpdater.Status = SyncStatus.Syncing;
					uiUpdater.FinalStatus = ret;
					uiUpdater.Group = SyncPart.Operations;
				}
				line = 1;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 335);
				}
				line = 2;
				if (ClsCache.GetCache(contextGuid).CacheRepository.IPardakht == null)
				{
					line = 3;
					_ = ClsCache.GetCache(contextGuid).CacheRepository.IPardakht;
					line = 4;
					throw new NullReferenceException("pardakhtRepo");
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 346);
				}
				line = 6;
				List<PardakhtBusiness> list = (await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.GetAllNotSentAsync(token, tranName)) ?? new List<PardakhtBusiness>();
				line = 7;
				if (uiUpdater != null)
				{
					uiUpdater.TotalCount = list?.Count ?? 0;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				line = 8;
				int current = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (PardakhtBusiness item in list)
					{
						current = checked(current + 1);
						line = 10;
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 360);
						}
						line = 11;
						tasks.Add(Task.Run(() => item.Send2ServerAsync(token, EnSystem.SignalR), token));
						if (uiUpdater != null)
						{
							uiUpdater.Count = current;
						}
						line = 12;
						if (tasks.Count >= maxParalelTask)
						{
							line = 13;
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							line = 14;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 370);
							line = 15;
							tasks.Remove(t);
							line = 16;
						}
						line = 17;
					}
				}
				line = 18;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 380);
				line = 19;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 382);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 384);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 387);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 388);
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

	public int NextOrdinal(CancellationToken token = default(CancellationToken))
	{
		List<Sanad_DetBusiness> enteghal = _enteghal;
		if (enteghal == null || !enteghal.Any())
		{
			return 0;
		}
		return checked(_enteghal.Max((Sanad_DetBusiness p) => p.Ordinal) + 1);
	}

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "", bool useCache = true)
	{
		return await TafsilBusiness.GetAsync(ContextGuid, system, TafsilGuid, tranName, token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
	}

	public async Task<int> GetId2Async(CancellationToken token = default(CancellationToken), string tranName = "")
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
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetId2Async", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 446);
		}
		return _id2;
	}

	public void SetId2(int value, CancellationToken token = default(CancellationToken))
	{
		_id2 = value;
	}

	public async Task<string> GetFullNameAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return (await GetTafsilAsync(system, token, tranName))?.Name ?? "";
	}

	public override DateTime GetDate()
	{
		return Date;
	}

	public async Task SetTafsilGuidAsync(Guid tafsilGuid, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			_beforeAccount = null;
			_tafsilGuid = tafsilGuid;
			TafsilBusiness tafsilBusiness = await GetTafsilAsync(system, token, tranName);
			TafsilGroupBusiness tafsilGroupBusiness = null;
			if (tafsilBusiness != null)
			{
				tafsilGroupBusiness = await tafsilBusiness.GetTafsilGroupAsync(system, token, tranName);
			}
			if (MoeinGuid == Guid.Empty)
			{
				MoeinGuid = tafsilGroupBusiness?.MoeinPardakhtGuid ?? Guid.Empty;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetTafsilGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 488);
		}
	}

	public override Task<int> GetSanadNumberAsync(CancellationToken token, string tranName)
	{
		return Task.FromResult(SanadNo);
	}

	public Task<WebPardakht> ToWebItemAsync(CancellationToken token = default(CancellationToken))
	{
		return PardakhtMapper.Instance.MapAsync(ContextGuid, this, new WebPardakht(), "", token);
	}

	public List<object> AllItems()
	{
		using (new JaegerMonitor("AllItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				List<object> list = new List<object>();
				if (Naghd != null)
				{
					foreach (PardakhtNaghdBusiness item in Naghd)
					{
						list.Add(item);
					}
				}
				if (Bank != null)
				{
					foreach (PardakhtBankBusiness item2 in Bank)
					{
						list.Add(item2);
					}
				}
				if (CheckM != null)
				{
					foreach (PardakhtCheckMBusiness item3 in CheckM)
					{
						list.Add(item3);
					}
				}
				if (CheckSh != null)
				{
					foreach (PardakhtCheckShBusiness item4 in CheckSh)
					{
						list.Add(item4);
					}
				}
				if (Enteghal != null)
				{
					foreach (Sanad_DetBusiness item5 in Enteghal)
					{
						list.Add(item5);
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AllItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 557);
				return null;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority = Priority.Low)
	{
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (ServerStatus == ServerStatusForClient.LinkToOrder)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource(10000);
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "PardakhtBusiness/Send2ServerAsync", 60000, token, token2);
				PardakhtBusiness pardakhtBusiness = await GetAsync(ContextGuid, Guid, fromDatabase: true, token);
				if (pardakhtBusiness == null)
				{
					throw new NullReferenceException($"pardakht with Guid({Guid}) does not exists on database");
				}
				if (pardakhtBusiness.ServerStatus == ServerStatusForClient.Delivered || pardakhtBusiness.ServerStatus == ServerStatusForClient.DirectDelivery)
				{
					return ret.AddInformation("current status on database is delivered!", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 597);
				}
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 598);
				}
				List<WebPardakht> list = new List<WebPardakht>();
				List<WebPardakht> list2 = list;
				list2.Add(await PardakhtMapper.Instance.MapAsync(ContextGuid, this, new WebPardakht(), "", token));
				List<Packet> paks = WebPardakht.GenerateSaveRequest(list);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 600);
				}
				using (new PardakhtWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token))
				{
					Task.Run(() => ClsCache.GetCache(ContextGuid).SendData(paks, priority), token);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 605);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 608);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 609);
			}
			return ret;
		}
	}

	[Obsolete("Just for transfer data from accounting version 5")]
	public Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, int accV5SanadNumber, CancellationToken token = default(CancellationToken))
	{
		_oldPardakht = new PardakhtBusiness(ContextGuid)
		{
			SanadNo = accV5SanadNumber
		};
		return SaveAsync(system, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10);
	}

	public static async Task<PardakhtBusiness> GetAsync(string contextGuid, int id2, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			KhazaneFilter filter = new KhazaneFilter
			{
				Id2From = id2,
				Id2To = id2
			};
			IEnumerable<PardakhtReportBusiness> enumerable = await GetAllAsync(contextGuid, system, filter, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			PardakhtReportBusiness pardakhtReportBusiness = enumerable.SingleOrDefault();
			return await GetAsync(contextGuid, pardakhtReportBusiness.Guid, fromDatabase: true, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 637);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "", bool await4Sent = false, SanadBusiness orderSanad = null, IMessageSender messageSender = null)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = Guid.NewGuid().ToString();
			}
			ENAction action = ENAction.Save;
			TaskCompletionSource<bool> saveFinished = new TaskCompletionSource<bool>();
			_sendEntitiesToServerToken?.Cancel();
			_sendEntitiesToServerToken = new CancellationTokenSource();
			CancellationToken sendEntitiesToServerToken = CancellationTokenSource.CreateLinkedTokenSource(token, _sendEntitiesToServerToken.Token).Token;
			try
			{
				_ = 21;
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
						sendToServer = sendToServer && ClsCache.GetCache(ContextGuid).BufferEnable;
						using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PardakhtBusiness/SaveAsync", 60000, token))
						{
							if (dis.Token.IsCancellationRequested)
							{
								result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 666);
							}
							else
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo;
								if (autoCommit)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 667), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 667);
								}
								transactionHasBeenCreated = true;
								if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && IsTemp)
								{
									throw new NotImplementedException("IsTemp is NotImplemented in Accounting");
								}
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
								returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, tranName, isDeleteAction: false, relatedOrder, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 687);
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
									{
										sendToServer = false;
									}
									Modified = DateTime.Now;
									DeliveryDate = DateTime.Now;
									if (system == EnSystem.UserInterface && ChangeStatus == EntityChangeSt.New)
									{
										UserGuid = (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)).Guid;
									}
									if (ret.HasError)
									{
										result = ret;
									}
									else
									{
										if (MetaData == null)
										{
											MetaData = new PardakhtMetaData();
										}
										if (MetaData.System == EnSystem.None)
										{
											MetaData.System = system;
										}
										if (CheckSh?.Any() ?? false)
										{
											foreach (PardakhtCheckShBusiness item in CheckSh)
											{
												item.PardakhtDate = DateM;
											}
										}
										List<BaseKhazaneDetail> list = DetailList.OrderBy((BaseKhazaneDetail p) => p.Ordinal).ToList();
										for (int i = 0; i < list.Count(); i++)
										{
											if (list[i].Ordinal != i + 1)
											{
												list[i].SetOrdinal(EnSystem.None, i + 1);
											}
										}
										if (system != 0 && system != EnSystem.Transfer)
										{
											await SanadStatusReportBusiness.ApplySanadStatusIdAsync(ContextGuid, this, (system == EnSystem.UserInterface) ? system : MetaData.System, token, tranName);
										}
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.SaveAsync(this, system, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 707);
										if (ret.HasError)
										{
											result = ret;
										}
										else
										{
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await UpdateCheckStatusAsync(system, token, tranName, isEbtal: false), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 709);
											if (ret.HasError)
											{
												result = ret;
											}
											else
											{
												if (CheckSh?.Any() ?? false)
												{
													foreach (PardakhtCheckShBusiness item2 in CheckSh)
													{
														ChequeBookBusiness chequeBookBusiness = await ChequeBookBusiness.GetAsync(ContextGuid, item2.Number, item2.BankTafsilGuid, token, tranName);
														if (chequeBookBusiness != null)
														{
															chequeBookBusiness.IsSaderShode = true;
															returnedSaveFuncInfo = ret;
															returnedSaveFuncInfo.AddReturnedValue(await chequeBookBusiness.SaveAsync(token, awaitOnSemaphore, tryCount, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 718);
														}
													}
												}
												if (ret.HasError)
												{
													result = ret;
												}
												else
												{
													if (IsTemp)
													{
														goto IL_118c;
													}
													SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
													if (orderSanad != null)
													{
														returnedSaveFuncInfo = ret;
														returnedSaveFuncInfo.AddReturnedValue(await orderSanad.JoinAsync(sanad, system, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 739);
														goto IL_118c;
													}
													if (system != EnSystem.Transfer && system != 0)
													{
														returnedSaveFuncInfo = ret;
														returnedSaveFuncInfo.AddReturnedValue(await sanad.CheckValidationAsync(system, true, tranName, false, token, new Tuple<Document, Guid>(Document.Pardakht, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 728);
													}
													if (SanadNo <= 0 && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
													{
														SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
														returnedSaveFuncInfo = ret;
														returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.SetSanadNoAsync(Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 732);
													}
													sanad.SanadNo = SanadNo;
													List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, token, tranName);
													returnedSaveFuncInfo = ret;
													returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.Pardakht, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 736);
													if (!ret.HasError)
													{
														goto IL_118c;
													}
													result = ret;
												}
											}
										}
									}
								}
							}
							goto end_IL_0180;
							IL_118c:
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								if (Source == ENSource.Android_HesabYar || system == EnSystem.Transfer || system == EnSystem.None)
								{
									goto end_IL_02a8;
								}
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await SaveFileAsync(token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 745);
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await UpdateDateMoaserAsync(ContextGuid, Guid, awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 747);
									if (ret.HasError)
									{
										result = ret;
									}
									else
									{
										returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await AghsatShBusiness.UpdatePardakhtInfoAsync(ContextGuid, Guid, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 749);
										if (!ret.HasError)
										{
											goto end_IL_02a8;
										}
										result = ret;
									}
								}
							}
							goto end_IL_0180;
							end_IL_02a8:;
						}
						goto end_IL_016a;
						end_IL_0180:;
					}
					catch (OperationCanceledException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 756);
						ret.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 757);
						goto end_IL_016a;
					}
					return result;
				}
				end_IL_016a:;
			}
			finally
			{
				TaskCompletionSource<ReturnedSaveFuncInfo> transactionResult = await ClsCache.ObserveTransactionDistinity(ContextGuid, tranName);
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 763), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 763);
				}
				if (!ret.HasError)
				{
					SaveTransactionDistinity(transactionResult, system, sendToServer, action, await4Sent, token, priority, messageSender, autoCommit);
					if (ClsCache.GetCache(ContextGuid).BufferEnable && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer && system != 0)
					{
						Task.Run(() => SendEntitiesToServerAsync(sendEntitiesToServerToken));
					}
				}
				if (ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					Task.Run(() => SanadBusiness.RemoveDuplicateSanadNoAsync(ContextGuid, token));
				}
				if (system != EnSystem.Transfer && system != 0)
				{
					PardakhtWebServiceRepository pardakhtWebServiceRepository = ClsCache.GetCache(ContextGuid)?.WebService?.Pardakht;
					if (pardakhtWebServiceRepository != null)
					{
						new Thread(pardakhtWebServiceRepository.ForceToSendDataToServer).Start();
					}
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

	private async Task SaveLogAsync(EnSystem system, ReturnedSaveFuncInfo ret)
	{
		_ = 2;
		try
		{
			if (ret.HasError && system != EnSystem.UserInterface && system != EnSystem.Transfer)
			{
				WebPardakht webEntity = await ToWebItemAsync();
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
					SanadType = EnLog.Pardakht,
					SanadGuid = Guid
				};
				CashiersLogBusiness cashiersLogBusiness2 = cashiersLogBusiness;
				string translate = "خطا در ذخیره پرداخت".GetTranslate();
				string text = $"{webEntity.Id2}";
				string arg = await GetFullNameAsync(system);
				cashiersLogBusiness2.Title = translate + " (" + text + ")" + $"{arg}-{Guid}\r\n" + ret.ErrorMessage + "\r\n" + Json.ToStringJson(webEntity, indented: true);
				cashiersLogBusiness.SaveAsync(system, sendToServer: true, awaitOnSemaphore: false, default(CancellationToken), 10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 815);
		}
	}

	private async Task<ReturnedSaveFuncInfo> SaveFileAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CheckSh?.Any() ?? false)
			{
				foreach (PardakhtCheckShBusiness item in CheckSh)
				{
					if ((item != null && item.AddedFiles.Any()) || (item.RemovedFiles?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, item.AddedFiles, item.RemovedFiles, token), ReturnedState.None, "SaveFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 827);
					}
				}
			}
			if (Bank?.Any() ?? false)
			{
				foreach (PardakhtBankBusiness item2 in Bank)
				{
					if ((item2 != null && item2.AddedFiles.Any()) || (item2.RemovedFiles?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, item2.AddedFiles, item2.RemovedFiles, token), ReturnedState.None, "SaveFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 834);
					}
				}
			}
			if (Enteghal?.Any() ?? false)
			{
				foreach (Sanad_DetBusiness item3 in Enteghal)
				{
					if ((item3 != null && item3.AddedFiles.Any()) || (item3.RemovedFiles?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await BaseFileAttachmentBusiness.SaveAsync(ContextGuid, item3.AddedFiles, item3.RemovedFiles, token), ReturnedState.None, "SaveFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 841);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 846);
			res.AddReturnedValue(ex, "", "SaveFileAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 847);
		}
		return res;
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token)
	{
		using (new JaegerMonitor("SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
						await Task.WhenAll<ReturnedSaveFuncInfo>(KolBusiness.ResendNotSents(ContextGuid, token, "", 5), MoeinBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), TafsilBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), AccountBusiness.ResendNotSentsAsync(ContextGuid, EnSystem.None, token, "", 10));
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 871);
			}
		}
	}

	private async Task SaveTransactionDistinity(TaskCompletionSource<ReturnedSaveFuncInfo> transactionResult, EnSystem system, bool sendToServer, ENAction action, bool await4Sent, CancellationToken token, Priority priority, IMessageSender messageSender, bool autoCommit)
	{
		try
		{
			ReturnedSaveFuncInfo ret = await transactionResult.Task;
			if (ret.HasError)
			{
				return;
			}
			ChangeStatus = EntityChangeSt.Edited;
			ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.ClearCachedItemAsync(Guid, token, "");
			CashiersLogBusiness.Logger(ContextGuid, system, sendToServer, EnLog.Pardakht, action, this);
			if (!ret.HasError && sendToServer && !IsTemp)
			{
				if (await4Sent && ClsCache.GetCache(ContextGuid).BufferLoginState.Status == EnLoginResult.Success && ServerStatus != ServerStatusForClient.LinkToOrder)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Send2ServerAsync(token, system, priority), ReturnedState.None, "SaveTransactionDistinity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 887);
				}
				else
				{
					Task.Run(() => Send2ServerAsync(token, system, priority));
				}
			}
			if (!ret.HasError && messageSender != null && autoCommit)
			{
				await SendMessageAsync(system, messageSender);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveTransactionDistinity", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 893);
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "FillSmsParametersAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 914);
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
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(ContextGuid, system, transferItem.TafsilGuid.Value, "", token, "FillSmsParametersTorTransferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "FillSmsParametersTorTransferAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 937);
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
			if (settings.Payamak.PardakhtAccountStatus != PayamakSendStatus.Disable)
			{
				string message2 = settings.Payamak.PardakhtMessage;
				message2 = await FillSmsParametersAsync(system, message2);
				if (!string.IsNullOrEmpty(message2))
				{
					string mobiles2 = string.Join(",", (await TafsilNumberBusiness.GetAsync(ContextGuid, _tafsilGuid))?.Where((TafsilNumberBusiness p) => p.Mobile)?.Select((TafsilNumberBusiness p) => p.NumberForDisplay()));
					if (mobiles2?.Any() ?? false)
					{
						CancellationToken token2 = new CancellationTokenSource().Token;
						Task.Run(() => messageSender.SendSmsAsync(settings.Payamak.PardakhtAccountStatus, settings.Payamak.Account_ID, message2, mobiles2, token2));
					}
				}
			}
			if (settings.Payamak.PardakhtEnteghalStatus == PayamakSendStatus.Disable || !(Enteghal?.Any() ?? false))
			{
				return;
			}
			string message = settings.Payamak.PardakhtEnteghalMessage;
			foreach (Sanad_DetBusiness item in Enteghal)
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
					Task.Run(() => messageSender.SendSmsAsync(settings.Payamak.PardakhtEnteghalStatus, settings.Payamak.Account_ID, message, mobiles, token));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 982);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateDateMoaserAsync(string contextGuid, Guid? guid, bool awaitOnSemaphore = true, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PardakhtBusiness/UpdateDateMoaserAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 998), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 998);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.UpdateDateMoaserAsync(guid, token, tranName), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1000);
							goto end_IL_0151;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 997);
						goto end_IL_00b5;
						end_IL_0151:;
					}
					goto end_IL_00a4;
					end_IL_00b5:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1004);
					ret.AddReturnedValue(ex, "", "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1005);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1010), ReturnedState.None, "UpdateDateMoaserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1010);
				}
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> UpdateCheckStatusAsync(EnSystem system, CancellationToken token, string tranName, bool isEbtal)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ENSource source = ClsCache.GetCache(ContextGuid).Source;
			IEnumerable<Tuple<Guid, int>> enumerable = CheckM?.GroupBy((PardakhtCheckMBusiness p) => p.GuidCheck)?.Select((IGrouping<Guid, PardakhtCheckMBusiness> p) => new Tuple<Guid, int>(p.Key, p.Count()))?.Where((Tuple<Guid, int> p) => p.Item2 > 1);
			if (enumerable != null && enumerable.Any())
			{
				throw new Exception("اسناد دریافتنی ذیل بیش از یکبار در برگه پرداخت درج شده اند".GetTranslate() + "\r\n" + string.Join(",", enumerable.Select((Tuple<Guid, int> p) => p.Item1)));
			}
			if (!isEbtal)
			{
				foreach (PardakhtCheckMBusiness item2 in _oldPardakht?.CheckM ?? new List<PardakhtCheckMBusiness>())
				{
					if (CheckM?.Any((PardakhtCheckMBusiness p) => p.GuidCheck == item2.GuidCheck) ?? false)
					{
						continue;
					}
					ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(ContextGuid, item2.GuidCheck, token, tranName);
					receptionCheckBusiness.Status = ENUMCheck.Mojod;
					if (source == ENSource.Accounting)
					{
						receptionCheckBusiness.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await receptionCheckBusiness.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1032);
					ChequeMOperationFilter filter = new ChequeMOperationFilter
					{
						AsGuid = Guid,
						AsType = Document.Pardakht,
						ChequeGuid = item2.GuidCheck
					};
					IEnumerable<ChequeMOperationBusiness> enumerable2 = (await ChequeMOperationBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.Where((ChequeMOperationBusiness q) => q.OldState.GetValueOrDefault(ENUMCheck.None) == ENUMCheck.Mojod && q.NewState == ENUMCheck.KharjShode);
					if (enumerable2 == null || !enumerable2.Any())
					{
						continue;
					}
					foreach (ChequeMOperationBusiness item3 in enumerable2)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await item3.RemoveAsync(system, isRemoveOldSanad: false, ENUMCheck.None, 1, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1043);
					}
				}
			}
			if (!isEbtal && system != EnSystem.Transfer)
			{
				foreach (PardakhtCheckMBusiness item in CheckM ?? new List<PardakhtCheckMBusiness>())
				{
					if (!(_oldPardakht?.CheckM?.Any((PardakhtCheckMBusiness p) => p.GuidCheck == item.GuidCheck)).GetValueOrDefault())
					{
						ReceptionCheckBusiness receptionCheckBusiness2 = await ReceptionCheckBusiness.GetAsync(ContextGuid, item.GuidCheck, token, tranName);
						if (receptionCheckBusiness2 != null && receptionCheckBusiness2.Status != ENUMCheck.KharjShode)
						{
							receptionCheckBusiness2.Status = ENUMCheck.KharjShode;
							if (source == ENSource.Accounting)
							{
								receptionCheckBusiness2.ServerStatus = ServerStatusForClient.None;
							}
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await receptionCheckBusiness2.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1057);
						}
					}
					ChequeMOperationFilter filter2 = new ChequeMOperationFilter
					{
						AsType = Document.Pardakht,
						ChequeGuid = item.GuidCheck,
						LastStatus = ENUMCheck.KharjShode,
						AsGuid = Guid
					};
					ChequeMOperationBusiness chequeMOperationBusiness = (await ChequeMOperationBusiness.GetAllAsync(ContextGuid, filter2, token, tranName))?.SingleOrDefault();
					if (chequeMOperationBusiness != null && (chequeMOperationBusiness.DebitTafsilGuid != TafsilGuid || chequeMOperationBusiness.SanadNO != SanadNo || chequeMOperationBusiness.AsGuid != Guid))
					{
						chequeMOperationBusiness.DebitTafsilGuid = TafsilGuid;
						chequeMOperationBusiness.DebitMoeinGuid = MoeinGuid;
						chequeMOperationBusiness.SanadNO = SanadNo;
						chequeMOperationBusiness.AsGuid = Guid;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await chequeMOperationBusiness.SaveAsync(system, sendToServer: true, token, awaitOnSemaphore: true, 10, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1076);
					}
				}
			}
			if (isEbtal)
			{
				foreach (PardakhtCheckMBusiness item4 in CheckM ?? new List<PardakhtCheckMBusiness>())
				{
					ReceptionCheckBusiness receptionCheckBusiness3 = await ReceptionCheckBusiness.GetAsync(ContextGuid, item4.GuidCheck, token, tranName);
					receptionCheckBusiness3.Status = ENUMCheck.Mojod;
					if (source == ENSource.Accounting)
					{
						receptionCheckBusiness3.ServerStatus = ServerStatusForClient.None;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await receptionCheckBusiness3.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1087);
				}
			}
			if (isEbtal)
			{
				foreach (PardakhtCheckShBusiness item5 in (await PardakhtCheckShBusiness.GetAllAsync(ContextGuid, Guid, token, tranName)) ?? new List<PardakhtCheckShBusiness>())
				{
					PardakhtCheckShBusiness pardakhtCheckShBusiness = await PardakhtCheckShBusiness.GetAsync(ContextGuid, item5.Guid, token, tranName);
					if (pardakhtCheckShBusiness.Status == ENCheckSHStatus.BatelShode)
					{
						ChequeShOperationFilter filter3 = new ChequeShOperationFilter
						{
							ChequeGuid = pardakhtCheckShBusiness.Guid
						};
						ChequeShOperationBusiness chequeShOperationBusiness = (await ChequeShOperationBusiness.GetAllAsync(ContextGuid, filter3, token, tranName))?.OrderByDescending((ChequeShOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
						if (chequeShOperationBusiness != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await chequeShOperationBusiness.RemoveAsync(10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1102);
						}
					}
					else if (pardakhtCheckShBusiness.Status == ENCheckSHStatus.Pardakhtani)
					{
						pardakhtCheckShBusiness.Status = ENCheckSHStatus.BatelShode;
						if (source == ENSource.Accounting)
						{
							pardakhtCheckShBusiness.ServerStatus = ServerStatusForClient.None;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckShBusiness.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1108);
					}
					else
					{
						ret.AddError($"ChequeSh \r\n Status:{pardakhtCheckShBusiness.Status.GetDisplay()} \r\n MasterGuid: {Guid}", "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1110);
					}
				}
			}
			if (!isEbtal)
			{
				List<PardakhtCheckShBusiness> list = (await PardakhtCheckShBusiness.GetAllAsync(ContextGuid, Guid, token, tranName))?.Where((PardakhtCheckShBusiness q) => !(CheckSh?.Select((PardakhtCheckShBusiness u) => u.Guid)?.Contains(q.Guid)).GetValueOrDefault())?.ToList();
				foreach (PardakhtCheckShBusiness item6 in list ?? new List<PardakhtCheckShBusiness>())
				{
					PardakhtCheckShBusiness pardakhtCheckShBusiness2 = await PardakhtCheckShBusiness.GetAsync(ContextGuid, item6.Guid, token, tranName);
					if (pardakhtCheckShBusiness2.Status == ENCheckSHStatus.BatelShode)
					{
						ChequeShOperationFilter filter4 = new ChequeShOperationFilter
						{
							ChequeGuid = pardakhtCheckShBusiness2.Guid
						};
						ChequeShOperationBusiness chequeShOperationBusiness2 = (await ChequeShOperationBusiness.GetAllAsync(ContextGuid, filter4, token, tranName))?.OrderByDescending((ChequeShOperationBusiness q) => q.Ordinal)?.FirstOrDefault();
						if (chequeShOperationBusiness2 != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await chequeShOperationBusiness2.RemoveAsync(10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1127);
						}
						continue;
					}
					if (pardakhtCheckShBusiness2.Status == ENCheckSHStatus.Pardakhtani)
					{
						pardakhtCheckShBusiness2.Status = ENCheckSHStatus.BatelShode;
						if (source == ENSource.Accounting)
						{
							pardakhtCheckShBusiness2.ServerStatus = ServerStatusForClient.None;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheckShBusiness2.SaveAsync(system, 10, token, tranName), ReturnedState.None, "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1133);
						continue;
					}
					throw new ArgumentException($"ChequeSh \r\n Status:{pardakhtCheckShBusiness2.Status.GetDisplay()} \r\n MasterGuid: {Guid}");
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1141);
			WebErrorLog.Instance.StartLog(ex, "", "UpdateCheckStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1142);
		}
		return ret;
	}

	internal Task<List<WebPardakhtCheckSh>> GetWebCheckShList(CancellationToken token = default(CancellationToken))
	{
		return PardakhtCheckShMapper.Instance.MapListAsync(ContextGuid, CheckSh, new List<WebPardakhtCheckSh>(), "", token);
	}

	internal Task<List<WebPardakhtNaghd>> GetWebNaghdList(CancellationToken token = default(CancellationToken))
	{
		return PardakhtNaghdMapper.Instance.MapListAsync(ContextGuid, Naghd, new List<WebPardakhtNaghd>(), "", token);
	}

	internal Task<List<WebPardakhtBank>> GetWebBankList(CancellationToken token = default(CancellationToken))
	{
		return PardakhtBankMapper.Instance.MapListAsync(ContextGuid, Bank, new List<WebPardakhtBank>(), "", token);
	}

	public static Task<Guid> Id2ToGuidAsync(string contextGuid, int id2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.Id2ToGuidAsync(id2, token, tranName);
	}

	public async Task<List<AndroidPrintStyle>> GetPrintStylesAsync()
	{
		IEnumerable<AndroidPrintStyle> enumerable = (await UserInformationBusiness.GetAsync(ContextGuid))?.AndroidPrintStyles?.Where((AndroidPrintStyle p) => p.PrintType == PrintType.Payement && !string.IsNullOrEmpty(p.Html));
		if (enumerable != null)
		{
			foreach (AndroidPrintStyle item in enumerable)
			{
				item.Html = item.Html.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"")
					.Replace("&apos;", "'")
					.Replace("&amp;", "&");
			}
		}
		return Json.ToStringJson(enumerable).FromJson<List<AndroidPrintStyle>>("GetPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1167);
	}

	public override async Task<BaseSanadBusiness<PardakhtBusiness>> GetOldItemAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		_ = 1;
		try
		{
			if (!_oldPardakhtGet.HasValue)
			{
				_oldPardakhtGet = false;
				_oldPardakht = await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.GetFromDbAsync(Guid, token, tranName);
				_oldPardakhtGet = true;
			}
			while (_oldPardakhtGet == false)
			{
				await Task.Delay(100);
			}
			return _oldPardakht;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldItemAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1185);
			throw;
		}
	}

	public async Task<List<AndroidPrintStyle>> GetFilledPrintStylesAsync(EnSystem system)
	{
		_ = 1;
		try
		{
			List<AndroidPrintStyle> styles = await GetPrintStylesAsync();
			foreach (AndroidPrintStyle item in styles)
			{
				await FillPrintSytleImageAsync(item, system);
			}
			return styles;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFilledPrintStylesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1200);
			throw;
		}
	}

	public async Task FillPrintSytleTextAsync(AndroidPrintStyle style, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("FillPrintSytleTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 2;
			try
			{
				TafsilBusiness tafsil = await GetTafsilAsync(EnSystem.UserInterface, token);
				decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, "", token);
				style.Html = ParameterReplacement.SetParameter_StringText("NO", (await GetId2Async(token)).ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("Date", DateSh, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("Time", Date.ToShortDateString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("CodeCus", tafsil.Code.ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringText("CusName", tafsil.Name, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("AfterAccount", account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("BeforeAccount", account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("CurrentAccount", account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("TotalSum", Sum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Sum_Naghd", SumNaghd, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Sum_Check", SumCheckSh, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalText("Sum_Havale", SumBank, style.Html);
				style.Html = SetParameters_AghlamText(style.Html);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillPrintSytleTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1231);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, string tranName, bool isDeleteAction, Tuple<Document, Guid> relatedOrder = null, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if ((system != EnSystem.Transfer && system != 0 && Date < new DateTime(2000, 1, 1)) || Date > new DateTime(2099, 1, 1))
				{
					result.AddError(system, "تاریخ وارد شده معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1241);
				}
				line = 1;
				if (MoeinGuid == Guid.Empty)
				{
					result.AddError(system, "حساب معین طرف حساب انتخاب شده معتبر نمی باشد و یا شما دسترسی به حساب معین انتخاب شده ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1244);
				}
				line = 2;
				if (TafsilGuid == Guid.Empty)
				{
					result.AddError(system, "حساب تفصیلی طرف حساب انتخاب شده معتبر نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1247);
				}
				line = 3;
				if (system != EnSystem.Transfer && system != 0 && string.IsNullOrEmpty(Description))
				{
					result.AddError(system, "بخش بابت سند پرداخت پر نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1250);
				}
				line = 4;
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				line = 5;
				if (Sum <= 0m && !isDeleteAction && (system != EnSystem.Transfer || system == EnSystem.None || (!(_checkSh?.Any() ?? false) && !(SumEnteghal != 0m))))
				{
					result.AddError(system, "پرداخت با مبلغ صفر یا منفی قابل ثبت نمیباشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1259);
				}
				line = 6;
				if (system != 0 && system != EnSystem.Transfer)
				{
					result.AddReturnedValue(CheckValidationNaghd(system), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1263);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = result;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationBank(system, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1264);
					line = 8;
					returnedSaveFuncInfo = result;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationCheckShAsync(system, isDeleteAction, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1266);
					returnedSaveFuncInfo = result;
					returnedSaveFuncInfo.AddReturnedValue(await CheckValidationCheckMAsync(system, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1267);
					line = 9;
					returnedSaveFuncInfo = result;
					EnSystem system2 = system;
					bool isDeleteAction2 = isDeleteAction;
					returnedSaveFuncInfo.AddReturnedValue(await CheckSanadValidationAsync(system2, isDeleteAction2, await GetId2Async(token, tranName), tranName, token, relatedOrder), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1269);
					line = 10;
					if (system != EnSystem.Transfer && system != 0 && !IsTemp && relatedOrder == null)
					{
						line = 11;
						List<HesabChanges> list = await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, new Tuple<Document, Guid>(Document.Pardakht, Guid));
						line = 12;
						if (list != null)
						{
							returnedSaveFuncInfo = result;
							returnedSaveFuncInfo.AddReturnedValue(await list.CheckAccountAsync(system, ContextGuid, token, tranName, Document.Pardakht, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1277);
						}
						line = 13;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1284);
				result.AddReturnedValue(ex, $"#Line_{line}", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1285);
			}
			return result;
		}
	}

	private ReturnedSaveFuncInfo CheckValidationNaghd(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<PardakhtNaghdBusiness> naghd = Naghd;
			if (naghd != null && naghd.Count > 0)
			{
				foreach (PardakhtNaghdBusiness item in Naghd)
				{
					if (item == null)
					{
						returnedSaveFuncInfo.AddError(system, "در لیست پرداخت نقدی مقدار نال موجود میباشد.".GetTranslate(), "", "CheckValidationNaghd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1299);
					}
					else
					{
						returnedSaveFuncInfo.AddReturnedValue(item.CheckValidation(system), ReturnedState.None, "CheckValidationNaghd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1302);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationNaghd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1307);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidationNaghd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1308);
		}
		return returnedSaveFuncInfo;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationBank(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<PardakhtBankBusiness> bank = Bank;
			if (bank != null && bank.Count > 0)
			{
				foreach (PardakhtBankBusiness item in Bank)
				{
					if (item == null)
					{
						res.AddError(system, "در لیست پرداخت حواله مقدار نال موجود میباشد.".GetTranslate(), "", "CheckValidationBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1322);
						continue;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await item.CheckValidation(system, token, tranName), ReturnedState.None, "CheckValidationBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1325);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1330);
			res.AddReturnedValue(ex, "", "CheckValidationBank", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1331);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationCheckShAsync(EnSystem system, bool isDeleteAction, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<PardakhtCheckShBusiness> checkSh = CheckSh;
			if (checkSh != null && checkSh.Count > 0)
			{
				foreach (PardakhtCheckShBusiness item in CheckSh)
				{
					PardakhtCheckShBusiness obj = item ?? throw new Exception("در لیست پرداخت اسناد پرداختنی شخصی نال موجود میباشد.".GetTranslate());
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await obj.CheckValidationAsync(system, isDeleteAction, tranName, token), ReturnedState.None, "CheckValidationCheckShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1345);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationCheckShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1350);
			res.AddReturnedValue(ex, "", "CheckValidationCheckShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1351);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> CheckValidationCheckMAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IReadOnlyList<PardakhtCheckMBusiness> checkM = CheckM;
			if (checkM != null && checkM.Count > 0)
			{
				foreach (PardakhtCheckMBusiness item in CheckM)
				{
					PardakhtCheckMBusiness obj = item ?? throw new Exception("در لیست پرداخت اسناد پرداختنی اشخاص نال موجود میباشد.".GetTranslate());
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await obj.CheckValidationAsync(system, this, isDeleteAction, token), ReturnedState.None, "CheckValidationCheckMAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1365);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationCheckMAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1370);
			res.AddReturnedValue(ex, "", "CheckValidationCheckMAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1371);
		}
		return res;
	}

	public async Task FillPrintSytleImageAsync(AndroidPrintStyle style, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("FillPrintSytleImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 4;
			try
			{
				TafsilBusiness tafsil = await GetTafsilAsync(EnSystem.UserInterface, token);
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				decimal account = await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, "", token);
				AndroidPrintStyle androidPrintStyle = style;
				androidPrintStyle.Html = ParameterReplacement.SetParameter_StringImage("NO", (await GetId2Async(token)).ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("HeadFont", cashier.Settings.Print.HeadFont ?? (40 + "px"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("BodyFont", cashier.Settings.Print.BodyFont ?? (20 + "px"), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("FactorType", "سند پرداخت", style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Date", DateSh, style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("Time", Date.ToShortDateString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CodeCus", tafsil.Code.ToString(), style.Html);
				style.Html = ParameterReplacement.SetParameter_StringImage("CusName", tafsil.Name, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("AfterAccount", account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("BeforeAccount", account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("CurrentAccount", account, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("TotalSum", Sum, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Sum_Naghd", SumNaghd, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Sum_Check", SumCheckSh, style.Html);
				style.Html = ParameterReplacement.SetParameter_DecimalImage("Sum_Havale", SumBank, style.Html);
				androidPrintStyle = style;
				androidPrintStyle.Html = await SetParameters_AghlamImage(style.Html, system, token, tranName);
				style.Html = style.Html.ToLower();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FillPrintSytleImageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1428);
				throw;
			}
		}
	}

	private string SetParameters_AghlamText(string data)
	{
		checked
		{
			using (new JaegerMonitor("SetParameters_AghlamText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
			{
				try
				{
					string text = "";
					ClsRegex clsRegex = new ClsRegex(data, "<ROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀a-zA-Z|,*0-9-></\r\n ]*</ROW>");
					string text2 = clsRegex.SuccessfullMatch.Replace("<ROW>", "").Replace("</ROW>", "");
					string successfullMatch = clsRegex.SuccessfullMatch;
					short num = 1;
					if (CheckSh != null && CheckSh.Count > 0)
					{
						foreach (PardakhtCheckShBusiness item in CheckSh)
						{
							string data2 = text2;
							data2 = ParameterReplacement.SetParameter_StringText("Radif", num.ToString(), data2);
							data2 = ParameterReplacement.SetParameter_StringText("RowDesc", item.RowDesc, data2);
							data2 = ParameterReplacement.SetParameter_DecimalText("RowSum", item.Price, data2);
							text = ((!(text.Trim() == "")) ? (text + "\n" + data2) : data2);
							num++;
						}
					}
					if (Naghd != null && Naghd.Count > 0)
					{
						foreach (PardakhtNaghdBusiness item2 in Naghd)
						{
							string data3 = text2;
							data3 = ParameterReplacement.SetParameter_StringText("Radif", num.ToString(), data3);
							data3 = ParameterReplacement.SetParameter_StringText("RowDesc", item2.RowDesc, data3);
							data3 = ParameterReplacement.SetParameter_DecimalText("RowSum", item2.Price, data3);
							text = ((!(text.Trim() == "")) ? (text + "\n" + data3) : data3);
							num++;
						}
					}
					if (Bank != null && Bank.Count > 0)
					{
						foreach (PardakhtBankBusiness item3 in Bank)
						{
							string data4 = text2;
							data4 = ParameterReplacement.SetParameter_StringText("Radif", num.ToString(), data4);
							data4 = ParameterReplacement.SetParameter_StringText("RowDesc", item3.RowDesc, data4);
							data4 = ParameterReplacement.SetParameter_DecimalText("RowSum", item3.Price, data4);
							text = ((!(text.Trim() == "")) ? (text + "\n" + data4) : data4);
							num++;
						}
					}
					if (successfullMatch == null || successfullMatch.Trim() == "")
					{
						return data;
					}
					return data.Replace(successfullMatch, text);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetParameters_AghlamText", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1497);
					throw;
				}
			}
		}
	}

	private async Task<string> SetParameters_AghlamImage(string data, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		checked
		{
			using (new JaegerMonitor("SetParameters_AghlamImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
			{
				_ = 1;
				try
				{
					string aghlam = "";
					ClsRegex clsRegex = new ClsRegex(data, "<ROW>[░▒▓│┤╡╢╖╕╣║╗╝╜╛┐└┴┬├─┼╞╟╚╔╩╦╠═╬╧╨╤╥╙╘╒╓╫╪┘┌█▄▌▐▀:=\"a-zA-Z|;,*0-9-></ضصثقفغعهخحجچپشسیبلاتنمکگظطزرذدئوژءآ\r\n ]*</ROW>");
					string aghlamTemplate = clsRegex.SuccessfullMatch.Replace("<ROW>", "").Replace("</ROW>", "");
					string aghlamTemplateWithRow = clsRegex.SuccessfullMatch;
					short index = 1;
					if (CheckSh != null && CheckSh.Count > 0)
					{
						foreach (PardakhtCheckShBusiness item3 in CheckSh)
						{
							string data2 = ParameterReplacement.SetParameter_StringImage("Radif", index.ToString(), aghlamTemplate);
							data2 = ParameterReplacement.SetParameter_StringImage("RowDesc", item3.AndroidPrintText, data2);
							data2 = ParameterReplacement.SetParameter_DecimalImage("RowSum", item3.Price, data2);
							aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + data2) : data2);
							index++;
						}
					}
					if (Naghd != null && Naghd.Count > 0)
					{
						foreach (PardakhtNaghdBusiness item2 in Naghd)
						{
							string temp4 = aghlamTemplate;
							temp4 = ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage("Radif", index.ToString(), temp4), parameterName: "RowDesc", parameterValue: await item2.GetAndroidPrintTextAsync(system, token, tranName)), parameterName: "RowSum", parameterValue: item2.Price);
							aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + temp4) : temp4);
							index++;
						}
					}
					if (Bank != null && Bank.Count > 0)
					{
						foreach (PardakhtBankBusiness item in Bank)
						{
							string temp4 = aghlamTemplate;
							temp4 = ParameterReplacement.SetParameter_DecimalImage(data: ParameterReplacement.SetParameter_StringImage(data: ParameterReplacement.SetParameter_StringImage("Radif", index.ToString(), temp4), parameterName: "RowDesc", parameterValue: await item.GetAndroidPrintTextAsync(system, token, tranName)), parameterName: "RowSum", parameterValue: item.Price);
							aghlam = ((!(aghlam.Trim() == "")) ? (aghlam + "\n" + temp4) : temp4);
							index++;
						}
					}
					if (aghlamTemplateWithRow == null || aghlamTemplateWithRow.Trim() == "")
					{
						return data;
					}
					return data.Replace(aghlamTemplateWithRow, aghlam);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "SetParameters_AghlamImage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1568);
					throw;
				}
			}
		}
	}

	public static async Task<IEnumerable<PardakhtReportBusiness>> GetAllAsync(string contextGuid, EnSystem system, KhazaneFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		CashiersLogBusiness.Logger(contextGuid, system, sendToServer: true, EnLog.Pardakht, ENAction.Report, null, null, "گزارش پرداخت");
		IEnumerable<PardakhtReportBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.GetAllAsync(filter, token, tranName);
		if (system == EnSystem.Transfer)
		{
			return enumerable;
		}
		return await ApplyAccessAsync(contextGuid, system, enumerable);
	}

	private static async Task<IEnumerable<PardakhtReportBusiness>> ApplyAccessAsync(string contextGuid, EnSystem system, IEnumerable<PardakhtReportBusiness> pardakhts, CancellationToken token = default(CancellationToken))
	{
		List<PardakhtReportBusiness> ret = new List<PardakhtReportBusiness>();
		try
		{
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (currentCashier == null)
			{
				return pardakhts;
			}
			UserAccBusiness user = await currentCashier.GetUserAsync(token);
			IEnumerable<int> illegalStatus = user.UserAccess.Pardakht.SanadStatusAccess?.Where((PardakhtSanadStatusAccess q) => !q.Show)?.Select((PardakhtSanadStatusAccess q) => q.StatusId);
			if (pardakhts?.Any() ?? false)
			{
				if (user.UserAccess.TafsilGroupJustListGuid?.Any() ?? false)
				{
					pardakhts = pardakhts?.Where((PardakhtReportBusiness p) => (p?.TafsilGroupGuid ?? Guid.Empty) != Guid.Empty && user.UserAccess.TafsilGroupJustListGuid.Contains(p.TafsilGroupGuid));
				}
				if (illegalStatus?.Any() ?? false)
				{
					pardakhts = pardakhts?.Where((PardakhtReportBusiness q) => !illegalStatus.Contains(q.StatusId) || q.StatusId <= 0);
				}
				foreach (PardakhtReportBusiness pardakht in pardakhts)
				{
					if (pardakht != null)
					{
						if (user.UserAccess.Pardakht.Pardakht_Report)
						{
							CheckPreviewDaysAndAddIfHasAccess(pardakht, user.UserAccess.Pardakht.Pardakht_ReportPreviuseDay, ret);
						}
						else if (user.UserAccess.Pardakht.Pardakht_ReportSelf && pardakht.CashierGuid == currentCashier.Guid)
						{
							CheckPreviewDaysAndAddIfHasAccess(pardakht, user.UserAccess.Pardakht.Pardakht_ReportPreviuseDay, ret);
						}
					}
				}
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1606);
			throw;
		}
	}

	private static void CheckPreviewDaysAndAddIfHasAccess(PardakhtReportBusiness pardakht, bool reportPreviuseDay, List<PardakhtReportBusiness> ret)
	{
		try
		{
			if (reportPreviuseDay)
			{
				ret.Add(pardakht);
			}
			else if (pardakht.DateM.Date == DateTime.Now.Date)
			{
				ret.Add(pardakht);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPreviewDaysAndAddIfHasAccess", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1621);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = await CheckValidationAsync(system, tranName, isDeleteAction: true, null, token);
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
			PardakhtBusiness obj = null;
			ReturnedSaveFuncInfo result;
			try
			{
				int num;
				_ = num - 1;
				_ = 13;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1636), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1636);
					}
					transactionHasBeenCreated = true;
					obj = await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.GetFromDbAsync(Guid, token, tranName);
					if (obj == null)
					{
						result = ret;
						goto IL_0eee;
					}
					if (system == EnSystem.UserInterface)
					{
						PardakhtSanadStatusAccess pardakhtSanadStatusAccess = (await (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName)).GetUserAsync(token, tranName)).UserAccess.Pardakht.SanadStatusAccess.SingleOrDefault((PardakhtSanadStatusAccess q) => q.StatusId == StatusId);
						if (pardakhtSanadStatusAccess != null && !pardakhtSanadStatusAccess.Delete)
						{
							result = ret.AddError("شما دسترسی به حذف برگه پرداخت در وضعیت جاری را ندارید".GetTranslate(), "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1646);
							goto IL_0eee;
						}
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await UpdateCheckStatusAsync(system, token, tranName, isEbtal: true), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1648);
					if (ret.HasError)
					{
						result = ret;
						goto IL_0eee;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1650);
					if (ret.HasError)
					{
						result = ret;
						goto IL_0eee;
					}
					if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.Pardakht, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1653);
					}
					if (ret.HasError)
					{
						result = ret;
						goto IL_0eee;
					}
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1655), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1655);
					if (ret.HasError)
					{
						result = ret;
						goto IL_0eee;
					}
					SanadBusiness obj2 = await GenerateSanadAsync(system, token, tranName);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await obj2.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(Document.Pardakht, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1658);
					ChequeMOperationFilter filter = new ChequeMOperationFilter
					{
						AsGuid = Guid,
						AsType = Document.Pardakht
					};
					IEnumerable<ChequeMOperationBusiness> enumerable = await ChequeMOperationBusiness.GetAllAsync(ContextGuid, filter, token, tranName);
					if (enumerable != null && enumerable.Any())
					{
						foreach (ChequeMOperationBusiness item in enumerable)
						{
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item.RemoveAsync(system, isRemoveOldSanad: false, ENUMCheck.None, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1667);
						}
					}
					if (ret.HasError)
					{
						result = ret;
						goto IL_0eee;
					}
					List<AghsatShBusiness> list = await AghsatShBusiness.GetAllAsync(ContextGuid, new AghsatShFilter
					{
						PardakhtGuid = Guid
					}, token, tranName);
					if (list != null && list.Any())
					{
						if (ret.HasError)
						{
							result = ret;
							goto IL_0eee;
						}
						foreach (AghsatShBusiness item2 in list)
						{
							item2.PardakhtGuid = null;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(system, token, Priority.Low, sendToServer: true, awaitOnSemaphore: true, await4send: false, 10, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1677);
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1683);
					ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1684);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1689), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1689);
				}
				if (!ret.HasError)
				{
					ClsCache.GetCache(ContextGuid).CacheRepository.IPardakht.ClearCachedItemAsync(Guid, token, "");
					CashiersLogBusiness.Logger(ContextGuid, system, sendToServer: true, EnLog.Pardakht, ENAction.Delete, obj);
					if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
					{
						Task.Run(() => SendEntitiesToServerAsync(token));
					}
				}
			}
			return ret;
			IL_0eee:
			return result;
		}
	}

	public static async Task<PardakhtBusiness> GetAsync(string contextGuid, Guid guid, bool fromDatabase, CancellationToken token = default(CancellationToken), string tranName = "")
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
				throw new Exception("بر ای سورس حسابداری پرداخت رو از دیتابیس بگیر!".GetTranslate());
			}
			if (fromDatabase)
			{
				return await cache.CacheRepository.IPardakht.GetFromDbAsync(guid, token, tranName);
			}
			return await cache.CacheRepository.IPardakht.GetAsync(guid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1716);
			throw;
		}
	}

	public static async Task<PardakhtReportBusiness> GetReportAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _connectionPoolSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.GetReportAsync(guid, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static Task<int> NextId2Async(string contextGuid, short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.NextId2Async(statusId, token, tranName);
	}

	public Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return NextId2Async(ContextGuid, statusId, token, tranName);
	}

	public string NaghdDesc()
	{
		return NaghdCount + " فقره -  جمع : " + SumNaghd.NpToString("N0", 0) + " " + ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit;
	}

	public string BankDesc()
	{
		return BankCount + " فقره -  جمع : " + SumBank.NpToString("N0", 0) + " " + ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit;
	}

	public string CheckShDesc()
	{
		return CheckShCount + " فقره -  جمع : " + SumCheckSh.NpToString("N0", 0) + " " + ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit;
	}

	public string CheckMDesc()
	{
		return CheckMCount + " فقره -  جمع : " + SumCheckM.NpToString("N0", 0) + " " + ClsCache.GetCache(ContextGuid).MultiLanguageConfig.CurrencyUnit;
	}

	public new virtual void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			_bank = null;
			_naghd = null;
			_checkM = null;
			_checkSh = null;
			_enteghal = null;
			base.Dispose(disposing: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1754);
		}
	}

	public PardakhtBusiness(string contextGuid, OrderBusiness order = null)
	{
		ContextGuid = contextGuid;
		_masterOrder = order;
	}

	internal static async Task<PardakhtBusiness> GetPardakhtBusinessAsync(string contextGuid, GardeshHesabBusiness hostGardesh, EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		PardakhtBusiness ret = new PardakhtBusiness(contextGuid);
		if (hostGardesh == null)
		{
			throw new NullReferenceException("hostGardesh");
		}
		try
		{
			ret.Guid = hostGardesh.AsGuid.Value;
			ret.Description = hostGardesh.SanadDesc;
			ret.Date = hostGardesh.Date;
			ret.SetId2(hostGardesh.Id2, token);
			ret.UserGuid = hostGardesh.UGuid;
			await ret.SetTafsilGuidAsync(hostGardesh.TafsilGuid.Value, system, token, tranName);
			string tempMessage = "";
			foreach (GardeshTafsilBusiness item in hostGardesh.TafsiliItems)
			{
				if (item != null)
				{
					switch (item.AsTypeEnum)
					{
					case DocumentDet.PardakhtBank:
					{
						PardakhtBankBusiness pardakhtBankBusiness = new PardakhtBankBusiness(contextGuid)
						{
							DateM = item.DateMoaser,
							RowDesc = item.RowDesc,
							Guid = item.Guid,
							MoeinGuid = item.MoeinGuid,
							DecimalGuid = item.DecimalGuid,
							MasterGuid = item.Guid,
							Number = item.Serial
						};
						PardakhtBankBusiness pardakhtBankBusiness2 = pardakhtBankBusiness;
						string tranName2 = tranName;
						pardakhtBankBusiness2.TafsilGuid = (await BankBusiness.GetAllAsync(contextGuid, system, "", token, true, tranName2))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						PardakhtBankBusiness pardakhtBankBusiness3 = pardakhtBankBusiness;
						pardakhtBankBusiness3.SetPrice(system, item.Debit);
						ret.AddToDetList(pardakhtBankBusiness3, system, token);
						break;
					}
					case DocumentDet.PardakhtEnteghal:
					{
						tempMessage += $"*enteghalPardakht//credit-debit:{item.Credit - item.Debit}*";
						PardakhtBankBusiness pardakhtBankBusiness2 = new PardakhtBankBusiness(contextGuid)
						{
							DateM = item.DateMoaser,
							RowDesc = item.RowDesc,
							Guid = item.Guid,
							MoeinGuid = item.MoeinGuid,
							DecimalGuid = item.DecimalGuid,
							MasterGuid = item.Guid,
							Number = item.Serial
						};
						PardakhtBankBusiness pardakhtBankBusiness = pardakhtBankBusiness2;
						string tranName2 = tranName;
						pardakhtBankBusiness.TafsilGuid = (await BankBusiness.GetAllAsync(contextGuid, system, "", token, true, tranName2))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						PardakhtBankBusiness pardakhtBankBusiness4 = pardakhtBankBusiness2;
						decimal value2 = item.Debit + item.Credit;
						pardakhtBankBusiness4.SetPrice(system, value2);
						ret.AddToDetList(pardakhtBankBusiness4, system, token);
						break;
					}
					case DocumentDet.PardakhtChSh:
					{
						PardakhtCheckShBusiness pardakhtCheckShBusiness = new PardakhtCheckShBusiness(contextGuid)
						{
							DateSaresid = item.DateMoaser,
							RowDesc = item.RowDesc,
							Guid = item.Guid,
							MasterGuid = item.Guid,
							Number = item.Serial
						};
						PardakhtCheckShBusiness pardakhtCheckShBusiness2 = pardakhtCheckShBusiness;
						string tranName2 = tranName;
						pardakhtCheckShBusiness2.BankTafsilGuid = (await BankBusiness.GetAllAsync(contextGuid, system, "", token, true, tranName2))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						PardakhtCheckShBusiness pardakhtCheckShBusiness3 = pardakhtCheckShBusiness;
						decimal value = item.Debit + item.Credit;
						pardakhtCheckShBusiness3.SetPrice(system, value);
						ret.AddToDetList(pardakhtCheckShBusiness3, system, token);
						break;
					}
					case DocumentDet.PardakhtN:
					{
						PardakhtNaghdBusiness pardakhtNaghdBusiness = new PardakhtNaghdBusiness(contextGuid)
						{
							RowDesc = item.RowDesc,
							Guid = item.Guid,
							MoeinGuid = item.MoeinGuid,
							MasterGuid = item.Guid
						};
						PardakhtNaghdBusiness pardakhtNaghdBusiness2 = pardakhtNaghdBusiness;
						string tranName2 = tranName;
						pardakhtNaghdBusiness2.TafsilGuid = (await SandoghBusiness.GetAllAsync(contextGuid, system, "", true, tranName2, token))?.FirstOrDefault()?.Guid ?? Guid.Empty;
						PardakhtNaghdBusiness pardakhtNaghdBusiness3 = pardakhtNaghdBusiness;
						pardakhtNaghdBusiness3.SetPrice(system, item.Debit);
						ret.AddToDetList(pardakhtNaghdBusiness3, system, token);
						break;
					}
					default:
						WebErrorLog.Instance.StartLog("not defined! \r\n" + Json.ToStringJson(item, indented: true), "", "GetPardakhtBusinessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1840);
						break;
					}
				}
			}
			if (!string.IsNullOrEmpty(tempMessage))
			{
				ret.Description += tempMessage;
			}
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtBusinessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1846);
			throw;
		}
	}

	public async Task UpdateBeforeAccountAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("UpdateBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 2;
			try
			{
				PardakhtBusiness pardakhtBusiness = (PardakhtBusiness)(await GetOldItemAsync(system, token, tranName));
				if (pardakhtBusiness == null || pardakhtBusiness.Sum <= 0m || (pardakhtBusiness?.IsTemp ?? true))
				{
					SetBeforeAccount(await AccountBusiness.GetAccountSumAsync(ContextGuid, null, TafsilGuid, true, tranName, token));
				}
				SetAfterAccount(await GetBeforeAccountAsync(system, token, tranName) - Sum);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateBeforeAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1863);
				throw;
			}
		}
	}

	public async Task<bool> GetCanInsertAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("GetCanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 1;
			try
			{
				UserAccBusiness obj = (await ((await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)) ?? throw new NullReferenceException("cashier is Null!!")).GetUserAsync(token)) ?? throw new NullReferenceException("user is Null");
				bool flag = obj.UserAccess.Pardakht.SanadStatusAccess.SingleOrDefault((PardakhtSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
				return obj.UserAccess.Pardakht.Pardakht_Insert && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetCanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1876);
				throw;
			}
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					return IsTemp;
				}
				CashierBusiness cashier = (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)) ?? throw new NullReferenceException("cashier is Null!!");
				UserAccBusiness userAccBusiness = (await cashier.GetUserAsync(token)) ?? throw new NullReferenceException("user is Null");
				return (userAccBusiness.UserAccess?.Pardakht?.Pardakht_Delete).GetValueOrDefault(true) || ((userAccBusiness.UserAccess?.Pardakht?.Pardakht_Delete_Self).GetValueOrDefault(true) && UserGuid == cashier.Guid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1888);
				throw;
			}
		}
	}

	public async Task<bool> CanUpdateAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 1;
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					return IsTemp;
				}
				CashierBusiness cashier = (await CashierBusiness.GetCurrentCashierAsync(ContextGuid)) ?? throw new NullReferenceException("cashier is Null!!");
				UserAccBusiness userAccBusiness = (await cashier.GetUserAsync(token, tranName)) ?? throw new NullReferenceException("user is Null");
				bool flag = userAccBusiness.UserAccess.Pardakht.SanadStatusAccess.SingleOrDefault((PardakhtSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
				return ((userAccBusiness.UserAccess?.Pardakht?.Pardakht_Update).GetValueOrDefault(true) || ((userAccBusiness.UserAccess?.Pardakht?.Pardakht_Update_Self).GetValueOrDefault(true) && UserGuid == cashier.Guid)) && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1902);
				throw;
			}
		}
	}

	public async Task<bool> CanPrintAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = (await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token)) ?? throw new NullReferenceException("cashier is Null!!");
				UserAccBusiness userAccBusiness = (await cashier.GetUserAsync(token, tranName)) ?? throw new NullReferenceException("user is Null");
				return (userAccBusiness.UserAccess?.Pardakht?.Pardakht_Print).GetValueOrDefault(true) || ((userAccBusiness.UserAccess?.Pardakht?.Pardakht_Print_Self).GetValueOrDefault(true) && UserGuid == cashier.Guid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanPrintAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1913);
				throw;
			}
		}
	}

	public async Task<bool> CanReportAsync(CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = await cashier.GetUserAsync(token);
				return userAccBusiness.UserAccess.Pardakht.Pardakht_Report || (userAccBusiness.UserAccess.Pardakht.Pardakht_ReportSelf && UserGuid == cashier.Guid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanReportAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 1925);
				throw;
			}
		}
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 39;
			try
			{
				AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				ClsCache cache = ClsCache.GetCache(ContextGuid);
				int num = _oldPardakht?.SanadNo ?? 0;
				if (system == EnSystem.UserInterface && ChangeStatus == EntityChangeSt.Edited && num == 0 && cache.Source == ENSource.Accounting)
				{
					throw new NotImplementedException("وضعیت ویرایش ولیکن".GetTranslate() + "OldPardakht==null!!");
				}
				Guid oldGuid = Guid.NewGuid();
				SanadBusiness sanad = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
				if (sanad != null)
				{
					oldGuid = sanad.Guid;
				}
				SanadBusiness sanadBusiness = new SanadBusiness(ContextGuid, sanad?.SanadNo ?? 0);
				SanadBusiness sanadBusiness2 = sanadBusiness;
				sanadBusiness2.Description = await this.GetPardakhtDescAsync(token, tranName);
				sanadBusiness.DateM = Date;
				sanadBusiness.UserGuid = UserGuid;
				sanadBusiness.Guid = oldGuid;
				sanadBusiness.SanadNo = SanadNo;
				sanadBusiness.SanadType = PacketParser.Entities.SanadType.Automatic;
				sanad = sanadBusiness;
				Sanad_DetBusiness sanad_DetBusiness = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
				{
					Credit = 0m,
					Debit = Sum
				};
				Sanad_DetBusiness sanad_DetBusiness2 = sanad_DetBusiness;
				sanad_DetBusiness2.RowDesc = await this.GetPardakhtDescAsync(token, tranName);
				sanad_DetBusiness.DecimalGuid = Guid.Empty;
				sanad_DetBusiness.Guid = Guid.NewGuid();
				sanad_DetBusiness.Qt = 0m;
				Sanad_DetBusiness peymentDebit = sanad_DetBusiness;
				await peymentDebit.SetMoeinGuidAsync(MoeinGuid, token, tranName);
				await peymentDebit.SetTafsilGuidAsync(TafsilGuid, tranName, token);
				if ((TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
				{
					await peymentDebit.SetTafsil2GuidAsync(TafsilGuid2, tranName, token);
				}
				if ((TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
				{
					await peymentDebit.SetTafsil3GuidAsync(TafsilGuid3, tranName, token);
				}
				await sanad.AddToListSanadAsync(peymentDebit, system, token);
				IReadOnlyList<PardakhtBankBusiness> bank3 = Bank;
				if (bank3 != null && bank3.Count > 0)
				{
					foreach (PardakhtBankBusiness bank in Bank.ToList())
					{
						if ((bank?.WageGuid ?? Guid.Empty) != Guid.Empty && (bank?.WagePrice ?? 0m) != 0m)
						{
							Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
							{
								Credit = 0m,
								Debit = bank.WagePrice
							};
							sanad_DetBusiness2 = sanad_DetBusiness3;
							sanad_DetBusiness2.RowDesc = await bank.GetPardakhtBankDescAsync(this, token, tranName);
							sanad_DetBusiness3.Guid = Guid.NewGuid();
							sanad_DetBusiness3.Qt = 0m;
							sanad_DetBusiness = sanad_DetBusiness3;
							await sanad_DetBusiness.SetMoeinGuidAsync(bank.WageMoeinGuid.Value, token, tranName);
							await sanad_DetBusiness.SetTafsilGuidAsync(bank.WageTafsilGuid, tranName, token);
							await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
						}
					}
				}
				IReadOnlyList<PardakhtNaghdBusiness> naghd2 = Naghd;
				if (naghd2 != null && naghd2.Count > 0)
				{
					foreach (PardakhtNaghdBusiness naghd in Naghd?.ToList())
					{
						sanad_DetBusiness2 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
						{
							Credit = naghd.Price,
							Debit = 0m
						};
						Sanad_DetBusiness sanad_DetBusiness3 = sanad_DetBusiness2;
						sanad_DetBusiness3.RowDesc = await naghd.GetPardakhtNaghdDescAsync(this, token, tranName);
						sanad_DetBusiness2.DecimalGuid = Guid.Empty;
						sanad_DetBusiness2.Guid = Guid.NewGuid();
						sanad_DetBusiness2.Qt = 0m;
						sanad_DetBusiness = sanad_DetBusiness2;
						await sanad_DetBusiness.SetMoeinGuidAsync(naghd.MoeinGuid, token, tranName);
						await sanad_DetBusiness.SetTafsilGuidAsync(naghd.TafsilGuid, tranName, token);
						await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
					}
				}
				IReadOnlyList<PardakhtBankBusiness> bank4 = Bank;
				if (bank4 != null && bank4.Count > 0)
				{
					foreach (PardakhtBankBusiness bank in Bank.ToList())
					{
						Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
						{
							Credit = bank.Price,
							Debit = 0m
						};
						sanad_DetBusiness2 = sanad_DetBusiness3;
						sanad_DetBusiness2.RowDesc = await bank.GetPardakhtBankDescAsync(this, token, tranName);
						sanad_DetBusiness3.DecimalGuid = bank.DecimalGuid;
						sanad_DetBusiness3.Guid = Guid.NewGuid();
						sanad_DetBusiness3.Qt = 0m;
						sanad_DetBusiness = sanad_DetBusiness3;
						await sanad_DetBusiness.SetMoeinGuidAsync(bank.MoeinGuid, token, tranName);
						await sanad_DetBusiness.SetTafsilGuidAsync(bank.TafsilGuid, tranName, token);
						await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
						if ((bank?.WageGuid ?? Guid.Empty) != Guid.Empty && (bank?.WagePrice ?? 0m) != 0m)
						{
							Sanad_DetBusiness sanad_DetBusiness4 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
							{
								Credit = bank.WagePrice,
								Debit = 0m
							};
							sanad_DetBusiness2 = sanad_DetBusiness4;
							sanad_DetBusiness2.RowDesc = await bank.GetPardakhtBankDescAsync(this, token, tranName);
							sanad_DetBusiness4.Guid = Guid.NewGuid();
							sanad_DetBusiness4.Qt = 0m;
							sanad_DetBusiness3 = sanad_DetBusiness4;
							await sanad_DetBusiness3.SetMoeinGuidAsync(bank.MoeinGuid, token, tranName);
							await sanad_DetBusiness3.SetTafsilGuidAsync(bank.TafsilGuid, tranName, token);
							await sanad.AddToListSanadAsync(sanad_DetBusiness3, system, token);
						}
					}
				}
				IReadOnlyList<PardakhtCheckMBusiness> checkM = CheckM;
				if (checkM != null && checkM.Count > 0)
				{
					foreach (PardakhtCheckMBusiness checqeM in CheckM.ToList())
					{
						if (checqeM != null)
						{
							ReceptionCheckBusiness receptionCheque = await ReceptionCheckBusiness.GetAsync(ContextGuid, checqeM.GuidCheck, token, tranName);
							Sanad_DetBusiness sanad_DetBusiness4 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
							{
								Credit = checqeM.Price,
								Debit = 0m
							};
							Sanad_DetBusiness sanad_DetBusiness3 = sanad_DetBusiness4;
							sanad_DetBusiness3.RowDesc = await checqeM.GetPardakhtCheckMDescAsync(this, token, tranName);
							sanad_DetBusiness4.DecimalGuid = checqeM.DecimalGuid;
							sanad_DetBusiness4.Guid = Guid.NewGuid();
							sanad_DetBusiness4.Qt = 0m;
							sanad_DetBusiness = sanad_DetBusiness4;
							Guid guid = receptionCheque?.DebitMoeinGuid ?? setting.CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception;
							Guid tafsilGuid = receptionCheque?.DebitTafsilGuid ?? setting.CodeSetting.CheckMCoding.Debit_TafsilGuid_Reception;
							checqeM.SetCreditMoeinGuid(guid);
							checqeM.SetCreditTafsilGuid(tafsilGuid);
							await sanad_DetBusiness.SetMoeinGuidAsync(guid, token, tranName);
							await sanad_DetBusiness.SetTafsilGuidAsync(tafsilGuid, tranName, token);
							await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
						}
					}
				}
				if (CheckSh?.Any() ?? false)
				{
					foreach (PardakhtCheckShBusiness checksh in CheckSh.Where((PardakhtCheckShBusiness p) => p.Status != ENCheckSHStatus.BatelShode).ToList())
					{
						Sanad_DetBusiness sanad_DetBusiness3 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
						{
							Credit = checksh.Price,
							Debit = 0m
						};
						Sanad_DetBusiness sanad_DetBusiness4 = sanad_DetBusiness3;
						sanad_DetBusiness4.RowDesc = await checksh.GetPardakhtCheckShDescAsync(this, token, tranName);
						sanad_DetBusiness3.DecimalGuid = Guid.Empty;
						sanad_DetBusiness3.Guid = Guid.NewGuid();
						sanad_DetBusiness3.Qt = 0m;
						sanad_DetBusiness = sanad_DetBusiness3;
						await sanad_DetBusiness.SetMoeinGuidAsync(setting.CodeSetting.CheckSHCoding.Debit_MoeinGuid_Pardakht, token, tranName);
						await sanad_DetBusiness.SetTafsilGuidAsync(checksh?.BankTafsilGuid ?? setting.CodeSetting.CheckSHCoding.Debit_TafsilGuid_Pardakht, tranName, token);
						await sanad.AddToListSanadAsync(sanad_DetBusiness, system, token);
					}
				}
				if (Enteghal?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item in Enteghal.ToList())
					{
						sanad_DetBusiness2 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.Pardakht, sanad.NextOrdinal())
						{
							Credit = item.Credit,
							Debit = item.Debit
						};
						Sanad_DetBusiness sanad_DetBusiness4 = sanad_DetBusiness2;
						string translate = "پرداخت".GetTranslate();
						sanad_DetBusiness4.RowDesc = translate + $"({await GetId2Async(token, tranName)} {Description} {item.RowDesc})";
						sanad_DetBusiness2.DecimalGuid = item.DecimalGuid;
						sanad_DetBusiness2.Guid = Guid.NewGuid();
						sanad_DetBusiness2.Qt = 0m;
						Sanad_DetBusiness sanad_DetBusiness3 = sanad_DetBusiness2;
						await sanad_DetBusiness3.SetMoeinGuidAsync(item.MoeinGuid, token, tranName);
						await sanad_DetBusiness3.SetTafsilGuidAsync(item.TafsilGuid, tranName, token);
						if ((TafsilGuid2 ?? Guid.Empty) != Guid.Empty)
						{
							await sanad_DetBusiness3.SetTafsil2GuidAsync(TafsilGuid2, tranName, token);
						}
						if ((TafsilGuid3 ?? Guid.Empty) != Guid.Empty)
						{
							await sanad_DetBusiness3.SetTafsil3GuidAsync(TafsilGuid3, tranName, token);
						}
						await sanad.AddToListSanadAsync(sanad_DetBusiness3, system, token);
					}
				}
				return sanad;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2109);
				throw;
			}
		}
	}

	public static async Task<PardakhtBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? tafsilGuid = null, string description = "", Guid? guid = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			PardakhtBusiness ret = new PardakhtBusiness(contextGuid)
			{
				ChangeStatus = EntityChangeSt.New,
				Source = ClsCache.GetCache(contextGuid).Source,
				UserGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid)
			};
			if (ret.Source != ENSource.Accounting)
			{
				ret.SanadNo = 0;
			}
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				await ret.SetTafsilGuidAsync(tafsilGuid.Value, (system != EnSystem.SmsParser) ? system : EnSystem.None, token);
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
			await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, ret, system, token);
			if (ret.Source != ENSource.Accounting)
			{
				await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.Add2CacheAsync(ret, token, "");
			}
			ret.MetaData = new PardakhtMetaData
			{
				System = system
			};
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2136);
			throw;
		}
	}

	public Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		return Task.FromResult<List<ProductsChanges>>(null);
	}

	public async Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		_ = 2;
		try
		{
			SanadBusiness newSanad = await GenerateSanadAsync(system, token, tranName);
			PardakhtBusiness pardakhtBusiness = (PardakhtBusiness)(await GetOldItemAsync(system, token, tranName));
			return CalcChanges.ComputeHesabChange(await SanadBusiness.GetAsync(ContextGuid, pardakhtBusiness?.SanadNo ?? 0, token, tranName), newSanad, isDeleteAction, autoSanadItemsForSave);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLstHesabChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2150);
			throw;
		}
	}

	public async Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return MoeinChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return TafsilChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public void RemoveFromDetList(BaseKhazaneDetail item)
	{
		using (new JaegerMonitor("RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (item == null)
				{
					return;
				}
				item.OnPriceChange -= Det_OnPriceChange;
				PardakhtNaghdBusiness pardakhtNaghdBusiness = item as PardakhtNaghdBusiness;
				if (pardakhtNaghdBusiness == null)
				{
					PardakhtCheckShBusiness pardakhtCheckShBusiness = item as PardakhtCheckShBusiness;
					if (pardakhtCheckShBusiness == null)
					{
						PardakhtCheckMBusiness pardakhtCheckMBusiness = item as PardakhtCheckMBusiness;
						if (pardakhtCheckMBusiness == null)
						{
							PardakhtBankBusiness pardakhtBankBusiness = item as PardakhtBankBusiness;
							if (pardakhtBankBusiness == null)
							{
								Sanad_DetBusiness sanad_DetBusiness = item as Sanad_DetBusiness;
								if (sanad_DetBusiness == null)
								{
									throw new NotImplementedException();
								}
								Sanad_DetBusiness sanad_DetBusiness2 = _enteghal?.FirstOrDefault((Sanad_DetBusiness p) => p.Guid == sanad_DetBusiness.Guid);
								if (sanad_DetBusiness2 != null)
								{
									_enteghal.Remove(sanad_DetBusiness2);
								}
							}
							else
							{
								PardakhtBankBusiness pardakhtBankBusiness2 = _bank?.FirstOrDefault((PardakhtBankBusiness p) => p.Guid == pardakhtBankBusiness.Guid);
								if (pardakhtBankBusiness2 != null)
								{
									_bank.Remove(pardakhtBankBusiness2);
								}
							}
						}
						else
						{
							PardakhtCheckMBusiness pardakhtCheckMBusiness2 = _checkM?.FirstOrDefault((PardakhtCheckMBusiness p) => p.Guid == pardakhtCheckMBusiness.Guid);
							if (pardakhtCheckMBusiness2 != null)
							{
								_checkM?.Remove(pardakhtCheckMBusiness2);
							}
						}
					}
					else
					{
						PardakhtCheckShBusiness pardakhtCheckShBusiness2 = _checkSh?.FirstOrDefault((PardakhtCheckShBusiness p) => p.Guid == pardakhtCheckShBusiness.Guid);
						if (pardakhtCheckShBusiness2 != null)
						{
							_checkSh?.Remove(pardakhtCheckShBusiness2);
						}
					}
				}
				else
				{
					PardakhtNaghdBusiness pardakhtNaghdBusiness2 = _naghd?.FirstOrDefault((PardakhtNaghdBusiness p) => p.Guid == pardakhtNaghdBusiness.Guid);
					if (pardakhtNaghdBusiness2 != null)
					{
						_naghd.Remove(pardakhtNaghdBusiness2);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2191);
			}
		}
	}

	public void AddToDetList(BaseKhazaneDetail item, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (item == null)
				{
					return;
				}
				if (item.Ordinal <= 0 && item.Ordinal <= 0)
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
				PardakhtNaghdBusiness pardakhtNaghdBusiness = item as PardakhtNaghdBusiness;
				if (pardakhtNaghdBusiness == null)
				{
					PardakhtCheckShBusiness pardakhtCheckShBusiness = item as PardakhtCheckShBusiness;
					if (pardakhtCheckShBusiness == null)
					{
						PardakhtCheckMBusiness pardakhtCheckMBusiness = item as PardakhtCheckMBusiness;
						if (pardakhtCheckMBusiness == null)
						{
							PardakhtBankBusiness pardakhtBankBusiness = item as PardakhtBankBusiness;
							if (pardakhtBankBusiness == null)
							{
								Sanad_DetBusiness sanad_DetBusiness = item as Sanad_DetBusiness;
								if (sanad_DetBusiness == null)
								{
									throw new NotImplementedException();
								}
								if (_enteghal == null)
								{
									_enteghal = new List<Sanad_DetBusiness>();
								}
								if (!_enteghal.Any((Sanad_DetBusiness p) => p.Guid == sanad_DetBusiness.Guid))
								{
									sanad_DetBusiness.MasterGuid = Guid;
									sanad_DetBusiness.AsGuid = Guid;
									if (sanad_DetBusiness.Debit > 0m)
									{
										sanad_DetBusiness.SetPrice(system, sanad_DetBusiness.Debit);
									}
									else
									{
										sanad_DetBusiness.SetPrice(system, sanad_DetBusiness.Credit);
									}
									_enteghal.Add(sanad_DetBusiness);
								}
							}
							else
							{
								if (_bank == null)
								{
									_bank = new List<PardakhtBankBusiness>();
								}
								if (!_bank.Any((PardakhtBankBusiness p) => p.Guid == pardakhtBankBusiness.Guid))
								{
									pardakhtBankBusiness.MasterGuid = Guid;
									_bank.Add(pardakhtBankBusiness);
								}
							}
						}
						else
						{
							if (_checkM == null)
							{
								_checkM = new List<PardakhtCheckMBusiness>();
							}
							if (!_checkM.Any((PardakhtCheckMBusiness p) => p.Guid == pardakhtCheckMBusiness.Guid))
							{
								pardakhtCheckMBusiness.MasterGuid = Guid;
								_checkM.Add(pardakhtCheckMBusiness);
							}
						}
					}
					else
					{
						if (_checkSh == null)
						{
							_checkSh = new List<PardakhtCheckShBusiness>();
						}
						if (!_checkSh.Any((PardakhtCheckShBusiness p) => p.Guid == pardakhtCheckShBusiness.Guid))
						{
							pardakhtCheckShBusiness.MasterGuid = Guid;
							_checkSh.Add(pardakhtCheckShBusiness);
						}
						else if (system == EnSystem.UserInterface)
						{
							PardakhtCheckShBusiness item2 = _checkSh.FirstOrDefault((PardakhtCheckShBusiness p) => p.Guid == pardakhtCheckShBusiness.Guid);
							_checkSh.Remove(item2);
							pardakhtCheckShBusiness.MasterGuid = Guid;
							_checkSh.Add(pardakhtCheckShBusiness);
						}
					}
				}
				else
				{
					if (_naghd == null)
					{
						_naghd = new List<PardakhtNaghdBusiness>();
					}
					if (!_naghd.Any((PardakhtNaghdBusiness p) => p.Guid == pardakhtNaghdBusiness.Guid))
					{
						pardakhtNaghdBusiness.MasterGuid = Guid;
						_naghd.Add(pardakhtNaghdBusiness);
					}
				}
				item.OnDisposed += Item_OnDisposed;
				item.OnOrdinalChanged += Item_OnOrdinalChanged;
				item.OnPriceChange += Det_OnPriceChange;
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2274);
			}
		}
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
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Item_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
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
				WebErrorLog.Instance.StartLog(ex, "", "Item_OnDisposed", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2294);
				throw;
			}
		}
	}

	public string AutoTitle(ENAction act)
	{
		throw new NotImplementedException();
	}

	public void AddRangeToNaghdList(List<PardakhtNaghdBusiness> list, bool clearList, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToNaghdList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (clearList)
				{
					_naghd = new List<PardakhtNaghdBusiness>();
				}
				if (list == null || !list.Any())
				{
					return;
				}
				if (_naghd == null)
				{
					_naghd = new List<PardakhtNaghdBusiness>();
				}
				foreach (PardakhtNaghdBusiness item in list)
				{
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
					item.MasterGuid = Guid;
					item.OnDisposed += Item_OnDisposed;
					item.OnOrdinalChanged += Item_OnOrdinalChanged;
					_naghd.Add(item);
				}
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddRangeToNaghdList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToNaghdList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2327);
			}
		}
	}

	public void AddRangeToBankList(List<PardakhtBankBusiness> list, bool clearList, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToBankList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (clearList)
				{
					_bank = new List<PardakhtBankBusiness>();
				}
				if (list == null || !list.Any())
				{
					return;
				}
				if (_bank == null)
				{
					_bank = new List<PardakhtBankBusiness>();
				}
				foreach (PardakhtBankBusiness item in list)
				{
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
					item.MasterGuid = Guid;
					item.OnDisposed += Item_OnDisposed;
					item.OnOrdinalChanged += Item_OnOrdinalChanged;
					_bank.Add(item);
				}
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddRangeToBankList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToBankList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2357);
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
				PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, sender, e, default(CancellationToken), 60, "Item_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Item_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2376);
		}
		return Task.CompletedTask;
	}

	public void AddRangeToChequeMList(List<PardakhtCheckMBusiness> list, bool clearList, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToChequeMList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (clearList)
				{
					_checkM = new List<PardakhtCheckMBusiness>();
				}
				if (list == null || !list.Any())
				{
					return;
				}
				if (_checkM == null)
				{
					_checkM = new List<PardakhtCheckMBusiness>();
				}
				foreach (PardakhtCheckMBusiness item in list)
				{
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
					item.MasterGuid = Guid;
					item.OnDisposed += Item_OnDisposed;
					item.OnOrdinalChanged += Item_OnOrdinalChanged;
					_checkM.Add(item);
				}
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddRangeToChequeMList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToChequeMList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2407);
			}
		}
	}

	public void AddRangeToChequeShList(List<PardakhtCheckShBusiness> list, bool clearList, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToChequeShList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (clearList)
				{
					_checkSh = new List<PardakhtCheckShBusiness>();
				}
				if (list == null || !list.Any())
				{
					return;
				}
				if (_checkSh == null)
				{
					_checkSh = new List<PardakhtCheckShBusiness>();
				}
				foreach (PardakhtCheckShBusiness item in list)
				{
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
					item.MasterGuid = Guid;
					item.OnDisposed += Item_OnDisposed;
					item.OnOrdinalChanged += Item_OnOrdinalChanged;
					_checkSh.Add(item);
				}
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddRangeToChequeShList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToChequeShList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2437);
			}
		}
	}

	public void AddRangeToEnteghalList(List<Sanad_DetBusiness> list, bool clearList, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToEnteghalList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			try
			{
				if (clearList)
				{
					_enteghal = new List<Sanad_DetBusiness>();
				}
				if (list == null || !list.Any())
				{
					return;
				}
				if (_enteghal == null)
				{
					_enteghal = new List<Sanad_DetBusiness>();
				}
				foreach (Sanad_DetBusiness item in list)
				{
					item.MasterGuid = Guid;
					item.OnDisposed += Item_OnDisposed;
					if (item.Debit > 0m)
					{
						item.SetPrice(system, item.Debit);
					}
					else
					{
						item.SetPrice(system, item.Credit);
					}
					if (item.Ordinal <= 0)
					{
						IReadOnlyCollection<BaseKhazaneDetail> detailList = DetailList;
						if (detailList != null && detailList.Any())
						{
							int num = DetailList.Max((BaseKhazaneDetail q) => q.Ordinal);
							if (num <= 0)
							{
								item.SetOrdinalChange(system, 1, throwOnOrdinalChanged: false);
							}
							else
							{
								item.SetOrdinalChange(system, checked(num + 1), throwOnOrdinalChanged: false);
							}
						}
						else
						{
							item.SetOrdinalChange(system, 1, throwOnOrdinalChanged: false);
						}
					}
					item.OnOrdinalChanged += Item_OnOrdinalChanged;
					_enteghal.Add(item);
				}
				if (this.OnSumChanged != null)
				{
					PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), token, 60, "AddRangeToEnteghalList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToEnteghalList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2471);
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendListAsync(EnSystem system, IEnumerable<PardakhtReportBusiness> pardakhtList, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (pardakhtList == null || !pardakhtList.Any())
			{
				return res;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			foreach (PardakhtReportBusiness item in pardakhtList?.ToList())
			{
				tasks.Add(item.SendAsync(token, system, Priority.Low));
				if (tasks.Count > 20)
				{
					await Task.WhenAny(tasks);
				}
			}
			await Task.WhenAll(tasks);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2493);
			res.AddReturnedValue(ex, "", "SendListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 2494);
		}
		return res;
	}

	public static Task<List<Guid>> GetNotTransferedAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.GetNotTransferedAsync(token, tranName);
	}

	public static Task<ReturnedSaveFuncInfo> UpdateTransferDateAsync(string contextGuid, OperationTransfer filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.UpdateTransferDateAsync(filter, token, tranName);
	}

	public async Task<IBaseKhazane> MapAsync(string contextGuid, IBaseKhazane source, IBaseKhazane destination, CancellationToken token)
	{
		return await PardakhtMapper.Instance.MapAsync(contextGuid, (PardakhtBusiness)source, (PardakhtBusiness)destination, "", token);
	}

	public Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName = "", Tuple<Document, Guid> relatedOrder = null, CancellationToken token = default(CancellationToken))
	{
		return CheckValidationAsync(system, tranName, isDeleteAction, relatedOrder, token);
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, bool sendToServer, Priority priority = Priority.Low, bool awaitOnSemaphore = true, string CustomerUserName = "", string CustomerPassword = "", int timeout = 60000, bool await4send = false, short tryCount = 10, string tranName = "", CancellationToken token = default(CancellationToken), SanadBusiness orderSanad = null, IMessageSender messageSender = null)
	{
		return SaveAsync(system, sendToServer, priority, token, awaitOnSemaphore, tryCount, tranName, await4send, orderSanad, messageSender);
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

	public async Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
		{
			_ = 25;
			try
			{
				if (system == EnSystem.Transfer)
				{
					return null;
				}
				List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
				if (Naghd?.Any() ?? false)
				{
					foreach (PardakhtNaghdBusiness item3 in Naghd)
					{
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, item3.TafsilGuid, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = MoeinGuid,
							DebitTafsilGuid = TafsilGuid,
							Date = DateM,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtN,
							AsTypeDocument = Document.Pardakht,
							Debit = item3.Price,
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = item3.MoeinGuid;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = item3.TafsilGuid;
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = "نقدی از".GetTranslate() + " " + tafsil?.Name + " " + item3.RowDesc;
						viewSanadDetailCoverBusiness.DateMoaser = DateM;
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
						viewSanadDetailCoverBusiness.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness.TafsilGuid3 = TafsilGuid3;
						list2.Add(viewSanadDetailCoverBusiness);
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = MoeinGuid,
							DebitTafsilGuid = TafsilGuid,
							Date = item3.DateM,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtN,
							AsTypeDocument = Document.Pardakht,
							Debit = 0m,
							Credit = item3.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness2.CreditMoeinGuid = item3.MoeinGuid;
						viewSanadDetailCoverBusiness2.CreditTafsilGuid = item3.TafsilGuid;
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = item3.RowDesc;
						viewSanadDetailCoverBusiness2.DateMoaser = DateM;
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
						list2.Add(viewSanadDetailCoverBusiness2);
					}
				}
				if (CheckSh?.Any() ?? false)
				{
					foreach (PardakhtCheckShBusiness item4 in CheckSh?.Where((PardakhtCheckShBusiness q) => q.Status != ENCheckSHStatus.BatelShode))
					{
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							CreditMoeinGuid = item4.CreditMoeinGuid,
							CreditTafsilGuid = item4.CreditTafsilGuid,
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtChSh,
							AsTypeDocument = Document.Pardakht,
							Debit = 0m,
							Credit = item4.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = item4.RowDesc;
						viewSanadDetailCoverBusiness.DateMoaser = item4.DateSaresid;
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
							viewSanadDetailCoverBusiness3.DebitMoeinGuid = item4.DebitA2reMoeinGuid;
							viewSanadDetailCoverBusiness3.DebitTafsilGuid = item4.DebitA2reTafsiGuid;
						}
						else
						{
							viewSanadDetailCoverBusiness3.DebitMoeinGuid = MoeinGuid;
							viewSanadDetailCoverBusiness3.DebitTafsilGuid = TafsilGuid;
						}
						list.Add(viewSanadDetailCoverBusiness3);
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, item4.BankTafsilGuid, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							CreditMoeinGuid = item4.CreditMoeinGuid,
							CreditTafsilGuid = item4.CreditTafsilGuid,
							Date = Date,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtChSh,
							AsTypeDocument = Document.Pardakht,
							Debit = item4.Price,
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = "چک شخصی سریال: " + item4.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item4.DateSarresidSh + " " + item4.RowDesc;
						viewSanadDetailCoverBusiness2.DateMoaser = item4.DateSaresid;
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
							viewSanadDetailCoverBusiness4.DebitMoeinGuid = item4.DebitA2reMoeinGuid;
							viewSanadDetailCoverBusiness4.DebitTafsilGuid = item4.DebitA2reTafsiGuid;
						}
						else
						{
							viewSanadDetailCoverBusiness4.DebitMoeinGuid = MoeinGuid;
							viewSanadDetailCoverBusiness4.DebitTafsilGuid = TafsilGuid;
						}
						list.Add(viewSanadDetailCoverBusiness4);
					}
				}
				if (CheckM?.Any() ?? false)
				{
					foreach (PardakhtCheckMBusiness item5 in CheckM)
					{
						_ = Guid.Empty;
						ReceptionCheckBusiness cheque = await ReceptionCheckBusiness.GetAsync(ContextGuid, item5.GuidCheck, token, tranName);
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(tafsilGuid: (!(cheque?.Aval2re ?? false)) ? (await cheque.GetReceptionMasterAsync(system, token, tranName)).TafsilGuid : cheque.A2re_PardazandeTafsilGuid.Value, contextGuid: ContextGuid, system: system, tranName: tranName, token: token, memberName: "GetSanadCoverListAsync", sourceFilePath: "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness5 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = MoeinGuid,
							DebitTafsilGuid = TafsilGuid,
							Date = DateM,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtChAshkhas,
							AsTypeDocument = Document.Pardakht,
							Debit = (cheque?.Price ?? 0m),
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness5;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness5;
						viewSanadDetailCoverBusiness.CreditMoeinGuid = await item5.GetCreditMoeinGuidAsync(token, tranName);
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness6 = viewSanadDetailCoverBusiness5;
						viewSanadDetailCoverBusiness6.CreditTafsilGuid = await item5.GetCreditTafsilGuidAsync(token, tranName);
						viewSanadDetailCoverBusiness5.SanadDesc = Description;
						viewSanadDetailCoverBusiness5.RDesc = "چک اشخاص سررسید: " + cheque?.DateSh + " شماره: " + cheque?.Number + " پردازنده: " + tafsil?.Name + " " + item5.RowDesc;
						viewSanadDetailCoverBusiness5.DateMoaser = cheque?.DateM ?? DateM;
						viewSanadDetailCoverBusiness5.DetailGuid = item5.Guid;
						viewSanadDetailCoverBusiness5.Guid = Guid.NewGuid();
						viewSanadDetailCoverBusiness5.DecimalGuid = null;
						viewSanadDetailCoverBusiness5.DecimalPrice = 0m;
						viewSanadDetailCoverBusiness5.DecimalQt = 0m;
						viewSanadDetailCoverBusiness5.DepotGuid = null;
						viewSanadDetailCoverBusiness5.Modified = DateTime.Now;
						viewSanadDetailCoverBusiness5.Price = 0m;
						viewSanadDetailCoverBusiness5.ProductGuid = null;
						viewSanadDetailCoverBusiness5.Qt = 0m;
						viewSanadDetailCoverBusiness5.Qt2 = 0m;
						viewSanadDetailCoverBusiness5.RBazaryab = 0m;
						viewSanadDetailCoverBusiness5.RDiscount = 0m;
						viewSanadDetailCoverBusiness5.Serial = cheque?.Number ?? "";
						viewSanadDetailCoverBusiness5.Status = true;
						viewSanadDetailCoverBusiness5.VahedGuid = null;
						viewSanadDetailCoverBusiness5.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness5.TafsilGuid3 = TafsilGuid3;
						list2.Add(viewSanadDetailCoverBusiness5);
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = MoeinGuid,
							DebitTafsilGuid = TafsilGuid,
							Date = DateM,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtChAshkhas,
							AsTypeDocument = Document.Pardakht,
							Debit = 0m,
							Credit = item5.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						viewSanadDetailCoverBusiness5 = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness5.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness6 = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness6.CreditMoeinGuid = await item5.GetCreditMoeinGuidAsync(token, tranName);
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = await item5.GetCreditTafsilGuidAsync(token, tranName);
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = item5.RowDesc;
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
						viewSanadDetailCoverBusiness2.Serial = cheque?.Number;
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = null;
						list2.Add(viewSanadDetailCoverBusiness2);
					}
				}
				if (Bank?.Any() ?? false)
				{
					foreach (PardakhtBankBusiness item6 in Bank)
					{
						TafsilBusiness tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, item6.TafsilGuid, tranName, token, "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
						List<ViewSanadDetailCoverBusiness> list2 = list;
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = MoeinGuid,
							DebitTafsilGuid = TafsilGuid,
							Date = DateM,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtBank,
							AsTypeDocument = Document.Pardakht,
							Debit = item6.Price,
							Credit = 0m,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						ViewSanadDetailCoverBusiness viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
						viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness.CreditMoeinGuid = item6.MoeinGuid;
						viewSanadDetailCoverBusiness.CreditTafsilGuid = item6.TafsilGuid;
						viewSanadDetailCoverBusiness.SanadDesc = Description;
						viewSanadDetailCoverBusiness.RDesc = "حواله بانکی به شماره: " + item6.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item6.DateSh + " " + item6.RowDesc;
						viewSanadDetailCoverBusiness.DateMoaser = DateM;
						viewSanadDetailCoverBusiness.DetailGuid = item6.Guid;
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
						viewSanadDetailCoverBusiness.Serial = item6.Number;
						viewSanadDetailCoverBusiness.Status = true;
						viewSanadDetailCoverBusiness.VahedGuid = null;
						viewSanadDetailCoverBusiness.TafsilGuid2 = TafsilGuid2;
						viewSanadDetailCoverBusiness.TafsilGuid3 = TafsilGuid3;
						list2.Add(viewSanadDetailCoverBusiness);
						list2 = list;
						viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
						{
							DebitMoeinGuid = MoeinGuid,
							DebitTafsilGuid = TafsilGuid,
							Date = item6.DateM,
							AsGuid = Guid,
							AsType = DocumentDet.PardakhtBank,
							AsTypeDocument = Document.Pardakht,
							Debit = 0m,
							Credit = item6.Price,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
						};
						viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
						viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
						viewSanadDetailCoverBusiness2.CreditMoeinGuid = item6.MoeinGuid;
						viewSanadDetailCoverBusiness2.CreditTafsilGuid = item6.TafsilGuid;
						viewSanadDetailCoverBusiness2.SanadDesc = Description;
						viewSanadDetailCoverBusiness2.RDesc = item6.RowDesc;
						viewSanadDetailCoverBusiness2.DateMoaser = DateM;
						viewSanadDetailCoverBusiness2.DetailGuid = item6.Guid;
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
						viewSanadDetailCoverBusiness2.Serial = item6.Number;
						viewSanadDetailCoverBusiness2.Status = true;
						viewSanadDetailCoverBusiness2.VahedGuid = null;
						list2.Add(viewSanadDetailCoverBusiness2);
						if ((item6?.WageGuid ?? Guid.Empty) != Guid.Empty && item6.WagePrice != 0m)
						{
							list2 = list;
							viewSanadDetailCoverBusiness = new ViewSanadDetailCoverBusiness(ContextGuid)
							{
								DebitMoeinGuid = item6.WageMoeinGuid.Value,
								DebitTafsilGuid = item6.WageTafsilGuid,
								Date = DateM,
								AsGuid = Guid,
								AsType = DocumentDet.PardakhtBank,
								AsTypeDocument = Document.Pardakht,
								Debit = item6.WagePrice,
								Credit = 0m,
								CashierGuid = UserGuid,
								SanadNo = SanadNo,
								SanadSt = SanadSt
							};
							viewSanadDetailCoverBusiness2 = viewSanadDetailCoverBusiness;
							viewSanadDetailCoverBusiness2.Id2 = await GetId2Async(token, tranName);
							viewSanadDetailCoverBusiness.CreditMoeinGuid = item6.MoeinGuid;
							viewSanadDetailCoverBusiness.CreditTafsilGuid = item6.TafsilGuid;
							viewSanadDetailCoverBusiness.SanadDesc = Description;
							viewSanadDetailCoverBusiness.RDesc = "کسر کارمزد حواله بانکی به شماره: " + item6.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item6.DateSh + " " + item6.RowDesc;
							viewSanadDetailCoverBusiness.DateMoaser = DateM;
							viewSanadDetailCoverBusiness.DetailGuid = item6.Guid;
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
							viewSanadDetailCoverBusiness.Serial = item6.Number;
							viewSanadDetailCoverBusiness.Status = true;
							viewSanadDetailCoverBusiness.VahedGuid = null;
							list2.Add(viewSanadDetailCoverBusiness);
							list2 = list;
							viewSanadDetailCoverBusiness2 = new ViewSanadDetailCoverBusiness(ContextGuid)
							{
								DebitMoeinGuid = item6.WageMoeinGuid.Value,
								DebitTafsilGuid = item6.WageTafsilGuid,
								Date = item6.DateM,
								AsGuid = Guid,
								AsType = DocumentDet.PardakhtBank,
								AsTypeDocument = Document.Pardakht,
								Debit = 0m,
								Credit = item6.WagePrice,
								CashierGuid = UserGuid,
								SanadNo = SanadNo,
								SanadSt = SanadSt
							};
							viewSanadDetailCoverBusiness = viewSanadDetailCoverBusiness2;
							viewSanadDetailCoverBusiness.Id2 = await GetId2Async(token, tranName);
							viewSanadDetailCoverBusiness2.CreditMoeinGuid = item6.MoeinGuid;
							viewSanadDetailCoverBusiness2.CreditTafsilGuid = item6.TafsilGuid;
							viewSanadDetailCoverBusiness2.SanadDesc = Description;
							viewSanadDetailCoverBusiness2.RDesc = "کسر کارمزد حواله بانکی به شماره: " + item6.Number + " بانک: " + tafsil?.Name + " تاریخ: " + item6.DateSh + " " + item6.RowDesc;
							viewSanadDetailCoverBusiness2.DateMoaser = DateM;
							viewSanadDetailCoverBusiness2.DetailGuid = item6.Guid;
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
							viewSanadDetailCoverBusiness2.Serial = item6.Number;
							viewSanadDetailCoverBusiness2.Status = true;
							viewSanadDetailCoverBusiness2.VahedGuid = null;
							viewSanadDetailCoverBusiness2.TafsilGuid2 = TafsilGuid2;
							viewSanadDetailCoverBusiness2.TafsilGuid3 = TafsilGuid3;
							list2.Add(viewSanadDetailCoverBusiness2);
						}
					}
				}
				List<Sanad_DetBusiness> lstEnteghatNew = new List<Sanad_DetBusiness>();
				if (Enteghal?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item2 in Enteghal)
					{
						lstEnteghatNew.Add(item2);
						Sanad_DetBusiness temp = new Sanad_DetBusiness(ContextGuid, system, item2.Guid, Document.Pardakht, NextOrdinal(token));
						await SanadDetMapper.Instance.MapAsync(ContextGuid, item2, temp, tranName, token);
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
							AsType = DocumentDet.PardakhtEnteghal,
							AsTypeDocument = Document.Pardakht,
							Debit = item2.Debit,
							Credit = item2.Credit,
							CashierGuid = UserGuid,
							SanadNo = SanadNo,
							SanadSt = SanadSt
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
				WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3017);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		string key = "UpdatePardakht";
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((await LocalSettingBusiness.GetAsync(contextGuid, key, awaitOnSemaphore: true, token))?.ParseToBoolean() ?? token.IsCancellationRequested)
			{
				return res;
			}
			IEnumerable<PardakhtReportBusiness> enumerable = await GetAllAsync(contextGuid, system, new KhazaneFilter
			{
				HasSumNaghd = true
			}, token);
			if (enumerable == null || !enumerable.Any())
			{
				await LocalSettingBusiness.SaveAsync(contextGuid, key, true.ToString(), awaitOnSemaphore: true, 10);
				return res;
			}
			foreach (PardakhtReportBusiness item in enumerable)
			{
				PardakhtBusiness pardakhtBusiness = await GetAsync(contextGuid, item.Guid, fromDatabase: true, token);
				if (!(pardakhtBusiness?.Naghd?.Any()).GetValueOrDefault())
				{
					continue;
				}
				IEnumerable<PardakhtNaghdBusiness> enumerable2 = pardakhtBusiness.Naghd.Where((PardakhtNaghdBusiness q) => q.MoeinGuid != ParentDefaults.MoeinCoding.CLSMoein7);
				if (enumerable2 == null || !enumerable2.Any())
				{
					continue;
				}
				foreach (PardakhtNaghdBusiness item2 in enumerable2)
				{
					item2.MoeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.SaveAsync(system, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, "", token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3044);
			}
			if (!res.HasError)
			{
				await LocalSettingBusiness.SaveAsync(contextGuid, key, true.ToString(), awaitOnSemaphore: true, 10);
			}
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3048);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3049);
			res.AddReturnedValue(ex2, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3049);
		}
		return res;
	}

	public static Task<ReturnedSaveFuncInfo> UpdateCustomerFullNameAsync(string contextGuid, Guid tafsilGuid, string name, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.UpdateCustomerFullNameAsync(tafsilGuid, name, token, tranName);
	}

	private Task Det_OnPriceChange(object sender, EventArgs e)
	{
		try
		{
			if (this.OnSumChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnSumChanged, this, new EventArgs(), default(CancellationToken), 60, "Det_OnPriceChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnPriceChange", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3061);
			throw;
		}
		return Task.CompletedTask;
	}

	public static async Task<List<SourceForDisplay>> GetAllExistingSourceAsync(string contextGuid, CancellationToken token, string tranName = "")
	{
		try
		{
			List<ENSource> list = await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.GetAllExistingSourceAsync(token, tranName);
			if (list == null || !list.Any())
			{
				return null;
			}
			return list.Select((ENSource q) => new SourceForDisplay(q)).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllExistingSourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3074);
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
			WebErrorLog.Instance.StartLog(ex, "", "ComputeOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3091);
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(string contextGuid, Guid guid, int statusId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs"))
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "PardakhtBusiness/ChangeStatusIdAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3104), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3104);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPardakht.ChangeStatusIdAsync(guid, statusId, token, tranName), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3106);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3103);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3110);
					res.AddReturnedValue(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3111);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3116), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBusiness.cs", 3116);
				}
			}
			return res;
		}
	}
}
