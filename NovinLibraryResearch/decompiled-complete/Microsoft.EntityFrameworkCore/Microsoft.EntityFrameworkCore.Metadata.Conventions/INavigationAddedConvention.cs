using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions;

public interface INavigationAddedConvention : IConvention
{
	void ProcessNavigationAdded([JetBrains.Annotations.NotNull] IConventionRelationshipBuilder relationshipBuilder, [JetBrains.Annotations.NotNull] IConventionNavigation navigation, [JetBrains.Annotations.NotNull] IConventionContext<IConventionNavigation> context);
}
