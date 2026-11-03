using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace PacketParser.TransferLayer;

public class CustomerSaveWaiter : WaiterBase
{
	private bool _isDisposed;

	private CancellationToken _token;

	private int _timeOut;

	private TaskCompletionSource<SaveResponse> _cusSaveSource = new TaskCompletionSource<SaveResponse>();

	private WebCus _cus;

	private static ConcurrentDictionary<Guid, TaskCompletionSource<SaveResponse>> dic_Tasks = new ConcurrentDictionary<Guid, TaskCompletionSource<SaveResponse>>();

	public Task<SaveResponse> GetSaveResponse()
	{
		return _cusSaveSource.Task;
	}

	public CustomerSaveWaiter(WebContextHolder context, Guid guid, CancellationToken Token = default(CancellationToken), int timeout = 30000)
		: base(context, guid, Token, timeout, "CustomerSaveWaiter")
	{
		try
		{
			_cus = context.WebCus;
			_token = Token;
			dic_Tasks.TryAdd(_cus.Guid, _cusSaveSource);
			_timeOut = checked(timeout * 1000);
			context.WebCus.OnSaveResponse += WebCus_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\CustomerSaveWaiter.cs", 37);
		}
	}

	private Task WebCus_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			if (e.SaveResponses.Any((SaveResponse x) => x.Guid == base.EntityGuid))
			{
				base.Waiter.TrySetResult(e.SaveResponses.SingleOrDefault((SaveResponse x) => x.Guid == base.EntityGuid));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebCus_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\CustomerSaveWaiter.cs", 49);
		}
		return Task.CompletedTask;
	}

	public async Task<SaveResponse> SendSaveRequest()
	{
		_ = 1;
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				_cusSaveSource.Task,
				Task.Delay(_timeOut, _token)
			}) == _cusSaveSource.Task)
			{
				return await _cusSaveSource.Task;
			}
			return new SaveResponse
			{
				SaveResultStatus = false
			};
		}
		catch (TimeoutException)
		{
			return new SaveResponse
			{
				SaveResultStatus = false
			};
		}
		catch (OperationCanceledException)
		{
			return new SaveResponse
			{
				SaveResultStatus = false
			};
		}
	}

	public static void OnSaveRespose(List<SaveResponse> items)
	{
		try
		{
			if (items == null || !items.Any())
			{
				return;
			}
			foreach (SaveResponse item in items)
			{
				if (dic_Tasks.TryGetValue(item.Guid, out var value))
				{
					value.TrySetResult(item);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnSaveRespose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\CustomerSaveWaiter.cs", 88);
		}
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				base.Instance.WebTafsil.OnSaveResponse -= WebCus_OnSaveResponse;
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Customer\\CustomerSaveWaiter.cs", 110);
		}
	}
}
