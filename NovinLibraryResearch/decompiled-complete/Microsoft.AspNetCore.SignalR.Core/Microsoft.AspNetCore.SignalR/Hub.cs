using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Internal;

namespace Microsoft.AspNetCore.SignalR;

public abstract class Hub : IDisposable
{
	private bool _disposed;

	private IHubCallerClients _clients;

	private HubCallerContext _context;

	private IGroupManager _groups;

	public IHubCallerClients Clients
	{
		get
		{
			CheckDisposed();
			return _clients;
		}
		set
		{
			CheckDisposed();
			_clients = value;
		}
	}

	public HubCallerContext Context
	{
		get
		{
			CheckDisposed();
			return _context;
		}
		set
		{
			CheckDisposed();
			_context = value;
		}
	}

	public IGroupManager Groups
	{
		get
		{
			CheckDisposed();
			return _groups;
		}
		set
		{
			CheckDisposed();
			_groups = value;
		}
	}

	public virtual Task OnConnectedAsync()
	{
		return Task.CompletedTask;
	}

	public virtual Task OnDisconnectedAsync(Exception exception)
	{
		return Task.CompletedTask;
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			Dispose(disposing: true);
			_disposed = true;
		}
	}

	private void CheckDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
	}
}
public abstract class Hub<T> : Hub where T : class
{
	private IHubCallerClients<T> _clients;

	public new IHubCallerClients<T> Clients
	{
		get
		{
			if (_clients == null)
			{
				_clients = new TypedHubClients<T>(base.Clients);
			}
			return _clients;
		}
		set
		{
			_clients = value;
		}
	}
}
