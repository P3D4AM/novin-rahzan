using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Metadata.Internal;

public class InternalNavigationBuilder : InternalModelItemBuilder<Navigation>
{
	public InternalNavigationBuilder([JetBrains.Annotations.NotNull] Navigation metadata, [JetBrains.Annotations.NotNull] InternalModelBuilder modelBuilder)
		: base(metadata, modelBuilder)
	{
	}
}
