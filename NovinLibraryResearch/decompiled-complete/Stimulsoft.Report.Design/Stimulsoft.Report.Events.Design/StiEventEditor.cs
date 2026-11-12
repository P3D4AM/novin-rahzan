using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Base.Blockly;
using Stimulsoft.Report.Chart;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Events.Design;

public class StiEventEditor : UITypeEditor
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
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		StiReport report = ((context.Instance is StiComponent stiComponent) ? stiComponent.Report : null);
		if (context.Instance is StiReport)
		{
			report = context.Instance as StiReport;
		}
		if (context.Instance is StiSeries stiSeries)
		{
			report = ((StiComponent)stiSeries.Chart).Report;
		}
		if (value is StiEvent eventValue)
		{
			RunEventEditor(report, eventValue);
		}
		return value;
	}

	public static DialogResult RunEventEditor(StiReport report, StiEvent eventValue)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!StiBlocklyEditorCreator.IsAssemblyLoaded)
		{
			return Stimulsoft.Report.Design.Editors.StiEventEditor.EditEventSwitch(report, eventValue);
		}
		if (string.IsNullOrEmpty(eventValue.Script))
		{
			if (StiOptions.Designer.Editors.ScriptEditor.BlocksScriptEditorByDefault)
			{
				return Stimulsoft.Report.Design.Editors.StiEventEditor.EditBlocklyEvent(report, eventValue);
			}
			return Stimulsoft.Report.Design.Editors.StiEventEditor.EditEventSwitch(report, eventValue);
		}
		if (eventValue.Script.StartsWith("<xml xmlns=\"https://developers.google.com/blockly/xml\">"))
		{
			return Stimulsoft.Report.Design.Editors.StiEventEditor.EditBlocklyEvent(report, eventValue);
		}
		return Stimulsoft.Report.Design.Editors.StiEventEditor.EditEventSwitch(report, eventValue);
	}
}
