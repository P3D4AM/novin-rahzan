using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public class StiStylesEditor : UITypeEditor
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
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Invalid comparison between Unknown and I4
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		if (context.Instance is StiTableOfContents)
		{
			StiTableOfContents stiTableOfContents = context.Instance as StiTableOfContents;
			StiReport report = stiTableOfContents.Report;
			List<StiBaseStyle> styles = report.Styles.ToList();
			try
			{
				report.Styles.Clear();
				report.Styles.AddRange(stiTableOfContents.Styles);
				StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(report.Designer.DesignerControl as StiDesignerControl, report);
				try
				{
					report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiStyleDesigner);
					if ((int)((Form)stiStyleDesigner).ShowDialog() == 1)
					{
						stiTableOfContents.Styles.Clear();
						stiTableOfContents.Styles.AddRange(report.Styles);
					}
				}
				finally
				{
					((IDisposable)stiStyleDesigner)?.Dispose();
				}
			}
			finally
			{
				report.Styles.Clear();
				report.Styles.AddRange(styles);
			}
			return stiTableOfContents.Styles;
		}
		if (!(context.Instance is StiReport stiReport))
		{
			return null;
		}
		StiStyleDesigner stiStyleDesigner2 = new StiStyleDesigner(stiReport.Designer.DesignerControl as StiDesignerControl, stiReport);
		try
		{
			stiReport.Designer.DesignerControl.AddOwnedForm((Form)(object)stiStyleDesigner2);
			((Form)stiStyleDesigner2).ShowDialog();
		}
		finally
		{
			((IDisposable)stiStyleDesigner2)?.Dispose();
		}
		stiReport.Designer?.Refresh();
		stiReport.ApplyStyles();
		return stiReport.Styles;
	}
}
