using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbContextPool
{
	DbContext Rent();

	bool Return([JetBrains.Annotations.NotNull] DbContext context);
}
