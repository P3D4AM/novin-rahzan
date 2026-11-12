using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Design.Editors;

namespace Stimulsoft.Report.Components.Design;

public class StiTextExpressionEditor : StiExpressionEditor
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
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		Array array = null;
		StiComponent stiComponent;
		if (context.Instance is Array)
		{
			array = context.Instance as Array;
			stiComponent = array.GetValue(0) as StiComponent;
		}
		else
		{
			stiComponent = context.Instance as StiComponent;
		}
		if (stiComponent == null)
		{
			return value;
		}
		DialogResult val = StiTextEditor.EditSystemText(stiComponent.Report, stiComponent as StiText);
		if ((int)val == 1)
		{
			if (array != null)
			{
				foreach (StiComponent item in array)
				{
					((StiText)item).Text.Value = ((StiText)stiComponent).Text.Value;
				}
			}
			if (context.PropertyDescriptor.Name == "Text" && value is StiExpression)
			{
				((StiExpression)value).Value = ((StiText)stiComponent).Text.Value;
				return value;
			}
		}
		return value;
	}
}
