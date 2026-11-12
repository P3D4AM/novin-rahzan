using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Panel;
using Stimulsoft.Dashboard.Viewer.Elements.Panel;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiPanelElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiPanelElementControl(element as StiPanelElement);
	}
}
