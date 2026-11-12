using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components.TextFormats.Design;

public class StiArgumentFormatEditor : UITypeEditor
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
		IStiChartElement stiChartElement = ((array == null) ? (context.Instance as IStiChartElement) : (array.GetValue(0) as IStiChartElement));
		StiFormatService stiFormatService = stiChartElement.ArgumentFormat.Clone() as StiFormatService;
		if (stiChartElement != null)
		{
			StiDesignerControl stiDesignerControl = stiChartElement.Report.Designer.DesignerControl as StiDesignerControl;
			StiText stiText = new StiText();
			stiText.TextFormat = stiFormatService.Clone() as StiFormatService;
			StiTextFormatEditorForm stiTextFormatEditorForm = new StiTextFormatEditorForm(stiText, stiDesignerControl);
			try
			{
				stiDesignerControl?.AddOwnedForm((Form)(object)stiTextFormatEditorForm);
				if ((int)((Form)stiTextFormatEditorForm).ShowDialog() != 1)
				{
					stiChartElement.ArgumentFormat = stiFormatService;
					return value;
				}
				stiChartElement.ArgumentFormat = stiText.TextFormat;
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
				foreach (IStiChartElement item in array)
				{
					item.ArgumentFormat = stiChartElement.ArgumentFormat.Clone() as StiFormatService;
				}
			}
			return value;
		}
		return null;
	}
}
