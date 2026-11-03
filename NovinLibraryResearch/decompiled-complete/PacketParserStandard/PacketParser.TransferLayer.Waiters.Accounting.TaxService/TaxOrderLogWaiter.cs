using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;

namespace PacketParser.TransferLayer.Waiters.Accounting.TaxService;

public class TaxOrderLogWaiter : WaiterBase, INpDisposable, IDisposable
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

	public TaxOrderLogWaiter(WebContextHolder context, Guid guid, CancellationToken token, int timeOutMiliSeconds = 300000)
		: base(context, guid, token, timeOutMiliSeconds, "TaxOrderLogWaiter")
	{
		try
		{
			context.TaxOrderLog.OnSaveResponse += InstanceOnSaveResponseAsync;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\TaxService\\TaxOrderLogWaiter.cs", 24);
		}
	}

	private Task InstanceOnSaveResponseAsync(object sender, SaveResponseEventArgs e)
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
			WebErrorLog.Instance.StartLog(ex, "", "InstanceOnSaveResponseAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\TaxService\\TaxOrderLogWaiter.cs", 34);
		}
		return Task.CompletedTask;
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				base.Instance.TaxOrderLog.OnSaveResponse -= InstanceOnSaveResponseAsync;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\TaxService\\TaxOrderLogWaiter.cs", 47);
		}
	}
}
