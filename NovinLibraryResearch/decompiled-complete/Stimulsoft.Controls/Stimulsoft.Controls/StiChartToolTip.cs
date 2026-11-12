using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiChartToolTip : ToolTip
{
	private int iconMargin = (int)(10.0 * StiScale.Factor);

	private Size iconSize = new Size(StiScale.I10, StiScale.I10);

	public Color ColorSeries { get; set; }

	public string Value { get; set; }

	public string Argument { get; set; }

	public string Weight { get; set; }

	private void OnPopup(object sender, PopupEventArgs e)
	{
		e.ToolTipSize = MeasureSize();
	}

	private void OnDraw(object sender, DrawToolTipEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		SolidBrush val = new SolidBrush(Color.White);
		try
		{
			graphics.FillRectangle((Brush)(object)val, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + StiScale.I10, e.Bounds.Height + StiScale.I10));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		SolidBrush val2 = new SolidBrush(Color.FromArgb(200, 200, 200));
		try
		{
			Pen val3 = new Pen((Brush)(object)val2, 1f);
			try
			{
				graphics.DrawRectangle(val3, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
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
		SolidBrush val4 = new SolidBrush(ColorSeries);
		try
		{
			graphics.FillRectangle((Brush)(object)val4, new Rectangle(e.Bounds.X + StiScale.I10, e.Bounds.Y + StiScale.I10, StiScale.I10, StiScale.I10));
		}
		finally
		{
			((IDisposable)val4)?.Dispose();
		}
		Font val5 = new Font("Microsoft Sans Serif", 12f, (FontStyle)1);
		try
		{
			Font val6 = new Font("Microsoft Sans Serif", 10f);
			try
			{
				graphics.DrawString(Value, val5, Brushes.Black, new PointF(e.Bounds.X + StiScale.XXI(30.0), e.Bounds.Y + StiScale.YYI(5.0)));
				graphics.DrawString(Argument, val6, Brushes.Black, new PointF(e.Bounds.X + StiScale.XXI(10.0), e.Bounds.Y + StiScale.YYI(30.0)));
				if (!string.IsNullOrEmpty(Weight))
				{
					graphics.DrawString(Weight, val6, Brushes.Black, new PointF(e.Bounds.X + StiScale.XXI(10.0), e.Bounds.Y + StiScale.XXI(50.0)));
				}
			}
			finally
			{
				((IDisposable)val6)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val5)?.Dispose();
		}
	}

	private Size MeasureSize()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				Font val3 = new Font("Microsoft Sans Serif", 12f, (FontStyle)1);
				try
				{
					Font val4 = new Font("Microsoft Sans Serif", 10f);
					try
					{
						SizeF sizeF = val2.MeasureString(Value, val3);
						SizeF sizeF2 = val2.MeasureString(Argument, val4);
						SizeF sizeF3 = val2.MeasureString(Argument, val4);
						float num = Math.Max(sizeF2.Width + (float)StiScale.XXI(10.0), (float)(2 * iconMargin + iconSize.Width) + sizeF.Width + (float)StiScale.XXI(10.0));
						float num2 = (float)(2 * iconMargin + iconSize.Height) + sizeF2.Height + (float)StiScale.XXI(10.0);
						if (!string.IsNullOrEmpty(Weight))
						{
							num2 += sizeF3.Height;
						}
						return new Size((int)num, (int)num2);
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
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public StiChartToolTip()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		((ToolTip)this).OwnerDraw = true;
		((ToolTip)this).Popup += new PopupEventHandler(OnPopup);
		((ToolTip)this).Draw += new DrawToolTipEventHandler(OnDraw);
	}
}
