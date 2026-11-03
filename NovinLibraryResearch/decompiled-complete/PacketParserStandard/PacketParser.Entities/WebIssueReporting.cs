using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class WebIssueReporting : IWebIssueReporting
{
	private static string BaseUrl => HttpHelpers.ApiUrl + "issuereporting/";

	public Guid Guid { get; set; }

	public Guid? VersionGuid { get; set; }

	public ENSource Source { get; set; }

	public string Version { get; set; }

	public DateTime InsertDate { get; set; }

	public string Title { get; set; }

	public string Discription { get; set; }

	public Guid CashierGuid { get; set; }

	public EnIssueReportingLevel Level { get; set; }

	public EnIssueReportingStatus Status { get; set; }

	public string ShInsertDate => InsertDate.MiladiToCalture();

	public string SourceForDisplay => Source.GetDisplay();

	public string LevelForDisplay => Level.GetDisplay();

	public string StatusForDisplay => Status.GetDisplay();

	public string CashierName { get; set; }

	public static async Task<List<WebIssueReporting>> GetAllAsync(CancellationToken token = default(CancellationToken), int timeOut = 1500)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebIssueReporting>>> returnedSaveFuncInfoWithValue = await (BaseUrl + "getall").GetFromApi<List<WebIssueReporting>>(token, 3, null, timeOut);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIssueReporting.cs", 37);
		}
		return null;
	}

	public static async Task DeleteAsync(Guid guid, CancellationToken token = default(CancellationToken), int timeOut = 1500)
	{
		try
		{
			await HttpHelpers.PostToApi(guid, BaseUrl + "delete", null, strongDeserialize: false, 15000, 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIssueReporting.cs", 46);
		}
	}

	public static async Task SaveAsync(WebIssueReporting item, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		try
		{
			await HttpHelpers.PostToApi(item, BaseUrl + "save", null, strongDeserialize: false, 15000, 3);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebIssueReporting.cs", 51);
		}
	}
}
