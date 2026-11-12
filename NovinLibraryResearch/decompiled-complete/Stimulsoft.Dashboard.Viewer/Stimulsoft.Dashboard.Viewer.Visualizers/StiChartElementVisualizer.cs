using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Chart;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiChartElementVisualizer : StiAtomElementVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiChartElementControl(element);
	}
}
