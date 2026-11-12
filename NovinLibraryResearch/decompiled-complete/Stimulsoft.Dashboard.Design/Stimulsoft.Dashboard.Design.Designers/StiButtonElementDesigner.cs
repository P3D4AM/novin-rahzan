using System.Windows.Forms;
using Stimulsoft.Dashboard.Components.Button;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events.Design;

namespace Stimulsoft.Dashboard.Design.Designers;

public class StiButtonElementDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		StiButtonElement stiButtonElement = component as StiButtonElement;
		return StiEventEditor.RunEventEditor(stiButtonElement.Report, stiButtonElement.ClickEvent);
	}

	public StiButtonElementDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
