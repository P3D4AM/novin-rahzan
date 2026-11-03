using System;
using System.Threading.Tasks;
using FormCustom.ServiceManagement;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PayamakUi.Classes.Service;

public class ServiceManager : ServiceBaseManager
{
	public override string ServiceName => "NpPayamakService";

	public override string ExeName => "PayamakService";

	public override string HubName => "SmsNetAccountHub";

	public ServiceManager(string contextGuid)
		: base(contextGuid)
	{
	}

	public override async Task<ReturnedSaveFuncInfo> InstallServiceAsync(string sqlConnectionString, string signalRHubBaseAddress, int signalRHubPortNumber)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.Manager.InstallServiceAsync(signalRHubBaseAddress, signalRHubPortNumber, sqlConnectionString), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\ServiceManager.cs", 23);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.Manager.SetFailureConfigAsync(), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\ServiceManager.cs", 24);
			if (!res.HasError)
			{
				EnvironmentManager<string>.SetValue(base.Manager.ServiceName, GetServicePath());
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CreateScheduledTaskAsync("AutoRestartPayamakService"), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\ServiceManager.cs", 28);
				res.AddInformation("سرویس پیامک با موفقیت نصب شد.", "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\ServiceManager.cs", 29);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\ServiceManager.cs", 34);
			res.AddReturnedValue(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\ServiceManager.cs", 35);
		}
		return res;
	}
}
