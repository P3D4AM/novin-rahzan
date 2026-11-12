using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Chart;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiChartLabelsStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private StiChartLabels chartLabels;

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Invalid comparison between Unknown and I4
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object item = val.Items[e.Index];
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
			object item = val.Items[e.Index];
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
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		chartLabels = context.Instance as StiChartLabels;
		if (chartLabels == null)
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
		foreach (StiChartLabelsStyle enumValue in typeof(StiChartLabelsStyle).GetEnumValues())
		{
			if (chartLabels.element.IsPieChart || chartLabels.element.IsPie3dChart || chartLabels.element.IsDoughnutChart || (enumValue != StiChartLabelsStyle.CategoryPercentOfTotal && enumValue != StiChartLabelsStyle.PercentOfTotal))
			{
				val.Items.Add((object)enumValue);
				if (chartLabels.Style == enumValue)
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
		if (val.SelectedItem is StiChartLabelsStyle)
		{
			return (StiChartLabelsStyle)val.SelectedItem;
		}
		return value;
	}

	private static string GetItemText(object item)
	{
		if (item is StiChartLabelsStyle)
		{
			string text = "StiChartLabelsStyle" + item.ToString();
			string text2 = StiLocalization.Get("PropertyEnum", text, throwError: false);
			return text2 ?? text;
		}
		return "";
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
				foreach (object item in listBox.Items)
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

	private static Size MeasureItem(ListBox listBox, object item)
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

	private static Size MeasureItem(object item, Graphics g, Font font)
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
