using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IConventionContext
{
	void StopProcessing();

	IConventionBatch DelayConventions();
}
public interface IConventionContext<in TMetadata> : IConventionContext
{
	void StopProcessing([JetBrains.Annotations.NotNull] TMetadata result);

	void StopProcessingIfChanged([JetBrains.Annotations.NotNull] TMetadata result);
}
