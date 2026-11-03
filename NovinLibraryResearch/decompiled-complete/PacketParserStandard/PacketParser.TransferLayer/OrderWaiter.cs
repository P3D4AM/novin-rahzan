using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace PacketParser.TransferLayer;

public class OrderWaiter : WaiterBase
{
	private bool _isDisposed;

	public new bool IsDisposed => _isDisposed;

	public OrderWaiter(WebContextHolder context, Guid entityGuid, CancellationToken token = default(CancellationToken), int timeout = 30000)
		: base(context, entityGuid, token, timeout, "OrderWaiter")
	{
		base.Instance.WebOrder.OnSaveResponse += FactorInstance_OnSaveResponse;
	}

	private async Task FactorInstance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			if (e.SaveResponses != null)
			{
				SaveResponse saveResponse = e.SaveResponses.FirstOrDefault((SaveResponse w) => w.Guid == base.EntityGuid);
				if (saveResponse != null)
				{
					base.Waiter.TrySetResult(saveResponse);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FactorInstance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\OrderWaiter.cs", 39);
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
				base.Instance.WebOrder.OnSaveResponse -= FactorInstance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\OrderWaiter.cs", 60);
		}
	}
}
