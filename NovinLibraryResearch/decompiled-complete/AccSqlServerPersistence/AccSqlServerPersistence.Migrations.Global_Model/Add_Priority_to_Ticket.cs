using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Global_Model;

[DbContext(typeof(Global_ModelContext))]
[Migration("20240105095540_Add_Priority_to_Ticket")]
public class Add_Priority_to_Ticket : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<short>("Priority", "Global_Ticket", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)(short)0, (string)null, (string)null, (bool?)null, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn("Priority", "Global_Ticket", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.22").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.CheckedSave", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsDebit"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ReportType"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_CheckedSave");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.En2Fa", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FName").IsRequired(true), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LName").IsRequired(true), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "LName" }).IsUnique(true).HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_L2F");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.MetaData", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EntityType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Key"), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DataType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Value").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[3] { "EntityGuid", "EntityType", "Key" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_MetaData");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Naghz", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Count"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Message" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Naghz");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGenerator", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("Document"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastUpdate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Rpt").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_ReportGenerator");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGeneratorDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(RelationalPropertyBuilderExtensions.HasColumnName<Guid>(b.Property<Guid>("MasterGuid"), "RepGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("OrderIndex"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ParameterName").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ParameterType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_ReportGeneratorDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.ContactUs", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CodeMeli").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateBirth"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Email").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Family").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Fax").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Jensiat"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Mob").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Mob2").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("NahveAshnaee").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ostan").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ReferencedId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SabegheKari").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Senf").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Shahrestan").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Shoghl").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Subject").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tahsilat").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tel").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tel2").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("WebSite").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Zip").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Type" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ContactUs");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Desc", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Destination").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DestinationType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Section"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Source").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SourceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Desc");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.FileInfo", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("CompressMode"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Crc"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("FileContent"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("FileLen"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FileName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Part"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SubDirectory").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Url"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Modified" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_FileInfo");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.FileInfoReTryDownload", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Crc"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("FileLen"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FileName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("NextTryCount"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Part"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ReTryCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SubDirectory").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Url"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Modified" });
			b.HasIndex(new string[1] { "ReTryCount" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_FileInfoReTryDownload");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.Card", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BackColor"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("Image"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Target"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TextString1"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TextString2"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TextString3"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TextString4"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Theme"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ToolTipMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_Card");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.Menu", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ColumnCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RowCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_Menu");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.MenuItem", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TypeGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_MenuItems");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.PushNotification", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DisplayCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpireDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HelperUri"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_PushNotifications");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.SaveTemp", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("SerialedData"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_SaveTemp");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.StimulQuery", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RefrencedId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Order"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("Id"), "int");
			b.HasKey(new string[2] { "RefrencedId", "Order" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "StimulQuery");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.HyperLink", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Category").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Enable"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Icon"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Order"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ParentId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Style").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Target").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Url"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_HyperLink");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.LocalSetting", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Key"), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Value").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Key" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_LocalSetting");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.LogTable", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LogPart"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserDescription").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Log");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.News", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Category").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullContent").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("KeyWords").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaDescription").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicUrl").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("QuickText").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Type"), "int");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_News");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Taghvim", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Monasebat").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("STRasmi"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[2] { "Guid", "DateM" }).HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_Taghvim");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.Ticket", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PartGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Priority"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReferenceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SourceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SourceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "PartGuid" });
			b.HasIndex(new string[1] { "ReferenceGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_Ticket");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM").ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSeen"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SourceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SourceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TargetGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TargetGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_TicketDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketPart", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EndStatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StartStatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TargetGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TargetGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_TicketPart");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Uploader.BackupLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Id").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("BackupType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DPId"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FileId"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FileName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsDelete"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastBackup"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastStateChange"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Message").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Path").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("RetryCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Status"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("TotalCrc"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("URL").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Id" });
			b.HasIndex(new string[1] { "DPId" });
			b.HasIndex(new string[1] { "FileId" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BackupLog");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Uploader.BackupSchedule", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BackupPath").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("BackupType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ConnectionString").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreateDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Houre"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(450)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Title" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BackupSchedule");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Uploader.BackupSettings", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id").ValueGeneratedOnAdd(), "int").HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Password").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("RememberMe"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Startup"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Username").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Zip"), "bit");
			b.HasKey(new string[1] { "Id" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BackupSettings");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Uploader.BackupTime", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateTime"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DayOfWeek"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentId"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BackupTime");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Uploader.UploadFile", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Id").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Crc"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("FileManagementParts"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("InsertTime"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("MaxChunk"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("MinChunk"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PartCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PartSent"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("RetryCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SubDirectory").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UrlPath").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Id" });
			b.HasIndex(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_UploadFile");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Uploader.UploadFilePart", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Order").ValueGeneratedOnAdd(), "bigint").HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("Bytes"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FileId"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSent"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("NextTry"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("RetryCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Order" });
			b.HasIndex(new string[1] { "FileId" });
			b.HasIndex(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_UploadFilePart");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.UserInformation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Username"), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Password").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserData").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Username" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_UserInformation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Global.IntroductionMethod", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_IntroductionMethod");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Global.Post", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("EndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("Image"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Link"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Startdate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_Post");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Global.Tickets.TicketTarget", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_TicketTarget");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Global.Tickets.TicketTargetDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TargetGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TargetType"), "smallint");
			b.HasKey(new string[3] { "MasterGuid", "TargetGuid", "TargetType" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_TicketTargetDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Global.Wage", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("CalculationMethod"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("MasterType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("MaxTransaction"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("MinTransaction"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceOrPercent"), "decimal(18,4)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ServerGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "ServerGuid" }).IsUnique(true), "[ServerGuid] IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Global_Wage");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGeneratorDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.ReportGenerator", "Master").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.MenuItem", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Global.Menu", "MasterItem").WithMany("DetailItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.Ticket", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Tickets.TicketPart", "Part").WithMany("Tickets").HasForeignKey(new string[1] { "PartGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Tickets.TicketDet", "Reference").WithMany("Tickets").HasForeignKey(new string[1] { "ReferenceGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Tickets.Ticket", "Ticket").WithMany("TicketDets").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Global.Tickets.TicketTarget", "Target").WithMany("TicketDetail").HasForeignKey(new string[1] { "TargetGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketPart", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Global.Tickets.TicketTarget", "TicketTarget").WithMany("TicketPart").HasForeignKey(new string[1] { "TargetGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Global.Tickets.TicketTargetDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Global.Tickets.TicketTarget", "Master").WithMany("TargetDetail").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
	}
}
