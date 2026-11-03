using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class RegisterCommitCusWaiter : IDisposable
{
	private bool _isDisposed;

	private CancellationToken _token;

	private int _timeOut;

	private TaskCompletionSource<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> _cusTmpCmtRsp = new TaskCompletionSource<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>>();

	private WebContextHolder _context;

	private WebCus _cus;

	public Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> GetTempSaveResponseAsync => _cusTmpCmtRsp.Task;

	public bool IsDisposed => _isDisposed;

	public RegisterCommitCusWaiter(WebCus cus, WebContextHolder context, CancellationToken token = default(CancellationToken), int timeout = 30)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs"))
		{
			try
			{
				_context = context;
				_cus = cus;
				context.WebCus.OnTempCommitResponse += CustomerInstance_OnTempCommitResponse;
				_token = token;
				_timeOut = checked(timeout * 1000);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs", 40);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>> SendTempCommitRequst()
	{
		using (new JaegerMonitor("SendTempCommitRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs"))
		{
			ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp> ret = new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>();
			try
			{
				if (await Task.WhenAny(new Task[2]
				{
					_cusTmpCmtRsp.Task,
					Task.Delay(_timeOut, _token)
				}) == _cusTmpCmtRsp.Task)
				{
					ret.AddReturnedValue(await _cusTmpCmtRsp.Task, ReturnedState.None, "SendTempCommitRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs", 56);
				}
			}
			catch (TimeoutException exception)
			{
				ret.AddReturnedValue(exception, "", "SendTempCommitRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs", 61);
			}
			catch (OperationCanceledException exception2)
			{
				ret.AddReturnedValue(exception2, "", "SendTempCommitRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs", 65);
			}
			return ret;
		}
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_context.WebCus.OnTempCommitResponse -= CustomerInstance_OnTempCommitResponse;
				_context = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCommitCusWaiter.cs", 80);
		}
	}

	private Task CustomerInstance_OnTempCommitResponse(object arg1, CustomerTempResponseEventArgs e)
	{
		if (e.RequestedCustomerGuid == _cus.Guid)
		{
			_cusTmpCmtRsp.TrySetResult(new ReturnedSaveFuncInfoWithValue<CusTmpCmtRsp>
			{
				value = e.CustomerSaveResponse.Status
			});
		}
		return Task.CompletedTask;
	}
}
