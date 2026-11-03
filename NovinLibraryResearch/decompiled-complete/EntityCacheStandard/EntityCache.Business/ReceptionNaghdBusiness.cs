using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class ReceptionNaghdBusiness : BaseKhazaneDetail, IReceptionNaghd, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private string _desc = "";

	private string _description;

	private Guid moeinGuid;

	private Guid? decimalGuid;

	public override KhazaneType Type => KhazaneType.Naghd;

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string DateSh => DateM.MiladiToCalture();

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

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid
	{
		get
		{
			if (moeinGuid == Guid.Empty)
			{
				moeinGuid = ParentDefaults.MoeinCoding.CLSMoein7;
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
	public string PriceString => base.Price.NpToString("N0", 0);

	public override string DescriptionForUi
	{
		get
		{
			if (!string.IsNullOrEmpty(_desc))
			{
				return _desc;
			}
			if (string.IsNullOrEmpty(_desc))
			{
				_desc = SandoghInfo?.Name ?? "";
			}
			if (!string.IsNullOrEmpty(Description))
			{
				_desc = _desc + " شرح سطر : " + Description;
			}
			return _desc;
		}
	}

	[JsonIgnore]
	public TafsilBusiness SandoghInfo => TafsilBusiness.Get(base.ContextGuid, TafsilGuid);

	public override FileManagement_Parts Part { get; }

	public new Guid MasterGuid { get; set; }

	public TafsilBusiness GetSandoughInfo(CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return TafsilBusiness.Get(base.ContextGuid, TafsilGuid, token, tranName);
	}

	public Task<TafsilBusiness> GetSandoughInfoAsync(EnSystem system, CancellationToken token, string tranName = "")
	{
		return TafsilBusiness.GetAsync(base.ContextGuid, system, TafsilGuid, tranName, token, "GetSandoughInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionNaghdBusiness.cs");
	}

	public ReceptionNaghdBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> CheckValidationAsync(EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			TafsilBusiness tafsilBusiness = await GetSandoughInfoAsync(system, token, tranName);
			if (TafsilGuid == Guid.Empty || tafsilBusiness == null || tafsilBusiness.HType != HesabType.Sandogh)
			{
				res.AddError(system, "حساب وارد شده جهت صندوق معتبر نمیباشد.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionNaghdBusiness.cs", 64);
			}
			if (base.Price <= 0m && system != EnSystem.Transfer)
			{
				res.AddError(system, "مبلغ وارد شده جهت واریز به صندوق معتبر نمیباشد.", "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionNaghdBusiness.cs", 66);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionNaghdBusiness.cs", 70);
			res.AddReturnedValue(ex, "", "CheckValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionNaghdBusiness.cs", 71);
		}
		return res;
	}

	public async Task<string> GetAndroidPrintTextAsync(EnSystem system, CancellationToken token, string tranName)
	{
		try
		{
			return "نقدی, " + (await GetSandoughInfoAsync(system, token, tranName))?.Name;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAndroidPrintTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Reception\\ReceptionNaghdBusiness.cs", 84);
			throw;
		}
	}

	public override Task<string> GetShortDescriptionAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return GetAndroidPrintTextAsync(system, token, tranName);
	}
}
