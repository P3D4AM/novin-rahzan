using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiTextFormatGaugeElementEditor : UITypeEditor
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
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		Array array = context.Instance as Array;
		IStiGaugeElement stiGaugeElement = ((array == null) ? (context.Instance as IStiGaugeElement) : (array.GetValue(0) as IStiGaugeElement));
		StiFormatService stiFormatService = stiGaugeElement.ValueFormat.Clone() as StiFormatService;
		if (stiGaugeElement != null)
		{
			StiDesignerControl stiDesignerControl = stiGaugeElement.Report.Designer.DesignerControl as StiDesignerControl;
			StiText stiText = new StiText();
			stiText.TextFormat = stiFormatService.Clone() as StiFormatService;
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(stiText, stiDesignerControl);
			try
			{
				stiDesignerControl?.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				if ((int)((Form)stiTextFormatEditorForm).ShowDialog() != 1)
				{
					stiGaugeElement.ValueFormat = stiFormatService;
					return value;
				}
				stiGaugeElement.ValueFormat = stiText.TextFormat;
				if (stiDesignerControl != null)
				{
					((Control)stiDesignerControl).Refresh();
				}
				stiDesignerControl?.Builder.Redraw();
			}
			finally
			{
				((IDisposable)stiTextFormatEditorForm)?.Dispose();
			}
			if (array != null)
			{
				foreach (IStiGaugeElement item in array)
				{
					item.ValueFormat = stiGaugeElement.ValueFormat.Clone() as StiFormatService;
				}
			}
			return value;
		}
		return null;
	}
}
