using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Global;
using AccSqlServerPersistence.Entities.Tickets;
using AccSqlServerPersistence.Entities.Uploader;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using PacketParser;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Entities.Global;
using SharedAccEntityFramworkPersistence.Entities.Global.Tasks;
using SharedAccEntityFramworkPersistence.Entities.Global.Tickets;

namespace AccSqlServerPersistence.Model;

public class Global_ModelContext : BaseModelContext
{
	public virtual DbSet<LocalSetting> LocalSettings { get; set; }

	public virtual DbSet<ContactUs> ContactUs { get; set; }

	public virtual DbSet<FileInfoReTryDownload> FileInfoReTryDownload { get; set; }

	public virtual DbSet<HyperLink> HyperLink { get; set; }

	public virtual DbSet<LogTable> Log { get; set; }

	public virtual DbSet<News> News { get; set; }

	public virtual DbSet<UserInformation> UserInformation { get; set; }

	public virtual DbSet<UploadFile> UploadFile { get; set; }

	public virtual DbSet<UploadFilePart> UploadFilePart { get; set; }

	public virtual DbSet<Ticket> Ticket { get; set; }

	public virtual DbSet<TicketDet> TicketDet { get; set; }

	public virtual DbSet<TicketPart> TicketPart { get; set; }

	public virtual DbSet<TicketPartDetail> TicketPartDetail { get; set; }

	public virtual DbSet<BackupLog> BackUpLog { get; set; }

	public virtual DbSet<BackupSchedule> BackupSchedules { get; set; }

	public virtual DbSet<BackupSettings> BackupSettings { get; set; }

	public virtual DbSet<BackupTime> BackupTimes { get; set; }

	public virtual DbSet<Desc> Desc { get; set; }

	public virtual DbSet<En2Fa> En2Fa { get; set; }

	public virtual DbSet<Naghz> Naghz { get; set; }

	public virtual DbSet<CheckedSave> CheckedSaves { get; set; }

	public virtual DbSet<FileInfo> FileInfoEntity { get; set; }

	public virtual DbSet<Calender> Calender { get; set; }

	public DbSet<PushNotification> PushNotification { get; set; }

	public DbSet<MetaData> MetaData { get; set; }

	public virtual DbSet<ReportGenerator> ReportGenerator { get; set; }

	public virtual DbSet<ReportGeneratorDet> ReportGeneratorDet { get; set; }

	public virtual DbSet<Menu> Menu { get; set; }

	public virtual DbSet<MenuItem> MenuItems { get; set; }

	public virtual DbSet<Card> Card { get; set; }

	public virtual DbSet<CardTargetRelation> CardTargetRelation { get; set; }

	public virtual DbSet<StimulQuery> StimulQuery { get; set; }

	public virtual DbSet<SaveTemp> SaveTemp { get; set; }

	public virtual DbSet<IntroductionMethod> IntroductionMethod { get; set; }

	public virtual DbSet<Wage> Wage { get; set; }

	public virtual DbSet<Post> Post { get; set; }

	public virtual DbSet<TaskClass> Tasks { get; set; }

	public virtual DbSet<SarNakh> SarNakhs { get; set; }

	public virtual DbSet<SarNakhLocation> SarNakhLocations { get; set; }

	public virtual DbSet<SarNakhNumbers> SarNakhNumbers { get; set; }

	public virtual DbSet<TaskCashierRelation> TaskCashierRelation { get; set; }

	public virtual DbSet<EntityDeliveryStatus> EntityDeliveryStatus { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder options)
	{
		if (!string.IsNullOrEmpty(base.ConnectionString))
		{
			SqlServerDbContextOptionsExtensions.UseSqlServer(options, base.ConnectionString, (Action<SqlServerDbContextOptionsBuilder>)delegate(SqlServerDbContextOptionsBuilder op)
			{
				((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).CommandTimeout((int?)150);
				((RelationalDbContextOptionsBuilder<SqlServerDbContextOptionsBuilder, SqlServerOptionsExtension>)(object)op).MigrationsHistoryTable("__Migrations_Global", "dbo");
			}).UseQueryTrackingBehavior((QueryTrackingBehavior)1);
			options.ConfigureWarnings((Action<WarningsConfigurationBuilder>)delegate(WarningsConfigurationBuilder warnings)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				warnings.Throw((EventId[])(object)new EventId[1] { RelationalEventId.BoolWithDefaultWarning });
			});
		}
	}

	public Global_ModelContext(string connectionString, string tranName, string debugerMessage, int timeout = 60000, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
		: base(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		try
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<StimulQuery>().HasKey((Expression<Func<StimulQuery, object>>)((StimulQuery k) => new { k.RefrencedId, k.Order }));
			modelBuilder.Entity<Desc>().HasIndex((Expression<Func<Desc, object>>)((Desc p) => p.ServerStatus));
			modelBuilder.Entity<ContactUs>().HasIndex((Expression<Func<ContactUs, object>>)((ContactUs p) => p.Type));
			modelBuilder.Entity<MetaData>().HasKey((Expression<Func<MetaData, object>>)((MetaData k) => new { k.EntityGuid, k.EntityType, k.Key }));
			modelBuilder.Entity<FileInfo>().HasIndex((Expression<Func<FileInfo, object>>)((FileInfo p) => p.Modified));
			modelBuilder.Entity<FileInfoReTryDownload>().HasIndex((Expression<Func<FileInfoReTryDownload, object>>)((FileInfoReTryDownload p) => p.Modified));
			modelBuilder.Entity<FileInfoReTryDownload>().HasIndex((Expression<Func<FileInfoReTryDownload, object>>)((FileInfoReTryDownload p) => p.ReTryCount));
			modelBuilder.Entity<UploadFilePart>().HasIndex((Expression<Func<UploadFilePart, object>>)((UploadFilePart p) => p.Guid));
			modelBuilder.Entity<UploadFilePart>().HasIndex((Expression<Func<UploadFilePart, object>>)((UploadFilePart p) => p.FileId));
			modelBuilder.Entity<BackupLog>().HasIndex((Expression<Func<BackupLog, object>>)((BackupLog p) => p.DPId));
			modelBuilder.Entity<BackupLog>().HasIndex((Expression<Func<BackupLog, object>>)((BackupLog p) => p.FileId));
			modelBuilder.Entity<TicketPartDetail>().HasKey((Expression<Func<TicketPartDetail, object>>)((TicketPartDetail q) => new { q.MasterGuid, q.TargetGuid, q.TargetType }));
			modelBuilder.Entity<BackupSchedule>().HasIndex((Expression<Func<BackupSchedule, object>>)((BackupSchedule p) => p.Title)).IsUnique(true);
			modelBuilder.Entity<UploadFile>().HasIndex((Expression<Func<UploadFile, object>>)((UploadFile p) => p.Guid));
			modelBuilder.Entity<Calender>().HasKey((Expression<Func<Calender, object>>)((Calender p) => new { p.Guid, p.DateM })).IsCluster(isCluster: false);
			modelBuilder.Entity<En2Fa>().HasKey((Expression<Func<En2Fa, object>>)((En2Fa p) => new { p.Guid })).IsCluster(isCluster: false);
			((IndexBuilder)(object)modelBuilder.Entity<En2Fa>().HasIndex((Expression<Func<En2Fa, object>>)((En2Fa p) => new { p.LName }))).IsCluster().IsUnique(true);
			modelBuilder.Entity<Naghz>().HasIndex((Expression<Func<Naghz, object>>)((Naghz p) => new { p.Message })).IsUnique(true);
			modelBuilder.Entity<Wage>().HasIndex((Expression<Func<Wage, object>>)((Wage p) => p.ServerGuid)).IsUnique(true);
			modelBuilder.Entity<CardTargetRelation>().HasKey((Expression<Func<CardTargetRelation, object>>)((CardTargetRelation k) => new { k.CardGuid, k.TargetGuid }));
			modelBuilder.Entity<EntityDeliveryStatus>().HasKey((Expression<Func<EntityDeliveryStatus, object>>)((EntityDeliveryStatus k) => new { k.EntityPart, k.IsTemp, k.EntityGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Ticket>().HasMany<TicketDet>((Expression<Func<Ticket, IEnumerable<TicketDet>>>)((Ticket x) => x.TicketDets)).WithOne((Expression<Func<TicketDet, Ticket>>)((TicketDet x) => x.Ticket))
				.HasForeignKey((Expression<Func<TicketDet, object>>)((TicketDet x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Ticket>().HasMany<Ticket>((Expression<Func<Ticket, IEnumerable<Ticket>>>)((Ticket x) => x.Refrences)).WithOne((Expression<Func<Ticket, Ticket>>)((Ticket x) => x.MasterTicket))
				.HasForeignKey((Expression<Func<Ticket, object>>)((Ticket x) => x.ReferenceGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TicketPart>().HasMany<TicketPartDetail>((Expression<Func<TicketPart, IEnumerable<TicketPartDetail>>>)((TicketPart x) => x.TicketPartDetails)).WithOne((Expression<Func<TicketPartDetail, TicketPart>>)((TicketPartDetail x) => x.TicketPart))
				.HasForeignKey((Expression<Func<TicketPartDetail, object>>)((TicketPartDetail x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<IntroductionMethod>().HasMany<SarNakh>((Expression<Func<IntroductionMethod, IEnumerable<SarNakh>>>)((IntroductionMethod x) => x.SarNakhs)).WithOne((Expression<Func<SarNakh, IntroductionMethod>>)((SarNakh x) => x.Introduction))
				.HasForeignKey((Expression<Func<SarNakh, object>>)((SarNakh x) => x.IntroductionGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<SarNakh>().HasMany<SarNakhLocation>((Expression<Func<SarNakh, IEnumerable<SarNakhLocation>>>)((SarNakh x) => x.Locations)).WithOne((Expression<Func<SarNakhLocation, SarNakh>>)((SarNakhLocation x) => x.SarNakh))
				.HasForeignKey((Expression<Func<SarNakhLocation, object>>)((SarNakhLocation x) => x.SarNakhGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<SarNakh>().HasMany<SarNakhNumbers>((Expression<Func<SarNakh, IEnumerable<SarNakhNumbers>>>)((SarNakh x) => x.Numbers)).WithOne((Expression<Func<SarNakhNumbers, SarNakh>>)((SarNakhNumbers x) => x.SarNakh))
				.HasForeignKey((Expression<Func<SarNakhNumbers, object>>)((SarNakhNumbers x) => x.SarNakhGuid))).OnDeleteNoAction();
			modelBuilder.Entity<TaskCashierRelation>().HasKey((Expression<Func<TaskCashierRelation, object>>)((TaskCashierRelation q) => new { q.TaskGuid, q.CashierGuid }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TaskCashierRelation>().HasOne<TaskClass>((Expression<Func<TaskCashierRelation, TaskClass>>)((TaskCashierRelation x) => x.Tasks)).WithMany((Expression<Func<TaskClass, IEnumerable<TaskCashierRelation>>>)((TaskClass x) => x.TaskCashierRelation))
				.HasForeignKey((Expression<Func<TaskCashierRelation, object>>)((TaskCashierRelation x) => x.TaskGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<TaskClass>().HasMany<TaskClass>((Expression<Func<TaskClass, IEnumerable<TaskClass>>>)((TaskClass x) => x.TaskClasses)).WithOne((Expression<Func<TaskClass, TaskClass>>)((TaskClass x) => x.Tasks))
				.HasForeignKey((Expression<Func<TaskClass, object>>)((TaskClass x) => x.MasterGuid))).OnDeleteNoAction();
			modelBuilder.Entity<TaskClass>().HasIndex((Expression<Func<TaskClass, object>>)((TaskClass x) => x.Id2)).IsUnique(true);
			modelBuilder.Entity<SarNakhLocation>().HasKey((Expression<Func<SarNakhLocation, object>>)((SarNakhLocation q) => new { q.SarNakhGuid, q.Ordinal }));
			modelBuilder.Entity<SarNakhNumbers>().HasKey((Expression<Func<SarNakhNumbers, object>>)((SarNakhNumbers q) => new { q.SarNakhGuid, q.Number }));
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<ReportGenerator>().HasMany<ReportGeneratorDet>((Expression<Func<ReportGenerator, IEnumerable<ReportGeneratorDet>>>)((ReportGenerator x) => x.Details)).WithOne((Expression<Func<ReportGeneratorDet, ReportGenerator>>)((ReportGeneratorDet x) => x.Master))
				.HasForeignKey((Expression<Func<ReportGeneratorDet, object>>)((ReportGeneratorDet x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Menu>().HasMany<MenuItem>((Expression<Func<Menu, IEnumerable<MenuItem>>>)((Menu x) => x.DetailItem)).WithOne((Expression<Func<MenuItem, Menu>>)((MenuItem x) => x.MasterItem))
				.HasForeignKey((Expression<Func<MenuItem, object>>)((MenuItem x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Menu>().HasMany<Menu>((Expression<Func<Menu, IEnumerable<Menu>>>)((Menu x) => x.Details)).WithOne((Expression<Func<Menu, Menu>>)((Menu x) => x.Master))
				.HasForeignKey((Expression<Func<Menu, object>>)((Menu x) => x.MasterGuid))).OnDeleteNoAction();
			((ReferenceCollectionBuilder)(object)modelBuilder.Entity<Card>().HasMany<CardTargetRelation>((Expression<Func<Card, IEnumerable<CardTargetRelation>>>)((Card x) => x.DetailItems)).WithOne((Expression<Func<CardTargetRelation, Card>>)((CardTargetRelation x) => x.CardItem))
				.HasForeignKey((Expression<Func<CardTargetRelation, object>>)((CardTargetRelation x) => x.CardGuid))).OnDeleteNoAction();
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(modelBuilder.Entity<Wage>().Property<decimal>((Expression<Func<Wage, decimal>>)((Wage q) => q.PriceOrPercent)), "decimal(18,5)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(modelBuilder.Entity<TicketDet>().Property<DateTime>((Expression<Func<TicketDet, DateTime>>)((TicketDet x) => x.DateM)), "getdate()");
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnModelCreating", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Model\\Global_ModelContext.cs", 151);
		}
	}
}
