using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20210628072856_ImproveMarqueeMessageTable")]
public class ImproveMarqueeMessageTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		Type typeFromHandle = typeof(DateTime);
		migrationBuilder.AlterColumn<DateTime>("FromDate", "Att_MarqueeMessages", "date", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "datetime", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(DateTime);
		migrationBuilder.AlterColumn<DateTime>("ToDate", "Att_MarqueeMessages", "date", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "datetime", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		Type typeFromHandle = typeof(DateTime);
		migrationBuilder.AlterColumn<DateTime>("FromDate", "Att_MarqueeMessages", "datetime", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "date", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(DateTime);
		migrationBuilder.AlterColumn<DateTime>("ToDate", "Att_MarqueeMessages", "datetime", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "date", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.10").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Attendance.MarqueeMessage", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<Guid>(RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier"), "NewId()");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnSource").ValueGeneratedOnAdd(), "smallint"), "5");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Enable"), "bit");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<int>(RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ForBelowVersions").ValueGeneratedOnAdd(), "int"), "999999999");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("FromDate"), "date");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedDate").ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<int>(RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RepeatCount").ValueGeneratedOnAdd(), "int"), "999999999");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ToDate"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_MarqueeMessages");
		});
	}
}
