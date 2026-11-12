using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Design;

public class StiDialogStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private StiComponent component;

	private List<StiDialogStyle> styles;

	public override void PaintValue(PaintValueEventArgs e)
	{
		if (e.Value == null)
		{
			return;
		}
		string name = e.Value as string;
		if (e.Context.Instance is StiComponent { Report: not null } stiComponent)
		{
			StiBaseStyle stiBaseStyle = stiComponent.Report.Styles[name];
			if (stiBaseStyle != null)
			{
				StiBaseStyle.DrawBox(e.Graphics, e.Bounds, stiBaseStyle, paintValue: true, paintImage: true);
			}
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		if (bounds.Height > 0 && e.Index >= 2)
		{
			StiDialogStyle style = styles[e.Index - 2];
			StiBaseStyle.DrawStyle(e.Graphics, bounds, style, e.State, fillBackground: true, paintImage: true);
		}
		else
		{
			if (e.Index >= 2)
			{
				return;
			}
			Font val = new Font("Arial", 8f);
			try
			{
				string text = StiLocalization.Get("Report", "EditStyles");
				if (e.Index == 0)
				{
					text = StiLocalization.Get("Report", "No");
				}
				StiControlPaint.DrawItem(e.Graphics, bounds, e.State, text, null, -1, val, SystemColors.Window, SystemColors.ControlText, 0, (RightToLeft)0, (StringAlignment)1);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (context == null || context.Instance == null)
		{
			return ((UITypeEditor)this).GetEditStyle(context);
		}
		return (UITypeEditorEditStyle)3;
	}

	private int GetMaxWidth(Graphics g, Font font)
	{
		int num = 0;
		foreach (StiDialogStyle style in styles)
		{
			int val = (int)g.MeasureString(style.Name, font).Width;
			num = Math.Max(num, val);
		}
		return StiScale.XXI(num + 10);
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		if (((ListControl)val).SelectedIndex == 1 && component.Report != null)
		{
			StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(component.Report.Designer.DesignerControl as StiDesignerControl, component.Report);
			try
			{
				if (component.Report.Designer != null && component.Report.Designer.DesignerControl != null)
				{
					component.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiStyleDesigner);
				}
				((Form)stiStyleDesigner).ShowDialog();
			}
			finally
			{
				((IDisposable)stiStyleDesigner)?.Dispose();
			}
			if (component.Report.Designer != null)
			{
				component.Report.Designer.Refresh();
			}
			component.Report.ApplyStyles();
			cancel = true;
		}
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		component = context.Instance as StiComponent;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as StiComponent;
		}
		if (component == null)
		{
			return null;
		}
		styles = new List<StiDialogStyle>();
		foreach (StiBaseStyle style in component.Report.Styles)
		{
			if (style is StiDialogStyle)
			{
				styles.Add(style as StiDialogStyle);
			}
		}
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2,
			ItemHeight = StiScale.YYI(25.0)
		};
		Graphics val2 = Graphics.FromHwnd(((Control)val).Handle);
		try
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				((Control)val).Width = GetMaxWidth(val2, val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		((ListControl)val).SelectedIndex = -1;
		int num = 2;
		val.Items.Add((object)string.Empty);
		val.Items.Add((object)StiLocalization.Get("Report", "EditStyles"));
		foreach (StiDialogStyle style2 in styles)
		{
			if (!string.IsNullOrEmpty(style2.Name))
			{
				val.Items.Add((object)style2.Name);
				if (value != null && style2.Name.ToLowerInvariant() == ((string)value).ToLowerInvariant())
				{
					((ListControl)val).SelectedIndex = num;
				}
				num++;
			}
		}
		((Control)val).Height = Math.Min(val.ItemHeight * val.Items.Count, StiScale.YYI(300.0));
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		val.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		edSvc.DropDownControl((Control)(object)val);
		if (!cancel)
		{
			return val.SelectedItem;
		}
		return value;
	}
}
