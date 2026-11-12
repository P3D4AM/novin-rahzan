using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiConditionsCollectionEditor : UITypeEditor
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
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Invalid comparison between Unknown and I4
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
		StiConditionsCollection conditions = ((IStiConditions)stiComponent).Conditions;
		StiConditionsEditorForm stiConditionsEditorForm = new StiConditionsEditorForm(stiComponent.Report, conditions, stiComponent);
		try
		{
			if (stiComponent.Report != null && stiComponent.Report.Designer != null)
			{
				stiComponent.Report.Designer.AddOwnedForm((Form)(object)stiConditionsEditorForm);
			}
			if ((int)((Form)stiConditionsEditorForm).ShowDialog() == 1 && array != null)
			{
				StiConditionsCollection conditions2 = ((IStiConditions)stiComponent).Conditions;
				foreach (StiComponent item in array)
				{
					item.Conditions = (StiConditionsCollection)conditions2.Clone();
				}
			}
			return ((IStiConditions)stiComponent).Conditions;
		}
		finally
		{
			((IDisposable)stiConditionsEditorForm)?.Dispose();
		}
	}
}
