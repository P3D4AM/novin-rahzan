using System;
using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiCenterPieLabelsGeom : StiSeriesLabelsGeom
{
	public StiBrush SeriesBrush { get; }

	public Color BorderColor { get; }

	public Color SeriesBorderColor { get; }

	public StiBrush SeriesLabelsBrush { get; }

	public StiBrush LabelBrush { get; }

	public string Text { get; }

	public StiRotationMode RotationMode { get; }

	public RectangleF LabelRect { get; set; }

	public float AngleToUse { get; }

	public StiAnimation Animation { get; set; }

	public override void Draw(StiContext context)
	{
		base.Draw(context);
		Rectangle rect = Rectangle.Round(base.ClientRectangle);
		StiChart stiChart = base.Series.Chart as StiChart;
		if (stiChart.IsAnimation && Animation == null)
		{
			Animation = new StiOpacityAnimation(StiChartHelper.GlobalBeginTimeElement, StiChartHelper.GlobalDurationElement);
		}
		DrawMarker(context, Rectangle.Round(LabelRect), SeriesBorderColor, SeriesBrush);
		int num = (int)Math.Round(LabelRect.X + LabelRect.Width / 2f, 0);
		int num2 = (int)Math.Round(LabelRect.Y + LabelRect.Height / 2f, 0);
		context.PushTranslateTransform(num, num2);
		context.PushRotateTransform(AngleToUse);
		DrawLabelArea(context, rect);
		DrawLabelText(context, rect);
		context.PopTransform();
		context.PopTransform();
	}

	private void DrawLabelArea(StiContext context, Rectangle rect)
	{
		if (rect.IsEmpty)
		{
			return;
		}
		StiChart stiChart = base.Series.Chart as StiChart;
		StiPenGeom pen = (base.SeriesLabels.DrawBorder ? new StiPenGeom(BorderColor, 1f) : null);
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

	public StiCenterPieLabelsGeom(IStiSeriesLabels seriesLabels, IStiSeries series, int index, double value, RectangleF clientRectangle, string text, StiBrush seriesBrush, StiBrush labelBrush, StiBrush seriesLabelsBrush, Color borderColor, Color seriesBorderColor, StiRotationMode rotationMode, RectangleF labelRect, float angleToUse, StiAnimation animation)
		: base(seriesLabels, series, index, value, clientRectangle)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		LabelBrush = labelBrush;
		BorderColor = borderColor;
		SeriesBorderColor = seriesBorderColor;
		SeriesLabelsBrush = seriesLabelsBrush;
		SeriesBrush = seriesBrush;
		RotationMode = rotationMode;
		LabelRect = labelRect;
		AngleToUse = angleToUse;
		Animation = animation;
	}
}
