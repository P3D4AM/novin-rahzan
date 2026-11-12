using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiElementStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private IStiDashboardElementStyle component;

	private static string LocEditStyles => Loc.Get("Report", "EditStyles");

	private StiReport Report => (component as IStiReportComponent).GetApp() as StiReport;

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
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		if (val.SelectedItem is string && val.SelectedItem as string == LocEditStyles && Report != null)
		{
			StiStyleDesigner stiStyleDesigner = new StiStyleDesigner(Report.Designer.DesignerControl as StiDesignerControl, Report);
			try
			{
				Report?.Designer?.DesignerControl?.AddOwnedForm((Form)(object)stiStyleDesigner);
				((Form)stiStyleDesigner).ShowDialog();
			}
			finally
			{
				((IDisposable)stiStyleDesigner)?.Dispose();
			}
			Report?.Designer?.Refresh();
			Report?.ApplyStyles();
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
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = false;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		component = context.Instance as IStiDashboardElementStyle;
		Array array = context.Instance as Array;
		if (component == null && array != null)
		{
			component = array.GetValue(0) as IStiDashboardElementStyle;
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
		if (!(component is IStiDashboard))
		{
			val.Items.Add((object)StiElementStyleIdent.Auto);
			val.Items.Add((object)"-");
			if (component.Style == StiElementStyleIdent.Auto)
			{
				selectedIndex = num;
			}
			num += 2;
		}
		List<StiBaseStyle> reportStyles = GetReportStyles();
		if (reportStyles.Any())
		{
			foreach (StiBaseStyle item in reportStyles)
			{
				val.Items.Add((object)item);
				if (component.Style == StiElementStyleIdent.Custom && component.CustomStyleName == item.Name)
				{
					selectedIndex = num;
				}
				num++;
			}
			val.Items.Add((object)"-");
			num++;
		}
		foreach (StiElementStyleIdent enumValue in typeof(StiElementStyleIdent).GetEnumValues())
		{
			if (enumValue != StiElementStyleIdent.Custom && enumValue != StiElementStyleIdent.Auto)
			{
				val.Items.Add((object)enumValue);
				if (component.Style == enumValue)
				{
					selectedIndex = num;
				}
				num++;
			}
		}
		if (StiOptions.Designer.PropertyGrid.AllowEditStyle)
		{
			val.Items.Add((object)"-");
			val.Items.Add((object)LocEditStyles);
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
		if (val.SelectedItem is StiElementStyleIdent)
		{
			return (StiElementStyleIdent)val.SelectedItem;
		}
		if (val.SelectedItem is StiBaseStyle)
		{
			StiBaseStyle stiBaseStyle = val.SelectedItem as StiBaseStyle;
			component.CustomStyleName = stiBaseStyle.Name;
			component.Style = StiElementStyleIdent.Custom;
			return StiElementStyleIdent.Custom;
		}
		return value;
	}

	private List<StiBaseStyle> GetReportStyles()
	{
		List<StiBaseStyle> list = Report.Styles.ToList();
		if (component is IStiChartElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiChartStyle).ToList();
		}
		if (component is IStiIndicatorElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiIndicatorStyle).ToList();
		}
		if (component is IStiProgressElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiProgressStyle).ToList();
		}
		else if (component is IStiGaugeElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiGaugeStyle).ToList();
		}
		else if (component is IStiRegionMapElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiMapStyle).ToList();
		}
		else if (component is IStiTableElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiTableStyle).ToList();
		}
		else if (component is IStiPivotTableElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiCrossTabStyle).ToList();
		}
		else if (component is IStiControlElement)
		{
			list = list.Where((StiBaseStyle s) => s is StiDialogStyle).ToList();
		}
		else
		{
			list.Clear();
		}
		return list;
	}

	private static string GetItemText(object item)
	{
		if (item is StiElementStyleIdent)
		{
			if ((StiElementStyleIdent)item == StiElementStyleIdent.Auto)
			{
				return Loc.GetMain("Auto");
			}
			string text = item.ToString();
			string text2 = StiLocalization.Get("PropertyColor", text, throwError: false);
			return text2 ?? text;
		}
		if (item is StiBaseStyle)
		{
			return (item as StiBaseStyle).Name;
		}
		if (item is string)
		{
			return item as string;
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
