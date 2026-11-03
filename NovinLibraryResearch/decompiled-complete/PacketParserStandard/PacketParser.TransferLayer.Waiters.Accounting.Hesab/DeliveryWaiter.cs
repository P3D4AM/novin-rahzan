using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class DeliveryWaiter : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private TaskCompletionSource<WebDelivery> Waiter = new TaskCompletionSource<WebDelivery>();

	public CancellationToken Token { get; private set; }

	public int TimeOutMiliSeconds { get; private set; }

	public WebContextHolder Context { get; private set; }

	public Guid EntityGuid { get; private set; }

	public EntityPart EntityPart { get; private set; }

	public bool IsDisposed => _isDisposed;

	public DeliveryWaiter(WebContextHolder context, Guid guid, EntityPart part, CancellationToken token, int timeOutMiliSeconds = 300000)
	{
		try
		{
			EntityGuid = guid;
			EntityPart = part;
			Context = context;
			TimeOutMiliSeconds = timeOutMiliSeconds;
			Token = token;
			context.WebDelivery.OnReadResponse += WebDelivery_OnReadResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryWaiter.cs", 33);
		}
	}

	private Task WebDelivery_OnReadResponse(object sender, ConnectionEventArgs<WebDelivery> e)
	{
		try
		{
			WebDelivery webDelivery = e.ConnectionData.SingleOrDefault((WebDelivery p) => p.Guid == EntityGuid && p.Part == EntityPart);
			if (webDelivery != null)
			{
				Waiter.TrySetResult(webDelivery);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebDelivery_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryWaiter.cs", 51);
		}
		return Task.CompletedTask;
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
				Context.WebDelivery.OnReadResponse -= WebDelivery_OnReadResponse;
				Context = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryWaiter.cs", 69);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebDelivery>> WaitForDelivery(int timeOut = 60000)
	{
		ReturnedSaveFuncInfoWithValue<WebDelivery> ret = new ReturnedSaveFuncInfoWithValue<WebDelivery>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(TimeOutMiliSeconds, Token),
				Waiter.Task
			}) == Waiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<WebDelivery> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await Waiter.Task;
			}
			else
			{
				string message = $"((delivery))__waiter timedOut or token Canceled after {TimeOutMiliSeconds}(ms)";
				ret.AddWarning(message, "", "WaitForDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryWaiter.cs", 83);
				ret.value = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryWaiter.cs", 89);
			ret.value = null;
			ret.AddReturnedValue(ex, "", "WaitForDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryWaiter.cs", 91);
		}
		return ret;
	}
}
