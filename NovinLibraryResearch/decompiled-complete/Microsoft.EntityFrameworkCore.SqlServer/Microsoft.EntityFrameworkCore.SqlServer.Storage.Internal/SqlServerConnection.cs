using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

public class SqlServerConnection : RelationalConnection, ISqlServerConnection, IRelationalConnection, IRelationalTransactionManager, IDbContextTransactionManager, IResettableService, IDisposable, IAsyncDisposable
{
	private bool? _multipleActiveResultSetsEnabled;

	private const int DefaultMasterConnectionCommandTimeout = 60;

	public override bool IsMultipleActiveResultSetsEnabled => (_multipleActiveResultSetsEnabled ?? (_multipleActiveResultSetsEnabled = new SqlConnectionStringBuilder(ConnectionString).MultipleActiveResultSets)).Value;

	protected override bool SupportsAmbientTransactions => true;

	public SqlServerConnection([JetBrains.Annotations.NotNull] RelationalConnectionDependencies dependencies)
		: base(dependencies)
	{
	}

	protected override DbConnection CreateDbConnection()
	{
		return new SqlConnection(ConnectionString);
	}

	public virtual ISqlServerConnection CreateMasterConnection()
	{
		SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString)
		{
			InitialCatalog = "master"
		};
		sqlConnectionStringBuilder.Remove("AttachDBFilename");
		DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(sqlConnectionStringBuilder.ConnectionString, delegate(SqlServerDbContextOptionsBuilder b)
		{
			b.CommandTimeout(CommandTimeout ?? 60);
		}).Options;
		return new SqlServerConnection(Dependencies.With(options));
	}
}
