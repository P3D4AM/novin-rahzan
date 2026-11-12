using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IIndexUniquenessChangedConvention : IConvention
{
	void ProcessIndexUniquenessChanged([JetBrains.Annotations.NotNull] IConventionIndexBuilder indexBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionIndexBuilder> context);
}
