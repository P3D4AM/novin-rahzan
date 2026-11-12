using System;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Blockly;
using Stimulsoft.Base.Blocks;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Gauge.Events;

namespace Stimulsoft.Report.Design.Editors;

public sealed class StiEventEditor
{
	public static DialogResult EditEvent(StiReport report, StiEvent ev)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		StiEventEditorForm stiEventEditorForm = new StiEventEditorForm(report, ev);
		try
		{
			report?.Designer?.UndoRedoSave(StiLocalization.Get("FormTitles", "EventEditorForm"), onlyCurrentPage: true);
			report?.Designer?.AddOwnedForm((Form)(object)stiEventEditorForm);
			return ((Form)stiEventEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiEventEditorForm)?.Dispose();
		}
	}

	public static DialogResult EditEventSwitch(StiReport report, StiEvent ev)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		StiEventEditorForm stiEventEditorForm = new StiEventEditorForm(report, ev);
		try
		{
			report?.Designer?.UndoRedoSave(StiLocalization.Get("FormTitles", "EventEditorForm"), onlyCurrentPage: true);
			report?.Designer?.AddOwnedForm((Form)(object)stiEventEditorForm);
			DialogResult result = ((Form)stiEventEditorForm).ShowDialog();
			if (stiEventEditorForm.SwitchToBlocks)
			{
				ev.Script = string.Empty;
				EditBlocklyEvent(report, ev);
			}
			return result;
		}
		finally
		{
			((IDisposable)stiEventEditorForm)?.Dispose();
		}
	}

	public static DialogResult EditBlocklyEvent(StiReport report, StiEvent eventValue)
	{
		IntPtr? intPtr = null;
		PointD? centerOwner = null;
		if (report?.Designer != null && report.Designer is StiDesignerControl stiDesignerControl)
		{
			intPtr = ((Control)stiDesignerControl).Handle;
			centerOwner = GetDesignerCenterPoint(stiDesignerControl);
		}
		bool showCurrentValue = eventValue is StiGetDataUrlEvent || eventValue is StiGetExcelSheetEvent || eventValue is StiGetExcelValueEvent || eventValue is Stimulsoft.Report.Events.StiGetValueEvent || eventValue is StiGetTextEvent;
		IStiBlocklyEditorWindow blocksEditorWindow = StiBlocklyEditorCreator.GetBlocklyEditorWindow(eventValue.Script, report, eventValue, showCurrentValue);
		if (blocksEditorWindow != null)
		{
			blocksEditorWindow.SwitchToCode += delegate
			{
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				StiEvent stiEvent = blocksEditorWindow.EventValue as StiEvent;
				stiEvent.Script = string.Empty;
				EditEventSwitch(blocksEditorWindow.Report as StiReport, stiEvent);
			};
			bool valueOrDefault = blocksEditorWindow.ShowDialog(intPtr, centerOwner).GetValueOrDefault();
			if (valueOrDefault)
			{
				eventValue.Script = blocksEditorWindow.Xml;
			}
			if (valueOrDefault)
			{
				return (DialogResult)1;
			}
			return (DialogResult)2;
		}
		return (DialogResult)2;
	}

	private static PointD GetDesignerCenterPoint(StiDesignerControl designer)
	{
		Form val = ((Control)designer).FindForm();
		Point point = ((Control)val).PointToScreen(Point.Empty);
		double system = StiScale.System;
		double x = (double)(point.X + ((Control)val).Width / 2) * system;
		double y = (double)(point.Y + ((Control)val).Height / 2) * system;
		return new PointD(x, y);
	}
}
