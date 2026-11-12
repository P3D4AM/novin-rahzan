using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Query;

public sealed class CompiledQueryCacheKeyGeneratorDependencies
{
	private readonly IExecutionStrategyFactory _executionStrategyFactory;

	public IModel Model { get; }

	public ICurrentDbContext CurrentContext { get; }

	public bool IsRetryingExecutionStrategy { get; }

	[EntityFrameworkInternal]
	public CompiledQueryCacheKeyGeneratorDependencies([JetBrains.Annotations.NotNull] IModel model, [JetBrains.Annotations.NotNull] ICurrentDbContext currentContext, [JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(model, "model");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(currentContext, "currentContext");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(executionStrategyFactory, "executionStrategyFactory");
		Model = model;
		CurrentContext = currentContext;
		_executionStrategyFactory = executionStrategyFactory;
		IsRetryingExecutionStrategy = executionStrategyFactory.Create().RetriesOnFailure;
	}

	public CompiledQueryCacheKeyGeneratorDependencies With([JetBrains.Annotations.NotNull] IModel model)
	{
		return new CompiledQueryCacheKeyGeneratorDependencies(model, CurrentContext, _executionStrategyFactory);
	}

	public CompiledQueryCacheKeyGeneratorDependencies With([JetBrains.Annotations.NotNull] ICurrentDbContext currentContext)
	{
		return new CompiledQueryCacheKeyGeneratorDependencies(Model, currentContext, _executionStrategyFactory);
	}

	public CompiledQueryCacheKeyGeneratorDependencies With([JetBrains.Annotations.NotNull] IExecutionStrategyFactory executionStrategyFactory)
	{
		return new CompiledQueryCacheKeyGeneratorDependencies(Model, CurrentContext, executionStrategyFactory);
	}
}
