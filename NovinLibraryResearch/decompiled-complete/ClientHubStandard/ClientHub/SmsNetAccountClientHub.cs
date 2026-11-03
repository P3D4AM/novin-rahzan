using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;
using Serilog;

namespace ClientHub;

public class SmsNetAccountClientHub : RetriableHubConnection, ISmsNetAccountSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public SmsNetAccountClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>> GetByIdAsync(int id)
	{
		ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> ret = new ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس پیامک برقرار نمیباشد.", "GetByIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 29);
			}
			ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>>(base.Connection, "GetByIdAsync", (object)id, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetByIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 32);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 38);
			ret.AddReturnedValue(ex, "", "GetByIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 39);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>> GetByNumberAsync(string senderNumber)
	{
		ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> ret = new ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 51);
			}
			ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount>>(base.Connection, "GetByNumberAsync", (object)senderNumber, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 54);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 60);
			ret.AddReturnedValue(ex, "", "GetByNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 61);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>>> GetAllAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>> ret = new ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 73);
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<BaseSmsNetAccount>>>(base.Connection, "GetAllAsync", (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 76);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 82);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 83);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(BaseSmsNetAccount item, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 95);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SaveAsync", (object)item, (object)token, default(CancellationToken)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 97);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 101);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 102);
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
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 114);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "DeleteAsync", (object)id, (object)token, default(CancellationToken)), ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 116);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 120);
			ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 121);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<decimal>> CheckAccountCreditAsync(int accountId, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<decimal> res = new ReturnedSaveFuncInfoWithValue<decimal>();
		try
		{
			ReturnedSaveFuncInfoWithValue<decimal> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "CheckAccountCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 130);
			if (res.HasError)
			{
				return res;
			}
			res.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<decimal>>(base.Connection, "CheckAccountCreditAsync", (object)accountId, (object)token, default(CancellationToken)), ReturnedState.None, "CheckAccountCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 133);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckAccountCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsNetAccountClientHub.cs", 135);
		}
		return res;
	}
}
