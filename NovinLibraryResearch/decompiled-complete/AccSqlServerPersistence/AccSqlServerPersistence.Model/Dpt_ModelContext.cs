using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AccSqlServerPersistence.Entities.Department;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Model;

public class Dpt_ModelContext : BaseModelContext
{
	public DbSet<LkInfo> LkInfo { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		SqlServerDbContextOptionsExtensions.UseSqlServer(options, base.ConnectionString, (Action<SqlServerDbContextOptionsBuilder>)delegate(SqlServerDbContextOptionsBuilder op)
		{
			((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).CommandTimeout((int?)150);
			((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).MigrationsHistoryTable("__Migrations_Dpt", "dbo");
		}).UseQueryTrackingBehavior((QueryTrackingBehavior)1);
		options.ConfigureWarnings((Action<WarningsConfigurationBuilder>)delegate(WarningsConfigurationBuilder warnings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			warnings.Throw((EventId[])(object)new EventId[1] { RelationalEventId.BoolWithDefaultWarning });
		});
	}

	public Dpt_ModelContext(string connectionString, string tranName, string debugerMessage, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		: base(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		try
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<LkInfo>().HasIndex((Expression<Func<LkInfo, object>>)((LkInfo q) => q.Guid)).IsUnique(true);
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<Guid>(modelBuilder.Entity<LkInfo>().Property<Guid>((Expression<Func<LkInfo, Guid>>)((LkInfo p) => p.Guid)), "NewID()");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreating", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\Dpt_ModelContext.cs", 53);
		}
	}
}
