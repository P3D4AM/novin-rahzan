using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public class RelationalExecutionStrategyFactory : IExecutionStrategyFactory
{
	private readonly Func<ExecutionStrategyDependencies, IExecutionStrategy> _createExecutionStrategy;

	protected virtual ExecutionStrategyDependencies Dependencies { get; }

	public RelationalExecutionStrategyFactory([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
		_createExecutionStrategy = ((dependencies.Options == null) ? null : RelationalOptionsExtension.Extract(dependencies.Options)?.ExecutionStrategyFactory) ?? new Func<ExecutionStrategyDependencies, IExecutionStrategy>(CreateDefaultStrategy);
	}

	protected virtual IExecutionStrategy CreateDefaultStrategy([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
	{
		return new NoopExecutionStrategy(Dependencies);
	}

	public virtual IExecutionStrategy Create()
	{
		return _createExecutionStrategy(Dependencies);
	}
}
