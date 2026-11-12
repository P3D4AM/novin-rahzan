using System;
using System.Windows.Forms;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Chart.Editor;
using Stimulsoft.Report.Design.Inplace;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.Gauge.Helpers;

namespace Stimulsoft.Gauge.Design;

public sealed class StiGaugeDesigner : StiInplaceDesigner
{
	public override DialogResult Design(StiComponent component)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		StiGauge stiGauge = (StiGauge)component;
		if (StiGaugeV2InitHelper.AllowOldEditor)
		{
			stiGauge.Mode = StiScaleMode.V1;
			StiCloudGaugeForm stiCloudGaugeForm = new StiCloudGaugeForm(stiGauge);
			try
			{
				if (component?.Report?.Designer != null)
				{
					component.Report.Designer.AddOwnedForm((Form)(object)stiCloudGaugeForm);
				}
				((Form)stiCloudGaugeForm).ShowDialog();
				return (DialogResult)1;
			}
			finally
			{
				((IDisposable)stiCloudGaugeForm)?.Dispose();
			}
		}
		stiGauge.Mode = StiScaleMode.V2;
		return base.Design(component);
	}

	protected override StiInplaceEditorForm CreateEditorForm(object component)
	{
		return new StiGaugeEditorFormV2(component as StiGauge);
	}

	public StiGaugeDesigner(IStiDesignerBase designer)
		: base(designer)
	{
	}
}
