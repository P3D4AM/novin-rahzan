using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiIndicatorElementStyle : StiElementStyle
{
	public abstract string LocalizedName { get; }

	public abstract Color GlyphColor { get; set; }

	[Browsable(false)]
	public virtual Color BackColor { get; set; } = Color.White;


	[Browsable(false)]
	public virtual Color ForeColor { get; set; } = Color.White;


	[Browsable(false)]
	public virtual Color HotBackColor { get; set; } = Color.White;


	[Browsable(false)]
	public virtual Color PositiveColor { get; set; } = Color.Green;


	[Browsable(false)]
	public virtual Color NegativeColor { get; set; } = Color.Red;


	public void DrawStyleForGallery(Graphics g, Rectangle rect, StiFontIcons indicatorFontIcons, bool isDashboard, bool isFormUI)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Expected O, but got Unknown
		GraphicsState val = g.Save();
		try
		{
			g.SmoothingMode = (SmoothingMode)2;
			g.InterpolationMode = (InterpolationMode)7;
			g.CompositingQuality = (CompositingQuality)2;
			if (isDashboard)
			{
				Color backColor = StiDashboardStyleHelper.GetBackColor(Ident);
				StiDrawing.FillRectangle(g, BackColor, rect);
				string content = StiFontIconsHelper.GetContent((StiFontIcons?)indicatorFontIcons);
				float num = ((StiScale.Factor > 1.0) ? 15f : 20f);
				Font val2 = new Font(StiFontIconsHelper.GetFontFamilyIcons(), (float)((double)num * StiScale.Factor));
				RectangleF rectangleF;
				try
				{
					SolidBrush val3 = new SolidBrush(GlyphColor);
					try
					{
						SizeF sizeF = g.MeasureString(content, val2);
						rectangleF = new RectangleF((float)rect.Right - sizeF.Width - (float)StiScale.XXI(8.0), (float)rect.Top + ((float)rect.Height - sizeF.Height) / 2f, sizeF.Width, sizeF.Height);
						g.DrawString(content, val2, (Brush)(object)val3, rectangleF);
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
				RectangleF rectangleF2 = new RectangleF(rect.Left, rect.Top, (float)rect.Width - rectangleF.Width, rect.Height);
				Color color = ((Ident == StiElementStyleIdent.Custom) ? ForeColor : StiDashboardStyleHelper.GetForeColor(Ident));
				Font val4 = new Font("Arial", (float)(20.0 * StiScale.Factor), (FontStyle)1, (GraphicsUnit)2);
				try
				{
					SolidBrush val5 = new SolidBrush(color);
					try
					{
						StringFormat val6 = new StringFormat();
						try
						{
							val6.Alignment = (StringAlignment)1;
							val6.LineAlignment = (StringAlignment)1;
							g.DrawString("1000", val4, (Brush)(object)val5, rectangleF2, val6);
							return;
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
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			Color backColor2 = StiDashboardStyleHelper.GetBackColor(Ident);
			StiDrawing.FillRectangle(g, BackColor, rect);
			float num2 = ((StiScale.Factor > 1.0) ? 13f : 17f);
			Font val7 = new Font(StiFontIconsHelper.GetFontFamilyIcons(), (float)((double)num2 * StiScale.Factor));
			try
			{
				SolidBrush val8 = new SolidBrush(GlyphColor);
				try
				{
					string content2 = StiFontIconsHelper.GetContent((StiFontIcons?)indicatorFontIcons);
					SizeF sizeF2 = g.MeasureString(content2, val7);
					RectangleF rectangleF3 = new RectangleF(rect.Left + StiScale.XXI(8.0), (float)rect.Top + ((float)rect.Height - sizeF2.Height) / 2f, sizeF2.Width, sizeF2.Height);
					g.DrawString(content2, val7, (Brush)(object)val8, rectangleF3);
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
		}
		finally
		{
			g.Restore(val);
		}
	}
}
