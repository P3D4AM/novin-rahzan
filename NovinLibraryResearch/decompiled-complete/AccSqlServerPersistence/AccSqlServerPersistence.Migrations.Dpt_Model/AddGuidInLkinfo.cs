using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Dpt_Model;

[DbContext(typeof(Dpt_ModelContext))]
[Migration("20250420070250_AddGuidInLkinfo")]
public class AddGuidInLkinfo : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<Guid>("Guid", "Dpt_LkInfo", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, "NewID()", (string)null, (bool?)null, (string)null);
		migrationBuilder.CreateIndex("IX_Dpt_LkInfo_Guid", "Dpt_LkInfo", "Guid", "Dbo", true, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex("IX_Dpt_LkInfo_Guid", "Dpt_LkInfo", "Dbo");
		migrationBuilder.DropColumn("Guid", "Dpt_LkInfo", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.22").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Department.LkInfo", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("LKSerial").ValueGeneratedOnAdd(), "bigint").HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AccountingPackage"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("BarCode"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("CallerID"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeActivateDays"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DisableActivationCodeDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpireDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("FailedDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FailedDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<Guid>(RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier"), "NewID()");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HozoorType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Hozor_Hoghogh"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsOffline"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("MaxActivationDays"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Net_Accounting"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Net_Hozor"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Net_Scales"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Scale"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ScaleCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SpecialAccountingPackage"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SpecialFacilities"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("StartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Tolid"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TypeAccounting"), "smallint");
			b.HasKey(new string[1] { "LKSerial" });
			b.HasIndex(new string[1] { "Guid" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Dpt_LkInfo");
		});
	}
}
