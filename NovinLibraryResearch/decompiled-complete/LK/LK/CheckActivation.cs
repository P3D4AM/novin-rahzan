using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.LkService;
using SharedActivation.LKLogics;

namespace LK;

public class CheckActivation
{
	public static bool? _tolidResult;

	private static long _ClientlkSerial = 0L;

	private static string _softwareSerial_Hozor = "";

	private static string _softwareSerial_Scalel = "";

	public static ReturnedSaveFuncInfoWithValue<Level_Hozor> CheckActivation_Hozor(string contextGuid, ILkUiFactory owner, bool ignoreHttpExceptions, ref bool isDemo)
	{
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Invalid comparison between Unknown and I4
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Invalid comparison between Unknown and I4
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		ReturnedSaveFuncInfoWithValue<Level_Hozor> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<Level_Hozor>();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2 = Demo.IsDemo(owner, Package.HoghoghVaDastmozd, ignoreHttpExceptions: true);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 35);
			isDemo = returnedSaveFuncInfoWithValue2.value;
			if (!isDemo)
			{
				Task.Run(() => CheckLkWithWebServiceAsync(Package.HoghoghVaDastmozd, ignoreHttpExceptions));
			}
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.HoghoghVaDastmozd);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 41);
			RegistryObject value = regObject.value;
			ReturnedSaveFuncInfoWithValue<string> clientNumber = GetClientNumber(Package.HoghoghVaDastmozd);
			returnedSaveFuncInfoWithValue.AddReturnedValue(clientNumber, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 45);
			string text = SoftwareSerialManager.GetSoftwareSerial_Hozor().value.RemoveNoNumbers();
			string text2 = LK.GetSerialNumber(owner, Package.HoghoghVaDastmozd).ToString().RemoveNoNumbers()
				.Trim();
			if (isDemo)
			{
				isDemo = true;
				Process.Start("http://" + ResourceManagement.TextBrandResourceManagement.Url1);
				Process.Start("http://" + ResourceManagement.TextBrandResourceManagement.Url2);
				Process.Start("http://" + ResourceManagement.TextBrandResourceManagement.Url3);
				if ((int)((Form)owner.NewFrmDemo(contextGuid, Package.HoghoghVaDastmozd)).ShowDialog((IWin32Window)(Form)owner) == 1)
				{
					returnedSaveFuncInfoWithValue.AddReturnedValue(value.SetIsDemo(value: true, Package.HoghoghVaDastmozd), ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 65);
					CLSRegistry.SetRegObject(value, Package.HoghoghVaDastmozd);
					returnedSaveFuncInfoWithValue.AddReturnedValue(CLSRegistry.SaveObject(Package.HoghoghVaDastmozd), ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 67);
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level5;
					return returnedSaveFuncInfoWithValue;
				}
				RunProcess(text2 + text);
				returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
				return returnedSaveFuncInfoWithValue;
			}
			if (Demo.TinyErrCode != "0" && !string.IsNullOrEmpty(Demo.TinyErrCode) && !value.GetIsDemo())
			{
				Demo.SetTinyErrCode(Demo.TinyErrCode, owner, Package.HoghoghVaDastmozd);
				ReturnedSaveFuncInfoWithValue<string> clientSet = CLSActivation.GetClientSet(Package.HoghoghVaDastmozd);
				returnedSaveFuncInfoWithValue.AddReturnedValue(clientSet, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 83);
				string value2 = clientSet.value;
				if (!Client.notvalid)
				{
					returnedSaveFuncInfoWithValue.AddReturnedValue(CLSActivation.SetClientSet("1", Package.HoghoghVaDastmozd), ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 88);
					_softwareSerial_Hozor = Client.SoftSerial;
					_ClientlkSerial = Client.LkSerial;
					return CLSActivation.ApplicationHozorType(Client.SoftSerial, Client.LkSerial.ToString(), Client.ActiveCode, owner, ignoreHttpExceptions: true);
				}
				if (value2 == "1")
				{
					Process.Start("http://" + ResourceManagement.TextBrandResourceManagement.Url1);
					Process.Start("http://" + ResourceManagement.TextBrandResourceManagement.Url2);
					Process.Start("http://" + ResourceManagement.TextBrandResourceManagement.Url3);
					MessageBox.Show("خطای کد 1 قفل سخت افزاری. 102");
					Process.GetCurrentProcess().Kill();
				}
			}
			ReturnedSaveFuncInfoWithValue<string> softwareSerial_Hozor = GetSoftwareSerial_Hozor();
			returnedSaveFuncInfoWithValue.AddReturnedValue(softwareSerial_Hozor, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 105);
			_ = softwareSerial_Hozor.value;
			if (string.IsNullOrEmpty(text2))
			{
				MessageBox.Show((IWin32Window)owner, "قفل سخت افزاری شناسایی نشد،\r\nلطفا قفل سخت افزاری را متصل نموده و مجددا برنامه را اجرا نمایید.", "پیغام سیستم", (MessageBoxButtons)0, (MessageBoxIcon)16, (MessageBoxDefaultButton)0, (MessageBoxOptions)524288);
				Process.GetCurrentProcess().Kill();
			}
			ServerTCP.StartServer(Package.HoghoghVaDastmozd);
			string text3 = text2;
			string text4 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level1(text, text3, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
			string text5 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level2(text, text3, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
			string text6 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level3(text, text3, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
			string text7 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level4(text, text3, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
			string text8 = ActivationCodeClass.ReturnActiveCode(ClsActivation.Level5(text, text3, HozoorPackage.General, (short)0, (short)0, DateTime.Now));
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue3 = checkDays(Package.HoghoghVaDastmozd, owner);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 122);
			ReturnedSaveFuncInfoWithValue<string> hoghoghActivationCode = CLSActivation.GetHoghoghActivationCode();
			returnedSaveFuncInfoWithValue.AddReturnedValue(hoghoghActivationCode, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 124);
			string value3 = hoghoghActivationCode.value;
			if (returnedSaveFuncInfoWithValue3.value)
			{
				if (value3 == text4)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level1;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text5)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level2;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text6)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level3;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text7)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level4;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text8)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level5;
					return returnedSaveFuncInfoWithValue;
				}
			}
			Form val = (Form)owner.NewFrmActivation(contextGuid, new AttendancePackageClass(), Package.HoghoghVaDastmozd);
			val.TopMost = true;
			if ((int)val.ShowDialog((IWin32Window)(Form)owner) == 1)
			{
				returnedSaveFuncInfoWithValue3 = checkDays(Package.HoghoghVaDastmozd, owner);
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 161);
				value3 = CLSActivation.GetHoghoghActivationCode().value;
				if (!returnedSaveFuncInfoWithValue3.value || value3 == "")
				{
					ServerTCP.StopServer();
					RunProcess(text2 + text);
					returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text4)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level1;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text5)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level2;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text6)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level3;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text7)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level4;
					return returnedSaveFuncInfoWithValue;
				}
				if (value3 == text8)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level5;
					return returnedSaveFuncInfoWithValue;
				}
			}
			else
			{
				RunProcess(text2 + text);
				ServerTCP.StopServer();
			}
			ServerTCP.StopServer();
			returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
		}
		catch (Exception exception)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(exception, "", "CheckActivation_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 206);
			ServerTCP.StopServer();
			returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static async Task CheckLkWithWebServiceAsync(Package package, bool ignoreHttpExceptions)
	{
		_ = 1;
		try
		{
			await Task.Delay(60000);
			await Demo.CheckLKWithWebServiceAsync(null, package, ignoreHttpExceptions: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLkWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 391);
		}
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetClientNumber(Package package)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			new RegistryObject();
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = RegistryObject.ReadRegistryKey(package);
			returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue2.value;
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetClientNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1026);
			string clientNetNumber = RegistryObject.FromJson(returnedSaveFuncInfoWithValue2.value).GetClientNetNumber();
			returnedSaveFuncInfoWithValue.value = clientNetNumber;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetClientNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1036);
			WebErrorLog.Instance.StartLog(ex, "", "GetClientNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1037);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<HozoorPackage> GetHozoorType(Package package)
	{
		ReturnedSaveFuncInfoWithValue<HozoorPackage> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<HozoorPackage>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = RegistryObject.ReadRegistryKey(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetHozoorType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1049);
			RegistryObject registryObject = RegistryObject.FromJson(returnedSaveFuncInfoWithValue2.value);
			returnedSaveFuncInfoWithValue.value = (HozoorPackage)registryObject.GetSoftwareType().ParseToInt();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = HozoorPackage.General;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetHozoorType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1057);
			WebErrorLog.Instance.StartLog(ex, "", "GetHozoorType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1058);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static string ResetDataBasePass(string LK_Serial, string SoftSerial)
	{
		try
		{
			string text = "  ";
			double num = Conversion.Val(LK_Serial);
			double num2 = Conversion.Val(SoftSerial);
			text = Convert.ToString((Math.Round(num * 2.0 + 10.0 * num2 - 0.024) - 4000.0).ToString().Substring(0, 10));
			if (text.Length < 10)
			{
				while (text.Length == 10)
				{
					text += "1";
				}
			}
			return text;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ResetDataBasePass", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1120);
		}
		return "";
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetSoftwareSerial_Hozor()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			if (!string.IsNullOrEmpty(_softwareSerial_Hozor))
			{
				returnedSaveFuncInfoWithValue.value = _softwareSerial_Hozor;
				return returnedSaveFuncInfoWithValue;
			}
			ReturnedSaveFuncInfoWithValue<string> softwareSerial_Hozor = SoftwareSerialManager.GetSoftwareSerial_Hozor();
			returnedSaveFuncInfoWithValue.AddReturnedValue(softwareSerial_Hozor, ReturnedState.None, "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1159);
			returnedSaveFuncInfoWithValue.value = softwareSerial_Hozor.value;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1165);
			WebErrorLog.Instance.StartLog(ex, "", "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1166);
		}
		return returnedSaveFuncInfoWithValue;
	}

	private static ReturnedSaveFuncInfoWithValue<bool> checkDays(Package package, ILkUiFactory owner)
	{
		ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<bool>();
		int num = 0;
		try
		{
			num = 1;
			ReturnedSaveFuncInfoWithValue<short> numberOfDays = CLSActivation.GetNumberOfDays(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(numberOfDays, ReturnedState.None, "checkDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1202);
			short value = numberOfDays.value;
			num = 3;
			num = 4;
			if (value < 0)
			{
				returnedSaveFuncInfoWithValue.value = false;
				return returnedSaveFuncInfoWithValue;
			}
			num = 10;
			num = 27;
			ReturnedSaveFuncInfoWithValue<DateTime> activeDate = CLSActivation.GetActiveDate(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(activeDate, ReturnedState.None, "checkDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1216);
			DateTime value2 = activeDate.value;
			long num2 = DateAndTime.DateDiff((DateInterval)4, value2, DateTime.Now, (FirstDayOfWeek)1, (FirstWeekOfYear)1);
			num = 28;
			if (num2 > value)
			{
				num = 29;
				ReturnedSaveFuncInfoWithValue<string> softwareSerial_Scale = SoftwareSerialManager.GetSoftwareSerial_Scale();
				returnedSaveFuncInfoWithValue.AddReturnedValue(softwareSerial_Scale, ReturnedState.None, "checkDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1224);
				string value3 = softwareSerial_Scale.value;
				RunProcess($"{LK.GetSerialNumber(owner, package)}{value3}".Trim());
				num = 30;
				returnedSaveFuncInfoWithValue.value = false;
				return returnedSaveFuncInfoWithValue;
			}
			num = 35;
			returnedSaveFuncInfoWithValue.value = true;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = false;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "checkDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1237);
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}//number of days =  " + CLSActivation.GetNumberOfDays(package)?.ToString() + "active date = " + CLSActivation.GetActiveDate(package), "checkDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1238);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static void RunProcess(string lkSerial)
	{
		if (string.IsNullOrEmpty(lkSerial))
		{
			throw new ArgumentException("Lk serial is null or empty!");
		}
		if (lkSerial.RemoveNoNumbers() != lkSerial)
		{
			throw new ArgumentException("Lk serial contains invalid characters! " + lkSerial);
		}
		try
		{
			Process.Start(ResourceManagement.TextBrandResourceManagement.CRMBaseURL + "/extend/code/" + lkSerial);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RunProcess", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CheckActivation.cs", 1298);
		}
	}
}
