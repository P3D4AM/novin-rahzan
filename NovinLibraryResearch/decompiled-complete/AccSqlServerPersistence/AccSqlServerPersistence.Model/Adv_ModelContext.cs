using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AccSqlServerPersistence.Entities.Advertise;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using PacketParser;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Model;

public class Adv_ModelContext : BaseModelContext
{
	public DbSet<Owner> Owners { get; set; }

	public DbSet<Visitor> Visitors { get; set; }

	public DbSet<AdvertiseLog> AdvertiseLogs { get; set; }

	public DbSet<SimCard> SimCards { get; set; }

	public DbSet<City> Cities { get; set; }

	public DbSet<State> States { get; set; }

	public DbSet<VisitorCity> VisitorCity { get; set; }

	public DbSet<VisitorAdv> VisitorAdv { get; set; }

	public DbSet<TitleBlackList> TitleBlackLists { get; set; }

	public DbSet<ContentBlackList> ContentBlackLists { get; set; }

	public DbSet<DivarCity> DivarCity { get; set; }

	public DbSet<SheypoorCity> SheypoorCity { get; set; }

	public DbSet<AdvTokens> AdvTokens { get; set; }

	public DbSet<AdvCategory> AdvCategory { get; set; }

	public DbSet<AdvGroup> AdvGroup { get; set; }

	public DbSet<AdvDepartment> AdvDepartment { get; set; }

	public DbSet<DepartmentCategory> DepartmentCategories { get; set; }

	public DbSet<ChatNumbers> ChatNumbers { get; set; }

	public DbSet<Advertise> Advertise { get; set; }

	public DbSet<CategoryGroup> AdvCategoryGroup { get; set; }

	public DbSet<Region> Regions { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		SqlServerDbContextOptionsExtensions.UseSqlServer(options, base.ConnectionString, (Action<SqlServerDbContextOptionsBuilder>)delegate(SqlServerDbContextOptionsBuilder op)
		{
			((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).CommandTimeout((int?)150);
			((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).MigrationsHistoryTable("__Migrations_Adv", "dbo");
		}).UseQueryTrackingBehavior((QueryTrackingBehavior)1);
		options.ConfigureWarnings((Action<WarningsConfigurationBuilder>)delegate(WarningsConfigurationBuilder warnings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			warnings.Throw((EventId[])(object)new EventId[1] { RelationalEventId.BoolWithDefaultWarning });
		});
	}

	public Adv_ModelContext(string connectionString, string tranName, string debugerMessage, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		: base(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		try
		{
			base.OnModelCreating(modelBuilder);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<State>().HasMany<City>((Expression<Func<State, IEnumerable<City>>>)((State x) => x.Cities)).WithOne((Expression<Func<City, State>>)((City x) => x.State))
				.HasForeignKey((Expression<Func<City, object>>)((City x) => x.StateGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<City>().HasMany<VisitorCity>((Expression<Func<City, IEnumerable<VisitorCity>>>)((City x) => x.VisitorCities)).WithOne((Expression<Func<VisitorCity, City>>)((VisitorCity x) => x.City))
				.HasForeignKey((Expression<Func<VisitorCity, object>>)((VisitorCity x) => x.CityGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Visitor>().HasMany<VisitorCity>((Expression<Func<Visitor, IEnumerable<VisitorCity>>>)((Visitor x) => x.VisitorCities)).WithOne((Expression<Func<VisitorCity, Visitor>>)((VisitorCity x) => x.Visitor))
				.HasForeignKey((Expression<Func<VisitorCity, object>>)((VisitorCity x) => x.VisitorGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Visitor>().HasMany<Owner>((Expression<Func<Visitor, IEnumerable<Owner>>>)((Visitor x) => x.Owners)).WithOne((Expression<Func<Owner, Visitor>>)((Owner x) => x.Visitor))
				.HasForeignKey((Expression<Func<Owner, object>>)((Owner x) => x.VisitorGuid))).OnDeleteNoAction();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreating", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\Adv_ModelContext.cs", 56);
		}
	}
}
