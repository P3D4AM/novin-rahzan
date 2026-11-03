using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs.Crm.CallCenter;
using PacketParser.Issabel;
using Serilog;

namespace ClientHub;

public class ReportClientHub : RetriableHubConnection, IReportSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<IsabelleCallCenterBase>> _setting;

	private LoginCallCenterToIssabel _login;

	public string ContextGuid { get; set; }

	public event Func<WebCallingLog, Task> OnNotifToClient;

	public ReportClientHub(string contextGuid, string endPointUrl, ILogger logger, LoginCallCenterToIssabel login)
		: base(endPointUrl, logger)
	{
		try
		{
			_login = login;
			ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 33);
		}
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<WebCallingLog>(base.Connection, "SaveResponseSendToAllClient", (Action<WebCallingLog>)delegate(WebCallingLog call)
			{
				try
				{
					if (call != null)
					{
						this.OnNotifToClient?.Invoke(call);
					}
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 47);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 50);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfo> LoginAsync()
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 58);
			if (ret.HasError)
			{
				return ret;
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "LoginAsync", (object)_login, default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 64);
			ret.AddReturnedValue(ex, "", "LoginAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 65);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>>> GetCallCenterSetting(CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>> res = new ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<List<IsabelleCallCenterBase>> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetCallCenterSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 74);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "GetCallCenterSetting", token);
			_setting = new TaskCompletionSource<List<IsabelleCallCenterBase>>();
			returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.value = await _setting.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCallCenterSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 82);
			res.AddReturnedValue(ex, "", "GetCallCenterSetting", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 83);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> MakeInternalCall(MakeInternalCallEventArgs callInfo1, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "MakeInternalCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 92);
			if (ret.HasError)
			{
				return ret;
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "MakeInternalCall", (object)callInfo1, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MakeInternalCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 98);
			ret.AddReturnedValue(ex, "", "MakeInternalCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 99);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> MakeOutGoinglCall(MakeOutGoingCallEventArgs callInfo, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "MakeOutGoinglCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 108);
			if (ret.HasError)
			{
				return ret;
			}
			await HubConnectionExtensions.SendAsync(base.Connection, "MakeOutGoinglCall", (object)callInfo, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MakeOutGoinglCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 115);
			ret.AddReturnedValue(ex, "", "MakeOutGoinglCall", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\Issabel\\ReportClientHub.cs", 116);
		}
		return ret;
	}
}
