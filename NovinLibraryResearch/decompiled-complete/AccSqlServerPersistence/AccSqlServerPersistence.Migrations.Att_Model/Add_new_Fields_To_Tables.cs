using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20211225102708_Add_new_Fields_To_Tables")]
public class Add_new_Fields_To_Tables : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>("Status", "Att_MarqueeMessages", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)false, (string)null, (string)null, (bool?)null, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn("Status", "Att_MarqueeMessages", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.10").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Attendance.MarqueeMessage", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<Guid>(RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier"), "NewId()");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ColorName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnSource").ValueGeneratedOnAdd(), "smallint"), "5");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Enable"), "bit");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<int>(RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ForBelowVersions").ValueGeneratedOnAdd(), "int"), "999999999");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ForLkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("FromDate"), "date");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedDate").ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<int>(RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RepeatCount").ValueGeneratedOnAdd(), "int"), "999999999");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ToDate"), "date");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_MarqueeMessages");
		});
	}
}
