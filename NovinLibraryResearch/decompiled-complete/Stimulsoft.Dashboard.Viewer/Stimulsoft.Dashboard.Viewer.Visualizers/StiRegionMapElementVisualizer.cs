using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.RegionMap;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiRegionMapElementVisualizer : StiAtomElementVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiRegionMapElementControl(element);
	}
}
