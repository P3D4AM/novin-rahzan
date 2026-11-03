using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.LockService;
using FormCustom.LockService.Forms;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.LkService;

namespace FormCustom.LockService.Class;

internal class ActivationPackage
{
	public Package Package { get; private set; }

	public AccountingPackageClass PackageClass { get; private set; }

	public string ContextGuid { get; private set; }

	public LkDemoInformation CheckDemoResult { get; private set; }

	public long Lk { get; private set; }

	public string SoftwareSerial { get; private set; }

	private ActivationPackage(string contextGuid, AccountingPackageClass packageClass, long lk, Package package, string soft, bool ignoreHttpExceptions)
	{
		try
		{
			ContextGuid = contextGuid;
			Package = package;
			Lk = lk;
			SoftwareSerial = soft;
			PackageClass = packageClass;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 38);
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<ActivationPackage>> GetFactoryAsync(string contextGuid, string version, AccountingPackageClass packageClass, long lk, Package package, string soft, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<ActivationPackage> res = new ReturnedSaveFuncInfoWithValue<ActivationPackage>();
		try
		{
			ActivationPackage activation = new ActivationPackage(contextGuid, packageClass, lk, package, soft, ignoreHttpExceptions);
			ReturnedSaveFuncInfoWithValue<ActivationPackage> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckExpireLock(contextGuid, lk, ignoreHttpExceptions, token), ReturnedState.None, "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 47);
			res.value = activation;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 52);
			res.AddReturnedValue(ex, "", "GetFactoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 53);
		}
		return res;
	}

	internal string GetPackageName(Package packageName)
	{
		try
		{
			return packageName switch
			{
				Package.Accounting => "حسابداری  ", 
				Package.HoghoghVaDastmozd => "  حقوق و دستمزد ", 
				Package.TamdideAccounting => "  حسابداری ", 
				Package.TamdideScale => " ترازو ", 
				Package.TamdideHozor => "حضور و غیاب   ", 
				_ => "", 
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPackageName", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 73);
			return "";
		}
	}

	private static async Task<ReturnedSaveFuncInfo> CheckExpireLock(string contextGuid, long lkSerial, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			new LKInfo();
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = await LkClient.GetLkAsync(lkSerial, ignoreHttpExceptions, token);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 84);
			}
			if (!res.HasError)
			{
				LKInfo lKInfo = returnedSaveFuncInfoWithValue.value;
				if (lKInfo == null)
				{
					lKInfo = new LKInfo
					{
						ExpireDate = DateTime.Now.AddDays(-365.0)
					};
				}
				if (Convert.ToDateTime(lKInfo?.ExpireDate) < DateTime.Now)
				{
					res.AddError("اعتبار قفل سخت افزاری شما در تاریخ " + lKInfo.ExpireDate.MiladiToCalture() + "\r\nبه پایان رسیده است.\r\n" + $"{lKInfo.LKSerial}\r\n" + "لطفا نسبت به تمدید قفل اقدام فرمائید", "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 92);
				}
				else if (lKInfo.IsFailed)
				{
					res.AddError("قفل سخت افزاری شما مجاز به استفاده از نرم افزار نمی باشد\r\nلطفا با پشتیبانی تماس حاصل فرمائید\r\n", "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 98);
				}
			}
		}
		catch (WebException exception)
		{
			res.AddReturnedValue(exception, "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 101);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 104);
			res.AddReturnedValue(ex, "", "CheckExpireLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 105);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> AccountingActivationAsync(LkActivationCodeModel model, string activationCode, List<PackageContainer> packageList, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(activationCode))
			{
				AccountingActivationCode accountingActivationCode = await LkClient.AccountingActivationCodeAsync(model, token);
				if (activationCode.RemoveNoNumbers().ToString() != activationCode)
				{
					return res.AddError(activationCode, "", "AccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 117);
				}
				if (!string.IsNullOrEmpty(accountingActivationCode.ValidationMessage))
				{
					res.AddError(accountingActivationCode.ValidationMessage, "", "AccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 120);
					Process.Start(new ProcessStartInfo
					{
						FileName = "https://crm.novinp.ir/extend/lk/" + model.LkSerial + model.SerialSoft,
						UseShellExecute = true
					});
					return res;
				}
				packageList = accountingActivationCode.Packages?.Where((PackageContainer q) => q != null && q.IsActive && !string.IsNullOrEmpty(q.ActivationCode))?.ToList();
				activationCode = accountingActivationCode.ActivationCode;
			}
			ReturnedSaveFuncInfoWithValue<AccountingPackageClass> returnedSaveFuncInfoWithValue = await NpLockHubInstances.Instance.SoftwareSerialManager.SetAccountingSoftwareSerial(SoftwareSerial, activationCode, packageList, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "AccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 134);
			switch (returnedSaveFuncInfoWithValue.value.Level)
			{
			default:
				res.AddError("خطا در فعالسازی اینترنتی نرم افزار حسابداری \r\n " + res.GetErrorMessage(), "", "AccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 143);
				break;
			case Level_Accounting.ForoshgahiSade:
			case Level_Accounting.ForoshgahiMotavaset:
			case Level_Accounting.ForoshgahiPishrafteh:
			case Level_Accounting.Jame:
				break;
			}
			((Form)new frmPackageResult(ContextGuid, returnedSaveFuncInfoWithValue.value.Level, returnedSaveFuncInfoWithValue.value.GetAllPackages())).ShowDialog();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 149);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> HoghoghVaDastmozdActivationAsync(string activationCode, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.SoftwareSerialManager.SetSoftwareSerial_HozorAsync(SoftwareSerial, default(CancellationToken)), ReturnedState.None, "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 157);
			ReturnedSaveFuncInfoWithValue<string> activationCodeResult = await NpLockHubInstances.Instance.CheckActivation.SetRemoveDaysAsync(activationCode.RemoveNoNumbers(), Package.HoghoghVaDastmozd, default(CancellationToken));
			res.AddReturnedValue(activationCodeResult, ReturnedState.None, "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 159);
			activationCode = activationCodeResult.value;
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetActivationCodeAsync(activationCode.RemoveNoNumbers(), Package.HoghoghVaDastmozd, default(CancellationToken)), ReturnedState.None, "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 161);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetHoghoghActivationCodeAsync(Package, activationCode, default(CancellationToken)), ReturnedState.None, "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 162);
			res.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.GetHozorTypeAsync(SoftwareSerial, Lk.ToString(), activationCode, CheckDemoResult?.IsDemo ?? false, default(CancellationToken)), ReturnedState.None, "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 164);
			if ((await NpLockHubInstances.Instance.ActivationCode.ReturnActiveCodeAsync(activationCode, default(CancellationToken))).value == activationCodeResult.value)
			{
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetActivateDateAsync(DateTime.Now, Package, default(CancellationToken)), ReturnedState.None, "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 167);
				res.AddInformation("زیر بسته ی حقوق و دستمزد با موفقیت فعالسازی شد", "", "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 168);
			}
			else
			{
				res.AddError("خطا در فعالسازی اینترنتی زیر بسته حقوق و دستمزد ", "", "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 171);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HoghoghVaDastmozdActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 174);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> TamdideHozorActivationAsync(string activationCode, LockInformation lockInformation, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((await NpLockHubInstances.Instance.ActivationCode.GetTamdidActivationCodeAsync(SoftwareSerial, Lk.ToString(), AccountingPackage.General, 0, 0, DateTime.Now, default(CancellationToken))).value == activationCode)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetActivateDateAsync(DateTime.Now, Package, default(CancellationToken)), ReturnedState.None, "TamdideHozorActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 185);
				lockInformation.X12Mx = DateTime.Now.Date;
				res.AddInformation("نسخه آزمایشی نرم افزار نرم افزار حضور، غیاب و حقوق، دستمزد(سطح3) ".GetTranslate() + " " + ResourceManagement.TextBrandResourceManagement.CompanyName + "  " + "با موفقیت تمدید گردید،لطفا\u064c نرم افزار را مجدد راه اندازی کنید".GetTranslate(), "پیغام سیستم".GetTranslate(), "TamdideHozorActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 187);
			}
			else
			{
				res.AddError("کد تمدید وارد شده معتبر نمیباشد".GetTranslate(), "", "TamdideHozorActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 190);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TamdideHozorActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 192);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> TamdideScaleActivationAsync(string activationCode, LockInformation lockInformation, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((await NpLockHubInstances.Instance.ActivationCode.GetTamdidActivationCodeAsync(SoftwareSerial, Lk.ToString(), AccountingPackage.General, 0, 0, DateTime.Now, default(CancellationToken))).value == activationCode)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetActivateDateAsync(DateTime.Now, Package, default(CancellationToken)), ReturnedState.None, "TamdideScaleActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 203);
				lockInformation.X12Mx = DateTime.Now.Date;
				res.AddInformation("نسخه آزمایشی نرم افزار ترازو".GetTranslate() + " " + ResourceManagement.TextBrandResourceManagement.CompanyName + " " + "با موفقیت تمدید گردید، لطفا\u064c نرم افزار را مجدد راه اندازی کنید".GetTranslate(), "", "TamdideScaleActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 205);
			}
			else
			{
				res.AddError("کد تمدید وارد شده معتبر نمیباشد".GetTranslate(), "", "TamdideScaleActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 208);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TamdideScaleActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 210);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> TamdideAccountingActivationAsync(string activationCode, LockInformation lockInformation, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((await NpLockHubInstances.Instance.ActivationCode.GetTamdidActivationCodeAsync(SoftwareSerial, Lk.ToString(), AccountingPackage.General, 0, 0, DateTime.Now, default(CancellationToken))).value == activationCode)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.SetActivateDateAsync(DateTime.Now, Package, default(CancellationToken)), ReturnedState.None, "TamdideAccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 221);
				lockInformation.X12Mx = DateTime.Now.Date;
				res.AddInformation("نسخه آزمایشی نرم افزار حسابداری".GetTranslate() + " " + ResourceManagement.TextBrandResourceManagement.CompanyName + " " + "با موفقیت تمدید گردید،لطفا\u064c نرم افزار را مجدد راه اندازی کنید".GetTranslate(), "", "TamdideAccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 223);
			}
			else
			{
				res.AddError("کد تمدید وارد شده معتبر نمیباشد".GetTranslate(), "", "TamdideAccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 226);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TamdideAccountingActivationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\ActivationPackage.cs", 228);
		}
		return res;
	}
}
