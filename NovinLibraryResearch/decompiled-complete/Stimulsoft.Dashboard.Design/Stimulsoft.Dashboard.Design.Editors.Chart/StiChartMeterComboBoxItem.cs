using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Design.Editors.Chart;

internal class StiChartMeterComboBoxItem
{
	public string Text { get; set; }

	public string Key { get; set; }

	public StiChartConditionalField Field { get; set; }

	public StiChartSeriesType SeriesType { get; set; }

	public override string ToString()
	{
		return Text;
	}
}
