using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Helper;

namespace Stimulsoft.Report.Check;

public class StiChartSeriesValueCheck : StiComponentCheck
{
	public override bool PreviewVisible => true;

	public override string ShortMessage => StiLocalizationExt.Get("CheckComponent", "StiCountDataDataSourceAtDataBandShort");

	public override string LongMessage => string.Format(StiLocalizationExt.Get("CheckComponent", "StiChartSeriesValueCheckLong"), ElementName);

	public override StiCheckStatus Status => StiCheckStatus.Warning;

	private bool Check()
	{
		if (base.Element is StiChart { DataSource: null } stiChart && stiChart.Series.Count > 0)
		{
			foreach (StiSeries item in stiChart.Series)
			{
				if (!string.IsNullOrEmpty(item.Value.Value))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override object ProcessCheck(StiReport report, object obj)
	{
		base.Element = obj;
		try
		{
			if (Check())
			{
				StiChartSeriesValueCheck stiChartSeriesValueCheck = new StiChartSeriesValueCheck();
				stiChartSeriesValueCheck.Element = obj;
				return stiChartSeriesValueCheck;
			}
			return null;
		}
		finally
		{
			base.Element = null;
		}
	}
}
