using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
using EntityCache.Business.Accounting.Khazane.Reception;
using EntityCache.Business.Accounting.Sanad;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.MetaData;
using PacketParser.Filters;
using PacketParser.Records;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business;

public class ReceptionCheckBusiness : BaseKhazaneDetail, IReceptionCheck, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasContext
{
	private static SemaphoreSlim _databaseSemaphore = new SemaphoreSlim(1, 1);

	private static SemaphoreSlim _connectionPoolSemaphore = new SemaphoreSlim(1, 10);

	private ReceptionBusiness _reception;

	private Guid? a2re_PardazandeTafsilGuid;

	private Guid? a2re_PardazandeMoeinGuid;

	private TafsilBusiness _tafsil;

	public override KhazaneType Type
	{
		get
		{
			if (!Personal)
			{
				return KhazaneType.ChequeM;
			}
			return KhazaneType.ChequeSh;
		}
	}

	public DateTime Modified { get; set; } = DateTime.Now;


	bool IHasStatus.Status => Status == ENUMCheck.BatelShode;

	public string BankName { get; set; }

	public DateTime Date { get; set; }

	public override DateTime DateM
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

	[JsonIgnore]
	public string DateSh => Date.MiladiToCalture();

	public Guid? VisitorGuid { get; set; }

	public ENUMCheck Status { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	Guid IHasMasterGuid.MasterGuid { get; set; }

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public Guid? A2re_PardazandeTafsilGuid
	{
		get
		{
			return a2re_PardazandeTafsilGuid;
		}
		set
		{
			a2re_PardazandeTafsilGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public Guid? A2re_PardazandeMoeinGuid
	{
		get
		{
			return a2re_PardazandeMoeinGuid;
		}
		set
		{
			a2re_PardazandeMoeinGuid = (((value ?? Guid.Empty) == Guid.Empty) ? null : value);
		}
	}

	public DateTime? A2re_ReceptionDate { get; set; }

	public bool Aval2re { get; set; }

	public string Description { get; set; }

	public string HesabNum { get; set; }

	public string Number { get; set; }

	public bool Personal { get; set; }

	public long PoshtNomreh { get; set; }

	public Guid DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public Guid? CreditA2reMoeinGuid { get; set; }

	public Guid? CreditA2reTafsilGuid { get; set; }

	public bool SayadiRegistered { get; set; }

	[JsonIgnore]
	public string AndroidPrintText => "چک ش" + Number + " " + BankName;

	[JsonIgnore]
	public string PriceString => base.Price.NpToString("N0", 0);

	[JsonIgnore]
	public string ReceptionDateSh => Modified.MiladiToCalture();

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public int? SanadNo { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiName { get; set; }

	public ReceptionChequeMetaData MetaData { get; set; }

	[JsonIgnore]
	public override string DescriptionForUi
	{
		get
		{
			string text = "تاریخ سررسید : " + Date.MiladiToCalture() + " * شماره : " + Number + " * بانک : " + BankName + " *  شماره حساب : " + HesabNum + " * شرح: " + Description + " * ";
			text = (Personal ? (text + "نوع : چک شخصی ") : (text + "نوع : چک اشخاص "));
			return text + " وضعیت در سیستم : " + Status.GetDisplay();
		}
	}

	public override FileManagement_Parts Part => FileManagement_Parts.Cheque;

	public async Task<TafsilBusiness> GetTafsilAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 4;
		try
		{
			if (_tafsil == null)
			{
				if (Status == ENUMCheck.Entezami_Mojod || Status == ENUMCheck.Entezami_Odat)
				{
					ReceptionChequeEntezamiBusiness receptionChequeEntezamiBusiness = await ReceptionChequeEntezamiBusiness.GetAsync(base.ContextGuid, base.Guid, token, tranName);
					if ((receptionChequeEntezamiBusiness?.DebitTafsilGuid ?? Guid.Empty) != Guid.Empty)
					{
						_tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, system, receptionChequeEntezamiBusiness.DebitTafsilGuid.Value, "", token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs");
					}
				}
				else if (Aval2re)
				{
					_tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, system, A2re_PardazandeTafsilGuid.Value, "", token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs");
				}
				else
				{
					Guid tafsilGuid = (await ReceptionReportBusiness.GetAsync(base.ContextGuid, base.MasterGuid.Value, token, tranName)).TafsilGuid;
					_tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, system, tafsilGuid, "", token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs");
				}
			}
			return _tafsil;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 96);
			throw;
		}
	}

	public ReceptionCheckBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public async Task<ReceptionBusiness> GetReceptionMasterAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		try
		{
			await _databaseSemaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			if (_reception == null && base.MasterGuid.HasValue)
			{
				_reception = await ReceptionBusiness.GetAsync(base.ContextGuid, base.MasterGuid ?? Guid.Empty, system, null, fromDatabase: true, token, tranName);
			}
			return _reception;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionMasterAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 133);
			return null;
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_databaseSemaphore.Release();
			}
		}
	}

	public void SetReceptionMaster(ReceptionBusiness value)
	{
		_reception = value;
	}

	public static async Task<ReturnedSaveFuncInfo> ChangeStatusAsync(string contextGuid, EnSystem system, Guid guidCheck, ENUMCheck Status, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 147), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 147);
				}
				transactionHasBeenCreated = true;
				List<ReceptionCheckBusiness> list = new List<ReceptionCheckBusiness>();
				ReceptionCheckBusiness receptionCheckBusiness = await GetAsync(contextGuid, guidCheck, token);
				if (receptionCheckBusiness == null)
				{
					ret.AddError(system, "Object is Null", "obj", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 153);
					return ret;
				}
				receptionCheckBusiness.Status = Status;
				list.Add(receptionCheckBusiness);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.SaveAsync(list, receptionCheckBusiness.MasterGuid ?? Guid.Empty, tryCount, token, tranName), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 159);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 163);
				ret.AddReturnedValue(ex, "", "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 164);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, ret.HasError, disposeContext: true, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 169), ReturnedState.None, "ChangeStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 169);
			}
		}
		return ret;
	}

	public static Task<string> NextPoshtNomreAsync(string contextGuid, int checkCount, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.NextPoshtNomreAsync(checkCount, token, tranName);
	}

	public static Task<ReceptionCheckBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.GetAsync(guid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(EnSystem system, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
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
			_ = 6;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 185), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 185);
				}
				transactionHasBeenCreated = true;
				if (!ret.HasError && Aval2re && !IsEnteghali && system == EnSystem.UserInterface)
				{
					if ((UserGuid ?? Guid.Empty) == Guid.Empty)
					{
						UserGuid = CashierBusiness.GetCurrentCashierGuid(base.ContextGuid);
					}
					if (SanadNo.GetValueOrDefault() <= 0)
					{
						SanadNo = await SanadBusiness.NextIdAsync(base.ContextGuid, token, tranName);
					}
					if (DebitMoeinGuid == Guid.Empty)
					{
						DebitMoeinGuid = (await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token, tranName)).CodeSetting.CheckMCoding.Debit_MoeinGuid_Reception;
					}
					SanadBusiness sanad = await GenerateSanadA2reAsync(system, token, tranName);
					List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, base.Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 198);
				}
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.IReceptionCheckRepository.SaveAsync(new List<ReceptionCheckBusiness> { this }, base.MasterGuid ?? Guid.Empty, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 201);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 205);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 206);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 211), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 211);
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.ChequM.RaiseOperationSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	public static Task<IEnumerable<ReceptionCheckReportBusiness>> GetAllAsync(string contextGuid, ReceptionChequeFilter filter, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.GetAllAsync(filter, token, tranName);
	}

	public async Task<Guid> GetBankVagozarGuidAsync(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<ChequeMOperationBusiness> enumerable = await ChequeMOperationBusiness.GetAllAsync(base.ContextGuid, new ChequeMOperationFilter
			{
				ChequeGuid = base.Guid
			}, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return Guid.Empty;
			}
			ChequeMOperationBusiness chequeMOperationBusiness = enumerable.OrderByDescending((ChequeMOperationBusiness q) => q.Ordinal).FirstOrDefault((ChequeMOperationBusiness q) => q.NewState == ENUMCheck.VagozarShode);
			if (chequeMOperationBusiness == null)
			{
				return Guid.Empty;
			}
			return chequeMOperationBusiness.DebitTafsilGuid ?? Guid.Empty;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetBankVagozarGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 231);
			throw;
		}
	}

	public async Task<SanadBusiness> GenerateSanadA2reAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 6;
		try
		{
			ENSource source = ClsCache.GetCache(base.ContextGuid).Source;
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, A2re_PardazandeTafsilGuid.Value, tranName, token, "GenerateSanadA2reAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs");
			Guid oldGuid = Guid.NewGuid();
			SanadBusiness sanad2 = await SanadBusiness.GetAsync(base.ContextGuid, SanadNo.Value, token, tranName);
			if (sanad2 != null)
			{
				oldGuid = sanad2.Guid;
			}
			sanad2 = new SanadBusiness(base.ContextGuid, SanadNo.Value)
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = base.ContextGuid,
				DateM = DateTime.Now,
				Description = "چک دریافتی ابتدای دوره (" + BankName + " - " + Number + ") ** طرف حساب " + tafsil.Name + " ** سررسید " + DateSh,
				Guid = oldGuid,
				IsTemp = true,
				Modified = DateTime.Now,
				SanadNo = SanadNo.GetValueOrDefault(),
				SanadSt = SanadSt.Movaghat,
				SanadType = SanadType.Automatic,
				Source = source,
				UserGuid = CashierBusiness.GetCurrentCashierGuid(base.ContextGuid),
				Status = true
			};
			Sanad_DetBusiness debit = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.CheckSanad, sanad2.NextOrdinal())
			{
				Credit = 0m,
				Debit = base.Price,
				DateSarresid = Date,
				RowDesc = sanad2.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad2.Guid,
				Modified = DateTime.Now,
				Serial = Number,
				Status = true
			};
			await debit.SetMoeinGuidAsync(DebitMoeinGuid);
			await debit.SetTafsilGuidAsync(DebitTafsilGuid, "", token);
			await sanad2.AddToListSanadAsync(debit, system, token);
			Sanad_DetBusiness credit = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.CheckSanad, sanad2.NextOrdinal())
			{
				Credit = base.Price,
				Debit = 0m,
				DateSarresid = Date,
				RowDesc = sanad2.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad2.Guid,
				Modified = DateTime.Now,
				Serial = Number,
				Status = true
			};
			await credit.SetMoeinGuidAsync(ParentDefaults.MoeinCoding.CLSMoein15);
			await sanad2.AddToListSanadAsync(credit, system, token);
			return sanad2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadA2reAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 296);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((DebitTafsilGuid ?? Guid.Empty) == Guid.Empty)
			{
				DebitTafsilGuid = ParentDefaults.TafsilCoding.CLSTafsil29766;
			}
			if (base.Price <= 0m)
			{
				res.AddError(system, "مبلغ سند دریافتنی(چک مشتری) معتبر نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 306);
			}
			if (system == EnSystem.Transfer)
			{
				return res;
			}
			ReceptionCheckBusiness receptionCheckBusiness = await GetAsync(base.ContextGuid, base.Guid, token, tranName);
			if (receptionCheckBusiness != null && receptionCheckBusiness.Status != ENUMCheck.Mojod && (receptionCheckBusiness.Price != base.Price || receptionCheckBusiness.Status != Status) && Status != ENUMCheck.Mojod)
			{
				res.AddError(system, "چک جاری در وضعیت".GetTranslate() + " " + receptionCheckBusiness.Status.GetDisplay().GetTranslate() + " " + " می باشد که قابل ویرایش نمی باشد".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 310);
			}
			if (system == EnSystem.UserInterface && !isDeleteAction)
			{
				AccGlobalSettings obj = await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token);
				DateTime chequeAndInstallmentStartDate = obj.DateAndTimePriod.ChequeAndInstallmentStartDate;
				DateTime chequeAndInstallmentEndDate = obj.DateAndTimePriod.ChequeAndInstallmentEndDate;
				if (Date < chequeAndInstallmentStartDate || Date > chequeAndInstallmentEndDate)
				{
					res.AddError(system, "شماره".GetTranslate() + Number + "\r\n" + "تاریخ سررسید".GetTranslate() + "(" + Date.MiladiToCalture() + ")\r\n" + "در بازه مجاز".GetTranslate() + "(" + chequeAndInstallmentStartDate.MiladiToCalture() + " تا ".GetTranslate() + chequeAndInstallmentEndDate.MiladiToCalture() + ")" + " نیست.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 318);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 324);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 325);
		}
		return res;
	}

	public static async Task<List<ReceptionCheckBusiness>> GetAllAsync(string contextGuid, Guid masterGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		bool semaphoreHasBeenTaken = false;
		await _connectionPoolSemaphore.WaitAsync();
		try
		{
			semaphoreHasBeenTaken = true;
			return await ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.GetAllAsync(masterGuid, token, tranName);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_connectionPoolSemaphore.Release();
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<ReceptionCheckReportBusiness> enumerable = await GetAllAsync(contextGuid, new ReceptionChequeFilter(), token);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			enumerable = enumerable?.Where((ReceptionCheckReportBusiness q) => q.Aval2re && !q.IsEnteghali);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (ReceptionCheckReportBusiness item in enumerable)
			{
				if ((item?.SanadNo).GetValueOrDefault() > 0)
				{
					SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(contextGuid, item.SanadNo.Value, token);
					if (sanadBusiness != null && (sanadBusiness.ListSanad_Det?.Any() ?? false) && !sanadBusiness.ListSanad_Det.Any((Sanad_DetBusiness q) => q.AsGuid == item.Guid))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, 1, "", token, new Tuple<Document, Guid>(Document.CheckSanad, sanadBusiness.Guid)), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 360);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 365);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 366);
		}
		return res;
	}

	public Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system)
	{
		try
		{
			if (system == EnSystem.Transfer)
			{
				return null;
			}
			return Task.FromResult(new List<ViewSanadDetailCoverBusiness>
			{
				new ViewSanadDetailCoverBusiness(base.ContextGuid)
				{
					DebitMoeinGuid = DebitMoeinGuid,
					DebitTafsilGuid = DebitTafsilGuid,
					Date = A2re_ReceptionDate.Value,
					AsGuid = base.Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = 0m,
					Credit = base.Price,
					CashierGuid = UserGuid.Value,
					SanadNo = SanadNo.Value,
					SanadSt = SanadSt.Movaghat,
					Id2 = 0,
					CreditMoeinGuid = CreditA2reMoeinGuid,
					CreditTafsilGuid = CreditA2reTafsilGuid,
					SanadDesc = Description,
					RDesc = "",
					DateMoaser = Date,
					DetailGuid = base.Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = 0m,
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = Number,
					Status = true,
					VahedGuid = null
				},
				new ViewSanadDetailCoverBusiness(base.ContextGuid)
				{
					DebitMoeinGuid = DebitMoeinGuid,
					DebitTafsilGuid = DebitTafsilGuid,
					Date = A2re_ReceptionDate.Value,
					AsGuid = base.Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = base.Price,
					Credit = 0m,
					CashierGuid = UserGuid.Value,
					SanadNo = SanadNo.Value,
					SanadSt = SanadSt.Movaghat,
					Id2 = 0,
					CreditMoeinGuid = CreditA2reMoeinGuid,
					CreditTafsilGuid = CreditA2reTafsilGuid,
					SanadDesc = Description,
					RDesc = "",
					DateMoaser = Date,
					DetailGuid = base.Guid,
					Guid = Guid.NewGuid(),
					DecimalGuid = null,
					DecimalPrice = 0m,
					DecimalQt = 0m,
					DepotGuid = null,
					Modified = DateTime.Now,
					Price = 0m,
					ProductGuid = null,
					Qt = 0m,
					Qt2 = 0m,
					RBazaryab = 0m,
					RDiscount = 0m,
					Serial = Number,
					Status = true,
					VahedGuid = null
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 454);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReceptionChequeFilter filter = new ReceptionChequeFilter();
			IEnumerable<ReceptionCheckReportBusiness> listCheque = await GetAllAsync(contextGuid, filter, token);
			if (!(listCheque?.Any() ?? false))
			{
				return res;
			}
			IEnumerable<SandoghBusiness> listSandouq = await SandoghBusiness.GetAllAsync(contextGuid, EnSystem.None, "", true, "", token);
			if (!(listSandouq?.Any() ?? false))
			{
				return res;
			}
			Guid sandouqGuid = listSandouq.FirstOrDefault().Guid;
			foreach (ReceptionCheckReportBusiness item in listCheque)
			{
				if (item == null)
				{
					continue;
				}
				ReceptionCheckBusiness receptionCheque = await GetAsync(contextGuid, item.Guid, token);
				if (receptionCheque == null)
				{
					continue;
				}
				if (!receptionCheque.DebitTafsilGuid.HasValue || !(listSandouq?.Select((SandoghBusiness q) => q.Guid)?.Contains(receptionCheque.DebitTafsilGuid ?? Guid.Empty)).GetValueOrDefault())
				{
					receptionCheque.DebitTafsilGuid = sandouqGuid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await receptionCheque.SaveAsync(EnSystem.Transfer, 10, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 477);
					if ((receptionCheque?.MasterGuid ?? Guid.Empty) == Guid.Empty)
					{
						continue;
					}
					ReceptionBusiness receptionBusiness = await ReceptionBusiness.GetAsync(contextGuid, receptionCheque.MasterGuid.Value, EnSystem.None, null, fromDatabase: true, token);
					if (receptionBusiness == null)
					{
						continue;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.SaveAsync(EnSystem.None, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, "", token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 481);
				}
				ChequeMOperationFilter filter2 = new ChequeMOperationFilter
				{
					ChequeGuid = item.Guid
				};
				IEnumerable<ChequeMOperationBusiness> enumerable = await ChequeMOperationBusiness.GetAllAsync(contextGuid, filter2, token);
				if (enumerable == null || !enumerable.Any())
				{
					continue;
				}
				foreach (ChequeMOperationBusiness item2 in enumerable)
				{
					bool flag = false;
					if (item2.NewState != ENUMCheck.KharjShode)
					{
						item2.IsDebitTafsilAffective = true;
						item2.IsCreditTafsilAffective = true;
						item2.IsSanadAffective = true;
						flag = true;
					}
					if ((item2.CreditTafsilGuid ?? Guid.Empty) == Guid.Empty || item2.CreditTafsilGuid == ParentDefaults.TafsilCoding.CLSTafsil29766)
					{
						if (item2.NewState == ENUMCheck.VosolShode)
						{
							item2.CreditTafsilGuid = item2.DebitTafsilGuid.Value;
						}
						else
						{
							item2.CreditTafsilGuid = sandouqGuid;
						}
						flag = true;
					}
					if ((item2.DebitTafsilGuid ?? Guid.Empty) == Guid.Empty || item2.DebitTafsilGuid == ParentDefaults.TafsilCoding.CLSTafsil29766)
					{
						item2.DebitTafsilGuid = sandouqGuid;
						flag = true;
					}
					if (item2.OldState.GetValueOrDefault() == ENUMCheck.Mojod && !(listSandouq?.Select((SandoghBusiness q) => q.Guid)?.Contains(item2.CreditTafsilGuid ?? Guid.Empty)).GetValueOrDefault())
					{
						item2.CreditTafsilGuid = sandouqGuid;
						flag = true;
					}
					if (flag)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(EnSystem.None, sendToServer: false, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 517);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 523);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionCheckBusiness.cs", 524);
		}
		return res;
	}

	public static Task<ReceptionCheckBusiness> GetAsync(string contextGuid, string number, CancellationToken token, ENUMCheck? status = null, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReceptionCheckRepository.GetAsync(number, token, status, tranName);
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return Task.FromResult(AndroidPrintText);
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[پردازنده]"),
			new SmsTemplateRecord("[شماره چک]"),
			new SmsTemplateRecord("[تاریخ چک]"),
			new SmsTemplateRecord("[مبلغ چک]")
		};
	}
}
