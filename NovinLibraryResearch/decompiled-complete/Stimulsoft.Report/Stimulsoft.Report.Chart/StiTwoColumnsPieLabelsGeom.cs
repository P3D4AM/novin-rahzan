using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiTwoColumnsPieLabelsGeom : StiSeriesLabelsGeom
{
	public StiBrush SeriesBrush { get; }

	public Color BorderColor { get; }

	public Color SeriesBorderColor { get; }

	public StiBrush LabelBrush { get; }

	public StiBrush SeriesLabelsBrush { get; }

	public string Text { get; }

	public RectangleF LabelRect { get; }

	public Color LineColor { get; }

	public PointF StartPoint { get; }

	public PointF EndPoint { get; set; }

	public PointF ArcPoint { get; }

	public PointF CenterPie { get; }

	public StiAnimation Animation { get; set; }

	public override void Draw(StiContext context)
	{
		base.Draw(context);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation && Animation == null)
		{
			Animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalDurationElement);
		}
		Rectangle rectangle = Rectangle.Round(base.ClientRectangle);
		DrawLines(context);
		DrawMarker(context, Rectangle.Round(rectangle), SeriesBorderColor, SeriesBrush);
		DrawLabelArea(context, rectangle);
		DrawLabelText(context, rectangle);
		if (IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetLineMouseOverColor(SeriesBrush), rectangle);
		}
	}

	private void DrawLines(StiContext context)
	{
		if (base.SeriesLabels.DrawBorder)
		{
			PointF? pointF = null;
			StiPenGeom pen = new StiPenGeom(LineColor, 1f);
			if ((CenterPie.Y > StartPoint.Y && EndPoint.Y > StartPoint.Y) || (CenterPie.Y < StartPoint.Y && EndPoint.Y < StartPoint.Y))
			{
				pointF = ((CenterPie.X > EndPoint.X) ? new PointF(EndPoint.X + 13f, StartPoint.Y) : new PointF(EndPoint.X - 13f, StartPoint.Y));
			}
			context.PushSmoothingModeToAntiAlias();
			PointF[] points = ((!pointF.HasValue) ? new PointF[3] { EndPoint, StartPoint, ArcPoint } : new PointF[4]
			{
				EndPoint,
				pointF.GetValueOrDefault(),
				StartPoint,
				ArcPoint
			});
			StiChart stiChart = base.Series.Chart as StiChart;
			if (stiChart.IsAnimation)
			{
				context.DrawAnimationLines(pen, points, Animation);
			}
			else
			{
				context.DrawLines(pen, points);
			}
			context.PopSmoothingMode();
		}
	}

	private void DrawLabelArea(StiContext context, Rectangle rect)
	{
		if (float.IsNaN(rect.X) || float.IsNaN(rect.Y) || float.IsNaN(rect.Width) || float.IsNaN(rect.Height))
		{
			return;
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		StiPenGeom pen = (base.SeriesLabels.DrawBorder ? new StiPenGeom(BorderColor, 1f) : null);
		if (stiChart.IsAnimation)
		{
			context.DrawAnimationRectangle(SeriesLabelsBrush, pen, rect, null, Animation);
			return;
		}
		context.FillRectangle(SeriesLabelsBrush, rect);
		if (base.SeriesLabels.DrawBorder)
		{
			context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
	}

	private void DrawLabelText(StiContext context, Rectangle rect)
	{
		StiChart stiChart = base.Series.Chart as StiChart;
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom stringFormatGeom = base.SeriesLabels.Core.GetStringFormatGeom(context);
		stringFormatGeom.Alignment = (StringAlignment)1;
		stringFormatGeom.LineAlignment = (StringAlignment)1;
		if (stiChart.IsAnimation)
		{
			context.DrawAnimationText(Text, font, LabelBrush, rect, stringFormatGeom, (StiRotationMode)4, 0f, base.SeriesLabels.Antialiasing, rect.Width, Animation);
		}
		else
		{
			context.DrawRotatedString(Text, font, LabelBrush, rect, stringFormatGeom, (StiRotationMode)4, 0f, base.SeriesLabels.Antialiasing, rect.Width);
		}
	}

	protected override void DrawMarker(StiContext context, Rectangle itemRect, object markerColor, StiBrush markerBrush)
	{
		if (!base.SeriesLabels.MarkerVisible)
		{
			return;
		}
		Rectangle empty = Rectangle.Empty;
		int num = (int)((float)itemRect.Right + 2f * context.Options.Zoom);
		int num2 = (int)((float)itemRect.Left - (float)(2 + base.SeriesLabels.MarkerSize.Width) * context.Options.Zoom);
		if (StiOptions.Engine.AllowFixPieChartMarkerAlignment)
		{
			if (base.SeriesLabels.MarkerAlignment == StiMarkerAlignment.Center)
			{
				empty.X = ((base.ClientRectangle.X < CenterPie.X) ? num : num2);
			}
			else if (base.SeriesLabels.MarkerAlignment == StiMarkerAlignment.Right)
			{
				empty.X = ((base.ClientRectangle.X < CenterPie.X) ? num : num2);
			}
			else
			{
				empty.X = ((base.ClientRectangle.X > CenterPie.X) ? num : num2);
			}
		}
		else if (base.SeriesLabels.MarkerAlignment == StiMarkerAlignment.Center)
		{
			empty.X = ((base.ClientRectangle.X < CenterPie.X) ? num : num2);
		}
		else
		{
			empty.X = ((base.SeriesLabels.MarkerAlignment == StiMarkerAlignment.Right) ? num : num2);
		}
		empty.Y = (int)((float)itemRect.Y + ((float)itemRect.Height - (float)base.SeriesLabels.MarkerSize.Height * context.Options.Zoom) / 2f);
		empty.Width = (int)((float)base.SeriesLabels.MarkerSize.Width * context.Options.Zoom);
		empty.Height = (int)((float)base.SeriesLabels.MarkerSize.Height * context.Options.Zoom);
		Color color = ((markerColor is Color) ? ((Color)markerColor) : Color.Black);
		StiPenGeom pen = new StiPenGeom(color, 1f);
		context.FillRectangle(markerBrush, empty.X, empty.Y, empty.Width, empty.Height);
		context.DrawRectangle(pen, empty.X, empty.Y, empty.Width, empty.Height);
	}

	public StiTwoColumnsPieLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle, string text, StiBrush seriesBrush, StiBrush labelBrush, StiBrush seriesLabelsBrush, Color borderColor, Color seriesBorderColor, RectangleF labelRect, Color lineColor, PointF startPoint, PointF endPoint, PointF arcPoint, PointF centerPie, StiAnimation animation)
		: base(seriesLabels, series, index, value, clientRectangle)
	{
		Text = text;
		SeriesLabelsBrush = seriesLabelsBrush;
		LabelBrush = labelBrush;
		LineColor = lineColor;
		BorderColor = borderColor;
		SeriesBorderColor = seriesBorderColor;
		SeriesBrush = seriesBrush;
		LabelRect = labelRect;
		StartPoint = startPoint;
		EndPoint = endPoint;
		ArcPoint = arcPoint;
		CenterPie = centerPie;
		Animation = animation;
	}
}
