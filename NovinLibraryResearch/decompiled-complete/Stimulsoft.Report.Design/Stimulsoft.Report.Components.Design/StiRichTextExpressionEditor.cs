using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Design.Editors;
using Stimulsoft.Report.Dialogs;

namespace Stimulsoft.Report.Components.Design;

public class StiRichTextExpressionEditor : StiExpressionEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return base.GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Invalid comparison between Unknown and I4
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Invalid comparison between Unknown and I4
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Invalid comparison between Unknown and I4
		StiComponent stiComponent = context.Instance as StiComponent;
		if (stiComponent is IStiRichTextBoxControl)
		{
			string text = value as string;
			DialogResult val = StiRichTextEditor.EditText(stiComponent.Report, ref text, showExpressionButton: false);
			if ((int)val == 1)
			{
				return text;
			}
		}
		else if (!(stiComponent is StiRichText stiRichText))
		{
			string text2 = ((StiExpression)value).Value;
			if ((int)StiRichTextEditor.EditText(stiComponent.Report, ref text2, showExpressionButton: true) == 1)
			{
				((StiExpression)value).Value = text2;
				return value;
			}
		}
		else if (context.PropertyDescriptor.Name == "Text")
		{
			if ((int)StiRichTextEditor.EditSystemText(stiComponent.Report, (StiRichText)stiComponent, StiRichTextEditorStartTab.Text) == 1)
			{
				return stiRichText.Text;
			}
		}
		else if (context.PropertyDescriptor.Name == "DataColumn")
		{
			if ((int)StiRichTextEditor.EditSystemText(stiComponent.Report, (StiRichText)stiComponent, StiRichTextEditorStartTab.DataColumn) == 1)
			{
				return stiRichText.DataColumn;
			}
		}
		else if (context.PropertyDescriptor.Name == "DataUrl" && (int)StiRichTextEditor.EditSystemText(stiComponent.Report, (StiRichText)stiComponent, StiRichTextEditorStartTab.DataUrl) == 1)
		{
			return stiRichText.DataUrl;
		}
		return value;
	}
}
