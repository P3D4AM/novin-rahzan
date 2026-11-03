using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using FormCustom.ServiceManagement;
using LK;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.LockService.Class;

public class LockServiceManager : ServiceBaseManager
{
	public override string ServiceName => "NpLockService";

	public override string ExeName => "NpLockService";

	public override string HubName => "CheckActivationHub";

	public ILkUiFactory Owner { get; private set; }

	public ENSource Source { get; private set; }

	public LockServiceManager(string contextGuid, ILkUiFactory owner, ENSource source)
		: base(contextGuid)
	{
		Owner = owner;
		Source = source;
		CheckServiceAsync();
	}

	private async Task CheckServiceAsync()
	{
		try
		{
			await Task.Delay(100);
			CheckService();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex.Message, "", "CheckServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 35);
		}
	}

	public Task CheckOCXAsync()
	{
		try
		{
			if (Owner == null)
			{
				return Task.CompletedTask;
			}
			Package package = Package.Accounting;
			string path = Path.Combine(Application.StartupPath, "NpLockService");
			switch (Source)
			{
			case ENSource.Baje:
			case ENSource.Department:
				package = Package.baje;
				break;
			case ENSource.TimeAttendance:
				package = Package.HoghoghVaDastmozd;
				break;
			}
			global::LK.LK.CheckOCX(Owner, package, path);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckOCXAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 54);
		}
		return Task.CompletedTask;
	}

	public override async Task<ReturnedSaveFuncInfo> InstallServiceAsync(string sqlConnectionString, string signalRHubBaseAddress, int signalRHubPortNumber)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			LockClientSetting setting = LockClientSetting.GetSetting();
			if (!string.IsNullOrEmpty(setting.SetUpIp) && signalRHubBaseAddress != setting.SetUpIp)
			{
				res.AddError("با توجه به اینکه آدرس هاب قفل تغییر کرده لطفا ابتدا تنظیمات را ذخیره نمایید.", "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 65);
			}
			if (setting.Port != 0 && signalRHubPortNumber != setting.Port)
			{
				res.AddError("با توجه به اینکه پورت هاب قفل تغییر کرده لطفا ابتدا تنظیمات را ذخیره نمایید.", "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 68);
			}
			if (res.HasError)
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.Manager.InstallServiceAsync(signalRHubBaseAddress, signalRHubPortNumber, sqlConnectionString), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 72);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await base.Manager.SetFailureConfigAsync(), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 73);
			if (!res.HasError)
			{
				EnvironmentManager<string>.SetValue(base.Manager.ServiceName, GetServicePath());
				await CheckOCXAsync();
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CreateScheduledTaskAsync("AutoRestartLockService"), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 80);
				res.AddInformation("سرویس قفل با موفقیت نصب شد.", "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 81);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 86);
			res.AddReturnedValue(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceManager.cs", 87);
		}
		finally
		{
			CheckService();
		}
		return res;
	}
}
