using System;
using System.Collections.Generic;
using Microsoft.Win32;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace LK;

public class CLSRegistry
{
	private static Dictionary<Package, RegistryObject> _buffer = new Dictionary<Package, RegistryObject>();

	public static ReturnedSaveFuncInfoWithValue<RegistryObject> GetRegObject(Package package)
	{
		ReturnedSaveFuncInfoWithValue<RegistryObject> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<RegistryObject>();
		try
		{
			_buffer.TryGetValue(package, out var value);
			if (value != null)
			{
				returnedSaveFuncInfoWithValue.value = value;
				return returnedSaveFuncInfoWithValue;
			}
			value = RegistryObject.FromJson(RegistryObject.ReadRegistryKey(package).value) ?? new RegistryObject();
			returnedSaveFuncInfoWithValue.value = value;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.value = new RegistryObject();
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GetRegObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CLSRegistry.cs", 32);
			WebErrorLog.Instance.StartLog(ex, "", "GetRegObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CLSRegistry.cs", 33);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static void SetRegObject(RegistryObject value, Package package)
	{
		_buffer.Remove(package);
		_buffer.Add(package, value);
		SaveObject(package);
	}

	public static ReturnedSaveFuncInfo SaveObject(Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			_buffer.TryGetValue(package, out var value);
			if (value != null)
			{
				string value2 = value.ToString();
				returnedSaveFuncInfo.AddReturnedValue(RegistryObject.WriteRegistryKey(value2, package), ReturnedState.None, "SaveObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CLSRegistry.cs", 54);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CLSRegistry.cs", 59);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SaveObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CLSRegistry.cs", 60);
		}
		return returnedSaveFuncInfo;
	}

	internal static void WriteLKForErrorHandler(string key, string value)
	{
		try
		{
			Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", key, value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteLKForErrorHandler", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\CLSRegistry.cs", 71);
		}
	}
}
