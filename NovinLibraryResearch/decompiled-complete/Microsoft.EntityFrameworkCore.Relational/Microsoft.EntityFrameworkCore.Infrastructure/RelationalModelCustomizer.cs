using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public class RelationalModelCustomizer : ModelCustomizer
{
	public RelationalModelCustomizer([JetBrains.Annotations.NotNull] ModelCustomizerDependencies dependencies)
		: base(dependencies)
	{
	}
}
