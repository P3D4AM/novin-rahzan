using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.LockService;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;

namespace EntityCache.Business.LkService;

public class LkDemoBusiness : IHasContext
{
	public string ContextGuid { get; private set; }

	public LkDemoInformation DemoInformation { get; private set; }

	public LkDemoBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public Task<ReturnedSaveFuncInfoWithValue<LkDemoInformation>> GetIsDemoAsync(string version, Package package, bool ignoreHttpExceptions, CancellationToken token = default(CancellationToken))
	{
		return GetIsDemoAsync(ContextGuid, version, DemoInformation, package, ignoreHttpExceptions, token);
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<LkDemoInformation>> GetIsDemoAsync(string contextGuid, string version, LkDemoInformation demoInformation, Package package, bool ignoreHttpExceptions, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> ret = new ReturnedSaveFuncInfoWithValue<LkDemoInformation>();
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue = ret;
		if (returnedSaveFuncInfoWithValue.value == null)
		{
			returnedSaveFuncInfoWithValue.value = new LkDemoInformation();
		}
		try
		{
			if (demoInformation == null)
			{
				ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue2 = await SetIfDemoAsync(contextGuid, version, demoInformation, package, ignoreHttpExceptions, token);
				ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetIsDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 31);
				if (ret.HasError)
				{
					return ret;
				}
				demoInformation = returnedSaveFuncInfoWithValue2.value;
			}
			ret.value = demoInformation;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetIsDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 39);
			ret.AddReturnedValue(ex, "", "GetIsDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 40);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<LkDemoInformation>> SetIfDemoAsync(string contextGuid, string version, LkDemoInformation lkDemoInfo, Package package, bool ignoreHttpExceptions, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> ret = new ReturnedSaveFuncInfoWithValue<LkDemoInformation>();
		ReturnedSaveFuncInfoWithValue<LkDemoInformation> returnedSaveFuncInfoWithValue = ret;
		if (returnedSaveFuncInfoWithValue.value == null)
		{
			returnedSaveFuncInfoWithValue.value = new LkDemoInformation();
		}
		try
		{
			if (lkDemoInfo != null)
			{
				return ret;
			}
			if (lkDemoInfo == null)
			{
				lkDemoInfo = new LkDemoInformation();
			}
			ReturnedSaveFuncInfoWithValue<string> checkLock = await NpLockHubInstances.Instance.Demo.CheckLockAsync(version, package, ignoreHttpExceptions, token);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 54);
			}
			ReturnedSaveFuncInfoWithValue<LockInformation> lkInfo = await LkActivatorBaseBusiness.GetLockInformationAsync(contextGuid, package, token);
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 57);
			}
			ret.AddReturnedValue(lkInfo, ReturnedState.None, "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 58);
			if (ret.HasError)
			{
				return ret;
			}
			if (!lkInfo.value.IsDemo)
			{
				ret.AddReturnedValue(checkLock, ReturnedState.None, "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 61);
			}
			lkDemoInfo.TinyErrorCode = checkLock.value;
			ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue2 = await GetLKConnectedAsync(contextGuid, version, ignoreHttpExceptions, package, token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 65);
			if (returnedSaveFuncInfoWithValue2.value)
			{
				lkDemoInfo.IsDemo = false;
				return ret;
			}
			ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue3 = await LkActivatorBaseBusiness.GetLkSerialAsync(contextGuid, token);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue3, ReturnedState.None, "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 74);
			if ((returnedSaveFuncInfoWithValue3.value == 1234567890 || (returnedSaveFuncInfoWithValue3.value <= 0 && lkInfo.value != null)) && (lkInfo.value.LkSerial <= 0 || lkInfo.value.LkSerial == 1234567890))
			{
				lkDemoInfo.IsDemo = true;
			}
			else
			{
				lkDemoInfo.IsDemo = false;
				ret.AddReturnedValue(checkLock, ReturnedState.None, "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 81);
			}
			ret.value = lkDemoInfo;
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue(ex, "", "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 88);
			WebErrorLog.Instance.StartLog(ex, "", "SetIfDemoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 89);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<bool>> GetLKConnectedAsync(string contextGuid, string version, bool ignoreHttpExceptions, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> ret = new ReturnedSaveFuncInfoWithValue<bool>();
		try
		{
			if ((await NpLockHubInstances.Instance.Demo.CheckLockAsync(version, package, ignoreHttpExceptions, token)).HasError)
			{
				ret.value = false;
			}
			else
			{
				ret.value = false;
			}
		}
		catch (Exception ex)
		{
			ret.value = false;
			ret.AddReturnedValue(ex, "", "GetLKConnectedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 105);
			WebErrorLog.Instance.StartLog(ex, "", "GetLKConnectedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\LkService\\LkDemoBusiness.cs", 106);
		}
		return ret;
	}
}
