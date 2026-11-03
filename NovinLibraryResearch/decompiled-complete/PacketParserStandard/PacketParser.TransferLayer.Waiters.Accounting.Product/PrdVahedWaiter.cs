using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace PacketParser.TransferLayer.Waiters.Accounting.Product;

public class PrdVahedWaiter : WaiterBase, INpDisposable, IDisposable
{
	private bool _isDisposed;

	public new bool IsDisposed => _isDisposed;

	public PrdVahedWaiter(WebContextHolder context, Guid guid, CancellationToken token, int timeOutMiliSeconds = 300000)
		: base(context, guid, token, timeOutMiliSeconds, "PrdVahedWaiter")
	{
		try
		{
			base.Instance.WebPrdVahed.OnSaveResponse += Instance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Product\\PrdVahedWaiter.cs", 26);
		}
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Product\\PrdVahedWaiter.cs", 38);
		}
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
				base.Instance.WebPrdVahed.OnSaveResponse -= Instance_OnSaveResponse;
				base.Dispose(disposing);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Product\\PrdVahedWaiter.cs", 56);
		}
	}
}
