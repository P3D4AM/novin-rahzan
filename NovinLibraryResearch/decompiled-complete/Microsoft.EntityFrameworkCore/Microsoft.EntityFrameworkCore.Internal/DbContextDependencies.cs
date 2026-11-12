using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Microsoft.EntityFrameworkCore.Internal;

public sealed class DbContextDependencies : IDbContextDependencies
{
	public IModel Model { get; }

	public IDbSetSource SetSource { get; }

	public IEntityFinderFactory EntityFinderFactory { get; }

	public IAsyncQueryProvider QueryProvider { get; }

	public IStateManager StateManager { get; }

	public IChangeDetector ChangeDetector { get; }

	public IEntityGraphAttacher EntityGraphAttacher { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Update> UpdateLogger { get; }

	public IDiagnosticsLogger<DbLoggerCategory.Infrastructure> InfrastructureLogger { get; }

	public DbContextDependencies([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IChangeDetector changeDetector, [JetBrains.Annotations.NotNull] IDbSetSource setSource, [JetBrains.Annotations.NotNull] IEntityFinderSource entityFinderSource, [JetBrains.Annotations.NotNull] IEntityGraphAttacher entityGraphAttacher, [JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] IAsyncQueryProvider queryProvider, [JetBrains.Annotations.NotNull] IStateManager stateManager, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Update> updateLogger, [JetBrains.Annotations.NotNull] IDiagnosticsLogger<DbLoggerCategory.Infrastructure> infrastructureLogger)
	{
		ChangeDetector = changeDetector;
		SetSource = setSource;
		EntityGraphAttacher = entityGraphAttacher;
		Model = model;
		QueryProvider = queryProvider;
		StateManager = stateManager;
		UpdateLogger = updateLogger;
		InfrastructureLogger = infrastructureLogger;
		EntityFinderFactory = new EntityFinderFactory(entityFinderSource, stateManager, setSource, currentContext.Context);
	}
}
