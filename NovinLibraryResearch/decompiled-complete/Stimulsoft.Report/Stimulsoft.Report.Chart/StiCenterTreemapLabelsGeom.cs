using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiCenterTreemapLabelsGeom : StiSeriesLabelsGeom
{
	public Color LabelColor { get; }

	public Color LabelBorderColor { get; }

	public StiBrush SeriesBrush { get; }

	public StiBrush SeriesLabelsBrush { get; }

	public Color SeriesBorderColor { get; }

	public StiFontGeom Font { get; }

	public string Text { get; }

	public StiAnimation Animation { get; private set; }

	public override void Draw(StiContext context)
	{
		base.Draw(context);
		Rectangle rectangle = Rectangle.Round(base.ClientRectangle);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation && Animation == null)
		{
			Animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalDurationElement);
		}
		DrawMarker(context, rectangle, SeriesBorderColor, SeriesBrush);
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
			context.FillRectangle(StiMouseOverHelper.GetLineMouseOverColor(SeriesBrush), rectangle);
		}
	}

	private void DrawLabelArea(StiContext context, Rectangle rect)
	{
		if (rect.IsEmpty)
		{
			return;
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		StiPenGeom pen = (base.SeriesLabels.DrawBorder ? new StiPenGeom(LabelBorderColor, 1f) : null);
		if (stiChart.IsAnimation)
		{
			context.DrawAnimationRectangle(SeriesLabelsBrush, pen, rect, null, Animation);
		}
		else
		{
			context.FillRectangle(SeriesLabelsBrush, rect);
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
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		StiSolidBrush brush = new StiSolidBrush(LabelColor);
		StiChart stiChart = base.Series.Chart as StiChart;
		StiFontGeom font = StiFontGeom.ChangeFontSize(base.SeriesLabels.Font, base.SeriesLabels.Font.Size * context.Options.Zoom);
		StiStringFormatGeom stringFormatGeom = base.SeriesLabels.Core.GetStringFormatGeom(context);
		stringFormatGeom.Alignment = (StringAlignment)1;
		stringFormatGeom.LineAlignment = (StringAlignment)1;
		if (stiChart.IsAnimation)
		{
			context.DrawAnimationText(Text, font, brush, rect, stringFormatGeom, (StiRotationMode)4, 0f, base.SeriesLabels.Antialiasing, rect.Width, Animation);
		}
		else
		{
			context.DrawRotatedString(Text, font, brush, rect, stringFormatGeom, (StiRotationMode)4, 0f, base.SeriesLabels.Antialiasing, rect.Width);
		}
	}

	public StiCenterTreemapLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle, string text, Color labelColor, Color labelBorderColor, StiBrush seriesBrush, StiBrush seriesLabelsBrush, Color seriesBorderColor, StiFontGeom font, StiAnimation animation)
		: base(seriesLabels, series, index, value, clientRectangle)
	{
		Text = text;
		LabelColor = labelColor;
		LabelBorderColor = labelBorderColor;
		SeriesBrush = seriesBrush;
		SeriesLabelsBrush = seriesLabelsBrush;
		SeriesBorderColor = seriesBorderColor;
		Font = font;
		Animation = animation;
	}
}
