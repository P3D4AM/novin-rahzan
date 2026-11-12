using System;
using System.Collections;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Design.CrossTab;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Design.Interaction;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Design;

internal static class StiReportCheckerHelper
{
	public static void InvokeTextProperty(StiDesignerControl designerControl, object component, string propName)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		PropertyInfo property = component.GetType().GetProperty(propName);
		if (!(property == null) && property.GetValue(component, null) is StiExpression { Value: var text } stiExpression)
		{
			StiTextEditor.EditText(designerControl.Report, ref text, StiCodeEditorMode.EditorExpression);
			stiExpression.Value = text;
		}
	}

	public static void InvokeEvent(StiDesignerControl designerControl, object comp, string eventName)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (comp == null)
		{
			return;
		}
		Type type = comp.GetType();
		PropertyInfo property = type.GetProperty(eventName);
		if (property == null || !(property.GetValue(comp, null) is StiEvent ev))
		{
			return;
		}
		StiEventEditorForm stiEventEditorForm = new StiEventEditorForm(designerControl.Report, ev);
		try
		{
			designerControl.UndoRedoSave(StiLocalization.Get("FormTitles", "EventEditorForm"), onlyCurrentPage: true);
			designerControl.AddOwnedForm((Form)(object)stiEventEditorForm);
			((Form)stiEventEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiEventEditorForm)?.Dispose();
		}
	}

	public static void InvokeConditions(StiDesignerControl desgineControl, StiComponent comp)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Invalid comparison between Unknown and I4
		try
		{
			if (desgineControl.Report == null)
			{
				return;
			}
			Hashtable globalConditions = new Hashtable();
			StiComponentsCollection stiComponentsCollection = new StiComponentsCollection { comp };
			StiConditionsCollection conditions = StiConditionsHelper.GetConditions(stiComponentsCollection, ref globalConditions);
			StiConditionsEditorForm stiConditionsEditorForm = new StiConditionsEditorForm(desgineControl.Report, conditions, stiComponentsCollection[0]);
			try
			{
				desgineControl.AddOwnedForm((Form)(object)stiConditionsEditorForm);
				if ((int)((Form)stiConditionsEditorForm).ShowDialog() == 1)
				{
					StiConditionsHelper.SetConditions(stiComponentsCollection, conditions, globalConditions);
				}
				((Control)desgineControl).Refresh();
				desgineControl.InvokeSelectedObjectsChanged();
			}
			finally
			{
				((IDisposable)stiConditionsEditorForm)?.Dispose();
			}
		}
		catch (Exception exception)
		{
			StiExceptionProvider.Show(exception);
		}
	}

	public static void InvokeRichTextEditor(StiDesignerControl designerControl, StiRichText richText)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		StiSystemRichTextEditorForm stiSystemRichTextEditorForm = new StiSystemRichTextEditorForm(designerControl.Report, richText);
		try
		{
			designerControl.AddOwnedForm((Form)(object)stiSystemRichTextEditorForm);
			((Form)stiSystemRichTextEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiSystemRichTextEditorForm)?.Dispose();
		}
	}

	public static void InvokeDataBandEditor(StiDesignerControl designerControl, StiComponent comp, StiDataSelectionControl.StartTab tab)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		StiBandForm stiBandForm = new StiBandForm(comp, tab);
		try
		{
			designerControl.AddOwnedForm((Form)(object)stiBandForm);
			((Form)stiBandForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiBandForm)?.Dispose();
		}
	}

	public static void InvokeCrossTabEditor(StiDesignerControl designerControl, StiCrossTab crossTab)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		StiCrossTabDesignerForm stiCrossTabDesignerForm = new StiCrossTabDesignerForm(crossTab);
		try
		{
			designerControl.AddOwnedForm((Form)(object)stiCrossTabDesignerForm);
			((Form)stiCrossTabDesignerForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiCrossTabDesignerForm)?.Dispose();
		}
	}

	public static void InvokeCrossTabDataSourceEditor(StiDesignerControl designerControl, StiCrossTab crossTab, StiCrossTabDataSourceForm.StartTab startTab)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		StiCrossTabDataSourceForm stiCrossTabDataSourceForm = new StiCrossTabDataSourceForm(crossTab, startTab);
		try
		{
			designerControl.AddOwnedForm((Form)(object)stiCrossTabDataSourceForm);
			((Form)stiCrossTabDataSourceForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiCrossTabDataSourceForm)?.Dispose();
		}
	}

	public static void InvokeInteraction(StiDesignerControl designerControl, StiComponent comp, StiInteractionEditorForm.StartTab startTab)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		StiInteractionEditorForm stiInteractionEditorForm = new StiInteractionEditorForm(comp, startTab);
		try
		{
			designerControl.AddOwnedForm((Form)(object)stiInteractionEditorForm);
			((Form)stiInteractionEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiInteractionEditorForm)?.Dispose();
		}
	}
}
