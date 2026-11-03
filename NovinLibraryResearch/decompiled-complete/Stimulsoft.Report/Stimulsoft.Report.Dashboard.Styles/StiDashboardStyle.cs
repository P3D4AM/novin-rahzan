using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiDashboardStyle : StiElementStyle
{
	public abstract string LocalizedName { get; }

	public abstract Color ForeColor { get; set; }

	public virtual Color BackColor { get; set; } = Color.White;


	public abstract Color TitleBackColor { get; set; }

	public abstract Color TitleForeColor { get; set; }

	public virtual Color BorderColor
	{
		get
		{
			return TitleBackColor;
		}
		set
		{
		}
	}

	public void DrawStyleForGallery(Graphics g, Rectangle rect)
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Expected O, but got Unknown
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		StiDrawing.FillRectangle(g, Color.White, rect);
		Rectangle rectangle = new Rectangle(rect.X + StiScale.I5, rect.Y + StiScale.I5, rect.Width - StiScale.I10, rect.Height - StiScale.I10);
		Rectangle rectangle2 = new Rectangle(rectangle.Location, new Size(rectangle.Width, rectangle.Height - StiScale.YYI(16.0)));
		Rectangle rectangle3 = new Rectangle(rectangle2.X, rectangle2.Bottom, rectangle2.Width, StiScale.YYI(16.0));
		StiDrawing.FillRectangle(g, BackColor, rectangle);
		StiDrawing.DrawRectangle(g, BorderColor, (double)rectangle2.X, (double)rectangle2.Y, (double)(rectangle2.Width - 1), (double)(rectangle2.Height - 1));
		Font val = new Font("Arial", 15f);
		try
		{
			StringFormat val2 = new StringFormat();
			try
			{
				val2.Alignment = (StringAlignment)1;
				val2.LineAlignment = (StringAlignment)1;
				GraphicsState val3 = g.Save();
				try
				{
					g.SmoothingMode = (SmoothingMode)2;
					g.InterpolationMode = (InterpolationMode)7;
					g.CompositingQuality = (CompositingQuality)2;
					StiDrawing.DrawString(g, "Aa", val, ForeColor, rectangle2, val2);
				}
				finally
				{
					g.Restore(val3);
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
		Font val4 = new Font("Arial", 8f);
		try
		{
			StringFormat val5 = new StringFormat();
			try
			{
				StiDrawing.FillRectangle(g, TitleBackColor, rectangle3);
				val5.Alignment = (StringAlignment)1;
				val5.LineAlignment = (StringAlignment)1;
				GraphicsState val6 = g.Save();
				try
				{
					g.SmoothingMode = (SmoothingMode)2;
					g.InterpolationMode = (InterpolationMode)7;
					g.CompositingQuality = (CompositingQuality)2;
					StiDrawing.DrawString(g, LocalizedName, val4, TitleForeColor, rectangle3, val5);
				}
				finally
				{
					g.Restore(val6);
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
}
