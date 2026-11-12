using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IKeyRemovedConvention : IConvention
{
	void ProcessKeyRemoved([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.NotNull] IConventionKey key, [JetBrains.Annotations.NotNull] IConventionContext<IConventionKey> context);
}
