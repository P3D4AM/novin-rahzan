using System.Linq;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Data.Helpers;

namespace Stimulsoft.Dashboard.Design.Editors.Helpers;

public static class StiChartConverter
{
	public static void ConvertToBubble(StiChartElement chart)
	{
		chart?.Arguments?.Where((StiArgumentChartMeter a) => a != null).ToList().ForEach(delegate(StiArgumentChartMeter a)
		{
			a.Expression = StiExpressionHelper.ReplaceFunction(a.Expression, "Sum");
		});
	}

	public static void ConvertFromBubble(StiChartElement chart)
	{
		chart?.Arguments?.Where((StiArgumentChartMeter a) => a != null).ToList().ForEach(delegate(StiArgumentChartMeter a)
		{
			a.Expression = StiExpressionHelper.RemoveFunction(a.Expression);
		});
	}

	public static void ConvertToGantt(StiChartElement chart)
	{
		chart?.Values?.Where((StiValueChartMeter a) => a != null).ToList().ForEach(delegate(StiValueChartMeter a)
		{
			a.Expression = StiExpressionHelper.RemoveFunction(a.Expression);
		});
	}
}
