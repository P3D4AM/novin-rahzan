using System;
using System.Windows.Forms;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Report.Components.Design;

public class StiBarCodeDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		StiBarCode barCode = component as StiBarCode;
		StiBarCodeEditorForm stiBarCodeEditorForm = new StiBarCodeEditorForm(barCode);
		try
		{
			if (component.Report.Designer != null && component.Report.Designer.DesignerControl != null)
			{
				component.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiBarCodeEditorForm);
			}
			((Form)stiBarCodeEditorForm).ShowDialog();
			return (DialogResult)1;
		}
		finally
		{
			((IDisposable)stiBarCodeEditorForm)?.Dispose();
		}
	}

	public StiBarCodeDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
