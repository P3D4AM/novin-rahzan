using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.Assistance.Net;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using Resources;
using TINYLib;

namespace LK;

public class Demo
{
	private static bool? _ISDemo = null;

	private static string _LKSerial = "";

	private static CancellationTokenSource _networkLockCheckToken = new CancellationTokenSource();

	private static bool _onlineLkChecked = false;

	private static string tinyErrCode = "0";

	public const int DemoDaysCount = 10;

	public static string TinyErrCode => tinyErrCode;

	public static string _tinyErrDesc => TinyErrCode switch
	{
		"1" => "قفل سخت افزاری پیدا نشد", 
		"2" => "نام کاربری قفل پیدا نشد", 
		"3" => "قفل سخت افزاری تغییر کرده است ", 
		"4" => "خطا در نوشتن در قفل سخت افزاری", 
		"5" => "خطای شبکه در قفل", 
		"6" => " خطا در دریافت و ارسال به قفل سخت افزاری", 
		"7" => "خطا در مجوز ورودی قفل سخت افزاری", 
		"8" => "خطا در خواندن ActiveX", 
		"9" => " شناسه قفل سخت افزاری نامعتبر است ", 
		"10" => "قفل سخت افزاری نامعتبر است", 
		"11" => "ErrorSharedDongle", 
		_ => "", 
	};

	public static string LKSerial
	{
		get
		{
			return "1234567890";
		}
		set
		{
			_LKSerial = value;
		}
	}

	public static void SetTinyErrCode(string value, ILkUiFactory owner, Package package)
	{
		tinyErrCode = value;
		_networkLockCheckToken?.Cancel();
		if (owner == null)
		{
			return;
		}
		RegistryObject value2 = CLSRegistry.GetRegObject(package).value;
		if (value != "0" && !string.IsNullOrEmpty(value) && !value2.GetIsDemo())
		{
			if (package != Package.HoghoghVaDastmozd)
			{
				_networkLockCheckToken = new CancellationTokenSource();
				CancellationToken token = _networkLockCheckToken.Token;
				Task.Run(() => ConnectAsync(owner, package, token));
			}
			else
			{
				Client.NetworkLockTask?.TrySetResult(result: false);
			}
		}
		else
		{
			Client.NetworkLockTask?.TrySetResult(result: false);
		}
	}

	private static async Task ConnectAsync(ILkUiFactory owner, Package package, CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				if (await PreClientConnect.ConnectAsync(owner.ContextGuid, owner, package, token))
				{
					break;
				}
			}
			catch (OperationCanceledException)
			{
				break;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "ConnectAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 90);
			}
			finally
			{
				await Task.Delay(60000);
			}
		}
	}

	private static ReturnedSaveFuncInfo SetIfDemo(ILkUiFactory owner, Package package, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (_ISDemo.HasValue)
			{
				returnedSaveFuncInfo.AddInformation($"_IsDemo has been set to :{_ISDemo}", "", "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 101);
				return returnedSaveFuncInfo;
			}
			returnedSaveFuncInfo.AddReturnedValue(CheckLock(owner, package, ignoreHttpExceptions), ReturnedState.None, "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 105);
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 108);
			long lkSerial = regObject.value.GetLkSerial();
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = LKConnected(owner, package);
			returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 113);
			if (returnedSaveFuncInfoWithValue.value)
			{
				_ISDemo = false;
				returnedSaveFuncInfo.AddInformation("detect connectedLk. so is demo is false.", "", "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 118);
				return returnedSaveFuncInfo;
			}
			if ((LK.GetSerialNumber(owner, package) == 1234567890 || LK.GetSerialNumber(owner, package) <= 0) && (lkSerial <= 0 || lkSerial == 1234567890))
			{
				_ISDemo = true;
			}
			else
			{
				_ISDemo = false;
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 128);
			WebErrorLog.Instance.StartLog(ex, "", "SetIfDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 129);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<bool> IsDemo(ILkUiFactory owner, Package package, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if (!_ISDemo.HasValue)
			{
				returnedSaveFuncInfoWithValue.AddReturnedValue(SetIfDemo(owner, package, ignoreHttpExceptions), ReturnedState.None, "IsDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 139);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "IsDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 143);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "IsDemo", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 144);
		}
		returnedSaveFuncInfoWithValue.value = _ISDemo.HasValue && _ISDemo.Value;
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<int> GetRemDays(Package package)
	{
		ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 155);
			DateTime? dateTime = regObject.value.GetX12mx();
			if (!dateTime.HasValue)
			{
				ReturnedSaveFuncInfoWithValue<RegistryObject> regObject2 = CLSRegistry.GetRegObject(package);
				returnedSaveFuncInfoWithValue.AddReturnedValue(regObject2, ReturnedState.None, "GetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 161);
				RegistryObject value = regObject2.value;
				returnedSaveFuncInfoWithValue.AddReturnedValue(value.SetX12mx(DateTime.Now.Date, package), ReturnedState.None, "GetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 163);
				CLSRegistry.SetRegObject(value, package);
				dateTime = DateTime.Now.Date;
			}
			DateTime value2 = Convert.ToDateTime(dateTime).AddDays(10.0);
			int num = Convert.ToInt32(-DateTime.Now.Date.Subtract(value2).Days);
			returnedSaveFuncInfoWithValue.value = ((num >= 0 && num <= 10) ? num : 0);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = 0;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 175);
			WebErrorLog.Instance.StartLog(ex, "", "GetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 176);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetRemDays(int value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 186);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetX12mx(DateTime.Now.Date, package), ReturnedState.None, "SetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 188);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 193);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetRemDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 194);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetNumberOfDemoRun(Package package)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 204);
			RegistryObject value = regObject.value;
			returnedSaveFuncInfoWithValue.value = value.GetNumberOfDemoRun();
		}
		catch (Exception exception)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(exception, "", "GetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 211);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetNumberOfDemoRun(string value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 221);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetNumberOfDemoRun(value, package), ReturnedState.None, "SetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 223);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "SetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 228);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 229);
			WebErrorLog.Instance.StartLog(ex, "", "SetNumberOfDemoRun", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 230);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfo CheckLock(ILkUiFactory owner, Package package, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			LK.CheckOCX(owner, package, Application.StartupPath);
			Tiny obj = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
			obj.Initialize = true;
			obj.UserPassWord = "1AC71CB943232DAA9537173191E24D8";
			obj.ShowTinyInfo = true;
			Tiny tiny = obj;
			if (tiny.TinyErrCode != 0)
			{
				Tiny obj2 = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
				obj2.Initialize = true;
				obj2.UserPassWord = "67DBD7543A784B58847D4C9D0AE2C3B";
				obj2.ShowTinyInfo = true;
				tiny = obj2;
			}
			if (tiny.TinyErrCode != 0)
			{
				Tiny obj3 = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
				obj3.Initialize = true;
				obj3.UserPassWord = "33905687687C70D395354141A1824B5";
				obj3.ShowTinyInfo = true;
				tiny = obj3;
			}
			long num = tiny.SerialNumber.RemoveNoNumbers().ParseToLong(0L);
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 263);
			long lkSerial = regObject.value.GetLkSerial();
			if (num <= 0)
			{
				num = 1234567890L;
			}
			if (lkSerial > 0 && lkSerial != num && num > 0 && ((int?)tiny?.TinyErrCode).GetValueOrDefault() != 1)
			{
				SoftwareSerialManager.Generate_SoftwareSerial_Accounting();
				SoftwareSerialManager.Generate_SoftwareSerial_Hozor();
				SoftwareSerialManager.Generate_SoftwareSerial_Scale();
			}
			returnedSaveFuncInfo.AddReturnedValue(LK.SetSerialNumber(num, package), ReturnedState.None, "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 277);
			if (tiny.TinyErrCode != 1)
			{
				Task.Run(() => CheckLKWithWebServiceAsync(null, package, ignoreHttpExceptions));
			}
			SetTinyErrCode(tiny.TinyErrCode.ToString(), owner, package);
			if (tiny.TinyErrCode == 0)
			{
				returnedSaveFuncInfo.AddInformation($"Tiny detected successfully serialNumber:{num}", "", "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 284);
			}
			else
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, $"Tiny Error Code:{tiny.TinyErrCode}//{global::Resources.Resources.TinyError}", "", "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 286);
			}
		}
		catch (WebException ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 290);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 291);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 295);
			returnedSaveFuncInfo.AddReturnedValue(ex2, "", "CheckLock", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 296);
		}
		return returnedSaveFuncInfo;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LKInfo>> CheckLKWithWebServiceAsync(LKInfo lk, Package package, bool ignoreHttpExceptions, bool forceCheck = false, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<LKInfo> ret = new ReturnedSaveFuncInfoWithValue<LKInfo>();
		int line = 0;
		try
		{
			Console.WriteLine($"({DateTime.Now})CheckLKWithWebServiceAsync started...");
			line = 1;
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = RegistryObject.ReadListFromObject("LkInfoJson");
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 309);
			LKInfo lKInfo = returnedSaveFuncInfoWithValue.value.FromJson<LKInfo>("CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 310);
			long lkSerial = lk?.LKSerial ?? lKInfo?.LKSerial ?? 0;
			if (lk == null)
			{
				lk = lKInfo;
			}
			line = 2;
			long valueOrDefault = (Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "LkSerial", null)?.ToString()?.ParseToLong(0L)).GetValueOrDefault();
			if (lkSerial <= 0)
			{
				lkSerial = lk?.LKSerial ?? 0;
			}
			if (lkSerial <= 0)
			{
				lkSerial = valueOrDefault;
			}
			if (valueOrDefault != 0L && valueOrDefault != lkSerial)
			{
				lkSerial = valueOrDefault;
			}
			if (((!_onlineLkChecked && lkSerial > 0) || forceCheck) && await NetworkAssistance.CheckIntenetAccessAsync())
			{
				ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue2 = await LkClient.GetLkAsync(lkSerial, ignoreHttpExceptions, token);
				if (!returnedSaveFuncInfoWithValue2.HasError)
				{
					_onlineLkChecked = true;
				}
				if (returnedSaveFuncInfoWithValue2?.value != null)
				{
					RegistryObject.WriteRegistryKey(Json.ToStringJson(returnedSaveFuncInfoWithValue2.value), "LkInfoJson");
					lk = returnedSaveFuncInfoWithValue2.value;
				}
			}
			line = 3;
			ret.value = lk;
			if (ret.HasError)
			{
				return ret;
			}
			line = 8;
			if ((lk != null && DateTime.Now.AddDays(1.0) > lk.ExpireDate && DateTime.Now.DayOfWeek != DayOfWeek.Friday) || (lk?.IsFailed ?? false))
			{
				line = 10;
				ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
				ret.AddReturnedValue(regObject, ReturnedState.None, "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 342);
				regObject.value.CleanActivationCodes();
				line = 11;
			}
			line = 12;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 351);
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 352);
		}
		finally
		{
			Console.WriteLine($"{DateTime.Now})CheckLKWithWebServiceAsync finished.\r\nresult:{!ret.HasError}");
		}
		return ret;
	}

	private static ReturnedSaveFuncInfoWithValue<bool> LKConnected(ILkUiFactory owner, Package package)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "LKConnected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 363);
			long lkSerial = regObject.value.GetLkSerial();
			if (lkSerial > 0 && lkSerial != 1234567890 && lkSerial == LK.GetSerialNumber(owner, package))
			{
				returnedSaveFuncInfoWithValue.value = true;
			}
			else
			{
				returnedSaveFuncInfoWithValue.value = false;
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = false;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "LKConnected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 373);
			WebErrorLog.Instance.StartLog(ex, "", "LKConnected", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 374);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static Task<ReturnedSaveFuncInfoWithValue<long>> CheckLockForLkReaderAsync()
	{
		ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<long>();
		try
		{
			Tiny obj = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
			obj.Initialize = true;
			obj.UserPassWord = "1AC71CB943232DAA9537173191E24D8";
			obj.ShowTinyInfo = true;
			Tiny tiny = obj;
			if (tiny.TinyErrCode != 0)
			{
				Tiny obj2 = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
				obj2.Initialize = true;
				obj2.UserPassWord = "67DBD7543A784B58847D4C9D0AE2C3B";
				obj2.ShowTinyInfo = true;
				tiny = obj2;
			}
			if (tiny.TinyErrCode != 0)
			{
				Tiny obj3 = (Tiny)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("0CA5A93D-F6BD-4398-809A-9F767276EF77")));
				obj3.Initialize = true;
				obj3.UserPassWord = "33905687687C70D395354141A1824B5";
				obj3.ShowTinyInfo = true;
				tiny = obj3;
			}
			if (tiny.TinyErrCode != 0)
			{
				returnedSaveFuncInfoWithValue.AddError(EnSystem.None, $"Tiny Error Code:{tiny.TinyErrCode}//{global::Resources.Resources.TinyError}", "CheckLockForLkReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 405);
			}
			else
			{
				long value = tiny.SerialNumber.RemoveNoNumbers().ParseToLong(0L);
				returnedSaveFuncInfoWithValue.value = value;
			}
		}
		catch (WebException ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLockForLkReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 414);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "CheckLockForLkReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 415);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CheckLockForLkReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 419);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex2, "", "CheckLockForLkReaderAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Demo\\Demo.cs", 420);
		}
		return Task.FromResult(returnedSaveFuncInfoWithValue);
	}
}
