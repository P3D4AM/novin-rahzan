using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerExecutionStrategyFactory : RelationalExecutionStrategyFactory
{
	public SqlServerExecutionStrategyFactory([JetBrains.Annotations.NotNull] ExecutionStrategyDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override IExecutionStrategy CreateDefaultStrategy(ExecutionStrategyDependencies dependencies)
	{
		return new SqlServerExecutionStrategy(dependencies);
	}
}
