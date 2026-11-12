using System.Collections;

namespace Stimulsoft.Base.Plans;

public static class StiCloudReportResults
{
	private static object lockObject = new object();

	private static Hashtable limits = new Hashtable();

	private static Hashtable Limits => limits ?? (limits = new Hashtable());

	public static void InitMaxReportPages(string reportGuid, int max)
	{
		lock (lockObject)
		{
			StiCloudReportLimits stiCloudReportLimits = GetLimits(reportGuid);
			stiCloudReportLimits.MaxReportPages = max;
		}
	}

	public static void InitMaxDataRows(string reportGuid, int max)
	{
		lock (lockObject)
		{
			StiCloudReportLimits stiCloudReportLimits = GetLimits(reportGuid);
			stiCloudReportLimits.MaxDataRows = max;
		}
	}

	private static StiCloudReportLimits GetLimits(string reportGuid)
	{
		lock (lockObject)
		{
			StiCloudReportLimits stiCloudReportLimits = Limits[reportGuid] as StiCloudReportLimits;
			if (stiCloudReportLimits == null)
			{
				stiCloudReportLimits = new StiCloudReportLimits();
				Limits[reportGuid] = stiCloudReportLimits;
			}
			return stiCloudReportLimits;
		}
	}

	public static void ClearLimits(string reportGuid)
	{
		lock (lockObject)
		{
			if (Limits.ContainsKey(reportGuid))
			{
				Limits.Remove(reportGuid);
			}
		}
	}

	public static StiCloudReportLimits GetAndRemoveLimits(string reportGuid)
	{
		StiCloudReportLimits stiCloudReportLimits = GetLimits(reportGuid);
		if (stiCloudReportLimits != null)
		{
			lock (lockObject)
			{
				Limits.Remove(reportGuid);
			}
		}
		return stiCloudReportLimits;
	}
}
