using System;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using SharedActivation.LKLogics;

namespace LK;

public class CLSActivation
{
	public static short _numberOfDays = 0;

	public static string _scaleNO = "";

	public static DateTime? _activeDate = null;

	public static string _clientNetNumber = "";

	public static string _isClientSet = "";

	public static string _softwareType = "";

	internal static ReturnedSaveFuncInfoWithValue<short> GetNumberOfDays(Package package)
	{
		ReturnedSaveFuncInfoWithValue<short> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<short>();
		try
		{
			if (_numberOfDays > 0)
			{
				returnedSaveFuncInfoWithValue.value = _numberOfDays;
				return returnedSaveFuncInfoWithValue;
			}
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 31);
			_numberOfDays = regObject.value.GetNumberOfdays().RemoveNoNumbers().ParseToShort(0);
			returnedSaveFuncInfoWithValue.value = _numberOfDays;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = 0;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 39);
			WebErrorLog.Instance.StartLog(ex, "", "GetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 40);
		}
		return returnedSaveFuncInfoWithValue;
	}

	internal static ReturnedSaveFuncInfo SetNumberOfDays(short value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_numberOfDays = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 51);
			RegistryObject value2 = regObject.value;
			if (value == 0)
			{
				returnedSaveFuncInfo.AddReturnedValue(value2.SetNumberOfdays("99", package), ReturnedState.None, "SetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 54);
			}
			else
			{
				returnedSaveFuncInfo.AddReturnedValue(value2.SetNumberOfdays($"{value}", package), ReturnedState.None, "SetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 56);
			}
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 61);
			WebErrorLog.Instance.StartLog(ex, "", "SetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 62);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetNumberOfDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 63);
		}
		return returnedSaveFuncInfo;
	}

	internal static ReturnedSaveFuncInfoWithValue<string> GetClientSet(Package package)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 73);
			_isClientSet = regObject.value.GetClientSet().RemoveNoNumbers();
			returnedSaveFuncInfoWithValue.value = _isClientSet;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 81);
			WebErrorLog.Instance.StartLog(ex, "", "GetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 82);
		}
		return returnedSaveFuncInfoWithValue;
	}

	internal static ReturnedSaveFuncInfo SetClientSet(string value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_isClientSet = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 93);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetClientSet(value, package), ReturnedState.None, "SetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 95);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 100);
			WebErrorLog.Instance.StartLog(ex, "", "SetClientSet", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 101);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfo SetClientNetNumber(string value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_clientNetNumber = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetClientNetNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 137);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetClientNetNumber(value, package), ReturnedState.None, "SetClientNetNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 139);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetClientNetNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 144);
			WebErrorLog.Instance.StartLog(ex, "", "SetClientNetNumber", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 145);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<DateTime> GetActiveDate(Package package)
	{
		ReturnedSaveFuncInfoWithValue<DateTime> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<DateTime>();
		try
		{
			if (_activeDate.HasValue)
			{
				returnedSaveFuncInfoWithValue.value = _activeDate.Value;
				return returnedSaveFuncInfoWithValue;
			}
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 202);
			RegistryObject value = regObject.value;
			_activeDate = value.GetActiveDate();
			returnedSaveFuncInfoWithValue.value = _activeDate.Value;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = DateTime.Now.AddYears(-10);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 210);
			WebErrorLog.Instance.StartLog(ex, "", "GetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 211);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetActiveDate(DateTime value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_activeDate = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 222);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetActiveDate(value, package), ReturnedState.None, "SetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 224);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 229);
			WebErrorLog.Instance.StartLog(ex, "", "SetActiveDate", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 230);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfo SetSoftWareType(string value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_softwareType = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetSoftWareType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 265);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetSoftwareType(value, package), ReturnedState.None, "SetSoftWareType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 267);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSoftWareType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 272);
			WebErrorLog.Instance.StartLog(ex, "", "SetSoftWareType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 273);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<Level_Accounting> ApplicationType(ILkUiFactory owner, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<Level_Accounting>();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2 = Demo.IsDemo(owner, Package.Accounting, ignoreHttpExceptions);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 283);
			ReturnedSaveFuncInfoWithValue<string> softwareSerial_Accounting = SoftwareSerialManager.GetSoftwareSerial_Accounting();
			returnedSaveFuncInfoWithValue.AddReturnedValue(softwareSerial_Accounting, ReturnedState.None, "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 285);
			string value = softwareSerial_Accounting.value;
			ReturnedSaveFuncInfoWithValue<string> accountingActivationCode = GetAccountingActivationCode();
			returnedSaveFuncInfoWithValue.AddReturnedValue(accountingActivationCode, ReturnedState.None, "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 288);
			ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue3 = ApplicationType(ActivationCode: accountingActivationCode.value, SoftwareSerial: value, HardwareSerial: returnedSaveFuncInfoWithValue2.value ? Demo.LKSerial : LK.GetSerialNumber(owner, Package.Accounting).ToString(), owner: owner);
			returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 291);
			returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue3.value;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = Level_Accounting.none;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 297);
			WebErrorLog.Instance.StartLog(ex, "", "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 298);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<Level_Accounting> ApplicationType(string SoftwareSerial, string HardwareSerial, string ActivationCode, ILkUiFactory owner)
	{
		ReturnedSaveFuncInfoWithValue<Level_Accounting> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<Level_Accounting>();
		try
		{
			HardwareSerial = HardwareSerial.Replace("-", "");
			if (HardwareSerial == "")
			{
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2 = Demo.IsDemo(owner, Package.Accounting, ignoreHttpExceptions: true);
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 311);
				if (returnedSaveFuncInfoWithValue2.value)
				{
					returnedSaveFuncInfoWithValue.value = Level_Accounting.ForoshgahiPishrafteh;
					return returnedSaveFuncInfoWithValue;
				}
				returnedSaveFuncInfoWithValue.value = Level_Accounting.LkIsDissconnected;
				return returnedSaveFuncInfoWithValue;
			}
			if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Accounting_Sade(SoftwareSerial, HardwareSerial, AccountingPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Accounting.ForoshgahiSade;
				return returnedSaveFuncInfoWithValue;
			}
			if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Accounting_Motavaset(SoftwareSerial, HardwareSerial, AccountingPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Accounting.ForoshgahiMotavaset;
				return returnedSaveFuncInfoWithValue;
			}
			if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Accounting_Pishrafteh(SoftwareSerial, HardwareSerial, AccountingPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Accounting.ForoshgahiPishrafteh;
				return returnedSaveFuncInfoWithValue;
			}
			if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Accounting_Jame(SoftwareSerial, HardwareSerial, AccountingPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Accounting.Jame;
				return returnedSaveFuncInfoWithValue;
			}
			returnedSaveFuncInfoWithValue.value = Level_Accounting.none;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 348);
			WebErrorLog.Instance.StartLog(ex, "", "ApplicationType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 349);
			returnedSaveFuncInfoWithValue.value = Level_Accounting.none;
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<Level_Hozor> ApplicationHozorType(string SoftwareSerial, string HardwareSerial, string ActivationCode, ILkUiFactory owner, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfoWithValue<Level_Hozor> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<Level_Hozor>();
		try
		{
			HardwareSerial = HardwareSerial.Replace("-", "");
			if (HardwareSerial == "")
			{
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2 = Demo.IsDemo(owner, Package.HoghoghVaDastmozd, ignoreHttpExceptions);
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "ApplicationHozorType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 363);
				if (returnedSaveFuncInfoWithValue2.value)
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.Level4;
				}
				else
				{
					returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
				}
				return returnedSaveFuncInfoWithValue;
			}
			if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Level1(SoftwareSerial, HardwareSerial, HozoorPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Hozor.Level1;
			}
			else if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Level2(SoftwareSerial, HardwareSerial, HozoorPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Hozor.Level2;
			}
			else if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Level3(SoftwareSerial, HardwareSerial, HozoorPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Hozor.Level3;
			}
			else if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Level4(SoftwareSerial, HardwareSerial, HozoorPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Hozor.Level4;
			}
			else if (ActivationCode == ActivationCodeClass.ReturnActiveCode(ClsActivation.Level5(SoftwareSerial, HardwareSerial, HozoorPackage.General, (short)0, (short)0, DateTime.Now)))
			{
				returnedSaveFuncInfoWithValue.value = Level_Hozor.Level5;
			}
			else
			{
				returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ApplicationHozorType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 386);
			WebErrorLog.Instance.StartLog(ex, "", "ApplicationHozorType", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 387);
			returnedSaveFuncInfoWithValue.value = Level_Hozor.None;
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<string> RemoveDays(string activationCode, Package package)
	{
		int num = 10000;
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = activationCode
		};
		try
		{
			if (string.IsNullOrEmpty(activationCode))
			{
				returnedSaveFuncInfoWithValue.AddInformation("string.IsNullOrEmpty(activationCode)", "", "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 576);
				return returnedSaveFuncInfoWithValue;
			}
			num = 10001;
			if (activationCode.Length < 10)
			{
				returnedSaveFuncInfoWithValue.AddInformation("activationCode.Length < 10", "", "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 582);
				return returnedSaveFuncInfoWithValue;
			}
			num = 2;
			if (activationCode.Length > 12)
			{
				num = 3;
				if (activationCode.Length == 16)
				{
					num = 4;
					returnedSaveFuncInfoWithValue.AddReturnedValue(SetSoftWareType(activationCode.Substring(activationCode.Length - 1, 1) + activationCode.Substring(0, 1), package), ReturnedState.None, "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 592);
					num = 5;
					activationCode = activationCode.Remove(0, 1);
					num = 6;
					activationCode = activationCode.Remove(activationCode.Length - 1, 1);
				}
				else
				{
					returnedSaveFuncInfoWithValue.AddReturnedValue(SetSoftWareType("00", package), ReturnedState.None, "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 599);
				}
				num = 8;
				returnedSaveFuncInfoWithValue.AddReturnedValue(SetClientNetNumber(activationCode.Substring(activationCode.Length - 1, 1) + activationCode.Substring(0, 1), package), ReturnedState.None, "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 601);
				num = 9;
				activationCode = activationCode.Remove(0, 1);
				num = 10;
				activationCode = activationCode.Remove(activationCode.Length - 1, 1);
				num = 11;
				returnedSaveFuncInfoWithValue.AddReturnedValue(SetNumberOfDays((activationCode.Substring(11, 1) + activationCode.Substring(4, 1)).ParseToShort(0), package), ReturnedState.None, "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 607);
				num = 12;
				activationCode = activationCode.Remove(4, 1).Remove(10, 1);
				num = 13;
				returnedSaveFuncInfoWithValue.value = activationCode;
			}
			num = 14;
			returnedSaveFuncInfoWithValue.value = activationCode;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"{num},{activationCode}", "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 620);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "RemoveDays", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 621);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<long> GetLKSerial(Package package)
	{
		ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<long>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 866);
			long lkSerial = regObject.value.GetLkSerial();
			returnedSaveFuncInfoWithValue.value = lkSerial;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = 0L;
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 875);
			WebErrorLog.Instance.StartLog(ex, "", "GetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 876);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetLKSerial(Package package, long value, bool ignoreHttpExceptions)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = Demo.IsDemo(null, package, ignoreHttpExceptions);
			returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1121);
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1123);
			RegistryObject value2 = regObject.value;
			if (returnedSaveFuncInfoWithValue.value)
			{
				returnedSaveFuncInfo.AddReturnedValue(value2.SetLkSerial(1234567890L, package), ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1127);
				CLSRegistry.SetRegObject(value2, package);
			}
			else if (value == 0L)
			{
				returnedSaveFuncInfo.AddReturnedValue(value2.SetLkSerial(0L, package), ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1134);
				CLSRegistry.SetRegObject(value2, package);
			}
			else
			{
				returnedSaveFuncInfo.AddReturnedValue(value2.SetLkSerial(value, package), ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1139);
				CLSRegistry.SetRegObject(value2, package);
			}
			ReturnedSaveFuncInfoWithValue<long> lKSerial = GetLKSerial(package);
			returnedSaveFuncInfo.AddReturnedValue(lKSerial, ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1144);
			if (lKSerial.value != value)
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = SoftwareSerialManager.Generate_SoftwareSerial_Accounting();
				returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1149);
				string value3 = returnedSaveFuncInfoWithValue2.value;
				returnedSaveFuncInfo.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Accounting(value3), ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1151);
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue3 = SoftwareSerialManager.Generate_SoftwareSerial_Hozor();
				returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1154);
				string value4 = returnedSaveFuncInfoWithValue3.value;
				returnedSaveFuncInfo.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Hozor(value4), ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1156);
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue4 = SoftwareSerialManager.Generate_SoftwareSerial_Scale();
				returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue4, ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1159);
				string value5 = returnedSaveFuncInfoWithValue4.value;
				returnedSaveFuncInfo.AddReturnedValue(SoftwareSerialManager.SetSoftwareSerial_Scale(value5), ReturnedState.None, "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1161);
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1166);
			WebErrorLog.Instance.StartLog(ex, "", "SetLKSerial", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1167);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetAccountingActivationCode()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.Accounting);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetAccountingActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1177);
			RegistryObject value = regObject.value;
			returnedSaveFuncInfoWithValue.value = value.GetAccountingActivationCode();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetAccountingActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1184);
			WebErrorLog.Instance.StartLog(ex, "", "GetAccountingActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1185);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetHoghoghActivationCode()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.HoghoghVaDastmozd);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1396);
			RegistryObject value = regObject.value;
			returnedSaveFuncInfoWithValue.value = value.GetHoghoghActivationCode();
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1403);
			WebErrorLog.Instance.StartLog(ex, "", "GetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1404);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetHoghoghActivationCode(string value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1414);
			RegistryObject value2 = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value2.SetHoghoghActivationCode(value), ReturnedState.None, "SetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1416);
			CLSRegistry.SetRegObject(value2, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1421);
			WebErrorLog.Instance.StartLog(ex, "", "SetHoghoghActivationCode", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\Activation\\CLSActivation.cs", 1422);
		}
		return returnedSaveFuncInfo;
	}
}
