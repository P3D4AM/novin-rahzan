using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.CrossTab;

namespace Stimulsoft.Report.Components.Design;

public class StiDataSourceEditor : UITypeEditor
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
		object instance = GetInstance(context);
		if (instance is StiCrossTab)
		{
			return EditCrossTabDataSource(context);
		}
		if (instance is StiChartCommon)
		{
			return EditChartCommonDataSource(context);
		}
		return EditComponentDataSource(context);
	}

	private static object GetInstance(ITypeDescriptorContext context)
	{
		if (context.Instance is Array)
		{
			return (context.Instance as Array).GetValue(0);
		}
		return context.Instance;
	}

	private static object EditCrossTabDataSource(ITypeDescriptorContext context)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		object instance = GetInstance(context);
		StiCrossTab stiCrossTab = instance as StiCrossTab;
		StiCrossTabDataSourceForm stiCrossTabDataSourceForm = new StiCrossTabDataSourceForm(stiCrossTab, StiCrossTabDataSourceForm.StartTab.DataSource);
		try
		{
			SaveUndoRedoItem(stiCrossTab, (Form)(object)stiCrossTabDataSourceForm);
			((Form)stiCrossTabDataSourceForm).ShowDialog();
			return ApplyResult(context, stiCrossTab);
		}
		finally
		{
			((IDisposable)stiCrossTabDataSourceForm)?.Dispose();
		}
	}

	private static object EditChartCommonDataSource(ITypeDescriptorContext context)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		object instance = GetInstance(context);
		StiChartCommon stiChartCommon = instance as StiChartCommon;
		StiChart chart = stiChartCommon.Chart;
		StiBandForm stiBandForm = new StiBandForm(chart, StiDataSelectionControl.StartTab.DataSource);
		try
		{
			SaveUndoRedoItem(chart, (Form)(object)stiBandForm);
			((Form)stiBandForm).ShowDialog();
			return ApplyResult(context, chart);
		}
		finally
		{
			((IDisposable)stiBandForm)?.Dispose();
		}
	}

	private static object EditComponentDataSource(ITypeDescriptorContext context)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		object instance = GetInstance(context);
		StiComponent component = instance as StiComponent;
		StiBandForm stiBandForm = new StiBandForm(component, StiDataSelectionControl.StartTab.DataSource);
		try
		{
			SaveUndoRedoItem(component, (Form)(object)stiBandForm);
			((Form)stiBandForm).ShowDialog();
			return ApplyResult(context, component);
		}
		finally
		{
			((IDisposable)stiBandForm)?.Dispose();
		}
	}

	private static object ApplyResult(ITypeDescriptorContext context, object component)
	{
		if (!(context.Instance is Array array))
		{
			return component;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array.GetValue(i) is IStiDataSource stiDataSource)
			{
				stiDataSource.DataSourceName = (component as IStiDataSource)?.DataSourceName;
			}
		}
		(component as StiComponent)?.Report?.Designer?.Refresh();
		return component;
	}

	private static void SaveUndoRedoItem(StiComponent component, Form form)
	{
		component?.Report?.Designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "DataSource"), onlyCurrentPage: true);
		component?.Report?.Designer?.AddOwnedForm(form);
	}
}
