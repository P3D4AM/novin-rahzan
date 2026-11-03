using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Helpers;

namespace Stimulsoft.Report.Chart;

public class StiLegendLineMarker : IStiLegendMarker
{
	public void Draw(StiContext context, IStiSeries serie, RectangleF rect, int colorIndex, int colorCount, int index)
	{
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		Color color = Color.Transparent;
		bool flag = false;
		bool flag2 = false;
		StiFontIcons? val = null;
		if (serie is IStiRadarSeries stiRadarSeries)
		{
			color = StiBrush.ToColor(stiRadarSeries.Marker.Brush);
			val = stiRadarSeries.Marker.Icon;
			flag2 = true;
		}
		if (serie is IStiRadarLineSeries stiRadarLineSeries)
		{
			color = stiRadarLineSeries.LineColor;
			flag2 = stiRadarLineSeries.Marker.Visible;
			val = stiRadarLineSeries.Marker.Icon;
			flag = true;
		}
		if (serie is IStiBaseLineSeries stiBaseLineSeries)
		{
			color = stiBaseLineSeries.LineColor;
			flag2 = stiBaseLineSeries.Marker.Visible;
			val = stiBaseLineSeries.Marker.Icon;
			flag = true;
		}
		if (serie is IStiStackedBaseLineSeries stiStackedBaseLineSeries)
		{
			color = stiStackedBaseLineSeries.LineColor;
			flag2 = stiStackedBaseLineSeries.Marker.Visible;
			val = stiStackedBaseLineSeries.Marker.Icon;
			flag = true;
		}
		if (serie is IStiScatterSeries stiScatterSeries)
		{
			color = StiBrush.ToColor(stiScatterSeries.Marker.Brush);
			val = stiScatterSeries.Marker.Icon;
			flag2 = true;
			flag = false;
		}
		if (serie is IStiScatterLineSeries stiScatterLineSeries)
		{
			color = stiScatterLineSeries.LineColor;
			flag2 = stiScatterLineSeries.Marker.Visible;
			val = stiScatterLineSeries.Marker.Icon;
			flag = true;
		}
		if (serie is IStiBubbleSeries { Brush: var val2 } stiBubbleSeries)
		{
			if (stiBubbleSeries.AllowApplyStyle)
			{
				StiBrush seriesBrush = stiBubbleSeries.Core.GetSeriesBrush(colorIndex, colorCount);
				val2 = stiBubbleSeries.ProcessSeriesBrushes(colorIndex, seriesBrush);
			}
			color = StiBrush.ToColor(val2);
			flag2 = true;
			flag = false;
		}
		if (val.HasValue)
		{
			StiFontIconsExHelper.DrawDirectionIcons(context, (object)new StiSolidBrush(color), rect, new SizeF(rect.Height, rect.Height), val.GetValueOrDefault(), null, verticalDirection: false);
			return;
		}
		if (flag)
		{
			StiPenGeom pen = new StiPenGeom(color);
			context.DrawRectangle(pen, rect);
			context.DrawLine(pen, rect.X, rect.Y + rect.Height / 2f, rect.Right, rect.Y + rect.Height / 2f);
		}
		if (flag2)
		{
			if (flag)
			{
				rect = new RectangleF(rect.X + rect.Width / 4f, rect.Y + rect.Height / 4f, rect.Width / 2f, rect.Height / 2f);
			}
			context.PushSmoothingModeToAntiAlias();
			context.FillEllipse((object)new StiSolidBrush(color), rect, null, index);
			context.PopSmoothingMode();
		}
	}
}
