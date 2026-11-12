using System;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiDataBandDesigner : StiBandDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		StiBandForm stiBandForm = new StiBandForm(component, StiDataSelectionControl.StartTab.DataSource);
		try
		{
			if (component != null && component.Report != null && component.Report.Designer != null)
			{
				component.Report.Designer.AddOwnedForm((Form)(object)stiBandForm);
			}
			return ((Form)stiBandForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiBandForm)?.Dispose();
		}
	}

	public StiDataBandDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
