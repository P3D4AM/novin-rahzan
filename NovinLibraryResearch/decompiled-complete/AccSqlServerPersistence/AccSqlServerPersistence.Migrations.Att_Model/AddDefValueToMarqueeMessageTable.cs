using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Att_Model;

[DbContext(typeof(Att_ModelContext))]
[Migration("20210628071552_AddDefValueToMarqueeMessageTable")]
public class AddDefValueToMarqueeMessageTable : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_MarqueeMessages", "MarqueeMessages", "Dbo");
		migrationBuilder.RenameTable("MarqueeMessages", "Dbo", "Att_MarqueeMessages", "Dbo");
		Type typeFromHandle = typeof(int);
		migrationBuilder.AlterColumn<int>("RepeatCount", "Att_MarqueeMessages", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, "999999999", (string)null, typeFromHandle, "int", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(DateTime);
		migrationBuilder.AlterColumn<DateTime>("ModifiedDate", "Att_MarqueeMessages", "datetime", (bool?)null, (int?)null, false, "Dbo", false, (object)null, "getdate()", (string)null, typeFromHandle, "datetime", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(int);
		migrationBuilder.AlterColumn<int>("ForBelowVersions", "Att_MarqueeMessages", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, "999999999", (string)null, typeFromHandle, "int", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(short);
		migrationBuilder.AlterColumn<short>("EnSource", "Att_MarqueeMessages", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, "5", (string)null, typeFromHandle, "smallint", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(Guid);
		migrationBuilder.AlterColumn<Guid>("Guid", "Att_MarqueeMessages", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, "NewId()", (string)null, typeFromHandle, "uniqueidentifier", (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		migrationBuilder.AddPrimaryKey("PK_Att_MarqueeMessages", "Att_MarqueeMessages", "Guid", "Dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropPrimaryKey("PK_Att_MarqueeMessages", "Att_MarqueeMessages", "Dbo");
		migrationBuilder.RenameTable("Att_MarqueeMessages", "Dbo", "MarqueeMessages", "Dbo");
		Type typeFromHandle = typeof(int);
		migrationBuilder.AlterColumn<int>("RepeatCount", "MarqueeMessages", "int", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, "999999999", (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(DateTime);
		migrationBuilder.AlterColumn<DateTime>("ModifiedDate", "MarqueeMessages", "datetime", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "datetime", (bool?)null, (int?)null, false, false, (object)null, "getdate()", (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(int);
		migrationBuilder.AlterColumn<int>("ForBelowVersions", "MarqueeMessages", "int", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, "999999999", (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(short);
		migrationBuilder.AlterColumn<short>("EnSource", "MarqueeMessages", "smallint", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, "5", (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(Guid);
		migrationBuilder.AlterColumn<Guid>("Guid", "MarqueeMessages", "uniqueidentifier", (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, "NewId()", (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		migrationBuilder.AddPrimaryKey("PK_MarqueeMessages", "MarqueeMessages", "Guid", "Dbo");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("FromDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedDate").ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<int>(RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RepeatCount").ValueGeneratedOnAdd(), "int"), "999999999");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ToDate"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Att_MarqueeMessages");
		});
	}
}
