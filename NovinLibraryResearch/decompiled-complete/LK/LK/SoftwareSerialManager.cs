using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using SharedActivation.LKLogics;

namespace LK;

public class SoftwareSerialManager
{
	private static string _SoftwareSerial_Hozor = "";

	internal static string _SoftwareSerial_Accounting = "";

	private static string _SoftwareSerial_Scale = "";

	private static string VersionAccounting
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(Application.StartupPath, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	private static string VersionScale
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(Application.StartupPath, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	private static string VersionHozor
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(Application.StartupPath, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	public static ReturnedSaveFuncInfoWithValue<string> Generate_SoftwareSerial_Hozor()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ResetCount(Package.HoghoghVaDastmozd), ReturnedState.None, "Generate_SoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 18);
			returnedSaveFuncInfoWithValue.value = Guid.NewGuid().ToString();
			returnedSaveFuncInfoWithValue.value = Strings.Right(Convert.ToString(Conversion.Fix((object)SoftwareSerialManager.Conv2Number(returnedSaveFuncInfoWithValue.value, true).ToString())), 10);
			returnedSaveFuncInfoWithValue.value = "05" + VersionHozor + returnedSaveFuncInfoWithValue.value;
			returnedSaveFuncInfoWithValue.value += CreateCheckSum(returnedSaveFuncInfoWithValue.value);
			returnedSaveFuncInfoWithValue.AddReturnedValue(SetSoftwareSerial_Hozor(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "Generate_SoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 25);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Generate_SoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 30);
			WebErrorLog.Instance.StartLog(ex, "", "Generate_SoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 31);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<string> Generate_SoftwareSerial_Accounting()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ResetCount(Package.Accounting), ReturnedState.None, "Generate_SoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 50);
			returnedSaveFuncInfoWithValue.value = Guid.NewGuid().ToString();
			returnedSaveFuncInfoWithValue.value.ParseToDouble(0.0, 3);
			string text = SoftwareSerialManager.Conv2Number(returnedSaveFuncInfoWithValue.value, true).ToString();
			returnedSaveFuncInfoWithValue.value = Strings.Right(text, 10);
			returnedSaveFuncInfoWithValue.value = "01" + VersionAccounting + returnedSaveFuncInfoWithValue.value;
			returnedSaveFuncInfoWithValue.value += CreateCheckSum(returnedSaveFuncInfoWithValue.value);
			returnedSaveFuncInfoWithValue.AddReturnedValue(SetSoftwareSerial_Accounting(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "Generate_SoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 57);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Generate_SoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 62);
			WebErrorLog.Instance.StartLog(ex, "", "Generate_SoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 63);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<string> Generate_SoftwareSerial_Scale()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			returnedSaveFuncInfoWithValue.value = Guid.NewGuid().ToString();
			returnedSaveFuncInfoWithValue.value = Strings.Right(Convert.ToString(Conversion.Fix((object)SoftwareSerialManager.Conv2Number(returnedSaveFuncInfoWithValue.value, true).ToString())), 10);
			returnedSaveFuncInfoWithValue.value = "12" + VersionScale + returnedSaveFuncInfoWithValue.value;
			returnedSaveFuncInfoWithValue.value += CreateCheckSum(returnedSaveFuncInfoWithValue.value);
			returnedSaveFuncInfoWithValue.AddReturnedValue(SetSoftwareSerial_Scale(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "Generate_SoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 80);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "Generate_SoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 85);
			WebErrorLog.Instance.StartLog(ex, "", "Generate_SoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 86);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetSoftwareSerial_Hozor()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			string softwareSerial_Hozor = _SoftwareSerial_Hozor;
			if (softwareSerial_Hozor != null && softwareSerial_Hozor.Length >= 10)
			{
				returnedSaveFuncInfoWithValue.value = _SoftwareSerial_Hozor;
				return returnedSaveFuncInfoWithValue;
			}
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.HoghoghVaDastmozd);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 103);
			_SoftwareSerial_Hozor = regObject.value.GetSoftwareSerial_Hozor().RemoveNoNumbers();
			Count(Package.HoghoghVaDastmozd);
			if (_SoftwareSerial_Hozor.Length < 2)
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = Generate_SoftwareSerial_Hozor();
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 110);
				_SoftwareSerial_Hozor = returnedSaveFuncInfoWithValue2.value;
			}
			returnedSaveFuncInfoWithValue.value = _SoftwareSerial_Hozor;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 117);
			returnedSaveFuncInfoWithValue.value = "";
			WebErrorLog.Instance.StartLog(ex, "", "GetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 119);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetSoftwareSerial_Hozor(string value)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_SoftwareSerial_Hozor = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.HoghoghVaDastmozd);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 131);
			RegistryObject value2 = regObject.value;
			value2._softwareSerialHozor = value;
			CLSRegistry.SetRegObject(value2, Package.HoghoghVaDastmozd);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 138);
			WebErrorLog.Instance.StartLog(ex, "", "SetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 139);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSoftwareSerial_Hozor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 140);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetSoftwareSerial_Accounting()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>
		{
			value = ""
		};
		try
		{
			if ((_SoftwareSerial_Accounting?.Length ?? 0) == 17)
			{
				returnedSaveFuncInfoWithValue.value = _SoftwareSerial_Accounting;
				return returnedSaveFuncInfoWithValue;
			}
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.Accounting);
			returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 157);
			string input = regObject.value.GetSoftwareSerial_Accounting() ?? "";
			input = input.RemoveNoNumbers();
			Count(Package.Accounting);
			if (input.Length != 17)
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = Generate_SoftwareSerial_Accounting();
				returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue2.value;
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 167);
				input = returnedSaveFuncInfoWithValue2.value;
			}
			if (input?.Substring(2, 4) != VersionAccounting)
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue3 = Generate_SoftwareSerial_Accounting();
				returnedSaveFuncInfoWithValue.value = returnedSaveFuncInfoWithValue3.value;
				returnedSaveFuncInfoWithValue.AddReturnedValue(regObject, ReturnedState.None, "GetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 174);
				input = returnedSaveFuncInfoWithValue3.value;
			}
			_SoftwareSerial_Accounting = input;
			returnedSaveFuncInfoWithValue.value = _SoftwareSerial_Accounting;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 184);
			WebErrorLog.Instance.StartLog(ex, "", "GetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 185);
		}
		finally
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue4 = returnedSaveFuncInfoWithValue;
			if (returnedSaveFuncInfoWithValue4.value == null)
			{
				string text2 = (returnedSaveFuncInfoWithValue4.value = "");
			}
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetSoftwareSerial_Accounting(string value)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_SoftwareSerial_Accounting = value;
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(Package.Accounting);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "SetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 198);
			RegistryObject value2 = regObject.value;
			value2._softwareSerialAccounting = value;
			CLSRegistry.SetRegObject(value2, Package.Accounting);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 205);
			WebErrorLog.Instance.StartLog(ex, "", "SetSoftwareSerial_Accounting", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 206);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfoWithValue<string> GetSoftwareSerial_Scale()
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			if (_SoftwareSerial_Scale.Length >= 14)
			{
				returnedSaveFuncInfoWithValue.value = _SoftwareSerial_Scale;
				return returnedSaveFuncInfoWithValue;
			}
			_SoftwareSerial_Scale = _SoftwareSerial_Scale.RemoveNoNumbers();
			Count(Package.Accounting);
			if (_SoftwareSerial_Scale.Length < 2)
			{
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue2 = Generate_SoftwareSerial_Scale();
				returnedSaveFuncInfoWithValue.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetSoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 231);
				_SoftwareSerial_Scale = returnedSaveFuncInfoWithValue2.value;
			}
			returnedSaveFuncInfoWithValue.value = _SoftwareSerial_Scale;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = "";
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetSoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 239);
			WebErrorLog.Instance.StartLog(ex, "", "GetSoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 240);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo SetSoftwareSerial_Scale(string value)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_SoftwareSerial_Scale = value;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetSoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 258);
			WebErrorLog.Instance.StartLog(ex, "", "SetSoftwareSerial_Scale", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 259);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfo Count(Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 315);
			RegistryObject value = regObject.value;
			if (value.GetX125() > 1000)
			{
				ReturnedSaveFuncInfoWithValue<RegistryObject> regObject2 = CLSRegistry.GetRegObject(package);
				returnedSaveFuncInfo.AddReturnedValue(regObject2, ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 321);
				RegistryObject value2 = regObject2.value;
				value2.SetX125(1, package);
				CLSRegistry.SetRegObject(value2, package);
				ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = Generate_SoftwareSerial_Hozor();
				returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 326);
				returnedSaveFuncInfo.AddReturnedValue(SetSoftwareSerial_Hozor(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 327);
				returnedSaveFuncInfoWithValue = Generate_SoftwareSerial_Accounting();
				returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 329);
				returnedSaveFuncInfo.AddReturnedValue(SetSoftwareSerial_Accounting(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 330);
				returnedSaveFuncInfoWithValue = GetSoftwareSerial_Scale();
				returnedSaveFuncInfo.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 332);
				returnedSaveFuncInfo.AddReturnedValue(SetSoftwareSerial_Scale(returnedSaveFuncInfoWithValue.value), ReturnedState.None, "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 333);
			}
			else
			{
				value.SetX125(value.GetX125() + 1, package);
				CLSRegistry.SetRegObject(value, package);
			}
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 343);
			WebErrorLog.Instance.StartLog(ex, "", "Count", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 344);
		}
		return returnedSaveFuncInfo;
	}

	public static ReturnedSaveFuncInfo ResetCount(Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfoWithValue<RegistryObject> regObject = CLSRegistry.GetRegObject(package);
			returnedSaveFuncInfo.AddReturnedValue(regObject, ReturnedState.None, "ResetCount", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 354);
			RegistryObject value = regObject.value;
			returnedSaveFuncInfo.AddReturnedValue(value.SetX125(1, package), ReturnedState.None, "ResetCount", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 356);
			CLSRegistry.SetRegObject(value, package);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "ResetCount", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 361);
			WebErrorLog.Instance.StartLog(ex, "", "ResetCount", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\SoftwareSerialManager.cs", 362);
		}
		return returnedSaveFuncInfo;
	}

	private static string CreateCheckSum(string serial)
	{
		if (serial.Length > 16)
		{
			serial = serial.Substring(0, 16);
		}
		int num = 0;
		int num2 = 0;
		num2 = 0;
		num = 3;
		char[] array = serial.ToCharArray();
		Array.Reverse((Array)array);
		serial = new string(array);
		string text = serial;
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			if (c.ToString().ParseToInt() >= 0 && c.ToString().ParseToInt() <= 9)
			{
				num2 += Conversion.Val(c) * num;
				num = ((num == 3) ? 1 : 3);
			}
		}
		int num3 = Convert.ToInt32(10 - num2 % 10);
		if (num3 >= 10)
		{
			return " 0";
		}
		return num3.ToString();
	}
}
