using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class ExecuteQueryWaiter : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private WebContextHolder _context;

	private Guid _packetRefrencedId;

	private int _timeOutMiliSeconds;

	private CancellationToken _token;

	private TaskCompletionSource<ExecuteQueryResponseEventArgs> _waiter;

	public bool IsDisposed => _isDisposed;

	public ExecuteQueryWaiter(WebContextHolder context, Guid packetRefrencedId, CancellationToken token, int timeOutMiliSeconds = 30000)
	{
		try
		{
			_context = context;
			_packetRefrencedId = packetRefrencedId;
			_timeOutMiliSeconds = timeOutMiliSeconds;
			_token = token;
			_waiter = new TaskCompletionSource<ExecuteQueryResponseEventArgs>();
			context.WebExecuteQuery.OnExecuteQueryResponse += WebExecuteQuery_OnExecuteQueryResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\ExecuteQueryWaiter.cs", 38);
		}
	}

	private Task WebExecuteQuery_OnExecuteQueryResponse(object sender, ConnectionEventArgs<ExecuteQueryResponseEventArgs> e)
	{
		try
		{
			if (e.ConnectionData.Single().PacketReferenceId == _packetRefrencedId)
			{
				_waiter.TrySetResult(e.ConnectionData.Single());
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebExecuteQuery_OnExecuteQueryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\ExecuteQueryWaiter.cs", 48);
		}
		return Task.CompletedTask;
	}

	public async Task<ExecuteQueryResponseEventArgs> WaiteForExecuteResponseAsync()
	{
		_ = 1;
		try
		{
			Task delayTask = Task.Delay(_timeOutMiliSeconds);
			if (await Task.WhenAny(new Task[2] { delayTask, _waiter.Task }) == delayTask)
			{
				return new ExecuteQueryResponseEventArgs
				{
					PacketReferenceId = _packetRefrencedId,
					Result = false,
					ResultMessage = $"متاسفانه پاسخ در زمان مناسب دریافت نشد.\r\nزمان انتظار جهت پاسخ: {_timeOutMiliSeconds / 1000:N0}(ثانبه)"
				};
			}
			return await _waiter.Task;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaiteForExecuteResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\ExecuteQueryWaiter.cs", 70);
			return new ExecuteQueryResponseEventArgs
			{
				PacketReferenceId = _packetRefrencedId,
				Result = false,
				ResultMessage = "خطای داخلی سرور.\r\n"
			};
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_context.WebExecuteQuery.OnExecuteQueryResponse -= WebExecuteQuery_OnExecuteQueryResponse;
				_context = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\ExecuteQueryWaiter.cs", 93);
		}
	}
}
