using System;
using System.Linq;
using System.Threading;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Assistance;

public class LockItem : INpDisposable, IDisposable
{
	private bool _isDisposed;

	public CancellationTokenSource TokenSource { get; private set; }

	public SemaphoreSlim Semaphore { get; private set; } = new SemaphoreSlim(1, 1);


	public CancellationToken Token => TokenSource?.Token ?? new CancellationToken(canceled: true);

	public bool IsDisposed => _isDisposed;

	public LockItem(params CancellationToken[] tokens)
	{
		try
		{
			if (tokens != null && tokens.Any())
			{
				TokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokens);
			}
			else
			{
				TokenSource = new CancellationTokenSource();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\LockItem.cs", 25);
		}
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
				TokenSource?.Cancel();
				TokenSource?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Assistance\\LockItem.cs", 46);
		}
	}
}
