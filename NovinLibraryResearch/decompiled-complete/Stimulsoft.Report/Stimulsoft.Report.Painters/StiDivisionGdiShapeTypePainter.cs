using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.ShapeTypes;

namespace Stimulsoft.Report.Painters;

public class StiDivisionGdiShapeTypePainter : StiGdiShapeTypePainter
{
	public override void Paint(object context, StiShape shape, StiShapeTypeService shapeType, RectangleF rect, float zoom)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (!(context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		Graphics g = (Graphics)context;
		GraphicsPath val = new GraphicsPath();
		try
		{
			float num = rect.Height / 3f;
			num += 4f;
			val.AddLines(new PointF[5]
			{
				new PointF(rect.X, rect.Y + num),
				new PointF(rect.Right, rect.Y + num),
				new PointF(rect.Right, rect.Bottom - num),
				new PointF(rect.X, rect.Bottom - num),
				new PointF(rect.X, rect.Y + num)
			});
			num -= 4f;
			RectangleF rectangleF = new RectangleF(rect.Width / 2f - num / 2f + rect.X, rect.Y + 1f, num, num);
			RectangleF rectangleF2 = new RectangleF(rect.Width / 2f - num / 2f + rect.X, rect.Bottom - 2f - num, num, num);
			val.AddEllipse(rectangleF);
			val.AddEllipse(rectangleF2);
			StiGdiShapeTypePainter.DrawShape(g, shape, val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
