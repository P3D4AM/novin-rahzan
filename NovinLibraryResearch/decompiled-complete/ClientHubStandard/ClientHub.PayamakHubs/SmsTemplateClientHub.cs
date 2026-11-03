using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using PacketParser.Payamak.Interfaces;
using Serilog;

namespace ClientHub.PayamakHubs;

public class SmsTemplateClientHub : RetriableHubConnection, ISmsTemplateSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public SmsTemplateClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>>> GetAllAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>> ret = new ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 29);
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<BaseSmsTemplate>>>(base.Connection, "GetAllAsync", (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 32);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 38);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 39);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<BaseSmsTemplate>> GetAsync(int id, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<BaseSmsTemplate> ret = new ReturnedSaveFuncInfoWithValue<BaseSmsTemplate>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 51);
			}
			ReturnedSaveFuncInfoWithValue<BaseSmsTemplate> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<BaseSmsTemplate>>(base.Connection, "GetAsync", (object)id, (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 54);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 60);
			ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 61);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(BaseSmsTemplate item, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 73);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SaveAsync", (object)item, (object)token, default(CancellationToken)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 75);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 79);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 80);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(int id, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 92);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "DeleteAsync", (object)id, (object)token, default(CancellationToken)), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 94);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 98);
			ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsTemplateClientHub.cs", 99);
		}
		return ret;
	}
}
