using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class GpsWaiter : IDisposable
{
	private TaskCompletionSource<bool> TaskCompletion = new TaskCompletionSource<bool>();

	private TaskCompletionSource<List<Guid>> TaskCompletionlst = new TaskCompletionSource<List<Guid>>();

	private int TimeOut;

	private WebContextHolder _context;

	private Guid _guid;

	private IEnumerable<Guid> _guids;

	private CancellationToken token;

	public GpsWaiter(int timeOut, CancellationToken token)
	{
		TimeOut = checked(timeOut * 60);
		this.token = token;
	}

	public async Task<ReturnedSaveFuncInfo> WaitForSaveResponse(Guid guid, WebContextHolder context)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_context = context;
			_guid = guid;
			context.WebGPS.OnSaveResponse -= GpsInstance_OnSaveResponse;
			context.WebGPS.OnSaveResponse += GpsInstance_OnSaveResponse;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 37);
			}
			TaskCompletion = new TaskCompletionSource<bool>();
			Task task = await Task.WhenAny(new Task[2]
			{
				TaskCompletion.Task,
				Task.Delay(TimeOut, token)
			});
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 41);
			}
			if (task != TaskCompletion.Task)
			{
				return ret.AddError(EnSystem.None, "result != TaskCompletion.Task", "guid", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 42);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 46);
			ret.AddReturnedValue(ex, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 47);
		}
		return ret;
	}

	public async Task<List<Guid>> WaitForSaveResponse(IEnumerable<Guid> guids, WebContextHolder context)
	{
		_ = 1;
		try
		{
			_guids = guids;
			context.WebGPS.OnSaveResponse -= GpsInstance_OnSaveListResponse;
			context.WebGPS.OnSaveResponse += GpsInstance_OnSaveListResponse;
			TaskCompletionlst = new TaskCompletionSource<List<Guid>>();
			return (await Task.WhenAny(new Task[2]
			{
				TaskCompletionlst.Task,
				Task.Delay(TimeOut)
			}) != TaskCompletionlst.Task) ? null : (await TaskCompletionlst.Task);
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 68);
			return null;
		}
	}

	private async Task GpsInstance_OnSaveResponse(object arg1, SaveResponseEventArgs e)
	{
		try
		{
			if ((e.SaveResponses.SingleOrDefault((SaveResponse w) => w.SaveResultStatus && w.Guid == _guid)?.Guid ?? Guid.Empty) != Guid.Empty)
			{
				TaskCompletionlst.TrySetResult(new List<Guid> { _guid });
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GpsInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 84);
		}
	}

	private async Task GpsInstance_OnSaveListResponse(object arg1, SaveResponseEventArgs e)
	{
		try
		{
			if (e.SaveResponses == null)
			{
				TaskCompletionlst.TrySetResult(null);
			}
			if (_guids == null && _guid == Guid.Empty)
			{
				return;
			}
			e.SaveResponses[0].Guid.ToString();
			_guids.FirstOrDefault((Guid w) => w == e.SaveResponses[0].Guid);
			List<Guid> result = (from w in e.SaveResponses
				where w.SaveResultStatus && _guids.Any((Guid a) => a == w.Guid)
				select w into s
				select s.Guid).ToList();
			TaskCompletionlst.TrySetResult(result);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GpsInstance_OnSaveListResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 101);
		}
	}

	public void Dispose()
	{
		Dispose(isDisposing: true);
	}

	public void Dispose(bool isDisposing)
	{
		try
		{
			_context.WebGPS.OnSaveResponse -= GpsInstance_OnSaveResponse;
			_context = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\GpsWaiter.cs", 112);
		}
	}
}
