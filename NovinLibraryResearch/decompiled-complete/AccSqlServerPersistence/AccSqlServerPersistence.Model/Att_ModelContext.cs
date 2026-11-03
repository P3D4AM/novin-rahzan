using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using PacketParser;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Entities.Attendance;
using SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

namespace AccSqlServerPersistence.Model;

public class Att_ModelContext : BaseModelContext
{
	private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create((Action<ILoggingBuilder>)delegate
	{
	});

	public virtual DbSet<AttPersonLeave> AttPersonLeaves { get; set; }

	public virtual DbSet<AttDepartment> AttDepartments { get; set; }

	public virtual DbSet<BiometricDetail> BioDatas { get; set; }

	public virtual DbSet<Biometric> Biometrics { get; set; }

	public virtual DbSet<BioDeviceRelation> BioDeviceRelations { get; set; }

	public virtual DbSet<BioWriteDevice> BioWriteDevices { get; set; }

	public virtual DbSet<BioWriteDeviceLog> BioWriteDeviceLogs { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		SqlServerDbContextOptionsExtensions.UseSqlServer(options, base.ConnectionString, (Action<SqlServerDbContextOptionsBuilder>)delegate(SqlServerDbContextOptionsBuilder op)
		{
			((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).CommandTimeout((int?)150);
			((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).MigrationsHistoryTable("__Migrations_Att", "dbo");
		}).UseQueryTrackingBehavior((QueryTrackingBehavior)1);
		options.ConfigureWarnings((Action<WarningsConfigurationBuilder>)delegate(WarningsConfigurationBuilder warnings)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			warnings.Throw((EventId[])(object)new EventId[1] { RelationalEventId.BoolWithDefaultWarning });
		});
	}

	public Att_ModelContext(string connectionString, string tranName, string debugerMessage, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		: base(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		try
		{
			base.OnModelCreating(modelBuilder);
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Biometric>().HasMany<BiometricDetail>((Expression<Func<Biometric, IEnumerable<BiometricDetail>>>)((Biometric x) => x.BiometricDetails)).WithOne((Expression<Func<BiometricDetail, Biometric>>)((BiometricDetail x) => x.BioItem))
				.HasForeignKey((Expression<Func<BiometricDetail, object>>)((BiometricDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Biometric>().HasMany<BioDeviceRelation>((Expression<Func<Biometric, IEnumerable<BioDeviceRelation>>>)((Biometric x) => x.BioDeviceRelations)).WithOne((Expression<Func<BioDeviceRelation, Biometric>>)((BioDeviceRelation x) => x.BioItem))
				.HasForeignKey((Expression<Func<BioDeviceRelation, object>>)((BioDeviceRelation x) => x.BioGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Biometric>().HasMany<BioWriteDevice>((Expression<Func<Biometric, IEnumerable<BioWriteDevice>>>)((Biometric x) => x.BioWriteDevices)).WithOne((Expression<Func<BioWriteDevice, Biometric>>)((BioWriteDevice x) => x.BioItem))
				.HasForeignKey((Expression<Func<BioWriteDevice, object>>)((BioWriteDevice x) => x.BioGuid))).OnDeleteNoAction();
			modelBuilder.Entity<BioDeviceRelation>().HasKey((Expression<Func<BioDeviceRelation, object>>)((BioDeviceRelation k) => new { k.BioGuid, k.DeviceGuid }));
			modelBuilder.Entity<BioWriteDevice>().HasKey((Expression<Func<BioWriteDevice, object>>)((BioWriteDevice k) => new { k.BioGuid, k.DeviceGuid, k.HardwareCode }));
			RelationalIndexBuilderExtensions.HasFilter<Biometric>(modelBuilder.Entity<Biometric>().HasIndex((Expression<Func<Biometric, object>>)((Biometric k) => new { k.DeviceGroup, k.PersonGuid })), "[PersonGuid] IS NOT NULL");
			modelBuilder.Entity<BioDeviceRelation>().HasIndex((Expression<Func<BioDeviceRelation, object>>)((BioDeviceRelation k) => new { k.DeviceGuid, k.HardwareCode })).IsUnique(true);
			modelBuilder.Entity<BioWriteDevice>().HasIndex((Expression<Func<BioWriteDevice, object>>)((BioWriteDevice k) => new { k.DeviceGuid, k.HardwareCode })).IsUnique(true);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreating", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\Att_ModelContext.cs", 71);
		}
	}
}
