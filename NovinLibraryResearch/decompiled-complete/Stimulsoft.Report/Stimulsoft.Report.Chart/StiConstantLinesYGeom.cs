using System;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Chart;

public class StiConstantLinesYGeom : StiCellGeom
{
	private IStiConstantLines line;

	private PointF point;

	private StiRotationMode mode;

	public IStiConstantLines Line => line;

	public PointF Point => point;

	public StiRotationMode Mode => mode;

	public override void Draw(StiContext context)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		RectangleF rectangleF = base.ClientRectangle;
		StiPenGeom stiPenGeom = new StiPenGeom(line.LineColor, line.LineWidth);
		stiPenGeom.PenStyle = line.LineStyle;
		context.DrawLine(stiPenGeom, rectangleF.Left, rectangleF.Top, rectangleF.Left + rectangleF.Width, rectangleF.Top);
		if (line.TitleVisible)
		{
			StiSolidBrush brush = new StiSolidBrush(line.LineColor);
			StiFontGeom font = StiFontGeom.ChangeFontSize(line.Font, line.Font.Size * context.Options.Zoom);
			StiStringFormatGeom genericStringFormat = context.GetGenericStringFormat();
			string text = StiReportParser.Parse(Line.Text, (IStiReportComponent)(object)(Line.Chart as StiChart));
			context.DrawRotatedString(text, font, brush, Point, genericStringFormat, Mode, 0f, line.Antialiasing, 0);
		}
	}

	public override bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		float num = (float)Math.Ceiling(line.LineWidth / 2f);
		RectangleF rectangleF = new RectangleF(base.ClientRectangle.Left, base.ClientRectangle.Top - num, base.ClientRectangle.Width, line.LineWidth);
		return rectangleF.Contains(x, y);
	}

	public StiConstantLinesYGeom(IStiConstantLines line, RectangleF clientRectangle, PointF point, StiRotationMode mode)
		: base(clientRectangle)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		this.line = line;
		this.point = point;
		this.mode = mode;
	}
}
