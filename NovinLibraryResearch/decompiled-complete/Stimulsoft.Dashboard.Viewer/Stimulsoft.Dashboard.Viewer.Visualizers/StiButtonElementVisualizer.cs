using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Dashboard.Viewer.Elements.Button;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Viewer.Visualizers;

public class StiButtonElementVisualizer : StiGdiVisualizer
{
	public override Control Render(IStiElement element)
	{
		StiButtonElement stiButtonElement = element as StiButtonElement;
		return (Control)(stiButtonElement.Type switch
		{
			StiButtonType.Button => new StiButtonElementControl(element), 
			StiButtonType.RadioButton => new StiCheckButtonElementControl(element, isRadioButton: true), 
			_ => new StiCheckButtonElementControl(element), 
		});
	}
}
