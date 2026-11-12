using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.Text;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiTextElementVisualizer : StiAtomElementVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiTextElementControl(element);
	}
}
