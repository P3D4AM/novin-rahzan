using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Department;

public class LkInfoWaiter : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private long _lkSerial;

	private TaskCompletionSource<SaveResponse> Waiter { get; } = new TaskCompletionSource<SaveResponse>();


	private CancellationTokenSource Token { get; set; }

	private WebContextHolder Context { get; }

	public bool IsDisposed => _isDisposed;

	public LkInfoWaiter(WebContextHolder context, long lkSerial, CancellationToken token, int timeOutMiliSeconds = 300000)
	{
		try
		{
			_lkSerial = lkSerial;
			Context = context;
			context.WebLkInfo.OnSaveResponse += Instance_OnSaveResponse;
			Token = CancellationTokenSource.CreateLinkedTokenSource(token, new CancellationTokenSource(timeOutMiliSeconds).Token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Department\\LkInfoWaiter.cs", 33);
		}
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			if (e.SaveResponses.Any((SaveResponse x) => x.ReferencedId == _lkSerial))
			{
				Waiter.TrySetResult(e.SaveResponses.SingleOrDefault((SaveResponse x) => x.ReferencedId == _lkSerial));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Department\\LkInfoWaiter.cs", 43);
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
				Token?.Cancel();
				Token?.Dispose();
				Token = null;
				Context.WebLkInfo.OnSaveResponse -= Instance_OnSaveResponse;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Department\\LkInfoWaiter.cs", 60);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<SaveResponse>> WaitForSaveResponse()
	{
		ReturnedSaveFuncInfoWithValue<SaveResponse> ret = new ReturnedSaveFuncInfoWithValue<SaveResponse>();
		try
		{
			ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = await Waiter.Task;
			if (IsDisposed || Token.IsCancellationRequested)
			{
				ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Department\\LkInfoWaiter.cs", 71);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Department\\LkInfoWaiter.cs", 76);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Department\\LkInfoWaiter.cs", 77);
		}
		return ret;
	}
}
