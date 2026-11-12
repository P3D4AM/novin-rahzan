using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Report.Components.Design;

public class StiCloneEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (!(context.Instance is StiComponent stiComponent))
		{
			return null;
		}
		StiContainerSelectForm stiContainerSelectForm = new StiContainerSelectForm(stiComponent);
		try
		{
			if (stiComponent.Report != null && stiComponent.Report.Designer != null)
			{
				stiComponent.Report.Designer.UndoRedoSave(StiLocalization.Get("PropertyMain", "Container"), onlyCurrentPage: true);
				stiComponent.Report.Designer.AddOwnedForm((Form)(object)stiContainerSelectForm);
			}
			((Form)stiContainerSelectForm).ShowDialog();
			return ((IStiClone)stiComponent).Container;
		}
		finally
		{
			((IDisposable)stiContainerSelectForm)?.Dispose();
		}
	}
}
