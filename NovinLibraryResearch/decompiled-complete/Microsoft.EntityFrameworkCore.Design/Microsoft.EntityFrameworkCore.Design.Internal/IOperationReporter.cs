using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Design.Internal;

public interface IOperationReporter
{
	void WriteError([JetBrains.Annotations.NotNull] string message);

	void WriteWarning([JetBrains.Annotations.NotNull] string message);

	void WriteInformation([JetBrains.Annotations.NotNull] string message);

	void WriteVerbose([JetBrains.Annotations.NotNull] string message);
}
