using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiCenterFunnelLabelsGeom : StiSeriesLabelsGeom
{
	public StiBrush SeriesBrush { get; }

	public Color BorderColor { get; }

	public Color SeriesBorderColor { get; }

	public StiBrush LabelBrush { get; }

	public string Text { get; }

	public RectangleF LabelRect { get; }

	public StiAnimation Animation { get; set; }

	public override void Draw(StiContext context)
	{
		base.Draw(context);
		Rectangle rectangle = Rectangle.Round(base.ClientRectangle);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation && Animation == null)
		{
			Animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalDurationElement);
		}
		DrawMarker(context, Rectangle.Round(LabelRect), SeriesBorderColor, SeriesBrush);
		int num = rectangle.X + rectangle.Width / 2;
		int num2 = rectangle.Y + rectangle.Height / 2;
		context.PushTranslateTransform(num, num2);
		context.PushRotateTransform(base.SeriesLabels.Angle);
		Rectangle rect = rectangle;
		rect.X = -rect.Width / 2;
		rect.Y = -rect.Height / 2;
		DrawLabelArea(context, rect);
		DrawLabelText(context, rect);
		context.PopTransform();
		context.PopTransform();
		if (IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetLineMouseOverColor(SeriesBrush), LabelRect.X, LabelRect.Y, LabelRect.Width, LabelRect.Height);
		}
	}

	private void DrawLabelArea(StiContext context, Rectangle rect)
	{
		if (rect.IsEmpty)
		{
			return;
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		StiPenGeom pen = (base.SeriesLabels.DrawBorder ? new StiPenGeom(BorderColor) : null);
		if (stiChart.IsAnimation)
		{
			context.DrawAnimationRectangle(base.SeriesLabels.Brush, pen, rect, null, Animation);
		}
		else
		{
			context.FillRectangle(base.SeriesLabels.Brush, rect);
			if (base.SeriesLabels.DrawBorder)
			{
				context.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
			}
		}
		if (IsMouseOver)
		{
			context.FillRectangle(StiMouseOverHelper.GetLineMouseOverColor(SeriesBrush), rect.X, rect.Y, rect.Width, rect.Height);
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

	public StiCenterFunnelLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle, string text, StiBrush seriesBrush, StiBrush labelBrush, Color borderColor, Color seriesBorderColor, RectangleF labelRect, StiAnimation animation)
		: base(seriesLabels, series, index, value, clientRectangle)
	{
		Text = text;
		LabelBrush = labelBrush;
		BorderColor = borderColor;
		SeriesBorderColor = seriesBorderColor;
		SeriesBrush = seriesBrush;
		LabelRect = labelRect;
		Animation = animation;
	}
}
