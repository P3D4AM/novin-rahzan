using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Table;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiTableElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiTableElementControl(element);
	}
}
