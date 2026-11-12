using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IModelCustomizer
{
	void Customize([JetBrains.Annotations.NotNull] ModelBuilder modelBuilder, [JetBrains.Annotations.NotNull] DbContext context);
}
