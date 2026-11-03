using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using Serilog;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class WaiterBase : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private string _caller;

	protected TaskCompletionSource<SaveResponse> Waiter { get; private set; }

	protected int Timeout { get; private set; }

	protected CancellationToken Token { get; private set; }

	protected Guid EntityGuid { get; private set; } = Guid.Empty;


	protected WebContextHolder Instance { get; private set; }

	public bool IsDisposed => _isDisposed;

	public WaiterBase(WebContextHolder instance, Guid guid, CancellationToken token, int timeOutMiliSeconds, string caller)
	{
		try
		{
			_caller = caller;
			Waiter = new TaskCompletionSource<SaveResponse>();
			Timeout = timeOutMiliSeconds;
			Token = token;
			Instance = instance;
			EntityGuid = guid;
			Instance.WebDelivery.OnReadResponse += WebDelivery_OnReadResponse;
			Task.Run(() => AutoDisposeAsync(Timeout));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 45);
		}
	}

	private async Task AutoDisposeAsync(int timeout)
	{
		try
		{
			Task timeOutTask = Task.Delay(timeout);
			if (await Task.WhenAny(new Task[2] { Waiter.Task, timeOutTask }) == timeOutTask)
			{
				Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AutoDisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 56);
			Dispose();
		}
	}

	public virtual void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Waiter.TrySetCanceled(Token);
				Instance.WebDelivery.OnReadResponse -= WebDelivery_OnReadResponse;
				Instance = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 76);
		}
	}

	private Task WebDelivery_OnReadResponse(object sender, ConnectionEventArgs<WebDelivery> e)
	{
		try
		{
			if (e.ConnectionData.Any((WebDelivery x) => x.Guid == EntityGuid))
			{
				WebDelivery webDelivery = e.ConnectionData.SingleOrDefault((WebDelivery x) => x.Guid == EntityGuid);
				if (webDelivery != null)
				{
					DateTime valueOrDefault = webDelivery.DeliveryDate.GetValueOrDefault();
					Waiter.TrySetResult(new SaveResponse(0, webDelivery.Guid, valueOrDefault, webDelivery.DeliveryStatus, webDelivery.ShortMessage, webDelivery.Message));
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebDelivery_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 93);
		}
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<SaveResponse>> WaitForSaveResponse(string description)
	{
		ReturnedSaveFuncInfoWithValue<SaveResponse> ret = new ReturnedSaveFuncInfoWithValue<SaveResponse>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(Timeout, Token),
				Waiter.Task
			}) == Waiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<SaveResponse> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await Waiter.Task;
			}
			else
			{
				string text = "Caller:" + _caller + "\r\n" + $"waiter timedOut or token Canceled after {Timeout}(ms)\r\n" + "Description:" + description;
				ret.AddWarning(text, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 110);
				ret.value = new SaveResponse(0, EntityGuid, DateTime.Now, saveResultStatus: false, text);
				Log.Warning(text);
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 118);
			ret.AddReturnedValue(ex2, "", "WaitForSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\WaiterBase.cs", 119);
		}
		return ret;
	}
}
