using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Adv_Model;

[DbContext(typeof(Adv_ModelContext))]
[Migration("20211225102623_Add_new_Fields_To_Tables")]
public class Add_new_Fields_To_Tables : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateIndex("IX_Adv_VisitorCity_CityGuid", "Adv_VisitorCity", "CityGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Adv_VisitorCity_VisitorGuid", "Adv_VisitorCity", "VisitorGuid", "Dbo", false, (string)null);
		migrationBuilder.AddForeignKey("FK_Adv_VisitorCity_Adv_City_CityGuid", "Adv_VisitorCity", "CityGuid", "Adv_City", "Dbo", "Dbo", "Guid", (ReferentialAction)0, (ReferentialAction)0);
		migrationBuilder.AddForeignKey("FK_Adv_VisitorCity_Adv_Visitor_VisitorGuid", "Adv_VisitorCity", "VisitorGuid", "Adv_Visitor", "Dbo", "Dbo", "Guid", (ReferentialAction)0, (ReferentialAction)0);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey("FK_Adv_VisitorCity_Adv_City_CityGuid", "Adv_VisitorCity", "Dbo");
		migrationBuilder.DropForeignKey("FK_Adv_VisitorCity_Adv_Visitor_VisitorGuid", "Adv_VisitorCity", "Dbo");
		migrationBuilder.DropIndex("IX_Adv_VisitorCity_CityGuid", "Adv_VisitorCity", "Dbo");
		migrationBuilder.DropIndex("IX_Adv_VisitorCity_VisitorGuid", "Adv_VisitorCity", "Dbo");
	}

	protected override void BuildTargetModel(ModelBuilder modelBuilder)
	{
		RelationalModelBuilderExtensions.HasDefaultSchema(modelBuilder, "Dbo").HasAnnotation("ProductVersion", (object)"3.1.10").HasAnnotation("Relational:MaxIdentifierLength", (object)128)
			.HasAnnotation("SqlServer:ValueGenerationStrategy", (object)(SqlServerValueGenerationStrategy)2);
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.AdvCategory", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Category");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.AdvDepartment", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ChatCityData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("ChatCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ChatData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Description"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsSendChat"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title"), "nvarchar(250)").HasMaxLength(250);
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Department");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.AdvGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("GroupName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Group");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.AdvTokens", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Number"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Token"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Tokens");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.Advertise", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ContentList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DivatCategoryList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("GroupGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ImageList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsPishNevis"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SheypoorCategoryList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("TitleList"), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Advertise");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.AdvertiseLog", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Adv").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("AdvType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Category").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("City").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Content").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DateSh").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("IP"), "nvarchar(30)").HasMaxLength(30);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastUpdate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterVisitorGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<decimal>(b.Property<decimal>("Price"), "decimal(18, 3)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Region").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("SimCardNumber"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("SlaveVisitorGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("State").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("StatusCode"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SubCategory1").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SubCategory2").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("URL").IsRequired(true), "nvarchar(500)").HasMaxLength(500);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UpdateDesc").IsRequired(true), "nvarchar(1000)").HasMaxLength(1000);
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("VisitCount"), "int");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_AdvertiseLog");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.CategoryGroup", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DivarCategoryList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SheypoorCategoryList"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_CategoryGroup");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.ChatNumbers", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Category"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("City"), "nvarchar(100)").HasMaxLength(100);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsUsed"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Number"), "nvarchar(15)").HasMaxLength(15);
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_ChatNumbers");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.City", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CityName").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("StateGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("Weight"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isDivarCity"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isNiazKadeCity"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isNiazmandyHaCity"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isSheypoorCity"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "StateGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_City");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.ContentBlackList", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Words").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_ContentBlackList");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.DepartmentCategory", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepartmentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FirstLevel"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SecondLevel"), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("ThirdLevel"), "nvarchar(50)").HasMaxLength(50);
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_DepartmentCategory");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.DivarCity", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CityName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_DivarCities");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.Owner", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Family").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Owner");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.Region", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Type"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Region");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.SheypoorCity", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("CityName"), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_SheypoorCities");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.SimCard", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DivarModified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("NextUseDivar"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("NextUseSheypoor"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<long>(b.Property<long>("Number"), "bigint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Operator"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("OwnerGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("SheypoorModified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("UseCount"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("UserName"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isSendDivarAdv"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isSendSheypoorAdv"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("isSheypoorBlocked"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_SimCard");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.State", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("StateName").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_State");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.TitleBlackList", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Words").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_TitleBlackList");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.Visitor", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("AdvReplacement").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("DepartmentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Enable_Status").IsRequired(true), "nvarchar(7)").HasMaxLength(7);
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("FullName").IsRequired(true), "nvarchar(50)").HasMaxLength(50);
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("IsAgent"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("MasterCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Phone"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SlaveCount"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_Visitor");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.VisitorAdv", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("AdvGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_VisitorAdv");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.VisitorCity", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("CityGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("VisitorGuid"), "uniqueidentifier");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "CityGuid" });
			b.HasIndex(new string[1] { "VisitorGuid" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_VisitorCity");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.City", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Advertise.State", "State").WithMany("Cities").HasForeignKey(new string[1] { "StateGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Advertise.VisitorCity", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Advertise.City", "City").WithMany("VisitorCities").HasForeignKey(new string[1] { "CityGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
			b.HasOne("AccSqlServerPersistence.Entities.Advertise.Visitor", "Visitor").WithMany("VisitorCities").HasForeignKey(new string[1] { "VisitorGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
	}
}
