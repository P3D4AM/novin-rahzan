using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage;

public class ExecutionResult<TResult>
{
	public virtual bool IsSuccessful { get; }

	public virtual TResult Result { get; }

	public ExecutionResult(bool successful, [JetBrains.Annotations.CanBeNull] TResult result)
	{
		IsSuccessful = successful;
		Result = result;
	}
}
