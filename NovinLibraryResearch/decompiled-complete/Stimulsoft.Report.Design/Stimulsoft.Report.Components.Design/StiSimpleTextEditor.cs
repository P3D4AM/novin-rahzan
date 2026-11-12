using System;
using System.ComponentModel;
using System.Drawing.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Components.Design;

public class StiSimpleTextEditor : UITypeEditor
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
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		string text = value as string;
		if ((int)StiTextEditor.EditText(null, ref text, StiCodeEditorMode.EditorText) == 2)
		{
			return value;
		}
		return text;
	}
}
