using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

public static class SqlServerDbContextOptionsExtensions
{
	public static DbContextOptionsBuilder UseSqlServer([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder optionsBuilder, [JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsBuilder, "optionsBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotEmpty(connectionString, "connectionString");
		SqlServerOptionsExtension extension = (SqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
		((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
		ConfigureWarnings(optionsBuilder);
		sqlServerOptionsAction?.Invoke(new SqlServerDbContextOptionsBuilder(optionsBuilder));
		return optionsBuilder;
	}

	public static DbContextOptionsBuilder UseSqlServer([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder optionsBuilder, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null)
	{
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(optionsBuilder, "optionsBuilder");
		Microsoft.EntityFrameworkCore.Utilities.Check.NotNull(connection, "connection");
		SqlServerOptionsExtension extension = (SqlServerOptionsExtension)GetOrCreateExtension(optionsBuilder).WithConnection(connection);
		((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);
		ConfigureWarnings(optionsBuilder);
		sqlServerOptionsAction?.Invoke(new SqlServerDbContextOptionsBuilder(optionsBuilder));
		return optionsBuilder;
	}

	public static DbContextOptionsBuilder<TContext> UseSqlServer<TContext>([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [JetBrains.Annotations.NotNull] string connectionString, [JetBrains.Annotations.CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null) where TContext : DbContext
	{
		return (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)optionsBuilder).UseSqlServer(connectionString, sqlServerOptionsAction);
	}

	public static DbContextOptionsBuilder<TContext> UseSqlServer<TContext>([JetBrains.Annotations.NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [JetBrains.Annotations.NotNull] DbConnection connection, [JetBrains.Annotations.CanBeNull] Action<SqlServerDbContextOptionsBuilder> sqlServerOptionsAction = null) where TContext : DbContext
	{
		return (DbContextOptionsBuilder<TContext>)((DbContextOptionsBuilder)optionsBuilder).UseSqlServer(connection, sqlServerOptionsAction);
	}

	private static SqlServerOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
	{
		return optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>() ?? new SqlServerOptionsExtension();
	}

	private static void ConfigureWarnings(DbContextOptionsBuilder optionsBuilder)
	{
		CoreOptionsExtension coreOptionsExtension = optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();
		coreOptionsExtension = coreOptionsExtension.WithWarningsConfiguration(coreOptionsExtension.WarningsConfiguration.TryWithExplicit(RelationalEventId.AmbientTransactionWarning, WarningBehavior.Throw));
		((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(coreOptionsExtension);
	}
}
