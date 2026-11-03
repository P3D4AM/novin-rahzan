using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public class StiSunburstEmptySeriesElementGeom : StiCellGeom
{
	public override void Draw(StiContext context)
	{
		float num = base.ClientRectangle.Width / 6f;
		DrawSunburst(context, base.ClientRectangle, num * 0.85f);
		DrawSunburst(context, new RectangleF(base.ClientRectangle.X + num, base.ClientRectangle.Y + num, base.ClientRectangle.Width - 2f * num, base.ClientRectangle.Height - 2f * num), num * 0.85f);
	}

	private void DrawSunburst(StiContext context, RectangleF rectPie, float widthElement)
	{
		StiPenGeom pen = new StiPenGeom(Color.LightGray);
		List<StiSegmentGeom> list = new List<StiSegmentGeom>();
		list.Add(new StiArcSegmentGeom(rectPie, 0f, 360f));
		list.Add(new StiLineSegmentGeom(rectPie.Right, rectPie.Y + rectPie.Height / 2f, rectPie.X + (rectPie.Width - widthElement), rectPie.Y + rectPie.Height / 2f));
		list.Add(new StiArcSegmentGeom(new RectangleF(rectPie.X + widthElement, rectPie.Y + widthElement, rectPie.Width - 2f * widthElement, rectPie.Height - 2f * widthElement), 0f, 360f));
		context.PushSmoothingModeToAntiAlias();
		context.FillPath(Color.FromArgb(50, Color.LightGray), list, rectPie);
		context.DrawPath(pen, list, rectPie);
		context.DrawLine(pen, rectPie.X + rectPie.Width / 2f, rectPie.Y + (rectPie.Height - widthElement), rectPie.X + rectPie.Width / 2f, rectPie.Bottom);
		context.PopSmoothingMode();
	}

	public StiSunburstEmptySeriesElementGeom(RectangleF clientRectangle)
		: base(clientRectangle)
	{
	}
}
