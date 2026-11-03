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

public class SmsSentClientHub : RetriableHubConnection, ISmsSentSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public SmsSentClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<BaseSmsSent>> GetAsync(int id)
	{
		ReturnedSaveFuncInfoWithValue<BaseSmsSent> ret = new ReturnedSaveFuncInfoWithValue<BaseSmsSent>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 29);
			}
			ReturnedSaveFuncInfoWithValue<BaseSmsSent> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<BaseSmsSent>>(base.Connection, "GetAsync", (object)id, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 32);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 38);
			ret.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 39);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>> GetAllAsync()
	{
		ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> ret = new ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 51);
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>>(base.Connection, "GetAllAsync", default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 54);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 60);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 61);
		}
		return ret;
	}

	public async Task<Dictionary<short, long>> GetAllCountDeliverStatus(CancellationToken token)
	{
		_ = 1;
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
			}
			return await HubConnectionExtensions.InvokeAsync<Dictionary<short, long>>(base.Connection, "GetAllCountDeliverStatus", (object)token, default(CancellationToken));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllCountDeliverStatus", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 79);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>> GetAllAsync(string number, DeliveryStatus deliverySt, string message, string senderNumber, DateTime? date1, DateTime? date2)
	{
		ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> ret = new ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 91);
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>>(base.Connection, "GetAllAsyncWithNumber", (object)number, (object)deliverySt, (object)message, (object)senderNumber, (object)date1, (object)date2, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 94);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 100);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 101);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(BaseSmsSent item, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 113);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SaveAsync", (object)item, (object)token, default(CancellationToken)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 115);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 119);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 120);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteListAsync(List<int> list, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 132);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "DeleteListAsync", (object)list, (object)token, default(CancellationToken)), ReturnedState.None, "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 134);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 138);
			ret.AddReturnedValue(ex, "", "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 139);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> CountAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 151);
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<int>>(base.Connection, "CountAsync", (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 154);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 160);
			ret.AddReturnedValue(ex, "", "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 161);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> CountSentTodayMessagesAsync(string senderNumber, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "CountSentTodayMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 173);
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<int>>(base.Connection, "CountSentTodayMessagesAsync", (object)senderNumber, (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CountSentTodayMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 176);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CountSentTodayMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 182);
			ret.AddReturnedValue(ex, "", "CountSentTodayMessagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 183);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<long>>> GetRefrensedIdsAsync(int accid, int count)
	{
		ReturnedSaveFuncInfoWithValue<List<long>> ret = new ReturnedSaveFuncInfoWithValue<List<long>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetRefrensedIdsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 195);
			}
			ReturnedSaveFuncInfoWithValue<List<long>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<long>>>(base.Connection, "GetRefrensedIdsAsync", (object)accid, (object)count, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetRefrensedIdsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 198);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRefrensedIdsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 204);
			ret.AddReturnedValue(ex, "", "GetRefrensedIdsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 205);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>> GetAllByRefrenceGuidAsync(Guid refrenceGuid)
	{
		ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> ret = new ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllByRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 217);
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<BaseSmsSent>>>(base.Connection, "GetAllByRefrenceGuidAsync", (object)refrenceGuid, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllByRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 220);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllByRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 226);
			ret.AddReturnedValue(ex, "", "GetAllByRefrenceGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsSentClientHub.cs", 227);
		}
		return ret;
	}
}
