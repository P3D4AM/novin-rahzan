using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage;

public interface IDatabaseProvider
{
	string Name { get; }

	bool IsConfigured([JetBrains.Annotations.NotNull] IDbContextOptions options);
}
