using System;

namespace Ninject.Infrastructure.Disposal;

public abstract class DisposableObject : IDisposableObject, IDisposable, INotifyWhenDisposed
{
	public bool IsDisposed { get; private set; }

	public event EventHandler Disposed;

	~DisposableObject()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		lock (this)
		{
			if (disposing && !IsDisposed)
			{
				IsDisposed = true;
				this.Disposed?.Invoke(this, EventArgs.Empty);
				this.Disposed = null;
				GC.SuppressFinalize(this);
			}
		}
	}
}
