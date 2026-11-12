using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Internal;

public interface IDbContextPoolable : IResettableService
{
	void SetPool([JetBrains.Annotations.CanBeNull] IDbContextPool contextPool);

	DbContextPoolConfigurationSnapshot SnapshotConfiguration();

	void Resurrect([JetBrains.Annotations.NotNull] DbContextPoolConfigurationSnapshot configurationSnapshot);
}
