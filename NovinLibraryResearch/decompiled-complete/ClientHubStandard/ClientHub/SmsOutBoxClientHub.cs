using System;
using System.Collections.Generic;
using System.Linq;
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

public class SmsOutBoxClientHub : RetriableHubConnection, ISmsOutBoxSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public SmsOutBoxClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<BaseSmsOutBox>> NextMessageGsmAsync(string senderNumber, EnSmsSenderType _type)
	{
		ReturnedSaveFuncInfoWithValue<BaseSmsOutBox> ret = new ReturnedSaveFuncInfoWithValue<BaseSmsOutBox>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "NextMessageGsmAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 31);
			}
			ReturnedSaveFuncInfoWithValue<BaseSmsOutBox> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<BaseSmsOutBox>>(base.Connection, "NextMessageGsmAsync", (object)senderNumber, (object)_type, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "NextMessageGsmAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 34);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NextMessageGsmAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 40);
			ret.AddReturnedValue(ex, "", "NextMessageGsmAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 41);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<int>>> SaveAsync(BaseSmsOutBox item, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<int>> ret = new ReturnedSaveFuncInfoWithValue<List<int>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 53);
			}
			ReturnedSaveFuncInfoWithValue<List<int>> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<int>>>(base.Connection, "SaveAsync", (object)item, (object)token, default(CancellationToken)), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 55);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 59);
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 60);
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
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 72);
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<int>>(base.Connection, "CountAsync", (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 75);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 81);
			ret.AddReturnedValue(ex, "", "CountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 82);
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
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 94);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "DeleteListAsync", (object)list, (object)token, default(CancellationToken)), ReturnedState.None, "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 96);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 100);
			ret.AddReturnedValue(ex, "", "DeleteListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 101);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> UpdateDeliveryStatusAsync(int deliverySt, long refrencedId, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "UpdateDeliveryStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 113);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "UpdateDeliveryStatusAsync", (object)deliverySt, (object)refrencedId, (object)token, default(CancellationToken)), ReturnedState.None, "UpdateDeliveryStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 115);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateDeliveryStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 119);
			ret.AddReturnedValue(ex, "", "UpdateDeliveryStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 120);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>>> GetAllAsync(CancellationToken token, DateTime date1, DateTime date2, bool st = true)
	{
		ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>> ret = new ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 132);
			}
			ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<BaseSmsOutBox>>>(base.Connection, "GetAllAsync", (object)token, (object)date1, (object)date2, (object)st, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 135);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 141);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 142);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<string>>> GetAllScadulingDateShAsync()
	{
		ReturnedSaveFuncInfoWithValue<List<string>> ret = new ReturnedSaveFuncInfoWithValue<List<string>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetAllScadulingDateShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 154);
			}
			ReturnedSaveFuncInfoWithValue<List<string>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<string>>>(base.Connection, "GetAllScadulingDateShAsync", default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllScadulingDateShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 157);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllScadulingDateShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 163);
			ret.AddReturnedValue(ex, "", "GetAllScadulingDateShAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 164);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> SelectAccountIdAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<int> ret = new ReturnedSaveFuncInfoWithValue<int>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "SelectAccountIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 176);
			}
			ReturnedSaveFuncInfoWithValue<int> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<int>>(base.Connection, "SelectAccountIdAsync", (object)token, default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "SelectAccountIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 179);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SelectAccountIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 185);
			ret.AddReturnedValue(ex, "", "SelectAccountIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 186);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<BaseSmsOutBox> items, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return ret;
			}
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 199);
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SaveRangeAsync", (object)items, (object)token, default(CancellationToken)), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 201);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 205);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\SmsOutBoxClientHub.cs", 206);
		}
		return ret;
	}
}
