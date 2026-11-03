using System;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace AccSqlServerPersistence.Migrations.Acc_Model;

[DbContext(typeof(Acc_ModelContext))]
[Migration("20210410121720_FirstMigration")]
public class FirstMigration : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema("Dbo");
		migrationBuilder.CreateTable("Acc_AghsatM_Group", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Count = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_AghsatM_Group", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CallingLog", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid8 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified5 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> ticketGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num9 = 15;
			OperationBuilder<AddColumnOperation> callerIdNumber = table.Column<string>((string)null, (bool?)null, num9, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rings = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num9 = 15;
			return new
			{
				Guid = guid8,
				Modified = modified5,
				TicketGuid = ticketGuid,
				CallerIdNumber = callerIdNumber,
				Rings = rings,
				DialedNumber = table.Column<string>((string)null, (bool?)null, num9, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				PickUpDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				HangUpDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				CoGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ExGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsIncoming = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsCompleteCall = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Version = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				CalcedNumber = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CallingLog", x => (object)x.Guid).Annotation("SqlServer:Clustered", (object)false);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CustomerTitle", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CustomerTitle", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Decimal", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid7 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num8 = 200;
			OperationBuilder<AddColumnOperation> name3 = table.Column<string>((string)null, (bool?)null, num8, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num8 = 200;
			OperationBuilder<AddColumnOperation> country = table.Column<string>((string)null, (bool?)null, num8, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> description3 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status3 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num8 = 50;
			return new
			{
				Guid = guid7,
				Name = name3,
				Country = country,
				Description = description3,
				Status = status3,
				Abstract = table.Column<string>((string)null, (bool?)null, num8, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				LastUpdate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				LastPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IconIndex = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Decimal", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Depot", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Manfi = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Depot", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_FullTextWords", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> id = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num7 = 400;
			return new
			{
				Id = id,
				Word = table.Column<string>((string)null, (bool?)null, num7, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_FullTextWords", x => (object)x.Id);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Gps", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Accuracy = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Activity = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Altitude = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Bearing = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ColorHex = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Distance = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FromLastPointDistance = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FromStartPointDistance = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GPSTime = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Icon = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Latitude = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Longitude = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PointDurationSeconds = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PreviewsID = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Speed = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Time = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserAccGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReferencedId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Gps", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_HesabType", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_HesabType", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxCustomerAllFields", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxCustomerAllFields", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductAllFields", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductAllFields", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductCodeField", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductCodeField", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductDescField", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductDescField", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductNameAndCodeField", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductNameAndCodeField", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductNameField", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductNameField", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT1Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT1Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT2Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT2Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT3Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT3Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT4Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT4Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT5Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT5Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT6Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT6Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT7Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT7Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_IxProductT8Field", (ColumnsBuilder table) => new
		{
			WordId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EntityGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_IxProductT8Field", x => (object)new { x.WordId, x.EntityGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_LastPrice", (ColumnsBuilder table) => new
		{
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Invalidated = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_LastPrice", x => (object)new { x.TafsilGuid, x.PrdGuid });
		}, (string)null);
		migrationBuilder.CreateTable("Acc_NoteGroup", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ParentGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_NoteGroup", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_OrderAbad", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DetailGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RefrencedGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ShapeInfo = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_OrderAbad", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdSerial", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DetailGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RefrencedGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GarrantyCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			StartDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EndDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sms = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SmsMob = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsKhoroj = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DetailType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdSerial", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Product", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid6 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified4 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num6 = 100;
			return new
			{
				Guid = guid6,
				Modified = modified4,
				IdProductLower = table.Column<string>((string)null, (bool?)null, num6, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				AutoSellSt = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BPercent1 = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BPercent2 = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BPercent3 = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BPercent4 = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Buy1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Buy2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Buy3 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BuyLast = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Code = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				CreateDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DiscountPercent = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				FullContent = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Height = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IdProduct = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsTax = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Tags = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsSerial = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				KardexPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Keywords = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				MetaDesc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Mojodi = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				OtherPics = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Pic = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				QuickText = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ScaleSt = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Score = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell1Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell2Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell3 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell3Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell4 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell4Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell5 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell5Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell6 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell6Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell7 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell7Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell8 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Sell8Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr1 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr2 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr3 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr4 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr5 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr6 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr7 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellKasr8 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood1 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood2 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood3 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood4 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood5 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood6 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood7 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SellSood8 = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Special = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T1 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T3 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T4 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T5 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T6 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T7 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				T8 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				V2Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				V3Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				V4Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				PartitionId = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				PayementType = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Width = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Weight = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				GuidGroup = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				GuidVahed = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				V2Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				V3Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				V4Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ImageList = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				OrderPoint = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Product", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ProductAttribute", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Value = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ProductAttribute", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ProductGroup", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Level = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FullCode = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HtmlFull = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HtmlQuick = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Pic = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Special = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TechInfoData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ParentGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ProductGroup", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ProductVahed", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ProductVahed", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_RasGiri", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BaseDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DayCount = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RecordCount = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sood_Mounthly = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sood_Total = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sood_Percent = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Total_Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve1 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve3 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve4 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve5 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve6 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve7 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve8 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve9 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Reserve10 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_RasGiri", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ResidStatus", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid5 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified3 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num5 = 150;
			return new
			{
				Guid = guid5,
				Modified = modified3,
				Name = table.Column<string>((string)null, (bool?)null, num5, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ResidStatus", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_SanadPrint", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tel = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Address = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Mob = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_SanadPrint", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_SuggestedBank", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ShobeCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AccountNo = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_SuggestedBank", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_TafsilGroup", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tbl = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeMin = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeMax = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Checked = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinSellGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinBuyGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinBackSellGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinBackBuyGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinReceptionGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinPardakhtGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinAmaniMaNazdDigaranGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinAmaniDigaranNazdMaGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_TafsilGroup", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Formula", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Formula", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_UserAcc", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsDisposed = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankGuid1 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankGuid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankGuid3 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankGuid4 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CusGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SandoghGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AllVisitorAccess = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AllSandoghsAccess = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AllDepotsAccess = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AllBanksAccess = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserAccessStr = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Settings = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SerializedData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DefaultCusGroup = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			LoginAccess = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_UserAcc", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ViewCheckM", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			STName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSanad = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSanadM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateCheckM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateBargashtORVosol = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateBargashtORVosolM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateToPlace = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Place = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IDSanad = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidSanad = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Personal = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HesabNum = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustomerFullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Aval2re = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PoshtNomreh = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ViewCheckM", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ViewCheckSh", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSarresid = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ST = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustomerFullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtDate = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IDSanad = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidSanad = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Aval2re = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSarresidM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtDesc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ViewCheckSh", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Visit", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ScheduledDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			StartDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CreationdDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitIsCompleted = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SerializedTicketGuids = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SerializedData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Visit", x => (object)x.Guid);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_DecimalUpdates", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_DecimalUpdates", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_DecimalUpdates_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_HesabGroup", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TypeGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Count = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_HesabGroup", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_HesabGroup_Acc_HesabType_TypeGuid", x => (object)x.TypeGuid, "Acc_HesabType", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Barcode", (ColumnsBuilder table) => new
		{
			BarCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CreateDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotId = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ExpireDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Barcode", x => (object)x.BarCode);
			table.ForeignKey("FK_Acc_Barcode_Acc_Depot_DepotGuid", x => (object)x.DepotGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Barcode_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdAnbarRelation", (ColumnsBuilder table) => new
		{
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AnbarGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Min = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Amount = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdAnbarRelation", x => (object)new { x.PrdGuid, x.AnbarGuid });
			table.ForeignKey("FK_Acc_PrdAnbarRelation_Acc_Depot_AnbarGuid", x => (object)x.AnbarGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarRelation_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdTechnical", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TechnicalType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ProductGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdTechnical", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PrdTechnical_Acc_Product_ProductGuid", x => (object)x.ProductGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ProductAttributeRelation", (ColumnsBuilder table) => new
		{
			ProductGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AttributeGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ProductAttributeRelation", x => (object)new { x.ProductGuid, x.AttributeGuid });
			table.ForeignKey("FK_Acc_ProductAttributeRelation_Acc_ProductAttribute_AttributeGuid", x => (object)x.AttributeGuid, "Acc_ProductAttribute", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)2);
			table.ForeignKey("FK_Acc_ProductAttributeRelation_Acc_Product_ProductGuid", x => (object)x.ProductGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)2);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_RasGiriDet", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Enheraf = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_RasGiriDet", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_RasGiriDet_Acc_RasGiri_MasterGuid", x => (object)x.MasterGuid, "Acc_RasGiri", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CustomerGroup", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Special = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinDefaultGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CustomerGroup", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_CustomerGroup_Acc_TafsilGroup_TafsilGroupGuid", x => (object)x.TafsilGroupGuid, "Acc_TafsilGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tafsil", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			System = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Account = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SerializedData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tafsil", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tafsil_Acc_TafsilGroup_TafsilGroupGuid", x => (object)x.TafsilGroupGuid, "Acc_TafsilGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Formula_Export", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FormulaGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PRDGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DPTGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Formula_Export", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Formula_Export_Acc_Depot_DPTGuid", x => (object)x.DPTGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Export_Acc_Tolid_Formula_FormulaGuid", x => (object)x.FormulaGuid, "Acc_Tolid_Formula", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Export_Acc_Product_PRDGuid", x => (object)x.PRDGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Export_Acc_ProductVahed_VGuid", x => (object)x.VGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Formula_Mavad", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FormulaGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PRDGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DPTGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Formula_Mavad", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Formula_Mavad_Acc_Depot_DPTGuid", x => (object)x.DPTGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Mavad_Acc_Tolid_Formula_FormulaGuid", x => (object)x.FormulaGuid, "Acc_Tolid_Formula", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Mavad_Acc_Product_PRDGuid", x => (object)x.PRDGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Mavad_Acc_ProductVahed_VGuid", x => (object)x.VGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Cashier", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HashedPassword = table.Column<byte[]>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			StrCashierTime = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Cashier", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Cashier_Acc_UserAcc_UserGuid", x => (object)x.UserGuid, "Acc_UserAcc", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Kol", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Mahiat = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			System = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Account = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Kol", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Kol_Acc_HesabGroup_GroupGuid", x => (object)x.GroupGuid, "Acc_HesabGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ChequeBook", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SerialNumber = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsSaderShode = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ChequeBook", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ChequeBook_Acc_Tafsil_BankGuid", x => (object)x.BankGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Customer", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid4 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> fullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> address = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> tell = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> address2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num4 = 100;
			OperationBuilder<AddColumnOperation> code = table.Column<string>((string)null, (bool?)null, num4, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> codeMeli = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> company = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> defPrice = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> description2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> eghtesadiNo = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> etebar = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> etebarCheck = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> family = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> idPath = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> isBazaryab = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> isSupervisor = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> isEtebari = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> job = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> location = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> name2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num4 = 50;
			OperationBuilder<AddColumnOperation> password = table.Column<string>((string)null, (bool?)null, num4, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> picFace = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> picSign = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> tafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rdate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rDate2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rdate2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rDate3 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR3 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR4 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR5 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR6 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR7 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR8 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR9 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> rSTR10 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> supervisorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> special = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status2 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num4 = 100;
			return new
			{
				Guid = guid4,
				FullName = fullName,
				Address1 = address,
				Tell1 = tell,
				Address2 = address2,
				Code = code,
				CodeMeli = codeMeli,
				Company = company,
				Date = date,
				DefPrice = defPrice,
				Description = description2,
				EghtesadiNo = eghtesadiNo,
				Etebar = etebar,
				EtebarCheck = etebarCheck,
				Family = family,
				IdPath = idPath,
				IsBazaryab = isBazaryab,
				IsSupervisor = isSupervisor,
				IsEtebari = isEtebari,
				Job = job,
				Location = location,
				Name = name2,
				Password = password,
				PicFace = picFace,
				PicSign = picSign,
				TafsilGuid = tafsilGuid,
				Rdate1 = rdate,
				RDate2 = rDate,
				RDate3 = rDate2,
				Rdate4 = rdate2,
				RDate5 = rDate3,
				RSTR1 = rSTR,
				RSTR10 = rSTR2,
				RSTR2 = rSTR3,
				RSTR3 = rSTR4,
				RSTR4 = rSTR5,
				RSTR5 = rSTR6,
				RSTR6 = rSTR7,
				RSTR7 = rSTR8,
				RSTR8 = rSTR9,
				RSTR9 = rSTR10,
				SupervisorGuid = supervisorGuid,
				Special = special,
				Status = status2,
				User = table.Column<string>((string)null, (bool?)null, num4, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Zip = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				CustType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SerializedData = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				GroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				VisitorGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				BPercent = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				InPlacePayment = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				TitleGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				TafsilLevel = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Customer", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Customer_Acc_CustomerGroup_GroupGuid", x => (object)x.GroupGuid, "Acc_CustomerGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Customer_Acc_Tafsil_Guid", x => (object)x.Guid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Customer_Acc_CustomerTitle_TitleGuid", x => (object)x.TitleGuid, "Acc_CustomerTitle", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Resid", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ID2 = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date1 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidVahed = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			State = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Qt = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Beyane = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Mandeh = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdModel = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdColor = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Problems = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Darkhast = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Resid", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Resid_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Resid_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_TafsilNumbers", (ColumnsBuilder table) => new
		{
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OriginalNumber = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CalcedNumber = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Prefix = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_TafsilNumbers", x => (object)new { x.TafsilGuid, x.OriginalNumber });
			table.ForeignKey("FK_Acc_TafsilNumbers_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_BankCheque", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidCheque = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidBank = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNO = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Avale2re_Transfer = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_BankCheque", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_BankCheque_Acc_Tafsil_GuidBank", x => (object)x.GuidBank, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_BankCheque_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CallingLine", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid3 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num3 = 200;
			OperationBuilder<AddColumnOperation> computerName = table.Column<string>((string)null, (bool?)null, num3, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> deviceNumber = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> lineNumber = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num3 = 15;
			return new
			{
				Guid = guid3,
				Modified = modified2,
				ComputerName = computerName,
				DeviceNumber = deviceNumber,
				LineNumber = lineNumber,
				Number = table.Column<string>((string)null, (bool?)null, num3, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				IsCo = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				CashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CallingLine", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_CallingLine_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CheckSHPardakht", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CheckSHPardakht", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_CheckSHPardakht_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ChequeSHBargasht", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ChequeSHBargasht", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ChequeSHBargasht_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ChequeSHEbtal", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ChequeSHEbtal", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ChequeSHEbtal_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_DepEnteghal", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			No = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ModifiedM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MashinNo = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GirandeName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RanandehName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_DepEnteghal", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_DepEnteghal_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Note", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Text = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSarResidM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Pirority = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PirorityName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			StatusName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Note", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Note_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Note_Acc_NoteGroup_GroupGuid", x => (object)x.GroupGuid, "Acc_NoteGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdSelected", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdSelected", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PrdSelected_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ReportGenerator", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Rpt = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SqlQuery = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM1 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeS = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HTreeGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CusGroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeG = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeHesab = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeKol = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SelectedPrd = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SelectedHesab = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceD1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceD2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceC1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceC2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdgGroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Percent = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ShahrId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			NahiyeId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Asli = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Faree = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Num1 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Num2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Numbers = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Logs = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Desc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ReportGenerator", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ReportGenerator_Acc_CustomerGroup_CusGroupGuid", x => (object)x.CusGroupGuid, "Acc_CustomerGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReportGenerator_Acc_Depot_DepotGuid", x => (object)x.DepotGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReportGenerator_Acc_Tafsil_HGuid", x => (object)x.HGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReportGenerator_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReportGenerator_Acc_Cashier_UGuid", x => (object)x.UGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Sanad", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Sanad", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Sanad_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ShortCut", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num2 = 200;
			OperationBuilder<AddColumnOperation> formName = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num2 = 200;
			OperationBuilder<AddColumnOperation> name = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> shortCutKey = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num2 = 200;
			OperationBuilder<AddColumnOperation> func = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> cashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			OperationBuilder<AddColumnOperation> description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num2 = 15;
			OperationBuilder<AddColumnOperation> backColor = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			num2 = 15;
			return new
			{
				Guid = guid2,
				Modified = modified,
				Status = status,
				FormName = formName,
				Name = name,
				ShortCutKey = shortCutKey,
				Func = func,
				CashierGuid = cashierGuid,
				Description = description,
				BackColor = backColor,
				ForeColor = table.Column<string>((string)null, (bool?)null, num2, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				ImageSrc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Enable = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Visable = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				SubItems = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ShortCut", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ShortCut_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_TafsilSelected", delegate(ColumnsBuilder table)
		{
			OperationBuilder<AddColumnOperation> guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null);
			int? num = 50;
			return new
			{
				Guid = guid,
				Code = table.Column<string>((string)null, (bool?)null, num, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				UGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
				Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
			};
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_TafsilSelected", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_TafsilSelected_Acc_Cashier_UGuid", x => (object)x.UGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ID2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNO = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadST = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SUMMavad = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SUMMahsol = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SUMHazine = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Moein", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Name = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KolGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Mahiat = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			System = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Peygiri = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tedadi = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Account = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CodeKol = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Moein", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Moein_Acc_Kol_KolGuid", x => (object)x.KolGuid, "Acc_Kol", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CusCard", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CusGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CardName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CardNO = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			STDesc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			STName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CusCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CusName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Pass = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateStart = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateExpire = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CusCard", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_CusCard_Acc_Customer_CusGuid", x => (object)x.CusGuid, "Acc_Customer", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_DepEnteghalDet", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DptGuidSrc = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DptGuidDst = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VahedGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2Qt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_DepEnteghalDet", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_DepEnteghalDet_Acc_Depot_DptGuidSrc", x => (object)x.DptGuidSrc, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_DepEnteghalDet_Acc_DepEnteghal_MasterGuid", x => (object)x.MasterGuid, "Acc_DepEnteghal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_DepEnteghalDet_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_DepEnteghalDet_Acc_ProductVahed_VahedGuid", x => (object)x.VahedGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdSelectedDetails", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdSelectedDetails", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PrdSelectedDetails_Acc_PrdSelected_MasterGuid", x => (object)x.MasterGuid, "Acc_PrdSelected", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ReportGeneratorDet", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RepGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ParameterName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Order = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ReportGeneratorDet", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ReportGeneratorDet_Acc_ReportGenerator_RepGuid", x => (object)x.RepGuid, "Acc_ReportGenerator", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_CashiersLog", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Title = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ser_OLD = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ser_New = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Action = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Part = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_CashiersLog", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_CashiersLog_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_CashiersLog_Acc_Sanad_CashierGuid", x => (object)x.CashierGuid, "Acc_Sanad", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_TafsilSelectedDetails", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_TafsilSelectedDetails", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_TafsilSelectedDetails_Acc_TafsilSelected_MasterGuid", x => (object)x.MasterGuid, "Acc_TafsilSelected", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_FareOrderCredit", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ViewOrder = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilCode = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_FareOrderCredit", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_FareOrderCredit_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_FareOrderCredit_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_GardeshTafsil", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EnAsType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsTypeEnum = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Debit = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Credit = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNO = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Discount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DiscountPercent = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			QT = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			QT2 = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Bazaryab = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadDesc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateMoaser = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Rem = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RowDesc = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KolGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalQt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_GardeshTafsil", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Depot_DepotGuid", x => (object)x.DepotGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Kol_KolGuid", x => (object)x.KolGuid, "Acc_Kol", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_Cashier_UGuid", x => (object)x.UGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_GardeshTafsil_Acc_ProductVahed_VGuid", x => (object)x.VGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Kardex", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ProductGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DetailGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Discount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Fare = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tax = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VorodQT = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KhorojQT = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UnitPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RemQT = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UnitSood = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalSood = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RowSood = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			validated = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RemUnitPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RowSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RemTotalPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RDescription = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ID2 = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Avarez = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Invalidated = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Kardex", x => (object)x.Guid).Annotation("SqlServer:Clustered", (object)false);
			table.ForeignKey("FK_Acc_Kardex_Acc_Depot_DepotGuid", x => (object)x.DepotGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Kardex_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Kardex_Acc_Product_ProductGuid", x => (object)x.ProductGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Kardex_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Kardex_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_MoeinTafsilLevelRelations", (ColumnsBuilder table) => new
		{
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGroupGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_MoeinTafsilLevelRelations", x => (object)new { x.MoeinGuid, x.TafsilGroupGuid });
			table.ForeignKey("FK_Acc_MoeinTafsilLevelRelations_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_MoeinTafsilLevelRelations_Acc_TafsilGroup_TafsilGroupGuid", x => (object)x.TafsilGroupGuid, "Acc_TafsilGroup", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Pardakht", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AfterAccount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BeforeAccount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsTemp = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AndroidIme = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CpuSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HddSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ip = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsRead = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			LkSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumCheckM = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumCheckSh = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumEnteghal = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumBank = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumNaghd = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Pardakht", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Pardakht_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Pardakht_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Pardakht_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PardakhtCheckSHA2re", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Is_Enteghali = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PardakhtCheckSHA2re", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PardakhtCheckSHA2re_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckSHA2re_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckSHA2re_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdAnbarGardani", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KasrDebitTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KasrDebitMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KasrCreditTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KasrCreditMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EzafeDebitTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EzafeCreditTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EzafeDebitMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EzafeCreditMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KasrSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EzafeSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdAnbarGardani", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Moein_EzafeCreditMoeinGuid", x => (object)x.EzafeCreditMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Tafsil_EzafeCreditTafsilGuid", x => (object)x.EzafeCreditTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Moein_EzafeDebitMoeinGuid", x => (object)x.EzafeDebitMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Tafsil_EzafeDebitTafsilGuid", x => (object)x.EzafeDebitTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Moein_KasrCreditMoeinGuid", x => (object)x.KasrCreditMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Tafsil_KasrCreditTafsilGuid", x => (object)x.KasrCreditTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Moein_KasrDebitMoeinGuid", x => (object)x.KasrDebitMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Tafsil_KasrDebitTafsilGuid", x => (object)x.KasrDebitTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardani_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Reception", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryStatus = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsChecked = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Source = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsTemp = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AndroidIme = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CpuSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HddSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ip = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			LkSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumNaghd = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumBank = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumCheck = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumEnteghal = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AfterAccount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BeforeAccount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DependencyType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PaymentMethod = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Reception", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Reception_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Reception_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Reception_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ReceptionChequeMA2re", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Is_Enteghali = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ReceptionChequeMA2re", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ReceptionChequeMA2re_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionChequeMA2re_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionChequeMA2re_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_SanadDet", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Debit = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Credit = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Qt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterSanad = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_SanadDet", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_SanadDet_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_SanadDet_Acc_Sanad_MasterGuid", x => (object)x.MasterGuid, "Acc_Sanad", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_SanadDet_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_SanadDet_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Formula_Hazine", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FormulaGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Formula_Hazine", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Formula_Hazine_Acc_Tolid_Formula_FormulaGuid", x => (object)x.FormulaGuid, "Acc_Tolid_Formula", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Hazine_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Formula_Hazine_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_AghsatShakhsi", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ID2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSarResid = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_AghsatShakhsi", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_AghsatShakhsi_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatShakhsi_Acc_Pardakht_PardakhtGuid", x => (object)x.PardakhtGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatShakhsi_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatShakhsi_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Lottery", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Lottery", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Lottery_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Lottery_Acc_Pardakht_PardakhtGuid", x => (object)x.PardakhtGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Lottery_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PardakhtBank", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PardakhtBank", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PardakhtBank_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtBank_Acc_Pardakht_MasterGuid", x => (object)x.MasterGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtBank_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtBank_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PardakhtCheckSh", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSaresid = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Aval2re = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			A2re_MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			A2re_TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PardakhtCheckSh", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PardakhtCheckSh_Acc_Moein_A2re_MoeinGuid", x => (object)x.A2re_MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckSh_Acc_Tafsil_A2re_TafsilGuid", x => (object)x.A2re_TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckSh_Acc_Moein_BankMoeinGuid", x => (object)x.BankMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckSh_Acc_Tafsil_BankTafsilGuid", x => (object)x.BankTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckSh_Acc_Pardakht_MasterGuid", x => (object)x.MasterGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PardakhtNaghd", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PardakhtNaghd", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PardakhtNaghd_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtNaghd_Acc_Pardakht_MasterGuid", x => (object)x.MasterGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtNaghd_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtNaghd_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Vam", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Zarib = table.Column<float>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VamMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VamTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SepordeMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SepordeTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AghsatGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ZamenGuid1 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ZamenGuid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ZamenGuid3 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ZamenGuid4 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date1 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date2 = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Vam", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Vam_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)1);
			table.ForeignKey("FK_Acc_Vam_Acc_Pardakht_PardakhtGuid", x => (object)x.PardakhtGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Moein_SepordeMoeinGuid", x => (object)x.SepordeMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Tafsil_SepordeTafsilGuid", x => (object)x.SepordeTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Moein_VamMoeinGuid", x => (object)x.VamMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Tafsil_VamTafsilGuid", x => (object)x.VamTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Tafsil_ZamenGuid1", x => (object)x.ZamenGuid1, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Tafsil_ZamenGuid2", x => (object)x.ZamenGuid2, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Tafsil_ZamenGuid3", x => (object)x.ZamenGuid3, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Vam_Acc_Tafsil_ZamenGuid4", x => (object)x.ZamenGuid4, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PrdAnbarGardaniDet", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrdGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DptGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			QtKasr = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			QtEzafe = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UnitPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			KasrSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			EzafeSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PrdAnbarGardaniDet", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PrdAnbarGardaniDet_Acc_Depot_DptGuid", x => (object)x.DptGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardaniDet_Acc_PrdAnbarGardani_MasterGuid", x => (object)x.MasterGuid, "Acc_PrdAnbarGardani", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PrdAnbarGardaniDet_Acc_Product_PrdGuid", x => (object)x.PrdGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_AghsatAshkhas", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSarResid = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionDescription = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Checked = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionId2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FullCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Code = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionSanadNo = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionDateSh = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionDateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSarResidSh = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TypePardakht = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Mounth = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Count = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidGroup = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceMande = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_AghsatAshkhas", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_AghsatAshkhas_Acc_AghsatM_Group_GuidGroup", x => (object)x.GuidGroup, "Acc_AghsatM_Group", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatAshkhas_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatAshkhas_Acc_Reception_ReceptionGuid", x => (object)x.ReceptionGuid, "Acc_Reception", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatAshkhas_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_AghsatAshkhas_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Order", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsDisposed = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AfterAccount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AghsatGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AndroidIme = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AndroidImeLong = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B1TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B2TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B3TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B4TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B1MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B2MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B3MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B4MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B3 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			B4 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BarnameNo = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BazaryabFullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BeforeAccount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CpuSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustomerFullName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Discount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DiscountRows = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FactorType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DailyFactorNo = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineTafsilGuid1 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineTafsilGuid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineTafsilGuid3 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineTafsilGuid4 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineTafsilGuid5 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineMoeinGuid1 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineMoeinGuid2 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineMoeinGuid3 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineMoeinGuid4 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HazineMoeinGuid5 = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine3 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine4 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazine5 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HddSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Ip = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Keraye = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FareMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FareTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			LkSerial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PardakhtSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrintCount = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PrintDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ReceptionSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Sum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SumKala = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TasvieDay = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Avarez = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AvarezTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AvarezMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tax = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TaxTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TaxMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalBazaryab = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalDiscount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserAccGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserAccId = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryId = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryShortMessage = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			IsTemp = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HostRefrenced = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustFamily = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustMobile = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustAddress = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustZipCode = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CustTell = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PaymentMethod = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DeliveryStatus = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Source = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ServerStatus = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Order", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_AvarezMoeinGuid", x => (object)x.AvarezMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_AvarezTafsilGuid", x => (object)x.AvarezTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_B1MoeinGuid", x => (object)x.B1MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_B1TafsilGuid", x => (object)x.B1TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_B2MoeinGuid", x => (object)x.B2MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_B2TafsilGuid", x => (object)x.B2TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_B3MoeinGuid", x => (object)x.B3MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_B3TafsilGuid", x => (object)x.B3TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_B4MoeinGuid", x => (object)x.B4MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_B4TafsilGuid", x => (object)x.B4TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_FareMoeinGuid", x => (object)x.FareMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_FareTafsilGuid", x => (object)x.FareTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_HazineMoeinGuid1", x => (object)x.HazineMoeinGuid1, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_HazineMoeinGuid2", x => (object)x.HazineMoeinGuid2, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_HazineMoeinGuid3", x => (object)x.HazineMoeinGuid3, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_HazineMoeinGuid4", x => (object)x.HazineMoeinGuid4, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_HazineTafsilGuid1", x => (object)x.HazineTafsilGuid1, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_HazineTafsilGuid2", x => (object)x.HazineTafsilGuid2, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_HazineTafsilGuid3", x => (object)x.HazineTafsilGuid3, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_HazineTafsilGuid4", x => (object)x.HazineTafsilGuid4, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Pardakht_PardakhtGuid", x => (object)x.PardakhtGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Reception_ReceptionGuid", x => (object)x.ReceptionGuid, "Acc_Reception", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Moein_TaxMoeinGuid", x => (object)x.TaxMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Tafsil_TaxTafsilGuid", x => (object)x.TaxTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Order_Acc_Cashier_UserAccGuid", x => (object)x.UserAccGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ReceptionCheck", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BankName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HesabNum = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Personal = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PoshtNomreh = table.Column<long>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			A2re_PardazandeMoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			A2re_PardazandeTafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			A2re_ReceptionDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Aval2re = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ReceptionCheck", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ReceptionCheck_Acc_Moein_A2re_PardazandeMoeinGuid", x => (object)x.A2re_PardazandeMoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionCheck_Acc_Tafsil_A2re_PardazandeTafsilGuid", x => (object)x.A2re_PardazandeTafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionCheck_Acc_Reception_MasterGuid", x => (object)x.MasterGuid, "Acc_Reception", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ReceptionHavale", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Date = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Number = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TerminalId = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PAN = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			STAN = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RRN = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MerchantId = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MessageId = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateTasviye = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			FullDescription = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Qt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ReceptionHavale", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ReceptionHavale_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionHavale_Acc_Reception_MasterGuid", x => (object)x.MasterGuid, "Acc_Reception", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionHavale_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionHavale_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ReceptionNaghd", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ReceptionNaghd", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ReceptionNaghd_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionNaghd_Acc_Reception_MasterGuid", x => (object)x.MasterGuid, "Acc_Reception", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionNaghd_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ReceptionNaghd_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ConnectedSanad_Det", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Debit = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Credit = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Qt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNo = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			AsGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterSanad = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ConnectedSanad_Det", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ConnectedSanad_Det_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ConnectedSanad_Det_Acc_Order_MasterGuid", x => (object)x.MasterGuid, "Acc_Order", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ConnectedSanad_Det_Acc_Sanad_MasterSanad", x => (object)x.MasterSanad, "Acc_Sanad", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ConnectedSanad_Det_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ConnectedSanad_Det_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_LotteryScores", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ExpirationDate = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			CashierGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Score = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Status = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_LotteryScores", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_LotteryScores_Acc_Cashier_CashierGuid", x => (object)x.CashierGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_LotteryScores_Acc_Order_OrderGuid", x => (object)x.OrderGuid, "Acc_Order", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_LotteryScores_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_OrderDetail", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInQuantity = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInPrice = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInDiscount = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInVisitorPrice1 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInVisitorPrice2 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInVisitorPrice3 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ChangeInVisitorPrice4 = table.Column<bool>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ProductGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ProductName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DepotGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Discount = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2Qt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorPrice1 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorPrice2 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorPrice3 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VisitorPrice4 = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalVisitorPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TotalHazinePrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Tax = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Avarez = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RowSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Hazine1Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Hazine2Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Hazine3Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Hazine4Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Hazine5Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalQt = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalPrice = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalName = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DecimalSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PriceType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderDetState = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_OrderDetail", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_OrderDetail_Acc_Decimal_DecimalGuid", x => (object)x.DecimalGuid, "Acc_Decimal", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_OrderDetail_Acc_Depot_DepotGuid", x => (object)x.DepotGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_OrderDetail_Acc_Order_MasterGuid", x => (object)x.MasterGuid, "Acc_Order", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_OrderDetail_Acc_Product_ProductGuid", x => (object)x.ProductGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_OrderDetail_Acc_ProductVahed_VGuid", x => (object)x.VGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Export", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PRDGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DPTGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2QT = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Export", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Export_Acc_Depot_DPTGuid", x => (object)x.DPTGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Export_Acc_Order_OrderGuid", x => (object)x.OrderGuid, "Acc_Order", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Export_Acc_Product_PRDGuid", x => (object)x.PRDGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Export_Acc_ProductVahed_VGuid", x => (object)x.VGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Hazine", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			TafsilGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MoeinGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			HType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Type = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Hazine", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Hazine_Acc_Moein_MoeinGuid", x => (object)x.MoeinGuid, "Acc_Moein", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Hazine_Acc_Order_OrderGuid", x => (object)x.OrderGuid, "Acc_Order", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Hazine_Acc_Tafsil_TafsilGuid", x => (object)x.TafsilGuid, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_Tolid_Mavad", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			OrderGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			PRDGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DPTGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Quantity = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Serial = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			VGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2QT = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			V2Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			RowSum = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_Tolid_Mavad", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_Tolid_Mavad_Acc_Depot_DPTGuid", x => (object)x.DPTGuid, "Acc_Depot", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Mavad_Acc_Order_OrderGuid", x => (object)x.OrderGuid, "Acc_Order", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Mavad_Acc_Product_PRDGuid", x => (object)x.PRDGuid, "Acc_Product", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_Tolid_Mavad_Acc_ProductVahed_VGuid", x => (object)x.VGuid, "Acc_ProductVahed", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ChequeMBargasht", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidCheque = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNO = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			BeforeSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ChequeMBargasht", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ChequeMBargasht_Acc_ReceptionCheck_GuidCheque", x => (object)x.GuidCheque, "Acc_ReceptionCheck", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ChequeMBargasht_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_ChequeVosol", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidCheque = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNO = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_ChequeVosol", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_ChequeVosol_Acc_ReceptionCheck_GuidCheque", x => (object)x.GuidCheque, "Acc_ReceptionCheck", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_ChequeVosol_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_PardakhtCheckM", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Description = table.Column<string>((string)null, (bool?)null, (int?)null, false, (string)null, true, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidCheck = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Price = table.Column<decimal>("decimal(18, 3)", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_PardakhtCheckM", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_PardakhtCheckM_Acc_ReceptionCheck_GuidCheck", x => (object)x.GuidCheck, "Acc_ReceptionCheck", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_PardakhtCheckM_Acc_Pardakht_MasterGuid", x => (object)x.MasterGuid, "Acc_Pardakht", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_SandoghCheque", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidCheque = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Id2 = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			GuidSandogh = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateM = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			UserGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadNO = table.Column<int>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			SanadSt = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			DateSabt = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_SandoghCheque", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_SandoghCheque_Acc_ReceptionCheck_GuidCheque", x => (object)x.GuidCheque, "Acc_ReceptionCheck", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_SandoghCheque_Acc_Tafsil_GuidSandogh", x => (object)x.GuidSandogh, "Acc_Tafsil", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
			table.ForeignKey("FK_Acc_SandoghCheque_Acc_Cashier_UserGuid", x => (object)x.UserGuid, "Acc_Cashier", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateTable("Acc_AbadDet", (ColumnsBuilder table) => new
		{
			Guid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			X = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Y = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Z = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Count = table.Column<double>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ShapeType = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			ShapeInformation = table.Column<short>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			Modified = table.Column<DateTime>("datetime", (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null),
			MasterGuid = table.Column<Guid>((string)null, (bool?)null, (int?)null, false, (string)null, false, (object)null, (string)null, (string)null, (bool?)null, (string)null)
		}, "Dbo", table =>
		{
			table.PrimaryKey("PK_Acc_AbadDet", x => (object)x.Guid);
			table.ForeignKey("FK_Acc_AbadDet_Acc_OrderDetail_MasterGuid", x => (object)x.MasterGuid, "Acc_OrderDetail", "Guid", "Dbo", (ReferentialAction)0, (ReferentialAction)0);
		}, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AbadDet_MasterGuid", "Acc_AbadDet", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatAshkhas_GuidGroup", "Acc_AghsatAshkhas", "GuidGroup", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatAshkhas_MoeinGuid", "Acc_AghsatAshkhas", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatAshkhas_ReceptionGuid", "Acc_AghsatAshkhas", "ReceptionGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatAshkhas_TafsilGuid", "Acc_AghsatAshkhas", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatAshkhas_UserGuid", "Acc_AghsatAshkhas", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatShakhsi_MoeinGuid", "Acc_AghsatShakhsi", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatShakhsi_PardakhtGuid", "Acc_AghsatShakhsi", "PardakhtGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatShakhsi_TafsilGuid", "Acc_AghsatShakhsi", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_AghsatShakhsi_UserGuid", "Acc_AghsatShakhsi", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_BankCheque_GuidBank", "Acc_BankCheque", "GuidBank", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_BankCheque_UserGuid", "Acc_BankCheque", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Barcode_DepotGuid", "Acc_Barcode", "DepotGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Barcode_PrdGuid", "Acc_Barcode", "PrdGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CallingLine_CashierGuid", "Acc_CallingLine", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CallingLine_DeviceNo_LineNo", "Acc_CallingLine", new string[2] { "DeviceNumber", "LineNumber" }, "Dbo", true, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CallingLog_CalcedNumber", "Acc_CallingLog", "CalcedNumber", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Calling_DateM", "Acc_CallingLog", "DateM", "Dbo", false, (string)null).Annotation("SqlServer:Clustered", (object)true);
		migrationBuilder.CreateIndex("IX_Acc_Cashier_UserGuid", "Acc_Cashier", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CashiersLog_CashierGuid", "Acc_CashiersLog", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CheckSHPardakht_UserGuid", "Acc_CheckSHPardakht", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeBook_BankGuid", "Acc_ChequeBook", "BankGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeMBargasht_GuidCheque", "Acc_ChequeMBargasht", "GuidCheque", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeMBargasht_UserGuid", "Acc_ChequeMBargasht", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeSHBargasht_UserGuid", "Acc_ChequeSHBargasht", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeSHEbtal_UserGuid", "Acc_ChequeSHEbtal", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeVosol_GuidCheque", "Acc_ChequeVosol", "GuidCheque", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ChequeVosol_UserGuid", "Acc_ChequeVosol", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ConnectedSanad_Det_DecimalGuid", "Acc_ConnectedSanad_Det", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ConnectedSanad_Det_MasterGuid", "Acc_ConnectedSanad_Det", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ConnectedSanad_Det_MasterSanad", "Acc_ConnectedSanad_Det", "MasterSanad", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ConnectedSanad_Det_MoeinGuid", "Acc_ConnectedSanad_Det", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ConnectedSanad_Det_TafsilGuid", "Acc_ConnectedSanad_Det", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CusCard_CusGuid", "Acc_CusCard", "CusGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Customer_Code", "Acc_Customer", "Code", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Customer_GroupGuid", "Acc_Customer", "GroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Customer_ServerStatus", "Acc_Customer", "ServerStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Customer_TitleGuid", "Acc_Customer", "TitleGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Customer_Username_Password", "Acc_Customer", new string[2] { "User", "Password" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_CustomerGroup_TafsilGroupGuid", "Acc_CustomerGroup", "TafsilGroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Decimal_Name_Country", "Acc_Decimal", new string[2] { "Name", "Country" }, "Dbo", true, "[Name] IS NOT NULL AND [Country] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_Acc_DecimalUpdates_DecimalGuid", "Acc_DecimalUpdates", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_DepEnteghal_UserGuid", "Acc_DepEnteghal", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_DepEnteghalDet_DptGuidSrc", "Acc_DepEnteghalDet", "DptGuidSrc", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_DepEnteghalDet_MasterGuid", "Acc_DepEnteghalDet", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_DepEnteghalDet_PrdGuid", "Acc_DepEnteghalDet", "PrdGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_DepEnteghalDet_VahedGuid", "Acc_DepEnteghalDet", "VahedGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_FareOrderCredit_MoeinGuid", "Acc_FareOrderCredit", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_FareOrderCredit_TafsilGuid", "Acc_FareOrderCredit", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_FullTextWords_Word", "Acc_FullTextWords", "Word", "Dbo", true, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_DecimalGuid", "Acc_GardeshTafsil", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_DepotGuid", "Acc_GardeshTafsil", "DepotGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_KolGuid", "Acc_GardeshTafsil", "KolGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_MoeinGuid", "Acc_GardeshTafsil", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_PrdGuid", "Acc_GardeshTafsil", "PrdGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_TafsilGuid", "Acc_GardeshTafsil", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_UGuid", "Acc_GardeshTafsil", "UGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_GardeshTafsil_VGuid", "Acc_GardeshTafsil", "VGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Gps_ServerStatus", "Acc_Gps", "ServerStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_HesabGroup_TypeGuid", "Acc_HesabGroup", "TypeGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxCustomerAllFields", "Acc_IxCustomerAllFields", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductAllFields", "Acc_IxProductAllFields", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IxProductCodeField", "Acc_IxProductCodeField", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductDescField", "Acc_IxProductDescField", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductNameAndCodeField", "Acc_IxProductNameAndCodeField", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductNameField", "Acc_IxProductNameField", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductT1Field", "Acc_IxProductT1Field", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductT2Field", "Acc_IxProductT2Field", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductT3Field", "Acc_IxProductT3Field", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductT4Field", "Acc_IxProductT4Field", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductT5Field", "Acc_IxProductT5Field", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("Acc_IxProductT6Field", "Acc_IxProductT6Field", new string[2] { "WordId", "EntityGuid" }, "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_IxProductT7Field_EntityGuid", "Acc_IxProductT7Field", "EntityGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_IxProductT7Field_WordId", "Acc_IxProductT7Field", "WordId", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_IxProductT8Field_EntityGuid", "Acc_IxProductT8Field", "EntityGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_IxProductT8Field_WordId", "Acc_IxProductT8Field", "WordId", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Kardex_DepotGuid", "Acc_Kardex", "DepotGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Kardex_MoeinGuid", "Acc_Kardex", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Kardex_TafsilGuid", "Acc_Kardex", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Kardex_UserGuid", "Acc_Kardex", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Kardex_prdGuid_DepotGuid_Date", "Acc_Kardex", new string[3] { "ProductGuid", "DepotGuid", "DateM" }, "Dbo", false, (string)null).Annotation("SqlServer:Clustered", (object)true);
		migrationBuilder.CreateIndex("IX_Acc_Kol_GroupGuid", "Acc_Kol", "GroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Lottery_CashierGuid", "Acc_Lottery", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Lottery_PardakhtGuid", "Acc_Lottery", "PardakhtGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Lottery_TafsilGuid", "Acc_Lottery", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_LotteryScores_CashierGuid", "Acc_LotteryScores", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_LotteryScores_OrderGuid", "Acc_LotteryScores", "OrderGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_LotteryScores_TafsilGuid", "Acc_LotteryScores", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Moein_KolGuid", "Acc_Moein", "KolGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_MoeinTafsilLevelRelations_TafsilGroupGuid", "Acc_MoeinTafsilLevelRelations", "TafsilGroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Note_CashierGuid", "Acc_Note", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Note_GroupGuid", "Acc_Note", "GroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_AvarezMoeinGuid", "Acc_Order", "AvarezMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_AvarezTafsilGuid", "Acc_Order", "AvarezTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B1MoeinGuid", "Acc_Order", "B1MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B1TafsilGuid", "Acc_Order", "B1TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B2MoeinGuid", "Acc_Order", "B2MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B2TafsilGuid", "Acc_Order", "B2TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B3MoeinGuid", "Acc_Order", "B3MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B3TafsilGuid", "Acc_Order", "B3TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B4MoeinGuid", "Acc_Order", "B4MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_B4TafsilGuid", "Acc_Order", "B4TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_DeliveryStatus", "Acc_Order", "DeliveryStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_FareMoeinGuid", "Acc_Order", "FareMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_FareTafsilGuid", "Acc_Order", "FareTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineMoeinGuid1", "Acc_Order", "HazineMoeinGuid1", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineMoeinGuid2", "Acc_Order", "HazineMoeinGuid2", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineMoeinGuid3", "Acc_Order", "HazineMoeinGuid3", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineMoeinGuid4", "Acc_Order", "HazineMoeinGuid4", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineTafsilGuid1", "Acc_Order", "HazineTafsilGuid1", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineTafsilGuid2", "Acc_Order", "HazineTafsilGuid2", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineTafsilGuid3", "Acc_Order", "HazineTafsilGuid3", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_HazineTafsilGuid4", "Acc_Order", "HazineTafsilGuid4", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_MoeinGuid", "Acc_Order", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_PardakhtGuid", "Acc_Order", "PardakhtGuid", "Dbo", true, "[PardakhtGuid] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_Acc_Order_ReceptionGuid", "Acc_Order", "ReceptionGuid", "Dbo", true, "[ReceptionGuid] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_Acc_Order_TafsilGuid", "Acc_Order", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_TaxMoeinGuid", "Acc_Order", "TaxMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_TaxTafsilGuid", "Acc_Order", "TaxTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Order_UserAccGuid", "Acc_Order", "UserAccGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Order_ID2_Type", "Acc_Order", new string[2] { "FactorType", "Id2" }, "Dbo", true, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_OrderDetail_DecimalGuid", "Acc_OrderDetail", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_OrderDetail_DepotGuid", "Acc_OrderDetail", "DepotGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_OrderDetail_MasterGuid", "Acc_OrderDetail", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_OrderDetail_ProductGuid", "Acc_OrderDetail", "ProductGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_OrderDetail_VGuid", "Acc_OrderDetail", "VGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Pardakht_Id2", "Acc_Pardakht", "Id2", "Dbo", true, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Pardakht_MoeinGuid", "Acc_Pardakht", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Pardakht_ServerStatus", "Acc_Pardakht", "ServerStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Pardakht_TafsilGuid", "Acc_Pardakht", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Pardakht_UserGuid", "Acc_Pardakht", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtBank_DecimalGuid", "Acc_PardakhtBank", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtBank_MasterGuid", "Acc_PardakhtBank", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtBank_MoeinGuid", "Acc_PardakhtBank", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtBank_TafsilGuid", "Acc_PardakhtBank", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckM_GuidCheck", "Acc_PardakhtCheckM", "GuidCheck", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckM_MasterGuid", "Acc_PardakhtCheckM", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSh_A2re_MoeinGuid", "Acc_PardakhtCheckSh", "A2re_MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSh_A2re_TafsilGuid", "Acc_PardakhtCheckSh", "A2re_TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSh_BankMoeinGuid", "Acc_PardakhtCheckSh", "BankMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSh_BankTafsilGuid", "Acc_PardakhtCheckSh", "BankTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSh_MasterGuid", "Acc_PardakhtCheckSh", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSHA2re_MoeinGuid", "Acc_PardakhtCheckSHA2re", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSHA2re_TafsilGuid", "Acc_PardakhtCheckSHA2re", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtCheckSHA2re_UserGuid", "Acc_PardakhtCheckSHA2re", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtNaghd_DecimalGuid", "Acc_PardakhtNaghd", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtNaghd_MasterGuid", "Acc_PardakhtNaghd", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtNaghd_MoeinGuid", "Acc_PardakhtNaghd", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PardakhtNaghd_TafsilGuid", "Acc_PardakhtNaghd", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_EzafeCreditMoeinGuid", "Acc_PrdAnbarGardani", "EzafeCreditMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_EzafeCreditTafsilGuid", "Acc_PrdAnbarGardani", "EzafeCreditTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_EzafeDebitMoeinGuid", "Acc_PrdAnbarGardani", "EzafeDebitMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_EzafeDebitTafsilGuid", "Acc_PrdAnbarGardani", "EzafeDebitTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_KasrCreditMoeinGuid", "Acc_PrdAnbarGardani", "KasrCreditMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_KasrCreditTafsilGuid", "Acc_PrdAnbarGardani", "KasrCreditTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_KasrDebitMoeinGuid", "Acc_PrdAnbarGardani", "KasrDebitMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_KasrDebitTafsilGuid", "Acc_PrdAnbarGardani", "KasrDebitTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardani_UserGuid", "Acc_PrdAnbarGardani", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardaniDet_DptGuid", "Acc_PrdAnbarGardaniDet", "DptGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardaniDet_MasterGuid", "Acc_PrdAnbarGardaniDet", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarGardaniDet_PrdGuid", "Acc_PrdAnbarGardaniDet", "PrdGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdAnbarRelation_AnbarGuid", "Acc_PrdAnbarRelation", "AnbarGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdSelected_UserGuid", "Acc_PrdSelected", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdSelectedDetails_MasterGuid", "Acc_PrdSelectedDetails", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_PrdTechnical_ProductGuid", "Acc_PrdTechnical", "ProductGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Product_IdProductLower", "Acc_Product", "IdProductLower", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Product_ServerStatus", "Acc_Product", "ServerStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ProductAttributeRelation_AttributeGuid", "Acc_ProductAttributeRelation", "AttributeGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ProductGroup_Status", "Acc_ProductGroup", "Status", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_RasGiriDet_MasterGuid", "Acc_RasGiriDet", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Reception_Id2", "Acc_Reception", "Id2", "Dbo", true, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Reception_MoeinGuid", "Acc_Reception", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Reception_ServerStatus", "Acc_Reception", "ServerStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Reception_TafsilGuid", "Acc_Reception", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Reception_UserGuid", "Acc_Reception", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionCheck_A2re_PardazandeMoeinGuid", "Acc_ReceptionCheck", "A2re_PardazandeMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionCheck_A2re_PardazandeTafsilGuid", "Acc_ReceptionCheck", "A2re_PardazandeTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionCheck_MasterGuid", "Acc_ReceptionCheck", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionChequeMA2re_MoeinGuid", "Acc_ReceptionChequeMA2re", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionChequeMA2re_TafsilGuid", "Acc_ReceptionChequeMA2re", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionChequeMA2re_UserGuid", "Acc_ReceptionChequeMA2re", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionHavale_DecimalGuid", "Acc_ReceptionHavale", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionHavale_MasterGuid", "Acc_ReceptionHavale", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionHavale_MoeinGuid", "Acc_ReceptionHavale", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionHavale_TafsilGuid", "Acc_ReceptionHavale", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionNaghd_DecimalGuid", "Acc_ReceptionNaghd", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionNaghd_MasterGuid", "Acc_ReceptionNaghd", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionNaghd_MoeinGuid", "Acc_ReceptionNaghd", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReceptionNaghd_TafsilGuid", "Acc_ReceptionNaghd", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReportGenerator_CusGroupGuid", "Acc_ReportGenerator", "CusGroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReportGenerator_DepotGuid", "Acc_ReportGenerator", "DepotGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReportGenerator_HGuid", "Acc_ReportGenerator", "HGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReportGenerator_PrdGuid", "Acc_ReportGenerator", "PrdGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReportGenerator_UGuid", "Acc_ReportGenerator", "UGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ReportGeneratorDet_RepGuid", "Acc_ReportGeneratorDet", "RepGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Resid_PrdGuid", "Acc_Resid", "PrdGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Resid_TafsilGuid", "Acc_Resid", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Sanad_UserGuid", "Acc_Sanad", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SanadDet_DecimalGuid", "Acc_SanadDet", "DecimalGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SanadDet_MasterGuid", "Acc_SanadDet", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SanadDet_MoeinGuid", "Acc_SanadDet", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SanadDet_TafsilGuid", "Acc_SanadDet", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SandoghCheque_GuidCheque", "Acc_SandoghCheque", "GuidCheque", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SandoghCheque_GuidSandogh", "Acc_SandoghCheque", "GuidSandogh", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_SandoghCheque_UserGuid", "Acc_SandoghCheque", "UserGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_ShortCut_CashierGuid", "Acc_ShortCut", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tafsil_TafsilGroupGuid", "Acc_Tafsil", "TafsilGroupGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_TafsilNumber_CalcedNumber", "Acc_TafsilNumbers", "CalcedNumber", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_TafsilSelected_UGuid", "Acc_TafsilSelected", "UGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_TafsilSelectedDetails_MasterGuid", "Acc_TafsilSelectedDetails", "MasterGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_CashierGuid", "Acc_Tolid", "CashierGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Export_DPTGuid", "Acc_Tolid_Export", "DPTGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Export_OrderGuid", "Acc_Tolid_Export", "OrderGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Export_PRDGuid", "Acc_Tolid_Export", "PRDGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Export_VGuid", "Acc_Tolid_Export", "VGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Export_DPTGuid", "Acc_Tolid_Formula_Export", "DPTGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Export_FormulaGuid", "Acc_Tolid_Formula_Export", "FormulaGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Export_PRDGuid", "Acc_Tolid_Formula_Export", "PRDGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Export_VGuid", "Acc_Tolid_Formula_Export", "VGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Hazine_FormulaGuid", "Acc_Tolid_Formula_Hazine", "FormulaGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Hazine_MoeinGuid", "Acc_Tolid_Formula_Hazine", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Hazine_TafsilGuid", "Acc_Tolid_Formula_Hazine", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Mavad_DPTGuid", "Acc_Tolid_Formula_Mavad", "DPTGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Mavad_FormulaGuid", "Acc_Tolid_Formula_Mavad", "FormulaGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Mavad_PRDGuid", "Acc_Tolid_Formula_Mavad", "PRDGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Formula_Mavad_VGuid", "Acc_Tolid_Formula_Mavad", "VGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Hazine_MoeinGuid", "Acc_Tolid_Hazine", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Hazine_OrderGuid", "Acc_Tolid_Hazine", "OrderGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Hazine_TafsilGuid", "Acc_Tolid_Hazine", "TafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Mavad_DPTGuid", "Acc_Tolid_Mavad", "DPTGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Mavad_OrderGuid", "Acc_Tolid_Mavad", "OrderGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Mavad_PRDGuid", "Acc_Tolid_Mavad", "PRDGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Tolid_Mavad_VGuid", "Acc_Tolid_Mavad", "VGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_UserAcc_ServerStatus", "Acc_UserAcc", "ServerStatus", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_MoeinGuid", "Acc_Vam", "MoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_PardakhtGuid", "Acc_Vam", "PardakhtGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_SepordeMoeinGuid", "Acc_Vam", "SepordeMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_SepordeTafsilGuid", "Acc_Vam", "SepordeTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_VamMoeinGuid", "Acc_Vam", "VamMoeinGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_VamTafsilGuid", "Acc_Vam", "VamTafsilGuid", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_ZamenGuid1", "Acc_Vam", "ZamenGuid1", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_ZamenGuid2", "Acc_Vam", "ZamenGuid2", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_ZamenGuid3", "Acc_Vam", "ZamenGuid3", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Vam_ZamenGuid4", "Acc_Vam", "ZamenGuid4", "Dbo", false, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Visit_ServerStatus", "Acc_Visit", "ServerStatus", "Dbo", false, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable("Acc_AbadDet", "Dbo");
		migrationBuilder.DropTable("Acc_AghsatAshkhas", "Dbo");
		migrationBuilder.DropTable("Acc_AghsatShakhsi", "Dbo");
		migrationBuilder.DropTable("Acc_BankCheque", "Dbo");
		migrationBuilder.DropTable("Acc_Barcode", "Dbo");
		migrationBuilder.DropTable("Acc_CallingLine", "Dbo");
		migrationBuilder.DropTable("Acc_CallingLog", "Dbo");
		migrationBuilder.DropTable("Acc_CashiersLog", "Dbo");
		migrationBuilder.DropTable("Acc_CheckSHPardakht", "Dbo");
		migrationBuilder.DropTable("Acc_ChequeBook", "Dbo");
		migrationBuilder.DropTable("Acc_ChequeMBargasht", "Dbo");
		migrationBuilder.DropTable("Acc_ChequeSHBargasht", "Dbo");
		migrationBuilder.DropTable("Acc_ChequeSHEbtal", "Dbo");
		migrationBuilder.DropTable("Acc_ChequeVosol", "Dbo");
		migrationBuilder.DropTable("Acc_ConnectedSanad_Det", "Dbo");
		migrationBuilder.DropTable("Acc_CusCard", "Dbo");
		migrationBuilder.DropTable("Acc_DecimalUpdates", "Dbo");
		migrationBuilder.DropTable("Acc_DepEnteghalDet", "Dbo");
		migrationBuilder.DropTable("Acc_FareOrderCredit", "Dbo");
		migrationBuilder.DropTable("Acc_FullTextWords", "Dbo");
		migrationBuilder.DropTable("Acc_GardeshTafsil", "Dbo");
		migrationBuilder.DropTable("Acc_Gps", "Dbo");
		migrationBuilder.DropTable("Acc_IxCustomerAllFields", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductAllFields", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductCodeField", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductDescField", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductNameAndCodeField", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductNameField", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT1Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT2Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT3Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT4Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT5Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT6Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT7Field", "Dbo");
		migrationBuilder.DropTable("Acc_IxProductT8Field", "Dbo");
		migrationBuilder.DropTable("Acc_Kardex", "Dbo");
		migrationBuilder.DropTable("Acc_LastPrice", "Dbo");
		migrationBuilder.DropTable("Acc_Lottery", "Dbo");
		migrationBuilder.DropTable("Acc_LotteryScores", "Dbo");
		migrationBuilder.DropTable("Acc_MoeinTafsilLevelRelations", "Dbo");
		migrationBuilder.DropTable("Acc_Note", "Dbo");
		migrationBuilder.DropTable("Acc_OrderAbad", "Dbo");
		migrationBuilder.DropTable("Acc_PardakhtBank", "Dbo");
		migrationBuilder.DropTable("Acc_PardakhtCheckM", "Dbo");
		migrationBuilder.DropTable("Acc_PardakhtCheckSh", "Dbo");
		migrationBuilder.DropTable("Acc_PardakhtCheckSHA2re", "Dbo");
		migrationBuilder.DropTable("Acc_PardakhtNaghd", "Dbo");
		migrationBuilder.DropTable("Acc_PrdAnbarGardaniDet", "Dbo");
		migrationBuilder.DropTable("Acc_PrdAnbarRelation", "Dbo");
		migrationBuilder.DropTable("Acc_PrdSelectedDetails", "Dbo");
		migrationBuilder.DropTable("Acc_PrdSerial", "Dbo");
		migrationBuilder.DropTable("Acc_PrdTechnical", "Dbo");
		migrationBuilder.DropTable("Acc_ProductAttributeRelation", "Dbo");
		migrationBuilder.DropTable("Acc_ProductGroup", "Dbo");
		migrationBuilder.DropTable("Acc_RasGiriDet", "Dbo");
		migrationBuilder.DropTable("Acc_ReceptionChequeMA2re", "Dbo");
		migrationBuilder.DropTable("Acc_ReceptionHavale", "Dbo");
		migrationBuilder.DropTable("Acc_ReceptionNaghd", "Dbo");
		migrationBuilder.DropTable("Acc_ReportGeneratorDet", "Dbo");
		migrationBuilder.DropTable("Acc_Resid", "Dbo");
		migrationBuilder.DropTable("Acc_ResidStatus", "Dbo");
		migrationBuilder.DropTable("Acc_SanadDet", "Dbo");
		migrationBuilder.DropTable("Acc_SanadPrint", "Dbo");
		migrationBuilder.DropTable("Acc_SandoghCheque", "Dbo");
		migrationBuilder.DropTable("Acc_ShortCut", "Dbo");
		migrationBuilder.DropTable("Acc_SuggestedBank", "Dbo");
		migrationBuilder.DropTable("Acc_TafsilNumbers", "Dbo");
		migrationBuilder.DropTable("Acc_TafsilSelectedDetails", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Export", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Formula_Export", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Formula_Hazine", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Formula_Mavad", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Hazine", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Mavad", "Dbo");
		migrationBuilder.DropTable("Acc_Vam", "Dbo");
		migrationBuilder.DropTable("Acc_ViewCheckM", "Dbo");
		migrationBuilder.DropTable("Acc_ViewCheckSh", "Dbo");
		migrationBuilder.DropTable("Acc_Visit", "Dbo");
		migrationBuilder.DropTable("Acc_OrderDetail", "Dbo");
		migrationBuilder.DropTable("Acc_AghsatM_Group", "Dbo");
		migrationBuilder.DropTable("Acc_Customer", "Dbo");
		migrationBuilder.DropTable("Acc_DepEnteghal", "Dbo");
		migrationBuilder.DropTable("Acc_NoteGroup", "Dbo");
		migrationBuilder.DropTable("Acc_PrdAnbarGardani", "Dbo");
		migrationBuilder.DropTable("Acc_PrdSelected", "Dbo");
		migrationBuilder.DropTable("Acc_ProductAttribute", "Dbo");
		migrationBuilder.DropTable("Acc_RasGiri", "Dbo");
		migrationBuilder.DropTable("Acc_ReportGenerator", "Dbo");
		migrationBuilder.DropTable("Acc_Sanad", "Dbo");
		migrationBuilder.DropTable("Acc_ReceptionCheck", "Dbo");
		migrationBuilder.DropTable("Acc_TafsilSelected", "Dbo");
		migrationBuilder.DropTable("Acc_Tolid_Formula", "Dbo");
		migrationBuilder.DropTable("Acc_Decimal", "Dbo");
		migrationBuilder.DropTable("Acc_Order", "Dbo");
		migrationBuilder.DropTable("Acc_ProductVahed", "Dbo");
		migrationBuilder.DropTable("Acc_CustomerTitle", "Dbo");
		migrationBuilder.DropTable("Acc_CustomerGroup", "Dbo");
		migrationBuilder.DropTable("Acc_Depot", "Dbo");
		migrationBuilder.DropTable("Acc_Product", "Dbo");
		migrationBuilder.DropTable("Acc_Pardakht", "Dbo");
		migrationBuilder.DropTable("Acc_Reception", "Dbo");
		migrationBuilder.DropTable("Acc_Moein", "Dbo");
		migrationBuilder.DropTable("Acc_Tafsil", "Dbo");
		migrationBuilder.DropTable("Acc_Cashier", "Dbo");
		migrationBuilder.DropTable("Acc_Kol", "Dbo");
		migrationBuilder.DropTable("Acc_TafsilGroup", "Dbo");
		migrationBuilder.DropTable("Acc_UserAcc", "Dbo");
		migrationBuilder.DropTable("Acc_HesabGroup", "Dbo");
		migrationBuilder.DropTable("Acc_HesabType", "Dbo");
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
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("TafsilGuid"), "uniqueidentifier");
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
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxCustomerAllFields");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxCustomerAllFields");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductAllFields", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductAllFields");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductAllFields");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductCodeField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "IxProductCodeField");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductCodeField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductDescField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductDescField");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductDescField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductNameAndCodeField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductNameAndCodeField");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductNameAndCodeField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductNameField", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductNameField");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductNameField");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT1Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductT1Field");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT1Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT2Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductT2Field");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT2Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT3Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductT3Field");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT3Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT4Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductT4Field");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT4Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT5Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductT5Field");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT5Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT6Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			RelationalIndexBuilderExtensions.HasName(b.HasIndex(new string[2] { "WordId", "EntityGuid" }), "Acc_IxProductT6Field");
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT6Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT7Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			b.HasIndex(new string[1] { "EntityGuid" });
			b.HasIndex(new string[1] { "WordId" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_IxProductT7Field");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Indexes.IxProductT8Field", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("WordId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("EntityGuid"), "uniqueidentifier");
			b.HasKey(new string[2] { "WordId", "EntityGuid" });
			b.HasIndex(new string[1] { "EntityGuid" });
			b.HasIndex(new string[1] { "WordId" });
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
