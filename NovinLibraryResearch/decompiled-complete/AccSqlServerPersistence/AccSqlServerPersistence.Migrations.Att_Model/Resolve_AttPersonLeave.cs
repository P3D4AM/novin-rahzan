using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20230719104022_Resolve_AttPersonLeave")]
public class Resolve_AttPersonLeave : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_Att_AttPersonLeave", "Att_AttPersonLeave", "Dbo");
		migrationBuilder.RenameTable("Att_AttPersonLeave", "Dbo", "Att_PersonLeave", "Dbo");
		migrationBuilder.AddColumn<short>("LeaveStatus", "Att_PersonLeave", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)(short)0, (string)null, (string)null, (bool?)null, (string)null);
		migrationBuilder.AddPrimaryKey("PK_Att_PersonLeave", "Att_PersonLeave", "Guid", "Dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_Att_PersonLeave", "Att_PersonLeave", "Dbo");
		migrationBuilder.DropColumn("LeaveStatus", "Att_PersonLeave", "Dbo");
		migrationBuilder.RenameTable("Att_PersonLeave", "Dbo", "Att_AttPersonLeave", "Dbo");
		migrationBuilder.AddPrimaryKey("PK_Att_AttPersonLeave", "Att_AttPersonLeave", "Guid", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.22").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
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
