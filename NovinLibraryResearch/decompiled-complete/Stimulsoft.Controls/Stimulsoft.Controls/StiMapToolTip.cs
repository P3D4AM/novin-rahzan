using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiMapToolTip : ToolTip
{
	public Brush ColorState { get; set; }

	public string StateName { get; set; }

	public string ValueText { get; set; }

	public string TotalText { get; set; }

	private void OnPopup(object sender, PopupEventArgs e)
	{
		e.ToolTipSize = MeasureSize();
	}

	private void OnDraw(object sender, DrawToolTipEventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		SolidBrush val = new SolidBrush(Color.White);
		try
		{
			graphics.FillRectangle((Brush)(object)val, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width + StiScaleUI.XXI(10.0), e.Bounds.Height + StiScaleUI.YYI(10.0)));
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
		GraphicsState val4 = graphics.Save();
		try
		{
			graphics.CompositingQuality = (CompositingQuality)2;
			graphics.SmoothingMode = (SmoothingMode)2;
			int num = e.Bounds.Y + StiScaleUI.YYI(10.0);
			if (!string.IsNullOrEmpty(ValueText) || !string.IsNullOrEmpty(TotalText))
			{
				num += 2;
			}
			graphics.FillEllipse(ColorState, new Rectangle(e.Bounds.X + StiScaleUI.XXI(10.0), num, StiScaleUI.XXI(16.0), StiScaleUI.YYI(16.0)));
			Font val5 = new Font("Microsoft Sans Serif", 10f);
			try
			{
				int num2 = e.Bounds.Y + StiScaleUI.YYI(11.0);
				SizeF sizeF = graphics.MeasureString(StateName, val5);
				graphics.DrawString(StateName, val5, Brushes.Black, new RectangleF(e.Bounds.X + StiScaleUI.XXI(32.0), num2, sizeF.Width + (float)StiScaleUI.XXI(4.0), StiScaleUI.YYI(16.0)));
				num2 += StiScaleUI.YYI(22.0);
				if (!string.IsNullOrEmpty(ValueText))
				{
					sizeF = graphics.MeasureString(ValueText, val5);
					graphics.DrawString(ValueText, val5, Brushes.Black, new RectangleF(e.Bounds.X + StiScaleUI.XXI(32.0), num2, sizeF.Width + (float)StiScaleUI.XXI(4.0), StiScaleUI.YYI(16.0)));
					num2 += StiScaleUI.YYI(22.0);
				}
				if (!string.IsNullOrEmpty(TotalText))
				{
					sizeF = graphics.MeasureString(TotalText, val5);
					graphics.DrawString(TotalText, val5, Brushes.Black, new RectangleF(e.Bounds.X + StiScaleUI.XXI(32.0), num2, sizeF.Width + (float)StiScaleUI.XXI(4.0), StiScaleUI.YYI(16.0)));
				}
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
		finally
		{
			graphics.Restore(val4);
		}
	}

	private Size MeasureSize()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				Font val3 = new Font("Microsoft Sans Serif", 10f);
				try
				{
					SizeF sizeF = val2.MeasureString(StateName, val3);
					float num = 0f;
					float num2 = 0f;
					num = sizeF.Width;
					num2 = (float)StiScaleUI.YY(20.0);
					if (!string.IsNullOrEmpty(ValueText))
					{
						sizeF = val2.MeasureString(ValueText, val3);
						num = Math.Max(num, sizeF.Width);
						num2 += sizeF.Height + (float)StiScaleUI.YY(6.0);
					}
					if (!string.IsNullOrEmpty(TotalText))
					{
						sizeF = val2.MeasureString(TotalText, val3);
						num = Math.Max(num, sizeF.Width);
						num2 += sizeF.Height + (float)StiScaleUI.YY(6.0);
					}
					num += (float)StiScaleUI.XX(24.0);
					return new Size((int)num + StiScaleUI.XXI(20.0), (int)num2 + StiScaleUI.YYI(14.0));
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

	public StiMapToolTip()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		((ToolTip)this).OwnerDraw = true;
		((ToolTip)this).Popup += new PopupEventHandler(OnPopup);
		((ToolTip)this).Draw += new DrawToolTipEventHandler(OnDraw);
	}
}
