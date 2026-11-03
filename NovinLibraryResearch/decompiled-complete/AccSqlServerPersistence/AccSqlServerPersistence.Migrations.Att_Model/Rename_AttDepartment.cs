using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20240312110113_Rename_AttDepartment")]
public class Rename_AttDepartment : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_AttDepartments", "AttDepartments", "Dbo");
		migrationBuilder.RenameTable("AttDepartments", "Dbo", "Att_Department", "Dbo");
		migrationBuilder.AddPrimaryKey("PK_Att_Department", "Att_Department", "Guid", "Dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_Att_Department", "Att_Department", "Dbo");
		migrationBuilder.RenameTable("Att_Department", "Dbo", "AttDepartments", "Dbo");
		migrationBuilder.AddPrimaryKey("PK_AttDepartments", "AttDepartments", "Guid", "Dbo");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reason"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("RequestDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_PersonLeave");
		});
	}
}
