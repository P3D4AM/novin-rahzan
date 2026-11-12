using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Infrastructure;

public interface ISingletonOptions
{
	void Initialize([JetBrains.Annotations.NotNull] IDbContextOptions options);

	void Validate([JetBrains.Annotations.NotNull] IDbContextOptions options);
}
