using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Sanad;
using EntityCache.Business.Accounting.Selection;
using EntityCache.Business.Extensions;
using EntityCache.Business.Report;
using EntityCache.Entities;
using EntityCache.Filters;
using EntityCache.Mapping.BusinessMappers.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Access.SanadStatusAccess;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.MetaData;
using PacketParser.EventArgs;
using PacketParser.Filters;
using PacketParser.Records.AnbarGardani;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace EntityCache.Business.Accounting;

public class PrdAnbarGardaniBusiness : IPrdAnbarGardani, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasContext, IChangeIntegrity, IEntityCache, ISyncFromClient2Server<PrdAnbarGardaniBusiness>, IDeliverableEntity, IEditableDelivery, IHasSanadStatusReport
{
	private List<PrdAnbarGardaniDetBusiness> _listAnbarGardaniDet;

	private DateTime _dateM = DateManagment.DateM;

	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private SemaphoreSlim _sendToServerSemaphore = new SemaphoreSlim(1, 1);

	private CancellationTokenSource _sendToken = new CancellationTokenSource();

	private PrdAnbarGardaniBusiness _oldPage;

	private Guid? _kasrDebitTafsilGuid;

	private Guid? _kasrCreditTafsilGuid;

	private Guid? _ezafeDebitTafsilGuid;

	private Guid? _ezafeCreditTafsilGuid;

	private Guid _kasrDebitMoeinGuid = Guid.Empty;

	private Guid _kasrCreditMoeinGuid = Guid.Empty;

	private Guid _ezafeDebitMoeinGuid = Guid.Empty;

	private Guid _ezafeCreditMoeinGuid = Guid.Empty;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	[JsonIgnore]
	public string Time => DateM.ToShortTimeString();

	public DateTime DateM
	{
		get
		{
			return _dateM;
		}
		set
		{
			_dateM = value;
		}
	}

	[JsonIgnore]
	public string DateSh => DateM.MiladiToCalture();

	public DateTime DateSabt { get; set; } = DateTime.Now;


	public int Number { get; set; }

	public SanadSt SanadSt { get; set; }

	public Guid? KasrDebitTafsilGuid
	{
		get
		{
			if ((!_kasrDebitTafsilGuid.HasValue || _kasrDebitTafsilGuid == Guid.Empty) && Guid == Guid.Empty)
			{
				_kasrDebitTafsilGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.KosoratDebitTafsilGuid;
			}
			if (_kasrDebitTafsilGuid == Guid.Empty)
			{
				_kasrDebitTafsilGuid = null;
			}
			return _kasrDebitTafsilGuid;
		}
		set
		{
			_kasrDebitTafsilGuid = value;
		}
	}

	public Guid KasrDebitMoeinGuid
	{
		get
		{
			if (_kasrDebitMoeinGuid == Guid.Empty)
			{
				_kasrDebitMoeinGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.KosoratDebitMoeinGuid;
			}
			return _kasrDebitMoeinGuid;
		}
		set
		{
			_kasrDebitMoeinGuid = value;
		}
	}

	public Guid? KasrCreditTafsilGuid
	{
		get
		{
			if ((!_kasrCreditTafsilGuid.HasValue || _kasrCreditTafsilGuid == Guid.Empty) && Guid == Guid.Empty)
			{
				_kasrCreditTafsilGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.KosoratCreditTafsilGuid;
			}
			if (_kasrCreditTafsilGuid == Guid.Empty)
			{
				_kasrCreditTafsilGuid = null;
			}
			return _kasrCreditTafsilGuid;
		}
		set
		{
			_kasrCreditTafsilGuid = value;
		}
	}

	public Guid KasrCreditMoeinGuid
	{
		get
		{
			if (_kasrCreditMoeinGuid == Guid.Empty)
			{
				_kasrCreditMoeinGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.KosoratCreditMoeinGuid;
			}
			return _kasrCreditMoeinGuid;
		}
		set
		{
			_kasrCreditMoeinGuid = value;
		}
	}

	public Guid? EzafeDebitTafsilGuid
	{
		get
		{
			if ((!_ezafeDebitTafsilGuid.HasValue || _ezafeDebitTafsilGuid == Guid.Empty) && Guid == Guid.Empty)
			{
				_ezafeDebitTafsilGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.EzafatDebitTafsilGuid;
			}
			if (_ezafeDebitTafsilGuid == Guid.Empty)
			{
				_ezafeDebitTafsilGuid = null;
			}
			return _ezafeDebitTafsilGuid;
		}
		set
		{
			_ezafeDebitTafsilGuid = value;
		}
	}

	public Guid? EzafeCreditTafsilGuid
	{
		get
		{
			if ((!_ezafeCreditTafsilGuid.HasValue || _ezafeCreditTafsilGuid == Guid.Empty) && Guid == Guid.Empty)
			{
				_ezafeCreditTafsilGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.EzafatCreditTafsilGuid;
			}
			if (_ezafeCreditTafsilGuid == Guid.Empty)
			{
				_ezafeCreditTafsilGuid = null;
			}
			return _ezafeCreditTafsilGuid;
		}
		set
		{
			_ezafeCreditTafsilGuid = value;
		}
	}

	public Guid EzafeDebitMoeinGuid
	{
		get
		{
			if (_ezafeDebitMoeinGuid == Guid.Empty)
			{
				_ezafeDebitMoeinGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.EzafatDebitMoeinGuid;
			}
			return _ezafeDebitMoeinGuid;
		}
		set
		{
			_ezafeDebitMoeinGuid = value;
		}
	}

	public Guid EzafeCreditMoeinGuid
	{
		get
		{
			if (_ezafeCreditMoeinGuid == Guid.Empty)
			{
				_ezafeCreditMoeinGuid = LocalSettingBusiness.GetSetting(ContextGuid).Kardex.EzafatCreditMoeinGuid;
			}
			return _ezafeCreditMoeinGuid;
		}
		set
		{
			_ezafeCreditMoeinGuid = value;
		}
	}

	[JsonIgnore]
	public decimal KasrSum => ListAnbarGardaniDet.Sum((PrdAnbarGardaniDetBusiness p) => Math.Round(p.QtKasr * p.UnitPrice, DigitForRound));

	[JsonIgnore]
	public decimal EzafeSum => ListAnbarGardaniDet.Sum((PrdAnbarGardaniDetBusiness p) => Math.Round(p.QtEzafe * p.UnitPrice, DigitForRound));

	[JsonIgnore]
	public decimal KasrCount => ListAnbarGardaniDet.Sum((PrdAnbarGardaniDetBusiness p) => p.QtKasr);

	[JsonIgnore]
	public decimal EzafeCount => ListAnbarGardaniDet.Sum((PrdAnbarGardaniDetBusiness p) => p.QtEzafe);

	public Guid UserGuid { get; set; }

	public Guid CashierGuid => UserGuid;

	public string Description { get; set; }

	public short StatusId { get; set; }

	public IReadOnlyList<PrdAnbarGardaniDetBusiness> ListAnbarGardaniDet
	{
		get
		{
			_listAnbarGardaniDet = _listAnbarGardaniDet?.Where((PrdAnbarGardaniDetBusiness p) => p != null && !p.IsDisposed)?.ToList() ?? new List<PrdAnbarGardaniDetBusiness>();
			return _listAnbarGardaniDet;
		}
	}

	public string ContextGuid { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public AnbarGardaniMetaData MetaData { get; set; }

	public SanadType Type { get; set; } = SanadType.Movaghat;


	public int SanadNo { get; set; }

	public bool IsTemp { get; set; }

	public Document AsType => Document.AnbarGardani;

	public DateTime? DateUtc { get; set; }

	[JsonIgnore]
	public short DigitForRound { get; }

	public PrdAnbarGardaniBusiness(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			ChangeStatus = EntityChangeSt.New;
			DigitForRound = (ClsCache.GetCache(ContextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 176);
		}
	}

	public async Task<bool> CanInsertAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				bool flag = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess.SingleOrDefault((AnbarGardaniSanadStatusAccess q) => q.StatusId == StatusId)?.Add ?? true;
				return userAccBusiness.UserAccess.AnbarGardani.AnbarGardani_Insert && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanInsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 192);
				throw;
			}
		}
	}

	public async Task<bool> CanDeleteAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				bool flag = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess.SingleOrDefault((AnbarGardaniSanadStatusAccess q) => q.StatusId == StatusId)?.Delete ?? true;
				return userAccBusiness.UserAccess.AnbarGardani.AnbarGardani_Delete && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanDeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 209);
				throw;
			}
		}
	}

	public async Task<bool> CanUpdateAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		using (new JaegerMonitor("CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			_ = 1;
			try
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(ContextGuid, token, tranName);
				UserAccBusiness userAccBusiness = ((cashierBusiness != null) ? (await cashierBusiness.GetUserAsync(token, tranName)) : null);
				bool flag = userAccBusiness.UserAccess.AnbarGardani.SanadStatusAccess.SingleOrDefault((AnbarGardaniSanadStatusAccess q) => q.StatusId == StatusId)?.Edit ?? true;
				return userAccBusiness.UserAccess.AnbarGardani.AnbarGardani_Update && flag;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CanUpdateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 226);
				throw;
			}
		}
	}

	public async Task<int> GetSanadNumberAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		if (SanadNo <= 0 && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting && Type != SanadType.Eftetahiye)
		{
			SanadNo = await SanadBusiness.NextIdAsync(ContextGuid, token, tranName);
		}
		return SanadNo;
	}

	private async Task<PrdAnbarGardaniBusiness> GetOldPage(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			if (_oldPage == null)
			{
				_oldPage = await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarGardani.GetFromDbAsync(Guid, token, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOldPage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 242);
		}
		return _oldPage;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool transactionHasBeenCreated = false;
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 7;
			try
			{
				ClearListDet();
				ENSource source = ClsCache.GetCache(ContextGuid).Source;
				if (autoTran)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 255), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 255);
				}
				transactionHasBeenCreated = true;
				if (source != ENSource.Android_HesabYar)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: true, this, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 258);
				}
				if (ret.HasError)
				{
					result = ret;
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarGardani.RemoveAsync(Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 260);
					if (ret.HasError)
					{
						result = ret;
					}
					else
					{
						if (source == ENSource.Android_HesabYar)
						{
							goto IL_0576;
						}
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.ViewSanadDetailCover.RemoveArticlesAsync(Guid, Document.AnbarGardani, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 264);
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
							if (sanadBusiness != null)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, tryCount, tranName, token, new Tuple<Document, Guid>(Document.AnbarGardani, Guid)), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 268);
							}
							if (!ret.HasError)
							{
								goto IL_0576;
							}
							result = ret;
						}
					}
				}
				goto IL_086f;
				IL_0576:
				if (system != EnSystem.Transfer)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 273), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 273);
					if (source != ENSource.Android_HesabYar)
					{
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, Document.AnbarGardani, DateM, 10, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 275);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 280);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 281);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 286), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 286);
			}
		}
		return ret;
		IL_086f:
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, short autoFixCount = 0, bool isGenerateSanad = true, bool isTemp = false, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		bool autoTran = string.IsNullOrEmpty(tranName);
		if (autoTran)
		{
			tranName = Guid.NewGuid().ToString();
		}
		bool transactionHasBeenCreated = false;
		ENSource source = ClsCache.GetCache(ContextGuid).Source;
		try
		{
			_ = 14;
			ReturnedSaveFuncInfo result;
			try
			{
				if (source == ENSource.Android_HesabYar && !_listAnbarGardaniDet.Any() && isTemp)
				{
					result = ret;
					goto end_IL_00db;
				}
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "PrdAnbarGardanBusiness/SaveAsync", 300000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 301);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 302), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 302);
						}
						transactionHasBeenCreated = true;
						if (!isTemp)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await CheckValidationAsync(system, isDeleteAction: false, autoFixCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 304);
						}
						if (ret.HasError)
						{
							result = ret;
						}
						else
						{
							if (Number <= 0)
							{
								Number = await NextNumberAsync(ContextGuid, StatusId, token, tranName);
							}
							if (system != EnSystem.Transfer && source != ENSource.Android_HesabYar)
							{
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ChangeIntegrity.UpdateProductsDatabase(ContextGuid, system, isDeleteAction: false, this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 309);
							}
							if (ret.HasError)
							{
								result = ret;
							}
							else
							{
								await SanadStatusReportBusiness.ApplySanadStatusIdAsync(ContextGuid, this, system, token, tranName);
								ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarGardani.SaveAsync(this, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 313);
								if (ret.HasError)
								{
									result = ret;
								}
								else
								{
									if (source == ENSource.Android_HesabYar)
									{
										isGenerateSanad = false;
										Type = SanadType.Movaghat;
									}
									if (Type == SanadType.Eftetahiye)
									{
										isGenerateSanad = false;
									}
									if (!isGenerateSanad || SanadNo == 1)
									{
										goto IL_0af3;
									}
									SanadBusiness sanad = await GenerateSanadAsync(system, token, tranName);
									if (sanad != null)
									{
										if (sanad.ListSanad_Det?.Any() ?? false)
										{
											if (SanadNo <= 0 && ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
											{
												SanadNo = await GetSanadNumberAsync(token, tranName);
											}
											if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
											{
												returnedSaveFuncInfo = ret;
												returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarGardani.SetSanadNoAsync(Guid, SanadNo, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 335);
											}
											sanad.SanadNo = SanadNo;
											List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, tranName);
											returnedSaveFuncInfo = ret;
											returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, dis.Token, true, false, tranName, lst, new Tuple<Document, Guid>(Document.AnbarGardani, Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 338);
										}
										else if (system != EnSystem.SignalR)
										{
											throw new ArgumentException("سند انبارگردانی بدون اقلام");
										}
										goto IL_0af3;
									}
									WebErrorLog.Instance.StartLog("داداش چرا سند نال میدی!!!!", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 326);
									ret.AddError(system, "سند نامعتبر", "sanad", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 327);
									result = ret;
								}
							}
						}
					}
					goto end_IL_00db;
					IL_0af3:
					if (ret.HasError)
					{
						result = ret;
					}
					else
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveSerialsAsync(system, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 345);
						if (!ret.HasError)
						{
							if (system != EnSystem.Transfer)
							{
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: false, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 350), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 350);
								if (source == ENSource.Accounting)
								{
									returnedSaveFuncInfo = ret;
									returnedSaveFuncInfo.AddReturnedValue(await KardexBusiness.FillAsync(ContextGuid, system, Guid, Document.AnbarGardani, DateM, 10, tranName, token), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 352);
								}
							}
							goto end_IL_01e5;
						}
						result = ret;
					}
					goto end_IL_00db;
					end_IL_01e5:;
				}
				goto end_IL_00c5;
				end_IL_00db:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 357);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 358);
				goto end_IL_00c5;
			}
			return result;
			end_IL_00c5:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 363), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 363);
			}
			if (autoTran && !ret.HasError && source != ENSource.Accounting && !isTemp)
			{
				Task.Run(() => Send2ServerAsync(token, system, null));
			}
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SaveSerialsAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<PrdAnbarGardaniDetBusiness> enumerable = ListAnbarGardaniDet?.Where((PrdAnbarGardaniDetBusiness q) => q.SerialHasBeenChanged);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			List<PrdSerialBusiness> serialsForAdd = new List<PrdSerialBusiness>();
			foreach (PrdAnbarGardaniDetBusiness det in enumerable)
			{
				if (det == null)
				{
					continue;
				}
				List<PrdSerialBusiness> serialList = await det.GetSerialsAsync(token, tranName);
				if (serialList?.Any() ?? false)
				{
					serialsForAdd.AddRange(serialList);
				}
				ProductSerialFilter filter = new ProductSerialFilter
				{
					DepotGuid = det.DptGuid,
					ProductGuid = det.PrdGuid
				};
				List<PrdSerialBusiness> list = (await PrdSerialBusiness.GetAllAsync(ContextGuid, filter, token, tranName))?.ToList();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any() && (serialList?.Any() ?? false))
				{
					IEnumerable<PrdSerialBusiness> enumerable2 = list.Where((PrdSerialBusiness q) => !serialList.Select((PrdSerialBusiness q) => q.Serial).Contains(q.Serial));
					if (enumerable2 != null && enumerable2.Any())
					{
						foreach (PrdSerialBusiness item in enumerable2)
						{
							int num = 1;
							if ((item?.DetList?.Any()).GetValueOrDefault())
							{
								num = item.DetList.Max((PrdSerialDetailBusiness q) => q.Ordinal);
							}
							item.AddDet(new PrdSerialDetailBusiness(ContextGuid)
							{
								AsType = Document.AnbarGardani,
								DetailGuid = det.Guid,
								Guid = Guid.NewGuid(),
								IsVorod = false,
								MasterGuid = item.Guid,
								Modified = DateTime.Now,
								Ordinal = checked((short)num),
								SanadDate = DateM
							});
						}
						returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await PrdSerialBusiness.SaveRangeAsync(system, token, enumerable2.ToList(), awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveSerialsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 412);
					}
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await PrdSerialBusiness.SaveRangeAsync(system, token, serialsForAdd, awaitOnSemaphore: true, tranName), ReturnedState.None, "SaveSerialsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 416);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveSerialsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 421);
			res.AddReturnedValue(ex, "", "SaveSerialsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 422);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, short autoFixCount = 0, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (KasrDebitMoeinGuid == Guid.Empty)
			{
				res.AddError("طرف حساب معین بدهکار کسورات انتخاب نشده است", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 431);
			}
			if (KasrCreditMoeinGuid == Guid.Empty)
			{
				res.AddError("طرف حساب معین بستانکار کسورات انتخاب نشده است", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 432);
			}
			if (EzafeDebitMoeinGuid == Guid.Empty)
			{
				res.AddError("طرف حساب معین بدهکار اضافات انتخاب نشده است", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 433);
			}
			if (EzafeCreditMoeinGuid == Guid.Empty)
			{
				res.AddError("طرف حساب معین بستانکار اضافات انتخاب نشده است", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 434);
			}
			bool flag = system == EnSystem.UserInterface && isDeleteAction;
			if (flag)
			{
				flag = !(await CanDeleteAsync(token, tranName));
			}
			if (flag)
			{
				return res.AddError("شما دسترسی به حذف انبارگردانی را ندارید", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 437);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			if (autoFixCount >= 0)
			{
				PrdAnbarGardaniBusiness prdAnbarGardaniBusiness = await GetByNumberAsync(ContextGuid, Number, token, tranName);
				if (prdAnbarGardaniBusiness != null && prdAnbarGardaniBusiness.Guid != Guid)
				{
					Number = await NextNumberAsync(ContextGuid, StatusId, token, tranName);
					returnedSaveFuncInfo = res;
					PrdAnbarGardaniBusiness prdAnbarGardaniBusiness2 = this;
					EnSystem system2 = system;
					bool isDeleteAction2 = isDeleteAction;
					short num = checked((short)(autoFixCount - 1));
					autoFixCount = num;
					returnedSaveFuncInfo.AddReturnedValue(await prdAnbarGardaniBusiness2.CheckValidationAsync(system2, isDeleteAction2, num, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 445);
					return res;
				}
			}
			if (autoFixCount <= 0 && system == EnSystem.UserInterface)
			{
				SanadBusiness oldSanad = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
				if (oldSanad != null && oldSanad.SanadSt == SanadSt.Daemi)
				{
					res.AddError(system, "این برگه به علت دائمی شدن سند غیر قابل ویرایش می باشد", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 453);
				}
				flag = oldSanad != null;
				if (flag)
				{
					flag = await oldSanad.GetSanadNoAsync(token, tranName) == SanadNo;
				}
				if (flag)
				{
					if ((oldSanad?.ListSanad_Det?.Any((Sanad_DetBusiness p) => !(p.AsGuid == Guid) || p.AsType != Document.AnbarGardani)).GetValueOrDefault() && oldSanad.DateM.Date != DateM.Date && !isDeleteAction)
					{
						res.AddError(system, $"تاریخ سند شماره ({SanadNo})\r\n" + ". با برگه جاری قابل تجمیع نمی باشد\r\nتاریخ سند تجمیعی جاری:" + oldSanad.DateSh + "\r\nتاریخ برگه:" + DateM.MiladiToCalture(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 467);
					}
				}
				else
				{
					SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
					if ((sanadBusiness?.ListSanad_Det?.Any((Sanad_DetBusiness p) => !(p.AsGuid == Guid) || p.AsType != Document.AnbarGardani)).GetValueOrDefault() && sanadBusiness.DateM.Date != DateM.Date)
					{
						res.AddError(system, $"تاریخ سند شماره ({SanadNo})\r\n" + ". با برگه جاری قابل تجمیع نمی باشد\r\nتاریخ سند تجمیعی جاری:" + oldSanad.DateSh + "\r\nتاریخ برگه:" + DateM.MiladiToCalture(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 481);
					}
				}
			}
			ProductCompactFilter filter = new ProductCompactFilter
			{
				LoadJustParents = false,
				Status = null
			};
			IEnumerable<ProductCompactBusiness> prdList = await ProductCompactBusiness.GetAllAsync(ContextGuid, system, filter, token, tranName);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(ListAnbarGardaniDet.Select((PrdAnbarGardaniDetBusiness p) => p.CheckValidationAsync(prdList, system, isDeleteAction, token, tranName))), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 493);
			List<ProductsChanges> prdChanges = await GetLstProductsAnbarChangesAsync(system, token, tranName, isDeleteAction);
			CheckProductAmountConfig config = new CheckProductAmountConfig(ContextGuid)
			{
				Date = DateM,
				FactorType = Document.AnbarGardani,
				IsDeleteAction = isDeleteAction,
				IsTempOrder = false,
				System = system
			};
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await prdChanges.CheckPrdAmountAsync(config, token, tranName), ReturnedState.None, "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 503);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 507);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 508);
		}
		return res;
	}

	public async Task<SanadBusiness> GenerateSanadAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 13;
		try
		{
			PrdAnbarGardaniBusiness old = await GetOldPage(token, tranName);
			Guid oldGuid = Guid.NewGuid();
			SanadBusiness sanad2 = await SanadBusiness.GetAsync(ContextGuid, SanadNo, token, tranName);
			if (sanad2 != null)
			{
				oldGuid = sanad2.Guid;
			}
			sanad2 = new SanadBusiness(ContextGuid, old?.SanadNo ?? 0)
			{
				Description = ((Type == SanadType.Eftetahiye) ? "سند افتتاحیه" : $"انبارگردانی ({Number}) {Description}"),
				UserGuid = UserGuid,
				DateM = DateM,
				SanadSt = SanadSt.Movaghat,
				SanadNo = SanadNo,
				Guid = oldGuid,
				SanadType = SanadType.Automatic
			};
			if (KasrSum > 0m)
			{
				Sanad_DetBusiness debitKosorat4 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AnbarGardani, sanad2.NextOrdinal())
				{
					Debit = KasrSum,
					Credit = 0m,
					RowDesc = $"انبارگردانی({Number} {Description})",
					DecimalGuid = Guid.Empty,
					Guid = Guid.NewGuid(),
					Qt = 0m,
					MasterGuid = sanad2.Guid,
					Modified = DateTime.Now,
					Status = true
				};
				await debitKosorat4.SetMoeinGuidAsync(KasrDebitMoeinGuid, token, tranName);
				await debitKosorat4.SetTafsilGuidAsync(KasrDebitTafsilGuid, tranName, token);
				await sanad2.AddToListSanadAsync(debitKosorat4, system, token);
			}
			if (EzafeSum > 0m)
			{
				Sanad_DetBusiness debitKosorat4 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AnbarGardani, sanad2.NextOrdinal())
				{
					Debit = EzafeSum,
					Credit = 0m,
					RowDesc = $"انبارگردانی({Number} {Description})",
					DecimalGuid = Guid.Empty,
					Guid = Guid.NewGuid(),
					Qt = 0m,
					AsType = Document.AnbarGardani,
					Modified = DateTime.Now,
					Status = true
				};
				await debitKosorat4.SetMoeinGuidAsync(EzafeDebitMoeinGuid, token, tranName);
				await debitKosorat4.SetTafsilGuidAsync(EzafeDebitTafsilGuid, tranName, token);
				await sanad2.AddToListSanadAsync(debitKosorat4, system, token);
				if (Type == SanadType.Eftetahiye)
				{
					debitKosorat4.SetOrdinal(system, 1, token);
				}
			}
			if (KasrSum > 0m)
			{
				Sanad_DetBusiness debitKosorat4 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AnbarGardani, sanad2.NextOrdinal())
				{
					Debit = 0m,
					Credit = KasrSum,
					RowDesc = $"انبارگردانی({Number} {Description})",
					DecimalGuid = Guid.Empty,
					Guid = Guid.NewGuid(),
					Qt = 0m,
					MasterGuid = sanad2.Guid,
					Modified = DateTime.Now,
					Status = true
				};
				await debitKosorat4.SetMoeinGuidAsync(KasrCreditMoeinGuid, token, tranName);
				await debitKosorat4.SetTafsilGuidAsync(KasrCreditTafsilGuid, tranName, token);
				await sanad2.AddToListSanadAsync(debitKosorat4, system, token);
			}
			if (EzafeSum > 0m)
			{
				Sanad_DetBusiness debitKosorat4 = new Sanad_DetBusiness(ContextGuid, system, Guid, Document.AnbarGardani, sanad2.NextOrdinal())
				{
					Debit = 0m,
					Credit = EzafeSum,
					RowDesc = $"انبارگردانی({Number} {Description})",
					DecimalGuid = Guid.Empty,
					Guid = Guid.NewGuid(),
					Qt = 0m,
					MasterGuid = sanad2.Guid,
					Modified = DateTime.Now,
					Status = true
				};
				await debitKosorat4.SetTafsilGuidAsync(EzafeCreditTafsilGuid, tranName, token);
				await debitKosorat4.SetMoeinGuidAsync(EzafeCreditMoeinGuid, token, tranName);
				await sanad2.AddToListSanadAsync(debitKosorat4, system, token);
				if (Type == SanadType.Eftetahiye)
				{
					debitKosorat4.SetOrdinal(system, 2, token);
				}
			}
			return sanad2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 613);
			throw;
		}
	}

	public async Task<List<ProductsChanges>> GetLstProductsAnbarChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction)
	{
		using (new JaegerMonitor("GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			List<ProductsChanges> ret = new List<ProductsChanges>();
			try
			{
				foreach (PrdAnbarGardaniDetBusiness item in (await GetOldPage(token, tranName))?.ListAnbarGardaniDet ?? new List<PrdAnbarGardaniDetBusiness>())
				{
					CalcChanges.ComputeAnbarGardaniChange(ContextGuid, item, ret, isNew: false, isDeleteAction);
				}
				foreach (PrdAnbarGardaniDetBusiness item2 in ListAnbarGardaniDet ?? new List<PrdAnbarGardaniDetBusiness>())
				{
					if (!isDeleteAction)
					{
						CalcChanges.ComputeAnbarGardaniChange(ContextGuid, item2, ret, isNew: true, isDeleteAction: false);
					}
				}
				return ret?.Where((ProductsChanges q) => q.qt != 0m)?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetLstProductsAnbarChangesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 634);
				throw;
			}
		}
	}

	public Task<List<HesabChanges>> GetLstHesabChangeAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public Task<List<MoeinChanges>> GetLstMoeinChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public Task<List<TafsilChanges>> GetLstTafsilChangesAsync(EnSystem system, CancellationToken token, string tranName, bool isDeleteAction, params Tuple<Document, Guid>[] autoSanadItemsForSave)
	{
		throw new NotImplementedException();
	}

	public static Task<PrdAnbarGardaniBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetAsync(guid, token, tranName);
	}

	public static Task<PrdAnbarGardaniBusiness> GetByNumberAsync(string contextGuid, int number, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetByNumberAsync(number, token, tranName);
	}

	public static Task<PrdAnbarGardaniBusiness> GetAsync(string contextGuid, int sanadNumber, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetAsync(sanadNumber, token, tranName);
	}

	public static Task<int> NextNumberAsync(string contextGuid, int? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.NextNumberAsync(statusId, token, tranName);
	}

	public Task<int> NextId2Async(short? statusId, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return NextNumberAsync(ContextGuid, statusId, token, tranName);
	}

	public void RemoveFromListDet(Guid detGuid)
	{
		using (new JaegerMonitor("RemoveFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			try
			{
				if (_listAnbarGardaniDet == null)
				{
					_listAnbarGardaniDet = new List<PrdAnbarGardaniDetBusiness>();
				}
				_listAnbarGardaniDet.FirstOrDefault((PrdAnbarGardaniDetBusiness p) => p.Guid == detGuid)?.Dispose();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 661);
				throw;
			}
		}
	}

	public void ClearListDet()
	{
		using (new JaegerMonitor("ClearListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			try
			{
				if (_listAnbarGardaniDet == null)
				{
					_listAnbarGardaniDet = new List<PrdAnbarGardaniDetBusiness>();
				}
				_listAnbarGardaniDet.Clear();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ClearListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 673);
			}
		}
	}

	public PrdAnbarGardaniDetBusiness GetFromListDet(Guid detGuid)
	{
		using (new JaegerMonitor("GetFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			try
			{
				if (_listAnbarGardaniDet == null)
				{
					_listAnbarGardaniDet = new List<PrdAnbarGardaniDetBusiness>();
				}
				return _listAnbarGardaniDet.FirstOrDefault((PrdAnbarGardaniDetBusiness p) => p.Guid == detGuid);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetFromListDet", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 686);
				throw;
			}
		}
	}

	public void AddToDetList(PrdAnbarGardaniDetBusiness item)
	{
		using (new JaegerMonitor("AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			try
			{
				if (item != null)
				{
					if (_listAnbarGardaniDet == null)
					{
						_listAnbarGardaniDet = new List<PrdAnbarGardaniDetBusiness>();
					}
					if (_listAnbarGardaniDet.All((PrdAnbarGardaniDetBusiness p) => p.Guid != item.Guid))
					{
						item.MasterGuid = item.Guid;
						item.AnbarGardaniMaster = this;
						_listAnbarGardaniDet.Add(item);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 706);
				throw;
			}
		}
	}

	public void AddRangeToDetList(List<PrdAnbarGardaniDetBusiness> detList, bool clearList)
	{
		using (new JaegerMonitor("AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			try
			{
				if (detList != null && detList.Any())
				{
					if (clearList)
					{
						_listAnbarGardaniDet?.Clear();
					}
					if (_listAnbarGardaniDet == null)
					{
						_listAnbarGardaniDet = new List<PrdAnbarGardaniDetBusiness>();
					}
					_listAnbarGardaniDet.AddRange(detList);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "AddRangeToDetList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 722);
				throw;
			}
		}
	}

	public static async Task<List<PrdAnbarGardaniReportBusiness>> GetAllAsync(string contextGuid, AnbarGardaniFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			return await ApplyAccessAsync(contextGuid, await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetAllAsync(filter, token, tranName), token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 735);
			throw;
		}
	}

	private static async Task<List<PrdAnbarGardaniReportBusiness>> ApplyAccessAsync(string contextGuid, IEnumerable<PrdAnbarGardaniReportBusiness> items, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			if (cashierBusiness == null)
			{
				return items?.ToList();
			}
			IEnumerable<int> illegalStatus = (await cashierBusiness.GetUserAsync(token)).UserAccess.AnbarGardani.SanadStatusAccess?.Where((AnbarGardaniSanadStatusAccess q) => !q.Show)?.Select((AnbarGardaniSanadStatusAccess q) => q.StatusId);
			if ((items?.Any() ?? false) && (illegalStatus?.Any() ?? false))
			{
				items = items?.Where((PrdAnbarGardaniReportBusiness q) => !illegalStatus.Contains(q.StatusId) || q.StatusId <= 0);
			}
			return items?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 753);
			throw;
		}
	}

	public Task<List<PrdAnbarGardaniDetBusiness>> GetAllProductMergedWithDetListAsync(AnbarGardaniFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return GetAllProductWithDetListAsync(ContextGuid, this, filter, token, tranName);
	}

	public async Task<IEnumerable<PrdAnbarGardaniDetBusiness>> FilterOnListAsync(List<PrdAnbarGardaniDetBusiness> tmpList, string search, Guid groupGuid, Guid depotGuid, bool ezafat, bool kosoorat, bool anbargardaniNashode, bool motabeghat, CancellationToken token)
	{
		using (new JaegerMonitor("FilterOnListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			List<PrdAnbarGardaniDetBusiness> filteredList = new List<PrdAnbarGardaniDetBusiness>();
			try
			{
				IEnumerable<PrdAnbarGardaniDetBusiness> detlisttmp = tmpList.Where((PrdAnbarGardaniDetBusiness p) => depotGuid == Guid.Empty || p.DptGuid == depotGuid);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (groupGuid != Guid.Empty)
				{
					IEnumerable<ProductGroupRelationBusiness> enumerable = await ProductGroupRelationBusiness.GetAllAsync(ContextGuid, null, groupGuid, token);
					if (token.IsCancellationRequested)
					{
						return null;
					}
					List<Guid> validProductGuidList = enumerable?.Select((ProductGroupRelationBusiness q) => q.ProductGuid)?.ToList();
					if (validProductGuidList?.Any() ?? false)
					{
						detlisttmp = detlisttmp?.Where((PrdAnbarGardaniDetBusiness q) => validProductGuidList.Contains(q.PrdGuid));
					}
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				List<string> list = search.SplitString();
				if (list != null && list.Count > 0)
				{
					foreach (string item in list)
					{
						if (token.IsCancellationRequested)
						{
							return null;
						}
						if (string.IsNullOrEmpty(item) || !(item.Trim() != ""))
						{
							continue;
						}
						detlisttmp = detlisttmp.Where((PrdAnbarGardaniDetBusiness p) => p.PrdName.StringComparisons(search) || p.PrdCode.StringComparisons(search) || p.PrdVahedName.StringComparisons(search) || p.PrdGroupName.StringComparisons(search) || p.PrdDepotName.StringComparisons(search) || (p.PrdBarcodeList?.Select((BarcodeBusiness q) => q.BarCode)?.Contains(search)).GetValueOrDefault());
					}
				}
				if (!(detlisttmp?.Any() ?? false))
				{
					return null;
				}
				if (!motabeghat && !anbargardaniNashode && !ezafat && !kosoorat)
				{
					filteredList.AddRange(detlisttmp);
					return filteredList;
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (motabeghat)
				{
					filteredList.AddRange(detlisttmp.Where(delegate(PrdAnbarGardaniDetBusiness q)
					{
						decimal? qtRealAmount = q.QtRealAmount;
						decimal amount = q.Amount;
						return (qtRealAmount.GetValueOrDefault() == amount) & qtRealAmount.HasValue;
					}));
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (anbargardaniNashode)
				{
					filteredList.AddRange(detlisttmp.Where((PrdAnbarGardaniDetBusiness p) => p.QtKasr == 0m && p.QtEzafe == 0m));
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (ezafat && kosoorat)
				{
					filteredList.AddRange(detlisttmp.Where((PrdAnbarGardaniDetBusiness p) => p.QtEzafe > 0m || p.QtKasr > 0m));
					return filteredList;
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (ezafat)
				{
					filteredList.AddRange(detlisttmp.Where((PrdAnbarGardaniDetBusiness p) => p.QtEzafe > 0m));
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (kosoorat)
				{
					filteredList.AddRange(detlisttmp.Where((PrdAnbarGardaniDetBusiness p) => p.QtKasr > 0m));
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FilterOnListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 815);
				throw;
			}
			return filteredList;
		}
	}

	public async Task AddFromAnotherTabAsync(PrdAnbarGardaniBusiness anbargardani, bool invert, bool changeDate, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (changeDate)
			{
				DateM = anbargardani.DateM;
			}
			foreach (PrdAnbarGardaniDetBusiness item in anbargardani.ListAnbarGardaniDet)
			{
				PrdAnbarGardaniDetBusiness prdAnbarGardaniDetBusiness = ListAnbarGardaniDet.FirstOrDefault((PrdAnbarGardaniDetBusiness p) => p.PrdGuid == item.PrdGuid && p.DptGuid == item.DptGuid);
				decimal kasr = (invert ? item.QtEzafe : item.QtKasr);
				decimal ezafe = (invert ? item.QtKasr : item.QtEzafe);
				if (prdAnbarGardaniDetBusiness == null)
				{
					ProductCompactBusiness prd = await ProductCompactBusiness.GetAsync(ContextGuid, item.PrdGuid, system, null, "", token, null);
					DepotBusiness depotBusiness = await DepotBusiness.GetAsync(ContextGuid, item.DptGuid, token);
					prdAnbarGardaniDetBusiness = new PrdAnbarGardaniDetBusiness(ContextGuid)
					{
						AnbarGardaniMaster = this,
						Amount = item.Amount,
						DptGuid = item.DptGuid,
						Guid = Guid.NewGuid(),
						MasterGuid = Guid,
						Modified = DateTime.Now,
						PrdGuid = item.PrdGuid,
						QtRealAmount = item.QtRealAmount,
						QtEzafe = Math.Round(ezafe, 3),
						QtKasr = Math.Round(kasr, 3),
						Status = true,
						PrdCode = (prd?.IdProduct ?? ""),
						PrdName = (prd?.Name ?? ""),
						PrdDepotName = (depotBusiness?.Name ?? ""),
						PrdVahedName = (prd?.VahedName ?? ""),
						PrdGroupName = (prd?.GroupName ?? ""),
						UnitPrice = item.UnitPrice
					};
					AddToDetList(prdAnbarGardaniDetBusiness);
				}
				else
				{
					prdAnbarGardaniDetBusiness.QtKasr = Math.Round(kasr, 3);
					prdAnbarGardaniDetBusiness.QtEzafe = Math.Round(ezafe, 3);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddFromAnotherTabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 865);
			throw;
		}
	}

	public static async Task<PrdAnbarGardaniBusiness> GenerateNewAsync(string contextGuid, Guid? guid = null, string desc = "", DateTime? date = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			PrdAnbarGardaniBusiness item = new PrdAnbarGardaniBusiness(contextGuid);
			if (guid.HasValue)
			{
				item.Guid = guid.Value;
			}
			item.UserGuid = CashierBusiness.GetCurrentCashierGuid(contextGuid);
			if (date.HasValue)
			{
				item.DateM = date.Value;
			}
			else
			{
				item.DateM = DateTime.Now;
			}
			item.Status = true;
			if (!string.IsNullOrEmpty(desc))
			{
				item.Description = desc;
			}
			PrdAnbarGardaniBusiness prdAnbarGardaniBusiness = item;
			prdAnbarGardaniBusiness.Number = await NextNumberAsync(contextGuid, null, token, tranName);
			item.SanadNo = 0;
			await SanadStatusReportBusiness.ApplySanadStatusIdAsync(contextGuid, item, EnSystem.None, token, tranName);
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 887);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 900), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 900);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarGardani.SetSaveResultAsync(item, token, tryCount, tranName), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 902);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 904);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 907);
				ret.AddReturnedValue(ex, "", "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 908);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 913), ReturnedState.None, "SetSaveResultAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 913);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> ResendNotSentsAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "", short maxparalelTask = 20)
	{
		using (new JaegerMonitor("ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 924);
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
					return ret.AddError(EnSystem.SignalR, "ارتباط با سرور برقرار نیست!".GetTranslate(), "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 927);
				}
				if (cache.Source == ENSource.Accounting)
				{
					return ret;
				}
				line = 5;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 931);
				}
				if (cache.CacheRepository.IPrdAnbarGardani == null)
				{
					throw new NullReferenceException("ClsCache.GetCache(contextGuid) .CacheRepository.IPrdAnbarGardani is null");
				}
				List<PrdAnbarGardaniBusiness> source = (await cache.CacheRepository.IPrdAnbarGardani.GetAllNotSentAsync(token, tranName)) ?? new List<PrdAnbarGardaniBusiness>();
				line = 6;
				source = source.Where((PrdAnbarGardaniBusiness w) => !w.IsTemp).ToList();
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 937);
				}
				if (source == null || !source.Any())
				{
					return ret;
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 939);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				int current = 0;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (source != null && source.Any())
				{
					foreach (PrdAnbarGardaniBusiness obj in source)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 945);
						}
						current = checked(current + 1);
						PrdAnbarGardaniBusiness existObj = await GetAsync(contextGuid, obj.Guid, token, tranName);
						if (existObj == null)
						{
							continue;
						}
						if (existObj.ServerStatus == ServerStatusForClient.Delivered || existObj.ServerStatus == ServerStatusForClient.DirectDelivery)
						{
							ret.AddInformation("current status on database is delivered!\r\nEntityType: IPrdAnbarGardani \r\n" + $"EntityGuid:{obj?.Guid ?? Guid.Empty}", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 951);
							continue;
						}
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 956);
						}
						tasks.Add(Task.Run(() => existObj.Send2ServerAsync(token, EnSystem.SignalR, null), token));
						if (tasks.Count >= maxparalelTask)
						{
							Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 961);
							tasks.Remove(t);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 965);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 966);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 969);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 972);
				ret.AddReturnedValue(ex, "", "ResendNotSentsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 973);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask = 20)
	{
		return ResendNotSentsAsync(contextGuid, token, tranName, maxparalelTask);
	}

	public async Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, TaskCompletionSource<ReturnedSaveFuncInfo> transactionDistinityResult, Priority priority = Priority.High)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
		{
			_ = 4;
			try
			{
				if (ClsCache.GetCache(ContextGuid).Source == ENSource.Accounting)
				{
					throw new NotImplementedException("این کد برای حسابداری نباید اجرا شود");
				}
				if (transactionDistinityResult != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await transactionDistinityResult.Task, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 987);
				}
				if (ret.HasError)
				{
					return ret;
				}
				_sendToken?.Cancel();
				_sendToken = new CancellationTokenSource();
				CancellationToken token2 = _sendToken.Token;
				using SemaphoreUtil dis = await _sendToServerSemaphore.GaurdAsync(true, "PrdAnbarGardaniBusiness/Send2ServerAsync", 60000, token, token2);
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 993);
				}
				List<WebPrdAnbarGardani> list = new List<WebPrdAnbarGardani>();
				List<WebPrdAnbarGardani> list2 = list;
				list2.Add(await PrdAnbarGardaniMapper.Instance.MapAsync(ContextGuid, this, new WebPrdAnbarGardani(), "", token));
				List<Packet> lstPacket = WebPrdAnbarGardani.GenerateSaveRequest(list, Guid.NewGuid());
				if (dis.Token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 995);
				}
				using PrdAnbarGardaniWaiter waiter = new PrdAnbarGardaniWaiter(WebContextHolder.GetWebContextHolder(ContextGuid), Guid, dis.Token);
				ClsCache.GetCache(ContextGuid).SendData(lstPacket, priority);
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = await waiter.WaitForSaveResponse("PrdAnbarGardaniWaiter");
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 999);
				if (!ret.HasError && returnedSaveFuncInfoWithValue?.value != null)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SetSaveResultAsync(returnedSaveFuncInfoWithValue.value, token, 10), ReturnedState.None, "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1001);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1003);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1006);
				ret.AddReturnedValue(ex, "", "Send2ServerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1007);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1020), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1020);
				}
				transactionHasBeenCreated = true;
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(ContextGuid).CacheRepository.IPrdAnbarGardani.SetDeliveryAsync(del, token, tryCount, tranName), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1022);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1024);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1027);
				ret.AddReturnedValue(ex, "", "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1028);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, ret.HasError, disposeContext: true, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1033), ReturnedState.None, "SetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1033);
			}
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateUnitPriceAsync(string contextGuid, PrdAnbarGardaniBusiness item, decimal def = 0m, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (item == null)
			{
				return res.AddError("PrdAnbarGardaniBusiness is Null for UpdateUnitPrice", "", "UpdateUnitPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1042);
			}
			if (!(item?.ListAnbarGardaniDet?.Any()).GetValueOrDefault())
			{
				return res;
			}
			foreach (PrdAnbarGardaniDetBusiness det in item.ListAnbarGardaniDet)
			{
				if (det != null && !(det.UnitPrice != 0m))
				{
					det.UnitPrice = ((await KardexBusiness.GetLastRecordsByDateAsync(contextGuid, item.DateM, det.PrdGuid, det.DptGuid, token))?.RemUnitPrice).GetValueOrDefault();
					if (det.UnitPrice <= 0m && def > 0m)
					{
						det.UnitPrice = def;
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateUnitPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1054);
			res.AddReturnedValue(ex, "", "UpdateUnitPriceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1055);
		}
		return res;
	}

	public Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, string tranName)
	{
		try
		{
			List<ViewSanadDetailCoverBusiness> list = new List<ViewSanadDetailCoverBusiness>();
			if (system == EnSystem.Transfer)
			{
				return Task.FromResult(list);
			}
			decimal num = ListAnbarGardaniDet?.Sum((PrdAnbarGardaniDetBusiness q) => q.QtEzafe * q.UnitPrice) ?? 0m;
			decimal num2 = ListAnbarGardaniDet?.Sum((PrdAnbarGardaniDetBusiness q) => q.QtKasr * q.UnitPrice) ?? 0m;
			if (num > 0m)
			{
				list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = EzafeDebitMoeinGuid,
					DebitTafsilGuid = EzafeDebitTafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.AnbarGardani,
					AsTypeDocument = Document.AnbarGardani,
					Debit = Math.Round(num),
					Credit = 0m,
					CashierGuid = UserGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = Number,
					CreditMoeinGuid = EzafeCreditMoeinGuid,
					CreditTafsilGuid = EzafeCreditTafsilGuid,
					SanadDesc = Description,
					RDesc = "",
					DateMoaser = DateM,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = (ListAnbarGardaniDet?.Sum((PrdAnbarGardaniDetBusiness q) => q.QtEzafe) ?? 0m),
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = "",
					Status = true,
					VahedGuid = null
				});
				list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = EzafeDebitMoeinGuid,
					DebitTafsilGuid = EzafeDebitTafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.AnbarGardani,
					AsTypeDocument = Document.AnbarGardani,
					Debit = 0m,
					Credit = Math.Round(num),
					CashierGuid = UserGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = Number,
					CreditMoeinGuid = EzafeCreditMoeinGuid,
					CreditTafsilGuid = EzafeCreditTafsilGuid,
					SanadDesc = Description,
					RDesc = "",
					DateMoaser = DateM,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = (ListAnbarGardaniDet?.Sum((PrdAnbarGardaniDetBusiness q) => q.QtEzafe) ?? 0m),
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = "",
					Status = true,
					VahedGuid = null
				});
			}
			if (num2 > 0m)
			{
				list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = KasrDebitMoeinGuid,
					DebitTafsilGuid = KasrDebitTafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.AnbarGardani,
					AsTypeDocument = Document.AnbarGardani,
					Debit = Math.Round(num2),
					Credit = 0m,
					CashierGuid = UserGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = Number,
					CreditMoeinGuid = KasrCreditMoeinGuid,
					CreditTafsilGuid = KasrCreditTafsilGuid,
					SanadDesc = Description,
					RDesc = "",
					DateMoaser = DateM,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = (ListAnbarGardaniDet?.Sum((PrdAnbarGardaniDetBusiness q) => q.QtKasr) ?? 0m),
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = "",
					Status = true,
					VahedGuid = null
				});
				list.Add(new ViewSanadDetailCoverBusiness(ContextGuid)
				{
					DebitMoeinGuid = KasrDebitMoeinGuid,
					DebitTafsilGuid = KasrDebitTafsilGuid,
					Date = DateM,
					AsGuid = Guid,
					AsType = DocumentDet.AnbarGardani,
					AsTypeDocument = Document.AnbarGardani,
					Debit = 0m,
					Credit = Math.Round(num2),
					CashierGuid = UserGuid,
					SanadNo = SanadNo,
					SanadSt = SanadSt,
					Id2 = Number,
					CreditMoeinGuid = KasrCreditMoeinGuid,
					CreditTafsilGuid = KasrCreditTafsilGuid,
					SanadDesc = Description,
					RDesc = "",
					DateMoaser = DateM,
					DetailGuid = Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = (ListAnbarGardaniDet?.Sum((PrdAnbarGardaniDetBusiness q) => q.QtKasr) ?? 0m),
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = "",
					Status = true,
					VahedGuid = null
				});
			}
			return Task.FromResult(list);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1224);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateTypeAsync(string contextGuid, Guid guid, SanadType type, CancellationToken token = default(CancellationToken), short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "PrdAnbarGardanBusiness/UpdateTypeAsync", 60000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1237);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1238), ReturnedState.None, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1238);
						}
						if (!ret.HasError)
						{
							transactionHasBeenCreated = true;
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.UpdateTypeAsync(guid, type, token, tryCount, tranName), ReturnedState.None, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1241);
							goto end_IL_010d;
						}
						result = ret;
					}
					goto end_IL_0076;
					end_IL_010d:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1245);
				ret.AddReturnedValue(ex, "", "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1246);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1251), ReturnedState.None, "UpdateTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1251);
			}
		}
		return ret;
	}

	public static Task<PrdAnbarGardaniReportBusiness> GetReportAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetReportAsync(guid, token, tranName);
	}

	public static async Task<List<PrdAnbarGardaniReportBusiness>> GetAllMoghayeratInTypeAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			List<PrdAnbarGardaniReportBusiness> list = new List<PrdAnbarGardaniReportBusiness>();
			List<PrdAnbarGardaniReportBusiness> lstEftetah = await GetAllAsync(contextGuid, new AnbarGardaniFilter
			{
				Type = SanadType.Eftetahiye
			}, token);
			if (token.IsCancellationRequested)
			{
				return list;
			}
			List<PrdAnbarGardaniReportBusiness> list2 = await GetAllAsync(contextGuid, new AnbarGardaniFilter
			{
				Type = SanadType.Ekhtetamiye
			}, token);
			if (token.IsCancellationRequested)
			{
				return list;
			}
			if ((lstEftetah?.Count() ?? 0) > 1)
			{
				list.AddRange(lstEftetah);
			}
			if ((list2?.Count() ?? 0) > 1)
			{
				list.AddRange(list2);
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllMoghayeratInTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1272);
			return null;
		}
	}

	public static async Task<PrdAnbarGardaniBusiness> GenerateEkhtetamAsync(string contextGuid, DateTime dateM)
	{
		_ = 2;
		try
		{
			PrdAnbarGardaniBusiness anbarGardani = await GenerateNewAsync(contextGuid, (await LocalSettingBusiness.GetSettingAsync(contextGuid)).MaliYearSettings.AnbarGardaniEkhtetamiyeGuid, "انبارگردانی پایان دوره");
			anbarGardani.KasrDebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoeinProductEnd;
			anbarGardani.KasrDebitTafsilGuid = null;
			anbarGardani.KasrCreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein50;
			anbarGardani.KasrCreditTafsilGuid = null;
			anbarGardani.EzafeCreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoeinProductEnd;
			anbarGardani.EzafeCreditTafsilGuid = null;
			anbarGardani.EzafeDebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoeinProductEnd;
			anbarGardani.EzafeDebitTafsilGuid = null;
			anbarGardani.Type = SanadType.Ekhtetamiye;
			anbarGardani.DateM = dateM;
			List<AnbarGardaniEkhtetamRecord> list = await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetAllEkhtetamRecordsAsync(default(CancellationToken), "");
			if (list == null || !list.Any())
			{
				return anbarGardani;
			}
			foreach (AnbarGardaniEkhtetamRecord item in list)
			{
				PrdAnbarGardaniDetBusiness factory = PrdAnbarGardaniDetBusiness.GetFactory(contextGuid, anbarGardani);
				factory.Amount = item.Amount;
				factory.DptGuid = item.DepotGuid;
				factory.Guid = Guid.NewGuid();
				factory.MasterGuid = anbarGardani.Guid;
				factory.Modified = DateTime.Now;
				factory.ContextGuid = contextGuid;
				factory.PrdCode = item.ProductCode;
				factory.PrdDepotName = item.DepotName;
				factory.PrdGuid = item.ProductGuid;
				factory.PrdName = item.ProductName;
				factory.PrdVahedName = item.UnitName;
				factory.QtRealAmount = default(decimal);
				factory.Status = true;
				factory.UnitPrice = item.UnitPrice;
				anbarGardani.AddToDetList(factory);
			}
			return anbarGardani;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateEkhtetamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1321);
			throw;
		}
	}

	public static async Task<PrdAnbarGardaniBusiness> GenerateResetNegativeBalancesToZeroAsync(string contextGuid, DateTime dateM)
	{
		_ = 2;
		try
		{
			await LocalSettingBusiness.GetSettingAsync(contextGuid);
			PrdAnbarGardaniBusiness anbarGardani = await GenerateNewAsync(contextGuid, null, "صفرکردن موجودی منفی در پایان دوره مالی", dateM);
			anbarGardani.Type = SanadType.Movaghat;
			List<PrdAnbarGardaniDetBusiness> list = (await anbarGardani.GetAllProductMergedWithDetListAsync(new AnbarGardaniFilter
			{
				DateFrom = dateM
			}))?.Where((PrdAnbarGardaniDetBusiness q) => q.Amount < 0m)?.ToList();
			if (list == null || !list.Any())
			{
				return anbarGardani;
			}
			foreach (PrdAnbarGardaniDetBusiness item in list)
			{
				PrdAnbarGardaniDetBusiness factory = PrdAnbarGardaniDetBusiness.GetFactory(contextGuid, anbarGardani);
				factory.Amount = item.Amount;
				factory.DptGuid = item.DptGuid;
				factory.Guid = Guid.NewGuid();
				factory.MasterGuid = anbarGardani.Guid;
				factory.Modified = DateTime.Now;
				factory.ContextGuid = contextGuid;
				factory.PrdCode = item.PrdCode;
				factory.PrdDepotName = item.PrdDepotName;
				factory.PrdGuid = item.PrdGuid;
				factory.PrdName = item.PrdName;
				factory.PrdVahedName = item.PrdVahedName;
				factory.QtRealAmount = default(decimal);
				factory.Status = true;
				factory.UnitPrice = item.UnitPrice;
				anbarGardani.AddToDetList(factory);
			}
			return anbarGardani;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateResetNegativeBalancesToZeroAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1361);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveEftetahAsync(string oldContextGuid, string newContextGuid, DateTime? dateM = null, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(oldContextGuid);
			SanadBusiness sanadEftetahiye = await SanadBusiness.GetAsync(newContextGuid, 1);
			PrdAnbarGardaniBusiness anbarGardani = await GetAsync(oldContextGuid, setting.MaliYearSettings.AnbarGardaniEkhtetamiyeGuid, token);
			if (anbarGardani == null)
			{
				PrdAnbarGardaniReportBusiness prdAnbarGardaniReportBusiness = (await GetAllAsync(oldContextGuid, new AnbarGardaniFilter
				{
					Type = SanadType.Ekhtetamiye
				}, token))?.SingleOrDefault();
				if (prdAnbarGardaniReportBusiness != null)
				{
					anbarGardani = await GetAsync(oldContextGuid, prdAnbarGardaniReportBusiness.Guid, token);
				}
			}
			if (anbarGardani == null)
			{
				return res;
			}
			PrdAnbarGardaniBusiness prdAnbarGardaniBusiness = await PrdAnbarGardaniMapper.Instance.MapAsync(newContextGuid, anbarGardani, new PrdAnbarGardaniBusiness(newContextGuid));
			prdAnbarGardaniBusiness.Number = 0;
			prdAnbarGardaniBusiness.SanadNo = 0;
			prdAnbarGardaniBusiness.KasrDebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein5;
			prdAnbarGardaniBusiness.KasrDebitTafsilGuid = null;
			prdAnbarGardaniBusiness.KasrCreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein5;
			prdAnbarGardaniBusiness.KasrCreditTafsilGuid = null;
			prdAnbarGardaniBusiness.EzafeCreditMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein15;
			prdAnbarGardaniBusiness.EzafeCreditTafsilGuid = null;
			prdAnbarGardaniBusiness.EzafeDebitMoeinGuid = ParentDefaults.MoeinCoding.CLSMoein5;
			prdAnbarGardaniBusiness.EzafeDebitTafsilGuid = null;
			prdAnbarGardaniBusiness.Description = "انبارگردانی ابتدای دوره";
			prdAnbarGardaniBusiness.Type = SanadType.Eftetahiye;
			prdAnbarGardaniBusiness.SanadSt = SanadSt.Daemi;
			if (sanadEftetahiye != null)
			{
				prdAnbarGardaniBusiness.DateM = sanadEftetahiye.DateM;
			}
			else if (dateM.HasValue)
			{
				prdAnbarGardaniBusiness.DateM = dateM.Value;
			}
			foreach (PrdAnbarGardaniDetBusiness item in prdAnbarGardaniBusiness.ListAnbarGardaniDet ?? Array.Empty<PrdAnbarGardaniDetBusiness>())
			{
				decimal qtKasr = item.QtKasr;
				item.QtEzafe = qtKasr;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await prdAnbarGardaniBusiness.SaveAsync(EnSystem.UserInterface, 0, isGenerateSanad: true, isTemp: false, default(CancellationToken), awaitOnSemaphore: true, 10), ReturnedState.None, "SaveEftetahAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1409);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveEftetahAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1413);
			res.AddReturnedValue(ex, "", "SaveEftetahAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1414);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> UpdateDateAsync(string contextGuid, Guid guid, DateTime date, CancellationToken token = default(CancellationToken), bool awaitOnSemaphore = true, string tranName = "")
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
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
				using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(awaitOnSemaphore, "UpdateDateAsync/SaveAsync", 300000, token))
				{
					if (dis.Token.IsCancellationRequested)
					{
						result = ret.AddErrorTokenIsCancelled("", "", "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1427);
					}
					else
					{
						if (autoTran)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 300000, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1428), ReturnedState.None, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1428);
						}
						transactionHasBeenCreated = true;
						if (!ret.HasError)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.UpdateDateAsync(guid, date, token, tranName), ReturnedState.None, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1431);
							goto end_IL_0112;
						}
						result = ret;
					}
					goto end_IL_0076;
					end_IL_0112:;
				}
				goto end_IL_0065;
				end_IL_0076:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1435);
				ret.AddReturnedValue(ex, "", "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1436);
				goto end_IL_0065;
			}
			return result;
			end_IL_0065:;
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1441), ReturnedState.None, "UpdateDateAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1441);
			}
		}
		return ret;
	}

	public void SetId2(int value, CancellationToken token)
	{
		Number = value;
	}

	public static async Task<List<PrdAnbarGardaniDetBusiness>> GetAllProductWithDetListAsync(string contextGuid, PrdAnbarGardaniBusiness master, AnbarGardaniFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.GetAllAsync(master, filter, token, tranName);
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusIdAsync(string contextGuid, Guid guid, int statusId, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs"))
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
					using (SemaphoreUtil dis = await _databaseSemaphore.GaurdAsync(true, "PrdAnbarGardaniBusiness/ChangeStatusIdAsync", 60000, token))
					{
						if (!dis.Token.IsCancellationRequested)
						{
							ReturnedSaveFuncInfo returnedSaveFuncInfo;
							if (autoCommit)
							{
								returnedSaveFuncInfo = res;
								returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1459), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1459);
							}
							transactionHasBeenCreated = true;
							returnedSaveFuncInfo = res;
							returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IPrdAnbarGardani.ChangeStatusIdAsync(guid, statusId, token, tranName), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1461);
							goto end_IL_012c;
						}
						result = res.AddErrorTokenIsCancelled("", "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1458);
						goto end_IL_0095;
						end_IL_012c:;
					}
					goto end_IL_0084;
					end_IL_0095:;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1465);
					res.AddReturnedValue(ex, "", "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1466);
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
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, res.HasError, disposeContext: true, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1471), ReturnedState.None, "ChangeStatusIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\PrdAnbarGardaniBusiness.cs", 1471);
				}
			}
			return res;
		}
	}
}
