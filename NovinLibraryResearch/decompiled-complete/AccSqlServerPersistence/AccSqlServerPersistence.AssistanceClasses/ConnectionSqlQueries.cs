using System;
using Microsoft.EntityFrameworkCore.Migrations;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class ConnectionSqlQueries
{
	public static void FixStringAccountnig(MigrationBuilder migrationBuilder)
	{
		try
		{
			migrationBuilder.Sql("Update Acc_VisitorArea Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Visit Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckSh Set PardakhtDesc='' Where PardakhtDesc is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckSh Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckSh Set CustomerFullName='' Where CustomerFullName is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckSh Set BankName='' Where BankName is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckM Set Place='' Where Place is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckM Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckM Set HesabNum='' Where HesabNum is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckM Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckM Set DateToPlace='' Where DateToPlace is null", false);
			migrationBuilder.Sql("Update Acc_ViewCheckM Set BankName='' Where BankName is null", false);
			migrationBuilder.Sql("Update Acc_Vam Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_UserAcc Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_UserAcc Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Mavad Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Hazine Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Formula_Mavad Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Formula_Hazine Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Formula_Export Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Formula Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Formula Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid_Export Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tolid Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_TafsilSelectedDetails Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_TafsilSelected Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_TafsilSelected Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_TafsilSelected Set Code='' Where Code is null", false);
			migrationBuilder.Sql("Update Acc_TafsilNumbers Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_TafsilGroup Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_TafsilGroup Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Tafsil Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Tafsil Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_SuggestedBank Set ShobeCode='' Where ShobeCode is null", false);
			migrationBuilder.Sql("Update Acc_SuggestedBank Set BankName='' Where BankName is null", false);
			migrationBuilder.Sql("Update Acc_SuggestedBank Set AccountNo='' Where AccountNo is null", false);
			migrationBuilder.Sql("Update Acc_ShortCut Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_ShortCut Set Func='' Where Func is null", false);
			migrationBuilder.Sql("Update Acc_ShortCut Set FormName='' Where FormName is null", false);
			migrationBuilder.Sql("Update Acc_ShortCut Set ForeColor='' Where ForeColor is null", false);
			migrationBuilder.Sql("Update Acc_ShortCut Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ShortCut Set BackColor='' Where BackColor is null", false);
			migrationBuilder.Sql("Update Acc_SanadPrint Set Tel='' Where Tel is null", false);
			migrationBuilder.Sql("Update Acc_SanadPrint Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_SanadPrint Set Mob='' Where Mob is null", false);
			migrationBuilder.Sql("Update Acc_SanadPrint Set Address='' Where Address is null", false);
			migrationBuilder.Sql("Update Acc_SanadDet Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Sanad Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ResidStatus Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Resid Set Problems='' Where Problems is null", false);
			migrationBuilder.Sql("Update Acc_Resid Set PrdSerial='' Where PrdSerial is null", false);
			migrationBuilder.Sql("Update Acc_Resid Set PrdModel='' Where PrdModel is null", false);
			migrationBuilder.Sql("Update Acc_Resid Set PrdColor='' Where PrdColor is null", false);
			migrationBuilder.Sql("Update Acc_Resid Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Resid Set Darkhast='' Where Darkhast is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionNaghd Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionHavale Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionHavale Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionCheck Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionCheck Set HesabNum='' Where HesabNum is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionCheck Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ReceptionCheck Set BankName='' Where BankName is null", false);
			migrationBuilder.Sql("Update Acc_Reception Set FullName='' Where FullName is null", false);
			migrationBuilder.Sql("Update Acc_Reception Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_RasGiriDet Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_RasGiri Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_RasGiri Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ProductVahed Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_ProductVahed Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ProductGroup Set Pic='' Where Pic is null", false);
			migrationBuilder.Sql("Update Acc_ProductGroup Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_ProductGroup Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ProductAttribute Set Value='' Where Value is null", false);
			migrationBuilder.Sql("Update Acc_ProductAttribute Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T1='' Where T1 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T2='' Where T2 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T3='' Where T3 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T4='' Where T4 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T5='' Where T5 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T6='' Where T6 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T7='' Where T7 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set T8='' Where T8 is null", false);
			migrationBuilder.Sql("Update Acc_Product Set Slug=Guid Where Slug is null", false);
			migrationBuilder.Sql("Update Acc_Product Set Sku=Guid Where Sku is null", false);
			migrationBuilder.Sql("Update Acc_Product Set Pic='' Where Pic is null", false);
			migrationBuilder.Sql("Update Acc_Product Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Product Set ImageList='' Where ImageList is null", false);
			migrationBuilder.Sql("Update Acc_Product Set IdProduct='' Where IdProduct is null", false);
			migrationBuilder.Sql("Update Acc_Product Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PrdTechnical Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_PrdTechnical Set Code='' Where Code is null", false);
			migrationBuilder.Sql("Update Acc_PrdSerial Set Serial='' Where Serial is null", false);
			migrationBuilder.Sql("Update Acc_PrdSerial Set GarrantyCode='' Where GarrantyCode is null", false);
			migrationBuilder.Sql("Update Acc_PrdSerial Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PrdSelectedDetails Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PrdSelected Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_PrdSelected Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PrdSelected Set Code='' Where Code is null", false);
			migrationBuilder.Sql("Update Acc_PrdAnbarGardani Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PardakhtNaghd Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PardakhtCheckSh Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_PardakhtCheckSh Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PardakhtCheckM Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_PardakhtBank Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_PardakhtBank Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Pardakht Set FullName='' Where FullName is null", false);
			migrationBuilder.Sql("Update Acc_Pardakht Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_OrderDetail Set ProductName='' Where ProductName is null", false);
			migrationBuilder.Sql("Update Acc_OrderDetail Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Order Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_NoteGroup Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Note Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_Note Set Text='' Where Text is null", false);
			migrationBuilder.Sql("Update Acc_Moein Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Moein Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_LotteryScores Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Lottery Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Kol Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Kol Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Kardex Set RDescription='' Where RDescription is null", false);
			migrationBuilder.Sql("Update Acc_Kardex Set ID2='' Where ID2 is null", false);
			migrationBuilder.Sql("Update Acc_Kardex Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_HesabType Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_HesabGroup Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_HesabGroup Set Code='' Where Code is null", false);
			migrationBuilder.Sql("Update Acc_GardeshTafsil Set SanadDesc='' Where SanadDesc is null", false);
			migrationBuilder.Sql("Update Acc_GardeshTafsil Set RowDesc='' Where RowDesc is null", false);
			migrationBuilder.Sql("Update Acc_GardeshTafsil Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Depot Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Depot Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_DepEnteghalDet Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_DepEnteghal Set RanandehName='' Where RanandehName is null", false);
			migrationBuilder.Sql("Update Acc_DepEnteghal Set MashinNo='' Where MashinNo is null", false);
			migrationBuilder.Sql("Update Acc_DepEnteghal Set GirandeName='' Where GirandeName is null", false);
			migrationBuilder.Sql("Update Acc_DepEnteghal Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Decimal Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Decimal Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Decimal Set Country='' Where Country is null", false);
			migrationBuilder.Sql("Update Acc_Decimal Set Abstract='' Where Abstract is null", false);
			migrationBuilder.Sql("Update Acc_CustomerTitle Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_CustomerGroup Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_CustomerGroup Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Zip='' Where Zip is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set PicSign='' Where PicSign is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set PicFace='' Where PicFace is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Password='' Where Password is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Location='' Where Location is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Job='' Where Job is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set FullName='' Where FullName is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Family='' Where Family is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set EghtesadiNo='' Where EghtesadiNo is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Company='' Where Company is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set CodeMeli='' Where CodeMeli is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Address2='' Where Address2 is null", false);
			migrationBuilder.Sql("Update Acc_Customer Set Address1='' Where Address1 is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set STName='' Where STName is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set STDesc='' Where STDesc is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set Pass='' Where Pass is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set CusName='' Where CusName is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set CusCode='' Where CusCode is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set CardName='' Where CardName is null", false);
			migrationBuilder.Sql("Update Acc_CusCard Set CardNO='' Where CardNO is null", false);
			migrationBuilder.Sql("Update Acc_ChequeBook Set SerialNumber='' Where SerialNumber is null", false);
			migrationBuilder.Sql("Update Acc_CashiersLog Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_Cashier Set UserName='' Where UserName is null", false);
			migrationBuilder.Sql("Update Acc_Cashier Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Cashier Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_CallingLog Set DialedNumber='' Where DialedNumber is null", false);
			migrationBuilder.Sql("Update Acc_CallingLog Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_CallingLog Set CallerIdNumber='' Where CallerIdNumber is null", false);
			migrationBuilder.Sql("Update Acc_CallingLine Set Number='' Where Number is null", false);
			migrationBuilder.Sql("Update Acc_Barcode Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_Barcode Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_AghsatShakhsi Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_AghsatAshkhas Set Description='' Where Description is null", false);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixStringAccountnig", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\AssistanceClasses\\ConnectionSqlQueries.cs", 237);
		}
	}

	public static void FixStringGlobal(MigrationBuilder migrationBuilder)
	{
		try
		{
			migrationBuilder.Sql("Update Global_Taghvim Set Monasebat='' Where Monasebat is null", false);
			migrationBuilder.Sql("Update Global_Taghvim Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Global_PushNotifications Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Global_PushNotifications Set Message='' Where Message is null", false);
			migrationBuilder.Sql("Update Global_MetaData Set Value='' Where Value is null", false);
			migrationBuilder.Sql("Update Acc_UserInformation Set UserData='' Where UserData is null", false);
			migrationBuilder.Sql("Update Acc_UserInformation Set Password='' Where Password is null", false);
			migrationBuilder.Sql("Update Acc_UploadFile Set UrlPath='' Where UrlPath is null", false);
			migrationBuilder.Sql("Update Acc_UploadFile Set SubDirectory='' Where SubDirectory is null", false);
			migrationBuilder.Sql("Update Acc_UploadFile Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_TicketPart Set Source='' Where Source is null", false);
			migrationBuilder.Sql("Update Acc_TicketPart Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_TicketPart Set Destination='' Where Destination is null", false);
			migrationBuilder.Sql("Update Acc_TicketPart Set [Desc]='' Where [Desc] is null", false);
			migrationBuilder.Sql("Update Acc_TicketDet Set SourceName='' Where SourceName is null", false);
			migrationBuilder.Sql("Update Acc_TicketDet Set Source='' Where Source is null", false);
			migrationBuilder.Sql("Update Acc_TicketDet Set Destination='' Where Destination is null", false);
			migrationBuilder.Sql("Update Acc_TicketDet Set [Desc]='' Where [Desc] is null", false);
			migrationBuilder.Sql("Update Acc_Ticket Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_Ticket Set Source='' Where Source is null", false);
			migrationBuilder.Sql("Update Acc_Ticket Set LastConversation='' Where LastConversation is null", false);
			migrationBuilder.Sql("Update Acc_Ticket Set Destination='' Where Destination is null", false);
			migrationBuilder.Sql("Update Acc_Ticket Set [Desc]='' Where [Desc] is null", false);
			migrationBuilder.Sql("Update Acc_News Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_News Set QuickText='' Where QuickText is null", false);
			migrationBuilder.Sql("Update Acc_News Set PicUrl='' Where PicUrl is null", false);
			migrationBuilder.Sql("Update Acc_News Set MetaDescription='' Where MetaDescription is null", false);
			migrationBuilder.Sql("Update Acc_News Set KeyWords='' Where KeyWords is null", false);
			migrationBuilder.Sql("Update Acc_News Set FullContent='' Where FullContent is null", false);
			migrationBuilder.Sql("Update Acc_News Set Category='' Where Category is null", false);
			migrationBuilder.Sql("Update Acc_Naghz Set Message='' Where Message is null", false);
			migrationBuilder.Sql("Update Acc_Log Set UserDescription='' Where UserDescription is null", false);
			migrationBuilder.Sql("Update Acc_Log Set Message='' Where Message is null", false);
			migrationBuilder.Sql("Update Acc_LocalSetting Set Value='' Where Value is null", false);
			migrationBuilder.Sql("Update Acc_L2F Set FName='' Where FName is null", false);
			migrationBuilder.Sql("Update Acc_HyperLink Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_HyperLink Set Target='' Where Target is null", false);
			migrationBuilder.Sql("Update Acc_HyperLink Set Style='' Where Style is null", false);
			migrationBuilder.Sql("Update Acc_HyperLink Set Category='' Where Category is null", false);
			migrationBuilder.Sql("Update Acc_FileInfoReTryDownload Set SubDirectory='' Where SubDirectory is null", false);
			migrationBuilder.Sql("Update Acc_FileInfoReTryDownload Set FileName='' Where FileName is null", false);
			migrationBuilder.Sql("Update Acc_FileInfo Set SubDirectory='' Where SubDirectory is null", false);
			migrationBuilder.Sql("Update Acc_FileInfo Set FileName='' Where FileName is null", false);
			migrationBuilder.Sql("Update Acc_Desc Set Source='' Where Source is null", false);
			migrationBuilder.Sql("Update Acc_Desc Set Destination='' Where Destination is null", false);
			migrationBuilder.Sql("Update Acc_Desc Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Zip='' Where Zip is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set WebSite='' Where WebSite is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Tel2='' Where Tel2 is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Tel='' Where Tel is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Tahsilat='' Where Tahsilat is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Subject='' Where Subject is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Shoghl='' Where Shoghl is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Shahrestan='' Where Shahrestan is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Senf='' Where Senf is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set SabegheKari='' Where SabegheKari is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Ostan='' Where Ostan is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Name='' Where Name is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set NahveAshnaee='' Where NahveAshnaee is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Mob2='' Where Mob2 is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Mob='' Where Mob is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Message='' Where Message is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Ip='' Where Ip is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Fax='' Where Fax is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Family='' Where Family is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Email='' Where Email is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Description='' Where Description is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set CodeMeli='' Where CodeMeli is null", false);
			migrationBuilder.Sql("Update Acc_ContactUs Set Address='' Where Address is null", false);
			migrationBuilder.Sql("Update Acc_BackupSettings Set Username='' Where Username is null", false);
			migrationBuilder.Sql("Update Acc_BackupSettings Set Password='' Where Password is null", false);
			migrationBuilder.Sql("Update Acc_BackupSchedule Set Title='' Where Title is null", false);
			migrationBuilder.Sql("Update Acc_BackupSchedule Set ConnectionString='' Where ConnectionString is null", false);
			migrationBuilder.Sql("Update Acc_BackupSchedule Set BackupPath='' Where BackupPath is null", false);
			migrationBuilder.Sql("Update Acc_BackupLog Set URL='' Where URL is null", false);
			migrationBuilder.Sql("Update Acc_BackupLog Set Path='' Where Path is null", false);
			migrationBuilder.Sql("Update Acc_BackupLog Set Message='' Where Message is null", false);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixStringGlobal", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\AssistanceClasses\\ConnectionSqlQueries.cs", 340);
		}
	}

	public static void FixTechnicalInfo(MigrationBuilder migrationBuilder)
	{
		try
		{
			migrationBuilder.Sql("truncate table Acc_PrdTechnical", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T1,1,Guid,GETDATE(),'true' from Acc_Product where isnull(T1,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T2,2,Guid,GETDATE(),'true' from Acc_Product where isnull(T2,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T3,3,Guid,GETDATE(),'true' from Acc_Product where isnull(T3,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T4,4,Guid,GETDATE(),'true' from Acc_Product where isnull(T4,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T5,5,Guid,GETDATE(),'true' from Acc_Product where isnull(T5,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T6,6,Guid,GETDATE(),'true' from Acc_Product where isnull(T6,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T7,7,Guid,GETDATE(),'true' from Acc_Product where isnull(T7,'')<>''", false);
			migrationBuilder.Sql("insert into Acc_PrdTechnical select newId(),'',T8,8,Guid,GETDATE(),'true' from Acc_Product where isnull(T8,'')<>''", false);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixTechnicalInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\AssistanceClasses\\ConnectionSqlQueries.cs", 356);
		}
	}

	public static void FixUtcDates(MigrationBuilder migrationBuilder)
	{
		try
		{
			migrationBuilder.Sql("update Acc_ViewCheckSh set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateSarresidM)", false);
			migrationBuilder.Sql("update Acc_ViewCheckM set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateCheckM)", false);
			migrationBuilder.Sql("update Acc_Reception set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), Date)", false);
			migrationBuilder.Sql("update Acc_PrdAnbarGardani set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_Pardakht set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), Date)", false);
			migrationBuilder.Sql("update Acc_Order set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), Date)", false);
			migrationBuilder.Sql("update Acc_Note set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_ChequeMOperation set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_ChequeShOperation set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_CashiersLog set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_CallingLog set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_AghsatAshkhas set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), Date)", false);
			migrationBuilder.Sql("update Acc_AghsatShakhsi set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_PardakhtBank set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_PardakhtCheckSh set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateSaresid)", false);
			migrationBuilder.Sql("update Acc_PardakhtNaghd set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
			migrationBuilder.Sql("update Acc_ReceptionCheck set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), Date)", false);
			migrationBuilder.Sql("update Acc_ReceptionHavale set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), Date)", false);
			migrationBuilder.Sql("update Acc_ReceptionNaghd set DateUtc=DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), DateM)", false);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FixUtcDates", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\AssistanceClasses\\ConnectionSqlQueries.cs", 382);
		}
	}
}
