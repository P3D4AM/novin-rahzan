using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public abstract class DbCommandInterceptor : IDbCommandInterceptor, IInterceptor
{
	public virtual InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
	{
		return result;
	}

	public virtual DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
	{
		return result;
	}

	public virtual InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
	{
		return result;
	}

	public virtual InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
	{
		return result;
	}

	public virtual InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
	{
		return result;
	}

	public virtual Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
	{
		return result;
	}

	public virtual object ScalarExecuted(DbCommand command, CommandExecutedEventData eventData, object result)
	{
		return result;
	}

	public virtual int NonQueryExecuted(DbCommand command, CommandExecutedEventData eventData, int result)
	{
		return result;
	}

	public virtual Task<DbDataReader> ReaderExecutedAsync(DbCommand command, CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task<object> ScalarExecutedAsync(DbCommand command, CommandExecutedEventData eventData, object result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual Task<int> NonQueryExecutedAsync(DbCommand command, CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.FromResult(result);
	}

	public virtual void CommandFailed(DbCommand command, CommandErrorEventData eventData)
	{
	}

	public virtual Task CommandFailedAsync(DbCommand command, CommandErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return Task.CompletedTask;
	}

	public virtual InterceptionResult DataReaderDisposing(DbCommand command, DataReaderDisposingEventData eventData, InterceptionResult result)
	{
		return result;
	}
}
