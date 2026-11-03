using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiProgressElementStyle : StiElementStyle
{
	public abstract string LocalizedName { get; }

	public virtual Color ForeColor { get; set; } = Color.Transparent;


	public abstract Color TrackColor { get; set; }

	public abstract Color BandColor { get; set; }

	public abstract Color[] SeriesColors { get; set; }

	[Browsable(false)]
	public virtual Color BackColor { get; set; } = Color.White;


	public void DrawStyleForGallery(Graphics g, Rectangle rect, StiProgressElementMode mode)
	{
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		GraphicsState val = g.Save();
		try
		{
			g.SmoothingMode = (SmoothingMode)2;
			g.InterpolationMode = (InterpolationMode)7;
			g.CompositingQuality = (CompositingQuality)2;
			Color backColor = StiDashboardStyleHelper.GetBackColor(Ident);
			StiDrawing.FillRectangle(g, backColor, rect);
			switch (mode)
			{
			case StiProgressElementMode.Circle:
			{
				float num = (float)Math.Min(rect.Width, rect.Height) - (float)StiScale.XXI(10.0);
				RectangleF rectangleF3 = new RectangleF((float)rect.X + ((float)rect.Width - num) / 2f, (float)rect.Y + ((float)rect.Height - num) / 2f, num, num);
				Pen val9 = new Pen(TrackColor, (float)(7.0 * StiScale.Factor));
				try
				{
					g.DrawEllipse(val9, rectangleF3);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
				if (SeriesColors != null && SeriesColors.Length != 0)
				{
					g.SetClip(new RectangleF(rectangleF3.X + rectangleF3.Width / 2f, rectangleF3.Y - (float)StiScale.YYI(5.0), rectangleF3.Width / 2f + (float)StiScale.XXI(10.0), rectangleF3.Height + (float)StiScale.YYI(10.0)));
					Pen val10 = new Pen(SeriesColors[0], (float)(7.0 * StiScale.Factor));
					try
					{
						g.DrawEllipse(val10, rectangleF3);
					}
					finally
					{
						((IDisposable)val10)?.Dispose();
					}
					g.ResetClip();
					g.SetClip(new RectangleF(rectangleF3.X - (float)StiScale.XXI(5.0), rectangleF3.Y + rectangleF3.Height / 2f, rectangleF3.Width / 2f + (float)StiScale.XXI(10.0), rectangleF3.Height / 2f + (float)StiScale.YYI(10.0)));
					Pen val11 = new Pen(SeriesColors[0], (float)(7.0 * StiScale.Factor));
					try
					{
						g.DrawEllipse(val11, rectangleF3);
					}
					finally
					{
						((IDisposable)val11)?.Dispose();
					}
					g.ResetClip();
				}
				rectangleF3.Y += 2f;
				Color baseColor = ((Ident == StiElementStyleIdent.Custom) ? ForeColor : StiDashboardStyleHelper.GetForeColor(Ident));
				baseColor = Color.FromArgb(255, baseColor);
				Font val12 = new Font("Arial", (float)(14.0 * StiScale.Factor), (FontStyle)1, (GraphicsUnit)2);
				try
				{
					SolidBrush val13 = new SolidBrush(baseColor);
					try
					{
						StringFormat val14 = new StringFormat();
						try
						{
							val14.Alignment = (StringAlignment)1;
							val14.LineAlignment = (StringAlignment)1;
							g.DrawString("75%", val12, (Brush)(object)val13, rectangleF3, val14);
							break;
						}
						finally
						{
							((IDisposable)val14)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val13)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val12)?.Dispose();
				}
			}
			case StiProgressElementMode.Pie:
			{
				float num2 = (float)Math.Min(rect.Width, rect.Height) - (float)StiScale.XXI(10.0);
				RectangleF rectangleF4 = new RectangleF((float)rect.X + ((float)rect.Width - num2) / 2f, (float)rect.Y + ((float)rect.Height - num2) / 2f, num2, num2);
				SolidBrush val15 = new SolidBrush(TrackColor);
				try
				{
					g.FillEllipse((Brush)(object)val15, rectangleF4);
				}
				finally
				{
					((IDisposable)val15)?.Dispose();
				}
				if (SeriesColors != null && SeriesColors.Length != 0)
				{
					g.SetClip(new RectangleF(rectangleF4.X + rectangleF4.Width / 2f, rectangleF4.Y, rectangleF4.Width / 2f, rectangleF4.Height));
					SolidBrush val16 = new SolidBrush(SeriesColors[0]);
					try
					{
						g.FillEllipse((Brush)(object)val16, rectangleF4);
					}
					finally
					{
						((IDisposable)val16)?.Dispose();
					}
					g.ResetClip();
					g.SetClip(new RectangleF(rectangleF4.X, rectangleF4.Y + rectangleF4.Height / 2f, rectangleF4.Width / 2f, rectangleF4.Height / 2f));
					SolidBrush val17 = new SolidBrush(SeriesColors[0]);
					try
					{
						g.FillEllipse((Brush)(object)val17, rectangleF4);
					}
					finally
					{
						((IDisposable)val17)?.Dispose();
					}
					g.ResetClip();
				}
				break;
			}
			case StiProgressElementMode.DataBars:
			{
				RectangleF rectangleF = new RectangleF(rect.X + StiScale.XXI(5.0), rect.Y + StiScale.YYI(5.0), rect.Width - StiScale.XXI(10.0), rect.Height - StiScale.YYI(10.0));
				SolidBrush val2 = new SolidBrush(TrackColor);
				try
				{
					g.FillRectangle((Brush)(object)val2, rectangleF);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				Color nativeForeColor = StiDashboardStyleHelper.GetNativeForeColor();
				Font val3 = new Font("Arial", (float)(25.0 * StiScale.Factor), (FontStyle)1, (GraphicsUnit)2);
				try
				{
					SolidBrush val4 = new SolidBrush(nativeForeColor);
					try
					{
						StringFormat val5 = new StringFormat();
						try
						{
							val5.Alignment = (StringAlignment)1;
							val5.LineAlignment = (StringAlignment)1;
							g.DrawString("75%", val3, (Brush)(object)val4, rectangleF, val5);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
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
				RectangleF rectangleF2 = new RectangleF(rectangleF.X, rectangleF.Y, rectangleF.Width * 0.75f, rectangleF.Height);
				if (SeriesColors != null && SeriesColors.Length != 0)
				{
					SolidBrush val6 = new SolidBrush(SeriesColors[0]);
					try
					{
						g.FillRectangle((Brush)(object)val6, rectangleF2);
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				g.SetClip(rectangleF2);
				Font val7 = new Font("Arial", (float)(25.0 * StiScale.Factor), (FontStyle)1, (GraphicsUnit)2);
				try
				{
					StringFormat val8 = new StringFormat();
					try
					{
						val8.Alignment = (StringAlignment)1;
						val8.LineAlignment = (StringAlignment)1;
						g.DrawString("75%", val7, Brushes.White, rectangleF, val8);
					}
					finally
					{
						((IDisposable)val8)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
				g.ResetClip();
				break;
			}
			}
		}
		finally
		{
			g.Restore(val);
		}
	}
}
