using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.Hesab;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.AccountingHubs.Hesab;

public class TafsilNumberClientHub : RetriableHubConnection, ITafsilNumberSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebTafsilNumbers>> _task;

	private TaskCompletionSource<List<WebTafsilNumbers>> _getAllTask;

	private TaskCompletionSource<List<WebTafsilNumbers>> _getInfoTask;

	private TaskCompletionSource<List<string>> _getTitle;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _removeTask;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveRangeTask;

	private TaskCompletionSource<List<WebTafsilNumberReport>> _getAllReport;

	public string ContextGuid { get; set; }

	public TafsilNumberClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebTafsilNumbers>>(base.Connection, "OnGetInfoAsync", (Action<List<WebTafsilNumbers>>)delegate(List<WebTafsilNumbers> e)
			{
				try
				{
					_task?.TrySetResult(e);
				}
				catch (Exception ex8)
				{
					WebErrorLog.Instance.StartLog(ex8, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 34);
				}
			});
			HubConnectionExtensions.On<List<WebTafsilNumbers>>(base.Connection, "OnGetAllAsync", (Action<List<WebTafsilNumbers>>)delegate(List<WebTafsilNumbers> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex7)
				{
					WebErrorLog.Instance.StartLog(ex7, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 42);
				}
			});
			HubConnectionExtensions.On<List<string>>(base.Connection, "OnGetTitleListAsync", (Action<List<string>>)delegate(List<string> e)
			{
				try
				{
					_getTitle?.TrySetResult(e);
				}
				catch (Exception ex6)
				{
					WebErrorLog.Instance.StartLog(ex6, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 50);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnRemoveAsync", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_removeTask?.TrySetResult(e);
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 58);
				}
			});
			HubConnectionExtensions.On<List<WebTafsilNumberReport>>(base.Connection, "OnGetAllReportAsync", (Action<List<WebTafsilNumberReport>>)delegate(List<WebTafsilNumberReport> e)
			{
				try
				{
					_getAllReport?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 66);
				}
			});
			HubConnectionExtensions.On<List<WebTafsilNumbers>>(base.Connection, "OnGetInfoByCalcNumberAsync", (Action<List<WebTafsilNumbers>>)delegate(List<WebTafsilNumbers> e)
			{
				try
				{
					_getInfoTask?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 74);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnSaveRangeAsync", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_saveRangeTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 82);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilNumberClientHub.cs", 85);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<WebTafsilNumbers>>> GetInfoAsync(long number, bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTafsilNumbers>>> GetAllAsync(Guid tafsilGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<string>>> GetTitleListAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid tafsilGuid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTafsilNumberReport>>> GetAllReportAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTafsilNumbers>>> GetInfoByCalcNumberAsync(long number, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebTafsilNumbers> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
