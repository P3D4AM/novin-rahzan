using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiCardsElementStyle : StiElementStyle, IStiCellIndicatorStyle
{
	public abstract string LocalizedName { get; }

	public virtual Color CellBackColor { get; set; }

	public virtual Color LineColor { get; set; } = Color.Gainsboro;


	public virtual Color CellForeColor { get; set; } = StiColor.Get("222");


	[Browsable(false)]
	public virtual Color BackColor { get; set; } = Color.White;


	[Browsable(false)]
	public virtual Color CellDataBarsOverlapped { get; set; } = Color.FromArgb(255, 51, 94, 150);


	[Browsable(false)]
	public virtual Color CellDataBarsPositive { get; set; } = Color.FromArgb(255, 99, 142, 198);


	[Browsable(false)]
	public virtual Color CellDataBarsNegative { get; set; } = Color.FromArgb(255, 255, 0, 0);


	[Browsable(false)]
	public virtual Color CellWinLossPositive { get; set; } = Color.FromArgb(255, 99, 142, 198);


	[Browsable(false)]
	public virtual Color CellWinLossNegative { get; set; } = Color.FromArgb(255, 255, 0, 0);


	[Browsable(false)]
	public virtual Color CellSparkline { get; set; } = Color.FromArgb(255, 83, 126, 182);


	[Browsable(false)]
	public virtual Color CellIndicatorPositive { get; set; } = Color.Green;


	[Browsable(false)]
	public virtual Color CellIndicatorNegative { get; set; } = Color.Red;


	[Browsable(false)]
	public virtual Color CellIndicatorNeutral { get; set; } = Color.LightGray;


	public void DrawStyleForGallery(Graphics g, Rectangle rect)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		GraphicsState val = g.Save();
		try
		{
			g.SmoothingMode = (SmoothingMode)2;
			g.InterpolationMode = (InterpolationMode)7;
			g.CompositingQuality = (CompositingQuality)2;
			Rectangle rectangle = new Rectangle(rect.X + StiScale.XXI(15.0), rect.Y + StiScale.YYI(5.0), rect.Width - StiScale.XXI(30.0), rect.Height - StiScale.YYI(10.0));
			int num = rectangle.Height / 3;
			int num2 = rectangle.Height * 2 / 3;
			rect.Inflate(-1, -1);
			StiDrawing.FillRectangle(g, BackColor, rect);
			StiDrawing.FillRectangle(g, CellBackColor, rectangle);
			rect.Inflate(-4, -4);
			StiDrawing.DrawRectangle(g, LineColor, rect);
			RectangleF rectangleF = new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width, num);
			Font val2 = new Font("Arial", (float)(17.0 * StiScale.Factor), (FontStyle)1, (GraphicsUnit)2);
			try
			{
				SolidBrush val3 = new SolidBrush(CellForeColor);
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val4.Alignment = (StringAlignment)2;
						val4.LineAlignment = (StringAlignment)1;
						g.DrawString("1000", val2, (Brush)(object)val3, rectangleF, val4);
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
			StiGdiContextPainter contextPainter = new StiGdiContextPainter(g);
			StiContext stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, 1f);
			object[] array = new object[8] { 2, 3, 1, 3, 7, 6, 2, 3 };
			RectangleD rect2 = default(RectangleD);
			((RectangleD)(ref rect2))._002Ector((double)rectangle.Left, (double)rectangleF.Bottom, (double)rectangle.Width, (double)num2);
			StiLineSparklinesCellPainter.Draw(stiContext, rect2, array, 1f, CellSparkline, showArea: true, showFirstLastMarker: true, showHighLowMarker: true);
			stiContext.Render(((RectangleD)(ref rect2)).ToRectangleF());
		}
		finally
		{
			g.Restore(val);
		}
	}
}
