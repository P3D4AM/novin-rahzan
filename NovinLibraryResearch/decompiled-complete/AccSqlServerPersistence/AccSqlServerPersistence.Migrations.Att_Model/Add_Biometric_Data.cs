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
[Migration("20250715090854_Add_Biometric_Data")]
public class Add_Biometric_Data : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable("BioDatas", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PersonGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EnrolType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ordinal = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Data = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioDatas", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("BioWriteDevices", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> deviceGuid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> hardwareCode2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> guid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num2 = 200;
			return new
			{
				DeviceGuid = deviceGuid2,
				HardwareCode = hardwareCode2,
				Guid = guid2,
				HarwareName = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BioInfo = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				InsertedDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				TryCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				NextTry = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioWriteDevices", x => (object)new { x.DeviceGuid, x.HardwareCode });
		}, (string)null);
		migrationBuilder.CreateTable("BioDeviceRels", (ColumnsBuilder table) => new
		{
			BioGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeviceGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HardwareCode = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Privilage = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FromDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ToDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioDeviceRels", x => (object)new { x.BioGuid, x.DeviceGuid });
			table.ForeignKey("FK_BioDeviceRels_BioDatas_BioGuid", x => (object)x.BioGuid, "BioDatas", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("BioWriteDeviceLogs", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> dateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num = 500;
			OperationBuilder<AddColumnOperation> description = table.Column<string>((string)null, (bool?)null, num, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> deviceGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> bioGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> hardwareCode = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num = 255;
			return new
			{
				Guid = guid,
				DateM = dateM,
				Description = description,
				DeviceGuid = deviceGuid,
				BioGuid = bioGuid,
				HardwareCode = hardwareCode,
				HardwareName = table.Column<string>((string)null, (bool?)null, num, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				RequestDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Result = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_BioWriteDeviceLogs", x => (object)x.Guid);
			table.ForeignKey("FK_BioWriteDeviceLogs_BioDatas_BioGuid", x => (object)x.BioGuid, "BioDatas", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateIndex("IX_BioWriteDeviceLogs_BioGuid", "BioWriteDeviceLogs", "BioGuid", "Dbo", false, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("BioDeviceRels", "Dbo");
		migrationBuilder.DropTable("BioWriteDeviceLogs", "Dbo");
		migrationBuilder.DropTable("BioWriteDevices", "Dbo");
		migrationBuilder.DropTable("BioDatas", "Dbo");
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
		modelBuilder.Entity("TimeAttendanceDAL.Dal.Device.BioData", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Data").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnrolType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PersonGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "BioDatas");
		});
		modelBuilder.Entity("TimeAttendanceDAL.Dal.Device.BioDeviceRel", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BioGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DeviceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("FromDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("HardwareCode"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Privilage"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("ToDateM"), "datetime");
			b.HasKey(new string[2] { "BioGuid", "DeviceGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "BioDeviceRels");
		});
		modelBuilder.Entity("TimeAttendanceDAL.Dal.Device.BioWriteDevice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DeviceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("HardwareCode"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BioInfo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HarwareName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("InsertedDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("NextTry"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TryCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "DeviceGuid", "HardwareCode" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "BioWriteDevices");
		});
		modelBuilder.Entity("TimeAttendanceDAL.Dal.Device.BioWriteDeviceLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
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
			b.HasIndex(new string[1] { "BioGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "BioWriteDeviceLogs");
		});
		modelBuilder.Entity("TimeAttendanceDAL.Dal.Device.BioDeviceRel", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("TimeAttendanceDAL.Dal.Device.BioData", "BioData").WithMany("BioDeviceRels").HasForeignKey(new string[1] { "BioGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("TimeAttendanceDAL.Dal.Device.BioWriteDeviceLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("TimeAttendanceDAL.Dal.Device.BioData", "BioData").WithMany("BioWriteDeviceLogs").HasForeignKey(new string[1] { "BioGuid" })
				.OnDelete((DeleteBehavior)5);
		});
	}
}
