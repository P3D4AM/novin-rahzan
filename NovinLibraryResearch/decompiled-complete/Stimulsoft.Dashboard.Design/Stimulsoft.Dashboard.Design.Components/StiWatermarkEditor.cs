using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Dashboard.Design.Editors;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Inplace;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiWatermarkEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)2;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		if (!(context.Instance is IStiDashboardWatermark stiDashboardWatermark))
		{
			return ((UITypeEditor)this).EditValue(context, provider, value);
		}
		StiDesignerControl stiDesignerControl = (stiDashboardWatermark?.GetApp() as StiReport)?.Designer?.DesignerControl as StiDesignerControl;
		StiWatermarkEditorForm stiWatermarkEditorForm = new StiWatermarkEditorForm(stiDashboardWatermark.DashboardWatermark, stiDashboardWatermark);
		stiDesignerControl?.AddOwnedForm((Form)(object)stiWatermarkEditorForm);
		StiInplaceDesigner.AddEditor(stiWatermarkEditorForm, stiDashboardWatermark as StiComponent);
		((Control)stiWatermarkEditorForm).Show();
		return value;
	}
}
