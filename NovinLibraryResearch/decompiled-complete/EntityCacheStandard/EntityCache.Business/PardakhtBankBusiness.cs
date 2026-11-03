using System;
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

public class PardakhtBankBusiness : BaseKhazaneDetail, IPardakhtBank, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage
{
	private string _number;

	private Guid moeinGuid;

	private Guid? decimalGuid;

	public override KhazaneType Type => KhazaneType.Havale;

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string DateSh => DateM.MiladiToCalture();

	public override string DescriptionForUi
	{
		get
		{
			string text = string.Format("بانک: {0}  شماره: {1} در تاریخ: {2}:{3} {4}", BankInfo?.Name ?? "", Number, DateM.Hour, DateM.Minute, DateSh);
			if (!string.IsNullOrEmpty(RowDesc))
			{
				text = text + "شرح سطر : " + RowDesc;
			}
			if (WagePrice != 0m)
			{
				text += $" (مبلغ کارمزد: {WagePrice:N0})";
			}
			return text;
		}
	}

	public string RowDesc { get; set; }

	public string Number
	{
		get
		{
			return _number;
		}
		set
		{
			_number = value?.FixString() ?? "";
		}
	}

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
	[XmlIgnore]
	public TafsilBusiness BankInfo => TafsilBusiness.Get(base.ContextGuid, TafsilGuid);

	[JsonIgnore]
	public string PriceString => base.Price.NpToString("N0", 0);

	public PardakhtHavaleMetaData MetaData { get; set; }

	public decimal? BankRealAccount { get; set; }

	public Guid? WageGuid { get; set; }

	public Guid? WageMoeinGuid { get; set; }

	public Guid? WageTafsilGuid { get; set; }

	public decimal WagePrice { get; set; }

	public override FileManagement_Parts Part => FileManagement_Parts.Havale;

	public new Guid MasterGuid { get; set; }

	public PardakhtBankBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidation(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			TafsilBusiness tafsilBusiness = await GetBankInfoAsync(system, token, tranName);
			if (tafsilBusiness == null || tafsilBusiness.HType != HesabType.Bank)
			{
				res.AddError(system, "حساب بانک وارد شده معتبر نمیباشد.".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs", 70);
			}
			if (base.Price <= 0m)
			{
				res.AddError(system, "مبلغ وارد شده جهت پرداخت از بانک معتبر نمیباشد.".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs", 72);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs", 76);
			res.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs", 77);
		}
		return res;
	}

	public async Task<string> GetAndroidPrintTextAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			TafsilBusiness tafsilBusiness = await GetBankInfoAsync(system, token, tranName);
			return "حواله".GetTranslate() + (" " + tafsilBusiness.Name).GetTranslate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAndroidPrintTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs", 90);
			throw;
		}
	}

	public override async Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			return await GetAndroidPrintTextAsync(system, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetShortDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs", 103);
			throw;
		}
	}

	public Task<TafsilBusiness> GetBankInfoAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return TafsilBusiness.GetAsync(base.ContextGuid, system, TafsilGuid, tranName, token, "GetBankInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtBankBusiness.cs");
	}
}
