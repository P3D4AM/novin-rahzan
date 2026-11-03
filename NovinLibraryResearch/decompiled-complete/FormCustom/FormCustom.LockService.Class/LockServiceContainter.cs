using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub.NPLockHub;
using ClientHub.NPLockHub.LockHub;
using EntityCache.Business.LkService;
using EntityCache.LockService;
using FormCustom.Cache;
using FormCustom.Froms.LkManage;
using FormCustom.LockService.Forms;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.LockService.Class;

public class LockServiceContainter
{
	public string ContextGuid { get; private set; }

	public event Func<bool> OnResponseCheckActivation;

	public LockServiceContainter(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<ReturnedSaveFuncInfo> InitializeServiceAsync(LkUiFactory owner, ENSource source)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (string.IsNullOrEmpty(ContextGuid))
		{
			throw new ArgumentException("ContextGuid");
		}
		try
		{
			LockClientSetting setting = LockClientSetting.GetSetting();
			LockServiceManager service = new LockServiceManager(ContextGuid, owner, source);
			ServiceStatus serviceStatus = service.CheckService();
			await service.CheckOCXAsync();
			if (string.IsNullOrEmpty(setting.Ip) && setting.Port == 0)
			{
				if (serviceStatus == ServiceStatus.NotFound)
				{
					setting = new LockClientSetting("127.0.0.1", "0.0.0.0", 7596);
					await LockClientSetting.SetSettingAsync(setting);
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await service.InstallServiceAsync(null, setting.SetUpIp, setting.Port), ReturnedState.None, "InitializeServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 48);
				}
				else
				{
					await ShowSettingFormAsync(owner, source);
					LockClientSetting.GetSetting();
					serviceStatus = service.CheckService();
				}
			}
			else
			{
				serviceStatus = service.CheckService();
			}
			if (serviceStatus == ServiceStatus.Stopped)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await service.StartServiceAsync(), ReturnedState.None, "InitializeServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 65);
			}
			setting = LockClientSetting.GetSetting();
			if (string.IsNullOrEmpty(setting.Ip) && setting.Port == 0)
			{
				return res.AddError("تنظیمات سرویس قفل معتبر نمی باشد", "", "InitializeServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 68);
			}
			if (NpLockHubInstances.Instance != null)
			{
				await NpLockHubInstances.Instance.DisposeAsync();
			}
			NpLockHubInstances instance = new NpLockHubInstances
			{
				CheckActivation = new CheckActivationHub(ContextGuid, $"http://{setting.Ip}:{setting.Port}/CheckActivationHub", null),
				Environment = new LkEnvironmentHub(ContextGuid, $"http://{setting.Ip}:{setting.Port}/LkEnvironmentHub", null),
				Demo = new DemoClientHub(ContextGuid, $"http://{setting.Ip}:{setting.Port}/DemoHub", null),
				ActivationCode = new LkActivationCodeHub(ContextGuid, $"http://{setting.Ip}:{setting.Port}/ActivationCodeHub", null),
				SoftwareSerialManager = new SoftwareSerialManagerHub(ContextGuid, $"http://{setting.Ip}:{setting.Port}/SoftWareSerialManagerHub", null),
				Server = new LkServerHub(ContextGuid, $"http://{setting.Ip}:{setting.Port}/LkServerHub", null)
			};
			NpLockHubInstances.SetInstance(ContextGuid, instance);
			LkActivatorBaseBusiness.OnKillProcess += LkActivatorBaseBusiness_OnKillProcess;
			LkActivatorBaseBusiness.OnProcessMessage += LkActivatorBaseBusiness_OnProcessMessage;
			LkActivatorBaseBusiness.OnProcessCrm += LkActivatorBaseBusiness_OnProcessCrm;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 89);
			res.AddReturnedValue(ex, "", "InitializeServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 90);
		}
		return res;
	}

	private async Task LkActivatorBaseBusiness_OnKillProcess(ILkUiFactory owner, ReturnedSaveFuncInfo res)
	{
		try
		{
			if (res.HasError)
			{
				await ((CacheBaseForm)owner).ShowReturnedSavedFuncInfoAsync(res, "پیغام سیستم", "LkActivatorBaseBusiness_OnKillProcess", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs");
			}
			Process.GetCurrentProcess().Kill();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LkActivatorBaseBusiness_OnKillProcess", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 101);
		}
	}

	private Task LkActivatorBaseBusiness_OnProcessCrm(string arg)
	{
		try
		{
			if (string.IsNullOrEmpty(arg))
			{
				return Task.CompletedTask;
			}
			Process.Start(arg);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LkActivatorBaseBusiness_OnProcessCrm", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 111);
		}
		return Task.CompletedTask;
	}

	private Task LkActivatorBaseBusiness_OnProcessMessage(List<string> lstUrl)
	{
		try
		{
			if (lstUrl != null && !lstUrl.Any())
			{
				return Task.CompletedTask;
			}
			foreach (string item in lstUrl)
			{
				if (string.IsNullOrEmpty(item))
				{
					return Task.CompletedTask;
				}
				Process.Start(item);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LkActivatorBaseBusiness_OnProcessMessage", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 127);
			throw;
		}
		return Task.CompletedTask;
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
			WebErrorLog.Instance.StartLog(ex, "", "RunProcess", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 140);
		}
	}

	private async Task ShowSettingFormAsync(LkUiFactory owner, ENSource source)
	{
		try
		{
			while (!((Control)owner).IsHandleCreated)
			{
				await Task.Delay(100);
				if (owner.IsDisposed)
				{
					return;
				}
			}
			if (owner.IsDisposed)
			{
				return;
			}
			((Control)owner).Invoke((Delegate)(Action)delegate
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				if (owner.IsDisposed)
				{
					return;
				}
				try
				{
					((Form)new frmLockSetting(ContextGuid, source)).ShowDialog((IWin32Window)(object)owner);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "ShowSettingFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 160);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowSettingFormAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LockServiceContainter.cs", 163);
		}
	}
}
