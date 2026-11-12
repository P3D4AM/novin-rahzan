using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Dashboard.Viewer.Elements.Table;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiOnlineMapElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		StiOnlineMapElement stiOnlineMapElement = (StiOnlineMapElement)element;
		if (stiOnlineMapElement.GetGisMeter() != null)
		{
			return (Control)(object)new StiGisElementControl(element);
		}
		return (Control)(object)new StiOnlineMapElementControl(element);
	}
}
