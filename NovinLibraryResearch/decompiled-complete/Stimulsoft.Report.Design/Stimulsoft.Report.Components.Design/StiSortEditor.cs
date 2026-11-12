using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.CrossTab;

namespace Stimulsoft.Report.Components.Design;

public class StiSortEditor : UITypeEditor
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
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		StiComponent stiComponent = context.Instance as StiComponent;
		if (context.Instance is StiChartCommon)
		{
			StiChartCommon stiChartCommon = context.Instance as StiChartCommon;
			stiComponent = stiChartCommon.Chart;
		}
		if (stiComponent is StiCrossTab)
		{
			StiCrossTabDataSourceForm stiCrossTabDataSourceForm = new StiCrossTabDataSourceForm((StiCrossTab)stiComponent, StiCrossTabDataSourceForm.StartTab.Sort);
			try
			{
				stiComponent?.Report?.Designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "Relation"), onlyCurrentPage: true);
				stiComponent?.Report?.Designer?.AddOwnedForm((Form)(object)stiCrossTabDataSourceForm);
				((Form)stiCrossTabDataSourceForm).ShowDialog();
				return ((IStiSort)stiComponent).Sort;
			}
			finally
			{
				((IDisposable)stiCrossTabDataSourceForm)?.Dispose();
			}
		}
		StiBandForm stiBandForm = new StiBandForm(stiComponent, StiDataSelectionControl.StartTab.Sort);
		try
		{
			stiComponent?.Report?.Designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "Sort"), onlyCurrentPage: true);
			stiComponent?.Report?.Designer?.AddOwnedForm((Form)(object)stiBandForm);
			((Form)stiBandForm).ShowDialog();
			return ((IStiSort)stiComponent).Sort;
		}
		finally
		{
			((IDisposable)stiBandForm)?.Dispose();
		}
	}
}
