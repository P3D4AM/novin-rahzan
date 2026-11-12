using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore.Storage;

public sealed class RelationalDatabaseDependencies
{
	public ICommandBatchPreparer BatchPreparer { get; }

	public IBatchExecutor BatchExecutor { get; }

	public IRelationalConnection Connection { get; }

	[EntityFrameworkInternal]
	public RelationalDatabaseDependencies([JetBrains.Annotations.NotNull] ICommandBatchPreparer batchPreparer, [JetBrains.Annotations.NotNull] IBatchExecutor batchExecutor, [JetBrains.Annotations.NotNull] IRelationalConnection connection)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(batchPreparer, "batchPreparer");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(batchExecutor, "batchExecutor");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		BatchPreparer = batchPreparer;
		BatchExecutor = batchExecutor;
		Connection = connection;
	}

	public RelationalDatabaseDependencies With([JetBrains.Annotations.NotNull] ICommandBatchPreparer batchPreparer)
	{
		return new RelationalDatabaseDependencies(batchPreparer, BatchExecutor, Connection);
	}

	public RelationalDatabaseDependencies With([JetBrains.Annotations.NotNull] IBatchExecutor batchExecutor)
	{
		return new RelationalDatabaseDependencies(BatchPreparer, batchExecutor, Connection);
	}

	public RelationalDatabaseDependencies With([JetBrains.Annotations.NotNull] IRelationalConnection connection)
	{
		return new RelationalDatabaseDependencies(BatchPreparer, BatchExecutor, connection);
	}
}
