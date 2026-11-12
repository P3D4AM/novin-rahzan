using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IPropertyAddedConvention : IConvention
{
	void ProcessPropertyAdded([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionPropertyBuilder> context);
}
