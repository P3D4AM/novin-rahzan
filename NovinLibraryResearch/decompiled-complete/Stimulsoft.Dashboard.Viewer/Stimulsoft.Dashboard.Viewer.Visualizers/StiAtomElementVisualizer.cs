using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiAtomElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiAtomElementControl(element);
	}
}
