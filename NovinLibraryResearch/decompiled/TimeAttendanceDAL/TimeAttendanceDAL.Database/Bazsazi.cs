using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.Waiting;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Dal;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.Salary;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Dal.User;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Database;

public class Bazsazi
{
	public static async Task<ReturnedSaveFuncInfo> BazsaziDb(string contextGuid, bool runScripts, CancellationToken token = default(CancellationToken))
	{
		Waiter waiter = new Waiter("شروع بازسازی اطلاعات", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
		ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
		AttLogger.LoggerUi.Information("BazsaziDb is working");
		try
		{
			Waiter obj = waiter;
			if (obj != null)
			{
				obj.Dispose();
			}
			waiter = new Waiter("در حال بررسی بانک اطلاعاتی", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
			AttLogger.LoggerUi.Information("CreteDbIfNotExist is working");
			result = await CLSDatabase.CreateDbIfNotExist(contextGuid);
			if (result.HasError)
			{
				AttLogger.LoggerErrorRSFI.Error("CreateDb has error:" + result.ErrorMessage);
				return result;
			}
			AttLogger.LoggerUi.Information("CreateDbIfNotExist is ok");
			if (runScripts)
			{
				Waiter obj2 = waiter;
				if (obj2 != null)
				{
					obj2.Dispose();
				}
				waiter = new Waiter("در حال اجرای دستورات بانک اطلاعاتی", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				AttLogger.LoggerUi.Information("RunScript is working");
				Waiter obj3 = waiter;
				if (obj3 != null)
				{
					obj3.Dispose();
				}
				result = await RunScript.Run(contextGuid);
				if (result.HasError)
				{
					AttLogger.LoggerErrorRSFI.Error("RunScript has error:" + result.ErrorMessage);
					return result;
				}
				AttLogger.LoggerUi.Information("RunScript is ok");
				Waiter obj4 = waiter;
				if (obj4 != null)
				{
					obj4.Dispose();
				}
				waiter = new Waiter("در حال تنظیم داده های پیش فرض", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				AttLogger.LoggerUi.Information("OutPutDevice.SetDefaults is working");
				result = await OutPutDevice.SetDefaultsAsync(contextGuid, token);
				if (result.HasError)
				{
					AttLogger.LoggerErrorRSFI.Error("OutPutDevice.SetDefaults has error:" + result.ErrorMessage);
					return result;
				}
				AttLogger.LoggerUi.Information("OutPutDevice.SetDefaults is ok");
				Waiter obj5 = waiter;
				if (obj5 != null)
				{
					obj5.Dispose();
				}
				waiter = new Waiter("در حال پردازش ترددها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				AttLogger.LoggerUi.Information("ProgressNotCalcedTaradods is working");
				if (result.HasError)
				{
					AttLogger.LoggerErrorRSFI.Error("ProgressNotCalcedTaradods has error:" + result.ErrorMessage);
					return result;
				}
				AttLogger.LoggerUi.Information("ProgressNotCalcedTaradods is ok");
				Waiter obj6 = waiter;
				if (obj6 != null)
				{
					obj6.Dispose();
				}
				waiter = new Waiter("در حال بررسی سطح دسترسی", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				AttLogger.LoggerUi.Information("MoveSecurityToPermisiion is working");
				result = await UserAttendance.MoveSecurityToPermisiionAsync(contextGuid, token);
				if (result.HasError)
				{
					AttLogger.LoggerErrorRSFI.Error("MoveSecurityToPermisiion has error:" + result.ErrorMessage);
					return result;
				}
				AttLogger.LoggerUi.Information("MoveSecurityToPermisiion is ok");
				TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
				if (!setting.IsGharardadDetConvertedToJson || !setting.IsGharardadDetUpdateTypeAccount)
				{
					Waiter obj7 = waiter;
					if (obj7 != null)
					{
						obj7.Dispose();
					}
					waiter = new Waiter("در حال بررسی قراردادها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
					AttLogger.LoggerUi.Information("ConverGharardadDetToJson is working");
					result = await GharardadNew.ConvertGharardadDetToJsonAsync(contextGuid, token);
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("ConverGharardadDetToJson has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("ConverGharardadDetToJson is ok");
					setting.IsGharardadDetConvertedToJson = true;
					setting.IsGharardadDetUpdateTypeAccount = true;
				}
				if (!setting.GharardadDetUpdateTypeAccountFromFishHesab)
				{
					Waiter obj8 = waiter;
					if (obj8 != null)
					{
						obj8.Dispose();
					}
					waiter = new Waiter("در حال بررسی جزئیات قراردادها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
					AttLogger.LoggerUi.Information("UpadetTypeAccountGharardadDet is working");
					result = await GharardadNew.UpadetTypeAccountGharardadDetAsync(contextGuid, token);
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("UpadetTypeAccountGharardadDet has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("UpadetTypeAccountGharardadDet is ok");
					setting.GharardadDetUpdateTypeAccountFromFishHesab = true;
				}
				if (!setting.IsFishDetConvertedToJson)
				{
					Waiter obj9 = waiter;
					if (obj9 != null)
					{
						obj9.Dispose();
					}
					waiter = new Waiter("در حال بررسی فیشهای حقوقی", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
					AttLogger.LoggerUi.Information("ConvertFishDetToJson is working");
					await Fish.ConvertFishDetToJsonAsync(contextGuid, token);
					AttLogger.LoggerUi.Information("ConvertFishDetToJson is ok");
					setting.IsFishDetConvertedToJson = true;
				}
				setting.UpdateDetails.IsPrsnTafsilUsedUpdate = true;
				AttLogger.LoggerUi.Information("Instance.Save is working");
				Waiter obj10 = waiter;
				if (obj10 != null)
				{
					obj10.Dispose();
				}
				waiter = new Waiter("در حال ذخیره تنظیمات", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				result = await setting.SaveAsync(contextGuid, token);
				if (result.HasError)
				{
					AttLogger.LoggerErrorRSFI.Error("Instance.Save has error:" + result.ErrorMessage);
					return result;
				}
				AttLogger.LoggerUi.Information("Instance.Save is ok");
				AttLogger.LoggerUi.Information("BazsaziDb is ok !!!!!!!!!!!!!!!!!!");
				Waiter obj11 = waiter;
				if (obj11 != null)
				{
					obj11.Dispose();
				}
				waiter = new Waiter("در حال بررسی داده های سرور", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				AttLogger.LoggerUi.Information("UpdateSomeDataAsync is working");
				result = await TimeAttendanceDAL.Utility.Utility.PingAsync("8.8.8.8");
				if (result.HasError)
				{
					string text = "بدلیل اینکه پینگ آدرس 8.8.8.8 پاسخ نمی دهد احتمالا دسترسی به اینترنت ندارید. لذا بروزرسانی تعدادی از جداول بانک اطلاعاتی ممکن است قابل انجام نباشد";
					AttLogger.LoggerErrorRSFI.Error(text + "\r\n " + result.ErrorMessage);
				}
				result = await Initialize.UpdateSomeDataAsync(contextGuid, default(CancellationToken));
				AttLogger.LoggerUi.Information("UpdateSomeDataAsync is ok");
				Waiter obj12 = waiter;
				if (obj12 != null)
				{
					obj12.Dispose();
				}
				waiter = new Waiter("در حال بررسی ترددها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				await LogOnPersonel.MoveDataDeviceToLogonPersonelAsync(contextGuid, default(CancellationToken));
				AttLogger.LoggerUi.Information("MoveDataDeviceToLogonPersonel is ok");
				Waiter obj13 = waiter;
				if (obj13 != null)
				{
					obj13.Dispose();
				}
				waiter = new Waiter("در حال بررسی جدول مشاغل بیمه", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs");
				await InsertMashaghel.InsertAsync(contextGuid, default(CancellationToken));
				AttLogger.LoggerUi.Information("InsertMashaghel is ok");
			}
		}
		catch (Exception ex)
		{
			result.AddReturnedValue((ReturnedState)2, ex.Message, "", "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs", 220);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "BazsaziDb", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\Bazsazi.cs", 221);
		}
		finally
		{
			Waiter obj14 = waiter;
			if (obj14 != null)
			{
				obj14.Dispose();
			}
		}
		return result;
	}
}
