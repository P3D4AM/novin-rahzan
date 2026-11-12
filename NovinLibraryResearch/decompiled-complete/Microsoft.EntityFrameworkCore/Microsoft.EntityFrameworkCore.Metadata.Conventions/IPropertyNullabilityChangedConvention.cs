using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IPropertyNullabilityChangedConvention : IConvention
{
	void ProcessPropertyNullabilityChanged([JetBrains.Annotations.NotNull] IConventionPropertyBuilder propertyBuilder, [JetBrains.Annotations.NotNull] IConventionContext<IConventionPropertyBuilder> context);
}
