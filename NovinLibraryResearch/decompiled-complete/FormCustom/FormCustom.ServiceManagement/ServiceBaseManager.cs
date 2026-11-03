using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Services;

namespace FormCustom.ServiceManagement;

public abstract class ServiceBaseManager : IServiceManager, IHasContext
{
	private CancellationTokenSource _serviceStatusChangedToken;

	public string ContextGuid { get; private set; }

	public NssmServiceManager Manager { get; private set; }

	public string StatusForDisplay => Manager.CheckService().GetDisplay().GetTranslate();

	public abstract string ServiceName { get; }

	public abstract string ExeName { get; }

	public abstract string HubName { get; }

	public static string SrviceFailureConfigKey => "ServiceFailureConfig";

	public ServiceStatus LastServiceStatus { get; private set; }

	public event Func<object, ServiceStatusChangesEventArgument, Task> OnStatusChanges;

	public ServiceBaseManager(string contextGuid)
	{
		try
		{
			ContextGuid = contextGuid;
			string servicePath = GetServicePath();
			SetManager(new NssmServiceManager(ServiceName, servicePath));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 39);
		}
	}

	public ServiceStatus CheckService()
	{
		ServiceStatus serviceStatus = Manager.CheckService();
		_serviceStatusChangedToken?.Cancel();
		_serviceStatusChangedToken = new CancellationTokenSource();
		CancellationToken token = _serviceStatusChangedToken.Token;
		LastServiceStatus = serviceStatus;
		if (this.OnStatusChanges != null)
		{
			PacketParser.Utilities.RaiseEvent(this.OnStatusChanges, this, new ServiceStatusChangesEventArgument(LastServiceStatus, serviceStatus, token), token, 60, "CheckService", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs");
		}
		return serviceStatus;
	}

	public void SetManager(NssmServiceManager manager)
	{
		Manager = manager;
	}

	public abstract Task<ReturnedSaveFuncInfo> InstallServiceAsync(string sqlConnectionString, string signalRHubBaseAddress, int signalRHubPortNumber);

	public virtual async Task<ReturnedSaveFuncInfo> RestartServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Manager.RestartServiceAsync(), ReturnedState.None, "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 62);
			if (!res.HasError)
			{
				res.AddInformation("سرویس " + Manager.ServiceName + " مجددا اجرا شد", "", "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 63);
			}
			CheckService();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 69);
			res.AddReturnedValue(ex, "", "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 70);
		}
		return res;
	}

	public virtual async Task<ReturnedSaveFuncInfo> StartServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Manager.StartServiceAsync(), ReturnedState.None, "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 79);
			if (!res.HasError)
			{
				res.AddInformation("سرویس " + Manager.ServiceName + " با موفقیت اجرا شد", "", "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 81);
			}
			CheckService();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 86);
			res.AddReturnedValue(ex, "", "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 87);
		}
		return res;
	}

	public virtual async Task<ReturnedSaveFuncInfo> StopServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Manager.StopServiceAsync(), ReturnedState.None, "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 96);
			if (!res.HasError)
			{
				res.AddInformation("سرویس " + Manager.ServiceName + " متوقف شد", "", "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 98);
			}
			CheckService();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 103);
			res.AddReturnedValue(ex, "", "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 104);
		}
		return res;
	}

	public virtual async Task<ReturnedSaveFuncInfo> UnInstallServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Manager.UnInstallServiceAsync(), ReturnedState.None, "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 113);
			if (!res.HasError)
			{
				res.AddInformation("سرویس(" + Manager.ServiceName + ") با موفقت حذف شد.", "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 115);
			}
			CheckService();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 120);
			res.AddReturnedValue(ex, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 121);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetFailureConfigAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await Manager.SetFailureConfigAsync(), ReturnedState.None, "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 130);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 134);
			res.AddReturnedValue(ex, "", "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 135);
		}
		return res;
	}

	protected string GetServicePath()
	{
		try
		{
			string value = EnvironmentManager<string>.GetValue(ServiceName);
			if (!string.IsNullOrEmpty(value))
			{
				return value;
			}
			return Path.Combine(Application.StartupPath, ExeName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServicePath", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 149);
			throw;
		}
	}

	private static void ProcessKiller(string processName)
	{
		try
		{
			Process process = Process.GetProcessesByName(processName)?.FirstOrDefault();
			if (process != null)
			{
				string text = process?.MainModule?.FileName;
				process.Kill();
				if (!string.IsNullOrEmpty(text) && File.Exists(text))
				{
					File.Delete(text);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ProcessKiller", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 165);
		}
	}

	public static async Task StartProcessKillerAsync()
	{
		_ = 1;
		try
		{
			await Task.Delay(60000);
			while (true)
			{
				ProcessKiller("ScaleDal");
				ProcessKiller("WebNP");
				await Task.Delay(300000);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartProcessKillerAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 180);
		}
	}

	public static List<ServiceFailureConfig> GetServiceFailureConfig()
	{
		try
		{
			return EnvironmentManager<List<ServiceFailureConfig>>.GetValue(SrviceFailureConfigKey);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServiceFailureConfig", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 191);
			return null;
		}
	}

	public static void SetServiceFailureConfig(List<ServiceFailureConfig> data)
	{
		try
		{
			if (data != null && data.Any())
			{
				EnvironmentManager<List<ServiceFailureConfig>>.SetValue(SrviceFailureConfigKey, data);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetServiceFailureConfig", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 202);
		}
	}

	public Task<ReturnedSaveFuncInfo> CreateScheduledTaskAsync(string taskName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0065: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			TaskService val = new TaskService();
			try
			{
				TaskDefinition val2 = val.NewTask();
				val2.RegistrationInfo.Description = "ری\u200cاستارت خودکار سرویس هر 15 دقیقه بدون نمایش پنجره";
				TimeTrigger val3 = new TimeTrigger
				{
					StartBoundary = DateTime.Now.AddMinutes(1.0),
					Repetition = new RepetitionPattern(TimeSpan.FromMinutes(15.0), TimeSpan.Zero, false)
				};
				val2.Triggers.Add<TimeTrigger>(val3);
				string text = Path.Combine(Application.StartupPath, "SilentServiceRestarter.exe");
				string serviceName = ServiceName;
				val2.Actions.Add<ExecAction>(new ExecAction(text, serviceName, (string)null));
				val2.Settings.Enabled = true;
				val2.Settings.AllowDemandStart = true;
				val2.Settings.StartWhenAvailable = true;
				val2.Settings.RunOnlyIfNetworkAvailable = false;
				val2.Principal.RunLevel = (TaskRunLevel)1;
				val.RootFolder.RegisterTaskDefinition(taskName, val2);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateScheduledTaskAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 246);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CreateScheduledTaskAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\ServiceBaseManager.cs", 247);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}
}
