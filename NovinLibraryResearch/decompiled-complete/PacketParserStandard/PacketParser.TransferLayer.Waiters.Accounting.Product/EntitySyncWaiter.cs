using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace PacketParser.TransferLayer.Waiters.Accounting.Product;

public class EntitySyncWaiter : WaiterBase, INpDisposable, IDisposable
{
	private bool _isDisposed;

	public new bool IsDisposed => _isDisposed;

	public EntitySyncWaiter(WebContextHolder context, Guid guid, CancellationToken token, int timeOutMiliSeconds = 300000)
		: base(context, guid, token, timeOutMiliSeconds, "ProductWaiter")
	{
		try
		{
			base.Instance.WebEntitySync.OnSaveResponse += Instance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\EntitySyncWaiter.cs", 27);
		}
	}

	private Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			SaveResponse saveResponse = e.SaveResponses.SingleOrDefault((SaveResponse x) => x.Guid == base.EntityGuid);
			if (saveResponse != null)
			{
				base.Waiter.TrySetResult(saveResponse);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\EntitySyncWaiter.cs", 40);
		}
		return Task.CompletedTask;
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				base.Instance.WebEntitySync.OnSaveResponse -= Instance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\EntitySyncWaiter.cs", 59);
		}
	}
}
