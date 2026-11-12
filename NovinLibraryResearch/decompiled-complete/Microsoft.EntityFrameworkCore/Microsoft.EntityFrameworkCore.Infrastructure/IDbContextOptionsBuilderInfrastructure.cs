using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface IDbContextOptionsBuilderInfrastructure
{
	void AddOrUpdateExtension<TExtension>([JetBrains.Annotations.NotNull] TExtension extension) where TExtension : class, IDbContextOptionsExtension;
}
