using System;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;

namespace Stimulsoft.Report.Design.Editors;

public sealed class StiRichTextEditor
{
	public static DialogResult EditText(StiReport report, ref string text, bool showExpressionButton, bool isPacked = true, bool showOpenSaveButtons = true)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		string text2 = (isPacked ? StiRichText.UnpackRtf(XmlConvert.DecodeName(text)) : text);
		StiRichTextEditorForm stiRichTextEditorForm = new StiRichTextEditorForm(report, text2, showOpenSaveButtons);
		try
		{
			stiRichTextEditorForm.ShowExpressionButton = showExpressionButton;
			if (report != null && report.Designer != null)
			{
				report.Designer.AddOwnedForm((Form)(object)stiRichTextEditorForm);
			}
			DialogResult result = ((Form)stiRichTextEditorForm).ShowDialog();
			text = (isPacked ? XmlConvert.EncodeName(StiRichText.PackRtf(stiRichTextEditorForm.TextValue)) : stiRichTextEditorForm.TextValue);
			return result;
		}
		finally
		{
			((IDisposable)stiRichTextEditorForm)?.Dispose();
		}
	}

	public static DialogResult EditSystemText(StiReport report, StiRichText textComp, StiRichTextEditorStartTab? startTab = null)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		StiSystemRichTextEditorForm stiSystemRichTextEditorForm = new StiSystemRichTextEditorForm(report, textComp, startTab);
		try
		{
			if (report != null && report.Designer != null)
			{
				report.Designer.AddOwnedForm((Form)(object)stiSystemRichTextEditorForm);
			}
			return ((Form)stiSystemRichTextEditorForm).ShowDialog();
		}
		finally
		{
			((IDisposable)stiSystemRichTextEditorForm)?.Dispose();
		}
	}
}
