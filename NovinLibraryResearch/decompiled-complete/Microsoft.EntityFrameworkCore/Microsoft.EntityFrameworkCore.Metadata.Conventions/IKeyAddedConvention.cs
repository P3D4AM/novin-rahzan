using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IKeyAddedConvention : IConvention
{
	void ProcessKeyAdded([JetBrains.Annotations.NotNull] IConventionKeyBuilder keyBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionKeyBuilder> context);
}
