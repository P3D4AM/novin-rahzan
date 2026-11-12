using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.PivotTable;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiPivotTableElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiPivotTableElementControl(element);
	}
}
