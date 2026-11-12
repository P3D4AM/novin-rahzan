using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Storage.Internal;

public interface INamedConnectionStringResolver
{
	string ResolveConnectionString([JetBrains.Annotations.NotNull] string connectionString);
}
