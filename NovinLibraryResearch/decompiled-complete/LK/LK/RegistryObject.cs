using System;
using System.Collections.Generic;
using LK.SystemVariable;
using Microsoft.Win32;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace LK;

public class RegistryObject : Serializable<RegistryObject>
{
	public string _softwareSerialHozor = "";

	public string _softwareSerialAccounting = "";

	public string _softwareSerialScale = "";

	public int _x125;

	public long _lkSerial;

	public string _tovlidActivationCode = "";

	public string _barcodeActivationCode = "";

	public string _scaleActivationCode = "";

	public string _callerIdActivationCode = "";

	public string _specialActivationCode = "";

	public string _hoghoghActivationCode = "";

	public string _softwareType = "00";

	public string _scaleNo = "01";

	public string _numberOfdays = "";

	public string _clientNetNumber = "00";

	public bool _isDemo;

	public string _clientSet = "0";

	public string _numberOfDemoRun = "0";

	public DateTime _activeDate = DateTime.Now.AddYears(-10);

	public Guid _guidClient;

	private LkInformation _lkInfo;

	public string accountingActivationCode { get; set; } = "";


	public List<Guid> ListOfClient { get; set; } = new List<Guid>();


	public string AccountingActivationCode
	{
		get
		{
			return accountingActivationCode;
		}
		set
		{
			accountingActivationCode = value;
		}
	}

	public string GetSoftwareSerial_Hozor()
	{
		return _softwareSerialHozor;
	}

	public ReturnedSaveFuncInfo SetSoftwareSerial_Hozor(string value)
	{
		_softwareSerialHozor = value;
		return Write(Package.HoghoghVaDastmozd);
	}

	public string GetSoftwareSerial_Accounting()
	{
		return _softwareSerialAccounting;
	}

	public ReturnedSaveFuncInfo SetSoftwareSerial_Accounting(string value)
	{
		_softwareSerialAccounting = value;
		return Write(Package.Accounting);
	}

	public string GetSoftwareSerial_Scale()
	{
		return _softwareSerialScale;
	}

	public int GetX125()
	{
		return _x125;
	}

	public ReturnedSaveFuncInfo SetX125(int value, Package package)
	{
		_x125 = value;
		return Write(package);
	}

	public long GetLkSerial()
	{
		return _lkSerial;
	}

	public ReturnedSaveFuncInfo SetLkSerial(long value, Package package)
	{
		_lkSerial = value;
		return Write(package);
	}

	public string GetAccountingActivationCode()
	{
		return AccountingActivationCode;
	}

	public ReturnedSaveFuncInfo SetAccountingActivationCode(string value)
	{
		AccountingActivationCode = value;
		return Write(Package.Accounting);
	}

	public string GetTolidActivationCode()
	{
		return _tovlidActivationCode;
	}

	public ReturnedSaveFuncInfoWithValue<string> GetBarcodeActivationCode()
	{
		return new ReturnedSaveFuncInfoWithValue<string>
		{
			value = _barcodeActivationCode
		};
	}

	public string GetScaleActivationCode()
	{
		return _scaleActivationCode;
	}

	public string GetSpecialActivationCode()
	{
		return _specialActivationCode;
	}

	public string GetHoghoghActivationCode()
	{
		return _hoghoghActivationCode;
	}

	public ReturnedSaveFuncInfo SetHoghoghActivationCode(string value)
	{
		_hoghoghActivationCode = value;
		return Write(Package.HoghoghVaDastmozd);
	}

	public string GetSoftwareType()
	{
		return _softwareType;
	}

	public ReturnedSaveFuncInfo SetSoftwareType(string value, Package package)
	{
		_softwareType = value;
		return Write(package);
	}

	public string GetNumberOfdays()
	{
		return _numberOfdays;
	}

	public ReturnedSaveFuncInfo SetNumberOfdays(string value, Package package)
	{
		_numberOfdays = value;
		return Write(package);
	}

	public DateTime? GetX12mx()
	{
		try
		{
			if (_lkInfo != null)
			{
				return _lkInfo.X12Mx;
			}
			_lkInfo = LkInformation.GetValue();
			if (_lkInfo == null)
			{
				return null;
			}
			return _lkInfo.X12Mx;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetX12mx", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 216);
			throw;
		}
	}

	public ReturnedSaveFuncInfo SetX12mx(DateTime value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (_lkInfo == null)
			{
				_lkInfo = new LkInformation();
			}
			_lkInfo.X12Mx = value;
			_lkInfo.SetValue();
			returnedSaveFuncInfo.AddReturnedValue(Write(package), ReturnedState.None, "SetX12mx", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 229);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetX12mx", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 233);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SetX12mx", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 234);
		}
		return returnedSaveFuncInfo;
	}

	public string GetClientNetNumber()
	{
		return _clientNetNumber;
	}

	public ReturnedSaveFuncInfo SetClientNetNumber(string value, Package package)
	{
		_clientNetNumber = value;
		return Write(package);
	}

	public bool GetIsDemo()
	{
		return _isDemo;
	}

	public ReturnedSaveFuncInfo SetIsDemo(bool value, Package package)
	{
		_isDemo = value;
		return Write(package);
	}

	public string GetClientSet()
	{
		return _clientSet;
	}

	public ReturnedSaveFuncInfo SetClientSet(string value, Package package)
	{
		_clientSet = value;
		return Write(package);
	}

	public string GetNumberOfDemoRun()
	{
		return _numberOfDemoRun;
	}

	public ReturnedSaveFuncInfo SetNumberOfDemoRun(string value, Package package)
	{
		_numberOfDemoRun = value;
		return Write(package);
	}

	public DateTime GetActiveDate()
	{
		return _activeDate;
	}

	public ReturnedSaveFuncInfo SetActiveDate(DateTime value, Package package)
	{
		_activeDate = value;
		return Write(package);
	}

	public Guid GetGuidClient()
	{
		return _guidClient;
	}

	public ReturnedSaveFuncInfo SetGuidClient(Guid value, Package package)
	{
		_guidClient = value;
		return Write(package);
	}

	public static RegistryObject FromJson(string data)
	{
		RegistryObject registryObject = new RegistryObject();
		try
		{
			if (data == null || data == "")
			{
				return registryObject;
			}
			registryObject = data.FromJson<RegistryObject>(sendErrorIfException: false, "FromJson", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 316);
			return registryObject ?? new RegistryObject();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FromJson", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 321);
			return registryObject;
		}
	}

	public static ReturnedSaveFuncInfoWithValue<string> ReadRegistryKey(Package package)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		int num = 0;
		try
		{
			string message = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinP" + package, "")?.ToString() ?? "";
			message = message.DecryptString();
			num = 3;
			returnedSaveFuncInfoWithValue.value = message;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ReadRegistryKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 340);
			returnedSaveFuncInfoWithValue.value = "";
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "ReadRegistryKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 343);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfoWithValue<string> ReadListFromObject(string Type)
	{
		ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			string message = Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinP" + Type, "").ToString();
			message = message.DecryptString();
			returnedSaveFuncInfoWithValue.value = message;
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ReadListFromObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 359);
			returnedSaveFuncInfoWithValue.value = "";
			WebErrorLog.Instance.StartLog(ex, "", "ReadListFromObject", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 361);
		}
		return returnedSaveFuncInfoWithValue;
	}

	public static ReturnedSaveFuncInfo WriteRegistryKey(string value, Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			value = value.EncryptData();
			Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinP" + package, value);
		}
		catch (Exception ex)
		{
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "WriteRegistryKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 377);
			WebErrorLog.Instance.StartLog(ex, "", "WriteRegistryKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 379);
		}
		return returnedSaveFuncInfo;
	}

	public static void WriteRegistryKey(string value, string Type)
	{
		try
		{
			value = value.EncryptData();
			Registry.SetValue("HKEY_LOCAL_MACHINE\\Software\\Novin", "NovinP" + Type, value);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WriteRegistryKey", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 394);
		}
	}

	public ReturnedSaveFuncInfo Write(Package package)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		returnedSaveFuncInfo.AddReturnedValue(CLSRegistry.SaveObject(package), ReturnedState.None, "Write", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 401);
		return returnedSaveFuncInfo;
	}

	public ReturnedSaveFuncInfo CleanActivationCodes()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			returnedSaveFuncInfo.AddReturnedValue(SetAccountingActivationCode(""), ReturnedState.None, "CleanActivationCodes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 410);
			returnedSaveFuncInfo.AddReturnedValue(SetHoghoghActivationCode(""), ReturnedState.None, "CleanActivationCodes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 411);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CleanActivationCodes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 416);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CleanActivationCodes", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\LkCSharp\\RegistryObject.cs", 417);
		}
		return returnedSaveFuncInfo;
	}
}
