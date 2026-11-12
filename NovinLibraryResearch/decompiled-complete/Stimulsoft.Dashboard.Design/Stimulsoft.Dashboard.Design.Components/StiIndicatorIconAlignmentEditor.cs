using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Indicator;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiIndicatorIconAlignmentEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private StiIndicatorElement component;

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		StiIconAlignment item = (StiIconAlignment)val.Items[e.Index];
		Graphics graphics = e.Graphics;
		string itemText = GetItemText(item);
		if (itemText == "-")
		{
			graphics.DrawLine(Pens.LightGray, bounds.X + StiScale.I6, bounds.Y + bounds.Height / 2, bounds.Right - StiScale.I6, bounds.Y + bounds.Height / 2);
			return;
		}
		bool flag = (e.State & 1) > 0;
		Color color = (flag ? SystemColors.MenuHighlight : Color.White);
		Color color2 = (flag ? SystemColors.HighlightText : SystemColors.ControlText);
		StiDrawing.FillRectangle(graphics, color, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (itemText == null)
		{
			return;
		}
		Font val2 = new Font("Arial", 8f);
		try
		{
			StringFormat val3 = new StringFormat();
			try
			{
				SolidBrush val4 = new SolidBrush(color2);
				try
				{
					val3.Alignment = (StringAlignment)0;
					val3.LineAlignment = (StringAlignment)1;
					val3.FormatFlags = (StringFormatFlags)4096;
					val3.Trimming = (StringTrimming)3;
					graphics.DrawString(itemText, val2, (Brush)(object)val4, (RectangleF)bounds, val3);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
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
	}

	private void ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (e.Index != -1)
		{
			ListBox val = (ListBox)((sender is ListBox) ? sender : null);
			StiIconAlignment item = (StiIconAlignment)val.Items[e.Index];
			e.ItemHeight = MeasureItem(val, item).Height;
		}
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
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

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		component = context.Instance as StiIndicatorElement;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as StiIndicatorElement;
		}
		if (component == null)
		{
			return null;
		}
		ListBox val = new ListBox
		{
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2,
			ItemHeight = 19
		};
		int num = 0;
		int selectedIndex = -1;
		foreach (StiIconAlignment enumValue in typeof(StiIconAlignment).GetEnumValues())
		{
			if (enumValue != StiIconAlignment.Center)
			{
				val.Items.Add((object)enumValue);
				if (component.IconAlignment == enumValue)
				{
					selectedIndex = num;
				}
				num++;
			}
		}
		((Control)val).Size = MeasureAllItems(val);
		((ListControl)val).SelectedIndex = selectedIndex;
		((Control)val).DoubleClick += ListBox_DoubleClick;
		val.Click += ListBox_DoubleClick;
		val.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		val.MeasureItem += new MeasureItemEventHandler(ListBox_MeasureItem);
		edSvc.DropDownControl((Control)(object)val);
		if (cancel)
		{
			return value;
		}
		return (StiIconAlignment)val.SelectedItem;
	}

	private static string GetItemText(StiIconAlignment item)
	{
		string text = item.ToString();
		string text2 = StiLocalization.Get("PropertyEnum", $"StiIconAlignment{item}");
		return text2 ?? text;
	}

	private Size MeasureAllItems(ListBox listBox)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		int num = 0;
		int num2 = 0;
		Graphics val = Graphics.FromHwnd(((Control)listBox).Handle);
		try
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				foreach (StiIconAlignment item in listBox.Items)
				{
					Size size = MeasureItem(item, val, val2);
					num += size.Height;
					num2 = Math.Max(num2, size.Width);
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		return new Size(num2, num);
	}

	private static Size MeasureItem(ListBox listBox, StiIconAlignment item)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Graphics val = Graphics.FromHwnd(((Control)listBox).Handle);
		try
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				return MeasureItem(item, val, val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private static Size MeasureItem(StiIconAlignment item, Graphics g, Font font)
	{
		string itemText = GetItemText(item);
		if (itemText == "-")
		{
			return new Size(StiScale.I3, StiScale.I3);
		}
		Size result = g.MeasureString(itemText, font).ToSize();
		result.Height += StiScale.XXI(12.0);
		return result;
	}
}
