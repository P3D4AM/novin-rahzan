using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.DatePicker;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiDatePickerElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		return (Control)(object)new StiDatePickerElementControl(element);
	}
}
