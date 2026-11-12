using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Drawing.Painters;

public class StiDashboardElementGdiPainter : StiPageGdiPainter
{
	public override void PaintBackground(StiPaintEventArgs e, Graphics g, StiPage page, double width, double height)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		double num = stiDashboard.Zoom * StiScale.Factor;
		RectangleD rectangleD = new RectangleD(0.0, 0.0, width * num, height * num);
		RectangleD rectangleD2 = (stiDashboard.IsDesigning ? e.ClipRectangle : rectangleD);
		Color dashboardBackColor = StiDashboardStyleHelper.GetDashboardBackColor(stiDashboard, isViewer: false);
		StiDrawing.FillRectangle(g, dashboardBackColor, rectangleD2.ToRectangleF());
		if (stiDashboard.DashboardWatermark != null)
		{
			StiAdvancedWatermarkGdiPainter.PaintWatermark(g, stiDashboard.DashboardWatermark, null, rectangleD, num);
		}
	}

	public override void PaintBorder(StiComponent component, Graphics g, RectangleF rect, float zoom, bool drawBorderFormatting, bool drawBorderSides)
	{
	}

	public override void PaintContentBorder(Graphics g, StiPage page, double width, double height)
	{
	}

	public override void PaintGridLines(Graphics g, StiPage page, double width, double height)
	{
		double num = page.Unit.ConvertToHInches(page.GridSize);
		double num2 = page.Zoom * StiScale.Factor;
		Color dashboardGridLinesColor = GetDashboardGridLinesColor(page);
		double num3 = width * num2;
		double num4 = num * num2;
		int num5 = 0;
		bool flag = true;
		for (double num6 = 0.0; num6 < num3; num6 += num4)
		{
			if ((num5 & 1) == 0 && !flag)
			{
				StiDrawing.DrawLine(g, dashboardGridLinesColor, num6, 0.0, num6, height * num2);
			}
			flag = false;
			num5++;
		}
		double num7 = height * num2;
		num4 = num * num2;
		num5 = 0;
		flag = true;
		for (double num8 = 0.0; num8 < num7; num8 += num4)
		{
			if ((num5 & 1) == 0 && !flag)
			{
				StiDrawing.DrawLine(g, dashboardGridLinesColor, 0.0, num8, width * num2, num8);
			}
			flag = false;
			num5++;
		}
	}

	public override void PaintGridDots(Graphics g, StiPage page, double width, double height)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		double num = page.Unit.ConvertToHInches(page.GridSize);
		double num2 = page.Zoom * StiScale.Factor;
		Color dashboardGridDotsColor = GetDashboardGridDotsColor(page);
		Bitmap val = new Bitmap((int)(width * num2) + 1, 1);
		try
		{
			double num3 = width * num2;
			double num4 = num * num2;
			for (double num5 = 0.0; num5 < num3; num5 += num4)
			{
				int num6 = (int)Math.Round(num5);
				if (num6 < ((Image)val).Width)
				{
					val.SetPixel(num6, 0, dashboardGridDotsColor);
				}
			}
			double num7 = height * num2;
			for (double num8 = 0.0; num8 < num7; num8 += num4)
			{
				g.DrawImage((Image)(object)val, 0, (int)num8);
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override Color GetSelectionBackgroundColor(StiPage page)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		Color baseColor = (StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style) ? Color.SkyBlue : Color.Blue);
		return Color.FromArgb(75, baseColor);
	}

	public override Color GetSelectionBorderColor(StiPage page)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		if (!StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style))
		{
			return Color.DimGray;
		}
		return Color.SteelBlue;
	}

	public Color GetDashboardGridLinesColor(StiPage page)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		if (!StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style))
		{
			return Color.Gainsboro;
		}
		return StiColor.Get("#3b4d5f");
	}

	public Color GetDashboardGridDotsColor(StiPage page)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		if (!StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style))
		{
			return StiColorUtils.Dark(Color.Gainsboro, 30);
		}
		return StiColor.Get("#3b4d5f");
	}

	public override Color GetTableLinesColor(StiPage page)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		if (!StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style))
		{
			return Color.Blue;
		}
		return Color.SteelBlue;
	}

	public override Color GetSelectionCornerColor(StiPage page)
	{
		StiDashboard stiDashboard = page as StiDashboard;
		if (!StiDashboardStyleHelper.IsDarkStyle(stiDashboard.Style))
		{
			return Color.Gray;
		}
		return Color.LightGray;
	}
}
