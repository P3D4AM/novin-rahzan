using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public class ExecutionStrategyFactory : IExecutionStrategyFactory
{
	protected virtual ExecutionStrategyDependencies Dependencies { get; }

	public ExecutionStrategyFactory([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Dependencies = dependencies;
	}

	public virtual IExecutionStrategy Create()
	{
		return new NoopExecutionStrategy(Dependencies);
	}
}
