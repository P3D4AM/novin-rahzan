using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class SanadPrintWaiter : WaiterBase, INpDisposable, IDisposable
{
	private bool _isDisposed;

	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public SanadPrintWaiter(WebContextHolder context, Guid guid, CancellationToken token, int timeOutMiliSeconds = 300000)
		: base(context, guid, token, timeOutMiliSeconds, "SanadPrintWaiter")
	{
		try
		{
			context.SanadPrint.OnSaveResponse += Instance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SanadPrintWaiter.cs", 23);
		}
	}

	private async Task Instance_OnSaveResponse(object sender, SaveResponseEventArgs e)
	{
		try
		{
			if (e.SaveResponses.SingleOrDefault().Guid == base.EntityGuid)
			{
				base.Waiter.TrySetResult(e.SaveResponses.SingleOrDefault((SaveResponse x) => x.Guid == base.EntityGuid));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SanadPrintWaiter.cs", 33);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				base.Instance.SanadPrint.OnSaveResponse -= Instance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\SanadPrintWaiter.cs", 45);
		}
	}
}
