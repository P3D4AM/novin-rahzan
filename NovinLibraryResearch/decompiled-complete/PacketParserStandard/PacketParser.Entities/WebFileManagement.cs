using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

[Serializable]
public class WebFileManagement : Serializable<WebFileManagement>, IFileManagement, IHasIntId
{
	private static string _url = HttpHelpers.ApiUrl + "Department/File/";

	public int Id { get; set; }

	public DateTime Modified { get; set; }

	public long CRC { get; set; }

	[JsonIgnore]
	public string DateSh
	{
		get
		{
			return DateM.MiladiToCalture();
		}
		set
		{
		}
	}

	public int PartID { get; set; }

	public string FileName { get; set; } = "";


	public string FullURL { get; set; } = "";


	public DateTime DateM { get; set; } = DateTime.Now;


	public string Title { get; set; } = "";


	public string Description { get; set; } = "";


	public string PageURL { get; set; } = "";


	public int Version { get; set; }

	public string VersionTXT { get; set; } = "";


	public ENSource Source
	{
		get
		{
			try
			{
				return (FileManagement_Parts)checked((short)PartID) switch
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
				WebErrorLog.Instance.StartLog(ex, "", "Source", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebFileManagment.cs", 113);
				return ENSource.None;
			}
		}
	}

	public static Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebFileManagement>>> GetLastVersionByPartFromApi(FileManagement_Parts item, CancellationToken token)
	{
		return (_url + $"GetLastVersionByPart/{(int)item}").GetFromApi<WebFileManagement>(token, 3);
	}

	public static Task<ReturnedSaveFuncInfoWithValue<WebResponse<WebFileManagement>>> GetLastVersionByEnsourceFromApi(ENSource item, CancellationToken token)
	{
		return (_url + $"GetLastVersionByEnSource/{(int)item}").GetFromApi<WebFileManagement>(token, 3);
	}
}
