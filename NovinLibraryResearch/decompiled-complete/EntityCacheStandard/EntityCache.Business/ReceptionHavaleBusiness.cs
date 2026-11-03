using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EntityCache.Business.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane;
using PacketParser.Entities.MetaData;

namespace EntityCache.Business;

public class ReceptionHavaleBusiness : BaseKhazaneDetail, IReceptionHavale, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage, IHasContext
{
	private Guid moeinGuid;

	private Guid? decimalGuid;

	public override KhazaneType Type => HavaleType switch
	{
		ENHavaleType.POS => KhazaneType.Poss, 
		_ => KhazaneType.Havale, 
	};

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public DateTime Date { get; set; } = DateManagment.DateM;


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

	public string DateSh => Date.MiladiToCalture();

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid
	{
		get
		{
			if (moeinGuid == Guid.Empty)
			{
				moeinGuid = ParentDefaults.MoeinCoding.CLSMoein4;
			}
			return moeinGuid;
		}
		set
		{
			moeinGuid = value;
		}
	}

	public string Number { get; set; }

	[JsonIgnore]
	internal string NumberForDescription
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(Number))
				{
					return "";
				}
				return " شماره: " + Number;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "NumberForDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 64);
			}
			return "";
		}
	}

	public string TerminalId { get; set; }

	public string PAN { get; set; }

	public string STAN { get; set; }

	[JsonIgnore]
	internal string STANForDescription
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(STAN))
				{
					return "";
				}
				return " پیگیری: " + STAN;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "STANForDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 81);
			}
			return "";
		}
	}

	public string RRN { get; set; }

	[JsonIgnore]
	internal string RRNForDescription
	{
		get
		{
			try
			{
				if (string.IsNullOrEmpty(RRN) || Description.Contains(RRN))
				{
					return "";
				}
				return " مرجع: " + RRN;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RRNForDescription", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 96);
			}
			return "";
		}
	}

	public string MerchantId { get; set; }

	public string MessageId { get; set; }

	public string Description { get; set; }

	public DateTime DateTasviye { get; set; } = DateTime.Now;


	[JsonIgnore]
	public string FullDescription
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(Description))
			{
				stringBuilder.Append(Description + " , ");
			}
			if (!string.IsNullOrEmpty(Number))
			{
				stringBuilder.Append(" ش.سند " + Number + " , ");
			}
			if (!string.IsNullOrEmpty(STAN))
			{
				stringBuilder.Append(" ش.پیگیری " + STAN + " , ");
			}
			if (!string.IsNullOrEmpty(RRN))
			{
				stringBuilder.Append(" ش.ارجاع " + RRN + " , ");
			}
			if (!string.IsNullOrEmpty(TerminalId))
			{
				stringBuilder.Append(" ش.پایانه " + TerminalId + " , ");
			}
			if (!string.IsNullOrEmpty(PAN))
			{
				stringBuilder.Append(" ش.پردازنده " + PAN + " , ");
			}
			return stringBuilder.ToString();
		}
	}

	public decimal Qt { get; set; }

	public Guid? DecimalGuid
	{
		get
		{
			return decimalGuid;
		}
		set
		{
			if ((value ?? Guid.Empty) == Guid.Empty)
			{
				decimalGuid = null;
			}
			else
			{
				decimalGuid = value;
			}
		}
	}

	[JsonIgnore]
	public string PriceString => base.Price.NpToString("N0", 0);

	public ReceptionHavaleMetaData MetaData { get; set; }

	public decimal? BankRealAccount { get; set; }

	public override string DescriptionForUi
	{
		get
		{
			string text = $"واریز به بانک: {BankInfo?.Name} درتاریخ: {Date.Hour}:{Date.Minute} {Date.MiladiToCalture()} شماره : {Number}";
			if (!string.IsNullOrEmpty(Description))
			{
				text = text + "شرح سطر : " + Description;
			}
			if (WagePrice != 0m)
			{
				text += $" (مبلغ کارمزد: {WagePrice:N0})";
			}
			return text;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public TafsilBusiness BankInfo => TafsilBusiness.Get(base.ContextGuid, TafsilGuid);

	public ENHavaleType HavaleType { get; set; }

	public new Guid MasterGuid { get; set; }

	public Guid? WageGuid { get; set; }

	public Guid? WageMoeinGuid { get; set; }

	public Guid? WageTafsilGuid { get; set; }

	public decimal WagePrice { get; set; }

	public override FileManagement_Parts Part => FileManagement_Parts.Havale;

	public ReceptionHavaleBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public async Task<string> GetAndroidPrintTextAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		try
		{
			return "حواله " + (await GetBankInfoAsync(system, token, tranName))?.Name + "," + FullDescription;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAndroidPrintTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 167);
			throw;
		}
	}

	public static Task<bool> IsPeygiriNumberTekrariAsync(string contextGuid, string number, Guid guid, Guid bankGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.IReceptionHavale.IsPeygiriNumberTekrariAsync(number, guid, bankGuid, token, tranName);
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			TafsilBusiness tafsilBusiness = await GetBankInfoAsync(system, token, tranName);
			if (TafsilGuid == Guid.Empty || (tafsilBusiness != null && tafsilBusiness.HType != HesabType.Bank))
			{
				res.AddError(system, "حساب وارد شده جهت بانک معتبر نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 180);
			}
			if (base.Price <= 0m && system != EnSystem.Transfer)
			{
				res.AddError(system, "مبلغ وارد شده جهت حواله بانکی معتبر نمیباشد.".GetTranslate(), "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 182);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 186);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 187);
		}
		return res;
	}

	public Task<TafsilBusiness> GetBankInfoAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		return TafsilBusiness.GetAsync(base.ContextGuid, system, TafsilGuid, tranName, token, "GetBankInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs");
	}

	public override async Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return await GetAndroidPrintTextAsync(system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetShortDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 201);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> FixWagesAsync(string contextGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		string key = "FixReceptionHavaleWages";
		ReturnedSaveFuncInfo result;
		try
		{
			_ = 3;
			try
			{
				ClsCache cache = ClsCache.GetCache(contextGuid);
				if (cache == null || cache.Source != ENSource.Accounting)
				{
					result = res;
					goto IL_04a9;
				}
				if ((await LocalSettingBusiness.GetAsync(contextGuid, key))?.ParseToBoolean() ?? false)
				{
					result = res;
					goto IL_04a9;
				}
				List<Guid> list = (await cache.CacheRepository.IReceptionHavale.GetAllMasterGuidsForFixWageAsync(default(CancellationToken), ""))?.Where((Guid q) => q != Guid.Empty)?.ToList();
				if (list == null || !list.Any())
				{
					result = res;
					goto IL_04a9;
				}
				foreach (Guid item in list)
				{
					ReceptionBusiness receptionBusiness = await ReceptionBusiness.GetAsync(contextGuid, item, EnSystem.None, null, fromDatabase: true);
					if (receptionBusiness != null)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
						returnedSaveFuncInfo.AddReturnedValue(await receptionBusiness.SaveAsync(EnSystem.None, sendToServer: false, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10), ReturnedState.None, "FixWagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 225);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FixWagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 230);
				res.AddReturnedValue(ex, "", "FixWagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionHavaleBusiness.cs", 231);
			}
		}
		finally
		{
			if (!res.HasError)
			{
				await LocalSettingBusiness.SaveAsync(contextGuid, key, true.ToString(), awaitOnSemaphore: true, 10);
			}
		}
		return res;
		IL_04a9:
		return result;
	}
}
