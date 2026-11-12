using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public interface IDatabaseModelFactory
{
	DatabaseModel Create([JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.NotNull] DatabaseModelFactoryOptions options);

	DatabaseModel Create([JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.NotNull] DatabaseModelFactoryOptions options);
}
