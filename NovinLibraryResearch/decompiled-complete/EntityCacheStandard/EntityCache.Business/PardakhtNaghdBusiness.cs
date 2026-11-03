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

public class PardakhtNaghdBusiness : BaseKhazaneDetail, IPardakhtNaghd, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private Guid moeinGuid;

	private string _desc = "";

	public override KhazaneType Type => KhazaneType.Naghd;

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


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
			if (!string.IsNullOrEmpty(RowDesc))
			{
				_desc = _desc + " شرح سطر : " + RowDesc;
			}
			return _desc;
		}
	}

	public string RowDesc { get; set; }

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

	[JsonIgnore]
	public TafsilBusiness SandoghInfo => TafsilBusiness.Get(base.ContextGuid, TafsilGuid);

	[JsonIgnore]
	public string PriceString => base.Price.NpToString("N0", 0);

	public override FileManagement_Parts Part { get; }

	public new Guid MasterGuid { get; set; }

	public PardakhtNaghdBusiness(string contextGuid)
	{
		base.ContextGuid = contextGuid;
	}

	public ReturnedSaveFuncInfo CheckValidation(EnSystem system)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (GetSandoghInfoAsync(system) != null)
			{
				TafsilBusiness sandoghInfo = SandoghInfo;
				if (sandoghInfo != null && sandoghInfo.HType == HesabType.Sandogh)
				{
					goto IL_005b;
				}
			}
			returnedSaveFuncInfo.AddError(system, "حساب صندوق وارد شده معتبر نمیباشد.".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs", 57);
			goto IL_005b;
			IL_005b:
			if (base.Price <= 0m)
			{
				returnedSaveFuncInfo.AddError(system, "مبلغ وارد شده جهت پرداخت نقدی معتبر نمیباشد.".GetTranslate(), "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs", 59);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs", 63);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs", 64);
		}
		return returnedSaveFuncInfo;
	}

	public async Task<TafsilBusiness> GetSandoghInfoAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return await TafsilBusiness.GetAsync(base.ContextGuid, system, TafsilGuid, tranName, token, "GetSandoghInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs");
	}

	public async Task<string> GetAndroidPrintTextAsync(EnSystem system, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			TafsilBusiness tafsilBusiness = await GetSandoghInfoAsync(system, token, tranName);
			return "نقد".GetTranslate() + " " + (tafsilBusiness?.Name ?? "").GetTranslate();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAndroidPrintTextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs", 78);
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
			WebErrorLog.Instance.StartLog(ex, "", "GetShortDescriptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Khazane\\Pardakht\\PardakhtNaghdBusiness.cs", 91);
			throw;
		}
	}
}
