using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components.Gauge;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Render;
using Stimulsoft.Dashboard.Visuals.Gauge;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Dashboard.Drawing.Atoms;

public class StiGaugeAtom : StiElementAtom
{
	internal List<StiGaugeIteration> Iterations { get; set; }

	internal StiGaugeElement GaugeElement => base.Element as StiGaugeElement;

	public override void Paint(Graphics g)
	{
		if (Iterations != null && Iterations.Any())
		{
			List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(base.Location, Iterations.Count(), GaugeElement);
			if (base.AnimationEngine == null || !base.AnimationEngine.IsRunning())
			{
				Iterations = new StiGaugeElementBuilder().Render(GaugeElement, new SizeD(locationRectangles[0].Width, locationRectangles[0].Height), DataTable).ToList();
			}
			PaintSeriesGauge(g, Iterations, locationRectangles, 1f);
		}
	}

	private void PaintSeriesGauge(Graphics g, IEnumerable<StiGaugeIteration> iterations, List<RectangleD> rectangles, float zoom)
	{
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		StiGaugeGdiPainter stiGaugeGdiPainter = new StiGaugeGdiPainter
		{
			IsDashboardMode = true,
			AnimationEngine = base.AnimationEngine
		};
		double? titleHeight = GetTitleHeight(g, iterations);
		double valueOrDefault = titleHeight.GetValueOrDefault(0.0);
		RectangleD rectangleD = rectangles[0];
		double titleMinFontSize = GetTitleMinFontSize(g, iterations, GaugeElement.Font, rectangleD.Width, rectangleD.Height);
		if (titleMinFontSize < (double)GaugeElement.Font.Size)
		{
			titleHeight = GetTitleHeight(g, iterations, (float)titleMinFontSize);
			valueOrDefault = titleHeight.GetValueOrDefault(0.0);
		}
		bool flag = false;
		int num = 0;
		foreach (StiGaugeIteration iteration in iterations)
		{
			RectangleF rect = rectangles[num].ToRectangleF();
			if (!rect.IntersectsWith(base.Location.ToRectangleF()))
			{
				return;
			}
			GraphicsState val = g.Save();
			try
			{
				if (titleHeight.HasValue)
				{
					RectangleD rect2 = new RectangleD(rect.X, rect.Y, rect.Width, valueOrDefault);
					PaintTitle(g, rect2, iteration.Series, titleMinFontSize);
				}
				RectangleD rectangleD2 = new RectangleD(rect.X, (double)rect.Y + valueOrDefault, rect.Width, (double)rect.Height - valueOrDefault);
				stiGaugeGdiPainter.DrawGauge(g, iteration.Gauge, rectangleD2.ToRectangleF(), zoom, isClip: false);
				num++;
				if (!flag && ((double)rect.Right - base.Location.Right > 1.0 || (double)rect.Bottom - base.Location.Bottom > 1.0))
				{
					flag = true;
				}
			}
			catch (Exception ex)
			{
				Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
				try
				{
					StringFormat val2 = new StringFormat();
					try
					{
						val2.LineAlignment = (StringAlignment)1;
						val2.Alignment = (StringAlignment)1;
						StiTextDrawing.DrawString(g, ex.Message, gdiFont, Color.Red, RectangleD.CreateFromRectangle(rect), val2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)gdiFont)?.Dispose();
				}
			}
			finally
			{
				g.Restore(val);
			}
		}
		if (flag && GaugeElement.IsDesigning)
		{
			DrawArrowMore(g);
		}
	}

	public RectangleD PaintTitle(Graphics g, RectangleD rect, string titleText, double size)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		if (string.IsNullOrEmpty(titleText))
		{
			return rect;
		}
		Font val = StiFontUtils.ChangeFontSize(GaugeElement.Font, (float)size);
		try
		{
			SizeD sizeD = StiTitleMeasureHelper.MeasureTitle(g, rect, GaugeElement.Font, titleText);
			RectangleD rect2 = new RectangleD(rect.X, rect.Y, rect.Width, sizeD.Height);
			if (!string.IsNullOrWhiteSpace(titleText))
			{
				Color foreColor = StiDashboardExpressionHelper.GetForeColor(GaugeElement, GaugeElement.ForeColor);
				Color color = ((foreColor != Color.Transparent) ? foreColor : StiDashboardStyleHelper.GetForeColor(base.Element));
				SolidBrush val2 = new SolidBrush(color);
				try
				{
					StiTextDrawing.DrawString(g, titleText, val, (Brush)(object)val2, rect2, StiAutoSizeHtmlTextHelper.GetDefaultTextOptions(), StiTextHorAlignment.Center, StiVertAlignment.Center, antialiasing: true, 1f);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			rect.Y += rect2.Height;
			rect.Height -= rect2.Height;
			return rect;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private double GetTitleMinFontSize(Graphics g, IEnumerable<StiGaugeIteration> iterations, Font font, double width, double height)
	{
		float? num = null;
		RectangleD rect = new RectangleD(0.0, 0.0, width, height);
		foreach (StiGaugeIteration iteration in iterations)
		{
			float num2 = StiAutoSizeTextHelper.MeasureFontSize(g, iteration.Series, rect, font);
			num = ((!num.HasValue) ? num2 : Math.Min(num.GetValueOrDefault(), num2));
		}
		return num.GetValueOrDefault();
	}

	private double? GetTitleHeight(Graphics g, IEnumerable<StiGaugeIteration> iterations, float? minSize = null)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		StiGaugeIteration stiGaugeIteration = iterations.FirstOrDefault((StiGaugeIteration c) => !string.IsNullOrEmpty(c.Series));
		if (stiGaugeIteration == null)
		{
			return null;
		}
		Font font = GaugeElement.Font;
		if (minSize.HasValue)
		{
			font = new Font(GaugeElement.Font.FontFamily, minSize.GetValueOrDefault());
		}
		double height = StiTitleMeasureHelper.MeasureTitle(g, base.Location, font, stiGaugeIteration.Series).Height;
		if (height > base.Location.Height / 2.0)
		{
			return null;
		}
		return height;
	}

	internal void DrawArrowMore(Graphics g)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		SmoothingMode smoothingMode = g.SmoothingMode;
		try
		{
			g.SmoothingMode = (SmoothingMode)4;
			RectangleF rectangleF = base.Location.ToRectangleF();
			float num = (float)(StiElementScale.Factor((IStiElement)GaugeElement) * StiScale.Factor);
			PointF[] array = new PointF[3]
			{
				new PointF(rectangleF.Right - 5f * num, rectangleF.Bottom - 5f * num),
				new PointF(rectangleF.Right - 5f * num, rectangleF.Bottom - 15f * num),
				new PointF(rectangleF.Right - 15f * num, rectangleF.Bottom - 5f * num)
			};
			SolidBrush val = new SolidBrush(Color.FromArgb(140, 140, 140));
			try
			{
				g.FillPolygon((Brush)(object)val, array);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			g.SmoothingMode = smoothingMode;
		}
	}

	public override void Render()
	{
		Iterations = new StiGaugeElementBuilder().Render(GaugeElement, base.Location.Size, DataTable).ToList();
	}

	public RectangleF GetContentRectangle(RectangleD rect, IStiElement element)
	{
		if (DataTable == null)
		{
			return rect.ToRectangleF();
		}
		IEnumerable<StiGaugeIteration> enumerable = new StiGaugeElementBuilder().Render(GaugeElement, rect.Size, DataTable);
		if (enumerable == null || !enumerable.Any())
		{
			return rect.ToRectangleF();
		}
		List<RectangleD> locationRectangles = StiElementLocationHelper.GetLocationRectangles(rect, enumerable.Count(), element);
		rect = StiElementLocationHelper.GetSumRectangle(locationRectangles);
		return rect.ToRectangleF();
	}

	public StiGaugeAtom(IStiElement element, RectangleD location, StiDataTable dataTable)
		: base(element, location, dataTable)
	{
	}

	public StiGaugeAtom(IStiElement element, RectangleD location, StiDataTable dataTable, StiAnimationEngine animation)
		: base(element, location, dataTable, animation)
	{
	}
}
