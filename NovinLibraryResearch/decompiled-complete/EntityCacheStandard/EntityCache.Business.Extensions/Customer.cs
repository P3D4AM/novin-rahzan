using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Business.Extensions;

public static class Customer
{
	public static async Task<string> WebFaceAsync(this ICusCompact entity, string contextGuid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("WebFaceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(entity.PicSign))
				{
					return "";
				}
				return ((await FileInfoBusiness.GetAsync(contextGuid, entity.Guid, FileManagement_Parts.PicCustomer, token))?.SingleOrDefault())?.GetLocalFilePath() ?? "";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebFaceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 26);
				throw;
			}
		}
	}

	public static async Task<string> WebSignatureAsync(this ICusCompact entity, string contextGuid, CancellationToken token = default(CancellationToken))
	{
		using (new JaegerMonitor("WebSignatureAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(entity.PicSign))
				{
					return "";
				}
				return ((await FileInfoBusiness.GetAsync(contextGuid, entity.Guid, FileManagement_Parts.PicSign, token))?.SingleOrDefault())?.GetLocalFilePath() ?? "";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "WebSignatureAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 41);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> ComapreAsync(this WebCus current, string contextGuid, CusBusiness old, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (old == null)
			{
				return res;
			}
			if ((current.Name ?? "").Trim() != (old.Name ?? "").Trim())
			{
				res.AddError("تغییر نام شخص از".GetTranslate() + "  " + old.Name + " " + " به ".GetTranslate() + " " + (current.Name ?? "").GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 52);
			}
			if ((current.Family ?? "").Trim() != (old.Family ?? "").Trim())
			{
				res.AddError("تغییر نام خانوادگی شخص از ".GetTranslate() + " " + (old.Family ?? "").GetTranslate() + " " + " به".GetTranslate() + " " + current.Family, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 53);
			}
			if (current.AreaGuid != old.AreaGuid)
			{
				VisitorAreaBusiness oldObject = await VisitorAreaBusiness.GetAsync(contextGuid, old.AreaGuid ?? Guid.Empty, token, tranName);
				VisitorAreaBusiness visitorAreaBusiness = await VisitorAreaBusiness.GetAsync(contextGuid, current.AreaGuid ?? Guid.Empty, token, tranName);
				if ((oldObject?.Name ?? "").Trim() != (visitorAreaBusiness?.Name ?? "").Trim())
				{
					res.AddError("تغییر مسیر شخص از".GetTranslate() + "  " + oldObject?.Name + " " + " به ".GetTranslate() + " " + (visitorAreaBusiness?.Name ?? "").GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 59);
				}
			}
			if ((current.Address1 ?? "").Trim() != (old.Address1 ?? "").Trim())
			{
				res.AddError("تغییر آدرس اول از".GetTranslate() + " " + (" " + old.Address1).GetTranslate() + " " + " به ".GetTranslate() + " " + (current.Address1 ?? "").GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 61);
			}
			if ((current.Address2 ?? "").Trim() != (old.Address2 ?? "").Trim())
			{
				res.AddError("تغییر آدرس دوم از ".GetTranslate() + " " + (old.Address2 ?? "").GetTranslate() + "  به  " + current.Address2, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 62);
			}
			if ((current.CodeMeli ?? "").Trim() != (old.CodeMeli ?? "").Trim())
			{
				res.AddError("تغییر کدملی شخص از".GetTranslate() + " " + (" " + old.CodeMeli).GetTranslate() + " " + " به".GetTranslate() + " " + (" " + current.CodeMeli).GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 63);
			}
			if ((current.Company ?? "").Trim() != (old.Company ?? "").Trim())
			{
				res.AddError("تغییر نام شرکت از".GetTranslate() + "  " + old.Company + " " + " به".GetTranslate() + " " + (" " + current.Company).GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 64);
			}
			if ((current.Description ?? "").Trim() != (old.Description ?? "").Trim())
			{
				res.AddError("تغییر توضیحات از".GetTranslate() + " " + (" " + old.Description).GetTranslate() + " " + " به".GetTranslate() + " " + (" " + current.Description).GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 65);
			}
			if ((current.EghtesadiNo ?? "").Trim() != (old.EghtesadiNo ?? "").Trim())
			{
				res.AddError("تغییر کد اقتصادی از".GetTranslate() + " " + (" " + old.EghtesadiNo + " ").GetTranslate() + " " + "به".GetTranslate() + " " + (" " + current.EghtesadiNo).GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 66);
			}
			if (current.Etebar != old.Etebar)
			{
				res.AddError("تغییر حداعتبار شخص از".GetTranslate() + " " + $" {old.Etebar:N0} " + " " + "به".GetTranslate() + " " + $" {current.Etebar:N0}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 67);
			}
			if (current.EtebarCheck != old.EtebarCheck)
			{
				res.AddError("تغییر حداعتبار چک از".GetTranslate() + " " + $" {old.EtebarCheck:N0}".GetTranslate() + " " + " به ".GetTranslate() + " " + $"{current.EtebarCheck:N0}", "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 68);
			}
			if ((current.Job ?? "").Trim() != (old.Job ?? "").Trim())
			{
				res.AddError("تغییر شغل شخص از".GetTranslate() + " " + (old.Job ?? "").GetTranslate() + " " + "به".GetTranslate() + " " + current.Job, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 69);
			}
			if ((current.Location ?? "").Trim() != (old.Location ?? "").Trim())
			{
				res.AddError("تغییر موقعیت جغرافیایی از".GetTranslate() + " " + (old.Location ?? "").GetTranslate() + " " + " به".GetTranslate() + "  " + current.Location, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 70);
			}
			if ((current.Password ?? "").Trim() != (old.Password ?? "").Trim())
			{
				res.AddError("تغییر رمز عبور از ".GetTranslate() + " " + (old.Password ?? "").GetTranslate() + " " + " به".GetTranslate() + "  " + current.Password, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 71);
			}
			if ((current.PicFace ?? "").Trim() != (old.PicFace ?? "").Trim())
			{
				res.AddError("تغییر تصویر چهره از".GetTranslate() + "  " + old.PicFace + " به " + current.PicFace, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 72);
			}
			if ((current.PicSign ?? "").Trim() != (old.PicSign ?? "").Trim())
			{
				res.AddError("تغییر تصویر امضا از".GetTranslate() + "  " + old.PicSign + " " + "به".GetTranslate() + " " + current.PicSign, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 73);
			}
			if ((current.User ?? "").Trim() != (old.User ?? "").Trim())
			{
				res.AddError("تغییر نام کاربری از ".GetTranslate() + " " + old.User + " " + " به ".GetTranslate() + " " + current.User, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 74);
			}
			if ((current.Zip ?? "").Trim() != (old.Zip ?? "").Trim())
			{
				res.AddError("تغییر کدپستی از".GetTranslate() + " " + (" " + old.Zip).GetTranslate() + "  به  " + current.Zip, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 75);
			}
			if (current.CustType != old.CustType)
			{
				res.AddError("تغییر نوع شخص از ".GetTranslate() + " " + old.CustType.GetDisplay().GetTranslate() + " به " + current.CustType.GetDisplay().GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 76);
			}
			if (current.GroupGuid != old.GroupGuid)
			{
				CusGroupBusiness oldObject3 = await CusGroupBusiness.GetAsync(contextGuid, old.GroupGuid, token, tranName);
				CusGroupBusiness cusGroupBusiness = await CusGroupBusiness.GetAsync(contextGuid, current.GroupGuid, token, tranName);
				if ((oldObject3?.Name ?? "").Trim() != (cusGroupBusiness?.Name ?? "").Trim())
				{
					res.AddError("تغییر گروه شخص از".GetTranslate() + " " + oldObject3?.Name + " " + " به".GetTranslate() + " " + cusGroupBusiness?.Name, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 84);
				}
			}
			if (current.TitleGuid != old.TitleGuid)
			{
				CusTitleBusiness oldObject4 = await CusTitleBusiness.GetAsync(contextGuid, old.TitleGuid, token, tranName);
				CusTitleBusiness cusTitleBusiness = await CusTitleBusiness.GetAsync(contextGuid, current.TitleGuid, token, tranName);
				if ((oldObject4?.Name ?? "").Trim() != (cusTitleBusiness?.Name ?? "").Trim())
				{
					res.AddError("تغییر عنوان شخص از".GetTranslate() + "  " + oldObject4?.Name + "  " + "به".GetTranslate() + "  " + cusTitleBusiness?.Name, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 91);
				}
			}
			if (current.TafsilGroupGuid != old.TafsilGroupGuid)
			{
				TafsilGroupBusiness oldObject2 = await TafsilGroupBusiness.GetAsync(contextGuid, old.TafsilGroupGuid, EnSystem.None, token, tranName);
				TafsilGroupBusiness tafsilGroupBusiness = await TafsilGroupBusiness.GetAsync(contextGuid, current.TafsilGroupGuid, EnSystem.None, token, tranName);
				if ((oldObject2?.Name ?? "").FixString().Trim() != (tafsilGroupBusiness?.Name ?? "").FixString().Trim())
				{
					res.AddError("تغییر گروه تفصیلی از".GetTranslate() + " " + (" " + oldObject2?.Name).GetTranslate() + "  به  " + (tafsilGroupBusiness?.Name ?? "").GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 98);
				}
			}
			if ((current.Email ?? "").Trim() != (old.Email ?? "").Trim())
			{
				res.AddError("تغییر پست الکترونیک از".GetTranslate() + " " + old.Email + "  به  " + current.Email, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 100);
			}
			if (current.Date.Date != old.Date.Date)
			{
				res.AddError("تغییر تاریخ افتتاح حساب از".GetTranslate() + old.Date.MiladiToCalture() + "به".GetTranslate() + " " + current.Date.MiladiToCalture(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 101);
			}
			if (current.TaxType != old.TaxType)
			{
				res.AddError("تغییر وضعیت پرداخت مالیات از ".GetTranslate() + " " + old.TaxType.GetDisplay().GetTranslate() + "به".GetTranslate() + " " + current.TaxType.GetDisplay().GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 102);
			}
			if (current.Status != old.Status)
			{
				res.AddError("تغییر وضعیت از ".GetTranslate() + " " + old.StatusForDisplay.GetTranslate() + " به " + current.StatusForDisplay.GetTranslate(), "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 103);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 107);
			res.AddReturnedValue(ex, "", "ComapreAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Customer.cs", 108);
		}
		return res;
	}
}
