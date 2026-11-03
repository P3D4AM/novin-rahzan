using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.Note;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using Serilog;

namespace ClientHub.AccountingHubs.Note;

public class NoteClientHub : RetriableHubConnection, INoteSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebNote>> _getAllByFilterTask;

	private TaskCompletionSource<List<WebNote>> _getAllTask;

	private TaskCompletionSource<WebNote> _getTask;

	private TaskCompletionSource<int> _countNumber;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveTask;

	public string ContextGuid { get; set; }

	public NoteClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebNote>>(base.Connection, "OnGetAllByFilterAsync", (Action<List<WebNote>>)delegate(List<WebNote> e)
			{
				try
				{
					_getAllByFilterTask?.TrySetResult(e);
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Note\\NoteClientHub.cs", 34);
				}
			});
			HubConnectionExtensions.On<WebNote>(base.Connection, "OnGetAllByFilterAsync", (Action<WebNote>)delegate(WebNote e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Note\\NoteClientHub.cs", 42);
				}
			});
			HubConnectionExtensions.On<int>(base.Connection, "OnCountNumbersInGroupAsync", (Action<int>)delegate(int e)
			{
				try
				{
					_countNumber?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Note\\NoteClientHub.cs", 50);
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
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Note\\NoteClientHub.cs", 58);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\Note\\NoteClientHub.cs", 61);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebNote>>> GetAllAsync(NoteFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebNote>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> CountNumbersInGroupAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebNote item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
