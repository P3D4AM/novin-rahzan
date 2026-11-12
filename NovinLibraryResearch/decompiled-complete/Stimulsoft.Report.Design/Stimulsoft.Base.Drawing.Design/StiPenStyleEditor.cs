using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Base.Drawing.Design;

public class StiPenStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	protected void ListBox_DrawItem(object sender, DrawItemEventArgs e)
	{
		StiPenStyleHelper.DrawPenStyle(e);
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

	public override void PaintValue(PaintValueEventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Pen val = new Pen(Color.Black, 2f);
		try
		{
			StiPenStyle stiPenStyle = StiPenStyle.Solid;
			if (e.Value != null)
			{
				stiPenStyle = (StiPenStyle)e.Value;
			}
			val.DashStyle = StiPenUtils.GetPenStyle(stiPenStyle);
			graphics.FillRectangle(Brushes.White, (int)Math.Ceiling(2.0 * StiScale.X), bounds.Top + (int)Math.Ceiling(1.0 * StiScale.Y), (int)Math.Ceiling(19.0 * StiScale.X), (int)Math.Ceiling(11.0 * StiScale.Y));
			int num = bounds.Top + bounds.Height / 2;
			switch (stiPenStyle)
			{
			case StiPenStyle.Double:
				val.Width = 1f;
				graphics.DrawLine(val, 2, num - (int)Math.Ceiling(1.0 * StiScale.Y), bounds.Width, num - (int)Math.Ceiling(1.0 * StiScale.Y));
				graphics.DrawLine(val, 2, num + (int)Math.Ceiling(1.0 * StiScale.Y), bounds.Width, num + (int)Math.Ceiling(1.0 * StiScale.Y));
				break;
			default:
				graphics.DrawLine(val, 2, num, bounds.Width, num);
				break;
			case StiPenStyle.None:
				break;
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
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
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		Array values = Enum.GetValues(typeof(StiPenStyle));
		StiPenStyle selectedIndex = StiPenStyle.Solid;
		if (value != null)
		{
			selectedIndex = (StiPenStyle)value;
		}
		ListBox val = new ListBox();
		try
		{
			val.BorderStyle = (BorderStyle)0;
			val.DrawMode = (DrawMode)2;
			val.ItemHeight = (int)Math.Ceiling(19.0 * StiScale.Y);
			((Control)val).Height = val.ItemHeight * values.Length;
			if (((Control)val).Height > (int)Math.Ceiling(300.0 * StiScale.Y))
			{
				((Control)val).Height = (int)Math.Ceiling(300.0 * StiScale.Y);
			}
			Graphics val2 = Graphics.FromHwnd(((Control)val).Handle);
			try
			{
				Font val3 = new Font("Arial", 8f);
				try
				{
					((Control)val).Width = StiPenStyleHelper.GetMaxWidthOfPenStyles(val2, val3);
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
			for (int i = 0; i < values.Length; i++)
			{
				val.Items.Add((object)((StiPenStyle)values.GetValue(i)).ToString());
			}
			((ListControl)val).SelectedIndex = (int)selectedIndex;
			val.DrawItem += new DrawItemEventHandler(ListBox_DrawItem);
			((Control)val).DoubleClick += ListBox_DoubleClick;
			val.Click += ListBox_DoubleClick;
			edSvc.DropDownControl((Control)(object)val);
			return (StiPenStyle)values.GetValue(((ListControl)val).SelectedIndex);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
