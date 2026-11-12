using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public abstract class QueryContext : IParameterValues
{
	private readonly IDictionary<string, object> _parameterValues = new Dictionary<string, object>();

	public virtual DbContext Context => Dependencies.CurrentContext.Context;

	protected virtual QueryContextDependencies Dependencies { get; }

	[EntityFrameworkInternal]
	public virtual IStateManager StateManager => Dependencies.StateManager;

	public virtual IQueryProvider QueryProvider => Dependencies.QueryProvider;

	public virtual IExecutionStrategyFactory ExecutionStrategyFactory => Dependencies.ExecutionStrategyFactory;

	public virtual IConcurrencyDetector ConcurrencyDetector => Dependencies.ConcurrencyDetector;

	public virtual CancellationToken CancellationToken { get; set; }

	public virtual IDiagnosticsLogger<DbLoggerCategory.Database.Command> CommandLogger => Dependencies.CommandLogger;

	public virtual IDiagnosticsLogger<DbLoggerCategory.Query> QueryLogger => Dependencies.QueryLogger;

	public virtual IReadOnlyDictionary<string, object> ParameterValues => (IReadOnlyDictionary<string, object>)_parameterValues;

	protected QueryContext([JetBrains.Annotations.NotNull] QueryContextDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual void SetNavigationIsLoaded([JetBrains.Annotations.NotNull] object entity, [JetBrains.Annotations.NotNull] INavigation navigation)
	{
		Dependencies.StateManager.TryGetEntry(entity).SetIsLoaded(navigation);
	}

	public virtual void AddParameter(string name, object value)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(name, "name");
		_parameterValues.Add(name, value);
	}

	[EntityFrameworkInternal]
	public virtual InternalEntityEntry StartTracking(IEntityType entityType, object entity, ValueBuffer valueBuffer)
	{
		return StateManager.StartTrackingFromQuery(entityType, entity, in valueBuffer);
	}
}
