using System;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiSubReportDesigner : StiComponentDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		StiSubReportPageSelectForm stiSubReportPageSelectForm = new StiSubReportPageSelectForm(component as StiSubReport, base.FirstRun);
		try
		{
			if (component != null && component.Report != null && component.Report.Designer != null)
			{
				component.Report.Designer.AddOwnedForm((Form)(object)stiSubReportPageSelectForm);
			}
			((Form)stiSubReportPageSelectForm).ShowDialog();
			base.Designer.Refresh();
			return ((Form)stiSubReportPageSelectForm).DialogResult;
		}
		finally
		{
			((IDisposable)stiSubReportPageSelectForm)?.Dispose();
		}
	}

	public StiSubReportDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
