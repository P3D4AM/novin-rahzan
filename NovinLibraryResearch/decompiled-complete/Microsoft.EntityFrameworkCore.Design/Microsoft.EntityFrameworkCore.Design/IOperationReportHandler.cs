using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design;

public interface IOperationReportHandler
{
	int Version { get; }

	void OnError([JetBrains.Annotations.NotNull] string message);

	void OnWarning([JetBrains.Annotations.NotNull] string message);

	void OnInformation([JetBrains.Annotations.NotNull] string message);

	void OnVerbose([JetBrains.Annotations.NotNull] string message);
}
