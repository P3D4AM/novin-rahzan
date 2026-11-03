using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class DeliveryBatchWaiter : INpDisposable, IDisposable
{
	private bool _isDisposed;

	private TaskCompletionSource<List<WebDelivery>> Waiter = new TaskCompletionSource<List<WebDelivery>>();

	public CancellationToken Token { get; private set; }

	public int TimeOutMiliSeconds { get; private set; }

	public WebContextHolder Context { get; private set; }

	public List<Guid> EntityGuids { get; private set; }

	public Guid PacketRefrencedId { get; private set; }

	public EntityPart EntityPart { get; private set; }

	public bool IsDisposed => _isDisposed;

	public DeliveryBatchWaiter(WebContextHolder context, Guid packetRefrencedId, List<Guid> guids, EntityPart part, CancellationToken token, int timeOutMiliSeconds = 300000)
	{
		try
		{
			PacketRefrencedId = packetRefrencedId;
			EntityGuids = guids;
			EntityPart = part;
			Context = context;
			TimeOutMiliSeconds = timeOutMiliSeconds;
			Token = token;
			context.WebDelivery.OnReadResponse += WebDelivery_OnReadResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryBatchWaiter.cs", 35);
		}
	}

	private Task WebDelivery_OnReadResponse(object sender, ConnectionEventArgs<WebDelivery> e)
	{
		try
		{
			if (e.PacketReferenceId != PacketRefrencedId)
			{
				return Task.CompletedTask;
			}
			if (e.ConnectionData.Count != EntityGuids.Count)
			{
				throw new InvalidOperationException("count error!!");
			}
			Waiter.TrySetResult(e.ConnectionData);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WebDelivery_OnReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryBatchWaiter.cs", 54);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryBatchWaiter.cs", 72);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebDelivery>>> WaitForDelivery()
	{
		ReturnedSaveFuncInfoWithValue<List<WebDelivery>> ret = new ReturnedSaveFuncInfoWithValue<List<WebDelivery>>();
		try
		{
			if (await Task.WhenAny(new Task[2]
			{
				Task.Delay(TimeOutMiliSeconds, Token),
				Waiter.Task
			}) == Waiter.Task)
			{
				ReturnedSaveFuncInfoWithValue<List<WebDelivery>> returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.value = await Waiter.Task;
			}
			else
			{
				string message = $"((delivery))__waiter timedOut or token Canceled after {TimeOutMiliSeconds}(ms)";
				ret.AddWarning(message, "", "WaitForDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryBatchWaiter.cs", 86);
				ret.value = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WaitForDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryBatchWaiter.cs", 92);
			ret.value = null;
			ret.AddReturnedValue(ex, "", "WaitForDelivery", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\DeliveryBatchWaiter.cs", 94);
		}
		return ret;
	}
}
