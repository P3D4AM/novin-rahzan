using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design;

internal class StiReportDataSourcesEditor : UITypeEditor
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
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Invalid comparison between Unknown and I4
		StiReportDataSourceCollection stiReportDataSourceCollection = value as StiReportDataSourceCollection;
		IReferenceService service = (IReferenceService)context.GetService(typeof(IReferenceService));
		StiReportDataSourcesEditorForm stiReportDataSourcesEditorForm = new StiReportDataSourcesEditorForm(service, stiReportDataSourceCollection.Report);
		try
		{
			stiReportDataSourceCollection?.Report?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiReportDataSourcesEditorForm);
			if ((int)((Form)stiReportDataSourcesEditorForm).ShowDialog() == 1)
			{
				context.OnComponentChanged();
			}
		}
		finally
		{
			((IDisposable)stiReportDataSourcesEditorForm)?.Dispose();
		}
		return value;
	}
}
