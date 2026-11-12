using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Indicator;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiIndicatorElementVisualizer : StiAtomElementVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiIndicatorElementControl(element);
	}
}
