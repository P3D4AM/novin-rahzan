using System;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Acc_Model;

[DbContext(typeof(Acc_ModelContext))]
[Migration("20250105120557_Add_PriceTypeGuid_to_Barcode")]
public class Add_PriceTypeGuid_to_Barcode : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<Guid>("PriceTypeGuid", "Acc_Barcode", (string)null, (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
		migrationBuilder.Sql(Resources.InsertPriceTypeGuidToBarcode, false);
		migrationBuilder.DropColumn("PriceType", "Acc_Barcode", "Dbo");
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn("PriceTypeGuid", "Acc_Barcode", "Dbo");
		migrationBuilder.AddColumn<short>("PriceType", "Acc_Barcode", "smallint", (bool?)null, (int?)null, false, "Dbo", false, (object)(short)0, (string)null, (string)null, (bool?)null, (string)null);
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.22").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.AbadDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Count"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ShapeInformation"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ShapeType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("X"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Y"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Z"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AbadDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResid"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidGroup"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Mounth"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("RasGiriType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("TypePardakht"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VajhEltezam"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("VajhEltezamModified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "GuidGroup" });
			b.HasIndex(new string[1] { "ReceptionGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AghsatAshkhas");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM_Group", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Count"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ProfitSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AghsatM_Group");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ChangeStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResid"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ID2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllBanksAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllDepotsAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllSandoghsAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AllVisitorAccess"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid1"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("BankGuid4"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CusGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DefaultCusGroup"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FareMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("FareTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("HashedPassword"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LoginAccess"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtSmsCustomerGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionSmsCustomerGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("SandoghGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Setting"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StrCashierTime"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserName").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CusGuid" });
			b.HasIndex(new string[1] { "FareMoeinGuid" });
			b.HasIndex(new string[1] { "FareTafsilGuid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "UserGuid" });
			b.HasIndex(new string[1] { "UserName" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Cashier");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.CashiersLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Action"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Level"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LogType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("NewSerializedData"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("OldSerializedData"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("System"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CashiersLog");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeBook", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BankGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSaderShode"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerialNumber").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "BankGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeBook");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeMOperation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("Avale2re_Transfer"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheque"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("IsCreditTafsilAffective"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("IsDebitTafsilAffective"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSanadAffective"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("NewState"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("OldState"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "DebitMoeinGuid" });
			b.HasIndex(new string[1] { "DebitTafsilGuid" });
			b.HasIndex(new string[1] { "GuidCheque" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeMOperation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeShOperation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ChequeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("NewState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("OldState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "ChequeGuid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "DebitMoeinGuid" });
			b.HasIndex(new string[1] { "DebitTafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ChequeShOperation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ComputerName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DeviceNumber"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsCo"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LineNumber"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(15)").HasMaxLength(15);
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CallerIdNumber").IsRequired(true), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CoGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DialedNumber").IsRequired(true), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ExGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("HangUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsCompleteCall"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsIncoming"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("PickUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Rings"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Uuid"), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "CalcedNumber" }), "IX_Acc_CallingLog_CalcedNumber");
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "DateM" }), "IX_Acc_Calling_DateM").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "Uuid" }).IsUnique(true), "Uuid IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CallingLog");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Abstract").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Country").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("IconIndex"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("LastPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastUpdate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Symbol"), "nvarchar(50)").HasMaxLength(50);
			b.HasKey(new string[1] { "Guid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "Name", "Country" }).IsUnique(true), "IX_Decimal_Name_Country");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GirandeName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MashinNo").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("No"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RanandehName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_DepEnteghal");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.DepEnteghalDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DptGuidDst"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DptGuidSrc"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VahedGuid"), "uniqueidentifier");
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
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Code"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TypeGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "TypeGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_HesabGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.HesabTypes", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Code"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_HesabType");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Kardex", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AmaniCount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DiscountPercentString"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Fare"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ID2").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Invalidated"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("KardexAffective"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KhorojQT"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RDescription").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RemQT"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VorodQT"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("validated"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "Invalidated" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "SanadGuid", "SanadType" }), "IX_Acc_Kardex_SanadGuid_SanadType");
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[5] { "ProductGuid", "DepotGuid", "DateM", "Ordinal", "Guid" }), "IX_Acc_Kardex_prdGuid_DepotGuid_Date_Ordinal").HasAnnotation("SqlServer:Clustered", (object)true);
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("GardeshHesab"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Mahiat"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Code" }).IsUnique(true);
			b.HasIndex(new string[1] { "GroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Kol");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Lottery", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpirationDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Score"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("GardeshHesab"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KolGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Mahiat"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Peygiri"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Tedadi"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Code" }).IsUnique(true);
			b.HasIndex(new string[1] { "KolGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Moein");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.MoeinTafsilGroupRelations", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Level").ValueGeneratedOnAdd(), "smallint"), "((1))");
			b.HasKey(new string[3] { "MoeinGuid", "TafsilGroupGuid", "Level" });
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_MoeinTafsilLevelRelations");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.Note", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateSarResidM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DisplayState"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("EntityType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Pirority"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PirorityName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StatusName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Text").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[2] { "EntityGuid", "Status" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Note");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.NoteGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EzafeCreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("EzafeCreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EzafeDebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("EzafeDebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("EzafeSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrCreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("KasrCreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrDebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("KasrDebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KasrSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("QtEzafe"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("QtKasr"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(RelationalPropertyBuilderExtensions.HasColumnName<decimal?>(b.Property<decimal?>("QtRealAmount"), "RealAmount"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSelected");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSerial", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("EndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GarrantyCode"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTransfer"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("StartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "GarrantyCode" }).IsUnique(true), "GarrantyCode IS NOT NULL");
			b.HasIndex(new string[1] { "ProductGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[2] { "Serial", "ProductGuid" }).IsUnique(true), "Serial IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSerial");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSerialDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsVorod"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal").ValueGeneratedOnAdd(), "smallint"), "((0))");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSerialDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdTechnical", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TechnicalType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[2] { "ProductGuid", "TechnicalType" }).HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdTechnical");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Prd_SelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSelectedDetails");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ProductAttribute", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal").ValueGeneratedOnAdd(), "smallint"), "((1))");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Slug").IsRequired(true), "nvarchar(24)").HasMaxLength(24);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "Slug" }).IsUnique(true), "Slug IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductAttribute");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ProductAttributeRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AttributeValueGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AttributeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			b.HasKey(new string[2] { "ProductGuid", "AttributeValueGuid" });
			b.HasIndex(new string[1] { "AttributeGuid" });
			b.HasIndex(new string[1] { "AttributeValueGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductAttributeRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Resid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Darkhast").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("GuidVahed"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("ID2"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Mandeh"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdColor").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdModel").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdSerial").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Problems").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Reserve"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			b.HasIndex(new string[1] { "ReceptionGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Resid");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ResidStatus", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ResidStatus");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.SanadPrint", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Mob").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tel").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "AsGuid", "AsType" }).IsUnique(true), "IX_Acc_SanadPrint_AsGuid_AsType").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadPrint");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Selection.ProductGroupRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			b.HasKey(new string[2] { "ProductGuid", "GroupGuid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductGroupRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ShortCut", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BackColor").IsRequired(true), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Enable"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ForeColor").IsRequired(true), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FormName").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Func").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ImageSrc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ShortCutKey"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SubItems"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Visable"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ShortCut");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TableRelationShip", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("RelatedSanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("RelatedSanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TableRelationShip");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tafsil", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSpecial"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumAghsat"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Code" }).IsUnique(true);
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tafsil");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BazaryabDefaultGuidData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CodeMax"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CodeMin"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tbl"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilNumber", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("OriginalNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CalcedNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Prefix"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[2] { "TafsilGuid", "OriginalNumber" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "CalcedNumber" }), "IX_Acc_TafsilNumber_CalcedNumber");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilNumbers");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "UGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilSelected");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilSelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("HGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilSelectedDetails");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MahsolMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MahsolTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MavadMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MavadTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumHazine"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumMahsol"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumMavad"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "MahsolMoeinGuid" });
			b.HasIndex(new string[1] { "MahsolTafsilGuid" });
			b.HasIndex(new string[1] { "MavadMoeinGuid" });
			b.HasIndex(new string[1] { "MavadTafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TolidFormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TolidFormulaIndex"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Qt"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "V2Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Export");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormula", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormulaExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "V2Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula_Export");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormulaHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula_Hazine");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormulaMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "V2Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Formula_Mavad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TolidFormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TolidFormulaIndex"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Hazine");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("RowSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TolidFormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TolidFormulaIndex"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Qt"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "V2Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tolid_Mavad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidSummary", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TolidSummary");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidSummaryDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AvailableAmount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ProducedAmount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RealAmount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TolidGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TolidSummaryDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TozieLists.TozieList", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CarNumber"), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DriverMobile1"), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DriverMobile2"), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DriverName").IsRequired(true), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TozieList");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TozieLists.TozieListDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TozieListDetails");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.UserAcc", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Id"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserAccessStr"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_UserAcc");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Vam", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AghsatGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Average"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AverageSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("AverageType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ProfitPercent"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Zarib"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateBargashtORVosolM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateCheckM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSanadM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateToPlace").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HesabNum").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PardazandeTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Personal"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Place").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("PoshtNomreh"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SayadiRegistered"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Sayadi_Identity"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Sayadi_Name"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Sayadi_NationalCode"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Status"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewCheckM");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ViewCheckSh", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ChequeStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CustomerFullName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarresidM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("IDSanad"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PardakhtDate"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PardakhtDesc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SayadiRegistered"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewCheckSh");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ViewSanadDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Credit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Debit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KolGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RowDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid4"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "KolGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "SanadNo" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "TafsilGuid2" });
			b.HasIndex(new string[1] { "TafsilGuid3" });
			b.HasIndex(new string[1] { "TafsilGuid4" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "AsGuid", "AsType" }), "IX_AcC_ViewSanadDetail_AsGuid_AsType").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewSanadDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.VisitorArea", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("FullCode"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Level"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_VisitorArea");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Barcode", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BarCode"), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreateDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PriceTypeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Type"), "int");
			b.HasKey(new string[1] { "BarCode" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			b.HasIndex(new string[2] { "PrdGuid", "Ordinal" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Barcode");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CusCard", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CardName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CardNo").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CusGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateExpire"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateStart"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pass").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CusGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CusCard");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Customer", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address1").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Address2").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AreaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ChequeBargashtiValidation"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CodeMeli").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Company").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("CustType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DefPrice"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("EghtesadiNo").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Email"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Etebar"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("EtebarCheck"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Family").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("IdPath"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("InPlacePayment"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsBazaryab"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEtebari"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSupervisor"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Job").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Location").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("OrderTasvieTimeValidation"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Password").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PhoneNumbers"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicFace").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicSign").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("RoleGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("SendToServerDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SupervisorGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TafsilLevel"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TaxType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TitleGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("User"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Zip").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "AreaGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "Email" }).IsUnique(true), "Email IS NOT NULL");
			b.HasIndex(new string[1] { "GroupGuid" });
			b.HasIndex(new string[1] { "RoleGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TitleGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "User" }).IsUnique(true), "[User] IS NOT NULL");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Customer");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CustomerGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinDefaultGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinDefaultGuid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CustomerTitle");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Depot", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("NegativeAmount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Depot");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.OrderDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AvarezPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInQuantity"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DecimalName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalQt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DicountCouponDetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DiscountPercentString"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("FeePrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("GrouppingGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine1Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine3Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine4Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Hazine5Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("OrderDetState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("Price2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PriceTypeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ProductName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ProfitPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReferenceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RowSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerialzeData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Tax"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TaxPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalHazinePrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalVisitorPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice4"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "DicountCouponDetailGuid" });
			b.HasIndex(new string[1] { "GrouppingGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "PriceTypeGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "ReferenceGuid" });
			b.HasIndex(new string[1] { "VGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OrderDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtBank", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("BankRealAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("WageGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("WageMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("WagePrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("WageTafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "WageMoeinGuid" });
			b.HasIndex(new string[1] { "WageTafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtBank");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtCheckM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheck"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiName").IsRequired(true).ValueGeneratedOnAdd(), "nvarchar(100)"), "((''))").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiNationalCode"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SayadiRegistered"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("BankTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSaresid"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitA2reMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitA2reTafsiGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("PardakhtDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiIdentity").IsRequired(true).ValueGeneratedOnAdd(), "nvarchar(100)"), "((''))").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiName").IsRequired(true).ValueGeneratedOnAdd(), "nvarchar(100)"), "((''))").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiNationalCode"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SayadiRegistered"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "A2re_MoeinGuid" });
			b.HasIndex(new string[1] { "A2re_TafsilGuid" });
			b.HasIndex(new string[1] { "BankTafsilGuid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "DebitA2reMoeinGuid" });
			b.HasIndex(new string[1] { "DebitA2reTafsiGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtCheckSh");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtNaghd", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("A2re_ReceptionDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditA2reMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditA2reTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HesabNum").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Personal"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("PoshtNomreh"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiIdentity").IsRequired(true).ValueGeneratedOnAdd(), "nvarchar(100)"), "((''))").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<string>(RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiName").IsRequired(true).ValueGeneratedOnAdd(), "nvarchar(100)"), "((''))").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SayadiNationalCode"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SayadiRegistered"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Status"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("UserGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "A2re_PardazandeMoeinGuid" });
			b.HasIndex(new string[1] { "A2re_PardazandeTafsilGuid" });
			b.HasIndex(new string[1] { "CreditA2reMoeinGuid" });
			b.HasIndex(new string[1] { "CreditA2reTafsilGuid" });
			b.HasIndex(new string[1] { "DebitMoeinGuid" });
			b.HasIndex(new string[1] { "DebitTafsilGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			b.HasIndex(new string[1] { "VisitorGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[3] { "Status", "Aval2re", "A2re_PardazandeTafsilGuid" }), "IX_Acc_ReceptionCheque_Status_Aval2re_PardazandeTafsilGuid").HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionCheck");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionHavale", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("BankRealAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateTasviye"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MerchantId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MessageId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PAN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RRN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STAN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TerminalId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("WageGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("WageMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("WagePrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("WageTafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "WageMoeinGuid" });
			b.HasIndex(new string[1] { "WageTafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionHavale");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionNaghd", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Word"), "nvarchar(400)").HasMaxLength(400);
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id"), "int");
			b.HasKey(new string[1] { "Word" });
			b.HasIndex(new string[1] { "Id" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_FullTextWords");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.FullTextWords_temp", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Word").IsRequired(true), "nvarchar(400)").HasMaxLength(400);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_FullTextWords_temp");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalQt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnAsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KolGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("QT"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("QT2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Rem"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RowDesc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SanadDesc").IsRequired(true), "nvarchar(max)");
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.TempBarcodePrint", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Barcode"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DetailDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GuarantyCode"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("GuarantyEndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("GuarantyStartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("MasterDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MasterDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TempDescription"), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TempBarcodePrint");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Gps", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Accuracy"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Activity"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Altitude"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Bearing"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ColorHex"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Distance"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("FromLastPointDistance"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("FromStartPointDistance"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("GPSTime"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Icon"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Latitude"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Longitude"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PointDurationSeconds"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PreviewsID"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ReferencedId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Speed"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Time"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Invalidated"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[2] { "TafsilGuid", "PrdGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_LastPrice");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OpenInvoice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PeopleCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("SettlementDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "PardakhtGuid" });
			b.HasIndex(new string[1] { "ReceptionGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OpenInvoice");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OpenInvoiceOrderRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("InvoiceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			b.HasKey(new string[2] { "InvoiceGuid", "OrderGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OpenInvoiceOrderRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OpenInvoicePlacementRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("InvoiceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			b.HasKey(new string[2] { "InvoiceGuid", "PlacementGuid" });
			b.HasIndex(new string[1] { "PlacementGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OpenInvoicePlacementRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Order", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AfterAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("AghsatGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AndroidIme"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("AndroidImeLong"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("AttachmentNo"), "int");
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
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DeliveryGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("DeliveryStatus"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DiscountMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountRows"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DiscountTafsilGuid"), "uniqueidentifier");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("IntroductionMethodGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("KardexAffective"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Keraye"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PardakhtSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PaymentMethod"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PriceTypeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PrintCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("PrintDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ProductTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ReceptionSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SanadAffective"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SellerFactorNo"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumKala"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TasvieDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TasvieDateMoaser"), "datetime");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TransferDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserAccGuid"), "uniqueidentifier");
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
			b.HasIndex(new string[1] { "DiscountMoeinGuid" });
			b.HasIndex(new string[1] { "DiscountTafsilGuid" });
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
			b.HasIndex(new string[1] { "PriceTypeGuid" });
			b.HasIndex(new string[1] { "ProductMoeinGuid" });
			b.HasIndex(new string[1] { "ProductTafsilGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "ReceptionGuid" }).IsUnique(true), "[ReceptionGuid] IS NOT NULL");
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "TaxMoeinGuid" });
			b.HasIndex(new string[1] { "TaxTafsilGuid" });
			b.HasIndex(new string[1] { "UserAccGuid" });
			b.HasIndex(new string[2] { "FactorType", "Date" });
			b.HasIndex(new string[3] { "FactorType", "Id2", "Status" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Order");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OrderAbad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateMoaser"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HddSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsRead"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumBank"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumCheckM"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumCheckSh"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumEnteghal"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumNaghd"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TransferDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Id2" }).IsUnique(true);
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Pardakht");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.PlacementOrderRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "PlacementGuid", "OrderGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PlacementOrderRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.PrdAnbarRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AnbarGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Amount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Min"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[2] { "PrdGuid", "AnbarGuid" });
			b.HasIndex(new string[1] { "AnbarGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdAnbarRelation");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Product", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("AutoSellSt"), "bit").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BPercent1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BPercent2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BPercent3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("BPercent4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreateDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DecimalMetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DiscountEndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DiscountStartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullContent"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidVahed"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Height"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("IdProduct").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ImageList").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("IndexingType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsDependentOnCurrency"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSerial"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTax"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KardexPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Keywords"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Kind"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Length"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Mojodi"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("OrderPoint"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("OtherPics"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PayementType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pic").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("QuickText"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RegularPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ScaleSt"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Score"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("SendToServerDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Sku").IsRequired(true), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Slug").IsRequired(true), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tags"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TaxGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TolidFormulaGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V3Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V3Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V4Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V4Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Weight"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Width"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidVahed" });
			b.HasIndex(new string[1] { "IndexingType" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "Sku" }).IsUnique(true);
			b.HasIndex(new string[1] { "Slug" }).IsUnique(true);
			b.HasIndex(new string[1] { "TaxGuid" });
			b.HasIndex(new string[1] { "TolidFormulaGuid" });
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("FullCode"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HtmlFull"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HtmlQuick"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Level"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pic").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ShortName").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Slug").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TechInfoData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			b.HasIndex(new string[1] { "Slug" }).IsUnique(true);
			b.HasIndex(new string[1] { "Status" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.ProductVahed", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("DecimalSales"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TaxUnitGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductVahed");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.RasGiri", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("BaseDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DayCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sood_Percent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sood_Total"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Total_Sum"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_RasGiri");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.RasGiriDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Enheraf"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateMoaser"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime?>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateUtc").IsRequired(true).ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool?>(b.Property<bool?>("DeliveryStatus"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DependencyType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HddSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsChecked"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LkSerial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("OrderType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PaymentMethod"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumBank"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumCheck"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumEnteghal"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumNaghd"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TransferDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Date" });
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateSarresid"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Debit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid4"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "AsGuid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "TafsilGuid2" });
			b.HasIndex(new string[1] { "TafsilGuid3" });
			b.HasIndex(new string[1] { "TafsilGuid4" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanads", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id").ValueGeneratedOnAdd(), "int").HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "SanadNo" }).IsUnique(true);
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Sanad");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.SuggestedBank", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AccountNo").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ShobeCode").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "BankName" }).IsUnique(true);
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
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
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.BillDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "PlacementGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BillDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.BillMaster", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNumber"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "PardakhtGuid" });
			b.HasIndex(new string[1] { "ReceptionGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_BillMaster");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ExtraPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ItemCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Placement");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.TempBill", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "OrderGuid", "PlacementGuid" });
			b.HasIndex(new string[1] { "PlacementGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TempBill");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Coding.AccountBallance", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("GardeshHesab"), "varbinary(max)");
			b.HasKey(new string[2] { "MoeinGuid", "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AccountBallance");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Coding.AccountClass", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Account"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[2] { "MoeinGuid", "TafsilGuid" });
			b.HasIndex(new string[1] { "Guid" }).IsUnique(true);
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Accounts");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Coding.AccountLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Action"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DebuggerInfo"), "nvarchar(1000)").HasMaxLength(1000);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("NewAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("OldAccount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_AccountLog");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Crm.CallingLogDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Uuid"), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CallFrom_Name"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CallFrom_Number"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CallTo_Name"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CallTo_Number"), "nvarchar(15)").HasMaxLength(15);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateInsert"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("HangUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LinkedId"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("PickUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Rings"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TicketGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			b.HasKey(new string[1] { "Uuid" }).HasAnnotation("SqlServer:Clustered", (object)true);
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CallingLogDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass.DepotReceipt", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BarnameNo"), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CarNumber"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DriverName"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DriverPhone"), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTemp"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RefrencedNumber"), "nvarchar(150)").HasMaxLength(150);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[2] { "Type", "Id2" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_DepotReceipt");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass.DepotReceiptDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReferenceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("V2Quantity"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "ReferenceGuid" }).IsUnique(true), "ReferenceGuid IS NOT NULL");
			b.HasIndex(new string[1] { "V2Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_DepotReceiptDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscount", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("CountCouponUse"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("CountCouponUseForUser"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateFrom"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateTo"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("SingleUse"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CouponDiscount");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AmountCoupon"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("CustomerContains"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EnDiscountType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("GardeshAffectiveDays"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("GardeshEndPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("GardeshStartPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("GiftGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("MaxBuy"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("MaxDiscountPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("MinBuy"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ProductContains"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GiftGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CouponDiscountDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountEntityRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CouponDetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("EntityType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[3] { "CouponDetailGuid", "EntityType", "EntityGuid" }).HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CouponDiscountEntityRelation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Factor.SanadStatusLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("NewStatus"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("OldStatus"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "NewStatus" });
			b.HasIndex(new string[1] { "OldStatus" });
			b.HasIndex(new string[1] { "OrderGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadStatusLog");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Discription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Titel"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TypeClass"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Gift");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.CustomerReceiptRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CustomerGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ReceiptGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "CustomerGuid", "ReceiptGuid" });
			b.HasIndex(new string[1] { "ReceiptGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Hotel_CustomerReceiptRelation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Facilities", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Facilities");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Hall", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Hall");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.PlacementFacilitiesRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("FacilitiesGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "PlacementGuid", "FacilitiesGuid" });
			b.HasIndex(new string[1] { "FacilitiesGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PlacementFacilitiesRelation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Receipt", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CustomerGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Penalty"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PepolCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PresenterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CustomerGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "PresenterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Hotel_Receipt");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.ReceiptDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AvarezPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("EndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ExtraCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ExtraPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Penalty"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("PricePerNight"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("StartDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Tax"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TaxPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TotalPrice"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "PlacementGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Hotel_ReceiptDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.ServiceReceiptDetailRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ReceiptDetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ServiceGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AvarezPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("AvarezPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TaxPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("TaxPrice"), "decimal(18, 3)");
			b.HasKey(new string[2] { "ReceiptDetailGuid", "ServiceGuid" });
			b.HasIndex(new string[1] { "ServiceGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Hotel_ServiceReceiptDetailRelation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht.PardakhtChequeEntezami", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ChequeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("PardakhtDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			b.HasKey(new string[1] { "ChequeGuid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "DebitMoeinGuid" });
			b.HasIndex(new string[1] { "DebitTafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PardakhtChequeEntezami");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception.ReceptionChequeEntezami", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ChequeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ReceptionDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			b.HasKey(new string[1] { "ChequeGuid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "CreditMoeinGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "DebitMoeinGuid" });
			b.HasIndex(new string[1] { "DebitTafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionChequeEntezami");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass.OrderDetailCouponCalculation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("RefrencedId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DicountCouponDetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RowSum"), "decimal(18, 3)");
			b.HasKey(new string[2] { "RefrencedId", "Guid" });
			b.HasIndex(new string[1] { "DicountCouponDetailGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_OrderDetailCouponCalculation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass.TaxOrderLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BaseTaxOrderId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("InsertDate").ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("InvoiceSeries"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSystem"), "bit");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<DateTime>(RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified").ValueGeneratedOnAdd(), "datetime"), "getdate()");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("NewState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("OldState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Subject"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TaxDeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TaxOrderId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TaxRefrenceNumber"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TaxResultErrorCode"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TaxResultMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TaxSendDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TaxUid"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "OrderGuid" }), "IX_Acc_TaxOrderLog_OrderGuid").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TaxOrderLog");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes.ProductAttributeValue", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[2] { "MasterGuid", "Name" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductAttributeValue");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductDecimalRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Amount"), "decimal(18, 3)");
			b.HasKey(new string[2] { "ProductGuid", "DecimalGuid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductPrices");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductPriceUpdate", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RegularPrice"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductPriceUpdate");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceLevel", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountCoefficient"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsDefault"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSystem"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[2] { "Name", "Type" }).IsUnique(true);
			b.HasIndex(new string[2] { "Ordinal", "Status" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductPriceLevel");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KasrRial"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PriceLevelGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Profit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("UnitGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "PriceLevelGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "UnitGuid" });
			b.HasIndex(new string[2] { "ProductGuid", "UnitGuid" }).HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ProductPriceRelation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass.Reservation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "Number" }).IsUnique(true);
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "ReceptionGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Reservation");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass.ReservationDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("EndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("OrderGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PeopleCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PlacementGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("StartDate"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "OrderGuid" });
			b.HasIndex(new string[1] { "PlacementGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReservationDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad.SanadArchives", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("Data"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadArchives");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad.SanadStatusReportUpdates", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("StatusId"), "int");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "StatusId" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[3] { "AsGuid", "Ordinal", "AsType" }), "IX_Acc_SanadStatusReportUpdates_AsGuid_Ordinal_AsType").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadStatusReportUpdates");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad.ViewSanadDetailCover", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("AsType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsTypeDocument"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Credit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DateMoaser"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Debit"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalQt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id2"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RBazaryab"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RDiscount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SanadDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid2"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid3"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid4"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VahedGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "CreditTafsilGuid" });
			b.HasIndex(new string[1] { "DebitTafsilGuid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "ProductGuid" });
			b.HasIndex(new string[1] { "TafsilGuid2" });
			b.HasIndex(new string[1] { "TafsilGuid3" });
			b.HasIndex(new string[1] { "TafsilGuid4" });
			b.HasIndex(new string[1] { "VahedGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "AsGuid", "AsTypeDocument" }), "IX_Acc_ViewSanadDetailCover_AsGuid_AsTypeDocument").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[3] { "CreditMoeinGuid", "CreditTafsilGuid", "Date" }), "IX_Acc_ViewSanadDetailCover_CreditItems").HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[3] { "DebitMoeinGuid", "DebitTafsilGuid", "Date" }), "IX_Acc_ViewSanadDetailCover_DebitItems").HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewSanadDetailCover");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.SanadStatusReport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ErrorMinutes"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long?>(b.Property<long?>("Id2From"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long?>(b.Property<long?>("Id2To"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(250)").HasMaxLength(250);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WarningMinutes"), "int");
			b.HasKey(new string[1] { "Id" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "Guid" }).IsUnique(true), "IX_Acc_SanadStatusReport_Guid");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadStatusReport");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup.ProductGroupSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdGroupSelected");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup.ProductGroupSelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdGroupSelectedDetails");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FilePath"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Priod"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("State"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Year"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TTMS");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMSDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AsGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AsType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TTMSDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Tax.TaxClass", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Slug").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			b.HasIndex(new string[1] { "Slug" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tax");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Tax.TaxClassDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("FeeTaxPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ProductTaxPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ProfitTaxPercent"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TaxDetail");
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Customer.CustomerRole", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PriceTypeGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("WordPressName"), "nvarchar(500)").HasMaxLength(500);
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "PriceTypeGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_CustomerRole");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.AbadDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.OrderAbad", "Master").WithMany("OrderAbads").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("CreditAghsat").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("CreditAghsat").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM_Group", "Groups").WithMany("AghsatAshkhas").HasForeignKey(new string[1] { "GuidGroup" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "Receptions").WithMany("AghsatAshkhas").HasForeignKey(new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Aghsat.AghsatM_Group", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("AghsatGroup").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("CreditAghsatGroup").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("CreditAghsatGroup").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moeins").WithMany("AghsatGroup").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsils").WithMany("AghsatGroup").HasForeignKey(new string[1] { "TafsilGuid" })
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Customer").WithMany("Cashiers").HasForeignKey(new string[1] { "CusGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "FareMoeinItem").WithMany("DefaultFareOrder").HasForeignKey(new string[1] { "FareMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "FareTafsilItem").WithMany("DefaultFareOrder").HasForeignKey(new string[1] { "FareTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.UserAcc", "UserItem").WithMany("Cashiers").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.CashiersLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("CashiersLogs").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeBook", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("ChequeBooks").HasForeignKey(new string[1] { "BankGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeMOperation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("ChequeMOperation").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("ChequeMOperationCredit").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("ChequeMOperationCredit").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitMoein").WithMany("ChequeMOperationDebit").HasForeignKey(new string[1] { "DebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitTafsil").WithMany("ChequeMOperationDebit").HasForeignKey(new string[1] { "DebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Details.ReceptionCheck", "Cheque").WithMany("ChequeMOperation").HasForeignKey(new string[1] { "GuidCheque" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Cheque.ChequeShOperation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("ChequeShOperation").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Details.PardakhtCheckSh", "Cheque").WithMany("ChequeShOperation").HasForeignKey(new string[1] { "ChequeGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("ChequeShOperationCredit").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("ChequeShOperationCredit").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitMoein").WithMany("ChequeShOperationDebit").HasForeignKey(new string[1] { "DebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitTafsil").WithMany("ChequeShOperationDebit").HasForeignKey(new string[1] { "DebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
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
				.OnDelete((DeleteBehavior)5);
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
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Note.NoteGroup", "Group").WithMany("Notes").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Notes").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdAnbarGardani", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "EzafeCreditMoein").WithMany("EzafeCredit").HasForeignKey(new string[1] { "EzafeCreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "EzafeCreditTafsil").WithMany("EzafeCredit").HasForeignKey(new string[1] { "EzafeCreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "EzafeDebitMoein").WithMany("EzafeDebit").HasForeignKey(new string[1] { "EzafeDebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "EzafeDebitTafsil").WithMany("EzafeDebit").HasForeignKey(new string[1] { "EzafeDebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "KasrCreditMoein").WithMany("KasrCredit").HasForeignKey(new string[1] { "KasrCreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "KasrCreditTafsil").WithMany("KasrCredit").HasForeignKey(new string[1] { "KasrCreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "KasrDebitMoein").WithMany("KasrDebit").HasForeignKey(new string[1] { "KasrDebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "KasrDebitTafsil").WithMany("KasrDebit").HasForeignKey(new string[1] { "KasrDebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSerial", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("Serials").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdSerialDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.PrdSerial", "MasterItem").WithMany("DetailItem").HasForeignKey(new string[1] { "MasterGuid" })
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
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes.ProductAttributeValue", "ProductAttributeValue").WithMany("ProductAttributeRelations").HasForeignKey(new string[1] { "AttributeValueGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ProductAttributeRelations").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Resid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("Resid").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("Resid").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("Resid").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "Reception").WithMany("Resid").HasForeignKey(new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Resid").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Selection.ProductGroupRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.ProductGroup", "Group").WithMany("ProductGroupRelations").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ProductGroupRelations").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ShortCut", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("ShortCuts").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)3)
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("Tolid").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MahsolMoein").WithMany("Mahsol").HasForeignKey(new string[1] { "MahsolMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "MahsolTafsil").WithMany("Mahsol").HasForeignKey(new string[1] { "MahsolTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MavadMoein").WithMany("Mavad").HasForeignKey(new string[1] { "MavadMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "MavadTafsil").WithMany("Mavad").HasForeignKey(new string[1] { "MavadTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidExport").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid", "Master").WithMany("TolidExport").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidExport").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidExport").HasForeignKey(new string[1] { "V2Guid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormulaExport", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormula", "Master").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidFormulaExport").HasForeignKey(new string[1] { "V2Guid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormulaHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormula", "Master").WithMany("TolidFormulaHaizne").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("TolidFormulaHaizne").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("TolidFormulaHaizne").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormulaMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormula", "Master").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidFormulaMavad").HasForeignKey(new string[1] { "V2Guid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidHazine", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid", "Master").WithMany("TolidHazine").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("TolidHazine").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("TolidHazine").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidMavad", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "Depot").WithMany("TolidMavad").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.Tolid", "Master").WithMany("TolidMavad").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("TolidMavad").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "Vahed").WithMany("TolidMavad").HasForeignKey(new string[1] { "V2Guid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidSummary", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("TolidSummary").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidSummaryDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "DepotItem").WithMany("TolidSummaryDetail").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidSummary", "MasterItem").WithMany("TolidSummaryDetail").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("TolidSummaryDetail").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TozieLists.TozieList", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("TozieLists").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TozieLists.TozieListDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.TozieLists.TozieList", "MasterItem").WithMany("DetailList").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Vam", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ViewSanadDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("SanadDetails").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DecimalItem").WithMany("SanadDetails").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Kol", "KolItem").WithMany("SanadDetails").HasForeignKey(new string[1] { "KolGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItem").WithMany("SanadDetails").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("SanadDetails").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil2").WithMany("SanadDetails2").HasForeignKey(new string[1] { "TafsilGuid2" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil3").WithMany("SanadDetails3").HasForeignKey(new string[1] { "TafsilGuid3" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil4").WithMany("SanadDetails4").HasForeignKey(new string[1] { "TafsilGuid4" })
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CusCard", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Customer", "Customers").WithMany("CusCard").HasForeignKey(new string[1] { "CusGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Customer", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.VisitorArea", "VisitorArea").WithMany("Customers").HasForeignKey(new string[1] { "AreaGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.CustomerGroup", "CusGroup").WithMany("Customers").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithOne("Customer").HasForeignKey("AccSqlServerPersistence.Entities.Customer", new string[1] { "Guid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Customer.CustomerRole", "Role").WithMany("Customers").HasForeignKey(new string[1] { "RoleGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.CustomerTitle", "CusTitle").WithMany("Customers").HasForeignKey(new string[1] { "TitleGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CustomerGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "MoeinItems").WithMany("CustomerGroups").HasForeignKey(new string[1] { "MoeinDefaultGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
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
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountDetail", "Coupon").WithMany("OrderDetails").HasForeignKey(new string[1] { "DicountCouponDetailGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Details.OrderDetail", "Group").WithMany("GrouppedDetail").HasForeignKey(new string[1] { "GrouppingGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("OrderDetailItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceLevel", "ProductPriceLevel").WithMany("OrderDetail").HasForeignKey(new string[1] { "PriceTypeGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("OrderDetails").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Details.OrderDetail", "Reference").WithMany("ReferenceDetail").HasForeignKey(new string[1] { "ReferenceGuid" })
				.OnDelete((DeleteBehavior)5);
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "WageMoeinItem").WithMany("PardakhtBankWage").HasForeignKey(new string[1] { "WageMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "WageTafsilItem").WithMany("PardakhtBankWage").HasForeignKey(new string[1] { "WageTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.PardakhtCheckM", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("PardakhtCheckMCredit").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("PardakhtCheckMCredit").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "BankTafsilItem").WithMany("PardakhtCheckShBank").HasForeignKey(new string[1] { "BankTafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("PardakhtCheckShCredit").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("PardakhtCheckShCredit").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitA2reMoein").WithMany("PardakhtCheckShDebitA2re").HasForeignKey(new string[1] { "DebitA2reMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitA2reTafsil").WithMany("PardakhtCheckShDebitA2re").HasForeignKey(new string[1] { "DebitA2reTafsiGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithMany("PardakhtCheckShItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("CashierPardakhtCheckSh").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5);
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoein").WithMany("CreditReceptionCheck").HasForeignKey(new string[1] { "CreditA2reMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsil").WithMany("CreditReceptionCheck").HasForeignKey(new string[1] { "CreditA2reTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitMoein").WithMany("DebitReceptionCheck").HasForeignKey(new string[1] { "DebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitTafsil").WithMany("DebitReceptionCheck").HasForeignKey(new string[1] { "DebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithMany("ReceptionCheckMItem").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("CashierReceptionCheck").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "VisitorTafsil").WithMany("VisitorReceptionCheck").HasForeignKey(new string[1] { "VisitorGuid" })
				.OnDelete((DeleteBehavior)5);
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "WageMoeinItem").WithMany("ReceptionHavaleWage").HasForeignKey(new string[1] { "WageMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "WageTafsilItem").WithMany("ReceptionHavaleWage").HasForeignKey(new string[1] { "WageTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Global.TempBarcodePrint", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("TempBarcodePrint").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("TempBarcodePrint").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxCustomerAllFields", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxCustomerAllFields").HasForeignKey(new string[1] { "WordId" }), "Fk_IxCustomerAllFields_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT1Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT1Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT1Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT2Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT2Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT2Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT3Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT3Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT3Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT4Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT4Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT4Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT5Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT5Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT5Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT6Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT6Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT6Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT7Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT7Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT7Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT8Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalForeignKeyBuilderExtensions.HasConstraintName(b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT8Field").HasForeignKey(new string[1] { "WordId" }), "Fk_IxProductT8Field_FullTextWords_WordId").HasPrincipalKey(new string[1] { "Id" }).OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.LastPrice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("LastPrice").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("LastPrice").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OpenInvoice", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("OpenInvoices").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "PardakhtItem").WithMany("OpenInvoices").HasForeignKey(new string[1] { "PardakhtGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithMany("OpenInvoices").HasForeignKey(new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("OpenInvoices").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OpenInvoiceOrderRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.OpenInvoice", "MasterItem").WithMany("OrderRelation").HasForeignKey(new string[1] { "InvoiceGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("OrderRelation").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.OpenInvoicePlacementRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.OpenInvoice", "MasterItem").WithMany("PlacementRelation").HasForeignKey(new string[1] { "InvoiceGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "PlacementItem").WithMany("PlacementRelation").HasForeignKey(new string[1] { "PlacementGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DiscountMoeinItem").WithMany("DiscountOrder").HasForeignKey(new string[1] { "DiscountMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DiscountTafsilItem").WithMany("DiscountOrder").HasForeignKey(new string[1] { "DiscountTafsilGuid" })
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
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceLevel", "ProductPriceLevel").WithMany("Order").HasForeignKey(new string[1] { "PriceTypeGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "ProductMoeinItem").WithMany("ProductOrder").HasForeignKey(new string[1] { "ProductMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "ProductTafsilItem").WithMany("ProductOrder").HasForeignKey(new string[1] { "ProductTafsilGuid" })
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.PlacementOrderRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("PlacementOrderRelation").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "PlacementItem").WithMany("PlacementOrderRelation").HasForeignKey(new string[1] { "PlacementGuid" })
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Product", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "VahedItem").WithMany("Products").HasForeignKey(new string[1] { "GuidVahed" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Tax.TaxClass", "TaxItem").WithMany("Products").HasForeignKey(new string[1] { "TaxGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tolid.TolidFormula", "TolidFormulaItem").WithMany("Products").HasForeignKey(new string[1] { "TolidFormulaGuid" })
				.OnDelete((DeleteBehavior)5);
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("SanadDet").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("SanadDet").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil2").WithMany("SanadDet2").HasForeignKey(new string[1] { "TafsilGuid2" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil3").WithMany("SanadDet3").HasForeignKey(new string[1] { "TafsilGuid3" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil4").WithMany("SanadDet4").HasForeignKey(new string[1] { "TafsilGuid4" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanads", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("SanadItems").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.BillDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.BillMaster", "BillMaster").WithMany("BillDetail").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("BillDetail").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "Placement").WithMany("BillDetails").HasForeignKey(new string[1] { "PlacementGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.BillMaster", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("BillMaster").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Pardakht", "Pardakht").WithMany("BillMaster").HasForeignKey(new string[1] { "PardakhtGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "Reception").WithMany("BillMaster").HasForeignKey(new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.ProductGroup", "ProductGroup").WithMany("Placements").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Hall", "Master").WithMany("Placement").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.TempBill", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("TempBills").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "Placement").WithMany("TempBills").HasForeignKey(new string[1] { "PlacementGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Coding.AccountClass", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("Accounts").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Accounts").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Coding.AccountLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("AccountLogs").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("AccountLogs").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Crm.CallingLogDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("CallingLogDetail").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLog", "Master").WithMany("CallingLogDetail").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass.DepotReceipt", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("DepotReceipts").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("DepotReceipts").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass.DepotReceiptDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "DepotItem").WithMany("DepotReceiptDetail").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass.DepotReceipt", "MasterItem").WithMany("DepotReceiptDetail").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("DepotReceiptDetail").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.ViewSanadDetail", "ReferenceItem").WithMany("DepotReceiptDetail").HasForeignKey(new string[1] { "ReferenceGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "VahedItem").WithMany("DepotReceiptDetail").HasForeignKey(new string[1] { "V2Guid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscount", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("CouponDiscounts").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Gift.Gift", "Gift").WithMany("CouponDiscountDetail").HasForeignKey(new string[1] { "GiftGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscount", "Master").WithMany("CouponDiscountDetails").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountEntityRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountDetail", "Coupon").WithMany("CouponDiscountEntityRelation").HasForeignKey(new string[1] { "CouponDetailGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Factor.SanadStatusLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "Cashier").WithMany("SanadStatusLog").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.SanadStatusReport", "SanadStatusReportNew").WithMany("SanadStatusLogNew").HasForeignKey(new string[1] { "NewStatus" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.SanadStatusReport", "SanadStatusReportOld").WithMany("SanadStatusLogOld").HasForeignKey(new string[1] { "OldStatus" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", (string)null).WithMany("OrderStatusLog").HasForeignKey(new string[1] { "OrderGuid" });
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.CustomerReceiptRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Customer", "Customer").WithMany("CustomerReceipt").HasForeignKey(new string[1] { "CustomerGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Receipt", "Receipt").WithMany("CustomerReceipt").HasForeignKey(new string[1] { "ReceiptGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.PlacementFacilitiesRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Facilities", "Facility").WithMany("PlacementFacilities").HasForeignKey(new string[1] { "FacilitiesGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "Room").WithMany("PlacementFacilities").HasForeignKey(new string[1] { "PlacementGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Receipt", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Receipts").HasForeignKey(new string[1] { "CustomerGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "Order").WithMany("Receipts").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "PresenterTafsil").WithMany("ReceiptsPresentor").HasForeignKey(new string[1] { "PresenterGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.ReceiptDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.Receipt", "Master").WithMany("ReceiptDetails").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "placement").WithMany("ReciptDetails").HasForeignKey(new string[1] { "PlacementGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.ServiceReceiptDetailRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel.ReceiptDetail", "ReceiptDetail").WithMany("ServiceReceiptDetail").HasForeignKey(new string[1] { "ReceiptDetailGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ServiceRecieptDetail").HasForeignKey(new string[1] { "ServiceGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht.PardakhtChequeEntezami", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("PardakhtChequeEntezami").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Details.PardakhtCheckSh", "ChequeItem").WithOne("PardakhtChequeEntezami").HasForeignKey("SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Pardakht.PardakhtChequeEntezami", new string[1] { "ChequeGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoeinItem").WithMany("PardakhtChequeEntezamiCredit").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsilItem").WithMany("PardakhtChequeEntezamiCredit").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitMoeinItem").WithMany("PardakhtChequeEntezamiDebit").HasForeignKey(new string[1] { "DebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitTafsilItem").WithMany("PardakhtChequeEntezamiDebit").HasForeignKey(new string[1] { "DebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception.ReceptionChequeEntezami", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ReceptionChequeEntezami").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Details.ReceptionCheck", "ChequeItem").WithOne("ReceptionChequeEntezami").HasForeignKey("SharedAccEntityFramworkPersistence.Entities.Accounting.Khazane.Reception.ReceptionChequeEntezami", new string[1] { "ChequeGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoeinItem").WithMany("ReceptionChequeEntezamiCredit").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsilItem").WithMany("ReceptionChequeEntezamiCredit").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitMoeinItem").WithMany("ReceptionChequeEntezamiDebit").HasForeignKey(new string[1] { "DebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitTafsilItem").WithMany("ReceptionChequeEntezamiDebit").HasForeignKey(new string[1] { "DebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass.OrderDetailCouponCalculation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon.CouponDiscountDetail", "Coupon").WithMany("OrderDetailCouponCalculation").HasForeignKey(new string[1] { "DicountCouponDetailGuid" });
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("OrderDetailCouponCalculation").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass.TaxOrderLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("TaxOrderLog").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes.ProductAttributeValue", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.ProductAttribute", "MasterItem").WithMany("ProductAttributeValues").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductDecimalRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "Decimal").WithMany("ProductPrices").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ProductPrices").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ProductPriceUpdate", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("Updates").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceLevel", "PriceLevelItem").WithMany("ProductPriceRelation").HasForeignKey(new string[1] { "PriceLevelGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("ProductPriceRelation").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "UnitItem").WithMany("ProductPriceRelation").HasForeignKey(new string[1] { "UnitGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass.Reservation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("Reservation").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("Reservation").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Reception", "ReceptionItem").WithMany("Reservation").HasForeignKey(new string[1] { "ReceptionGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "TafsilItem").WithMany("Reservation").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass.ReservationDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.ReservationClass.Reservation", "MasterItem").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Order", "OrderItem").WithMany("ReservationDetail").HasForeignKey(new string[1] { "OrderGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Bill.Placement", "PlacementItem").WithMany("ReservationDetail").HasForeignKey(new string[1] { "PlacementGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad.SanadStatusReportUpdates", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("SanadStatusReportUpdates").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.SanadStatusReport", "StatusItem").WithMany("SanadStatusReportUpdates").HasForeignKey(new string[1] { "StatusId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad.ViewSanadDetailCover", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ViewSanadDetailCover").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "CreditMoeinItem").WithMany("CreditViewSanadDetailCover").HasForeignKey(new string[1] { "CreditMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "CreditTafsilItem").WithMany("CreditViewSanadDetailCover").HasForeignKey(new string[1] { "CreditTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "DebitMoeinItem").WithMany("DebitViewSanadDetailCover").HasForeignKey(new string[1] { "DebitMoeinGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "DebitTafsilItem").WithMany("DebitViewSanadDetailCover").HasForeignKey(new string[1] { "DebitTafsilGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.DecimalEntity", "DecimalItem").WithMany("ViewSanadDetailCover").HasForeignKey(new string[1] { "DecimalGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Depot", "DepotItem").WithMany("ViewSanadDetailCover").HasForeignKey(new string[1] { "DepotGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Product", "ProductItem").WithMany("ViewSanadDetailCover").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil2").WithMany("ViewSanadDetailCover2").HasForeignKey(new string[1] { "TafsilGuid2" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil3").WithMany("ViewSanadDetailCover3").HasForeignKey(new string[1] { "TafsilGuid3" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil4").WithMany("ViewSanadDetailCover4").HasForeignKey(new string[1] { "TafsilGuid4" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "VahedItem").WithMany("ViewSanadDetailCover").HasForeignKey(new string[1] { "VahedGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup.ProductGroupSelected", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("ProductGroupSelected").HasForeignKey(new string[1] { "CashierGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup.ProductGroupSelectedDetails", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.ProductGroup", "GroupItem").WithMany("ProductGroupSelectedDetails").HasForeignKey(new string[1] { "GroupGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Selection.ProductGroup.ProductGroupSelected", "Master").WithMany("ProductGroupSelectedDetails").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMSDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject.TTMS", "MasterItem").WithMany("DetailList").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Accounting.Tax.TaxClassDetail", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Tax.TaxClass", "MasterItem").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("SharedAccEntityFramworkPersistence.Entities.Customer.CustomerRole", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("SharedAccEntityFramworkPersistence.Entities.Accounting.Products.ProductPriceLevel", "ProductPriceLevel").WithMany("CustomerRole").HasForeignKey(new string[1] { "PriceTypeGuid" })
				.OnDelete((DeleteBehavior)5);
		});
	}
}
