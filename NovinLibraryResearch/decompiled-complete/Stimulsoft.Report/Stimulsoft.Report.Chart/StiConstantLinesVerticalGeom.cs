using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiConstantLinesVerticalGeom : StiCellGeom
{
	public IStiConstantLines Line { get; }

	public PointF Point { get; }

	public StiRotationMode Mode { get; }

	public override void Draw(StiContext context)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(Line.LineColor, Line.LineWidth);
		stiPenGeom.PenStyle = Line.LineStyle;
		context.DrawLine(stiPenGeom, rectangleF.Left, rectangleF.Top, rectangleF.Left, rectangleF.Bottom);
		if (Line.TitleVisible)
		{
			StiSolidBrush brush = new StiSolidBrush(Line.LineColor);
			StiFontGeom font = StiFontGeom.ChangeFontSize(Line.Font, Line.Font.Size * context.Options.Zoom);
			StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
			string text = StiReportParser.Parse(Line.Text, (IStiReportComponent)(object)(Line.Chart as StiChart));
			context.DrawRotatedString(text, font, brush, Point, genericStringFormat, Mode, 90f, Line.Antialiasing, 0);
		}
	}

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		float num = (float)Math.Ceiling(Line.LineWidth / 2f);
		RectangleF rectangleF = new RectangleF(base.ClientRectangle.Left - num, base.ClientRectangle.Top, Line.LineWidth, base.ClientRectangle.Height);
		return rectangleF.Contains(x, y);
	}

	public StiConstantLinesVerticalGeom(IStiConstantLines line, RectangleF clientRectangle, PointF point, StiRotationMode mode)
		: base(clientRectangle)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Line = line;
		Point = point;
		Mode = mode;
	}
}
