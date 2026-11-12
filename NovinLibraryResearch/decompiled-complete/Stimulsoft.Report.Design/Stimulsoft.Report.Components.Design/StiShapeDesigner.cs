using System;
using System.Windows.Forms;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.Components.Design;

public class StiShapeDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		StiShape shape = component as StiShape;
		StiShapeEditorForm stiShapeEditorForm = new StiShapeEditorForm(shape);
		try
		{
			if (component.Report.Designer != null && component.Report.Designer.DesignerControl != null)
			{
				component.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiShapeEditorForm);
			}
			((Form)stiShapeEditorForm).ShowDialog();
			return (DialogResult)1;
		}
		finally
		{
			((IDisposable)stiShapeEditorForm)?.Dispose();
		}
	}

	public StiShapeDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
