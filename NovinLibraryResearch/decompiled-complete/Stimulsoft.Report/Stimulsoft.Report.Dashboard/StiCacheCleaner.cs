using Stimulsoft.Base;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Report.Dashboard;

public static class StiCacheCleaner
{
	public static void Clean(IStiElement element)
	{
		object reportKey;
		if (element == null)
		{
			reportKey = null;
		}
		else
		{
			IStiApp app = ((IStiAppComponent)element).GetApp();
			reportKey = ((app != null) ? ((IStiAppCell)app).GetKey() : null);
		}
		Clean((string)reportKey);
	}

	public static void Clean(IStiAppDictionary dictionary)
	{
		object reportKey;
		if (dictionary == null)
		{
			reportKey = null;
		}
		else
		{
			IStiApp app = dictionary.GetApp();
			reportKey = ((app != null) ? ((IStiAppCell)app).GetKey() : null);
		}
		Clean((string)reportKey);
	}

	public static void Clean(IStiApp app)
	{
		if (app != null)
		{
			Clean((app != null) ? ((IStiAppCell)app).GetKey() : null);
		}
	}

	public static void Clean(string reportKey = null)
	{
		StiElementDataCache.CleanCache(reportKey);
		StiDataPicker.CleanCache(reportKey);
		StiDataJoiner.CleanCache(reportKey);
		StiDataFiltrator.CleanCache(reportKey);
		StiDataSorter.CleanCache(reportKey);
		StiDataActionOperator.CleanCache(reportKey);
		StiReportParser.CleanCache(reportKey);
		StiOnlineMapRepaintHelper.Clean(reportKey);
		StiPivotToContainerCache.Clean(reportKey);
		StiPivotTableToCrossTabCache.Clean(reportKey);
		StiPivotToConvertedStateCache.Clean(reportKey);
		StiDashboardImageHyperlinkCache.Clean(reportKey);
		StiSimpleShadowCache.Clean(reportKey);
	}
}
