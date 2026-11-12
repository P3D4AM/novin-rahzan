using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Chart.Design;

namespace Stimulsoft.Report.Components.Design;

public class StiMasterComponentEditor : UITypeEditor
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
		if (instance is StiChartCommon)
		{
			return EditChartCommonMasterComponent(context);
		}
		return EditComponentMasterComponent(context);
	}

	private static object GetInstance(ITypeDescriptorContext context)
	{
		if (context.Instance is Array)
		{
			return (context.Instance as Array).GetValue(0);
		}
		return context.Instance;
	}

	private static object EditChartCommonMasterComponent(ITypeDescriptorContext context)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		object instance = GetInstance(context);
		StiChartCommon stiChartCommon = instance as StiChartCommon;
		StiChart chart = stiChartCommon.Chart;
		StiBandForm stiBandForm = new StiBandForm(chart, StiDataSelectionControl.StartTab.MasterComponent);
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

	private static object EditComponentMasterComponent(ITypeDescriptorContext context)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		object instance = GetInstance(context);
		StiComponent component = instance as StiComponent;
		StiBandForm stiBandForm = new StiBandForm(component, StiDataSelectionControl.StartTab.MasterComponent);
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
			return (component as IStiMasterComponent)?.MasterComponent;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array.GetValue(i) is IStiMasterComponent stiMasterComponent)
			{
				stiMasterComponent.MasterComponent = (component as IStiMasterComponent)?.MasterComponent;
			}
		}
		(component as StiComponent)?.Report?.Designer?.Refresh();
		return component;
	}

	private static void SaveUndoRedoItem(StiComponent component, Form form)
	{
		component?.Report?.Designer?.UndoRedoSave(StiLocalization.Get("PropertyMain", "MasterComponent"), onlyCurrentPage: true);
		component?.Report?.Designer?.AddOwnedForm(form);
	}
}
