using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClientHub;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.GoldAndCurrency;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace SignalRCoreConnectionCore.GoldAndCurrency;

public class GoldAndCurrencySignalRClient : RetriableHubConnection, IGoldAndCurrencySignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private bool _isDisposed;

	public string ContextGuid { get; set; }

	public event Func<List<WebGoldAndCurrencyDetail>, Task> DataRecieved;

	public event Func<List<WebGoldAndCurrencyReport>, Task> OnLogin;

	public new event Func<object, EventArgs, Task> OnDisposed;

	public GoldAndCurrencySignalRClient(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebGoldAndCurrencyDetail>>(base.Connection, "NotifyPriceChangeAsync", (Action<List<WebGoldAndCurrencyDetail>>)delegate(List<WebGoldAndCurrencyDetail> listData)
			{
				this.DataRecieved?.Invoke(listData);
			});
			HubConnectionExtensions.On<List<WebGoldAndCurrencyReport>>(base.Connection, "ClientOnLoginAsync", (Action<List<WebGoldAndCurrencyReport>>)delegate(List<WebGoldAndCurrencyReport> listData)
			{
				this.OnLogin?.Invoke(listData);
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 40);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>>> GetRankedDataAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>> ret = new ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>>();
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
				return ret.AddError("ارتباط با سرویس برقرار نمیباشد.", "GetRankedDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 51);
			}
			ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>> returnedSaveFuncInfoWithValue = await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfoWithValue<List<WebGoldAndCurrencyReport>>>(base.Connection, "GetRankedDataAsync", default(CancellationToken));
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetRankedDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 54);
			if (ret.HasError)
			{
				return ret;
			}
			ret.value = returnedSaveFuncInfoWithValue.value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetRankedDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 60);
			ret.AddReturnedValue(ex, "", "GetRankedDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 61);
		}
		return ret;
	}

	public async Task NotifyPriceChangeAsync(List<WebGoldAndCurrencyDetail> entities)
	{
		_ = 1;
		try
		{
			if ((int)base.Connection.State == 0)
			{
				await base.Connection.StopAsync(default(CancellationToken));
			}
			await base.Connection.SendCoreAsync("SendChangedDataToClientsAsync", new object[1] { entities }, new CancellationTokenSource(20000).Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "NotifyPriceChangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 76);
		}
	}

	public override async ValueTask DisposeAsync()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				this.DataRecieved = null;
				if (this.OnDisposed != null)
				{
					Utilities.RaiseEvent(this.OnDisposed, this, new EventArgs(), default(CancellationToken), 60, "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs");
				}
				await base.DisposeAsync();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencySignalRClient.cs", 91);
		}
	}
}
