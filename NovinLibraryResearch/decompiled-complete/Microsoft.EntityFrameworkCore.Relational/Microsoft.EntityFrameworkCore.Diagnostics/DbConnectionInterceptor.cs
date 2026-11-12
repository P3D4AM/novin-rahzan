using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class DbConnectionInterceptor : IDbConnectionInterceptor, IInterceptor
{
	public virtual InterceptionResult ConnectionOpening(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
	{
		return result;
	}

	public virtual Task<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
	{
	}

	public virtual Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public virtual InterceptionResult ConnectionClosing(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
	{
		return result;
	}

	public virtual Task<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
	{
		return Task.FromResult(result);
	}

	public virtual void ConnectionClosed(DbConnection connection, ConnectionEndEventData eventData)
	{
	}

	public virtual Task ConnectionClosedAsync(DbConnection connection, ConnectionEndEventData eventData)
	{
		return Task.CompletedTask;
	}

	public virtual void ConnectionFailed(DbConnection connection, ConnectionErrorEventData eventData)
	{
	}

	public virtual Task ConnectionFailedAsync(DbConnection connection, ConnectionErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}
}
