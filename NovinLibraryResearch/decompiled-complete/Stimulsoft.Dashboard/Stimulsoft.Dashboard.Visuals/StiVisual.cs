using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Visuals;

public abstract class StiVisual
{
	public void Draw(Graphics g, RectangleF rect)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		StiGdiContextPainter contextPainter = new StiGdiContextPainter(g);
		StiContext stiContext = new StiContext(contextPainter, isGdi: true, isWpf: false, isPrinting: false, (float)StiScale.Factor);
		Draw(stiContext, rect);
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		g.SmoothingMode = (SmoothingMode)4;
		g.TextRenderingHint = (TextRenderingHint)4;
		stiContext.Render(rect);
		g.SmoothingMode = smoothingMode;
		g.TextRenderingHint = textRenderingHint;
	}

	internal void DrawArrowMore(StiContext context, IStiElement element, RectangleF rect, float factor)
	{
		StiCornerRadius stiCornerRadius = (element as IStiCornerRadius)?.CornerRadius;
		if (stiCornerRadius != null && stiCornerRadius.BottomRight > 0f)
		{
			rect.Width -= StiScale.I(stiCornerRadius.BottomRight / 4f);
			rect.Height -= StiScale.I(stiCornerRadius.BottomRight / 4f);
		}
		PointF[] points = new PointF[3]
		{
			new PointF(rect.Right - 5f * factor, rect.Bottom - 5f * factor),
			new PointF(rect.Right - 5f * factor, rect.Bottom - 15f * factor),
			new PointF(rect.Right - 15f * factor, rect.Bottom - 5f * factor)
		};
		List<StiSegmentGeom> path = new List<StiSegmentGeom>
		{
			new StiLinesSegmentGeom(points)
		};
		context.FillPath(Color.FromArgb(140, 140, 140), path, rect);
	}

	public abstract void Draw(StiContext context, RectangleF rect);
}
