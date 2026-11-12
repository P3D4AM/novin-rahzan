using System;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiImageDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		StiImage image = component as StiImage;
		StiImageEditorForm stiImageEditorForm = new StiImageEditorForm(image);
		try
		{
			component.Report.Designer.AddOwnedForm((Form)(object)stiImageEditorForm);
			((Form)stiImageEditorForm).ShowDialog();
			return (DialogResult)1;
		}
		finally
		{
			((IDisposable)stiImageEditorForm)?.Dispose();
		}
	}

	public StiImageDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
