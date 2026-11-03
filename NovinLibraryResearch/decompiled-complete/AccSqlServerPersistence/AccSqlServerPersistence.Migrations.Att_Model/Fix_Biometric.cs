using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20250804082145_Fix_Biometric")]
public class Fix_Biometric : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey("FK_BioWriteDeviceLogs_BioDatas_BioGuid", "BioWriteDeviceLogs", "Dbo");
		migrationBuilder.DropTable("BioDeviceRels", "Dbo");
		migrationBuilder.DropTable("BioWriteDevices", "Dbo");
		migrationBuilder.DropTable("BioDatas", "Dbo");
		migrationBuilder.DropIndex("IX_BioWriteDeviceLogs_BioGuid", "BioWriteDeviceLogs", "Dbo");
		migrationBuilder.CreateTable("Att_Biometric", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeviceGroup = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PersonGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FingerPrintCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HasPalm = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HasFace = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HasCard = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HasPassword = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Att_Biometric", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Att_BioDeviceRelation", (ColumnsBuilder table) => new
		{
			BioGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeviceGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HardwareCode = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Privilage = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FromDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ToDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Att_BioDeviceRelation", x => (object)new { x.BioGuid, x.DeviceGuid });
			table.ForeignKey("FK_Att_BioDeviceRelation_Att_Biometric_BioGuid", x => (object)x.BioGuid, "Att_Biometric", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Att_BiometricDetail", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EnrolType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ordinal = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Data = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Att_BiometricDetail", x => (object)x.Guid);
			table.ForeignKey("FK_Att_BiometricDetail_Att_Biometric_MasterGuid", x => (object)x.MasterGuid, "Att_Biometric", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Att_BioWriteDevice", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> bioGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> deviceGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> hardwareCode = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num = 250;
			return new
			{
				BioGuid = bioGuid,
				DeviceGuid = deviceGuid,
				HardwareCode = hardwareCode,
				HarwareName = table.Column<string>((string)null, (bool?)null, num, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				InsertedDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				TryCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				NextTry = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Att_BioWriteDevice", x => (object)new { x.BioGuid, x.DeviceGuid });
			table.ForeignKey("FK_Att_BioWriteDevice_Att_Biometric_BioGuid", x => (object)x.BioGuid, "Att_Biometric", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateIndex("IX_Att_Biometric_DeviceGroup_PersonGuid", "Att_Biometric", new string[2] { "DeviceGroup", "PersonGuid" }, "Dbo", false, "[PersonGuid] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_Att_BiometricDetail_MasterGuid", "Att_BiometricDetail", "MasterGuid", "Dbo", false, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("Att_BioDeviceRelation", "Dbo");
		migrationBuilder.DropTable("Att_BiometricDetail", "Dbo");
		migrationBuilder.DropTable("Att_BioWriteDevice", "Dbo");
		migrationBuilder.DropTable("Att_Biometric", "Dbo");
		migrationBuilder.CreateTable("BioDatas", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Data = table.Column<string>("nvarchar(max)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EnrolType = table.Column<short>("smallint", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ordinal = table.Column<int>("int", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PersonGuid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioDatas", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("BioWriteDevices", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> deviceGuid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> hardwareCode = table.Column<int>("int", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> bioInfo = table.Column<string>("nvarchar(max)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> guid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num = 200;
			return new
			{
				DeviceGuid = deviceGuid,
				HardwareCode = hardwareCode,
				BioInfo = bioInfo,
				Guid = guid,
				HarwareName = table.Column<string>("nvarchar(200)", (bool?)null, num, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				InsertedDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				NextTry = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				TryCount = table.Column<short>("smallint", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				UserGuid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioWriteDevices", x => (object)new { x.DeviceGuid, x.HardwareCode });
		}, (string)null);
		migrationBuilder.CreateTable("BioDeviceRels", (ColumnsBuilder table) => new
		{
			BioGuid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeviceGuid = table.Column<Guid>("uniqueidentifier", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FromDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HardwareCode = table.Column<int>("int", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Privilage = table.Column<short>("smallint", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>("bit", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ToDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioDeviceRels", x => (object)new { x.BioGuid, x.DeviceGuid });
			table.ForeignKey("FK_BioDeviceRels_BioDatas_BioGuid", x => (object)x.BioGuid, "BioDatas", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateIndex("IX_BioWriteDeviceLogs_BioGuid", "BioWriteDeviceLogs", "BioGuid", "Dbo", false, (string)null);
		migrationBuilder.AddForeignKey("FK_BioWriteDeviceLogs_BioDatas_BioGuid", "BioWriteDeviceLogs", "BioGuid", "BioDatas", "Dbo", "Dbo", "Guid", (ReferentialAction)0, (ReferentialAction)0);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.22").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.AttDepartment", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AdminGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreatedDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_Department");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.AttPersonLeave", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateFrom"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateTo"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Kind"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LeaveStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("ManagerDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ManagerMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PersonGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Quiddity"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reason"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("RequestDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_PersonLeave");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BioDeviceRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BioGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DeviceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("FromDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("HardwareCode"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Privilage"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("ToDateM"), "datetime");
			b.HasKey(new string[2] { "BioGuid", "DeviceGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_BioDeviceRelation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BioWriteDevice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BioGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DeviceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("HardwareCode"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HarwareName"), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("InsertedDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("NextTry"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TryCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "BioGuid", "DeviceGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_BioWriteDevice");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BioWriteDeviceLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BioGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DeviceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("HardwareCode"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HardwareName"), "nvarchar(255)").HasMaxLength(255);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("RequestDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Result"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "BioWriteDeviceLogs");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.Biometric", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DeviceGroup"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("FingerPrintCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("HasCard"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("HasFace"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("HasPalm"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("HasPassword"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PersonGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[2] { "DeviceGroup", "PersonGuid" }), "[PersonGuid] IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_Biometric");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BiometricDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Data").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnrolType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_BiometricDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BioDeviceRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.Biometric", "BioItem").WithMany("BioDeviceRelations").HasForeignKey(new string[1] { "BioGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BioWriteDevice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.Biometric", "BioItem").WithMany("BioWriteDevices").HasForeignKey(new string[1] { "BioGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.BiometricDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric.Biometric", "BioItem").WithMany("BiometricDetails").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
	}
}
