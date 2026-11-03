using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class CallingLogWaiter : IDisposable
{
	private bool _isDisposed;

	private TaskCompletionSource<bool> TaskCompletion = new TaskCompletionSource<bool>();

	private TaskCompletionSource<IEnumerable<Guid>> TaskCompletionlst;

	private TaskCompletionSource<IEnumerable<string>> TaskCompletionlstUuid;

	private Guid _guid;

	private string _uuid;

	private int TimeOut;

	private CancellationToken token;

	private WebCallingLog _instance;

	private IEnumerable<Guid> _guids;

	private IEnumerable<string> _uuids;

	public CallingLogWaiter(int timeOutMiliSeconds, CancellationToken token)
	{
		TimeOut = timeOutMiliSeconds;
		this.token = token;
	}

	public CallingLogWaiter(WebCallingLog instance, CancellationToken token, int timeOutMiliSeconds = 300000)
	{
		try
		{
			TaskCompletionlst = new TaskCompletionSource<IEnumerable<Guid>>();
			TimeOut = timeOutMiliSeconds;
			this.token = token;
			_instance = instance;
			instance.OnSaveResponse += WebCallingLogInstance_OnSaveResponse1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 42);
		}
	}

	public async Task<ReturnedSaveFuncInfo> WaitForSaveResponse(Guid guid, WebContextHolder context)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 50);
			}
			_guid = guid;
			context.WebCallingLog.OnSaveResponse -= CallingLogInstance_OnSaveResponse;
			context.WebCallingLog.OnSaveResponse += CallingLogInstance_OnSaveResponse;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 55);
			}
			Task task = await Task.WhenAny(new Task[2]
			{
				TaskCompletion.Task,
				Task.Delay(TimeOut, token)
			});
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 58);
			}
			if (task != TaskCompletion.Task)
			{
				ret.AddError(EnSystem.None, "result != TaskCompletion.Task", "guid", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 59);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 64);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 65);
		}
		return ret;
	}

	public async Task<IEnumerable<Guid>> WaitForSaveResponse(IEnumerable<Guid> guids, WebContextHolder context)
	{
		_ = 1;
		try
		{
			_guids = guids;
			context.WebCallingLog.OnSaveResponse += WebCallingLogInstance_OnSaveResponse1;
			if (await Task.WhenAny(new Task[2]
			{
				TaskCompletionlst.Task,
				Task.Delay(TimeOut)
			}) == TaskCompletionlst.Task)
			{
				return await TaskCompletionlst.Task;
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 87);
			return null;
		}
	}

	public async Task<ReturnedSaveFuncInfo> WaitForSaveResponse(string uuid, WebContextHolder context)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 97);
			}
			_uuid = uuid;
			context.WebCallingLog.OnSaveResponse -= CallingLogInstance_OnSaveResponse;
			context.WebCallingLog.OnSaveResponse += CallingLogInstance_OnSaveResponse;
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 102);
			}
			Task task = await Task.WhenAny(new Task[2]
			{
				TaskCompletion.Task,
				Task.Delay(TimeOut, token)
			});
			if (token.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 105);
			}
			if (task != TaskCompletion.Task)
			{
				ret.AddError(EnSystem.None, "result != TaskCompletion.Task", "uuid", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 106);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 111);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 112);
		}
		return ret;
	}

	public async Task<IEnumerable<string>> WaitForSaveResponse(IEnumerable<string> uuids, WebContextHolder context)
	{
		_ = 1;
		try
		{
			_uuids = uuids;
			context.WebCallingLog.OnSaveResponse += WebCallingLogInstance_OnSaveResponse1;
			if (await Task.WhenAny(new Task[2]
			{
				TaskCompletionlst.Task,
				Task.Delay(TimeOut)
			}) == TaskCompletionlstUuid.Task)
			{
				return await TaskCompletionlstUuid.Task;
			}
			return null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 134);
			return null;
		}
	}

	private Task WebCallingLogInstance_OnSaveResponse1(object arg1, SaveResponseEventArgs e)
	{
		using (new JaegerMonitor("WebCallingLogInstance_OnSaveResponse1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs"))
		{
			int num = 0;
			try
			{
				num = 1;
				if (e.SaveResponses == null)
				{
					TaskCompletionlst.TrySetResult(null);
				}
				num = 50;
				IEnumerable<Guid> result = e?.SaveResponses?.Where(delegate(SaveResponse w)
				{
					SaveResponse saveResponse = w;
					return saveResponse != null && saveResponse.SaveResultStatus && (_guids?.Any((Guid a) => a == w.Guid) ?? false);
				})?.Select((SaveResponse s) => s.Guid);
				num = 6;
				TaskCompletionlst.TrySetResult(result);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{num}", "WebCallingLogInstance_OnSaveResponse1", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 157);
				TaskCompletionlst.TrySetCanceled();
			}
			return Task.CompletedTask;
		}
	}

	private Task CallingLogInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			if ((e.SaveResponses?.Select((SaveResponse p) => p.Guid)?.Contains(_guid)).GetValueOrDefault())
			{
				TaskCompletion?.TrySetResult(result: true);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CallingLogInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 170);
		}
		return Task.CompletedTask;
	}

	public virtual void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				TaskCompletion?.TrySetCanceled();
				TaskCompletion = null;
				TaskCompletionlst?.TrySetCanceled();
				TaskCompletionlst = null;
				if (_instance != null)
				{
					_instance.OnSaveResponse -= WebCallingLogInstance_OnSaveResponse1;
				}
				_instance = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\CallingLogWaiter.cs", 190);
		}
	}
}
