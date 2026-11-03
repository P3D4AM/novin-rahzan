using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters;

public class SettingWaiter : IDisposable
{
	private WebContextHolder _context;

	private string _settingKey;

	private TaskCompletionSource<bool> _taskCompletionSource = new TaskCompletionSource<bool>();

	public SettingWaiter(string contextGuid, string settingKey)
	{
		_context = WebContextHolder.GetWebContextHolder(contextGuid);
		_settingKey = settingKey;
		_context.WebApplicationSetting.OnSaveResponse += WebApplicationSetting_OnSaveResponse;
	}

	private async Task WebApplicationSetting_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			_taskCompletionSource.TrySetResult(result: true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebApplicationSetting_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\SettingWaiter.cs", 28);
		}
	}

	public async Task<ReturnedSaveFuncInfo> WaitForSaveResponse(int timeoutMiliSeconds = 30000)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Task timeOutTask = Task.Delay(timeoutMiliSeconds);
			if (await Task.WhenAny(new Task[2] { timeOutTask, _taskCompletionSource.Task }) == timeOutTask)
			{
				return ret.AddError(EnSystem.None, "waiter timed out. ", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\SettingWaiter.cs", 39);
			}
			if (!(await _taskCompletionSource.Task))
			{
				ret.AddError(EnSystem.None, "error on save settings.", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\SettingWaiter.cs", 41);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\SettingWaiter.cs", 46);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\SettingWaiter.cs", 47);
		}
		return ret;
	}

	public void Dispose()
	{
		try
		{
			_context.WebApplicationSetting.OnSaveResponse -= WebApplicationSetting_OnSaveResponse;
			_taskCompletionSource?.TrySetCanceled();
			_context = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\SettingWaiter.cs", 60);
		}
	}
}
