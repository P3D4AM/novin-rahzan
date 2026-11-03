using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer;

public class RegisterCustomerWaiter : IDisposable
{
	private WebContextHolder _context;

	private CancellationToken _token;

	private int _timeOut;

	private TaskCompletionSource<CustomerSaveResponse> _cusTmpCmtRsp = new TaskCompletionSource<CustomerSaveResponse>();

	private WebCus _cus;

	public Task<CustomerSaveResponse> GetTempSaveResponse => _cusTmpCmtRsp.Task;

	public RegisterCustomerWaiter(WebCus cus, WebContextHolder context, CancellationToken Token = default(CancellationToken), int timeout = 30)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs"))
		{
			try
			{
				_cus = cus;
				context.WebCus.OnTempCommitResponse += CustomerInstance_OnTempSaveResponse;
				_context = context;
				_token = Token;
				_timeOut = checked(timeout * 1000);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs", 34);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<CustomerSaveResponse>> SendTempRequst()
	{
		ReturnedSaveFuncInfoWithValue<CustomerSaveResponse> ret = new ReturnedSaveFuncInfoWithValue<CustomerSaveResponse>();
		using (new JaegerMonitor("SendTempRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs"))
		{
			_ = 1;
			try
			{
				if (await Task.WhenAny(new Task[2]
				{
					_cusTmpCmtRsp.Task,
					Task.Delay(_timeOut, _token)
				}) == _cusTmpCmtRsp.Task)
				{
					ReturnedSaveFuncInfoWithValue<CustomerSaveResponse> returnedSaveFuncInfoWithValue = ret;
					returnedSaveFuncInfoWithValue.value = await _cusTmpCmtRsp.Task;
				}
				else
				{
					ret.value = new CustomerSaveResponse
					{
						Status = CusTmpCmtRsp.ErrorInServer
					};
					ret.AddError(EnSystem.None, "timeOut", "SendTempRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs", 50);
				}
			}
			catch (TimeoutException exception)
			{
				ret.AddReturnedValue(exception, "", "SendTempRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs", 55);
				ret.value = new CustomerSaveResponse
				{
					Status = CusTmpCmtRsp.ErrorInServer
				};
			}
			catch (OperationCanceledException exception2)
			{
				ret.AddReturnedValue(exception2, "", "SendTempRequst", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs", 60);
				ret.value = new CustomerSaveResponse
				{
					Status = CusTmpCmtRsp.ErrorInServer
				};
			}
			return ret;
		}
	}

	private async Task CustomerInstance_OnTempSaveResponse(object sender, CustomerTempResponseEventArgs e)
	{
		if (e.RequestedCustomerGuid == _cus.Guid)
		{
			_cusTmpCmtRsp.TrySetResult(e.CustomerSaveResponse);
		}
	}

	public void Dispose()
	{
		try
		{
			_context.WebCus.OnTempCommitResponse -= CustomerInstance_OnTempSaveResponse;
			_context = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\RegisterCustomerWaiter.cs", 79);
		}
	}
}
