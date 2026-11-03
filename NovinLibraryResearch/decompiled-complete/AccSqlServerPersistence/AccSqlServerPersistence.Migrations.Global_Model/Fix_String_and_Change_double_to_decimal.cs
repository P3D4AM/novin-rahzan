using System;
using AccSqlServerPersistence.AssistanceClasses;
using AccSqlServerPersistence.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccSqlServerPersistence.Migrations.Global_Model;

[DbContext(typeof(Global_ModelContext))]
[Migration("20220331054741_Fix_String_and_Change_double_to_decimal")]
public class Fix_String_and_Change_double_to_decimal : Migration
{
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex("IX_Acc_Naghz_Message", "Acc_Naghz", "Dbo");
		migrationBuilder.DropIndex("IX_Acc_BackupSchedule_Title", "Acc_BackupSchedule", "Dbo");
		ConnectionSqlQueries.FixStringGlobal(migrationBuilder);
		Type typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Monasebat", "Global_Taghvim", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Description", "Global_Taghvim", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Global_PushNotifications", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Global_PushNotifications", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Value", "Global_MetaData", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("UserData", "Acc_UserInformation", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Password", "Acc_UserInformation", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("UrlPath", "Acc_UploadFile", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SubDirectory", "Acc_UploadFile", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Name", "Acc_UploadFile", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_TicketPart", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Name", "Acc_TicketPart", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_TicketPart", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Desc", "Acc_TicketPart", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SourceName", "Acc_TicketDet", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_TicketDet", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_TicketDet", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Desc", "Acc_TicketDet", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_Ticket", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_Ticket", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("LastConversation", "Acc_Ticket", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_Ticket", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Desc", "Acc_Ticket", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("QuickText", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("PicUrl", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("MetaDescription", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("KeyWords", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("FullContent", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Category", "Acc_News", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_Naghz", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(450)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("UserDescription", "Acc_Log", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_Log", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Value", "Acc_LocalSetting", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		int? num = 150;
		typeFromHandle = typeof(string);
		int? num2 = 150;
		migrationBuilder.AlterColumn<string>("FName", "Acc_L2F", (string)null, (bool?)null, num, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(150)", (bool?)null, num2, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_HyperLink", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Target", "Acc_HyperLink", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Style", "Acc_HyperLink", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Category", "Acc_HyperLink", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SubDirectory", "Acc_FileInfoReTryDownload", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("FileName", "Acc_FileInfoReTryDownload", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SubDirectory", "Acc_FileInfo", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("FileName", "Acc_FileInfo", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_Desc", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_Desc", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Description", "Acc_Desc", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Zip", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("WebSite", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Tel2", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Tel", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Tahsilat", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Subject", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Shoghl", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Shahrestan", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Senf", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SabegheKari", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Ostan", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Name", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("NahveAshnaee", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Mob2", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Mob", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Ip", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Fax", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Family", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Email", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Description", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("CodeMeli", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Address", "Acc_ContactUs", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Username", "Acc_BackupSettings", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Password", "Acc_BackupSettings", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_BackupSchedule", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(450)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("ConnectionString", "Acc_BackupSchedule", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("BackupPath", "Acc_BackupSchedule", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("URL", "Acc_BackupLog", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Path", "Acc_BackupLog", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_BackupLog", (string)null, (bool?)null, (int?)null, false, "Dbo", false, (object)null, (string)null, (string)null, typeFromHandle, "nvarchar(max)", (bool?)null, (int?)null, false, true, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Naghz_Message", "Acc_Naghz", "Message", "Dbo", true, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_BackupSchedule_Title", "Acc_BackupSchedule", "Title", "Dbo", true, (string)null);
	}

	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex("IX_Acc_Naghz_Message", "Acc_Naghz", "Dbo");
		migrationBuilder.DropIndex("IX_Acc_BackupSchedule_Title", "Acc_BackupSchedule", "Dbo");
		Type typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Monasebat", "Global_Taghvim", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Description", "Global_Taghvim", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Global_PushNotifications", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Global_PushNotifications", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Value", "Global_MetaData", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("UserData", "Acc_UserInformation", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Password", "Acc_UserInformation", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("UrlPath", "Acc_UploadFile", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SubDirectory", "Acc_UploadFile", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Name", "Acc_UploadFile", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_TicketPart", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Name", "Acc_TicketPart", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_TicketPart", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Desc", "Acc_TicketPart", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SourceName", "Acc_TicketDet", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_TicketDet", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_TicketDet", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Desc", "Acc_TicketDet", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_Ticket", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_Ticket", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("LastConversation", "Acc_Ticket", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_Ticket", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Desc", "Acc_Ticket", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("QuickText", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("PicUrl", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("MetaDescription", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("KeyWords", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("FullContent", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Category", "Acc_News", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_Naghz", "nvarchar(450)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("UserDescription", "Acc_Log", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_Log", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Value", "Acc_LocalSetting", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		int? num = 150;
		typeFromHandle = typeof(string);
		int? num2 = 150;
		migrationBuilder.AlterColumn<string>("FName", "Acc_L2F", "nvarchar(150)", (bool?)null, num, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, num2, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_HyperLink", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Target", "Acc_HyperLink", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Style", "Acc_HyperLink", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Category", "Acc_HyperLink", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SubDirectory", "Acc_FileInfoReTryDownload", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("FileName", "Acc_FileInfoReTryDownload", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SubDirectory", "Acc_FileInfo", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("FileName", "Acc_FileInfo", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Source", "Acc_Desc", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Destination", "Acc_Desc", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Description", "Acc_Desc", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Zip", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("WebSite", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Tel2", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Tel", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Tahsilat", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Subject", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Shoghl", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Shahrestan", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Senf", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("SabegheKari", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Ostan", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Name", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("NahveAshnaee", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Mob2", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Mob", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Ip", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Fax", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Family", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Email", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Description", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("CodeMeli", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Address", "Acc_ContactUs", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Username", "Acc_BackupSettings", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Password", "Acc_BackupSettings", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Title", "Acc_BackupSchedule", "nvarchar(450)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("ConnectionString", "Acc_BackupSchedule", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("BackupPath", "Acc_BackupSchedule", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("URL", "Acc_BackupLog", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Path", "Acc_BackupLog", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		typeFromHandle = typeof(string);
		migrationBuilder.AlterColumn<string>("Message", "Acc_BackupLog", "nvarchar(max)", (bool?)null, (int?)null, false, "Dbo", true, (object)null, (string)null, (string)null, typeFromHandle, (string)null, (bool?)null, (int?)null, false, false, (object)null, (string)null, (string)null, (bool?)null, (bool?)null, (string)null, (string)null);
		migrationBuilder.CreateIndex("IX_Acc_Naghz_Message", "Acc_Naghz", "Message", "Dbo", true, "[Message] IS NOT NULL");
		migrationBuilder.CreateIndex("IX_Acc_BackupSchedule_Title", "Acc_BackupSchedule", "Title", "Dbo", true, "[Title] IS NOT NULL");
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
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("LastUpdate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Rpt").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(200)").HasMaxLength(200);
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Log", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
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
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Desc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DescSecondSide"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Destination").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DestinationType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("LastConversation").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("PartGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SecondSide"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Source").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SourceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("TicketStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Title").IsRequired(true), "nvarchar(max)");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "PartGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_Ticket");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("DateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Desc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Destination").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DestinationType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("InsertDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("MasterGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("NextDateM"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Seen"), "bit");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Source").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SourceName").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SourceType"), "smallint");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "MasterGuid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TicketDet");
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketPart", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("Guid").ValueGeneratedOnAdd(), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime?>(b.Property<DateTime?>("DeliveryDate"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<int>(b.Property<int>("DeliveryId"), "int");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("DeliveryShortMessage"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Desc").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Destination").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("DestinationType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("HostRefrenced"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("Index"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<DateTime>(b.Property<DateTime>("Modified"), "datetime");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Name").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<Guid>(b.Property<Guid>("ParentGuid"), "uniqueidentifier");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("SerializedData"), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("ServerStatus"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<string>(b.Property<string>("Source").IsRequired(true), "nvarchar(max)");
			RelationalPropertyBuilderExtensions.HasColumnType<short>(b.Property<short>("SourceType"), "smallint");
			RelationalPropertyBuilderExtensions.HasColumnType<bool>(b.Property<bool>("Status"), "bit");
			b.HasKey(new string[1] { "Guid" });
			b.HasIndex(new string[1] { "ServerStatus" });
			RelationalEntityTypeBuilderExtensions.ToTable(b, "Acc_TicketPart");
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
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Accounting.ReportGeneratorDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Accounting.ReportGenerator", "Master").WithMany("Details").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.Ticket", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Tickets.TicketPart", "Part").WithMany("Tickets").HasForeignKey(new string[1] { "PartGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
		modelBuilder.Entity("AccSqlServerPersistence.Entities.Tickets.TicketDet", (Action<EntityTypeBuilder>)delegate(EntityTypeBuilder b)
		{
			b.HasOne("AccSqlServerPersistence.Entities.Tickets.Ticket", "Ticket").WithMany("TicketDets").HasForeignKey(new string[1] { "MasterGuid" })
				.OnDelete((DeleteBehavior)5)
				.IsRequired(true);
		});
	}
}
