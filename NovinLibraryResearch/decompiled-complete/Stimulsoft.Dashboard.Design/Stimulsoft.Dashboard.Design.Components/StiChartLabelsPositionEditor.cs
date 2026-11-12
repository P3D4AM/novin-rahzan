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
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiChartLabelsPositionEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private StiChartLabels component;

	private static string LocEditStyles => Loc.Get("Report", "EditStyles");

	private StiReport Report => (component as IStiReportComponent).GetApp() as StiReport;

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object obj = ((e.Index >= 0) ? val.Items[e.Index] : null);
		Graphics graphics = e.Graphics;
		string text = obj?.ToString();
		if (text == "-")
		{
			graphics.DrawLine(Pens.LightGray, bounds.X + StiScale.I6, bounds.Y + bounds.Height / 2, bounds.Right - StiScale.I6, bounds.Y + bounds.Height / 2);
			return;
		}
		bool flag = (e.State & 1) > 0;
		Color color = (flag ? SystemColors.MenuHighlight : Color.White);
		Color color2 = (flag ? SystemColors.HighlightText : SystemColors.ControlText);
		StiDrawing.FillRectangle(graphics, color, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (text == null)
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
					graphics.DrawString(text, val2, (Brush)(object)val4, (RectangleF)bounds, val3);
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
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		if (val.SelectedItem is string && val.SelectedItem as string == LocEditStyles && Report != null)
		{
			StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(Report.Designer.DesignerControl as StiDesignerControl, Report);
			try
			{
				if (Report.Designer != null && Report.Designer.DesignerControl != null)
				{
					Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiStyleDesigner);
				}
				((Form)stiStyleDesigner).ShowDialog();
			}
			finally
			{
				((IDisposable)stiStyleDesigner)?.Dispose();
			}
			if (Report.Designer != null)
			{
				Report.Designer.Refresh();
			}
			Report.ApplyStyles();
			cancel = true;
		}
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
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		component = context.Instance as StiChartLabels;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as StiChartLabels;
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
		int selectedIndex = -1;
		StiChartElement element = component.element;
		if (element.IsAxisAreaChart)
		{
			val.Items.AddRange(new object[12]
			{
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.InsideEnd),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.InsideBase),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Center),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.OutsideEnd),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.OutsideBase),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Outside),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Left),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Value),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Right)
			});
			if (element.IsStackedChart)
			{
				val.Items.Add((object)"-");
				val.Items.Add((object)StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Total));
			}
		}
		if (element.IsWaterfallChart)
		{
			val.Items.AddRange(new object[8]
			{
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.InsideEnd),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.InsideBase),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Center),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.OutsideEnd),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.OutsideBase),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Outside)
			});
		}
		else if (element.IsPieChart)
		{
			val.Items.AddRange(new object[7]
			{
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.InsideEnd),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Center),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Outside),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.TwoColumns)
			});
		}
		else if (element.IsPie3dChart)
		{
			val.Items.AddRange(new object[3]
			{
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Center)
			});
		}
		else if (element.IsFunnelChart)
		{
			val.Items.AddRange(new object[5]
			{
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Left),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Center),
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Right)
			});
		}
		else if (element.IsTreemapChart || element.IsRadarChart || element.IsDoughnutChart || element.IsSunburstChart)
		{
			val.Items.AddRange(new object[3]
			{
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None),
				"-",
				StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.Center)
			});
		}
		else
		{
			StiChartLabelsPositionConverter.ConvertPositionToString(StiChartLabelsPosition.None);
		}
		int num = 0;
		foreach (string item in val.Items)
		{
			if (item == StiChartLabelsPositionConverter.ConvertPositionToString((StiChartLabelsPosition)value))
			{
				selectedIndex = num;
			}
			num++;
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
		string text2 = val.SelectedItem as string;
		if (text2 == "-")
		{
			return value;
		}
		return StiChartLabelsPositionConverter.ConvertStringToPosition(text2);
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
		string text = item.ToString();
		if (text == "-")
		{
			return new Size(StiScale.I3, StiScale.I3);
		}
		Size result = g.MeasureString(text, font).ToSize();
		result.Height += StiScale.XXI(12.0);
		return result;
	}
}
