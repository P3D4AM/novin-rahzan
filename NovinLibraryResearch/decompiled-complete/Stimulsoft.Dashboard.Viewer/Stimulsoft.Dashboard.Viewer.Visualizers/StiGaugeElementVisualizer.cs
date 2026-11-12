using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Gauge;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiGaugeElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiGaugeElementControl(element);
	}
}
