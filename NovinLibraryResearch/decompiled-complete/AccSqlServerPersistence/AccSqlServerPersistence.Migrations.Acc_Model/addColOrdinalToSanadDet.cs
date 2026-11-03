using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Acc_Model;

[DbContext(typeof(Acc_ModelContext))]
[Migration("20221002120700_addColOrdinalToSanadDet")]
public class addColOrdinalToSanadDet : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<int>("Ordinal", "Acc_SanadDet", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)0, (string)null, (string)null, (bool?)null, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn("Ordinal", "Acc_SanadDet", "Dbo");
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
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Checked"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Count"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResid"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("TypePardakht"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
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
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ChangeStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResid"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ID2"), "int");
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
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("HashedPassword"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LoginAccess"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("SandoghGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Setting"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StrCashierTime"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("UserGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CusGuid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Cashier");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.CashiersLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Action"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("LogType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SanadGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ser_New"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ser_OLD"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("NewState"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("OldState"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNO"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("NewState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("OldState"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("PickUpDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Rings"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TicketGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Version"), "int");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "CalcedNumber" }), "IX_Acc_CallingLog_CalcedNumber");
			b.HasIndex(new string[1] { "CoGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[1] { "DateM" }), "IX_Acc_Calling_DateM").HasAnnotation("SqlServer:Clustered", (object)true);
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ModifiedM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("No"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RanandehName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Count"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_HesabType");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Kardex", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("Avarez"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DetailGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Fare"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ID2").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Id").ValueGeneratedOnAdd(), "int").HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Invalidated"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KhorojQT"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VisitorPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("VorodQT"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal?>(b.Property<decimal?>("validated"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "UserGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[4] { "ProductGuid", "DepotGuid", "DateM", "Ordinal" }), "IX_Acc_Kardex_prdGuid_DepotGuid_Date_Ordinal").HasAnnotation("SqlServer:Clustered", (object)true);
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
			b.HasKey(new string[2] { "MoeinGuid", "TafsilGroupGuid" });
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_MoeinTafsilLevelRelations");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.Note", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CashierGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSarResidM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DisplayState"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("EntityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short?>(b.Property<short?>("EntityType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Pirority"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PirorityName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Status"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StatusName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Text").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "GroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Note");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Note.NoteGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrCreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("KasrCreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("KasrDebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("KasrDebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KasrSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Number"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("QtEzafe"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("QtKasr"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("EndDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GarrantyCode"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(200)").HasMaxLength(200);
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasDefaultValueSql<short>(RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Ordinal").ValueGeneratedOnAdd(), "smallint"), "((0))");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_PrdSerialDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.PrdTechnical", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Code").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Value").IsRequired(true), "nvarchar(max)");
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Resid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Beyane"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Darkhast").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("GuidVahed"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("ID2"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Mandeh"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdColor").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdModel").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PrdSerial").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Problems").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("State"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
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
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_SanadPrint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sum"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TableRelationShip");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<byte[]>(b.Property<byte[]>("GardeshHesab"), "varbinary(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("HType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SumAghsat"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("System"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGroupGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Code" }).IsUnique(true);
			b.HasIndex(new string[1] { "TafsilGroupGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Tafsil");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BazaryabDefaultGuidData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Checked"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CodeMax"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CodeMin"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tbl"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CodeMax" }).IsUnique(true);
			b.HasIndex(new string[1] { "CodeMin" }).IsUnique(true);
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TafsilGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.TafsilNumber", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("OriginalNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("CalcedNumber"), "bigint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Settings"), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date1"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date2"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Zarib"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateBargashtORVosolM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateCheckM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSanadM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateToPlace").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HesabNum").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PardakhtGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PardazandeTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Personal"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Place").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("PoshtNomreh"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidSanad"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("IDSanad"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PardakhtDate"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PardakhtDesc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("RowDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" }).HasAnnotation("SqlServer:Clustered", (object)false);
			b.HasIndex(new string[1] { "CashierGuid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "KolGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "SanadNo" });
			b.HasIndex(new string[1] { "TafsilGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "AsGuid", "AsType" }), "IX_AcC_ViewSanadDetail_AsGuid_AsType").HasAnnotation("SqlServer:Clustered", (object)true);
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ViewSanadDetail");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.VisitorArea", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Code"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("FullCode"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Level"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_VisitorArea");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Barcode", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BarCode"), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("CreateDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DepotId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("ExpireDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PriceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Type"), "int");
			b.HasKey(new string[1] { "BarCode" });
			b.HasIndex(new string[1] { "DepotGuid" });
			b.HasIndex(new string[1] { "PrdGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Barcode");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.CusCard", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CardNO").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CardName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CusCode").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CusGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CusName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateExpire"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateStart"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pass").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STDesc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("STName").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Password").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicFace").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PicSign").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Manfi"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ChangeInQuantity"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DecimalName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalQt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DecimalSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepotGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Discount"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PriceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ProductName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Quantity"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("RowSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Tax"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CreditMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidCheck"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitA2reMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitA2reTafsiGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("PardakhtDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("SanadNo"), "int");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("A2re_ReceptionDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Aval2re"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("BankName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditA2reMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("CreditA2reTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DebitMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DebitTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HesabNum").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsEnteghali"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Personal"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("PoshtNomreh"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int?>(b.Property<int?>("SanadNo"), "int");
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
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_ReceptionCheck");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Details.ReceptionHavale", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateTasviye"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("DecimalGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullDescription"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MerchantId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MessageId"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("PAN"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Date"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Invalidated"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Ip"), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ProductMoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ProductTafsilGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("ReceptionGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("ReceptionSum"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SellerFactorNo"), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("TransferDate"), "datetime");
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
			b.HasIndex(new string[1] { "ProductMoeinGuid" });
			b.HasIndex(new string[1] { "ProductTafsilGuid" });
			RelationalIndexBuilderExtensions.HasFilter(b.HasIndex(new string[1] { "ReceptionGuid" }).IsUnique(true), "[ReceptionGuid] IS NOT NULL");
			b.HasIndex(new string[1] { "TafsilGuid" });
			b.HasIndex(new string[1] { "TaxMoeinGuid" });
			b.HasIndex(new string[1] { "TaxTafsilGuid" });
			b.HasIndex(new string[1] { "UserAccGuid" });
			b.HasIndex(new string[2] { "FactorType", "Date" });
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName").IsRequired(true), "nvarchar(max)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.PrdAnbarRelation", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PrdGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AnbarGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Amount"), "decimal(18, 3)");
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("DiscountPercent"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullContent"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidGroup"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GuidVahed"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Height"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("IdProduct").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ImageList").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSerial"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsTax"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("KardexPrice"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Keywords"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("MetaDesc"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Mojodi"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("OrderPoint"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("OtherPics"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("PartitionId"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("PayementType"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Pic").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("QuickText"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("ScaleSt"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Score"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell1Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell2Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell3Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell4Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell5"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell5Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell6"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell6Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell7"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell7Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell8"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Sell8Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr5"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr6"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr7"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellKasr8"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood1"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood2"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood3"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood4"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood5"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood6"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood7"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("SellSood8"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Sku").IsRequired(true), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Slug").IsRequired(true), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T1").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T2").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T3").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T4").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T5").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T6").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T7").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("T8").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Tags"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V2Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V2Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V3Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V3Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("V4Guid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("V4Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Weight"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Width"), "decimal(18, 3)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "GuidGroup" });
			b.HasIndex(new string[1] { "GuidVahed" });
			b.HasIndex(new string[1] { "IdProduct" });
			b.HasIndex(new string[1] { "ServerStatus" });
			b.HasIndex(new string[1] { "Sku" }).IsUnique(true);
			b.HasIndex(new string[1] { "Slug" }).IsUnique(true);
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Slug").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Special"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TechInfoData"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "Name" }).IsUnique(true);
			b.HasIndex(new string[1] { "Slug" }).IsUnique(true);
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(450)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateSabt"), "datetime");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MoeinGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Ordinal"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Qt"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Serial"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid?>(b.Property<Guid?>("TafsilGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "AsGuid" });
			b.HasIndex(new string[1] { "DecimalGuid" });
			b.HasIndex(new string[1] { "MoeinGuid" });
			b.HasIndex(new string[1] { "TafsilGuid" });
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("SanadNo"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadSt"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SanadType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Source"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Customer").WithMany("Cashiers").HasForeignKey(new string[1] { "CusGuid" })
				.OnDelete((DeleteBehavior)5);
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Crm.CallingLine", "Line").WithMany("Logs").HasForeignKey(new string[1] { "CoGuid" })
				.OnDelete((DeleteBehavior)5);
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
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("ProductAttributeRelations").HasForeignKey(new string[1] { "ProductGuid" })
				.OnDelete((DeleteBehavior)3)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.Resid", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Product", "Product").WithMany("Resid").HasForeignKey(new string[1] { "PrdGuid" })
				.OnDelete((DeleteBehavior)5);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("Resid").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxCustomerAllFields", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxCustomerAllFields").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT1Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT1Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT2Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT2Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT3Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT3Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT4Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT4Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT5Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT5Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT6Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT6Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT7Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT7Field").HasForeignKey(new string[1] { "WordId" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT8Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.FullTextWords", "FullTextWords").WithMany("IxProductT8Field").HasForeignKey(new string[1] { "WordId" })
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
			b.HasOne("AccSqlServerPersistence.Entities.ProductGroup", "GroupItem").WithMany("Products").HasForeignKey(new string[1] { "GuidGroup" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.ProductVahed", "VahedItem").WithMany("Products").HasForeignKey(new string[1] { "GuidVahed" })
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
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Moein", "Moein").WithMany("SanadDet").HasForeignKey(new string[1] { "MoeinGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Tafsil", "Tafsil").WithMany("SanadDet").HasForeignKey(new string[1] { "TafsilGuid" })
				.OnDelete((DeleteBehavior)5);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Sanads", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.Cashier", "CashierItem").WithMany("SanadItems").HasForeignKey(new string[1] { "UserGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
	}
}
