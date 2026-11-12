using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Design;

public class StiGlobalizationContainerCollectionEditor : UITypeEditor
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
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (!(context.Instance is StiReport stiReport))
		{
			return value;
		}
		stiReport?.Designer?.UndoRedoSave(StiLocalization.Get("FormGlobalizationEditor", "title"));
		StiGlobalizationEditorForm stiGlobalizationEditorForm = new StiGlobalizationEditorForm(stiReport);
		try
		{
			stiReport?.Designer?.AddOwnedForm((Form)(object)stiGlobalizationEditorForm);
			((Form)stiGlobalizationEditorForm).ShowDialog();
			return stiReport.GlobalizationStrings;
		}
		finally
		{
			((IDisposable)stiGlobalizationEditorForm)?.Dispose();
		}
	}
}
