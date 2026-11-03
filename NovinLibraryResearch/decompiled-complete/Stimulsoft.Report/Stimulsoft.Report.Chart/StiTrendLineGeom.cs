using System.Drawing;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Chart;

public class StiTrendLineGeom : StiCellGeom
{
	private IStiTrendLine trendLine;

	private PointF pointStart;

	private PointF pointEnd;

	public override void Draw(StiContext context)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
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
			StiNullableDrawing.DrawLines(context, stiPenGeom, GetArray(pointStart, pointEnd));
			context.PopTransform();
		}
		StiPenGeom stiPenGeom2 = new StiPenGeom(lineColor, lineWidth * context.Options.Zoom);
		stiPenGeom2.PenStyle = lineStyle;
		context.DrawLine(stiPenGeom2, pointStart.X, pointStart.Y, pointEnd.X, pointEnd.Y);
		if (trendLine.TitleVisible)
		{
			StiSolidBrush brush = new StiSolidBrush(trendLine.LineColor);
			StiFontGeom font = StiFontGeom.ChangeFontSize(trendLine.Font, trendLine.Font.Size * context.Options.Zoom);
			StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
			PointF empty = PointF.Empty;
			StiRotationMode mode = (StiRotationMode)4;
			switch (trendLine.Position)
			{
			case StiTrendLine.StiTextPosition.LeftBottom:
				empty = pointStart;
				mode = (StiRotationMode)0;
				break;
			case StiTrendLine.StiTextPosition.LeftTop:
				empty = pointStart;
				mode = (StiRotationMode)2;
				break;
			case StiTrendLine.StiTextPosition.RightBottom:
				empty = pointEnd;
				mode = (StiRotationMode)6;
				break;
			case StiTrendLine.StiTextPosition.RightTop:
				empty = pointEnd;
				mode = (StiRotationMode)8;
				break;
			}
			context.DrawRotatedString(trendLine.Text, font, brush, empty, genericStringFormat, mode, 0f, antialiasing: true, 0);
		}
		context.PopSmoothingMode();
	}

	private static PointF?[] GetArray(PointF pointStart, PointF pointEnd)
	{
		return new PointF?[2] { pointStart, pointEnd };
	}

	public StiTrendLineGeom(PointF pointStart, PointF pointEnd, IStiTrendLine trendLine)
		: base(StiBaseLineSeriesGeom.GetClientRectangle(GetArray(pointStart, pointEnd), trendLine.LineWidth))
	{
		this.pointStart = pointStart;
		this.pointEnd = pointEnd;
		this.trendLine = trendLine;
	}
}
