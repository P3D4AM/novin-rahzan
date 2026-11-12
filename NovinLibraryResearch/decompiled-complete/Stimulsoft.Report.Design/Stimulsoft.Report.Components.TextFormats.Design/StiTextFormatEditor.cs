using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiTextFormatEditor : UITypeEditor
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
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Invalid comparison between Unknown and I4
		Array array = context.Instance as Array;
		IStiTextFormat stiTextFormat = ((array != null) ? (array.GetValue(0) as IStiTextFormat) : (context.Instance as IStiTextFormat));
		StiFormatService textFormat = stiTextFormat.TextFormat.Clone() as StiFormatService;
		if (stiTextFormat != null)
		{
			StiDesignerControl stiDesignerControl = null;
			if (stiTextFormat is StiComponent)
			{
				StiComponent stiComponent = stiTextFormat as StiComponent;
				stiDesignerControl = stiComponent.Report.Designer.DesignerControl as StiDesignerControl;
			}
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(stiTextFormat, stiDesignerControl);
			try
			{
				if (stiDesignerControl != null)
				{
					stiDesignerControl.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				}
				else
				{
					Form val = ((IEnumerable)Application.OpenForms).Cast<Form>().Last();
					if (val != null)
					{
						val.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
					}
				}
				if ((int)((Form)stiTextFormatEditorForm).ShowDialog() == 2)
				{
					stiTextFormat.TextFormat = textFormat;
					return value;
				}
			}
			finally
			{
				((IDisposable)stiTextFormatEditorForm)?.Dispose();
			}
			if (array != null)
			{
				foreach (StiComponent item in array)
				{
					((IStiTextFormat)item).TextFormat = stiTextFormat.TextFormat.Clone() as StiFormatService;
				}
			}
			return value;
		}
		return null;
	}
}
