using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiColorsCollectionEditor : UITypeEditor
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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Invalid comparison between Unknown and I4
		if (!(value is Color[] colors))
		{
			return value;
		}
		StiColorsEditorForm stiColorsEditorForm = new StiColorsEditorForm(colors);
		try
		{
			Form val = ((IEnumerable)Application.OpenForms).Cast<Form>().Last();
			if (val != null)
			{
				val.AddOwnedForm((Form)(object)stiColorsEditorForm);
			}
			if ((int)((Form)stiColorsEditorForm).ShowDialog() == 1)
			{
				return stiColorsEditorForm.Colors.Clone();
			}
			return value;
		}
		finally
		{
			((IDisposable)stiColorsEditorForm)?.Dispose();
		}
	}
}
