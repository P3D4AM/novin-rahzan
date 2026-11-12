using System.Data.Common;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Microsoft.EntityFrameworkCore.Scaffolding;

public abstract class DatabaseModelFactory : IDatabaseModelFactory
{
	public abstract DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options);

	public abstract DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options);
}
