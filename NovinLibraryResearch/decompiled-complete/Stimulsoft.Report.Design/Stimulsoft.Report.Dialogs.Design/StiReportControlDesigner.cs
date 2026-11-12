using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiReportControlDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (component is StiReportControl stiReportControl)
		{
			return StiEventEditor.EditEvent(stiReportControl.Report, stiReportControl.GetDefaultEvent());
		}
		return StiEventEditor.EditEvent(component.Report, ((StiForm)component).GetDefaultEvent());
	}

	public StiReportControlDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
