using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TransferLayer.Waiters.Accounting.Hesab;

public class KolWaiter : WaiterBase
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

	public KolWaiter(WebContextHolder instance, Guid guid, CancellationToken token, int timeOutMiliSeconds = 300000)
		: base(instance, guid, token, timeOutMiliSeconds, "KolWaiter")
	{
		try
		{
			base.Instance.WebKol.OnSaveResponse += Instance_OnSaveResponse;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Hesab\\KolWaiter.cs", 24);
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
			WebErrorLog.Instance.StartLog(ex, "", "Instance_OnSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Hesab\\KolWaiter.cs", 34);
		}
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				_isDisposed = true;
				base.Instance.WebKol.OnSaveResponse -= Instance_OnSaveResponse;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.Error(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\TransferLayer\\Waiters\\Accounting\\Hesab\\KolWaiter.cs", 50);
		}
	}
}
