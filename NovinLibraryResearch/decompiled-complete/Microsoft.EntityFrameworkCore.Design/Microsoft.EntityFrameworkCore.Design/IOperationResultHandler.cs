using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

public interface IOperationResultHandler
{
	int Version { get; }

	void OnResult([JetBrains.Annotations.CanBeNull] object value);

	void OnError([JetBrains.Annotations.NotNull] string type, [JetBrains.Annotations.NotNull] string message, [JetBrains.Annotations.NotNull] string stackTrace);
}
