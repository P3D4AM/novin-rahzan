using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.ServiceManagement;

public class NssmServiceManager
{
	private Process _process;

	private StringBuilder _outPut;

	private StringBuilder _error;

	public string ServiceName { get; set; }

	public string Path { get; set; }

	public NssmServiceManager(string serviceName, string path)
	{
		try
		{
			Path = path;
			if (serviceName.EndsWith(".exe"))
			{
				serviceName = serviceName.Replace(".exe", "");
			}
			ServiceName = serviceName;
			Init();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 32);
		}
	}

	private void Init()
	{
		try
		{
			_process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe")
			{
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				UseShellExecute = false,
				CreateNoWindow = true,
				WorkingDirectory = Path
			};
			_process.StartInfo = startInfo;
			_outPut = new StringBuilder();
			_error = new StringBuilder();
			AutoResetEvent outputWaitHandle = new AutoResetEvent(initialState: false);
			AutoResetEvent errorWaitHandle = new AutoResetEvent(initialState: false);
			_process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (e.Data == null)
				{
					outputWaitHandle.Set();
				}
				else
				{
					_outPut.AppendLine(e.Data);
				}
			};
			_process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
			{
				if (e.Data == null)
				{
					errorWaitHandle.Set();
				}
				else
				{
					_outPut.AppendLine(e.Data);
				}
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Init", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 65);
		}
	}

	private ReturnedSaveFuncInfo CheckPathValidation()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (!Directory.Exists(Path))
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "فایل ها یا پوشه مورد نظر سرویس(" + ServiceName + ") مورد نظر یافت نشد.\r\nلطفا ابتدا سرویس را در پوشه اجرای برنامه قرارداده و سپس امتحان نمایید\r\nآدرس مورد نظر برای سرویس:\r\n" + Path, "", "CheckPathValidation", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 74);
				return returnedSaveFuncInfo;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckPathValidation", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 83);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckPathValidation", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 84);
		}
		return returnedSaveFuncInfo;
	}

	private ServiceController GetService()
	{
		try
		{
			ServiceController[] services = ServiceController.GetServices();
			foreach (ServiceController val in services)
			{
				if (val.ServiceName == ServiceName)
				{
					return val;
				}
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetService", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 98);
		}
		return null;
	}

	public ServiceStatus CheckService()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected I4, but got Unknown
		try
		{
			ServiceController service = GetService();
			if (service == null)
			{
				return ServiceStatus.NotFound;
			}
			ServiceControllerStatus status = service.Status;
			switch (status - 1)
			{
			case 4:
				return ServiceStatus.ContinuePending;
			case 6:
				return ServiceStatus.Paused;
			case 5:
				return ServiceStatus.PausePending;
			case 3:
				return ServiceStatus.Running;
			case 1:
				return ServiceStatus.StartPending;
			case 0:
				return ServiceStatus.Stopped;
			case 2:
				return ServiceStatus.StopPending;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckService", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 118);
		}
		return ServiceStatus.NotFound;
	}

	public async Task<ReturnedSaveFuncInfo> InstallServiceAsync(string ip, int port, string sqlConnectionSring = "")
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ServiceStatus serviceStatus = CheckService();
			if (serviceStatus != ServiceStatus.NotFound)
			{
				res.AddError(EnSystem.None, "سرویس".GetTranslate() + " (" + ServiceName + ") " + "در حال حاضر نصب و در حالت".GetTranslate() + " " + serviceStatus.GetDisplay().GetTranslate() + " " + "می باشد".GetTranslate(), "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 129);
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await InstallScaleAsync(ip, port, sqlConnectionSring), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 132);
			if (res.HasError)
			{
				return res;
			}
			await Task.Delay(1000);
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await StartScaleAsync(), ReturnedState.None, "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 135);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 139);
			res.AddReturnedValue(ex, "", "InstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 140);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> InstallScaleAsync(string ip, int port, string sqlConnectionSring)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			res.AddReturnedValue(CheckPathValidation(), ReturnedState.None, "InstallScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 149);
			if (res.HasError)
			{
				return res;
			}
			_process.Start();
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
			await _process.StandardInput.WriteLineAsync("cd /d " + Path);
			string text = System.IO.Path.Combine(Path, ServiceName + ".exe");
			string value = $"nssm install {ServiceName} \"{text}\" \"service --ip:{ip} --port:{port} --connection-string:\"\"{sqlConnectionSring}\"\"\"";
			if (string.IsNullOrEmpty(sqlConnectionSring))
			{
				value = $"nssm install {ServiceName} \"{text}\" \"service --ip:{ip} --port:{port}\"\"\"";
			}
			await _process.StandardInput.WriteLineAsync(value);
			await _process.StandardInput.WriteLineAsync("ScaleService firewall add 6000");
			_process.CancelOutputRead();
			_process.CancelErrorRead();
			_process.Close();
			_error = (_outPut = new StringBuilder());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InstallScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 172);
			res.AddReturnedValue(ex, "", "InstallScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 173);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> StartServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CheckService() == ServiceStatus.Running)
			{
				res.AddError(EnSystem.None, "سرویس مورد نظر در حال حاضر فعال می باشد".GetTranslate(), "", "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 185);
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await StartScaleAsync(), ReturnedState.None, "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 188);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 192);
			res.AddReturnedValue(ex, "", "StartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 193);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> StartScaleAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			int tryCount = 0;
			res.AddReturnedValue(CheckPathValidation(), ReturnedState.None, "StartScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 203);
			if (res.HasError)
			{
				return res;
			}
			ServiceController sc = GetService();
			if (sc == null)
			{
				return res.AddError(EnSystem.None, "اجرای سرویس با شکست مواجه شد.".GetTranslate() + "\r\n" + "سرویسی با نام".GetTranslate() + "(" + ServiceName + ") " + "یافت نشد".GetTranslate(), "", "StartScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 207);
			}
			if ((int)sc.Status == 1)
			{
				sc.Start();
				while ((int)sc.Status == 1)
				{
					if (tryCount > 5)
					{
						res.AddError(EnSystem.None, "سیستم قادر به اجرای سرویس".GetTranslate() + "(" + ServiceName + ")" + " نمی باشد".GetTranslate(), "", "StartScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 216);
						return res;
					}
					await Task.Delay(1000);
					sc.Refresh();
					tryCount++;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StartScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 227);
			res.AddReturnedValue(ex, "", "StartScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 228);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> StopServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ServiceStatus serviceStatus = CheckService();
			if (serviceStatus == ServiceStatus.Stopped)
			{
				res.AddError(EnSystem.None, "سرویس".GetTranslate() + " (" + ServiceName + ") " + " در حال حاضر در وضعیت".GetTranslate() + " " + $" ({serviceStatus}-{serviceStatus.GetDisplay().GetTranslate()})" + " می باشد".GetTranslate(), "", "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 240);
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await StopScaleAsync(), ReturnedState.None, "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 243);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 247);
			res.AddReturnedValue(ex, "", "StopServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 248);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> StopScaleAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			int tryCount = 0;
			res.AddReturnedValue(CheckPathValidation(), ReturnedState.None, "StopScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 258);
			if (res.HasError)
			{
				return res;
			}
			ServiceController sc = GetService();
			if (sc == null)
			{
				return res.AddError(EnSystem.None, "توقف سرویس با شکست موجه شد.".GetTranslate() + "\r\n" + "سرویس".GetTranslate() + " (" + ServiceName + ")" + " یافت نشد".GetTranslate(), "", "StopScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 261);
			}
			if ((int)sc.Status != 1)
			{
				sc.Stop();
				while ((int)sc.Status != 1)
				{
					if (tryCount > 5)
					{
						res.AddError(EnSystem.None, "سیستم قادر به توقف سرویس".GetTranslate() + " (" + ServiceName + ") " + " نمی باشد".GetTranslate(), "", "StopScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 270);
						return res;
					}
					await Task.Delay(1000);
					sc.Refresh();
					tryCount++;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "StopScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 281);
			res.AddReturnedValue(ex, "", "StopScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 282);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> UnInstallServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CheckService() == ServiceStatus.NotFound)
			{
				res.AddError(EnSystem.None, "سرویس مورد نظر یافت نشد".GetTranslate(), "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 294);
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = await StopScaleAsync();
			if (returnedSaveFuncInfo.HasError)
			{
				foreach (string error in returnedSaveFuncInfo.ErrorList)
				{
					res.AddWarning(error, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 301);
				}
			}
			if (returnedSaveFuncInfo.HasWarning)
			{
				foreach (string warning in returnedSaveFuncInfo.WarningList)
				{
					res.AddWarning(warning, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 305);
				}
			}
			if (returnedSaveFuncInfo.HasInformation)
			{
				foreach (string information in returnedSaveFuncInfo.InformationList)
				{
					res.AddInformation(information, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 309);
				}
			}
			if (res.HasError)
			{
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo2 = res;
			returnedSaveFuncInfo2.AddReturnedValue(await UnInstallScaleAsync(), ReturnedState.None, "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 313);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 317);
			res.AddReturnedValue(ex, "", "UnInstallServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 318);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> UnInstallScaleAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			res.AddReturnedValue(CheckPathValidation(), ReturnedState.None, "UnInstallScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 327);
			if (res.HasError)
			{
				return res;
			}
			_process.Start();
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
			await _process.StandardInput.WriteLineAsync("cd /d " + Path);
			await _process.StandardInput.WriteLineAsync("nssm remove " + ServiceName + " confirm");
			_process.CancelOutputRead();
			_process.CancelErrorRead();
			_process.Close();
			_error = (_outPut = new StringBuilder());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UnInstallScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 345);
			res.AddReturnedValue(ex, "", "UnInstallScaleAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 346);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RestartServiceAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CheckService() == ServiceStatus.NotFound)
			{
				res.AddError(EnSystem.None, "سرویس مورد نظر یافت نشد".GetTranslate(), "", "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 358);
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await StopScaleAsync(), ReturnedState.None, "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 361);
			if (res.HasError)
			{
				return res;
			}
			returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await StartServiceAsync(), ReturnedState.None, "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 363);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 367);
			res.AddReturnedValue(ex, "", "RestartServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 368);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetFailureConfigAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (CheckService() == ServiceStatus.NotFound)
			{
				res.AddError(EnSystem.None, "سرویس مورد نظر یافت نشد".GetTranslate(), "", "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 380);
				return res;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SetServiceFailureConfigAsync(), ReturnedState.None, "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 384);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 388);
			res.AddReturnedValue(ex, "", "SetFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 389);
		}
		return res;
	}

	private async Task<ReturnedSaveFuncInfo> SetServiceFailureConfigAsync()
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			res.AddReturnedValue(CheckPathValidation(), ReturnedState.None, "SetServiceFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 398);
			if (res.HasError)
			{
				return res;
			}
			_process.Start();
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
			await _process.StandardInput.WriteLineAsync("cd /d " + Path);
			await _process.StandardInput.WriteLineAsync("sc failure " + ServiceName + " reset=60 actions= restart/60000/restart/60000/restart/60000");
			_process.CancelOutputRead();
			_process.CancelErrorRead();
			_process.Close();
			_error = (_outPut = new StringBuilder());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetServiceFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 416);
			res.AddReturnedValue(ex, "", "SetServiceFailureConfigAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ServiceManagement\\Classes\\NssmServiceManager.cs", 417);
		}
		return res;
	}
}
