using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Acc_Model;

[DbContext(typeof(Acc_ModelContext))]
[Migration("20210414102131_Remove_TafsilGuid_From_Customer")]
public class Remove_TafsilGuid_From_Customer : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn("TafsilGuid", "Acc_Customer", "Dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		object obj = new Guid("00000000-0000-0000-0000-000000000000");
		migrationBuilder.AddColumn<Guid>("TafsilGuid", "Acc_Customer", "uniqueidentifier", (bool?)null, (int?)null, false, "Dbo", false, obj, (string)null, (string)null, (bool?)null, (string)null);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.10").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.AbadDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Count"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ShapeInformation"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ShapeType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("X"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Y"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Z"), "float");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AbadDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Checked"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Count"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResid"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateSarResidSh"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidGroup"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Mounth"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceMande"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("ReceptionDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ReceptionDateSh"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ReceptionDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ReceptionId2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ReceptionSanadNo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ReceptionSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("TypePardakht"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserName"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidGroup" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "ReceptionGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AghsatAshkhas");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM_Group", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Count"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AghsatM_Group");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResid"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ID2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "PardakhtGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AghsatShakhsi");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cashier", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("HashedPassword"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StrCashierTime"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserName"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Cashier");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.CashiersLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Action"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Part"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ser_New"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ser_OLD"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CashiersLog");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeBook", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BankGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSaderShode"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerialNumber"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "BankGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeBook");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.ChequeSHBargasht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeSHBargasht");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.ChequeSHEbtal", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeSHEbtal");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.ChequeSHPardakht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CheckSHPardakht");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.PardakhtChequeSHA2re", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Is_Enteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("PardakhtDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtCheckSHA2re");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.BankCheque", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Avale2re_Transfer"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidBank"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheque"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidBank" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BankCheque");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.ChequeMBargasht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("BeforeSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheque"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidCheque" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeMBargasht");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.ChequeVosol", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheque"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidCheque" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeVosol");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.ReceptionChequeMA2re", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Is_Enteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ReceptionDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionChequeMA2re");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.SandoghCheque", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheque"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSandogh"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidCheque" });
			b.HasIndex(new string[1] { "GuidSandogh" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SandoghCheque");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ComputerName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DeviceNumber"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsCo"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LineNumber"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "DeviceNumber", "LineNumber" }).IsUnique(true), "IX_Acc_CallingLine_DeviceNo_LineNo");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CallingLine");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CalcedNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CallerIdNumber"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CoGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DialedNumber"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ExGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("HangUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsCompleteCall"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsIncoming"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("PickUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Rings"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TicketGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "CalcedNumber" }), "IX_Acc_CallingLog_CalcedNumber");
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "DateM" }), "IX_Acc_Calling_DateM").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CallingLog");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Abstract"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Country"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("IconIndex"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("LastPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastUpdate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalIndexBuilderExtensions.HasFilter(RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "Name", "Country" }).IsUnique(true), "IX_Decimal_Name_Country"), "[Name] IS NOT NULL AND [Country] IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Decimal");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DecimalUpdates", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_DecimalUpdates");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DepEnteghal", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GirandeName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MashinNo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("No"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RanandehName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_DepEnteghal");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DepEnteghalDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DptGuidDst"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DptGuidSrc"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("V2Qt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VahedGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DptGuidSrc" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			b.HasIndex(new string[1] { "VahedGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_DepEnteghalDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.FareOrderCredit", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MoeinName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("TafsilCode"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TafsilName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ViewOrder"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_FareOrderCredit");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.HesabGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Count"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TypeGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "TypeGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_HesabGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.HesabTypes", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_HesabType");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Kardex", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Fare"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ID2"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Invalidated"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("KhorojQT"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<float?>(b.Property<float?>("RemQT"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RemTotalPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RemUnitPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RowSood"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RowSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Tax"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("TotalHazine"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("TotalSood"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("UnitPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("UnitSood"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("UserGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("VorodQT"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("validated"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[3] { "ProductGuid", "DepotGuid", "DateM" }), "IX_Acc_Kardex_prdGuid_DepotGuid_Date").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Kardex");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Kol", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Account"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Code"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Mahiat"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Kol");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Lottery", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "PardakhtGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Lottery");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.LotteryScore", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpirationDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Score"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_LotteryScores");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Moein", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Account"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("CodeKol"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KolGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Mahiat"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Peygiri"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Tedadi"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "KolGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Moein");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.MoeinTafsilGroupRelations", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "MoeinGuid", "TafsilGroupGuid" });
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_MoeinTafsilLevelRelations");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.Note", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResidM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("HGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Pirority"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PirorityName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StatusName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Text"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Note");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.NoteGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_NoteGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdAnbarGardani", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EzafeCreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EzafeCreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EzafeDebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EzafeDebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("EzafeSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrCreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrCreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrDebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrDebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KasrSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "EzafeCreditMoeinGuid" });
			b.HasIndex(new string[1] { "EzafeCreditTafsilGuid" });
			b.HasIndex(new string[1] { "EzafeDebitMoeinGuid" });
			b.HasIndex(new string[1] { "EzafeDebitTafsilGuid" });
			b.HasIndex(new string[1] { "KasrCreditMoeinGuid" });
			b.HasIndex(new string[1] { "KasrCreditTafsilGuid" });
			b.HasIndex(new string[1] { "KasrDebitMoeinGuid" });
			b.HasIndex(new string[1] { "KasrDebitTafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdAnbarGardani");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdAnbarGardaniDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DptGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("EzafeSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KasrSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("QtEzafe"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("QtKasr"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("UnitPrice"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DptGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdAnbarGardaniDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSelected");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSerial", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DetailType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("EndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GarrantyCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsKhoroj"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("RefrencedGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Sms"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SmsMob"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("StartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSerial");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdTechnical", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TechnicalType"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdTechnical");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Prd_SelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSelectedDetails");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ProductAttribute", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Value"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductAttribute");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ProductAttributeRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AttributeGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "ProductGuid", "AttributeGuid" });
			b.HasIndex(new string[1] { "AttributeGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductAttributeRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGenerator", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Asli"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CodeG"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("CodeHesab"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CodeKol"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CodeS"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CusGroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Desc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Faree"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("HGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("HTreeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Logs"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MoeinCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("NahiyeId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Num1"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Num2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Numbers"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Percent"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdgGroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceC1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceC2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceD1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PriceD2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Rpt"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SelectedHesab"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SelectedPrd"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ShahrId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SqlQuery"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CusGroupGuid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "HGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			b.HasIndex(new string[1] { "UGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReportGenerator");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGeneratorDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Order"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ParameterName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("RepGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "RepGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReportGeneratorDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Resid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Beyane"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Darkhast"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidVahed"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("ID2"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Mandeh"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdColor"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdModel"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Problems"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("State"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Resid");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ResidStatus", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ResidStatus");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.SanadPrint", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Mob"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tel"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadPrint");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ShortCut", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BackColor"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Enable"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ForeColor"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FormName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Func"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ImageSrc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ShortCutKey"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SubItems"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Visable"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ShortCut");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tafsil", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Account"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tafsil");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Checked"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CodeMax"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CodeMin"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Id"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinAmaniDigaranNazdMaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinAmaniMaNazdDigaranGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinBackBuyGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinBackSellGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinBuyGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinPardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinSellGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tbl"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilNumber", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("OriginalNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CalcedNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Prefix"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			b.HasKey(new string[2] { "TafsilGuid", "OriginalNumber" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "CalcedNumber" }), "IX_Acc_TafsilNumber_CalcedNumber");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilNumbers");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilSelected");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilSelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("HGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilSelectedDetails");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ID2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("SUMHazine"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("SUMMahsol"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("SUMMavad"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadST"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DPTGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PRDGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double?>(b.Property<double?>("V2QT"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DPTGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "PRDGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Export");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormula", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormulaExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DPTGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PRDGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DPTGuid" });
			b.HasIndex(new string[1] { "FormulaGuid" });
			b.HasIndex(new string[1] { "PRDGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula_Export");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormulaHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "FormulaGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula_Hazine");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormulaMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DPTGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PRDGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DPTGuid" });
			b.HasIndex(new string[1] { "FormulaGuid" });
			b.HasIndex(new string[1] { "PRDGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula_Mavad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Hazine");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DPTGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PRDGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RowSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double?>(b.Property<double?>("V2QT"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DPTGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "PRDGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Mavad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.UserAcc", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllBanksAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllDepotsAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllSandoghsAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllVisitorAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid1"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid4"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CusGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DefaultCusGroup"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Id"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsDisposed"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LoginAccess"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("SandoghGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Settings"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserAccessStr"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_UserAcc");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Vam", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AghsatGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SepordeMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SepordeTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VamMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VamTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ZamenGuid1"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ZamenGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ZamenGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ZamenGuid4"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Zarib"), "real");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "PardakhtGuid" });
			b.HasIndex(new string[1] { "SepordeMoeinGuid" });
			b.HasIndex(new string[1] { "SepordeTafsilGuid" });
			b.HasIndex(new string[1] { "VamMoeinGuid" });
			b.HasIndex(new string[1] { "VamTafsilGuid" });
			b.HasIndex(new string[1] { "ZamenGuid1" });
			b.HasIndex(new string[1] { "ZamenGuid2" });
			b.HasIndex(new string[1] { "ZamenGuid3" });
			b.HasIndex(new string[1] { "ZamenGuid4" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Vam");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ViewCheckM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustomerFullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateBargashtORVosol"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateBargashtORVosolM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateCheckM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateSanad"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSanadM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateToPlace"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HesabNum"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("IDSanad"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Personal"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Place"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("PoshtNomreh"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Status"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewCheckM");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ViewCheckSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustomerFullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateSarresid"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarresidM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("IDSanad"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PardakhtDate"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PardakhtDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ST"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewCheckSh");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Barcode", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BarCode"), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreateDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DepotId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpireDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PriceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Type"), "int");
			b.HasKey(new string[1] { "BarCode" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Barcode");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.ConnectedSanad_Det", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Credit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Debit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("MasterType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Qt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MasterSanad" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ConnectedSanad_Det");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CusCard", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CardNO"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CardName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CusCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CusGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CusName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateExpire"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateStart"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pass"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CusGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CusCard");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Customer", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address1"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address2"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("BPercent"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CodeMeli"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Company"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("CustType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DefPrice"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("EghtesadiNo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Etebar"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("EtebarCheck"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Family"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("IdPath"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("InPlacePayment"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsBazaryab"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEtebari"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSupervisor"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Job"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Location"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Password"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicFace"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicSign"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("RDate2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("RDate3"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("RDate5"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR1"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR10"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR2"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR3"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR4"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR5"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR6"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR7"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR8"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RSTR9"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("Rdate1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("Rdate4"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SupervisorGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TafsilLevel"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tell1"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TitleGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("User"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Zip"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Code" });
			b.HasIndex(new string[1] { "GroupGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TitleGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "User", "Password" }), "IX_Customer_Username_Password");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Customer");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CustomerGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinDefaultGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CustomerGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CustomerTitle", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CustomerTitle");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Depot", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Manfi"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Depot");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.OrderDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInDiscount"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInPrice"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInQuantity"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInVisitorPrice1"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInVisitorPrice2"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInVisitorPrice3"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInVisitorPrice4"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DecimalName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("DecimalQt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine1Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine3Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine4Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine5Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("OrderDetState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PriceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ProductName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Quantity"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RowSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Tax"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazinePrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalVisitorPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double?>(b.Property<double?>("V2Qt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice4"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OrderDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtBank", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtBank");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtCheckM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheck"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidCheck" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtCheckM");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtCheckSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("A2re_MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("A2re_TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BankMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BankTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSaresid"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("PardakhtDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "A2re_MoeinGuid" });
			b.HasIndex(new string[1] { "A2re_TafsilGuid" });
			b.HasIndex(new string[1] { "BankMoeinGuid" });
			b.HasIndex(new string[1] { "BankTafsilGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtCheckSh");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtNaghd", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtNaghd");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionCheck", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("A2re_PardazandeMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("A2re_PardazandeTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("A2re_ReceptionDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HesabNum"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Personal"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("PoshtNomreh"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Status"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "A2re_PardazandeMoeinGuid" });
			b.HasIndex(new string[1] { "A2re_PardazandeTafsilGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionCheck");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionHavale", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateTasviye"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MerchantId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MessageId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PAN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Qt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RRN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STAN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TerminalId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionHavale");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionNaghd", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionNaghd");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.FullTextWords", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Word").IsRequired(true), "nvarchar(400)").HasMaxLength(400);
			b.HasKey(new string[1] { "Id" });
			b.HasIndex(new string[1] { "Word" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_FullTextWords");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.GardeshTafsil", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("AsTypeEnum"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Bazaryab"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Credit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateMoaser"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Debit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("DecimalQt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("DiscountPercent"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnAsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KolGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("QT"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("QT2"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Rem"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RowDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SanadDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadSt"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "KolGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_GardeshTafsil");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Gps", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Accuracy"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Activity"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Altitude"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Bearing"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ColorHex"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Distance"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("FromLastPointDistance"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("FromStartPointDistance"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("GPSTime"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Icon"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Latitude"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Longitude"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("PointDurationSeconds"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PreviewsID"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ReferencedId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Speed"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Time"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("UId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserAccGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Gps");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxCustomerAllFields", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxCustomerAllFields");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductAllFields", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductAllFields");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductCodeField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductCodeField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductDescField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductDescField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductNameAndCodeField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductNameAndCodeField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductNameField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductNameField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT1Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT1Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT2Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT2Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT3Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT3Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT4Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT4Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT5Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT5Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT6Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT6Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT7Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT7Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT8Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT8Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.LastPrice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Invalidated"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			b.HasKey(new string[2] { "TafsilGuid", "PrdGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_LastPrice");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Order", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AfterAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AghsatGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AndroidIme"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("AndroidImeLong"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AvarezMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AvarezTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("B1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B1MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B1TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("B2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B2MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B2TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("B3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B3MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B3TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("B4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B4MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("B4TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BarnameNo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BazaryabFullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BeforeAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CpuSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustAddress"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustFamily"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustMobile"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustTell"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustZipCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustomerFullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DailyFactorNo"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DeliveryGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("DeliveryStatus"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountRows"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("FactorType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("FareMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("FareTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineMoeinGuid1"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineMoeinGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineMoeinGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineMoeinGuid4"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineMoeinGuid5"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineTafsilGuid1"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineTafsilGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineTafsilGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineTafsilGuid4"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("HazineTafsilGuid5"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HddSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsDisposed"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Keraye"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PardakhtSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PaymentMethod"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PriceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PrintCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("PrintDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ReceptionSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumKala"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("TasvieDay"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Tax"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TaxMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TaxTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalBazaryab"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalDiscount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazine"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazine1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazine2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazine3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazine4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazine5"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserAccGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("UserAccId"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "AvarezMoeinGuid" });
			b.HasIndex(new string[1] { "AvarezTafsilGuid" });
			b.HasIndex(new string[1] { "B1MoeinGuid" });
			b.HasIndex(new string[1] { "B1TafsilGuid" });
			b.HasIndex(new string[1] { "B2MoeinGuid" });
			b.HasIndex(new string[1] { "B2TafsilGuid" });
			b.HasIndex(new string[1] { "B3MoeinGuid" });
			b.HasIndex(new string[1] { "B3TafsilGuid" });
			b.HasIndex(new string[1] { "B4MoeinGuid" });
			b.HasIndex(new string[1] { "B4TafsilGuid" });
			b.HasIndex(new string[1] { "DeliveryStatus" });
			b.HasIndex(new string[1] { "FareMoeinGuid" });
			b.HasIndex(new string[1] { "FareTafsilGuid" });
			b.HasIndex(new string[1] { "HazineMoeinGuid1" });
			b.HasIndex(new string[1] { "HazineMoeinGuid2" });
			b.HasIndex(new string[1] { "HazineMoeinGuid3" });
			b.HasIndex(new string[1] { "HazineMoeinGuid4" });
			b.HasIndex(new string[1] { "HazineTafsilGuid1" });
			b.HasIndex(new string[1] { "HazineTafsilGuid2" });
			b.HasIndex(new string[1] { "HazineTafsilGuid3" });
			b.HasIndex(new string[1] { "HazineTafsilGuid4" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "PardakhtGuid" }).IsUnique(true), "[PardakhtGuid] IS NOT NULL");
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "ReceptionGuid" }).IsUnique(true), "[ReceptionGuid] IS NOT NULL");
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "TaxMoeinGuid" });
			b.HasIndex(new string[1] { "TaxTafsilGuid" });
			b.HasIndex(new string[1] { "UserAccGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "FactorType", "Id2" }).IsUnique(true), "IX_Order_ID2_Type");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Order");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OrderAbad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("OrderType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("RefrencedGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ShapeInfo"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OrderAbad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Pardakht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AfterAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AndroidIme"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BeforeAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CpuSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HddSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsRead"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumBank"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumCheckM"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumCheckSh"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumEnteghal"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumNaghd"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Id2" }).IsUnique(true);
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Pardakht");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.PrdAnbarRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AnbarGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Amount"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Min"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[2] { "PrdGuid", "AnbarGuid" });
			b.HasIndex(new string[1] { "AnbarGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdAnbarRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Product", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AutoSellSt"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("BPercent1"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("BPercent2"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("BPercent3"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("BPercent4"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Buy1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Buy2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Buy3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BuyLast"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreateDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("DiscountPercent"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullContent"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidGroup"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidVahed"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Height"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("IdProduct"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("IdProductLower"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ImageList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSerial"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTax"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KardexPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Keywords"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Mojodi"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("OrderPoint"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("OtherPics"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PartitionId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PayementType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pic"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("QuickText"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ScaleSt"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Score"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell1Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell2Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell3Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell4Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell5"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell5Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell6"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell6Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell7"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell7Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell8"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sell8Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr1"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr2"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr3"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr4"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr5"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr6"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr7"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellKasr8"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood1"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood2"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood3"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood4"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood5"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood6"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood7"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("SellSood8"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T1"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T2"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T3"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T4"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T5"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T6"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T7"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T8"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tags"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("V2Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V3Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("V3Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V4Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("V4Qt"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Weight"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Width"), "real");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "IdProductLower" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Product");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.ProductGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("FullCode"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HtmlFull"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HtmlQuick"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Level"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pic"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TechInfoData"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Status" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.ProductVahed", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductVahed");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.RasGiri", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("BaseDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DayCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RecordCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve1"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve10"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve2"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve3"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve4"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve5"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve6"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve7"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve8"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve9"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sood_Mounthly"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Sood_Percent"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sood_Total"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Total_Sum"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_RasGiri");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.RasGiriDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<float>(b.Property<float>("Enheraf"), "real");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_RasGiriDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Reception", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AfterAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AndroidIme"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BeforeAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CpuSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("DeliveryStatus"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DependencyType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HddSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsChecked"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PaymentMethod"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumBank"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumCheck"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumEnteghal"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumNaghd"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Id2" }).IsUnique(true);
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Reception");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanad_Det", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Credit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Debit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("MasterType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<double>(b.Property<double>("Qt"), "float");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanads", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Sanad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.SuggestedBank", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AccountNo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ShobeCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SuggestedBank");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Visit", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreationdDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ScheduledDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedTicketGuids"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("StartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("VisitIsCompleted"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("VisitNo"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Visit");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.AbadDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Details.OrderDetail", "OrderDetail").WithMany("OrderAbads").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM_Group", "Groups").WithMany("AghsatAshkhas").HasForeignKey(new string[1] { "GuidGroup" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moeins").WithMany("AghsatAshkhas").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "Receptions").WithMany("AghsatAshkhas").HasForeignKey(new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsils").WithMany("AghsatAshkhas").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("AghsatMs").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moeins").WithMany("AghsatShakhsi").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "Pardakhts").WithMany("AghsatShakhsi").HasForeignKey(new string[1] { "PardakhtGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsils").WithMany("AghsatShakhsi").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("AghsatShs").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cashier", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.UserAcc", "UserItem").WithMany("Cashiers").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.CashiersLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("CashiersLogs").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Sanads", "Sanads").WithMany("CashiersLogs").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeBook", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("ChequeBooks").HasForeignKey(new string[1] { "BankGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.ChequeSHBargasht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ChequeSHBargashts").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.ChequeSHEbtal", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ChequeSHEbtals").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.ChequeSHPardakht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ChequeSHPardakhts").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Pardakhtcheque.PardakhtChequeSHA2re", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("PardakhtChequeSHA2re").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("PardakhtChequeSHA2re").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("PardakhtChequeSHA2res").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.BankCheque", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsils").WithMany("BankCheques").HasForeignKey(new string[1] { "GuidBank" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("BankCheques").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.ChequeMBargasht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Details.ReceptionCheck", "Receptioncheque").WithMany("ChequeMBargashts").HasForeignKey(new string[1] { "GuidCheque" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ChequeMBargashts").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.ChequeVosol", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Details.ReceptionCheck", "ReceptionCheck").WithMany("ChequeVosols").HasForeignKey(new string[1] { "GuidCheque" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ChequeVosols").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.ReceptionChequeMA2re", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("ReceptionChequeMa2re").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("ReceptionChequeMa2re").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ReceptionChequeMA2res").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.Receptioncheque.SandoghCheque", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Details.ReceptionCheck", "ReceptionCheck").WithMany("SandoghCheques").HasForeignKey(new string[1] { "GuidCheque" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("SandoghCheques").HasForeignKey(new string[1] { "GuidSandogh" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("SandoghCheques").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("CallingLines").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DecimalUpdates", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DecimalEntity").WithMany("DecimalUpdateses").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DepEnteghal", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("DepEnteghals").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DepEnteghalDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("DepEnteghalDets").HasForeignKey(new string[1] { "DptGuidSrc" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DepEnteghal", "DepEnteghal").WithMany("DepEnteghalDets").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("DepEnteghalDets").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "ProductVahed").WithMany("DepEnteghalDets").HasForeignKey(new string[1] { "VahedGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.FareOrderCredit", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("FareOrderCredits").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("FareOrderCredits").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.HesabGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.HesabTypes", "HesabTypes").WithMany("HesabGroups").HasForeignKey(new string[1] { "TypeGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Kardex", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("Kardexes").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("Kardexes").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("Kardexes").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Kardexes").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("Kardexs").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Kol", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.HesabGroup", "HesabGroup").WithMany("Kols").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Lottery", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("Lotteries").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "Pardakht").WithMany("Lotteries").HasForeignKey(new string[1] { "PardakhtGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Lotteries").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.LotteryScore", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("LotteryScores").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("LotteryScores").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("LotteryScores").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Moein", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Kol", "Kol").WithMany("Moeins").HasForeignKey(new string[1] { "KolGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.MoeinTafsilGroupRelations", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("MoeinTafsilLevelRelation").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TafsilGroup", "TafsilLevelItem").WithMany("MoeinTafsilLevelRelations").HasForeignKey(new string[1] { "TafsilGroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.Note", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("Notes").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Note.NoteGroup", "Group").WithMany("Notes").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdAnbarGardani", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "EzafeCreditMoein").WithMany("EzafeCredit").HasForeignKey(new string[1] { "EzafeCreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "EzafeCreditTafsil").WithMany("EzafeCredit").HasForeignKey(new string[1] { "EzafeCreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "EzafeDebitMoein").WithMany("EzafeDebit").HasForeignKey(new string[1] { "EzafeDebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "EzafeDebitTafsil").WithMany("EzafeDebit").HasForeignKey(new string[1] { "EzafeDebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "KasrCreditMoein").WithMany("KasrCredit").HasForeignKey(new string[1] { "KasrCreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "KasrCreditTafsil").WithMany("KasrCredit").HasForeignKey(new string[1] { "KasrCreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "KasrDebitMoein").WithMany("KasrDebit").HasForeignKey(new string[1] { "KasrDebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "KasrDebitTafsil").WithMany("KasrDebit").HasForeignKey(new string[1] { "KasrDebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("PrdAnbarGardanis").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdAnbarGardaniDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("AnbarGardani").HasForeignKey(new string[1] { "DptGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.PrdAnbarGardani", "Master").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("AnbarGardani").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("PrdSelecteds").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdTechnical", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("Technicals").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Prd_SelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.PrdSelected", "Master").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ProductAttributeRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.ProductAttribute", "ProductAttribute").WithMany("ProductAttributeRelations").HasForeignKey(new string[1] { "AttributeGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ProductAttributeRelations").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGenerator", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.CustomerGroup", "CustGroup").WithMany("ReportGenerator").HasForeignKey(new string[1] { "CusGroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("ReportGenerator").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("ReportGenerator").HasForeignKey(new string[1] { "HGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ReportGenerator").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ReportGenerators").HasForeignKey(new string[1] { "UGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGeneratorDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.ReportGenerator", "Master").WithMany("Details").HasForeignKey(new string[1] { "RepGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Resid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("Resid").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Resid").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ShortCut", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("ShortCuts").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tafsil", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TafsilGroup", "TafsilGroupItems").WithMany("TafsilRealtion").HasForeignKey(new string[1] { "TafsilGroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilNumber", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("TafsilNumber").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("CusSelecteds").HasForeignKey(new string[1] { "UGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilSelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TafsilSelected", "TafsilSelected").WithMany("TafsilSelectedDetails").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("Tolid").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidExport").HasForeignKey(new string[1] { "DPTGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("TolidExport").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidExport").HasForeignKey(new string[1] { "PRDGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidExport").HasForeignKey(new string[1] { "VGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormulaExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "DPTGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TolidFormula", "Master").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "FormulaGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "PRDGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "VGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormulaHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TolidFormula", "Master").WithMany("TolidFormulaHaizne").HasForeignKey(new string[1] { "FormulaGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("TolidFormulaHaizne").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("TolidFormulaHaizne").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidFormulaMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "DPTGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TolidFormula", "Master").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "FormulaGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "PRDGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "VGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("TolidHazine").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("TolidHazine").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("TolidHazine").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TolidMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidMavad").HasForeignKey(new string[1] { "DPTGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("TolidMavad").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidMavad").HasForeignKey(new string[1] { "PRDGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidMavad").HasForeignKey(new string[1] { "VGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Vam", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("Vam").HasForeignKey(new string[1] { "MoeinGuid" });
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "Pardakht").WithMany("Vam").HasForeignKey(new string[1] { "PardakhtGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "VamSepordeMoein").WithMany("VamSepordeMoein").HasForeignKey(new string[1] { "SepordeMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "VamSepordeTafsil").WithMany("VamSepordeTafsil").HasForeignKey(new string[1] { "SepordeTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "VamGirandeMoein").WithMany("VamGirandeMoein").HasForeignKey(new string[1] { "VamMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "VamGirandeTafsil").WithMany("VamGirandeTafsil").HasForeignKey(new string[1] { "VamTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Zamen1").WithMany("VamZamen1").HasForeignKey(new string[1] { "ZamenGuid1" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Zamen2").WithMany("VamZamen2").HasForeignKey(new string[1] { "ZamenGuid2" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Zamen3").WithMany("VamZamen3").HasForeignKey(new string[1] { "ZamenGuid3" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Zamen4").WithMany("VamZamen4").HasForeignKey(new string[1] { "ZamenGuid4" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Barcode", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depots").WithMany("Barcodes").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Products").WithMany("Barcodes").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.ConnectedSanad_Det", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DecimalEntity").WithMany("ConnectedSanadDets").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("ConnectedSanadDets").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Sanads", "Sanad").WithMany("ConnectedSanadDets").HasForeignKey(new string[1] { "MasterSanad" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("ConnectedSanadDets").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("ConnectedSanadDets").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CusCard", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Customer", "Customers").WithMany("CusCard").HasForeignKey(new string[1] { "CusGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Customer", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.CustomerGroup", "CusGroup").WithMany("Customers").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithOne("Customer").HasForeignKey("AccSqlServerPersistence.Entities.Customer", new string[1] { "Guid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.CustomerTitle", "CusTitle").WithMany("Customers").HasForeignKey(new string[1] { "TitleGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CustomerGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TafsilGroup", "TafsilGroupItems").WithMany("CustomerGroupRealtion").HasForeignKey(new string[1] { "TafsilGroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.OrderDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DecimalItem").WithMany("OrderDetailItem").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "DepotItem").WithMany("OrderDetailItem").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("OrderDetailItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("OrderDetails").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "VahedItem").WithMany("OrderDetailItem").HasForeignKey(new string[1] { "VGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtBank", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DeciamlItem").WithMany("PardakhtBankItem").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithMany("PardakhtBankItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("PardakhtBank").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("PardakhtBank").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtCheckM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Details.ReceptionCheck", "CheckItem").WithMany("PardakhtCheckMItem").HasForeignKey(new string[1] { "GuidCheck" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithMany("PardakhtCheckMItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtCheckSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("PardakhtCheckShA2re").HasForeignKey(new string[1] { "A2re_MoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("PardakhtCheckShA2re").HasForeignKey(new string[1] { "A2re_TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "BankMoeinItem").WithMany("PardakhtCheckShBank").HasForeignKey(new string[1] { "BankMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "BankTafsilItem").WithMany("PardakhtCheckShBank").HasForeignKey(new string[1] { "BankTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithMany("PardakhtCheckShItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtNaghd", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DeciamlItem").WithMany("PardakhtNaghdItem").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithMany("PardakhtNaghdItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("PardakhtNaghd").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("PardakhtNaghd").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionCheck", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "A2re_MoeinItem").WithMany("ReceptionCheck").HasForeignKey(new string[1] { "A2re_PardazandeMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "A2re_TafsilItem").WithMany("ReceptionCheck").HasForeignKey(new string[1] { "A2re_PardazandeTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithMany("ReceptionCheckMItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionHavale", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DeciamlItem").WithMany("ReceptionHavaleItem").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithMany("ReceptionHavaleItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("ReceptionHavale").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("ReceptionHavale").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionNaghd", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DeciamlItem").WithMany("ReceptionNaghdItem").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithMany("ReceptionNaghdItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("ReceptionNaghd").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("ReceptionNaghd").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.GardeshTafsil", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DecimalEntity").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Kol", "Kol").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "KolGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "UGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "ProductVahed").WithMany("GardeshTafsils").HasForeignKey(new string[1] { "VGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Order", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "AvarezMoeinItem").WithMany("AvarezOrder").HasForeignKey(new string[1] { "AvarezMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "AvarezTafsilItem").WithMany("AvarezOrder").HasForeignKey(new string[1] { "AvarezTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "B1MoeinItem").WithMany("B1Order").HasForeignKey(new string[1] { "B1MoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "B1TafsilItem").WithMany("B1Order").HasForeignKey(new string[1] { "B1TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "B2MoeinItem").WithMany("B2Order").HasForeignKey(new string[1] { "B2MoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "B2TafsilItem").WithMany("B2Order").HasForeignKey(new string[1] { "B2TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "B3MoeinItem").WithMany("B3Order").HasForeignKey(new string[1] { "B3MoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "B3TafsilItem").WithMany("B3Order").HasForeignKey(new string[1] { "B3TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "B4MoeinItem").WithMany("B4Order").HasForeignKey(new string[1] { "B4MoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "B4TafsilItem").WithMany("B4Order").HasForeignKey(new string[1] { "B4TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "FareMoeinItem").WithMany("FareOrder").HasForeignKey(new string[1] { "FareMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "FareTafsilItem").WithMany("FareOrder").HasForeignKey(new string[1] { "FareTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Hazine1MoeinItem").WithMany("Hazine1Order").HasForeignKey(new string[1] { "HazineMoeinGuid1" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Hazine2MoeinItem").WithMany("Hazine2Order").HasForeignKey(new string[1] { "HazineMoeinGuid2" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Hazine3MoeinItem").WithMany("Hazine3Order").HasForeignKey(new string[1] { "HazineMoeinGuid3" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Hazine4MoeinItem").WithMany("Hazine4Order").HasForeignKey(new string[1] { "HazineMoeinGuid4" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Hazine1TafsilItem").WithMany("Hazine1Order").HasForeignKey(new string[1] { "HazineTafsilGuid1" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Hazine2TafsilItem").WithMany("Hazine2Order").HasForeignKey(new string[1] { "HazineTafsilGuid2" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Hazine3TafsilItem").WithMany("Hazine3Order").HasForeignKey(new string[1] { "HazineTafsilGuid3" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Hazine4TafsilItem").WithMany("Hazine4Order").HasForeignKey(new string[1] { "HazineTafsilGuid4" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("MoeinOrder").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithOne("OrderItem").HasForeignKey("AccSqlServerPersistence.Entities.Order", new string[1] { "PardakhtGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithOne("OrderItem").HasForeignKey("AccSqlServerPersistence.Entities.Order", new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("TafsilOrder").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "TaxMoeinItem").WithMany("TaxOrder").HasForeignKey(new string[1] { "TaxMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TaxTafsilItem").WithMany("TaxOrder").HasForeignKey(new string[1] { "TaxTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("CashierOrder").HasForeignKey(new string[1] { "UserAccGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Pardakht", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("MoeinPardakht").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("TafsilPardakht").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("CashierPardakht").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.PrdAnbarRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("PrdAnbarRelations").HasForeignKey(new string[1] { "AnbarGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("PrdAnbarRelations").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.RasGiriDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.RasGiri", "Master").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Reception", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("MoeinReception").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("TafsilReception").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("Receptions").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanad_Det", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "Decimal").WithMany("SanadDet").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Sanads", "Master").WithMany("SanadDet").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("SanadDet").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("SanadDet").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanads", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("SanadItems").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
	}
}
