using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiTrendCurveGeom : StiCellGeom
{
	private PointF?[] points;

	private IStiTrendLine trendLine;

	public PointF?[] Points => points;

	public IStiTrendLine TrendLine => trendLine;

	public override void Draw(StiContext context)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		PointF[] array = new PointF[points.Length];
		for (int i = 0; i < points.Length; i++)
		{
			array[i] = points[i].Value;
		}
		Color lineColor = trendLine.LineColor;
		float lineWidth = trendLine.LineWidth;
		StiPenStyle lineStyle = trendLine.LineStyle;
		bool showShadow = trendLine.ShowShadow;
		float num = lineWidth * context.Options.Zoom;
		context.PushSmoothingModeToAntiAlias();
		if (showShadow)
		{
			StiPenGeom stiPenGeom = new StiPenGeom(Color.FromArgb(50, 0, 0, 0), num + 0.5f * context.Options.Zoom);
			stiPenGeom.PenStyle = lineStyle;
			context.PushTranslateTransform(num, num);
			StiNullableDrawing.DrawCurve(context, stiPenGeom, points, 0f);
			context.PopTransform();
		}
		StiPenGeom stiPenGeom2 = new StiPenGeom(lineColor, lineWidth * context.Options.Zoom);
		stiPenGeom2.PenStyle = lineStyle;
		context.DrawCurve(stiPenGeom2, array, 0f);
		if (trendLine.TitleVisible && array.Length != 0)
		{
			StiSolidBrush brush = new StiSolidBrush(trendLine.LineColor);
			StiFontGeom font = StiFontGeom.ChangeFontSize(trendLine.Font, trendLine.Font.Size * context.Options.Zoom);
			StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
			PointF pos = PointF.Empty;
			PointF pointF = array[0];
			PointF pointF2 = array[^1];
			StiRotationMode mode = (StiRotationMode)4;
			switch (trendLine.Position)
			{
			case StiTrendLine.StiTextPosition.LeftBottom:
				pos = pointF;
				mode = (StiRotationMode)0;
				break;
			case StiTrendLine.StiTextPosition.LeftTop:
				pos = pointF;
				mode = (StiRotationMode)2;
				break;
			case StiTrendLine.StiTextPosition.RightBottom:
				pos = pointF2;
				mode = (StiRotationMode)6;
				break;
			case StiTrendLine.StiTextPosition.RightTop:
				pos = pointF2;
				mode = (StiRotationMode)8;
				break;
			}
			context.DrawRotatedString(trendLine.Text, font, brush, pos, genericStringFormat, mode, 0f, antialiasing: true, 0);
		}
		context.PopSmoothingMode();
	}

	public StiTrendCurveGeom(PointF?[] points, IStiTrendLine trendLine)
		: base(StiBaseLineSeriesGeom.GetClientRectangle(points, trendLine.LineWidth))
	{
		this.points = points;
		this.trendLine = trendLine;
	}
}
