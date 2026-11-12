namespace Microsoft.EntityFrameworkCore.Storage;

public interface IExecutionStrategyFactory
{
	IExecutionStrategy Create();
}
