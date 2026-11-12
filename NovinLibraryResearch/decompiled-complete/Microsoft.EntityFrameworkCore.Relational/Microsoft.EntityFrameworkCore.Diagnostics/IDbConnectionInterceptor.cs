using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public interface IDbConnectionInterceptor : IInterceptor
{
	InterceptionResult ConnectionOpening([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEventData eventData, InterceptionResult result);

	Task<InterceptionResult> ConnectionOpeningAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default(CancellationToken));

	void ConnectionOpened([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEndEventData eventData);

	Task ConnectionOpenedAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEndEventData eventData, CancellationToken cancellationToken = default(CancellationToken));

	InterceptionResult ConnectionClosing([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEventData eventData, InterceptionResult result);

	Task<InterceptionResult> ConnectionClosingAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEventData eventData, InterceptionResult result);

	void ConnectionClosed([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEndEventData eventData);

	Task ConnectionClosedAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionEndEventData eventData);

	void ConnectionFailed([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionErrorEventData eventData);

	Task ConnectionFailedAsync([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] ConnectionErrorEventData eventData, CancellationToken cancellationToken = default(CancellationToken));
}
