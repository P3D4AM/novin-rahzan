using System;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiCloneDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		StiContainerSelectForm stiContainerSelectForm = new StiContainerSelectForm(component);
		try
		{
			if (component != null && component.Report != null && component.Report.Designer != null)
			{
				component.Report.Designer.AddOwnedForm((Form)(object)stiContainerSelectForm);
			}
			if (component.Page == null)
			{
				component.Page = base.Designer.Report.GetCurrentPage();
			}
			((Form)stiContainerSelectForm).ShowDialog();
			base.Designer.Refresh();
			return ((Form)stiContainerSelectForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiContainerSelectForm)?.Dispose();
		}
	}

	public StiCloneDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
