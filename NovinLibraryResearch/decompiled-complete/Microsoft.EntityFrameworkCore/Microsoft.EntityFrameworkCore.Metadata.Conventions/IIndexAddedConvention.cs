using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IIndexAddedConvention : IConvention
{
	void ProcessIndexAdded([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionIndexBuilder> context);
}
