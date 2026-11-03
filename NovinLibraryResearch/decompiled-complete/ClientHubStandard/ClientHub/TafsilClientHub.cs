using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using Serilog;

namespace ClientHub;

public class TafsilClientHub : RetriableHubConnection, ITafsilSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<WebTafsil> _getTask;

	private TaskCompletionSource<WebTafsil> _getByType;

	private TaskCompletionSource<WebTafsil> _getByCode;

	private TaskCompletionSource<List<WebTafsil>> _getAllTask;

	private TaskCompletionSource<ReturnedSaveFuncInfoWithValue<long>> _nextCodeTask;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveTask;

	public string ContextGuid { get; set; }

	public TafsilClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<WebTafsil>(base.Connection, "OnGetTafsil", (Action<WebTafsil>)delegate(WebTafsil e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex7)
				{
					WebErrorLog.Instance.StartLog(ex7, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 36);
				}
			});
			HubConnectionExtensions.On<List<WebTafsil>>(base.Connection, "OnGetAllItems", (Action<List<WebTafsil>>)delegate(List<WebTafsil> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex6)
				{
					WebErrorLog.Instance.StartLog(ex6, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 44);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfoWithValue<long>>(base.Connection, "OnNextCodeInGroupAsync", (Action<ReturnedSaveFuncInfoWithValue<long>>)delegate(ReturnedSaveFuncInfoWithValue<long> e)
			{
				try
				{
					_nextCodeTask?.TrySetResult(e);
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 52);
				}
			});
			HubConnectionExtensions.On<WebTafsil>(base.Connection, "OnGetByTypeAndCodeAsync", (Action<WebTafsil>)delegate(WebTafsil e)
			{
				try
				{
					_getByType?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 60);
				}
			});
			HubConnectionExtensions.On<WebTafsil>(base.Connection, "OnGetByCodeAsync", (Action<WebTafsil>)delegate(WebTafsil e)
			{
				try
				{
					_getByCode?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 68);
				}
			});
			HubConnectionExtensions.On<ReturnedSaveFuncInfo>(base.Connection, "OnSaveAsync", (Action<ReturnedSaveFuncInfo>)delegate(ReturnedSaveFuncInfo e)
			{
				try
				{
					_saveTask?.TrySetResult(e);
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 76);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Hesab\\TafsilClientHub.cs", 79);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebTafsil>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebTafsil>>> GetAllAsync(EnSystem system, TafsilFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<long>> NextCodeInGroupAsync(Guid tfGuid, EnSuggestionType suggestionMode, CancellationToken token, string tanName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebTafsil>> GetByTypeAndCodeAsync(HesabType hesabType, long code, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebTafsil>> GetByCodeAsync(long tafsilCode, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebTafsil tafsil, bool updateAccount, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
