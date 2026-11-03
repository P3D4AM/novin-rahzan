using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class LastPriceWaiter : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private readonly WebContextHolder _instance;

	private readonly LastPriceCompositeKey _key;

	private int _timeout { get; set; }

	private TaskCompletionSource<SaveResponse<LastPriceCompositeKey>> _waiter { get; set; }

	private CancellationToken _token { get; set; }

	public bool IsDisposed => _isDisposed;

	public LastPriceWaiter(WebContextHolder context, LastPriceCompositeKey key, CancellationToken token, int timeOutMiliSeconds = 300000)
	{
		try
		{
			_waiter = new TaskCompletionSource<SaveResponse<LastPriceCompositeKey>>();
			_instance = context;
			_key = key;
			_timeout = timeOutMiliSeconds;
			_token = token;
			_instance.WebLastPrice.OnSaveResponse += Instance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LastPriceWaiter.cs", 39);
		}
	}

	private Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs<LastPriceCompositeKey> e)
	{
		try
		{
			List<SaveResponse<LastPriceCompositeKey>> saveResponses = e.SaveResponses;
			if (saveResponses != null && saveResponses.Any((SaveResponse<LastPriceCompositeKey> p) => p.Key == null))
			{
				throw new NullReferenceException("save responses contains null keys!\r\n#SaveResponseJsonData:\r\n" + Json.ToStringJson(e.SaveResponses));
			}
			if (e.SaveResponses.Any((SaveResponse<LastPriceCompositeKey> x) => x.Key.ProductGuid == _key.ProductGuid && x.Key.TafsilGuid == _key.TafsilGuid))
			{
				_waiter.TrySetResult(e.SaveResponses.SingleOrDefault((SaveResponse<LastPriceCompositeKey> x) => x.Key.TafsilGuid == _key.TafsilGuid && x.Key.ProductGuid == _key.ProductGuid));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LastPriceWaiter.cs", 54);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<SaveResponse<LastPriceCompositeKey>>> WaitForSaveResponse()
	{
		ReturnedSaveFuncInfoWithValue<SaveResponse<LastPriceCompositeKey>> ret = new ReturnedSaveFuncInfoWithValue<SaveResponse<LastPriceCompositeKey>>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(_timeout, _token),
				_waiter.Task
			}) == _waiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<SaveResponse<LastPriceCompositeKey>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await _waiter.Task;
			}
			else
			{
				string text = $"((LastPrice))_waiter timedOut or token Canceled after {_timeout}(ms). EnSystem.SignalR";
				ret.AddError(EnSystem.SignalR, text, "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LastPriceWaiter.cs", 69);
				ret.value = new SaveResponse<LastPriceCompositeKey>(0, new LastPriceCompositeKey
				{
					ProductGuid = _key.ProductGuid,
					TafsilGuid = _key.TafsilGuid
				}, DateTime.Now, saveResultStatus: false, text);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LastPriceWaiter.cs", 80);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LastPriceWaiter.cs", 81);
		}
		return ret;
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
				_waiter?.TrySetCanceled(_token);
				_instance.WebLastPrice.OnSaveResponse -= Instance_OnSaveResponse;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\LastPriceWaiter.cs", 100);
		}
	}
}
