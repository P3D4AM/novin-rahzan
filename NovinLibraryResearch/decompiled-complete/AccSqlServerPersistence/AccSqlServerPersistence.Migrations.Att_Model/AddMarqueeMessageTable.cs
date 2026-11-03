using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20210628062624_AddMarqueeMessageTable")]
public class AddMarqueeMessageTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema("Dbo");
		migrationBuilder.CreateTable("MarqueeMessages", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Message = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FromDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ToDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ForBelowVersions = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EnSource = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RepeatCount = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Enable = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ModifiedDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_MarqueeMessages", x => (object)x.Guid);
		}, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("MarqueeMessages", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.10").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Attendance.MarqueeMessage", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnSource"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Enable"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ForBelowVersions"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("FromDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RepeatCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ToDate"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "MarqueeMessages");
		});
	}
}
