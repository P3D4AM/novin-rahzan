using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Extensions;
using EntityCache.Business.Report;
using EntityCache.Entities;
using EntityCache.Interfaces;
using EntityCache.Mapping.BusinessMappers.Sanad;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.Access;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.EventArgs.Sanad;
using PacketParser.Filters;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business;

public class SanadBusiness : ISanad, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, ISanadBusiness, IChangeIntegrity, IHasContext, ISyncFromClient2Server<SanadBusiness>, IDeliverableEntity, IEditableDelivery, IHasSanadStatusReport
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static BaseLocks _sanadNoSemaphores = new BaseLocks();

	private bool _isDisposed;

	private SanadBusiness _oldSanad;

	private List<Sanad_DetBusiness> listSanadDet;

	private DateTime? dateM;

	private int _oldSanadNumber;

	private SemaphoreSlim oldSanadGetSemaphore = new SemaphoreSlim(1, 1);

	private bool OldSanadGet;

	private EntityChangeSt changeStatus;

	private Guid guid = Guid.NewGuid();

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private List<SanadAttchmentModel> _attachmentList;

	public EntityChangeSt ChangeStatus
	{
		get
		{
			if (changeStatus == EntityChangeSt.None)
			{
				changeStatus = EntityChangeSt.New;
			}
			return changeStatus;
		}
		set
		{
			if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
			{
				if (value != EntityChangeSt.New && (value != EntityChangeSt.Edited || _oldSanadNumber == 0))
				{
					throw new ArgumentException("please fill oldSanadNumber before set ChangeStatus to edited!");
				}
				changeStatus = value;
			}
			else
			{
				changeStatus = value;
			}
		}
	}

	public ENSource Source { get; set; } = ENSource.Accounting;


	public string ContextGuid { get; set; }

	public IReadOnlyList<Sanad_DetBusiness> ListSanad_Det => listSanadDet?.OrderBy((Sanad_DetBusiness p) => p.Ordinal)?.ToList();

	public int Id { get; set; }

	public DateTime DateM
	{
		get
		{
			return dateM ?? DateTime.Now;
		}
		set
		{
			dateM = value;
		}
	}

	public string DateSh => DateM.MiladiToCalture();

	public short StatusId { get; set; }

	public string Description { get; set; }

	public int OldSanadNumber => _oldSanadNumber;

	public int SanadNo { get; set; }

	public string SanadStName => SanadSt.GetDisplay().GetTranslate();

	public SanadSt SanadSt { get; set; } = SanadSt.Movaghat;


	public SanadType SanadType { get; set; } = SanadType.Movaghat;


	public string SanadTypeName
	{
		get
		{
			if (SanadType == SanadType.Movaghat)
			{
				return "عادی";
			}
			if (SanadType == SanadType.Eftetahiye)
			{
				return "افتتاحیه";
			}
			if (SanadType == SanadType.Ekhtetamiye)
			{
				return "اختتامیه";
			}
			if (SanadType == SanadType.CostAndBenefit)
			{
				return "سود و زیانی";
			}
			return "اتومات";
		}
	}

	public Guid UserGuid { get; set; }

	public Guid CashierGuid => UserGuid;

	public Guid Guid
	{
		get
		{
			return guid;
		}
		set
		{
			guid = value;
			List<Sanad_DetBusiness> list = listSanadDet;
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (Sanad_DetBusiness item in listSanadDet)
			{
				if (item.AsType == Document.Sanad)
				{
					item.AsGuid = guid;
				}
			}
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public bool IsTemp { get; set; }

	public SanadMetaData MetaData { get; set; }

	public bool IsDisposed => _isDisposed;

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public decimal Debit
	{
		get
		{
			using (new JaegerMonitor("Debit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
			{
				decimal result = default(decimal);
				try
				{
					IReadOnlyList<Sanad_DetBusiness> listSanad_Det = ListSanad_Det;
					if (listSanad_Det != null && listSanad_Det.Count > 0)
					{
						foreach (Sanad_DetBusiness item in ListSanad_Det)
						{
							result += item?.Debit ?? 0m;
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Debit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 132);
				}
				return result;
			}
		}
	}

	public decimal Credit
	{
		get
		{
			using (new JaegerMonitor("Credit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
			{
				decimal result = default(decimal);
				try
				{
					IReadOnlyList<Sanad_DetBusiness> listSanad_Det = ListSanad_Det;
					if (listSanad_Det != null && listSanad_Det.Count > 0)
					{
						foreach (Sanad_DetBusiness item in ListSanad_Det)
						{
							result += item?.Credit ?? 0m;
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "Credit", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 148);
				}
				return result;
			}
		}
	}

	public SanadBusiness EventManager => ClsCache.GetCache(ContextGuid).BusinessAggregator.SanadBusiness;

	public Document AsType => Document.Sanad;

	public event Func<object, EventArgs, Task> OnSumChanged_static;

	public event Func<object, EventArgs, Task> OnItemRemoved;

	public event Func<object, EventArgs, Task> OnItemAdded;

	public event Func<object, OrdinalChangedEventArgs, Task> OnOrdinalChanged;

	public SanadBusiness(string contextGuid, int oldSanadNo)
	{
		ContextGuid = contextGuid;
		_oldSanadNumber = oldSanadNo;
		if (_oldSanadNumber == 0)
		{
			ChangeStatus = EntityChangeSt.New;
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.ManualSanad.ManualSanad_Update || (accessLevel.ManualSanad.ManualSanad_Update_Self && CashierGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 172);
			return false;
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
			AccessLevel accessLevel = await cashier.GetUserAccessAsync(token);
			return accessLevel.ManualSanad.ManualSanad_Delete || (accessLevel.ManualSanad.ManualSanad_Delete_Self && CashierGuid == cashier.Guid);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 186);
			return false;
		}
	}

	private async Task<SanadBusiness> GetOldSanadAsync(CancellationToken token, string tranName, EnSystem system, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		_ = 4;
		try
		{
			if (OldSanadGet)
			{
				return _oldSanad;
			}
			using SemaphoreUtil dis = await oldSanadGetSemaphore.GaurdAsync(true, "SanadBusiness/GetOldSand", 60000, token);
			if (dis.Token.IsCancellationRequested)
			{
				return null;
			}
			if (autoSanadItemsForSave?.Any() ?? false)
			{
				foreach (Tuple<Document, Guid> tuple in autoSanadItemsForSave)
				{
					if (tuple != null)
					{
						if (_oldSanad == null)
						{
							_oldSanad = await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.GetFromDbAsync(tuple.Item2, tuple.Item1, token, tranName);
							continue;
						}
						SanadBusiness oldSanad = _oldSanad;
						await oldSanad.JoinAsync(await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.GetFromDbAsync(tuple.Item2, tuple.Item1, token, tranName), system, token);
					}
				}
			}
			else
			{
				_oldSanad = await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.GetFromDbAsync(Guid, Document.Sanad, token, tranName);
			}
			OldSanadGet = true;
			return _oldSanad;
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetOldSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 212);
			throw;
		}
	}

	public async Task<int> GetSanadNoAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (SanadNo <= 0 && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
		{
			return await NextIdAsync(ContextGuid, token, tranName);
		}
		return SanadNo;
	}

	public async Task<bool> GetIsEditAsync(CancellationToken token, string tranName, EnSystem system)
	{
		SanadBusiness sanadBusiness = await GetOldSanadAsync(token, tranName, system);
		return sanadBusiness != null && !(sanadBusiness.Debit <= 0m) && !(sanadBusiness?.IsTemp ?? true);
	}

	public Task<Sanad_DetBusiness> AddToListSanadAsync(Guid asGuid, Document asType, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		return AddToListSanadAsync(new Sanad_DetBusiness(ContextGuid, system, asGuid, asType, NextOrdinal()), system, token);
	}

	public Task<Sanad_DetBusiness> AddToListSanadAsync(Sanad_DetBusiness sanadDetBusiness, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			try
			{
				if (listSanadDet == null)
				{
					listSanadDet = new List<Sanad_DetBusiness>();
				}
				if (listSanadDet.Any((Sanad_DetBusiness p) => p.Guid == sanadDetBusiness.Guid))
				{
					throw new InvalidOperationException("Duplicate det Guid is adding to Sanad items...");
				}
				List<Sanad_DetBusiness> list = listSanadDet;
				int num = ((list != null && list.Any()) ? (listSanadDet?.Max((Sanad_DetBusiness p) => p.Ordinal) ?? 0) : 0);
				listSanadDet.Add(sanadDetBusiness);
				sanadDetBusiness.MasterSanadItem = this;
				if (system == EnSystem.Transfer)
				{
					return Task.FromResult(sanadDetBusiness);
				}
				sanadDetBusiness.OnSumChanged -= Det_OnSumChanged;
				sanadDetBusiness.OnSumChanged += Det_OnSumChanged;
				sanadDetBusiness.OnDispose -= Det_OnDispose;
				sanadDetBusiness.OnDispose += Det_OnDispose;
				sanadDetBusiness.OnOrdinalChanged -= Det_OnOrdinalChanged;
				sanadDetBusiness.OnOrdinalChanged += Det_OnOrdinalChanged;
				sanadDetBusiness.SetOrdinalChange(system, checked(num + 1), throwOnOrdinalChanged: false, token);
				if (system == EnSystem.UserInterface)
				{
					if (this.OnItemAdded != null)
					{
						PacketParser.Utilities.RaiseEvent(this.OnItemAdded, this, new EventArgs(), token, 60, "AddToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
					}
					if (EventManager?.OnSumChanged_static != null)
					{
						PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged_static, this, new EventArgs(), token, 60, "AddToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 260);
			}
			return Task.FromResult(sanadDetBusiness);
		}
	}

	public Task RemoveFromListSanad(Guid guid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			try
			{
				if (listSanadDet == null)
				{
					listSanadDet = new List<Sanad_DetBusiness>();
				}
				listSanadDet.FirstOrDefault((Sanad_DetBusiness p) => p.Guid == guid)?.Dispose(system);
				if (EventManager?.OnSumChanged_static != null)
				{
					return PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged_static, this, new EventArgs(), token, 60, "RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListSanad", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 275);
			}
			return Task.CompletedTask;
		}
	}

	public async Task<string> GetUserNameAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (UserGuid == Guid.Empty)
			{
				return "";
			}
			return (await CashierBusiness.GetAsync(ContextGuid, UserGuid, token, tranName))?.Name ?? "";
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetUserNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 288);
			throw;
		}
	}

	public void ClearListSanad(EnSystem system)
	{
		List<Sanad_DetBusiness> list = listSanadDet;
		if (list != null && list.Any())
		{
			foreach (Sanad_DetBusiness item in listSanadDet.ToList())
			{
				DisposeDet(item, system);
			}
		}
		listSanadDet = new List<Sanad_DetBusiness>();
	}

	public Task AddRangeToListSanadAsync(List<Sanad_DetBusiness> sanaddet, EnSystem system, bool appendCurrentItems = false, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			try
			{
				if (listSanadDet == null)
				{
					listSanadDet = new List<Sanad_DetBusiness>();
				}
				if (!appendCurrentItems)
				{
					ClearListSanad(system);
				}
				if (sanaddet != null && sanaddet.Any())
				{
					foreach (Sanad_DetBusiness item in sanaddet)
					{
						if (item != null)
						{
							item.MasterSanadItem = this;
							item.OnSumChanged -= Det_OnSumChanged;
							item.OnSumChanged += Det_OnSumChanged;
							item.OnDispose -= Det_OnDispose;
							item.OnDispose += Det_OnDispose;
							item.OnOrdinalChanged -= Det_OnOrdinalChanged;
						}
					}
				}
				List<Sanad_DetBusiness> list = listSanadDet;
				int num = ((list != null && list.Any()) ? (listSanadDet?.Max((Sanad_DetBusiness p) => p.Ordinal) ?? 0) : 0);
				if (listSanadDet != null && sanaddet != null && sanaddet.Any())
				{
					foreach (Sanad_DetBusiness row in sanaddet)
					{
						Sanad_DetBusiness sanad_DetBusiness = listSanadDet.SingleOrDefault((Sanad_DetBusiness p) => p.Guid == row.Guid);
						if (sanad_DetBusiness != null)
						{
							if (system == EnSystem.UserInterface)
							{
								throw new InvalidOperationException("sanad already conains exists row Guid\r\n" + $"ExistsDebit:{sanad_DetBusiness.Debit:N0}/ExistsCredit{sanad_DetBusiness.Credit:N0}/ExistsDesc:{sanad_DetBusiness.RowDesc}\r\n" + $"rowDebit:{row.Debit:N0}/RowCredit{row.Credit:N0}/rowDesc:{row.RowDesc}\r\n" + $"Guid:{row.Guid}\r\n" + $"ExistsItem Is refrencedEqual current row item:{sanad_DetBusiness == row}");
							}
							row.Guid = Guid.NewGuid();
						}
						listSanadDet.Add(row);
						if (this.OnItemAdded != null)
						{
							PacketParser.Utilities.RaiseEvent(this.OnItemAdded, this, new EventArgs(), token, 60, "AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
						}
					}
				}
				if (sanaddet != null && sanaddet.Any())
				{
					foreach (Sanad_DetBusiness item2 in sanaddet)
					{
						item2.SetOrdinalChange(system, num = checked(num + 1), throwOnOrdinalChanged: false, token);
					}
					foreach (Sanad_DetBusiness item3 in sanaddet)
					{
						item3.OnOrdinalChanged += Det_OnOrdinalChanged;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToListSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 351);
				throw;
			}
			return Task.CompletedTask;
		}
	}

	public int NextOrdinal()
	{
		try
		{
			List<Sanad_DetBusiness> list = listSanadDet;
			if (list != null && list.Any())
			{
				return checked((listSanadDet?.Max((Sanad_DetBusiness p) => p.Ordinal) ?? (-1)) + 1);
			}
			return 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextOrdinal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 366);
			throw;
		}
	}

	private async Task Det_OnOrdinalChanged(object sender, OrdinalChangedEventArgs e)
	{
		try
		{
			if (!IsDisposed && e.OldOrdinal != e.NewOrdinal && e.OldOrdinal >= 0)
			{
				(listSanadDet?.ToList().FirstOrDefault((Sanad_DetBusiness p) => p != null && p.Ordinal == e.NewOrdinal && p.Guid != ((Sanad_DetBusiness)sender).Guid))?.SetOrdinalChange(e.System, e.OldOrdinal, throwOnOrdinalChanged: true);
				if (this.OnOrdinalChanged != null)
				{
					await PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, this, e, default(CancellationToken), 60, "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnOrdinalChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 386);
		}
	}

	private Task Det_OnDispose(object sender, DisposedEventArgs e)
	{
		try
		{
			if (IsDisposed)
			{
				return Task.CompletedTask;
			}
			Sanad_DetBusiness sanad_DetBusiness = (Sanad_DetBusiness)sender;
			listSanadDet.Remove(sanad_DetBusiness);
			int num = 0;
			foreach (Sanad_DetBusiness item in listSanadDet?.ToList()?.Where((Sanad_DetBusiness p) => p != null && !p.IsDisposed)?.OrderBy((Sanad_DetBusiness p) => p.Ordinal)?.ToList() ?? new List<Sanad_DetBusiness>())
			{
				item.SetOrdinalChange(e.System, num = checked(num + 1), throwOnOrdinalChanged: false);
			}
			if (this.OnItemRemoved != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnItemRemoved, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
			}
			if (EventManager?.OnSumChanged_static != null)
			{
				PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged_static, this, new EventArgs(), default(CancellationToken), 60, "Det_OnDispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
			}
			if (this.OnOrdinalChanged != null)
			{
				PacketParser.Utilities.RaiseEvent(this.OnOrdinalChanged, this, new OrdinalChangedEventArgs(sanad_DetBusiness.Ordinal, -1, sanad_DetBusiness.Guid, e.System), default(CancellationToken), 60, "Det_OnDispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnDispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 408);
			throw;
		}
		return Task.CompletedTask;
	}

	private Task Det_OnSumChanged(object arg1, EventArgs arg2)
	{
		try
		{
			if (IsDisposed)
			{
				return Task.CompletedTask;
			}
			if (EventManager?.OnSumChanged_static != null)
			{
				return PacketParser.Utilities.RaiseEvent(EventManager.OnSumChanged_static, this, new EventArgs(), default(CancellationToken), 60, "Det_OnSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Det_OnSumChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 419);
			throw;
		}
		return Task.CompletedTask;
	}

	public void SetAttachmentModel(List<SanadAttchmentModel> model)
	{
		try
		{
			if (model == null)
			{
				_attachmentList = null;
				return;
			}
			if (_attachmentList == null)
			{
				_attachmentList = new List<SanadAttchmentModel>();
			}
			_attachmentList.AddRange(model);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetAttachmentModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 434);
		}
	}

	public static Task<SanadBusiness> GetAsync(string contextGuid, int SanadNo, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAsync(SanadNo, token, tranName);
	}

	public static Task<SanadBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAsync(guid, token, tranName);
	}

	public static Task<SanadBusiness> GetEkhtetamieSanadAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetEkhtetamieSanadAsync(token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> SanadSortAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		bool autoCommit = string.IsNullOrEmpty(tranName);
		bool transactionHasBeenCreated = false;
		if (autoCommit)
		{
			tranName = Guid.NewGuid().ToString();
		}
		try
		{
			using (await _databaseSemaphore.GaurdAsync(true, "SanadBusiness/SanadSortAsync", 60000, token))
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoCommit)
				{
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 451), ReturnedState.None, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 451);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.SanadSortAsync(token, tranName), ReturnedState.None, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 453);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 457);
			res.AddReturnedValue(ex, "", "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 458);
		}
		if (autoCommit && transactionHasBeenCreated)
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 461), ReturnedState.None, "SanadSortAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 461);
		}
		return res;
	}

	public static Task<SanadSt> GetSanadStBySanadNoAsync(string contextGuid, EnSystem system, int SanadNo, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetSanadStBySanadNoAsync(SanadNo, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeSanadStAsync(string contextGuid, string sanadNOs, SanadSt st, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		using (new JaegerMonitor("ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = $"SanadBusiness_ChangeSanadSt_{Guid.NewGuid()}";
			}
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "SanadBusiness/ChangeSanadStAsync", 60000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 475);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 476), ReturnedState.None, "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 476);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.ChangeSanadStatusAsync(sanadNOs, st, tryCount, token, tranName), ReturnedState.None, "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 478);
							if (!ret.HasError)
							{
								goto end_IL_012a;
							}
							result = ret;
						}
						goto end_IL_0096;
						end_IL_012a:;
					}
					goto end_IL_0085;
					end_IL_0096:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 483);
					ret.AddReturnedValue(ex, "", "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 484);
					goto end_IL_0085;
				}
				return result;
				end_IL_0085:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 489), ReturnedState.None, "ChangeSanadStAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 489);
				}
			}
			return ret;
		}
	}

	public static Task<bool> CheckGhestAsync(string contextGuid, Guid sanadGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.CheckGhestAsync(sanadGuid, token, tranName);
	}

	public static Task<int> CountOfAutoSanadWithDiffrentDateAsync(string contextGuid, int sanadNo, DateTime dateM, Guid asGuid1, short asType1, Guid asGuid2, short asType2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.CountOfAutoSanadWithDiffrentDateAsync(sanadNo, dateM, asGuid1, asType1, asGuid2, asType2, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, CancellationToken token, bool isAutomaticSanad, bool awaitOnSemaphore, string tranName, IEnumerable<ViewSanadDetailCoverBusiness> lst, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = $"SanadBusiness_Save_{Guid.NewGuid()}";
			}
			if (!(autoSanadItemsForSave?.Any() ?? false))
			{
				throw new ArgumentException("please fill list of AsGuid/AsType items.");
			}
			LockItem lockItem = await _sanadNoSemaphores.GetItemAsync(SanadNo.ToString(), "", ClsCache.GetCache(ContextGuid)?.BufferCancellationToken ?? default(CancellationToken), token);
			try
			{
				int num;
				_ = num - 1;
				_ = 10;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await lockItem.Semaphore.GaurdAsync(awaitOnSemaphore, "SanadBusiness/SaveAsync", 300000, token))
					{
						if (dis.Token.IsCancellationRequested)
						{
							result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 508);
						}
						else
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 509), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 509);
							}
							transactionHasBeenCreated = true;
							OldSanadGet = false;
							_oldSanad = null;
							await GetOldSanadAsync(token, tranName, system, autoSanadItemsForSave);
							if (MetaData == null)
							{
								MetaData = new SanadMetaData();
							}
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isAutomaticSanad, tranName, isDeleteAction: false, token, autoSanadItemsForSave), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 516);
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								if (SanadType == SanadType.Note)
								{
									goto IL_0620;
								}
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateAccountDatabase(ContextGuid, system, isDeleteAction: false, this, 10, token, tranName, rollBack: false, autoSanadItemsForSave), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 529);
								if (!ret.HasError)
								{
									goto IL_0620;
								}
								result = ret;
							}
						}
						goto end_IL_01e5;
						IL_0620:
						ENSource source = ClsCache.GetCache(ContextGuid).Source;
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if ((autoSanadItemsForSave?.Any() ?? false) && autoSanadItemsForSave.Count() == 1 && autoSanadItemsForSave.Single().Item1 == Document.Sanad)
							{
								List<Sanad_DetBusiness> removedItems = ListSanad_Det.Where((Sanad_DetBusiness p) => p.AsType != Document.Sanad)?.ToList();
								if (removedItems.Any())
								{
									foreach (Sanad_DetBusiness item in removedItems)
									{
										listSanadDet.Remove(item);
									}
								}
								if (source == ENSource.Accounting || (listSanadDet?.Any() ?? false))
								{
									await SanadStatusReportBusiness.ApplySanadStatusIdAsync(ContextGuid, this, system, token, tranName);
									ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.SaveAsync("SanadBusiness.SaveAsync", this, system, _oldSanadNumber, token, tranName, lst, autoSanadItemsForSave), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 547);
								}
								listSanadDet.AddRange(removedItems);
							}
							else if (source == ENSource.Accounting)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.SaveAsync("SanadBusiness.SaveAsync", this, system, _oldSanadNumber, token, tranName, lst, autoSanadItemsForSave), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 553);
							}
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								if (SanadType == SanadType.Eftetahiye)
								{
									AnbarGardaniFilter filter = new AnbarGardaniFilter
									{
										Type = SanadType.Eftetahiye
									};
									PrdAnbarGardaniReportBusiness prdAnbarGardaniReportBusiness = (await PrdAnbarGardaniBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.SingleOrDefault();
									if (prdAnbarGardaniReportBusiness != null && prdAnbarGardaniReportBusiness.DateM != DateM)
									{
										ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
										returnedSaveFuncInfo.AddReturnedValue(await PrdAnbarGardaniBusiness.UpdateDateAsync(ContextGuid, prdAnbarGardaniReportBusiness.Guid, DateM, token, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 562);
									}
								}
								if ((_attachmentList?.Any() ?? false) && SanadType != SanadType.Note)
								{
									IEnumerable<IGrouping<Document, SanadAttchmentModel>> enumerable = from q in _attachmentList
										group q by q.AsType;
									foreach (IGrouping<Document, SanadAttchmentModel> item2 in enumerable)
									{
										Document key = item2.Key;
										if ((uint)(key - 209) <= 1u)
										{
											ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await DepotReceiptBusiness.UpdateRefrenceGuidAsync(ContextGuid, item2.Select((SanadAttchmentModel q) => q).ToList(), awaitOnSemaphore: false, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 573);
											continue;
										}
										throw new NotImplementedException();
									}
									goto end_IL_02ab;
								}
								result = ret;
							}
						}
						goto end_IL_01e5;
						end_IL_02ab:;
					}
					goto end_IL_01cf;
					end_IL_01e5:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "Error on save sanad2//", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 581);
					ret.AddReturnedValue(new ReturnedSaveFuncInfo(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 582), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 582);
					throw;
				}
				return result;
				end_IL_01cf:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 588), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 588);
				}
				if (autoCommit && !ret.HasError && ClsCache.GetCache(ContextGuid).Source != ENSource.Accounting)
				{
					Task.Run(() => Send2ServerAsync(token, system, null));
				}
				if (!ret.HasError && ClsCache.GetCache(ContextGuid).BufferEnable && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
				{
					Task.Run(() => SendEntitiesToServerAsync(token));
				}
			}
			return ret;
		}
	}

	private async Task SendEntitiesToServerAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			await Task.Delay(500);
			await Task.WhenAll<ReturnedSaveFuncInfo>(KolBusiness.ResendNotSents(ContextGuid, token, "", 5), MoeinBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), TafsilBusiness.ResendNotSentsAsync(ContextGuid, token, "", 20), AccountBusiness.ResendNotSentsAsync(ContextGuid, EnSystem.None, token, "", 10));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendEntitiesToServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 607);
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isAutomaticSanad, string tranName, bool isDeleteAction, CancellationToken token, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		using (new JaegerMonitor("CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				OldSanadGet = false;
				_oldSanad = null;
				await GetOldSanadAsync(token, tranName, system, autoSanadItemsForSave);
				if (system == EnSystem.Transfer || system == EnSystem.TransferMaliYear)
				{
					return ret;
				}
				CashierBusiness cashier = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName);
				UserAccBusiness userAccBusiness = ((cashier != null) ? (await cashier.GetUserAsync(token, tranName)) : null);
				UserAccBusiness user = userAccBusiness;
				if (UserGuid == Guid.Empty && !isDeleteAction)
				{
					ret.AddError(system, "کاربر صادر کننده سند مشخص نشده است.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 622);
				}
				if (await CashierBusiness.GetAsync(ContextGuid, UserGuid, token, tranName) == null)
				{
					ret.AddError(system, "شناسه کاربر صادر کننده سند معتبر نمیباشد.".GetTranslate() + "\r\n" + $" Guid:{UserGuid}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 624);
				}
				if (string.IsNullOrEmpty(Description) && !isDeleteAction)
				{
					ret.AddError(system, "شرح سند تکمیل نشده است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 625);
				}
				if (Debit != Credit && Math.Abs(Debit - Credit) >= 1m && !isDeleteAction)
				{
					ret.AddError(system, "ستون بدهکار و بستانکار تراز نیست".GetTranslate() + "\r\n" + "جمع ستون بدهکار".GetTranslate() + ":" + $" {Debit:N0}\r\n" + "جمع ستون بستانکار".GetTranslate() + ":" + $" {Credit:N0}\r\n" + "مبلغ اختلاف".GetTranslate() + ":" + $" {Credit - Debit:N0}", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 626);
				}
				if (!isAutomaticSanad && Debit == 0m && system != EnSystem.Transfer && !isDeleteAction)
				{
					ret.AddError(system, "سند با مبلغ صفر قابل ثبت نمیباشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 633);
					if (system != EnSystem.UserInterface)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddError(Json.ToStringJson(await ToWebItemAsync(token), indented: true) ?? "", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 635);
					}
				}
				if (!isDeleteAction && _oldSanad != null && _oldSanad.SanadType != SanadType.Note && SanadType == SanadType.Note)
				{
					ret.AddError("امکان تبدیل سند".GetTranslate() + (" " + _oldSanad.SanadTypeName).GetTranslate() + " " + " به سند یادداشت وجود ندارد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 638);
				}
				AccGlobalSettings obj = await LocalSettingBusiness.GetSettingAsync(ContextGuid, token);
				DateTime maliyearStart = obj.MaliYearSettings.MaliyearStart;
				DateTime maliyearEnd = obj.MaliYearSettings.MaliyearEnd;
				if ((DateM < maliyearStart || DateM > maliyearEnd) && system != EnSystem.Transfer && !isDeleteAction)
				{
					ret.AddError(system, "تاریخ سند".GetTranslate() + "(" + DateM.MiladiToCalture() + ")\r\n" + " در بازه مجاز".GetTranslate() + "(" + maliyearStart.MiladiToCalture() + " تا ".GetTranslate() + maliyearEnd.MiladiToCalture() + ")" + " نیست.".GetTranslate() + "\r\n" + "کاربر گرامی میتوانید از طریق منوی امکانات/تنظیمات برنامه/تاریخ و بازه های زمانی اقدام به تعیین بازه زمانی مجاز جهت صدور سند در سال مالی جاری نمایید.".GetTranslate() + "\r\n" + "بدیهی است که جهت جلوگیری از صدور اسناد با تاریخ های اشتباه؛".GetTranslate() + "\r\n" + "صدور هرگونه سندی در بازه زمانی خارج از محدوده مشخص شده قابل انجام نخواهد بود.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 644);
				}
				if ((ListSanad_Det?.Sum((Sanad_DetBusiness q) => q.Credit) ?? 0m) > 99999999999999m && !isDeleteAction)
				{
					ret.AddError("مبلغ سند بیشتر از حد مجاز است".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 651);
				}
				int num = 1;
				if (ListSanad_Det?.Any() ?? false)
				{
					foreach (Sanad_DetBusiness item in ListSanad_Det)
					{
						ret.AddReturnedValue(item.CheckValidation(system, num, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 656);
						num = checked(num + 1);
					}
				}
				else if (!isDeleteAction)
				{
					ret.AddError("عدم امکان ذخیره سازی سند فاقد اقلام.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 660);
				}
				if (system != EnSystem.Transfer && SanadType != SanadType.Eftetahiye && SanadType != SanadType.Ekhtetamiye)
				{
					List<HesabChanges> changes = await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave);
					if (!(autoSanadItemsForSave?.Any() ?? false))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await changes.CheckAccountAsync(system, ContextGuid, token, tranName, Document.Sanad, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 666);
					}
					else
					{
						foreach (Tuple<Document, Guid> tuple in autoSanadItemsForSave)
						{
							if (!((tuple?.Item2 ?? Guid.Empty) == Guid.Empty))
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await changes.CheckAccountAsync(system, ContextGuid, token, tranName, tuple.Item1, isDeleteAction), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 672);
							}
						}
					}
				}
				if ((SanadType == SanadType.Eftetahiye || SanadType == SanadType.Ekhtetamiye) && system == EnSystem.UserInterface && !isDeleteAction)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await CheckTarazAccountAsync(token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 678);
					if (ret.HasError)
					{
						return ret;
					}
				}
				if (SanadType == SanadType.Ekhtetamiye)
				{
					IEnumerable<Sanad_DetBusiness> enumerable = ListSanad_Det?.Where((Sanad_DetBusiness q) => q.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoeinProductEnd);
					if ((enumerable?.Count() ?? 0) > 1)
					{
						await ret.AddErrorAsync("سند اختتامیه شامل".GetTranslate() + $" {enumerable.Count()}" + " ردیف موجودی کالای پایان دوره".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 685);
					}
					if (ret.HasError)
					{
						return ret;
					}
				}
				if (!isAutomaticSanad && system != EnSystem.Transfer)
				{
					if (await GetIsEditAsync(token, tranName, system))
					{
						if (!(await cashier.GetUserAsync(token, tranName)).UserAccess.ManualSanad.ManualSanad_Update)
						{
							ret.AddError(system, "شما دسترسی اصلاح سند را ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 693);
						}
						if (!user.UserAccess.ManualSanad.ManualSanad_Update && UserGuid != cashier.Guid)
						{
							ret.AddError(system, "شما دسترسی اصلاح سند ثبت شده توسط سایر کاربران را ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 696);
						}
					}
					else if (!user.UserAccess.ManualSanad.ManualSanad_Insert)
					{
						ret.AddError(system, "شما دسترسی صدور سند را ندارید".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 701);
					}
				}
				if (SanadSt == SanadSt.Daemi)
				{
					ret.AddError(system, "سند دائم قابل ویرایش نمیباشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 704);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 708);
				ret.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 709);
				throw;
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> CheckTarazAccountAsync(CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<HesabGroupBusiness> hesabGroupList = await HesabGroupBusiness.GetAllAsync(ContextGuid, null, null, token, tranName);
			IEnumerable<KolBusiness> enumerable = await KolBusiness.GetAllAsync(ContextGuid, "", token, null, null, tranName);
			IEnumerable<Guid> hesabGroupGuidListTaraz = from q in hesabGroupList
				where q.TypeGuid == ParentDefaults.HesabTypeGuids.Darayi || q.TypeGuid == ParentDefaults.HesabTypeGuids.Bedehi || q.TypeGuid == ParentDefaults.HesabTypeGuids.Sarmaye
				select q.Guid;
			IEnumerable<KolBusiness> source = enumerable?.Where((KolBusiness q) => hesabGroupGuidListTaraz.Contains(q.GroupGuid));
			IEnumerable<Guid> hesabGroupGuidListEntezami = from q in hesabGroupList
				where q.TypeGuid == ParentDefaults.HesabTypeGuids.HesabHayeEntezami
				select q.Guid;
			hesabGroupGuidListEntezami = hesabGroupGuidListEntezami.Where((Guid q) => q != ParentDefaults.HesabGroupGuids.HesabSanati && q != ParentDefaults.HesabGroupGuids.Other);
			ReturnedSaveFuncInfoWithValue<List<Sanad_DetBusiness>> returnedSaveFuncInfoWithValue = await RemoveEntezamiAsync(kolGuidListForRemove: (enumerable?.Where((KolBusiness q) => hesabGroupGuidListEntezami.Contains(q.GroupGuid))).Select((KolBusiness q) => q.Guid).ToList(), existDetails: await RemoveKolAsync(ListSanad_Det, source.Select((KolBusiness q) => q.Guid).ToList(), token, tranName), token: token, tranName: tranName);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckTarazAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 735);
			if ((returnedSaveFuncInfoWithValue?.value?.Any()).GetValueOrDefault())
			{
				await res.AddErrorAsync("در سند".GetTranslate() + "  " + SanadTypeName + " " + " فقط باید حساب های ترازنامه ای و انتظامی موازنه وجود داشته باشد ".GetTranslate() + "\r\n" + string.Join("\r\n", returnedSaveFuncInfoWithValue.value.Select((Sanad_DetBusiness q) => $"{q.CodeMoein} - {q.MoeinName}")), "", "CheckTarazAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 736);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckTarazAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 741);
			res.AddReturnedValue(ex, "", "CheckTarazAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 742);
		}
		return res;
	}

	private async Task<List<Sanad_DetBusiness>> RemoveKolAsync(IReadOnlyList<Sanad_DetBusiness> existDetails, List<Guid> kolGuidListForRemove, CancellationToken token, string tranName)
	{
		try
		{
			string contextGuid = ContextGuid;
			CancellationToken? token2 = token;
			IEnumerable<MoeinBusiness> enumerable = await MoeinBusiness.GetAllAsync(contextGuid, "", null, null, token2, null, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return existDetails.ToList();
			}
			List<Guid> moeinGuidList = (from q in enumerable
				where kolGuidListForRemove.Contains(q.KolGuid)
				select q.Guid).ToList();
			if (!(moeinGuidList?.Any() ?? false))
			{
				return existDetails.ToList();
			}
			return existDetails.Where((Sanad_DetBusiness q) => !moeinGuidList.Contains(q.MoeinGuid))?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveKolAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 758);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfoWithValue<List<Sanad_DetBusiness>>> RemoveEntezamiAsync(IReadOnlyList<Sanad_DetBusiness> existDetails, List<Guid> kolGuidListForRemove, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfoWithValue<List<Sanad_DetBusiness>> res = new ReturnedSaveFuncInfoWithValue<List<Sanad_DetBusiness>>();
		try
		{
			string contextGuid = ContextGuid;
			CancellationToken? token2 = token;
			List<Guid> moeinGuidList = (from q in (await MoeinBusiness.GetAllAsync(contextGuid, "", null, null, token2, null, tranName))?.Where((MoeinBusiness q) => kolGuidListForRemove.Contains(q.KolGuid))
				select q.Guid)?.ToList();
			if (!(moeinGuidList?.Any() ?? false))
			{
				return res;
			}
			List<Sanad_DetBusiness> items = existDetails?.Where((Sanad_DetBusiness q) => moeinGuidList.Contains(q.MoeinGuid))?.ToList();
			decimal num = items?.Sum((Sanad_DetBusiness q) => q.Debit) ?? 0m;
			decimal num2 = items?.Sum((Sanad_DetBusiness q) => q.Credit) ?? 0m;
			if (num2 != num)
			{
				await res.AddErrorAsync("مجموع بدهکار حساب های انتظامی".GetTranslate() + " " + $" {num:N0}" + " " + " و مجموع بستانکار آن".GetTranslate() + " " + $" {num2:N0}" + " " + "می باشد که با توجه به عدم تراز بودن آنها، امکان صدور سند وجود ندارد".GetTranslate(), "", "RemoveEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 774);
			}
			res.value = existDetails?.Except(items)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 779);
			res.AddReturnedValue(ex, "", "RemoveEntezamiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 780);
		}
		return res;
	}

	public Task<WebSanad> ToWebItemAsync(CancellationToken token = default(CancellationToken))
	{
		return SanadMapper.Instance.MapAsync(ContextGuid, this, new WebSanad(), "", token);
	}

	public static Task<int> NextIdAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.NextIdAsync(token, tranName);
	}

	public Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return Task.FromResult(0);
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount, string tranName, CancellationToken token, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			bool transactionHasBeenCreated = false;
			if (!(autoSanadItemsForSave?.Any() ?? false))
			{
				throw new ArgumentException("please fill asType/AsGuid list");
			}
			ReturnedSaveFuncInfo ret = await CheckValidationAsync(system, !(autoSanadItemsForSave?.Any((Tuple<Document, Guid> p) => p != null && IsSanad(p.Item1)) ?? false), tranName, isDeleteAction: true, token, autoSanadItemsForSave);
			if (ret.HasError)
			{
				return ret;
			}
			bool autoCommit = string.IsNullOrEmpty(tranName);
			if (autoCommit)
			{
				tranName = $"SanadBusiness_Remove_{Guid.NewGuid()}";
			}
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					if (autoCommit)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 799), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 799);
					}
					transactionHasBeenCreated = true;
					if (SanadType != SanadType.Note)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateAccountDatabase(ContextGuid, system, isDeleteAction: true, this, tryCount, token, tranName, rollBack: false, autoSanadItemsForSave), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 802);
					}
					if (ret.HasError)
					{
						return ret;
					}
					foreach (Tuple<Document, Guid> item in autoSanadItemsForSave)
					{
						if (item != null)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.RemoveAsync(item.Item2, item.Item1, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 808);
							if (ClsCache.GetCache(ContextGuid)?.CacheRepository?.ViewSanadDetailCover != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(item.Item2, item.Item1, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 810);
							}
						}
					}
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 815);
					ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 816);
					throw;
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 821), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 821);
				}
				if (!ret.HasError && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && system != EnSystem.Transfer)
				{
					Task.Run(() => SendEntitiesToServerAsync(token));
				}
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> RemoveDuplicateSanadNoAsync(string contextGuid, CancellationToken token, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = $"RemoveDuplicateSanadNoAsync_{Guid.NewGuid()}";
			}
			try
			{
				_ = 1;
				try
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo;
					if (autoCommit)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 836), ReturnedState.None, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 836);
					}
					transactionHasBeenCreated = true;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.RemoveDuplicateSanadNoAsync(token, tranName), ReturnedState.None, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 838);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 842);
					ret.AddReturnedValue(ex, "", "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 843);
				}
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 848), ReturnedState.None, "RemoveDuplicateSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 848);
				}
			}
			return ret;
		}
	}

	private bool IsSanad(Document sanadType)
	{
		return sanadType == Document.Sanad;
	}

	public static Task<int> GetSanadNoByGuidAsync(string contextGuid, Guid Guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetSanadNoByGuidAsync(Guid, token, tranName);
	}

	public static Task<Guid> GetGuidBySanadNoAsync(string contextGuid, int sanadNo, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetGuidBySanadNoAsync(sanadNo, token, tranName);
	}

	public static Task<bool> IsVSanadDetailAsync(string contextGuid, int SanadNo, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.IsVSanadDetailAsync(SanadNo, token, tranName);
	}

	public static Task<SanadSt> GetSanadSTByGuidTypeAsync(string contextGuid, Guid SanadGuid, Document SanadType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetSanadStByGuidTypeAsync(SanadGuid, SanadType, token, tranName);
	}

	public async Task<string> AutoTitleAsync(ENAction act, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("AutoTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			try
			{
				string text = "";
				switch (act)
				{
				case ENAction.Add:
					text = "صدور".GetTranslate();
					break;
				case ENAction.Delete:
					text = "حذف".GetTranslate();
					break;
				case ENAction.Edit:
					text = "ویرایش".GetTranslate();
					break;
				case ENAction.Report:
					text = "مشاهده".GetTranslate();
					break;
				}
				string text2 = text;
				string translate = " سند حسابداری".GetTranslate();
				int num = await GetSanadNoAsync(token, tranName);
				return text2 + translate + "(" + num + ")" + "صادر شده در تاریخ".GetTranslate() + ":" + DateSh;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AutoTitleAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 876);
				throw;
			}
		}
	}

	public async Task<string> AutoDescriptionAsync(ENAction act, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("AutoDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			try
			{
				return await AutoTitleAsync(act, token, tranName);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AutoDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 889);
				throw;
			}
		}
	}

	public static Task<IEnumerable<SanadReportBusiness>> GetAllReportAsync(string contextGuid, SanadFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAllAsync(filter, token, tranName);
	}

	public static Task<List<SanadDetReportBusiness>> GetAllRoznameSanadDetAsync(string contextGuid, DateTime date1, DateTime date2, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAllRoznameSanadDetAsync(date1, date2, tranName, token);
	}

	public static Task<List<SanadDetRoznameKolBusiness>> GetAllRoznameKolSanadDetAsync(string contextGuid, EnDaftarRoznameReportType type, DateTime date1, DateTime date2, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAllSanadDetRoznameKolAsync(date1, date2, tranName, token);
	}

	public static Task<List<SanadDetRoznameKolBusiness>> GetTestTarazNameAsync(string contextGuid, DateTime date, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetTestTarazNameAsync(date, tranName, token);
	}

	public static Task<List<MoeinTafsilReportModel>> GetMoein2TafsilsRptAsync(string contextGuid, DateTime date1, DateTime date2, Guid moeinGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetMoein2TafsilsRptAsync(date1, date2, moeinGuid, tranName, token);
	}

	public static Task<List<MoeinTafsilReportModel>> GetTafsil2MoeinsRprAsync(string contextGuid, DateTime date1, DateTime date2, Guid tafsilGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetTafsil2MoeinsRptAsync(date1, date2, tafsilGuid, tranName, token);
	}

	public static Task<List<MoeinTafsilReportModel>> GetTafsilTajmeeReportAsync(string contextGuid, DateTime date1, DateTime date2, string moeinCode1, string moeinCode2, string tafsilCode1, string tafsilCode2, string tranName, CancellationToken token = default(CancellationToken))
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetTafsilTajmeeReportAsync(date1, date2, moeinCode1, moeinCode2, tafsilCode1, tafsilCode2, token, tranName);
	}

	public static async Task<List<SanadDetReportBusiness>> GetMoeinTafsilGardeshAsync(string contextGuid, DateTime d1, DateTime d2, Guid tafsilGuid, string moeinCode1, string moeinCode2, string tranName, CancellationToken token = default(CancellationToken))
	{
		try
		{
			List<SanadDetReportBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetMoeinTafsilGardeshAsync(d1, d2, tafsilGuid, moeinCode1, moeinCode2, tranName, token);
			decimal num = default(decimal);
			if (list != null && list.Any())
			{
				foreach (SanadDetReportBusiness item in list.OrderBy((SanadDetReportBusiness p) => p.RowIndex))
				{
					num = (item.Remain = num + item.Credit - item.Debit);
				}
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetMoeinTafsilGardeshAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 923);
			throw;
		}
	}

	public async Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return MoeinChanges.Convert(await GetLstHesabChangeAsync(system, token, tranName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string traName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return TafsilChanges.Convert(await GetLstHesabChangeAsync(system, token, traName, isDeleteAction, autoSanadItemsForSave));
	}

	public async Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		return CalcChanges.ComputeHesabChange(await GetOldSanadAsync(token, tranName, system, autoSanadItemsForSave), this, isDeleteAction, autoSanadItemsForSave);
	}

	public Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		return null;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeSanadNoAsync(string contextGuid, List<int> oldSanadNo, int newSanadNo, string description, DateTime date, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			bool autoCommit = string.IsNullOrEmpty(tranName);
			bool transactionHasBeenCreated = false;
			if (autoCommit)
			{
				tranName = $"SanadBusiness_Save_{Guid.NewGuid()}";
			}
			try
			{
				_ = 2;
				ReturnedSaveFuncInfo result;
				try
				{
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "SanadBusiness/ChangeSanadNoAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 945), ReturnedState.None, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 945);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.ChangeSanadNoAsync(oldSanadNo, newSanadNo, description, date, token, tranName), ReturnedState.None, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 947);
							goto end_IL_012a;
						}
						result = ret.AddErrorTokenIsCancelled("", "", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 944);
						goto end_IL_0096;
						end_IL_012a:;
					}
					goto end_IL_0085;
					end_IL_0096:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "Error on ChangeSanadNoAsync Sanad//", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 951);
					ret.AddReturnedValue(new ReturnedSaveFuncInfo(ex, "", "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 952), ReturnedState.None, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 952);
					goto end_IL_0085;
				}
				return result;
				end_IL_0085:;
			}
			finally
			{
				if (autoCommit && transactionHasBeenCreated)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 957), ReturnedState.None, "ChangeSanadNoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 957);
				}
			}
			return ret;
		}
	}

	public Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public static async Task<List<SanadDetReportBusiness>> GetAllGardeshSanadAsync(string contextGuid, ENHesabLevel level, DateTime? date1 = null, DateTime? date2 = null, long? code1 = 0L, long? code2 = 0L, long? codeMoein = 0L, CancellationToken token = default(CancellationToken), Guid? kolGuid = null, int? maxCount = null, short maxParalelThreads = 20, string tranName = "")
	{
		try
		{
			date1.GetValueOrDefault();
			if (!date1.HasValue)
			{
				DateTime value = new DateTime(1990, 1, 1);
				date1 = value;
			}
			date2.GetValueOrDefault();
			if (!date2.HasValue)
			{
				DateTime value = new DateTime(2999, 1, 1);
				date2 = value;
			}
			List<SanadDetReportBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAllGardeshSanadAsync(level, date1.Value, date2.Value, code1, code2, codeMoein, kolGuid, tranName, maxCount, maxParalelThreads, token);
			list = level switch
			{
				ENHesabLevel.Kol => list?.OrderBy((SanadDetReportBusiness p) => p.CodeKol)?.ThenBy((SanadDetReportBusiness p) => p.RowIndex)?.ToList(), 
				ENHesabLevel.Moein => list?.OrderBy((SanadDetReportBusiness p) => p.CodeMoein)?.ThenBy((SanadDetReportBusiness p) => p.RowIndex)?.ToList(), 
				ENHesabLevel.Tafsil => list?.OrderBy((SanadDetReportBusiness p) => p.CodeTafsil)?.ThenBy((SanadDetReportBusiness q) => q.DateM)?.ThenBy((SanadDetReportBusiness p) => p.RowIndex)?.ToList(), 
				_ => throw new ArgumentException(), 
			};
			SanadDetReportBusiness.UpdateList(list);
			return level switch
			{
				ENHesabLevel.Kol => list?.OrderBy((SanadDetReportBusiness p) => p.CodeKol)?.ThenBy((SanadDetReportBusiness p) => p.RowIndex)?.ToList(), 
				ENHesabLevel.Moein => list?.OrderBy((SanadDetReportBusiness p) => p.CodeMoein)?.ThenBy((SanadDetReportBusiness p) => p.RowIndex)?.ToList(), 
				ENHesabLevel.Tafsil => list?.OrderBy((SanadDetReportBusiness p) => p.CodeTafsil)?.ThenBy((SanadDetReportBusiness q) => q.DateM)?.ThenBy((SanadDetReportBusiness p) => p.RowIndex)?.ToList(), 
				_ => throw new ArgumentException(), 
			};
		}
		catch (TaskCanceledException)
		{
			return new List<SanadDetReportBusiness>();
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllGardeshSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 991);
			throw;
		}
	}

	public static Task<SanadBusiness> GenerateNewWithDefaultsAsync(string contextGuid, string description = "", Guid? guid = null, CancellationToken token = default(CancellationToken), string tranName = "", SanadType? type = null)
	{
		try
		{
			SanadBusiness sanadBusiness = null;
			sanadBusiness = new SanadBusiness(contextGuid, 0)
			{
				ChangeStatus = EntityChangeSt.New,
				Source = ClsCache.GetCache(contextGuid).Source,
				UserGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid),
				DateM = DateTime.Now
			};
			if (!string.IsNullOrEmpty(description))
			{
				sanadBusiness.Description = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				sanadBusiness.Guid = guid.Value;
			}
			else
			{
				sanadBusiness.Guid = Guid.NewGuid();
			}
			if (type.HasValue)
			{
				sanadBusiness.SanadType = type.Value;
			}
			else
			{
				sanadBusiness.SanadType = SanadType.Movaghat;
			}
			return Task.FromResult(sanadBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1020);
			throw;
		}
	}

	internal async Task<ReturnedSaveFuncInfo> JoinAsync(SanadBusiness sanad, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(sanad?.listSanadDet?.Any()).GetValueOrDefault())
			{
				return ret;
			}
			await AddRangeToListSanadAsync(sanad.ListSanad_Det.ToList(), system, appendCurrentItems: true, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "JoinAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1034);
			ret.AddReturnedValue(ex, "", "JoinAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1035);
		}
		return ret;
	}

	public static async Task<SanadBusiness> InverseAsync(string contextGuid, SanadBusiness master, string desc = "", DateTime? date = null, int sanadno = 0, SanadType? type = null, string rowDesc = "")
	{
		try
		{
			CancellationToken token = default(CancellationToken);
			if (master == null)
			{
				throw new ArgumentNullException("master");
			}
			master.ChangeStatus = EntityChangeSt.New;
			SanadBusiness sanadBusiness = await SanadMapper.Instance.MapAsync(contextGuid, master, new SanadBusiness(contextGuid, 0), "", token);
			if (!string.IsNullOrEmpty(desc))
			{
				sanadBusiness.Description = desc;
			}
			if (date.HasValue)
			{
				sanadBusiness.DateM = date.Value;
			}
			if (sanadno != 0)
			{
				sanadBusiness.SanadNo = sanadno;
			}
			if (type.HasValue)
			{
				sanadBusiness.SanadType = type.Value;
			}
			foreach (Sanad_DetBusiness item in sanadBusiness.ListSanad_Det)
			{
				decimal debit = item.Debit;
				decimal credit = item.Credit;
				item.Debit = credit;
				item.Credit = debit;
				if (!string.IsNullOrEmpty(rowDesc))
				{
					item.RowDesc = rowDesc;
				}
			}
			return sanadBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InverseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1065);
			throw;
		}
	}

	[Obsolete("pass system", true)]
	public virtual void Dispose()
	{
		throw new NotImplementedException();
	}

	[Obsolete("pass system", true)]
	public virtual void Dispose(bool disposing)
	{
		throw new NotImplementedException();
	}

	public void Dispose(EnSystem system)
	{
		Dispose(disposing: true, system);
	}

	public void Dispose(bool disposing, EnSystem system)
	{
		try
		{
			if (_isDisposed)
			{
				return;
			}
			_isDisposed = true;
			this.OnItemRemoved = null;
			this.OnOrdinalChanged = null;
			List<Sanad_DetBusiness> list = listSanadDet?.ToList();
			listSanadDet = null;
			if (list == null || !list.Any())
			{
				return;
			}
			foreach (Sanad_DetBusiness item in list)
			{
				DisposeDet(item, system);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1088);
		}
	}

	private void DisposeDet(Sanad_DetBusiness item, EnSystem system)
	{
		if (item != null)
		{
			item.OnDispose -= Det_OnDispose;
			item.OnSumChanged -= Det_OnSumChanged;
			item.OnOrdinalChanged -= Det_OnOrdinalChanged;
		}
		item?.Dispose(system);
	}

	public static Task<SanadBusiness> GetFromDatabaseAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetFromDatabaseAsync(guid, token, tranName);
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
		try
		{
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1111), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1111);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.SetSaveResult(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1113);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1115);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1118);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1119);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1124), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1124);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxparalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1135);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1138);
				}
				if (cache.Source == ENSource.Accounting)
				{
					return ret;
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1142);
				}
				if (cache.CacheRepository.ISanad == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.ISanad is null");
				}
				List<Guid> list = (await cache.CacheRepository.ISanad.GetAllNotSentAsync(token, tranName)) ?? new List<Guid>();
				line = 6;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1147);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1149);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1155);
						}
						current = checked(current + 1);
						SanadBusiness obj = await GetFromDatabaseAsync(contextGuid, item, token, tranName);
						if (obj.ServerStatus == ServerStatusForClient.Delivered || obj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: Sanad \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1161);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1166);
						}
						tasks.Add(Task.Run(() => obj.Send2ServerAsync(token, EnSystem.SignalR, null), token));
						if (tasks.Count >= maxparalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1171);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1175);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1176);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1179);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1182);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1183);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
			_ = 1;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1198), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1198);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ISanad.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1200);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1202);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1205);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1206);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1211), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1211);
			}
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, TaskCompletionSource<ReturnedSaveFuncInfo> transactionDistinityResult, Priority priority = Priority.High)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (transactionDistinityResult != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await transactionDistinityResult.Task, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1222);
				}
				if (ret.HasError)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "SanadBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1228);
				}
				List<WebSanad> list = new List<WebSanad>();
				List<WebSanad> list2 = list;
				list2.Add(await SanadMapper.Instance.MapAsync(ContextGuid, this, new WebSanad(), "", token));
				List<Packet> lstPacket = WebSanad.GenerateSaveRequest(list, Guid.NewGuid());
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1230);
				}
				using SanadWaiter waiter = new SanadWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("SanadWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1234);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1236);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1238);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1241);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1242);
			}
			return ret;
		}
	}

	public Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system)
	{
		try
		{
			List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
			if (system == EnSystem.Transfer)
			{
				return Task.FromResult(list);
			}
			foreach (Sanad_DetBusiness item in ListSanad_Det?.Where((Sanad_DetBusiness q) => q.AsType == Document.Sanad)?.OrderBy((Sanad_DetBusiness q) => q.Ordinal)?.ToList())
			{
				list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = item.MoeinGuid,
					DebitTafsilGuid = item.TafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.SanadHesabdari,
					AsTypeDocument = Document.Sanad,
					Debit = item.Debit,
					Credit = item.Credit,
					CashierGuid = UserGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = SanadNo,
					CreditMoeinGuid = item.MoeinGuid,
					CreditTafsilGuid = item.TafsilGuid,
					SanadDesc = Description,
					RDesc = item.RowDesc,
					DateMoaser = DateM,
					DetailGuid = item.Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = item.DecimalGuid,
					DecimalPrice = ((item.Qt > 0m) ? ((Debit + Credit) / item.Qt) : 0m),
					DecimalQt = item.Qt,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = 0m,
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = item.Serial,
					Status = true,
					VahedGuid = null,
					TafsilGuid2 = item.TafsilGuid2,
					TafsilGuid3 = item.TafsilGuid3,
					TafsilGuid4 = item.TafsilGuid4
				});
			}
			return Task.FromResult(list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1298);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> GetSanadTemporaryAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		try
		{
			List<Sanad_DetBusiness> list = await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetSanadTemporaryAsync(token, tranName);
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			string tranName2 = tranName;
			SanadType? type = SanadType.CostAndBenefit;
			SanadBusiness sanad = await GenerateNewWithDefaultsAsync(contextGuid, "", null, token, tranName2, type);
			foreach (Sanad_DetBusiness item2 in list)
			{
				if (item2.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoeinProductEnd)
				{
					res.AddError("حساب معین موجودی کالای پایان دوره، با تفصیلی برداشت سند خورده است و امکان بستن حساب های موقت وجود ندارد".GetTranslate(), "GetSanadTemporaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1315);
					res.value = null;
					return res;
				}
				decimal debit = item2.Debit;
				decimal credit = item2.Credit;
				item2.Debit = credit;
				item2.Credit = debit;
				item2.AsGuid = sanad.Guid;
				await sanad.AddToListSanadAsync(item2, EnSystem.None, token);
			}
			await sanad.AddDetailAsync(ParentDefaults.MoeinCoding.CLSMoein5, token);
			await sanad.AddDetailAsync(ParentDefaults.MoeinCoding.CLSMoein50, token);
			IEnumerable<KolBusiness> enumerable = await KolBusiness.GetAllAsync(contextGuid, "", token, true, ParentDefaults.HesabGroupGuids.HesabSanati, tranName);
			if (enumerable != null && enumerable.Any())
			{
				List<Guid> kolListGuid = enumerable.Select((KolBusiness q) => q.Guid).ToList();
				CancellationToken? token2 = token;
				bool? status = true;
				tranName2 = tranName;
				IEnumerable<MoeinBusiness> enumerable2 = (await MoeinBusiness.GetAllAsync(contextGuid, "", null, null, token2, status, tranName2))?.Where((MoeinBusiness q) => kolListGuid.Contains(q.KolGuid) && q.Account != 0m);
				if (enumerable2 != null && enumerable2.Any())
				{
					foreach (MoeinBusiness item in enumerable2)
					{
						MoeinBusiness temp = await MoeinBusiness.GetAsync(contextGuid, item.Guid, "", token);
						if (temp == null)
						{
							continue;
						}
						IEnumerable<AccountBusiness> accountList = await AccountBusiness.GetAllAsync(contextGuid, item.Guid, null, true, token, tranName);
						if (!(accountList?.Any() ?? false))
						{
							await sanad.AddDetailAsync(item.Guid, token);
							continue;
						}
						foreach (AccountBusiness account in accountList)
						{
							Sanad_DetBusiness det4 = await sanad.AddToListSanadAsync(new Sanad_DetBusiness(contextGuid, EnSystem.None, sanad.Guid, Document.Sanad, sanad.NextOrdinal()), EnSystem.None, token);
							await det4.SetMoeinGuidAsync(account.MoeinGuid, token);
							await det4.SetTafsilGuidAsync(account.TafsilGuid, tranName, token);
							if (account.Account > 0m)
							{
								det4.Debit = account.Account;
								det4.Credit = 0m;
							}
							else if (account.Account < 0m)
							{
								det4.Debit = 0m;
								det4.Credit = -account.Account;
							}
						}
						decimal num = accountList.Sum((AccountBusiness q) => q.Account);
						if (num != temp.Account)
						{
							decimal moeinAccount = temp.Account - num;
							Sanad_DetBusiness det4 = await sanad.AddToListSanadAsync(new Sanad_DetBusiness(contextGuid, EnSystem.None, sanad.Guid, Document.Sanad, sanad.NextOrdinal()), EnSystem.None, token);
							await det4.SetMoeinGuidAsync(temp.Guid, token);
							if (moeinAccount > 0m)
							{
								det4.Debit = moeinAccount;
								det4.Credit = 0m;
							}
							else if (moeinAccount < 0m)
							{
								det4.Debit = 0m;
								det4.Credit = -moeinAccount;
							}
						}
					}
				}
			}
			decimal debitSum = sanad.ListSanad_Det?.Sum((Sanad_DetBusiness q) => q.Debit) ?? 0m;
			decimal creditSum = sanad.ListSanad_Det?.Sum((Sanad_DetBusiness q) => q.Credit) ?? 0m;
			if (debitSum > creditSum)
			{
				Sanad_DetBusiness det4 = await sanad.AddToListSanadAsync(new Sanad_DetBusiness(contextGuid, EnSystem.None, sanad.Guid, Document.Sanad, sanad.NextOrdinal()), EnSystem.None, token);
				await det4.SetMoeinGuidAsync(ParentDefaults.MoeinCoding.CLSMoein29799, token);
				det4.Debit = 0m;
				det4.Credit = debitSum - creditSum;
			}
			else if (creditSum > debitSum)
			{
				Sanad_DetBusiness det4 = await sanad.AddToListSanadAsync(new Sanad_DetBusiness(contextGuid, EnSystem.None, sanad.Guid, Document.Sanad, sanad.NextOrdinal()), EnSystem.None, token);
				await det4.SetMoeinGuidAsync(ParentDefaults.MoeinCoding.CLSMoein29799, token);
				det4.Debit = creditSum - debitSum;
				det4.Credit = 0m;
			}
			res.value = sanad;
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadTemporaryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1409);
			throw;
		}
	}

	private async Task AddDetailAsync(Guid moeinGuid, CancellationToken token)
	{
		_ = 4;
		try
		{
			MoeinBusiness temp = await MoeinBusiness.GetAsync(ContextGuid, moeinGuid, "", token);
			if (temp.Account > 0m)
			{
				Sanad_DetBusiness det = await AddToListSanadAsync(new Sanad_DetBusiness(ContextGuid, EnSystem.None, Guid, Document.Sanad, NextOrdinal()), EnSystem.None, token);
				await det.SetMoeinGuidAsync(moeinGuid, token);
				det.Debit = temp.Account;
				det.Credit = 0m;
			}
			else if (temp.Account < 0m)
			{
				Sanad_DetBusiness det = await AddToListSanadAsync(new Sanad_DetBusiness(ContextGuid, EnSystem.None, Guid, Document.Sanad, NextOrdinal()), EnSystem.None, token);
				await det.SetMoeinGuidAsync(moeinGuid, token);
				det.Debit = 0m;
				det.Credit = -temp.Account;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1437);
			throw;
		}
	}

	private async Task AddDetailAsync(Guid moeinGuid, Guid? tafsilGuid, decimal account, CancellationToken token, string tranName)
	{
		_ = 2;
		try
		{
			if (!(account == 0m))
			{
				Sanad_DetBusiness det = await AddToListSanadAsync(new Sanad_DetBusiness(ContextGuid, EnSystem.None, Guid, Document.Sanad, NextOrdinal()), EnSystem.None, token);
				await det.SetMoeinGuidAsync(moeinGuid, token, tranName);
				await det.SetTafsilGuidAsync(tafsilGuid, tranName, token);
				if (account > 0m)
				{
					det.Debit = account;
					det.Credit = 0m;
				}
				else if (account < 0m)
				{
					det.Debit = 0m;
					det.Credit = -account;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDetailAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1460);
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<SanadBusiness>> GetSanadBardashAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfoWithValue<SanadBusiness> res = new ReturnedSaveFuncInfoWithValue<SanadBusiness>();
		try
		{
			MoeinBusiness bardashMoein = await MoeinBusiness.GetAsync(contextGuid, ParentDefaults.MoeinCoding.CLSMoein3, tranName, token);
			IEnumerable<TafsilBusiness> bardashtList = await TafsilBusiness.GetAllAsync(contextGuid, EnSystem.None, new TafsilFilter
			{
				HesabType = HesabType.Bardasht
			}, token, tranName, "GetSanadBardashAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs");
			List<Guid> bardashtGuidList = bardashtList?.Select((TafsilBusiness q) => q.Guid)?.ToList();
			SanadType? type = SanadType.CostAndBenefit;
			SanadBusiness sanad = await GenerateNewWithDefaultsAsync(contextGuid, "", null, token, tranName, type);
			IEnumerable<AccountBusiness> allAccounts = await AccountBusiness.GetAllAsync(contextGuid, bardashtList?.Select((TafsilBusiness q) => q.Guid)?.ToList(), true, token, tranName);
			if (bardashtGuidList?.Any() ?? false)
			{
				foreach (Guid guid in bardashtGuidList)
				{
					if (guid == Guid.Empty)
					{
						continue;
					}
					IEnumerable<AccountBusiness> enumerable = allAccounts?.Where((AccountBusiness q) => q.TafsilGuid == guid && q.Account != 0m);
					if (enumerable == null || !enumerable.Any())
					{
						continue;
					}
					foreach (AccountBusiness item in enumerable)
					{
						if (!((item?.Account ?? 0m) <= 0m))
						{
							await sanad.AddDetailAsync(item.MoeinGuid, item.TafsilGuid, item.Account, token, tranName);
						}
					}
				}
			}
			if ((bardashMoein?.Account ?? 0m) != 0m)
			{
				IEnumerable<AccountBusiness> bardashtAccount = (await AccountBusiness.GetAllAsync(contextGuid, bardashMoein.Guid, null, true, token, tranName))?.Where((AccountBusiness q) => !bardashtGuidList.Contains(q.TafsilGuid));
				if (bardashtAccount?.Any() ?? false)
				{
					foreach (AccountBusiness item2 in bardashtAccount)
					{
						await sanad.AddDetailAsync(item2.MoeinGuid, item2.TafsilGuid, item2.Account, token, tranName);
					}
				}
				decimal account = bardashMoein.Account - ((allAccounts?.Where((AccountBusiness q) => q.MoeinGuid == bardashMoein.Guid)?.Sum((AccountBusiness q) => q.Account)).GetValueOrDefault() + (bardashtAccount?.Sum((AccountBusiness q) => q.Account) ?? 0m));
				await sanad.AddDetailAsync(bardashMoein.Guid, null, account, token, tranName);
			}
			if (!(sanad?.ListSanad_Det?.Any()).GetValueOrDefault())
			{
				return res;
			}
			decimal num = sanad.ListSanad_Det.Sum((Sanad_DetBusiness q) => q.Debit);
			decimal num2 = sanad.ListSanad_Det.Sum((Sanad_DetBusiness q) => q.Credit);
			if (num > num2)
			{
				await sanad.AddDetailAsync(ParentDefaults.MoeinCoding.CLSMoein15, null, num - num2, token, tranName);
			}
			else if (num2 > num)
			{
				await sanad.AddDetailAsync(ParentDefaults.MoeinCoding.CLSMoein15, null, num2 - num, token, tranName);
			}
			if (sanad.Debit > 0m || sanad.Credit > 0m)
			{
				res.value = sanad;
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadBardashAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1513);
			throw;
		}
	}

	public static Task<IEnumerable<SanadGhestViewModel>> GetAllSanadGhestAsync(string contextGuid, Guid sanadGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.ISanad.GetAllSanadGhestAsync(sanadGuid, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> TransferSanadEftetahiyeAsync(string oldContextGuid, string newContextGuid, string sanadDesc, string rowDesc, DateTime? dateM)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Guid maliyearSanadEKhtetamieGuid = (await LocalSettingBusiness.GetSettingAsync(oldContextGuid)).MaliYearSettings.MaliyearSanadEKhtetamieGuid;
			SanadBusiness master = (await GetAsync(oldContextGuid, maliyearSanadEKhtetamieGuid)) ?? throw new ArgumentNullException("سند اختتامیه NULL شد".GetTranslate());
			SanadBusiness newSanad = await InverseAsync(newContextGuid, master, sanadDesc, dateM, 1, SanadType.Eftetahiye, rowDesc);
			Sanad_DetBusiness sanad_DetBusiness = newSanad?.ListSanad_Det?.SingleOrDefault((Sanad_DetBusiness q) => q.MoeinGuid == ParentDefaults.MoeinCoding.CLSMoeinProductEnd);
			if (sanad_DetBusiness != null)
			{
				await sanad_DetBusiness.SetMoeinGuidAsync(ParentDefaults.MoeinCoding.CLSMoein5);
			}
			List<ViewSanadDetailCoverBusiness> lst = await newSanad.GetSanadCoverListAsync(EnSystem.UserInterface);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await newSanad.SaveAsync(EnSystem.TransferMaliYear, default(CancellationToken), false, true, "", lst, new Tuple<Document, Guid>(Document.Sanad, newSanad.Guid)), ReturnedState.None, "TransferSanadEftetahiyeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1534);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TransferSanadEftetahiyeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1538);
			res.AddReturnedValue(ex, "", "TransferSanadEftetahiyeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1539);
		}
		return res;
	}

	public void SetId2(int value, CancellationToken token)
	{
		SanadNo = value;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(string contextGuid, Guid guid, int statusId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs"))
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "SanadBusiness/ChangeStatusIdAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1555), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1555);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.ISanad.ChangeStatusIdAsync(guid, statusId, token, tranName), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1557);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1554);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1561);
					res.AddReturnedValue(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1562);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1567), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Sanad\\SanadBusiness.cs", 1567);
				}
			}
			return res;
		}
	}
}
