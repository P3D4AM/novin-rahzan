using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IDbCommandInterceptor : IInterceptor
{
	InterceptionResult<DbCommand> CommandCreating([JetBrains.Annotations.NotNull] CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result);

	DbCommand CommandCreated([JetBrains.Annotations.NotNull] CommandEndEventData eventData, [JetBrains.Annotations.NotNull] DbCommand result);

	InterceptionResult<DbDataReader> ReaderExecuting([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandEventData eventData, InterceptionResult<DbDataReader> result);

	InterceptionResult<object> ScalarExecuting([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandEventData eventData, InterceptionResult<object> result);

	InterceptionResult<int> NonQueryExecuting([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandEventData eventData, InterceptionResult<int> result);

	Task<InterceptionResult<DbDataReader>> ReaderExecutingAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default(CancellationToken));

	Task<InterceptionResult<object>> ScalarExecutingAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default(CancellationToken));

	Task<InterceptionResult<int>> NonQueryExecutingAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default(CancellationToken));

	DbDataReader ReaderExecuted([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandExecutedEventData eventData, [JetBrains.Annotations.NotNull] DbDataReader result);

	object ScalarExecuted([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandExecutedEventData eventData, [JetBrains.Annotations.CanBeNull] object result);

	int NonQueryExecuted([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandExecutedEventData eventData, int result);

	Task<DbDataReader> ReaderExecutedAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandExecutedEventData eventData, [JetBrains.Annotations.NotNull] DbDataReader result, CancellationToken cancellationToken = default(CancellationToken));

	Task<object> ScalarExecutedAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandExecutedEventData eventData, [JetBrains.Annotations.CanBeNull] object result, CancellationToken cancellationToken = default(CancellationToken));

	Task<int> NonQueryExecutedAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandExecutedEventData eventData, int result, CancellationToken cancellationToken = default(CancellationToken));

	void CommandFailed([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandErrorEventData eventData);

	Task CommandFailedAsync([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] CommandErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken));

	InterceptionResult DataReaderDisposing([JetBrains.Annotations.NotNull] DbCommand command, [JetBrains.Annotations.NotNull] DataReaderDisposingEventData eventData, InterceptionResult result);
}
