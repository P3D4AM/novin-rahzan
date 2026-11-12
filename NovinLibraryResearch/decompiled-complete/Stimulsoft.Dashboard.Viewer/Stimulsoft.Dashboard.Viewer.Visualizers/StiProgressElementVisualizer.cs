using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Progress;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiProgressElementVisualizer : StiAtomElementVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiProgressElementContainer(element);
	}
}
