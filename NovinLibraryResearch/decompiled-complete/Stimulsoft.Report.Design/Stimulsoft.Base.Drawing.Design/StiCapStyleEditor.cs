using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Base.Drawing.Design;

public class StiCapStyleEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	protected void OnDoubleClick(object sender, EventArgs e)
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
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Expected O, but got Unknown
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		Pen val = new Pen(Color.DimGray, 1f);
		try
		{
			StiCapStyle stiCapStyle = StiCapStyle.Arrow;
			if (e.Value != null)
			{
				stiCapStyle = (StiCapStyle)e.Value;
			}
			val.DashStyle = (DashStyle)0;
			graphics.FillRectangle(Brushes.White, (int)Math.Ceiling(2.0 * StiScale.X), bounds.Top + 1, (int)Math.Ceiling(19.0 * StiScale.X), (int)Math.Ceiling(11.0 * StiScale.Y));
			int num = bounds.Top + bounds.Height / 2;
			graphics.DrawLine(val, (int)Math.Ceiling(4.0 * StiScale.X), num, bounds.Width - (int)Math.Ceiling(4.0 * StiScale.X), num);
			PointF[] array = null;
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)4;
			switch (stiCapStyle)
			{
			case StiCapStyle.Arrow:
			{
				Rectangle rectangle6 = new Rectangle(bounds.Right - (int)Math.Ceiling(9.0 * StiScale.X), bounds.Y + (int)Math.Ceiling(1.0 * StiScale.Y), (int)Math.Ceiling(5.0 * StiScale.X), (int)Math.Ceiling(11.0 * StiScale.Y));
				graphics.SmoothingMode = smoothingMode;
				array = new PointF[3]
				{
					new PointF(rectangle6.Right, (float)rectangle6.Y + (float)(rectangle6.Height / 2)),
					new PointF(rectangle6.Left, rectangle6.Y),
					new PointF(rectangle6.Left, rectangle6.Bottom)
				};
				SolidBrush val7 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillPolygon((Brush)(object)val7, array);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Open:
			{
				Rectangle rectangle5 = new Rectangle(bounds.Right - (int)Math.Ceiling(9.0 * StiScale.X), bounds.Y + (int)Math.Ceiling(3.0 * StiScale.Y), (int)Math.Ceiling(5.0 * StiScale.X), (int)Math.Ceiling(7.0 * StiScale.Y));
				array = new PointF[3]
				{
					new PointF(rectangle5.X, rectangle5.Y),
					new PointF(rectangle5.Right, (float)rectangle5.Y + (float)(rectangle5.Height / 2)),
					new PointF(rectangle5.X, rectangle5.Bottom)
				};
				Pen val6 = new Pen(Color.DimGray);
				try
				{
					graphics.DrawLines(val6, array);
				}
				finally
				{
					((IDisposable)val6)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Stealth:
			{
				Rectangle rectangle4 = new Rectangle(bounds.Right - (int)Math.Ceiling(14.0 * StiScale.X), bounds.Y + (int)Math.Ceiling(1.0 * StiScale.Y), (int)Math.Ceiling(10.0 * StiScale.X), (int)Math.Ceiling(11.0 * StiScale.Y));
				array = new PointF[5]
				{
					new PointF(rectangle4.X, rectangle4.Y),
					new PointF(rectangle4.Right, (float)rectangle4.Y + (float)(rectangle4.Height / 2)),
					new PointF(rectangle4.X, rectangle4.Bottom),
					new PointF((float)rectangle4.X + (float)(rectangle4.Width / 3), (float)rectangle4.Y + (float)(rectangle4.Height / 2)),
					new PointF(rectangle4.X, rectangle4.Y)
				};
				SolidBrush val5 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillPolygon((Brush)(object)val5, array);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Diamond:
			{
				Rectangle rectangle3 = new Rectangle(bounds.Right - (int)Math.Ceiling(9.0 * StiScale.X), num - (int)Math.Ceiling(3.0 * StiScale.Y), (int)Math.Ceiling(7.0 * StiScale.X), (int)Math.Ceiling(7.0 * StiScale.Y));
				array = new PointF[4]
				{
					new PointF(rectangle3.X, (float)rectangle3.Y + (float)(rectangle3.Height / 2)),
					new PointF((float)rectangle3.X + (float)(rectangle3.Width / 2), rectangle3.Y),
					new PointF(rectangle3.Right, (float)rectangle3.Y + (float)(rectangle3.Height / 2)),
					new PointF((float)rectangle3.X + (float)(rectangle3.Width / 2), rectangle3.Bottom)
				};
				SolidBrush val4 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillPolygon((Brush)(object)val4, array);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Oval:
			{
				Rectangle rectangle2 = new Rectangle(bounds.Right - (int)Math.Ceiling(9.0 * StiScale.X), num - (int)Math.Ceiling(3.0 * StiScale.Y), (int)Math.Ceiling(6.0 * StiScale.X), (int)Math.Ceiling(6.0 * StiScale.Y));
				SolidBrush val3 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillEllipse((Brush)(object)val3, rectangle2);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				break;
			}
			case StiCapStyle.Square:
			{
				Rectangle rectangle = new Rectangle(bounds.Right - (int)Math.Ceiling(9.0 * StiScale.X), num - (int)Math.Ceiling(3.0 * StiScale.Y), (int)Math.Ceiling(7.0 * StiScale.X), (int)Math.Ceiling(7.0 * StiScale.Y));
				graphics.SmoothingMode = smoothingMode;
				SolidBrush val2 = new SolidBrush(Color.DimGray);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				break;
			}
			}
			graphics.SmoothingMode = smoothingMode;
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
		Array values = Enum.GetValues(typeof(StiCapStyle));
		StiCapStyle selectedIndex = StiCapStyle.Arrow;
		if (value != null)
		{
			selectedIndex = (StiCapStyle)value;
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
					((Control)val).Width = StiCapStyleHelper.GetMaxWidthOfCapStyles(val2, val3);
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
				val.Items.Add((object)((StiCapStyle)values.GetValue(i)).ToString());
			}
			((ListControl)val).SelectedIndex = (int)selectedIndex;
			val.DrawItem += new DrawItemEventHandler(DrawItem);
			((Control)val).DoubleClick += OnDoubleClick;
			val.Click += OnDoubleClick;
			edSvc.DropDownControl((Control)(object)val);
			return (StiCapStyle)values.GetValue(((ListControl)val).SelectedIndex);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected void DrawItem(object sender, DrawItemEventArgs e)
	{
		StiCapStyleHelper.DrawCapStyle(e);
	}
}
