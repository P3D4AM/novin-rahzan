using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.AppBug;

[Serializable]
public class AppBug : Serializable<AppBug>
{
	[Required]
	public Guid Guid { get; set; } = Guid.NewGuid();


	[Required]
	public FileManagement_Parts PartID { get; set; }

	[Required]
	public long Version { get; set; }

	[Required]
	public bool HasBug { get; set; }

	[Required]
	[MinLength(3)]
	public string BugDesc { get; set; }

	public DateTime Modified { get; set; }

	public DateTime DateInsert { get; set; }

	[Required]
	public int MangmentID { get; set; }

	[JsonIgnore]
	public ENSource Source
	{
		get
		{
			try
			{
				return PartID switch
				{
					FileManagement_Parts.None => ENSource.None, 
					FileManagement_Parts.Slider => ENSource.None, 
					FileManagement_Parts.LogoIconBanner => ENSource.None, 
					FileManagement_Parts.PicPrd => ENSource.None, 
					FileManagement_Parts.PicCustomer => ENSource.None, 
					FileManagement_Parts.PicSign => ENSource.None, 
					FileManagement_Parts.Ticket => ENSource.Accounting, 
					FileManagement_Parts.Other => ENSource.Accounting, 
					FileManagement_Parts.PicPrdGroup => ENSource.Accounting, 
					FileManagement_Parts.NPBackupService => ENSource.NPBackupService, 
					FileManagement_Parts.Temp => ENSource.None, 
					FileManagement_Parts.Global => ENSource.None, 
					FileManagement_Parts.Accounting => ENSource.Accounting, 
					FileManagement_Parts.Baje => ENSource.Accounting, 
					FileManagement_Parts.TimeAttendance => ENSource.TimeAttendance, 
					FileManagement_Parts.SharedFiles => ENSource.None, 
					FileManagement_Parts.PicPrdOthers => ENSource.None, 
					FileManagement_Parts.Pictures => ENSource.None, 
					FileManagement_Parts.Stimulsoft2018 => ENSource.None, 
					FileManagement_Parts.Stimulsoft2013 => ENSource.None, 
					FileManagement_Parts.WebService => ENSource.WebService, 
					FileManagement_Parts.WebSms => ENSource.NpsmsWebSiteApi, 
					FileManagement_Parts.WebGsm => ENSource.PayamakGSM, 
					FileManagement_Parts.ChequePrint => ENSource.ChekPrint, 
					FileManagement_Parts.Hesabyar => ENSource.Android_HesabYar, 
					FileManagement_Parts.Sms => ENSource.PayamakService, 
					FileManagement_Parts.InternetUpdate => ENSource.InternetUpdater, 
					FileManagement_Parts.DynamicReportBuilder => ENSource.Accounting, 
					FileManagement_Parts.WebNp => ENSource.WebNP, 
					FileManagement_Parts.SefareshGir => ENSource.Android_SefareshGir, 
					FileManagement_Parts.ScaleDal => ENSource.None, 
					_ => ENSource.None, 
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Source", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\AppBug\\AppBug.cs", 111);
				return ENSource.None;
			}
		}
	}

	[Required]
	public BugsFixStatus BugFixStatus { get; set; }

	[JsonIgnore]
	public string BugFixStatusString => BugFixStatus.GetDisplay();

	[Required]
	public Guid CashierGuid { get; set; }

	[Required]
	public string Title { get; set; }

	[JsonIgnore]
	public string CashierName { get; set; } = "";


	[JsonIgnore]
	public string DateInsertFa => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(DateInsert);

	[JsonIgnore]
	public string ModifiedFa => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(Modified);
}
