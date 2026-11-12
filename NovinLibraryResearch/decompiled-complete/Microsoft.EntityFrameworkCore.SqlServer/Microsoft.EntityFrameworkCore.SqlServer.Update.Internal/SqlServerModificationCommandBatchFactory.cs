using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;

public class SqlServerModificationCommandBatchFactory : IModificationCommandBatchFactory
{
	private readonly ModificationCommandBatchFactoryDependencies _dependencies;

	private readonly IDbContextOptions _options;

	public SqlServerModificationCommandBatchFactory([JetBrains.Annotations.NotNull] ModificationCommandBatchFactoryDependencies dependencies, [JetBrains.Annotations.NotNull] IDbContextOptions options)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(dependencies, "dependencies");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(options, "options");
		_dependencies = dependencies;
		_options = options;
	}

	public virtual ModificationCommandBatch Create()
	{
		SqlServerOptionsExtension sqlServerOptionsExtension = _options.Extensions.OfType<SqlServerOptionsExtension>().FirstOrDefault();
		return new SqlServerModificationCommandBatch(_dependencies, sqlServerOptionsExtension?.MaxBatchSize);
	}
}
