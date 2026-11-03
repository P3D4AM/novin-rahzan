using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace AccSqlServerPersistence.Migrations.Dpt_Model;

[DbContext(typeof(Dpt_ModelContext))]
[Migration("20220426131437_FirstMigration")]
public class FirstMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema("Dbo");
		migrationBuilder.CreateTable("Dpt_LkInfo", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AccountingPackage = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TypeAccounting = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HozoorType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			LKSerial = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ExpireDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			StartDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tolid = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BarCode = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Scale = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SpecialAccountingPackage = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Hozor_Hoghogh = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ScaleCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MaxActivationDays = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Net_Accounting = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Net_Hozor = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Net_Scales = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CallerID = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TamdidPrice_Total_1year_Net = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FailedDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FailedDescription = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Dpt_LkInfo", x => (object)x.Guid);
		}, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("Dpt_LkInfo", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.22").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Department.LkInfo", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AccountingPackage"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("BarCode"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("CallerID"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpireDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("FailedDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FailedDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HozoorType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Hozor_Hoghogh"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("LKSerial"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("MaxActivationDays"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Net_Accounting"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Net_Hozor"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Net_Scales"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Scale"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ScaleCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SpecialAccountingPackage"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("StartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TamdidPrice_Total_1year_Net"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Tolid"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TypeAccounting"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Dpt_LkInfo");
		});
	}
}
