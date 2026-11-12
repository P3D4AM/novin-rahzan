using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.TreeViewBox;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiTreeViewBoxElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		IStiTreeViewBoxElement stiTreeViewBoxElement = element as IStiTreeViewBoxElement;
		if (stiTreeViewBoxElement.SelectionMode == StiItemSelectionMode.One)
		{
			return (Control)(object)new StiOneTreeViewBoxElementControl(element);
		}
		return (Control)(object)new StiMultiTreeViewBoxElementControl(element);
	}
}
