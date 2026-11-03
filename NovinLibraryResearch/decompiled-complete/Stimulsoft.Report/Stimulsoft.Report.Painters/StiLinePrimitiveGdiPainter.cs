using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public abstract class StiLinePrimitiveGdiPainter : StiComponentGdiPainter
{
	public virtual void DrawDot(Graphics g, Pen pen, double x, double y, float size)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		SmoothingMode smoothingMode = g.SmoothingMode;
		if (!StiOptions.Engine.DisableAntialiasingInPainters)
		{
			g.SmoothingMode = (SmoothingMode)4;
		}
		RectangleF rectangleF = new RectangleF((float)x - size, (float)y - size, size * 2f, size * 2f);
		g.FillEllipse(Brushes.White, rectangleF);
		g.DrawEllipse(pen, rectangleF);
		g.SmoothingMode = smoothingMode;
	}

	public void AddDotToPath(GraphicsPath path, double x, double y, float size)
	{
		RectangleF rectangleF = new RectangleF((float)x - size, (float)y - size, size * 2f, size * 2f);
		path.AddEllipse(rectangleF);
	}

	public override void PaintHighlight(StiComponent component, StiPaintEventArgs e)
	{
	}

	protected void CheckRectForOverflow(ref RectangleD rect)
	{
		int num = 536870911;
		if (((RectangleD)(ref rect)).X > (double)num)
		{
			((RectangleD)(ref rect)).X = num;
		}
		if (((RectangleD)(ref rect)).X < (double)(-num))
		{
			((RectangleD)(ref rect)).X = -num;
		}
		if (((RectangleD)(ref rect)).Width > (double)num)
		{
			((RectangleD)(ref rect)).Width = num;
		}
		if (((RectangleD)(ref rect)).Width < (double)(-num))
		{
			((RectangleD)(ref rect)).Width = -num;
		}
		if (((RectangleD)(ref rect)).Y > (double)num)
		{
			((RectangleD)(ref rect)).Y = num;
		}
		if (((RectangleD)(ref rect)).Y < (double)(-num))
		{
			((RectangleD)(ref rect)).Y = -num;
		}
		if (((RectangleD)(ref rect)).Height > (double)num)
		{
			((RectangleD)(ref rect)).Height = num;
		}
		if (((RectangleD)(ref rect)).Height < (double)(-num))
		{
			((RectangleD)(ref rect)).Height = -num;
		}
	}
}
