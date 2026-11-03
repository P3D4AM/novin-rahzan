using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace AccSqlServerPersistence.Migrations.Adv_Model;

[DbContext(typeof(Adv_ModelContext))]
[Migration("20210408101648_Adv_FirstMigration")]
public class Adv_FirstMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema("Dbo");
		migrationBuilder.CreateTable("Adv_Advertise", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid12 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified12 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status5 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num12 = 200;
			return new
			{
				Guid = guid12,
				Modified = modified12,
				Status = status5,
				Name = table.Column<string>((string)null, (bool?)null, num12, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				GroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsPishNevis = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ImageList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				TitleList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ContentList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DivatCategoryList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SheypoorCategoryList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Advertise", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_AdvertiseLog", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid11 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified11 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num11 = 500;
			OperationBuilder<AddColumnOperation> title = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> content = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> simCardNumber = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> state = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> city2 = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> region = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> category = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> subCategory = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> subCategory2 = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 500;
			OperationBuilder<AddColumnOperation> uRL = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 30;
			OperationBuilder<AddColumnOperation> iP = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 1000;
			OperationBuilder<AddColumnOperation> updateDesc = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> visitCount = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 50;
			OperationBuilder<AddColumnOperation> dateSh = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> dateM2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num11 = 200;
			return new
			{
				Guid = guid11,
				Modified = modified11,
				Title = title,
				Content = content,
				SimCardNumber = simCardNumber,
				State = state,
				City = city2,
				Region = region,
				Price = price,
				Category = category,
				SubCategory1 = subCategory,
				SubCategory2 = subCategory2,
				URL = uRL,
				IP = iP,
				UpdateDesc = updateDesc,
				VisitCount = visitCount,
				DateSh = dateSh,
				DateM = dateM2,
				Adv = table.Column<string>((string)null, (bool?)null, num11, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				MasterVisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SlaveVisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				StatusCode = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				AdvType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				LastUpdate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_AdvertiseLog", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Category", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid10 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified10 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status4 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num10 = 50;
			return new
			{
				Guid = guid10,
				Modified = modified10,
				Status = status4,
				Name = table.Column<string>((string)null, (bool?)null, num10, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ParentGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Category", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_CategoryGroup", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DivarCategoryList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SheypoorCategoryList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_CategoryGroup", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_ChatNumbers", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid9 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified9 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num9 = 15;
			OperationBuilder<AddColumnOperation> number = table.Column<string>((string)null, (bool?)null, num9, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> dateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num9 = 100;
			OperationBuilder<AddColumnOperation> city = table.Column<string>((string)null, (bool?)null, num9, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num9 = 200;
			return new
			{
				Guid = guid9,
				Modified = modified9,
				Number = number,
				DateM = dateM,
				City = city,
				Category = table.Column<string>((string)null, (bool?)null, num9, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsUsed = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_ChatNumbers", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_City", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid8 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified8 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status3 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num8 = 200;
			return new
			{
				Guid = guid8,
				Modified = modified8,
				Status = status3,
				CityName = table.Column<string>((string)null, (bool?)null, num8, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				StateGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Weight = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				isDivarCity = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				isSheypoorCity = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				isNiazKadeCity = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				isNiazmandyHaCity = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_City", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_ContentBlackList", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Words = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_ContentBlackList", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Department", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid7 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified7 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num7 = 250;
			return new
			{
				Guid = guid7,
				Modified = modified7,
				Title = table.Column<string>((string)null, (bool?)null, num7, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ChatData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsSendChat = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ChatCount = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ChatCityData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Department", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_DepartmentCategory", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid6 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified6 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status2 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> departmentGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num6 = 50;
			OperationBuilder<AddColumnOperation> firstLevel = table.Column<string>((string)null, (bool?)null, num6, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num6 = 50;
			OperationBuilder<AddColumnOperation> secondLevel = table.Column<string>((string)null, (bool?)null, num6, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num6 = 50;
			return new
			{
				Guid = guid6,
				Modified = modified6,
				Status = status2,
				DepartmentGuid = departmentGuid,
				FirstLevel = firstLevel,
				SecondLevel = secondLevel,
				ThirdLevel = table.Column<string>((string)null, (bool?)null, num6, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_DepartmentCategory", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_DivarCities", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid5 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified5 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num5 = 200;
			return new
			{
				Guid = guid5,
				Modified = modified5,
				CityName = table.Column<string>((string)null, (bool?)null, num5, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_DivarCities", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Group", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid4 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified4 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num4 = 200;
			return new
			{
				Guid = guid4,
				Modified = modified4,
				GroupName = table.Column<string>((string)null, (bool?)null, num4, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ParentGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Group", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Owner", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid3 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified3 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num3 = 50;
			OperationBuilder<AddColumnOperation> name = table.Column<string>((string)null, (bool?)null, num3, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num3 = 50;
			return new
			{
				Guid = guid3,
				Modified = modified3,
				Name = name,
				Family = table.Column<string>((string)null, (bool?)null, num3, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				VisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Owner", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Region", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Region", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_SheypoorCities", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num2 = 200;
			return new
			{
				Guid = guid2,
				Modified = modified2,
				CityName = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_SheypoorCities", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_SimCard", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SheypoorModified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			NextUseDivar = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			NextUseSheypoor = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			isSheypoorBlocked = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			isSendDivarAdv = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			isSendSheypoorAdv = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OwnerGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UseCount = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DivarModified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Operator = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_SimCard", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_State", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			StateName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_State", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_TitleBlackList", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Words = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_TitleBlackList", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Tokens", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Token = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Tokens", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_Visitor", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> masterCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> slaveCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> isAgent = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num = 200;
			OperationBuilder<AddColumnOperation> advReplacement = table.Column<string>((string)null, (bool?)null, num, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num = 50;
			OperationBuilder<AddColumnOperation> fullName = table.Column<string>((string)null, (bool?)null, num, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num = 7;
			return new
			{
				Guid = guid,
				Modified = modified,
				MasterCount = masterCount,
				SlaveCount = slaveCount,
				IsAgent = isAgent,
				AdvReplacement = advReplacement,
				FullName = fullName,
				Status = status,
				Enable_Status = table.Column<string>((string)null, (bool?)null, num, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Phone = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DepartmentGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_Visitor", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_VisitorAdv", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AdvGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_VisitorAdv", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Adv_VisitorCity", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Adv_VisitorCity", x => (object)x.Guid);
		}, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("Adv_Advertise", "Dbo");
		migrationBuilder.DropTable("Adv_AdvertiseLog", "Dbo");
		migrationBuilder.DropTable("Adv_Category", "Dbo");
		migrationBuilder.DropTable("Adv_CategoryGroup", "Dbo");
		migrationBuilder.DropTable("Adv_ChatNumbers", "Dbo");
		migrationBuilder.DropTable("Adv_City", "Dbo");
		migrationBuilder.DropTable("Adv_ContentBlackList", "Dbo");
		migrationBuilder.DropTable("Adv_Department", "Dbo");
		migrationBuilder.DropTable("Adv_DepartmentCategory", "Dbo");
		migrationBuilder.DropTable("Adv_DivarCities", "Dbo");
		migrationBuilder.DropTable("Adv_Group", "Dbo");
		migrationBuilder.DropTable("Adv_Owner", "Dbo");
		migrationBuilder.DropTable("Adv_Region", "Dbo");
		migrationBuilder.DropTable("Adv_SheypoorCities", "Dbo");
		migrationBuilder.DropTable("Adv_SimCard", "Dbo");
		migrationBuilder.DropTable("Adv_State", "Dbo");
		migrationBuilder.DropTable("Adv_TitleBlackList", "Dbo");
		migrationBuilder.DropTable("Adv_Tokens", "Dbo");
		migrationBuilder.DropTable("Adv_Visitor", "Dbo");
		migrationBuilder.DropTable("Adv_VisitorAdv", "Dbo");
		migrationBuilder.DropTable("Adv_VisitorCity", "Dbo");
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
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Adv_VisitorCity");
		});
	}
}
