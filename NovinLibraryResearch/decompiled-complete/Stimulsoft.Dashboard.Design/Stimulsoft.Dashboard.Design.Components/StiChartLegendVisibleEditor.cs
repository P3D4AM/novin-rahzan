using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Dashboard.Components.Chart;
using Stimulsoft.Report.Design.Components;

namespace Stimulsoft.Dashboard.Design.Components;

public class StiChartLegendVisibleEditor : UITypeEditor
{
	private class DoubleBufferedListBox : ListBox
	{
		public DoubleBufferedListBox()
		{
			((Control)this).SetStyle((ControlStyles)131072, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).DoubleBuffered = true;
		}
	}

	private IWindowsFormsEditorService edSvc;

	private bool cancel;

	private int colorCellSize = StiScale.XXI(14.0);

	private int colorCellStep = StiScale.I4;

	private int hotTrackingIndex = -1;

	private const int IndexAlwaysValue = 0;

	private const int IndexAutoValue = 1;

	private const int IndexFalseValue = 2;

	private string LocAlways
	{
		get
		{
			if (!StiPropertyGridOptions.Localizable)
			{
				return "Always";
			}
			return Loc.GetMain("Always");
		}
	}

	private string LocAuto
	{
		get
		{
			if (!StiPropertyGridOptions.Localizable)
			{
				return "Auto";
			}
			return Loc.GetMain("Auto");
		}
	}

	private string LocFalse
	{
		get
		{
			if (!StiPropertyGridOptions.Localizable)
			{
				return "False";
			}
			return Loc.Get("PropertyEnum", "boolFalse");
		}
	}

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Invalid comparison between Unknown and I4
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bounds = e.Bounds;
		bounds.Width--;
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		object obj = val.Items[e.Index];
		Graphics graphics = e.Graphics;
		string text = obj as string;
		bool flag = (e.State & 1) > 0;
		bool isHotLight = e.Index == hotTrackingIndex;
		Color foregroundColor = StiListBoxPainter.GetForegroundColor(flag, isHotLight, isFocused: true);
		Color backgroundColor = StiListBoxPainter.GetBackgroundColor(flag, isHotLight, isFocused: true);
		StiDrawing.FillRectangle(graphics, backgroundColor, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		if (flag && StiUX.IsDark)
		{
			StiDrawing.FillRectangle(graphics, StiUX.Accent.ChangeAlpha(50), bounds.X, bounds.Y, bounds.Width + 1, bounds.Height);
		}
		if (text != null)
		{
			Font val2 = new Font("Arial", 8f);
			try
			{
				bounds.Inflate(-StiScale.I4, 0);
				TextFormatFlags val3 = (TextFormatFlags)4;
				TextRenderer.DrawText((IDeviceContext)(object)graphics, text, val2, bounds, foregroundColor, val3);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
	}

	private void ListBox_MeasureItem(object sender, MeasureItemEventArgs e)
	{
		if (e.Index != -1)
		{
			e.ItemWidth = GetTotalWidth();
			e.ItemHeight = colorCellSize + colorCellStep * 2;
		}
	}

	protected void ListBox_DoubleClick(object sender, EventArgs e)
	{
		cancel = false;
		edSvc.CloseDropDown();
	}

	private void ListBox_MouseLeave(object sender, EventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		hotTrackingIndex = -1;
		((Control)val).Invalidate();
	}

	private void ListBox_MouseMove(object sender, MouseEventArgs e)
	{
		ListBox val = (ListBox)((sender is ListBox) ? sender : null);
		int num = val.IndexFromPoint(e.Location);
		if (hotTrackingIndex != num)
		{
			hotTrackingIndex = num;
			((Control)val).Invalidate();
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

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		cancel = true;
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		string name = context.PropertyDescriptor.Name;
		StiChartLegend stiChartLegend = context.Instance as StiChartLegend;
		DoubleBufferedListBox doubleBufferedListBox = new DoubleBufferedListBox();
		((ListBox)doubleBufferedListBox).BorderStyle = (BorderStyle)0;
		((ListBox)doubleBufferedListBox).DrawMode = (DrawMode)2;
		DoubleBufferedListBox doubleBufferedListBox2 = doubleBufferedListBox;
		int selectedIndex = -1;
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocAlways);
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocAuto);
		((ListBox)doubleBufferedListBox2).Items.Add((object)LocFalse);
		if (stiChartLegend.Visibility == StiLegendVisibility.Always)
		{
			selectedIndex = 0;
		}
		if (stiChartLegend.Visibility == StiLegendVisibility.Auto)
		{
			selectedIndex = 1;
		}
		if (stiChartLegend.Visibility == StiLegendVisibility.False)
		{
			selectedIndex = 2;
		}
		((Control)doubleBufferedListBox2).Width = GetTotalWidth();
		((Control)doubleBufferedListBox2).Height = GetTotalHeight((ListBox)(object)doubleBufferedListBox2);
		((ListControl)doubleBufferedListBox2).SelectedIndex = selectedIndex;
		((Control)doubleBufferedListBox2).DoubleClick += ListBox_DoubleClick;
		((ListBox)doubleBufferedListBox2).Click += ListBox_DoubleClick;
		((ListBox)doubleBufferedListBox2).DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		((ListBox)doubleBufferedListBox2).MeasureItem += new MeasureItemEventHandler(ListBox_MeasureItem);
		((Control)doubleBufferedListBox2).MouseMove += new MouseEventHandler(ListBox_MouseMove);
		((Control)doubleBufferedListBox2).MouseLeave += ListBox_MouseLeave;
		edSvc.DropDownControl((Control)(object)doubleBufferedListBox2);
		if (cancel)
		{
			return value;
		}
		string text = ((ListBox)doubleBufferedListBox2).SelectedItem?.ToString();
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 0)
		{
			stiChartLegend.Visibility = StiLegendVisibility.Always;
			return stiChartLegend.Visible;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 1)
		{
			stiChartLegend.Visibility = StiLegendVisibility.Auto;
			return stiChartLegend.Visible;
		}
		if (((ListControl)doubleBufferedListBox2).SelectedIndex == 2)
		{
			stiChartLegend.Visibility = StiLegendVisibility.False;
			return stiChartLegend.Visible;
		}
		return value;
	}

	private int GetTotalWidth()
	{
		return (colorCellSize + colorCellStep) * 6 + colorCellStep;
	}

	private int GetTotalHeight(ListBox listBox)
	{
		return Math.Min(listBox.Items.Count, 12) * (colorCellSize + colorCellStep * 2);
	}
}
