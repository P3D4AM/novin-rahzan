using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design.Components;

public class StiFontEditor : UITypeEditor
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
		return ChangeFont(context, (Font)((value is Font) ? value : null));
	}

	public static Font ChangeFont(ITypeDescriptorContext context, Font font)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Invalid comparison between Unknown and I4
		StiReport stiReport = FindReportInContext(context);
		StiFontEditorForm stiFontEditorForm = new StiFontEditorForm(font, stiReport);
		try
		{
			stiReport?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiFontEditorForm);
			if ((int)((Form)stiFontEditorForm).ShowDialog() == 1)
			{
				return stiFontEditorForm.SelectedFont;
			}
			return font;
		}
		finally
		{
			((IDisposable)stiFontEditorForm)?.Dispose();
		}
	}

	public static StiReport FindReportInContext(ITypeDescriptorContext context)
	{
		StiReport stiReport = ((context.Instance is StiComponent stiComponent) ? stiComponent.Report : null);
		if (stiReport != null)
		{
			return stiReport;
		}
		StiReportPropertyGrid stiReportPropertyGrid = context.GetType()?.GetProperty("OwnerGrid")?.GetValue(context) as StiReportPropertyGrid;
		Control val = ((Control)stiReportPropertyGrid).Parent;
		while (val != null && !(val is StiStyleDesigner) && !(val is StiDesignerControl))
		{
			val = ((val != null) ? val.Parent : null);
		}
		if (!(val is StiStyleDesigner))
		{
			return (val as StiDesignerControl)?.Report;
		}
		return (val as StiStyleDesigner)?.Report;
	}
}
