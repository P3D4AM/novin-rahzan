using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;

namespace EntityCache.DescriptionParser;

public static class KhazaneDescParser
{
	public static string Id2 => "[شماره]";

	public static string ReceptionDescription => "[شرح دریافت]";

	public static string PardakhtDescription => "[شرح پرداخت]";

	public static string HavaleNo => "[شماره حواله]";

	public static string CheckNo => "[شماره چک]";

	public static string DateSarresidSh => "[تاریخ چک]";

	public static string DetailDescription => "[شرح سطر]";

	public static string BankName => "[نام بانک]";

	public static async Task<string> GetPardakhtDescAsync(this PardakhtBusiness p, CancellationToken token, string tranName)
	{
		string desc = "";
		try
		{
			desc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.PardakhtDescription;
			if (string.IsNullOrEmpty(desc))
			{
				desc = "سند پرداخت شماره : [شماره] [شرح پرداخت]";
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(token, tranName)).ToString());
			desc = desc.Replace(PardakhtDescription, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 32);
		}
		return desc;
	}

	public static async Task<string> GetPardakhtNaghdDescAsync(this PardakhtNaghdBusiness p, PardakhtBusiness master, CancellationToken token, string tranName)
	{
		string descNaghd = "";
		try
		{
			descNaghd = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.PardakhtNaghdDescription;
			if (string.IsNullOrEmpty(descNaghd))
			{
				descNaghd = "پرداخت نقدی طی برگه شماره : [شماره] بابت : [شرح پرداخت] [شرح سطر]";
			}
			string text = descNaghd;
			string id = Id2;
			descNaghd = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			descNaghd = descNaghd.Replace(PardakhtDescription, master.Description);
			descNaghd = descNaghd.Replace(DetailDescription, p.RowDesc);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtNaghdDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 47);
		}
		return descNaghd;
	}

	public static async Task<string> GetPardakhtBankDescAsync(this PardakhtBankBusiness p, PardakhtBusiness master, CancellationToken token, string tranName)
	{
		string bankDesc = "";
		try
		{
			bankDesc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.PardakhtHavaleDescription;
			if (string.IsNullOrEmpty(bankDesc))
			{
				bankDesc = "پرداخت حواله شماره : [شماره حواله] بابت : [شرح پرداخت] [شرح سطر]";
			}
			string text = bankDesc;
			string id = Id2;
			bankDesc = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			bankDesc = bankDesc.Replace(PardakhtDescription, master.Description);
			bankDesc = bankDesc.Replace(DetailDescription, p.RowDesc);
			bankDesc = bankDesc.Replace(HavaleNo, p.Number);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtBankDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 63);
		}
		return bankDesc;
	}

	public static async Task<string> GetPardakhtCheckMDescAsync(this PardakhtCheckMBusiness p, PardakhtBusiness master, CancellationToken token, string tranName)
	{
		string checkMDesc = "";
		try
		{
			checkMDesc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.PardakhtCheckMDescription;
			if (string.IsNullOrEmpty(checkMDesc))
			{
				checkMDesc = "پرداخت چک وارده شماره : [شماره چک] بابت: [شرح پرداخت] [شرح سطر]";
			}
			string text = checkMDesc;
			string id = Id2;
			checkMDesc = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			checkMDesc = checkMDesc.Replace(PardakhtDescription, master.Description);
			checkMDesc = checkMDesc.Replace(DetailDescription, p.RowDesc);
			ReceptionCheckBusiness receptionCheckBusiness = await ReceptionCheckBusiness.GetAsync(p.ContextGuid, p.GuidCheck, token, tranName);
			if (receptionCheckBusiness != null)
			{
				checkMDesc = checkMDesc.Replace(CheckNo, receptionCheckBusiness.Number);
				checkMDesc = checkMDesc.Replace(DateSarresidSh, receptionCheckBusiness.DateSh);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtCheckMDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 84);
		}
		return checkMDesc;
	}

	public static async Task<string> GetPardakhtCheckShDescAsync(this PardakhtCheckShBusiness p, PardakhtBusiness master, CancellationToken token, string tranName)
	{
		string checkShDesc = "";
		try
		{
			checkShDesc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.PardakhtCheckSHDescription;
			if (string.IsNullOrEmpty(checkShDesc))
			{
				checkShDesc = "پرداخت چک شخصی شماره: [شماره چک] به تاریخ : [تاریخ چک] بابت [شرح پرداخت] [شرح سطر]";
			}
			string text = checkShDesc;
			string id = Id2;
			checkShDesc = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			checkShDesc = checkShDesc.Replace(PardakhtDescription, master.Description);
			checkShDesc = checkShDesc.Replace(DetailDescription, p.RowDesc);
			checkShDesc = checkShDesc.Replace(CheckNo, p.Number);
			checkShDesc = checkShDesc.Replace(DateSarresidSh, p.DateSarresidSh);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPardakhtCheckShDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 101);
		}
		return checkShDesc;
	}

	public static async Task<string> GetReceptionDescAsync(this ReceptionBusiness p, CancellationToken token, string tranName)
	{
		string desc = "";
		try
		{
			desc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.ReceptionDescrption;
			if (string.IsNullOrEmpty(desc))
			{
				desc = "سند دریافت شماره : [شماره] [شرح دریافت]";
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(token, tranName)).ToString());
			desc = desc.Replace(ReceptionDescription, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 115);
		}
		return desc;
	}

	public static async Task<string> GetReceptionNaghdDescAsync(this ReceptionNaghdBusiness p, ReceptionBusiness master, CancellationToken token, string tranName)
	{
		string descNaghd = "";
		try
		{
			descNaghd = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.ReceptionNaghdDescription;
			if (string.IsNullOrEmpty(descNaghd))
			{
				descNaghd = "دریافت نقدی طی برگه شماره : [شماره] بابت : [شرح دریافت] [شرح سطر]";
			}
			string text = descNaghd;
			string id = Id2;
			descNaghd = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			descNaghd = descNaghd.Replace(ReceptionDescription, master.Description);
			descNaghd = descNaghd.Replace(DetailDescription, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionNaghdDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 130);
		}
		return descNaghd;
	}

	public static async Task<string> GetReceptionBankDescAsync(this ReceptionHavaleBusiness p, ReceptionBusiness master, CancellationToken token, string tranName)
	{
		string bankDesc = "";
		try
		{
			bankDesc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.ReceptionHavaleDescription;
			if (string.IsNullOrEmpty(bankDesc))
			{
				bankDesc = "دریافت حواله شماره : [شماره حواله] بابت : [شرح دریافت] [شرح سطر]";
			}
			string text = bankDesc;
			string id = Id2;
			bankDesc = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			bankDesc = bankDesc.Replace(ReceptionDescription, master.Description);
			bankDesc = bankDesc.Replace(DetailDescription, p.Description);
			bankDesc = bankDesc.Replace(HavaleNo, p.Number);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionBankDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 146);
		}
		return bankDesc;
	}

	public static async Task<string> GetReceptionCheckDescAsync(this ReceptionCheckBusiness p, ReceptionBusiness master, CancellationToken token, string tranName)
	{
		string checkMDesc = "";
		try
		{
			checkMDesc = LocalSettingBusiness.GetSetting(p.ContextGuid).AutoDescription.ReceptionCheckDescription;
			if (string.IsNullOrEmpty(checkMDesc))
			{
				checkMDesc = "دریافت چک شماره : [شماره چک] [نام بانک] به تاریخ : [تاریخ چک] بابت : [شرح دریافت] [شرح سطر]";
			}
			string text = checkMDesc;
			string id = Id2;
			checkMDesc = text.Replace(id, (await master.GetId2Async(token, tranName)).ToString());
			checkMDesc = checkMDesc.Replace(ReceptionDescription, master.Description);
			checkMDesc = checkMDesc.Replace(DetailDescription, p.Description);
			checkMDesc = checkMDesc.Replace(CheckNo, p.Number);
			checkMDesc = checkMDesc.Replace(DateSarresidSh, p.DateSh);
			checkMDesc = checkMDesc.Replace(BankName, p.BankName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetReceptionCheckDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\KhazaneDescParser.cs", 164);
		}
		return checkMDesc;
	}
}
