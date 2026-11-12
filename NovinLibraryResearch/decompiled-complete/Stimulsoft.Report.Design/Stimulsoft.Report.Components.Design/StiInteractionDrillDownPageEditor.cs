using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;

namespace Stimulsoft.Report.Components.Design;

public class StiInteractionDrillDownPageEditor : UITypeEditor
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
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		StiInteraction stiInteraction = context.Instance as StiInteraction;
		StiSeriesInteraction stiSeriesInteraction = context.Instance as StiSeriesInteraction;
		if (stiInteraction == null && stiSeriesInteraction == null)
		{
			return null;
		}
		StiInteractionDrillDownPageSelectForm stiInteractionDrillDownPageSelectForm = new StiInteractionDrillDownPageSelectForm(stiInteraction, stiSeriesInteraction);
		try
		{
			StiReport stiReport = ((stiInteraction != null) ? stiInteraction.ParentComponent.Report : stiSeriesInteraction.ParentComponent.Report);
			stiReport?.Designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "DrillDownPage"), onlyCurrentPage: true);
			stiReport?.Designer?.AddOwnedForm((Form)(object)stiInteractionDrillDownPageSelectForm);
			((Form)stiInteractionDrillDownPageSelectForm).ShowDialog();
			if (stiInteraction != null)
			{
				return stiInteraction.DrillDownPage;
			}
			return stiSeriesInteraction.DrillDownPage;
		}
		finally
		{
			((IDisposable)stiInteractionDrillDownPageSelectForm)?.Dispose();
		}
	}
}
