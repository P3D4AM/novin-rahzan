using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.TreeView;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiTreeViewElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		IStiTreeViewElement stiTreeViewElement = element as IStiTreeViewElement;
		if (stiTreeViewElement.SelectionMode == StiItemSelectionMode.One)
		{
			return (Control)(object)new StiOneTreeViewElementControl(element);
		}
		return (Control)(object)new StiMultiTreeViewElementControl(element);
	}
}
