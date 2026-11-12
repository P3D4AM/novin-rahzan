using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Dialogs.Design;

public class StiFormDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (component is IStiReportControl)
		{
			return StiEventEditor.EditEvent(component.Report, ((IStiReportControl)component).GetDefaultEvent());
		}
		return StiEventEditor.EditEvent(component.Report, ((StiForm)component).GetDefaultEvent());
	}

	public StiFormDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
