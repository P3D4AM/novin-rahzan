using System.Windows.Forms;
using Stimulsoft.Report.Design.Check;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Report.Design.Components;

public class StiCheckStatusDesigner : StiInplaceDesigner
{
	private void Form_Checked(object sender, StiCheckStatusEventArgs e)
	{
		if (base.Designer is StiDesignerControl stiDesignerControl)
		{
			stiDesignerControl.InvokeShowingReportCheckerResults(e);
		}
	}

	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		StiChecksViewerForm stiChecksViewerForm = new StiChecksViewerForm(component as StiReport, checkKeyEscape: false);
		stiChecksViewerForm.Checked += Form_Checked;
		if (base.Designer != null && base.Designer.DesignerControl != null)
		{
			base.Designer.DesignerControl.AddOwnedForm((Form)(object)stiChecksViewerForm);
		}
		return stiChecksViewerForm;
	}

	public StiCheckStatusDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
