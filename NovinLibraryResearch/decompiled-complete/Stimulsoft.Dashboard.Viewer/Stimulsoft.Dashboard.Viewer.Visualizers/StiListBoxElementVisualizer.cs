using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.ListBox;
using Stimulsoft.Dashboard.Viewer.Elements.ListBox;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiListBoxElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		StiListBoxElement stiListBoxElement = element as StiListBoxElement;
		if (stiListBoxElement.Orientation == StiItemOrientation.Horizontal)
		{
			if (stiListBoxElement.SelectionMode == StiItemSelectionMode.One)
			{
				return (Control)(object)new StiOneHorizontalListBoxElementControl(element);
			}
			return (Control)(object)new StiMultiHorizontalListBoxElementControl(element);
		}
		if (stiListBoxElement.SelectionMode == StiItemSelectionMode.One)
		{
			return (Control)(object)new StiOneListBoxElementControl(element);
		}
		return (Control)(object)new StiMultiListBoxElementControl(element);
	}
}
