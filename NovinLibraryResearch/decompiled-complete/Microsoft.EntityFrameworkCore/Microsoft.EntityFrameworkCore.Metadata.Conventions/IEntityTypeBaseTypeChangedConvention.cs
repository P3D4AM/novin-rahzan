using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface IEntityTypeBaseTypeChangedConvention : IConvention
{
	void ProcessEntityTypeBaseTypeChanged([JetBrains.Annotations.NotNull] IConventionEntityTypeBuilder entityTypeBuilder, [JetBrains.Annotations.CanBeNull] IConventionEntityType newBaseType, [JetBrains.Annotations.CanBeNull] IConventionEntityType oldBaseType, [JetBrains.Annotations.NotNull] IConventionContext<IConventionEntityType> context);
}
