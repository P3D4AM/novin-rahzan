using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Entities.NoteGroup;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace ClientHub.AccountingHubs.NoteGroup;

public class NoteGroupClientHub : RetriableHubConnection, INoteGroupSignalRClient, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	private TaskCompletionSource<List<WebNoteGroup>> _getAllTask;

	private TaskCompletionSource<List<WebNoteGroup>> _getAllByGuidTask;

	private TaskCompletionSource<WebNoteGroup> _checkTask;

	private TaskCompletionSource<WebNoteGroup> _getTask;

	private TaskCompletionSource<int> _countGroup;

	private TaskCompletionSource<ReturnedSaveFuncInfo> _saveTask;

	public string ContextGuid { get; set; }

	public NoteGroupClientHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		try
		{
			HubConnectionExtensions.On<List<WebNoteGroup>>(base.Connection, "OnGetAllAsync", (Action<List<WebNoteGroup>>)delegate(List<WebNoteGroup> e)
			{
				try
				{
					_getAllTask?.TrySetResult(e);
				}
				catch (Exception ex7)
				{
					WebErrorLog.Instance.StartLog(ex7, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 37);
				}
			});
			HubConnectionExtensions.On<WebNoteGroup>(base.Connection, "OnCheckNameAsync", (Action<WebNoteGroup>)delegate(WebNoteGroup e)
			{
				try
				{
					_checkTask?.TrySetResult(e);
				}
				catch (Exception ex6)
				{
					WebErrorLog.Instance.StartLog(ex6, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 45);
				}
			});
			HubConnectionExtensions.On<List<WebNoteGroup>>(base.Connection, "OnGetAllAsync", (Action<List<WebNoteGroup>>)delegate(List<WebNoteGroup> e)
			{
				try
				{
					_getAllByGuidTask?.TrySetResult(e);
				}
				catch (Exception ex5)
				{
					WebErrorLog.Instance.StartLog(ex5, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 53);
				}
			});
			HubConnectionExtensions.On<WebNoteGroup>(base.Connection, "OnGetAsync", (Action<WebNoteGroup>)delegate(WebNoteGroup e)
			{
				try
				{
					_getTask?.TrySetResult(e);
				}
				catch (Exception ex4)
				{
					WebErrorLog.Instance.StartLog(ex4, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 61);
				}
			});
			HubConnectionExtensions.On<int>(base.Connection, "OnCountGroupChildsAsync", (Action<int>)delegate(int e)
			{
				try
				{
					_countGroup?.TrySetResult(e);
				}
				catch (Exception ex3)
				{
					WebErrorLog.Instance.StartLog(ex3, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 70);
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
					WebErrorLog.Instance.StartLog(ex2, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 78);
				}
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeConnectionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\AccountingHubs\\NoteGroup\\NoteGroupClientHub.cs", 81);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebNoteGroup>>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebNoteGroup>> CheckNameAsync(Guid guid, string name, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebNoteGroup>>> GetAllAsync(Guid parentGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebNoteGroup>> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfoWithValue<int>> CountGroupChildsAsync(Guid groupGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(WebNoteGroup item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
