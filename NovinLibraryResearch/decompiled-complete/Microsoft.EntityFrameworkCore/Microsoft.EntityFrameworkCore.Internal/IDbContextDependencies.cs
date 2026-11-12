using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbContextDependencies
{
	IModel Model { get; }

	IDbSetSource SetSource { get; }

	IEntityFinderFactory EntityFinderFactory { get; }

	IAsyncQueryProvider QueryProvider { get; }

	IStateManager StateManager { get; }

	IChangeDetector ChangeDetector { get; }

	IEntityGraphAttacher EntityGraphAttacher { get; }

	IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

	IDiagnosticsLogger<DbLoggerCategory.Infrastructure> InfrastructureLogger { get; }
}
