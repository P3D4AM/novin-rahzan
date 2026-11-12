using System;
using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Editors;

public sealed class StiTextEditor
{
	public static DialogResult EditText(StiReport report, ref string text, StiCodeEditorMode mode, string editorTextTitle = null, bool allowPackExpression = true, string[] hints = null)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		StiTextEditorForm stiTextEditorForm = new StiTextEditorForm(report, text, mode, allowPackExpression, hints);
		try
		{
			report?.Designer?.AddOwnedForm((Form)(object)stiTextEditorForm);
			if (!string.IsNullOrEmpty(editorTextTitle))
			{
				((Control)stiTextEditorForm).Text = editorTextTitle;
			}
			DialogResult result = ((Form)stiTextEditorForm).ShowDialog();
			text = stiTextEditorForm.TextValue;
			return result;
		}
		finally
		{
			((IDisposable)stiTextEditorForm)?.Dispose();
		}
	}

	public static DialogResult EditSystemText(StiReport report, StiText textComp)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		StiSystemTextEditorForm stiSystemTextEditorForm = new StiSystemTextEditorForm(report, textComp);
		try
		{
			if (report != null && report.Designer != null)
			{
				report.Designer.AddOwnedForm((Form)(object)stiSystemTextEditorForm);
			}
			return ((Form)stiSystemTextEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiSystemTextEditorForm)?.Dispose();
		}
	}
}
