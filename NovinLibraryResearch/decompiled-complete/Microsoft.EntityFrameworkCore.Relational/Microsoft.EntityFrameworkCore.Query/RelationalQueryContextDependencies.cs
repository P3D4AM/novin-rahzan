using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class RelationalQueryContextDependencies
{
	public IRelationalConnection RelationalConnection { get; }

	[Obsolete("Moved to QueryContextDependencies")]
	public IExecutionStrategyFactory ExecutionStrategyFactory { get; }

	[EntityFrameworkInternal]
	public RelationalQueryContextDependencies([JetBrains.Annotations.NotNull] IRelationalConnection relationalConnection, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(relationalConnection, "relationalConnection");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executionStrategyFactory, "executionStrategyFactory");
		RelationalConnection = relationalConnection;
		ExecutionStrategyFactory = executionStrategyFactory;
	}

	public RelationalQueryContextDependencies With([JetBrains.Annotations.NotNull] IRelationalConnection relationalConnection)
	{
		return new RelationalQueryContextDependencies(relationalConnection, ExecutionStrategyFactory);
	}

	public RelationalQueryContextDependencies With([JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		return new RelationalQueryContextDependencies(RelationalConnection, executionStrategyFactory);
	}
}
