using System.Windows.Forms;
using Stimulsoft.Dashboard.Viewer.Elements.ComboBox;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiComboBoxElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		IStiComboBoxElement stiComboBoxElement = element as IStiComboBoxElement;
		if (stiComboBoxElement.SelectionMode == StiItemSelectionMode.One)
		{
			return (Control)(object)new StiOneComboBoxElementControl(element);
		}
		return (Control)(object)new StiMultiComboBoxElementControl(element);
	}
}
