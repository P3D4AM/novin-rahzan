using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design;

public class StiStyleConditionsCollectionEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)2;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Invalid comparison between Unknown and I4
		Array array = null;
		StiBaseStyle stiBaseStyle;
		if (context.Instance is Array)
		{
			array = context.Instance as Array;
			stiBaseStyle = array.GetValue(0) as StiBaseStyle;
		}
		else
		{
			stiBaseStyle = context.Instance as StiBaseStyle;
		}
		if (stiBaseStyle == null)
		{
			return value;
		}
		StiStyleConditionsCollection conditions = stiBaseStyle.Conditions;
		StiStyleConditionsEditorForm stiStyleConditionsEditorForm = new StiStyleConditionsEditorForm(conditions);
		try
		{
			if ((int)((Form)stiStyleConditionsEditorForm).ShowDialog() == 1)
			{
				if (array == null)
				{
					return stiBaseStyle.Conditions;
				}
				foreach (StiStyle item in array)
				{
					item.Conditions = stiBaseStyle.Conditions.Clone() as StiStyleConditionsCollection;
				}
			}
			return stiBaseStyle.Conditions;
		}
		finally
		{
			((IDisposable)stiStyleConditionsEditorForm)?.Dispose();
		}
	}
}
