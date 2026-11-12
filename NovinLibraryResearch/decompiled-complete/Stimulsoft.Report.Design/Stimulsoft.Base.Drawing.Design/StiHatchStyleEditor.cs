using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Base.Drawing.Design;

public class StiHatchStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	private void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		StiHatchStyleHelper.DrawHatchStyle(e);
	}

	internal void ListBox_DoubleClick(object sender, EventArgs e)
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

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		if (e.Value != null)
		{
			Rectangle bounds = e.Bounds;
			e.Graphics.FillRectangle((Brush)new HatchBrush((HatchStyle)e.Value, Color.Black, Color.White), bounds);
		}
	}

	public override bool GetPaintValueSupported(ITypeDescriptorContext context)
	{
		return true;
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		HatchStyle val = StiBrushes.HatchStyles[0];
		if (value != null)
		{
			val = (HatchStyle)value;
		}
		ListBox val2 = new ListBox
		{
			Height = (int)Math.Ceiling(300.0 * StiScale.Y),
			BorderStyle = (BorderStyle)0,
			DrawMode = (DrawMode)2,
			ItemHeight = (int)Math.Ceiling(19.0 * StiScale.Y)
		};
		int num = 0;
		int num2 = 0;
		Graphics val3 = Graphics.FromHwnd(((Control)val2).Handle);
		try
		{
			Font val4 = new Font("Arial", 8f);
			try
			{
				HatchStyle[] hatchStyles = StiBrushes.HatchStyles;
				foreach (HatchStyle val5 in hatchStyles)
				{
					num = Math.Max(num, StiHatchStyleHelper.GetWidthOfHatchStyle(val3, val4, num2));
					val2.Items.Add((object)val5);
					if (val == val5)
					{
						((ListControl)val2).SelectedIndex = num2;
					}
					num2++;
				}
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
		((Control)val2).Width = num;
		val2.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
		((Control)val2).DoubleClick += ListBox_DoubleClick;
		val2.Click += ListBox_DoubleClick;
		edSvc.DropDownControl((Control)(object)val2);
		return StiBrushes.HatchStyles[((ListControl)val2).SelectedIndex];
	}
}
