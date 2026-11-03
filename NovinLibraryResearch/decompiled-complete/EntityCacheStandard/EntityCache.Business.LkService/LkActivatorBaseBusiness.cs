using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.LkService.Interfaces;
using EntityCache.LockService;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.LkService;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;

namespace EntityCache.Business.LkService;

public abstract class LkActivatorBaseBusiness : IHasContext
{
	public const int DemoDaysCount = 10;

	public string ContextGuid { get; private set; }

	public ILkClientManager ClientManagerInstance { get; private set; }

	public ENSource Source { get; private set; }

	public static string VersionAccounting
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	public static string VersionScale
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	public static string VersionHozor
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	public static string VersionDepartment
	{
		get
		{
			string text = FileVersionInfo.GetVersionInfo(Path.Combine(AppContext.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".exe")).FileVersion.RemoveNoNumbers();
			while (text.Length < 4)
			{
				text += "0";
			}
			return text.Substring(0, 4);
		}
	}

	public static event Func<List<string>, Task> OnProcessMessage;

	public static event Func<ILkUiFactory, ReturnedSaveFuncInfo, Task> OnKillProcess;

	public static event Func<string, Task> OnProcessCrm;

	public LkActivatorBaseBusiness(string contextGuid, ENSource source, ILkClientManager instance)
	{
		try
		{
			ContextGuid = contextGuid;
			ClientManagerInstance = instance;
			Source = source;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 87);
		}
	}

	protected void FireOnProcessMessage(List<string> urlList)
	{
		LkActivatorBaseBusiness.OnProcessMessage?.Invoke(urlList);
	}

	protected void FireOnKillProcess(ILkUiFactory owner, ReturnedSaveFuncInfo res)
	{
		LkActivatorBaseBusiness.OnKillProcess?.Invoke(owner, res);
	}

	protected void FireOnProcessCrm(string lkSerial)
	{
		LkActivatorBaseBusiness.OnProcessCrm?.Invoke(ResourceManagement.TextBrandResourceManagement.CRMBaseURL + "/extend/code/" + lkSerial);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<LkDemoInformation>> CheckDemoAsync(Package package, CancellationToken token)
	{
		return CheckDemoAsync(ContextGuid, package, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LkDemoInformation>> CheckDemoAsync(string contextGuid, Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> res = new ReturnedSaveFuncInfoWithValue<LkDemoInformation>();
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue = res;
		if (returnedSaveFuncInfoWithValue.value == null)
		{
			returnedSaveFuncInfoWithValue.value = new LkDemoInformation();
		}
		try
		{
			LkDemoBusiness demo = new LkDemoBusiness(contextGuid);
			if (package == Package.Accounting)
			{
				ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue2 = res;
				returnedSaveFuncInfoWithValue2.AddReturnedValue(await demo.GetIsDemoAsync(VersionAccounting, package, ignoreHttpExceptions: true, token), ReturnedState.None, "CheckDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 102);
			}
			if (package == Package.HoghoghVaDastmozd)
			{
				ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue2 = res;
				returnedSaveFuncInfoWithValue2.AddReturnedValue(await demo.GetIsDemoAsync(VersionHozor, package, ignoreHttpExceptions: true, token), ReturnedState.None, "CheckDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 104);
			}
			if (package == Package.baje)
			{
				ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue2 = res;
				returnedSaveFuncInfoWithValue2.AddReturnedValue(await demo.GetIsDemoAsync(VersionDepartment, package, ignoreHttpExceptions: true, token), ReturnedState.None, "CheckDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 106);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 110);
			res.AddReturnedValue(ex, "", "CheckDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 111);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> CheckLKWithWebServiceAsync(string contextGuid, Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.CheckActivation.CheckLKWithWebServiceAsync(package, token), ReturnedState.None, "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 120);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckLKWithWebServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 122);
		}
		return res;
	}

	protected Task<ReturnedSaveFuncInfoWithValue<LockInformation>> GetLockInformationAsync(Package package, CancellationToken token)
	{
		return GetLockInformationAsync(ContextGuid, package, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LockInformation>> GetLockInformationAsync(string contextGuid, Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<LockInformation> res = new ReturnedSaveFuncInfoWithValue<LockInformation>();
		try
		{
			ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue = await NpLockHubInstances.Instance.Environment.GetLockInformationAsync(package, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 132);
			res.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 137);
			res.AddReturnedValue(ex, "", "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 138);
		}
		return res;
	}

	protected Task<ReturnedSaveFuncInfoWithValue<long>> GetLkSerialAsync(CancellationToken token)
	{
		return GetLkSerialAsync(ContextGuid, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<long>> GetLkSerialAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<long> res = new ReturnedSaveFuncInfoWithValue<long>();
		try
		{
			NpLockHubInstances instance = NpLockHubInstances.Instance;
			if (instance == null)
			{
				res.value = 0L;
				return res;
			}
			ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await instance.Environment.GetLkSerialAsync(token), ReturnedState.None, "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 154);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 158);
			res.AddReturnedValue(ex, "", "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 159);
		}
		return res;
	}

	protected async Task<ReturnedSaveFuncInfoWithValue<LKInfo>> GetLkInfoAsync(CancellationToken token)
	{
		return await GetLkInfoAsync(ContextGuid, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LKInfo>> GetLkInfoAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<LKInfo> res = new ReturnedSaveFuncInfoWithValue<LKInfo>();
		try
		{
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await NpLockHubInstances.Instance.Environment.GetLkInfoAsync(token), ReturnedState.None, "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 169);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 171);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> SetLockInformationAsync(string contextGuid, LockInformation lk, Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await NpLockHubInstances.Instance.Environment.SetLockInformationAsync(lk, package, token), ReturnedState.None, "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 179);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 181);
		}
		return res;
	}

	protected Task<ReturnedSaveFuncInfoWithValue<clsLkError>> CheckValidLkClientAsync(string uniqueId, Package package, CancellationToken token = default(CancellationToken))
	{
		return NpLockHubInstances.Instance.Server.CheckValidLkClientAsync(uniqueId, package, token);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetActivationCodeAsync(Package package, CancellationToken token)
	{
		return NpLockHubInstances.Instance.CheckActivation.GetActivationCodeAsync(package, token);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<bool>> CheckDaysAsync(Package package, LockInformation lkInfo, CancellationToken token)
	{
		return CheckDaysAsync(ContextGuid, lkInfo, package, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<bool>> CheckDaysAsync(string contextGuid, LockInformation lkInfo, Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if (lkInfo == null)
			{
				ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue = await GetLockInformationAsync(contextGuid, package, token);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CheckDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 196);
				lkInfo = returnedSaveFuncInfoWithValue.value;
			}
			short num = (lkInfo?.NumberOfdays?.RemoveNoNumbers() ?? "").ParseToShort(0);
			if (num < 0)
			{
				ret.value = false;
				return ret;
			}
			if ((long)((lkInfo?.ActiveDate ?? DateTime.Now.AddYears(-10)) - DateTime.Now).Days > (long)num)
			{
				return ret;
			}
			ret.value = true;
		}
		catch (Exception ex)
		{
			ret.value = false;
			ret.AddReturnedValue(ex, "", "CheckDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 221);
			WebErrorLog.Instance.StartLog(ex, "", "CheckDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 222);
		}
		return ret;
	}

	protected async Task<ReturnedSaveFuncInfoWithValue<string>> GetClientSetAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await NpLockHubInstances.Instance.CheckActivation.GetClientSetAsync(package, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 232);
			res.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 237);
			res.AddReturnedValue(ex, "", "GetClientSetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 238);
		}
		return res;
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerialAccountingAsync(CancellationToken token)
	{
		return GetSoftwareSerialAccountingAsync(ContextGuid, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerialAccountingAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await NpLockHubInstances.Instance.SoftwareSerialManager.GetSoftwareSerial_AccountingAsync(VersionAccounting, token), ReturnedState.None, "GetSoftwareSerialAccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 248);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSoftwareSerialAccountingAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 250);
		}
		return res;
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerialHozorAsync(CancellationToken token)
	{
		return GetSoftwareSerialHozorAsync(ContextGuid, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<string>> GetSoftwareSerialHozorAsync(string contextGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<string> res = new ReturnedSaveFuncInfoWithValue<string>();
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.value = (await NpLockHubInstances.Instance.SoftwareSerialManager.GetSoftwareSerial_HozorAsync(VersionHozor, token)).value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSoftwareSerialHozorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 261);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<int>> GetRemDaysAsync(string contextGuid, Package package, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue = await GetLockInformationAsync(contextGuid, package, token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetRemDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 270);
			ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue2 = returnedSaveFuncInfoWithValue;
			LockInformation regObject = returnedSaveFuncInfoWithValue2.value ?? (returnedSaveFuncInfoWithValue2.value = new LockInformation());
			if (!regObject.X12Mx.HasValue)
			{
				regObject.X12Mx = DateTime.Now.Date;
				ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue3 = ret;
				returnedSaveFuncInfoWithValue3.AddReturnedValue(await SetLockInformationAsync(contextGuid, regObject, package, token), ReturnedState.None, "GetRemDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 275);
			}
			DateTime value = Convert.ToDateTime(regObject.X12Mx).AddDays(10.0);
			int num = Convert.ToInt32(checked(-DateTime.Now.Date.Subtract(value).Days));
			ret.value = ((num >= 0 && num <= 10) ? num : 0);
		}
		catch (Exception ex)
		{
			ret.value = 0;
			ret.AddReturnedValue(ex, "", "GetRemDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 285);
			WebErrorLog.Instance.StartLog(ex, "", "GetRemDaysAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 286);
		}
		return ret;
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingSadeActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		return NpLockHubInstances.Instance.ActivationCode.GetAccountingSadeActivationCodeAsync(softwareSerial, lkSerial, accountingPack, days, clientNetNumber, expiredDate, token);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingMotevasetActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		return NpLockHubInstances.Instance.ActivationCode.GetAccountingMotevasetActivationCodeAsync(softwareSerial, lkSerial, accountingPack, days, clientNetNumber, expiredDate, token);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingPishrafteActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		return NpLockHubInstances.Instance.ActivationCode.GetAccountingPishrafteActivationCodeAsync(softwareSerial, lkSerial, accountingPack, days, clientNetNumber, expiredDate, token);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<string>> GetAccountingJameActivationCodeAsync(string softwareSerial, string lkSerial, AccountingPackage accountingPack, short days, short clientNetNumber, DateTime expiredDate, CancellationToken token)
	{
		return NpLockHubInstances.Instance.ActivationCode.GetAccountingJameActivationCodeAsync(softwareSerial, lkSerial, accountingPack, days, clientNetNumber, expiredDate, token);
	}

	protected Task<ReturnedSaveFuncInfoWithValue<Level_Accounting>> GetApplicationTypeAsync(string softwareSerial, string hardwareSerial, string activationCode, CancellationToken token)
	{
		return NpLockHubInstances.Instance.CheckActivation.GetApplicationTypeAsync(softwareSerial, hardwareSerial, activationCode, token);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<Level_Hozor>> GetHozorTypeAsync(string contextGuid, string softwareSerial, string hardwareSerial, string activationCode, bool isDemo, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<Level_Hozor> res = new ReturnedSaveFuncInfoWithValue<Level_Hozor>();
		try
		{
			ReturnedSaveFuncInfoWithValue<Level_Hozor> returnedSaveFuncInfoWithValue = await NpLockHubInstances.Instance.CheckActivation.GetHozorTypeAsync(softwareSerial, hardwareSerial, activationCode, isDemo, token);
			res.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 306);
			res.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetHozorTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkActivatorBaseBusiness.cs", 309);
		}
		return res;
	}
}
