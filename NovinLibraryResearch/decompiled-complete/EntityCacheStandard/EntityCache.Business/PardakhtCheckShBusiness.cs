using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Cheque;
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

public class PardakhtCheckShBusiness : BaseKhazaneDetail, IPardakhtCheckSh, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, INotifyPropertyChanged, IHasChangeStatus
{
	private Guid? a2re_MoeinGuid;

	private Guid? a2re_TafsilGuid;

	private PardakhtBusiness _pardakht;

	private string _tafsilName = "";

	private DateTime _paymentDate = DateTime.Now;

	public override KhazaneType Type => KhazaneType.ChequeSh;

	public DateTime Modified { get; set; } = DateTime.Now;


	public ServerStatusForClient ServerStatus { get; set; }

	bool IHasStatus.Status => Status == ENCheckSHStatus.Pardakhtani;

	public bool IsEnteghali { get; set; }

	public Guid? UserGuid { get; set; }

	public DateTime DateSaresid { get; set; } = DateTime.Now;


	public override DateTime DateM
	{
		get
		{
			return DateSaresid;
		}
		set
		{
			DateSaresid = value;
		}
	}

	[JsonIgnore]
	public string DateSarresidSh => DateSaresid.MiladiToCalture();

	[JsonIgnore]
	public TafsilBusiness BankInfo => TafsilBusiness.Get(base.ContextGuid, BankTafsilGuid);

	[JsonIgnore]
	public override string DescriptionForUi
	{
		get
		{
			string text = "سریال:" + Number + " - بانک:" + BankInfo?.Name + " سررسید:" + DateSarresidSh;
			if (!string.IsNullOrEmpty(RowDesc))
			{
				text = text + "شرح سطر : " + RowDesc;
			}
			return text;
		}
	}

	public string RowDesc { get; set; }

	public string Number { get; set; }

	public ENCheckSHStatus Status { get; set; }

	public string StatusName => Status.GetDisplay();

	public Guid? CreditTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid BankTafsilGuid { get; set; }

	Guid IHasMasterGuid.MasterGuid { get; set; }

	public DateTime PardakhtDate
	{
		get
		{
			return _paymentDate;
		}
		set
		{
			_paymentDate = value;
		}
	}

	[JsonIgnore]
	public string PardakhtDateSh => PardakhtDate.MiladiToCalture();

	public bool Aval2re { get; set; }

	public bool SayadiRegistered { get; set; }

	public Guid? A2re_MoeinGuid
	{
		get
		{
			return a2re_MoeinGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				a2re_MoeinGuid = null;
			}
			else
			{
				a2re_MoeinGuid = value;
			}
		}
	}

	public Guid? A2re_TafsilGuid
	{
		get
		{
			return a2re_TafsilGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				a2re_TafsilGuid = null;
			}
			else
			{
				a2re_TafsilGuid = value;
			}
		}
	}

	[JsonIgnore]
	public string BankName { get; set; }

	[JsonIgnore]
	public string AndroidPrintText => "چک س" + Number + " " + BankName + " ";

	[JsonIgnore]
	public string PriceString => base.Price.NpToString("N0", 0);

	[JsonIgnore]
	public bool IsChecked { get; set; }

	public Guid? DebitA2reMoeinGuid { get; set; }

	public Guid? DebitA2reTafsiGuid { get; set; }

	public string TafsilName
	{
		get
		{
			if (Aval2re && IsEnteghali)
			{
				return _tafsilName + " (ابتدای دوره انتقالی)";
			}
			if (Aval2re)
			{
				return _tafsilName + " (ابتدای دوره)";
			}
			return _tafsilName;
		}
		set
		{
			_tafsilName = value;
		}
	}

	public int? SanadNo { get; set; }

	public string SayadiNationalCode { get; set; }

	public string SayadiIdentity { get; set; }

	public string SayadiName { get; set; }

	public PardakhtChequeShMetaData MetaData { get; set; }

	public EntityChangeSt ChangeStatus { get; set; }

	public override FileManagement_Parts Part => FileManagement_Parts.Cheque;

	public new event PropertyChangedEventHandler PropertyChanged;

	public PardakhtCheckShBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
		ChangeStatus = EntityChangeSt.New;
	}

	public async Task<string> GetTafsilNameAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 2;
		try
		{
			if ((base.MasterGuid ?? Guid.Empty) == Guid.Empty)
			{
				return (await TafsilBusiness.GetAsync(base.ContextGuid, system, A2re_TafsilGuid ?? Guid.Empty, tranName, token, "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs"))?.Name;
			}
			ENSource source = ClsCache.GetCache(base.ContextGuid).Source;
			_pardakht = await PardakhtBusiness.GetAsync(base.ContextGuid, base.MasterGuid ?? Guid.Empty, source == ENSource.Accounting, token, tranName);
			return await _pardakht.GetFullNameAsync(system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetTafsilNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 108);
			throw;
		}
	}

	public async Task<TafsilBusiness> GetBankInfoAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await TafsilBusiness.GetAsync(base.ContextGuid, system, BankTafsilGuid, tranName, token, "GetBankInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs");
	}

	public static Task<PardakhtCheckShBusiness> GetAsync(string contextGuid, Guid guid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakhtCheckSh.GetAsync(guid, token, tranName);
	}

	public static Task<List<PardakhtCheckShBusiness>> GetAllAsync(string contextGuid, Guid masterGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakhtCheckSh.GetAllAsync(masterGuid, token, tranName);
	}

	public static Task<IEnumerable<PardakhtCheckShReportBusiness>> GetAllAsync(string contextGuid, PardakhtChequeFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IPardakhtCheckSh.GetAllAsync(filter, token, tranName);
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
			_ = 5;
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (autoTran)
				{
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 126), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 126);
				}
				transactionHasBeenCreated = true;
				if (!ret.HasError && Aval2re && system == EnSystem.UserInterface)
				{
					if (SanadNo.GetValueOrDefault() <= 0)
					{
						SanadNo = await SanadBusiness.NextIdAsync(base.ContextGuid, token, tranName);
					}
					if ((UserGuid ?? Guid.Empty) == Guid.Empty)
					{
						UserGuid = CashierBusiness.GetCurrentCashierGuid(base.ContextGuid);
					}
					SanadBusiness sanad = await GenerateSanadA2reAsync(system, token, tranName);
					List<ViewSanadDetailCoverBusiness> lst = await GetSanadCoverListAsync(system, tranName);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await sanad.SaveAsync(system, token, true, true, tranName, lst, new Tuple<Document, Guid>(Document.CheckSanad, base.Guid)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 134);
				}
				if (ret.HasError)
				{
					return ret;
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(base.ContextGuid).CacheRepository.IPardakhtCheckSh.SaveAsync(new List<PardakhtCheckShBusiness> { this }, base.MasterGuid ?? Guid.Empty, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 137);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 141);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 142);
			}
		}
		finally
		{
			if (autoTran && transactionHasBeenCreated)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(base.ContextGuid, tranName, ret.HasError, disposeContext: true, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 147), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 147);
			}
			if (!ret.HasError && system == EnSystem.UserInterface)
			{
				ClsCache.GetCache(base.ContextGuid).BusinessAggregator.Events.ChequeSh.RaiseOperationSaved(this, EventArgs.Empty);
			}
		}
		return ret;
	}

	public static Task<PardakhtCheckShBusiness> GenerateNewWithDefaultsAsync(string contextGuid, EnSystem system, Guid? tafsilGuid = null, string description = "", Guid? guid = null, string tranName = "")
	{
		try
		{
			PardakhtCheckShBusiness pardakhtCheckShBusiness = new PardakhtCheckShBusiness(contextGuid);
			if ((tafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				pardakhtCheckShBusiness.A2re_TafsilGuid = tafsilGuid.Value;
			}
			if (!string.IsNullOrEmpty(description))
			{
				pardakhtCheckShBusiness.RowDesc = description.FixString();
			}
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				pardakhtCheckShBusiness.Guid = guid.Value;
			}
			pardakhtCheckShBusiness.Status = ENCheckSHStatus.Pardakhtani;
			return Task.FromResult(pardakhtCheckShBusiness);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateNewWithDefaultsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 174);
			throw;
		}
	}

	public async Task<SanadBusiness> GenerateSanadA2reAsync(EnSystem system, CancellationToken token, string tranName)
	{
		_ = 7;
		try
		{
			ENSource source = ClsCache.GetCache(base.ContextGuid).Source;
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(base.ContextGuid, EnSystem.None, A2re_TafsilGuid.Value, tranName, token, "GenerateSanadA2reAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs");
			TafsilBusiness bankInfo = await GetBankInfoAsync(system, token, tranName);
			Guid oldGuid = Guid.NewGuid();
			SanadBusiness sanad2 = await SanadBusiness.GetAsync(base.ContextGuid, SanadNo.Value, token, tranName);
			if (sanad2 != null)
			{
				oldGuid = sanad2.Guid;
			}
			sanad2 = new SanadBusiness(base.ContextGuid, SanadNo.GetValueOrDefault())
			{
				ChangeStatus = EntityChangeSt.New,
				ContextGuid = base.ContextGuid,
				DateM = DateTime.Now,
				Description = "چک پرداختنی ابتدای دوره (" + bankInfo?.Name + " - " + Number + ") ** طرف حساب " + tafsil.Name + " ** سررسید " + DateSarresidSh,
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
				DateSarresid = DateSaresid,
				RowDesc = sanad2.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad2.Guid,
				Modified = DateTime.Now,
				Serial = Number,
				Status = true
			};
			await debit.SetMoeinGuidAsync(ParentDefaults.MoeinCoding.CLSMoein15);
			await sanad2.AddToListSanadAsync(debit, system, token);
			Sanad_DetBusiness credit = new Sanad_DetBusiness(base.ContextGuid, system, base.Guid, Document.CheckSanad, sanad2.NextOrdinal())
			{
				Credit = base.Price,
				Debit = 0m,
				DateSarresid = DateSaresid,
				RowDesc = sanad2.Description,
				Guid = Guid.NewGuid(),
				MasterGuid = sanad2.Guid,
				Modified = DateTime.Now,
				Serial = Number,
				Status = true
			};
			await credit.SetMoeinGuidAsync(CreditMoeinGuid);
			await credit.SetTafsilGuidAsync(CreditTafsilGuid, "", token);
			await sanad2.AddToListSanadAsync(credit, system, token);
			return sanad2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSanadA2reAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 240);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, bool isDeleteAction, string tranName, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			TafsilBusiness tafsilBusiness = await GetBankInfoAsync(system, token, tranName);
			if (tafsilBusiness == null || tafsilBusiness.HType != HesabType.Bank)
			{
				res.AddError(system, "بانک طرف حساب پرداخت چک شخصی وارد شده معتبر نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 250);
			}
			if (base.Price <= 0m)
			{
				res.AddError(system, "مبلغ وارد شده جهت پرداخت چک شخصی معتبر نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 251);
			}
			if (system == EnSystem.UserInterface && !isDeleteAction)
			{
				AccGlobalSettings obj = await LocalSettingBusiness.GetSettingAsync(base.ContextGuid, token);
				DateTime chequeAndInstallmentStartDate = obj.DateAndTimePriod.ChequeAndInstallmentStartDate;
				DateTime chequeAndInstallmentEndDate = obj.DateAndTimePriod.ChequeAndInstallmentEndDate;
				if (DateM < chequeAndInstallmentStartDate || DateM > chequeAndInstallmentEndDate)
				{
					res.AddError(system, "شماره " + Number + "\r\nتاریخ سررسید(" + DateM.MiladiToCalture() + ")\r\n در بازه مجاز(" + chequeAndInstallmentStartDate.MiladiToCalture() + " تا " + chequeAndInstallmentEndDate.MiladiToCalture() + ") نیست.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 259);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 264);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 265);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, EnSystem system, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IEnumerable<PardakhtCheckShReportBusiness> enumerable = await GetAllAsync(contextGuid, new PardakhtChequeFilter(), token);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			enumerable = enumerable?.Where((PardakhtCheckShReportBusiness q) => q.Aval2re && !q.IsEnteghali);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (PardakhtCheckShReportBusiness item in enumerable)
			{
				if ((item?.SanadNo).GetValueOrDefault() > 0)
				{
					SanadBusiness sanadBusiness = await SanadBusiness.GetAsync(contextGuid, item.SanadNo.Value, token);
					if (sanadBusiness != null && (sanadBusiness.ListSanad_Det?.Any() ?? false) && !sanadBusiness.ListSanad_Det.Any((Sanad_DetBusiness q) => q.AsGuid == item.Guid))
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await sanadBusiness.RemoveAsync(system, 1, "", token, new Tuple<Document, Guid>(Document.CheckSanad, sanadBusiness.Guid)), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 285);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 290);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 291);
		}
		return res;
	}

	public Task<List<ViewSanadDetailCoverBusiness>> GetSanadCoverListAsync(EnSystem system, string tranName)
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
					DebitMoeinGuid = DebitA2reMoeinGuid,
					DebitTafsilGuid = DebitA2reTafsiGuid,
					Date = PardakhtDate,
					AsGuid = base.Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = 0m,
					Credit = base.Price,
					CashierGuid = UserGuid.Value,
					SanadNo = SanadNo.Value,
					SanadSt = SanadSt.Movaghat,
					Id2 = 0,
					CreditMoeinGuid = CreditMoeinGuid,
					CreditTafsilGuid = CreditTafsilGuid,
					SanadDesc = "",
					RDesc = "",
					DateMoaser = DateSaresid,
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
					DebitMoeinGuid = DebitA2reMoeinGuid,
					DebitTafsilGuid = DebitA2reTafsiGuid,
					Date = PardakhtDate,
					AsGuid = base.Guid,
					AsType = DocumentDet.ChequeSanad,
					AsTypeDocument = Document.CheckSanad,
					Debit = base.Price,
					Credit = 0m,
					CashierGuid = UserGuid.Value,
					SanadNo = SanadNo.Value,
					SanadSt = SanadSt.Movaghat,
					Id2 = 0,
					CreditMoeinGuid = CreditMoeinGuid,
					CreditTafsilGuid = CreditTafsilGuid,
					SanadDesc = "",
					RDesc = "",
					DateMoaser = DateSaresid,
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
			WebErrorLog.Instance.StartLog(ex, "", "GetSanadCoverListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 379);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixDataAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			PardakhtChequeFilter filter = new PardakhtChequeFilter();
			IEnumerable<PardakhtCheckShReportBusiness> enumerable = await GetAllAsync(contextGuid, filter, token);
			if (enumerable == null || !enumerable.Any())
			{
				return res;
			}
			foreach (PardakhtCheckShReportBusiness item in enumerable)
			{
				if (item == null)
				{
					continue;
				}
				PardakhtCheckShBusiness pardakhtCheque = await GetAsync(contextGuid, item.Guid, token);
				if (pardakhtCheque == null)
				{
					continue;
				}
				if (pardakhtCheque.CreditTafsilGuid == ParentDefaults.TafsilCoding.CLSTafsil29765)
				{
					pardakhtCheque.CreditTafsilGuid = pardakhtCheque.BankTafsilGuid;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await pardakhtCheque.SaveAsync(EnSystem.Transfer, 10, token), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 399);
					if ((pardakhtCheque?.MasterGuid ?? Guid.Empty) == Guid.Empty)
					{
						continue;
					}
					PardakhtBusiness pardakhtBusiness = await PardakhtBusiness.GetAsync(contextGuid, pardakhtCheque.MasterGuid.Value, fromDatabase: true, token);
					if (pardakhtBusiness == null)
					{
						continue;
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await pardakhtBusiness.SaveAsync(EnSystem.None, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 403);
				}
				ChequeShOperationFilter filter2 = new ChequeShOperationFilter
				{
					ChequeGuid = item.Guid
				};
				IEnumerable<ChequeShOperationBusiness> enumerable2 = await ChequeShOperationBusiness.GetAllAsync(contextGuid, filter2, token);
				if (enumerable2 == null || !enumerable2.Any())
				{
					continue;
				}
				foreach (ChequeShOperationBusiness item2 in enumerable2)
				{
					bool flag = false;
					if ((item2.CreditTafsilGuid ?? Guid.Empty) == Guid.Empty || item2.CreditTafsilGuid == ParentDefaults.TafsilCoding.CLSTafsil29765)
					{
						item2.CreditTafsilGuid = pardakhtCheque.BankTafsilGuid;
						flag = true;
					}
					if ((item2.DebitTafsilGuid ?? Guid.Empty) == Guid.Empty || item2.DebitTafsilGuid == ParentDefaults.TafsilCoding.CLSTafsil29765)
					{
						item2.DebitTafsilGuid = pardakhtCheque.BankTafsilGuid;
						flag = true;
					}
					if (flag)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await item2.SaveAsync(EnSystem.None, sendToServer: false, token, awaitOnSemaphore: true, 10), ReturnedState.None, "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 423);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 429);
			res.AddReturnedValue(ex, "", "FixDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtCheckShBusiness.cs", 430);
		}
		return res;
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return Task.FromResult(AndroidPrintText);
	}

	public static List<SmsTemplateRecord> GetSmsTemplateList()
	{
		return new List<SmsTemplateRecord>
		{
			new SmsTemplateRecord("[موجودی بانک]"),
			new SmsTemplateRecord("[نام بانک]"),
			new SmsTemplateRecord("[گیرنده]"),
			new SmsTemplateRecord("[شماره چک]"),
			new SmsTemplateRecord("[تاریخ چک]"),
			new SmsTemplateRecord("[مبلغ چک]")
		};
	}
}
