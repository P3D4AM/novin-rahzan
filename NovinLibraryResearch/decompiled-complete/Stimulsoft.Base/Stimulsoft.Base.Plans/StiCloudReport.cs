using System.Collections;

namespace Stimulsoft.Base.Plans;

public static class StiCloudReport
{
	private static object lockObject = new object();

	private static Hashtable rowsFetched;

	private static Hashtable RowsFetched => rowsFetched ?? (rowsFetched = new Hashtable());

	public static int GetMaxReportPages(string reportGuid)
	{
		StiCloudPlanIdent stiCloudPlanIdent = StiCloudPlan.GetPlanIdent(reportGuid);
		if ((stiCloudPlanIdent & StiCloudPlanIdent.DSingle) > StiCloudPlanIdent.Trial)
		{
			stiCloudPlanIdent -= 64;
		}
		if ((stiCloudPlanIdent & StiCloudPlanIdent.DTeam) > StiCloudPlanIdent.Trial)
		{
			stiCloudPlanIdent -= 128;
		}
		if ((stiCloudPlanIdent & StiCloudPlanIdent.DEnterprise) > StiCloudPlanIdent.Trial)
		{
			stiCloudPlanIdent -= 256;
		}
		return StiCloudPlan.GetPlan(stiCloudPlanIdent).MaxReportPages;
	}

	public static int GetMaxDataRows(string reportGuid)
	{
		return StiCloudPlan.GetPlan(reportGuid).MaxDataRows;
	}

	public static int GetUsedDataRows(string reportGuid)
	{
		lock (lockObject)
		{
			object obj = RowsFetched[reportGuid];
			return (obj is int) ? ((int)obj) : 0;
		}
	}

	public static int AddDataRows(string reportGuid, int rowsCount)
	{
		lock (lockObject)
		{
			object obj = RowsFetched[reportGuid];
			int num = ((obj is int) ? ((int)obj) : 0);
			num += rowsCount;
			RowsFetched[reportGuid] = num;
			return num;
		}
	}

	public static void ResetDataRows(string reportGuid)
	{
		lock (lockObject)
		{
			if (!string.IsNullOrEmpty(reportGuid))
			{
				RowsFetched[reportGuid] = 0;
			}
		}
	}
}
