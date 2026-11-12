using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Data.SqlClient;

public abstract class SqlRetryLogicBaseProvider
{
	public EventHandler<SqlRetryingEventArgs> Retrying { get; set; }

	public SqlRetryLogicBase RetryLogic { get; protected set; }

	public abstract TResult Execute<TResult>(object sender, Func<TResult> function);

	public abstract Task<TResult> ExecuteAsync<TResult>(object sender, Func<Task<TResult>> function, CancellationToken cancellationToken = default(CancellationToken));

	public abstract Task ExecuteAsync(object sender, Func<Task> function, CancellationToken cancellationToken = default(CancellationToken));
}
