using System;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiPageDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (StiOptions.Engine.GlobalEvents.InvokeRunPageSetupInDesigner(component as StiPage))
		{
			return (DialogResult)2;
		}
		StiPageSetupForm stiPageSetupForm = new StiPageSetupForm(component as StiPage, base.Designer, 0);
		try
		{
			base.Designer.AddOwnedForm((Form)(object)stiPageSetupForm);
			DialogResult result = ((Form)stiPageSetupForm).ShowDialog();
			base.Designer.Refresh();
			return result;
		}
		finally
		{
			((IDisposable)stiPageSetupForm)?.Dispose();
		}
	}

	public StiPageDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
